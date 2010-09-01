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
using System.Collections;
using System.Data.Common;
using DevFuel.Core.UI;
using System.Reflection;
using System.Diagnostics;
//using System.Transactions;
using System.ComponentModel;
using DevFuel.Core.Data.Extensions;

namespace DevFuel.Core.Data.StronglyTyped
{
    /// <summary>
    /// Interface that represents extended methods of controlling a DataSet
    /// </summary>
    public interface IDataSetController
    {
        /// <summary>
        /// Gets the number of tables within the DataSet.
        /// </summary>
        /// <value>The table count.</value>
        /// Updated 1/3/2008 by ee
        int TableCount { get;}
        /// <summary>
        /// Loads the data set and fails if a single error occurs.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="connection">The connection string.</param>
        /// <param name="progressObserver">The Progress Observer.</param>
        /// <returns>True if the loaded dataSet is different from the data source</returns>
        /// Updated 1/3/2008 by ee
        bool LoadDataSet(DataSet dataSet, string connection, IProgressObserver progressObserver);
        /// <summary>
        /// Saves the data set and fails if a single error occurs.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="progressObserver">The Progress Observer.</param>
        /// <returns></returns>
        /// Updated 1/3/2008 by ee
        void SaveDataSet(DataSet dataSet, string connection, IProgressObserver progressObserver);
        /// <summary>
        /// Loads the data set in a fault tolerent way. Each encountered exception is logged in the AggregateException.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="progressObserver">The progress observer.</param>
        /// <param name="aggregateException">The aggregate exception.</param>
        /// <returns>True if the loaded dataSet is different from the data source</returns>
        /// Updated 1/3/2008 by ee
        bool LoadDataSet(DataSet dataSet, string connection, IProgressObserver progressObserver, AggregateException aggregateException);
        /// <summary>
        /// Saves the data set in a fault tolerent way. Each encountered exception is logged in the AggregateException.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="progressObserver">The progress observer.</param>
        /// <param name="aggregateException">The aggregate exception.</param>
        /// <returns></returns>
        /// Updated 1/3/2008 by ee
        void SaveDataSet(DataSet dataSet, string connection, IProgressObserver progressObserver, AggregateException aggregateException);

        /// <summary>
        /// Adds the wrapper aspects.
        /// </summary>
        /// <param name="wrapperNamePattern">The wrapper name pattern.</param>
        /// <param name="aspects">The aspects.</param>
        void AddWrapperAspects(string wrapperNamePattern, params IComparable[] aspects);
    }

    /// <summary>
    /// An implementation of IDataSetController
    /// </summary>
    public class DataSetController : IDataSetController
    {
        /// <summary>
        /// Gets the number of tables within the DataSet.
        /// </summary>
        /// <value>The table count.</value>
        /// Updated 1/3/2008 by ee
        public int TableCount { get { return m_DataRowControllers.Count; } }
        private SortedDictionary<string, IDataRowController> m_DataRowControllers = null;
        private List<string> m_TopDownKeys = null;
        private List<string> m_BottomUpKeys = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSetController"/> class.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="dataRowControllerType">Type of the data row controller.</param>
        public DataSetController(DataSet dataSet, Type dataRowControllerType)
        {
            m_DataRowControllers = new SortedDictionary<string, IDataRowController>();
            foreach (IDataRowController drc in BuildDataRowControllers(dataSet, dataRowControllerType))
            {
                m_DataRowControllers.Add(drc.Name, drc);
            }
        }

        /// <summary>
        /// Builds the table name order.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        public void BuildTableNameOrder(DataSet dataSet)
        {
            m_TopDownKeys = new List<string>(GetDataTableNamesInTopDownOrder(dataSet));
            m_BottomUpKeys = new List<string>(GetDataTableNamesInBottomUpOrder(dataSet));
        }

