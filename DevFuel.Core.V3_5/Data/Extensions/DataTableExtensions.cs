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
using System.Reflection;


namespace DevFuel.Core.Data.Extensions
{
    /// <summary>
    /// Host for Extension Methods on the DataTable class
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// A Parent/Child Relationship that is needed to be accounted for in a DataTable merge. (When merge of one table relies on one or or more parent tables
        /// </summary>
        public class MergeDataRelation
        {
            private DataRelation m_DataRelation;
            /// <summary>
            /// Gets or sets the data relation.
            /// </summary>
            /// <value>The data relation.</value>
            public DataRelation DataRelation
            {
                get { return m_DataRelation; }
                set { m_DataRelation = value; }
            }

            private string m_KeyColumnName;
            /// <summary>
            /// Gets or sets the name of the key column.
            /// </summary>
            /// <value>The name of the key column.</value>
            public string KeyColumnName
            {
                get { return m_KeyColumnName; }
                set { m_KeyColumnName = value; }
            }


            /// <summary>
            /// Initializes a new instance of the <see cref="MergeDataRelation"/> class.
            /// </summary>
            /// <param name="keyColumnName">Name of the key column.</param>
            /// <param name="dr">The dr.</param>
            public MergeDataRelation(string keyColumnName, DataRelation dr)
            {
                m_DataRelation = dr;
                m_KeyColumnName = keyColumnName;
            }
        }


        /// <summary>
        /// Gets the type of the data row.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <returns></returns>
        public static Type GetDataRowType(this DataTable dataTable)
        {
            Type type = dataTable.GetType();
            MethodInfo mi = type.GetMethod("GetRowType", BindingFlags.Instance | BindingFlags.NonPublic);
            if (mi != null && dataTable != null)
                return (Type)mi.Invoke(dataTable, new object[] { });
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Merges from.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="destDataTable">The dest data table.</param>
        /// <param name="srcDataTable">The SRC data table.</param>
        /// <param name="keyColumnName">Name of the key column.</param>
        /// <param name="parentRelations">The parent relations.</param>
        public static void MergeFrom<T>(this DataTable destDataTable, DataTable srcDataTable, string keyColumnName, params MergeDataRelation[] parentRelations)
        {
            SortedDictionary<T, DataRow[]> rowMatches = new SortedDictionary<T, DataRow[]>();
            foreach (DataRow srcRow in srcDataTable.Rows)
            {
                if (!srcRow.IsNull(keyColumnName))
                    rowMatches.Add((T)srcRow[keyColumnName], new DataRow[] { null, srcRow });
            }

            foreach (DataRow destRow in destDataTable.Select())
            {
                DataRow[] p;
                if (rowMatches.TryGetValue((T)destRow[keyColumnName], out p))
                    p[0] = destRow;
                else //Delete...destination without a source
                {
                    destRow.Delete();
                }
            }

            List<DataColumn> dataColumns = new List<DataColumn>();
            foreach (DataColumn col in destDataTable.Columns)
            {
                dataColumns.Add(col);
            }
            foreach (DataColumn primaryKeyColumn in destDataTable.PrimaryKey)
            {
                dataColumns.Remove(primaryKeyColumn);
            }
            foreach (DataRelation rel in destDataTable.ParentRelations)
            {
                foreach (DataColumn foreignKeyColumn in rel.ChildColumns)
                    dataColumns.Remove(foreignKeyColumn);
            }

            foreach (DataRelation rel in destDataTable.ChildRelations)
            {
                foreach (DataColumn foreignKeyColumn in rel.ParentColumns)
                {
                    if (dataColumns.Contains(foreignKeyColumn))
                        dataColumns.Remove(foreignKeyColumn);
                }
            }

            foreach (DataRow[] p in rowMatches.Values)
            {
                DataRow destRow = p[0];
                DataRow srcRow = p[1];
                if (destRow == null) //New...source without destination
                {
                    destRow = destDataTable.NewRow();

                    foreach (MergeDataRelation parentRelation in parentRelations)
                    {
                        //Find the parent row for this relation of the srcRow
                        DataRow sourceParentRow = srcRow.GetParentRow(parentRelation.DataRelation);

                        //Build a where clause that searches for a matching destParentRow based on the keycolumn of the mergedatarelation
                        List<string> expressions = new List<string>();
                        for (int colIndex = 0; colIndex < parentRelation.DataRelation.ParentColumns.Length; colIndex++)
                        {
                            if (parentRelation.DataRelation.ChildColumns[colIndex].DataType == typeof(int))
                                expressions.Add(string.Format("{0} = {1}", parentRelation.KeyColumnName, sourceParentRow[parentRelation.KeyColumnName]));
                            else if (parentRelation.DataRelation.ChildColumns[colIndex].DataType == typeof(Guid) || parentRelation.DataRelation.ChildColumns[colIndex].DataType == typeof(string))
                                expressions.Add(string.Format("{0} = '{1}'", parentRelation.KeyColumnName, sourceParentRow[parentRelation.KeyColumnName]));
                        }

                        string whereClause = string.Join(" AND ", expressions.ToArray());
                        DataRow[] destParentRows = destDataTable.ParentRelations[parentRelation.DataRelation.RelationName].ParentTable.Select(whereClause);

                        if (destParentRows.Length > 0)
                        {
                            for (int colIndex = 0; colIndex < parentRelation.DataRelation.ParentColumns.Length; colIndex++)
                            {
                                DataColumn dcDest = parentRelation.DataRelation.ChildColumns[colIndex];
                                DataColumn dcSrc = parentRelation.DataRelation.ParentColumns[colIndex];
                                destRow[dcDest.ColumnName] = destParentRows[0][dcSrc.ColumnName];
                            }
                        }
                    }

                    foreach (DataColumn dc in dataColumns)
                    {
                        object dest = destRow[dc];
                        object src = srcRow[dc.ColumnName];
                        destRow[dc] = srcRow[dc.ColumnName];
                    }

                    foreach (DataColumn dc in destDataTable.PrimaryKey)
                    {
                        object dest = destRow[dc];
                        object src = srcRow[dc.ColumnName];
                        destRow[dc] = srcRow[dc.ColumnName];
                    }

                    destDataTable.Rows.Add(destRow);
                }
                else //MergeRows
                {
                    foreach (DataColumn dc in dataColumns)
                    {
                        if (dc.ColumnName == keyColumnName || dc.ReadOnly)
                            continue;
                        object dest = destRow[dc];
                        object src = srcRow[dc.ColumnName];
                        if (!src.Equals(dest))
                            destRow[dc] = srcRow[dc.ColumnName];
                    }
                }
            }
        }
    }
}
