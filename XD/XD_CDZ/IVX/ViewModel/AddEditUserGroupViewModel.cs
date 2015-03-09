using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using System.ComponentModel;
using BOCOM.DataModel;
using System.Windows.Forms;

namespace BOCOM.IVX.ViewModel
{
    public class AddEditUserGroupViewModel : INotifyPropertyChanged
    {
        private UserGroupInfo m_newUserGroup;
        public UserGroupInfo NewUserGroup
        {
            get { return m_newUserGroup ?? new UserGroupInfo(); }
            set { m_newUserGroup = value; }
        }

        private UserGroupInfo m_oldUserGroup;
        public UserGroupInfo OldUserGroup
        {
            get { return m_oldUserGroup ?? new UserGroupInfo(); }
            set { m_oldUserGroup = value; }
        }
        private string m_errorString;
        public string ErrorString
        {
            get { return m_errorString; }
            set
            {
                m_errorString = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ErrorString"));
                }
            }
        }

        public int CheckUserGroup(UserGroupInfo newinfo, UserGroupInfo oldinfo)
        {
            return 0;
        }

        public int AddUserGroup()
        {
            int ck = CheckUserGroup(NewUserGroup, OldUserGroup);
            if (ck > 0)
            {
                ErrorString = "errorstring:" + ck;
                return ck;
            }

            uint ret = 0;
            try
            {
                ret = IVX.Framework.Container.Instance.VDAConfigService.AddUserGroup(NewUserGroup);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "添加用户组");
            }

            if (ret > 0)
            {
                //增加用户组后添加一个默认案件
                try
                {
                    CaseInfo newcase = new CaseInfo { CaseName = NewUserGroup.UserGroupName, CaseNo = "CASE_" + NewUserGroup.UserGroupName, UserGroupId = ret, CaseType = 0 };
                    Framework.Container.Instance.CaseManagerService.AddCase(newcase);
                    return 0;
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "添加默认案件");
                }
            }

            ErrorString = "errorstring:-1";
            return -1;
        }
        public int EditUserGroup()
        {
            int ck = CheckUserGroup(NewUserGroup, OldUserGroup);
            if (ck > 0)
            {
                ErrorString = "errorstring:" + ck;
                return ck;
            }

            bool ret = false;
            try
            {
                ret = IVX.Framework.Container.Instance.VDAConfigService.EditUserGroup(NewUserGroup);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "添加默认案件");
            }

            if (ret)
            {
                return 0;
            }
            else
            {
                ErrorString = "errorstring:-1";
                return -1;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
