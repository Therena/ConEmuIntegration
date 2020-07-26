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
using Microsoft.VisualStudio.Shell;
using System;
using System.IO;
using System.Linq;

namespace ConEmuIntegration.Helper
{
    internal sealed class Folders
    {
        public string GetSolutionPath(Solution sln)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var slnFile = new FileInfo(sln.FullName);
            if (slnFile.Directory.Exists)
            {
                return slnFile.Directory.FullName;
            }
            return string.Empty;
        }

        public string GetProjectPath(Project proj)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            // C# Project has the FullPath property
            if (HasProperty(proj.Properties, "FullPath"))
            {
                var filePath = proj.Properties.Item("FullPath").Value.ToString();
                var fullPath = new FileInfo(filePath);
                return fullPath.Directory.FullName;
            }

            // C++ Project has the ProjectFile property
            if (HasProperty(proj.Properties, "ProjectFile"))
            {
                var filePath = proj.Properties.Item("ProjectFile").Value.ToString();
                var fullPath = new FileInfo(filePath);
                return fullPath.Directory.FullName;
            }
            return string.Empty;
        }

        public string GetProjectItemPath(ProjectItem item)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (HasProperty(item.Properties, "FullPath") == false)
            {
                return string.Empty;
            }

            var fullPathProperty = item.Properties.Item("FullPath");
            if (fullPathProperty == null)
            {
                return string.Empty;
            }

            var fullPath = new FileInfo(fullPathProperty.Value.ToString());
            return fullPath.Directory.FullName;
        }

        public string GetExecutableParameter(Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var prop = GetActiveConfigurationProperties(project);
            if (HasProperty(prop, "StartArguments"))
            {
                return prop.Item("StartArguments").Value.ToString();
            }

            if (HasProperty(prop, "DebugSettings"))
            {
                var debugProps = prop.Item("DebugSettings").Value as Properties;
                if (HasProperty(debugProps, "CommandArguments"))
                {
                    return debugProps.Item("CommandArguments").Value.ToString();
                }
            }
            return string.Empty;
        }

        public FileInfo GetExecutable(Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var prop = GetActiveConfigurationProperties(project);
            if (HasProperty(prop, "StartAction"))
            {
                var filePath = GetExecutableWithAction(prop);
                if (filePath != null)
                {
                    return filePath;
                }
            }

            if (HasProperty(prop, "DebugSettings"))
            {
                var debugProps = prop.Item("DebugSettings").Value as Properties;
                if (HasProperty(debugProps, "Command"))
                {
                    var file = new FileInfo(debugProps.Item("Command").Value.ToString());
                    if (file.Exists)
                    {
                        return file;
                    }
                }
            }

            if (HasProperty(prop, "PrimaryOutput"))
            {
                return new FileInfo(prop.Item("PrimaryOutput").Value.ToString());
            }

            if (HasProperty(prop, "OutputPath") == false)
            {
                return null;
            }

            var outputPath = prop.Item("OutputPath").Value.ToString();

            if (HasProperty(project.Properties, "OutputFileName") == false)
            {
                return null;
            }

            var outputFileName = project.Properties.Item("OutputFileName").Value.ToString();

            var prjPath = GetProjectPath(project);
            if (string.IsNullOrWhiteSpace(prjPath))
            {
                return null;
            }
            return new FileInfo(Path.Combine(prjPath, outputPath, outputFileName));
        }

        public string GetOutputPath(Project proj)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var prjPath = GetProjectPath(proj);
            if (string.IsNullOrWhiteSpace(prjPath))
            {
                return string.Empty;
            }

            Properties prop = null;
            string probKey = string.Empty;
            if (proj.ConfigurationManager.ActiveConfiguration.Properties == null)
            {
                if (HasProperty(proj.Properties, "ActiveConfiguration") == false)
                {
                    return string.Empty;
                }

                prop = proj.Properties.Item("ActiveConfiguration").Value as Properties;
                if (HasProperty(prop, "PrimaryOutput"))
                {
                    probKey = "PrimaryOutput";
                }
            }
            else
            {
                prop = proj.ConfigurationManager.ActiveConfiguration.Properties;
                if (HasProperty(prop, "OutputPath"))
                {
                    probKey = "OutputPath";
                }
            }

            if (HasProperty(prop, probKey) == false)
            {
                return string.Empty;
            }

            var filePath = prop.Item(probKey).Value.ToString();
            try
            {
                if (Path.IsPathRooted(filePath) == false)
                {
                    filePath = Path.Combine(prjPath, filePath);
                }

                if (IsDirectory(filePath))
                {
                    return filePath;
                }
                else
                {
                    return new FileInfo(filePath).Directory.FullName;
                }
            }
            catch (IOException)
            {
                return filePath;
            }
        }

        private bool IsDirectory(string path)
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

        private FileInfo GetExecutableForStartActionProgram(Properties prop)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (HasProperty(prop, "StartProgram") == false)
            {
                return null;
            }

            string path = prop.Item("StartProgram").Value.ToString();
            return new FileInfo(path);
        }

        private FileInfo GetExecutableWithAction(Properties prop)
        {
            var action = (VSLangProj.prjStartAction)prop.Item("StartAction").Value;
            if (action == VSLangProj.prjStartAction.prjStartActionProgram)
            {
                return GetExecutableForStartActionProgram(prop);
            }
            else if (action == VSLangProj.prjStartAction.prjStartActionURL)
            {
                throw new Exception("Unable to open a URL in the conemu console");
            }
            return null;
        }

        private Properties GetActiveConfigurationProperties(Project proj)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (proj.ConfigurationManager.ActiveConfiguration.Properties == null)
            {
                if (HasProperty(proj.Properties, "ActiveConfiguration") == false)
                {
                    return null;
                }

                return proj.Properties.Item("ActiveConfiguration").Value as Properties;
            }
            return proj.ConfigurationManager.ActiveConfiguration.Properties;
        }

        private bool HasProperty(Properties properties, string propertyName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (properties != null)
            {
                foreach (Property item in properties)
                {
                    if (item != null && item.Name == propertyName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
