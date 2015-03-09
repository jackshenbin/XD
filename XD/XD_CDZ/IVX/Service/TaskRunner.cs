using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace BOCOM.IVX.Service
{
    /// <summary>
    /// 循环从队列里取任务执行
    /// 任务执行完通告请求者
    /// 添加任务时，根据策略， 把队列里还没有执行的任务移除， 还是检查有重复的任务
    /// </summary>
    public class TaskRunner<TIN, TOUT> : ITask
    {
        #region Fields

        private Queue<TaskItem<TIN, TOUT>> m_TaskItems;
        
        private ManualResetEventSlim m_MRETaskAdded = new ManualResetEventSlim(false);
        
        private bool m_IsRunning = true;

        private string m_Name;

        private Func<Queue<TaskItem<TIN, TOUT>>, TaskItem<TIN, TOUT>, bool> m_ApplyInsertTaskItemPolicy;

        private bool m_IsExcutingFuncToRun = false;

        private object m_SyncObjCancelExcutingFuncToRun = new object();

        private Task m_LoopRunTask;

        private CancellationToken m_CancellationToken;

        private CancellationTokenSource m_TokenSource;

        #endregion

        #region Properties

        public bool IsExecutingFuncToRun
        {
            get { return m_IsExcutingFuncToRun; }
            set
            {
                lock (m_SyncObjCancelExcutingFuncToRun)
                {
                    m_IsExcutingFuncToRun = value;
                }
            }
        }

        #endregion

        #region Constructors

        public TaskRunner(string name, Func<Queue<TaskItem<TIN, TOUT>>, TaskItem<TIN, TOUT>, bool> applyInsertTaskItemPolicy)
        {
            m_Name = name;
            m_ApplyInsertTaskItemPolicy = applyInsertTaskItemPolicy;
            m_TaskItems = new Queue<TaskItem<TIN, TOUT>>();

        }

        #endregion

        #region Public helper functions

        public void AddTask(TaskItem<TIN, TOUT> taskItem)
        {
            lock (m_TaskItems)
            {
                if (m_LoopRunTask == null)
                {
                    TaskFactory factory = new TaskFactory();

                    m_TokenSource = new CancellationTokenSource();
                    m_LoopRunTask = factory.StartNew(new Action(LoopRun), m_TokenSource.Token);
                    
                    // m_LoopUpdateFileStatusTask.Start();
                }

                // 应用插入新任务策略， 在策略中检查是否删除队列中的已有任务
                bool toInsert = true;
                if (m_ApplyInsertTaskItemPolicy != null)
                {
                    toInsert = m_ApplyInsertTaskItemPolicy(m_TaskItems, taskItem);
                }
                if (toInsert)
                {
                    m_TaskItems.Enqueue(taskItem);
                    MyLog4Net.Container.Instance.Log.DebugFormat("{0}: Add Task item: {1}, Total: {2}",
                        m_Name, taskItem.GetHashCode().ToString(), m_TaskItems.Count);
                    m_MRETaskAdded.Set();
                }
            }
        }

        public bool StopAndClear()
        {
            lock (m_TaskItems)
            {
                m_TaskItems.Clear();
            }
            return !IsExecutingFuncToRun;
        }

        public bool Stop()
        {
            MyLog4Net.Container.Instance.Log.DebugFormat("Entering TaskRunner.Stop, {0} ...", this.m_Name);
            bool result = false;
            StopAndClear();
            m_IsRunning = false;
            m_MRETaskAdded.Set();
            int i = 0;

            if (m_LoopRunTask == null)
            {
                result = true;
            }
            else
            {
                while (m_LoopRunTask.Status != TaskStatus.RanToCompletion && i++ < 100)
                {
                    MyLog4Net.Container.Instance.Log.DebugFormat("TaskRunner.Stop, {0}, wait for taskloop complete, {1}", this.m_Name, i);
                    Thread.Sleep(500);
                    //System.Windows.Forms.Application.DoEvents();
                }


                if (m_LoopRunTask.Status == TaskStatus.RanToCompletion)
                {
                    m_LoopRunTask.Dispose();
                    result = true;
                }
            }

            MyLog4Net.Container.Instance.Log.DebugFormat("TaskRunner.Stop, {0}, taskloop exit: {1}", this.m_Name, result);

            return result;
        }

        #endregion

        #region Private helper functions

        private void LoopRun()
        {
            TaskItem<TIN, TOUT> taskItem = null;
            bool hasItem = false;
            while (m_IsRunning)
            {
                lock (m_TaskItems)
                {
                    hasItem = false;
                    if (m_TaskItems.Count > 0)
                    {
                        hasItem = true;
                        taskItem = m_TaskItems.Dequeue();
                        MyLog4Net.Container.Instance.Log.DebugFormat("{0}: Dequeued Task item: {1}, Total: {2}",
                        m_Name, taskItem.GetHashCode().ToString(), m_TaskItems.Count);
                    }
                }

                if (hasItem)
                {
                    try
                    {
                        IsExecutingFuncToRun = true;

                        if (taskItem.DelayExecute > 0)
                        {
                            Thread.Sleep(taskItem.DelayExecute);
                        }
                        TOUT result = taskItem.FuncToRun.Invoke(taskItem.Para);

                        IsExecutingFuncToRun = false;
                        if (taskItem.Callback != null)
                        {
                            taskItem.Callback.Invoke(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        MyLog4Net.Container.Instance.Log.Error("LoopRunTask error: ", ex);
                        Debug.Assert(false, ex.Message);
                    }
                }
                else
                {
                    m_MRETaskAdded.Wait();
                    m_MRETaskAdded.Reset();
                }

            } // endof  lock (m_Tasks)
        }
        
        #endregion

    }

    public interface ITask
    {
        bool Stop();
    }
}
