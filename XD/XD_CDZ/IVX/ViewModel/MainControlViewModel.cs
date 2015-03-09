using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Framework;
using BOCOM.DataModel;
using System.Windows.Forms;
using BOCOM.IVX.Protocol;

namespace BOCOM.IVX.ViewModel
{
    public class MainControlViewModel
    {
        #region Fields

        private ICommand m_logoutCommand;
        private ICommand m_quitCommand;
        private ICommand m_showUserProfileCommand;

        #endregion

        #region Properties

        //public ICommand LogoutCommand
        //{
        //    get
        //    {
        //        return m_logoutCommand ??(m_logoutCommand = new LogoutCommand(this));
        //    }
        //}
        
        //public ICommand QuitCommand
        //{
        //    get
        //    {
        //        return m_quitCommand ??(m_quitCommand = new QuitCommand(this));
        //    }
        //}

        //public ICommand ShowUserProfileCommand
        //{
        //    get
        //    {
        //        return m_showUserProfileCommand ?? (m_showUserProfileCommand = new ShowUserProfileCommand(this)); ;
        //    }
        //}

        public string UserInfo
        {
            get 
            {
                return "当前登录用户："+Framework.Environment.UserName;
            }
        }

        public string ServerIPInfo
        {
            get
            {
                return "当前服务器地址："+Framework.Environment.ServerIP;
            }
        }

        public string SDKVersionInfo
        {
            get
            {
                string sRet = "SDK版本：N/A";
                try
                {
                    return "SDK版本："+ Framework.Container.Instance.VDAConfigService.GetSDKVersion();
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "获取SDK版本");
                }
                return sRet;
            }
        }
        public string ServerVersionInfo
        {
            get
            {
                string sRet = "服务器版本：N/A";
                try
                {
                    return "服务器版本：" + Framework.Container.Instance.VDAConfigService.GetServerVersion();
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "获取服务器版本");
                }
                return sRet;
            }
        }
        #endregion

        #region Public helper functions

        //public void Execute(object sender, object parameter)
        //{
        //    ((ICommand)sender).Execute(parameter);
        //}

        #endregion
    }

    //#region Other classes

    //public class LogoutCommand : ICommand
    //{
    //    private MainControlViewModel m_VM;

    //    public LogoutCommand(MainControlViewModel vm)
    //    {
    //        m_VM = vm;
    //    }

    //    public void Execute(object sender)
    //    {
    //        if (DialogResult.Yes == Framework.Container.Instance.InteractionService.ShowMessageBox(
    //        "是否确定注销?", Framework.Environment.PROGRAM_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
    //        {
    //            Framework.Container.Instance.EvtAggregator.GetEvent<LogoutEvent>().Publish("登出");
    //        }
    //    }
    //}

    //public class QuitCommand : ICommand
    //{
    //    private MainControlViewModel m_VM;

    //    public QuitCommand(MainControlViewModel vm)
    //    {
    //        m_VM = vm;
    //    }

    //    public void Execute(object sender)
    //    {
    //        if (DialogResult.Yes == Framework.Container.Instance.InteractionService.ShowMessageBox(
    //       "是否确定退出?", Framework.Environment.PROGRAM_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
    //        {
    //            Framework.Container.Instance.EvtAggregator.GetEvent<QuitEvent>().Publish("退出");
    //        }
    //    }
    //}

    //public class ShowUserProfileCommand : ICommand
    //{
    //    private MainControlViewModel m_VM;

    //    public ShowUserProfileCommand(MainControlViewModel vm)
    //    {
    //        m_VM = vm;
    //    }

    //    public void Execute(object sender)
    //    {
    //        UIFuncItemInfo.SHOWUSERPROFILE.Subject = this;
    //        Framework.Container.Instance.EvtAggregator.GetEvent<NavigateEvent>().Publish
    //            (UIFuncItemInfo.SHOWUSERPROFILE);
    //    }
    //}

    //#endregion

}
