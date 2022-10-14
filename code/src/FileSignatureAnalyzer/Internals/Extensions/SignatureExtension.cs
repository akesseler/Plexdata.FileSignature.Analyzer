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

using Plexdata.Utilities.Analyzers.Interfaces.Models;
using Plexdata.Utilities.Analyzers.Internals.Comparers;
using Plexdata.Utilities.Analyzers.Internals.Definitions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Plexdata.Utilities.Analyzers.Internals.Extensions
{
    /// <summary>
    /// A collection of extension methods related to managing file 
    /// signatures.
    /// </summary>
    /// <remarks>
    /// This class contains a collection of extension methods related 
    /// to managing file signatures.
    /// </remarks>
    internal static class SignatureExtension
    {
        #region Public Methods

        /// <summary>
        /// Splits provided source value into its pieces.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method splits provided source value into its pieces using 
        /// space, dash, comma or semicolon as splitting characters.
        /// </para>
        /// Be aware, each empty entry is removed as well as each further 
        /// valid entry is trimmed.
        /// </remarks>
        /// <param name="value">
        /// The value to be split into its pieces.
        /// </param>
        /// <returns>
        /// A list of signature digits. An empty list is returned if provided 
        /// source is null or empty or whitespace.
        /// </returns>
        /// <seealso cref="SignatureExtension.CheckSignature(String[])"/>
        /// <seealso cref="SignatureExtension.MergeSignature(String[], Int32)"/>
        public static String[] SplitSignature(this String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return new String[0];
            }

            return value.Split(InternalDefinitions.SignatureDelimiters)
                .Where(x => !String.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim().ToUpper()).ToArray();
        }

        /// <summary>
        /// Checks provided source list and confirms them as valid signature 
        /// items.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method checks provided source list and confirms them as valid 
        /// signature items. Each signature item is considered as valid as soon 
        /// as it represents a valid hexadecimal value or a valid placeholder.
        /// </para>
        /// <para>
        /// Please keep in mind, each signature item must consist of exactly two 
        /// characters, either in range of [0..9] or in range of [A..F]. Hexadecimal 
        /// letters are treaded case insensitive. Furthermore, each nibble can contain 
        /// a question mark can as placeholder.
        /// </para>
        /// </remarks>
        /// <example>
        /// For example, a valid file signature may look like this one:
        /// <code>
        ///  01 23 45 ?? ?9 aB Cd
        /// </code>
        /// </example>
        /// <param name="value">
        /// The list of items to be checked.
        /// </param>
        /// <returns>
        /// True if each of the provided source items has been considered as valid 
        /// and false otherwise.
        /// </returns>
        /// <seealso cref="SignatureExtension.SplitSignature(String)"/>
        /// <seealso cref="SignatureExtension.MergeSignature(String[], Int32)"/>
        /// <seealso cref="IsPlaceholder(Char)"/>
        /// <seealso cref="IsHexadecimal(Char)"/>
        public static Boolean CheckSignature(this String[] value)
        {
            if (value is null || value.Length < 1)
            {
                return false;
            }

            foreach (String current in value)
            {
                if (String.IsNullOrWhiteSpace(current) || current.Length != 2)
                {
                    return false;
                }

                Char upper = current[0];

                if (!upper.IsPlaceholder() && !upper.IsHexadecimal())
                {
                    return false;
                }

                Char lower = current[1];

                if (!lower.IsPlaceholder() && !lower.IsHexadecimal())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Merges provided source list back into a signature string.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method merges provided source list back into a signature string. 
        /// Each result item is separated by a spaces as well as each hexadecimal 
        /// letter is converted into its upper case representation.
        /// </para>
        /// </remarks>
        /// <param name="value">
        /// The list of items to be merged.
        /// </param>
        /// <param name="count">
        /// The number of items taken into account or less than one for unlimited. 
        /// Default value is zero.
        /// </param>
        /// <returns>
        /// The internal signature representation of provided source list. An empty 
        /// string is returned if provided source is null or empty.
        /// </returns>
        /// <seealso cref="SignatureExtension.SplitSignature(String)"/>
        /// <seealso cref="SignatureExtension.CheckSignature(String[])"/>
        public static String MergeSignature(this String[] value, Int32 count = 0)
        {
            if (!value?.Any() ?? true)
            {
                return String.Empty;
            }

            String separator = InternalDefinitions.SignatureSeparator.ToString();

            if (count > 0)
            {
                return String.Join(separator, value.Take(count).Select(x => x.ToUpper()));
            }

            return String.Join(separator, value.Select(x => x.ToUpper()));
        }

        /// <summary>
        /// Merges provided source list back into a signature string.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method merges provided source list back into a signature string. 
        /// Each result item is separated by a spaces as well as each hexadecimal 
        /// letter is converted into its upper case representation.
        /// </para>
        /// </remarks>
        /// <param name="value">
        /// The list of items to be merged.
        /// </param>
        /// <param name="count">
        /// The number of items taken into account or less than one for unlimited. 
        /// Default value is zero.
        /// </param>
        /// <returns>
        /// The internal signature representation of provided source list. An empty 
        /// string is returned if provided source is null or empty.
        /// </returns>
        public static String MergeSignature(this Byte[] value, Int32 count = 0)
        {
            if (!value?.Any() ?? true)
            {
                return String.Empty;
            }

            String separator = InternalDefinitions.SignatureSeparator.ToString();

            if (count > 0)
            {
                return String.Join(separator, value.Take(count).Select(x => x.ToString("X2")));
            }

            return String.Join(separator, value.Select(x => x.ToString("X2")));
        }

        /// <summary>
        /// Validates the list of file signature items.
        /// </summary>
        /// <remarks>
        /// This method validates list of file signature items.
        /// </remarks>
        /// <param name="signatures">
        /// List of file signatures to be validated.
        /// </param>
        /// <exception cref="ArgumentException">
        /// This exception is thrown in case of either provided list is null or empty 
        /// or it contains at least one null item.
        /// </exception>
        public static void Validate(this IEnumerable<IFileSignature> signatures)
        {
            if (!signatures?.Any() ?? true)
            {
                throw new ArgumentException($"Parameter '{nameof(signatures)}' must not be null or empty.", nameof(signatures));
            }

            if (signatures.Any(x => x == null))
            {
                throw new ArgumentException($"Parameter '{nameof(signatures)}' contains at least one null signature.", nameof(signatures));
            }
        }

        /// <summary>
        /// Sorts the list of file signatures.
        /// </summary>
        /// <remarks>
        /// This method sorts the list of file signatures by their signature length.
        /// The sort order of the result list is from longest signature down to the 
        /// shortest one.
        /// </remarks>
        /// <param name="source">
        /// The unsorted source list.
        /// </param>
        /// <returns>
        /// The sorted result list.
        /// </returns>
        /// <seealso cref="FileSignatureLengthComparer"/>
        public static IEnumerable<IFileSignature> SortByLength(this IEnumerable<IFileSignature> source)
        {
            List<IFileSignature> result = source.ToList();

            result.Sort(new FileSignatureLengthComparer());

            return result;
        }

        /// <summary>
        /// Checks if <paramref name="buffer"/> is equal to the <paramref name="signature"/>.
        /// </summary>
        /// <remarks>
        /// This method checks if <paramref name="buffer"/> is equal to the <paramref name="signature"/> 
        /// respecting placeholders.
        /// </remarks>
        /// <param name="signature">
        /// The signature to checked against the buffer.
        /// </param>
        /// <param name="buffer">
        /// The buffer to be compared with the signature.
        /// </param>
        /// <returns>
        /// True is returned when both lengths are equal as well as each byte (except placeholders) 
        /// is equal. False is returned in any other case.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown either if <paramref name="signature"/> or if <paramref name="buffer"/> 
        /// is null.
        /// </exception>
        /// <seealso cref="SplitNibbles(String)"/>
        /// <seealso cref="SplitNibbles(Byte)"/>
        /// <seealso cref="IsPlaceholder(Char)"/>
        public static Boolean IsEqual(this IFileSignature signature, Byte[] buffer)
        {
            if (signature == null)
            {
                throw new ArgumentNullException(nameof(signature), $"Value of '{nameof(signature)}' must not be null.");
            }

            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer), $"Value of '{nameof(buffer)}' must not be null.");
            }

            if (signature.Length != buffer.Length)
            {
                return false;
            }

            Debug.WriteLine($"Compare: {signature.Digits.MergeSignature()} <=> {buffer.MergeSignature()}");

            for (Int32 index = 0; index < signature.Length; index++)
            {
                Char[] x = signature.Digits[index].SplitNibbles();
                Char[] y = buffer[index].SplitNibbles();

                if (!x[0].IsPlaceholder() && x[0] != y[0])
                {
                    return false;
                }

                if (!x[1].IsPlaceholder() && x[1] != y[1])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Tries to confirm provided extension.
        /// </summary>
        /// <remarks>
        /// This method tries to confirm provided <paramref name="extension"/> with one of the 
        /// extensions assigned to provided <paramref name="signature"/>.
        /// </remarks>
        /// <param name="signature">
        /// The file signature to be confirmed.
        /// </param>
        /// <param name="extension">
        /// The file extension to be checked.
        /// </param>
        /// <returns>
        /// True is returned as soon as one of the signature extensions fits provided extension, 
        /// otherwise false is returned.
        /// </returns>
        public static Boolean GetConfirmed(this IFileSignature signature, String extension)
        {
            if (signature == null || String.IsNullOrWhiteSpace(signature.Extensions) || String.IsNullOrWhiteSpace(extension))
            {
                return false;
            }

            Char delimiter = InternalDefinitions.ExtensionDelimiter;

            foreach (String current in signature.Extensions.Split(delimiter).OrderByDescending(x => x.Length))
            {
                if (extension.EndsWith(current, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Check if provided character represents a placeholder.
        /// </summary>
        /// <remarks>
        /// This method check if provided character represents a placeholder.
        /// </remarks>
        /// <param name="value">
        /// The character to be checked.
        /// </param>
        /// <returns>
        /// True if <paramref name="value"/> represents a placeholder and false 
        /// otherwise.
        /// </returns>
        /// <seealso cref="InternalDefinitions.SignaturePlaceholder"/>
        private static Boolean IsPlaceholder(this Char value)
        {
            return value == InternalDefinitions.SignaturePlaceholder;
        }

        /// <summary>
        /// Check if provided character represents a hexadecimal character.
        /// </summary>
        /// <remarks>
        /// This method check if provided character represents a hexadecimal 
        /// character.
        /// </remarks>
        /// <param name="value">
        /// The character to be checked.
        /// </param>
        /// <returns>
        /// True if <paramref name="value"/> represents a hexadecimal character 
        /// and false otherwise.
        /// </returns>
        private static Boolean IsHexadecimal(this Char value)
        {
            return (value >= '0' && value <= '9') ||
                   (value >= 'a' && value <= 'f') ||
                   (value >= 'A' && value <= 'F');
        }

        /// <summary>
        /// Splits provided string of length two into an array of length two.
        /// </summary>
        /// <remarks>
        /// This method splits provided string of length two into an array of length 
        /// two. The first character represents the upper nibble and the second character 
        /// represents the lower nibble.
        /// </remarks>
        /// <param name="source">
        /// The string to be split into its characters.
        /// </param>
        /// <returns>
        /// An array of length two containing the upper and lower nibble.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if <paramref name="source"/> is null or its length 
        /// is not two.
        /// </exception>
        /// <seealso cref="SplitNibbles(Byte)"/>
        private static Char[] SplitNibbles(this String source)
        {
            if (source?.Length != 2)
            {
                throw new ArgumentException("The hexadecimal string to split must consist of exactly two character.", nameof(source));
            }

            return new Char[] { source[0], source[1] };
        }

        /// <summary>
        /// Splits provided byte into an array of length two.
        /// </summary>
        /// <remarks>
        /// This method splits provided byte into an array of length two.
        /// </remarks>
        /// <param name="source">
        /// The byte to be split.
        /// </param>
        /// <returns>
        /// An array of length two containing the upper and lower nibble.
        /// </returns>
        /// <seealso cref="SplitNibbles(String)"/>
        private static Char[] SplitNibbles(this Byte source)
        {
            return source.ToString("X2").SplitNibbles();
        }

        #endregion
    }
}
