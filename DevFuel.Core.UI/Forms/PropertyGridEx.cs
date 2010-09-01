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
using System.Collections;

namespace DevFuel.Core.UI.Forms
{
    /// <summary>
    /// An enhanced PropertyGrid that allows use of the Tab key to navigate, etc.
    /// </summary>
    public class PropertyGridEx : PropertyGrid
    {
        /// <summary>
        /// Gets the internal property grid.
        /// </summary>
        /// <value>The internal property grid.</value>
        public Control InternalPropertyGrid
        {
            get { return base.Controls[2]; }
        }

        //public event KeyEventHandler CmdKeyDown;

        //protected void OnCmdKeyDown(Object sender, KeyEventArgs e)
        //{
        //    if (CmdKeyDown != null)
        //        CmdKeyDown(sender, e);
        //}

        /// <summary>
        /// Do special processing for Tab key.
        /// </summary>
        /// <param name="msg">A <see cref="T:System.Windows.Forms.Message"/>, passed by reference, that represents the window message to process.</param>
        /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys"/> values that represents the key to process.</param>
        /// <returns>
        /// true if the character was processed by the control; otherwise, false.
        /// </returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (ProcessTabKey(msg, keyData))
                return true;

            Keys translatedKeys = Keys.None;
            if ((((int)keyData) & (int)(Keys.LButton | Keys.MButton | Keys.Back)) != (int)Keys.None)
                translatedKeys |= Keys.Enter;
            translatedKeys |= keyData;
            KeyEventArgs a = new KeyEventArgs(translatedKeys);

            switch (msg.Msg)
            {
                case (int)WindowsMessages.WM_KEYDOWN:
                    OnKeyDown(a);
                    break;
                case (int)WindowsMessages.WM_SYSKEYDOWN:
                    OnKeyDown(a);
                    break;
                case (int)WindowsMessages.WM_KEYUP:
                    OnKeyUp(a);
                    break;
                case (int)WindowsMessages.WM_SYSKEYUP:
                    OnKeyUp(a);
                    break;
            }

            if (a.Handled)
                return true;
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private bool ProcessTabKey(Message msg, Keys keyData)
        {
            if ((keyData == Keys.Tab) || (keyData == (Keys.Tab | Keys.Shift)))
            {
                GridItem selectedItem = SelectedGridItem;
                GridItem root = selectedItem;
                while (root.Parent != null)
                {
                    root = root.Parent;
                }
                // Find all expanded items and put them in a list.
                ArrayList items = new ArrayList();
                AddExpandedItems(root, items);
                if (selectedItem != null)
                {
                    // Find selectedItem.
                    int foundIndex = items.IndexOf(selectedItem);
                    if ((keyData & Keys.Shift) == Keys.Shift)
                    {
                        foundIndex--;
                        if (foundIndex < 0)
                        {
                            foundIndex = items.Count - 1;
                        }
                        SelectedGridItem = (GridItem)items[foundIndex];
                    }
                    else
                    {
                        foundIndex++;
                        if (foundIndex >= items.Count)
                        {
                            foundIndex = 0;
                        }
                        SelectedGridItem = (GridItem)items[foundIndex];
                    }

                    return true;
                }
            }
            return false;
        }

        private void AddExpandedItems(GridItem parent, IList items)
        {
            if (parent.PropertyDescriptor != null)
            {
                items.Add(parent);
            }
            if (parent.Expanded)
            {
                foreach (GridItem child in parent.GridItems)
                {
                    AddExpandedItems(child, items);
                }
            }
        }

    }
}
