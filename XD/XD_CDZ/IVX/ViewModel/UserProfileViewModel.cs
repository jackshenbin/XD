using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BOCOM.IVX.Framework;
using BOCOM.IVX.Protocol;

namespace BOCOM.IVX.ViewModel
{
    public class UserProfileViewModel
    {
        public string UserName { get { return Framework.Environment.UserName; } }

        public string Role 
        {
            get
            {
                string role = "N/A";
                try
                {
                    role = Framework.Container.Instance.AuthenticationService.GetLoginUserRoleStr();
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "获取播放状态");
                }
                return role;
            }
        }


        public string ServerIP { get; set; }

        public void ShowUserDetailInfo()
        {

        }

        public void Logout()
        {
            if (DialogResult.Yes == Framework.Container.Instance.InteractionService.ShowMessageBox(
            "是否确定注销?", Framework.Environment.PROGRAM_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                Framework.Container.Instance.EvtAggregator.GetEvent<LogoutEvent>().Publish(true);
            }
        }
    }
}
