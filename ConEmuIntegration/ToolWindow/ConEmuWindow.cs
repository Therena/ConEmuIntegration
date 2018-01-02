﻿//
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
using Microsoft.VisualStudio.Shell.Interop;
using ConEmuIntegration.ConEmuProduct;
using EnvDTE;

namespace ConEmuIntegration.ToolWindow
{
    [Guid("4893abc5-3f3b-4868-9fcf-dd4bb3bd7b37")]
    public class ConEmuWindow : ToolWindowPane
    {
        private ConEmuWindowControl m_Control;

        public WindowEvents WindowEvents { get; private set; }

        public ConEmuWindow() : base(null)
        {
            this.Caption = ProductEnvironment.Instance.GetWindowCaption();

            m_Control = new ConEmuWindowControl();
            m_Control.ConEmuClosed += ConEmuClosed;
            this.Content = m_Control;
        }

        private void ConEmuClosed(object sender, EventArgs e)
        {
            var windowFrame = (IVsWindowFrame)this.Frame;
            windowFrame.Hide();
        }
    }
}
