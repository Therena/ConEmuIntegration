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
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ConEmuIntegration.ConEmu
{
    internal class ProductEnvironment
    {
        private List<string> m_TempFiles;
        private List<string> m_SearchPaths;

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
            m_SearchPaths.Add(@"C:\Users\David Roller\Downloads\conemu-inside-master\conemu-inside-master\ConEmuInside\bin\Debug\ConEmu");
        }

        ~ProductEnvironment()
        {
            foreach(var file in m_TempFiles)
            {
                File.Delete(file);
            }
        }

        public bool CheckConEmu()
        {
            if (File.Exists(GetConEmuExecutable()) == false)
            {
                return false;
            }

            if (File.Exists(GetConLibrary()) == false)
            {
                return false;
            }
            return true;
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

        public string GetWindowCaption()
        {
            return "ConEmu Console";
        }

        public string GetConEmuExecutable()
        {
            string conemu = ExtendPath("ConEmu.exe");
            return conemu;
        }

        public string GetConLibrary()
        {
            string conemu = ExtendPath("ConEmuCD.dll");
            return conemu;
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
