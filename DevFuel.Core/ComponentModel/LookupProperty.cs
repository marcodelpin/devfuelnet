using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using DevFuel.Core.Data;

namespace DevFuel.Core.ComponentModel
{

    /// <summary>
    /// Interface that defines the behavior of a Property that looks up its value from some other data source
    /// </summary>
    public interface ILookupProperty
    {
        /// <summary>
        /// Gets a value indicating whether the property allows null.
        /// </summary>
        /// <value><c>true</c> if property allows null; otherwise, <c>false</c>.</value>
        bool AllowNull {get;}
        /// <summary>
        /// Gets a value indicating whether the property allows the creation of a new item.
        /// </summary>
        /// <value><c>true</c> if [allow new]; otherwise, <c>false</c>.</value>
        bool AllowNew {get;}
        /// <summary>
        /// Gets a value indicating whether [allow edit].
        /// </summary>
        /// <value><c>true</c> if [allow edit]; otherwise, <c>false</c>.</value>
        bool AllowEdit { get;}
        /// <summary>
        /// Gets a value indicating whether [allow delete].
        /// </summary>
        /// <value><c>true</c> if [allow delete]; otherwise, <c>false</c>.</value>
        bool AllowDelete { get;}
        /// <summary>
        /// Gets the binding source.
        /// </summary>
        /// <value>The binding source.</value>
        BindingSource BindingSource {get;}
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        object Value {get; set;}
        /// <summary>
        /// Gets or sets the aspect.
        /// </summary>
        /// <value>The aspect.</value>
        IComparable Aspect { get; set;}
        /// <summary>
        /// Occurs when [value changed].
        /// </summary>
        event EventHandler ValueChanged;
        /// <summary>
        /// Raises the <see cref="E:ValueChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void OnValueChanged(EventArgs e);
        /// <summary>
        /// Creates the new value.
        /// </summary>
        /// <returns></returns>
        object CreateNewValue();
        /// <summary>
        /// Edits the value.
        /// </summary>
        /// <param name="value">The value.</param>
        void EditValue(object value);
        /// <summary>
        /// Deletes the value.
        /// </summary>
        /// <param name="value">The value.</param>
        void DeleteValue(object value);
    }

