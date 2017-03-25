//
// Copyright 2017 David Roller
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
using System.IO;

namespace ConEmuIntegration.Helper
{
    internal sealed class Folders
    {
        public string GetProjectItemPath(ProjectItem item)
        {
            if (PropertyHelper.HasProperty(item.Properties, "FullPath") == false)
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
            var prjPath = ProjectPath.GetProjectPath(proj);
            if (string.IsNullOrWhiteSpace(prjPath))
            {
                return string.Empty;
            }

            string probKey = string.Empty;
            var prop = PropertyHelper.GetActiveConfigurationProperties(proj);
            if (PropertyHelper.HasProperty(prop, "PrimaryOutput"))
            {
                probKey = "PrimaryOutput";
            }
            else if (PropertyHelper.HasProperty(prop, "OutputPath"))
            {
                probKey = "OutputPath";
            }
            else
            {
                return string.Empty;
            }

            var filePath = prop.Item(probKey).Value.ToString();
            if (Path.IsPathRooted(filePath) == false)
            {
                filePath = Path.Combine(prjPath, filePath);
            }

            var attr = File.GetAttributes(filePath);
            if (attr.HasFlag(FileAttributes.Directory))
            {
                return filePath;
            }
            else
            {
                return new FileInfo(filePath).Directory.FullName;
            }
        }
    }
}
