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
using Plexdata.Utilities.Analyzers.Internals.Definitions;
using Plexdata.Utilities.Analyzers.Internals.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Plexdata.Utilities.Analyzers.Models
{
    /// <summary>
    /// A class representing a file signature setup.
    /// </summary>
    /// <remarks>
    /// This class provides all properties needed to define a file signature.
    /// </remarks>
    [DebuggerDisplay("{GetDebuggerDisplay(),nq}")]
    public class FileSignature : IFileSignature
    {
        #region Private Fields

        private String name = String.Empty;
        private String remarks = String.Empty;
        private String extensions = String.Empty;
        private Int64 offset = 0;
        private String signature = String.Empty;
        private String[] digits = new String[0];

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <remarks>
        /// The default constructor initializes a new instance of this class.
        /// </remarks>
        public FileSignature() { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets and sets the name of this file signature item.
        /// </summary>
        /// <remarks>
        /// This property allows to get and set the name of this file signature 
        /// item. An empty string is used if provided value is null.
        /// </remarks>
        /// <value>
        /// The file signature item name.
        /// </value>
        public String Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    value = String.Empty;
                }

                this.name = value;
            }
        }

        /// <summary>
        /// Gets and sets additional file signature notes, comments, etc.
        /// </summary>
        /// <remarks>
        /// This property allows to get and set additional file signature notes, 
        /// comments, etc. An empty string is used if provided value is null.
        /// </remarks>
        /// <value>
        /// The file signature item remarks.
        /// </value>
        public String Remarks
        {
            get
            {
                return this.remarks;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    value = String.Empty;
                }

                this.remarks = value;
            }
        }

        /// <summary>
        /// Gets and sets the comma separated list of associated file extensions.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property allows to get and set the comma separated list of associated 
        /// file extensions. An empty string is used if provided value is null.
        /// </para>
        /// <para>
        /// Please keep in mind, each entry is trimmed as well as prepended by a dot.
        /// </para>
        /// </remarks>
        /// <value>
        /// The file signature item extensions.
        /// </value>
        public String Extensions
        {
            get
            {
                return this.extensions;
            }
            set
            {
                this.extensions = this.NormalizeFileExtensions(value);
            }
        }

        /// <summary>
        /// Gets and sets the position (offset) where a file signature starts from.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property gets and sets the offset of a signature within a particular 
        /// file.
        /// </para>
        /// </remarks>
        /// <value>
        /// The file signature item offset.
        /// </value>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided value less than zero.
        /// </exception>
        public Int64 Offset
        {
            get
            {
                return this.offset;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException($"Provided value of \"{value}\" must not be less than zero.", nameof(this.Offset));
                }

                this.offset = value;
            }
        }

        /// <summary>
        /// Returns length (byte count) of current signature.
        /// </summary>
        /// <remarks>
        /// This property returns length (byte count) of current signature.
        /// </remarks>
        /// <value>
        /// The file signature item length.
        /// </value>
        /// <seealso cref="FileSignature.Signature"/>
        /// <seealso cref="FileSignature.Digits"/>
        public Int32 Length
        {
            get
            {
                return this.digits.Length;
            }
        }

        /// <summary>
        /// Gets and sets the signature of a particular file.
        /// </summary>
        /// <remarks>
        /// This property allows to get and set the signature of a particular 
        /// file.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided value cannot be considered as 
        /// valid file signature.
        /// </exception>
        /// <value>
        /// The file signature item's signature sequence.
        /// </value>
        /// <seealso cref="FileSignature.Digits"/>
        public String Signature
        {
            get
            {
                return this.signature;
            }
            set
            {
                String[] helper = value.SplitSignature();

                if (!helper.CheckSignature())
                {
                    throw new ArgumentException($"Provided value of \"{value}\" does not contain a valid signature.", nameof(this.Signature));
                }

                this.digits = helper;
                this.signature = helper.MergeSignature();
            }
        }

        /// <summary>
        /// Returns list of digits of current signature.
        /// </summary>
        /// <remarks>
        /// This property returns list of digits of current signature.
        /// </remarks>
        /// <value>
        /// The file signature item's signature digits.
        /// </value>
        /// <seealso cref="FileSignature.Signature"/>
        /// <seealso cref="FileSignature.Length"/>
        public String[] Digits
        {
            get
            {
                return this.digits;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Normalizes a list of file extensions.
        /// </summary>
        /// <remarks>
        /// This method normalizes a list of file extensions by splitting the 
        /// <paramref name="value"/> into its parts and reorganizing them by a 
        /// leading dot.
        /// </remarks>
        /// <param name="value">
        /// A comma separated list of file extensions.
        /// </param>
        /// <returns>
        /// A comma separated list of reorganized file extensions.
        /// </returns>
        /// <seealso cref="FileSignature.Extensions"/>
        /// <seealso cref="InternalDefinitions.ExtensionDelimiter"/>
        /// <seealso cref="InternalDefinitions.ExtensionSeparator"/>
        private String NormalizeFileExtensions(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return String.Empty;
            }

            Char delimiter = InternalDefinitions.ExtensionDelimiter;
            Char separator = InternalDefinitions.ExtensionSeparator;

            IEnumerable<String> pieces = value.Split(delimiter)
                .Where(x => !String.IsNullOrWhiteSpace(x))
                .Select(x => String.Format("{0}{1}", separator, x.Trim().TrimStart(separator)));

            return String.Join(delimiter.ToString(), pieces);
        }

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
            String signature = this.Signature;

            if (length > count)
            {
                signature = String.Format("{0}...", signature.Substring(0, (count * 3) - 1));
            }

            return String.Format(
                "{0}={1}; {2}={3}; {4}=\"{5}\"",
                nameof(this.Offset), offset,
                nameof(this.Length), length,
                nameof(this.Signature), signature);
        }

        #endregion
    }
}
