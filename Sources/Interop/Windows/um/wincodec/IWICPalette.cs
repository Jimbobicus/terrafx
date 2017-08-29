// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("00000040-A8F2-4877-BA0A-FD2B6645FB94")]
    public /* blittable */ unsafe struct IWICPalette
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IWICPalette* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IWICPalette* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IWICPalette* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _InitializePredefined(
            [In] IWICPalette* This,
            [In] WICBitmapPaletteType ePaletteType,
            [In, ComAliasName("BOOL")] int fAddTransparentColor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _InitializeCustom(
            [In] IWICPalette* This,
            [In, ComAliasName("WICColor[]")] uint* pColors,
            [In, ComAliasName("UINT")] uint cCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _InitializeFromBitmap(
            [In] IWICPalette* This,
            [In, Optional] IWICBitmapSource* pISurface,
            [In, ComAliasName("UINT")] uint cCount,
            [In, ComAliasName("BOOL")] int fAddTransparentColor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _InitializeFromPalette(
            [In] IWICPalette* This,
            [In] IWICPalette* pIPalette = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int __GetType(
            [In] IWICPalette* This,
            [Out] WICBitmapPaletteType* pePaletteType
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetColorCount(
            [In] IWICPalette* This,
            [Out, ComAliasName("UINT")] uint* pcCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetColors(
            [In] IWICPalette* This,
            [In, ComAliasName("UINT")] uint cCount,
            [Out, ComAliasName("WICColor[]")] uint* pColors,
            [Out, ComAliasName("UINT")] uint* pcActualColors
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _IsBlackWhite(
            [In] IWICPalette* This,
            [Out, ComAliasName("BOOL")] int* pfIsBlackWhite
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _IsGrayscale(
            [In] IWICPalette* This,
            [Out, ComAliasName("BOOL")] int* pfIsGrayscale
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _HasAlpha(
            [In] IWICPalette* This,
            [Out, ComAliasName("BOOL")] int* pfHasAlpha
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IWICPalette* This = &this)
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
            fixed (IWICPalette* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IWICPalette* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int InitializePredefined(
            [In] WICBitmapPaletteType ePaletteType,
            [In, ComAliasName("BOOL")] int fAddTransparentColor
        )
        {
            fixed (IWICPalette* This = &this)
            {
                return MarshalFunction<_InitializePredefined>(lpVtbl->InitializePredefined)(
                    This,
                    ePaletteType,
                    fAddTransparentColor
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int InitializeCustom(
            [In, ComAliasName("WICColor[]")] uint* pColors,
            [In, ComAliasName("UINT")] uint cCount
        )
        {
            fixed (IWICPalette* This = &this)
            {
                return MarshalFunction<_InitializeCustom>(lpVtbl->InitializeCustom)(
                    This,
                    pColors,
                    cCount
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int InitializeFromBitmap(
            [In, Optional] IWICBitmapSource* pISurface,
            [In, ComAliasName("UINT")] uint cCount,
            [In, ComAliasName("BOOL")] int fAddTransparentColor
        )
        {
            fixed (IWICPalette* This = &this)
            {
                return MarshalFunction<_InitializeFromBitmap>(lpVtbl->InitializeFromBitmap)(
                    This,
                    pISurface,
                    cCount,
                    fAddTransparentColor
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int InitializeFromPalette(
            [In] IWICPalette* pIPalette = null
        )
        {
            fixed (IWICPalette* This = &this)
            {
                return MarshalFunction<_InitializeFromPalette>(lpVtbl->InitializeFromPalette)(
                    This,
                    pIPalette
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int _GetType(
            [Out] WICBitmapPaletteType* pePaletteType
        )
        {
            fixed (IWICPalette* This = &this)
            {
                return MarshalFunction<__GetType>(lpVtbl->_GetType)(
                    This,
                    pePaletteType
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetColorCount(
            [Out, ComAliasName("UINT")] uint* pcCount
        )
        {
            fixed (IWICPalette* This = &this)
            {
                return MarshalFunction<_GetColorCount>(lpVtbl->GetColorCount)(
                    This,
                    pcCount
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetColors(
            [In, ComAliasName("UINT")] uint cCount,
            [Out, ComAliasName("WICColor[]")] uint* pColors,
            [Out, ComAliasName("UINT")] uint* pcActualColors
        )
        {
            fixed (IWICPalette* This = &this)
            {
                return MarshalFunction<_GetColors>(lpVtbl->GetColors)(
                    This,
                    cCount,
                    pColors,
                    pcActualColors
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int IsBlackWhite(
            [Out, ComAliasName("BOOL")] int* pfIsBlackWhite
        )
        {
            fixed (IWICPalette* This = &this)
            {
                return MarshalFunction<_IsBlackWhite>(lpVtbl->IsBlackWhite)(
                    This,
                    pfIsBlackWhite
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int IsGrayscale(
            [Out, ComAliasName("BOOL")] int* pfIsGrayscale
        )
        {
            fixed (IWICPalette* This = &this)
            {
                return MarshalFunction<_IsGrayscale>(lpVtbl->IsGrayscale)(
                    This,
                    pfIsGrayscale
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int HasAlpha(
            [Out, ComAliasName("BOOL")] int* pfHasAlpha
        )
        {
            fixed (IWICPalette* This = &this)
            {
                return MarshalFunction<_HasAlpha>(lpVtbl->HasAlpha)(
                    This,
                    pfHasAlpha
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

            #region Fields
            public IntPtr InitializePredefined;

            public IntPtr InitializeCustom;

            public IntPtr InitializeFromBitmap;

            public IntPtr InitializeFromPalette;

            public IntPtr _GetType;

            public IntPtr GetColorCount;

            public IntPtr GetColors;

            public IntPtr IsBlackWhite;

            public IntPtr IsGrayscale;

            public IntPtr HasAlpha;
            #endregion
        }
        #endregion
    }
}

