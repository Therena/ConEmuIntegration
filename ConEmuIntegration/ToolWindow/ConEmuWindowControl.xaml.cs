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
using ConEmuIntegration.ConEmu;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;

namespace ConEmuIntegration.ToolWindow
{
    public partial class ConEmuWindowControl : UserControl
    {
        public ConEmuWindowControl()
        {
            this.InitializeComponent();
        }

        public void RunConEmu()
        {
            if (ProductEnvironment.Instance.CheckConEmuAndDisplay() == false)
            {
                return;
            }

            string conemu = ProductEnvironment.Instance.GetConEmuExecutable();
            string configFile = ProductEnvironment.Instance.GetConfigurationFile();
            string parameter = "-NoKeyHooks " + 
                "-InsideWnd 0x" + plnConEmu.Handle.ToString("X") + " " +
                "-LoadCfgFile \"" + configFile + "\" " +
                " -Dir \"" + Directory.GetCurrentDirectory() + "\"" +
                " -detached -cmd \"{cmd}\"";

            ProductEnvironment.Instance.ConEmuProcess = Process.Start(conemu, parameter);

            var macro = "Shell(\"new_console\", \"\", \"{cmd}\")";
            ExecuteGuiMacro(macro);
        }

        private void ExecuteGuiMacro(string macro)
        {
            if (ProductEnvironment.Instance.ConEmuProcess == null)
            {
                return;
            }

            string conemu = ProductEnvironment.Instance.GetConEmuLibrary();
            var macroHelper = new ConEmuMacro(conemu);
            macroHelper.Execute(ProductEnvironment.Instance.ConEmuProcess.Id.ToString(), macro);
        }
    }
}