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
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Controls;

namespace ConEmuIntegration.Helper
{
    internal static class ProjectSettings
    {
        private static Dictionary<string, object> GetAllProperty(EnvDTE.Properties properties)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var propertyList = new Dictionary<string, object>();

            if (properties == null)
            {
                return propertyList;
            }

            foreach (Property item in properties)
            {
                try
                {
                    if (item == null)
                    {
                        continue;
                    }

                    propertyList.Add(item.Name, item.Value);
                }
                catch (COMException) { }
                catch (NotImplementedException) { }
                catch (TargetParameterCountException) { }
            }

            return propertyList;
        }

        public static FileInfo GetSolutionPath(Solution sln)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var slnFile = new FileInfo(sln.FullName);
            if (slnFile.Directory == null || slnFile.Directory.Exists == false)
            {
                throw new FileNotFoundException("Unable to find the path for the selected item");
            }

            return new FileInfo(slnFile.FullName);
        }

        public static FileInfo GetSelectedItemPath(SelectedItem selectedItem)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (selectedItem.Project != null)
            {
                return GetProjectPath(selectedItem.Project);
            }

            if (selectedItem.ProjectItem != null)
            {
                return GetProjectItemPath(selectedItem.ProjectItem);
            }

            throw new FileNotFoundException("Unable to find the path for the selected item");
        }

        private static EnvDTE.Properties GetActiveConfigurationProperties(Project proj)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (proj.ConfigurationManager.ActiveConfiguration.Properties != null)
            {
                return proj.ConfigurationManager.ActiveConfiguration.Properties;
            }

            var propertyList = GetAllProperty(proj.Properties);
            if (propertyList.ContainsKey("ActiveConfiguration") == false)
            {
                throw new NullReferenceException("Unable to find the active configuration");
            }

            return proj.Properties.Item("ActiveConfiguration").Value as EnvDTE.Properties;
        }

        private static bool IsDirectory(string path)
        {
            try
            {
                var attr = File.GetAttributes(path);
                return attr.HasFlag(FileAttributes.Directory);
            }
            catch (IOException)
            {
                bool found = new[]
                    {
                        Path.DirectorySeparatorChar,
                        Path.AltDirectorySeparatorChar
                    }.Any(x => path.EndsWith(x.ToString()));

                return found;
            }
        }

        private static bool IsRooted(string filePath)
        {
            try
            {
                return Path.IsPathRooted(filePath);
            }
            catch (IOException) { }

            return true;
        }

        private static FileInfo ConstructOutputFileName(Project proj, string filePath)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (IsRooted(filePath) == false)
            {
                var projectPath = GetProjectPath(proj);
                filePath = Path.Combine(projectPath.Directory.FullName, filePath ?? string.Empty);
            }

            if (IsDirectory(filePath))
            {
                var propertyList = GetAllProperty(proj.Properties);
                if (propertyList.ContainsKey("OutputFileName"))
                {
                    filePath = Path.Combine(filePath ?? string.Empty, propertyList["OutputFileName"].ToString());
                }
                else
                {
                    throw new FileNotFoundException("Unable to find the project output path");
                }
            }

            return new FileInfo(filePath);
        }

        public static string GetProjectDebugParameter(Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var prop = GetActiveConfigurationProperties(project);

            var propertyList = GetAllProperty(prop);

            if (propertyList.ContainsKey("StartArguments"))
            {
                return propertyList["StartArguments"].ToString();
            }

            if (propertyList.ContainsKey("DebugSettings"))
            {
                var propertyListDebugSettings = GetAllProperty(prop.Item("DebugSettings").Value as EnvDTE.Properties);
                if (propertyListDebugSettings.ContainsKey("CommandArguments"))
                {
                    return propertyList["CommandArguments"].ToString();
                }
            }

            throw new FileNotFoundException("Unable to find the project start parameters");
        }

        //private static FileInfo GetExecutableForStartActionProgram(EnvDTE.Properties prop)
        //{
        //    ThreadHelper.ThrowIfNotOnUIThread();

        //    var propertyList = GetAllProperty(prop);

        //    if (propertyList.ContainsKey("DebugSettings") == false)
        //    {
        //        return null;
        //    }

        //    string path = prop.Item("StartProgram").Value.ToString();
        //    return new FileInfo(path);
        //}

        //private static FileInfo GetProjectDebugApplicationFromAction(EnvDTE.Properties prop)
        //{
        //    ThreadHelper.ThrowIfNotOnUIThread();

        //    var propertyList = GetAllProperty(prop);
        //    if (propertyList.ContainsKey("StartAction"))
        //    {
        //        return GetProjectDebugApplicationFromAction(prop.Item("StartAction").Value as EnvDTE.Properties);
        //    }

        //    var action = (VSLangProj.prjStartAction)prop.Item("StartAction").Value;
        //    if (action == VSLangProj.prjStartAction.prjStartActionProgram)
        //    {
        //        return GetExecutableForStartActionProgram(prop);
        //    }
        //    else if (action == VSLangProj.prjStartAction.prjStartActionURL)
        //    {
        //        throw new Exception("Unable to open a URL in the conemu console");
        //    }

        //    throw new FileNotFoundException("Unable to find the start action");
        //}

        public static FileInfo GetProjectDebugApplication(Project proj)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var prop = GetActiveConfigurationProperties(proj);

            var propertyList = GetAllProperty(prop);

            //if (propertyList.ContainsKey("StartAction"))
            //{
            //    return GetProjectDebugApplicationFromAction(prop.Item("StartAction").Value as EnvDTE.Properties);
            //}
            
            if (propertyList.ContainsKey("DebugSettings"))
            {
                var propertyListDebugSettings = GetAllProperty(prop.Item("DebugSettings").Value as EnvDTE.Properties);
                if (propertyListDebugSettings.ContainsKey("Command"))
                {
                    return new FileInfo(propertyListDebugSettings["Command"].ToString());
                }
            }

            return GetOutputFileName(proj);
        }

        public static FileInfo GetOutputFileName(Project proj)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var prop = GetActiveConfigurationProperties(proj);

            var propertyList = GetAllProperty(prop);

            string propertyName;
            if (propertyList.ContainsKey("PrimaryOutput"))
            {
                propertyName = "PrimaryOutput";
            }
            else if (propertyList.ContainsKey("CodeAnalysisInputAssembly"))
            {
                propertyName = "CodeAnalysisInputAssembly";
            }
            else if (propertyList.ContainsKey("OutputPath"))
            {
                propertyName = "OutputPath";
            }
            else
            {
                throw new FileNotFoundException("Unable to find the project output path");
            }

            var filePath = propertyList[propertyName].ToString();
            return ConstructOutputFileName(proj, filePath);
        }

        public static FileInfo GetProjectItemPath(ProjectItem item)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var propertyList = GetAllProperty(item.Properties);

            if (propertyList.ContainsKey("FullPath") == false)
            {
                throw new FileNotFoundException("Unable to find the project item full path");
            }
            return new FileInfo(propertyList["FullPath"].ToString());
        }

        private static FileInfo ConstructProjectPath(Dictionary<string, object> projectProperties, string filePath)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (IsDirectory(filePath))
            {
                if (projectProperties.ContainsKey("FileName"))
                {
                    filePath = Path.Combine(filePath ?? string.Empty, projectProperties["FileName"].ToString());
                }
                else
                {
                    throw new FileNotFoundException("Unable to find the project path");
                }
            }

            return new FileInfo(filePath);
        }

        public static FileInfo GetProjectPath(Project proj)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var propertyList = GetAllProperty(proj.Properties);

            string propertyName;
            if (propertyList.ContainsKey("FullProjectFileName"))
            {
                propertyName = "FullProjectFileName";
            }
            else if (propertyList.ContainsKey("FullPath"))
            {
                propertyName = "FullPath";
            }
            else if (propertyList.ContainsKey("ProjectFile"))
            {
                propertyName = "ProjectFile";
            }
            else
            {
                throw new FileNotFoundException("Unable to find the project path");
            }

            return ConstructProjectPath(propertyList, propertyList[propertyName].ToString());
        }

        public static FileInfo LookingForSelectedItem(IServiceProvider serviceProvider)
        {
            IVsUIHierarchyWindow hierarchyWindow = VsShellUtilities.GetUIHierarchyWindow(serviceProvider,
                VSConstants.StandardToolWindows.SolutionExplorer);

            ThreadHelper.ThrowIfNotOnUIThread();

            var pane = hierarchyWindow as WindowPane;
            if (pane == null)
            {
                throw new FileNotFoundException("Unable to find the items full path");
            }

            var paneContent = pane.Content as Panel;
            if (paneContent == null || paneContent.Children.Count == 0)
            {
                throw new FileNotFoundException("Unable to find the items full path");
            }

            foreach (ContentPresenter contentPresenter in paneContent.Children)
            {
                try
                {
                    if (contentPresenter == null)
                    {
                        continue;
                    }

                    var listBox = contentPresenter.Content.GetType().GetProperties().Single(p => p.Name == "TreeView" &&
                                            p.PropertyType.FullName == "Microsoft.VisualStudio.PlatformUI.SolutionPivotTreeView")
                        .GetValue(contentPresenter.Content) as ListBox;

                    if (listBox.SelectedItem == null)
                    {
                        continue;
                    }

                    var selectedItemNode = listBox.SelectedItem as Microsoft.Internal.VisualStudio.PlatformUI.IVirtualizingTreeNode;
                    if (selectedItemNode == null)
                    {
                        continue;
                    }

                    var sourceItem = selectedItemNode.Item.GetType().GetProperties().Single(p => p.Name == "SourceItem")
                        .GetValue(selectedItemNode.Item);

                    var canonicalName = sourceItem.GetType().GetProperties().Single(p => p.Name == "CanonicalName")
                        .GetValue(sourceItem) as string;

                    return new FileInfo(canonicalName);
                }
                catch
                {
                    continue;
                }
            }

            throw new FileNotFoundException("Unable to find the items full path");
        }
    }
}
