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
using Plexdata.Utilities.Analyzers.Internals.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Plexdata.Utilities.Analyzers.Processors
{
    /// <summary>
    /// A class representing a processor to handle <i>Byte Order Mark</i> (BOM).
    /// </summary>
    /// <remarks>
    /// <para>
    /// This processor checks for an existence of BOM bytes and if found 
    /// it moves current stream position behind them.
    /// </para>
    /// <list type="table">
    /// <listheader><term>Encoding</term><description>BOM Bytes</description></listheader>
    /// <item><term>UTF-8</term><description><c>0xEF, 0xBB, 0xBF</c></description></item>
    /// <item><term>UTF-32 LE</term><description><c>0xFF, 0xFE, 0x00, 0x00</c></description></item>
    /// <item><term>UTF-32 BE</term><description><c>0x00, 0x00, 0xFE, 0xFF</c></description></item>
    /// <item><term>UTF-16 LE</term><description><c>0xFF, 0xFE</c></description></item>
    /// <item><term>UTF-16 BE</term><description><c>0xFE, 0xFF</c></description></item>
    /// </list>
    /// </remarks>
    public class ByteOrderMarkProcessor : IByteOrderMarkProcessor
    {
        #region Private Fields

        /// <summary>
        /// A field keeping all supported BOM preambles.
        /// </summary>
        /// <remarks>
        /// This field holds a list of supported BOM preambles.
        /// </remarks>
        private static readonly List<(String, Byte[])> preambles = null;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <remarks>
        /// The default constructor initializes a new instance of this class.
        /// </remarks>
        public ByteOrderMarkProcessor() { }

        /// <summary>
        /// The static class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor just initializes all static class values.
        /// </remarks>
        static ByteOrderMarkProcessor()
        {
            // Keep this order!
            ByteOrderMarkProcessor.preambles = new List<(String, Byte[])>()
            {
                ( "UTF-8",     new Byte[] { 0xEF, 0xBB, 0xBF       } ),
                ( "UTF-32 LE", new Byte[] { 0xFF, 0xFE, 0x00, 0x00 } ),
                ( "UTF-32 BE", new Byte[] { 0x00, 0x00, 0xFE, 0xFF } ),
                ( "UTF-16 LE", new Byte[] { 0xFF, 0xFE             } ),
                ( "UTF-16 BE", new Byte[] { 0xFE, 0xFF             } ),
            };
        }

        #endregion

        #region Public Methods

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
        /// <seealso cref="ByteOrderMarkProcessor.HasPreamble(Stream, Byte[])"/>
        public Stream Process(Stream stream)
        {
            stream.Validate();

            foreach ((String name, Byte[] data) in ByteOrderMarkProcessor.preambles)
            {
                if (this.HasPreamble(stream, data))
                {
                    Debug.WriteLine($"Found BOM of type '{name}'");
                    break;
                }
            }

            return stream;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Checks whether passed <paramref name="stream"/> starts with passed 
        /// <paramref name="bytes"/>.
        /// </summary>
        /// <remarks>
        /// This method checks whether passed <paramref name="stream"/> starts with 
        /// passed <paramref name="bytes"/> and if found it moves current stream 
        /// position behind of them.
        /// </remarks>
        /// <param name="stream">
        /// The stream to be checked.
        /// </param>
        /// <param name="bytes">
        /// An array of bytes to be checked.
        /// </param>
        /// <returns>
        /// <para>
        /// True if <paramref name="stream"/> starts with the sequence of passed 
        /// <paramref name="bytes"/>. Pay attention, in this case current <paramref name="stream"/> 
        /// position points behind that sequence of <paramref name="bytes"/>. 
        /// </para>
        /// <para>
        /// False is returned if the <paramref name="stream"/> does not start with 
        /// that sequence of <paramref name="bytes"/>. In that case current <paramref name="stream"/> 
        /// position is reset to its start.
        /// </para>
        /// </returns>
        /// <seealso cref="ByteOrderMarkProcessor.IsEqual(Byte[], Byte[])"/>
        private Boolean HasPreamble(Stream stream, Byte[] bytes)
        {
            stream.Position = 0;

            if (!stream.TryReadBuffer(0, bytes.Length, out Byte[] temp))
            {
                return false;
            }

            if (this.IsEqual(bytes, temp))
            {
                stream.Position = bytes.Length;

                return true;
            }

            stream.Position = 0;

            return false;
        }

        /// <summary>
        /// Compares two byte arrays and tests for equality.
        /// </summary>
        /// <remarks>
        /// This method compares two byte arrays and tests for equality.
        /// </remarks>
        /// <param name="x">
        /// The first byte array to compare.
        /// </param>
        /// <param name="y">
        /// The second byte array to compare.
        /// </param>
        /// <returns>
        /// True if both byte arrays are equal and false otherwise.
        /// </returns>
        private Boolean IsEqual(Byte[] x, Byte[] y)
        {
            Debug.Assert(x.Length == y.Length, "Both arrays should never have a different length because of calling method TryReadBuffer().");

            Int32 length = x.Length;

            for (Int32 index = 0; index < length; index++)
            {
                if (x[index] != y[index])
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
