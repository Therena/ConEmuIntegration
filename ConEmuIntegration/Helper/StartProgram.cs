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
using System;
using System.IO;

namespace ConEmuIntegration.Helper
{
    internal class StartProgram
    {
        private FileInfo GetExecutableForStartActionProgram(Properties prop)
        {
            if (PropertyHelper.HasProperty(prop, "StartProgram") == false)
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

        public string GetParameter(Project project)
        {
            var prop = PropertyHelper.GetActiveConfigurationProperties(project);
            if (PropertyHelper.HasProperty(prop, "StartArguments"))
            {
                return prop.Item("StartArguments").Value.ToString();
            }

            if (PropertyHelper.HasProperty(prop, "DebugSettings"))
            {
                var debugProps = prop.Item("DebugSettings").Value as Properties;
                if (PropertyHelper.HasProperty(debugProps, "CommandArguments"))
                {
                    return debugProps.Item("CommandArguments").Value.ToString();
                }
            }
            return string.Empty;
        }

        public FileInfo GetExecutable(Project project)
        {
            var prop = PropertyHelper.GetActiveConfigurationProperties(project);
            if (PropertyHelper.HasProperty(prop, "StartAction"))
            {
                var filePath = GetExecutableWithAction(prop);
                if (filePath != null)
                {
                    return filePath;
                }
            }

            if (PropertyHelper.HasProperty(prop, "DebugSettings"))
            {
                var debugProps = prop.Item("DebugSettings").Value as Properties;
                if (PropertyHelper.HasProperty(debugProps, "Command"))
                {
                    var file = new FileInfo(debugProps.Item("Command").Value.ToString());
                    if(file.Exists)
                    {
                        return file;
                    }
                }
            }

            if (PropertyHelper.HasProperty(prop, "PrimaryOutput"))
            {
                return new FileInfo(prop.Item("PrimaryOutput").Value.ToString());
            }

            if (PropertyHelper.HasProperty(prop, "OutputPath") == false)
            {
                return null;
            }

            var outputPath = prop.Item("OutputPath").Value.ToString();

            if (PropertyHelper.HasProperty(project.Properties, "OutputFileName") == false)
            {
                return null;
            }

            var outputFileName = project.Properties.Item("OutputFileName").Value.ToString();

            var prjPath = ProjectPath.GetProjectPath(project);
            if (string.IsNullOrWhiteSpace(prjPath))
            {
                return null;
            }
            return new FileInfo(Path.Combine(prjPath, outputPath, outputFileName));
        }
    }
}
