/*
 * Copyright (C) 2013 @JamesMontemagno http://www.montemagno.com http://www.refractored.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * Converted from: http://lists.ximian.com/pipermail/monodroid/2011-August/005648.html and
 * http://stackoverflow.com/questions/6064510/how-to-get-ip-address-of-the-device/13007325#13007325
 */
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Android.Runtime;
using Java.Net;

namespace com.refractored.monodroidtoolkit
{
    public static class NetworkUtils
    {
        /// <summary>
        /// Returns MAC address of the given interface name.
        /// </summary>
        /// <param name="interfaceName">interfaceName eth0, wlan0 or NULL=use first interface </param>
        /// <returns>mac address or empty string</returns>
        public static string GetMacAddress(string interfaceName)
        {
            try
            {
                var interfaces = GetAllNetworkInterfaces();
                foreach (var intf in interfaces)
                {
                    if (!string.IsNullOrWhiteSpace(interfaceName))
                    {
                        if(!intf.NetworkInterface.Name.Equals(interfaceName, StringComparison.InvariantCultureIgnoreCase))
                            continue; 
                    }

                    var mac = intf.NetworkInterface.GetHardwareAddress();
                    if (mac == null)
                        return string.Empty;

                    var buf = new StringBuilder();
                    for (int idx = 0; idx < mac.Length; idx++)
                    {
                        buf.Append(mac[idx].ToString("x2"));
                        if (idx != mac.Length - 1)
                            buf.Append(":");
                    }

                    return buf.ToString();
                }
            }
            catch (Exception)
            {
            }

            return string.Empty;
        }

