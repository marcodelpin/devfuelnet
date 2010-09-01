using System;
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
    /// Extends <see cref="System.ComponentModel.IEditableObject"/> by providing events to raise during edit state changes.
    /// </summary>
    internal interface INotifyingEditableObject : IEditableObject
    {
        /// <summary>
        /// An edit has started on the object.
        /// </summary>
        /// <remarks>
        /// This event should be raised from BeginEdit().
        /// </remarks>
        event EventHandler EditBegun;
        /// <summary>
        /// The editing of the object was cancelled.
        /// </summary>
        /// <remarks>
        /// This event should be raised from CancelEdit().
        /// </remarks>
        event EventHandler EditCancelled;
        /// <summary>
        /// The editing of the object was ended.
        /// </summary>
        /// <remarks>
        /// This event should be raised from EndEdit().
        /// </remarks>
        event EventHandler EditEnded;
    }
}
