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

using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Plexdata.Utilities.Analyzers.Extensions;
using Plexdata.Utilities.Analyzers.Factories;
using Plexdata.Utilities.Analyzers.Interfaces;
using Plexdata.Utilities.Analyzers.Processors;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.Utilities.Analyzers.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    internal class DependencyInjectionExtensionTests
    {
        private IServiceCollection services;
        private IServiceProvider provider;

        [SetUp]
        public void SetUp()
        {
            this.services = new ServiceCollection();

            this.provider = this.services
                .RegisterSignatureAnalyzer()
                .BuildServiceProvider();
        }

        [Test]
        public void RegisterSignatureAnalyzer_IServiceCollectionIsNull_ThrowsNothing()
        {
            IServiceCollection instance = null;

            Assert.That(() => instance.RegisterSignatureAnalyzer(), Throws.Nothing);
        }

        [Test]
        public void RegisterSignatureAnalyzer_IServiceCollectionIsValid_IServiceProviderGetServiceIFileSignatureFactoryInstanceOfWasResolved()
        {
            Object instance = this.provider.GetService<IFileSignatureFactory>();

            Assert.That(instance, Is.InstanceOf<FileSignatureFactory>());
        }

        [Test]
        public void RegisterSignatureAnalyzer_IServiceCollectionIsValid_IServiceProviderGetServiceIByteOrderMarkProcessorInstanceOfWasResolved()
        {
            Object instance = this.provider.GetService<IByteOrderMarkProcessor>();

            Assert.That(instance, Is.InstanceOf<ByteOrderMarkProcessor>());
        }

        [Test]
        public void RegisterSignatureAnalyzer_IServiceCollectionIsValid_IServiceProviderGetServiceIFileSignatureAnalyzerInstanceOfWasResolved()
        {
            Object instance = this.provider.GetService<IFileSignatureAnalyzer>();

            Assert.That(instance, Is.InstanceOf<FileSignatureAnalyzer>());
        }
    }
}
