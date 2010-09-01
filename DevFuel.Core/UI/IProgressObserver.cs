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

namespace DevFuel.Core.UI
{
    /// <summary>
    /// Represents an entity that can observe a sequence of tasks in a process
    /// </summary>
    public interface IProgressObserver
    {
        /// <summary>
        /// Sets the step label.
        /// </summary>
        /// <value>The step label.</value>
        string StepLabel { set; }
        /// <summary>
        /// Sets the task label.
        /// </summary>
        /// <value>The task label.</value>
        string TaskLabel { set; }
        /// <summary>
        /// Sets the step count.
        /// </summary>
        /// <value>The step count.</value>
        int StepCount { set; }
        /// <summary>
        /// Sets the step current.
        /// </summary>
        /// <value>The step current.</value>
        int StepCurrent { set; }
        /// <summary>
        /// Completes the task.
        /// </summary>
        void CompleteTask();
    }

    /// <summary>
    /// A Progress Observer that does nothing with progress information
    /// </summary>
    public class NullProgressObserver : IProgressObserver
    {
        #region IProgressObserver Members

        /// <summary>
        /// Sets the step label. Does nothing.
        /// </summary>
        /// <value>The step label.</value>
        public string StepLabel
        {
            set { }
        }

        /// <summary>
        /// Sets the task label. Does nothing.
        /// </summary>
        /// <value>The task label.</value>
        public string TaskLabel
        {
            set { }
        }

        /// <summary>
        /// Sets the step count. Does nothing.
        /// </summary>
        /// <value>The step count.</value>
        public int StepCount
        {
            set { }
        }

        /// <summary>
        /// Sets the current step. Does nothing.
        /// </summary>
        /// <value>The step current.</value>
        public int StepCurrent
        {
            set { }
        }

        /// <summary>
        /// Completes the task. Does nothing.
        /// </summary>
        public void CompleteTask()
        {
        }

        #endregion
    }
}
