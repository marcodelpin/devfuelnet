using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;


#region COPYRIGHT
/*
The following code originated from blw.sourceforge.net and has been adapted 
by E. Edwards of http://www.devfuel.com in order to provide additional features. It remains under the BSD license:
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

namespace DevFuel.Core.ComponentModel
{
    /// <summary>
    /// Serves a wrapper for items being viewed in a <see cref="BindingListView&lt;T&gt;"/>.
    /// This class implements <see cref="INotifyingEditableObject"/> so will raise the necessary events during 
    /// the item edit life-cycle.
    /// </summary>
    /// <remarks>
    /// If <typeparamref name="T"/> implements <see cref="System.ComponentModel.IEditableObject"/> this class will call BeginEdit/CancelEdit/EndEdit on the <typeparamref name="T"/> object as well.
    /// If <typeparamref name="T"/> implements <see cref="System.ComponentModel.IDataErrorInfo"/> this class will use that implementation as its own.
    /// </remarks>
    /// <typeparam name="T">The type of object being viewed.</typeparam>
    [Serializable]
    public class ObjectView<T> : INotifyingEditableObject, IDataErrorInfo, INotifyPropertyChanged, ICustomTypeDescriptor
    {
        /// <summary>
        /// Creates a new ObjectViewT wrapper for a <typeparamref name="T"/> object.
        /// </summary>
        /// <param name="object">The <typeparamref name="T"/> object being wrapped.</param>
        /// <param name="parent">The parent.</param>
        public ObjectView(T @object, AggregateBindingListView<T> parent)
        {
            m_Parent = parent;

            Object = @object;
            if (Object is INotifyPropertyChanged)
            {
                ((INotifyPropertyChanged)Object).PropertyChanged += new PropertyChangedEventHandler(ObjectPropertyChanged);
            }

            if (typeof(ICustomTypeDescriptor).IsAssignableFrom(typeof(T)))
            {
                m_IsCustomTypeDescriptor = true;
                m_CustomTypeDescriptor = Object as ICustomTypeDescriptor;
                Debug.Assert(m_CustomTypeDescriptor != null);
            }

            m_ProvidedViews = new Dictionary<string, object>();
            CreateProvidedViews();
        }

        /// <summary>
        /// The view containing this ObjectView.
        /// </summary>
        private AggregateBindingListView<T> m_Parent;
        /// <summary>
        /// Flag that signals if we are currently editing the object.
        /// </summary>
        private bool m_IsEditing;
        /// <summary>
        /// The actual object being edited.
        /// </summary>
        private T m_Object;
        /// <summary>
        /// Flag set to true if type of T implements ICustomTypeDescriptor
        /// </summary>
        private bool m_IsCustomTypeDescriptor;
        /// <summary>
        /// Holds the Object pre-casted ICustomTypeDescriptor (if supported).
        /// </summary>
        private ICustomTypeDescriptor m_CustomTypeDescriptor;
        /// <summary>
        /// A collection of BindingListView objects, indexed by name, for views auto-provided for any generic IList members.
        /// </summary>
        private Dictionary<string, object> m_ProvidedViews;

        /// <summary>
        /// Gets the object being edited.
        /// </summary>
        public T Object
        {
            get { return m_Object; }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Object", Properties.Resources.BLV_ObjectCannotBeNull);
                }
                m_Object = value;
            }
        }

        /// <summary>
        /// Gets the provided view.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public object GetProvidedView(string name)
        {
            return m_ProvidedViews[name];
        }

        /// <summary>
        /// Casts an ObjectView&lt;T&gt; to a T by getting the wrapped T object.
        /// </summary>
        /// <param name="eo">The ObjectView&lt;T&gt; to cast to a T</param>
        /// <returns>The object that is wrapped.</returns>
        public static explicit operator T(ObjectView<T> eo)
        {
            return eo.Object;
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
            if (obj == null)
            {
                return false;
            }

            if (obj is T)
            {
                return Object.Equals(obj);
            }
            else if (obj is ObjectView<T>)
            {
                return Object.Equals((obj as ObjectView<T>).Object);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        private static SortedDictionary<PropertyInfo, PropertyAccessorDelegate> m_PropertyAccessorDelegates = new SortedDictionary<PropertyInfo, PropertyAccessorDelegate>();
        private static SortedDictionary<PropertyInfo, PropertyMutatorDelegate> m_PropertyMutatorDelegates = new SortedDictionary<PropertyInfo, PropertyMutatorDelegate>();

        /// <summary>
        /// Gets the property info.
        /// </summary>
        /// <param name="sPropertyName">Name of the s property.</param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo(string sPropertyName)
        {
            PropertyInfo pi = typeof(T).GetProperty(sPropertyName);
            if (pi == null)
                throw new ArgumentException(string.Format("Property '{0}' is not a member of type '{1}'", sPropertyName, typeof(T).FullName), "propertyName");
            return pi;
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="sPropertyName">Name of the s property.</param>
        /// <returns></returns>
        public static object GetProperty(T obj, string sPropertyName)
        {
            PropertyAccessorDelegate pad = GetPropertyAccessor(sPropertyName);
            return pad(obj);
        }

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="sPropertyName">Name of the s property.</param>
        /// <param name="value">The value.</param>
        public static void SetProperty(T obj, string sPropertyName, object value)
        {
            PropertyMutatorDelegate pmd = GetPropertyMutator(sPropertyName);
            pmd(obj, value);
        }

        /// <summary>
        /// Gets the property accessor.
        /// </summary>
        /// <param name="sPropertyName">Name of the s property.</param>
        /// <returns></returns>
        public static PropertyAccessorDelegate GetPropertyAccessor(string sPropertyName)
        {
            return GetPropertyAccessor(GetPropertyInfo(sPropertyName));
        }

        /// <summary>
        /// Gets the property accessor.
        /// </summary>
        /// <param name="pi">The pi.</param>
        /// <returns></returns>
        public static PropertyAccessorDelegate GetPropertyAccessor(PropertyInfo pi)
        {
            PropertyAccessorDelegate pad = null;
            if (!m_PropertyAccessorDelegates.TryGetValue(pi, out pad))
            {
                pad = BuildPropertyAccessor(pi);
                m_PropertyAccessorDelegates.Add(pi, pad);
            }
            return pad;
        }

        /// <summary>
        /// Gets the property mutator.
        /// </summary>
        /// <param name="sPropertyName">Name of the s property.</param>
        /// <returns></returns>
        public static PropertyMutatorDelegate GetPropertyMutator(string sPropertyName)
        {
            return GetPropertyMutator(GetPropertyInfo(sPropertyName));
        }

        /// <summary>
        /// Gets the property mutator.
        /// </summary>
        /// <param name="pi">The pi.</param>
        /// <returns></returns>
        public static PropertyMutatorDelegate GetPropertyMutator(PropertyInfo pi)
        {
            PropertyMutatorDelegate pmd = null;
            if (!m_PropertyMutatorDelegates.TryGetValue(pi, out pmd))
            {
                pmd = BuildPropertyMutator(pi);
                m_PropertyMutatorDelegates.Add(pi, pmd);
            }
            return pmd;
        }

        /// <summary>
        /// Delegate to Access a Property
        /// </summary>
        public delegate object PropertyAccessorDelegate(T obj);
        /// <summary>
        /// Builds the property accessor.
        /// </summary>
        /// <param name="pi">The pi.</param>
        /// <returns></returns>
        private static PropertyAccessorDelegate BuildPropertyAccessor(PropertyInfo pi)
        {
            MethodInfo getMethod = pi.GetGetMethod();
            Debug.Assert(getMethod != null);

            DynamicMethod dm = new DynamicMethod("__blv_get_" + pi.Name, typeof(object), new Type[] { typeof(T) }, typeof(ObjectView<T>), true);
            ILGenerator il = dm.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.EmitCall(OpCodes.Call, getMethod, null);

            // Return the result of the comparison.
            il.Emit(OpCodes.Ret);

            // Create the delegate pointing at the dynamic method.
            return (PropertyAccessorDelegate)dm.CreateDelegate(typeof(PropertyAccessorDelegate));
        }

        /// <summary>
        /// Delegate to Mutate a Property
        /// </summary>
        public delegate void PropertyMutatorDelegate(T obj, object value);
        /// <summary>
        /// Builds the property mutator.
        /// </summary>
        /// <param name="pi">The pi.</param>
        /// <returns></returns>
        private static PropertyMutatorDelegate BuildPropertyMutator(PropertyInfo pi)
        {
            MethodInfo setMethod = pi.GetSetMethod();
            Debug.Assert(setMethod != null);

            DynamicMethod dm = new DynamicMethod("__blv_set_" + pi.Name, typeof(object), new Type[] { typeof(T) }, typeof(ObjectView<T>), true);
            ILGenerator il = dm.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            if(pi.PropertyType.IsValueType)
                il.Emit(OpCodes.Box, pi.PropertyType);
            il.EmitCall(OpCodes.Call, setMethod, null);

            // Return the result of the comparison.
            il.Emit(OpCodes.Ret);

            // Create the delegate pointing at the dynamic method.
            return (PropertyMutatorDelegate)dm.CreateDelegate(typeof(PropertyMutatorDelegate));
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified property name.
        /// </summary>
        /// <value></value>
        public object this[string sPropertyName]
        {
            get
            {
                return GetProperty(this.Object, sPropertyName);
            }
            set
            {
                SetProperty(this.Object, sPropertyName, value);
            }
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return Object.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return Object.ToString();
        }
        
        private void ObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Raise our own event
            OnPropertyChanged(sender, new PropertyChangedEventArgs(e.PropertyName));
        }

        private bool ShouldProvideViewOf(PropertyDescriptor listProp)
        {
            return m_Parent.ShouldProvideView(listProp);
        }

        private string GetProvidedViewName(PropertyDescriptor listProp)
        {
            return m_Parent.GetProvidedViewName(listProp);
        }

        private void CreateProvidedViews()
        {
            foreach (PropertyDescriptor prop in (this as ICustomTypeDescriptor).GetProperties())
            {
                if (ShouldProvideViewOf(prop))
                {
                    object view = m_Parent.CreateProvidedView(this, prop);
                    string viewName = GetProvidedViewName(prop);
                    m_ProvidedViews.Add(viewName, view);
                }
            }
        }

        #region INotifyEditableObject Members

        /// <summary>
        /// Indicates an edit has just begun.
        /// </summary>
        [field: NonSerialized]
        public event EventHandler EditBegun;

        /// <summary>
        /// Indicates the edit was cancelled.
        /// </summary>
        [field: NonSerialized]
        public event EventHandler EditCancelled;

        /// <summary>
        /// Indicated the edit was ended.
        /// </summary>
        [field: NonSerialized]
        public event EventHandler EditEnded;

        /// <summary>
        /// Called when [edit begun].
        /// </summary>
        protected virtual void OnEditBegun()
        {
            if (EditBegun != null)
            {
                EditBegun(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when [edit cancelled].
        /// </summary>
        protected virtual void OnEditCancelled()
        {
            if (EditCancelled != null)
            {
                EditCancelled(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when [edit ended].
        /// </summary>
        protected virtual void OnEditEnded()
        {
            if (EditEnded != null)
            {
                EditEnded(this, EventArgs.Empty);
            }
        }

        #endregion

        #region IEditableObject Members

        /// <summary>
        /// Begins an edit on an object.
        /// </summary>
        public void BeginEdit()
        {
            // As per documentation, this method may get called multiple times for a single edit.
            // So we set a flag to only honor the first call.
            if (!m_IsEditing)
            {
                m_IsEditing = true;

                // If possible call the object'schemaSet BeginEdit() method
                // to let it do what ever it needs e.g. save state
                if (Object is IEditableObject)
                {
                    ((IEditableObject)Object).BeginEdit();
                }
                // Raise the EditBegun event.                
                OnEditBegun();
            }
        }

        /// <summary>
        /// Discards changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> call.
        /// </summary>
        public void CancelEdit()
        {
            // We can only cancel if currently editing
            if (m_IsEditing)
            {
                // If possible call the object'schemaSet CancelEdit() method
                // to let it do what ever it needs e.g. rollback state
                if (Object is IEditableObject)
                {
                    ((IEditableObject)Object).CancelEdit();
                }
                // Raise the EditCancelled event.
                OnEditCancelled();
                // No longer editing now.
                m_IsEditing = false;
            }
        }

        /// <summary>
        /// Pushes changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> or <see cref="M:System.ComponentModel.IBindingList.AddNew"/> call into the underlying object.
        /// </summary>
        public void EndEdit()
        {
            // We can only end if currently editing
            if (m_IsEditing)
            {
                // If possible call the object'schemaSet EndEdit() method
                // to let it do what ever it needs e.g. commit state
                if (Object is IEditableObject)
                {
                    ((IEditableObject)Object).EndEdit();
                }
                // Raise the EditEnded event.
                OnEditEnded();
                // No longer editing now.
                m_IsEditing = false;
            }
        }

        #endregion

        #region IDataErrorInfo Members

        // If the wrapped Object support IDataErrorInfo we forward calls to it.
        // Otherwise, we just return empty strings that signal "no error".

        string IDataErrorInfo.Error
        {
            get
            {
                if (Object is IDataErrorInfo)
                {
                    return ((IDataErrorInfo)Object).Error;
                }
                return string.Empty;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (Object is IDataErrorInfo)
                {
                    return ((IDataErrorInfo)Object)[columnName];
                }
                return string.Empty;
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(sender, args);
            }
        }

        #endregion

        #region ICustomTypeDescriptor Members

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            if (m_IsCustomTypeDescriptor)
            {
                return m_CustomTypeDescriptor.GetAttributes();
            }
            else
            {
                return TypeDescriptor.GetAttributes(Object);
            }
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            if (m_IsCustomTypeDescriptor)
            {
                return m_CustomTypeDescriptor.GetClassName();
            }
            else
            {
                return typeof(T).FullName;
            }
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            if (m_IsCustomTypeDescriptor)
            {
                return m_CustomTypeDescriptor.GetComponentName();
            }
            else
            {
                return TypeDescriptor.GetFullComponentName(Object);
            }
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            if (m_IsCustomTypeDescriptor)
            {
                return m_CustomTypeDescriptor.GetConverter();
            }
            else
            {
                return TypeDescriptor.GetConverter(Object);
            }
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            if (m_IsCustomTypeDescriptor)
            {
                return m_CustomTypeDescriptor.GetDefaultEvent();
            }
            else
            {
                return TypeDescriptor.GetDefaultEvent(Object);
            }
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            if (m_IsCustomTypeDescriptor)
            {
                return m_CustomTypeDescriptor.GetDefaultProperty();
            }
            else
            {
                return TypeDescriptor.GetDefaultProperty(Object);
            }
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            if (m_IsCustomTypeDescriptor)
            {
                return m_CustomTypeDescriptor.GetEditor(editorBaseType);
            }
            else
            {
                return null;
            }
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            if (m_IsCustomTypeDescriptor)
            {
                return m_CustomTypeDescriptor.GetEvents();
            }
            else
            {
                return TypeDescriptor.GetEvents(Object, attributes);
            }
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            if (m_IsCustomTypeDescriptor)
            {
                return m_CustomTypeDescriptor.GetEvents();
            }
            else
            {
                return TypeDescriptor.GetEvents(Object);
            }
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            if (m_IsCustomTypeDescriptor)
            {
                return m_CustomTypeDescriptor.GetProperties();
            }
            else
            {
                return TypeDescriptor.GetProperties(Object, attributes);
            }
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            if (m_IsCustomTypeDescriptor)
            {
                return m_CustomTypeDescriptor.GetProperties();
            }
            else
            {
                return TypeDescriptor.GetProperties(Object);
            }
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            if (m_IsCustomTypeDescriptor)
            {
                return m_CustomTypeDescriptor.GetPropertyOwner(pd);
            }
            else
            {
                return Object;
            }
        }

        #endregion
    }
}
