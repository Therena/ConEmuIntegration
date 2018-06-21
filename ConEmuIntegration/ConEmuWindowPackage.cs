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
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using ConEmuIntegration.Settings;
using ConEmuIntegration.ConEmuProduct;
using ConEmuIntegration.ToolWindow;
using System.Threading;

namespace ConEmuIntegration
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(ConEmuWindow))]
    [Guid(ConEmuWindowPackage.PackageGuidString)]
    [ProvideOptionPage(typeof(OptionPageGrid), "ConEmu Integration", "Paths", 0, 0, true)]
    [ProvideOptionPage(typeof(OptionPageGridConEmu), "ConEmu Integration", "Settings", 0, 0, true)]
    public sealed class ConEmuWindowPackage : AsyncPackage
    {
        public const string PackageGuidString = "ff00f158-c7e9-46b0-a559-e1b3c8996343";
        
        public ConEmuWindowPackage()
        {
            ProductEnvironment.Instance.Package = this;
        }
        
        protected override System.Threading.Tasks.Task InitializeAsync(
            CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            ConEmuWindowCommand.Initialize(this);
            base.Initialize();
            SolutionExplorer.OpenConEmuHereProjectItem.Initialize(this);
            SolutionExplorer.OpenConEmuHereSolution.Initialize(this);
            SolutionExplorer.ExecuteInConEmu.Initialize(this);
            SolutionExplorer.OpenOutpathInConEmu.Initialize(this);
            SolutionExplorer.OpenInConEmuHereFolderView.Initialize(this);
            SolutionExplorer.OpenConEmuHereItemNode.Initialize(this);

            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}