        /// <summary>
        /// Gets the data row controllers.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <returns></returns>
        public IEnumerable<IDataRowController> GetDataRowControllers(DataSet dataSet)
        {
            return BuildDataRowControllers(dataSet, typeof(DataRowController<,,,>));
        }

        //public Type GetRowType(DataTable dataTable)
        //{
        //    Type type = dataTable.GetType();
        //    MethodInfo mi = type.GetMethod("GetRowType", BindingFlags.Instance | BindingFlags.NonPublic);
        //    if (mi != null && dataTable != null)
        //        return (Type)mi.Invoke(dataTable, new object[] { });
        //    else
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// Adds the wrapper aspects.
        /// </summary>
        /// <param name="wrapperNamePattern">The wrapper name pattern.</param>
        /// <param name="aspects">The aspects.</param>
        public void AddWrapperAspects(string wrapperNamePattern, params IComparable[] aspects)
        {
            foreach (IDataRowController drc in m_DataRowControllers.Values)
            {
                string wrapperName = string.Format(wrapperNamePattern, drc.DataRowType.Namespace, drc.Name);
                Assembly a = drc.DataRowType.Assembly;
                Type dataRowWrapperType = a.GetType(wrapperName);
                if (dataRowWrapperType != null)
                {
                    foreach (IComparable aspect in aspects)
                    {
                        drc.AddAspect(aspect, dataRowWrapperType);
                    }
                }   
            }
        }

        /// <summary>
        /// Builds the data row controllers.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="controllerType">Type of the controller.</param>
        /// <returns></returns>
        protected IEnumerable<IDataRowController> BuildDataRowControllers(DataSet dataSet, Type controllerType)
        {
            return BuildDataRowControllers(dataSet, controllerType, "{0}TableAdapters.{1}TableAdapter");
        }

        /// <summary>
        /// Builds the data row controllers.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="controllerType">Type of the controller.</param>
        /// <param name="tableAdapterNamePattern">The table adapter name pattern.</param>
        /// <returns></returns>
        protected IEnumerable<IDataRowController> BuildDataRowControllers(DataSet dataSet, Type controllerType, string tableAdapterNamePattern)
        {
            Type dataSetType = dataSet.GetType();
            Type dataTableType;
            Type dataRowType;
            Type tableAdapterType;
            Assembly a = dataSetType.Assembly;
            foreach (DataTable dt in dataSet.Tables)
            {
                string objectName = dt.TableName;
                dataTableType = dt.GetType();
                dataRowType = dt.GetDataRowType();

                string dataAdapterName = string.Format(tableAdapterNamePattern, dataSetType.FullName, objectName);
                tableAdapterType = a.GetType(dataAdapterName);
                if (tableAdapterType == null)
                    tableAdapterType = typeof(TableAdapterExtensions.DefaultTableAdapter);

                Type genericDataRowControllerType = controllerType.MakeGenericType(new Type[] { 
                        dataSetType, 
                        dataRowType, 
                        dataTableType, 
                        tableAdapterType
                         });
                IDataRowController controller = Activator.CreateInstance(genericDataRowControllerType) as IDataRowController;
                if (controller != null)
                    yield return controller;
            }
            yield break;
        }

        /// <summary>
        /// Saves the data set.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="o">The o.</param>
        public void SaveDataSet(DataSet dataSet, string connectionString, IProgressObserver o)
        {
            SaveDataSet(dataSet, connectionString, o, null);
        }

