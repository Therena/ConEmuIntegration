//
// Copyright 2016 David Roller
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
    internal sealed class OptionPageGrid : DialogPage
    {
        [Category("ConEmu Integration")]
        [DisplayName("Executable")]
        [Description("Path to the ConEmu executable (ConEmu[64].exe) file")]
        public string ConEmuPath { get; set; }

        [Category("ConEmu Integration")]
        [DisplayName("Library")]
        [Description("Path to the ConEmu library (ConEmuCD[64].dll) file")]
        public string ConEmuLibraryPath { get; set; }
    }
}
