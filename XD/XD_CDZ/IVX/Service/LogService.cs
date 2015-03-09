using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.DataModel;
using BOCOM.IVX.Protocol;
using System.Runtime.InteropServices;
using BOCOM.IVX.Framework;

namespace BOCOM.IVX.Service
{
    public class LogService
    {
        LogSearchParam m_param = new LogSearchParam();

        public LogService()
        {

            Framework.Container.Instance.IVXProtocol.LogSearchItemReceived += new Action<List<LogSearchResultInfo>>(IVXProtocol_LogSearchItemReceived);
        }

        void IVXProtocol_LogSearchItemReceived(List<LogSearchResultInfo> obj)
        {
            if (obj.Count > 0)
            {
                LogSearchResultInfo info = obj.Last();
                m_param.LogId = info.LogId;
            }
            Framework.Container.Instance.EvtAggregator.GetEvent<LogSearchReceivedEvent>().Publish(obj);
        }


        public bool LogSearch(LogSearchParam param)
        {
            m_param = param;
            return Framework.Container.Instance.IVXProtocol.LogSearch(m_param);
        }

        public bool NextPageLogSearch()
        {
            m_param.LogCount = 20;
            return Framework.Container.Instance.IVXProtocol.LogSearch(m_param);
        }


    }
}
