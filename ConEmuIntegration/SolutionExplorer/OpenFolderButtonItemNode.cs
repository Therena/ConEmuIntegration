﻿//
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
using ConEmuIntegration.ConEmuProduct;
using Microsoft.VisualStudio.Shell;

namespace ConEmuIntegration.SolutionExplorer
{
    internal sealed class OpenFolderButtonItemNode
    {
        public const int CommandId = 0x3001;
        public static readonly Guid CommandSet = new Guid("A1662AFB-0383-428D-A77D-DF353609B716");

        private readonly AsyncPackage m_Package;

        private readonly OpenInConEmu m_OpenInConEmu;

        private OpenFolderButtonItemNode(AsyncPackage package)
        {
            this.m_Package = package ?? throw new ArgumentNullException(nameof(package));
            m_OpenInConEmu = new OpenInConEmu();

            if (ServiceProvider.GetService(typeof(IMenuCommandService)) is OleMenuCommandService commandService)
            {
                var menuCommandId = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandId);
                commandService.AddCommand(menuItem);
            }
        }

        public static OpenFolderButtonItemNode Instance
        {
            get;
            private set;
        }

        private IServiceProvider ServiceProvider => m_Package;

        public static void Initialize(AsyncPackage package)
        {
            Instance = new OpenFolderButtonItemNode(package);
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            ProductEnvironment.Instance.OpenConEmuToolWindow();
            m_OpenInConEmu.OpenItem();
        }
    }
}
