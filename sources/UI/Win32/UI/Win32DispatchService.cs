// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Interop;
using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.Win32Utilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.UI
{
    /// <summary>Provides access to a Win32 based dispatch subsystem.</summary>
    public sealed unsafe class Win32DispatchService : DispatchService
    {
        private static ValueLazy<Win32DispatchService> s_instance = new ValueLazy<Win32DispatchService>(CreateDispatchService);

        private readonly double _tickFrequency;
        private readonly ConcurrentDictionary<Thread, Dispatcher> _dispatchers;

        private Win32DispatchService()
        {
            _tickFrequency = GetTickFrequency();
            _dispatchers = new ConcurrentDictionary<Thread, Dispatcher>();
        }

        /// <summary>Gets the <see cref="Win32DispatchService" /> instance for the current program.</summary>
        public static Win32DispatchService Instance => s_instance.Value;

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="QueryPerformanceCounter(LARGE_INTEGER*)" /> failed.</exception>
        public override Timestamp CurrentTimestamp
        {
            get
            {
                LARGE_INTEGER performanceCount;
                ThrowExternalExceptionIfFalse(QueryPerformanceCounter(&performanceCount), nameof(QueryPerformanceCounter));

                var ticks = (long)(performanceCount.QuadPart * _tickFrequency);
                return new Timestamp(ticks);
            }
        }

        /// <inheritdoc />
        public override Dispatcher DispatcherForCurrentThread => GetDispatcher(Thread.CurrentThread);

        /// <inheritdoc />
        public override Dispatcher GetDispatcher(Thread thread)
        {
            ThrowIfNull(thread, nameof(thread));
            return _dispatchers.GetOrAdd(thread, (parentThread) => new Win32Dispatcher(this, parentThread));
        }

        /// <inheritdoc />
        public override bool TryGetDispatcher(Thread thread, [MaybeNullWhen(false)] out Dispatcher dispatcher)
        {
            ThrowIfNull(thread, nameof(thread));
            return _dispatchers.TryGetValue(thread, out dispatcher!);
        }

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing) { }

        private static Win32DispatchService CreateDispatchService() => new Win32DispatchService();

        private static double GetTickFrequency()
        {
            LARGE_INTEGER frequency;
            ThrowExternalExceptionIfFalse(QueryPerformanceFrequency(&frequency), nameof(QueryPerformanceFrequency));

            const double TicksPerSecond = Timestamp.TicksPerSecond;
            return TicksPerSecond / frequency.QuadPart;
        }
    }
}
