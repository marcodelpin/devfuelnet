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
using System.Windows.Forms;
using System.Drawing;

namespace DevFuel.Core.UI.Forms
{
    /// <summary>
    /// Extends the RichTextBox to allow additional formatting options
    /// </summary>
    public class RichTextBoxEx : RichTextBox
    {
        /// <summary>
        /// Appends the format.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="text">The text.</param>
        /// <param name="args">The args.</param>
        public void AppendFormat(Color color, string text, params object[] args)
        {
            AppendFormat(SystemFonts.DefaultFont, color, text, args);
        }

        /// <summary>
        /// Appends the format.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="color">The color.</param>
        /// <param name="text">The text.</param>
        /// <param name="args">The args.</param>
        public void AppendFormat(Font font, Color color, string text, params object[] args)
        {
            string s = string.Format(text, args);
            this.SelectionStart = this.TextLength;
            this.SelectionLength = 0;
            if (font != null)
                this.SelectionFont = font;
            else
                this.SelectionFont = SystemFonts.DefaultFont;
            if (color != null)
                this.SelectionColor = color;
            else
                this.SelectionColor = this.ForeColor;
            this.SelectedText = s;
        }

        /// <summary>
        /// Appends the format.
        /// </summary>
        /// <param name="bold">if set to <c>true</c> [bold].</param>
        /// <param name="italic">if set to <c>true</c> [italic].</param>
        /// <param name="color">The color.</param>
        /// <param name="text">The text.</param>
        /// <param name="args">The args.</param>
        public void AppendFormat(bool bold, bool italic, Color color, string text, params object[] args)
        {
            FontStyle fs = FontStyle.Regular;
            if (bold)
                fs |= FontStyle.Bold;
            if (italic)
                fs |= FontStyle.Italic;
            Font f = new Font(SystemFonts.DefaultFont, fs) as Font;
            AppendFormat(f, color, text, args);
        }

        /// <summary>
        /// Appends the format.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="args">The args.</param>
        public void AppendFormat(string text, params object[] args)
        {
            AppendFormat(SystemFonts.DefaultFont, this.ForeColor, text, args);
        }

        /// <summary>
        /// Appends the line.
        /// </summary>
        public void AppendLine()
        {
            AppendFormat(SystemFonts.DefaultFont, this.ForeColor, Environment.NewLine);
        }
    }
}
