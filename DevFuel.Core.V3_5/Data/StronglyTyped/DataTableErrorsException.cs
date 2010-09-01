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
using System.Data;

namespace DevFuel.Core.Data.StronglyTyped
{
    /// <summary>
    /// An Exception that details one or more DataTableErrors
    /// </summary>
    public class DataTableErrorsException : Exception
    {
        /// <summary>
        /// The DataRows that were in error
        /// </summary>
        protected DataRow[] m_ErrorRows;
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableErrorsException"/> class.
        /// </summary>
        /// <param name="table">The table.</param>
        public DataTableErrorsException(DataTable table)
        {
            m_ErrorRows = table.GetErrors();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableErrorsException"/> class.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="innerException">The inner exception.</param>
        public DataTableErrorsException(DataTable table, Exception innerException)
            : base(string.Empty, innerException)
        {
            m_ErrorRows = table.GetErrors();
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
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("The following rows are in error:");
                foreach (DataRow dr in m_ErrorRows)
                {
                    sb.AppendLine(dr.ToString());
                }
                return sb.ToString();
            }
        }
    }
}
