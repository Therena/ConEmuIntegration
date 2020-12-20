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
using ConEmu.WinForms;
using ConEmuIntegration.Settings;
using ConEmuIntegration.ToolWindow;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConEmuIntegration.ConEmuProduct
{
    internal class ProductEnvironment
    {
        private readonly List<string> m_TempFiles;
        private readonly List<string> m_SearchPaths;
        private readonly List<string> m_StoredGuiMacros;

        public ConEmuWindowPackage Package { get; set; }
        public ConEmuControl ConEmu { get; set; }
        public static ProductEnvironment Instance { get; } = new ProductEnvironment();

        private ProductEnvironment()
        {
            m_TempFiles = new List<string>();
            m_StoredGuiMacros = new List<string>();

            m_SearchPaths = new List<string> {Directory.GetCurrentDirectory()};
            m_SearchPaths.AddRange(Environment.ExpandEnvironmentVariables("%PATH%").Split(';'));
        }

        ~ProductEnvironment()
        {
            foreach (var file in m_TempFiles)
            {
                File.Delete(file);
            }
        }
        private bool IsConEmuToolWindowVisible(IVsWindowFrame windowFrame)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (windowFrame != null && VSConstants.S_OK != windowFrame.IsVisible())
            {
                return false;
            }

            if (Instance.ConEmu == null)
            {
                return false;
            }

            return Instance.ConEmu.IsConsoleEmulatorOpen;
        }

        public void OpenConEmuToolWindow()
        {
            if (this.Package == null || this.Package.Zombied)
            {
                return;
            }

            try
            {
                if (CheckConEmuAndDisplay() == false)
                {
                    return;
                }

                var window = this.Package.FindToolWindow(typeof(ConEmuWindow), 0, true) as ConEmuWindow;
                if (window?.Frame == null)
                {
                    throw new NotSupportedException("Cannot create tool window");
                }

                ThreadHelper.ThrowIfNotOnUIThread();

                IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
                ErrorHandler.ThrowOnFailure(windowFrame.Show());

                if (IsConEmuToolWindowVisible(windowFrame) == false)
                {
                    window.RunConEmu();
                }
            }
            catch (Exception error)
            {
                ExceptionMessageBox box = new ExceptionMessageBox();
                box.SetException(error);
                box.ShowDialog();
            }
        }

        public void ConEmuToolWindowLoaded()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            foreach (var macro in m_StoredGuiMacros)
            {
                ExecuteGuiMacro(macro);
            }
            m_StoredGuiMacros.Clear();
        }

        public void ExecuteGuiMacro(string macro)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (IsConEmuToolWindowVisible(null) == false)
            {
                OpenConEmuToolWindow();
                m_StoredGuiMacros.Add(macro);
                return;
            }

            Instance.ConEmu.RunningSession?.ExecuteGuiMacroTextSync(macro);
        }

        public bool CheckConEmu()
        {
            var conemu = GetConEmuExecutable();
            if (string.IsNullOrWhiteSpace(conemu) ||
                File.Exists(conemu) == false)
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
                var conemu = GetConEmuExecutable();
                ExceptionMessageBox box = new ExceptionMessageBox();
                box.SetException("Unable to find the ConEmu installation",
                    "Please set the paths of your ConEmu installation in the Visual Studio options pane" +
                    Environment.NewLine + Environment.NewLine +
                    "ConEmu Path: " + conemu + Environment.NewLine);
                box.ShowDialog();
            }
            return result;
        }

        public string GetConfigurationFile()
        {
            if (this.Package != null)
            {
                var page = (OptionPageGridConEmuPaths)this.Package.GetDialogPage(typeof(OptionPageGridConEmuPaths));
                if (page != null)
                {
                    if (string.IsNullOrWhiteSpace(page.ConEmuXML) == false)
                    {
                        return page.ConEmuXML;
                    }
                }
            }
            return IntegratedDefaultSettings();
        }

        private string IntegratedDefaultSettings()
        {
            string configFilePath = Path.GetTempFileName() + ".xml";
            File.WriteAllText(configFilePath, Properties.Resources.ConEmu);

            m_TempFiles.Add(configFilePath);
            return configFilePath;
        }

        public string GetDefaultTask()
        {
            var page = (OptionPageGridConEmuPaths) Package?.GetDialogPage(typeof(OptionPageGridConEmuPaths));
            if (page != null)
            {
                if (string.IsNullOrWhiteSpace(page.DefaultTask) == false)
                {
                    return page.DefaultTask;
                }
            }
            return "{Shells::PowerShell}";
        }

        public bool UseNormalChangeDirectory()
        {
            string task = GetDefaultTask();
            return task.Contains("cmd") == false;
        }

        public string GetWindowCaption()
        {
            return "ConEmu Console";
        }

        public string GetConEmuExecutable()
        {
            var page = (OptionPageGridConEmuPaths) Package?.GetDialogPage(typeof(OptionPageGridConEmuPaths));
            if (page != null)
            {
                if (string.IsNullOrWhiteSpace(page.ConEmuPath) == false)
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
