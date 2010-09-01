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
using System.Diagnostics;
using DevFuel.Core.Data.Extensions;

namespace DevFuel.Core.Data.StronglyTyped
{

    /// <summary>
    /// An Interface that represents the ability of an object to provide abstract and advanced operations on a DataRow from a particular DataTable
    /// </summary>
    public interface IDataRowController
    {
        /// <summary>
        /// Gets the type of the data set.
        /// </summary>
        /// <value>The type of the data set.</value>
        Type DataSetType { get;}
        /// <summary>
        /// Gets the type of the data row.
        /// </summary>
        /// <value>The type of the data row.</value>
        Type DataRowType { get;}
        /// <summary>
        /// Gets the type of the data table.
        /// </summary>
        /// <value>The type of the data table.</value>
        Type DataTableType { get;}
        /// <summary>
        /// Gets the type of the table adapter.
        /// </summary>
        /// <value>The type of the table adapter.</value>
        Type TableAdapterType { get;}
        /// <summary>
        /// Gets the caption.
        /// </summary>
        /// <value>The caption.</value>
        string Caption {get;}
        /// <summary>
        /// Gets the caption plural.
        /// </summary>
        /// <value>The caption plural.</value>
        string CaptionPlural{get;}
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name {get;}
        /// <summary>
        /// Gets the name of the data table.
        /// </summary>
        /// <value>The name of the data table.</value>
        string DataTableName { get;}
        /// <summary>
        /// Creates the new row.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <returns></returns>
        DataRow CreateNewRow(DataSet dataSet);
        /// <summary>
        /// Fills the row defaults.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="row">The row.</param>
        void FillRowDefaults(DataSet dataSet, DataRow row);
        /// <summary>
        /// Wraps the specified row.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="context">The context.</param>
        /// <param name="aspect">The aspect.</param>
        /// <returns></returns>
        IDataRowWrapper Wrap(DataRow row, object context, IComparable aspect);
        /// <summary>
        /// Gets the type of the wrapper.
        /// </summary>
        /// <param name="aspect">The aspect.</param>
        /// <returns></returns>
        Type GetWrapperType(IComparable aspect);
        /// <summary>
        /// Adds the aspect.
        /// </summary>
        /// <param name="aspect">The aspect.</param>
        /// <param name="wrapperType">Type of the wrapper.</param>
        void AddAspect(IComparable aspect, Type wrapperType);
        /// <summary>
        /// Adds the row.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="row">The row.</param>
        void AddRow(DataSet dataSet, DataRow row);
        /// <summary>
        /// Deletes the row.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="row">The row.</param>
        void DeleteRow(DataSet dataSet, DataRow row);
        /// <summary>
        /// Creates the new table adapter.
        /// </summary>
        /// <returns></returns>
        Component CreateNewTableAdapter();
    }

    /// <summary>
    /// Interface that represents a DataRow with a custom ability with regards to being added to a table
    /// </summary>
    public interface IDataRowCustomAdd
    {
        /// <summary>
        /// Called when [before add].
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        void OnBeforeAdd(DataSet dataSet);
        /// <summary>
        /// Called when [after add].
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        void OnAfterAdd(DataSet dataSet);
    }

    /// <summary>
    /// Interface that represents a DataRow with a custom ability with regards to being removed from a table
    /// </summary>
    public interface IDataRowCustomDelete
    {
        /// <summary>
        /// Called when [before delete].
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        void OnBeforeDelete(DataSet dataSet);
        /// <summary>
        /// Called when [after delete].
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        void OnAfterDelete(DataSet dataSet);
    }

    /// <summary>
    /// Interface that represents a DataRow with a custom ability with regards to being filled with default values
    /// </summary>
    public interface IDataRowCustomFillDefaults
    {
        /// <summary>
        /// Called when [before fill defaults].
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        void OnBeforeFillDefaults(DataSet dataSet);
        /// <summary>
        /// Called when [after fill defaults].
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        void OnAfterFillDefaults(DataSet dataSet);
    }

    /// <summary>
    /// Generic Implementation of the IDataRowController Interface
    /// </summary>
    /// <typeparam name="D">DataSet Type that houses the DataTable of this DataRow</typeparam>
    /// <typeparam name="R">DataRow Type</typeparam>
    /// <typeparam name="T">DataTable Type that houses the DataRow</typeparam>
    /// <typeparam name="A">TableAdapter Type for the DataTable that houses this DataRow</typeparam>
    public class DataRowController<D,R,T,A> : IDataRowController where D:DataSet where R:DataRow where T:DataTable where A:Component
    {
        /// <summary>
        /// Gets the type of the data set.
        /// </summary>
        /// <value>The type of the data set.</value>
        public Type DataSetType {get{return typeof(D);}}
        /// <summary>
        /// Gets the type of the data row.
        /// </summary>
        /// <value>The type of the data row.</value>
        public Type DataRowType {get{return typeof(R);}}
        /// <summary>
        /// Gets the type of the data table.
        /// </summary>
        /// <value>The type of the data table.</value>
        public Type DataTableType {get{return typeof(T);}}
        /// <summary>
        /// Gets the type of the table adapter.
        /// </summary>
        /// <value>The type of the table adapter.</value>
        public Type TableAdapterType {get{return typeof(A);}}

