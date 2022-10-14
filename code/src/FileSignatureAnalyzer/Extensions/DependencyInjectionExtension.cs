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
using Plexdata.Utilities.Analyzers.Factories;
using Plexdata.Utilities.Analyzers.Interfaces;
using Plexdata.Utilities.Analyzers.Processors;

namespace Plexdata.Utilities.Analyzers.Extensions
{
    /// <summary>
    /// An extension class to perform all dependency injections.
    /// </summary>
    /// <remarks>
    /// This extension class represents a helper used to perform 
    /// all needed dependency injections.
    /// </remarks>
    public static class DependencyInjectionExtension
    {
        #region Public Methods

        /// <summary>
        /// Performs all dependency registrations.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method performs the registration of all dependency provided by this 
        /// package.
        /// </para>
        /// <para>
        /// The implementation of <see cref="IFileSignatureFactory"/> is registered as 
        /// <i>Singleton</i>.
        /// </para>
        /// <para>
        /// The implementations of <see cref="IByteOrderMarkProcessor"/> and 
        /// <see cref="IFileSignatureAnalyzer"/> instead are registered as <i>Transient</i>.
        /// </para>
        /// </remarks>
        /// <param name="services">
        /// The dependency container where to register all provided dependencies.
        /// </param>
        /// <returns>
        /// The dependency container where all dependencies are registered.
        /// </returns>
        public static IServiceCollection RegisterSignatureAnalyzer(this IServiceCollection services)
        {
            if (services == null)
            {
                return services;
            }

            services.AddSingleton<IFileSignatureFactory, FileSignatureFactory>();

            services.AddTransient<IByteOrderMarkProcessor, ByteOrderMarkProcessor>();
            services.AddTransient<IFileSignatureAnalyzer, FileSignatureAnalyzer>();

            return services;
        }

        #endregion
    }
}
