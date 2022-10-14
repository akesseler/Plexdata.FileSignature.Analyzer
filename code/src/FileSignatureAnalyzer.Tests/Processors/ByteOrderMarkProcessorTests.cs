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
using Plexdata.Utilities.Analyzers.Interfaces;
using Plexdata.Utilities.Analyzers.Processors;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Plexdata.Utilities.Analyzers.Tests.Processors
{
    [ExcludeFromCodeCoverage]
    internal class ByteOrderMarkProcessorTests
    {
        private Byte[] buffer;
        private List<Byte[]> preambles = null;

        [SetUp]
        public void SetUp()
        {
            this.buffer = new Byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };

            this.preambles = new List<Byte[]>()
            {
                new Byte[] { 0xEF, 0xBB, 0xBF       },
                new Byte[] { 0xFF, 0xFE             },
                new Byte[] { 0xFE, 0xFF             },
                new Byte[] { 0xFF, 0xFE, 0x00, 0x00 },
                new Byte[] { 0x00, 0x00, 0xFE, 0xFF },
            };
        }

        [Test]
        public void Process_StreamIsNull_ThrowsArgumentNullException()
        {
            Stream stream = null;

            Assert.That(() => this.CreateInstance().Process(stream), Throws.ArgumentNullException);
        }

        [TestCase(false, false)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void Process_StreamIsInvalid_ThrowsInvalidOperationException(Boolean read, Boolean seek)
        {
            Mock<Stream> mock = new Mock<Stream>();
            mock.SetupGet(x => x.CanRead).Returns(read);
            mock.SetupGet(x => x.CanSeek).Returns(seek);

            Stream stream = mock.Object;

            Assert.That(() => this.CreateInstance().Process(stream), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void Process_StreamIsEmpty_StreamPositionIsZero()
        {
            using (MemoryStream stream = this.CreateStream(null, null))
            {
                this.CreateInstance().Process(stream);

                Assert.That(stream.Position, Is.Zero);
            }
        }

        [Test]
        public void Process_StreamWithBufferOnly_StreamPositionIsZero()
        {
            using (MemoryStream stream = this.CreateStream(null, this.buffer))
            {
                this.CreateInstance().Process(stream);

                Assert.That(stream.Position, Is.Zero);
            }
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void Process_StreamWithPreambleOnly_StreamPositionAsExpected(Int32 index)
        {
            using (MemoryStream stream = this.CreateStream(this.preambles[index], null))
            {
                this.CreateInstance().Process(stream);

                Assert.That(stream.Position, Is.EqualTo(this.preambles[index].Length));
            }
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void Process_StreamWithPreambleAndBuffer_StreamPositionAsExpected(Int32 index)
        {
            using (MemoryStream stream = this.CreateStream(this.preambles[index], this.buffer))
            {
                this.CreateInstance().Process(stream);

                Assert.That(stream.Position, Is.EqualTo(this.preambles[index].Length));
            }
        }

        private IByteOrderMarkProcessor CreateInstance()
        {
            return new ByteOrderMarkProcessor();
        }

        private MemoryStream CreateStream(Byte[] preamble, Byte[] buffer)
        {
            MemoryStream result = new MemoryStream();

            if (preamble?.Length > 0)
            {
                result.Write(preamble, 0, preamble.Length);
                result.Position = preamble.Length;
            }

            if (buffer?.Length > 0)
            {
                result.Write(buffer, 0, buffer.Length);
                result.Position = buffer.Length;
            }

            return result;
        }
    }
}
