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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using DevFuel.Core.Threading;
using System.Threading;

namespace DevFuel.Core.UI.Forms
{
    /// <summary>
    /// A dialog that displays the progress of an asynchronous task
    /// </summary>
    public partial class TaskProgressBox : Form, IProgressObserver
    {
        private bool m_Closed = false;
        
        private Task m_Task;
        /// <summary>
        /// Gets or sets the task.
        /// </summary>
        /// <value>The task.</value>
        public Task Task
        {
            get { return m_Task; }
            set { m_Task = value; }
        }

        
        private float stepCount = 1;

        private delegate void delStringSet(String text);
        private delegate void delIntSet(int val);

        private delIntSet OnSetStepCount;
        private delStringSet OnSetStepLabel;
        private delIntSet OnSetStepCurrent;
        private delStringSet OnSetTaskLabel;

        private void SetStepLabel(string sLabel)
        {
            lblStep.Text = sLabel;
        }

        private void SetTaskLabel(string sLabel)
        {
            this.Text = sLabel;
        }

        private void SetStepCount(int iCount)
        {
            stepCount = iCount;

            if (stepCount <= 0.0f)
                stepCount = 1.0f;

            progAction.Style = ProgressBarStyle.Blocks;
        }

        private void SetStepCurrent(int iCurrent)
        {
            float current = iCurrent;
            if (current > stepCount)
                current = stepCount;
            else if (current < float.MinValue)
                current = float.MinValue;
            if (stepCount <= 0.0f)
                stepCount = 1.0f;
            int value = (int)((current/stepCount)*1000);
            progAction.Value = value;
            if (progAction.Maximum == iCurrent)
            {
                progAction.Value = 999;
                progAction.Refresh();
                Thread.Sleep(300);
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="TaskProgressBox"/> class.
        /// </summary>
        public TaskProgressBox()
        {
            InitializeComponent();
            OnSetStepCount = new delIntSet(SetStepCount);
            OnSetStepCurrent = new delIntSet(SetStepCurrent);
            OnSetStepLabel = new delStringSet(SetStepLabel);
            OnSetTaskLabel = new delStringSet(SetTaskLabel);
        }

        private void DlgProgress_Load(object sender, EventArgs e)
        {
            if (HostApplicationResources.DialogIcon != null)
                this.Icon = HostApplicationResources.DialogIcon;
            System.Threading.ThreadPool.QueueUserWorkItem(
                              new System.Threading.WaitCallback(DoTask),
                              this);
        }

        private void DoTask(object status)
        {
            TaskProgressBox p = status as TaskProgressBox;
            p.Task.Run(p, p.Task.Parameters);
        }

        private void DlgProgress_Shown(object sender, EventArgs e)
        {

        }

        #region IProgressObserver Members

        /// <summary>
        /// Sets the step label.
        /// </summary>
        /// <value>The step label.</value>
        public string StepLabel
        {
            set { Invoke(OnSetStepLabel, value); }
        }

        /// <summary>
        /// Sets the task label.
        /// </summary>
        /// <value>The task label.</value>
        public string TaskLabel
        {
            set { Invoke(OnSetTaskLabel, value); }
        }

        /// <summary>
        /// Sets the step count.
        /// </summary>
        /// <value>The step count.</value>
        public int StepCount
        {
            set { Invoke(OnSetStepCount, value); }
        }

        /// <summary>
        /// Sets the step current.
        /// </summary>
        /// <value>The step current.</value>
        public int StepCurrent
        {
            set { Invoke(OnSetStepCurrent, value); }
        }

        /// <summary>
        /// Completes the task.
        /// </summary>
        public void CompleteTask()
        {
            if (!m_Closed)
            {
                Invoke(OnSetStepCurrent, 1000);
                Invoke(new EventHandler(delegate(object sender, EventArgs e) {
                    this.Close(); 
                }), null, EventArgs.Empty);
            }
        }

        #endregion


        /// <summary>
        /// Shows the specified task.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="parent">The parent.</param>
        public static void Show(Task task, IWin32Window parent )
        {
            TaskProgressBox dlg = new TaskProgressBox();
            dlg.Task = task;
            dlg.Show(parent);
        }

        private void TaskProgressBox_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_Closed = true;
        }

       
    }
}