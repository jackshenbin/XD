using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Protocol.Model;
using BOCOM.IVX.Framework;
using BOCOM.DataModel;
using System.Data;

namespace BOCOM.IVX.ViewModel
{
    public class LogListViewModel : ViewModelBase,IEventAggregatorSubscriber
    {

        DataTable m_LogList = null;

        public DataTable LogList
        {
            get
            {
                if (m_LogList == null)
                {
                    m_LogList = new DataTable("LogList");
                    DataColumn LogId = m_LogList.Columns.Add("LogId", typeof(UInt32));
                    //m_LogList.PrimaryKey = new DataColumn[] { LogId };
                    m_LogList.Columns.Add("LogTime");
                    m_LogList.Columns.Add("LogType");
                    m_LogList.Columns.Add("LogDetail");
                    m_LogList.Columns.Add("Description");
                    m_LogList.Columns.Add("Level");
                    m_LogList.Columns.Add("OptName");
                    m_LogList.Columns.Add("LogSearchResultInfo", typeof(LogSearchResultInfo));

                }
                return m_LogList;
            }
            set { m_LogList = value; }
        }

        public LogListViewModel()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<LogSearchReceivedEvent>().Subscribe(OnLogSearchReceived, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<LogSearchBeginingEvent>().Subscribe(OnLogSearchBegining,Microsoft.Practices.Prism.Events.ThreadOption.PublisherThread);
            Framework.Container.Instance.RegisterEventSubscriber(this);
        }

        void OnLogSearchReceived(List<LogSearchResultInfo> info)
        {
            
            info.ForEach(item => {
                m_LogList.Rows.Add(new object[] {item.LogId, item.LogTime, (int)item.LogType, (int)item.LogDetail, item.Description, (int)item.Level, item.OptName,item });
            });
        }
        void OnLogSearchBegining(string obj)
        {
            Clear();
        }
        public void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<LogSearchReceivedEvent>().Unsubscribe(OnLogSearchReceived);
            Framework.Container.Instance.EvtAggregator.GetEvent<LogSearchBeginingEvent>().Unsubscribe(OnLogSearchBegining);
        }
        public void NextPageLogSearch()
        {
            try
            {
                Framework.Container.Instance.ServerLogService.NextPageLogSearch();
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "日志下一页");
            }
        }
        public void Clear()
        {
            m_LogList.Clear();
        }
    }
}
