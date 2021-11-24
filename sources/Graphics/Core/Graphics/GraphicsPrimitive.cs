// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics primitive which represents the most basic renderable object.</summary>
public abstract class GraphicsPrimitive : GraphicsDeviceObject
{
    private readonly GraphicsPipeline _pipeline;
    private readonly GraphicsMemoryRegion<GraphicsResource> _vertexBufferRegion;
    private readonly GraphicsMemoryRegion<GraphicsResource> _indexBufferRegion;
    private readonly GraphicsMemoryRegion<GraphicsResource>[] _inputResourceRegions;
    private readonly uint _vertexBufferStride;
    private readonly uint _indexBufferStride;

    /// <summary>Initializes a new instance of the <see cref="GraphicsPrimitive" /> class.</summary>
    /// <param name="device">The device which manages the primitive.</param>
    /// <param name="pipeline">The pipeline used for rendering the primitive.</param>
    /// <param name="vertexBufferRegion">The buffer region which holds the vertices for the primitive.</param>
    /// <param name="vertexBufferStride">The stride of the vertices in <paramref name="vertexBufferRegion" />.</param>
    /// <param name="indexBufferRegion">The buffer region which holds the indices for the primitive or <c>default</c> if none exists.</param>
    /// <param name="indexBufferStride">The stride of the indices in <paramref name="indexBufferRegion" />.</param>
    /// <param name="inputResourceRegions">The resource regions which hold the input data for the primitive or an empty span if none exist.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="pipeline" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="vertexBufferRegion" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="pipeline" /> is incompatible as it belongs to a different device.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="vertexBufferRegion" /> was not created for <paramref name="device" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexBufferRegion" /> was not created for <paramref name="device" />.</exception>
    protected GraphicsPrimitive(GraphicsDevice device, GraphicsPipeline pipeline, in GraphicsMemoryRegion<GraphicsResource> vertexBufferRegion, uint vertexBufferStride, in GraphicsMemoryRegion<GraphicsResource> indexBufferRegion, uint indexBufferStride, ReadOnlySpan<GraphicsMemoryRegion<GraphicsResource>> inputResourceRegions)
        : base(device)
    {
        ThrowIfNull(pipeline, nameof(pipeline));
        ThrowIfNull(vertexBufferRegion.Collection, nameof(vertexBufferRegion));

        if (pipeline.Device != device)
        {
            ThrowForInvalidParent(pipeline.Device, nameof(pipeline));
        }

        if (vertexBufferRegion.Device != device)
        {
            ThrowForInvalidParent(vertexBufferRegion.Device, nameof(vertexBufferRegion));
        }

        if ((indexBufferRegion.Collection is not null) && (indexBufferRegion.Device != device))
        {
            ThrowForInvalidParent(indexBufferRegion.Device, nameof(indexBufferRegion));
        }

        _pipeline = pipeline;

        _vertexBufferRegion = vertexBufferRegion;
        _indexBufferRegion = indexBufferRegion;
        _inputResourceRegions = inputResourceRegions.ToArray();

        _vertexBufferStride = vertexBufferStride;
        _indexBufferStride = indexBufferStride;
    }

    /// <summary>Gets the buffer region which holds the indices for the primitive or <c>default</c> if none exists.</summary>
    public ref readonly GraphicsMemoryRegion<GraphicsResource> IndexBufferRegion => ref _indexBufferRegion;

    /// <summary>Gets the stride of the index buffer region, in bytes.</summary>
    public uint IndexBufferStride => _indexBufferStride;

    /// <summary>Gets the resource regions which hold the input data for the primitive or <see cref="ReadOnlySpan{T}.Empty" /> if none exist.</summary>
    public ReadOnlySpan<GraphicsMemoryRegion<GraphicsResource>> InputResourceRegions => _inputResourceRegions;

    /// <summary>Gets the pipeline used for rendering the primitive.</summary>
    public GraphicsPipeline Pipeline => _pipeline;

    /// <summary>Gets the buffer region which holds the vertices for the primitive.</summary>
    public ref readonly GraphicsMemoryRegion<GraphicsResource> VertexBufferRegion => ref _vertexBufferRegion;

    /// <summary>Gets the stride of the vertex buffer region, in bytes.</summary>
    public uint VertexBufferStride => _vertexBufferStride;
}
