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
using Plexdata.Utilities.Analyzers.Internals.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Plexdata.Utilities.Analyzers.Interfaces
{
    /// <summary>
    /// An interface to perform file signature analysis.
    /// </summary>
    /// <remarks>
    /// This interface performs all steps necessary to analyze file signatures.
    /// </remarks>
    public interface IFileSignatureAnalyzer
    {
        /// <summary>
        /// Opens file represented by '<paramref name="path"/>' for read and runs analysation.
        /// </summary>
        /// <remarks>
        /// This method opens the file represented by '<paramref name="path"/>' for read access 
        /// and runs the analysation of file signatures.
        /// </remarks>
        /// <param name="path">
        /// The path of file to by analyzed.
        /// </param>
        /// <param name="signatures">
        /// A list of file signatures to be validated.
        /// </param>
        /// <returns>
        /// A list of matching analyze results. 
        /// </returns>
        /// <exception cref="Exception">
        /// Each exception that is thrown by <see cref="File.Open(String, FileMode, FileAccess, FileShare)"/>.
        /// </exception>
        /// <seealso cref="Analyze(Stream, IEnumerable{IFileSignature})"/>
        IEnumerable<IAnalyzerResult> Analyze(String path, IEnumerable<IFileSignature> signatures);

        /// <summary>
        /// Runs file signature analysis on provide <paramref name="stream"/>.
        /// </summary>
        /// <remarks>
        /// This method runs the file signature analysis on provide <paramref name="stream"/>. 
        /// This stream must have read as well as seek access.
        /// </remarks>
        /// <param name="stream">
        /// The stream (usually a file stream) to be analyzed.
        /// </param>
        /// <param name="signatures">
        /// A list of file signatures to be validated.
        /// </param>
        /// <returns>
        /// A list of matching analyze results. In case of none of the signatures matches the 
        /// result contains one <see cref="AnalyzerResult.Unknown"/> entry.
        /// </returns>
        /// <exception cref="Exception">
        /// Each exception that can come from <see cref="StreamExtension.Validate(Stream)"/> 
        /// or from <see cref="SignatureExtension.Validate(IEnumerable{IFileSignature})"/>.
        /// </exception>
        /// <seealso cref="Analyze(String, IEnumerable{IFileSignature})"/>
        /// <seealso cref="StreamExtension.Validate(Stream)"/>
        /// <seealso cref="SignatureExtension.Validate(IEnumerable{IFileSignature})"/>
        IEnumerable<IAnalyzerResult> Analyze(Stream stream, IEnumerable<IFileSignature> signatures);
    }
}
