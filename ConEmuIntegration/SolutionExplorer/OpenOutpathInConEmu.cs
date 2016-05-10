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
using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE80;
using EnvDTE;
using ConEmuIntegration.Helper;

namespace ConEmuIntegration.SolutionExplorer
{
    internal sealed class OpenOutpathInConEmu
    {
        public const int CommandId = 256;

        public static readonly Guid CommandSet = new Guid("5f17d8c2-738d-4d1b-8f18-8458da05717d");

        private readonly Package package;

        private OpenOutpathInConEmu(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        public static OpenOutpathInConEmu Instance
        {
            get;
            private set;
        }

        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        public static void Initialize(Package package)
        {
            Instance = new OpenOutpathInConEmu(package);
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            var dte = this.ServiceProvider.GetService(typeof(SDTE)) as DTE2;
            if (dte.SelectedItems.Count <= 0)
            {
                return;
            }

            var folders = new Folders();
            foreach (SelectedItem selectedItem in dte.SelectedItems)
            {
                if (selectedItem.Project != null)
                {
                    var path = folders.GetOutputPath(selectedItem.Project);
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        return;
                    }

                    ExecuteInConEmu.Instance.ExecuteGuiMacro("Print(@\"cd \"\"" +
                        path.Replace("\"", "\"\"") + "\"\"\",\"\n\")");
                    ExecuteInConEmu.Instance.DisplayConEmu();
                }
            }
        }
    }
}
