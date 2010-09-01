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

//Based roughly on the example from the PowerCollection Point class, 
//and the Tuple idea from http://www.ugmfree.it/TipsCSharp.aspx

using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace DevFuel.Core.Collections.Generic
{
    [Serializable]
    public class Tuple<T1> : IComparable, IComparable<Tuple<T1>>
    {
        private static IComparer<T1> comparerT1 = Comparer<T1>.Default;
        private static IEqualityComparer<T1> eqalityComparerT1 = EqualityComparer<T1>.Default;
        public T1 First;

        public Tuple()
        {
            this.First = default(T1);
        }

        public Tuple(T1 first)
        {
            this.First = first;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Tuple<T1>)
            {
                Tuple<T1> other = (Tuple<T1>)obj;
                return Equals(other);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(Tuple<T1> other)
        {
            return eqalityComparerT1.Equals(First, other.First);
        }

        public override int GetHashCode()
        {
            return (First == null) ? base.GetHashCode() : First.GetHashCode();
        }

        public int CompareTo(Tuple<T1> other)
        {
            return comparerT1.Compare(First, other.First);
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj is Tuple<T1>)
                return CompareTo((Tuple<T1>)obj);
            else
                throw new ArgumentException("Parameter is not a Tuple<T1>", "obj");
        }

        public override string ToString()
        {
            return string.Format("First: {0}", (First == null) ? "null" : First.ToString());
        }

        public static bool operator ==(Tuple<T1> lhs, Tuple<T1> rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Tuple<T1> lhs, Tuple<T1> rhs)
        {
            return !lhs.Equals(rhs);
        }
    }

    [Serializable]
    public class Tuple<T1, T2> : Tuple<T1>, IComparable, IComparable<Tuple<T1,T2>>
    {
        private static IComparer<T2> comparerT2 = Comparer<T2>.Default;
        private static IEqualityComparer<T2> eqalityComparerT2 = EqualityComparer<T2>.Default;
        public T2 Second;

        public Tuple() : base()
        {
            this.Second = default(T2);
        }

        public Tuple(T1 first, T2 second) : base(first)
        {
            this.Second = second;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Tuple<T1,T2>)
            {
                Tuple<T1,T2> other = (Tuple<T1,T2>)obj;
                return Equals(other);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public bool Equals(Tuple<T1,T2> other)
        {
            return base.Equals((Tuple<T1>)other) && eqalityComparerT2.Equals(Second, other.Second);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ (Second == null ? 0x00 : Second.GetHashCode());
        }

        public int CompareTo(Tuple<T1,T2> other)
        {
            int baseCompare = base.CompareTo(other);                           
            if (baseCompare != 0)
                return baseCompare;
            else
                return comparerT2.Compare(Second, other.Second);
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj is Tuple<T1,T2>)
                return CompareTo((Tuple<T1,T2>)obj);
            else
                throw new ArgumentException("Parameter is not a Tuple<T1,T2>", "obj");
        }

        public override string ToString()
        {
            return string.Format("{0}, Second: {1}", base.ToString(), (Second == null) ? "null" : Second.ToString());
        }

        public static bool operator ==(Tuple<T1,T2> lhs, Tuple<T1,T2> rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Tuple<T1,T2> lhs, Tuple<T1,T2> rhs)
        {
            return !lhs.Equals(rhs);
        }
    }

    [Serializable]
    public class Tuple<T1, T2, T3> : Tuple<T1, T2>, IComparable, IComparable<Tuple<T1, T2, T3>>
    {
        private static IComparer<T3> comparerT3 = Comparer<T3>.Default;
        private static IEqualityComparer<T3> eqalityComparerT3 = EqualityComparer<T3>.Default;
        public T3 Third;

        public Tuple() : base()
        {
            this.Third = default(T3);
        }

        public Tuple(T1 first, T2 second, T3 third)
            : base(first, second)
        {
            this.Third = third;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Tuple<T1, T2, T3>)
            {
                Tuple<T1, T2, T3> other = (Tuple<T1, T2, T3>)obj;
                return Equals(other);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public bool Equals(Tuple<T1, T2, T3> other)
        {
            return base.Equals((Tuple <T1, T2>)other) && eqalityComparerT3.Equals(Third, other.Third);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ (Third == null ? 0x00 : Third.GetHashCode());
        }

        public int CompareTo(Tuple<T1, T2, T3> other)
        {
            int baseCompare = base.CompareTo(other);
            if (baseCompare != 0)
                return baseCompare;
            else
                return comparerT3.Compare(Third, other.Third);
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj is Tuple<T1, T2, T3>)
                return CompareTo((Tuple<T1, T2, T3>)obj);
            else
                throw new ArgumentException("Parameter is not a Tuple<T1,T2,T3>", "obj");
        }

        public override string ToString()
        {
            return string.Format("{0}, Third: {1}", base.ToString(), (Third == null) ? "null" : Third.ToString());
        }

        public static bool operator ==(Tuple<T1, T2, T3> lhs, Tuple<T1, T2, T3> rhs)
        {
            if (lhs == null)
                return rhs == null;
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Tuple<T1, T2, T3> lhs, Tuple<T1, T2, T3> rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}