        /// <summary>
        /// Saves the data set.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="o">The o.</param>
        /// <param name="aX">A X.</param>
        public void SaveDataSet(DataSet dataSet, string connectionString, IProgressObserver o, AggregateException aX)
        {
            List<KeyValuePair<DataViewRowState, DataRow[]>> actions = new List<KeyValuePair<DataViewRowState, DataRow[]>>();
            SortedDictionary<string, Component> tableAdapters = new SortedDictionary<string, Component>();
            KeyValuePair<DataViewRowState, DataRow[]> action;
            Component adapter = null;
            int iCount = 0;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                //Find all delete actions
                foreach (string key in m_BottomUpKeys)
                {
                    action = new KeyValuePair<DataViewRowState, DataRow[]>(DataViewRowState.Deleted, dataSet.Tables[key].Select("", "", DataViewRowState.Deleted));
                    if (action.Value.Length > 0)
                        actions.Add(action);
                }

                //Find all add actions
                foreach (string key in m_TopDownKeys)
                {
                    action = new KeyValuePair<DataViewRowState, DataRow[]>(DataViewRowState.Added, dataSet.Tables[key].Select("", "", DataViewRowState.Added));
                    if (action.Value.Length > 0)
                        actions.Add(action);
                }

                //Find all modified actions
                foreach (string key in m_TopDownKeys)
                {
                    action = new KeyValuePair<DataViewRowState, DataRow[]>(DataViewRowState.Added, dataSet.Tables[key].Select("", "", DataViewRowState.ModifiedCurrent));
                    if (action.Value.Length > 0)
                        actions.Add(action);
                }

                o.StepCount = actions.Count;

                foreach (KeyValuePair<DataViewRowState, DataRow[]> change in actions)
                {
                    string key = change.Value[0].Table.TableName;
                    DataTable dt = dataSet.Tables[key];
                    IDataRowController drc = GetDataRowController(dt.GetDataRowType());
                    if (!tableAdapters.TryGetValue(key, out adapter))
                    {
                        adapter = drc.CreateNewTableAdapter();
                        adapter.SetConnectionString(connectionString);
                        if (transaction == null)
                        {
                            connection = adapter.GetConnection();
                            connection.Open();
                            transaction = adapter.BeginTransaction();
                        }
                        else
                        {
                            adapter.SetTransaction(transaction);
                        }
                        tableAdapters.Add(drc.Name, adapter);
                    }
                    switch(change.Key)
                    {
                        case DataViewRowState.Deleted:
                        {
                            string label = string.Format("Deleting {0} Items(s) from {1}", change.Value.Length, drc.CaptionPlural);
                            try
                            {
                                o.StepLabel = label;
                                adapter.Update(change.Value);
                            }
                            catch (Exception x)
                            {
                                if (aX != null)
                                {
                                    aX.AddException(new Exception(string.Format("Error {0}",label),x));
                                }
                                else
                                    throw x;
                            }
                            o.StepCurrent = iCount++;
                        }
                        break;
                        case DataViewRowState.ModifiedCurrent:
                        {
                            string label = string.Format("Updating {0} Items(s) in {1}", change.Value.Length, drc.CaptionPlural);
                            try
                            {
                                o.StepLabel = label;
                                adapter.Update(change.Value);
                            }
                            catch (Exception x)
                            {
                                if (aX != null)
                                {
                                    aX.AddException(new Exception(string.Format("Error {0}", label), x));
                                }
                                else
                                    throw x;
                            }
                            o.StepCurrent = iCount++;
                        }
                        break;
                        case DataViewRowState.Added:
                        {
                            string label = string.Format("Adding {0} Items(s) to {1}", change.Value.Length, drc.CaptionPlural);
                            try
                            {
                                o.StepLabel = label;
                                adapter.Update(change.Value);
                            }
                            catch (Exception x)
                            {
                                if (change.Value[0].Table.HasErrors)
                                    x = new DataTableErrorsException(change.Value[0].Table, x);
                                if (aX != null)
                                {
                                    aX.AddException(new Exception(string.Format("Error {0}", label), x));
                                }
                                else
                                    throw x;
                            }
                            o.StepCurrent = iCount++;
                        }
                        break;
                    }
                }
            
                if (transaction != null)
                    transaction.Commit();
            }
            catch (Exception x)
            {
                if (transaction != null)
                    transaction.Rollback();
                throw x;
            }
            finally
            {
                if (transaction != null)
                    transaction.Dispose();
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        /// <summary>
        /// Saves the data table.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="connectionString">The connection string.</param>
        public void SaveDataTable(DataSet dataSet, string tableName, string connectionString)
        {
            Component adapter = GetTableAdapter(tableName);
            adapter.SetConnectionString(connectionString);
            adapter.Update(dataSet);                
        }


        /// <summary>
        /// Loads the data set.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public bool LoadDataSet(DataSet dataSet, string connectionString, IProgressObserver o)
        {
            return LoadDataSet(dataSet, connectionString, o, null);
        }

        /// <summary>
        /// Loads the data set.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="o">The o.</param>
        /// <param name="aX">A X.</param>
        /// <returns></returns>
        public bool LoadDataSet(DataSet dataSet, string connection, IProgressObserver o, AggregateException aX)
        {           
            o.StepCount = TableCount;
            int iCount = 0;
            string sKey;
            foreach (Component adapter in TopDownAdapters)
            {
                try
                {
                    sKey = adapter.GetTableName();
                    o.StepLabel = string.Format("Loading {0}...", sKey);
                    adapter.SetConnectionString(connection);
                    try
                    {
                        adapter.Fill(dataSet.Tables[sKey]);
                    }
                    catch (Exception fillException)
                    {
                        if (aX != null)
                        {
                            MigrateDataTable(adapter.GetConnection(), sKey, dataSet.Tables[sKey], aX);
                        }
                        else
                            throw fillException;
                    }
                }
                catch (Exception x)
                {
                    if (aX != null)
                    {
                        aX.AddException(x);
                    }
                    else
                        throw x;
                }
                o.StepCurrent = iCount++;
            }
            if (aX != null)
                return aX.HasException;
            return false; 
        }

        private void MigrateDataTable(DbConnection connection, string tableName, DataTable destDataTable, AggregateException aX)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            DbCommand cmd = connection.CreateCommand();
            cmd.CommandText = string.Format("SELECT * FROM [{0}]", tableName);
            DbDataReader rdr = cmd.ExecuteReader();
            DataTable schemaDataTable = rdr.GetSchemaTable();
            List<int> columns = new List<int>();
            foreach (DataColumn dc in destDataTable.Columns)
            {
                DataRow[] drCols = schemaDataTable.Select(string.Format("ColumnName='{0}'", dc.ColumnName));
                int i = -1;
                if (drCols.Length > 0)
                    i = (int)drCols[0][1];
                columns.Add(i);
            }

            while (rdr.Read())
            {
                try
                {
                    DataRow dr = destDataTable.NewRow();
                    foreach (DataColumn dc in destDataTable.Columns)
                    {
                        int iColumn = columns[dc.Ordinal];
                        if (iColumn >= 0)
                        {
                            object obj = rdr.GetValue(iColumn);
                            if (obj is DBNull)
                            {
                                if (dc.AllowDBNull)
                                {
                                    dr[dc.Ordinal] = DBNull.Value;
                                }
                                else
                                {
                                    if (dc.DataType == typeof(Guid))
                                    {
                                        dr[dc.Ordinal] = Guid.Empty;
                                    }
                                    else if (dc.DataType == typeof(int))
                                    {
                                        dr[dc.Ordinal] = 0;
                                    }
                                }
                            }
                            else if (obj.GetType() != dc.DataType)
                            {
                                dr[dc.Ordinal] = Convert.ChangeType(obj, dc.DataType);
                            }
                            else
                            {
                                dr[dc.Ordinal] = obj;
                            }
                        }
                        else
                        {
                            dr[dc.Ordinal] = Activator.CreateInstance(dc.DataType);
                        }
                    }
                    destDataTable.Rows.Add(dr);
                }
                catch (Exception x)
                {
                    aX.AddException(x);
                }
            }
        }

