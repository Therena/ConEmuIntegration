# ConEmu Integration

![ConEmu Integration Icon](https://github.com/Therena/ConEmuIntegration/blob/master/Images/extension.png?raw=true)

Using the console emulator ConEmu within Visual Studio.

![CodeQL](https://github.com/Therena/ConEmuIntegration/actions/workflows/codeql-analysis.yml/badge.svg)

Download this extension from the Visual Studio Marketplace
- Visual Studio 2015 - Visual Studio 2017: [ConEmu Integration 2017](https://marketplace.visualstudio.com/items?itemName=DavidRoller.ConEmuIntegration)
- Visual Studio 2019: [ConEmu Integration 2019](https://marketplace.visualstudio.com/items?itemName=DavidRoller.ConEmuIntegration2019)
- Visual Studio 2022: [ConEmu Integration 2022](https://marketplace.visualstudio.com/items?itemName=DavidRoller.ConEmuIntegration2022)

This project integrates the console emulator ConEmu in Visual Studio.
ConEmu is not part of this extension and needs to be downloaded / installed separately.
See the ConEmu section for further detail about the console emulator.

If you want to use your own ConEmu Settings or change the default settings please use your own configuration file.
To define custom Task please also use an external configuration file.
See "Optional: External ConEmu configuration XML" how to do this.

## Supported languages

- en-US American English
- de-DE Standard German

## System Requirements

- Installed Visual Studio (best: newest and updated - min. 2015)
- Installed or portable version of ConEmu
- ConEmu build 151201 or higher is required
- ConEmu.exe can be used in x86 as well as in x64 version
- Path to ConEmu[64].exe set in the Visual Studio settings

## ConEmu

ConEmu is a Windows console emulator which is not part of this Visual Studio extension. Please download and install it separately. You can find them using the links below.

<img src="https://avatars0.githubusercontent.com/u/1222388?v=3&s=460" width=100>

ConEmu project: [GitHub](https://github.com/Maximus5/ConEmu)

ConEmu integration: [GitHub](https://github.com/Maximus5/ConEmu-inside)
and [NuGet Package](https://www.nuget.org/packages/ConEmu.Control.WinForms/)

## Features

- Integration of ConEmu as ToolWindow to Visual Studio
- ConEmu Tool Window can be opens from "View" menu
- Integration of ConEmu in Solution Explorer
  - Open the containing folder of the item in ConEmu
  - Open the output path of the project in ConEmu
  - Execute the from the project created binary using ConEmu
  - Integration in the folder view mode of solution explorer
- Setting page for ConEmu and Shell settings

### Integration of ConEmu as ToolWindow to Visual Studio

This extension integrates ConEmu as tool window in Visual Studio.

![ConEmu integrated in Visual Studio](https://github.com/Therena/ConEmuIntegration/blob/master/Images/ConEmuVisualStudio.png?raw=true)

### ConEmu Tool Window can be opens from "View" menu

The ConEmu tool window can be open from the view menu of Visual Studio

![Open ConEmu integrated in Visual Studio](https://github.com/Therena/ConEmuIntegration/blob/master/Images/ComEmuInViewMenu.png?raw=true)

### Integration of ConEmu in Solution Explorer

The current opened folder in ConEmu can be changed directly from solution explorer.
The output file of your project can be also executed in ConEmu.

![ConEmu integration in solution explorer](https://github.com/Therena/ConEmuIntegration/blob/master/Images/ConEmuSolutionExplorer.png?raw=true)

### Integration in the folder view mode of solution explorer

The current opened folder in ConEmu can be changed directly from solution explorer in folder view mode.

![ConEmu integration in folder view mode](https://github.com/Therena/ConEmuIntegration/blob/master/Images/ConEmuFolderView.png?raw=true)

## Settings

![ConEmu executable path in Visual Studio settings](https://github.com/Therena/ConEmuIntegration/blob/master/Images/SettingsConEmu.png?raw=true)

### Path settings

Please download / install the ConEmu separately. It isn't part of this extension.
After you have downloaded / installed ConEmu please set the paths in the settings of this extension.
Therefor open the settings of Visual Studio and navigate to the "ConEmu Integration" section.

### Optional: ConEmu settings

Visual Studio settings page to make it possible to change settings of ConEmu.

#### Change the default ConEmu task

Using this configuration it is possible to change the startup or default task which is used by the embedded ConEmu
The default setting for this is {cmd}.

#### External ConEmu configuration XML

The extension contains an internal configuration which isn't changeable.
If you want to change this configuration or use an custom configuration please set the path to your configuration in the Visual Studio settings.

Please keep in mind that the extension doesn't support all configuration settings of ConEmu.
Don't using the internal configuration could cause incompatibilities.

## License

[Apache 2.0](https://github.com/Therena/ConEmuIntegration/blob/master/LICENSE)

ConEmu and ConEmu Inside has different licenses.
Please have a look on their Github pages for the license details.
