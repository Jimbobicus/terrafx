// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Numerics;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D12;
using static TerraFX.Interop.DirectX.D3D12_COMMAND_LIST_TYPE;
using static TerraFX.Interop.DirectX.D3D12_DESCRIPTOR_HEAP_TYPE;
using static TerraFX.Interop.DirectX.D3D12_RESOURCE_STATES;
using static TerraFX.Interop.DirectX.D3D12_RTV_DIMENSION;
using static TerraFX.Interop.DirectX.D3D_PRIMITIVE_TOPOLOGY;
using static TerraFX.Interop.DirectX.DXGI_FORMAT;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsContext : GraphicsContext
{
    private readonly D3D12GraphicsFence _fence;
    private readonly D3D12GraphicsFence _waitForExecuteCompletionFence;

    private ValueLazy<Pointer<ID3D12CommandAllocator>> _d3d12CommandAllocator;
    private ValueLazy<Pointer<ID3D12GraphicsCommandList>> _d3d12GraphicsCommandList;
    private ValueLazy<Pointer<ID3D12Resource>> _d3d12RenderTargetResource;
    private ValueLazy<D3D12_CPU_DESCRIPTOR_HANDLE> _d3d12RenderTargetView;

    private VolatileState _state;

    internal D3D12GraphicsContext(D3D12GraphicsDevice device, int index)
        : base(device, index)
    {
        _fence = new D3D12GraphicsFence(device);
        _waitForExecuteCompletionFence = new D3D12GraphicsFence(device);

        _d3d12CommandAllocator = new ValueLazy<Pointer<ID3D12CommandAllocator>>(CreateD3D12CommandAllocator);
        _d3d12GraphicsCommandList = new ValueLazy<Pointer<ID3D12GraphicsCommandList>>(CreateD3D12GraphicsCommandList);
        _d3d12RenderTargetView = new ValueLazy<D3D12_CPU_DESCRIPTOR_HANDLE>(CreateD3D12RenderTargetDescriptor);
        _d3d12RenderTargetResource = new ValueLazy<Pointer<ID3D12Resource>>(CreateD3D12RenderTargetResource);

        _ = _state.Transition(to: Initialized);
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsContext" /> class.</summary>
    ~D3D12GraphicsContext() => Dispose(isDisposing: false);

    /// <summary>Gets the <see cref="ID3D12CommandAllocator" /> used by the context.</summary>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public ID3D12CommandAllocator* D3D12CommandAllocator => _d3d12CommandAllocator.Value;

    /// <summary>Gets the <see cref="ID3D12GraphicsCommandList" /> used by the context.</summary>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public ID3D12GraphicsCommandList* D3D12GraphicsCommandList => _d3d12GraphicsCommandList.Value;

    /// <summary>Gets the <see cref="ID3D12Resource" /> for the render target used by the context.</summary>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public ID3D12Resource* D3D12RenderTargetResource => _d3d12RenderTargetResource.Value;

    /// <summary>Gets the <see cref="D3D12_CPU_DESCRIPTOR_HANDLE" /> for the render target used by the context.</summary>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public D3D12_CPU_DESCRIPTOR_HANDLE D3D12RenderTargetView => _d3d12RenderTargetView.Value;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => (D3D12GraphicsDevice)base.Device;

    /// <inheritdoc />
    public override D3D12GraphicsFence Fence => _fence;

    /// <summary>Gets a fence that is used to wait for the context to finish execution.</summary>
    public D3D12GraphicsFence WaitForExecuteCompletionFence => _waitForExecuteCompletionFence;

    /// <inheritdoc />
    public override void BeginDrawing(ColorRgba backgroundColor)
    {
        var commandList = D3D12GraphicsCommandList;

        var renderTargetResourceBarrier = D3D12_RESOURCE_BARRIER.InitTransition(D3D12RenderTargetResource, D3D12_RESOURCE_STATE_PRESENT, D3D12_RESOURCE_STATE_RENDER_TARGET);
        commandList->ResourceBarrier(1, &renderTargetResourceBarrier);

        var renderTargetView = D3D12RenderTargetView;
        commandList->OMSetRenderTargets(1, &renderTargetView, RTsSingleHandleToDescriptorRange: TRUE, pDepthStencilDescriptor: null);

        var surface = Device.Surface;

        var surfaceWidth = surface.Width;
        var surfaceHeight = surface.Height;

        var viewport = new D3D12_VIEWPORT {
            Width = surfaceWidth,
            Height = surfaceHeight,
            MinDepth = D3D12_MIN_DEPTH,
            MaxDepth = D3D12_MAX_DEPTH,
        };
        commandList->RSSetViewports(1, &viewport);

        var scissorRect = new RECT {
            right = (int)surfaceWidth,
            bottom = (int)surfaceHeight,
        };
        commandList->RSSetScissorRects(1, &scissorRect);

        commandList->ClearRenderTargetView(renderTargetView, (float*)&backgroundColor, NumRects: 0, pRects: null);
        commandList->IASetPrimitiveTopology(D3D_PRIMITIVE_TOPOLOGY_TRIANGLELIST);
    }

    /// <inheritdoc />
    public override void BeginFrame()
    {
        var fence = Fence;

        fence.Wait();
        fence.Reset();

        var d3d12CommandAllocator = D3D12CommandAllocator;

        ThrowExternalExceptionIfFailed(d3d12CommandAllocator->Reset(), nameof(ID3D12CommandAllocator.Reset));
        ThrowExternalExceptionIfFailed(D3D12GraphicsCommandList->Reset(d3d12CommandAllocator, pInitialState: null), nameof(ID3D12GraphicsCommandList.Reset));
    }

    /// <inheritdoc />
    public override void Copy(GraphicsBuffer destination, GraphicsBuffer source)
        => Copy((D3D12GraphicsBuffer)destination, (D3D12GraphicsBuffer)source);

    /// <inheritdoc />
    public override void Copy(GraphicsTexture destination, GraphicsBuffer source)
        => Copy((D3D12GraphicsTexture)destination, (D3D12GraphicsBuffer)source);

    /// <inheritdoc cref="Copy(GraphicsBuffer, GraphicsBuffer)" />
    public void Copy(D3D12GraphicsBuffer destination, D3D12GraphicsBuffer source)
    {
        ThrowIfNull(destination);
        ThrowIfNull(source);

        var commandList = D3D12GraphicsCommandList;

        var destinationCpuAccess = destination.CpuAccess;
        var sourceCpuAccess = source.CpuAccess;

        var d3d12DestinationResource = destination.D3D12Resource;
        var d3d12SourceResource = source.D3D12Resource;

        var d3d12DestinationResourceState = destination.D3D12ResourceState;
        var d3d12SourceResourceState = source.D3D12ResourceState;

        BeginCopy();

        commandList->CopyResource(d3d12DestinationResource, d3d12SourceResource);

        EndCopy();

        void BeginCopy()
        {
            var resourceBarriers = stackalloc D3D12_RESOURCE_BARRIER[2];
            var numResourceBarriers = 0u;

            if (destinationCpuAccess == GraphicsResourceCpuAccess.None)
            {
                resourceBarriers[numResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12DestinationResource,
                    stateBefore: d3d12DestinationResourceState,
                    stateAfter: D3D12_RESOURCE_STATE_COPY_DEST
                );
                numResourceBarriers++;
            }

            if (sourceCpuAccess == GraphicsResourceCpuAccess.None)
            {
                resourceBarriers[numResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12SourceResource,
                    stateBefore: d3d12SourceResourceState,
                    stateAfter: D3D12_RESOURCE_STATE_COPY_SOURCE
                );
                numResourceBarriers++;
            }

            if (numResourceBarriers != 0)
            {
                commandList->ResourceBarrier(numResourceBarriers, resourceBarriers);
            }
        }

        void EndCopy()
        {
            var resourceBarriers = stackalloc D3D12_RESOURCE_BARRIER[2];
            var numResourceBarriers = 0u;

            if (sourceCpuAccess == GraphicsResourceCpuAccess.None)
            {
                resourceBarriers[numResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12SourceResource,
                    stateBefore: D3D12_RESOURCE_STATE_COPY_SOURCE,
                    stateAfter: d3d12SourceResourceState
                );
                numResourceBarriers++;
            }

            if (destinationCpuAccess == GraphicsResourceCpuAccess.None)
            {
                resourceBarriers[numResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12DestinationResource,
                    stateBefore: D3D12_RESOURCE_STATE_COPY_DEST,
                    stateAfter: d3d12DestinationResourceState
                );
                numResourceBarriers++;
            }

            if (numResourceBarriers != 0)
            {
                commandList->ResourceBarrier(numResourceBarriers, resourceBarriers);
            }
        }
    }

    /// <inheritdoc cref="Copy(GraphicsTexture, GraphicsBuffer)" />
    public void Copy(D3D12GraphicsTexture destination, D3D12GraphicsBuffer source)
    {
        ThrowIfNull(destination);
        ThrowIfNull(source);

        var device = Device.D3D12Device;
        var commandList = D3D12GraphicsCommandList;

        var destinationCpuAccess = destination.CpuAccess;
        var sourceCpuAccess = source.CpuAccess;

        var d3d12DestinationResource = destination.D3D12Resource;
        var d3d12SourceResource = source.D3D12Resource;

        var d3d12DestinationResourceState = destination.D3D12ResourceState;
        var d3d12SourceResourceState = source.D3D12ResourceState;

        BeginCopy();

        D3D12_PLACED_SUBRESOURCE_FOOTPRINT sourceFootprint;

        var destinationDesc = d3d12DestinationResource->GetDesc();
        device->GetCopyableFootprints(&destinationDesc, FirstSubresource: 0, NumSubresources: 1, BaseOffset: 0, &sourceFootprint, pNumRows: null, pRowSizeInBytes: null, pTotalBytes: null);

        var d3d12DestinationTextureCopyLocation = new D3D12_TEXTURE_COPY_LOCATION(d3d12DestinationResource, Sub: 0);
        var d3d12SourceTextureCopyLocation = new D3D12_TEXTURE_COPY_LOCATION(d3d12SourceResource, in sourceFootprint);

        commandList->CopyTextureRegion(&d3d12DestinationTextureCopyLocation, DstX: 0, DstY: 0, DstZ: 0, &d3d12SourceTextureCopyLocation, pSrcBox: null);

        EndCopy();

        void BeginCopy()
        {
            var resourceBarriers = stackalloc D3D12_RESOURCE_BARRIER[2];
            var numResourceBarriers = 0u;

            if (destinationCpuAccess == GraphicsResourceCpuAccess.None)
            {
                resourceBarriers[numResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12DestinationResource,
                    stateBefore: d3d12DestinationResourceState,
                    stateAfter: D3D12_RESOURCE_STATE_COPY_DEST
                );
                numResourceBarriers++;
            }

            if (sourceCpuAccess == GraphicsResourceCpuAccess.None)
            {
                resourceBarriers[numResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12SourceResource,
                    stateBefore: d3d12SourceResourceState,
                    stateAfter: D3D12_RESOURCE_STATE_COPY_SOURCE
                );
                numResourceBarriers++;
            }

            if (numResourceBarriers != 0)
            {
                commandList->ResourceBarrier(numResourceBarriers, resourceBarriers);
            }
        }

        void EndCopy()
        {
            var resourceBarriers = stackalloc D3D12_RESOURCE_BARRIER[2];
            var numResourceBarriers = 0u;

            if (sourceCpuAccess == GraphicsResourceCpuAccess.None)
            {
                resourceBarriers[numResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12SourceResource,
                    stateBefore: D3D12_RESOURCE_STATE_COPY_SOURCE,
                    stateAfter: d3d12SourceResourceState
                );
                numResourceBarriers++;
            }

            if (destinationCpuAccess == GraphicsResourceCpuAccess.None)
            {
                resourceBarriers[numResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12DestinationResource,
                    stateBefore: D3D12_RESOURCE_STATE_COPY_DEST,
                    stateAfter: d3d12DestinationResourceState
                );
                numResourceBarriers++;
            }

            if (numResourceBarriers != 0)
            {
                commandList->ResourceBarrier(numResourceBarriers, resourceBarriers);
            }
        }
    }

    /// <inheritdoc />
    public override void Draw(GraphicsPrimitive primitive)
        => Draw((D3D12GraphicsPrimitive)primitive);

    /// <inheritdoc cref="Draw(GraphicsPrimitive)" />
    public void Draw(D3D12GraphicsPrimitive primitive)
    {
        ThrowIfNull(primitive);

        var commandList = D3D12GraphicsCommandList;
        var pipeline = primitive.Pipeline;

        commandList->SetGraphicsRootSignature(pipeline.Signature.D3D12RootSignature);
        commandList->SetPipelineState(pipeline.D3D12PipelineState);

        var descriptorHeaps = stackalloc ID3D12DescriptorHeap*[1] {
            primitive.D3D12CbvSrvUavDescriptorHeap,
        };
        commandList->SetDescriptorHeaps(1, descriptorHeaps);

        ref readonly var vertexBufferRegion = ref primitive.VertexBufferRegion;
        var vertexBuffer = (D3D12GraphicsBuffer)vertexBufferRegion.Collection;

        var d3d12VertexBufferView = new D3D12_VERTEX_BUFFER_VIEW {
            BufferLocation = vertexBuffer.D3D12Resource->GetGPUVirtualAddress() + vertexBufferRegion.Offset,
            StrideInBytes = primitive.VertexBufferStride,
            SizeInBytes = (uint)vertexBufferRegion.Size,
        };
        commandList->IASetVertexBuffers(StartSlot: 0, NumViews: 1, &d3d12VertexBufferView);

        var inputResourceRegions = primitive.InputResourceRegions;
        var inputResourceRegionsLength = inputResourceRegions.Length;

        var rootDescriptorTableIndex = 0;
        var cbvSrvUavDescriptorHandleIncrementSize = Device.CbvSrvUavDescriptorHandleIncrementSize;

        for (var index = 0; index < inputResourceRegionsLength; index++)
        {
            var inputResourceRegion = inputResourceRegions[index];

            if (inputResourceRegion.Collection is D3D12GraphicsBuffer d3d12GraphicsBuffer)
            {
                var gpuVirtualAddress = d3d12GraphicsBuffer.D3D12Resource->GetGPUVirtualAddress();
                commandList->SetGraphicsRootConstantBufferView(unchecked((uint)index), gpuVirtualAddress + inputResourceRegion.Offset);
            }
            else if (inputResourceRegion.Collection is D3D12GraphicsTexture d3d12GraphicsTexture)
            {
                var gpuDescriptorHandleForHeapStart = primitive.D3D12CbvSrvUavDescriptorHeap->GetGPUDescriptorHandleForHeapStart();
                commandList->SetGraphicsRootDescriptorTable(unchecked((uint)index), gpuDescriptorHandleForHeapStart.Offset(rootDescriptorTableIndex, cbvSrvUavDescriptorHandleIncrementSize));
                rootDescriptorTableIndex++;
            }
        }

        ref readonly var indexBufferRegion = ref primitive.IndexBufferRegion;

        if (indexBufferRegion.Collection is D3D12GraphicsBuffer indexBuffer)
        {
            var indexBufferStride = primitive.IndexBufferStride;
            var indexFormat = DXGI_FORMAT_R16_UINT;

            if (indexBufferStride != 2)
            {
                Assert(AssertionsEnabled && (indexBufferStride == 4));
                indexFormat = DXGI_FORMAT_R32_UINT;
            }

            var d3d12IndexBufferView = new D3D12_INDEX_BUFFER_VIEW {
                BufferLocation = indexBuffer.D3D12Resource->GetGPUVirtualAddress() + indexBufferRegion.Offset,
                SizeInBytes = (uint)indexBufferRegion.Size,
                Format = indexFormat,
            };
            commandList->IASetIndexBuffer(&d3d12IndexBufferView);

            commandList->DrawIndexedInstanced(IndexCountPerInstance: (uint)(indexBufferRegion.Size / indexBufferStride), InstanceCount: 1, StartIndexLocation: 0, BaseVertexLocation: 0, StartInstanceLocation: 0);
        }
        else
        {
            commandList->DrawInstanced(VertexCountPerInstance: (uint)(vertexBufferRegion.Size / primitive.VertexBufferStride), InstanceCount: 1, StartVertexLocation: 0, StartInstanceLocation: 0);
        }
    }

    /// <inheritdoc />
    public override void EndDrawing()
    {
        var renderTargetResourceBarrier = D3D12_RESOURCE_BARRIER.InitTransition(D3D12RenderTargetResource, D3D12_RESOURCE_STATE_RENDER_TARGET, D3D12_RESOURCE_STATE_PRESENT);
        D3D12GraphicsCommandList->ResourceBarrier(1, &renderTargetResourceBarrier);
    }

    /// <inheritdoc />
    public override void EndFrame()
    {
        var commandList = D3D12GraphicsCommandList;

        var commandQueue = Device.D3D12CommandQueue;
        ThrowExternalExceptionIfFailed(commandList->Close(), nameof(ID3D12GraphicsCommandList.Close));
        commandQueue->ExecuteCommandLists(1, (ID3D12CommandList**)&commandList);

        var executeGraphicsFence = WaitForExecuteCompletionFence;
        ThrowExternalExceptionIfFailed(commandQueue->Signal(executeGraphicsFence.D3D12Fence, executeGraphicsFence.D3D12FenceSignalValue), nameof(ID3D12CommandQueue.Signal));

        executeGraphicsFence.Wait();
        executeGraphicsFence.Reset();
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            _d3d12GraphicsCommandList.Dispose(ReleaseIfNotNull);
            _d3d12CommandAllocator.Dispose(ReleaseIfNotNull);
            _d3d12RenderTargetResource.Dispose(ReleaseIfNotNull);

            _waitForExecuteCompletionFence?.Dispose();
            _fence?.Dispose();
        }

        _state.EndDispose();
    }

    internal void OnGraphicsSurfaceSizeChanged(object? sender, PropertyChangedEventArgs<Vector2> eventArgs)
    {
        if (_d3d12RenderTargetView.IsValueCreated)
        {
            _d3d12RenderTargetView.Reset(CreateD3D12RenderTargetDescriptor);

            ReleaseIfNotNull(_d3d12RenderTargetResource.Value);
            _d3d12RenderTargetResource.Reset(CreateD3D12RenderTargetResource);
        }
    }

    private Pointer<ID3D12CommandAllocator> CreateD3D12CommandAllocator()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsContext));

        ID3D12CommandAllocator* d3d12CommandAllocator;
        ThrowExternalExceptionIfFailed(Device.D3D12Device->CreateCommandAllocator(D3D12_COMMAND_LIST_TYPE_DIRECT, __uuidof<ID3D12CommandAllocator>(), (void**)&d3d12CommandAllocator), nameof(ID3D12Device.CreateCommandAllocator));

        return d3d12CommandAllocator;
    }

    private Pointer<ID3D12GraphicsCommandList> CreateD3D12GraphicsCommandList()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsContext));

        ID3D12GraphicsCommandList* d3d12GraphicsCommandList;
        ThrowExternalExceptionIfFailed(Device.D3D12Device->CreateCommandList(nodeMask: 0, D3D12_COMMAND_LIST_TYPE_DIRECT, D3D12CommandAllocator, pInitialState: null, __uuidof<ID3D12GraphicsCommandList>(), (void**)&d3d12GraphicsCommandList), nameof(ID3D12Device.CreateCommandList));

        // Command lists are created in the recording state, but there is nothing
        // to record yet. The main loop expects it to be closed, so close it now.
        ThrowExternalExceptionIfFailed(d3d12GraphicsCommandList->Close(), nameof(ID3D12GraphicsCommandList.Close));

        return d3d12GraphicsCommandList;
    }

    private Pointer<ID3D12Resource> CreateD3D12RenderTargetResource()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsContext));

        ID3D12Resource* renderTargetResource;
        ThrowExternalExceptionIfFailed(Device.DxgiSwapChain->GetBuffer(unchecked((uint)Index), __uuidof<ID3D12Resource>(), (void**)&renderTargetResource), nameof(IDXGISwapChain.GetBuffer));

        return renderTargetResource;
    }

    private D3D12_CPU_DESCRIPTOR_HANDLE CreateD3D12RenderTargetDescriptor()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsContext));

        D3D12_CPU_DESCRIPTOR_HANDLE renderTargetViewHandle;

        var device = Device;
        var d3d12Device = device.D3D12Device;

        var renderTargetViewDesc = new D3D12_RENDER_TARGET_VIEW_DESC {
            Format = device.SwapChainFormat,
            ViewDimension = D3D12_RTV_DIMENSION_TEXTURE2D,
            Anonymous = new D3D12_RENDER_TARGET_VIEW_DESC._Anonymous_e__Union {
                Texture2D = new D3D12_TEX2D_RTV(),
            },
        };

        var renderTargetDescriptorIncrementSize = d3d12Device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_RTV);

        renderTargetViewHandle = device.D3D12RenderTargetDescriptorHeap->GetCPUDescriptorHandleForHeapStart();
        _ = renderTargetViewHandle.Offset(Index, renderTargetDescriptorIncrementSize);

        d3d12Device->CreateRenderTargetView(D3D12RenderTargetResource, &renderTargetViewDesc, renderTargetViewHandle);

        return renderTargetViewHandle;
    }
}
