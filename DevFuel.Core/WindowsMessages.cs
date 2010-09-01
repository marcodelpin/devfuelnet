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

#region COPYRIGHT
/*
The following code originated from:
http://www.pinvoke.net/default.aspx/Enums/WindowsMessages.html
And was released under this license (terms of use):
http://www.pinvoke.net/termsofuse.htm
 
It has been adapted and integrated
by E. Edwards of http://www.devfuel.com. 

It has been re-released
Copyright (c) 2007-2008, E. Edwards 
under the new BSD License:

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

namespace DevFuel.Core
{
    /// <summary>
    /// An Enumeration of Standard Windows Message Codes
    /// </summary>
    public enum WindowsMessages
    {
        /// <summary>
        /// 
        /// </summary>
        WM_ACTIVATE = 0x6,
        /// <summary>
        /// 
        /// </summary>
        WM_ACTIVATEAPP = 0x1C,
        /// <summary>
        /// 
        /// </summary>
        WM_AFXFIRST = 0x360,
        /// <summary>
        /// 
        /// </summary>
        WM_AFXLAST = 0x37F,
        /// <summary>
        /// 
        /// </summary>
        WM_APP = 0x8000,
        /// <summary>
        /// 
        /// </summary>
        WM_ASKCBFORMATNAME = 0x30C,
        /// <summary>
        /// 
        /// </summary>
        WM_CANCELJOURNAL = 0x4B,
        /// <summary>
        /// 
        /// </summary>
        WM_CANCELMODE = 0x1F,
        /// <summary>
        /// 
        /// </summary>
        WM_CAPTURECHANGED = 0x215,
        /// <summary>
        /// 
        /// </summary>
        WM_CHANGECBCHAIN = 0x30D,
        /// <summary>
        /// 
        /// </summary>
        WM_CHAR = 0x102,
        /// <summary>
        /// 
        /// </summary>
        WM_CHARTOITEM = 0x2F,
        /// <summary>
        /// 
        /// </summary>
        WM_CHILDACTIVATE = 0x22,
        /// <summary>
        /// 
        /// </summary>
        WM_CLEAR = 0x303,
        /// <summary>
        /// 
        /// </summary>
        WM_CLOSE = 0x10,
        /// <summary>
        /// 
        /// </summary>
        WM_COMMAND = 0x111,
        /// <summary>
        /// 
        /// </summary>
        WM_COMPACTING = 0x41,
        /// <summary>
        /// 
        /// </summary>
        WM_COMPAREITEM = 0x39,
        /// <summary>
        /// 
        /// </summary>
        WM_CONTEXTMENU = 0x7B,
        /// <summary>
        /// 
        /// </summary>
        WM_COPY = 0x301,
        /// <summary>
        /// 
        /// </summary>
        WM_COPYDATA = 0x4A,
        /// <summary>
        /// 
        /// </summary>
        WM_CREATE = 0x1,
        /// <summary>
        /// 
        /// </summary>
        WM_CTLCOLORBTN = 0x135,
        /// <summary>
        /// 
        /// </summary>
        WM_CTLCOLORDLG = 0x136,
        /// <summary>
        /// 
        /// </summary>
        WM_CTLCOLOREDIT = 0x133,
        /// <summary>
        /// 
        /// </summary>
        WM_CTLCOLORLISTBOX = 0x134,
        /// <summary>
        /// 
        /// </summary>
        WM_CTLCOLORMSGBOX = 0x132,
        /// <summary>
        /// 
        /// </summary>
        WM_CTLCOLORSCROLLBAR = 0x137,
        /// <summary>
        /// 
        /// </summary>
        WM_CTLCOLORSTATIC = 0x138,
        /// <summary>
        /// 
        /// </summary>
        WM_CUT = 0x300,
        /// <summary>
        /// 
        /// </summary>
        WM_DEADCHAR = 0x103,
        /// <summary>
        /// 
        /// </summary>
        WM_DELETEITEM = 0x2D,
        /// <summary>
        /// 
        /// </summary>
        WM_DESTROY = 0x2,
        /// <summary>
        /// 
        /// </summary>
        WM_DESTROYCLIPBOARD = 0x307,
        /// <summary>
        /// 
        /// </summary>
        WM_DEVICECHANGE = 0x219,
        /// <summary>
        /// 
        /// </summary>
        WM_DEVMODECHANGE = 0x1B,
        /// <summary>
        /// 
        /// </summary>
        WM_DISPLAYCHANGE = 0x7E,
        /// <summary>
        /// 
        /// </summary>
        WM_DRAWCLIPBOARD = 0x308,
        /// <summary>
        /// 
        /// </summary>
        WM_DRAWITEM = 0x2B,
        /// <summary>
        /// 
        /// </summary>
        WM_DROPFILES = 0x233,
        /// <summary>
        /// 
        /// </summary>
        WM_ENABLE = 0xA,
        /// <summary>
        /// 
        /// </summary>
        WM_ENDSESSION = 0x16,
        /// <summary>
        /// 
        /// </summary>
        WM_ENTERIDLE = 0x121,
        /// <summary>
        /// 
        /// </summary>
        WM_ENTERMENULOOP = 0x211,
        /// <summary>
        /// 
        /// </summary>
        WM_ENTERSIZEMOVE = 0x231,
        /// <summary>
        /// 
        /// </summary>
        WM_ERASEBKGND = 0x14,
        /// <summary>
        /// 
        /// </summary>
        WM_EXITMENULOOP = 0x212,
        /// <summary>
        /// 
        /// </summary>
        WM_EXITSIZEMOVE = 0x232,
        /// <summary>
        /// 
        /// </summary>
        WM_FONTCHANGE = 0x1D,
        /// <summary>
        /// 
        /// </summary>
        WM_GETDLGCODE = 0x87,
        /// <summary>
        /// 
        /// </summary>
        WM_GETFONT = 0x31,
        /// <summary>
        /// 
        /// </summary>
        WM_GETHOTKEY = 0x33,
        /// <summary>
        /// 
        /// </summary>
        WM_GETICON = 0x7F,
        /// <summary>
        /// 
        /// </summary>
        WM_GETMINMAXINFO = 0x24,
        /// <summary>
        /// 
        /// </summary>
        WM_GETOBJECT = 0x3D,
        /// <summary>
        /// 
        /// </summary>
        WM_GETSYSMENU = 0x313,
        /// <summary>
        /// 
        /// </summary>
        WM_GETTEXT = 0xD,
        /// <summary>
        /// 
        /// </summary>
        WM_GETTEXTLENGTH = 0xE,
        /// <summary>
        /// 
        /// </summary>
        WM_HANDHELDFIRST = 0x358,
        /// <summary>
        /// 
        /// </summary>
        WM_HANDHELDLAST = 0x35F,
        /// <summary>
        /// 
        /// </summary>
        WM_HELP = 0x53,
        /// <summary>
        /// 
        /// </summary>
        WM_HOTKEY = 0x312,
        /// <summary>
        /// 
        /// </summary>
        WM_HSCROLL = 0x114,
        /// <summary>
        /// 
        /// </summary>
        WM_HSCROLLCLIPBOARD = 0x30E,
        /// <summary>
        /// 
        /// </summary>
        WM_ICONERASEBKGND = 0x27,
        /// <summary>
        /// 
        /// </summary>
        WM_IME_CHAR = 0x286,
        /// <summary>
        /// 
        /// </summary>
        WM_IME_COMPOSITION = 0x10F,
        /// <summary>
        /// 
        /// </summary>
        WM_IME_COMPOSITIONFULL = 0x284,
        /// <summary>
        /// 
        /// </summary>
        WM_IME_CONTROL = 0x283,
        /// <summary>
        /// 
        /// </summary>
        WM_IME_ENDCOMPOSITION = 0x10E,
        /// <summary>
        /// 
        /// </summary>
        WM_IME_KEYDOWN = 0x290,
        /// <summary>
        /// 
        /// </summary>
        WM_IME_KEYLAST = 0x10F,
        /// <summary>
        /// 
        /// </summary>
        WM_IME_KEYUP = 0x291,
        /// <summary>
        /// 
        /// </summary>
        WM_IME_NOTIFY = 0x282,
        /// <summary>
        /// 
        /// </summary>
        WM_IME_REQUEST = 0x288,
        /// <summary>
        /// 
        /// </summary>
        WM_IME_SELECT = 0x285,
        /// <summary>
        /// 
        /// </summary>
        WM_IME_SETCONTEXT = 0x281,
        /// <summary>
        /// 
        /// </summary>
        WM_IME_STARTCOMPOSITION = 0x10D,
        /// <summary>
        /// 
        /// </summary>
        WM_INITDIALOG = 0x110,
        /// <summary>
        /// 
        /// </summary>
        WM_INITMENU = 0x116,
        /// <summary>
        /// 
        /// </summary>
        WM_INITMENUPOPUP = 0x117,
        /// <summary>
        /// 
        /// </summary>
        WM_INPUTLANGCHANGE = 0x51,
        /// <summary>
        /// 
        /// </summary>
        WM_INPUTLANGCHANGEREQUEST = 0x50,
        /// <summary>
        /// 
        /// </summary>
        WM_KEYDOWN = 0x100,
        /// <summary>
        /// 
        /// </summary>
        WM_KEYFIRST = 0x100,
        /// <summary>
        /// 
        /// </summary>
        WM_KEYLAST = 0x108,
        /// <summary>
        /// 
        /// </summary>
        WM_KEYUP = 0x101,
        /// <summary>
        /// 
        /// </summary>
        WM_KILLFOCUS = 0x8,
        /// <summary>
        /// 
        /// </summary>
        WM_LBUTTONDBLCLK = 0x203,
        /// <summary>
        /// 
        /// </summary>
        WM_LBUTTONDOWN = 0x201,
        /// <summary>
        /// 
        /// </summary>
        WM_LBUTTONUP = 0x202,
        /// <summary>
        /// 
        /// </summary>
        WM_MBUTTONDBLCLK = 0x209,
        /// <summary>
        /// 
        /// </summary>
        WM_MBUTTONDOWN = 0x207,
        /// <summary>
        /// 
        /// </summary>
        WM_MBUTTONUP = 0x208,
        /// <summary>
        /// 
        /// </summary>
        WM_MDIACTIVATE = 0x222,
        /// <summary>
        /// 
        /// </summary>
        WM_MDICASCADE = 0x227,
        /// <summary>
        /// 
        /// </summary>
        WM_MDICREATE = 0x220,
        /// <summary>
        /// 
        /// </summary>
        WM_MDIDESTROY = 0x221,
        /// <summary>
        /// 
        /// </summary>
        WM_MDIGETACTIVE = 0x229,
        /// <summary>
        /// 
        /// </summary>
        WM_MDIICONARRANGE = 0x228,
        /// <summary>
        /// 
        /// </summary>
        WM_MDIMAXIMIZE = 0x225,
        /// <summary>
        /// 
        /// </summary>
        WM_MDINEXT = 0x224,
        /// <summary>
        /// 
        /// </summary>
        WM_MDIREFRESHMENU = 0x234,
        /// <summary>
        /// 
        /// </summary>
        WM_MDIRESTORE = 0x223,
        /// <summary>
        /// 
        /// </summary>
        WM_MDISETMENU = 0x230,
        /// <summary>
        /// 
        /// </summary>
        WM_MDITILE = 0x226,
        /// <summary>
        /// 
        /// </summary>
        WM_MEASUREITEM = 0x2C,
        /// <summary>
        /// 
        /// </summary>
        WM_MENUCHAR = 0x120,
        /// <summary>
        /// 
        /// </summary>
        WM_MENUCOMMAND = 0x126,
        /// <summary>
        /// 
        /// </summary>
        WM_MENUDRAG = 0x123,
        /// <summary>
        /// 
        /// </summary>
        WM_MENUGETOBJECT = 0x124,
        /// <summary>
        /// 
        /// </summary>
        WM_MENURBUTTONUP = 0x122,
        /// <summary>
        /// 
        /// </summary>
        WM_MENUSELECT = 0x11F,
        /// <summary>
        /// 
        /// </summary>
        WM_MOUSEACTIVATE = 0x21,
        /// <summary>
        /// 
        /// </summary>
        WM_MOUSEFIRST = 0x200,
        /// <summary>
        /// 
        /// </summary>
        WM_MOUSEHOVER = 0x2A1,
        /// <summary>
        /// 
        /// </summary>
        WM_MOUSELAST = 0x20A,
        /// <summary>
        /// 
        /// </summary>
        WM_MOUSELEAVE = 0x2A3,
        /// <summary>
        /// 
        /// </summary>
        WM_MOUSEMOVE = 0x200,
        /// <summary>
        /// 
        /// </summary>
        WM_MOUSEWHEEL = 0x20A,
        /// <summary>
        /// 
        /// </summary>
        WM_MOVE = 0x3,
        /// <summary>
        /// 
        /// </summary>
        WM_MOVING = 0x216,
        /// <summary>
        /// 
        /// </summary>
        WM_NCACTIVATE = 0x86,
        /// <summary>
        /// 
        /// </summary>
        WM_NCCALCSIZE = 0x83,
        /// <summary>
        /// 
        /// </summary>
        WM_NCCREATE = 0x81,
        /// <summary>
        /// 
        /// </summary>
        WM_NCDESTROY = 0x82,
        /// <summary>
        /// 
        /// </summary>
        WM_NCHITTEST = 0x84,
        /// <summary>
        /// 
        /// </summary>
        WM_NCLBUTTONDBLCLK = 0xA3,
        /// <summary>
        /// 
        /// </summary>
        WM_NCLBUTTONDOWN = 0xA1,
        /// <summary>
        /// 
        /// </summary>
        WM_NCLBUTTONUP = 0xA2,
        /// <summary>
        /// 
        /// </summary>
        WM_NCMBUTTONDBLCLK = 0xA9,
        /// <summary>
        /// 
        /// </summary>
        WM_NCMBUTTONDOWN = 0xA7,
        /// <summary>
        /// 
        /// </summary>
        WM_NCMBUTTONUP = 0xA8,
        /// <summary>
        /// 
        /// </summary>
        WM_NCMOUSEHOVER = 0x2A0,
        /// <summary>
        /// 
        /// </summary>
        WM_NCMOUSELEAVE = 0x2A2,
        /// <summary>
        /// 
        /// </summary>
        WM_NCMOUSEMOVE = 0xA0,
        /// <summary>
        /// 
        /// </summary>
        WM_NCPAINT = 0x85,
        /// <summary>
        /// 
        /// </summary>
        WM_NCRBUTTONDBLCLK = 0xA6,
        /// <summary>
        /// 
        /// </summary>
        WM_NCRBUTTONDOWN = 0xA4,
        /// <summary>
        /// 
        /// </summary>
        WM_NCRBUTTONUP = 0xA5,
        /// <summary>
        /// 
        /// </summary>
        WM_NEXTDLGCTL = 0x28,
        /// <summary>
        /// 
        /// </summary>
        WM_NEXTMENU = 0x213,
        /// <summary>
        /// 
        /// </summary>
        WM_NOTIFY = 0x4E,
        /// <summary>
        /// 
        /// </summary>
        WM_NOTIFYFORMAT = 0x55,
        /// <summary>
        /// 
        /// </summary>
        WM_NULL = 0x0,
        /// <summary>
        /// 
        /// </summary>
        WM_PAINT = 0xF,
        /// <summary>
        /// 
        /// </summary>
        WM_PAINTCLIPBOARD = 0x309,
        /// <summary>
        /// 
        /// </summary>
        WM_PAINTICON = 0x26,
        /// <summary>
        /// 
        /// </summary>
        WM_PALETTECHANGED = 0x311,
        /// <summary>
        /// 
        /// </summary>
        WM_PALETTEISCHANGING = 0x310,
        /// <summary>
        /// 
        /// </summary>
        WM_PARENTNOTIFY = 0x210,
        /// <summary>
        /// 
        /// </summary>
        WM_PASTE = 0x302,
        /// <summary>
        /// 
        /// </summary>
        WM_PENWINFIRST = 0x380,
        /// <summary>
        /// 
        /// </summary>
        WM_PENWINLAST = 0x38F,
        /// <summary>
        /// 
        /// </summary>
        WM_POWER = 0x48,
        /// <summary>
        /// 
        /// </summary>
        WM_PRINT = 0x317,
        /// <summary>
        /// 
        /// </summary>
        WM_PRINTCLIENT = 0x318,
        /// <summary>
        /// 
        /// </summary>
        WM_QUERYDRAGICON = 0x37,
        /// <summary>
        /// 
        /// </summary>
        WM_QUERYENDSESSION = 0x11,
        /// <summary>
        /// 
        /// </summary>
        WM_QUERYNEWPALETTE = 0x30F,
        /// <summary>
        /// 
        /// </summary>
        WM_QUERYOPEN = 0x13,
        /// <summary>
        /// 
        /// </summary>
        WM_QUERYUISTATE = 0x129,
        /// <summary>
        /// 
        /// </summary>
        WM_QUEUESYNC = 0x23,
        /// <summary>
        /// 
        /// </summary>
        WM_QUIT = 0x12,
        /// <summary>
        /// 
        /// </summary>
        WM_RBUTTONDBLCLK = 0x206,
        /// <summary>
        /// 
        /// </summary>
        WM_RBUTTONDOWN = 0x204,
        /// <summary>
        /// 
        /// </summary>
        WM_RBUTTONUP = 0x205,
        /// <summary>
        /// 
        /// </summary>
        WM_RENDERALLFORMATS = 0x306,
        /// <summary>
        /// 
        /// </summary>
        WM_RENDERFORMAT = 0x305,
        /// <summary>
        /// 
        /// </summary>
        WM_SETCURSOR = 0x20,
        /// <summary>
        /// 
        /// </summary>
        WM_SETFOCUS = 0x7,
        /// <summary>
        /// 
        /// </summary>
        WM_SETFONT = 0x30,
        /// <summary>
        /// 
        /// </summary>
        WM_SETHOTKEY = 0x32,
        /// <summary>
        /// 
        /// </summary>
        WM_SETICON = 0x80,
        /// <summary>
        /// 
        /// </summary>
        WM_SETREDRAW = 0xB,
        /// <summary>
        /// 
        /// </summary>
        WM_SETTEXT = 0xC,
        /// <summary>
        /// 
        /// </summary>
        WM_SETTINGCHANGE = 0x1A,
        /// <summary>
        /// 
        /// </summary>
        WM_SHOWWINDOW = 0x18,
        /// <summary>
        /// 
        /// </summary>
        WM_SIZE = 0x5,
        /// <summary>
        /// 
        /// </summary>
        WM_SIZECLIPBOARD = 0x30B,
        /// <summary>
        /// 
        /// </summary>
        WM_SIZING = 0x214,
        /// <summary>
        /// 
        /// </summary>
        WM_SPOOLERSTATUS = 0x2A,
        /// <summary>
        /// 
        /// </summary>
        WM_STYLECHANGED = 0x7D,
        /// <summary>
        /// 
        /// </summary>
        WM_STYLECHANGING = 0x7C,
        /// <summary>
        /// 
        /// </summary>
        WM_SYNCPAINT = 0x88,
        /// <summary>
        /// 
        /// </summary>
        WM_SYSCHAR = 0x106,
        /// <summary>
        /// 
        /// </summary>
        WM_SYSCOLORCHANGE = 0x15,
        /// <summary>
        /// 
        /// </summary>
        WM_SYSCOMMAND = 0x112,
        /// <summary>
        /// 
        /// </summary>
        WM_SYSDEADCHAR = 0x107,
        /// <summary>
        /// 
        /// </summary>
        WM_SYSKEYDOWN = 0x104,
        /// <summary>
        /// 
        /// </summary>
        WM_SYSKEYUP = 0x105,
        /// <summary>
        /// 
        /// </summary>
        WM_SYSTIMER = 0x118,  // undocumented, see http://support.microsoft.com/?id=108938
        /// <summary>
        /// 
        /// </summary>
        WM_TCARD = 0x52,
        /// <summary>
        /// 
        /// </summary>
        WM_TIMECHANGE = 0x1E,
        /// <summary>
        /// 
        /// </summary>
        WM_TIMER = 0x113,
        /// <summary>
        /// 
        /// </summary>
        WM_UNDO = 0x304,
        /// <summary>
        /// 
        /// </summary>
        WM_UNINITMENUPOPUP = 0x125,
        /// <summary>
        /// 
        /// </summary>
        WM_USER = 0x400,
        /// <summary>
        /// 
        /// </summary>
        WM_USERCHANGED = 0x54,
        /// <summary>
        /// 
        /// </summary>
        WM_VKEYTOITEM = 0x2E,
        /// <summary>
        /// 
        /// </summary>
        WM_VSCROLL = 0x115,
        /// <summary>
        /// 
        /// </summary>
        WM_VSCROLLCLIPBOARD = 0x30A,
        /// <summary>
        /// 
        /// </summary>
        WM_WINDOWPOSCHANGED = 0x47,
        /// <summary>
        /// 
        /// </summary>
        WM_WINDOWPOSCHANGING = 0x46,
        /// <summary>
        /// 
        /// </summary>
        WM_WININICHANGE = 0x1A,
        /// <summary>
        /// 
        /// </summary>
        WM_XBUTTONDBLCLK = 0x20D,
        /// <summary>
        /// 
        /// </summary>
        WM_XBUTTONDOWN = 0x20B,
        /// <summary>
        /// 
        /// </summary>
        WM_XBUTTONUP = 0x20C
    }
}
