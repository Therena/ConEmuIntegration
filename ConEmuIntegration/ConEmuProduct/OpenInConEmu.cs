//
// Copyright 2021 David Roller 
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
using ConEmuIntegration.Helper;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.IO;

namespace ConEmuIntegration.ConEmuProduct
{
    internal sealed class OpenInConEmu
    {
        public void OpenSolution()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (ProductEnvironment.Instance.Package == null)
            {
                return;
            }

            var provider = ProductEnvironment.Instance.Package as IServiceProvider;
            var dte = provider.GetService(typeof(DTE)) as DTE;
            if (dte != null && dte.SelectedItems.Count <= 0)
            {
                return;
            }

            var path = ProjectSettings.GetSolutionPath(dte?.Solution);
            SendChangeFolder(path.Directory);
        }

        public void Execute()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (ProductEnvironment.Instance.Package == null)
            {
                return;
            }

            var provider = ProductEnvironment.Instance.Package as IServiceProvider;
            var dte = provider.GetService(typeof(DTE)) as DTE;
            if (dte == null)
            {
                return;
            }
            
            if (dte.SelectedItems.Count <= 0)
            {
                return;
            }

            foreach (SelectedItem selectedItem in dte.SelectedItems)
            {
                if (selectedItem.Project == null)
                {
                    continue;
                }

                var exePath = ProjectSettings.GetProjectDebugApplication(selectedItem.Project);
                if (exePath == null)
                {
                    continue;
                }

                var attr = File.GetAttributes(exePath.FullName);
                if (attr.HasFlag(FileAttributes.Directory))
                {
                    continue;
                }

                string command = "\"\"" + exePath.FullName.Replace("\"", "\"\"") + "\"\"";

                var parameter = ProjectSettings.GetProjectDebugParameter(selectedItem.Project);
                if (string.IsNullOrWhiteSpace(parameter) == false)
                {
                    command += " " + parameter;
                }

                if (string.IsNullOrWhiteSpace(command) == false)
                {
                    SendInvokeCommand(command);
                    return;
                }
            }
        }

        public void OpenProject()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (ProductEnvironment.Instance.Package == null)
            {
                return;
            }

            var provider = ProductEnvironment.Instance.Package as IServiceProvider;
            var dte = provider.GetService(typeof(DTE)) as DTE;
            if (dte == null)
            {
                return;
            }

            if (dte.SelectedItems.Count <= 0)
            {
                return;
            }

            foreach (SelectedItem selectedItem in dte.SelectedItems)
            {
                var path = ProjectSettings.GetSelectedItemPath(selectedItem);
                SendChangeFolder(path.Directory);
                return;
            }
        }

        public void OpenItem()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (ProductEnvironment.Instance.Package == null)
            {
                return;
            }

            var provider = ProductEnvironment.Instance.Package as IServiceProvider;
            var dte = provider.GetService(typeof(DTE)) as DTE;
            if (dte == null)
            {
                return;
            }
            
            if (dte.SelectedItems.Count <= 0)
            {
                return;
            }

            FileInfo path = null;
            try
            {
                foreach (SelectedItem selectedItem in dte.SelectedItems)
                {
                    path = ProjectSettings.GetSelectedItemPath(selectedItem);
                }
            }
            catch (FileNotFoundException)
            {
                path = ProjectSettings.LookingForSelectedItem(provider);
            }

            SendChangeFolder(path.Directory);
        }

        private void SendChangeFolder(DirectoryInfo folder)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var cd = ProductEnvironment.Instance.UseNormalChangeDirectory();
            if (cd)
            {
                ProductEnvironment.Instance.ExecuteGuiMacro("Print(@\"cd \"\"" +
                    folder.FullName.Replace("\"", "\"\"") + "\"\"\",\"\n\")");
            }
            else
            {
                ProductEnvironment.Instance.ExecuteGuiMacro("Print(@\"cd /d \"\"" +
                    folder.FullName.Replace("\"", "\"\"") + "\"\"\",\"\n\")");
            }
        }

        private void SendInvokeCommand(string command)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var cd = ProductEnvironment.Instance.UseNormalChangeDirectory();
            if (cd)
            {
                ProductEnvironment.Instance.ExecuteGuiMacro("Print(@\"Invoke-Item " + command + "\",\"\n\")");
            }
            else
            {
                ProductEnvironment.Instance.ExecuteGuiMacro("Print(@\"" + command + "\",\"\n\")");
            }
        }
    }
}
