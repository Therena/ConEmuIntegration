# ConEmu Integration

<img src="https://github.com/Therena/ConEmuIntegration/blob/master/Images/extension.png?raw=true" width=100>

Using the console emulator ConEmu within Visual Studio.<br />
Download this extension from the <a href="https://visualstudiogallery.msdn.microsoft.com/a0536370-40e4-4141-8f51-5f00d0434012">VS Gallery</a>

This project integrates the console emulator ConEmu in Visual Studio.<br />
ConEmu is not part of this extension and needs to be downloaded / installed separately.<br />
See the ConEmu section for further detail about the console emulator.

See [changelog](https://github.com/Therena/ConEmuIntegration/blob/master/CHANGELOG.md).

If you want to use your own ConEmu Settings or change the default settings please use your own configuration file.
To define custom Task please also use an external configuration file.
See "Optional: External Conemu configuration XML" how to do this.

## System Requirements

- Installed Visual Studio 2015 or Visual Studio 2013
- Installed or portable version of ConEmu
- ConEmu build 151201 or higher is required
- ConEmu.exe can be used in x86 as well as in x64 version
- Path to ConEmu[64].exe set in the Visual Studio settings

## ConEmu

ConEmu is a Windows console emulator which is not part of this Visual Studio extension.
Please download and install it separately. You can find them using the links below.

<img src="https://avatars0.githubusercontent.com/u/1222388?v=3&s=460" width=100>

ConEmu project: <a href="https://github.com/Maximus5/ConEmu">GitHub</a><br />
ConEmu integration: <a href="https://github.com/Maximus5/ConEmu-inside">GitHub</a> and <a href="https://www.nuget.org/packages/ConEmu.Control.WinForms/">NuGet Package</a>

## Features

- Integration of ConEmu as ToolWindow to Visual Studio
- ConEmu Tool Window can be opens from "View" menu
- Integration of ConEmu in Solution Explorer
    - Open the containing folder of the item in ConEmu
    - Open the output path of the project in ConEmu
    - Execute the from the project created binary using ConEmu
- Setting page for ConEmu and Shell settings

### Integration of ConEmu as ToolWindow to Visual Studio
This extension integrates ConEmu as tool window in Visual Studio.<br/>

![](https://github.com/Therena/ConEmuIntegration/blob/master/Images/ConEmuVisualStudio.png?raw=true)

### ConEmu Tool Window can be opens from "View" menu
The ConEmu tool window can be open from the view menu of Visual Studio<br/>
![](https://github.com/Therena/ConEmuIntegration/blob/master/Images/ComEmuInViewMenu.png?raw=true)

### Integration of ConEmu in Solution Explorer
The current opened folder in ConEmu can be changed directly from solution explorer.<br/>
The output file of your project can be also executed in ConEmu.<br/>
![](https://github.com/Therena/ConEmuIntegration/blob/master/Images/ConEmuSolutionExplorer.png?raw=true)

## Settings

### Path settings
Please download / install the ConEmu separately. It isn't part of this extension.
After you have downloaded / installed ConEmu please set the paths in the settings of this extension.
Therefor open the settings of Visual Studio and navigate to the "ConEmu Integration" section.

Please set the path to the ConEmu executable file in the section of the configuration:<br/>
![](https://github.com/Therena/ConEmuIntegration/blob/master/Images/SettingsConEmuExe.png?raw=true)
E.g.: C:\Users\David Roller\Downloads\ConEmuPack.160504\ConEmu.exe<br/>

### ConEmu settings
Visual Studio settings page to make it possible to change settings of ConEmu.

#### Change the default ConEmu task
Using this configuration it is possible to change the startup or default task which is used by the embedded ConEmu<br/>
The default setting for this is {cmd}.<br/>
![](https://github.com/Therena/ConEmuIntegration/blob/master/Images/SettingsConEmuShell.png?raw=true)

#### Optional: External Conemu configuration XML
The extension contains an internal configuration which isn't changeable.
If you want to change this configuration or use an custom configuration please set the path to your configuration 
in the Visual Studio settings.<br/>
![](https://github.com/Therena/ConEmuIntegration/blob/master/Images/SettingsConEmuConfig.png?raw=true)
E.g.: C:\Users\David Roller\AppData\Roaming\ConEmu.xml<br/>

Please keep in mind that the extension doesn't support all configuration settings of ConEmu.
Don't using the internal configuration could cause incompatibilities. 

## Contribute
Check out the [contribution guidelines](https://github.com/Therena/ConEmuIntegration/blob/master/CONTRIBUTING.md)
if you want to contribute to this project.

For cloning and building this project yourself, make sure
to install the
[Extensibility Tools 2015](https://visualstudiogallery.msdn.microsoft.com/ab39a092-1343-46e2-b0f1-6a3f91155aa6)
extension for Visual Studio which enables some features
used by this project.

## License
[Apache 2.0](https://github.com/Therena/ConEmuIntegration/blob/master/LICENSE)

ConEmu and ConEmu Inside has different licenses.<br />
Please have a look on their Github pages for the license details.
