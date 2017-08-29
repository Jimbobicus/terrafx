// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop.Desktop
{
    [Guid("191CFAC3-A341-470D-B26E-A864F428319C")]
    public /* blittable */ unsafe struct IDXGIOutputDuplication
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IDXGIOutputDuplication* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IDXGIOutputDuplication* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IDXGIOutputDuplication* This
        );
        #endregion

        #region IDXGIObject Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetPrivateData(
            [In] IDXGIOutputDuplication* This,
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In, ComAliasName("UINT")] uint DataSize,
            [In] void* pData
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetPrivateDataInterface(
            [In] IDXGIOutputDuplication* This,
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In] IUnknown* pUnknown = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetPrivateData(
            [In] IDXGIOutputDuplication* This,
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In, Out, ComAliasName("UINT")] uint* pDataSize,
            [Out] void* pData
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetParent(
            [In] IDXGIOutputDuplication* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppParent
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetDesc(
            [In] IDXGIOutputDuplication* This,
            [Out] DXGI_OUTDUPL_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _AcquireNextFrame(
            [In] IDXGIOutputDuplication* This,
            [In, ComAliasName("UINT")] uint TimeoutInMilliseconds,
            [Out] DXGI_OUTDUPL_FRAME_INFO* pFrameInfo,
            [Out] IDXGIResource** ppDesktopResource
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetFrameDirtyRects(
            [In] IDXGIOutputDuplication* This,
            [In, ComAliasName("UINT")] uint DirtyRectsBufferSize,
            [Out, ComAliasName("RECT[]")] RECT* pDirtyRectsBuffer,
            [Out, ComAliasName("UINT")] uint* pDirtyRectsBufferSizeRequired
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetFrameMoveRects(
            [In] IDXGIOutputDuplication* This,
            [In, ComAliasName("UINT")] uint MoveRectsBufferSize,
            [Out, ComAliasName("DXGI_OUTDUPL_MOVE_RECT[]")] DXGI_OUTDUPL_MOVE_RECT* pMoveRectBuffer,
            [Out, ComAliasName("UINT")] uint* pMoveRectsBufferSizeRequired
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetFramePointerShape(
            [In] IDXGIOutputDuplication* This,
            [In, ComAliasName("UINT")] uint PointerShapeBufferSize,
            [Out] void* pPointerShapeBuffer,
            [Out, ComAliasName("UINT")] uint* pPointerShapeBufferSizeRequired,
            [Out] DXGI_OUTDUPL_POINTER_SHAPE_INFO* pPointerShapeInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _MapDesktopSurface(
            [In] IDXGIOutputDuplication* This,
            [Out] DXGI_MAPPED_RECT* pLockedRect
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _UnMapDesktopSurface(
            [In] IDXGIOutputDuplication* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _ReleaseFrame(
            [In] IDXGIOutputDuplication* This
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IDXGIOutputDuplication* This = &this)
            {
                return MarshalFunction<_QueryInterface>(lpVtbl->QueryInterface)(
                    This,
                    riid,
                    ppvObject
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint AddRef()
        {
            fixed (IDXGIOutputDuplication* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IDXGIOutputDuplication* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region IDXGIObject Methods
        [return: ComAliasName("HRESULT")]
        public int SetPrivateData(
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In, ComAliasName("UINT")] uint DataSize,
            [In] void* pData
        )
        {
            fixed (IDXGIOutputDuplication* This = &this)
            {
                return MarshalFunction<_SetPrivateData>(lpVtbl->SetPrivateData)(
                    This,
                    Name,
                    DataSize,
                    pData
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetPrivateDataInterface(
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In] IUnknown* pUnknown = null
        )
        {
            fixed (IDXGIOutputDuplication* This = &this)
            {
                return MarshalFunction<_SetPrivateDataInterface>(lpVtbl->SetPrivateDataInterface)(
                    This,
                    Name,
                    pUnknown
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetPrivateData(
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In, Out, ComAliasName("UINT")] uint* pDataSize,
            [Out] void* pData
        )
        {
            fixed (IDXGIOutputDuplication* This = &this)
            {
                return MarshalFunction<_GetPrivateData>(lpVtbl->GetPrivateData)(
                    This,
                    Name,
                    pDataSize,
                    pData
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetParent(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppParent
        )
        {
            fixed (IDXGIOutputDuplication* This = &this)
            {
                return MarshalFunction<_GetParent>(lpVtbl->GetParent)(
                    This,
                    riid,
                    ppParent
                );
            }
        }
        #endregion

        #region Methods
        public void GetDesc(
            [Out] DXGI_OUTDUPL_DESC* pDesc
        )
        {
            fixed (IDXGIOutputDuplication* This = &this)
            {
                MarshalFunction<_GetDesc>(lpVtbl->GetDesc)(
                    This,
                    pDesc
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int AcquireNextFrame(
            [In, ComAliasName("UINT")] uint TimeoutInMilliseconds,
            [Out] DXGI_OUTDUPL_FRAME_INFO* pFrameInfo,
            [Out] IDXGIResource** ppDesktopResource
        )
        {
            fixed (IDXGIOutputDuplication* This = &this)
            {
                return MarshalFunction<_AcquireNextFrame>(lpVtbl->AcquireNextFrame)(
                    This,
                    TimeoutInMilliseconds,
                    pFrameInfo,
                    ppDesktopResource
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetFrameDirtyRects(
            [In, ComAliasName("UINT")] uint DirtyRectsBufferSize,
            [Out, ComAliasName("RECT[]")] RECT* pDirtyRectsBuffer,
            [Out, ComAliasName("UINT")] uint* pDirtyRectsBufferSizeRequired
        )
        {
            fixed (IDXGIOutputDuplication* This = &this)
            {
                return MarshalFunction<_GetFrameDirtyRects>(lpVtbl->GetFrameDirtyRects)(
                    This,
                    DirtyRectsBufferSize,
                    pDirtyRectsBuffer,
                    pDirtyRectsBufferSizeRequired
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetFrameMoveRects(
            [In, ComAliasName("UINT")] uint MoveRectsBufferSize,
            [Out, ComAliasName("DXGI_OUTDUPL_MOVE_RECT[]")] DXGI_OUTDUPL_MOVE_RECT* pMoveRectBuffer,
            [Out, ComAliasName("UINT")] uint* pMoveRectsBufferSizeRequired
        )
        {
            fixed (IDXGIOutputDuplication* This = &this)
            {
                return MarshalFunction<_GetFrameMoveRects>(lpVtbl->GetFrameMoveRects)(
                    This,
                    MoveRectsBufferSize,
                    pMoveRectBuffer,
                    pMoveRectsBufferSizeRequired
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetFramePointerShape(
            [In, ComAliasName("UINT")] uint PointerShapeBufferSize,
            [Out] void* pPointerShapeBuffer,
            [Out, ComAliasName("UINT")] uint* pPointerShapeBufferSizeRequired,
            [Out] DXGI_OUTDUPL_POINTER_SHAPE_INFO* pPointerShapeInfo
        )
        {
            fixed (IDXGIOutputDuplication* This = &this)
            {
                return MarshalFunction<_GetFramePointerShape>(lpVtbl->GetFramePointerShape)(
                    This,
                    PointerShapeBufferSize,
                    pPointerShapeBuffer,
                    pPointerShapeBufferSizeRequired,
                    pPointerShapeInfo
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int MapDesktopSurface(
            [Out] DXGI_MAPPED_RECT* pLockedRect
        )
        {
            fixed (IDXGIOutputDuplication* This = &this)
            {
                return MarshalFunction<_MapDesktopSurface>(lpVtbl->MapDesktopSurface)(
                    This,
                    pLockedRect
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int UnMapDesktopSurface()
        {
            fixed (IDXGIOutputDuplication* This = &this)
            {
                return MarshalFunction<_UnMapDesktopSurface>(lpVtbl->UnMapDesktopSurface)(
                    This
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int ReleaseFrame()
        {
            fixed (IDXGIOutputDuplication* This = &this)
            {
                return MarshalFunction<_ReleaseFrame>(lpVtbl->ReleaseFrame)(
                    This
                );
            }
        }
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region IDXGIObject Fields
            public IntPtr SetPrivateData;

            public IntPtr SetPrivateDataInterface;

            public IntPtr GetPrivateData;

            public IntPtr GetParent;
            #endregion

            #region Fields
            public IntPtr GetDesc;

            public IntPtr AcquireNextFrame;

            public IntPtr GetFrameDirtyRects;

            public IntPtr GetFrameMoveRects;

            public IntPtr GetFramePointerShape;

            public IntPtr MapDesktopSurface;

            public IntPtr UnMapDesktopSurface;

            public IntPtr ReleaseFrame;
            #endregion
        }
        #endregion
    }
}

