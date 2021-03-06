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
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using ConEmuIntegration.ConEmuProduct;

namespace ConEmuIntegration.ToolWindow
{
    [Guid("F4D34FFE-6D41-47F1-BD2C-7897995974AB")]
    public sealed class ConEmuWindow : ToolWindowPane
    {
        private readonly ConEmuWindowControl m_ConEmuControl;

        public ConEmuWindow() : base(null)
        {
            this.Caption = ProductEnvironment.Instance.GetWindowCaption();

            m_ConEmuControl = new ConEmuWindowControl();
            m_ConEmuControl.ConEmuClosed += ConEmuClosed;
            this.Content = m_ConEmuControl;
        }

        public bool RunConEmu()
        {
            if (null == m_ConEmuControl)
            {
                return false;
            }

            ThreadHelper.ThrowIfNotOnUIThread();

            return m_ConEmuControl.RunConEmu();
        }

        private void ConEmuClosed(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var windowFrame = (IVsWindowFrame)this.Frame;
            windowFrame.Hide();
        }
    }
}
