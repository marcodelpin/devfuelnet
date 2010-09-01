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
using System.ComponentModel;
using System.Reflection;

namespace DevFuel.Core.Data.StronglyTyped
{
    /// <summary>
    /// Interface that represents a customizable "wrapper" around a DataRow that allows you to control how that Row is viewed/edited (typically in a PropertyGrid or reflection-based usage scenario)
    /// </summary>
    public interface IDataRowWrapper
    {
        /// <summary>
        /// Gets the data row.
        /// </summary>
        /// <value>The data row.</value>
        DataRow DataRow { get; }
        /// <summary>
        /// Initializes the specified wrapper with a data row and context object.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <param name="context">The context. Usually the DataSet</param>
        void Initialize(DataRow dataRow, object context);
    }

    /// <summary>
    /// A Templated implementation of the IDataRowWrapper for a particular DataRow and Context Type
    /// </summary>
    /// <typeparam name="R">A type of DataRow</typeparam>
    /// <typeparam name="C">A type of Context Object</typeparam>
    public class DataRowWrapper<R,C> : IDataRowWrapper where R : DataRow
    {
        /// <summary>
        /// 
        /// </summary>
        protected R m_Row;
        /// <summary>
        /// Gets or sets the strongly typed row.
        /// </summary>
        /// <value>The row.</value>
        [Browsable(false)]
        public R Row
        {
            get { return m_Row; }
            set { m_Row = value; }
        }

        /// <summary>
        /// Initializes the specified wrapper with a data row and context object.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <param name="context">The context. Usually the DataSet</param>
        public void Initialize(DataRow dataRow, object context)
        {
            if (dataRow == null)
                throw new Exception(Strings.CannotWrapNullRow);
            if (!(dataRow is R))
                throw new Exception(string.Format(Strings.InvalidRowType, typeof(R).Name, dataRow.GetType().Name));
            m_Row = dataRow as R;
            m_Context = (C)context;
            OnInitialize();
        }

        /// <summary>
        /// Called during Initialize method. Override for customizable Initialization
        /// </summary>
        protected virtual void OnInitialize()
        {
        }

        #region IDataRowWrapper Members
        /// <summary>
        /// Gets the data row.
        /// </summary>
        /// <value>The data row.</value>
        [Browsable(false)]
        public DataRow DataRow
        {
            get
            {
                return m_Row;
            }
            set
            {
                m_Row = value as R;
            }
        }

        /// <summary>
        /// Gets the string value from the specified column.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>the string value for the named column</returns>
        public virtual string GetString(string columnName)
        {
            return GetString(m_Row, columnName);
        }

        /// <summary>
        /// Gets the string value from the specified column.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public static String GetString(DataRow dataRow, string columnName)
        {
            if (dataRow.IsNull(columnName))
                return null;
            return (string)dataRow[columnName];
        }

        /// <summary>
        /// Sets the string.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="s">The s.</param>
        public virtual void SetString(string columnName, string s)
        {
            SetString(m_Row, columnName, s);
        }

        /// <summary>
        /// Sets the string value on the specified column.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="s">The s.</param>
        public static void SetString(DataRow dataRow, string columnName, string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                dataRow[columnName] = s;
            }
            else
            {
                if (dataRow.Table.Columns[columnName].AllowDBNull)
                    dataRow[columnName] = System.Convert.DBNull;
                else
                    dataRow[columnName] = string.Empty;
            }
            FireChangedColumnEvent(dataRow, columnName);
        }

        /// <summary>
        /// Fires the changed column event.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <param name="columnName">Name of the column.</param>
        public static void FireChangedColumnEvent(DataRow dataRow, string columnName)
        {
            Type t = dataRow.GetType();
            MethodInfo mi = t.GetMethod(string.Format("On{0}Changed", columnName), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
            if (mi != null)
            {
                mi.Invoke(dataRow, new object[] { EventArgs.Empty });
            }
        }

        /// <summary>
        /// Gets a Nullable object from the specified column.
        /// </summary>
        /// <typeparam name="T">The type of Nullable Object</typeparam>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public virtual Nullable<T> GetNullable<T>(string columnName) where T : struct
        {
            return GetNullable<T>(m_Row, columnName);
        }

        /// <summary>
        /// Gets the nullable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataRow">The data row.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public static Nullable<T> GetNullable<T>(DataRow dataRow, string columnName) where T : struct
        {
            Nullable<T> n = new Nullable<T>();
            if (!dataRow.IsNull(columnName))
                n = (T)dataRow[columnName];
            return n;
        }

        /// <summary>
        /// Sets the nullable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="n">The n.</param>
        public virtual void SetNullable<T>(string columnName, Nullable<T> n) where T : struct
        {
            SetNullable<T>(m_Row, columnName, n);
        }

        /// <summary>
        /// Sets the nullable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataRow">The data row.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="n">The n.</param>
        public static void SetNullable<T>(DataRow dataRow, string columnName, Nullable<T> n) where T : struct
        {
            if (n.HasValue)
            {
                dataRow[columnName] = n.Value;
            }
            else
            {
                if (dataRow.Table.Columns[columnName].AllowDBNull)
                    dataRow[columnName] = System.Convert.DBNull;
                else
                    dataRow[columnName] = default(T);
            }
            FireChangedColumnEvent(dataRow, columnName);
        }

        /// <summary>
        /// 
        /// </summary>
        protected C m_Context;
        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>The context.</value>
        [Browsable(false)]
        public C Context
        {
            get { return m_Context; }
            set { m_Context = value; }
        }
        #endregion

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return m_Row.ToString();
        }

        #region IDataRowWrapper Members




        #endregion

        #region ICloneable Members

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            return null;
        }

        #endregion
    }
}
