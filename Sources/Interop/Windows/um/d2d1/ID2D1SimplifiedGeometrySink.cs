// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    /// <summary>Describes a geometric path that does not contain quadratic bezier curves or arcs.</summary>
    [Guid("2CD9069E-12E2-11DC-9FED-001143A055F9")]
    public /* blittable */ unsafe struct ID2D1SimplifiedGeometrySink
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] ID2D1SimplifiedGeometrySink* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] ID2D1SimplifiedGeometrySink* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] ID2D1SimplifiedGeometrySink* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _SetFillMode(
            [In] ID2D1SimplifiedGeometrySink* This,
            [In] D2D1_FILL_MODE fillMode
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _SetSegmentFlags(
            [In] ID2D1SimplifiedGeometrySink* This,
            [In] D2D1_PATH_SEGMENT vertexFlags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _BeginFigure(
            [In] ID2D1SimplifiedGeometrySink* This,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F startPoint,
            [In] D2D1_FIGURE_BEGIN figureBegin
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _AddLines(
            [In] ID2D1SimplifiedGeometrySink* This,
            [In, ComAliasName("D2D1_POINT_2F[]")] D2D_POINT_2F* points,
            [In, ComAliasName("UINT32")] uint pointsCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _AddBeziers(
            [In] ID2D1SimplifiedGeometrySink* This,
            [In, ComAliasName("D2D1_BEZIER_SEGMENT[]")] D2D1_BEZIER_SEGMENT* beziers,
            [In, ComAliasName("UINT32")] uint beziersCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _EndFigure(
            [In] ID2D1SimplifiedGeometrySink* This,
            [In] D2D1_FIGURE_END figureEnd
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Close(
            [In] ID2D1SimplifiedGeometrySink* This
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (ID2D1SimplifiedGeometrySink* This = &this)
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
            fixed (ID2D1SimplifiedGeometrySink* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (ID2D1SimplifiedGeometrySink* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        public void SetFillMode(
            [In] D2D1_FILL_MODE fillMode
        )
        {
            fixed (ID2D1SimplifiedGeometrySink* This = &this)
            {
                MarshalFunction<_SetFillMode>(lpVtbl->SetFillMode)(
                    This,
                    fillMode
                );
            }
        }

        public void SetSegmentFlags(
            [In] D2D1_PATH_SEGMENT vertexFlags
        )
        {
            fixed (ID2D1SimplifiedGeometrySink* This = &this)
            {
                MarshalFunction<_SetSegmentFlags>(lpVtbl->SetSegmentFlags)(
                    This,
                    vertexFlags
                );
            }
        }

        public void BeginFigure(
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F startPoint,
            [In] D2D1_FIGURE_BEGIN figureBegin
        )
        {
            fixed (ID2D1SimplifiedGeometrySink* This = &this)
            {
                MarshalFunction<_BeginFigure>(lpVtbl->BeginFigure)(
                    This,
                    startPoint,
                    figureBegin
                );
            }
        }

        public void AddLines(
            [In, ComAliasName("D2D1_POINT_2F[]")] D2D_POINT_2F* points,
            [In, ComAliasName("UINT32")] uint pointsCount
        )
        {
            fixed (ID2D1SimplifiedGeometrySink* This = &this)
            {
                MarshalFunction<_AddLines>(lpVtbl->AddLines)(
                    This,
                    points,
                    pointsCount
                );
            }
        }

        public void AddBeziers(
            [In, ComAliasName("D2D1_BEZIER_SEGMENT[]")] D2D1_BEZIER_SEGMENT* beziers,
            [In, ComAliasName("UINT32")] uint beziersCount
        )
        {
            fixed (ID2D1SimplifiedGeometrySink* This = &this)
            {
                MarshalFunction<_AddBeziers>(lpVtbl->AddBeziers)(
                    This,
                    beziers,
                    beziersCount
                );
            }
        }

        public void EndFigure(
            [In] D2D1_FIGURE_END figureEnd
        )
        {
            fixed (ID2D1SimplifiedGeometrySink* This = &this)
            {
                MarshalFunction<_EndFigure>(lpVtbl->EndFigure)(
                    This,
                    figureEnd
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Close()
        {
            fixed (ID2D1SimplifiedGeometrySink* This = &this)
            {
                return MarshalFunction<_Close>(lpVtbl->Close)(
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

            #region Fields
            public IntPtr SetFillMode;

            public IntPtr SetSegmentFlags;

            public IntPtr BeginFigure;

            public IntPtr AddLines;

            public IntPtr AddBeziers;

            public IntPtr EndFigure;

            public IntPtr Close;
            #endregion
        }
        #endregion
    }
}

