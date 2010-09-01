using System;
using System.Collections.Generic;


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
    /// An Item Filter Implementation that provides a composite of multiple filters
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CompositeItemFilter<T> : IItemFilter<T>
    {
        private List<IItemFilter<T>> _filters;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeItemFilter&lt;T&gt;"/> class.
        /// </summary>
        public CompositeItemFilter()
        {
            _filters = new List<IItemFilter<T>>();
        }

        /// <summary>
        /// Adds the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public void AddFilter(IItemFilter<T> filter)
        {
            _filters.Add(filter);
        }

        /// <summary>
        /// Removes the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public void RemoveFilter(IItemFilter<T> filter)
        {
            _filters.Remove(filter);
        }

        /// <summary>
        /// Tests if the item should be included.
        /// </summary>
        /// <param name="item">The item to test.</param>
        /// <returns>
        /// True if the item should be included, otherwise false.
        /// </returns>
        public bool Include(T item)
        {
            foreach (IItemFilter<T> filter in _filters)
            {
                if (!filter.Include(item))
                {
                    return false;
                }
            }
            return true;
        }

    }
}
