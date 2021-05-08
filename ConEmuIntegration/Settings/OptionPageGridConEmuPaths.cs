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
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ConEmuIntegration.Settings
{
    [Guid("5C8D8AF1-4323-4DBD-B569-6EC502F00009")]
    internal sealed class OptionPageGridConEmuPaths : DialogPage
    {
        private OptionPageGridConEmuPathsControl m_Page = new OptionPageGridConEmuPathsControl();

        [Category("ConEmu Integration")]
        [DisplayName("Executable")]
        public string ConEmuPath { get; set; }

        [Category("ConEmu Integration")]
        [DisplayName("Default Task")]
        public string DefaultTask { get; set; }

        [Category("ConEmu Integration")]
        [DisplayName("XML configuration")]
        public string ConEmuXML { get; set; }

        protected override IWin32Window Window
        {
            get
            {
                m_Page.optionsPage = this;
                m_Page.Initialize();
                return m_Page;
            }
        }

        protected override void OnApply(PageApplyEventArgs e)
        {
            if(e.ApplyBehavior == ApplyKind.Apply)
            {
                m_Page.OnApply();
            }
            base.OnApply(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            m_Page.Initialize();
            base.OnClosed(e);
        }
    }
}
