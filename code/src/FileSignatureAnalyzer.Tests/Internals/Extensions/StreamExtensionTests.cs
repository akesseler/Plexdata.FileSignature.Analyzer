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
using Plexdata.Utilities.Analyzers.Internals.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Plexdata.Utilities.Analyzers.Tests.Internals.Extensions
{
    [ExcludeFromCodeCoverage]
    internal class StreamExtensionTests
    {
        #region Validate

        [Test]
        public void Validate_StreamIsNull_ThrowsArgumentNullException()
        {
            Stream stream = null;

            Assert.That(() => stream.Validate(), Throws.ArgumentNullException);
        }

        [TestCase(false, false)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void Validate_StreamIsInvalid_ThrowsInvalidOperationException(Boolean read, Boolean seek)
        {
            Mock<Stream> mock = new Mock<Stream>();

            mock.SetupGet(x => x.CanRead)
                .Returns(read);
            mock.SetupGet(x => x.CanSeek)
                .Returns(seek);

            Stream stream = mock.Object;

            Assert.That(() => stream.Validate(), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void Validate_StreamIsValid_ThrowsNothing()
        {
            Mock<Stream> mock = new Mock<Stream>();

            mock.SetupGet(x => x.CanRead)
                .Returns(true);
            mock.SetupGet(x => x.CanSeek)
                .Returns(true);

            Stream stream = mock.Object;

            Assert.That(() => stream.Validate(), Throws.Nothing);
        }

        #endregion

        #region TryReadBuffer

        [Test]
        public void TryReadBuffer_OffsetIsInvalid_ResultIsFalse()
        {
            Mock<Stream> mock = new Mock<Stream>();

            Stream stream = mock.Object;

            Assert.That(() => stream.TryReadBuffer(-1, 3, out Byte[] result), Is.False);
        }

        [Test]
        public void TryReadBuffer_StreamLengthIsInvalid_ResultIsFalse()
        {
            Mock<Stream> mock = new Mock<Stream>();

            mock.SetupGet(x => x.Length)
                .Returns(1);

            Stream stream = mock.Object;

            Assert.That(() => stream.TryReadBuffer(0, 3, out Byte[] result), Is.False);
        }

        [Test]
        public void TryReadBuffer_StreamIsValid_StreamWasReadAsExpectedAndResultIsTrue()
        {
            Int64 position = -1;
            Byte[] buffer = null;
            Int32 offset = -1;
            Int32 length = -1;

            Mock<Stream> mock = new Mock<Stream>();

            mock.SetupGet(x => x.Length)
                .Returns(15);
            mock.SetupSet(x => x.Position = It.IsAny<Int64>())
                .Callback((Int64 x) => { position = x; });
            mock.Setup(x => x.Read(It.IsAny<Byte[]>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                .Callback((Byte[] b, Int32 o, Int32 l) => { buffer = b; offset = o; length = l; })
                .Returns(0);

            Stream stream = mock.Object;

            Assert.That(stream.TryReadBuffer(5, 3, out Byte[] result), Is.True);

            Assert.That(position, Is.EqualTo(5));
            Assert.That(buffer, Is.Not.Null);
            Assert.That(buffer.Length, Is.EqualTo(3));
            Assert.That(offset, Is.EqualTo(0));
            Assert.That(length, Is.EqualTo(3));
        }

        [Test]
        public void TryReadBuffer_StreamReadThrows_ResultIsFalse()
        {
            Mock<Stream> mock = new Mock<Stream>();

            mock.SetupGet(x => x.Length)
                .Returns(3);
            mock.Setup(x => x.Read(It.IsAny<Byte[]>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                .Throws<Exception>();

            Stream stream = mock.Object;

            Assert.That(() => stream.TryReadBuffer(0, 3, out Byte[] result), Is.False);
        }

        #endregion

        #region GetFileExtension

        [Test]
        public void GetFileExtension_StreamIsNotFileStream_ResultIsEmpty()
        {
            Mock<Stream> mock = new Mock<Stream>();

            Stream stream = mock.Object;

            Assert.That(stream.GetFileExtension(), Is.Empty);
        }

        [Test]
        public void GetFileExtension_StreamIsFileStreamOfSomeTempFile_ResultAsExpected()
        {
            String path = Path.GetTempFileName();

            try
            {
                String expected = Path.GetExtension(path);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    Assert.That(stream.GetFileExtension(), Is.EqualTo(expected));
                }
            }
            finally
            {
                try { File.Delete(path); } catch { }
            }
        }

        #endregion
    }
}
