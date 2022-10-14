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

using Moq;
using NUnit.Framework;
using Plexdata.Utilities.Analyzers.Interfaces.Models;
using Plexdata.Utilities.Analyzers.Internals.Comparers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.Utilities.Analyzers.Tests.Internals.Comparers
{
    [ExcludeFromCodeCoverage]
    internal class FileSignatureLengthComparerTests
    {
        [Test]
        public void Compare_ItemOneIsNullAndItemTwoIsNull_ResultIsZero()
        {
            Mock<IFileSignature> item1 = null;
            Mock<IFileSignature> item2 = null;

            Assert.That(this.CreateInstance().Compare(item1?.Object, item2?.Object), Is.Zero);
        }

        [Test]
        public void Compare_ItemOneIsNullAndItemTwoIsValid_ResultIsOne()
        {
            Mock<IFileSignature> item1 = null;
            Mock<IFileSignature> item2 = new Mock<IFileSignature>();

            Assert.That(this.CreateInstance().Compare(item1?.Object, item2?.Object), Is.EqualTo(1));
        }

        [Test]
        public void Compare_ItemOneIsValidAndItemTwoIsNull_ResultIsMinusOne()
        {
            Mock<IFileSignature> item1 = new Mock<IFileSignature>();
            Mock<IFileSignature> item2 = null;

            Assert.That(this.CreateInstance().Compare(item1?.Object, item2?.Object), Is.EqualTo(-1));
        }

        [Test]
        public void Compare_ItemOneIsLessThanItemTwo_ResultIsOne()
        {
            Mock<IFileSignature> item1 = this.CreateMock(2);
            Mock<IFileSignature> item2 = this.CreateMock(5);

            Assert.That(this.CreateInstance().Compare(item1?.Object, item2?.Object), Is.EqualTo(1));
        }

        [Test]
        public void Compare_ItemOneIsGreaterThanItemTwo_ResultIsMinusOne()
        {
            Mock<IFileSignature> item1 = this.CreateMock(5);
            Mock<IFileSignature> item2 = this.CreateMock(2);

            Assert.That(this.CreateInstance().Compare(item1?.Object, item2?.Object), Is.EqualTo(-1));
        }

        [Test]
        public void Compare_ItemOneIsEqualToItemTwo_ResultIsZero()
        {
            Mock<IFileSignature> item1 = this.CreateMock(3);
            Mock<IFileSignature> item2 = this.CreateMock(3);

            Assert.That(this.CreateInstance().Compare(item1?.Object, item2?.Object), Is.Zero);
        }

        private IComparer<IFileSignature> CreateInstance()
        {
            return new FileSignatureLengthComparer();
        }

        private Mock<IFileSignature> CreateMock(Int32 length)
        {
            Mock<IFileSignature> result = new Mock<IFileSignature>();

            result.Setup(x => x.Length).Returns(length);

            return result;
        }
    }
}
