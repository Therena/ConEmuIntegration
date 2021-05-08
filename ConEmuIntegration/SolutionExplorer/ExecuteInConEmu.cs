//
// Copyright 2021 David Roller 
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
using ConEmuIntegration.ConEmuProduct;
using EnvDTE;

namespace ConEmuIntegration.SolutionExplorer
{
    internal sealed class ExecuteInConEmu
    {
        public const int CommandId = 0x3005;
        public static readonly Guid CommandSet = new Guid("A1662AFB-0383-428D-A77D-DF353609B716");

        private readonly AsyncPackage m_Package;

        private readonly OpenInConEmu m_OpenInConEmu;

        private ExecuteInConEmu(AsyncPackage package)
        {
            this.m_Package = package ?? throw new ArgumentNullException(nameof(package));
            m_OpenInConEmu = new OpenInConEmu();

            if (ServiceProvider.GetService(typeof(IMenuCommandService)) is OleMenuCommandService commandService)
            {
                var menuCommandId = new CommandID(CommandSet, CommandId);
                var menuItem = new OleMenuCommand(this.MenuItemCallback, menuCommandId);
                menuItem.BeforeQueryStatus += MenuItem_BeforeQueryStatus;
                commandService.AddCommand(menuItem);
            }
        }

        private void MenuItem_BeforeQueryStatus(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            OleMenuCommand menuItem = (OleMenuCommand)sender;

            var dte = this.ServiceProvider.GetService(typeof(DTE)) as DTE;
            if (dte == null)
            {
                return;
            }
            
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

        private IServiceProvider ServiceProvider => m_Package;

        public static void Initialize(AsyncPackage package)
        {
            Instance = new ExecuteInConEmu(package);
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            ProductEnvironment.Instance.OpenConEmuToolWindow();
            m_OpenInConEmu.Execute();
        }
    }
}
