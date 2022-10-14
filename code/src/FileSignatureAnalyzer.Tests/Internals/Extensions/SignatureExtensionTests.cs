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
using Plexdata.Utilities.Analyzers.Internals.Extensions;
using Plexdata.Utilities.Analyzers.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Plexdata.Utilities.Analyzers.Tests.Internals.Extensions
{
    [ExcludeFromCodeCoverage]
    internal class SignatureExtensionTests
    {
        #region SplitSignature

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void SplitSignature_SourceIsInvalid_ResultIsEmpty(String source)
        {
            Assert.That(source.SplitSignature(), Is.Empty);
        }

        [Test]
        public void SplitSignature_SourceIsValid_ResultAsExpected()
        {
            String[] expected = new String[] { "AA", "BB", "CC", "DD", "EE", "FF" };

            String source = "   AA  bb\t, \t, \tCc - dD ; \tEE ; ;; FF;";

            String[] actual = source.SplitSignature();

            Assert.That(Enumerable.SequenceEqual(actual, expected), Is.True);
        }

        #endregion

        #region CheckSignature

        [Test]
        public void CheckSignature_SourceIsNull_ResultIsFalse()
        {
            String[] source = null;

            Assert.That(source.CheckSignature(), Is.False);
        }

        [Test]
        public void CheckSignature_SourceIsEmpty_ResultIsFalse()
        {
            String[] source = new String[0];

            Assert.That(source.CheckSignature(), Is.False);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("A")]
        [TestCase("ABC")]
        public void CheckSignature_SourceWithOneInvalidItem_ResultIsFalse(String item)
        {
            String[] source = new String[] { item };

            Assert.That(source.CheckSignature(), Is.False);
        }

        [TestCase("x1")]
        [TestCase("xa")]
        [TestCase("xA")]
        [TestCase("X1")]
        [TestCase("Xa")]
        [TestCase("XA")]
        [TestCase("x?")]
        [TestCase("X?")]
        public void CheckSignature_SourceWithOneInvalidUpperNibble_ResultIsFalse(String item)
        {
            String[] source = new String[] { item };

            Assert.That(source.CheckSignature(), Is.False);
        }

        [TestCase("1x")]
        [TestCase("ax")]
        [TestCase("Ax")]
        [TestCase("1X")]
        [TestCase("aX")]
        [TestCase("AX")]
        [TestCase("?x")]
        [TestCase("?X")]
        public void CheckSignature_SourceWithOneInvalidLowerNibble_ResultIsFalse(String item)
        {
            String[] source = new String[] { item };

            Assert.That(source.CheckSignature(), Is.False);
        }

        [TestCase("01", "??", "23", "45", "67", "8?", "?9", "ab", "cd", "ef")]
        [TestCase("01", "??", "23", "45", "67", "8?", "?9", "AB", "CD", "EF")]
        public void CheckSignature_SourceIsValid_ResultIsTrue(params String[] source)
        {
            Assert.That(source.CheckSignature(), Is.True);
        }

        #endregion

        #region MergeSignature

        [Test]
        public void MergeSignature_StringSourceIsNull_ResultIsEmpty()
        {
            String[] source = null;

            Assert.That(source.MergeSignature(), Is.Empty);
        }

        [Test]
        public void MergeSignature_StringSourceIsEmpty_ResultIsEmpty()
        {
            String[] source = new String[0];

            Assert.That(source.MergeSignature(), Is.Empty);
        }

        [TestCase("01 ?? 23 45 67 8? ?9 AB CD EF", "01", "??", "23", "45", "67", "8?", "?9", "ab", "cd", "ef")]
        [TestCase("01 ?? 23 45 67 8? ?9 AB CD EF", "01", "??", "23", "45", "67", "8?", "?9", "AB", "CD", "EF")]
        public void MergeSignature_StringSourceIsValid_ResultAsExpected(String expected, params String[] source)
        {
            Assert.That(source.MergeSignature(), Is.EqualTo(expected));
        }

        [TestCase("01 ?? 23", "01", "??", "23", "45", "67", "8?", "?9", "ab", "cd", "ef")]
        [TestCase("01 ?? 23", "01", "??", "23", "45", "67", "8?", "?9", "AB", "CD", "EF")]
        public void MergeSignature_StringSourceIsValidCountIsThree_ResultAsExpected(String expected, params String[] source)
        {
            Assert.That(source.MergeSignature(3), Is.EqualTo(expected));
        }

        [Test]
        public void MergeSignature_ByteSourceIsNull_ResultIsEmpty()
        {
            Byte[] source = null;

            Assert.That(source.MergeSignature(), Is.Empty);
        }

        [Test]
        public void MergeSignature_ByteSourceIsEmpty_ResultIsEmpty()
        {
            Byte[] source = new Byte[0];

            Assert.That(source.MergeSignature(), Is.Empty);
        }

        [Test]
        public void MergeSignature_ByteSourceIsValid_ResultAsExpected(/*String expected, params Byte[] source*/)
        {
            String expected = "01 23 45 67 89 AB CD EF";

            Byte[] source = new Byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };

            Assert.That(source.MergeSignature(), Is.EqualTo(expected));
        }

        [Test]
        public void MergeSignature_ByteSourceIsValidCountIsThree_ResultAsExpected()
        {
            String expected = "01 23 45";

            Byte[] source = new Byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };

            Assert.That(source.MergeSignature(3), Is.EqualTo(expected));
        }

        #endregion

        #region Validate

        [Test]
        public void Validate_SignaturesListIsNull_ThrowsArgumentException()
        {
            IEnumerable<IFileSignature> signatures = null;

            Assert.That(() => signatures.Validate(), Throws.ArgumentException);
        }

        [Test]
        public void Validate_SignaturesListIsEmpty_ThrowsArgumentException()
        {
            IEnumerable<IFileSignature> signatures = Enumerable.Empty<IFileSignature>();

            Assert.That(() => signatures.Validate(), Throws.ArgumentException);
        }

        [Test]
        public void Validate_SignaturesListWithInvalidItem_ThrowsArgumentException()
        {
            IEnumerable<IFileSignature> signatures = new List<IFileSignature>()
            {
                new FileSignature(),
                null,
                new FileSignature(),
            };

            Assert.That(() => signatures.Validate(), Throws.ArgumentException);
        }

        [Test]
        public void Validate_SignaturesListIsValid_ThrowsNothing()
        {
            IEnumerable<IFileSignature> signatures = new List<IFileSignature>()
            {
                new FileSignature(),
                new FileSignature(),
                new FileSignature(),
            };

            Assert.That(() => signatures.Validate(), Throws.Nothing);
        }

        #endregion

        #region SortByLength

        [Test]
        public void SortByLength_SignaturesListIsValid_SortOrderAsExpected()
        {
            IEnumerable<IFileSignature> signatures = new List<IFileSignature>()
            {
                new FileSignature() { Signature = "AA" },
                new FileSignature() { Signature = "AA BB" },
                new FileSignature() { Signature = "AA 11 CC" },
            };

            IFileSignature[] actual = signatures.SortByLength().ToArray();

            Assert.That(actual[0].Signature, Is.EqualTo("AA 11 CC"));
            Assert.That(actual[1].Signature, Is.EqualTo("AA BB"));
            Assert.That(actual[2].Signature, Is.EqualTo("AA"));
        }

        #endregion

        #region IsEqual

        [Test]
        public void IsEqual_InstanceIsNull_ThrowsArgumentNullException()
        {
            IFileSignature instance = null;

            Byte[] buffer = new Byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };

            Assert.That(() => instance.IsEqual(buffer), Throws.ArgumentNullException);
        }

        [Test]
        public void IsEqual_BufferIsNull_ThrowsArgumentNullException()
        {
            IFileSignature instance = new FileSignature();

            Byte[] buffer = null;

            Assert.That(() => instance.IsEqual(buffer), Throws.ArgumentNullException);
        }

        [Test]
        public void IsEqual_LengthIsDifferent_ResultIsFalse()
        {
            IFileSignature instance = new FileSignature();

            Byte[] buffer = new Byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };

            Assert.That(instance.IsEqual(buffer), Is.False);
        }

        [Test]
        public void IsEqual_EqualWithoutPlaceholders_ResultIsTrue()
        {
            IFileSignature instance = new FileSignature()
            {
                Signature = "01 23 45 67 89 ab cd ef"
            };

            Byte[] buffer = new Byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };

            Assert.That(instance.IsEqual(buffer), Is.True);
        }

        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x03)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x30)]
        public void IsEqual_NotEqualWithoutPlaceholders_ResultIsFalse(params Byte[] buffer)
        {
            IFileSignature instance = new FileSignature()
            {
                Signature = "11 22 33"
            };

            Assert.That(instance.IsEqual(buffer), Is.False);
        }

        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x03)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x13)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x23)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x33)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x43)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x53)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x63)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x73)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x83)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x93)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0xA3)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0xB3)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0xC3)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0xD3)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0xE3)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0xF3)]
        public void IsEqual_EqualWithUpperPlaceholders_ResultIsTrue(params Byte[] buffer)
        {
            IFileSignature instance = new FileSignature()
            {
                Signature = "11 22 ?3"
            };

            Assert.That(instance.IsEqual(buffer), Is.True);
        }

        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x30)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x31)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x32)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x33)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x34)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x35)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x36)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x37)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x38)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x39)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x3A)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x3B)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x3C)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x3D)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x3E)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x3F)]
        public void IsEqual_EqualWithLowerPlaceholders_ResultIsTrue(params Byte[] buffer)
        {
            IFileSignature instance = new FileSignature()
            {
                Signature = "11 22 3?"
            };

            Assert.That(instance.IsEqual(buffer), Is.True);
        }

        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x03)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x13)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x23)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x33)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x43)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x53)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x63)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x73)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x83)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x93)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0xA3)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0xB3)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0xC3)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0xD3)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0xE3)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0xF3)]
        public void IsEqual_NotEqualWithUpperPlaceholders_ResultIsFalse(params Byte[] buffer)
        {
            IFileSignature instance = new FileSignature()
            {
                Signature = "11 22 ?4"
            };

            Assert.That(instance.IsEqual(buffer), Is.False);
        }

        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x30)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x31)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x32)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x33)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x34)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x35)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x36)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x37)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x38)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x39)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x3A)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x3B)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x3C)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x3D)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x3E)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x3F)]
        public void IsEqual_NotEqualWithLowerPlaceholders_ResultIsFalse(params Byte[] buffer)
        {
            IFileSignature instance = new FileSignature()
            {
                Signature = "11 22 4?"
            };

            Assert.That(instance.IsEqual(buffer), Is.False);
        }

        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x00)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x11)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x22)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x33)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x44)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x55)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x66)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x77)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x88)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0x99)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0xAA)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0xBB)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0xCC)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0xDD)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0xEE)]
        [TestCase((Byte)0x11, (Byte)0x22, (Byte)0xFF)]
        public void IsEqual_EqualWithDoublePlaceholders_ResultIsTrue(params Byte[] buffer)
        {
            IFileSignature instance = new FileSignature()
            {
                Signature = "11 22 ??"
            };

            Assert.That(instance.IsEqual(buffer), Is.True);
        }

        #endregion

        #region GetConfirmed

        [Test]
        public void GetConfirmed_InstanceIsNull_ResultIsFalse()
        {
            IFileSignature instance = null;

            Assert.That(instance.GetConfirmed("ext"), Is.False);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void GetConfirmed_InstanceExtensionsIsInvalid_ResultIsFalse(String value)
        {
            IFileSignature instance = new FileSignature()
            {
                Extensions = value
            };

            Assert.That(instance.GetConfirmed("ext"), Is.False);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void GetConfirmed_ParameterExtensionIsInvalid_ResultIsFalse(String value)
        {
            IFileSignature instance = new FileSignature()
            {
                Extensions = "ext"
            };

            Assert.That(instance.GetConfirmed(value), Is.False);
        }

        [TestCase("other")]
        [TestCase("OtHer")]
        [TestCase("OTHER")]
        public void GetConfirmed_ParameterExtensionIsNotIncluded_ResultIsFalse(String value)
        {
            IFileSignature instance = new FileSignature()
            {
                Extensions = "ext"
            };

            Assert.That(instance.GetConfirmed(value), Is.False);
        }

        [TestCase(".ext2")]
        [TestCase(".eXt2")]
        [TestCase(".EXT2")]
        public void GetConfirmed_ParameterExtensionIsIncluded_ResultIsTrue(String value)
        {
            IFileSignature instance = new FileSignature()
            {
                Extensions = "ext1,ext2,ext3"
            };

            Assert.That(instance.GetConfirmed(value), Is.True);
        }

        [TestCase(".doc.tar.z")]
        [TestCase(".doc.z")]
        public void GetConfirmed_SomeRealisticTest_ResultIsTrue(String value)
        {
            IFileSignature instance = new FileSignature()
            {
                Extensions = "z,tar.z"
            };

            Assert.That(instance.GetConfirmed(value), Is.True);
        }

        #endregion
    }
}
