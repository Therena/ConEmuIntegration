//
// Copyright 2020 David Roller 
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//  http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
using Microsoft.VisualStudio.Shell;
using System.ComponentModel;

namespace ConEmuIntegration.Settings
{
    internal sealed class OptionPageGridConEmuSettings : DialogPage
    {
        [Category("ConEmu Integration")]
        [DisplayName("Default Task")]
        [Description("Task which should be used as default (Change may require restart of Visual Studio)")]
        public string DefaultTask { get; set; }

        [Category("ConEmu Integration")]
        [DisplayName("XML configuration")]
        [Description("[Optional] Use an external configuration file (ConEmu.xml) for integrated conemu " + 
            "(Change may require restart of Visual Studio)")]
        public string ConEmuXML { get; set; }
    }
}