        /// <summary>
        /// Gets the table adapter.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public Component GetTableAdapter(string tableName)
        {
            IDataRowController drc = null;
            if (m_DataRowControllers.TryGetValue(tableName, out drc))
            {
                return drc.CreateNewTableAdapter();
            }
            return null;
        }

        /// <summary>
        /// Loads the data table.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="connection">The connection.</param>
        public void LoadDataTable(DataSet dataSet, string tableName, string connection)
        {      
            Component adapter = GetTableAdapter(tableName);
            adapter.SetConnectionString(connection);
            adapter.Fill(dataSet.Tables[tableName]);
        }

        /// <summary>
        /// Gets the top down adapters.
        /// </summary>
        /// <value>The top down adapters.</value>
        public IEnumerable TopDownAdapters
        {
            get
            {
                foreach (string sKey in m_TopDownKeys)
                {
                    yield return GetTableAdapter(sKey);
                }
                yield break;
            }

        }

        /// <summary>
        /// Gets the bottom up adapters.
        /// </summary>
        /// <value>The bottom up adapters.</value>
        public IEnumerable BottomUpAdapters
        {
            get
            {
                foreach (string sKey in m_BottomUpKeys)
                {
                    yield return GetTableAdapter(sKey);
                }
                yield break;
            }
        }

