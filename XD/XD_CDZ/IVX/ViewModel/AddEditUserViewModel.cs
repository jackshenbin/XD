using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using BOCOM.DataModel;
using System.Windows.Forms;

namespace BOCOM.IVX.ViewModel
{
    public class AddEditUserViewModel : AddEditViewModelBase
    {
        public int UserRoleType
        {
            get { return (int)m_newUser.UserRoleType; }
            set { m_newUser.UserRoleType = (uint)value; }
        }

        private UserInfo m_newUser;
        public UserInfo NewUser
        {
            get { return m_newUser ?? new UserInfo(); }
            set { m_newUser = value; }
        }

        private UserInfo m_oldUser;
        public UserInfo OldUser
        {
            get { return m_oldUser ?? new UserInfo(); }
            set { m_oldUser = value; }
        }


        public string UserGroupName
        {
            get
            {
                string groupName = string.Empty;
                try
                {
                    groupName = Framework.Container.Instance.VDAConfigService.GetUserGroupNameById(m_newUser.UserGroupID);
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "获取用户组名");
                }
                return groupName;
            }
        }
        public string ErrorString{get;set;}

        public int CheckUser(UserInfo newinfo, UserInfo oldinfo)
        { return 0; }

        public AddEditUserViewModel(UserInfo user, bool isEditMode = false)
            : base("用户", isEditMode)
        {
            OldUser = user;
            NewUser = user.Clone();
        }

        private bool CheckName()
        {
            string msg;
            bool bRet = Common.TextUtil.ValidateIfEmptyString(NewUser.UserName, "名称", out msg);

            if (!bRet)
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME,
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return bRet;
        }

        private bool CheckPass()
        {
            string msg;
            bool bRet = Common.TextUtil.ValidateIfEmptyString(NewUser.UserPwd, "密码", out msg);

            if (!bRet)
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME,
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return bRet;
        }

        protected override bool Validate()
        {
            bool bRetName = true;
            
            if (IsEditMode)
            {
                if (String.CompareOrdinal(OldUser.UserName, NewUser.UserName) != 0)
                {
                    bRetName = CheckName();
                }
            }
            else
            {
                bRetName = CheckName();
            }

            bool bRetPass = CheckPass();
            return bRetName && bRetPass;
        }

        protected override bool AddEx()
        {
            bool bRet = false;

            try
            {
                IVX.Framework.Container.Instance.VDAConfigService.AddUser(NewUser);
                bRet = true;
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "添加用户");
            }
            
            return bRet;
        }

        protected override bool EditEx()
        {
            bool bRet = false;
            try
            { 
                bRet = IVX.Framework.Container.Instance.VDAConfigService.EditUser(NewUser);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "修改用户");
            }

            return bRet;
        }
    }
}
