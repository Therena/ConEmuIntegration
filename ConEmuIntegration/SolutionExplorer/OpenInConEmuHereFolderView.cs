//
// Copyright 2019 David Roller 
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

namespace ConEmuIntegration.SolutionExplorer
{
    internal sealed class OpenInConEmuHereFolderView
    {
        public const int CommandId = 256;

        public static readonly Guid CommandSet = new Guid("6F7E7F02-08A0-4AC4-88CA-CB9023401766");

        private readonly AsyncPackage package;

        private OpenInConEmu m_OpenInConEmu;

        private OpenInConEmuHereFolderView(AsyncPackage package)
        {
            this.package = package ?? throw new ArgumentNullException("package");
            m_OpenInConEmu = new OpenInConEmu();

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        public static OpenInConEmuHereFolderView Instance
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
            Instance = new OpenInConEmuHereFolderView(package);
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            ExecuteInConEmu.Instance.DisplayConEmu();
            m_OpenInConEmu.OpenFolderView();
        }
    }
}
