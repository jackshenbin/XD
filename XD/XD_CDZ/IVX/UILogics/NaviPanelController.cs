using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BOCOM.IVX.Framework;
using BOCOM.DataModel;
using DevExpress.XtraTab;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BOCOM.IVX.Views.Content;
using BOCOM.IVX.Views.ResourceTree;
using BOCOM.IVX.ViewModel;

namespace BOCOM.IVX.UILogics
{
    public class NaviPanelController : IEventAggregatorSubscriber
    {
        #region Fields

        private UIFuncItemInfo m_CurrentFuncItemInfo;

        private UIFuncItemInfo m_PreviousFuncItemInfo;

        private Control m_tabCtrlContainer;

        #endregion
        
        #region Private helper functions

        private string GetName(UIFunctionEnum funcItem)
        {
            string name = string.Empty;
            switch (funcItem)
            {
                case UIFunctionEnum.MyCaseList:
                case UIFunctionEnum.NewCase:
                case UIFunctionEnum.CurrCase:
                    name = "案件管理";
                    break;
                case UIFunctionEnum.ImportVideos:
                case UIFunctionEnum.ImportPictures:
                //case UIFunctionEnum.RunningTasks:
                //case UIFunctionEnum.FinishedTasks:
                case UIFunctionEnum.VideoTasks:
                case UIFunctionEnum.PictureTasks:
                case UIFunctionEnum.TaskStatus:
                case UIFunctionEnum.NewTask:
                    name = "任务管理";
                    break;
                case UIFunctionEnum.Search:
                case UIFunctionEnum.SearchMotionObject:
                case UIFunctionEnum.SearchFace:
                case UIFunctionEnum.SearchVehicle:
                case UIFunctionEnum.SearchByImage:
                    name = "智能检索";
                    break;
                case UIFunctionEnum.LiveVideo:
                    name = "视频播放";
                    break;
                case UIFunctionEnum.BriefVideo:
                    name = "浓缩播放";
                    break;
                case UIFunctionEnum.CameraManagement:
                case UIFunctionEnum.PlatManagement:
                case UIFunctionEnum.UserManagement:
                case UIFunctionEnum.ClusterMonitor:
                case UIFunctionEnum.VDAServerManagement:
                case UIFunctionEnum.PASServerManagement:
                case UIFunctionEnum.FtpHttpServerManagement:
                case UIFunctionEnum.MediaServerManagement:
                case UIFunctionEnum.MediaRouterManagement:
                case UIFunctionEnum.VDAResultServerManagement:
                case UIFunctionEnum.ClientRouterManagement:
                case UIFunctionEnum.LogManagement:
                case UIFunctionEnum.CaseManagement:
                    name = "系统配置";
                    break;
                case UIFunctionEnum.CaseExport:
                case UIFunctionEnum.TagExport:
                    name = "汇总导出";
                    break;
                default:
                    break;
            }

            return name;

        }
        
        private XtraUserControl CreateNaviContent(UIFunctionEnum funcItem)
        {
            XtraUserControl ctl = null;
            switch (funcItem)
            {
                case UIFunctionEnum.MyCaseList:
                case UIFunctionEnum.NewCase:
                case UIFunctionEnum.CurrCase:
                    ctl = new BOCOM.IVX.Views.ResourceTree.ucCaseTreeView();
                    break;
                case UIFunctionEnum.ImportVideos:
                case UIFunctionEnum.ImportPictures:
                //case UIFunctionEnum.RunningTasks:
                //case UIFunctionEnum.FinishedTasks:
                case UIFunctionEnum.VideoTasks:
                case UIFunctionEnum.PictureTasks:
                case UIFunctionEnum.TaskStatus:
                case UIFunctionEnum.NewTask:
                    ctl = new BOCOM.IVX.Views.ResourceTree.ucTaskTreeView();
                    break;
                case UIFunctionEnum.Search:
                case UIFunctionEnum.SearchMotionObject:
                case UIFunctionEnum.SearchFace:
                case UIFunctionEnum.SearchVehicle:
                case UIFunctionEnum.SearchByImage:
                    ctl = new ucSearchVideoTreeView();  //new BOCOM.IVX.Views.ResourceTree.ucCameraTreeView();
                    break;
                case UIFunctionEnum.LiveVideo:
                    ctl = new BOCOM.IVX.Views.ResourceTree.ucResourceTreeViewByTaskBase()
                    {
                        ViewModelbase = new VideoPictureTreeViewByTaskModelBase(TreeShowType.Video)
                    };
                    break;
                case UIFunctionEnum.BriefVideo:
                    ctl = new BOCOM.IVX.Views.ResourceTree.ucBrivfCameraTreeView();
                    break;
                case UIFunctionEnum.CameraManagement:
                case UIFunctionEnum.PlatManagement:
                case UIFunctionEnum.UserManagement:
                case UIFunctionEnum.ClusterMonitor:
                case UIFunctionEnum.PASServerManagement:
                case UIFunctionEnum.FtpHttpServerManagement:
                case UIFunctionEnum.VDAServerManagement:
                case UIFunctionEnum.MediaServerManagement:
                case UIFunctionEnum.MediaRouterManagement:
                case UIFunctionEnum.VDAResultServerManagement:
                case UIFunctionEnum.ClientRouterManagement:
                case UIFunctionEnum.LogManagement:
                case UIFunctionEnum.CaseManagement:
                    ctl = new BOCOM.IVX.Views.ResourceTree.ucSysconfigTreeView();
                    break;
                case UIFunctionEnum.CaseExport:
                case UIFunctionEnum.TagExport:
                    ctl = new BOCOM.IVX.Views.ResourceTree.ucExportTreeView();
                    break;
                default:
                    break;
            }

            return ctl;

        }
        
