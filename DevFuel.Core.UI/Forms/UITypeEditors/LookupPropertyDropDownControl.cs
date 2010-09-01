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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevFuel.Core.ComponentModel;
using System.Windows.Forms.Design;
using System.Collections;
using DevFuel.Core.UI.Drawing.Design.Generic;
using DevFuel.Core.UI.Drawing.Design;

namespace DevFuel.Core.UI.Forms
{
    /// <summary>
    /// A drop down control (For use with the DropDownUITypeEditor) that displays the possible values on an ILookupProperty
    /// </summary>
    public partial class LookupPropertyDropDownControl : UserControl, IDropDownUITypeEditable
    {
        /// <summary>
        /// Occurs when the Values enumeration changes.
        /// </summary>
        public event EventHandler ValuesChanged;
        /// <summary>
        /// Raises the <see cref="E:ValuesChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnValuesChanged(EventArgs e)
        {
            if (ValuesChanged != null)
            {
                ValuesChanged(this, e);
            }
        }

        private List<ILookupProperty> m_Properties = new List<ILookupProperty>();
        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>The properties.</value>
        public List<ILookupProperty> Properties
        {
            get { return m_Properties; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupPropertyDropDownControl"/> class.
        /// </summary>
        public LookupPropertyDropDownControl()
        {
            InitializeComponent();
        }

        private void lbxOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxOptions.SelectedItem is DataRowView)
            {
                foreach (ILookupProperty p in Properties)
                {
                    p.Value = (lbxOptions.SelectedItem as DataRowView).Row;
                }
                m_EditorService.CloseDropDown();
            }
        }

        private void lbxOptions_Format(object sender, ListControlConvertEventArgs e)
        {
            if (e.ListItem is DataRowView)
            {
                e.Value = (e.ListItem as DataRowView).Row.ToString();
            }
        }

        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            object o = Properties[0].CreateNewValue();
            if (o != null)
            {
                foreach (ILookupProperty p in Properties)
                {
                    p.Value = o;
                }
                m_EditorService.CloseDropDown();
            }
           
        }

        private void btnNone_Click(object sender, EventArgs e)
        {
            foreach (ILookupProperty p in Properties)
            {
                p.Value = null;
            }
            m_EditorService.CloseDropDown();
        }

        private void contextMnu_Opening(object sender, CancelEventArgs e)
        {
           
        }

        private void miEdit_Click(object sender, EventArgs e)
        {
            //if (lbxOptions.SelectedItem != null)
            //{
            //    m_Property.EditValue(lbxOptions.SelectedItem);
            //}
        }

        private void miDelete_Click(object sender, EventArgs e)
        {
            //if (lbxOptions.SelectedItem != null)
            //{
            //    if (DialogResult.Yes == MessageBox.Show("Are you sure you want to delete \"\"?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            //        m_Property.DeleteValue(lbxOptions.SelectedItem);
            //}
        }

        private void lbxOptions_MouseClick(object sender, MouseEventArgs e)
        {
            int index = lbxOptions.IndexFromPoint(e.Location);
            if (index >= 0)
            {

            }
        }

        private IWindowsFormsEditorService m_EditorService;
        /// <summary>
        /// Gets or sets the editor service.
        /// </summary>
        /// <value>The editor service.</value>
        public IWindowsFormsEditorService EditorService
        {
            get { return m_EditorService; }
            set { m_EditorService = value; }
        }

        private ITypeDescriptorContext m_Context;
        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>The context.</value>
        public ITypeDescriptorContext Context
        {
            get { return m_Context; }
            set { m_Context = value; }
        }

        IEnumerable IUITypeEditable.Values
        {
            get
            {
                return m_Properties;
            }
            set
            {
                m_Properties = new List<ILookupProperty>(UITypeEditorEx.Specialize<ILookupProperty>(value));
                InitLookup();
            }
        }

        private void InitLookup()
        {
            ILookupProperty prop = null;
            if (Properties.Count > 0)
                prop = Properties[0];
            else
            {
                lbxOptions.DataSource = null;
                return;
            }

            object selection = prop.Value;
            foreach (ILookupProperty p in Properties)
            {
                if (selection == DBNull.Value)
                {
                    selection = p.Value;
                }
                else if (selection != null)
                {
                    if (!selection.Equals(p.Value))
                    {
                        selection = null;
                    }
                }
            }
            lbxOptions.DataSource = prop.BindingSource;
            int i = 0;
            foreach (DataRowView o in lbxOptions.Items)
            {
                if (o.Row.Equals(selection))
                {
                    lbxOptions.SelectedIndex = i;
                    break;
                }
                i++;
            }
            btnAdd.Enabled = prop.AllowNew;
            btnNone.Enabled = prop.AllowNull;
            miDelete.Enabled = prop.AllowDelete;
            miEdit.Enabled = prop.AllowEdit;
            
            lbxOptions.SelectedIndexChanged += new EventHandler(lbxOptions_SelectedIndexChanged);
        }
    }
}
