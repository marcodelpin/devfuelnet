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
using System.Reflection;

namespace DevFuel.Core
{
    /// <summary>
    /// A single Exception that represents a List of Exceptions aggregated over time.
    /// </summary>
    public class AggregateException : Exception
    {
        private List<Exception> m_Exceptions = new List<Exception>();
        /// <summary>
        /// Gets the exceptions.
        /// </summary>
        /// <value>The exceptions.</value>
        public IEnumerable<Exception> Exceptions
        {
            get { return m_Exceptions; }
        }

        /// <summary>
        /// Adds the exception.
        /// </summary>
        /// <param name="x">The x.</param>
        public void AddException(Exception x)
        {
            m_Exceptions.Add(x);
        }

        /// <summary>
        /// Gets a value indicating whether this instance has an exception.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has an exception; otherwise, <c>false</c>.
        /// </value>
        public bool HasException
        {
            get
            {
                return m_Exceptions.Count > 0;
            }
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <value></value>
        /// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
        public override string Message
        {
            get
            {
                StringBuilder sb = new StringBuilder(base.Message);
                sb.AppendLine();
                int i = 1;
                foreach (Exception x in m_Exceptions)
                {
                    Exception y = x;
                    while (y is TargetInvocationException)
                    {
                        y = y.InnerException;
                    }
                    sb.AppendLine();
                    sb.AppendFormat("{0}. ", i);
                    sb.AppendLine(y.Message);
                    int j = 1;
                    while (y.InnerException != null)
                    {
                        y = y.InnerException;
                        while (y is TargetInvocationException)
                        {
                            y = y.InnerException;
                        }
                        sb.AppendLine();
                        sb.AppendFormat("{0}.{1} ", i, j++);
                        sb.AppendLine(y.Message);
                    }
                    i++;
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets a string representation of the frames on the call stack at the time the current exception was thrown.
        /// </summary>
        /// <value></value>
        /// <returns>A string that describes the contents of the call stack, with the most recent method call appearing first.</returns>
        /// <PermissionSet>
        /// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*"/>
        /// </PermissionSet>
        public override string StackTrace
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                int i = 1;
                foreach (Exception x in m_Exceptions)
                {
                    Exception y = x;
                    while (y is TargetInvocationException)
                    {
                        y = y.InnerException;
                    }
                    sb.AppendLine("----------------------------------");
                    sb.AppendFormat("{0}. {1}", i++, x.Message);
                    sb.AppendLine();
                    sb.AppendLine("----------------------------------");
                    sb.AppendLine(y.StackTrace);
                    sb.AppendLine();
                }
                return sb.ToString();
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AggregateException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateException"/> class.
        /// </summary>
        public AggregateException() : base("One or more Exceptions have occurred:")
        {
        }
    }
}
