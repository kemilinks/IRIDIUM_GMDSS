using KemiLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Entity
{
    public abstract class KemiTask
    {
        private int waitTaskCompletionInSeconds = 15;
        protected System.Threading.Tasks.Task task;
        protected AutoResetEvent reset;
        protected bool stop = false;

        protected int WaitTaskCompletionInSeconds
        {
            get { return waitTaskCompletionInSeconds; }
            set { this.waitTaskCompletionInSeconds = value; }
        }

        public int SleepIntervalInSeconds { get; set; }

        public virtual void StartTask()
        {
            try
            {
                this.stop = false;
                this.reset = new AutoResetEvent(false);
                task = new System.Threading.Tasks.Task(() => RunTask());
                task.Start();
            }
            catch (Exception ex)
            {
                LogWriter.LogException(KemiLogger.LogWriter.Level.SEVERE, "StartTask Error", ex);
            }
        }

        public virtual void StopTask()
        {
            try
            {
                this.stop = true;
                this.reset.Set();
                this.task.Wait(waitTaskCompletionInSeconds * 1000);
                EndTask();
            }
            catch (Exception ex)
            {
                LogWriter.LogException(KemiLogger.LogWriter.Level.SEVERE, "StopTask Error", ex);
            }
        }

        protected virtual void RunTask()
        {
            while (!this.stop)
            {
                try
                {
                    DoTask();
                }
                catch (Exception ex)
                {
                    LogWriter.LogException(KemiLogger.LogWriter.Level.SEVERE, "RunTask Error", ex);
                }

                this.reset.WaitOne(this.SleepIntervalInSeconds * 1000); // seconds to milliseconds
            }
        }

        protected abstract void DoTask();//need to be implemented
        protected abstract void EndTask();//need to be implemented for clearning up
    }
}
