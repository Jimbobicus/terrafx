// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("00020402-0000-0000-C000-000000000046")]
    public /* blittable */ unsafe struct ITypeLib
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] ITypeLib* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] ITypeLib* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] ITypeLib* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate uint GetTypeInfoCount(
            [In] ITypeLib* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetTypeInfo(
            [In] ITypeLib* This,
            [In, ComAliasName("UINT")] uint index,
            [Out] ITypeInfo** ppTInfo = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetTypeInfoType(
            [In] ITypeLib* This,
            [In, ComAliasName("UINT")] uint index,
            [Out] TYPEKIND* pTKind
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetTypeInfoOfGuid(
            [In] ITypeLib* This,
            [In, ComAliasName("REFGUID")] Guid* guid,
            [Out] ITypeInfo** ppTinfo = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetLibAttr(
            [In] ITypeLib* This,
            [Out] TLIBATTR** ppTLibAttr
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetTypeComp(
            [In] ITypeLib* This,
            [Out] ITypeComp** ppTComp = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDocumentation(
            [In] ITypeLib* This,
            [In, ComAliasName("INT")] int index,
            [Out, Optional, ComAliasName("BSTR")] char** pBstrName,
            [Out, Optional, ComAliasName("BSTR")] char** pBstrDocString,
            [Out, ComAliasName("DWORD")] uint* pdwHelpContext,
            [Out, ComAliasName("BSTR")] char** pBstrHelpFile = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int IsName(
            [In] ITypeLib* This,
            [In, Out, ComAliasName("LPOLESTR")] char* szNameBuf,
            [In, ComAliasName("ULONG")] uint lHashVal,
            [Out, ComAliasName("BOOL")] int* pfName
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int FindName(
            [In] ITypeLib* This,
            [In, Out, ComAliasName("LPOLESTR")] char* szNameBuf,
            [In, ComAliasName("ULONG")] uint lHashVal,
            [Out] ITypeInfo** ppTInfo,
            [Out, ComAliasName("MEMBERID")] int* rgMemId,
            [In, Out, ComAliasName("USHORT")] ushort* pcFound
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ReleaseTLibAttr(
            [In] ITypeLib* This,
            [In] TLIBATTR* pTLibAttr
        );
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
            public IntPtr GetTypeInfoCount;

            public IntPtr GetTypeInfo;

            public IntPtr GetTypeInfoType;

            public IntPtr GetTypeInfoOfGuid;

            public IntPtr GetLibAttr;

            public IntPtr GetTypeComp;

            public IntPtr GetDocumentation;

            public IntPtr IsName;

            public IntPtr FindName;

            public IntPtr ReleaseTLibAttr;
            #endregion
        }
        #endregion
    }
}
