//
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
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using ConEmuIntegration.ConEmuProduct;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using ConEmuIntegration.ToolWindow;
using ConEmuIntegration.Settings;

namespace ConEmuIntegration
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(ConEmuWindow))]
    [Guid(ConEmuWindowPackage.PackageGuidString)]
    [ProvideOptionPage(typeof(OptionPageGridConEmuPaths), "ConEmu Integration", "Paths", 0, 0, true)]
    [ProvideOptionPage(typeof(OptionPageGridConEmuSettings), "ConEmu Integration", "Settings", 0, 0, true)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
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

            SolutionExplorer.ExecuteInConEmu.Initialize(this);
            SolutionExplorer.OpenFolderButtonItemNode.Initialize(this);
            SolutionExplorer.OpenFolderButtonProjectNode.Initialize(this);
            SolutionExplorer.OpenFolderButtonSolutionNode.Initialize(this);
            SolutionExplorer.OpenOutDirectoryButtonProjectNode.Initialize(this);

            base.Initialize();

            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}
