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

using Plexdata.Utilities.Analyzers.Interfaces.Models;
using Plexdata.Utilities.Analyzers.Internals.Extensions;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.Utilities.Analyzers.Internals.Models
{
    /// <summary>
    /// Represents any analysation result.
    /// </summary>
    /// <remarks>
    /// This class represents any analysation result and implements 
    /// interface <see cref="IAnalyzerResult"/>.
    /// </remarks>
    [DebuggerDisplay("{GetDebuggerDisplay(),nq}")]
    internal class AnalyzerResult : IAnalyzerResult
    {
        #region Public Fields

        /// <summary>
        /// Represents any unknown analysation result.
        /// </summary>
        /// <remarks>
        /// This field represents any unknown analysation result. For example, 
        /// an <seealso cref="Unknown"/> analysation result can be the result if 
        /// a file signature was not found within provided file signatures.
        /// </remarks>
        public static readonly IAnalyzerResult Unknown = new AnalyzerResult() { Name = nameof(AnalyzerResult.Unknown) };

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <remarks>
        /// The default constructor initializes a new instance of this class.
        /// </remarks>
        public AnalyzerResult() { }

        /// <summary>
        /// Initializes the static fields of this class
        /// </summary>
        /// <remarks>
        /// The static constructor initializes the static fields of this class.
        /// </remarks>
        static AnalyzerResult() { }

        #endregion

        #region Public Properties

        /// <inheritdoc />
        public Boolean IsUnknown
        {
            get
            {
                return Object.ReferenceEquals(this, AnalyzerResult.Unknown);
            }
        }

        /// <inheritdoc />
        public Boolean Confirmed { get; internal set; } = false;

        /// <inheritdoc />
        public String Name { get; internal set; } = String.Empty;

        /// <inheritdoc />
        public String Remarks { get; internal set; } = String.Empty;

        /// <inheritdoc />
        public String Extension { get; internal set; } = String.Empty;

        /// <inheritdoc />
        public Int64 Offset { get; internal set; } = -1;

        /// <inheritdoc />
        public Int32 Length
        {
            get
            {
                return this.Signature?.Length ?? 0;
            }
        }

        /// <inheritdoc />
        public Byte[] Signature { get; internal set; } = new Byte[0];

        /// <inheritdoc />
        public IFileSignature Match { get; internal set; } = null;

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts this instance into its string representation.
        /// </summary>
        /// <remarks>
        /// This method converts all properties of this instance into its 
        /// human readable representation.
        /// </remarks>
        /// <returns>
        /// A string representation of all properties and their values.
        /// </returns>
        [ExcludeFromCodeCoverage]
        public override String ToString()
        {
            return
                $"{nameof(this.Confirmed)}: \"{this.Confirmed}\"; " +
                $"{nameof(this.Name)}: \"{this.Name}\"; " +
                $"{nameof(this.Extension)}: \"{this.Extension}\"; " +
                $"{nameof(this.Offset)}: {this.Offset:#,##0}; " +
                $"{nameof(this.Length)}: {this.Length:#,##0}; " +
                $"{nameof(this.Signature)}: [{this.Signature.MergeSignature()}]; " +
                $"{nameof(this.Remarks)}: \"{this.Remarks}\"";
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Converts this instance into a string for debugging.
        /// </summary>
        /// <remarks>
        /// This method converts some of the properties of this instance 
        /// into a string that can be seen while debugging.
        /// </remarks>
        /// <returns>
        /// A string containing of some property values for debugging.
        /// </returns>
        [ExcludeFromCodeCoverage]
        private String GetDebuggerDisplay()
        {
            const Int32 count = 10;

            Int64 offset = this.Offset;
            Int32 length = this.Length;
            String digits = this.Signature.MergeSignature(count);
            String suffix = length > count ? "..." : String.Empty;

            return $"{this.Name}: ({offset:#,##0}, {length:#,##0}) [{digits}{suffix}]";
        }

        #endregion
    }
}
