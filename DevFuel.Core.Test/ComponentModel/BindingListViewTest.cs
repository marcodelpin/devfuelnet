using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using DevFuel.Core.ComponentModel;
using System.Diagnostics;

namespace DevFuel.Core.Test
{
    public interface IVehicle
    {
        int WheelCount {get;}
        string Name {get;}
        float TopSpeed { get;}
        bool CanFly { get; }
    }

    public class Bicycle : IVehicle
    {

        public Bicycle(string sName, bool bCanFly, float fTopSpeed)
        {
            m_Name = sName;
            m_CanFly = bCanFly;
            m_TopSpeed = fTopSpeed;
        }
        #region IVehicle Members
        public int WheelCount
        {
            get { return 2; }
        }

        private bool m_CanFly;
        public bool CanFly
        {
            get { return m_CanFly; }
            set { m_CanFly = value; }
        }

        private string m_Name;
        public string Name
        {
            get { return m_Name; }
        }

        private float m_TopSpeed;
        public float TopSpeed
        {
            get { return m_TopSpeed; }
        }
        #endregion

        public override string ToString()
        {
            return Name;
        }
    }

    public class Automobile : IVehicle
    {
        #region IVehicle Members
        public Automobile(string sName, bool bCanFly, float fTopSpeed)
        {
            m_Name = sName;
            m_CanFly = bCanFly;
            m_TopSpeed = fTopSpeed;
        }

        public int WheelCount
        {
            get { return 4; }
        }

        private bool m_CanFly;
        public bool CanFly
        {
            get { return m_CanFly; }
            set { m_CanFly = value; }
        }

        private string m_Name;
        public string Name
        {
            get { return m_Name; }
        }