        private IDataTableCaptionProvider m_CaptionProvider = null;
        /// <summary>
        /// Gets or sets the caption provider.
        /// </summary>
        /// <value>The caption provider.</value>
        public IDataTableCaptionProvider CaptionProvider
        {
            get {
                if (m_CaptionProvider == null)
                {
                    try
                    {
                        m_CaptionProvider = DataSetType.InvokeMember("DataTableCaptionProvider", BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Static, null, null, new object[] { }) as IDataTableCaptionProvider;
                    }
                    catch
                    {
                    }
                    if (m_CaptionProvider == null)
                        m_CaptionProvider = new DefaultDataTableCaptionProvider();
                }
                return m_CaptionProvider; }
            set { m_CaptionProvider = value; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name {get {return typeof(R).Name.Substring(0,typeof(R).Name.Length - 3);}}
        /// <summary>
        /// Gets the name of the data table.
        /// </summary>
        /// <value>The name of the data table.</value>
        public string DataTableName {get {return typeof(T).Name.Substring(0,typeof(T).Name.Length - 9);}}
        /// <summary>
        /// Info about the method that creates a new DataRow
        /// </summary>
        protected MethodInfo m_NewRow = null;
        /// <summary>
        /// Info about the method that adds a DataRow to a DataTable
        /// </summary>
        protected MethodInfo m_AddRow = null;

        /// <summary>
        /// Gets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption
        {
            get
            {
                return CaptionProvider.GetCaption(typeof(T));
            }
        }

        /// <summary>
        /// Gets the caption plural.
        /// </summary>
        /// <value>The caption plural.</value>
        public string CaptionPlural
        {
            get
            {
                return CaptionProvider.GetCaptionPlural(typeof(T));
            }
        }

        SortedDictionary<IComparable, Type> m_WrapperTypes = new SortedDictionary<IComparable, Type>();

        /// <summary>
        /// Gets the type of the wrapper.
        /// </summary>
        /// <param name="aspect">The aspect.</param>
        /// <returns></returns>
        public Type GetWrapperType(IComparable aspect)
        {
            Type type = null;
            m_WrapperTypes.TryGetValue(aspect, out type);
            return type;
        }

        /// <summary>
        /// Wraps the specified row.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="context">The context.</param>
        /// <param name="aspect">The aspect.</param>
        /// <returns></returns>
        public IDataRowWrapper Wrap(DataRow row, object context, IComparable aspect)
        {
            if (row.GetType() != typeof(R))
                return null;

            Type type = null;
            if (!m_WrapperTypes.TryGetValue(aspect, out type))
                throw new Exception(string.Format("No such wrapper type exists ({0}).", aspect));
            
            IDataRowWrapper wrapper = Activator.CreateInstance(type) as IDataRowWrapper;
            wrapper.Initialize(row, context);
            return wrapper;
        }

        /// <summary>
        /// Adds the aspect.
        /// </summary>
        /// <param name="aspect">The aspect.</param>
        /// <param name="wrapperType">Type of the wrapper.</param>
        public void AddAspect(IComparable aspect, Type wrapperType)
        {
            m_WrapperTypes[aspect] = wrapperType;
        }

        /// <summary>
        /// Creates the new row.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <returns></returns>
        public DataRow CreateNewRow(DataSet dataSet)
        {
            T table = dataSet.Tables[DataTableName] as T;
            if (m_NewRow == null)
            {
                m_NewRow = typeof(T).GetMethod(string.Format("New{0}", typeof(R).Name));
            }
            return m_NewRow.Invoke(table, new object[] { }) as DataRow;
        }

        /// <summary>
        /// Fills the row defaults.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="row">The row.</param>
        public void FillRowDefaults(DataSet dataSet, DataRow row)
        {
            IDataRowCustomFillDefaults cr = null;
            if (typeof(IDataRowCustomFillDefaults).IsAssignableFrom(row.GetType()))
            {
                cr = (IDataRowCustomFillDefaults)row;
            }

            if (cr != null)
                cr.OnBeforeFillDefaults(dataSet);

            try
            {
                SortedDictionary<string, DataColumn> columns = new SortedDictionary<string, DataColumn>();
                foreach (DataColumn dc in row.Table.Columns)
                {
                    columns.Add(dc.ColumnName, dc);
                }

                //First ensure we have default primary keys
                foreach (DataColumn dc in row.Table.PrimaryKey)
                {
                    if (row.IsNull(dc))
                    {
                        if (dc.DataType == typeof(string))
                            row[dc.Ordinal] = row.GetHashCode().ToString();
                        else if (dc.DataType == typeof(int))
                            row[dc.Ordinal] = row.GetHashCode();
                        else if (dc.DataType == typeof(Guid))
                            row[dc.Ordinal] = Guid.NewGuid();
                    }
                    columns.Remove(dc.ColumnName); //exclude column from consideration
                }

                //remove foreign key columns from consideration
                foreach (DataRelation r in row.Table.ParentRelations)
                {
                    foreach (DataColumn dcChild in r.ChildColumns)
                    {
                        columns.Remove(dcChild.ColumnName);
                    }
                }

                //Now fill in any non-nullable, considered columns
                foreach (DataColumn dc in columns.Values)
                {
                    if (!dc.AllowDBNull && row.IsNull(dc))
                    {
                        if (dc.DataType == typeof(string))
                            row[dc.Ordinal] = string.Empty;
                        else if (dc.DataType == typeof(int))
                            row[dc.Ordinal] = 0;
                        else if (dc.DataType == typeof(bool))
                            row[dc.Ordinal] = true;
                        else if (dc.DataType == typeof(Guid))
                            row[dc.Ordinal] = Guid.Empty;
                    }
                }

                
            }
            catch
            {
            }

            OnFillDefaults(dataSet, row);

            if (cr != null)
                cr.OnAfterFillDefaults(dataSet);
        }

        /// <summary>
        /// Called when [fill defaults].
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="row">The row.</param>
        protected virtual void OnFillDefaults(DataSet dataSet, DataRow row)
        {
        }


        /// <summary>
        /// Adds the row.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="row">The row.</param>
        public void AddRow(DataSet dataSet, DataRow row)
        {
            IDataRowCustomAdd cr = null;
            if (typeof(IDataRowCustomAdd).IsAssignableFrom(row.GetType()))
            {
                cr = (IDataRowCustomAdd)row;
            }

            if (cr != null)
                cr.OnBeforeAdd(dataSet);
            T table = dataSet.Tables[DataTableName] as T;
            if (m_AddRow == null)
            {
                m_AddRow = typeof(T).GetMethod(string.Format("Add{0}", typeof(R).Name), new Type[] { typeof(R)});
            }
            try
            {
                m_AddRow.Invoke(table, new object[] { row });
            }
            catch (TargetInvocationException x)
            {
                throw x.InnerException;
            }
            if (cr != null)
                cr.OnAfterAdd(dataSet);
        }

        /// <summary>
        /// Deletes the row.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="row">The row.</param>
        public void DeleteRow(DataSet dataSet, DataRow row)
        {
            IDataRowCustomDelete cr = null;
            if (typeof(IDataRowCustomDelete).IsAssignableFrom(row.GetType()))
            {
                cr = (IDataRowCustomDelete)row;
            }
            if (cr != null)
                cr.OnBeforeDelete(dataSet);
            foreach (DataRelation childRelation in row.Table.ChildRelations)
            {
                if (childRelation.ChildKeyConstraint == null)
                {
                    bool allowsNull = true;

                    foreach (DataColumn col in childRelation.ChildColumns)
                    {
                        if (!col.AllowDBNull)
                            allowsNull = false;
                    }
                    if (allowsNull)
                    {
                        DataRow[] rows = row.GetChildRows(childRelation);
                        foreach (DataRow dr in rows)
                        {
                            foreach (DataColumn col in childRelation.ChildColumns)
                            {
                                dr[col.Ordinal] = DBNull.Value;
                            }
                        }
                    }
                }
            }
            row.Delete();
            if (cr != null)
                cr.OnAfterDelete(dataSet);
        }

        /// <summary>
        /// Creates a new table adapter.
        /// </summary>
        /// <returns></returns>
        public Component CreateNewTableAdapter()
        {
            Component tableAdapter = null;
            if (typeof(A) == typeof(TableAdapterExtensions.DefaultTableAdapter))
            {
                tableAdapter = Activator.CreateInstance(typeof(A), typeof(T)) as Component;
            }
            else
            {
                tableAdapter = Activator.CreateInstance(typeof(A)) as Component;
            }
            return tableAdapter;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return this.Caption;
        }
    }
}
