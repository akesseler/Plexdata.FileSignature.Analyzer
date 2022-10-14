## Overview

The help file project named `FileSignatureAnalyzer.wiki.shfbproj` has been created using [Sandcastle Help File Builder](https://ewsoftware.github.io/SHFB/html/bd1ddb51-1c4f-434f-bb1a-ce2135d3a909.htm) version v2020.3.6.0.

## Building the Help

Usually, there should be no need to change this file because of the help is released as Wiki. But if you like, you can download _Sandcastle Help File Builder_ from [https://github.com/EWSoftware/SHFB/releases](https://github.com/EWSoftware/SHFB/releases) and modify the project help fitting your own needs.

For example you may like to create an HTML version of the project API documentation. In such a case just download and install the _Sandcastle Help File Builder_ as mentioned above. Then follow the steps below:

- As first, you should create a copy of the help file project `FileSignatureAnalyzer.wiki.shfbproj` and rename it, for instance into `FileSignatureAnalyzer.html.shfbproj`. 
- As next, open this new file with the _Sandcastle Help File Builder_.
- Thereafter, show tab _Project Properties_ and move to section _Build_. 
- Then choose _VS2013_ from _Presentation style_ combo-box.
- Now un-tick the _HTML Help 1 (chm)_ setting and tick setting _Website (HTML/ASP.NET)_ instead.
- Then you should change the output path accordingly. For this purpose move to section _Paths_ and modify the value of box _Help content output path_. For example you could change this path into `html\`. 
- Finally, rebuild the whole help.

After a successful build you will find the result inside the project file’s sub-folder you named above.

### Additional Conditions

Ensure the utilities _Microsoft Build Tools 2017_ (and perhaps _HTML Help Workshop_ as well) are installed on your system. 
If not, you should download and install them. Below please find the download links of these programs.

* _MS Build Tools 2017_: See section **Trouble Shooting**
* _HTML Help Workshop_: https://www.microsoft.com/en-us/download/details.aspx?id=21138

## Trouble Shooting

Unfortunately, the latest versions of _Sandcastle_ do no longer support _MS Build Tools 2015_ (which easily 
were installed by downloading and executing the setup from https://www.microsoft.com/en-us/download/details.aspx?id=48159). 

On the other hand, there should be no problem with _Sandcastle_ when using it together with _Visual Studio 2017_.

But when _Visual Studio 2019_ is used exclusively, then _Sandcastle_ will raise an error message that _The 
tools version "15.0" is unrecognized._ And this is not a problem of a wrong configuration. As workaround, 
only install _MSBuild_ (and the Roslyn-Compiler) from _Visual Studio 2017_ and the problem should be fixed.
