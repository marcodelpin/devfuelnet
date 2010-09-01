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
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Collections;
using DevFuel.Core.ComponentModel;

namespace DevFuel.Core.UI.Drawing.Design.Generic
{
    /// <summary>
    /// A generic form for use in a ModalUITypeEditor that lists objects of type T
    /// </summary>
    /// <typeparam name="T">The type of object to list</typeparam>
    public class ListModalUITypeEditorForm<T> : ListModalUITypeEditorForm, IUITypeEditable<List<T>>
    {
        /// <summary>
        /// Notification of an event on an object instance
        /// </summary>
        public delegate void InstanceEventHandler(object sender, T instance);
        /// <summary>
        /// Occurs when [instance created].
        /// </summary>
        public event InstanceEventHandler InstanceCreated;
        /// <summary>
        /// Occurs when [destroying instance].
        /// </summary>
        public event InstanceEventHandler DestroyingInstance;
        /// <summary>
        /// Occurs when [item removed].
        /// </summary>
        public event InstanceEventHandler ItemRemoved;
        /// <summary>
        /// Occurs when [item added].
        /// </summary>
        public event InstanceEventHandler ItemAdded;

        private List<T> m_CacheList = null;
        /// <summary>
        /// The attached editor
        /// </summary>
        protected ListModalUITypeEditor<T> m_AttachedEditor = null;
        private List<List<T>> m_ValuesT;
        private BindingListView<T> m_List = null;
        /// <summary>
        /// Gets or sets the values as a strongly typed enumeration.
        /// </summary>
        /// <value>The values T.</value>
        public IEnumerable<List<T>> ValuesT
        {
            get { return m_ValuesT; }
            set { m_ValuesT = new List<List<T>>(value); BuildMetaCollection(); }
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <value>The list.</value>
        public IList<T> List
        {
            get { return m_List.SourceLists[0] as IList<T>; }
        }


        private void BuildMetaCollection()
        {
            //For now, only handle a single list at at time
            foreach (List<T> list in ValuesT)
            {
                m_List = new BindingListView<T>(list);
                ProcessCollection(list);
                lbxItems.DataSource = m_List;
                return;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListModalUITypeEditorForm&lt;T&gt;"/> class.
        /// </summary>
        public ListModalUITypeEditorForm()
            : base()
        {
            
        }

        /// <summary>
        /// Handles the Format event of the lbxItems control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.ListControlConvertEventArgs"/> instance containing the event data.</param>
        protected override void lbxItems_Format(object sender, ListControlConvertEventArgs e)
        {
            if (e.DesiredType == typeof(string) && e.ListItem is ObjectView<T>)
            {
                e.Value = (e.ListItem as ObjectView<T>).Object.ToString();
            }
        }

        /// <summary>
        /// Gets the default name of the item.
        /// </summary>
        /// <returns></returns>
        protected override string GetDefaultItemName()
        {
            return typeof(T).Name;
        }

        /// <summary>
        /// Handles the Load event of the ListEditorForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void ListEditorForm_Load(object sender, EventArgs e)
        {
            base.ListEditorForm_Load(sender, e);
            lbxItems.DataSource = null;
        }

        /// <summary>
        /// Gets the data types that this collection editor can contain
        /// </summary>
        /// <param name="coll">The collection for which to return the available types</param>
        /// <returns>An array of data types that this collection can contain.</returns>
        protected virtual Type[] CreateNewItemTypes(IList<T> coll)
        {
            return new Type[] { typeof(T) };
        }

        /// <summary>
        /// Creates a new instance of the specified collection item type.
        /// </summary>
        /// <param name="itemType">The type of item to create. </param>
        /// <returns>A new instance of the specified object.</returns>
        protected virtual T CreateInstance(Type itemType)
        {
            if (!typeof(T).IsAssignableFrom(itemType))
                throw new Exception(string.Format("Type '{0}' Must be assignable from Type '{1}'", typeof(T).Name, itemType.Name));
            T instance = (T)Activator.CreateInstance(itemType, false);
            OnInstanceCreated(instance);
            return instance;
        }

        /// <summary>
        /// Destroys the specified instance of the object.
        /// </summary>
        /// <param name="instance">The object to destroy. </param>
        protected virtual void DestroyInstance(T instance)
        {
            OnDestroyingInstance(instance);
            if (instance is IDisposable) { ((IDisposable)instance).Dispose(); }
            instance = default(T);
        }


        /// <summary>
        /// Called when [destroying instance].
        /// </summary>
        /// <param name="instance">The instance.</param>
        protected virtual void OnDestroyingInstance(T instance)
        {
            if (DestroyingInstance != null)
            {
                DestroyingInstance(this, instance);
            }
        }


        /// <summary>
        /// Called when [instance created].
        /// </summary>
        /// <param name="instance">The instance.</param>
        protected virtual void OnInstanceCreated(T instance)
        {
            if (InstanceCreated != null)
            {
                InstanceCreated(this, instance);
            }
        }

        /// <summary>
        /// Called when [item removed].
        /// </summary>
        /// <param name="item">The item.</param>
        protected virtual void OnItemRemoved(T item)
        {
            if (ItemRemoved != null)
            {
                ItemRemoved(this, item);
            }
        }

        /// <summary>
        /// Called when [item added].
        /// </summary>
        /// <param name="Item">The item.</param>
        protected virtual void OnItemAdded(T Item)
        {
            if (ItemAdded != null)
            {
                ItemAdded(this, Item);
            }
        }

        private void MoveItem(IList<T> list, int index, int step)
        {
            if (index < 0 || index >= list.Count)
                return;
            
            int newIndex = index + step;
            if (newIndex < 0 || newIndex > list.Count)
                return;
            
            T temp = list[newIndex];
            list[newIndex] = list[index];
            list[index] = temp;
            temp = default(T);
        }

        /// <summary>
        /// Refreshes the available types.
        /// </summary>
        /// <param name="collection">The collection.</param>
        protected virtual void RefreshAvailableTypes(IList<T> collection)
        {
            btnAdd.DropDownItems.Clear();
            btnAdd.DefaultItem = null;
            foreach (Type type in CreateNewItemTypes(collection))
            {
                ToolStripButton btnTypeAdd = new ToolStripButton(type.Name);
                btnTypeAdd.Tag = type;
                btnTypeAdd.Click += new EventHandler(btnTypeAdd_Click);
                btnAdd.DropDownItems.Add(btnTypeAdd);
                if (btnAdd.DefaultItem == null)
                    btnAdd.DefaultItem = btnTypeAdd;
            }
            btnAdd.DropDown.Width = CalculateDDListWidth();
        }

        void btnTypeAdd_Click(object sender, EventArgs e)
        {
            ToolStripButton btn = sender as ToolStripButton;
            lbxItems.BeginUpdate();
            if (List != null && btn != null)
            {
                //create a new item to add to the List and a corespondent TreeNodeEx to add to the treeview nodes
                Type type = (Type)btn.Tag;
                T newItem = CreateInstance(type);

                //get the current  possition  and the parent collections to insert into
                T selItem = (T)lbxItems.SelectedItem;

                if (selItem != null && lbxItems.SelectedIndex != lbxItems.Items.Count - 1)
                {
                    int position = lbxItems.SelectedIndex + 1;
                    List.Insert(position, newItem);
                    lbxItems.Items.Insert(position, newItem);
                }
                else //empty collection
                {
                    List.Add(newItem);
                    lbxItems.Items.Add(newItem);
                }

                OnItemAdded(newItem);
                lbxItems.SelectedItem = newItem;

            }
            lbxItems.EndUpdate();
        }

        private void ProcessCollection(IList<T> collection)
        {
            RefreshAvailableTypes(collection);
        }

        private int CalculateDDListWidth()
        {
            int width = 0;
            Graphics g = btnAdd.DropDown.CreateGraphics();

            foreach (ToolStripItem item in this.btnAdd.DropDownItems)
            {
                int itemWidth = (int)g.MeasureString(item.Text, btnAdd.DropDown.Font).Width;
                width = Math.Max(width, itemWidth);
            }
            width = Math.Max(width, btnAdd.Width + 20);
            return width;
        }

        /// <summary>
        /// Handles the Click event of the btnOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void btnOK_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void btnCancel_Click(object sender, System.EventArgs e)
        {
            UndoChanges(m_CacheList, List);
            this.Close();

        }

        /// <summary>
        /// Handles the Click event of the btnRemove control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void btnRemove_Click(object sender, System.EventArgs e)
        {
            lbxItems.BeginUpdate();
            T selItem = (T)lbxItems.SelectedItem;
            if (selItem != null)
            {
                int selIndex = lbxItems.SelectedIndex;

                lbxItems.Items.Remove(selItem);
                List.Remove(selItem);
                if (lbxItems.Items.Count > selIndex) { lbxItems.SelectedItem = lbxItems.Items[selIndex]; }
                else if (lbxItems.Items.Count > 0) { lbxItems.SelectedItem = lbxItems.Items[selIndex - 1]; }
                else { this.gridSelection.SelectedObject = null; }

                OnItemRemoved(selItem);
            }
            lbxItems.EndUpdate();
        }


        /// <summary>
        /// Handles the Click event of the btnMoveUp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void btnMoveUp_Click(object sender, System.EventArgs e)
        {
            lbxItems.BeginUpdate();
            T selItem = (T)lbxItems.SelectedItem;
            if (selItem != null)
            {
                int prevIndex = lbxItems.SelectedIndex - 1;
                MoveItem(List, List.IndexOf(selItem), -1);
                lbxItems.Items.Clear();
                lbxItems.Items.AddRange(ToObjectArray(this.List));
                lbxItems.SelectedItem = lbxItems.Items[prevIndex];
            }
            lbxItems.EndUpdate();
        }


        /// <summary>
        /// Handles the Click event of the btnMoveDown control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void btnMoveDown_Click(object sender, System.EventArgs e)
        {
            lbxItems.BeginUpdate();
            T selItem = (T)lbxItems.SelectedItem;
            if (selItem != null)
            {
                int nextIndex = lbxItems.SelectedIndex + 1;

                MoveItem(List, List.IndexOf(selItem), 1);
                lbxItems.Items.Clear();
                lbxItems.Items.AddRange(ToObjectArray(this.List));
                lbxItems.SelectedItem = lbxItems.Items[nextIndex];
            }
            lbxItems.EndUpdate();
        }

        private object[] ToObjectArray(IList<T> iList)
        {
            if (iList == null || iList.Count < 1)
                return new object[] { };
            object[] items = new object[iList.Count];
            int i = 0;
            foreach (T item in iList)
            {
                items[i++] = item;
            }
            return items;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lbxItems control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void lbxItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxItems.SelectedItems.Count > 1)
            {
                btnMoveDown.Enabled = false;
                btnMoveUp.Enabled = false;
                btnRemove.Enabled = CanRemoveItems;
                object[] selected = new object[lbxItems.SelectedItems.Count];
                int i = 0;
                foreach (T item in lbxItems.SelectedItems)
                {
                    selected[i++] = item;
                }
                gridSelection.SelectedObjects = selected;
            }
            else if (lbxItems.SelectedItems.Count == 1)
            {
                btnMoveDown.Enabled = (lbxItems.SelectedIndex < m_List.Count - 1) && CanMoveItems;
                btnMoveUp.Enabled = (lbxItems.SelectedIndex > 0) && CanMoveItems;
                btnRemove.Enabled = CanRemoveItems;
                gridSelection.SelectedObject = lbxItems.SelectedItem;
            }
            else
            {
                btnMoveDown.Enabled = false;
                btnMoveUp.Enabled = false;
                btnRemove.Enabled = false;
                gridSelection.SelectedObject = null;
            }
        }


        //protected override void gridSelection_PropertyValueChanged(object schemaSet, System.Windows.Forms.PropertyValueChangedEventArgs e)
        //{
        //    lbxItems.BeginUpdate();
        //    T selItem = (T)lbxItems.SelectedItem;

        //    //SetProperties(selItem, selItem);

        //    lbxItems.EndUpdate();
        //}


        //protected override void lbxItems_BeforeSelect(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
        //{
        //    gridSelection.SelectedObject = ((T)e.Node).Value;
        //}

        //protected override void lbxItems_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        //{
        //    T ti = (T)e.Node;
        //    if (ti.Value.GetType() != lastItemType)
        //    {
        //        lastItemType = ti.Value.GetType();
        //        IList<T> coll;
        //        if (ti.Parent != null) { coll = ((T)ti.Parent).SubItems; }
        //        else { coll = List; }
        //        ProcessCollection(coll);

        //    }
        //}


        //protected override void gridSelection_SelectedGridItemChanged(object sender, System.Windows.Forms.SelectedGridItemChangedEventArgs e)
        //{

        //    if (m_AttachedEditor != null)
        //    {
        //        m_AttachedEditor.CollectionChanged -= new CollectionEditorUITypeEditor<T>.CollectionChangedEventHandler(ValChanged);
        //        m_AttachedEditor = null;
        //    }

        //    if (e.NewSelection.Value is IList<T>)
        //    {
        //        m_AttachedEditor = (CollectionEditorUITypeEditor<T>)e.NewSelection.PropertyDescriptor.GetEditor(typeof(System.Drawing.Design.UITypeEditor)) as CollectionEditorUITypeEditor<T>;
        //        if (m_AttachedEditor != null)
        //        {
        //            m_AttachedEditor.CollectionChanged += new CollectionEditorUITypeEditor<T>.CollectionChangedEventHandler(ValChanged);
        //        }
        //    }



        //}


        private void ValChanged(object sender, object instance, object value)
        {
            lbxItems.BeginUpdate();
            T ti = (T)lbxItems.SelectedItem;
            //SetProperties(ti, instance);
            lbxItems.EndUpdate();
        }

        private void UndoChanges(IList<T> source, IList<T> dest)
        {
            foreach (T o in dest)
            {
                if (!source.Contains(o))
                {
                    DestroyInstance(o);
                    OnItemRemoved(o);
                }

            }

            dest.Clear();
            CopyItems(source, dest);
        }

        private void CopyItems(IList<T> source, IList<T> dest)
        {
            foreach (T o in source)
            {
                dest.Add(o);
                OnItemAdded(o);
            }
        }

        #region IUITypeEditable<T> Members

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>The values.</value>
        public IEnumerable Values
        {
            get
            {
                return ValuesT;
            }
            set
            {
                ValuesT = new List<List<T>>(UITypeEditorEx.Specialize<List<T>>(value));
            }
        }

        #endregion

        private void InitializeComponent()
        {
            this.tscMain.ContentPanel.SuspendLayout();
            this.tscMain.SuspendLayout();
            this.splitter.Panel1.SuspendLayout();
            this.splitter.Panel2.SuspendLayout();
            this.splitter.SuspendLayout();
            this.SuspendLayout();
            // 
            // tscMain
            // 
            // 
            // tscMain.ContentPanel
            // 
            this.tscMain.ContentPanel.Size = new System.Drawing.Size(592, 316);
            // 
            // splitter
            // 
            this.splitter.Size = new System.Drawing.Size(572, 306);
            // 
            // lbxItems
            // 
            this.lbxItems.Size = new System.Drawing.Size(275, 303);
            // 
            // gridSelection
            // 
            this.gridSelection.Size = new System.Drawing.Size(287, 306);
            // 
            // ListEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(592, 366);
            this.Name = "ListEditorForm";
            this.Text = "Item List Editor";
            this.tscMain.ContentPanel.ResumeLayout(false);
            this.tscMain.ResumeLayout(false);
            this.tscMain.PerformLayout();
            this.splitter.Panel1.ResumeLayout(false);
            this.splitter.Panel2.ResumeLayout(false);
            this.splitter.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
