<p align="center">
  <a href="https://github.com/akesseler/Plexdata.FileSignature.Analyzer/blob/master/LICENSE.md" alt="license">
    <img src="https://img.shields.io/github/license/akesseler/Plexdata.FileSignature.Analyzer.svg" />
  </a>
  <a href="https://github.com/akesseler/Plexdata.FileSignature.Analyzer/releases/latest" alt="latest">
    <img src="https://img.shields.io/github/release/akesseler/Plexdata.FileSignature.Analyzer.svg" />
  </a>
  <a href="https://github.com/akesseler/Plexdata.FileSignature.Analyzer/archive/master.zip" alt="master">
    <img src="https://img.shields.io/github/languages/code-size/akesseler/Plexdata.FileSignature.Analyzer.svg" />
  </a>
  <a href="https://github.com/akesseler/Plexdata.FileSignature.Analyzer/wiki" alt="wiki">
    <img src="https://img.shields.io/badge/wiki-API-orange.svg" />
  </a>
</p>

# Plexdata File Signature Analyzer

The idea is to have a tool that checks the file content for a particular byte sequence to verify 
that a file extension fits the actual file type. The _Plexdata File Signature Analyzer_ represents 
such a utility that analyzes binary signatures of any file type.

## Table of Contents

1. [Licensing](#licensing)
1. [Features](#features)
1. [Examples](#examples)
   1. [Simple Usage](#simple-usage)
   1. [Signature Configuration](#signature-configuration)
   1. [Using Placeholders](#using-placeholders)
   1. [Dependency Injection](#dependency-injection)
1. [Default Signatures](#default-signatures)
1. [Documentation](#documentation)
1. [Framework](#framework)
1. [Downloads](#downloads)
1. [Known Issues](#known-issues)
1. [Visions](#visions)

## Licensing <a name="licensing"></a>

The software has been published under the terms of _MIT License_.

## Features <a name="features"></a>

Main feature of this library is that users are able to define their own signature setups. But this 
library provides a set of [Default Signatures](#default-signatures) as well.

Another feature is the possibility to use signature placeholders. This might be very useful in cases 
when only a portion of a byte sequence describes a file signature. An example of such use case is for 
instance a version number within a file signature. Such a version number might be independent of a 
particular file signature and can vary from file to file. More information are available in section 
[Using Placeholders](#using-placeholders).

Furthermore, files with _Byte Order Mark_ (BOM) are also supported. This means each of following BOM 
sequences is skipped automatically during file analyzation. See list below for a summary of currently 
supported BOM bytes.

|Bytes|Name|
|---|---|
|EF BB BF|UTF-8|
|FF FE|UTF-16 LE|
|FE FF|UTF-16 BE|
|FF FE 00 00|UTF-32 LE|
|00 00 FE FF|UTF-32 BE|

Good to know, dependency injection via interface `IServiceCollection` (available in 
`Microsoft.Extensions.DependencyInjection`) is supported as well. How to use dependency injection is 
explained in section [Dependency Injection](#dependency-injection).

Unfortunately, only one signature setup with just one single offset is supported per file type. But 
this might change in future versions. See also section [Visions](#visions) for planned extensions.

## Examples <a name="examples"></a>

### Simple Usage <a name="simple-usage"></a>

Example below shows how to use the _Plexdata File Signature Analyzer_ together with all predefined 
default signature setups.

```
private void Run()
{
    Console.Write("Input a fully qualified file name: ");
    String filename = Console.ReadLine();

    IEnumerable<IAnalyzerResult> result = this.analyzer.Analyze(filename, this.factory.CreateDefaultSignatures());

    Console.WriteLine("The analysis result is:");
    Console.WriteLine(String.Join(Environment.NewLine, result.Select(x => x.ToString())));
    Console.ReadKey();
}
```

As shown above, the _Plexdata File Signature Analyzer_ just uses the list of default signatures by 
getting them from `IFileSignatureFactory`. See also section [Default Signatures](#default-signatures) 
for more information about supported default signatures.

### Signature Configuration <a name="signature-configuration"></a>

In this example is shown how to define an own signature list as well as how to use it together with 
the _Plexdata File Signature Analyzer_. 

```
private void Run()
{
    Console.Write("Input a fully qualified file name: ");
    String filename = Console.ReadLine();

    IEnumerable<IFileSignature> signatures = new List<FileSignature>()
    {
        new FileSignature()
        {
            Name       = "Executable",
            Remarks    = "DOS MZ executable and its descendants (including NE and PE)",
            Extensions = "exe,scr,sys,dll,fon,cpl,iec,ime,rs,tsp,mz",
            Signature  = "4D 5A",
        }
    };

    IEnumerable<IAnalyzerResult> result = this.analyzer.Analyze(filename, signatures);

    Console.WriteLine("The analysis result is:");
    Console.WriteLine(String.Join(Environment.NewLine, result.Select(x => x.ToString())));
    Console.ReadKey();
}
```

The only thing to do is to create an own list of instances of `IFileSignature` and to provide it as 
parameter of method call `Analyze()`. Such a user defined signature list could come from a configuration 
file, for example.

### Using Placeholders <a name="using-placeholders"></a>

_Placeholder_ means that a portion of a file signature shall be ignored during file analyzation. For 
this purpose such a file signature includes the wildcard _Question Mark_ (`?`), either for a complete 
byte or just for one nibble. See below how to use signature placeholders.

Signature with one byte to ignore.

```
Signature = "FF D8 FF E1 ?? 42 45 78 69 66 00 00"
```

Signature with two bytes to ignore.

```
Signature = "FF D8 FF E1 ?? ?? 45 78 69 66 00 00"
```

Signature with two bytes to ignore at different positions.

```
Signature = "FF D8 FF E1 ?? 45 78 69 66 00 ?? 00"
```

Signature with one upper nibble to ignore.

```
Signature = "FF D8 FF E1 ?0 42 45 78 69 66 00 00"
```

Signature with one lower nibble to ignore.

```
Signature = "FF D8 FF E1 0? 42 45 78 69 66 00 00"
```

For sure, a signature like `"?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ??"` does not make any sense, except 
you want to check that the length of a file is at least _n_ bytes.

### Dependency Injection <a name="dependency-injection"></a>

In this section it is wanted to show how the _Plexdata File Signature Analyzer_ can be used together 
with _Dependency Injection_. The example below uses an instance of `IServiceCollection` as dependency 
container as well as a typical container registration extension method. 

```
class Program
{
    static void Main(String[] _)
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-US");
        Console.OutputEncoding = Encoding.UTF8;

        IServiceCollection services = new ServiceCollection();

        services.AddTransient<Program, Program>();
        services.RegisterSignatureAnalyzer();

        IServiceProvider provider = services.BuildServiceProvider();

        provider.GetService<Program>().Run();
    }

    private readonly IFileSignatureAnalyzer analyzer;
    private readonly IFileSignatureFactory factory;

    public Program(IFileSignatureFactory factory, IFileSignatureAnalyzer analyzer)
    {
        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        this.analyzer = analyzer ?? throw new ArgumentNullException(nameof(analyzer));
    }

    private void Run()
    {
        // The doing..
    }
}
```

As show above, first of all an instance of `IServiceCollection` is created and class `Program` is 
registered as _Transient_. Thereafter, extension method `RegisterSignatureAnalyzer()` is called which 
automatically does the registration of all interfaces of the _Plexdata File Signature Analyzer_. 
Finally, class `Program` is instantiated and executed just by calling `provider.GetService<Program>().Run()`. 
The rest is done automatically by the container injector.

## Default Signatures <a name="default-signatures"></a>

In this section please find the list of all default file signatures supported at the moment. Please 
note, the _Plexdata File Signature Analyzer_ supports pretty common file types only. More information 
about other file signatures can be found on the _Internet_ as well as on 
[Wikipedia](https://en.wikipedia.org/wiki/List_of_file_signatures).

|Signature|Offset|Name|Extensions|Remarks|
|---|---|---|---|---|
|4D 5A|0|Executable|.exe .scr .sys .dll .fon .cpl .iec .ime .rs .tsp .mz|DOS MZ executable and its descendants (including NE and PE)|
|89 50 4E 47 0D 0A 1A 0A|0|PNG Image|.png|Image encoded in the Portable Network Graphics format|
|37 7A BC AF 27 1C|0|7 Zip|.7z|7 Zip file format|
|3C 3F 78 6D 6C 20|0|XML (UTF-8)|.xml|XML file format (UTF-8)|
|3C 00 3F 00 78 00 6D 00 6C 00 20 00|0|XML (UTF-16 LE)|.xml|XML file format (UTF-16 LE)|
|00 3C 00 3F 00 78 00 6D 00 6C 00 20|0|XML (UTF-16 BE)|.xml|XML file format (UTF-16 BE)|
|50 4B 03 04|0|PK ZIP|.zip .aar .apk .docx .epub .ipa .jar .kmz .maff .msix .odp .ods .odt .pk3 .pk4 .pptx .usdz .vsdx .xlsx .xpi|Zip file format and formats based on it, such as EPUB, JAR, ODF, OOXML|
|50 4B 05 06|0|PK ZIP (empty archive)|.zip .aar .apk .docx .epub .ipa .jar .kmz .maff .msix .odp .ods .odt .pk3 .pk4 .pptx .usdz .vsdx .xlsx .xpi|Zip file format and formats based on it, such as EPUB, JAR, ODF, OOXML|
|50 4B 07 08|0|PK ZIP  (spanned archive)|.zip .aar .apk .docx .epub .ipa .jar .kmz .maff .msix .odp .ods .odt .pk3 .pk4 .pptx .usdz .vsdx .xlsx .xpi|Zip file format and formats based on it, such as EPUB, JAR, ODF, OOXML|
|53 51 4C 69 74 65 20 66 6F 72 6D 61 74 20 33 00|0|SQLite Database|.sqlitedb .sqlite .db|SQLite database file|
|00 00 01 00|0|Icon File|.ico|Computer icon encoded in ICO file format|
|1F 9D|0|TAR ZIP|.z .tar.z|Compressed file (often tar zip) using Lempel-Ziv-Welch algorithm|
|1F A0|0|TAR ZIP|.z .tar.z|Compressed file (often tar zip) using LZH algorithm|
|42 5A 68|0|BZIP2|.bz2|Compressed file using Bzip2 algorithm|
|75 73 74 61 72 00 30 30|257|TAR ZIP (POSIX)|.tar|Compressed file tar archive|
|75 73 74 61 72 20 20 00|257|TAR ZIP (GNU)|.tar|Compressed file tar archive|
|D0 CF 11 E0 A1 B1 1A E1|0|Microsoft Office File|.doc .xls .ppt .msi .msg|Compound File Binary Format, a container format defined by Microsoft COM. It can contain the equivalent of files and directories. It is used by Windows Installer and for documents in older versions of Microsoft Office. It can be used by other programs as well that rely on the COM and OLE API's.|
|7B 5C 72 74 66 31|0|Rich Text|.rtf|Rich text file format|
|4D 53 43 46|0|Microsoft Cabinet|.cab|Microsoft cabinet file|
|47 49 46 38 37 61|0|GIF Image (GIF87a)|.gif|Image file encoded in the Graphics Interchange Format (GIF)|
|47 49 46 38 39 61|0|GIF Image (GIF89a)|.gif|Image file encoded in the Graphics Interchange Format (GIF)|
|49 49 2A 00|0|TIFF Image (little-endian)|.tif .tiff|Tagged Image File Format (TIFF)|
|4D 4D 00 2A|0|TIFF Image (big-endian)|.tif .tiff|Tagged Image File Format (TIFF)|
|FF D8 FF DB|0|JPEG Image|.jpg .jpeg|JPEG raw or in the JFIF or Exif file format|
|FF D8 FF E0 00 10 4A 46 49 46 00 01|0|JPEG Image|.jpg .jpeg|JPEG raw or in the JFIF or Exif file format|
|FF D8 FF EE|0|JPEG Image|.jpg .jpeg|JPEG raw or in the JFIF or Exif file format|
|FF D8 FF E1 ?? ?? 45 78 69 66 00 00|0|JPEG Image|.jpg .jpeg|JPEG raw or in the JFIF or Exif file format|
|FF D8 FF E0|0|JPEG Image|.jpg|JPEG raw or in the JFIF or Exif file format|
|00 00 00 0C 6A 50 20 20 0D 0A 87 0A|0|JPEG Image|.jp2 .j2k .jpf .jpm .jpg2 .j2c .jpc .jpx .mj2|JPEG 2000 format|
|FF 4F FF 51|0|JPEG Image|.jp2 .j2k .jpf .jpm .jpg2 .j2c .jpc .jpx .mj2|JPEG 2000 format|
|52 61 72 21 1A 07 00|0|RAR ZIP (v1.50)|.rar|Roshal ARchive compressed archive v1.50 onwards|
|52 61 72 21 1A 07 01 00|0|RAR ZIP (v5.00)|.rar|Roshal ARchive compressed archive v5.00 onwards|
|25 21 50 53|0|PS Document|.ps|PostScript document|
|25 21 50 53 2D 41 64 6F 62 65 2D 33 2E 30 20 45 50 53 46 2D 33 2E 30|0|EPS Document (v3.0)|.eps .epsf|Encapsulated PostScript file version 3.0|
|25 21 50 53 2D 41 64 6F 62 65 2D 33 2E 31 20 45 50 53 46 2D 33 2E 30|0|EPS Document (v3.1)|.eps .epsf|Encapsulated PostScript file version 3.1|
|49 54 53 46 03 00 00 00 60 00 00 00|0|CHM Help|.chm|MS Windows HtmlHelp Data|
|25 50 44 46 2D|0|PDF Document|.pdf|PDF document|
|52 49 46 46 ?? ?? ?? ?? 57 41 56 45|0|WAV File|.wav|Waveform Audio File Format|
|52 49 46 46 ?? ?? ?? ?? 41 56 49 20|0|AVI File|.avi|Audio Video Interleave video format|
|FF FB|0|MP3 File|.mp3|MPEG-1 Layer 3 file without an ID3 tag or with an ID3v1 tag (which is appended at the end of the file)|
|FF F3|0|MP3 File|.mp3|MPEG-1 Layer 3 file without an ID3 tag or with an ID3v1 tag (which is appended at the end of the file)|
|FF F2|0|MP3 File|.mp3|MPEG-1 Layer 3 file without an ID3 tag or with an ID3v1 tag (which is appended at the end of the file)|
|49 44 33|0|MP3 File|.mp3|MP3 file with an ID3v2 container|
|42 4D|0|BMP Image|.bmp .dib|BMP file, a bitmap format used mostly in the Windows world|
|1F 8B|0|GZIP File|.gz .tar.gz|GZIP compressed file|
|FD 37 7A 58 5A 00|0|XZ File|.xz .tar.xz|XZ compression utility using LZMA2 compression|

## Documentation <a name="documentation"></a>

The full API documentation is available as Wiki and can be read under [https://github.com/akesseler/Plexdata.FileSignature.Analyzer/wiki](https://github.com/akesseler/Plexdata.FileSignature.Analyzer/wiki).

## Framework <a name="framework"></a>

Current target framework of this library is the _.NET Standard v2.0_.

## Downloads <a name="downloads"></a>

The latest release can be obtained from [https://github.com/akesseler/Plexdata.FileSignature.Analyzer/releases/latest](https://github.com/akesseler/Plexdata.FileSignature.Analyzer/releases/latest).

The main branch can be downloaded as ZIP from [https://github.com/akesseler/Plexdata.FileSignature.Analyzer/archive/master.zip](https://github.com/akesseler/Plexdata.FileSignature.Analyzer/archive/master.zip).

## Known Issues <a name="known-issues"></a>

There are some exception that are not (yet) supported. These exceptions are, for example, searching from 
the back, several alternative signatures, multiple offsets within same file, respecting byte endianness, 
and maybe more.

## Visions <a name="visions"></a>

Functions respectively features listed below might be available in later versions.

* Support of multiple signatures per file type.
* Support of multiple offsets per file type.
* Combination of multiple signatures with multiple offsets.
  * Combine them with an AND or an OR operation.
* Finding a file signature at the end of file.
* Finding repeated file signatures at every n'th byte offset.


