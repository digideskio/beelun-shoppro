using System;
using System.Net;
using System.Collections.Generic;

using HappyDog.SL.Resources;
using HappyDog.SL.Common;
using HappyDog.SL.EventArguments;

namespace HappyDog.SL.Data
{
    /// <summary>
    /// We often need run tasks sequencially
    /// Task tracker makes sure tasks run with specified order
    /// Once one task fails, all fail.
    /// All tasks are working on the same data set
    /// </summary>
    public class TaskTracker
    {
        #region Private vars
        // running tag. If we are running, add() is not allowed
        private bool bRunning = false;

        // Total task number
        private int totalTasksNum = 0;

        // Hold a reference to the input param
        private object param = null;

        // Task container
        private List<ITask> taskList = new List<ITask>();
        #endregion

        /// <summary>
        /// Event handler when all tasks finish inside the task tracker
        /// </summary>
        public event EventHandler<HDCallBackEventArgs> onComplete;
        public event EventHandler<HDCallBackEventArgs> onProgress;

        /// <summary>
        /// Constructor 
        /// </summary>
        public TaskTracker() { }

        /// <summary>
        /// Destructor
        /// </summary>
        ~TaskTracker()
        {
            foreach (ITask t in taskList)
            {
                t.onComplete -= new EventHandler<HDCallBackEventArgs>(Task_onComplete);
            }
        }

        /// <summary>
        /// Add a task into the task traker. Valid only when the task tracker is not running
        /// </summary>
        /// <param name="task"></param>
        public void Add(ITask task)
        {
            if (!this.bRunning)
            {
                task.onComplete += new EventHandler<HDCallBackEventArgs>(Task_onComplete);
                taskList.Add(task);
            }
        }

        /// <summary>
        /// Start run tasks one by one
        /// </summary>
        /// <param name="param"></param>
        public void StartAsync(object param)
        {
            this.bRunning = true;
            this.totalTasksNum = taskList.Count;
            this.param = param;

            if (this.totalTasksNum != 0)
            {
                if(this.onProgress != null)
                {
                    this.onProgress(this, new HDCallBackEventArgs(0, new TaskProgress(taskList[0].TaskDescription, this.GetPercentage(0))));
                }
                taskList[0].StartAsync(this.param, 0);
            }
        }

        #region Private methods
        /// <summary>
        /// One task is complete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Task_onComplete(object sender, HDCallBackEventArgs e)
        {
            if (e.Error != null)
            {
                this.onComplete(this, e);
            }
            else
            {
                int nextId = (int)e.tag + 1;
                if (nextId < this.totalTasksNum)
                {
                    if (this.onProgress != null)
                    {
                        this.onProgress(this, new HDCallBackEventArgs(nextId, new TaskProgress(taskList[nextId].TaskDescription, this.GetPercentage(nextId))));
                    }
                    taskList[nextId].StartAsync(this.param, nextId);
                }
                else
                {
                    // this.onComplete(this, new TaskArgs(TaskArgsStatus.SUCCESS, null, 0));
                    this.onComplete(this, new HDCallBackEventArgs(0, null));
                }
            }
        }

        /// <summary>
        /// Get current progress
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int GetPercentage(int i)
        {
            if (this.totalTasksNum != 0)
            {
                return (((i+1) * 100)/ this.totalTasksNum);
            }
            else
            {
                return 0;
            }
        }
        #endregion
    }

    /// <summary>
    /// Task progress task
    /// </summary>
    public class TaskProgress
    {
        public string TaskDescription { get; set; }
        public int percentage { get; set; }

        private TaskProgress() { }
        public TaskProgress(string desp, int percentage)
        {
            this.TaskDescription = desp;
            this.percentage = percentage;
        }
    }
}
