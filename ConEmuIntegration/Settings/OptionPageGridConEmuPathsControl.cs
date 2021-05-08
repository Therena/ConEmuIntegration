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
using ConEmuIntegration.ConEmuProduct;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

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

            labelExecutable.Text = Properties.Resources.SettingsConEmuPath;
            labelXMLConfiguration.Text = Properties.Resources.SettingsXMLConfiguration;
            labelDefaultTask.Text = Properties.Resources.SettingsDefaultTask;
            
            _ = SetupButtonImagesAsync(KnownMonikers.OpenFileDialog, buttonXMLConfiguration);
            _ = SetupButtonImagesAsync(KnownMonikers.OpenFileDialog, buttonOpenConEmuFilePath);
            _ = SetupButtonImagesAsync(KnownMonikers.ClearWindowContent, buttonXMLConfigurationClear);
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

        private async System.Threading.Tasks.Task SetupButtonImagesAsync(ImageMoniker moniker, Button button)
        {
            var package = ProductEnvironment.Instance.Package;
            var imageServiceTask = package.GetServiceAsync(typeof(SVsImageService));

            int size;
            if (button.Height > button.Width)
            {
                size = button.Width - 4;
            }
            else
            {
                size = button.Height - 4;
            }

            var imageAttributes = new ImageAttributes
            {
                Flags = (uint)_ImageAttributesFlags.IAF_RequiredFlags,
                ImageType = (uint)_UIImageType.IT_Icon,
                Format = (uint)_UIDataFormat.DF_WPF,
                Dpi = 96,
                LogicalHeight = size,
                LogicalWidth = size,
                StructSize = Marshal.SizeOf(typeof(ImageAttributes)),
            };

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var imageService = (await imageServiceTask) as IVsImageService2;
            var image = imageService.GetImage(moniker, imageAttributes);

            image.get_Data(out object data);

            var dataBitmap = data as BitmapSource;
            button.Image = BitmapFromSource(dataBitmap);
        }

        private System.Drawing.Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            System.Drawing.Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new System.Drawing.Bitmap(outStream);
            }
            return bitmap;
        }

        private void ButtonOpenConEmuFilePath_Click(object sender, EventArgs e)
        {
            var result = OpenConEmuFilePath.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxExecutable.Text = OpenConEmuFilePath.FileName;
            }
        }

        private void ButtonXMLConfiguration_Click(object sender, EventArgs e)
        {
            var result = OpenXMLConfiguration.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxXMLConfiguration.Text = OpenXMLConfiguration.FileName;
            }
        }

        private void ButtonXMLConfigurationClear_Click(object sender, EventArgs e)
        {
            textBoxXMLConfiguration.Text = string.Empty;
        }
    }
}
