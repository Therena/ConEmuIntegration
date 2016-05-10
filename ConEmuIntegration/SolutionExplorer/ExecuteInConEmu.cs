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
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class ExecuteInConEmu
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 256;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("074e29bb-eb5c-4400-9ef0-f8abfbbe337b");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteInConEmu"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private ExecuteInConEmu(Package package)
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

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static ExecuteInConEmu Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new ExecuteInConEmu(package);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
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
            ExecuteGuiMacro("Print(@\"\"\"" + file.Replace("\"", "\"\"") + "\"\"\",\"\n\")");
        }

        private void ExecuteGuiMacro(string macro)
        {
            if (ProductEnvironment.Instance.ConEmuProcess == null)
            {
                return;
            }

            string conemu = ProductEnvironment.Instance.GetConEmuLibrary();
            var macroHelper = new ConEmuMacro(conemu);
            macroHelper.Execute(ProductEnvironment.Instance.ConEmuProcess.Id.ToString(), macro);
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
