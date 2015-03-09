using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.ViewModel;
using BOCOM.IVX.Framework;
using BOCOM.DataModel;
using System.Windows.Forms;

namespace BOCOM.IVX.UILogics
{
    /// <summary>
    /// 负责调用登录相关Service 方法，
    /// 并且跳转界面
    /// </summary>
    public class LoginLogic : IEventAggregatorSubscriber
    {
        #region Fields

        private LoginViewModel m_VM;

        #endregion

        #region Constructors

        public LoginLogic(LoginViewModel viewModel)
        {
            m_VM = viewModel;
            Framework.Container.Instance.EvtAggregator.GetEvent<UserLoginEvent>().Subscribe(OnUserLoggedin);
            Framework.Container.Instance.RegisterEventSubscriber(this);
        }

        #endregion

        #region Public helper functions

        public void Login()
        {
            uint timeout = 10;
            bool bRet;

            try
            {
                bRet = Framework.Container.Instance.AuthenticationService.Login(
                m_VM.UserName,
                m_VM.Password, m_VM.ServerIP, 60012, timeout, 0);
            }
            catch (TimeoutException ex)
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox("登录超时", Framework.Environment.PROGRAM_NAME,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region Event handlers

        private void OnUserLoggedin(LoginToken token)
        {
            m_VM.RaiseCommandExecutedEvent(new CommandExecuteResultEventArgs(true));
            //Framework.Environment.SaveConfig();
        }

        #endregion

        #region IEventAggregatorSubscriber implementations

        public void UnSubscribe()
        {
            System.Diagnostics.Trace.WriteLine("LoginLogic.UnSubscribe");

            Framework.Container.Instance.EvtAggregator.GetEvent<UserLoginEvent>().Unsubscribe(OnUserLoggedin);
        }

        #endregion
    }
}
