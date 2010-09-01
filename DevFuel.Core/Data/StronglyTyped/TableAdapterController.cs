//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Data.Common;
//using System.Data;
//using System.Reflection;
//using System.Data.OleDb;
//using System.ComponentModel;

//namespace DevFuel.Core.Data.StronglyTyped
//{
//    public class DefaultTableAdapter : Component
//    {
//        Type m_DataTableType = null;
//        public DefaultTableAdapter(Type dataTableType)
//        {
//            m_DataTableType = dataTableType;
//        }

//        public string GetTableName()
//        {
//            if (m_DataTableType.Name.EndsWith("DataTable"))
//                return m_DataTableType.Name.Substring(0, m_DataTableType.Name.Length - 9);
//            return m_DataTableType.Name;
//        }
//    }

//    public class TableAdapterController
//    {
//        public string GetTableName(object tableAdapter)
//        {
//            if (tableAdapter is DefaultTableAdapter)
//            {
//                return (tableAdapter as DefaultTableAdapter).GetTableName();
//            }
//            Type type = tableAdapter.GetType();
//            if (type.Name.EndsWith("TableAdapter"))
//                return type.Name.Substring(0, type.Name.Length - 12);
//            return type.Name;
//        }

//        public DbConnection GetConnection(object tableAdapter)
//        {
//            Type type = tableAdapter.GetType();
//            PropertyInfo pi = type.GetProperty("Connection", BindingFlags.NonPublic | BindingFlags.Instance);
//            if (pi != null)
//            {
//                DbConnection connection = (DbConnection)pi.GetValue(tableAdapter, null);
//                return connection;
//            }
//            return null;
//        }
//        public void SetConnection(object tableAdapter, DbConnection connection)
//        {
//            Type type = tableAdapter.GetType();
//            PropertyInfo pi = type.GetProperty("Connection", BindingFlags.NonPublic | BindingFlags.Instance);
//            if (pi != null)
//                pi.SetValue(tableAdapter, connection, null);
//        }

//        public void SetConnectionString(object tableAdapter, string connectionString)
//        {
//            DbConnection connection = GetConnection(tableAdapter);
//            if (connection != null)
//            {
//                connection.ConnectionString = connectionString;
//            }
//        }

//        public DbDataAdapter GetDataAdapter(object tableAdapter)
//        {
//            Type type = tableAdapter.GetType();
//            PropertyInfo adapterProperty = type.GetProperty("Adapter", BindingFlags.NonPublic | BindingFlags.Instance);
//            if (adapterProperty != null)
//            {
//                DbDataAdapter a = (DbDataAdapter)adapterProperty.GetValue(tableAdapter, null);
//                return a;
//            }
//            return null;
//        }

//        public void Fill(object tableAdapter, DataTable dataTable)
//        {
//            try
//            {
//                Type type = tableAdapter.GetType();
//                MethodInfo mi = type.GetMethod("Fill");
//                if (mi != null && dataTable != null && tableAdapter != null)
//                    mi.Invoke(tableAdapter, new object[] { dataTable });
//                else
//                {

//                }
//            }
//            catch (Exception x)
//            {
//                throw new Exception(string.Format("Encountered Problem when loading from \"{0}\"", dataTable.TableName), x);
//            }
//        }

//        public int Update(object tableAdapter, DataSet dataSet)
//        {
//            Type type = tableAdapter.GetType();
//            MethodInfo mi = type.GetMethod("Update", new Type[] { dataSet.GetType() });
//            if (mi != null && dataSet != null && tableAdapter != null)
//                return (int)mi.Invoke(tableAdapter, new object[] { dataSet });
//            else
//            {
//                return -1;
//            }
//        }

//        public int Update(object tableAdapter, DataRow[] rows)
//        {
//            Type type = tableAdapter.GetType();
//            MethodInfo mi = type.GetMethod("Update", new Type[] { typeof(DataRow[]) });
//            if (mi != null && rows != null && tableAdapter != null)
//                return (int)mi.Invoke(tableAdapter, new object[] { rows });
//            else
//            {
//                return -1;
//            }
//        }


//        public DbTransaction BeginTransaction(object tableAdapter)
//        {
//            DbConnection connection = GetConnection(tableAdapter);
//            if (connection != null)
//            {
//                DbTransaction t = connection.BeginTransaction();
//                SetTransaction(tableAdapter, t);
//                return t;
//            }
//            return null;
//        }

//        public void SetTransaction(object adapter, DbTransaction transaction)
//        {
//            Type type = adapter.GetType();

//            PropertyInfo commandsProperty = type.GetProperty("CommandCollection", BindingFlags.NonPublic | BindingFlags.Instance);
//            IDbCommand[] commands = (IDbCommand[])commandsProperty.GetValue(adapter, null);
//            foreach (DbCommand command in commands)
//            {
//                command.Transaction = transaction;
//                command.Connection = transaction.Connection;
//            }

//            DbDataAdapter a = GetDataAdapter(adapter);
//            if (a != null)
//            {
//                if (a.InsertCommand != null)
//                {
//                    a.InsertCommand.Transaction = transaction;
//                    a.InsertCommand.Connection = transaction.Connection;
//                }
//                if (a.UpdateCommand != null)
//                {
//                    a.UpdateCommand.Transaction = transaction;
//                    a.InsertCommand.Connection = transaction.Connection;
//                }
//                if (a.SelectCommand != null)
//                {
//                    a.SelectCommand.Transaction = transaction;
//                    a.InsertCommand.Connection = transaction.Connection;
//                }
//                if (a.DeleteCommand != null)
//                {
//                    a.DeleteCommand.Transaction = transaction;
//                    a.InsertCommand.Connection = transaction.Connection;
//                }
//            }
//            SetConnection(adapter, transaction.Connection);
//        }
//    }
//}
