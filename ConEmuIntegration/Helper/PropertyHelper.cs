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

namespace ConEmuIntegration.Helper
{
    internal static class PropertyHelper
    {
        public static Properties GetActiveConfigurationProperties(Project proj)
        {
            if (proj.ConfigurationManager.ActiveConfiguration.Properties == null)
            {
                if (PropertyHelper.HasProperty(proj.Properties, "ActiveConfiguration") == false)
                {
                    return null;
                }

                return proj.Properties.Item("ActiveConfiguration").Value as Properties;
            }
            return proj.ConfigurationManager.ActiveConfiguration.Properties;
        }

        public static bool HasProperty(Properties properties, string propertyName)
        {
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
