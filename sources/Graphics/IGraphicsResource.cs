// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics
{
    /// <summary>A graphics resource bound to a graphics device.</summary>
    public unsafe interface IGraphicsResource : IGraphicsMemoryRegionCollection<IGraphicsResource>, IDisposable
    {
        /// <summary>Gets the alignment of the resource.</summary>
        ulong Alignment { get; }

        /// <summary>Gets the allocator which created the resource.</summary>
        GraphicsMemoryAllocator Allocator { get; }

        /// <summary>Gets the block which contains the resource.</summary>
        IGraphicsMemoryBlock Block { get; }

        /// <summary>Gets the CPU access capabilities of the resource.</summary>
        GraphicsResourceCpuAccess CpuAccess { get; }

        /// <summary>Gets the memory block region in which the resource exists.</summary>
        ref readonly GraphicsMemoryRegion<IGraphicsMemoryBlock> MemoryBlockRegion { get; }

        /// <summary>Gets the offset of the resource, in bytes.</summary>
        ulong Offset { get; }

        /// <summary>Maps the resource into CPU memory.</summary>
        /// <typeparam name="T">The type of data contained by the resource.</typeparam>
        /// <returns>A pointer to the mapped resource.</returns>
        /// <remarks>This overload should be used when all memory should be mapped.</remarks>
        T* Map<T>()
            where T : unmanaged;

        /// <summary>Maps the resource into CPU memory.</summary>
        /// <typeparam name="T">The type of data contained by the resource.</typeparam>
        /// <param name="region">The region of memory that will be mapped.</param>
        /// <returns>A pointer to the mapped resource.</returns>
        T* Map<T>(in GraphicsMemoryRegion<IGraphicsResource> region)
            where T : unmanaged;

        /// <summary>Maps the resource into CPU memory.</summary>
        /// <typeparam name="T">The type of data contained by the resource.</typeparam>
        /// <param name="range">The range of memory that will be mapped.</param>
        /// <returns>A pointer to the mapped resource.</returns>
        T* Map<T>(Range range)
            where T : unmanaged;

        /// <summary>Maps the resource into CPU memory.</summary>
        /// <typeparam name="T">The type of data contained by the resource.</typeparam>
        /// <param name="rangeOffset">The offset into the resource at which memory will start being read.</param>
        /// <param name="rangeLength">The amount of memory which will be read.</param>
        /// <returns>A pointer to the mapped resource.</returns>
        T* Map<T>(nuint rangeOffset, nuint rangeLength)
            where T : unmanaged;

        /// <summary>Maps the resource into CPU memory.</summary>
        /// <typeparam name="T">The type of data contained by the resource.</typeparam>
        /// <returns>A pointer to the mapped resource.</returns>
        /// <remarks>This overload should be used when all memory will be read.</remarks>
        T* MapForRead<T>()
            where T : unmanaged;

        /// <summary>Maps the resource into CPU memory.</summary>
        /// <typeparam name="T">The type of data contained by the resource.</typeparam>
        /// <param name="readRegion">The region of memory that will be read.</param>
        /// <returns>A pointer to the mapped resource.</returns>
        T* MapForRead<T>(in GraphicsMemoryRegion<IGraphicsResource> readRegion)
            where T : unmanaged;

        /// <summary>Maps the resource into CPU memory.</summary>
        /// <typeparam name="T">The type of data contained by the resource.</typeparam>
        /// <param name="readRange">The range of memory that will be read.</param>
        /// <returns>A pointer to the mapped resource.</returns>
        T* MapForRead<T>(Range readRange)
            where T : unmanaged;

        /// <summary>Maps the resource into CPU memory.</summary>
        /// <typeparam name="T">The type of data contained by the resource.</typeparam>
        /// <param name="readRangeOffset">The offset into the resource at which memory will start being read.</param>
        /// <param name="readRangeLength">The amount of memory which will be read.</param>
        /// <returns>A pointer to the mapped resource.</returns>
        T* MapForRead<T>(nuint readRangeOffset, nuint readRangeLength)
            where T : unmanaged;

        /// <summary>Unmaps the resource from CPU memory.</summary>
        /// <remarks>This overload should be used when no memory was written.</remarks>
        void Unmap();

        /// <summary>Unmaps the resource from CPU memory.</summary>
        /// <remarks>This overload should be used when all memory was written.</remarks>
        void UnmapAndWrite();

        /// <summary>Unmaps the resource from CPU memory.</summary>
        /// <param name="writtenRegion">The region of memory which was written.</param>
        void UnmapAndWrite(in GraphicsMemoryRegion<IGraphicsResource> writtenRegion);

        /// <summary>Unmaps the resource from CPU memory.</summary>
        /// <param name="writtenRange">The range of memory which was written.</param>
        void UnmapAndWrite(Range writtenRange);

        /// <summary>Unmaps the resource from CPU memory.</summary>
        /// <param name="writtenRangeOffset">The offset into the resource at which memory started being written.</param>
        /// <param name="writtenRangeLength">The amount of memory which was written.</param>
        void UnmapAndWrite(nuint writtenRangeOffset, nuint writtenRangeLength);
    }
}
