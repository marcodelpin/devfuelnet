using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;


#region COPYRIGHT
/*
The following code originated from blw.sourceforge.net and has been adapted 
by E. Edwards of http://www.devfuel.com in order to provide additional features. It remains under the BSD license:
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

namespace DevFuel.Core.ComponentModel
{
    /// <summary>
    /// A searchable, sortable, filterable, data bindable view of a list of objects.
    /// </summary>
    /// <typeparam name="T">The type of object in the list.</typeparam>
    public class BindingListView<T> : AggregateBindingListView<T>
    {
        /// <summary>
        /// Creates a new <see cref="BindingListView&lt;T&gt;"/> of a given IBindingList.
        /// All items in the list must be of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="list">The list of objects to base the view on.</param>
        public BindingListView(IList list)
            : base()
        {
            DataSource = list;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingListView&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public BindingListView(IContainer container)
            : base(container)
        {
            DataSource = null;
        }

        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        [DefaultValue(null)]
        [AttributeProvider(typeof(IListSource))]
        public IList DataSource
        {
            get
            {
                IEnumerator<IList> e = GetDataSources().GetEnumerator();
                e.MoveNext();
                return e.Current;
            }
            set
            {
                if (value == null)
                {
                    // Clear all current data
                    //SourceLists = new BindingList<IList<T>>();
                    NewItemsList = null;
                    FilterAndSort();
                    OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
                    return;
                }

                if (!(value is ICollection<T>))
                {
                    // list is not a strongy-type collection.
                    // Check that items in list are all of type T
                    foreach (object item in value)
                    {
                        if (!(item is T))
                        {
                            throw new ArgumentException(string.Format(Properties.Resources.BLV_InvalidListItemType, typeof(T).FullName), "DataSource");
                        }
                    }
                }

                //SourceLists = new object[] { value };
                NewItemsList = value;
            }
        }

        private bool ShouldSerializeDataSource()
        {
            return (SourceLists.Count > 0);
        }

        /// <summary>
        /// Event handler for when SourceLists is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void SourceListsChanged(object sender, ListChangedEventArgs e)
        {
            if ((SourceLists.Count > 1 && e.ListChangedType == ListChangedType.ItemAdded) || e.ListChangedType == ListChangedType.ItemDeleted)
            {
                throw new Exception("BindingListView allows strictly one source list.");
            }
            else
            {
                base.SourceListsChanged(sender, e);
            }
        }
    }
}
