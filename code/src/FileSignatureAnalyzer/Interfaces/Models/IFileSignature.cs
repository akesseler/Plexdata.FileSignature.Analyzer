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

namespace Plexdata.Utilities.Analyzers.Interfaces.Models
{
    /// <summary>
    /// An interface representing a file signature setup.
    /// </summary>
    /// <remarks>
    /// This interface represents an abstraction of the implementation 
    /// of each file signature setup.
    /// </remarks>
    public interface IFileSignature
    {
        /// <summary>
        /// Gets the name of this file signature item.
        /// </summary>
        /// <remarks>
        /// This property allows to get the name of this file signature 
        /// item. An empty string is used if provided value is null.
        /// </remarks>
        /// <value>
        /// The file signature item name.
        /// </value>
        String Name { get; }

        /// <summary>
        /// Gets additional file signature notes, comments, etc.
        /// </summary>
        /// <remarks>
        /// This property allows to get additional file signature notes, 
        /// comments, etc. An empty string is used if provided value is null.
        /// </remarks>
        /// <value>
        /// The file signature item remarks.
        /// </value>
        String Remarks { get; }

        /// <summary>
        /// Gets the comma separated list of associated file extensions.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property allows to get the comma separated list of associated 
        /// file extensions. An empty string is used if provided value is null.
        /// </para>
        /// <para>
        /// Please keep in mind, each entry is trimmed as well as prepended by 
        /// a dot.
        /// </para>
        /// </remarks>
        /// <value>
        /// The file signature item extensions.
        /// </value>
        String Extensions { get; }

        /// <summary>
        /// Gets the position (offset) where a file signature starts from.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property gets the offset of a signature within a particular 
        /// file.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided value less than zero.
        /// </exception>
        /// <value>
        /// The file signature item offset.
        /// </value>
        Int64 Offset { get; }

        /// <summary>
        /// Returns length (byte count) of current signature.
        /// </summary>
        /// <remarks>
        /// This property returns length (byte count) of current signature.
        /// </remarks>
        /// <value>
        /// The file signature item length.
        /// </value>
        /// <seealso cref="IFileSignature.Signature"/>
        /// <seealso cref="IFileSignature.Digits"/>
        Int32 Length { get; }

        /// <summary>
        /// Gets the signature of a particular file.
        /// </summary>
        /// <remarks>
        /// This property allows to get the signature of a particular 
        /// file.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if provided value cannot be considered as 
        /// valid file signature.
        /// </exception>
        /// <value>
        /// The file signature item's signature sequence.
        /// </value>
        /// <seealso cref="IFileSignature.Digits"/>
        String Signature { get; }

        /// <summary>
        /// Returns list of digits of current signature.
        /// </summary>
        /// <remarks>
        /// This property returns list of digits of current signature.
        /// </remarks>
        /// <value>
        /// The file signature item's signature digits.
        /// </value>
        /// <seealso cref="IFileSignature.Signature"/>
        /// <seealso cref="IFileSignature.Length"/>
        String[] Digits { get; }
    }
}
