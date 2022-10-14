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
using Plexdata.Utilities.Analyzers.Factories;
using Plexdata.Utilities.Analyzers.Interfaces;
using Plexdata.Utilities.Analyzers.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.Utilities.Analyzers.Tests.Factories
{
    [ExcludeFromCodeCoverage]
    internal class FileSignatureFactoryTests
    {
        // NOTE: It doesn't make any sense to test more, such as length, content, etc.

        [Test]
        public void CreateDefaultSignatures_MethodCall_ResultIsNotNull()
        {
            Assert.That(this.CreateInstance().CreateDefaultSignatures(), Is.Not.Null);
        }

        [Test]
        [Explicit]
        public void CreateDefaultSignatures_DumpAllForDocumentation_NoConfirmableTestResult()
        {
            IEnumerable<IFileSignature> signatures = this.CreateInstance().CreateDefaultSignatures();

            Debug.WriteLine("--------------------------------------------------");

            Debug.WriteLine("/// <table>");
            Debug.WriteLine("/// <tr><th>{0}</th><th>{1}</th><th>{2}</th><th>{3}</th><th>{4}</th></tr>",
                nameof(IFileSignature.Signature),
                nameof(IFileSignature.Offset),
                nameof(IFileSignature.Name),
                nameof(IFileSignature.Extensions),
                nameof(IFileSignature.Remarks));

            foreach (IFileSignature signature in signatures)
            {
                Debug.WriteLine("/// <tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>",
                    signature.Signature,
                    signature.Offset,
                    signature.Name,
                    String.Join(" ", signature.Extensions.Split(',')),
                    signature.Remarks);
            }

            Debug.WriteLine("/// </table>");

            Debug.WriteLine("--------------------------------------------------");

            Debug.WriteLine("|{0}|{1}|{2}|{3}|{4}|",
                nameof(IFileSignature.Signature),
                nameof(IFileSignature.Offset),
                nameof(IFileSignature.Name),
                nameof(IFileSignature.Extensions),
                nameof(IFileSignature.Remarks));

            Debug.WriteLine("|---|---|---|---|---|");

            foreach (IFileSignature signature in signatures)
            {
                Debug.WriteLine("|{0}|{1}|{2}|{3}|{4}|",
                    signature.Signature,
                    signature.Offset,
                    signature.Name,
                    String.Join(" ", signature.Extensions.Split(',')),
                    signature.Remarks);
            }

            Debug.WriteLine("--------------------------------------------------");
        }

        private IFileSignatureFactory CreateInstance()
        {
            return new FileSignatureFactory();
        }
    }
}
