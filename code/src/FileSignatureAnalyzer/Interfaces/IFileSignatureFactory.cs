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
using System.Collections.Generic;

namespace Plexdata.Utilities.Analyzers.Interfaces
{
    /// <summary>
    /// An interface representing a general factory.
    /// </summary>
    /// <remarks>
    /// This interface represents an abstraction of the 
    /// implementation a general factory class.
    /// </remarks>
    public interface IFileSignatureFactory
    {
        /// <summary>
        /// Creates the list of default signatures.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method creates a list containing all supported default signatures.
        /// This default signature list might be used or users may configure their 
        /// own list of signatures. See below for a list of all supported default 
        /// signatures.
        /// </para>
        /// <table>
        /// <tr><th>Signature</th><th>Offset</th><th>Name</th><th>Extensions</th><th>Remarks</th></tr>
        /// <tr><td>4D 5A</td><td>0</td><td>Executable</td><td>.exe .scr .sys .dll .fon .cpl .iec .ime .rs .tsp .mz</td><td>DOS MZ executable and its descendants (including NE and PE)</td></tr>
        /// <tr><td>89 50 4E 47 0D 0A 1A 0A</td><td>0</td><td>PNG Image</td><td>.png</td><td>Image encoded in the Portable Network Graphics format</td></tr>
        /// <tr><td>37 7A BC AF 27 1C</td><td>0</td><td>7 Zip</td><td>.7z</td><td>7 Zip file format</td></tr>
        /// <tr><td>3C 3F 78 6D 6C 20</td><td>0</td><td>XML (UTF-8)</td><td>.xml</td><td>XML file format (UTF-8)</td></tr>
        /// <tr><td>3C 00 3F 00 78 00 6D 00 6C 00 20 00</td><td>0</td><td>XML (UTF-16 LE)</td><td>.xml</td><td>XML file format (UTF-16 LE)</td></tr>
        /// <tr><td>00 3C 00 3F 00 78 00 6D 00 6C 00 20</td><td>0</td><td>XML (UTF-16 BE)</td><td>.xml</td><td>XML file format (UTF-16 BE)</td></tr>
        /// <tr><td>50 4B 03 04</td><td>0</td><td>PK ZIP</td><td>.zip .aar .apk .docx .epub .ipa .jar .kmz .maff .msix .odp .ods .odt .pk3 .pk4 .pptx .usdz .vsdx .xlsx .xpi</td><td>Zip file format and formats based on it, such as EPUB, JAR, ODF, OOXML</td></tr>
        /// <tr><td>50 4B 05 06</td><td>0</td><td>PK ZIP (empty archive)</td><td>.zip .aar .apk .docx .epub .ipa .jar .kmz .maff .msix .odp .ods .odt .pk3 .pk4 .pptx .usdz .vsdx .xlsx .xpi</td><td>Zip file format and formats based on it, such as EPUB, JAR, ODF, OOXML</td></tr>
        /// <tr><td>50 4B 07 08</td><td>0</td><td>PK ZIP  (spanned archive)</td><td>.zip .aar .apk .docx .epub .ipa .jar .kmz .maff .msix .odp .ods .odt .pk3 .pk4 .pptx .usdz .vsdx .xlsx .xpi</td><td>Zip file format and formats based on it, such as EPUB, JAR, ODF, OOXML</td></tr>
        /// <tr><td>53 51 4C 69 74 65 20 66 6F 72 6D 61 74 20 33 00</td><td>0</td><td>SQLite Database</td><td>.sqlitedb .sqlite .db</td><td>SQLite database file</td></tr>
        /// <tr><td>00 00 01 00</td><td>0</td><td>Icon File</td><td>.ico</td><td>Computer icon encoded in ICO file format</td></tr>
        /// <tr><td>1F 9D</td><td>0</td><td>TAR ZIP</td><td>.z .tar.z</td><td>Compressed file (often tar zip) using Lempel-Ziv-Welch algorithm</td></tr>
        /// <tr><td>1F A0</td><td>0</td><td>TAR ZIP</td><td>.z .tar.z</td><td>Compressed file (often tar zip) using LZH algorithm</td></tr>
        /// <tr><td>42 5A 68</td><td>0</td><td>BZIP2</td><td>.bz2</td><td>Compressed file using Bzip2 algorithm</td></tr>
        /// <tr><td>75 73 74 61 72 00 30 30</td><td>257</td><td>TAR ZIP (POSIX)</td><td>.tar</td><td>Compressed file tar archive</td></tr>
        /// <tr><td>75 73 74 61 72 20 20 00</td><td>257</td><td>TAR ZIP (GNU)</td><td>.tar</td><td>Compressed file tar archive</td></tr>
        /// <tr><td>D0 CF 11 E0 A1 B1 1A E1</td><td>0</td><td>Microsoft Office File</td><td>.doc .xls .ppt .msi .msg</td><td>Compound File Binary Format, a container format defined by Microsoft COM. It can contain the equivalent of files and directories. It is used by Windows Installer and for documents in older versions of Microsoft Office. It can be used by other programs as well that rely on the COM and OLE API's.</td></tr>
        /// <tr><td>7B 5C 72 74 66 31</td><td>0</td><td>Rich Text</td><td>.rtf</td><td>Rich text file format</td></tr>
        /// <tr><td>4D 53 43 46</td><td>0</td><td>Microsoft Cabinet</td><td>.cab</td><td>Microsoft cabinet file</td></tr>
        /// <tr><td>47 49 46 38 37 61</td><td>0</td><td>GIF Image (GIF87a)</td><td>.gif</td><td>Image file encoded in the Graphics Interchange Format (GIF)</td></tr>
        /// <tr><td>47 49 46 38 39 61</td><td>0</td><td>GIF Image (GIF89a)</td><td>.gif</td><td>Image file encoded in the Graphics Interchange Format (GIF)</td></tr>
        /// <tr><td>49 49 2A 00</td><td>0</td><td>TIFF Image (little-endian)</td><td>.tif .tiff</td><td>Tagged Image File Format (TIFF)</td></tr>
        /// <tr><td>4D 4D 00 2A</td><td>0</td><td>TIFF Image (big-endian)</td><td>.tif .tiff</td><td>Tagged Image File Format (TIFF)</td></tr>
        /// <tr><td>FF D8 FF DB</td><td>0</td><td>JPEG Image</td><td>.jpg .jpeg</td><td>JPEG raw or in the JFIF or Exif file format</td></tr>
        /// <tr><td>FF D8 FF E0 00 10 4A 46 49 46 00 01</td><td>0</td><td>JPEG Image</td><td>.jpg .jpeg</td><td>JPEG raw or in the JFIF or Exif file format</td></tr>
        /// <tr><td>FF D8 FF EE</td><td>0</td><td>JPEG Image</td><td>.jpg .jpeg</td><td>JPEG raw or in the JFIF or Exif file format</td></tr>
        /// <tr><td>FF D8 FF E1 ?? ?? 45 78 69 66 00 00</td><td>0</td><td>JPEG Image</td><td>.jpg .jpeg</td><td>JPEG raw or in the JFIF or Exif file format</td></tr>
        /// <tr><td>FF D8 FF E0</td><td>0</td><td>JPEG Image</td><td>.jpg</td><td>JPEG raw or in the JFIF or Exif file format</td></tr>
        /// <tr><td>00 00 00 0C 6A 50 20 20 0D 0A 87 0A</td><td>0</td><td>JPEG Image</td><td>.jp2 .j2k .jpf .jpm .jpg2 .j2c .jpc .jpx .mj2</td><td>JPEG 2000 format</td></tr>
        /// <tr><td>FF 4F FF 51</td><td>0</td><td>JPEG Image</td><td>.jp2 .j2k .jpf .jpm .jpg2 .j2c .jpc .jpx .mj2</td><td>JPEG 2000 format</td></tr>
        /// <tr><td>52 61 72 21 1A 07 00</td><td>0</td><td>RAR ZIP (v1.50)</td><td>.rar</td><td>Roshal ARchive compressed archive v1.50 onwards</td></tr>
        /// <tr><td>52 61 72 21 1A 07 01 00</td><td>0</td><td>RAR ZIP (v5.00)</td><td>.rar</td><td>Roshal ARchive compressed archive v5.00 onwards</td></tr>
        /// <tr><td>25 21 50 53</td><td>0</td><td>PS Document</td><td>.ps</td><td>PostScript document</td></tr>
        /// <tr><td>25 21 50 53 2D 41 64 6F 62 65 2D 33 2E 30 20 45 50 53 46 2D 33 2E 30</td><td>0</td><td>EPS Document (v3.0)</td><td>.eps .epsf</td><td>Encapsulated PostScript file version 3.0</td></tr>
        /// <tr><td>25 21 50 53 2D 41 64 6F 62 65 2D 33 2E 31 20 45 50 53 46 2D 33 2E 30</td><td>0</td><td>EPS Document (v3.1)</td><td>.eps .epsf</td><td>Encapsulated PostScript file version 3.1</td></tr>
        /// <tr><td>49 54 53 46 03 00 00 00 60 00 00 00</td><td>0</td><td>CHM Help</td><td>.chm</td><td>MS Windows HtmlHelp Data</td></tr>
        /// <tr><td>25 50 44 46 2D</td><td>0</td><td>PDF Document</td><td>.pdf</td><td>PDF document</td></tr>
        /// <tr><td>52 49 46 46 ?? ?? ?? ?? 57 41 56 45</td><td>0</td><td>WAV File</td><td>.wav</td><td>Waveform Audio File Format</td></tr>
        /// <tr><td>52 49 46 46 ?? ?? ?? ?? 41 56 49 20</td><td>0</td><td>AVI File</td><td>.avi</td><td>Audio Video Interleave video format</td></tr>
        /// <tr><td>FF FB</td><td>0</td><td>MP3 File</td><td>.mp3</td><td>MPEG-1 Layer 3 file without an ID3 tag or with an ID3v1 tag (which is appended at the end of the file)</td></tr>
        /// <tr><td>FF F3</td><td>0</td><td>MP3 File</td><td>.mp3</td><td>MPEG-1 Layer 3 file without an ID3 tag or with an ID3v1 tag (which is appended at the end of the file)</td></tr>
        /// <tr><td>FF F2</td><td>0</td><td>MP3 File</td><td>.mp3</td><td>MPEG-1 Layer 3 file without an ID3 tag or with an ID3v1 tag (which is appended at the end of the file)</td></tr>
        /// <tr><td>49 44 33</td><td>0</td><td>MP3 File</td><td>.mp3</td><td>MP3 file with an ID3v2 container</td></tr>
        /// <tr><td>42 4D</td><td>0</td><td>BMP Image</td><td>.bmp .dib</td><td>BMP file, a bitmap format used mostly in the Windows world</td></tr>
        /// <tr><td>1F 8B</td><td>0</td><td>GZIP File</td><td>.gz .tar.gz</td><td>GZIP compressed file</td></tr>
        /// <tr><td>FD 37 7A 58 5A 00</td><td>0</td><td>XZ File</td><td>.xz .tar.xz</td><td>XZ compression utility using LZMA2 compression</td></tr>
        /// </table>
        /// </remarks>
        /// <returns>
        /// The list of all supported default signatures.
        /// </returns>
        IEnumerable<IFileSignature> CreateDefaultSignatures();
    }
}
