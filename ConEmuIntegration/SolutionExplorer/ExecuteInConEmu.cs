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
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using ConEmuIntegration.ConEmu;
using EnvDTE80;
using EnvDTE;
using System.IO;
using ConEmuIntegration.ToolWindow;

namespace ConEmuIntegration.SolutionExplorer
{
    internal sealed class ExecuteInConEmu
    {
        public const int CommandId = 256;
        
        public static readonly Guid CommandSet = new Guid("074e29bb-eb5c-4400-9ef0-f8abfbbe337b");
        
        private readonly Package package;

        private OpenInConEmu m_OpenInConEmu;

        private ExecuteInConEmu(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;
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
            OleMenuCommand menuItem = (OleMenuCommand)sender;
            IVsMonitorSelection vsMonSel = (IVsMonitorSelection)this.ServiceProvider.GetService(typeof(SVsShellMonitorSelection));

            var dte = this.ServiceProvider.GetService(typeof(SDTE)) as DTE2;
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
        
        public static void Initialize(Package package)
        {
            Instance = new ExecuteInConEmu(package);
        }
        
        private void MenuItemCallback(object sender, EventArgs e)
        {
            if (ProductEnvironment.Instance.Package == null)
            {
                return;
            }

            var provider = ProductEnvironment.Instance.Package as IServiceProvider;
            var dte = provider.GetService(typeof(SDTE)) as DTE2;
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
            if (HasProperty(project.Properties, "FullPath") == false)
            {
                return;
            }

            var prjFile = new FileInfo(project.Properties.Item("FullPath").Value.ToString());
            var projectPath = prjFile.Directory.FullName;
            
            var activeConvProp = project.ConfigurationManager.ActiveConfiguration.Properties;
            if (HasProperty(activeConvProp, "OutputPath") == false)
            {
                return;
            }

            var outputPath = activeConvProp.Item("OutputPath").Value.ToString();
            
            if (HasProperty(project.Properties, "OutputFileName") == false)
            {
                return;
            }

            var outputFileName = project.Properties.Item("OutputFileName").Value.ToString();

            var file = Path.Combine(projectPath, outputPath, outputFileName);

            var shell = ProductEnvironment.Instance.GetShellType();
            if (shell == Settings.OptionPageGridConEmu.ShellTypes.PowerShell)
            {
                ExecuteGuiMacro("Print(@\"Invoke-Item \"\"" + file.Replace("\"", "\"\"") + "\"\"\",\"\n\")");
            }
            else
            {
                ExecuteGuiMacro("Print(@\"\"\"" + file.Replace("\"", "\"\"") + "\"\"\",\"\n\")");
            }
            DisplayConEmu();
        }

        public void ExecuteGuiMacro(string macro)
        {
            if (ProductEnvironment.Instance.ConEmuProcess == null)
            {
                return;
            }

            string conemu = ProductEnvironment.Instance.GetConEmuLibrary();
            var macroHelper = new ConEmuMacro(conemu);
            macroHelper.Execute(ProductEnvironment.Instance.ConEmuProcess.Id.ToString(), macro);
        }

        public void DisplayConEmu()
        {
            if (ProductEnvironment.Instance.CheckConEmuAndDisplay() == false)
            {
                return;
            }

            ToolWindowPane window = this.package.FindToolWindow(typeof(ConEmuWindow), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException("Cannot create tool window");
            }

            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }

        private bool HasProperty(Properties properties, string propertyName)
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
