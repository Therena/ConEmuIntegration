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
using ConEmuIntegration.ConEmuProduct;

namespace ConEmuIntegration.ToolWindow
{
    internal sealed class ConEmuWindowCommand
    {
        public const int CommandId = 0x0100;
        private readonly Package package;
        public static readonly Guid CommandSet = new Guid("5df544d6-c00a-4471-936b-d5441108ccfd");

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

        public static void Initialize(Package package)
        {
            Instance = new ConEmuWindowCommand(package);
        }

        private void ShowToolWindow(object sender, EventArgs e)
        {
            ProductEnvironment.Instance.OpenConEmuToolWindow();
        }
    }
}
