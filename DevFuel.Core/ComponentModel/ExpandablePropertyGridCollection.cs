using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DevFuel.Core.ComponentModel
{
    //Sample usage:
    //[Category("MyCategory")]
    //[DisplayName("MyProperty")]
    //[Description("MyDescription")]
    //[TypeConverter(typeof(ExpandableObjectConverter))]
    //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    //public ExpandablePropertyGridCollection<MyType> MyProperty {...}


    /// <summary>
    /// An Interface that represents the ability of an item to retrieve its property name (as opposed to its value)
    /// </summary>
    public interface IExpandablePropertyGridCollectionItem
    {
        /// <summary>
        /// Returns the property name string for this item.
        /// </summary>
        /// <returns></returns>
        string ToPropertyNameString();
    }

    /// <summary>
    /// A Templated collection of items that can be expanded in the PropertyGrid control
    /// </summary>
    /// <typeparam name="T">An IExpandablePropertyGridCollectionItem derived Type</typeparam>
    public class ExpandablePropertyGridCollection<T> : ICustomTypeDescriptor where T: IExpandablePropertyGridCollectionItem
    {
        private ICollection<T> m_Collection;
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpandablePropertyGridCollection&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="c">The c.</param>
        public ExpandablePropertyGridCollection(ICollection<T> c)
        {
            m_Collection = c;
        }

        public ExpandablePropertyGridCollection()
        {
            m_Collection = null;
        }

        protected void SetCollection(ICollection<T> c)
        {
            m_Collection = c;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpandablePropertyGridCollection&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="c">The c.</param>
        public ExpandablePropertyGridCollection(T[] c)
        {
            m_Collection = new List<T>();
            foreach (T t in c)
                m_Collection.Add(t);
        }

        private AttributeCollection m_Attributes = new AttributeCollection();
        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public AttributeCollection Attributes
        {
            get { return m_Attributes; }
            set { m_Attributes = value; }
        }


        /// <summary>
        /// Returns a collection of custom attributes for this instance of a component.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.ComponentModel.AttributeCollection"/> containing the attributes for this object.
        /// </returns>
        public AttributeCollection GetAttributes()
        {
            return m_Attributes;
        }

        /// <summary>
        /// Returns the class name of this instance of a component.
        /// </summary>
        /// <returns>
        /// The class name of the object, or null if the class does not have a name.
        /// </returns>
        public string GetClassName()
        {
            return string.Empty;
        }

        /// <summary>
        /// Returns the name of this instance of a component.
        /// </summary>
        /// <returns>
        /// The name of the object, or null if the object does not have a name.
        /// </returns>
        public string GetComponentName()
        {
            return string.Empty;
        }

        /// <summary>
        /// Returns a type converter for this instance of a component.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.TypeConverter"/> that is the converter for this object, or null if there is no <see cref="T:System.ComponentModel.TypeConverter"/> for this object.
        /// </returns>
        public TypeConverter GetConverter()
        {
            return new ExpandableObjectConverter();
        }

        /// <summary>
        /// Returns the default event for this instance of a component.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.ComponentModel.EventDescriptor"/> that represents the default event for this object, or null if this object does not have events.
        /// </returns>
        public EventDescriptor GetDefaultEvent()
        {
            return null;
        }

        /// <summary>
        /// Returns the default property for this instance of a component.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptor"/> that represents the default property for this object, or null if this object does not have properties.
        /// </returns>
        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        /// <summary>
        /// Returns an editor of the specified type for this instance of a component.
        /// </summary>
        /// <param name="editorBaseType">A <see cref="T:System.Type"/> that represents the editor for this object.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> of the specified type that is the editor for this object, or null if the editor cannot be found.
        /// </returns>
        public object GetEditor(Type editorBaseType)
        {
            return null;
        }

        /// <summary>
        /// Returns the events for this instance of a component using the specified attribute array as a filter.
        /// </summary>
        /// <param name="attributes">An array of type <see cref="T:System.Attribute"/> that is used as a filter.</param>
        /// <returns>
        /// An <see cref="T:System.ComponentModel.EventDescriptorCollection"/> that represents the filtered events for this component instance.
        /// </returns>
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return null;
        }

        /// <summary>
        /// Returns the events for this instance of a component.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.ComponentModel.EventDescriptorCollection"/> that represents the events for this component instance.
        /// </returns>
        public EventDescriptorCollection GetEvents()
        {
            return null;
        }

        /// <summary>
        /// Returns the properties for this instance of a component using the attribute array as a filter.
        /// </summary>
        /// <param name="attributes">An array of type <see cref="T:System.Attribute"/> that is used as a filter.</param>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> that represents the filtered properties for this component instance.
        /// </returns>
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return GetProperties();
        }

        /// <summary>
        /// Returns the properties for this instance of a component.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> that represents the properties for this component instance.
        /// </returns>
        public PropertyDescriptorCollection GetProperties()
        {
            PropertyDescriptorCollection c = new PropertyDescriptorCollection(new PropertyDescriptor[] { }, false);

            Attribute[] aa = new Attribute[0];
            if (m_Attributes != null)
            {
                aa = new Attribute[m_Attributes.Count];
                m_Attributes.CopyTo(aa, 0);
            }
            foreach (T t in m_Collection)
            {
                c.Add(new ExpandablePropertyGridItemDescriptor<T>(t, aa));
            }
            return c;
        }

        /// <summary>
        /// Returns an object that contains the property described by the specified property descriptor.
        /// </summary>
        /// <param name="pd">A <see cref="T:System.ComponentModel.PropertyDescriptor"/> that represents the property whose owner is to be found.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that represents the owner of the specified property.
        /// </returns>
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return null;
        }
        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Empty;
        }

        /// <summary>
        /// A Property Descriptor that can describe an expandable property grid collection item
        /// </summary>
        /// <typeparam name="V"></typeparam>
        public class ExpandablePropertyGridItemDescriptor<V> : PropertyDescriptor where V : IExpandablePropertyGridCollectionItem
        {
            V m_Item;
            /// <summary>
            /// Initializes a new instance of the <see cref="ExpandablePropertyGridCollection&lt;T&gt;.ExpandablePropertyGridItemDescriptor&lt;V&gt;"/> class.
            /// </summary>
            /// <param name="item">The item.</param>
            /// <param name="ac">The ac.</param>
            public ExpandablePropertyGridItemDescriptor(V item, Attribute[] ac)
                : base(item.ToPropertyNameString(), ac)
            {
                m_Item = item;
            }

            /// <summary>
            /// When overridden in a derived class, returns whether resetting an object changes its value.
            /// </summary>
            /// <param name="component">The component to test for reset capability.</param>
            /// <returns>
            /// true if resetting the component changes its value; otherwise, false.
            /// </returns>
            public override bool CanResetValue(object component)
            {
                return false;
            }

            /// <summary>
            /// When overridden in a derived class, gets the type of the component this property is bound to.
            /// </summary>
            /// <value></value>
            /// <returns>A <see cref="T:System.Type"/> that represents the type of component this property is bound to. When the <see cref="M:System.ComponentModel.PropertyDescriptor.GetValue(System.Object)"/> or <see cref="M:System.ComponentModel.PropertyDescriptor.SetValue(System.Object,System.Object)"/> methods are invoked, the object specified might be an instance of this type.</returns>
            public override Type ComponentType
            {
                get { return typeof(ExpandablePropertyGridCollection<V>); }
            }

            /// <summary>
            /// When overridden in a derived class, gets the current value of the property on a component.
            /// </summary>
            /// <param name="component">The component with the property for which to retrieve the value.</param>
            /// <returns>
            /// The value of a property for a given component.
            /// </returns>
            public override object GetValue(object component)
            {
                return m_Item;
            }

            /// <summary>
            /// When overridden in a derived class, gets a value indicating whether this property is read-only.
            /// </summary>
            /// <value></value>
            /// <returns>true if the property is read-only; otherwise, false.</returns>
            public override bool IsReadOnly
            {
                get { return false; }
            }

            /// <summary>
            /// When overridden in a derived class, gets the type of the property.
            /// </summary>
            /// <value></value>
            /// <returns>A <see cref="T:System.Type"/> that represents the type of the property.</returns>
            public override Type PropertyType
            {
                get { return m_Item.GetType(); }
            }

            /// <summary>
            /// When overridden in a derived class, resets the value for this property of the component to the default value.
            /// </summary>
            /// <param name="component">The component with the property value that is to be reset to the default value.</param>
            public override void ResetValue(object component)
            {
            }

            /// <summary>
            /// When overridden in a derived class, sets the value of the component to a different value.
            /// </summary>
            /// <param name="component">The component with the property value that is to be set.</param>
            /// <param name="value">The new value.</param>
            public override void SetValue(object component, object value)
            {
            }

            /// <summary>
            /// When overridden in a derived class, determines a value indicating whether the value of this property needs to be persisted.
            /// </summary>
            /// <param name="component">The component with the property to be examined for persistence.</param>
            /// <returns>
            /// true if the property should be persisted; otherwise, false.
            /// </returns>
            public override bool ShouldSerializeValue(object component)
            {
                return true;
            }
        }

    }
}
