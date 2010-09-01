using System;
using System.Collections.Generic;
using System.Text;


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
    /// Defines a general method to test it an item should be included in a <see cref="BindingListView&lt;T&gt;"/>.
    /// </summary>
    /// <typeparam name="T">The type of item to be filtered.</typeparam>
    public interface IItemFilter<T>
    {
        /// <summary>
        /// Tests if the item should be included.
        /// </summary>
        /// <param name="item">The item to test.</param>
        /// <returns>True if the item should be included, otherwise false.</returns>
        bool Include(T item);
    }

    /// <summary>
    /// A dummy filter that is used when no filter is needed.
    /// It simply includes any and all items tested.
    /// </summary>
    public class IncludeAllItemFilter<T> : IItemFilter<T>
    {
        /// <summary>
        /// Tests if the item should be included.
        /// </summary>
        /// <param name="item">The item to test.</param>
        /// <returns>
        /// True if the item should be included, otherwise false.
        /// </returns>
        public bool Include(T item)
        {
            // All items are to be included.
            // So always return true.
            return true;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return Properties.Resources.BLV_NoFilter;
        }

        #region Singleton Accessor

        private static IncludeAllItemFilter<T> _instance;

        /// <summary>
        /// Gets the singleton instance of <see cref="IncludeAllItemFilter&lt;T&gt;"/>.
        /// </summary>
        /// <value>The instance.</value>
        public static IncludeAllItemFilter<T> Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new IncludeAllItemFilter<T>();
                }
                return _instance;
            }
        }

        #endregion
    }

    /// <summary>
    /// A filter that uses a user-defined <see cref="Predicate&lt;T&gt;"/> to test items for inclusion in <see cref="BindingListView&lt;T&gt;"/>.
    /// </summary>
    public class PredicateItemFilter<T> : IItemFilter<T>
    {
        /// <summary>
        /// Creates a new <see cref="PredicateItemFilter&lt;T&gt;"/> that uses the specified <see cref="Predicate&lt;T&gt;"/> and default name.
        /// </summary>
        /// <param name="includeDelegate">The <see cref="Predicate&lt;T&gt;"/> used to test items.</param>
        public PredicateItemFilter(Predicate<T> includeDelegate)
            : this(includeDelegate, null)
        {
            // The other constructor is called to do the work.
        }

        /// <summary>
        /// Creates a new <see cref="PredicateItemFilter&lt;T&gt;"/> that uses the specified <see cref="Predicate&lt;T&gt;"/>.
        /// </summary>
        /// <param name="includeDelegate">The <see cref="PredicateItemFilter&lt;T&gt;"/> used to test items.</param>
        /// <param name="name">The name used for the ToString() return value.</param>
        public PredicateItemFilter(Predicate<T> includeDelegate, string name)
        {
            // We don't allow a null string. Use the default instead.
            _name = name ?? defaultName;
            if (includeDelegate != null)
            {
                _includeDelegate = includeDelegate;
            }
            else
            {
                throw new ArgumentNullException("includeDelegate", Properties.Resources.BLV_IncludeDelegateCannotBeNull);
            }
        }

        private Predicate<T> _includeDelegate;
        private string _name;
        private readonly string defaultName = Properties.Resources.BLV_PredicateFilter;

        /// <summary>
        /// Tests if the item should be included.
        /// </summary>
        /// <param name="item">The item to test.</param>
        /// <returns>
        /// True if the item should be included, otherwise false.
        /// </returns>
        public bool Include(T item)
        {
            return _includeDelegate(item);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return _name;
        }
    }

    // TODO: Implement this class
    /*
    public class ExpressionItemFilter<T> : IItemFilter<T>
    {
        public ExpressionItemFilter(string expression)
        {
            // TODO: Parse expression into predicate
        }

        public bool Include(T item)
        {
            // TODO: use expression...
            return true;
        }
    }
    */

    // TODO: Implement this class
    /*
    public class CSharpItemFilter<T> : IItemFilter<T>
    {
        public CSharpItemFilter(string filterSourceCode)
        {
            
        }

        public bool Include(T item)
        {
            // TODO: implement this method...
            return true;
        }
    }
    */
}
