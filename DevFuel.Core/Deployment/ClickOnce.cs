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

using System.Collections.Generic;
using System.Text;
using System.Deployment.Application;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Reflection;
using System.Text.RegularExpressions;
using System;

//Based on post from http://forums.microsoft.com/MSDN/ShowPost.aspx?PostID=99515&SiteID=1

namespace DevFuel.Core.Deployment
{
    /// <summary>
    /// Support class to deal with certain ClickOnce Installer situations
    /// </summary>
    public class ClickOnce
    {
        /// <summary>
        /// Gets the published version.
        /// </summary>
        /// <value>The published version.</value>
        public static Version PublishedVersion
        {
            get
            {
                Version version = Assembly.GetEntryAssembly().GetName().Version;
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                    version = ad.CurrentVersion;
                }
                return version;
            }
        }

        /// <summary>
        /// Registers the type of the file.
        /// </summary>
        /// <param name="FileType">Type of the file.</param>
        public static void RegisterFileType(String FileType)
        {
            if (ApplicationDeployment.IsNetworkDeployed && ApplicationDeployment.CurrentDeployment.IsFirstRun)
            {
                RegistryKey pathToApp = Registry.ClassesRoot.CreateSubKey(ApplicationKey);
                pathToApp.CreateSubKey("DefaultIcon").SetValue(String.Empty, Application.ExecutablePath);
                pathToApp.CreateSubKey("shell").CreateSubKey("open").CreateSubKey("command").SetValue(String.Empty, "explorer \"" + PathToApplication + "?LoadFile=%1\"");

                Registry.ClassesRoot.CreateSubKey(FileType).SetValue(String.Empty, ApplicationKey);
            }

        }

        /// <summary>
        /// Gets the path to application.
        /// </summary>
        /// <value>The path to application.</value>
        public static String PathToApplication
        {
            get
            {
                string startUpURL = String.Empty;
                if (ApplicationDeployment.IsNetworkDeployed && ApplicationDeployment.CurrentDeployment.UpdateLocation != null)
                {
                    startUpURL = ApplicationDeployment.CurrentDeployment.UpdateLocation.AbsoluteUri;

                    if (ApplicationDeployment.CurrentDeployment.UpdateLocation.Query != null && ApplicationDeployment.CurrentDeployment.UpdateLocation.Query.Length > 0)
                    {
                        startUpURL = startUpURL.Replace(ApplicationDeployment.CurrentDeployment.UpdateLocation.Query, String.Empty);
                    }
                }

                return Uri.UnescapeDataString(startUpURL);
            }
        }
        /// <summary>
        /// Gets the application key.
        /// </summary>
        /// <value>The application key.</value>
        public static String ApplicationKey
        {
            get
            {
                return Regex.Replace(Application.ProductName, "[^A-z0-9]", "");
            }
        }
        /// <summary>
        /// Makes the application autostart.
        /// </summary>
        public static void MakeApplicationAutostart()
        {
            RegistryKey StartUp = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\RunOnce");
            // This Ensures that the only Latest version will autostart
            if (ApplicationDeployment.IsNetworkDeployed && ApplicationDeployment.CurrentDeployment.UpdateLocation != null)
            {
                StartUp.SetValue(ApplicationKey, "explorer \"" + PathToApplication + "?autostart\"");
            }
        }

    }
}
