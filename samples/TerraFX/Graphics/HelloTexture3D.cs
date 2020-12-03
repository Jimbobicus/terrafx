// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Reflection;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using TerraFX.Numerics;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Samples.Graphics
{
    /// <summary>
    /// Demonstrates the use of
    /// * Quads (Vertex buffer + Index Buffer, see HelloQuad)
    /// * Texture3D (256x256x256, representing the RGB cube, extension of HelloTexture)
    /// * ConstBuffer (transformation matrix as in HelloConstBuffer, but here to animate the 3D texture coordinates)
    /// Will show a quad cutting through the RGB cube and being animated to move back and forth in texture coordinate space.
    /// </summary>
    public class HelloTexture3D : HelloWindow
    {
        private GraphicsPrimitive _quadPrimitive = null!;
        private IGraphicsBuffer _constantBuffer = null!;
        private IGraphicsBuffer _indexBuffer = null!;
        private IGraphicsBuffer _vertexBuffer = null!;
        private float _texturePosition;

        public HelloTexture3D(string name, params Assembly[] compositionAssemblies)
            : base(name, compositionAssemblies)
        {
        }

        public override void Cleanup()
        {
            _quadPrimitive?.Dispose();
            _constantBuffer?.Dispose();
            _indexBuffer?.Dispose();
            _vertexBuffer?.Dispose();
            base.Cleanup();
        }

        public override void Initialize(Application application)
        {
            base.Initialize(application);

            var graphicsDevice = GraphicsDevice;
            var currentGraphicsContext = graphicsDevice.CurrentContext;

            using var vertexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024);
            using var indexStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024);
            using var textureStagingBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Default, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024 * 1024);

            _constantBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Constant, GraphicsResourceCpuAccess.CpuToGpu, 64 * 1024);
            _indexBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Index, GraphicsResourceCpuAccess.GpuOnly, 64 * 1024);
            _vertexBuffer = graphicsDevice.MemoryAllocator.CreateBuffer(GraphicsBufferKind.Vertex, GraphicsResourceCpuAccess.GpuOnly, 64 * 1024);

            currentGraphicsContext.BeginFrame();
            _quadPrimitive = CreateQuadPrimitive(currentGraphicsContext, vertexStagingBuffer, indexStagingBuffer, textureStagingBuffer);
            currentGraphicsContext.EndFrame();

            graphicsDevice.Signal(currentGraphicsContext.Fence);
            graphicsDevice.WaitForIdle();
        }

        protected override unsafe void Update(TimeSpan delta)
        {
            var graphicsDevice = GraphicsDevice;
            var graphicsSurface = graphicsDevice.Surface;
            var scale255_256 = 255f / 256f;
            var aspectRatio = graphicsSurface.Width / graphicsSurface.Height;
            var scaleX = scale255_256;
            var scaleY = scale255_256 / aspectRatio;
            var scaleZ = scale255_256;

            const float TranslationSpeed = MathF.PI;

            var radians = _texturePosition;
            {
                radians += (float)(TranslationSpeed * delta.TotalSeconds);
                radians %= MathF.Tau;
            }
            _texturePosition = radians;
            var z = scaleZ * (0.5f + (0.5f * MathF.Cos(radians)));

            var constantBufferRegion = _quadPrimitive.InputResourceRegions[1];
            var constantBuffer = _constantBuffer;
            var pConstantBuffer = constantBuffer.Map<Matrix4x4>(in constantBufferRegion);

            // Shaders take transposed matrices, so we want to set X.W
            pConstantBuffer[0] = new Matrix4x4(
                new Vector4(scaleX, 0.0f, 0.0f, 0.5f), // +0.5 since the input coordinates are in range [-.5, .5]  but output needs to be [0, 1]
                new Vector4(0.0f, scaleY, 0.0f, 0.5f), // +0.5 since the input coordinates are in range [-.5, .5]  but output needs to be [0, 1]
                new Vector4(0.0f, 0.0f, 1.0f, z),
                new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
            );

            constantBuffer.UnmapAndWrite(in constantBufferRegion);
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.Draw(_quadPrimitive);
            base.Draw(graphicsContext);
        }

        private unsafe GraphicsPrimitive CreateQuadPrimitive(GraphicsContext graphicsContext, IGraphicsBuffer vertexStagingBuffer, IGraphicsBuffer indexStagingBuffer, IGraphicsBuffer textureStagingBuffer)
        {
            var graphicsDevice = GraphicsDevice;
            var graphicsSurface = graphicsDevice.Surface;

            var graphicsPipeline = CreateGraphicsPipeline(graphicsDevice, "Texture3D", "main", "main");

            var constantBuffer = _constantBuffer;
            var indexBuffer = _indexBuffer;
            var vertexBuffer = _vertexBuffer;

            var vertexBufferRegion = CreateVertexBufferRegion(graphicsContext, vertexBuffer, vertexStagingBuffer, aspectRatio: graphicsSurface.Width / graphicsSurface.Height);
            graphicsContext.Copy(vertexBuffer, vertexStagingBuffer);

            var indexBufferRegion = CreateIndexBufferRegion(graphicsContext, indexBuffer, indexStagingBuffer);
            graphicsContext.Copy(indexBuffer, indexStagingBuffer);

            var inputResourceRegions = new GraphicsMemoryRegion<IGraphicsResource>[3] {
                CreateConstantBufferRegion(graphicsContext, constantBuffer),
                CreateConstantBufferRegion(graphicsContext, constantBuffer),
                CreateTexture3DRegion(graphicsContext, textureStagingBuffer),
            };
            return graphicsDevice.CreatePrimitive(graphicsPipeline, vertexBufferRegion, indexBufferRegion, inputResourceRegions);

            static GraphicsMemoryRegion<IGraphicsResource> CreateConstantBufferRegion(GraphicsContext graphicsContext, IGraphicsBuffer constantBuffer)
            {
                var constantBufferRegion = constantBuffer.Allocate(SizeOf<Matrix4x4>(), alignment: 256, stride: SizeOf<Matrix4x4>());
                var pConstantBuffer = constantBuffer.Map<Matrix4x4>(in constantBufferRegion);

                pConstantBuffer[0] = Matrix4x4.Identity;

                constantBuffer.UnmapAndWrite(in constantBufferRegion);
                return constantBufferRegion;
            }

            static GraphicsMemoryRegion<IGraphicsResource> CreateIndexBufferRegion(GraphicsContext graphicsContext, IGraphicsBuffer indexBuffer, IGraphicsBuffer indexStagingBuffer)
            {
                var indexBufferRegion = indexBuffer.Allocate(SizeOf<ushort>() * 6, alignment: 2, stride: SizeOf<ushort>());
                var pIndexBuffer = indexStagingBuffer.Map<ushort>(in indexBufferRegion);

                // clockwise when looking at the triangle from the outside

                pIndexBuffer[0] = 0;
                pIndexBuffer[1] = 1;
                pIndexBuffer[2] = 2;

                pIndexBuffer[3] = 0;
                pIndexBuffer[4] = 2;
                pIndexBuffer[5] = 3;

                indexStagingBuffer.UnmapAndWrite(in indexBufferRegion);
                return indexBufferRegion;
            }

            static GraphicsMemoryRegion<IGraphicsResource> CreateTexture3DRegion(GraphicsContext graphicsContext, IGraphicsBuffer textureStagingBuffer)
            {
                const uint TextureWidth = 256;
                const uint TextureHeight = 256;
                const ushort TextureDepth = 256;
                const uint TextureDz = TextureWidth * TextureHeight;
                const uint TexturePixels = TextureDz * TextureDepth;

                var texture3D = graphicsContext.Device.MemoryAllocator.CreateTexture(GraphicsTextureKind.ThreeDimensional, GraphicsResourceCpuAccess.None, TextureWidth, TextureHeight, TextureDepth);
                var texture3DRegion = texture3D.Allocate(texture3D.Size, alignment: 4, stride: sizeof(uint));
                var pTextureData = textureStagingBuffer.Map<uint>(in texture3DRegion);

                for (uint n = 0; n < TexturePixels; n++)
                {
                    var x = n % TextureWidth;
                    var y = n % TextureDz / TextureWidth;
                    var z = n / TextureDz;

                    pTextureData[n] = 0xFF000000 | (z << 16) | (y << 8) | (x << 0);
                }

                textureStagingBuffer.UnmapAndWrite(in texture3DRegion);
                graphicsContext.Copy(texture3D, textureStagingBuffer);

                return texture3DRegion;
            }

            static GraphicsMemoryRegion<IGraphicsResource> CreateVertexBufferRegion(GraphicsContext graphicsContext, IGraphicsBuffer vertexBuffer, IGraphicsBuffer vertexStagingBuffer, float aspectRatio)
            {
                var vertexBufferRegion = vertexBuffer.Allocate(SizeOf<Texture3DVertex>() * 4, alignment: 16, stride: SizeOf<Texture3DVertex>());
                var pVertexBuffer = vertexStagingBuffer.Map<Texture3DVertex>(in vertexBufferRegion);

                pVertexBuffer[0] = new Texture3DVertex {             //
                    Position = new Vector3(-0.5f, 0.5f, 0.0f),       //   y          in this setup
                    UVW = new Vector3(0, 1, 0.5f),                   //   ^     z    the origin o
                };                                                   //   |   /      is in the middle
                                                                     //   | /        of the rendered scene
                pVertexBuffer[1] = new Texture3DVertex {             //   o------>x
                    Position = new Vector3(0.5f, 0.5f, 0.0f),        //
                    UVW = new Vector3(1, 1, 0.5f),                   //   0 ----- 1
                };                                                   //   | \     |
                                                                     //   |   \   |
                pVertexBuffer[2] = new Texture3DVertex {             //   |     \ |
                    Position = new Vector3(0.5f, -0.5f, 0.0f),       //   3-------2
                    UVW = new Vector3(1, 0, 0.5f),                   //
                };

                pVertexBuffer[3] = new Texture3DVertex {
                    Position = new Vector3(-0.5f, -0.5f, 0.0f),
                    UVW = new Vector3(0, 0, 0.5f),
                };

                vertexStagingBuffer.UnmapAndWrite(in vertexBufferRegion);
                return vertexBufferRegion;
            }

            GraphicsPipeline CreateGraphicsPipeline(GraphicsDevice graphicsDevice, string shaderName, string vertexShaderEntryPoint, string pixelShaderEntryPoint)
            {
                var signature = CreateGraphicsPipelineSignature(graphicsDevice);
                var vertexShader = CompileShader(graphicsDevice, GraphicsShaderKind.Vertex, shaderName, vertexShaderEntryPoint);
                var pixelShader = CompileShader(graphicsDevice, GraphicsShaderKind.Pixel, shaderName, pixelShaderEntryPoint);

                return graphicsDevice.CreatePipeline(signature, vertexShader, pixelShader);
            }

            static GraphicsPipelineSignature CreateGraphicsPipelineSignature(GraphicsDevice graphicsDevice)
            {
                var inputs = new GraphicsPipelineInput[1] {
                    new GraphicsPipelineInput(
                        new GraphicsPipelineInputElement[2] {
                            new GraphicsPipelineInputElement(typeof(Vector3), GraphicsPipelineInputElementKind.Position, size: 12),
                            new GraphicsPipelineInputElement(typeof(Vector3), GraphicsPipelineInputElementKind.TextureCoordinate, size: 12),
                        }
                    ),
                };

                var resources = new GraphicsPipelineResource[3] {
                    new GraphicsPipelineResource(GraphicsPipelineResourceKind.ConstantBuffer, GraphicsShaderVisibility.Vertex),
                    new GraphicsPipelineResource(GraphicsPipelineResourceKind.ConstantBuffer, GraphicsShaderVisibility.Vertex),
                    new GraphicsPipelineResource(GraphicsPipelineResourceKind.Texture, GraphicsShaderVisibility.Pixel),
                };

                return graphicsDevice.CreatePipelineSignature(inputs, resources);
            }
        }
    }
}
