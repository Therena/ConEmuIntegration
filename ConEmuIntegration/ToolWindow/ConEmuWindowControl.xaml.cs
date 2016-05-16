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
using ConEmu.WinForms;
using ConEmuIntegration.ConEmuProduct;
using System;
using System.Windows.Controls;
using System.Xml;

namespace ConEmuIntegration.ToolWindow
{
    public partial class ConEmuWindowControl : UserControl
    {
        public event EventHandler ConEmuClosed;
        private bool m_HasExited = false;

        public ConEmuWindowControl()
        {
            this.InitializeComponent();
        }

        public bool RunConEmu()
        {
            try
            {
                if(ProductEnvironment.Instance.ConEmu == null || m_HasExited)
                {
                    RunConEmuSession();
                }

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

        private void RunConEmuSession()
        {
            ProductEnvironment.Instance.ConEmu = new ConEmuControl();
            ProductEnvironment.Instance.ConEmu.Dock = System.Windows.Forms.DockStyle.Fill;

            var info = new ConEmu.WinForms.ConEmuStartInfo();
            info.ConEmuExecutablePath = ProductEnvironment.Instance.GetConEmuExecutable();

            info.BaseConfiguration = new XmlDocument();
            info.BaseConfiguration.Load(ProductEnvironment.Instance.GetConfigurationFile());

            info.ConsoleProcessExitedEventSink = ConEmuProcess_Exited;

            ProductEnvironment.Instance.ConEmu.AutoStartInfo = info;

            wfhConEmu.Child = ProductEnvironment.Instance.ConEmu;

            m_HasExited = false;
        }

        public void FocusConEmu()
        {
            ProductEnvironment.Instance.ConEmu.Focus();
        }

        private void ConEmuProcess_Exited(object sender, ConsoleProcessExitedEventArgs e)
        {
            this.ConEmuClosed?.Invoke(this, new EventArgs());
            m_HasExited = true;
        }

        private void ExecuteGuiMacro(string macro)
        {
            if (ProductEnvironment.Instance.ConEmu.IsConsoleEmulatorOpen == false)
            {
                return;
            }
            ProductEnvironment.Instance.ConEmu.RunningSession.ExecuteGuiMacroTextSync(macro);
        }
    }
}