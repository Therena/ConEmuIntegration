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
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace ConEmuIntegration
{
    public partial class ConEmuWindowControl : UserControl
    {
        private Process m_ConEmuProcess;
        private List<string> m_SearchPaths;

        private FileInfo m_ConfigFile;

        public ConEmuWindowControl()
        {
            this.InitializeComponent();

            m_ConfigFile = new FileInfo(ExportConfiguration());

            m_SearchPaths = new List<string>();
            m_SearchPaths.Add(Directory.GetCurrentDirectory());
            m_SearchPaths.AddRange(Environment.ExpandEnvironmentVariables("%PATH%").Split(';'));
            m_SearchPaths.Add(@"C:\Users\David Roller\Downloads\conemu-inside-master\conemu-inside-master\ConEmuInside\bin\Debug\ConEmu");
        }

        public void Cleanup()
        {
            if(m_ConfigFile.Exists)
            {
                m_ConfigFile.Delete();
            }
        }

        private string ExportConfiguration()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var configFile = "ConEmuIntegration.Settings.ConEmu.xml";

            var configFileContent = "";
            using (var stream = new StreamReader(assembly.GetManifestResourceStream(configFile)))
            {
                configFileContent = stream.ReadToEnd();
            }

            string configFilePath = Path.GetTempFileName() + ".xml";
            File.WriteAllText(configFilePath, configFileContent);

            return configFilePath;
        }

        public void RunConEmu()
        {
            string conemu = ExtendPath("ConEmu.exe");
            string parameter = "-NoKeyHooks " + 
                "-InsideWnd 0x" + plnConEmu.Handle.ToString("X") + " " +
                "-LoadCfgFile \"" + m_ConfigFile.FullName + "\" " +
                " -Dir \"" + Directory.GetCurrentDirectory() + "\"" +
                " -detached -cmd \"{cmd}\"";
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

            var macro = "Shell(\"new_console\", \"\", \"{cmd}\")";
            ExecuteGuiMacro(macro);
        }

        private string ExtendPath(string file)
        {
            foreach (var path in m_SearchPaths)
            {
                FileInfo fileInfo = new FileInfo(Path.Combine(path, file));
                if(fileInfo.Exists)
                {
                    return fileInfo.FullName;
                }
            }
            return file;
        }

        private void ExecuteGuiMacro(string macro)
        {
            string conemu = ExtendPath("ConEmuCD.dll");
            var macroHelper = new ConEmuMacro(conemu);
            macroHelper.Execute(m_ConEmuProcess.Id.ToString(), macro);
        }
    }
}