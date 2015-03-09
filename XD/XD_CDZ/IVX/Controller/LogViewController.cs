using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Controls;
using BOCOM.IVX.Framework;
using BOCOM.IVX.Protocol;
using BOCOM.DataModel;
using System.Diagnostics;
using BOCOM.IVX.Protocol.Model;
using BOCOM.IVX.Service;

namespace BOCOM.IVX.Controller
{
    public class LogViewController
    {
        private ucLogList m_ucLogResultList;
        private LogSearchResultSummary m_Summary;
        private object m_SyncObjSwitchSearch = new object();
        private LogReqInfo m_reqPara;
        private Service.TaskRunner<LogReqInfo, LogSearchResultSummary> m_SearchTaskRunner;

        public LogViewController(ucLogList ucLogResultList)
        {
            m_ucLogResultList = ucLogResultList;

            Framework.Container.Instance.EvtAggregator.GetEvent<SearchLogEvent>().Subscribe(OnSearchLogRequested);

            m_SearchTaskRunner = Framework.Container.Instance.GetTaskRunner<LogReqInfo, LogSearchResultSummary>(
                "TaskRunner For SearchLog", this.ApplyInsertTaskItemPolicy);
        }

        public void Reset(ucLogList ucLogList)
        {
            m_ucLogResultList = ucLogList;
            m_Summary = null;
        }

        public void OnSearchLogRequested(LogReqInfo logReqPara)
        {
            if (m_ucLogResultList.IsDisposed)
            {
                return;
            }

            if (this.m_ucLogResultList.InvokeRequired)
            {
                try
                {
                    Action<LogReqInfo> action = new Action<LogReqInfo>(this.OnSearchLogRequested);
                    this.m_ucLogResultList.BeginInvoke(action, new object[] { logReqPara });
                }
                catch (Exception ex)
                {
                    Framework.Container.Instance.Log.Error("Invoke or BeginInvoke error: ", ex);
                    Debug.Assert(false, ex.Message);
                }
                return;
            }

            m_ucLogResultList.Clear();
            if (m_Summary != null)
            {
                m_Summary.SelectedPageNumberChanged -= new EventHandler(Summary_SelectedPageNumberChanged);
                m_Summary.SortFieldChanged -= new EventHandler(Summary_SortFieldChanged);
            }

            SwitchSearch(logReqPara);
        }

        private void SwitchSearch(LogReqInfo reqPara)
        {
            lock (m_SyncObjSwitchSearch)
            {
                bool stopSearchResult = m_SearchTaskRunner.StopAndClear();
                m_reqPara = reqPara;
                AddSearchTask(reqPara);
            }
        }

        private LogSearchResultSummary Search(LogReqInfo reqPara)
        {
            LogSearchResultSummary summary = Framework.Container.Instance.ServerLogService.SearchLogRecord(reqPara);
            return summary;
        }

        private void AddSearchTask(LogReqInfo reqPara)
        {
            Service.TaskItem<LogReqInfo, LogSearchResultSummary> item = new Service.TaskItem<LogReqInfo, LogSearchResultSummary>();
            item.FuncToRun = new Func<LogReqInfo, LogSearchResultSummary>(this.Search);
            item.Callback = new Action<LogSearchResultSummary>(this.OnSearchCompleted);
            item.Para = reqPara;

            m_SearchTaskRunner.AddTask(item);

            Framework.Container.Instance.Log.Debug("m_SearchTaskRunner.AddTask ");
        }

        private bool ApplyInsertTaskItemPolicy(Queue<TaskItem<LogReqInfo, LogSearchResultSummary>> taskItems, TaskItem<LogReqInfo, LogSearchResultSummary> taskItem)
        {
            if (taskItems != null)
            {
                taskItems.Clear();
            }
            return true ;
        }

        private void DisplayResult(LogSearchResultSummary summary)
        {
            m_Summary = summary;
            m_Summary.SelectedPageNumberChanged += new EventHandler(this.Summary_SelectedPageNumberChanged);
            m_Summary.SortFieldChanged += new EventHandler(Summary_SortFieldChanged);
            m_ucLogResultList.DisplayResult(summary);
        }

        void Summary_SortFieldChanged(object sender, EventArgs e)
        {
            // 对于车辆来说， 不能直接在UI线程上执行， 会影响 GridControl 排序
            if (!m_ucLogResultList.InvokeRequired)
            {
                Action<object, EventArgs> action = new Action<object, EventArgs>(this.Summary_SortFieldChanged);
                action.BeginInvoke(sender, e, null, null);
                return;
            }

            LogSearchResultSummary summary = sender as LogSearchResultSummary;
            if (summary != null)
            {
                OnSearchLogRequested(summary.SearchPara);
            }
        }

        private void OnSearchCompleted(LogSearchResultSummary summary)
        {
            if (summary != null)
            {
                lock (m_SyncObjSwitchSearch)
                {
                    if (!summary.SearchPara.Equals(m_reqPara))
                    {
                        return;
                    }
                }

                if(!m_ucLogResultList.IsDisposed)
                {
                    m_ucLogResultList.BeginInvoke(new Action<LogSearchResultSummary>(this.DisplayResult), new object[]{summary});
                }
            }
        }

        void Summary_SelectedPageNumberChanged(object sender, EventArgs e)
        {
            LogSearchResultSummary summary = sender as LogSearchResultSummary;

            if (summary != null)
            {
                OnSearchLogRequested(summary.SearchPara);
            }
        }

    }
}
