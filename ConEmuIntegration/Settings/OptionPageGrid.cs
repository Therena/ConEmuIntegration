//
// Copyright 2019 David Roller 
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
using System;
using System.ComponentModel;
using System.IO;

namespace ConEmuIntegration.Settings
{
    internal sealed class OptionPageGrid : DialogPage
    {
        private string m_ConEmuPath = "";

        [Category("ConEmu Integration")]
        [DisplayName("Executable")]
        [Description("Path to the ConEmu executable (ConEmu.exe) file")]
        public string ConEmuPath
        {
            get
            {
                return m_ConEmuPath;
            }
            set
            {
                ValidateTheConemuPath(value);
                m_ConEmuPath = value;
            }
        }

        private void ValidateTheConemuPath(string value)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    return;
                }

                var file = new FileInfo(value);
                if (file.Exists == false)
                {
                    throw new Exception("The given ConEmu executable doesn't exists");
                }

                if (file.Name.ToUpper() != "CONEMU.EXE" && file.Name.ToUpper() != "CONEMU64.EXE")
                {
                    throw new Exception("The given path might not be the correct ConEmu executable. " +
                        "Please use \"ConEmu.exe\" or \"ConEmu64.exe\" for this configuration");
                }
            }
            catch(Exception error)
            {
                string message = "ConEmu Integration: Executable configuration not valid: " + error.Message;
                throw new Exception(message, error);
            }
        }
    }
}
