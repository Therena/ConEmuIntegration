﻿//
// Copyright 2018 David Roller
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
using ConEmuIntegration.ConEmuProduct;
using ConEmuIntegration.Helper;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace ConEmuIntegration.SolutionExplorer
{
    internal sealed class OpenInConEmu
    {
        public void OpenSolution()
        {
            if (ProductEnvironment.Instance.Package == null)
            {
                return;
            }

            var provider = ProductEnvironment.Instance.Package as IServiceProvider;
            var dte = provider.GetService(typeof(DTE)) as DTE;
            if (dte.SelectedItems.Count <= 0)
            {
                return;
            }

            var folders = new Folders();
            var path = folders.GetSolutionPath(dte.Solution);

            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            SendChangeFolder(path);
        }

        public void Open()
        {
            if (ProductEnvironment.Instance.Package == null)
            {
                return;
            }

            var provider = ProductEnvironment.Instance.Package as IServiceProvider;
            var dte = provider.GetService(typeof(DTE)) as DTE;
            if (dte.SelectedItems.Count <= 0)
            {
                return;
            }

            var folders = new Folders();
            foreach (SelectedItem selectedItem in dte.SelectedItems)
            {
                var path = "";
                if (selectedItem.Project != null)
                {
                    path = folders.GetProjectPath(selectedItem.Project);
                }

                if (selectedItem.ProjectItem != null)
                {
                    path = folders.GetProjectItemPath(selectedItem.ProjectItem);
                }
                
                if (string.IsNullOrWhiteSpace(path) == false)
                {
                    SendChangeFolder(path);
                    return;
                }
            }
        }

        private void SendChangeFolder(string folder)
        {
            var cd = ProductEnvironment.Instance.UseNormalChangeDirectory();
            if (cd)
            {
                ProductEnvironment.Instance.ExecuteGuiMacro("Print(@\"cd \"\"" +
                    folder.Replace("\"", "\"\"") + "\"\"\",\"\n\")");
            }
            else
            {
                ProductEnvironment.Instance.ExecuteGuiMacro("Print(@\"cd /d \"\"" +
                    folder.Replace("\"", "\"\"") + "\"\"\",\"\n\")");
            }
        }
    }
}
