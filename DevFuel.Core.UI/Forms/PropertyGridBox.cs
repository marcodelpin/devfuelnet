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
    /// Allow the user to view and edit the public properties of any object
    /// </summary>
    public partial class PropertyGridBox : Form
    {
        private object m_Value;
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public object Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        private string m_Caption;
        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption
        {
            get { return m_Caption; }
            set { m_Caption = value; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyGridBox"/> class.
        /// </summary>
        public PropertyGridBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Shows the specified object.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="value">The value.</param>
        /// <param name="caption">The caption.</param>
        /// <returns></returns>
        public static DialogResult Show(IWin32Window owner, object value, string caption)
        {
            PropertyGridBox dlg = new PropertyGridBox();
            dlg.Caption = caption;
            dlg.Value = value;
            return dlg.ShowDialog(owner);
        }

        private void PropertyGridBox_Load(object sender, EventArgs e)
        {
            if (HostApplicationResources.DialogIcon != null)
                this.Icon = HostApplicationResources.DialogIcon;
            this.Text = m_Caption;
            propGridNewObject.SelectedObject = m_Value;
            propGridNewObject.Focus();
            propGridNewObject.GetNextControl(null, true);
            if (m_Value is IEditableObject)
            {
                (m_Value as IEditableObject).BeginEdit();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (m_Value is IEditableObject)
            {
                (m_Value as IEditableObject).EndEdit();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (m_Value is IEditableObject)
            {
                (m_Value as IEditableObject).CancelEdit();
            }
        }
    }
}