        /// <summary>
        /// Wraps the specified dr.
        /// </summary>
        /// <param name="dr">The dr.</param>
        /// <param name="dataSet">The data set.</param>
        /// <param name="aspect">The aspect.</param>
        /// <returns></returns>
        public IDataRowWrapper Wrap(DataRow dr, DataSet dataSet, IComparable aspect)
        {
            Type tRow = dr.GetType();
            IDataRowController drc = null;
            if (m_DataRowControllers.TryGetValue(tRow.Name.Substring(0, tRow.Name.Length - 3), out drc))
            {
                return drc.Wrap(dr, dataSet, aspect);
            }
            throw new Exception("A Controller does not exist for this DataRow");
        }

        /// <summary>
        /// Gets the data row controller.
        /// </summary>
        /// <param name="tDataRow">The t data row.</param>
        /// <returns></returns>
        public IDataRowController GetDataRowController(Type tDataRow)
        {
            IDataRowController drc = null;
            if (m_DataRowControllers.TryGetValue(tDataRow.Name.Substring(0, tDataRow.Name.Length - 3), out drc))
            {
                return drc;
            }
            return null;
        }

        /// <summary>
        /// Gets the data row controller.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <returns></returns>
        public IDataRowController GetDataRowController(DataRow row)
        {
            return GetDataRowController(row.GetType());
        }

        /// <summary>
        /// Gets the data row controllers.
        /// </summary>
        /// <param name="suffix">The suffix.</param>
        /// <returns></returns>
        public IEnumerable<IDataRowController> GetDataRowControllers(string suffix)
        {
            foreach (IDataRowController drc in m_DataRowControllers.Values)
            {
                if (drc.Name.EndsWith(suffix))
                    yield return drc;
            }
            yield break;
        }

        /// <summary>
        /// Gets the data row controllers.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IDataRowController> GetDataRowControllers()
        {
            foreach (IDataRowController drc in m_DataRowControllers.Values)
            {
                yield return drc;
            }
            yield break;
        }

