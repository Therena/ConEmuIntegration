﻿//
// Copyright 2020 David Roller 
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

namespace ConEmuIntegration.ToolWindow
{
    internal sealed class ConEmuWindowCommand
    {
        public const int CommandId = 0x0400;
        private readonly Package package;
        public static readonly Guid CommandSet = new Guid("A1662AFB-0383-428D-A77D-DF353609B716");

        private ConEmuWindowCommand(Package package)
        {
            this.package = package ?? throw new ArgumentNullException("package");

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.ShowToolWindow, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        public static ConEmuWindowCommand Instance
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
            Instance = new ConEmuWindowCommand(package);
        }

        private void ShowToolWindow(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            ProductEnvironment.Instance.OpenConEmuToolWindow();
        }
    }
}