        private float m_TopSpeed;
        public float TopSpeed
        {
            get { return m_TopSpeed; }
        }
        #endregion
        private int m_DoorCount;
        public int DoorCount
        {
            get { return m_DoorCount; }
            set { m_DoorCount = value; }
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Sedan : Automobile
    {
        public Sedan(string sName, bool bCanFly, float fTopSpeed, int iSeatCount) : base(sName, bCanFly, fTopSpeed)
        {
            m_SeatCount = iSeatCount;
        }

        private int m_SeatCount;
        public int SeatCount
        {
            get { return m_SeatCount; }
            set { m_SeatCount = value; }
        }
    }

    [TestFixture]
    public class BindingListViewTest
    {
        #region TestObjects
        private Bicycle m_TenSpeed;
        public Bicycle TenSpeed
        {
            get { return m_TenSpeed; }
            set { m_TenSpeed = value; }
        }

        private Bicycle m_Bmx;
        public Bicycle Bmx
        {
            get { return m_Bmx; }
            set { m_Bmx = value; }
        }

        private Bicycle m_MountainBike;
        public Bicycle MountainBike
        {
            get { return m_MountainBike; }
            set { m_MountainBike = value; }
        }

        private Automobile m_MiniCooper;
        public Automobile MiniCooper
        {
            get { return m_MiniCooper; }
            set { m_MiniCooper = value; }
        }

        private Automobile m_Tank;
        public Automobile Tank
        {
            get { return m_Tank; }
            set { m_Tank = value; }
        }

        private Automobile m_SmartCar;
        public Automobile SmartCar
        {
            get { return m_SmartCar; }
            set { m_SmartCar = value; }
        }

        private Sedan m_LincolnTownCar;
        public Sedan LincolnTownCar
        {
            get { return m_LincolnTownCar; }
            set { m_LincolnTownCar = value; }
        }

        private Sedan m_BuickLaCrosse;
        public Sedan BuickLaCrosse
        {
            get { return m_BuickLaCrosse; }
            set { m_BuickLaCrosse = value; }
        }

        private Sedan m_MitsubishiGallant;
        public Sedan MitsubishiGallant
        {
            get { return m_MitsubishiGallant; }
            set { m_MitsubishiGallant = value; }
        }
        #endregion

        private List<IVehicle> m_VehicleList;
        public List<IVehicle> VehicleList
        {
            get { return m_VehicleList; }
            set { m_VehicleList = value; }
        }

        private BindingListView<IVehicle> m_VehicleBlv;
        public BindingListView<IVehicle> VehicleBlv
        {
            get { return m_VehicleBlv; }
            set { m_VehicleBlv = value; }
        }

        [SetUp]
        public void Setup()
        {
            this.VehicleList = new List<IVehicle>();
            VehicleList.Add(this.Bmx = new Bicycle("BMX", true, 5));
            VehicleList.Add(this.BuickLaCrosse = new Sedan("Buick LaCrosse", false, 88, 5));
            VehicleList.Add(this.LincolnTownCar = new Sedan("Lincoln Town Car", false, 88, 5));
            VehicleList.Add(this.MiniCooper = new Automobile("Mini Cooper", true, 192));
            VehicleList.Add(this.MitsubishiGallant = new Sedan("Mitsubishi Gallant", false, 88, 5));
            VehicleList.Add(this.MountainBike = new Bicycle("Mountain Bike", true, 7));
            VehicleList.Add(this.SmartCar = new Automobile("Smart Car", false, 2));
            VehicleList.Add(this.Tank = new Automobile("Tank", false, 1));
            VehicleList.Add(this.TenSpeed = new Bicycle("Ten Speed", false, 10));
            VehicleBlv = new BindingListView<IVehicle>(VehicleList);
        }

        [NUnit.Framework.Test(Description="Sort the Interface BLV by a string property with implied ASC order")]
        public void TestInterfaceBlv_StringSort_ImpliedASC()
        {
            VehicleBlv.Sort = "Name";
            DumpBlv(VehicleBlv, "Name");
        }

        private void DumpBlv<T>(BindingListView<T> blv, string sProp)
        {
            Console.WriteLine();
            Console.WriteLine(sProp);
            foreach (ObjectView<T> ov in blv)
            {
                object obj = ov[sProp];
                string s = "NULL";
                if (obj != null)
                    s = obj.ToString();
                Console.WriteLine(string.Format("{0}->", ov.Object.ToString(), s));
            }
        }

        [NUnit.Framework.Test(Description="Sort the Interface BLV by a string property with explicit ASC order")]
        public void TestInterfaceBlv_StringSort_ASC()
        {
            VehicleBlv.Sort = "Name ASC";
            DumpBlv(VehicleBlv, "Name");
        }

        [NUnit.Framework.Test(Description="Sort the Interface BLV by a string property with explicit DESC order")]
        public void TestInterfaceBlv_StringSort_DESC()
        {
            VehicleBlv.Sort = "Name DESC";
            DumpBlv(VehicleBlv, "Name");
        }

        [NUnit.Framework.Test(Description="Sort the Interface BLV by an int property with implied ASC order")]
        public void TestInterfaceBlv_IntSort_ImpliedASC()
        {
            VehicleBlv.Sort = "WheelCount";
            DumpBlv(VehicleBlv, "WheelCount");
        }

        [NUnit.Framework.Test(Description="Sort the Interface BLV by an int property with explicit ASC order")]
        public void TestInterfaceBlv_IntSort_ASC()
        {
            VehicleBlv.Sort = "WheelCount ASC";
            DumpBlv(VehicleBlv, "WheelCount");
        }

        [NUnit.Framework.Test(Description="Sort the Interface BLV by an in property with explicit DESC order")]
        public void TestInterfaceBlv_IntSort_DESC()
        {
            VehicleBlv.Sort = "WheelCount DESC";
            DumpBlv(VehicleBlv, "WheelCount");
        }

        [NUnit.Framework.Test(Description="Sort the Interface BLV by a property does not exist. Should fail.")]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInterfaceBlv_NoSuchMember()
        {
            VehicleBlv.Sort = "Noogie";
            DumpBlv(VehicleBlv, "Noogie");
        }

        [NUnit.Framework.Test(Description="Sort the Interface BLV by a property that is in an implementing class, but not in the interface. Should Fail.")]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInterfaceBlv_NoSuchMemberInInterface()
        {
            VehicleBlv.Sort = "SeatCount";
            DumpBlv(VehicleBlv, "SeatCount");
        }


    }
}
