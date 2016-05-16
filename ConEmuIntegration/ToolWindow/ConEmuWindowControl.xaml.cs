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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConEmuIntegration.ToolWindow
{
    public partial class ConEmuWindowControl : UserControl
    {
        public event EventHandler ConEmuClosed;

        public ConEmuWindowControl()
        {
            this.InitializeComponent();
        }

        public bool RunConEmu()
        {
            try
            {
                if(ProductEnvironment.Instance.ConEmuProcess != null)
                {
                    if(ProductEnvironment.Instance.ConEmuProcess.HasExited != false)
                    {
                        return true;
                    }
                }

                if (ProductEnvironment.Instance.CheckConEmuAndDisplay() == false)
                {
                    return false;
                }

                string conemu = ProductEnvironment.Instance.GetConEmuExecutable();
                string configFile = ProductEnvironment.Instance.GetConfigurationFile();
                string shell = ProductEnvironment.Instance.ShellTypeToString(
                    ProductEnvironment.Instance.GetShellType());

                string parameter = "-NoKeyHooks -Multi -NoCloseConfirm -NoQuake " +
                    "-InsideWnd 0x" + pnlConEmu.Handle.ToString("X") + " " +
                    "-LoadCfgFile \"" + configFile + "\" ";

                ProductEnvironment.Instance.ConEmuProcess = Process.Start(conemu, parameter);
                ProductEnvironment.Instance.ConEmuProcess.EnableRaisingEvents = true;
                ProductEnvironment.Instance.ConEmuProcess.Exited += ConEmuProcess_Exited;

                int cbMultiShowButtons = 2549;
                ExecuteGuiMacro("SetOption(\"Check\", " + cbMultiShowButtons + ", 0)");
                ExecuteGuiMacro("SetOption(\"Check\", " + cbMultiShowButtons + ", 1)");

                return true;
            }
            catch (Exception error)
            {
                ExceptionMessageBox box = new ExceptionMessageBox();
                box.SetException(error);
                box.ShowDialog();
            }
            return false;
        }

        private void ConEmuProcess_Exited(object sender, EventArgs e)
        {
            this.ConEmuClosed?.Invoke(this, new EventArgs());
        }

        public void FocusConEmu()
        {

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