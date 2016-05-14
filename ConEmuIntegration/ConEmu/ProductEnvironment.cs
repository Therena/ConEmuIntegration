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
using ConEmuIntegration.Settings;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using static ConEmuIntegration.Settings.OptionPageGridConEmu;

namespace ConEmuIntegration.ConEmu
{
    internal class ProductEnvironment
    {
        private List<string> m_TempFiles;
        private List<string> m_SearchPaths;

        public Package Package { get; set; }
        public Process ConEmuProcess { get; set; }

        private static ProductEnvironment m_Instance = new ProductEnvironment();
        public static ProductEnvironment Instance
        {
            get
            {
                return m_Instance;
            }
        }

        private ProductEnvironment()
        {
            m_TempFiles = new List<string>();

            m_SearchPaths = new List<string>();
            m_SearchPaths.Add(Directory.GetCurrentDirectory());
            m_SearchPaths.AddRange(Environment.ExpandEnvironmentVariables("%PATH%").Split(';'));
        }

        ~ProductEnvironment()
        {
            foreach (var file in m_TempFiles)
            {
                File.Delete(file);
            }
        }

        public bool CheckConEmu()
        {
            var conemu = GetConEmuExecutable();
            if (string.IsNullOrWhiteSpace(conemu) ||
                File.Exists(conemu) == false)
            {
                return false;
            }

            var conemulib = GetConEmuLibrary();
            if (string.IsNullOrWhiteSpace(conemulib) ||
                File.Exists(conemulib) == false)
            {
                return false;
            }
            return true;
        }

        public bool CheckConEmuAndDisplay()
        {
            bool result = CheckConEmu();
            if (result == false)
            {
                var caption = Instance.GetWindowCaption();

                var conemu = GetConEmuExecutable();
                var conemulib = GetConEmuLibrary();
                ExceptionMessageBox box = new ExceptionMessageBox();
                box.SetException("Unable to find the ConEmu installation",
                    "Please set the paths of your ConEmu installation in the Visual Studio options pane" +
                    Environment.NewLine + Environment.NewLine +
                    "ConEmu Path: " + conemu + Environment.NewLine +
                    "ConEmuCD Path: " + conemulib + Environment.NewLine);
                box.ShowDialog();
            }
            return result;
        }

        public string GetConfigurationFile()
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

            m_TempFiles.Add(configFilePath);
            return configFilePath;
        }

        public ShellTypes GetShellType()
        {
            if (this.Package != null)
            {
                var page = (OptionPageGridConEmu)this.Package.GetDialogPage(typeof(OptionPageGridConEmu));
                if (page != null)
                {
                    return page.Shell;
                }
            }
            return ShellTypes.CMD;
        }

        public string ShellTypeToString(ShellTypes shell)
        {
            switch (shell)
            {
                case ShellTypes.CMD:
                    return "{cmd}";
                case ShellTypes.PowerShell:
                    return "{powershell}";
                default:
                    return "{cmd}";
            }
        }

        public string GetWindowCaption()
        {
            return "ConEmu Console";
        }

        public string GetConEmuExecutable()
        {
            if (this.Package != null)
            {
                var page = (OptionPageGrid)this.Package.GetDialogPage(typeof(OptionPageGrid));
                if (page != null)
                {
                    string path = page.ConEmuPath.Trim(' ', '\n', '\r', '\"', '\'');
                    if (string.IsNullOrWhiteSpace(path) == false)
                    {
                        return path;
                    }
                }
            }
            return ExtendPath("ConEmu.exe");
        }

        public string GetConEmuLibrary()
        {
            if (this.Package != null)
            {
                var page = (OptionPageGrid)this.Package.GetDialogPage(typeof(OptionPageGrid));
                if (page != null)
                {
                    string path = page.ConEmuLibraryPath.Trim(' ', '\n', '\r', '\"', '\'');
                    if (string.IsNullOrWhiteSpace(path) == false)
                    {
                        return path;
                    }
                }
            }
            return ExtendPath("ConEmuCD.dll");
        }

        public string ExtendPath(string file)
        {
            foreach (var path in m_SearchPaths)
            {
                FileInfo fileInfo = new FileInfo(Path.Combine(path, file));
                if (fileInfo.Exists)
                {
                    return fileInfo.FullName;
                }
            }
            return file;
        }

    }
}
