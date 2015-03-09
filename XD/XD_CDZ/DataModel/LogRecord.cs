using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.DataModel
{
    public class LogRecord
    {
        private LogResInfo m_TLogInfo;

        // 1系统日志 2操作日志 3管理日志
        private static readonly string[] LOGCATS = new string[] { "系统日志", "操作日志", "管理日志" };
        // 1普通 2警告 3 错误
        private static readonly string[] LOGLEVELS = new string[] { "普通", "警告", "错误" };

        public string Category
        {
            get
            {
                string sRet = string.Empty;
                int index = m_TLogInfo.lbiOfLog.iTypeOfLog - 1;

                if(index >= 0 && index < LOGCATS.Length)
                {
                    sRet = LOGCATS[index];
                }

                return sRet;
            }
        }

        public string DetailCat
        {
            get
            {
                string sRet = string.Empty;
                LogDetailCat detailCat = null;// Framework.Container.Instance.ServerLogService.GetLogDetail(m_TLogInfo.lbiOfLog.iDetailOfLog);
                if (detailCat != null)
                {
                    sRet = detailCat.Name;
                }
                return sRet;
            }
        }

        public string Level
        {
            get
            {
                string sRet = string.Empty;
                int index = m_TLogInfo.lbiOfLog.iLevelOfLog - 1;

                if (index >= 0 && index < LOGLEVELS.Length)
                {
                    sRet = LOGLEVELS[index];
                }

                return sRet;
            }
        }

        public string DateTime
        {
            get
            {
                return m_TLogInfo.szLogTime;
            }
        }

        public string Description
        {
            get
            {
                return m_TLogInfo.szLogDscp;
            }
        }

        public string Operator
        {
            get
            {
                return m_TLogInfo.lbiOfLog.szUserName;
            }
        }

        public LogRecord(LogResInfo tLogInfo)
        {
            m_TLogInfo = tLogInfo;
        }
    }
}
