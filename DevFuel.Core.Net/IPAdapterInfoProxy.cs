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
using System.Runtime.InteropServices;

namespace DevFuel.Core.Net
{
    public class IPAdapterInfoProxy : IDisposable
    {
        public class IPMaskedAddress
        {
            public IPMaskedAddress(string address, string mask)
            {
                m_Address = address;
                m_Mask = mask;
            }
            private string m_Address;
            public string Address
            {
                get { return m_Address; }
                set { m_Address = value; }
            }

            private string m_Mask;
            public string Mask
            {
                get { return m_Mask; }
                set { m_Mask = value; }
            }

        }
        public class IPAdapterInfo
        {
            public enum InterfaceType : uint
            {
                Other = 1,
                Ethernet = 6,
                TokenRing = 9,
                FDDI = 15,
                PPP = 23,
                Loopback = 24,
                SLIP = 28
            }

            private IP_ADAPTER_INFO m_Info;
            internal IP_ADAPTER_INFO Info
            {
                get { return m_Info; }
                set { m_Info = value; }
            }

            public int Index
            {
                get { return m_Info.Index; }
            }
            public InterfaceType Type
            {
                get { return (InterfaceType)m_Info.Type; }
            }

            public string AdapterName
            {
                get { return m_Info.AdapterName; }
            }

            public string AdapterDescription
            {
                get { return m_Info.AdapterDescription; }
            }

            public bool DhcpEnabled
            {
                get { return m_Info.DhcpEnabled == 1; }
            }

            private List<IPMaskedAddress> m_DhcpServer = new List<IPMaskedAddress>();
            public IEnumerable<IPMaskedAddress> DhcpServer
            {
                get { return m_DhcpServer; }
            }

            private List<IPMaskedAddress> m_IPAddressList = new List<IPMaskedAddress>();
            public IEnumerable<IPMaskedAddress> IPAddressList
            {
                get { return m_IPAddressList; }
            }

            private List<IPMaskedAddress> m_GatewayList = new List<IPMaskedAddress>();
            public IEnumerable<IPMaskedAddress> GatewayList
            {
                get { return m_GatewayList; }
            }
            
            public DateTime LeaseObtained
            {
                get 
                {
                    return new DateTime(1970, 1, 1).AddSeconds(m_Info.LeaseObtained).ToLocalTime();
                }
            }

            public byte[] MacAddressBytes
            {
                get
                {
                    return m_Info.Address;
                }
            }

            public string MacAddress
            {
                get
                {
                    string address = string.Empty;
                    for (int i = 0; i < m_Info.AddressLength - 1; i++)
                    {
                        address += string.Format("{0:X2}-", m_Info.Address[i]);
                    }
                    address += string.Format("{0:X2}-", m_Info.Address[m_Info.AddressLength - 1]);
                    return address;
                }
            }

            public bool HaveWins
            {
                get { return m_Info.HaveWins; }
            }

            private List<IPMaskedAddress> m_PrimaryWinsServer = new List<IPMaskedAddress>();
            public IEnumerable<IPMaskedAddress> PrimaryWinsServer
            {
                get { return m_IPAddressList; }
            }

            private List<IPMaskedAddress> m_SecondaryWinsServer = new List<IPMaskedAddress>();
            public IEnumerable<IPMaskedAddress> SecondaryWinsServer
            {
                get { return m_SecondaryWinsServer; }
            }
        }    

        #region PINVOKE defines
        const int MAX_ADAPTER_DESCRIPTION_LENGTH = 128;
        const int ERROR_BUFFER_OVERFLOW = 111;
        const int MAX_ADAPTER_NAME_LENGTH = 256;
        const int MAX_ADAPTER_ADDRESS_LENGTH = 8;


        [DllImport("iphlpapi.dll", CharSet = CharSet.Ansi)]
        protected static extern int GetAdaptersInfo(IntPtr pAdapterInfo, ref Int64 pBufOutLen);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        internal struct IP_ADDRESS_STRING
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string Address;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        internal struct IP_ADDR_STRING
        {
            public IntPtr Next;
            public IP_ADDRESS_STRING IpAddress;
            public IP_ADDRESS_STRING IpMask;
            public Int32 Context;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        internal struct IP_ADAPTER_INFO
        {
            public IntPtr Next;
            public Int32 ComboIndex;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_ADAPTER_NAME_LENGTH + 4)]
            public string AdapterName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_ADAPTER_DESCRIPTION_LENGTH + 4)]
            public string AdapterDescription;
            public UInt32 AddressLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_ADAPTER_ADDRESS_LENGTH)]
            public byte[] Address;
            public Int32 Index;
            public UInt32 Type;
            public UInt32 DhcpEnabled;
            public IntPtr CurrentIpAddress;
            public IP_ADDR_STRING IpAddressList;
            public IP_ADDR_STRING GatewayList;
            public IP_ADDR_STRING DhcpServer;
            public bool HaveWins;
            public IP_ADDR_STRING PrimaryWinsServer;
            public IP_ADDR_STRING SecondaryWinsServer;
            public Int32 LeaseObtained;
            public Int32 LeaseExpires;
        }
        #endregion

        IntPtr m_InfoListPtr = IntPtr.Zero;

        public IPAdapterInfoProxy()
        {
            long structSize = Marshal.SizeOf(typeof(IP_ADAPTER_INFO));
            m_InfoListPtr = Marshal.AllocHGlobal(new IntPtr(structSize));
            int ret = GetAdaptersInfo(m_InfoListPtr, ref structSize);

            if (ret == ERROR_BUFFER_OVERFLOW) // ERROR_BUFFER_OVERFLOW == 111
            {
                // Buffer was too small, reallocate the correct size for the buffer.
                m_InfoListPtr = Marshal.ReAllocHGlobal(m_InfoListPtr, new IntPtr(structSize));

                ret = GetAdaptersInfo(m_InfoListPtr, ref structSize);
            } // if

            if (ret != 0)
            {
                this.Dispose();
            }

        }


        public IEnumerable<IPAdapterInfo> Adapters
        {
            get
            {
                if (m_InfoListPtr == IntPtr.Zero)
                    yield break;
   
                IntPtr pEntry = m_InfoListPtr;

                do
                {
                    // Retrieve the adapter info from the memory address
                    IP_ADAPTER_INFO entry = (IP_ADAPTER_INFO)Marshal.PtrToStructure(pEntry, typeof(IP_ADAPTER_INFO));

                    IPAdapterInfo info = new IPAdapterInfo();
                    info.Info = entry;

                    yield return info;

                    // Get next adapter (if any)
                    pEntry = entry.Next;

                }
                while (pEntry != IntPtr.Zero);
           }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (m_InfoListPtr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(m_InfoListPtr);
                m_InfoListPtr = IntPtr.Zero;
            }
        }

        #endregion
    }
}
