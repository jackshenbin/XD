using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Framework;
using System.ComponentModel;
using BOCOM.IVX.Protocol;

namespace BOCOM.IVX.ViewModel
{
    public class MainFormViewModel : INotifyPropertyChanged, IEventAggregatorSubscriber
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler CloseRequest;

        #region Fields

        private string m_Caption;

        private string m_composedCaption;

        #endregion

        #region Properties

        public string ComposedCaption
        {
            get
            {
                return m_composedCaption;
            }
            set
            {
                if (string.Compare(m_composedCaption, value, true) != 0)
                {
                    m_composedCaption = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ComposedCaption"));
                    }
                }
            }
        }

        #endregion

        #region Constructors

        public MainFormViewModel(string caption)
        {
            ComposedCaption = m_Caption = caption;
            Framework.Container.Instance.EvtAggregator.GetEvent<EnterCaseEvent>().Subscribe(OnEnteringCase);
            Framework.Container.Instance.EvtAggregator.GetEvent<CaseModifiedEvent>().Subscribe(OnCaseModified);
            Framework.Container.Instance.EvtAggregator.GetEvent<LeaveCaseEvent>().Subscribe(OnLeaveCase);
            Framework.Container.Instance.EvtAggregator.GetEvent<LogoutEvent>().Subscribe(OnLogoutRequest, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<QuitEvent>().Subscribe(OnQuitRequest,Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.RegisterEventSubscriber(this);

            Framework.Container.Instance.IVXProtocol.EventDisConnectd += new DelegateDisConnectd(IVXProtocol_EventDisConnectd);
        }

        #endregion

        public void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<LeaveCaseEvent>().Unsubscribe(OnEnteringCase);
            Framework.Container.Instance.EvtAggregator.GetEvent<CaseModifiedEvent>().Unsubscribe(OnCaseModified);
            Framework.Container.Instance.EvtAggregator.GetEvent<EnterCaseEvent>().Unsubscribe(OnLeaveCase);
            Framework.Container.Instance.EvtAggregator.GetEvent<LogoutEvent>().Unsubscribe(OnLogoutRequest);
            Framework.Container.Instance.EvtAggregator.GetEvent<QuitEvent>().Unsubscribe(OnQuitRequest);

            Framework.Container.Instance.IVXProtocol.EventDisConnectd -= IVXProtocol_EventDisConnectd;
        }

        #region Event handlers

        private void OnEnteringCase(string caseName)
        {
            ComposedCaption = string.Format("{0} - 当前案件: {1}", m_Caption, caseName);
        }
        private void OnCaseModified(DataModel.CaseInfo caseInfo)
        {
            if (Framework.Container.Instance.EnteredCase != null && caseInfo.CaseID == Framework.Container.Instance.EnteredCase.CaseID)
            {
                ComposedCaption = string.Format("{0} - 当前案件: {1}", m_Caption, caseInfo.CaseName);
            }
        }
        private void OnLeaveCase(string caseName)
        {
            ComposedCaption =  m_Caption;
        }

        private void OnLogoutRequest(bool isLogoutByUser)
        {
            if (isLogoutByUser && !CheckUpload())
                return;

            Framework.Container.Instance.Cleanup();
            Framework.Environment.IsBeingLogout = true;
            Framework.Environment.IsCloseMainForm = true ;
            

            if (CloseRequest != null)
            {
                CloseRequest(this, EventArgs.Empty);
            }
        }
        private bool CheckUpload()
        {
            if (Framework.Container.Instance.EnteredCase != null)
            {
                try
                {
                    List<uint> list = Framework.Container.Instance.TaskManagerService.GetLocalUploadTaskUnit();
                    if (list.Count > 0)
                    {
                        BOCOM.DataModel.TaskUnitInfo info = Framework.Container.Instance.TaskManagerService.GetTaskUnitById(list[0]);
                        string msg = string.Format("【{0}】等 {1} 个文件正在导入，现在退出会导致任务单元导入失败，{2}是否确定要退出？",
                            info.TaskUnitName, list.Count.ToString(), System.Environment.NewLine);

                        if (Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME, System.Windows.Forms.MessageBoxButtons.YesNo)
                            == System.Windows.Forms.DialogResult.No)
                            return false;

                    }
                }
                catch (SDKCallException) { }
            }
            return true;
        }

        private void OnQuitRequest(string sender)
        {
            if (!CheckUpload())
                return;

            Framework.Container.Instance.Cleanup();
            Framework.Environment.IsBeingLogout = false ;
            Framework.Environment.IsCloseMainForm = true ;
            
            if (CloseRequest != null)
            {
                CloseRequest(this, EventArgs.Empty);
            }
        }

        void IVXProtocol_EventDisConnectd(uint userData)
        {
            if (Framework.Container.Instance.MainControl.InvokeRequired)
            {
                Framework.Container.Instance.MainControl.BeginInvoke(
                    new Action<uint>(this.IVXProtocol_EventDisConnectd), new object[] { userData });
                return;
            }
            Framework.Container.Instance.InteractionService.ShowMessageBox("与服务器断开连接，请重新登录。", Framework.Environment.PROGRAM_NAME);
            // new System.Threading.Thread(LogOutEvent).Start();
            Framework.Container.Instance.EvtAggregator.GetEvent<LogoutEvent>().Publish(false );
        }

        #endregion
    }
}
