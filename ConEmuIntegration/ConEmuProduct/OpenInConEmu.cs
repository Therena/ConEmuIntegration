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
            ThreadHelper.ThrowIfNotOnUIThread();
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

        public void OpenFolderView()
        {
            if (ProductEnvironment.Instance.Package == null)
            {
                return;
            }

            var provider = ProductEnvironment.Instance.Package as IServiceProvider;
            IVsUIHierarchyWindow hierarchyWindow = VsShellUtilities.GetUIHierarchyWindow(provider,
                VSConstants.StandardToolWindows.SolutionExplorer);

            ThreadHelper.ThrowIfNotOnUIThread();

            var pane = hierarchyWindow as WindowPane;
            if(pane == null)
            {
                throw new Exception("Cannot find the hirarchy pane in solution explorer");
            }

            var paneContent = pane.Content as System.Windows.Controls.Panel;
            if (paneContent == null || paneContent.Children.Count == 0)
            {
                throw new Exception("Cannot find the control panel in solution explorer");
            }

            var contentPresenter = paneContent.Children[0] as System.Windows.Controls.ContentPresenter;
            if (contentPresenter == null)
            {
                throw new Exception("Cannot find the content presenter in solution explorer");
            }

            var listBox = contentPresenter.Content as System.Windows.Controls.ListBox;
            if (listBox == null || listBox.SelectedItem == null)
            {
                throw new Exception("Cannot find the listbox in solution explorer");
            }

            var selectedItemNode = listBox.SelectedItem as Microsoft.Internal.VisualStudio.PlatformUI.IVirtualizingTreeNode;
            if (selectedItemNode == null)
            {
                throw new Exception("Cannot find the selected tree node in solution explorer");
            }

            var selectedItem = selectedItemNode.Item as Microsoft.VisualStudio.Workspace.VSIntegration.UI.IUINode;
            if (selectedItem == null)
            {
                throw new Exception("Cannot find the selected UI node in solution explorer");
            }

            var fileSystemItem = selectedItem.WorkspaceVisualNode as Microsoft.VisualStudio.Workspace.VSIntegration.UI.IFileSystemNode;
            if (fileSystemItem == null)
            {
                throw new Exception("Cannot find the selected file system item in solution explorer");
            }

            string path = fileSystemItem.FullPath;
            FileAttributes attr = File.GetAttributes(path);
            if (attr.HasFlag(FileAttributes.Directory) == false)
            {
                var file = new FileInfo(path);
                path = file.Directory.FullName;
            }

            if (string.IsNullOrWhiteSpace(path) == false)
            {
                SendChangeFolder(path);
                return;
            }
        }

        private void SendChangeFolder(string folder)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
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
