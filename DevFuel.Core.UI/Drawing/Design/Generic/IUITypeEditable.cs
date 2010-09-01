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
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Collections;

namespace DevFuel.Core.UI.Drawing.Design.Generic
{
    /// <summary>
    /// An interface that, when implemented by a type, allows for advanced editing of that type's instances with UITypeEditorEx
    /// </summary>
    public interface IUITypeEditable
    {
        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>The values.</value>
        IEnumerable Values { get; set;}
        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>The context.</value>
        ITypeDescriptorContext Context { get; set;}
        /// <summary>
        /// Occurs when the Values enumeration changes.
        /// </summary>
        event EventHandler ValuesChanged;
    }

    /// <summary>
    /// A templated version of IUITypeEditable
    /// </summary>
    public interface IUITypeEditable<T> : IUITypeEditable
    {
        /// <summary>
        /// Gets or sets the values as a strongly typed enumeration.
        /// </summary>
        /// <value>The values T.</value>
        IEnumerable<T> ValuesT { get; set;}
    }
    
    /// <summary>
    /// An extended version of IUITypeEditable that facilitates the DropDownUITypeEditor class
    /// </summary>
    public interface IDropDownUITypeEditable : IUITypeEditable
    {
        /// <summary>
        /// Gets or sets the editor service.
        /// </summary>
        /// <value>The editor service.</value>
        IWindowsFormsEditorService EditorService { get; set;}
    }

    /// <summary>
    /// A templated version of IDropDownUITypeEditable
    /// </summary>
    public interface IDropDownUITypeEditable<T> : IDropDownUITypeEditable
    {
        /// <summary>
        /// Gets or sets the values as a strongly typed enumeration.
        /// </summary>
        /// <value>The values T.</value>
        IEnumerable<T> ValuesT { get; set; }
    }
}

