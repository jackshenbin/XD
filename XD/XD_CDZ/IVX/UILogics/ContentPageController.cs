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
using BOCOM.IVX.ViewModel;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Protocol.Model;


namespace BOCOM.IVX.UILogics
{
    public class ContentPageController : IEventAggregatorSubscriber
    {
        #region Fields

        private PanelControl m_tabCtrlContainer;

        private FormDownloadInfoList m_FormDownloadInfoList;

        private SplitContainerControl m_splitContainer;

        private LabelControl m_captionLabel;

        private ucContentBase m_CurrentContentPage;

        private UIFuncItemInfo m_CurrentFuncItemInfo;

        private UIFuncItemInfo m_PreviousFuncItemInfo;

        private Dictionary<UIFunctionEnum, ucContentBase> m_DTFunction2TabPage;

        private List<UIFunctionEnum> m_UIFuncsDependonCase;

        #endregion
        
        #region Constructors

        public ContentPageController(PanelControl tabControl, LabelControl captionLabel, SplitContainerControl splitContainer)
        {
            this.m_tabCtrlContainer = tabControl;
            this.m_captionLabel = captionLabel;
            this.m_splitContainer = splitContainer;

            m_UIFuncsDependonCase = new List<UIFunctionEnum>();

            m_DTFunction2TabPage = new Dictionary<UIFunctionEnum, ucContentBase>();
            Framework.Container.Instance.EvtAggregator.GetEvent<NavigateEvent>().Subscribe(this.OnUINavigatorEvent);
            Framework.Container.Instance.EvtAggregator.GetEvent<ShowDialogEvent>().Subscribe(this.OnShowDialogEvent);

            Framework.Container.Instance.EvtAggregator.GetEvent<LeaveCaseEvent>().Subscribe(OnLeaveCase);

            Framework.Container.Instance.RegisterEventSubscriber(this);
        }

        #endregion

        #region Private helper functions

        private ucContentBase CreateContentPage(UIFuncItemInfo itemInfo)
        {
            UIFunctionEnum funcItem = itemInfo.Function;
            ucContentBase ctrl = null;
            Form dlg;
            switch (funcItem)
            {
                case UIFunctionEnum.MyCaseList:
                    ctrl = new ucMyCaseList();
                    break;
                case UIFunctionEnum.CurrCase:
                    ctrl = new ucCurrentCase(itemInfo.Subject as CaseInfo);
                    break;
                case UIFunctionEnum.CaseExport:
                    ctrl = new ucCaseExport();
                    break;
                case UIFunctionEnum.NewCase:
                    dlg = new FormNewCase(new CaseInfo() { CaseHappenTime = DateTime.Now });
                    dlg.ShowDialog();
                    break;
                case UIFunctionEnum.ModifyCase:
                    dlg = new FormNewCase(itemInfo.Subject as CaseInfo, true);
                    dlg.ShowDialog();
                    break;
                case UIFunctionEnum.ImportVideos:
                    ctrl = new ucVideoTaskStatus();
                    break;
                case UIFunctionEnum.ImportPictures:
                    ctrl = new ucPictureTaskStatus();
                    break;
                case UIFunctionEnum.Search:
                //case UIFunctionEnum.SearchMotionObject:
                //case UIFunctionEnum.SearchFace:
                //case UIFunctionEnum.SearchVehicle:
                //case UIFunctionEnum.SearchByImage:
                    ctrl = new ucSearch();
                    break;
                case UIFunctionEnum.LiveVideo:
                    ctrl = new ucPlayVideo();
                    break;
                case UIFunctionEnum.BriefVideo:
                    ctrl = new ucPlayBriefVideoNew();
                    break;
                //case UIFunctionEnum.RunningTasks:
                //    ctrl = new ucRunningTasks();
                //    break;
                //case UIFunctionEnum.FinishedTasks:
                //    ctrl = new ucFinishedTasks();
                //    break;
                case UIFunctionEnum.VideoTasks:
                    ctrl = new ucVideoTaskStatus();
                    break;
                case UIFunctionEnum.PictureTasks:
                    ctrl = new ucPictureTaskStatus();
                    break;
                case UIFunctionEnum.TaskStatus:
                    ctrl = new ucTasksStatus();
                    break;
                case UIFunctionEnum.NewTask:
                    //ctrl = new ucCreateTaskWizard();
                    dlg = new FormCreateTaskWizard();
                    dlg.StartPosition = FormStartPosition.CenterParent;
                    dlg.ShowDialog();
                    break;
                case UIFunctionEnum.NewTaskUnit:
                    // ctrl = new ucCreateTaskWizard(itemInfo.Subject as TaskInfo);
                    dlg = new FormCreateTaskWizard(itemInfo.Subject as TaskInfo);
                    dlg.StartPosition = FormStartPosition.CenterParent;
                    dlg.ShowDialog();
                    break;
                case UIFunctionEnum.CameraManagement:
                    ctrl = new ucCameraManagement();
                    break;
                case UIFunctionEnum.PlatManagement:
                    ctrl = new ucVideoSupplierDeviceManagement();
                    break;
                case UIFunctionEnum.UserManagement:
                    ctrl = new ucUserManagement();
                    break;
                case UIFunctionEnum.ClusterMonitor:
                    ctrl = new ucClusterMonitor();
                    break;
                case UIFunctionEnum.VDAServerManagement:
                    ctrl = new ucVDAServerManagement();
                    break;
                case UIFunctionEnum.PASServerManagement:
                    ctrl = new ucPASServerManagement();
                    break;
                case UIFunctionEnum.FtpHttpServerManagement:
                    ctrl = new ucFtpHttpServerManagement();
                    break;
                case UIFunctionEnum.MediaServerManagement:
                    ctrl = new ucMediaServerManagement();
                    break;
                case UIFunctionEnum.MediaRouterManagement:
                    ctrl = new ucMediaRouterManagement();
                    break;
                case UIFunctionEnum.VDAResultServerManagement:
                    ctrl = new ucVDAResultServerManagement();
                    break;
                case UIFunctionEnum.ClientRouterManagement:
                    ctrl = new ucClientRouterManagement();
                    break;
                case UIFunctionEnum.LogManagement:
                    ctrl = new ucLogManagement();
                    break;
                case UIFunctionEnum.CaseManagement:
                    ctrl = new ucCaseManagement();
                    break;
                case UIFunctionEnum.TagExport:
                    ctrl = new ucTagExport();
                    break;
               default:
                    break;
            }

            return ctrl;
        }

