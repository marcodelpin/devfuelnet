#region COPYRIGHT
/*
The following code was created
by E. Edwards of http://www.devfuel.com. It is released for use under the BSD license:
Copyright (c) 2007-2008, E. Edwards 

All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
    * Neither the name of devfuel.com nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace DevFuel.Core.UI.Forms
{
    /// <summary>
    /// Displays an exception to the user in a useful manner
    /// </summary>
    public partial class ExceptionBox : Form
    {
        private Exception m_Exception;
        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception
        {
            get { return m_Exception; }
            set { 
                m_Exception = value;
                tbxMessage.Text = m_Exception.Message;
                StringBuilder sb = new StringBuilder();
                Exception x = m_Exception.InnerException;
                while (x != null)
                {
                    sb.AppendLine(x.Message.Trim());
                    sb.AppendLine();
                    x = x.InnerException;
                }
                sb.AppendLine(m_Exception.StackTrace);
                tbxInner.Text = sb.ToString();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionBox"/> class.
        /// </summary>
        public ExceptionBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Shows the specified exception.
        /// </summary>
        /// <param name="x">The exception.</param>
        public static void Show(Exception x)
        {
            ExceptionBox d = new ExceptionBox();
            while (x is TargetInvocationException && x != null)
            {
                x = x.InnerException;
            }

            d.Exception = x;
            d.ShowDialog();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(tbxMessage.Text + Environment.NewLine + tbxInner.Text);
        }

        private void ExceptionBox_Load(object sender, EventArgs e)
        {
            if (HostApplicationResources.DialogIcon != null)
                this.Icon = HostApplicationResources.DialogIcon;
        }
    }
}