using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModel
{
    public class Container
    {
        private static Container s_Instance = null;
        private ILog m_Log = null;
        private ILog m_OperateLog = null;

        public static Container Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = new Container();
                }
                return s_Instance;
            }
        }
        public ILog Log
        {
            get
            {
                if (m_Log == null)
                {
                    string configFile = Path.Combine(Environment.CurrentDirectory, "ICAS.exe.config");
                    FileInfo fi = new FileInfo(configFile);
                    // log4net.Repository.ILoggerRepository repository = LogManager.CreateRepository("ICAS");
                    // log4net.Config.XmlConfigurator.Configure(repository, fi);
                    log4net.Config.XmlConfigurator.Configure(fi);
                    m_Log = LogManager.GetLogger("DebuggingLog");
                }
                return m_Log;
            }
        }

        public ILog OperateLog
        {
            get
            {
                if (m_OperateLog == null)
                {
                    log4net.Config.XmlConfigurator.Configure();
                    m_OperateLog = LogManager.GetLogger("OperateLog");
                }
                return m_OperateLog;
            }
        }

    }
}
