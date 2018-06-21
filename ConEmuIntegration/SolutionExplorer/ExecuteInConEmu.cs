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
using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using ConEmuIntegration.ConEmuProduct;
using EnvDTE;
using ConEmuIntegration.Helper;
using System.IO;

namespace ConEmuIntegration.SolutionExplorer
{
    internal sealed class ExecuteInConEmu
    {
        public const int CommandId = 256;

        public static readonly Guid CommandSet = new Guid("074e29bb-eb5c-4400-9ef0-f8abfbbe337b");

        private readonly AsyncPackage package;

        private OpenInConEmu m_OpenInConEmu;

        private ExecuteInConEmu(AsyncPackage package)
        {
            this.package = package ?? throw new ArgumentNullException("package");
            m_OpenInConEmu = new OpenInConEmu();

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new OleMenuCommand(this.MenuItemCallback, menuCommandID);
                menuItem.BeforeQueryStatus += MenuItem_BeforeQueryStatus;
                commandService.AddCommand(menuItem);
            }
        }

        private void MenuItem_BeforeQueryStatus(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            OleMenuCommand menuItem = (OleMenuCommand)sender;
            IVsMonitorSelection vsMonSel = (IVsMonitorSelection)this.ServiceProvider.GetService(typeof(SVsShellMonitorSelection));

            var dte = this.ServiceProvider.GetService(typeof(DTE)) as DTE;
            if (dte.SelectedItems.Count <= 0)
            {
                menuItem.Visible = false;
                return;
            }

            foreach (SelectedItem selectedItem in dte.SelectedItems)
            {
                if (selectedItem.Project != null)
                {
                    menuItem.Visible = true;
                    return;
                }
            }

            menuItem.Visible = false;
        }

        public static ExecuteInConEmu Instance
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

        public static void Initialize(AsyncPackage package)
        {
            Instance = new ExecuteInConEmu(package);
        }

        private void MenuItemCallback(object sender, EventArgs e)
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

            foreach (SelectedItem selectedItem in dte.SelectedItems)
            {
                if (selectedItem.Project != null)
                {
                    RunExecutable(selectedItem.Project);
                }
            }
        }

        private void RunExecutable(Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var prog = new StartProgram();
            var exe = prog.GetExecutable(project);

            if (exe == null)
            {
                return;
            }

            var attr = File.GetAttributes(exe.FullName);
            if (attr.HasFlag(FileAttributes.Directory))
            {
                return;
            }
            
            string command = "\"\"" + exe.FullName.Replace("\"", "\"\"") + "\"\"";

            var parameter = prog.GetParameter(project);
            if (string.IsNullOrWhiteSpace(parameter) == false)
            {
                command += " " + parameter;
            }

            var cd = ProductEnvironment.Instance.UseNormalChangeDirectory();
            if (cd)
            {
                ProductEnvironment.Instance.ExecuteGuiMacro("Print(@\"Invoke-Item " + command + "\",\"\n\")");
            }
            else
            {
                ProductEnvironment.Instance.ExecuteGuiMacro("Print(@\"" + command + "\",\"\n\")");
            }
        }

        public void DisplayConEmu()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (ProductEnvironment.Instance.CheckConEmuAndDisplay() == false)
            {
                return;
            }

            ProductEnvironment.Instance.OpenConEmuToolWindow();
        }
    }
}
