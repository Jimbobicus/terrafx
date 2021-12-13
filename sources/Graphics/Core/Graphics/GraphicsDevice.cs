// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics device which provides state management and isolation for a graphics adapter.</summary>
public abstract partial class GraphicsDevice : IDisposable
{
    private readonly GraphicsAdapter _adapter;
    private readonly GraphicsService _service;

    /// <summary>Initializes a new instance of the <see cref="GraphicsDevice" /> class.</summary>
    /// <param name="adapter">The underlying adapter for the device.</param>
    /// <exception cref="ArgumentNullException"><paramref name="adapter" /> is <c>null</c>.</exception>
    protected GraphicsDevice(GraphicsAdapter adapter)
    {
        ThrowIfNull(adapter);

        _adapter = adapter;
        _service = adapter.Service;
    }

    /// <summary>Gets the underlying adapter for the device.</summary>
    public GraphicsAdapter Adapter => _adapter;

    /// <summary>Gets the memory allocator for the device.</summary>
    public abstract GraphicsMemoryAllocator MemoryAllocator { get; }

    /// <summary>Gets the service which enumerated <see cref="Adapter" />.</summary>
    public GraphicsService Service => _service;

    /// <summary>Creates a new graphics fence for the device.</summary>
    /// <param name="isSignalled">The default state of <see cref="GraphicsFence.IsSignalled" /> for the created fence.</param>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract GraphicsFence CreateFence(bool isSignalled);

    /// <summary>Creates a new graphics pipeline signature for the device.</summary>
    /// <param name="inputs">The inputs given to the graphics pipeline or <see cref="ReadOnlySpan{T}.Empty" /> if none exist.</param>
    /// <param name="resources">The resources available to the graphics pipeline or <see cref="ReadOnlySpan{T}.Empty" /> if none exist.</param>
    /// <returns>A new graphics pipeline signature created for the device.</returns>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract GraphicsPipelineSignature CreatePipelineSignature(ReadOnlySpan<GraphicsPipelineInput> inputs = default, ReadOnlySpan<GraphicsPipelineResource> resources = default);

    /// <summary>Creates a new graphics primitive for the device.</summary>
    /// <param name="pipeline">The pipeline used for rendering the graphics primitive.</param>
    /// <param name="vertexBufferView">The vertex buffer view which holds the vertices for the graphics primitive.</param>
    /// <param name="indexBufferView">The index buffer view which holds the indices for the graphics primitive or <c>default</c> if none exists.</param>
    /// <param name="inputResourceViews">The resource views which hold the input data for the graphics primitive or an empty span if none exist.</param>
    /// <exception cref="ArgumentNullException"><paramref name="pipeline" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="pipeline" /> was not created for this device.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="vertexBufferView" /> was not created for this device.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexBufferView" /> was not created for this device.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract GraphicsPrimitive CreatePrimitive(GraphicsPipeline pipeline, in GraphicsResourceView vertexBufferView, in GraphicsResourceView indexBufferView = default, ReadOnlySpan<GraphicsResourceView> inputResourceViews = default);

    /// <summary>Creates a new graphics shader for the device.</summary>
    /// <param name="kind">The kind of graphics shader to create.</param>
    /// <param name="bytecode">The underlying bytecode for the graphics shader.</param>
    /// <param name="entryPointName">The name of the entry point for the graphics shader.</param>
    /// <returns>A new graphics shader created for the device.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="entryPointName" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="kind" /> is unsupported.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract GraphicsShader CreateShader(GraphicsShaderKind kind, ReadOnlySpan<byte> bytecode, string entryPointName);

    /// <summary>Rents a graphics render context from the device.</summary>
    /// <returns>A graphics render context for the device.</returns>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract GraphicsRenderContext RentRenderContext();

    /// <summary>Creates a new graphics render pass for the device.</summary>
    /// <param name="surface">The surface used by the render pass.</param>
    /// <param name="renderTargetFormat">The format of render targets used by the render pass.</param>
    /// <param name="minimumRenderTargetCount">The minimum number of render targets to create or <c>zero</c> to use the system default.</param>
    /// <returns>A new graphics render pass created for the device.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="surface" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minimumRenderTargetCount" /> is greater than the maximum number of allowed render targets.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract GraphicsRenderPass CreateRenderPass(IGraphicsSurface surface, GraphicsFormat renderTargetFormat, uint minimumRenderTargetCount = 0);

    /// <summary>Returns a graphics render context to the device for further use.</summary>
    /// <param name="renderContext">The graphics render context that should be returned.</param>
    /// <exception cref="ArgumentNullException"><paramref name="renderContext" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="renderContext" /> is not owned by the device.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract void ReturnRenderContext(GraphicsRenderContext renderContext);

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(isDisposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Signals a graphics fence.</summary>
    /// <param name="fence">The fence to be signalled</param>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract void Signal(GraphicsFence fence);

    /// <summary>Waits for the device to become idle.</summary>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract void WaitForIdle();

    /// <inheritdoc cref="Dispose()" />
    /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
    protected abstract void Dispose(bool isDisposing);
}
