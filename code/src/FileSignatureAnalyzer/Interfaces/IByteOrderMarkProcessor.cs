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
using System.IO;

namespace Plexdata.Utilities.Analyzers.Interfaces
{
    /// <summary>
    /// An interface representing a processor to handle <i>Byte Order 
    /// Mark</i> (BOM).
    /// </summary>
    /// <remarks>
    /// This interface represents an abstraction of the implementation 
    /// a processor that handles <i>Byte Order Mark</i> (BOM).
    /// </remarks>
    public interface IByteOrderMarkProcessor
    {
        /// <summary>
        /// Checks if <paramref name="stream"/> has one of the <i>Byte Order 
        /// Marks</i> and if so it moves its position behind BOM.
        /// </summary>
        /// <remarks>
        /// This method checks if provided <paramref name="stream"/> has 
        /// one of these <i>Byte Order Marks</i> "UTF-8", "UTF-32 LE", "UTF-32 BE", 
        /// "UTF-16 LE" or "UTF-16 BE" and if so it moves the stream position 
        /// behind that BOM.
        /// </remarks>
        /// <param name="stream">
        /// The stream to be checked.
        /// </param>
        /// <returns>
        /// The provided stream, possibly with a new position.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown in case of the <paramref name="stream"/> 
        /// is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This exception is thrown in case of either the stream has no read 
        /// access or no seek access.
        /// </exception>
        Stream Process(Stream stream);
    }
}
