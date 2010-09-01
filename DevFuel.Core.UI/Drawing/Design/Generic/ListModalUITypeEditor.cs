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

/***
 *This code is based on ideas from:
 * http://www.codeproject.com/KB/cs/dzcollectioneditor.aspx
 * It has been adapted and mostly redesigned by E. Edwards  for use in this library.*/
using System;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel.Design;
using System.Collections.Generic;

namespace DevFuel.Core.UI.Drawing.Design.Generic
{
    /// <summary>
    /// Generic UITypeEditor that displays a list in a modal dialog
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public class ListModalUITypeEditor<T>: ModalUITypeEditor<ListModalUITypeEditorForm<T>,List<T>>
	{

        /// <summary>
        /// Delegate for when a collection has changed
        /// </summary>
		public delegate void CollectionChangedEventHandler(object sender, object instance, object value);
        /// <summary>
        /// Occurs when [collection changed].
        /// </summary>
		public event CollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListModalUITypeEditor&lt;T&gt;"/> class.
        /// </summary>
        public ListModalUITypeEditor()
		{
        }

        /// <summary>
        /// Called after the dialog is created.
        /// </summary>
        /// <param name="dlg">The dialog.</param>
        protected override void OnAfterDialogCreated(ListModalUITypeEditorForm<T> dlg)
        {
            dlg.ItemAdded += new ListModalUITypeEditorForm<T>.InstanceEventHandler(ItemAdded);
            dlg.ItemRemoved += new ListModalUITypeEditorForm<T>.InstanceEventHandler(ItemRemoved);
        }

        /// <summary>
        /// Called when [after dialog confirmed].
        /// </summary>
        /// <param name="dlg">The DLG.</param>
        protected override void OnAfterDialogConfirmed(ListModalUITypeEditorForm<T> dlg)
        {
            foreach (List<T> valueT in dlg.ValuesT)
                OnCollectionChanged(dlg.Context.Instance, valueT);
        }
		
		private void ItemAdded(object sender, T item)
		{	
			if(m_Context!=null && m_Context.Container!=null)
			{
				IComponent icomp=item as IComponent;
				if(icomp !=null )
				{	
					m_Context.Container.Add(icomp);									
				}
			}
		}
	
		private void ItemRemoved(object sender, T item)
		{			
			if(m_Context!=null && m_Context.Container!=null)
			{
				IComponent icomp=item as IComponent;
				if(icomp!=null)
				{						
					m_Context.Container.Remove(icomp);
				}
			}
		}

        /// <summary>
        /// Called when [collection changed].
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The value.</param>
		protected virtual void OnCollectionChanged(object instance ,object value)
		{
            if (CollectionChanged != null)
            {
                CollectionChanged(this, instance, value);
            }
		}

        /// <summary>
        /// Creates the form.
        /// </summary>
        /// <returns></returns>
		protected virtual ListModalUITypeEditorForm<T> CreateForm()
		{
			return new ListModalUITypeEditorForm<T>();
		}		
	}
}
