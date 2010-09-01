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

namespace DevFuel.Core.UI.Forms
{
    /// <summary>
    /// Window to collect user input in a customizable manner
    /// </summary>
    public partial class InputBox : Form
    {
        /// <summary>
        /// Gets or sets the instructions.
        /// </summary>
        /// <value>The instructions.</value>
        public string Instructions
        {
            get { return lblInstructions.Text; }
            set { lblInstructions.Text = value; }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value
        {
            get { return tbValue.Text; }
            set { tbValue.Text = value; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="InputBox"/> class.
        /// </summary>
        public InputBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Shows the InputBox.
        /// </summary>
        /// <param name="sTitle">The title.</param>
        /// <param name="sInstructions">The instructions.</param>
        /// <param name="sValue">The default value.</param>
        /// <param name="owner">The owner.</param>
        /// <returns>The user's input</returns>
        public static string Show(string sTitle, string sInstructions, string sValue, IWin32Window owner)
        {
            InputBox f = new InputBox();
            f.Text = sTitle;
            f.Instructions = sInstructions;
            f.Value = sValue;
            if (f.ShowDialog(owner) == DialogResult.OK)
                return f.Value;
            return string.Empty;
        }

        private void InputBox_Load(object sender, EventArgs e)
        {
            if (HostApplicationResources.DialogIcon != null)
                this.Icon = HostApplicationResources.DialogIcon;
        }
    }
}