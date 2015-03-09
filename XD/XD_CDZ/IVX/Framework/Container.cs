using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// using BOCOM.IVX.Logic;
using Microsoft.Practices.Prism.Events;
using log4net;
using BOCOM.IVX.Interfaces;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using BOCOM.IVX.Customization;
using System.IO;
using System.Windows.Forms;
using BOCOM.DataModel;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Common;

namespace BOCOM.IVX.Framework
{
    public class Container
    {
        #region Fields

        private static Container s_Instance = null;

        // private VASearchService m_VASearchLogic;
        private EventAggregator m_evtAggregator = null;

        private CacheManager m_cacheMgr = null;

        private CompositionContainer m_MEFContainer = null;

        private BaseStyle m_CusomizedStyle = null;

        private VVMDataBindings m_vvmDataBindings = null;

        private List<IEventAggregatorSubscriber> m_eventSubscribers = null;


        private Common.FtpWeb m_ftpService = null;
        #endregion

        #region Properties

        public CaseInfo EnteredCase { get; set; }

        //public VASearchService VASearchService
        //{
        //    get
        //    {
        //        if (m_VASearchLogic == null)
        //        {
        //            m_VASearchLogic = new VASearchService();
        //        }
        //        return m_VASearchLogic;
        //    }
        //}

        public EventAggregator EvtAggregator
        {
            get
            {
                return this.m_evtAggregator;
            }
        }

        //public ILog Log
        //{
        //    get
        //    {
        //        if (m_Log == null)
        //        {
        //            string configFile = Path.Combine(Environment.CurrentDirectory, "IVX.exe.config");
        //            FileInfo fi = new FileInfo(configFile);
        //            // log4net.Repository.ILoggerRepository repository = LogManager.CreateRepository("ICAS");
        //            // log4net.Config.XmlConfigurator.Configure(repository, fi);
        //            log4net.Config.XmlConfigurator.Configure(fi);
        //            m_Log = LogManager.GetLogger("DebuggingLog");
        //        }
        //        return m_Log;
        //    }
        //}

        //public ILog OperateLog
        //{
        //    get
        //    {
        //        if (m_OperateLog == null)
        //        {
        //            log4net.Config.XmlConfigurator.Configure();
        //            m_OperateLog = LogManager.GetLogger("OperateLog");
        //        }
        //        return m_OperateLog;
        //    }
        //}

        public CacheManager CacheMgr
        {
            get
            {
                return m_cacheMgr;
            }
        }

        //public VAResultListController ResultListController
        //{
        //    get;
        //    set;
        //}



        public BaseStyle CustomizedStyle
        {
            get
            {
                if (m_CusomizedStyle == null)
                {
                    if (Framework.Environment.PRODUCT_TYPE == Framework.Environment.E_PRODUCT_TYPE.SH_PRODUCT)
                    {
                        m_CusomizedStyle = new ShanghaiPoliceStyle();
                    }
                    else if (Framework.Environment.PRODUCT_TYPE == Environment.E_PRODUCT_TYPE.NO_LOG)
                    {
                        m_CusomizedStyle = new NeuterStyle();
                    }
                    else
                    {
                        m_CusomizedStyle = new BaseStyle();
                    }
                }
                return m_CusomizedStyle;
            }
        }

        public CompositionContainer MEFContainer
        {
            get
            {
                return m_MEFContainer;
            }
        }
        [Import(typeof(IInteractionService))]
        public IInteractionService InteractionService { get; set; }

        public Control MainControl
        {
            get;
            set;
        }
        
        public VVMDataBindings VVMDataBindings
        {
            get
            {
                if (m_vvmDataBindings == null)
                {
                    m_vvmDataBindings = new VVMDataBindings();
                }
                return m_vvmDataBindings;
            }
        }






        public BOCOM.IVX.Protocol.IVXProtocol IVXProtocol { get; set; }



        public TaskUnitInfo[] SelectedTaskUnitsForSearch
        {
            get;
            set;
        }

        //public SearchResultViewModel SearchResultViewModel
        //{
        //    get
        //    {
        //        if (m_SearchResultViewModel == null)
        //        {
        //            m_SearchResultViewModel = new SearchResultViewModel();
        //        }
        //        return m_SearchResultViewModel;
        //    }
        //}


        public Common.FtpWeb FtpService
        {
            get 
            {
                if (m_ftpService == null)
                {
                    string ftproot ="";//= Framework.Container.Instance.VDAConfigService.GetFtpFileServiceURL();
                    string ip, port, user, pass, path;
                    Common.FtpWeb.GetFtpUrlInfo(ftproot, out ip, out port, out user, out pass, out path);

                    m_ftpService = new Common.FtpWeb(ip+":"+port, path, user, pass);
                    //m_ftpService = new Common.FtpWeb("192.168.42.6", "素材库/3.0测试视频", "public", "public123$");
                }
                return m_ftpService;
            }

        }

        #endregion

        #region Constructors

        private Container()
        {
            this.m_evtAggregator = new EventAggregator();
            m_eventSubscribers = new List<IEventAggregatorSubscriber>();

            m_cacheMgr = new CacheManager();

            var cataLog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            m_MEFContainer = new CompositionContainer(cataLog);
            
            try
            {
                m_MEFContainer.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }

            //VideoSearchService = new VideoSearchService();
        }

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

        #endregion

        #region Private helper functions
        
        private void StopVideoPlay()
        {
            try
            {
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "清理播放相关资源");
            }
        }

        #endregion

        #region Public helper functions



        public bool StopAllTaskRunner()
        {
            bool result = true;

            
            return result;
        }
        
        public void RegisterEventSubscriber(IEventAggregatorSubscriber subscriber)
        {
            if (!m_eventSubscribers.Contains(subscriber))
            {
                m_eventSubscribers.Add(subscriber);
            }
        }

        public void UnRegisterEventSubscriber(IEventAggregatorSubscriber subscriber)
        {
            if (m_eventSubscribers.Contains(subscriber))
            {
                subscriber.UnSubscribe();
                m_eventSubscribers.Remove(subscriber);
            }
        }

        public void UnSubscribeEvents()
        {
            if (m_eventSubscribers.Count > 0)
            {
                foreach (IEventAggregatorSubscriber subscriber in m_eventSubscribers)
                {
                    subscriber.UnSubscribe();
                }
                m_eventSubscribers.Clear();
            }
        }

        public void Cleanup()
        {
            try
            {
                MyLog4Net.Container.Instance.Log.Info("Container.Cleanup ...");
                SelectedTaskUnitsForSearch = null;
                
                StopAllTaskRunner();
                MyLog4Net.Container.Instance.Log.Info("Container.Cleanup: UnSubscribeEvents ... ");
                UnSubscribeEvents();
                
                if (m_vvmDataBindings != null)
                    m_vvmDataBindings.RemoveBindings();
                
                EnteredCase = null;
                Framework.Environment.Reset();
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "清理资源", false);
            }
            MyLog4Net.Container.Instance.Log.Info("Container.Cleanup ...");
        }

        public void LeaveCase()
        {
            StopVideoPlay();
            Framework.Container.Instance.EvtAggregator.GetEvent<PreLeaveCaseEvent>().Publish("");
            Framework.Container.Instance.IVXProtocol.LeaveCase(Framework.Container.Instance.EnteredCase.CaseID);
            Framework.Container.Instance.EnteredCase = null;
            Framework.Container.Instance.EvtAggregator.GetEvent<LeaveCaseEvent>().Publish("");

        }
        
        #endregion

    }
}
