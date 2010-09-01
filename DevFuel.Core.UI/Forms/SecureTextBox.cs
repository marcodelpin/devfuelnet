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

//Source: http://weblogs.asp.net/pglavich/archive/2006/02/26/439077.aspx
//Found through: http://www.codeproject.com/KB/security/simple_password_manager.aspx


#region Using Statements

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security;
using System.Runtime.InteropServices;

#endregion

namespace DevFuel.Core.UI.Forms
{
    /// <summary>
    /// This is a TextBox implementation that uses the System.Security.SecureString as its backing
    /// store instead of standard managed string instance. At no time, is a managed string instance
    /// used to hold a component of the textual entry.
    /// It does not display any text and relies on the 'PasswordChar' character to display the amount of
    /// characters entered. If no password char is defined, then an 'asterisk' is used.
    /// </summary>
    public partial class SecureTextBox : TextBox
    {
        #region Private fields

        private bool _displayChar = false;
        SecureString _secureEntry = new SecureString();

        private TextBox _innerTextBox = new TextBox();

        #endregion

        #region Constructor 
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureTextBox"/> class.
        /// </summary>
        public SecureTextBox()
        {
            InitializeComponent();

            this.PasswordChar = '*';   // default to an asterisk
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The secure string instance captured so far.
        /// This is the preferred method of accessing the string contents.
        /// </summary>
        public SecureString SecureText
        {
            get
            {
                return _secureEntry;
            }
            set
            {
                _secureEntry = value;
                this.Text = new string(this.PasswordChar,_secureEntry.Length);
            }
        }

        /// <summary>
        /// Allows the consumer to retrieve this string instance as a character array. NOte that this is still
        /// visible plainly in memory and should be 'consumed' as wuickly as possible, then the contents
        /// 'zero-ed' so that they cannot be viewed.
        /// </summary>
        public char[] CharacterData
        {
            get
            {
                char[] bytes = new char[_secureEntry.Length];
                IntPtr ptr = IntPtr.Zero;

                try
                {
                    ptr = Marshal.SecureStringToBSTR(_secureEntry);
                    bytes = new char[_secureEntry.Length];
                    Marshal.Copy(ptr, bytes,0,_secureEntry.Length);
                }
                finally
                {
                    if (ptr != IntPtr.Zero)
                        Marshal.ZeroFreeBSTR(ptr);
                }

                return bytes;
            }
        }


        #endregion

        #region ProcessKeyMessage

        /// <summary>
        /// Processes a keyboard message.
        /// </summary>
        /// <param name="m">A <see cref="T:System.Windows.Forms.Message"/>, passed by reference, that represents the window message to process.</param>
        /// <returns>
        /// true if the message was processed by the control; otherwise, false.
        /// </returns>
        protected override bool  ProcessKeyMessage(ref Message m)
        {

            if (_displayChar)
            {
                return base.ProcessKeyMessage(ref m);
            }
            else
            {
                _displayChar = true;
                return true;
            }
        }

        #endregion

        #region IsInputChar

        /// <summary>
        /// Determines if a character is an input character that the control recognizes.
        /// </summary>
        /// <param name="charCode">The character to test.</param>
        /// <returns>
        /// true if the character should be sent directly to the control and not preprocessed; otherwise, false.
        /// </returns>
        protected override bool IsInputChar(char charCode)
        {
            int startPos = this.SelectionStart;

            bool isChar = base.IsInputChar(charCode);
            if (isChar)
            {
                int keyCode = (int)charCode;

                // If the key pressed is NOT a control/cursor type key, then add it to our instance.
                // Note: This does not catch the SHIFT key or anything like that
                if (!Char.IsControl(charCode) && !char.IsHighSurrogate(charCode) && !char.IsLowSurrogate(charCode))
                {

                    if (this.SelectionLength > 0)
                    {
                        for (int i = 0; i < this.SelectionLength; i++)
                            _secureEntry.RemoveAt(this.SelectionStart);
                    }

                    if (startPos == _secureEntry.Length)
                    {
                        _secureEntry.AppendChar(charCode);
                    }
                    else
                    {
                        _secureEntry.InsertAt(startPos, charCode);
                    }

                    this.Text = new string(PasswordChar, _secureEntry.Length);


                    _displayChar = false;
                    startPos++;
                    
                    this.SelectionStart = startPos;
                }
                else
                {
                    // We need to check what key has been pressed.
                    IButtonControl btn = null;

                    switch (keyCode)
                    {
                        case (int)Keys.Back:
                            if (this.SelectionLength == 0 && startPos > 0)
                            {
                                startPos--;
                                _secureEntry.RemoveAt(startPos);
                                this.Text = new string('*', _secureEntry.Length);
                                this.SelectionStart = startPos;
                            }
                            else if (this.SelectionLength > 0)
                            {
                                for (int i = 0; i < this.SelectionLength; i++)
                                    _secureEntry.RemoveAt(this.SelectionStart);
                            }
                            _displayChar = false;   // If we dont do this, we get a 'double' BACK keystroke effect

                            break;
                        case (int)Keys.Tab:
                            // Selected the next control on the TopLevelControl (System.Windows.Forms) and set the focus on this control.
                            this.TopLevelControl.SelectNextControl(this, true, true, true, true);

                            _displayChar = false;

                            break;

                        case (int)Keys.Enter:
                            // Perform the click event of the default accept button.
                            btn = this.FindForm().AcceptButton;

                            if (btn != null)
                                btn.PerformClick();

                            _displayChar = false;

                            break;

                        case (int)Keys.Escape:
                            // Perform the click event of the default cancel button.
                            btn = this.FindForm().CancelButton;
                            if (btn != null)
                                btn.PerformClick();

                            _displayChar = false;

                            break;
                    }
                }
            }
            else
                _displayChar = true;

            return isChar;
        }

        #endregion

        #region IsInputKey

        /// <summary>
        /// Determines whether the specified key is an input key or a special key that requires preprocessing.
        /// </summary>
        /// <param name="keyData">One of the key's values.</param>
        /// <returns>
        /// true if the specified key is an input key; otherwise, false.
        /// </returns>
        protected override bool IsInputKey(Keys keyData)
        {
            bool result = true;

            // Note: This whole section is only to deal with the 'Delete' key.

            bool allowedToDelete =
                (
                     ((keyData & Keys.Delete) == Keys.Delete)
                );

            // Debugging only
            //this.Parent.Text = keyData.ToString() + " " + ((int)keyData).ToString() + " allowedToDelete = " + allowedToDelete.ToString();

            if (allowedToDelete)
            {
                if (this.SelectionLength == _secureEntry.Length)
                {
                    _secureEntry.Clear();
                }
                else if (this.SelectionLength > 0)
                {
                    for (int i = 0; i < this.SelectionLength; i++)
                        _secureEntry.RemoveAt(this.SelectionStart);

                }
                else
                {
                    if ((keyData & Keys.Delete) == Keys.Delete && this.SelectionStart < this.Text.Length)
                        _secureEntry.RemoveAt(this.SelectionStart);
                }

            }

            return result;

        }

        #endregion

    }
}
