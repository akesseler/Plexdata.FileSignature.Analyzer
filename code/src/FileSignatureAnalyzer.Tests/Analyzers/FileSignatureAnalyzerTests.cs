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
using Plexdata.Utilities.Analyzers.Interfaces.Models;
using Plexdata.Utilities.Analyzers.Internals.Models;
using Plexdata.Utilities.Analyzers.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Plexdata.Utilities.Analyzers.Tests
{
    class FileSignatureAnalyzerTests
    {
        private Mock<IByteOrderMarkProcessor> processor;
        private Mock<Stream> stream;

        private IEnumerable<IFileSignature> signatures;

        [SetUp]
        public void SetUp()
        {
            this.processor = new Mock<IByteOrderMarkProcessor>();
            this.stream = new Mock<Stream>();

            this.signatures = new List<IFileSignature>()
            {
                new FileSignature() { Signature = "11 22 33 44" }
            };

            this.stream.SetupGet(x => x.CanRead)
                .Returns(true);

            this.stream.SetupGet(x => x.CanSeek)
                .Returns(true);
        }

        [TestCase(typeof(IByteOrderMarkProcessor))]
        public void FileSignatureAnalyzer_DependencyIsNull_ThrowsArgumentNullException(Type type)
        {
            this.processor = type == typeof(IByteOrderMarkProcessor) ? null : this.processor;

            Assert.That(() => this.CreateInstance(), Throws.ArgumentNullException);
        }

        [Test]
        public void Analyze_StreamIsNull_ThrowsArgumentNullException()
        {
            this.stream = null;

            Assert.That(() => this.CreateInstance().Analyze(this.stream?.Object, this.signatures), Throws.ArgumentNullException);
        }

        [Test]
        public void Analyze_StreamCanReadIsFalse_ThrowsInvalidOperationException()
        {
            this.stream.SetupGet(x => x.CanRead)
                .Returns(false);

            Assert.That(() => this.CreateInstance().Analyze(this.stream.Object, this.signatures), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void Analyze_StreamCanSeekIsFalse_ThrowsInvalidOperationException()
        {
            this.stream.SetupGet(x => x.CanSeek)
                .Returns(false);

            Assert.That(() => this.CreateInstance().Analyze(this.stream.Object, this.signatures), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void Analyze_SignaturesIsNull_ThrowsArgumentException()
        {
            this.signatures = null;

            Assert.That(() => this.CreateInstance().Analyze(this.stream.Object, this.signatures), Throws.ArgumentException);
        }

        [Test]
        public void Analyze_SignaturesIsEmpty_ThrowsArgumentException()
        {
            this.signatures = Enumerable.Empty<IFileSignature>();

            Assert.That(() => this.CreateInstance().Analyze(this.stream.Object, this.signatures), Throws.ArgumentException);
        }

        [Test]
        public void Analyze_SignaturesWithNullItem_ThrowsArgumentException()
        {
            this.signatures = Enumerable.Repeat<IFileSignature>(null, 1);

            Assert.That(() => this.CreateInstance().Analyze(this.stream.Object, this.signatures), Throws.ArgumentException);
        }

        [Test]
        public void Analyze_ParametersValid_StreamPositionWasSetAsExpected()
        {
            this.CreateInstance().Analyze(this.stream.Object, this.signatures);

            this.stream.VerifySet(x => x.Position = 0);
        }

        [Test]
        public void Analyze_ParametersValid_SignaturesSortedDescending()
        {
            List<Int32> counts = new List<Int32>();

            this.signatures = new List<IFileSignature>()
            {
                new FileSignature() { Signature = "11"          },
                new FileSignature() { Signature = "11 22"       },
                new FileSignature() { Signature = "11 22 33"    },
                new FileSignature() { Signature = "11 22 33 44" }
            };

            this.stream.SetupGet(x => x.Length)
                .Returns(4);

            this.stream.Setup(x => x.Read(It.IsAny<Byte[]>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                .Callback((Byte[] b, Int32 o, Int32 c) => counts.Add(c));

            this.CreateInstance().Analyze(this.stream.Object, this.signatures);

            Assert.That(counts[0], Is.EqualTo(4));
            Assert.That(counts[1], Is.EqualTo(3));
            Assert.That(counts[2], Is.EqualTo(2));
            Assert.That(counts[3], Is.EqualTo(1));
        }

        [Test]
        public void Analyze_ParametersValid_ByteOrderMarkProcessorProcessWasCalledOnce()
        {
            this.CreateInstance().Analyze(this.stream.Object, this.signatures);

            this.processor.Verify(x => x.Process(this.stream.Object), Times.Once);
        }

        [Test]
        public void Analyze_ParametersValidSignatureOffsetIsZero_StreamPositionIsAdjustedAsExpected()
        {
            List<Int64> positions = new List<Int64>();

            this.stream.SetupGet(x => x.Length)
                .Returns(8);

            this.stream.SetupSet(x => x.Position = It.IsAny<Int64>())
                .Callback((Int64 p) => positions.Add(p));

            this.CreateInstance().Analyze(this.stream.Object, this.signatures);

            Assert.That(positions[0], Is.EqualTo(0));
            Assert.That(positions[1], Is.EqualTo(0));
        }

        [Test]
        public void Analyze_ParametersValidSignatureOffsetIsThree_StreamPositionIsAdjustedAsExpected()
        {
            List<Int64> positions = new List<Int64>();

            this.signatures = new List<IFileSignature>()
            {
                new FileSignature() { Offset = 3, Signature = "11 22 33 44" }
            };

            this.stream.SetupGet(x => x.Length)
                .Returns(11);

            this.stream.SetupSet(x => x.Position = It.IsAny<Int64>())
                .Callback((Int64 p) => positions.Add(p));

            this.CreateInstance().Analyze(this.stream.Object, this.signatures);

            Assert.That(positions[0], Is.EqualTo(0));
            Assert.That(positions[1], Is.EqualTo(3));
        }

        [Test]
        public void Analyze_ParametersValidWithBomSignatureOffsetIsZero_StreamPositionIsAdjustedAsExpected()
        {
            List<Int64> positions = new List<Int64>();

            this.stream.SetupGet(x => x.Position)
                .Returns(4); // BOM position...

            this.stream.SetupGet(x => x.Length)
                .Returns(8);

            this.stream.SetupSet(x => x.Position = It.IsAny<Int64>())
                .Callback((Int64 p) => positions.Add(p));

            this.CreateInstance().Analyze(this.stream.Object, this.signatures);

            Assert.That(positions[0], Is.EqualTo(0));
            Assert.That(positions[1], Is.EqualTo(4));
        }

        [Test]
        public void Analyze_ParametersValidWithBomSignatureOffsetIsThree_StreamPositionIsAdjustedAsExpected()
        {
            List<Int64> positions = new List<Int64>();

            this.signatures = new List<IFileSignature>()
            {
                new FileSignature() { Offset = 3, Signature = "11 22 33 44" }
            };

            this.stream.SetupGet(x => x.Position)
                .Returns(4); // BOM position...

            this.stream.SetupGet(x => x.Length)
                .Returns(11);

            this.stream.SetupSet(x => x.Position = It.IsAny<Int64>())
                .Callback((Int64 p) => positions.Add(p));

            this.CreateInstance().Analyze(this.stream.Object, this.signatures);

            Assert.That(positions[0], Is.EqualTo(0));
            Assert.That(positions[1], Is.EqualTo(7));
        }

        [Test]
        public void Analyze_ParametersValidAndNoSignatureFound_ResultIsAnalyzerResultUnknown()
        {
            IEnumerable<IAnalyzerResult> actual = this.CreateInstance().Analyze(this.stream.Object, this.signatures);

            Assert.That(actual.First(), Is.SameAs(AnalyzerResult.Unknown));
        }

        [Test]
        public void Analyze_ParametersValidAndSignatureFound_ResultIsExpectedAnalyzerResult()
        {
            this.signatures = new List<IFileSignature>()
            {
                new FileSignature() { Name = "test-signature-name", Remarks = "test-signature-remarks", Offset = 3, Signature = "11 22 33 44" }
            };

            this.stream.SetupGet(x => x.Length)
                .Returns(7);

            this.stream.Setup(x => x.Read(It.IsAny<Byte[]>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                .Callback((Byte[] b, Int32 o, Int32 c) =>
                {
                    b[0] = 0x11; b[1] = 0x22; b[2] = 0x33; b[3] = 0x44;
                });

            IAnalyzerResult actual = this.CreateInstance().Analyze(this.stream.Object, this.signatures).First();

            Assert.That(actual.Name, Is.EqualTo("test-signature-name"));
            Assert.That(actual.Remarks, Is.EqualTo("test-signature-remarks"));
            Assert.That(actual.Offset, Is.EqualTo(3));
            Assert.That(String.Join(" ", actual.Signature.Select(x => x.ToString("X2"))), Is.EqualTo("11 22 33 44"));
            Assert.That(actual.Match, Is.SameAs(this.signatures.First()));
        }

        private IFileSignatureAnalyzer CreateInstance()
        {
            return new FileSignatureAnalyzer(
                this.processor?.Object
            );
        }
    }
}
