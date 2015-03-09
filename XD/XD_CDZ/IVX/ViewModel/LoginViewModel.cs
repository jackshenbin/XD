using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Framework;
using System.ComponentModel;
using BOCOM.IVX.UILogics;
using System.Windows.Forms;
using BOCOM.IVX.Common;

namespace BOCOM.IVX.ViewModel
{
    public class LoginViewModel : ViewModelBase, INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        #region Fields

        private string m_ServerIP;

        private string m_UserName;
        
        private string m_Password;

        private ICommand m_loginCommand;

        private LoginLogic m_Logic;

        #endregion

        #region Properties

        public string ServerIP
        {
            get { return m_ServerIP; }
            set { m_ServerIP = value; }
        }

        public string UserName
        {
            get { return m_UserName; }
            set { m_UserName = value; }
        }

        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }


        public bool RememberPassword 
        {
            get
            {
                return Framework.Environment.SavePassword;
            }
            set
            {
                Framework.Environment.SavePassword = value;
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                return m_loginCommand??(m_loginCommand = new LoginCommand(this));
            }
        }

        #endregion

        #region Constructors

        public LoginViewModel()
        {
            ServerIP = Framework.Environment.ServerIP;
            UserName = Framework.Environment.UserName;
             Password = Framework.Environment.Password;
            //Password = "admin";
            RememberPassword = Framework.Environment.SavePassword;

            m_Logic = new LoginLogic(this);

        }

        public LoginViewModel(string serverIP, string userName, string password)
        {
            m_Logic = new LoginLogic(this);
        }

        #endregion

        private bool Validate()
        {
            bool bRet = true;

            if (!TextUtil.ValidateIPAddress(ServerIP))
            {
                bRet = false;
                Framework.Container.Instance.InteractionService.ShowMessageBox("服务器地址不正确", Framework.Environment.PROGRAM_NAME,
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (string.IsNullOrEmpty(UserName))
            {
                bRet = false;
                Framework.Container.Instance.InteractionService.ShowMessageBox("用户名不能为空", Framework.Environment.PROGRAM_NAME,
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (string.IsNullOrEmpty(Password))
            {
                bRet = false;
                Framework.Container.Instance.InteractionService.ShowMessageBox("密码不能为空", Framework.Environment.PROGRAM_NAME,
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return bRet;
        }

        #region Public helper functions

        public void Login()
        {
            bool bRet = Validate();
            if (!bRet)
            {
                return;
            }
                        
            string msg = "原因未知";
            try
            {
                bRet = Framework.Container.Instance.AuthenticationService.Login(UserName, Password, ServerIP, 60012, 10, 0);
            }
            catch (BOCOM.IVX.Protocol.SDKCallException ex)
            {
                bRet = false;
                if (!string.IsNullOrEmpty(ex.Message))
                {
                    msg = ex.Message;
                }
            }

            if (!bRet)
            {
                msg = string.Format("登录失败，{0}", msg);
                Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

    }

    #region Other classes

    public class LoginCommand : ICommand
    {
        private LoginViewModel m_VM;

        public LoginCommand(LoginViewModel vm)
        {
            m_VM = vm;
        }

        public void Execute(object sender)
        {
            Framework.Container.Instance.AuthenticationService.Login(m_VM.UserName,
                m_VM.Password, m_VM.ServerIP, 60012, 30, 0);
        }
    }

    #endregion
}


