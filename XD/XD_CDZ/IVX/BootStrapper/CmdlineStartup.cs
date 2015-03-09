using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BOCOM.IVX.BootStrapper
{
    public class CmdlineStartup : Startup
    {
        private string m_User;
        private string m_Pass;
        private string m_IP;

        public CmdlineStartup(string[] args) 
        {
            try
            {
                m_IP = args[0];
                m_User = args[1];
                m_Pass = args[2];
                string loginargs = "";
                foreach (string s in args)
                {
                    loginargs += s+";";
                }
                MyLog4Net.Container.Instance.Log.Info("Version：" + Framework.Environment.VersionDetail);
                MyLog4Net.Container.Instance.Log.Info("用户登录参数：" + loginargs);
            }
            catch (Exception ex)
            {
                MyLog4Net.Container.Instance.Log.Error(ex);
            }

        }

        public override bool Start()
        {
            try
            {
                //Framework.Container.Instance.AuthenticationService.Login(
                //    m_User, m_Pass, m_IP, 0, 10, 0);
            }
            catch (Protocol.SDKCallException ex)
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox("程序运行失败：["+ex.ErrorCode+"]"+ex.Message, Framework.Environment.PROGRAM_NAME,
    MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

            if (Framework.Environment.IsLoggedIn)
            {
                using (MainForm dlg = new MainForm())
                {
                    Application.Run(dlg);
                }
                if (!Framework.Environment.IsBeingLogout)
                {
                    return true   ;
                }
            }

            return false  ;
        }

    }
}
