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
    /// Displays multiple messages to the user
    /// </summary>
    public partial class MessagesBox : Form
    {
        private List<string> m_MessageList;
        /// <summary>
        /// Gets or sets the message list.
        /// </summary>
        /// <value>The message list.</value>
        public List<string> MessageList
        {
            get { return m_MessageList; }
            set { 
                m_MessageList = value;
                tbxMessage.Text = string.Join(Environment.NewLine, value.ToArray());
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagesBox"/> class.
        /// </summary>
        public MessagesBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Shows the specified message list.
        /// </summary>
        /// <param name="messageList">The message list.</param>
        /// <param name="title">The title.</param>
        public static void Show(List<string> messageList, string title)
        {
            MessagesBox d = new MessagesBox();
            d.MessageList = messageList;
            d.Text = title;
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