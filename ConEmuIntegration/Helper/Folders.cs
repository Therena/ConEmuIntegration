//
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
using EnvDTE;
using Microsoft.VisualStudio.Shell;
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
