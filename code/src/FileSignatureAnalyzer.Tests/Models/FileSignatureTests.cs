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
using Plexdata.Utilities.Analyzers.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Plexdata.Utilities.Analyzers.Tests.Models
{
    [ExcludeFromCodeCoverage]
    internal class FileSignatureTests
    {
        #region Property Name

        [Test]
        public void Name_DefaultConstruction_PropertyIsEmpty()
        {
            FileSignature instance = new FileSignature();

            Assert.That(instance.Name, Is.Empty);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Name_ValueIsInvalid_PropertyIsEmpty(String value)
        {
            FileSignature instance = new FileSignature() { Name = value };

            Assert.That(instance.Name, Is.Empty);
        }

        [TestCase("name")]
        [TestCase(" name")]
        [TestCase("name ")]
        [TestCase(" name ")]
        public void Name_ValueIsValid_PropertyAsExpected(String value)
        {
            FileSignature instance = new FileSignature() { Name = value };

            Assert.That(instance.Name, Is.EqualTo(value));
        }

        #endregion

        #region Property Remarks

        [Test]
        public void Remarks_DefaultConstruction_PropertyIsEmpty()
        {
            FileSignature instance = new FileSignature();

            Assert.That(instance.Remarks, Is.Empty);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Remarks_ValueIsInvalid_PropertyIsEmpty(String value)
        {
            FileSignature instance = new FileSignature() { Remarks = value };

            Assert.That(instance.Remarks, Is.Empty);
        }

        [TestCase("some comments")]
        [TestCase(" some comments")]
        [TestCase("some comments ")]
        [TestCase(" some comments ")]
        public void Remarks_ValueIsValid_PropertyAsExpected(String value)
        {
            FileSignature instance = new FileSignature() { Remarks = value };

            Assert.That(instance.Remarks, Is.EqualTo(value));
        }

        #endregion

        #region Property Extensions

        [Test]
        public void Extensions_DefaultConstruction_PropertyIsEmpty()
        {
            FileSignature instance = new FileSignature();

            Assert.That(instance.Extensions, Is.Empty);
        }

        [Test]
        public void Extensions_ValueIsNull_PropertyIsEmpty()
        {
            FileSignature instance = new FileSignature() { Extensions = null };

            Assert.That(instance.Extensions, Is.Empty);
        }

        [Test]
        public void Extensions_ValueIsValid_PropertyAsExpected()
        {
            String value = " .ext, .eXT ,  .ext , .ext,  .ext , ,  ext, ext ,  EXT ,  ext,  ext ,   ";

            FileSignature instance = new FileSignature() { Extensions = value };

            Assert.That(instance.Extensions, Is.EqualTo(".ext,.eXT,.ext,.ext,.ext,.ext,.ext,.EXT,.ext,.ext"));
        }

        #endregion

        #region Property Signature

        [Test]
        public void Signature_DefaultConstruction_PropertiesAreEmpty()
        {
            FileSignature instance = new FileSignature();

            Assert.That(instance.Signature, Is.Empty);
            Assert.That(instance.Digits, Is.Empty);
            Assert.That(instance.Length, Is.Zero);
        }

        [Test]
        public void Signature_ValueIsInvalid_ThrowsArgumentException()
        {
            FileSignature instance = new FileSignature();

            Assert.That(() => instance.Signature = null, Throws.ArgumentException);
        }

        [Test]
        public void Signature_ValueIsInvalid_PropertiesAsExpected()
        {
            FileSignature instance = new FileSignature();

            instance.Signature = "01-23-45-67-89-ab-cd-ef";

            Assert.That(instance.Signature, Is.EqualTo("01 23 45 67 89 AB CD EF"));
            Assert.That(Enumerable.SequenceEqual(instance.Digits, new String[] { "01", "23", "45", "67", "89", "AB", "CD", "EF" }), Is.True);
            Assert.That(instance.Length, Is.EqualTo(8));
        }

        #endregion

        #region Property Offset

        [Test]
        public void Offset_DefaultConstruction_PropertyIsEmpty()
        {
            FileSignature instance = new FileSignature();

            Assert.That(instance.Offset, Is.Zero);
        }

        [Test]
        public void Offset_ValueIsLessThanZero_ThrowsArgumentException()
        {
            FileSignature instance = new FileSignature();

            Assert.That(() => instance.Offset = -1, Throws.ArgumentException);
        }

        [TestCase(0)]
        [TestCase(42)]
        [TestCase(666)]
        public void Offset_ValueIsValid_PropertyAsExpected(Int64 value)
        {
            FileSignature instance = new FileSignature();

            instance.Offset = value;

            Assert.That(instance.Offset, Is.EqualTo(value));
        }

        #endregion
    }
}
