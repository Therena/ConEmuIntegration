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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ConEmuIntegration
{
    public partial class ConEmuWindowControl : UserControl
    {
        private Process m_ConEmuProcess;
        private List<string> m_SearchPaths;

        public ConEmuWindowControl()
        {
            this.InitializeComponent();

            m_SearchPaths = new List<string>();
            m_SearchPaths.Add(Directory.GetCurrentDirectory());
            m_SearchPaths.AddRange(Environment.ExpandEnvironmentVariables("%PATH%").Split(';'));
            m_SearchPaths.Add(@"C:\Users\David Roller\Downloads\conemu-inside-master\conemu-inside-master\ConEmuInside\bin\Debug\ConEmu");
        }

        public void RunConEmu()
        {
            string conemu = GetConEmu();
            string parameter = "-NoKeyHooks " + 
                "-InsideWnd 0x" + plnConEmu.Handle.ToString("X") + " " +
                "-LoadCfgFile \"" + @"C:\SRC\GitHub\ConEmuIntegration\ConEmuIntegration\Settings\ConEmu.xml" + "\" " +
                " -Dir \"" + Directory.GetCurrentDirectory() + "\"" +
                " -detached";
            try
            {
                m_ConEmuProcess = Process.Start(conemu, parameter);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + Environment.NewLine +
                    "Command:" + Environment.NewLine + conemu + Environment.NewLine + 
                    Environment.NewLine + ex.GetType().FullName + 
                    " (" + ex.NativeErrorCode.ToString() + ")",
                    "ConEmu Command Explorer", MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                throw;
            }
        }

        private string GetConEmu()
        {
            foreach (var path in m_SearchPaths)
            {
                FileInfo file = new FileInfo(Path.Combine(path, "ConEmu.exe"));
                if(file.Exists)
                {
                    return file.FullName;
                }

                FileInfo file64 = new FileInfo(Path.Combine(path, "ConEmu64.exe"));
                if (file64.Exists)
                {
                    return file64.FullName;
                }
            }
            return "ConEmu.exe";
        }
    }
}