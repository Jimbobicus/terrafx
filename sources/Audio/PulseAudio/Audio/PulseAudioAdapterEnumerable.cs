// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TerraFX.Interop.PulseAudio;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Audio
{
    /// <inheritdoc />
    public class PulseAudioAdapterEnumerable : IAudioAdapterEnumerable
    {
        // Only call the internal methods from a single thread (usually the pulse event loop thread)
        // _completeSignal may be overwritten improperly causing a deadlock if multiple threads try
        // to call internal methods concurrently

        // It is unlikely that more than 16 devices will be present, so to prevent excessive resizing
        // we allocate the backing collection with 16 elements.
        private const int DefaultAudioAdapterCount = 16;

        private readonly List<IAudioAdapter> _backingCollection;
        private readonly Thread _eventLoopThread;
        private readonly unsafe delegate* unmanaged<pa_context*, pa_source_info*, int, void*, void> _sourceCallback;
        private readonly unsafe delegate* unmanaged<pa_context*, pa_sink_info*, int, void*, void> _sinkCallback;

        private TaskCompletionSource<bool> _completeSignal;

        internal unsafe PulseAudioAdapterEnumerable(Thread eventLoopThread, delegate* unmanaged<pa_context*, pa_source_info*, int, void*, void> sourceCallback, delegate* unmanaged<pa_context*, pa_sink_info*, int, void*, void> sinkCallback)
        {
            _backingCollection = new List<IAudioAdapter>(DefaultAudioAdapterCount);
            _eventLoopThread = eventLoopThread;
            _sourceCallback = sourceCallback;
            _sinkCallback = sinkCallback;
            // Run continuations asynchronously so that we do not block the event loop thread and potentially deadlock
            _completeSignal = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        }

        internal unsafe delegate* unmanaged<pa_context*, pa_source_info*, int, void*, void> SourceCallback => _sourceCallback;

        internal unsafe delegate* unmanaged<pa_context*, pa_sink_info*, int, void*, void> SinkCallback => _sinkCallback;

        private void SetCompleteSignal(bool value)
        {
            var previous = Interlocked.Exchange(ref _completeSignal, new TaskCompletionSource<bool>());
            _ = previous.TrySetResult(value);
        }

        internal unsafe void Add(IAudioAdapter adapter)
        {
            ThrowIfNotThread(_eventLoopThread);

            _backingCollection.Add(adapter);
            SetCompleteSignal(false);
        }

        internal void Complete()
        {
            ThrowIfNotThread(_eventLoopThread);

            // Should always be successful due to being called only from one thread
            // Do not overwrite _completeSignal past this point
            _ = _completeSignal.TrySetResult(true);
        }

        /// <inheritdoc />
        public async IAsyncEnumerator<IAudioAdapter> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            var complete = false;

            for (var position = 0; !complete; position++)
            {
                if (position >= _backingCollection.Count)
                {
                    var done = await _completeSignal.Task;
                    complete = position >= _backingCollection.Count && done;
                }

                if (!complete)
                {
                    yield return _backingCollection[position];
                }
            }
        }

        /// <inheritdoc />
        public IEnumerator<IAudioAdapter> GetEnumerator()
        {
            var complete = false;

            for (var position = 0; !complete; position++)
            {
                if (position >= _backingCollection.Count)
                {
                    var task = _completeSignal.Task;
                    task.Wait();
                    var done = task.Result;
                    complete = position >= _backingCollection.Count && done;
                }

                if (!complete)
                {
                    yield return _backingCollection[position];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
