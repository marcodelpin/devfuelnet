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
using DevFuel.Core.UI;

namespace DevFuel.Core.Threading
{
    /// <summary>
    /// Delegate that Handles a Task with an IProgressObserver and some number of parameters
    /// </summary>
    public delegate object TaskHandler(IProgressObserver w, object[] parameters);
    /// <summary>
    /// A Single unit of asynchronous work
    /// </summary>
    public abstract class Task
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Task"/> class.
        /// </summary>
        /// <param name="host">The task host.</param>
        /// Updated 1/3/2008 by ee
        public Task(ITaskHost host)
        {
            m_Host = host;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Task"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="tag">The tag.</param>
        public Task(ITaskHost host, object tag)
        {
            m_Tag = tag;
            m_Host = host;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Task"/> class without a TaskHost.
        /// </summary>
        /// Updated 1/3/2008 by ee
        public Task()
        {
            m_Host = null;
        }

        /// <summary>
        /// Called when [do task].
        /// </summary>
        /// <param name="w">The w.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        internal abstract object OnDoTask(IProgressObserver w, object[] parameters);

        private object m_Result = null;
        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>The result.</value>
        public object Result
        {
            get { return m_Result; }
        }

        /// <summary>
        /// The Tag Storage field
        /// </summary>
        protected object m_Tag = null;
        /// <summary>
        /// Gets the tag.
        /// </summary>
        /// <value>The tag.</value>
        public object Tag
        {
            get { return m_Tag; }
        }

        private Exception m_Exception = null;
        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception
        {
            get { return m_Exception; }
        }

        private object[] m_Parameters;
        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public object[] Parameters
        {
            get { return m_Parameters; }
        }

        private ITaskHost m_Host;
        /// <summary>
        /// Gets the host.
        /// </summary>
        /// <value>The host.</value>
        public ITaskHost Host
        {
            get { return m_Host; }
        }

        /// <summary>
        /// Invokes this task in the context of the ITaskHost with the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// Updated 1/3/2008 by ee
        public void Invoke(params object[] parameters)
        {
            m_Parameters = parameters;
            if (m_Host != null)
                m_Host.InvokeTask(this);
            else
                CallDoTask(new NullProgressObserver());
        }

        /// <summary>
        /// Runs this Task Directly with the specified parameters and IProgressObserver.
        /// </summary>
        /// <param name="progressObserver">The progress observer.</param>
        /// <param name="parameters">The parameters.</param>
        /// Updated 1/3/2008 by ee
        public void Run(IProgressObserver progressObserver, params object[] parameters)
        {
            m_Parameters = parameters;
            CallDoTask(progressObserver);
        }

        /// <summary>
        /// Runs with the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public void Run(params object[] parameters)
        {
            Run(new NullProgressObserver(), parameters);
        }

        /// <summary>
        /// Calls the do task.
        /// </summary>
        /// <param name="progressObserver">The progress observer.</param>
        protected void CallDoTask(IProgressObserver progressObserver)
        {
            try
            {
                m_Result = OnDoTask(progressObserver, m_Parameters);
                progressObserver.CompleteTask();
                m_Exception = null;
                if (m_Host != null)
                {
                    if (m_Host.Invoker != null)
                        m_Host.Invoker.Invoke(new EventHandler<TaskEventArgs>(m_Host.OnTaskComplete), this, new TaskEventArgs(this));
                    else
                        m_Host.OnTaskComplete(this, new TaskEventArgs(this));
                }
            }
            catch (Exception x)
            {
                progressObserver.CompleteTask();
                m_Exception = x;
                m_Result = null;
                if (m_Host != null)
                {
                    if (m_Host.Invoker != null)
                        m_Host.Invoker.Invoke(new EventHandler<TaskEventArgs>(m_Host.OnTaskException), this, new TaskEventArgs(this));
                    else
                        m_Host.OnTaskException(this, new TaskEventArgs(this));
                }
            }
        }
    }

    /// <summary>
    /// A Task that utilizes a Delegate
    /// </summary>
    public class DelegateTask : Task
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateTask"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="tag">The tag.</param>
        public DelegateTask(ITaskHost host, TaskHandler handler, object tag)
            : base(host, tag)
        {
            DoTask += handler;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateTask"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="handler">The handler.</param>
        public DelegateTask(ITaskHost host, TaskHandler handler)
            : base(host)
        {
            DoTask += handler;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateTask"/> class.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public DelegateTask(TaskHandler handler)
            : base()
        {
            DoTask += handler;
        }

        /// <summary>
        /// Occurs when [do task].
        /// </summary>
        protected event TaskHandler DoTask;
        /// <summary>
        /// Fires the DoTask Event.
        /// </summary>
        /// <param name="progressObserver">The progress observer.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        internal override object OnDoTask(IProgressObserver progressObserver, object[] parameters)
        {
            if (DoTask != null)
                return DoTask(progressObserver, parameters);
            return null;
        }
    }

    /// <summary>
    /// An implementation of the abstract Task With generics.
    /// </summary>
    /// <typeparam name="T">The Return Type</typeparam>
    /// <typeparam name="V">The Parameter Type</typeparam>
    public abstract class SimpleTask<T,V> : Task
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleTask&lt;T, V&gt;"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="tag">The tag.</param>
        public SimpleTask(ITaskHost host, object tag) : base(host, tag)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleTask&lt;T, V&gt;"/> class.
        /// </summary>
        /// <param name="host">The task host.</param>
        /// Updated 1/3/2008 by ee
        public SimpleTask(ITaskHost host) : base(host)
        {
            m_Tag = this.GetType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleTask&lt;T, V&gt;"/> class.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public SimpleTask(TaskHandler handler)
            : base()
        {
            m_Tag = this.GetType();
        }

        internal override object OnDoTask(IProgressObserver progressObserver, object[] parameters)
        {
            V parameter = default(V);
            if (parameters.Length > 0 && typeof(V).IsAssignableFrom(parameters[0].GetType()))
                parameter = (V)parameters[0];
            return HandleTask(progressObserver, parameter);
        }

        /// <summary>
        /// Handles the task.
        /// </summary>
        /// <param name="progressObserver">The progress observer.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        protected abstract T HandleTask(IProgressObserver progressObserver, V parameter);
    }


    /// <summary>
    /// Argument class for Task Related Events
    /// </summary>
    public class TaskEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskEventArgs"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        public TaskEventArgs(Task task)
        {
            m_Task = task;
        }
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
    }
    /// <summary>
    /// Interface that represents the ability of an object to host a Task's Execution
    /// </summary>
    public interface ITaskHost : IInvokable
    {
        /// <summary>
        /// Invokes the task.
        /// </summary>
        /// <param name="task">The task.</param>
        void InvokeTask(Task task);
        /// <summary>
        /// Called when [task complete].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="args">The <see cref="DevFuel.Core.Threading.TaskEventArgs"/> instance containing the event data.</param>
        void OnTaskComplete(object source, TaskEventArgs args);
        /// <summary>
        /// Called when [task exception].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="args">The <see cref="DevFuel.Core.Threading.TaskEventArgs"/> instance containing the event data.</param>
        void OnTaskException(object source, TaskEventArgs args);
    }
}
