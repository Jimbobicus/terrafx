// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    /// <summary>Represents an collection of gradient stops that can then be the source resource for either a linear or radial gradient brush.</summary>
    [Guid("AE1572F4-5DD0-4777-998B-9279472AE63B")]
    public /* blittable */ unsafe struct ID2D1GradientStopCollection1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] ID2D1GradientStopCollection1* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] ID2D1GradientStopCollection1* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] ID2D1GradientStopCollection1* This
        );
        #endregion

        #region ID2D1Resource Delegates
        /// <summary>Retrieve the factory associated with this resource.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetFactory(
            [In] ID2D1GradientStopCollection1* This,
            [Out] ID2D1Factory** factory
        );
        #endregion

        #region ID2D1GradientStopCollection Delegates
        /// <summary>Returns the number of stops in the gradient.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint _GetGradientStopCount(
            [In] ID2D1GradientStopCollection1* This
        );

        /// <summary>Copies the gradient stops from the collection into the caller's interface.  The
        /// returned colors have straight alpha.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetGradientStops(
            [In] ID2D1GradientStopCollection1* This,
            [Out, ComAliasName("D2D1_GRADIENT_STOP[]")] D2D1_GRADIENT_STOP* gradientStops,
            [In, ComAliasName("UINT32")] uint gradientStopsCount
        );

        /// <summary>Returns whether the interpolation occurs with 1.0 or 2.2 gamma.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_GAMMA _GetColorInterpolationGamma(
            [In] ID2D1GradientStopCollection1* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_EXTEND_MODE _GetExtendMode(
            [In] ID2D1GradientStopCollection1* This
        );
        #endregion

        #region Delegates
        /// <summary>Copies the gradient stops from the collection into the caller's memory. If this object was created using ID2D1DeviceContext::CreateGradientStopCollection, this method returns the same values as were specified in the creation method. If this object was created using ID2D1RenderTarget::CreateGradientStopCollection, the stops returned here will first be transformed into the gamma space specified by the colorInterpolationGamma parameter.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetGradientStops1(
            [In] ID2D1GradientStopCollection1* This,
            [Out, ComAliasName("D2D1_GRADIENT_STOP[]")] D2D1_GRADIENT_STOP* gradientStops,
            [In, ComAliasName("UINT32")] uint gradientStopsCount
        );

        /// <summary>Returns the color space in which interpolation occurs. If this object was created using ID2D1RenderTarget::CreateGradientStopCollection, this method returns the color space related to the color interpolation gamma.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_COLOR_SPACE _GetPreInterpolationSpace(
            [In] ID2D1GradientStopCollection1* This
        );

        /// <summary>Returns the color space colors will be converted to after interpolation occurs. If this object was created using ID2D1RenderTarget::CreateGradientStopCollection, this method returns D2D1_COLOR_SPACE_SRGB.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_COLOR_SPACE _GetPostInterpolationSpace(
            [In] ID2D1GradientStopCollection1* This
        );

        /// <summary>Returns the buffer precision of this gradient. If this object was created using ID2D1RenderTarget::CreateGradientStopCollection, this method returns D2D1_BUFFER_PRECISION_8BPC_UNORM.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_BUFFER_PRECISION _GetBufferPrecision(
            [In] ID2D1GradientStopCollection1* This
        );

        /// <summary>Returns the interpolation mode used to interpolate colors in the gradient.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_COLOR_INTERPOLATION_MODE _GetColorInterpolationMode(
            [In] ID2D1GradientStopCollection1* This
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (ID2D1GradientStopCollection1* This = &this)
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
            fixed (ID2D1GradientStopCollection1* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (ID2D1GradientStopCollection1* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region ID2D1Resource Methods
        public void GetFactory(
            [Out] ID2D1Factory** factory
        )
        {
            fixed (ID2D1GradientStopCollection1* This = &this)
            {
                MarshalFunction<_GetFactory>(lpVtbl->GetFactory)(
                    This,
                    factory
                );
            }
        }
        #endregion

        #region ID2D1GradientStopCollection Methods
        [return: ComAliasName("UINT32")]
        public uint GetGradientStopCount()
        {
            fixed (ID2D1GradientStopCollection1* This = &this)
            {
                return MarshalFunction<_GetGradientStopCount>(lpVtbl->GetGradientStopCount)(
                    This
                );
            }
        }

        public void GetGradientStops(
            [Out, ComAliasName("D2D1_GRADIENT_STOP[]")] D2D1_GRADIENT_STOP* gradientStops,
            [In, ComAliasName("UINT32")] uint gradientStopsCount
        )
        {
            fixed (ID2D1GradientStopCollection1* This = &this)
            {
                MarshalFunction<_GetGradientStops>(lpVtbl->GetGradientStops)(
                    This,
                    gradientStops,
                    gradientStopsCount
                );
            }
        }

        public D2D1_GAMMA GetColorInterpolationGamma()
        {
            fixed (ID2D1GradientStopCollection1* This = &this)
            {
                return MarshalFunction<_GetColorInterpolationGamma>(lpVtbl->GetColorInterpolationGamma)(
                    This
                );
            }
        }

        public D2D1_EXTEND_MODE GetExtendMode()
        {
            fixed (ID2D1GradientStopCollection1* This = &this)
            {
                return MarshalFunction<_GetExtendMode>(lpVtbl->GetExtendMode)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        public void GetGradientStops1(
            [Out, ComAliasName("D2D1_GRADIENT_STOP[]")] D2D1_GRADIENT_STOP* gradientStops,
            [In, ComAliasName("UINT32")] uint gradientStopsCount
        )
        {
            fixed (ID2D1GradientStopCollection1* This = &this)
            {
                MarshalFunction<_GetGradientStops1>(lpVtbl->GetGradientStops1)(
                    This,
                    gradientStops,
                    gradientStopsCount
                );
            }
        }

        public D2D1_COLOR_SPACE GetPreInterpolationSpace()
        {
            fixed (ID2D1GradientStopCollection1* This = &this)
            {
                return MarshalFunction<_GetPreInterpolationSpace>(lpVtbl->GetPreInterpolationSpace)(
                    This
                );
            }
        }

        public D2D1_COLOR_SPACE GetPostInterpolationSpace()
        {
            fixed (ID2D1GradientStopCollection1* This = &this)
            {
                return MarshalFunction<_GetPostInterpolationSpace>(lpVtbl->GetPostInterpolationSpace)(
                    This
                );
            }
        }

        public D2D1_BUFFER_PRECISION GetBufferPrecision()
        {
            fixed (ID2D1GradientStopCollection1* This = &this)
            {
                return MarshalFunction<_GetBufferPrecision>(lpVtbl->GetBufferPrecision)(
                    This
                );
            }
        }

        public D2D1_COLOR_INTERPOLATION_MODE GetColorInterpolationMode()
        {
            fixed (ID2D1GradientStopCollection1* This = &this)
            {
                return MarshalFunction<_GetColorInterpolationMode>(lpVtbl->GetColorInterpolationMode)(
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

            #region ID2D1Resource Fields
            public IntPtr GetFactory;
            #endregion

            #region ID2D1GradientStopCollection Fields
            public IntPtr GetGradientStopCount;

            public IntPtr GetGradientStops;

            public IntPtr GetColorInterpolationGamma;

            public IntPtr GetExtendMode;
            #endregion

            #region Fields
            public IntPtr GetGradientStops1;

            public IntPtr GetPreInterpolationSpace;

            public IntPtr GetPostInterpolationSpace;

            public IntPtr GetBufferPrecision;

            public IntPtr GetColorInterpolationMode;
            #endregion
        }
        #endregion
    }
}

