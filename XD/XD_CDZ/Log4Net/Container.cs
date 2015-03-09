using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.IO;
using System.Reflection;

namespace MyLog4Net
{
    public  class Container
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
                    Assembly asm = Assembly.GetCallingAssembly();

                    string configFile = Path.Combine(Directory.GetParent(asm.Location).FullName , "XD.exe.config");
                    FileInfo fi = new FileInfo(configFile);

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
                    Assembly asm = Assembly.GetCallingAssembly();

                    string configFile = Path.Combine(Directory.GetParent(asm.Location).FullName, "XD.exe.config");
                    FileInfo fi = new FileInfo(configFile);

                    log4net.Config.XmlConfigurator.Configure(fi);
                    m_OperateLog = LogManager.GetLogger("OperateLog");
                }
                return m_OperateLog;
            }
        }

    }

    public static class ILogExtension
    {
        public static void DebugWithDebugView(this ILog log, object Message)
        {
            System.Diagnostics.Trace.WriteLine(Message);
            log.Debug(Message);
        }
        public static void ErrorWithDebugView(this ILog log, object Message)
        {
            System.Diagnostics.Trace.WriteLine(Message);
            log.Error(Message);
        }
    }


}
