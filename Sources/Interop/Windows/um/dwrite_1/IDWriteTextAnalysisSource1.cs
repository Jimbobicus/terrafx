// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The interface implemented by the client to provide needed information to the text analyzer, such as the text and associated text properties. If any of these callbacks returns an error, the analysis functions will stop prematurely and return a callback error.</summary>
    [Guid("639CFAD8-0FB4-4B21-A58A-067920120009")]
    public /* blittable */ unsafe struct IDWriteTextAnalysisSource1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] IDWriteTextAnalysisSource1* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] IDWriteTextAnalysisSource1* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] IDWriteTextAnalysisSource1* This
        );
        #endregion

        #region IDWriteTextAnalysisSource Delegates
        /// <summary>Get a block of text starting at the specified text position. Returning NULL indicates the end of text - the position is after the last character. This function is called iteratively for each consecutive block, tying together several fragmented blocks in the backing store into a virtual contiguous string.</summary>
        /// <param name="textPosition">First position of the piece to obtain. All positions are in UTF16 code-units, not whole characters, which matters when supplementary characters are used.</param>
        /// <param name="textString">Address that receives a pointer to the text block at the specified position.</param>
        /// <param name="textLength">Number of UTF16 units of the retrieved chunk. The returned length is not the length of the block, but the length remaining in the block, from the given position until its end. So querying for a position that is 75 positions into a 100 position block would return 25.</param>
        /// <returns>Pointer to the first character at the given text position. NULL indicates no chunk available at the specified position, either because textPosition >= the entire text content length or because the queried position is not mapped into the app's backing store.</returns>
        /// <remarks> Although apps can implement sparse textual content that only maps part of the backing store, the app must map any text that is in the range passed to any analysis functions.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetTextAtPosition(
            [In] IDWriteTextAnalysisSource1* This,
            [In, ComAliasName("UINT32")] uint textPosition,
            [Out, ComAliasName("WCHAR[]")] char** textString,
            [Out, ComAliasName("UINT32")] uint* textLength
        );

        /// <summary>Get a block of text immediately preceding the specified position.</summary>
        /// <param name="textPosition">Position immediately after the last position of the chunk to obtain.</param>
        /// <param name="textString">Address that receives a pointer to the text block at the specified position.</param>
        /// <param name="textLength">Number of UTF16 units of the retrieved block. The length returned is from the given position to the front of the block.</param>
        /// <returns>Pointer to the first character at (textPosition - textLength). NULL indicates no chunk available at the specified position, either because textPosition == 0,the textPosition > the entire text content length, or the queried position is not mapped into the app's backing store.</returns>
        /// <remarks> Although apps can implement sparse textual content that only maps part of the backing store, the app must map any text that is in the range passed to any analysis functions.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetTextBeforePosition(
            [In] IDWriteTextAnalysisSource1* This,
            [In, ComAliasName("UINT32")] uint textPosition,
            [Out, ComAliasName("WCHAR[]")] char** textString,
            [Out, ComAliasName("UINT32")] uint* textLength
        );

        /// <summary>Get paragraph reading direction.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_READING_DIRECTION GetParagraphReadingDirection(
            [In] IDWriteTextAnalysisSource1* This
        );

        /// <summary>Get locale name on the range affected by it.</summary>
        /// <param name="textPosition">Position to get the locale name of.</param>
        /// <param name="textLength">Receives the length from the given position up to the next differing locale.</param>
        /// <param name="localeName">Address that receives a pointer to the locale at the specified position.</param>
        /// <remarks> The localeName pointer must remain valid until the next call or until the analysis returns.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetLocaleName(
            [In] IDWriteTextAnalysisSource1* This,
            [In, ComAliasName("UINT32")] uint textPosition,
            [Out, ComAliasName("UINT32")] uint* textLength,
            [Out, ComAliasName("WCHAR[]")] char** localeName
        );

        /// <summary>Get number substitution on the range affected by it.</summary>
        /// <param name="textPosition">Position to get the number substitution of.</param>
        /// <param name="textLength">Receives the length from the given position up to the next differing number substitution.</param>
        /// <param name="numberSubstitution">Address that receives a pointer to the number substitution at the specified position.</param>
        /// <remarks> Any implementation should return the number substitution with an incremented ref count, and the analysis will release when finished with it (either before the next call or before it returns). However, the sink callback may hold onto it after that.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetNumberSubstitution(
            [In] IDWriteTextAnalysisSource1* This,
            [In, ComAliasName("UINT32")] uint textPosition,
            [Out, ComAliasName("UINT32")] uint* textLength,
            [Out] IDWriteNumberSubstitution** numberSubstitution
        );
        #endregion

        #region Delegates
        /// <summary>The text analyzer calls back to this to get the desired glyph orientation and resolved bidi level, which it uses along with the script properties of the text to determine the actual orientation of each character, which it reports back to the client via the sink SetGlyphOrientation method.</summary>
        /// <param name="textPosition">First position of the piece to obtain. All positions are in UTF-16 code-units, not whole characters, which matters when supplementary characters are used.</param>
        /// <param name="textLength">Number of UTF-16 units of the retrieved chunk. The returned length is not the length of the block, but the length remaining in the block, from the given position until its end. So querying for a position that is 75 positions into a 100 postition block would return 25.</param>
        /// <param name="glyphOrientation">The type of glyph orientation the client wants for this range, up to the returned text length.</param>
        /// <param name="bidiLevel">The bidi level for this range up to the returned text length, which comes from an earlier bidirectional analysis.</param>
        /// <returns> Standard HRESULT error code. Returning an error will abort the analysis.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetVerticalGlyphOrientation(
            [In] IDWriteTextAnalysisSource1* This,
            [In, ComAliasName("UINT32")] uint textPosition,
            [Out, ComAliasName("UINT32")] uint* textLength,
            [Out] DWRITE_VERTICAL_GLYPH_ORIENTATION* glyphOrientation,
            [Out, ComAliasName("UINT8")] byte* bidiLevel
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

            #region IDWriteTextAnalysisSource Fields
            public IntPtr GetTextAtPosition;

            public IntPtr GetTextBeforePosition;

            public IntPtr GetParagraphReadingDirection;

            public IntPtr GetLocaleName;

            public IntPtr GetNumberSubstitution;
            #endregion

            #region Fields
            public IntPtr GetVerticalGlyphOrientation;
            #endregion
        }
        #endregion
    }
}
