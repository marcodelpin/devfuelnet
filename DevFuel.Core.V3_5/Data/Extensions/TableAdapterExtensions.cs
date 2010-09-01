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
using System.Data.Common;
using System.Data;
using System.Reflection;
using System.Data.OleDb;
using System.ComponentModel;

namespace DevFuel.Core.Data.Extensions
{
    /// <summary>
    /// Extension Method Holder for StronglyTyped Table Adapters
    /// </summary>
    public static class TableAdapterExtensions
    {
        /// <summary>
        /// A default TableAdapter if none exists for a DataTable
        /// </summary>
        public class DefaultTableAdapter : Component
        {
            Type m_DataTableType = null;
            /// <summary>
            /// Initializes a new instance of the <see cref="DefaultTableAdapter"/> class.
            /// </summary>
            /// <param name="dataTableType">Type of the data table.</param>
            public DefaultTableAdapter(Type dataTableType)
            {
                m_DataTableType = dataTableType;
            }

            /// <summary>
            /// Gets the name of the table.
            /// </summary>
            /// <returns></returns>
            public string GetTableName()
            {
                if (m_DataTableType.Name.EndsWith("DataTable"))
                    return m_DataTableType.Name.Substring(0, m_DataTableType.Name.Length - 9);
                return m_DataTableType.Name;
            }
        }


        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <param name="tableAdapter">The table adapter.</param>
        /// <returns></returns>
        public static string GetTableName(this Component tableAdapter)
        {
            if (tableAdapter is DefaultTableAdapter)
            {
                return (tableAdapter as DefaultTableAdapter).GetTableName();
            }
            Type type = tableAdapter.GetType();
            if (type.Name.EndsWith("TableAdapter"))
                return type.Name.Substring(0, type.Name.Length - 12);
            return type.Name;
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <param name="tableAdapter">The table adapter.</param>
        /// <returns></returns>
        public static DbConnection GetConnection(this Component tableAdapter)
        {  
            Type type = tableAdapter.GetType();
            PropertyInfo pi = type.GetProperty("Connection", BindingFlags.NonPublic | BindingFlags.Instance);
            if (pi != null)
            {
                DbConnection connection = (DbConnection)pi.GetValue(tableAdapter, null);
                return connection;
            }
            return null;
        }
        /// <summary>
        /// Sets the connection.
        /// </summary>
        /// <param name="tableAdapter">The table adapter.</param>
        /// <param name="connection">The connection.</param>
        public static void SetConnection(this Component tableAdapter, DbConnection connection)
        {
            Type type = tableAdapter.GetType();
            PropertyInfo pi = type.GetProperty("Connection", BindingFlags.NonPublic | BindingFlags.Instance);
            if (pi != null)
                pi.SetValue(tableAdapter, connection, null);
        }

        /// <summary>
        /// Sets the connection string.
        /// </summary>
        /// <param name="tableAdapter">The table adapter.</param>
        /// <param name="connectionString">The connection string.</param>
        public static void SetConnectionString(this Component tableAdapter, string connectionString)
        {
            DbConnection connection = GetConnection(tableAdapter);
            if (connection != null)
            {
                connection.ConnectionString = connectionString;
            }
        }

        /// <summary>
        /// Gets the data adapter.
        /// </summary>
        /// <param name="tableAdapter">The table adapter.</param>
        /// <returns></returns>
        public static DbDataAdapter GetDataAdapter(this Component tableAdapter)
        {    
            Type type = tableAdapter.GetType();
            PropertyInfo adapterProperty = type.GetProperty("Adapter", BindingFlags.NonPublic | BindingFlags.Instance);
            if (adapterProperty != null)
            {
                DbDataAdapter a = (DbDataAdapter)adapterProperty.GetValue(tableAdapter, null);
                return a;
            }
            return null;
        }

        /// <summary>
        /// Fills the specified data table.
        /// </summary>
        /// <param name="tableAdapter">The table adapter.</param>
        /// <param name="dataTable">The data table.</param>
        public static void Fill(this Component tableAdapter, DataTable dataTable)
        {
            try
            {
                Type type = tableAdapter.GetType();
                MethodInfo mi = type.GetMethod("Fill");
                if (mi != null && dataTable != null && tableAdapter != null)
                    mi.Invoke(tableAdapter, new object[] { dataTable });
                else
                {

                }
            }
            catch (Exception x)
            {
                throw new Exception(string.Format("Encountered Problem when loading from \"{0}\"", dataTable.TableName), x);
            }
        }

        /// <summary>
        /// Updates the specified table adapter.
        /// </summary>
        /// <param name="tableAdapter">The table adapter.</param>
        /// <param name="dataSet">The data set.</param>
        /// <returns></returns>
        public static int Update(this Component tableAdapter, DataSet dataSet)
        {
            Type type = tableAdapter.GetType();
            MethodInfo mi = type.GetMethod("Update", new Type[] { dataSet.GetType() });
            if (mi != null && dataSet != null && tableAdapter != null)
                return (int)mi.Invoke(tableAdapter, new object[] { dataSet });
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Updates the specified table adapter.
        /// </summary>
        /// <param name="tableAdapter">The table adapter.</param>
        /// <param name="rows">The rows.</param>
        /// <returns></returns>
        public static int Update(this Component tableAdapter, DataRow[] rows)
        {
            Type type = tableAdapter.GetType();
            MethodInfo mi = type.GetMethod("Update", new Type[] { typeof(DataRow[]) });
            if (mi != null && rows != null && tableAdapter != null)
                return (int)mi.Invoke(tableAdapter, new object[] { rows });
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="tableAdapter">The table adapter.</param>
        /// <returns></returns>
        public static DbTransaction BeginTransaction(this Component tableAdapter)
        {
            DbConnection connection = GetConnection(tableAdapter);
            if (connection != null)
            {
                DbTransaction t = connection.BeginTransaction();
                SetTransaction(tableAdapter, t);
                return t;
            }
            return null;
        }

        /// <summary>
        /// Sets the transaction.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        /// <param name="transaction">The transaction.</param>
        public static void SetTransaction(this Component adapter, DbTransaction transaction)
        { 
            Type type = adapter.GetType();

            PropertyInfo commandsProperty = type.GetProperty("CommandCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            IDbCommand[] commands = (IDbCommand[])commandsProperty.GetValue(adapter, null);
            foreach (DbCommand command in commands)
            {
                command.Transaction = transaction;
                command.Connection = transaction.Connection;
            }

            DbDataAdapter a = GetDataAdapter(adapter);
            if (a != null)
            {
                if (a.InsertCommand != null)
                {
                    a.InsertCommand.Transaction = transaction;
                    a.InsertCommand.Connection = transaction.Connection;
                }
                if (a.UpdateCommand != null)
                {
                    a.UpdateCommand.Transaction = transaction;
                    a.InsertCommand.Connection = transaction.Connection;
                }
                if (a.SelectCommand != null)
                {
                    a.SelectCommand.Transaction = transaction;
                    a.InsertCommand.Connection = transaction.Connection;
                }
                if (a.DeleteCommand != null)
                {
                    a.DeleteCommand.Transaction = transaction;
                    a.InsertCommand.Connection = transaction.Connection;
                }
            }
            SetConnection(adapter, transaction.Connection);
        }
    }
}
