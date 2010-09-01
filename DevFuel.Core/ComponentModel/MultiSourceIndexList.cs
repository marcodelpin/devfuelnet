using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;

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
    internal class MultiSourceIndexList<T> : List<KeyValuePair<ListItemPair<T>, int>>
    {
        public void Add(IList sourceList, ObjectView<T> item, int index)
        {
            Add(new KeyValuePair<ListItemPair<T>, int>(new ListItemPair<T>(sourceList, item), index));
        }

        /// <summary>
        /// Searches for a given source index value, returning the list index of the value.
        /// </summary>
        /// <param name="sourceList">The source list.</param>
        /// <param name="sourceIndex">The source index to find.</param>
        /// <returns>
        /// Returns the index in this list of the source index, or -1 if not found.
        /// </returns>
        public int IndexOfSourceIndex(IList sourceList, int sourceIndex)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].Key.List == sourceList && this[i].Value == sourceIndex)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Searches for a given item, returning the index of the value in this list.
        /// </summary>
        /// <param name="item">The <typeparamref name="T"/> item to search for.</param>
        /// <returns>
        /// Returns the index in this list of the item, or -1 if not found.
        /// </returns>
        public int IndexOfItem(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].Key.Item.Object.Equals(item) && this[i].Value > -1)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Searches for a given item'schemaSet <see cref="ObjectView&lt;T&gt;"/> wrapper, returning the index of the value in this list.
        /// </summary>
        /// <param name="item">The <see cref="ObjectView&lt;T&gt;"/> to search for.</param>
        /// <returns>Returns the index in this list of the item, or -1 if not found.</returns>
        public int IndexOfKey(ObjectView<T> item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].Key.Item.Equals(item) && this[i].Value > -1)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Checks if the list contains a given item.
        /// </summary>
        /// <param name="item">The <typeparamref name="T"/> item to check for.</param>
        /// <returns>True if the item is contained in the list, otherwise false.</returns>
        public bool ContainsItem(T item)
        {
            return (IndexOfItem(item) != -1);
        }

        /// <summary>
        /// Checks if the list contains a given <see cref="ObjectView&lt;T&gt;"/> key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// True if the key is contained in the list, otherwise false.
        /// </returns>
        public bool ContainsKey(ObjectView<T> key)
        {
            return (IndexOfKey(key) != -1);
        }

        /// <summary>
        /// Returns an array of all the <see cref="ObjectView&lt;T&gt;"/> keys in the list.
        /// </summary>
        public ObjectView<T>[] Keys
        {
            get
            {
                return ConvertAll<ObjectView<T>>(new Converter<KeyValuePair<ListItemPair<T>, int>, ObjectView<T>>(
                    delegate(KeyValuePair<ListItemPair<T>, int> kvp)
                    { return kvp.Key.Item; }
                )).ToArray();
            }
        }

        /// <summary>
        /// Returns an <see cref="IEnumerator&lt;T&gt;"/> to iterate over all the keys in this list.
        /// </summary>
        /// <returns>The <see cref="IEnumerator&lt;T&gt;"/> to use.</returns>
        public IEnumerator<ObjectView<T>> GetKeyEnumerator()
        {
            foreach (KeyValuePair<ListItemPair<T>, int> kvp in this)
            {
                yield return kvp.Key.Item;
            }
        }
    }

    internal class ListItemPair<T>
    {
        private IList _list;
        private ObjectView<T> _item;

        public ListItemPair(IList list, ObjectView<T> item)
        {
            _list = list;
            _item = item;
        }

        public IList List
        {
            get
            {
                return _list;
            }
        }

        public ObjectView<T> Item
        {
            get
            {
                return _item;
            }
        }
    }
}