        private XtraUserControl GetNaviContent(UIFunctionEnum funcItem)
        {
            XtraUserControl tabTree = null;
            string name = GetName(funcItem);
            if (m_tabCtrlContainer.Controls.ContainsKey(name))
            {
                tabTree = (XtraUserControl)m_tabCtrlContainer.Controls[name];
            }
            else
            {
                tabTree = CreateNaviContent(funcItem);
                if (tabTree != null)
                {
                    tabTree.Dock = DockStyle.Fill;
                    tabTree.Name = GetName(funcItem);
                    tabTree.BorderStyle = BorderStyle.None;
                    m_tabCtrlContainer.Controls.Add(tabTree);
                }
            }
            return tabTree;
        }

        #endregion

        #region Constructors

        public NaviPanelController(Control container)
        {
            m_tabCtrlContainer = container;
            Framework.Container.Instance.EvtAggregator.GetEvent<NavigateEvent>().Subscribe(this.OnUINavigatorEvent);
            Framework.Container.Instance.EvtAggregator.GetEvent<LeaveCaseEvent>().Subscribe(OnLeaveCase);
            Framework.Container.Instance.RegisterEventSubscriber(this);
        }

        #endregion

        #region IEventAggregatorSubscriber implementations

        public void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<NavigateEvent>().Unsubscribe(this.OnUINavigatorEvent);
            Framework.Container.Instance.EvtAggregator.GetEvent<LeaveCaseEvent>().Unsubscribe(OnLeaveCase);
        }

        #endregion
        
        #region Event handlers

        public void OnUINavigatorEvent(UIFuncItemInfo funcItemInfo)
        {
            if (funcItemInfo.Function == UIFunctionEnum.Backward)
            {
                if (m_PreviousFuncItemInfo != null)
                {
                    OnUINavigatorEvent(m_PreviousFuncItemInfo);
                }
                return;
            }

            funcItemInfo = Container.Instance.NaviRecord.GetSubItem(funcItemInfo);
            Container.Instance.NaviRecord.RegisterSubItem(funcItemInfo);

            // m_tabCtrlContainer.SuspendLayout();

            XtraUserControl tabTree = GetNaviContent(funcItemInfo.Function);
            if (tabTree != null)
            {
                tabTree.BringToFront();
                m_PreviousFuncItemInfo = m_CurrentFuncItemInfo;

                if (funcItemInfo.Function == UIFunctionEnum.Backward)
                {
                    m_PreviousFuncItemInfo = null;
                    m_CurrentFuncItemInfo = m_PreviousFuncItemInfo;
                }
                else
                {
                    m_CurrentFuncItemInfo = funcItemInfo;
                }
            }
            // m_tabCtrlContainer.ResumeLayout();
        }

        private void OnLeaveCase(string caseName)
        {
            
            // 移除与Case 相关的界面
            IEventAggregatorSubscriber contentPage;
            foreach (UIFunctionEnum key in Enum.GetValues(typeof(UIFunctionEnum)))
            {
                string keyname = GetName(key);
                if (m_tabCtrlContainer.Controls.ContainsKey(keyname))
                {
                    contentPage = m_tabCtrlContainer.Controls[keyname] as IEventAggregatorSubscriber;
                    //IEventAggregatorSubscriber subscriber = contentPage.DataSource as IEventAggregatorSubscriber;
                    if (contentPage != null)
                    {
                        Framework.Container.Instance.UnRegisterEventSubscriber(contentPage);
                    }
                    m_tabCtrlContainer.Controls.RemoveByKey(keyname);
                }
            }

        }

        #endregion
    }
}