    /// <summary>
    /// Templated Implementation of ILookupProperty
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LookupProperty<T> : ILookupProperty where T:class
    {
        private IComparable m_Aspect;
        /// <summary>
        /// Gets or sets the aspect.
        /// </summary>
        /// <value>The aspect.</value>
        public IComparable Aspect
        {
            get { return m_Aspect; }
            set { m_Aspect = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupProperty&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="bindingSource">The binding source.</param>
        /// <param name="allowNull">if set to <c>true</c> [allow null].</param>
        /// <param name="allowNew">if set to <c>true</c> [allow new].</param>
        /// <param name="allowEdit">if set to <c>true</c> [allow edit].</param>
        /// <param name="allowDelete">if set to <c>true</c> [allow delete].</param>
        public LookupProperty(T value, BindingSource bindingSource, bool allowNull, bool allowNew, bool allowEdit, bool allowDelete)
        {
            m_AllowNull = allowNull;
            m_AllowNew = allowNew;
            m_AllowEdit = allowEdit;
            m_AllowDelete = allowDelete;
            m_BindingSource = bindingSource;
            m_ValueT = value;
        }

        /// <summary>
        /// Gets the name of the property type.
        /// </summary>
        /// <value>The name of the property type.</value>
        public virtual string PropertyTypeName { get { return typeof(T).Name; } }

        private bool m_AllowNull = true;
        /// <summary>
        /// Gets a value indicating whether the property allows null.
        /// </summary>
        /// <value><c>true</c> if property allows null; otherwise, <c>false</c>.</value>
        public bool AllowNull
        {
            get { return m_AllowNull; }
            set { m_AllowNull = value; }
        }

        private bool m_AllowEdit = true;
        /// <summary>
        /// Gets a value indicating whether [allow edit].
        /// </summary>
        /// <value><c>true</c> if [allow edit]; otherwise, <c>false</c>.</value>
        public bool AllowEdit
        {
            get { return m_AllowEdit; }
            set { m_AllowEdit = value; }
        }

        private bool m_AllowDelete = true;
        /// <summary>
        /// Gets a value indicating whether [allow delete].
        /// </summary>
        /// <value><c>true</c> if [allow delete]; otherwise, <c>false</c>.</value>
        public bool AllowDelete
        {
            get { return m_AllowDelete; }
            set { m_AllowDelete = value; }
        }

        private bool m_AllowNew = true;
        /// <summary>
        /// Gets a value indicating whether the property allows the creation of a new item.
        /// </summary>
        /// <value><c>true</c> if [allow new]; otherwise, <c>false</c>.</value>
        public bool AllowNew
        {
            get { return m_AllowNew && m_BindingSource.AllowNew; }
            set { m_AllowNew = value; }
        }

        private BindingSource m_BindingSource;
        /// <summary>
        /// Gets the binding source.
        /// </summary>
        /// <value>The binding source.</value>
        public BindingSource BindingSource
        {
            get { return m_BindingSource; }
            set { m_BindingSource = value; }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public object Value
        {
            get { return m_ValueT; }
            set { ValueT = value as T; }
        }

        private T m_ValueT;
        /// <summary>
        /// Gets or sets the value T.
        /// </summary>
        /// <value>The value T.</value>
        public T ValueT
        {
            get { return m_ValueT; }
            set
            {
                if (value != m_ValueT)
                {
                    m_ValueT = value;
                    OnValueChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Occurs when [value changed].
        /// </summary>
        public event EventHandler ValueChanged;
        /// <summary>
        /// Raises the <see cref="E:ValueChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void OnValueChanged(EventArgs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, e);
            }
        }

        /// <summary>
        /// Creates the new value.
        /// </summary>
        /// <returns></returns>
        public virtual object CreateNewValue()
        {
            return null;
        }

        /// <summary>
        /// Edits the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public virtual void EditValue(object value)
        {
        }

        /// <summary>
        /// Deletes the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public virtual void DeleteValue(object value)
        {
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            if (m_ValueT == null)
                return Strings.PropGridNull;
            return m_ValueT.ToString();
        }
        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
        public override bool Equals(object obj)
        {
            bool equals = false;
            if (obj != null && m_ValueT != null)
            {
                if (obj is LookupProperty<T>)
                {
                    equals = m_ValueT.Equals((obj as LookupProperty<T>).Value);
                }
                else
                {
                    equals = m_ValueT.Equals(obj);
                }
            }
            else
                equals = base.Equals(obj);
            return equals;
        }
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            if (m_ValueT != null)
                return m_ValueT.GetHashCode();
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// A Lookup Property that does its lookup from a DataTable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataTableLookupProperty<T> : LookupProperty<T> where T : DataRow
    {
        private DataTable m_DataTable;
        /// <summary>
        /// Gets the data table.
        /// </summary>
        /// <value>The data table.</value>
        public DataTable DataTable
        {
            get { return m_DataTable; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableLookupProperty&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="dataSet">The data set.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="allowNull">if set to <c>true</c> [allow null].</param>
        /// <param name="allowNew">if set to <c>true</c> [allow new].</param>
        /// <param name="allowEdit">if set to <c>true</c> [allow edit].</param>
        /// <param name="allowDelete">if set to <c>true</c> [allow delete].</param>
        public DataTableLookupProperty(T value, DataSet dataSet, string tableName, bool allowNull, bool allowNew, bool allowEdit, bool allowDelete)
            : base(value, new BindingSource(dataSet, tableName), allowNull, allowNew, allowEdit, allowDelete)
        {
            m_DataTable = dataSet.Tables[tableName];
        }
    }
}