        private ucContentBase GetContentPage(UIFuncItemInfo itemInfo)
        {
            ucContentBase tabPage = null;
            UIFuncItemInfo itemInfoPar = itemInfo;

            if (itemInfo.Parent != null && itemInfo.Parent == UIFuncItemInfo.SEARCH)
            {
                itemInfoPar = itemInfo.Parent;
            }

            UIFunctionEnum funcItem = itemInfoPar.Function;

            if (m_DTFunction2TabPage.ContainsKey(funcItem))
            {
                tabPage = m_DTFunction2TabPage[funcItem];
            }
            else
            {
                tabPage = CreateContentPage(itemInfoPar);
                if (tabPage != null)
                {
                    tabPage.Dock = DockStyle.Fill;
                    tabPage.BorderStyle = BorderStyle.None;
                    AddPage(itemInfoPar, tabPage);
                }
            }

            if (itemInfo == UIFuncItemInfo.SEARCHMOTIONOBJECT)
            {
                Framework.Container.Instance.EvtAggregator.GetEvent<SearchVideoFilerChangedEvent>().Publish(SearchResourceResultType.Normal);
            }
            else if (itemInfo == UIFuncItemInfo.SEARCHFACE)
            {
                Framework.Container.Instance.EvtAggregator.GetEvent<SearchVideoFilerChangedEvent>().Publish(SearchResourceResultType.Face);
            }
            else if (itemInfo == UIFuncItemInfo.SEARCHVEHICLE)
            {
                Framework.Container.Instance.EvtAggregator.GetEvent<SearchVideoFilerChangedEvent>().Publish(SearchResourceResultType.Vehicle);
            }
            else if (itemInfo == UIFuncItemInfo.SEARCHBYIMAGE)
            {
                Framework.Container.Instance.EvtAggregator.GetEvent<SearchVideoFilerChangedEvent>().Publish(SearchResourceResultType.NoUse);
            }

            return tabPage;
        }
        
        private void AddPage(UIFuncItemInfo itemInfo, ucContentBase tabPage)
        {
            m_tabCtrlContainer.Controls.Add(tabPage);
            m_DTFunction2TabPage.Add(itemInfo.Function, tabPage);

            if (itemInfo.DependsOnCase)
            {
                m_UIFuncsDependonCase.Add(itemInfo.Function);
            }
        }

        private void RemovePage(UIFuncItemInfo itemInfo, ucContentBase tabPage)
        {
            tabPage.Controls.Remove(tabPage);
            m_DTFunction2TabPage.Remove(itemInfo.Function);
            tabPage.UnregisterEventHandlers();
            
            if (itemInfo.DependsOnCase)
            {
                m_UIFuncsDependonCase.Remove(itemInfo.Function);
            }
        }


        #endregion
        
        #region IEventAggregatorSubscriber implementations

