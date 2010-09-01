using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DevFuel.Core.Collections.Generic;

namespace DevFuel.Core.Test.Collections
{
    [TestFixture]
    public class TupleTests
    {
        [Test]
        public void ConstructorsAndElements()
        {
            Tuple<int> tInt = new Tuple<int>();
            Assert.AreEqual(0, tInt.First);
            tInt = new Tuple<int>(123);
            Assert.AreEqual(123, tInt.First);

            Tuple<int, string> tIntStr = new Tuple<int, string>();
            Assert.AreEqual(0, tIntStr.First);
            Assert.AreEqual(null, tIntStr.Second);
            tIntStr = new Tuple<int, string>(123, "string");
            Assert.AreEqual(123, tIntStr.First);
            Assert.AreEqual("string", tIntStr.Second);

            Tuple<int, string, double> tIntStrDbl = new Tuple<int, string, double>();
            Assert.AreEqual(0, tIntStrDbl.First);
            Assert.AreEqual(null, tIntStrDbl.Second);
            Assert.AreEqual(0.0, tIntStrDbl.Third);
            tIntStrDbl = new Tuple<int, string, double>(123, "string", 3.14);
            Assert.AreEqual(123, tIntStrDbl.First);
            Assert.AreEqual("string", tIntStrDbl.Second);
            Assert.AreEqual(3.14, tIntStrDbl.Third);
        }

        [Test]
        public void Equals()
        {
            bool b;

            Tuple<int> tInt_42 = new Tuple<int>(42);
            Tuple<int> tInt_53 = new Tuple<int>(53);
            Tuple<int> tInt_42too = new Tuple<int>(42);
            Tuple<string> tStr1 = new Tuple<string>(new string('z', 3));
            Tuple<string> tStr2 = new Tuple<string>(new string('y', 3));
            Tuple<string> tStr3 = new Tuple<string>(new string('z', 3));
            Tuple<string> tStr4 = new Tuple<string>(null);


            b = tInt_42.Equals(tInt_53); Assert.IsFalse(b);
            b = tInt_42.Equals(tInt_42too); Assert.IsTrue(b);

            b = tInt_42 == tInt_53; Assert.IsFalse(b);
            b = tInt_42 == tInt_42too; Assert.IsTrue(b);

            b = tInt_42 != tInt_53; Assert.IsTrue(b);
            b = tInt_42 != tInt_42too; Assert.IsFalse(b);

            b = tStr1.Equals(tStr2); Assert.IsFalse(b);
            b = tStr1.Equals(tStr3); Assert.IsTrue(b);
            b = tStr1.Equals(tStr4); Assert.IsFalse(b);

            b = tStr1 == tStr2; Assert.IsFalse(b);
            b = tStr1 == tStr3; Assert.IsTrue(b);
            b = tStr1 == tStr4; Assert.IsFalse(b);

            b = tStr1 != tStr2; Assert.IsTrue(b);
            b = tStr1 != tStr3; Assert.IsFalse(b);
            b = tStr1 != tStr4; Assert.IsTrue(b);

            Tuple<int, string> tIntStr_42_zzz = new Tuple<int, string>(42, new string('z', 3));
            Tuple<int, string> tIntStr_53_zzz = new Tuple<int, string>(53, new string('z', 3));
            Tuple<int, string> tIntStr_42_zzzz = new Tuple<int, string>(42, new string('z', 4));
            Tuple<int, string> tIntStr_42_zzz_too = new Tuple<int, string>(42, new string('z', 3));
            Tuple<int, string> tIntStr_122_yyy = new Tuple<int, string>(122, new string('y', 3));
            Tuple<int, string> tIntStr_122_null = new Tuple<int, string>(122, null);
            Tuple<int, string> tIntStr_122_null_too = new Tuple<int, string>(122, null);


            b = tIntStr_42_zzz.Equals(tIntStr_53_zzz); Assert.IsFalse(b);
            b = tIntStr_42_zzz.Equals(tIntStr_42_zzzz); Assert.IsFalse(b);
            b = tIntStr_42_zzz.Equals(tIntStr_42_zzz_too); Assert.IsTrue(b);
            b = tIntStr_42_zzz.Equals(tIntStr_122_yyy); Assert.IsFalse(b);
            b = tIntStr_42_zzz.Equals("hi"); Assert.IsFalse(b);
            b = tIntStr_122_null.Equals(tIntStr_122_null_too); Assert.IsTrue(b);
            b = tIntStr_42_zzz == tIntStr_53_zzz; Assert.IsFalse(b);
            b = tIntStr_42_zzz == tIntStr_42_zzzz; Assert.IsFalse(b);
            b = tIntStr_42_zzz == tIntStr_42_zzz_too; Assert.IsTrue(b);
            b = tIntStr_42_zzz == tIntStr_122_yyy; Assert.IsFalse(b);
            b = tIntStr_122_null == tIntStr_122_null_too; Assert.IsTrue(b);
            b = tIntStr_42_zzz != tIntStr_53_zzz; Assert.IsTrue(b);
            b = tIntStr_42_zzz != tIntStr_42_zzzz; Assert.IsTrue(b);
            b = tIntStr_42_zzz != tIntStr_42_zzz_too; Assert.IsFalse(b);
            b = tIntStr_42_zzz != tIntStr_122_yyy; Assert.IsTrue(b);
            b = tIntStr_122_null != tIntStr_122_null_too; Assert.IsFalse(b);


            Tuple<int, string, double> tIntStrDbl_42_zzz_1 = new Tuple<int, string, double>(42, new string('z', 3), 1.0);
            Tuple<int, string, double> tIntStrDbl_53_zzz_1 = new Tuple<int, string, double>(53, new string('z', 3), 1.0);
            Tuple<int, string, double> tIntStrDbl_42_zzzz_1 = new Tuple<int, string, double>(42, new string('z', 4), 1.0);
            Tuple<int, string, double> tIntStrDbl_42_zzz_2 = new Tuple<int, string, double>(42, new string('z', 4), 2.0);
            Tuple<int, string, double> tIntStrDbl_42_zzz_1_too = new Tuple<int, string, double>(42, new string('z', 3), 1.0);

            b = tIntStrDbl_42_zzz_1.Equals(tIntStrDbl_53_zzz_1); Assert.IsFalse(b);
            b = tIntStrDbl_42_zzz_1.Equals(tIntStrDbl_42_zzzz_1); Assert.IsFalse(b);
            b = tIntStrDbl_42_zzz_1.Equals(tIntStrDbl_42_zzz_2); Assert.IsFalse(b);
            b = tIntStrDbl_42_zzz_1.Equals(tIntStrDbl_42_zzz_1_too); Assert.IsTrue(b);
        }

        [Test]
        public void HashCode()
        {
            new Tuple<int>(123).GetHashCode();
            new Tuple<int, string>(123, "string").GetHashCode();
            new Tuple<int, string, double>(123, "string", 3.14).GetHashCode();
            new Tuple<Version, string, Version>(new Version(), null, null).GetHashCode();
        }

        [Test]
        public void ToStringTest()
        {
            Console.WriteLine(new Tuple<int>(123).ToString());
            Console.WriteLine(new Tuple<int, string>(123, "string").ToString());
            Console.WriteLine(new Tuple<int, string, double>(123, "string", 3.14).ToString());
            Console.WriteLine(new Tuple<Version, string, Version>(new Version(), null, null).ToString());
        }
    }
}
