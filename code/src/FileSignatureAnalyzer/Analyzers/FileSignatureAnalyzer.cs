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

using Plexdata.Utilities.Analyzers.Interfaces;
using Plexdata.Utilities.Analyzers.Interfaces.Models;
using Plexdata.Utilities.Analyzers.Internals.Extensions;
using Plexdata.Utilities.Analyzers.Internals.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Plexdata.Utilities.Analyzers
{
    /// <summary>
    /// A class to perform file signature analysis.
    /// </summary>
    /// <remarks>
    /// This class implements all steps necessary to perform a file signature analysis.
    /// </remarks>
    public class FileSignatureAnalyzer : IFileSignatureAnalyzer
    {
        #region Private Fields

        /// <summary>
        /// The <i>Byte Order Mark</i> (BOM) processor.
        /// </summary>
        /// <remarks>
        /// This field holds an instance of a <i>Byte Order Mark</i> (BOM) processor to by used.
        /// </remarks>
        private readonly IByteOrderMarkProcessor processor;

        #endregion

        #region Construction

        /// <summary>
        /// The only class constructor.
        /// </summary>
        /// <remarks>
        /// The class provides this constructor only.
        /// </remarks>
        /// <param name="processor">
        /// An instance of <see cref="IByteOrderMarkProcessor"/> to be used to process 
        /// possibly existing <i>Byte Order Mark</i> (BOM) bytes.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown in case of parameter '<paramref name="processor"/>'
        /// is null.
        /// </exception>
        public FileSignatureAnalyzer(IByteOrderMarkProcessor processor)
        {
            this.processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        #endregion

        #region Public Methods

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
        public IEnumerable<IAnalyzerResult> Analyze(String path, IEnumerable<IFileSignature> signatures)
        {
            using (FileStream file = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return this.Analyze(file, signatures);
            }
        }

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
        public IEnumerable<IAnalyzerResult> Analyze(Stream stream, IEnumerable<IFileSignature> signatures)
        {
            List<IAnalyzerResult> results = new List<IAnalyzerResult>();

            stream.Validate();
            signatures.Validate();

            stream.Position = 0;

            this.processor.Process(stream);

            Int64 position = stream.Position; // Save current stream offset after BOM check!

            foreach (IFileSignature signature in signatures.SortByLength())
            {
                // In case of BOM, value of 'position' is greater than zero and has to be skipped!
                Int64 offset = position + signature.Offset;
                Int32 length = signature.Length;

                if (!stream.TryReadBuffer(offset, length, out Byte[] buffer))
                {
                    continue;
                }

                if (signature.IsEqual(buffer))
                {
                    String extension = stream.GetFileExtension();

                    results.Add(new AnalyzerResult()
                    {
                        Confirmed = signature.GetConfirmed(extension),
                        Name = signature.Name,
                        Remarks = signature.Remarks,
                        Extension = extension,
                        Offset = offset,
                        Signature = buffer,
                        Match = signature
                    });
                }
            }

            if (results.Count > 0)
            {
                return results;
            }

            return Enumerable.Repeat(AnalyzerResult.Unknown, 1);
        }

        #endregion
    }
}
