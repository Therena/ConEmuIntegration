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

namespace ConEmuIntegration.SolutionExplorer
{
    internal sealed class OpenConEmuHereProjectItem
    {
        public const int CommandId = 256;
        
        public static readonly Guid CommandSet = new Guid("c75f2974-4759-448c-bf2a-c400d83fe990");
        
        private readonly Package package;

        private OpenInConEmu m_OpenInConEmu;

        private OpenConEmuHereProjectItem(Package package)
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
        
        public static OpenConEmuHereProjectItem Instance
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
            Instance = new OpenConEmuHereProjectItem(package);
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            ExecuteInConEmu.Instance.DisplayConEmu();
            m_OpenInConEmu.Open();
        }
    }
}
