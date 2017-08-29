// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("DC2BB46D-3F07-481E-8625-220C4AEDBB33")]
    public /* blittable */ unsafe struct IWICEnumMetadataItem
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IWICEnumMetadataItem* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IWICEnumMetadataItem* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IWICEnumMetadataItem* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Next(
            [In] IWICEnumMetadataItem* This,
            [In, ComAliasName("ULONG")] uint celt,
            [In, Out, Optional] PROPVARIANT* rgeltSchema,
            [In, Out] PROPVARIANT* rgeltId,
            [In, Out] PROPVARIANT* rgeltValue = null,
            [Out, ComAliasName("ULONG")] uint* pceltFetched = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Skip(
            [In] IWICEnumMetadataItem* This,
            [In, ComAliasName("ULONG")] uint celt
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Reset(
            [In] IWICEnumMetadataItem* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Clone(
            [In] IWICEnumMetadataItem* This,
            [Out] IWICEnumMetadataItem** ppIEnumMetadataItem = null
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IWICEnumMetadataItem* This = &this)
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
            fixed (IWICEnumMetadataItem* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IWICEnumMetadataItem* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int Next(
            [In, ComAliasName("ULONG")] uint celt,
            [In, Out, Optional] PROPVARIANT* rgeltSchema,
            [In, Out] PROPVARIANT* rgeltId,
            [In, Out] PROPVARIANT* rgeltValue = null,
            [Out, ComAliasName("ULONG")] uint* pceltFetched = null
        )
        {
            fixed (IWICEnumMetadataItem* This = &this)
            {
                return MarshalFunction<_Next>(lpVtbl->Next)(
                    This,
                    celt,
                    rgeltSchema,
                    rgeltId,
                    rgeltValue,
                    pceltFetched
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Skip(
            [In, ComAliasName("ULONG")] uint celt
        )
        {
            fixed (IWICEnumMetadataItem* This = &this)
            {
                return MarshalFunction<_Skip>(lpVtbl->Skip)(
                    This,
                    celt
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Reset()
        {
            fixed (IWICEnumMetadataItem* This = &this)
            {
                return MarshalFunction<_Reset>(lpVtbl->Reset)(
                    This
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Clone(
            [Out] IWICEnumMetadataItem** ppIEnumMetadataItem = null
        )
        {
            fixed (IWICEnumMetadataItem* This = &this)
            {
                return MarshalFunction<_Clone>(lpVtbl->Clone)(
                    This,
                    ppIEnumMetadataItem
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
            public IntPtr Next;

            public IntPtr Skip;

            public IntPtr Reset;

            public IntPtr Clone;
            #endregion
        }
        #endregion
    }
}

