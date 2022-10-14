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
using System;
using System.Collections.Generic;

namespace Plexdata.Utilities.Analyzers.Internals.Comparers
{
    /// <summary>
    /// A class representing a special file signature comparer.
    /// </summary>
    /// <remarks>
    /// This class represents a special file signature comparer 
    /// that only comares file signatures by their length.
    /// </remarks>
    internal class FileSignatureLengthComparer : IComparer<IFileSignature>
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <remarks>
        /// The default constructor initializes a new instance of this class.
        /// </remarks>
        public FileSignatureLengthComparer() { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Compares the length of two file signatures.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method compares the length of two file signatures.
        /// </para>
        /// <para>
        /// But beware, the comparison result is the opposite of left is less than right.
        /// This is necessary to have a descending sort order which ensures that longer 
        /// signatures are put in front of shorter signatures.
        /// </para>
        /// </remarks>
        /// <param name="x">
        /// The first object to compare.
        /// </param>
        /// <param name="y">
        /// The second object to compare.
        /// </param>
        /// <returns>
        /// <para>
        /// A signed integer that indicates the relative values of <paramref name="x"/> 
        /// and <paramref name="y"/>, as shown in the following table.
        /// </para>
        /// <list type="table">
        /// <listheader><term>Value</term><description>Meaning</description></listheader>
        /// <item><term>Less than zero</term><description>x is less than y.</description></item>
        /// <item><term>Zero</term><description>x equals y.</description></item>
        /// <item><term>Greater than zero</term><description>x is greater than y.</description></item>
        /// </list>
        /// </returns>
        public Int32 Compare(IFileSignature x, IFileSignature y)
        {
            // NOTE: Descending sort order to ensure longer signatures are in front of shorter signatures.

            if (x == null && y == null)
            {
                return 0;
            }

            if (x == null && y != null)
            {
                return 1;
            }

            if (x != null && y == null)
            {
                return -1;
            }

            if (x.Length < y.Length)
            {
                return 1;
            }

            if (x.Length > y.Length)
            {
                return -1;
            }

            return 0;
        }

        #endregion
    }
}
