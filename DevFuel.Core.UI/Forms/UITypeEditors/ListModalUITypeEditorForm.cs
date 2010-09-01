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
using System.Collections;
using System.Windows.Forms.Design;

namespace DevFuel.Core.UI.Drawing.Design
{
    /// <summary>
    /// A ModalUITypeEditor form that displays a list
    /// </summary>
    public partial class ListModalUITypeEditorForm : Form
    {
        private BitArray m_ListPermissions = new BitArray(4, true);
        /// <summary>
        /// Gets or sets a value indicating whether [list is read only].
        /// </summary>
        /// <value><c>true</c> if [list is read only]; otherwise, <c>false</c>.</value>
        public bool ListIsReadOnly
        {
            get { return !(CanEditItems || CanMoveItems || CanAddItems || CanRemoveItems); }
            set { m_ListPermissions = new BitArray(4, false); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can edit items.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can edit items; otherwise, <c>false</c>.
        /// </value>
        public bool CanEditItems
        {
            get { return m_ListPermissions[0]; }
            set { m_ListPermissions[0] = gridSelection.Enabled = value;  }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can move items.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can move items; otherwise, <c>false</c>.
        /// </value>
        public bool CanMoveItems
        {
            get { return m_ListPermissions[1]; }
            set { m_ListPermissions[1] = btnMoveDown.Enabled = btnMoveUp.Enabled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can add items.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can add items; otherwise, <c>false</c>.
        /// </value>
        public bool CanAddItems
        {
            get { return m_ListPermissions[2]; }
            set { m_ListPermissions[2] = btnAdd.Enabled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can remove items.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can remove items; otherwise, <c>false</c>.
        /// </value>
        public bool CanRemoveItems
        {
            get { return m_ListPermissions[3]; }
            set { m_ListPermissions[3] = btnRemove.Enabled = value; }
        }

        /// <summary>
        /// Occurs when values have changed.
        /// </summary>
        public event EventHandler ValuesChanged;
        /// <summary>
        /// Called when [values changed].
        /// </summary>
        public void OnValuesChanged()
        {
            if (ValuesChanged != null)
                ValuesChanged(this, EventArgs.Empty);
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

        private ITypeDescriptorContext m_Context = null;
        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>The context.</value>
        public ITypeDescriptorContext Context
        {
            get { return m_Context; }
            set { m_Context = value; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ListModalUITypeEditorForm"/> class.
        /// </summary>
        public ListModalUITypeEditorForm()
        {
            InitializeComponent();
            
        }

        /// <summary>
        /// Handles the Load event of the ListEditorForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void ListEditorForm_Load(object sender, EventArgs e)
        {
            if (HostApplicationResources.DialogIcon != null)
                this.Icon = HostApplicationResources.DialogIcon;
            string itemName = GetDefaultItemName();
            string text = null;
            if (m_Context != null)
            {
                ListEditorSettingsAttribute a = m_Context.PropertyDescriptor.Attributes[typeof(ListEditorSettingsAttribute)] as ListEditorSettingsAttribute;
                if (a != null)
                {
                    if (!string.IsNullOrEmpty(a.Text))
                        text = a.Text;
                    if (!string.IsNullOrEmpty(a.ItemName))
                        itemName = a.Text;
                    CanAddItems = a.CanAddItems;
                    CanEditItems = a.CanEditItems;
                    CanRemoveItems = a.CanRemoveItems;
                    CanMoveItems = a.CanMoveItems;
                }
            }
            else //Default abilities
            {
                CanAddItems = true;
                CanRemoveItems = true;
                CanMoveItems = true;
                CanEditItems = true;
            }
            //No selection yet...disabled by default
            btnRemove.Enabled = false;
            btnMoveUp.Enabled = false; 
            btnMoveDown.Enabled = false;
            if (!string.IsNullOrEmpty(text))
                this.Text = text;
            else
                this.Text = string.Format(this.Text, itemName);
        }

        /// <summary>
        /// Gets the default name of the item.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetDefaultItemName()
        {
            return "Item";
        }


        /// <summary>
        /// Handles the Click event of the btnOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void btnOK_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void btnCancel_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Click event of the btnMoveDown control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void btnMoveDown_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Click event of the btnMoveUp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void btnMoveUp_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Click event of the btnRemove control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void btnRemove_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the ButtonClick event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void btnAdd_ButtonClick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lbxItems control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void lbxItems_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Format event of the lbxItems control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.ListControlConvertEventArgs"/> instance containing the event data.</param>
        protected virtual void lbxItems_Format(object sender, ListControlConvertEventArgs e)
        {
            
        }

        
    }

    /// <summary>
    /// 
    /// </summary>
    public class ListEditorSettingsAttribute : Attribute
    {
        private bool m_CanEditItems = true;
        /// <summary>
        /// Gets or sets a value indicating whether this instance can edit items.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can edit items; otherwise, <c>false</c>.
        /// </value>
        public bool CanEditItems
        {
            get { return m_CanEditItems; }
            set { m_CanEditItems = value; }
        }

        private bool m_CanMoveItems = true;
        /// <summary>
        /// Gets or sets a value indicating whether this instance can move items.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can move items; otherwise, <c>false</c>.
        /// </value>
        public bool CanMoveItems
        {
            get { return m_CanMoveItems; }
            set { m_CanMoveItems = value; }
        }

        private bool m_CanAddItems = true;
        /// <summary>
        /// Gets or sets a value indicating whether this instance can add items.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can add items; otherwise, <c>false</c>.
        /// </value>
        public bool CanAddItems
        {
            get { return m_CanAddItems; }
            set { m_CanAddItems = value; }
        }

        private bool m_CanRemoveItems = true;
        /// <summary>
        /// Gets or sets a value indicating whether this instance can remove items.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can remove items; otherwise, <c>false</c>.
        /// </value>
        public bool CanRemoveItems
        {
            get { return m_CanRemoveItems; }
            set { m_CanRemoveItems = value; }
        }

        private string m_Text;
        /// <summary>
        /// Gets or sets the Form.Text property for the list editor.
        /// </summary>
        /// <value>The text.</value>
        /// Updated 1/17/2008 by ee
        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        private string m_ItemName;
        /// <summary>
        /// Gets or sets the displayed name of an item in the list.
        /// </summary>
        /// <value>The name of the item.</value>
        /// Updated 1/17/2008 by ee
        public string ItemName
        {
            get { return m_ItemName; }
            set { m_ItemName = value; }
        }
    }
}