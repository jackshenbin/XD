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
using System.IO;
using System.Windows.Forms;
using BOCOM.DataModel;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Common;
using BOCOM.IVX.Service;

namespace BOCOM.IVX.Framework
{
    public class Container
    {
        #region Fields

        private static Container s_Instance = null;

        // private VASearchService m_VASearchLogic;
        private EventAggregator m_evtAggregator = null;


        private CompositionContainer m_MEFContainer = null;


        private VVMDataBindings m_vvmDataBindings = null;

        private List<IEventAggregatorSubscriber> m_eventSubscribers = null;


        private  DevStateService m_DevStateService;

        #endregion

        #region Properties
        public  DevStateService DevStateService
        {
            get
            {
                if (m_DevStateService == null)
                {
                    m_DevStateService = new DevStateService();
                }
                return m_DevStateService;
            }
        }


        public EventAggregator EvtAggregator
        {
            get
            {
                return this.m_evtAggregator;
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


        #endregion

        #region Constructors

        private Container()
        {
            this.m_evtAggregator = new EventAggregator();
            m_eventSubscribers = new List<IEventAggregatorSubscriber>();


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
                
                StopAllTaskRunner();
                MyLog4Net.Container.Instance.Log.Info("Container.Cleanup: UnSubscribeEvents ... ");
                UnSubscribeEvents();
                
                if (m_vvmDataBindings != null)
                    m_vvmDataBindings.RemoveBindings();
                DevStateService.Stop();
                m_DevStateService = null;

                Framework.Environment.Reset();
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "清理资源", false);
            }
            MyLog4Net.Container.Instance.Log.Info("Container.Cleanup ...");
        }

        
        #endregion

    }
}