        public void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<NavigateEvent>().Unsubscribe(this.OnUINavigatorEvent);
            Framework.Container.Instance.EvtAggregator.GetEvent<ShowDialogEvent>().Unsubscribe(this.OnShowDialogEvent);
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
            m_tabCtrlContainer.SuspendLayout();
            m_splitContainer.SuspendLayout();

            if (funcItemInfo.Function == UIFunctionEnum.ShowDownloadListForm)
            {
                if (m_FormDownloadInfoList == null)
                {
                    System.Diagnostics.Trace.WriteLine("m_FormDownloadInfoList = new FormDownloadInfoList()");
                    m_FormDownloadInfoList = new FormDownloadInfoList();
                    m_FormDownloadInfoList.StartPosition = FormStartPosition.CenterParent;
                    m_FormDownloadInfoList.FormClosed += new FormClosedEventHandler(FormDownloadInfoList_FormClosed);
                    m_FormDownloadInfoList.Show(Framework.Container.Instance.MainControl);
                }
                else
                {
                    System.Diagnostics.Trace.WriteLine("m_FormDownloadInfoList.Show()");

                    m_FormDownloadInfoList.Show();
                }
            }

            funcItemInfo = Container.Instance.NaviRecord.GetSubItem(funcItemInfo);
            int oldSplitPosition = m_splitContainer.SplitterPosition;
            
            // m_tabCtrlContainer.SuspendLayout();

            ucContentBase tabPage = GetContentPage(funcItemInfo);
            
            if (tabPage != null)
            {
                this.m_captionLabel.Text = funcItemInfo.Caption;
                this.m_captionLabel.Visible = tabPage.ShowCaption;

                tabPage.BringToFront();
                
                if (m_CurrentContentPage != null)
                {
                    if (!m_CurrentContentPage.RetainWhenDisppear)
                    {
                        RemovePage(m_CurrentFuncItemInfo, m_CurrentContentPage);
                        
                        if (m_CurrentContentPage.ViewModel != null)
                        {
                            Framework.Container.Instance.VVMDataBindings.RemoveBindings(m_CurrentContentPage.ViewModel);
                        }
                    }
                    else
                    {
                        m_PreviousFuncItemInfo = m_CurrentFuncItemInfo;
                    }
                }

                m_CurrentContentPage = tabPage;

                if (funcItemInfo.Function == UIFunctionEnum.Backward)
                {
                    m_PreviousFuncItemInfo = null;
                    m_CurrentFuncItemInfo = m_PreviousFuncItemInfo;
                }
                else
                {
                    m_CurrentFuncItemInfo = funcItemInfo;
                }

                int splitPosition;
                Container.Instance.NaviRecord.RegisterSubItem(funcItemInfo, oldSplitPosition, out splitPosition);

                if (splitPosition > -1)
                {
                    m_splitContainer.SplitterPosition = splitPosition;
                    object o = AppDomain.CurrentDomain.GetData("OCXContainer");
                    if (o == null)
                    {
                        m_splitContainer.PanelVisibility = (splitPosition == 0) ? SplitPanelVisibility.Panel2 : SplitPanelVisibility.Both;
                    }
                }
            }
            m_tabCtrlContainer.ResumeLayout();
            m_splitContainer.ResumeLayout();
        }

        public void OnShowDialogEvent(UIFuncItemInfo itemInfo)
        {
            UIFunctionEnum funcItem = itemInfo.Function;
            ucContentBase ctrl = null;
            Form dlg;
            switch (funcItem)
            {
                case UIFunctionEnum.NewCase:
                    dlg = new FormNewCase(new CaseInfo() { CaseHappenTime = DateTime.Now });
                    dlg.ShowDialog();
                    break;
                case UIFunctionEnum.ModifyCase:
                    dlg = new FormNewCase(itemInfo.Subject as CaseInfo, true);
                    dlg.ShowDialog();
                    break;
            }
        }

        private void OnLeaveCase(string caseName)
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<NavigateEvent>().Publish(UIFuncItemInfo.MYCASELIST);

            // 移除与Case 相关的界面
            ucContentBase contentPage;
            foreach (UIFunctionEnum key in m_UIFuncsDependonCase)
            {
                contentPage = m_DTFunction2TabPage[key];
                m_tabCtrlContainer.Controls.Remove(contentPage);
                m_DTFunction2TabPage.Remove(key);

                IEventAggregatorSubscriber subscriber = contentPage.DataSource as IEventAggregatorSubscriber;
                if (subscriber != null)
                {
                    Framework.Container.Instance.UnRegisterEventSubscriber(subscriber);
                }
            }

            m_UIFuncsDependonCase.Clear();
        }

        void FormDownloadInfoList_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_FormDownloadInfoList = null;
        }

        #endregion

    }
}
