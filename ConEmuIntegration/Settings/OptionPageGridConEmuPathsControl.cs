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
using System.IO;
using System.Windows.Forms;

namespace ConEmuIntegration.Settings
{
    public partial class OptionPageGridConEmuPathsControl : UserControl
    {
        internal OptionPageGridConEmuPaths optionsPage;

        public OptionPageGridConEmuPathsControl()
        {
            InitializeComponent();
            OpenXMLConfiguration.InitialDirectory = Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData);
        }

        public void Initialize()
        {
            textBoxExecutable.Text = optionsPage.ConEmuPath;
            textBoxDefaultTask.Text = optionsPage.DefaultTask;
            textBoxXMLConfiguration.Text = optionsPage.ConEmuXML;
        }

        internal void OnApply()
        {
            optionsPage.DefaultTask = textBoxDefaultTask.Text;
            optionsPage.ConEmuXML = textBoxXMLConfiguration.Text;
            optionsPage.ConEmuPath = textBoxExecutable.Text;
        }

        private void buttonOpenConEmuFilePath_Click(object sender, EventArgs e)
        {
            var result = OpenConEmuFilePath.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxExecutable.Text = OpenConEmuFilePath.FileName;
            }
        }

        private void buttonXMLConfiguration_Click(object sender, EventArgs e)
        {
            var result = OpenXMLConfiguration.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxXMLConfiguration.Text = OpenXMLConfiguration.FileName;
            }
        }
    }
}