        /// <summary>
        /// Gets the data tables in top down order.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <returns></returns>
        public IEnumerable<DataTable> GetDataTablesInTopDownOrder(DataSet dataSet)
        {
            int frontierEchelon = 1;
            int completedEchelon = 0;

            List<DataTable> incompleteTables = new List<DataTable>();
            List<DataTable> completeTables = new List<DataTable>();
            List<DataTable> removedTables = new List<DataTable>();
            Trace.WriteLine("--------------------");
            foreach (DataTable dt in dataSet.Tables)
            {
                if (dt.ParentRelations.Count == 0) //All tables without parents are roots
                {
                    dt.ExtendedProperties["Echelon"] =  0;
                    completeTables.Add(dt);
                }
                else
                {
                    dt.ExtendedProperties["Echelon"] = 1;
                    incompleteTables.Add(dt);
                }
            }
#if DEBUG
            DumpEchelons(completeTables, 1);
#endif


            while (completedEchelon < frontierEchelon)
            {
                foreach (DataTable dt in incompleteTables)
                {
                    int echelon = (int)dt.ExtendedProperties["Echelon"];
                    bool changed = false;
                    foreach (DataRelation parentRelation in dt.ParentRelations) //Check this table'schemaSet parent tables.
                    {
                        DataTable parentTable = parentRelation.ParentTable;
                        int parentTableEchelon = (int)parentTable.ExtendedProperties["Echelon"];
                        if (parentTableEchelon > completedEchelon && parentTable.TableName != dt.TableName)
                        {
                            if (parentTableEchelon+1 > echelon) //move beyond parent table, if we haven't already
                                dt.ExtendedProperties["Echelon"] = echelon = parentTableEchelon+1;
                            if (echelon > frontierEchelon)
                                frontierEchelon = echelon;
                            changed = true;
                        }
                    }
                    if (!changed) //if this table didn't move, then its done
                    {
                        removedTables.Add(dt);
                    }
                }
                //Stop considering removed tables
                foreach (DataTable dt in removedTables)
                {
                    incompleteTables.Remove(dt);
                    completeTables.Add(dt);
                }
                removedTables.Clear();
                completedEchelon++;
#if DEBUG
                DumpEchelons(completeTables, completedEchelon);
                DumpEchelons(incompleteTables, completedEchelon);
#endif
            }

            completeTables.Sort(delegate(DataTable left, DataTable right)
            {
                int leftEchelon = (int)left.ExtendedProperties["Echelon"];
                int rightEchelon = (int)right.ExtendedProperties["Echelon"];
                if (leftEchelon > rightEchelon)
                    return 1;
                else if (leftEchelon == rightEchelon)
                    return 0;
                return -1;
            });

            return completeTables;
        }

#if DEBUG
        private void DumpEchelons(List<DataTable> completeTables, int completeEchelons)
        {
            completeTables.Sort(delegate(DataTable left, DataTable right)
            {
                int leftEchelon = (int)left.ExtendedProperties["Echelon"];
                int rightEchelon = (int)right.ExtendedProperties["Echelon"];
                if (leftEchelon > rightEchelon)
                    return 1;
                else if (leftEchelon == rightEchelon)
                    return 0;
                return -1;
            });
            Trace.WriteLine(string.Format("-------------"));
            foreach (DataTable dt in completeTables)
            {
                int echelon = (int)dt.ExtendedProperties["Echelon"];
                Trace.WriteLine(string.Format("{0} {1}{2}", echelon, dt.TableName, echelon <= completeEchelons ? "*":""));
            }
        }        
#endif

        /// <summary>
        /// Gets the data tables in bottom up order.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <returns></returns>
        public IEnumerable<DataTable> GetDataTablesInBottomUpOrder(DataSet dataSet)
        {
            List<DataTable> list = new List<DataTable>(GetDataTablesInTopDownOrder(dataSet));
            list.Reverse();
            return list;
        }

        /// <summary>
        /// Gets the data table names in top down order.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <returns></returns>
        public IEnumerable<string> GetDataTableNamesInTopDownOrder(DataSet dataSet)
        {
            foreach (DataTable dt in GetDataTablesInTopDownOrder(dataSet))
            {
                yield return dt.TableName;
            }
            yield break;
        }

        /// <summary>
        /// Gets the data table names in bottom up order.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <returns></returns>
        public IEnumerable<string> GetDataTableNamesInBottomUpOrder(DataSet dataSet)
        {
            foreach (DataTable dt in GetDataTablesInBottomUpOrder(dataSet))
            {
                yield return dt.TableName;
            }
            yield break;
        }
    }
}
