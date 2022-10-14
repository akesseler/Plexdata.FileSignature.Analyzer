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

using Plexdata.Utilities.Analyzers.Internals.Definitions;
using System;
using System.Diagnostics;
using System.IO;

namespace Plexdata.Utilities.Analyzers.Internals.Extensions
{
    /// <summary>
    /// A collection of extension methods related to managing file 
    /// streams.
    /// </summary>
    /// <remarks>
    /// This class contains a collection of extension methods related 
    /// to managing file streams.
    /// </remarks>
    internal static class StreamExtension
    {
        #region Public Methods

        /// <summary>
        /// Validates the stream for read and seek access.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This extension method validates parameter <paramref name="stream"/> to be not 
        /// null as well as for read and seek access.
        /// </para>
        /// </remarks>
        /// <param name="stream">
        /// The stream to validate.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown in case of the <paramref name="stream"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This exception is thrown in case of either the stream has no read access or 
        /// no seek access.
        /// </exception>
        public static void Validate(this Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream), $"The '{nameof(stream)}' must not be null.");
            }

            if (!stream.CanRead)
            {
                throw new InvalidOperationException($"The '{nameof(stream)}' must have read access.");
            }

            if (!stream.CanSeek)
            {
                throw new InvalidOperationException($"The '{nameof(stream)}' must have seek access.");
            }
        }

        /// <summary>
        /// Tries reading a number of bytes at given position.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This extension method tries to read a number of bytes at given position.
        /// </para>
        /// <para>
        /// Note that the caller is responsible for managing the stream positions themselves. 
        /// Furthermore, the caller is responsible to ensure that the stream is valid as 
        /// well as that reading and seeking the stream is possible.
        /// </para>
        /// </remarks>
        /// <param name="stream">
        /// The stream to read from.
        /// </param>
        /// <param name="offset">
        /// The position within the stream to start reading from.
        /// </param>
        /// <param name="length">
        /// The length of the buffer to read from the stream.
        /// </param>
        /// <param name="result">
        /// The buffer with read bytes or null if it fails.
        /// </param>
        /// <returns>
        /// True if reading the number of bytes of specified length at specified offset 
        /// was successful and false otherwise.
        /// </returns>
        /// <seealso cref="StreamExtension.Validate(Stream)"/>
        public static Boolean TryReadBuffer(this Stream stream, Int64 offset, Int32 length, out Byte[] result)
        {
            result = null;

            try
            {
                if (offset < 0)
                {
                    Debug.WriteLine($"Provided stream offset {offset:#,##0} is less than zero.");
                    return false;
                }

                if (offset + length > stream.Length)
                {
                    Debug.WriteLine($"Length to read (offset: {offset:#,##0}, length: {length:#,##0}) exceeds actual stream length of {stream.Length:#,##0}.");
                    return false;
                }

                result = new Byte[length];

                stream.Position = offset;
                stream.Read(result, 0, length);

                return true;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
                return false;
            }
        }

        /// <summary>
        /// Returns the file extension of provided <paramref name="stream"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This extension method returns the file extension of provided stream but only 
        /// if underlying stream is actually an instance of a <see cref="FileStream"/>.
        /// </para>
        /// <para>
        /// Furthermore, everything starting at first dot is treated as file extension. 
        /// This is actually in contrast to default extension handling, which only treats 
        /// everything behind last dot as extension.
        /// </para>
        /// </remarks>
        /// <param name="stream">
        /// The stream to get its file extension from.
        /// </param>
        /// <returns>
        /// The extension of underlying file stream or an empty string.
        /// </returns>
        public static String GetFileExtension(this Stream stream)
        {
            if (stream is FileStream fs && !String.IsNullOrWhiteSpace(fs.Name))
            {
                try
                {
                    String file = Path.GetFileName(fs.Name);

                    Int32 index = 0;

                    if ((index = file.IndexOf(InternalDefinitions.ExtensionSeparator)) < 0)
                    {
                        return String.Empty;
                    }

                    return file.Substring(index, file.Length - index);
                }
                catch
                {
                }
            }

            return String.Empty;
        }

        #endregion
    }
}
