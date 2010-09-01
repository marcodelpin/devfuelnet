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
using Microsoft.Win32.SafeHandles;
using System.Runtime.CompilerServices;
using System.Security;

namespace DevFuel.Core.Security
{
    public static class CredUI
    {
        private const int MAX_USER_NAME = 100;
        private const int MAX_PASSWORD = 100;
        private const int MAX_DOMAIN = 100;

        public enum ReturnCode
        {
            NO_ERROR = 0,
            ERROR_CANCELLED = 1223,
            ERROR_NO_SUCH_LOGON_SESSION = 1312,
            ERROR_NOT_FOUND = 1168,
            ERROR_INVALID_ACCOUNT_NAME = 1315,
            ERROR_INSUFFICIENT_BUFFER = 122,
            ERROR_INVALID_PARAMETER = 87,
            ERROR_INVALID_FLAGS = 1004,
        }

        [Flags]
        public enum Flags //CREDUI_FLAGS
        {
            INCORRECT_PASSWORD = 0x1,
            DO_NOT_PERSIST = 0x2,
            REQUEST_ADMINISTRATOR = 0x4,
            EXCLUDE_CERTIFICATES = 0x8,
            REQUIRE_CERTIFICATE = 0x10,
            SHOW_SAVE_CHECK_BOX = 0x40,
            ALWAYS_SHOW_UI = 0x80,
            REQUIRE_SMARTCARD = 0x100,
            PASSWORD_ONLY_OK = 0x200,
            VALIDATE_USERNAME = 0x400,
            COMPLETE_USERNAME = 0x800,
            PERSIST = 0x1000,
            SERVER_CREDENTIAL = 0x4000,
            EXPECT_CONFIRMATION = 0x20000,
            GENERIC_CREDENTIALS = 0x40000,
            USERNAME_TARGET_CREDENTIALS = 0x80000,
            KEEP_USERNAME = 0x100000,
        }

        private struct CREDUI_INFO
        {
            public int cbSize;
            public IntPtr hwndParent;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszMessageText;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszCaptionText;
            public IntPtr hbmBanner;
        }     

        [DllImport("credui.dll", EntryPoint = "CredUIConfirmCredentialsW", CharSet=CharSet.Unicode)]
        private static extern ReturnCode _CredUIConfirmCredentials(string targetName, [MarshalAs(UnmanagedType.Bool)] bool confirm);

        [DllImport("credui.dll", EntryPoint = "CredUIPromptForCredentialsW", CharSet = CharSet.Unicode)]
        private static extern ReturnCode _CredUIPromptForCredentials(ref CREDUI_INFO info,
                  string targetName,
                  IntPtr reserved1,
                  int iError,
                  StringBuilder userName,
                  int maxUserName,
                  StringBuilder password,
                  int maxPassword,
                  [MarshalAs(UnmanagedType.Bool)] ref bool pfSave,
                  Flags flags);

        public static ReturnCode ConfirmCredentials(string pszTargetName, bool bConfirm)
        {
            return _CredUIConfirmCredentials(pszTargetName, bConfirm);
        }

        public static ReturnCode PromptForCredentials(
                  IntPtr hWndParent,
                  string pszCaptionText,
                  string pszMessageText,
                  IntPtr hBitmap,
                  string pszTargetName,
                  int dwAuthError,
                  ref string pszUserName,
                  ref string pszPassword,
                  ref bool pfSave,
                  Flags dwFlags)
        {
            CREDUI_INFO info = new CREDUI_INFO();

            info.hbmBanner = hBitmap;
            info.hwndParent = hWndParent;
            info.pszCaptionText = pszCaptionText;
            info.pszMessageText = pszMessageText;
            StringBuilder user = new StringBuilder(0,MAX_USER_NAME);
            StringBuilder pwd = new StringBuilder(0,MAX_PASSWORD);
            info.cbSize = Marshal.SizeOf(info);

            ReturnCode result = _CredUIPromptForCredentials(
                          ref info,
                          pszTargetName,
                          IntPtr.Zero,
                          dwAuthError,
                          user,
                          MAX_USER_NAME,
                          pwd,
                          MAX_PASSWORD,
                          ref pfSave,
                          dwFlags);

            pszUserName = user.ToString();
            pszPassword = pwd.ToString();

            return result;
        }
    }
}
