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

using NUnit.Framework;
using Plexdata.Utilities.Analyzers.Interfaces.Models;
using Plexdata.Utilities.Analyzers.Internals.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.Utilities.Analyzers.Tests.Internals.Models
{
    [ExcludeFromCodeCoverage]
    internal class AnalyzerResultTests
    {
        [Test]
        public void AnalyzerResult_DefaultConstruction_PropertiesWithDefaultValues()
        {
            IAnalyzerResult instance = new AnalyzerResult();

            Assert.That(instance.IsUnknown, Is.False);
            Assert.That(instance.Confirmed, Is.False);
            Assert.That(instance.Name, Is.Empty);
            Assert.That(instance.Remarks, Is.Empty);
            Assert.That(instance.Extension, Is.Empty);
            Assert.That(instance.Offset, Is.EqualTo(-1));
            Assert.That(instance.Length, Is.Zero);
            Assert.That(instance.Signature, Is.Empty);
            Assert.That(instance.Match, Is.Null);
        }

        [Test]
        public void AnalyzerResult_InstanceIsUnknown_PropertiesAsExpected()
        {
            IAnalyzerResult instance = AnalyzerResult.Unknown;

            Assert.That(instance.IsUnknown, Is.True);
            Assert.That(instance.Confirmed, Is.False);
            Assert.That(instance.Name, Is.EqualTo(nameof(AnalyzerResult.Unknown)));
            Assert.That(instance.Remarks, Is.Empty);
            Assert.That(instance.Extension, Is.Empty);
            Assert.That(instance.Offset, Is.EqualTo(-1));
            Assert.That(instance.Length, Is.Zero);
            Assert.That(instance.Signature, Is.Empty);
            Assert.That(instance.Match, Is.Null);
        }

        [Test]
        public void AnalyzerResult_SignatureIsNull_PropertyLengthIsZero()
        {
            IAnalyzerResult instance = new AnalyzerResult()
            {
                Signature = null
            };

            Assert.That(instance.Length, Is.Zero);
        }

        [Test]
        public void AnalyzerResult_SignatureIsEmpty_PropertyLengthIsZero()
        {
            IAnalyzerResult instance = new AnalyzerResult()
            {
                Signature = new Byte[0]
            };

            Assert.That(instance.Length, Is.Zero);
        }

        [Test]
        public void AnalyzerResult_SignatureIsValid_PropertyLengthAsExpected()
        {
            IAnalyzerResult instance = new AnalyzerResult()
            {
                Signature = new Byte[10]
            };

            Assert.That(instance.Length, Is.EqualTo(10));
        }
    }
}