        /// <summary>
        /// Get IP address from first non-localhost interface
        /// </summary>
        /// <param name="useIPv4">true=return ipv4, false=return ipv6</param>
        /// <returns>address or empty string</returns>
        public static String GetIPAddress(bool useIPv4 = true)
        {
            try
            {
                var interfaces = GetAllNetworkInterfaces();
                foreach (var intf in interfaces)
                {
                    for (int index = 0; index < intf.InitAddresses.Count; index++)
                    {
                        var addr = intf.InitAddresses[index];
                        var ipAddr = intf.IpAddresses[index];
                        if (!intf.IsLoopback)
                        {
                            String sAddr = addr.HostAddress.ToUpper();
                            bool isIPv4 = ipAddr.AddressFamily == AddressFamily.InterNetwork;
                            if (useIPv4)
                            {
                                if (isIPv4)
                                    return sAddr;
                            }
                            else
                            {
                                if (!isIPv4)
                                {
                                    int delim = sAddr.IndexOf('%'); // drop ip6 port suffix
                                    return delim < 0 ? sAddr : sAddr.Substring(0, delim);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                
            }

            return string.Empty;
        }

        /// <summary>
        /// Get system's hostname
        /// Can NOT be called from UI thread
        /// </summary>
        /// <returns>hostname or empty string</returns>
        public static string GetHostName()
        {
            try
            {
                var interfaces = GetAllNetworkInterfaces();
                foreach (var intf in interfaces)
                {
                    for (int index = 0; index < intf.InitAddresses.Count; index++)
                    {
                        var addr = intf.InitAddresses[index];
                        var ipAddr = intf.IpAddresses[index];
                       // var hostName = intf.HostNames[index];
                        
                        if (!addr.IsLoopbackAddress)
                        {
                            bool isIPv4 = ipAddr.AddressFamily == AddressFamily.InterNetwork;
                            if (isIPv4)
                            {
                                var hostName = addr.HostName.ToUpper();
                                if (string.IsNullOrWhiteSpace(hostName))
                                    return string.Empty;

                                return hostName;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                
            }
            return string.Empty;
        }


        public class NetworkInfoModel
        {
            public NetworkInterface NetworkInterface { get; set; }
            public List<IPAddress> IpAddresses { get; set; }
            public List<InetAddress> InitAddresses { get; set; }
            //public List<String> HostNames { get; set; }
            public bool IsLoopback { get; set; }
        }

        public static List<NetworkInfoModel> GetAllNetworkInterfaces()
        {


            // NetworkInfoModel is a class that we define.  You'll needto define something similar.

            var adaptors = new List<NetworkInfoModel>();
 
            try
            {

                var networkInterfaceClass = JNIEnv.FindClass("java/net/NetworkInterface");
                var inetAddressClass = JNIEnv.FindClass("java/net/InetAddress");
                var getNetworkInterfacesMethod = JNIEnv.GetStaticMethodID(networkInterfaceClass, "getNetworkInterfaces", "()Ljava/util/Enumeration;");
                var networkInterfacesEnumeration = JNIEnv.CallStaticObjectMethod(networkInterfaceClass, getNetworkInterfacesMethod);
                var enumerationClass = JNIEnv.FindClass("java/util/Enumeration");
                var hasMoreElementsMethod = JNIEnv.GetMethodID(enumerationClass, "hasMoreElements", "()Z");
                var nextElementMethod = JNIEnv.GetMethodID(enumerationClass, "nextElement", "()Ljava/lang/Object;");

                
                var hasMoreInterfaces = JNIEnv.CallBooleanMethod(networkInterfacesEnumeration, hasMoreElementsMethod);

                int moreInterfacesCount = 0;

                while (hasMoreInterfaces)
                {
                    moreInterfacesCount++;
                    if (moreInterfacesCount > 100)//hack for infinit loop
                        break;

                    var currentInterface = JNIEnv.CallObjectMethod(networkInterfacesEnumeration, nextElementMethod);
                    hasMoreInterfaces = JNIEnv.CallBooleanMethod(networkInterfacesEnumeration, hasMoreElementsMethod);

                    var adapter = new Java.Lang.Object(currentInterface, JniHandleOwnership.DoNotTransfer).JavaCast<NetworkInterface>();
                    var getInetAddressesMethod = JNIEnv.GetMethodID(networkInterfaceClass, "getInetAddresses", "()Ljava/util/Enumeration;");
                    var inetAddressesEnumeration = JNIEnv.CallObjectMethod(currentInterface, getInetAddressesMethod);
                    var hasMoreInetAddresses = JNIEnv.CallBooleanMethod(inetAddressesEnumeration, hasMoreElementsMethod);



                    // the following methods require android 2.3 
                    var isLoopbackMethod = Android.Runtime.JNIEnv.GetMethodID(networkInterfaceClass, "isLoopback", "()Z");
                    var isLoopback = Android.Runtime.JNIEnv.CallBooleanMethod(currentInterface, isLoopbackMethod);

                    

                    //IntPtr isUpMethod =Android.Runtime.JNIEnv.GetMethodID(networkInterfaceClass, "isUp","()Z");
                    //bool isUp =Android.Runtime.JNIEnv.CallBooleanMethod(currentInterface, isUpMethod);

                    var networkInfo = new NetworkInfoModel
                        {
                            NetworkInterface = adapter,
                            InitAddresses = new List<InetAddress>(),
                            IpAddresses = new List<IPAddress>(),
                           // HostNames =  new List<string>(),
                            IsLoopback = isLoopback
                        };

                    int moreInetCount = 0;
                    while (hasMoreInetAddresses)
                    {
                        moreInetCount++;
                        if (moreInetCount > 100) //hack for infinit loop
                            break;

                        var currentInetAddress = JNIEnv.CallObjectMethod(inetAddressesEnumeration, nextElementMethod);
                        hasMoreInetAddresses = JNIEnv.CallBooleanMethod(inetAddressesEnumeration, hasMoreElementsMethod);
                        var address = new Java.Lang.Object(currentInetAddress, JniHandleOwnership.DoNotTransfer).JavaCast<InetAddress>();

                        //var getHostNameMethod = Android.Runtime.JNIEnv.GetMethodID(inetAddressClass, "getHostName", "()Ljava/lang/String;");
                        //IntPtr getHostResultPtr = JNIEnv.CallObjectMethod(currentInetAddress, getHostNameMethod);

                        //var getHostResult = new Java.Lang.Object(getHostResultPtr, JniHandleOwnership.TransferLocalRef).JavaCast<Java.Lang.String>();

                        IPAddress ipAddr;

                        var success = IPAddress.TryParse(address.HostAddress, out ipAddr);
                        if (!success)
                            continue;

                        networkInfo.InitAddresses.Add(address);
                        networkInfo.IpAddresses.Add(ipAddr);
                        //networkInfo.HostNames.Add(getHostResult.ToString());
                        adaptors.Add(networkInfo);
                    }
                }
            }
            catch (Exception ex)
            {
            }

 

            return adaptors;

        }
    }
}