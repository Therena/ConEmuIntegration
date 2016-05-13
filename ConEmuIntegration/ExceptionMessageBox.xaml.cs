//
// Copyright 2016 David Roller
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConEmuIntegration
{
    /// <summary>
    /// Interaction logic for ExceptionMessageBox.xaml
    /// </summary>
    public partial class ExceptionMessageBox : Window
    {
        public string Message { get; private set; }

        public ExceptionMessageBox()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public void SetException(Exception exc)
        {
            this.Message = exc.Message;

            string text = "";
            Exception inner = exc;
            while (inner != null)
            {
                text = inner.Message + Environment.NewLine +
                    inner.StackTrace + Environment.NewLine;
                inner = inner.InnerException;
            }
            rtbStackTrace.Document.Blocks.Clear();
            rtbStackTrace.Document.Blocks.Add(new Paragraph(new Run(text)));
        }

        public void SetException(string message, string details)
        {
            this.Message = message;
            rtbStackTrace.Document.Blocks.Clear();
            rtbStackTrace.Document.Blocks.Add(new Paragraph(new Run(details)));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
