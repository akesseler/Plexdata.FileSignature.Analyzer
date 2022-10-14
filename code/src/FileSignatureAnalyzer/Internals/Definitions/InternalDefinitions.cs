/*
 * MIT License
 * 
 * Copyright (c) 2022 plexdata.de
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;

namespace Plexdata.Utilities.Analyzers.Internals.Definitions
{
    /// <summary>
    /// Provides a set of constant values.
    /// </summary>
    /// <remarks>
    /// This class provides a set of constant values that 
    /// are used internally.
    /// </remarks>
    internal static class InternalDefinitions
    {
        #region Public Fields

        /// <summary>
        /// Represents a placeholder.
        /// </summary>
        /// <remarks>
        /// The question mark ('<c>?</c>') is used as placeholder character.
        /// </remarks>
        public static readonly Char SignaturePlaceholder = '?';

        /// <summary>
        /// Character list used to separate the parts of a signature.
        /// </summary>
        /// <remarks>
        /// A signature is passed as a string and must be separated into its individual 
        /// components. This separation can be done either by space ('<c> </c>') or by 
        /// dash ('<c>-</c>') or by comma ('<c>,</c>') or by semicolon ('<c>;</c>').
        /// </remarks>
        public static readonly Char[] SignatureDelimiters = new Char[] { ' ', '-', ',', ';' };

        /// <summary>
        /// Character used to reorganize the parts of a signature.
        /// </summary>
        /// <remarks>
        /// Each passed signature string must be reorganized. For this purpose a space 
        /// ('<c> </c>') is used as internal delimiter.
        /// </remarks>
        public static readonly Char SignatureSeparator = InternalDefinitions.SignatureDelimiters[0];

        /// <summary>
        /// Character used to split file extensions.
        /// </summary>
        /// <remarks>
        /// File extensions are passed as string and must be split into their individual parts.
        /// For this purpose a comma ('<c>,</c>') is used.
        /// </remarks>
        public static readonly Char ExtensionDelimiter = ',';

        /// <summary>
        /// File extension separator.
        /// </summary>
        /// <remarks>
        /// The file extension separator separates the file name from its extension which is 
        /// usually a dot ('<c>.</c>').
        /// </remarks>
        public static readonly Char ExtensionSeparator = '.';

        #endregion

        #region Construction

        /// <summary>
        /// Initializes the static fields of this class
        /// </summary>
        /// <remarks>
        /// The static constructor initializes the static fields of this class.
        /// </remarks>
        static InternalDefinitions() { }

        #endregion
    }
}
