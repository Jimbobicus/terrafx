// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using TerraFX.Threading;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.ApplicationModel;

/// <summary>A multimedia-based application.</summary>
public sealed class Application
{
    private const int Stopped = 1;
    private const int Running = 2;
    private const int Exiting = 3;

    private readonly Thread _parentThread;
    private readonly ApplicationServiceProvider _serviceProvider;

    private VolatileState _state;

    /// <summary>Initializes a new instance of the <see cref="Application" /> class.</summary>
    /// <param name="serviceProvider">The object which provides services for the instance.</param>
    /// <exception cref="ArgumentNullException"><paramref name="serviceProvider" /> is <c>null</c>.</exception>
    public Application(ApplicationServiceProvider serviceProvider)
    {
        ThrowIfNull(serviceProvider, nameof(serviceProvider));

        _parentThread = Thread.CurrentThread;
        _serviceProvider = serviceProvider;

        _ = _state.Transition(to: Stopped);
    }

    /// <summary>Occurs when the event loop for the current instance becomes idle.</summary>
    public event EventHandler<ApplicationIdleEventArgs>? Idle;

    /// <summary>Gets a value that indicates whether the event loop for the instance is running.</summary>
    public bool IsRunning => _state == Running;

    /// <summary>Gets the <see cref="Thread" /> that was used to create the instance.</summary>
    public Thread ParentThread => _parentThread;

    /// <summary>Gets the <see cref="ApplicationServiceProvider" /> for the instance.</summary>
    public ApplicationServiceProvider ServiceProvider => _serviceProvider;

    /// <summary>Requests that the instance exits the event loop.</summary>
    /// <remarks>
    ///   <para>This method does nothing if <see cref="IsRunning" /> is <c>false</c>.</para>
    ///   <para>This method can be called from any thread.</para>
    /// </remarks>
    public void RequestExit() => _ = _state.TryTransition(from: Running, to: Exiting);

    /// <summary>Runs the event loop for the instance.</summary>
    /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="ParentThread" />.</exception>
    /// <exception cref="InvalidOperationException">The state of the object is not <see cref="Stopped" />.</exception>
    /// <remarks>This method does nothing if an exit is requested before the first iteration of the event loop has started.</remarks>
    public void Run()
    {
        ThrowIfNotThread(_parentThread);

        // We enforce the starting transition to be Stopped, which also covers attempting to run a disposed object.
        // However, we do not enforce the ending transition to also be Stopped, as we may have stopped due to disposal.

        _state.Transition(from: Stopped, to: Running);
        {
            var windowService = _serviceProvider.WindowService;

            var dispatchService = windowService.DispatchService;
            var dispatcher = dispatchService.DispatcherForCurrentThread;
            var previousTimestamp = dispatchService.CurrentTimestamp;

            var previousFrameCount = 0u;
            var framesPerSecond = 0u;
            var framesThisSecond = 0u;

            var secondCounter = TimeSpan.Zero;

            // We need to do an initial dispatch to cover the case where a quit
            // message was posted before the message pump was started, otherwise
            // we can end up with a NullReferenceException when we try to execute
            // OnIdle.

            dispatcher.ExitRequested += OnDispatcherExitRequested;
            dispatcher.DispatchPending();

            while (_state == Running)
            {
                var currentTimestamp = dispatchService.CurrentTimestamp;
                var frameCount = previousFrameCount++;
                {
                    var delta = currentTimestamp - previousTimestamp;
                    secondCounter += delta;

                    OnIdle(delta, framesPerSecond);
                    framesThisSecond++;

                    if (secondCounter.TotalSeconds >= 1.0)
                    {
                        framesPerSecond = framesThisSecond;
                        framesThisSecond = 0;

                        var ticks = secondCounter.Ticks - TimeSpan.TicksPerSecond;
                        secondCounter = TimeSpan.FromTicks(ticks);
                    }
                }
                previousFrameCount = frameCount;
                previousTimestamp = currentTimestamp;

                dispatcher.DispatchPending();
            }

            dispatcher.ExitRequested -= OnDispatcherExitRequested;
        }
        _ = _state.TryTransition(from: Exiting, to: Stopped);
    }

    private void OnDispatcherExitRequested(object? sender, EventArgs e) => RequestExit();

    private void OnIdle(TimeSpan delta, uint framesPerSecond)
    {
        var idle = Idle;

        if (idle != null)
        {
            var eventArgs = new ApplicationIdleEventArgs(delta, framesPerSecond);
            idle(this, eventArgs);
        }
    }
}
