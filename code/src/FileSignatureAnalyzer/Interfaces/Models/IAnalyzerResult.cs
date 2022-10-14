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

using Plexdata.Utilities.Analyzers.Internals.Models;
using System;

namespace Plexdata.Utilities.Analyzers.Interfaces.Models
{
    /// <summary>
    /// An interface representing any analysation result.
    /// </summary>
    /// <remarks>
    /// This interface represents an abstraction of the implementation 
    /// of any analysation result.
    /// </remarks>
    public interface IAnalyzerResult
    {
        /// <summary>
        /// Gets a value indicating if a file signature could be determined or not.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property represents some kind of management value to easily check 
        /// if a file signature could be determined or not.
        /// </para>
        /// </remarks>
        /// <value>
        /// True if no file signature could be determined and false otherwise.
        /// </value>
        Boolean IsUnknown { get; }

        /// <summary>
        /// Gets a value indicating whether a file signature could be confirmed.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property returns a value that indicates if a file signature could be 
        /// confirmed.
        /// </para>
        /// <para>
        /// A file signature is considered as confirmed as soon as its binary signature 
        /// could be found within a file at expected position and that file's extension 
        /// is part of the list of file extensions that comes from source extensions.
        /// </para>
        /// </remarks>
        /// <value>
        /// True if a file signature could be confirmed and false otherwise.
        /// </value>
        Boolean Confirmed { get; }

        /// <summary>
        /// Gets the name of matching file signature.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property returns the name of matching file signature or 
        /// <see cref="AnalyzerResult.Unknown"/> if no file signature could be determined. 
        /// This property is optional and might be empty.
        /// </para>
        /// </remarks>
        /// <value>
        /// The name of matching file signature or <see cref="AnalyzerResult.Unknown"/>. 
        /// </value>
        String Name { get; }

        /// <summary>
        /// Gets the remarks of matching file signature.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property returns the remarks of matching file signature. This property 
        /// is optional and might be empty.
        /// </para>
        /// </remarks>
        /// <value>
        /// The remarks of matching file signature.
        /// </value>
        String Remarks { get; }

        /// <summary>
        /// Gets the file name extension of matching file signature.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property returns the file name extension of matching file signature. 
        /// The actual value come from underlying file stream and might be empty.
        /// </para>
        /// </remarks>
        /// <value>
        /// The file name extension of matching file signature.
        /// </value>
        String Extension { get; }

        /// <summary>
        /// Gets the offset within the file where the signature was found.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property returns the real offset within the file stream where the 
        /// signature was found. This property is mandatory. But the actual used offset 
        /// might vary, especially in case of an existing <i>Byte Order Mark</i> (BOM).
        /// </para>
        /// </remarks>
        /// <value>
        /// The offset of the signature within the file stream. 
        /// </value>
        Int64 Offset { get; }

        /// <summary>
        /// Gets the length of found signature.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property returns the length of found signature.
        /// </para>
        /// </remarks>
        /// <value>
        /// The length of found signature.
        /// </value>
        Int32 Length { get; }

        /// <summary>
        /// Gets the bytes of found signature.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property returns the actual bytes of found signature. This is 
        /// especially interesting in case of a signature match took place by using 
        /// placeholders.
        /// </para>
        /// </remarks>
        /// <value>
        /// The bytes of found signature.
        /// </value>
        Byte[] Signature { get; }

        /// <summary>
        /// Gets the matching file signature instance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property returns the matching file signature instance, which is one 
        /// of the source signatures.
        /// </para>
        /// </remarks>
        /// <value>
        /// The matching file signature instance.
        /// </value>
        IFileSignature Match { get; }
    }
}
