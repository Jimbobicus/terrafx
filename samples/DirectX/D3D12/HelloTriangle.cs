// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from D3D12HelloWindow.h in https://github.com/Microsoft/DirectX-Graphics-Samples
// Original source is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Interop.D3D_FEATURE_LEVEL;
using static TerraFX.Interop.D3D_PRIMITIVE_TOPOLOGY;
using static TerraFX.Interop.D3D_ROOT_SIGNATURE_VERSION;
using static TerraFX.Interop.D3D12;
using static TerraFX.Interop.D3D12_BLEND;
using static TerraFX.Interop.D3D12_BLEND_OP;
using static TerraFX.Interop.D3D12_COLOR_WRITE_ENABLE;
using static TerraFX.Interop.D3D12_COMMAND_LIST_TYPE;
using static TerraFX.Interop.D3D12_COMMAND_QUEUE_FLAGS;
using static TerraFX.Interop.D3D12_CONSERVATIVE_RASTERIZATION_MODE;
using static TerraFX.Interop.D3D12_CPU_PAGE_PROPERTY;
using static TerraFX.Interop.D3D12_CULL_MODE;
using static TerraFX.Interop.D3D12_DESCRIPTOR_HEAP_FLAGS;
using static TerraFX.Interop.D3D12_DESCRIPTOR_HEAP_TYPE;
using static TerraFX.Interop.D3D12_FENCE_FLAGS;
using static TerraFX.Interop.D3D12_FILL_MODE;
using static TerraFX.Interop.D3D12_HEAP_FLAGS;
using static TerraFX.Interop.D3D12_HEAP_TYPE;
using static TerraFX.Interop.D3D12_INPUT_CLASSIFICATION;
using static TerraFX.Interop.D3D12_LOGIC_OP;
using static TerraFX.Interop.D3D12_MEMORY_POOL;
using static TerraFX.Interop.D3D12_PRIMITIVE_TOPOLOGY_TYPE;
using static TerraFX.Interop.D3D12_RESOURCE_BARRIER_FLAGS;
using static TerraFX.Interop.D3D12_RESOURCE_BARRIER_TYPE;
using static TerraFX.Interop.D3D12_RESOURCE_DIMENSION;
using static TerraFX.Interop.D3D12_RESOURCE_FLAGS;
using static TerraFX.Interop.D3D12_RESOURCE_STATES;
using static TerraFX.Interop.D3D12_ROOT_SIGNATURE_FLAGS;
using static TerraFX.Interop.D3D12_TEXTURE_LAYOUT;
using static TerraFX.Interop.D3DCompiler;
using static TerraFX.Interop.DXGI;
using static TerraFX.Interop.DXGI_FORMAT;
using static TerraFX.Interop.DXGI_SWAP_EFFECT;
using static TerraFX.Interop.Kernel32;
using static TerraFX.Interop.Windows;
using static TerraFX.Samples.DirectX.D3D12.DXSampleHelper;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Samples.DirectX.D3D12
{
    public unsafe class HelloTriangle : DXSample
    {
        #region Constants
        private const uint FrameCount = 2;
        #endregion

        #region Fields
        // Pipeline objects
        private D3D12_VIEWPORT _viewport;
        private RECT _scissorRect;
        private IDXGISwapChain3* _swapChain;
        private ID3D12Device* _device;
        private RenderTargets_e__FixedBuffer _renderTargets;
        private ID3D12CommandAllocator* _commandAllocator;
        private ID3D12CommandQueue* _commandQueue;
        private ID3D12RootSignature* _rootSignature;
        private ID3D12DescriptorHeap* _rtvHeap;
        private ID3D12PipelineState* _pipelineState;
        private ID3D12GraphicsCommandList* _commandList;
        private uint _rtvDescriptorSize;

        // App resources.
        private ID3D12Resource* _vertexBuffer;
        private D3D12_VERTEX_BUFFER_VIEW _vertexBufferView;

        // Synchronization objects.
        private uint _frameIndex;
        private IntPtr _fenceEvent;
        private ID3D12Fence* _fence;
        private ulong _fenceValue;
        #endregion

        #region Constructors
        public HelloTriangle(uint width, uint height, string name)
            : base(width, height, name)
        {
            _viewport = new D3D12_VIEWPORT {
                TopLeftX = 0,
                TopLeftY = 0,
                Width = width,
                Height = height,
                MinDepth = D3D12_MIN_DEPTH,
                MaxDepth = D3D12_MAX_DEPTH
            };

            _scissorRect = new RECT {
                left = 0,
                top = 0,
                right = unchecked((int)width),
                bottom = unchecked((int)height)
            };
        }
        #endregion

        #region Methods
        public override void OnInit()
        {
            LoadPipeline();
            LoadAssets();
        }

        // Update frame-based values.
        public override void OnUpdate()
        {
        }

        // Render the scene.
        public override void OnRender()
        {
            // Record all the commands we need to render the scene into the command list.
            PopulateCommandList();

            // Execute the command list.
            var ppCommandLists = stackalloc ID3D12CommandList*[1];
            {
                ppCommandLists[0] = (ID3D12CommandList*)_commandList;
            }
            _commandQueue->ExecuteCommandLists(1, ppCommandLists);

            // Present the frame.
            ThrowIfFailed(nameof(IDXGISwapChain3.Present), _swapChain->Present(1, 0));

            WaitForPreviousFrame();
        }

        public override void OnDestroy()
        {
            // Ensure that the GPU is no longer referencing resources that are about to be
            // cleaned up by the destructor.
            WaitForPreviousFrame();

            CloseHandle(_fenceEvent);
        }

        // Load the rendering pipeline dependencies.
        private void LoadPipeline()
        {
            Guid iid;
            ID3D12Debug* debugController = null;
            IDXGIFactory4* factory = null;
            IDXGIAdapter* adapter = null;
            IDXGISwapChain1* swapChain = null;

            try
            {
                var dxgiFactoryFlags = 0u;

#if DEBUG
                // Enable the debug layer (requires the Graphics Tools "optional feature").
                // NOTE: Enabling the debug layer after device creation will invalidate the active device.
                {
                    iid = IID_ID3D12Debug;
                    if (SUCCEEDED(D3D12GetDebugInterface(&iid, (void**)&debugController)))
                    {
                        debugController->EnableDebugLayer();

                        // Enable additional debug layers.
                        dxgiFactoryFlags |= DXGI_CREATE_FACTORY_DEBUG;
                    }
                }
#endif

                iid = IID_IDXGIFactory4;
                ThrowIfFailed(nameof(CreateDXGIFactory2), CreateDXGIFactory2(dxgiFactoryFlags, &iid, (void**)&factory));

                if (_useWarpDevice)
                {
                    iid = IID_IDXGIAdapter;
                    ThrowIfFailed(nameof(IDXGIFactory4.EnumWarpAdapter), factory->EnumWarpAdapter(&iid, (void**)&adapter));
                }
                else
                {
                    adapter = GetHardwareAdapter(factory);
                }

                fixed (ID3D12Device** device = &_device)
                {
                    iid = IID_ID3D12Device;
                    ThrowIfFailed(nameof(D3D12CreateDevice), D3D12CreateDevice((IUnknown*)adapter, D3D_FEATURE_LEVEL_11_0, &iid, (void**)device));
                }

                // Describe and create the command queue.
                var queueDesc = new D3D12_COMMAND_QUEUE_DESC {
                    Flags = D3D12_COMMAND_QUEUE_FLAG_NONE,
                    Type = D3D12_COMMAND_LIST_TYPE_DIRECT
                };

                fixed (ID3D12CommandQueue** commandQueue = &_commandQueue)
                {
                    iid = IID_ID3D12CommandQueue;
                    ThrowIfFailed(nameof(ID3D12Device.CreateCommandQueue), _device->CreateCommandQueue(&queueDesc, &iid, (void**)commandQueue));
                }

                // Describe and create the swap chain.
                var swapChainDesc = new DXGI_SWAP_CHAIN_DESC1 {
                    BufferCount = FrameCount,
                    Width = _width,
                    Height = _height,
                    Format = DXGI_FORMAT_R8G8B8A8_UNORM,
                    BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT,
                    SwapEffect = DXGI_SWAP_EFFECT_FLIP_DISCARD,
                    SampleDesc = new DXGI_SAMPLE_DESC {
                        Count = 1
                    }
                };

                ThrowIfFailed(nameof(IDXGIFactory4.CreateSwapChainForHwnd), factory->CreateSwapChainForHwnd(
                    (IUnknown*)_commandQueue,         // Swap chain needs the queue so that it can force a flush on it.
                    Win32Application.Hwnd,
                    &swapChainDesc,
                    null,
                    null,
                    &swapChain
                ));

                // This sample does not support fullscreen transitions.
                ThrowIfFailed(nameof(IDXGIFactory4.MakeWindowAssociation), factory->MakeWindowAssociation(Win32Application.Hwnd, DXGI_MWA_NO_ALT_ENTER));

                fixed (IDXGISwapChain3** swapChain3 = &_swapChain)
                {
                    iid = IID_IDXGISwapChain3;
                    ThrowIfFailed(nameof(IDXGISwapChain1.QueryInterface), swapChain->QueryInterface(&iid, (void**)swapChain3));
                    _frameIndex = _swapChain->GetCurrentBackBufferIndex();
                }

                // Create descriptor heaps.
                {
                    // Describe and create a render target view (RTV) descriptor heap.
                    var rtvHeapDesc = new D3D12_DESCRIPTOR_HEAP_DESC {
                        NumDescriptors = FrameCount,
                        Type = D3D12_DESCRIPTOR_HEAP_TYPE_RTV,
                        Flags = D3D12_DESCRIPTOR_HEAP_FLAG_NONE
                    };

                    fixed (ID3D12DescriptorHeap** rtvHeap = &_rtvHeap)
                    {
                        iid = IID_ID3D12DescriptorHeap;
                        ThrowIfFailed(nameof(ID3D12Device.CreateDescriptorHeap), _device->CreateDescriptorHeap(&rtvHeapDesc, &iid, (void**)rtvHeap));
                    }

                    _rtvDescriptorSize = _device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_RTV);
                }

                // Create frame resources.
                {
                    D3D12_CPU_DESCRIPTOR_HANDLE rtvHandle;
                    _rtvHeap->GetCPUDescriptorHandleForHeapStart(&rtvHandle);

                    // Create a RTV for each frame.
                    iid = IID_ID3D12Resource;

                    for (var n = 0u; n < FrameCount; n++)
                    {
                        ID3D12Resource* renderTarget;
                        {
                            ThrowIfFailed(nameof(IDXGISwapChain3.GetBuffer), _swapChain->GetBuffer(n, &iid, (void**)&renderTarget));
                            _device->CreateRenderTargetView(renderTarget, null, rtvHandle);
                            rtvHandle.ptr = (UIntPtr)((byte*)rtvHandle.ptr + _rtvDescriptorSize);
                        }
                        _renderTargets[unchecked((int)n)] = renderTarget;
                    }
                }

                fixed (ID3D12CommandAllocator** commandAllocator = &_commandAllocator)
                {
                    iid = IID_ID3D12CommandAllocator;
                    ThrowIfFailed(nameof(ID3D12Device.CreateRenderTargetView), _device->CreateCommandAllocator(D3D12_COMMAND_LIST_TYPE_DIRECT, &iid, (void**)commandAllocator));
                }
            }
            finally
            {
                if (debugController != null)
                {
                    debugController->Release();
                }

                if (factory != null)
                {
                    factory->Release();
                }

                if (adapter != null)
                {
                    adapter->Release();
                }

                if (swapChain != null)
                {
                    swapChain->Release();
                }
            }
        }

        // Load the sample assets.
        private void LoadAssets()
        {
            Guid iid;
            ID3DBlob* signature = null;
            ID3DBlob* error = null;
            ID3DBlob* vertexShader = null;
            ID3DBlob* pixelShader = null;

            try
            {
                // Create an empty root signature.
                {
                    var rootSignatureDesc = new D3D12_ROOT_SIGNATURE_DESC {
                        Flags = D3D12_ROOT_SIGNATURE_FLAG_ALLOW_INPUT_ASSEMBLER_INPUT_LAYOUT
                    };

                    ThrowIfFailed(nameof(D3D12SerializeRootSignature), D3D12SerializeRootSignature(&rootSignatureDesc, D3D_ROOT_SIGNATURE_VERSION_1, &signature, &error));

                    fixed (ID3D12RootSignature** rootSignature = &_rootSignature)
                    {
                        iid = IID_ID3D12RootSignature;
                        ThrowIfFailed(nameof(ID3D12Device._CreateRootSignature), _device->CreateRootSignature(0, signature->GetBufferPointer(), signature->GetBufferSize(), &iid, (void**)rootSignature));
                    }
                }

                // Create the pipeline state, which includes compiling and loading shaders.
                {
#if DEBUG
                    // Enable better shader debugging with the graphics debugging tools.
                    var compileFlags = D3DCOMPILE_DEBUG | D3DCOMPILE_SKIP_OPTIMIZATION;
#else
                    var compileFlags = 0u;
#endif
                    fixed (char* fileName = GetAssetFullPath(@"D3D12\Assets\Shaders\HelloTriangle.hlsl"))
                    {
                        var entryPoint = 0x00006E69614D5356;    // VSMain
                        var target = 0x0000305F355F7376;        // vs_5_0
                        ThrowIfFailed(nameof(D3DCompileFromFile), D3DCompileFromFile(fileName, null, null, (sbyte*)&entryPoint, (sbyte*)&target, compileFlags, 0, &vertexShader, null));

                        entryPoint = 0x00006E69614D5350;        // PSMain
                        target = 0x0000305F355F7370;            // ps_5_0
                        ThrowIfFailed(nameof(D3DCompileFromFile), D3DCompileFromFile(fileName, null, null, (sbyte*)&entryPoint, (sbyte*)&target, compileFlags, 0, &pixelShader, null));
                    }

                    // Define the vertex input layout.
                    var inputElementDescs = stackalloc D3D12_INPUT_ELEMENT_DESC[2];
                    {
                        var semanticName0 = stackalloc sbyte[9];
                        {
                            ((ulong*)semanticName0)[0] = 0x4E4F495449534F50;      // POSITION
                        }
                        inputElementDescs[0] = new D3D12_INPUT_ELEMENT_DESC {
                            SemanticName = semanticName0,
                            SemanticIndex = 0,
                            Format = DXGI_FORMAT_R32G32B32_FLOAT,
                            InputSlot = 0,
                            AlignedByteOffset = 0,
                            InputSlotClass = D3D12_INPUT_CLASSIFICATION_PER_VERTEX_DATA,
                            InstanceDataStepRate = 0
                        };

                        var semanticName1 = 0x000000524F4C4F43;                     // COLOR
                        inputElementDescs[1] = new D3D12_INPUT_ELEMENT_DESC {
                            SemanticName = (sbyte*)&semanticName1,
                            SemanticIndex = 0,
                            Format = DXGI_FORMAT_R32G32B32A32_FLOAT,
                            InputSlot = 0,
                            AlignedByteOffset = 12,
                            InputSlotClass = D3D12_INPUT_CLASSIFICATION_PER_VERTEX_DATA,
                            InstanceDataStepRate = 0
                        };
                    }

                    // Describe and create the graphics pipeline state object (PSO).
                    var psoDesc = new D3D12_GRAPHICS_PIPELINE_STATE_DESC {
                        InputLayout = new D3D12_INPUT_LAYOUT_DESC {
                            pInputElementDescs = inputElementDescs,
                            NumElements = 2
                        },
                        pRootSignature = _rootSignature,
                        VS = new D3D12_SHADER_BYTECODE {
                            pShaderBytecode = vertexShader->GetBufferPointer(),
                            BytecodeLength = vertexShader->GetBufferSize()
                        },
                        PS = new D3D12_SHADER_BYTECODE {
                            pShaderBytecode = pixelShader->GetBufferPointer(),
                            BytecodeLength = pixelShader->GetBufferSize()
                        },
                        RasterizerState = new D3D12_RASTERIZER_DESC {
                            FillMode = D3D12_FILL_MODE_SOLID,
                            CullMode = D3D12_CULL_MODE_BACK,
                            FrontCounterClockwise = FALSE,
                            DepthBias = D3D12_DEFAULT_DEPTH_BIAS,
                            DepthBiasClamp = D3D12_DEFAULT_DEPTH_BIAS_CLAMP,
                            SlopeScaledDepthBias = D3D12_DEFAULT_SLOPE_SCALED_DEPTH_BIAS,
                            DepthClipEnable = TRUE,
                            MultisampleEnable = FALSE,
                            AntialiasedLineEnable = FALSE,
                            ForcedSampleCount = 0,
                            ConservativeRaster = D3D12_CONSERVATIVE_RASTERIZATION_MODE_OFF
                        },
                        BlendState = new D3D12_BLEND_DESC {
                            AlphaToCoverageEnable = FALSE,
                            IndependentBlendEnable = FALSE
                        },
                        DepthStencilState = new D3D12_DEPTH_STENCIL_DESC {
                            DepthEnable = FALSE,
                            StencilEnable = FALSE
                        },
                        SampleMask = uint.MaxValue,
                        PrimitiveTopologyType = D3D12_PRIMITIVE_TOPOLOGY_TYPE_TRIANGLE,
                        NumRenderTargets = 1,
                        SampleDesc = new DXGI_SAMPLE_DESC {
                            Count = 1
                        }
                    };
                    {
                        var defaultRenderTargetBlendDesc = new D3D12_RENDER_TARGET_BLEND_DESC {
                            BlendEnable = FALSE,
                            LogicOpEnable = FALSE,
                            SrcBlend = D3D12_BLEND_ONE,
                            DestBlend = D3D12_BLEND_ZERO,
                            BlendOp = D3D12_BLEND_OP_ADD,
                            SrcBlendAlpha = D3D12_BLEND_ONE,
                            DestBlendAlpha = D3D12_BLEND_ZERO,
                            BlendOpAlpha = D3D12_BLEND_OP_ADD,
                            LogicOp = D3D12_LOGIC_OP_NOOP,
                            RenderTargetWriteMask = (byte)D3D12_COLOR_WRITE_ENABLE_ALL
                        };

                        for (var i = 0; i < D3D12_SIMULTANEOUS_RENDER_TARGET_COUNT; ++i)
                        {
                            psoDesc.BlendState.RenderTarget[i] = defaultRenderTargetBlendDesc;
                        }

                        psoDesc.RTVFormats[0] = DXGI_FORMAT_R8G8B8A8_UNORM;
                    }
                    fixed (ID3D12PipelineState** pipelineState = &_pipelineState)
                    {
                        iid = IID_ID3D12PipelineState;
                        ThrowIfFailed(nameof(ID3D12Device._CreateGraphicsPipelineState), _device->CreateGraphicsPipelineState(&psoDesc, &iid, (void**)pipelineState));
                    }
                }

                // Create the command list.
                fixed (ID3D12GraphicsCommandList** commandList = &_commandList)
                {
                    iid = IID_ID3D12GraphicsCommandList;
                    ThrowIfFailed(nameof(ID3D12Device.CreateCommandList), _device->CreateCommandList(0, D3D12_COMMAND_LIST_TYPE_DIRECT, _commandAllocator, _pipelineState, &iid, (void**)commandList));
                }

                // Command lists are created in the recording state, but there is nothing
                // to record yet. The main loop expects it to be closed, so close it now.
                ThrowIfFailed(nameof(ID3D12GraphicsCommandList.Close), _commandList->Close());

                // Create the vertex buffer.
                {
                    // Define the geometry for a triangle.
                    var triangleVertices = stackalloc Vertex[3];
                    {
                        triangleVertices[0] = new Vertex {
                            Position = new Vector3(0.0f, 0.25f * _aspectRatio, 0.0f),
                            Color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f)
                        };
                        triangleVertices[1] = new Vertex {
                            Position = new Vector3(0.25f, -0.25f * _aspectRatio, 0.0f),
                            Color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f)
                        };
                        triangleVertices[2] = new Vertex {
                            Position = new Vector3(-0.25f, -0.25f * _aspectRatio, 0.0f),
                            Color = new Vector4(0.0f, 0.0f, 1.0f, 1.0f)
                        };
                    }

                    var vertexBufferSize = SizeOf<Vertex>() * 3;

                    // Note: using upload heaps to transfer static data like vert buffers is not
                    // recommended. Every time the GPU needs it, the upload heap will be marshalled
                    // over. Please read up on Default Heap usage. An upload heap is used here for
                    // code simplicity and because there are very few verts to actually transfer.
                    fixed (ID3D12Resource** vertexBuffer = &_vertexBuffer)
                    {
                        var heapProperties = new D3D12_HEAP_PROPERTIES {
                            Type = D3D12_HEAP_TYPE_UPLOAD,
                            CPUPageProperty = D3D12_CPU_PAGE_PROPERTY_UNKNOWN,
                            MemoryPoolPreference = D3D12_MEMORY_POOL_UNKNOWN,
                            CreationNodeMask = 1,
                            VisibleNodeMask = 1
                        };

                        var bufferDesc = new D3D12_RESOURCE_DESC {
                            Dimension = D3D12_RESOURCE_DIMENSION_BUFFER,
                            Alignment = 0,
                            Width = vertexBufferSize,
                            Height = 1,
                            DepthOrArraySize = 1,
                            MipLevels = 1,
                            Format = DXGI_FORMAT_UNKNOWN,
                            SampleDesc = new DXGI_SAMPLE_DESC {
                                Count = 1,
                                Quality = 0,
                            },
                            Layout = D3D12_TEXTURE_LAYOUT_ROW_MAJOR,
                            Flags = D3D12_RESOURCE_FLAG_NONE
                        };

                        iid = IID_ID3D12Resource;
                        ThrowIfFailed(nameof(ID3D12Device._CreateCommittedResource), _device->CreateCommittedResource(
                            &heapProperties,
                            D3D12_HEAP_FLAG_NONE,
                            &bufferDesc,
                            D3D12_RESOURCE_STATE_GENERIC_READ,
                            null,
                            &iid,
                            (void**)vertexBuffer
                        ));
                    }

                    // Copy the triangle data to the vertex buffer.
                    byte* pVertexDataBegin;
                    var readRange = new D3D12_RANGE {       // We do not intend to read from this resource on the CPU.
                        Begin = UIntPtr.Zero,
                        End = UIntPtr.Zero
                    };
                    ThrowIfFailed(nameof(ID3D12Resource._Map), _vertexBuffer->Map(0, &readRange, (void**)&pVertexDataBegin));
                    Unsafe.CopyBlock(pVertexDataBegin, triangleVertices, vertexBufferSize);
                    _vertexBuffer->Unmap(0, null);

                    // Initialize the vertex buffer view.
                    _vertexBufferView.BufferLocation = _vertexBuffer->GetGPUVirtualAddress();
                    _vertexBufferView.StrideInBytes = SizeOf<Vertex>();
                    _vertexBufferView.SizeInBytes = vertexBufferSize;
                }

                // Create synchronization objects and wait until assets have been uploaded to the GPU.
                {
                    fixed (ID3D12Fence** fence = &_fence)
                    {
                        iid = IID_ID3D12Fence;
                        ThrowIfFailed(nameof(ID3D12Device.CreateFence), _device->CreateFence(0, D3D12_FENCE_FLAG_NONE, &iid, (void**)fence));
                        _fenceValue = 1;
                    }

                    // Create an event handle to use for frame synchronization.
                    _fenceEvent = CreateEvent(null, FALSE, FALSE, null);
                    if (_fenceEvent == IntPtr.Zero)
                    {
                        ThrowExternalExceptionForLastHRESULT(nameof(CreateEvent));
                    }

                    // Wait for the command list to execute; we are reusing the same command
                    // list in our main loop but for now, we just want to wait for setup to
                    // complete before continuing.
                    WaitForPreviousFrame();
                }
            }
            finally
            {
                if (signature != null)
                {
                    signature->Release();
                }

                if (error != null)
                {
                    error->Release();
                }

                if (vertexShader != null)
                {
                    vertexShader->Release();
                }

                if (pixelShader != null)
                {
                    pixelShader->Release();
                }
            }
        }

        private void PopulateCommandList()
        {
            // Command list allocators can only be reset when the associated
            // command lists have finished execution on the GPU; apps should use
            // fences to determine GPU execution progress.
            ThrowIfFailed(nameof(ID3D12CommandAllocator.Reset), _commandAllocator->Reset());

            // However, when ExecuteCommandList() is called on a particular command
            // list, that command list can then be reset at any time and must be before
            // re-recording.
            ThrowIfFailed(nameof(ID3D12GraphicsCommandList.Reset), _commandList->Reset(_commandAllocator, _pipelineState));

            // Set necessary state.
            _commandList->SetGraphicsRootSignature(_rootSignature);

            fixed (D3D12_VIEWPORT* viewport = &_viewport)
            {
                _commandList->RSSetViewports(1, viewport);
            }

            fixed (RECT* scissorRect = &_scissorRect)
            {
                _commandList->RSSetScissorRects(1, scissorRect);
            }

            // Indicate that the back buffer will be used as a render target.
            var barrier = new D3D12_RESOURCE_BARRIER {
                Type = D3D12_RESOURCE_BARRIER_TYPE_TRANSITION,
                Flags = D3D12_RESOURCE_BARRIER_FLAG_NONE,
                Transition = new D3D12_RESOURCE_TRANSITION_BARRIER {
                    pResource = _renderTargets[unchecked((int)_frameIndex)],
                    StateBefore = D3D12_RESOURCE_STATE_PRESENT,
                    StateAfter = D3D12_RESOURCE_STATE_RENDER_TARGET,
                    Subresource = D3D12_RESOURCE_BARRIER_ALL_SUBRESOURCES
                }
            };
            _commandList->ResourceBarrier(1, &barrier);

            D3D12_CPU_DESCRIPTOR_HANDLE rtvHandle;
            _rtvHeap->GetCPUDescriptorHandleForHeapStart(&rtvHandle);
            rtvHandle.ptr = (UIntPtr)((byte*)rtvHandle.ptr + _frameIndex * _rtvDescriptorSize);
            _commandList->OMSetRenderTargets(1, &rtvHandle, FALSE, null);

            // Record commands.
            var clearColor = stackalloc float[4];
            {
                clearColor[0] = 0.0f;
                clearColor[1] = 0.2f;
                clearColor[2] = 0.4f;
                clearColor[3] = 1.0f;
            }
            _commandList->ClearRenderTargetView(rtvHandle, clearColor, 0, null);
            _commandList->IASetPrimitiveTopology(D3D_PRIMITIVE_TOPOLOGY_TRIANGLELIST);

            fixed (D3D12_VERTEX_BUFFER_VIEW* vertexBufferView = &_vertexBufferView)
            {
                _commandList->IASetVertexBuffers(0, 1, vertexBufferView);
            }

            _commandList->DrawInstanced(3, 1, 0, 0);

            // Indicate that the back buffer will now be used to present.
            {
                barrier.Transition.StateBefore = D3D12_RESOURCE_STATE_RENDER_TARGET;
                barrier.Transition.StateAfter = D3D12_RESOURCE_STATE_PRESENT;
            }
            _commandList->ResourceBarrier(1, &barrier);

            ThrowIfFailed(nameof(ID3D12GraphicsCommandList.Close), _commandList->Close());
        }

        private void WaitForPreviousFrame()
        {
            // WAITING FOR THE FRAME TO COMPLETE BEFORE CONTINUING IS NOT BEST PRACTICE.
            // This is code implemented as such for simplicity. The D3D12HelloFrameBuffering
            // sample illustrates how to use fences for efficient resource usage and to
            // maximize GPU utilization.

            // Signal and increment the fence value.
            var fence = _fenceValue;
            ThrowIfFailed(nameof(ID3D12CommandQueue.Signal), _commandQueue->Signal(_fence, fence));
            _fenceValue++;

            // Wait until the previous frame is finished.
            if (_fence->GetCompletedValue() < fence)
            {
                ThrowIfFailed(nameof(ID3D12Fence.SetEventOnCompletion), _fence->SetEventOnCompletion(fence, _fenceEvent));
                WaitForSingleObject(_fenceEvent, INFINITE);
            }

            _frameIndex = _swapChain->GetCurrentBackBufferIndex();
        }

        protected override void Dispose(bool isDisposing)
        {
            var swapChain = _swapChain;

            if (swapChain != null)
            {
                _swapChain = null;
                swapChain->Release();
            }

            var device = _device;

            if (device != null)
            {
                _device = null;
                device->Release();
            }

            for (var index = 0; index < FrameCount; index++)
            {
                var renderTarget = _renderTargets[index];

                if (renderTarget != null)
                {
                    _renderTargets[index] = null;
                    renderTarget->Release();
                }
            }

            var commandAllocator = _commandAllocator;

            if (commandAllocator != null)
            {
                _commandAllocator = null;
                commandAllocator->Release();
            }

            var commandQueue = _commandQueue;

            if (commandQueue != null)
            {
                _commandQueue = null;
                commandQueue->Release();
            }

            var rootSignature = _rootSignature;

            if (rootSignature != null)
            {
                _rootSignature = null;
                rootSignature->Release();
            }

            var rtvHeap = _rtvHeap;

            if (rtvHeap != null)
            {
                _rtvHeap = null;
                rtvHeap->Release();
            }

            var pipelineState = _pipelineState;

            if (pipelineState != null)
            {
                _pipelineState = null;
                pipelineState->Release();
            }

            var commandList = _commandList;

            if (commandList != null)
            {
                _commandList = null;
                commandList->Release();
            }

            var vertexBuffer = _vertexBuffer;

            if (vertexBuffer != null)
            {
                _vertexBuffer = null;
                vertexBuffer->Release();
            }

            var fence = _fence;

            if (fence != null)
            {
                _fence = null;
                fence->Release();
            }

            base.Dispose(isDisposing);
        }
        #endregion

        #region Structs
        [Unmanaged]
        private unsafe struct RenderTargets_e__FixedBuffer
        {
            #region Fields
#pragma warning disable CS0649
            public ID3D12Resource* E0;

            public ID3D12Resource* E1;
#pragma warning restore CS0649
            #endregion

            #region Properties
            public ID3D12Resource* this[int index]
            {
                get
                {
                    fixed (ID3D12Resource** e = &E0)
                    {
                        return e[index];
                    }
                }

                set
                {
                    fixed (ID3D12Resource** e = &E0)
                    {
                        e[index] = value;
                    }
                }
            }
            #endregion
        }

        [Unmanaged]
        private struct Vertex
        {
            #region Fields
            public Vector3 Position;

            public Vector4 Color;
            #endregion
        }
        #endregion
    }
}
