using System;
using System.Collections.Generic;
using System.ComponentModel;


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
    class ProvidedViewPropertyDescriptorEx<T> : PropertyDescriptor
    {
        public ProvidedViewPropertyDescriptorEx(string name, Type propertyType)
            : base(name, null)
        {
            _propertyType = propertyType;
        }

        private Type _propertyType;

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get { return typeof(ObjectView<T>); }
        }

        public override object GetValue(object component)
        {
            if (ComponentType.IsAssignableFrom(component.GetType()))
            {
                return (component as ObjectView<T>).GetProvidedView(Name);
            }

            throw new ArgumentException("Type of component is not valid.", "component");
        }

        public override bool IsReadOnly
        {
            get { return true; }
        }

        public override Type PropertyType
        {
            get { return _propertyType; }
        }

        public override void ResetValue(object component)
        {
            throw new NotSupportedException();
        }

        public override void SetValue(object component, object value)
        {
            throw new NotSupportedException();
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public static bool CanProvideViewOf(PropertyDescriptor prop)
        {
            Type listTypeDef = typeof(IList<object>).GetGenericTypeDefinition();
            Type propType = prop.PropertyType;
            Type[] args = propType.GetGenericArguments();
            // Is this a generic type, with only one generic parameter.
            if (args.Length == 1)
            {
                // Create type IList<T> where T is args[0]
                Type listType = listTypeDef.MakeGenericType(args);
                // Check if the property type implements IList<T>
                // but is not an IBindingListView (or better).
                if (listType.IsAssignableFrom(propType) && !typeof(IBindingListView).IsAssignableFrom(propType))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
