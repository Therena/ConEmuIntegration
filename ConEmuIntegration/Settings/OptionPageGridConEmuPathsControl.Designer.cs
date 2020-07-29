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
namespace ConEmuIntegration.Settings
{
    partial class OptionPageGridConEmuPathsControl
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.OpenConEmuFilePath = new System.Windows.Forms.OpenFileDialog();
            this.GroupBoxConEmu = new System.Windows.Forms.GroupBox();
            this.textBoxExecutable = new System.Windows.Forms.TextBox();
            this.buttonOpenConEmuFilePath = new System.Windows.Forms.Button();
            this.labelExecutable = new System.Windows.Forms.Label();
            this.groupBoxSettings = new System.Windows.Forms.GroupBox();
            this.panelXMLConfiguration = new System.Windows.Forms.Panel();
            this.textBoxXMLConfiguration = new System.Windows.Forms.TextBox();
            this.buttonXMLConfigurationClear = new System.Windows.Forms.Button();
            this.labelXMLConfiguration = new System.Windows.Forms.Label();
            this.panelDefaultTask = new System.Windows.Forms.Panel();
            this.textBoxDefaultTask = new System.Windows.Forms.TextBox();
            this.labelDefaultTask = new System.Windows.Forms.Label();
            this.OpenXMLConfiguration = new System.Windows.Forms.OpenFileDialog();
            this.buttonXMLConfiguration = new System.Windows.Forms.Button();
            this.GroupBoxConEmu.SuspendLayout();
            this.groupBoxSettings.SuspendLayout();
            this.panelXMLConfiguration.SuspendLayout();
            this.panelDefaultTask.SuspendLayout();
            this.SuspendLayout();
            // 
            // OpenConEmuFilePath
            // 
            this.OpenConEmuFilePath.DefaultExt = "exe";
            this.OpenConEmuFilePath.FileName = "ConEmu64.exe";
            this.OpenConEmuFilePath.Filter = "ConEmu|ConEmu*.exe|ConEmu64|ConEmu64.exe";
            this.OpenConEmuFilePath.InitialDirectory = "C:\\Program Files\\ConEmu";
            this.OpenConEmuFilePath.ReadOnlyChecked = true;
            // 
            // GroupBoxConEmu
            // 
            this.GroupBoxConEmu.Controls.Add(this.textBoxExecutable);
            this.GroupBoxConEmu.Controls.Add(this.buttonOpenConEmuFilePath);
            this.GroupBoxConEmu.Controls.Add(this.labelExecutable);
            this.GroupBoxConEmu.Dock = System.Windows.Forms.DockStyle.Top;
            this.GroupBoxConEmu.Location = new System.Drawing.Point(0, 0);
            this.GroupBoxConEmu.Name = "GroupBoxConEmu";
            this.GroupBoxConEmu.Padding = new System.Windows.Forms.Padding(10);
            this.GroupBoxConEmu.Size = new System.Drawing.Size(971, 86);
            this.GroupBoxConEmu.TabIndex = 0;
            this.GroupBoxConEmu.TabStop = false;
            this.GroupBoxConEmu.Text = "Conemu";
            // 
            // textBoxExecutable
            // 
            this.textBoxExecutable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxExecutable.Location = new System.Drawing.Point(98, 29);
            this.textBoxExecutable.Name = "textBoxExecutable";
            this.textBoxExecutable.ReadOnly = true;
            this.textBoxExecutable.Size = new System.Drawing.Size(813, 26);
            this.textBoxExecutable.TabIndex = 1;
            // 
            // buttonOpenConEmuFilePath
            // 
            this.buttonOpenConEmuFilePath.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonOpenConEmuFilePath.Location = new System.Drawing.Point(911, 29);
            this.buttonOpenConEmuFilePath.MaximumSize = new System.Drawing.Size(50, 26);
            this.buttonOpenConEmuFilePath.Name = "buttonOpenConEmuFilePath";
            this.buttonOpenConEmuFilePath.Size = new System.Drawing.Size(50, 26);
            this.buttonOpenConEmuFilePath.TabIndex = 2;
            this.buttonOpenConEmuFilePath.Text = "...";
            this.buttonOpenConEmuFilePath.UseVisualStyleBackColor = true;
            this.buttonOpenConEmuFilePath.Click += new System.EventHandler(this.buttonOpenConEmuFilePath_Click);
            // 
            // labelExecutable
            // 
            this.labelExecutable.AutoSize = true;
            this.labelExecutable.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelExecutable.Location = new System.Drawing.Point(10, 29);
            this.labelExecutable.Name = "labelExecutable";
            this.labelExecutable.Size = new System.Drawing.Size(88, 20);
            this.labelExecutable.TabIndex = 0;
            this.labelExecutable.Text = "Executable";
            this.labelExecutable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBoxSettings
            // 
            this.groupBoxSettings.Controls.Add(this.panelXMLConfiguration);
            this.groupBoxSettings.Controls.Add(this.panelDefaultTask);
            this.groupBoxSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxSettings.Location = new System.Drawing.Point(0, 86);
            this.groupBoxSettings.Name = "groupBoxSettings";
            this.groupBoxSettings.Padding = new System.Windows.Forms.Padding(10);
            this.groupBoxSettings.Size = new System.Drawing.Size(971, 115);
            this.groupBoxSettings.TabIndex = 1;
            this.groupBoxSettings.TabStop = false;
            this.groupBoxSettings.Text = "Settings";
            // 
            // panelXMLConfiguration
            // 
            this.panelXMLConfiguration.Controls.Add(this.textBoxXMLConfiguration);
            this.panelXMLConfiguration.Controls.Add(this.buttonXMLConfiguration);
            this.panelXMLConfiguration.Controls.Add(this.buttonXMLConfigurationClear);
            this.panelXMLConfiguration.Controls.Add(this.labelXMLConfiguration);
            this.panelXMLConfiguration.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelXMLConfiguration.Location = new System.Drawing.Point(10, 66);
            this.panelXMLConfiguration.Name = "panelXMLConfiguration";
            this.panelXMLConfiguration.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.panelXMLConfiguration.Size = new System.Drawing.Size(951, 37);
            this.panelXMLConfiguration.TabIndex = 3;
            // 
            // textBoxXMLConfiguration
            // 
            this.textBoxXMLConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxXMLConfiguration.Location = new System.Drawing.Point(155, 5);
            this.textBoxXMLConfiguration.Name = "textBoxXMLConfiguration";
            this.textBoxXMLConfiguration.ReadOnly = true;
            this.textBoxXMLConfiguration.Size = new System.Drawing.Size(696, 26);
            this.textBoxXMLConfiguration.TabIndex = 1;
            // 
            // buttonXMLConfigurationClear
            // 
            this.buttonXMLConfigurationClear.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonXMLConfigurationClear.Location = new System.Drawing.Point(901, 5);
            this.buttonXMLConfigurationClear.MaximumSize = new System.Drawing.Size(50, 26);
            this.buttonXMLConfigurationClear.Name = "buttonXMLConfigurationClear";
            this.buttonXMLConfigurationClear.Size = new System.Drawing.Size(50, 26);
            this.buttonXMLConfigurationClear.TabIndex = 3;
            this.buttonXMLConfigurationClear.Text = "...";
            this.buttonXMLConfigurationClear.UseVisualStyleBackColor = true;
            this.buttonXMLConfigurationClear.Click += new System.EventHandler(this.buttonXMLConfigurationClear_Click);
            // 
            // labelXMLConfiguration
            // 
            this.labelXMLConfiguration.AutoSize = true;
            this.labelXMLConfiguration.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelXMLConfiguration.Location = new System.Drawing.Point(0, 5);
            this.labelXMLConfiguration.MinimumSize = new System.Drawing.Size(155, 0);
            this.labelXMLConfiguration.Name = "labelXMLConfiguration";
            this.labelXMLConfiguration.Size = new System.Drawing.Size(155, 20);
            this.labelXMLConfiguration.TabIndex = 0;
            this.labelXMLConfiguration.Text = "XML configuration";
            this.labelXMLConfiguration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelDefaultTask
            // 
            this.panelDefaultTask.Controls.Add(this.textBoxDefaultTask);
            this.panelDefaultTask.Controls.Add(this.labelDefaultTask);
            this.panelDefaultTask.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDefaultTask.Location = new System.Drawing.Point(10, 29);
            this.panelDefaultTask.Name = "panelDefaultTask";
            this.panelDefaultTask.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.panelDefaultTask.Size = new System.Drawing.Size(951, 37);
            this.panelDefaultTask.TabIndex = 2;
            // 
            // textBoxDefaultTask
            // 
            this.textBoxDefaultTask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDefaultTask.Location = new System.Drawing.Point(155, 5);
            this.textBoxDefaultTask.Name = "textBoxDefaultTask";
            this.textBoxDefaultTask.Size = new System.Drawing.Size(796, 26);
            this.textBoxDefaultTask.TabIndex = 1;
            // 
            // labelDefaultTask
            // 
            this.labelDefaultTask.AutoSize = true;
            this.labelDefaultTask.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelDefaultTask.Location = new System.Drawing.Point(0, 5);
            this.labelDefaultTask.MinimumSize = new System.Drawing.Size(155, 0);
            this.labelDefaultTask.Name = "labelDefaultTask";
            this.labelDefaultTask.Size = new System.Drawing.Size(155, 20);
            this.labelDefaultTask.TabIndex = 0;
            this.labelDefaultTask.Text = "Default Task";
            this.labelDefaultTask.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OpenXMLConfiguration
            // 
            this.OpenXMLConfiguration.DefaultExt = "xml";
            this.OpenXMLConfiguration.FileName = "ConEmu.xml";
            this.OpenXMLConfiguration.Filter = "ConEmu.xml|*.xml";
            this.OpenXMLConfiguration.ReadOnlyChecked = true;
            // 
            // buttonXMLConfiguration
            // 
            this.buttonXMLConfiguration.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonXMLConfiguration.Location = new System.Drawing.Point(851, 5);
            this.buttonXMLConfiguration.MaximumSize = new System.Drawing.Size(50, 26);
            this.buttonXMLConfiguration.Name = "buttonXMLConfiguration";
            this.buttonXMLConfiguration.Size = new System.Drawing.Size(50, 26);
            this.buttonXMLConfiguration.TabIndex = 4;
            this.buttonXMLConfiguration.Text = "...";
            this.buttonXMLConfiguration.UseVisualStyleBackColor = true;
            this.buttonXMLConfiguration.Click += new System.EventHandler(this.buttonXMLConfiguration_Click);
            // 
            // OptionPageGridConEmuPathsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxSettings);
            this.Controls.Add(this.GroupBoxConEmu);
            this.Name = "OptionPageGridConEmuPathsControl";
            this.Size = new System.Drawing.Size(971, 628);
            this.GroupBoxConEmu.ResumeLayout(false);
            this.GroupBoxConEmu.PerformLayout();
            this.groupBoxSettings.ResumeLayout(false);
            this.panelXMLConfiguration.ResumeLayout(false);
            this.panelXMLConfiguration.PerformLayout();
            this.panelDefaultTask.ResumeLayout(false);
            this.panelDefaultTask.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog OpenConEmuFilePath;
        private System.Windows.Forms.GroupBox GroupBoxConEmu;
        private System.Windows.Forms.Label labelExecutable;
        private System.Windows.Forms.TextBox textBoxExecutable;
        private System.Windows.Forms.Button buttonOpenConEmuFilePath;
        private System.Windows.Forms.GroupBox groupBoxSettings;
        private System.Windows.Forms.Panel panelXMLConfiguration;
        private System.Windows.Forms.TextBox textBoxXMLConfiguration;
        private System.Windows.Forms.Label labelXMLConfiguration;
        private System.Windows.Forms.Panel panelDefaultTask;
        private System.Windows.Forms.TextBox textBoxDefaultTask;
        private System.Windows.Forms.Label labelDefaultTask;
        private System.Windows.Forms.Button buttonXMLConfigurationClear;
        private System.Windows.Forms.OpenFileDialog OpenXMLConfiguration;
        private System.Windows.Forms.Button buttonXMLConfiguration;
    }
}
