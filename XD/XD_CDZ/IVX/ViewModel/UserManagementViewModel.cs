using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Views.Content;
using BOCOM.IVX.Framework;
using BOCOM.DataModel;
using System.Windows.Forms;

namespace BOCOM.IVX.ViewModel
{
    public class UserManagementViewModel : IEventAggregatorSubscriber
    {
        public event EventHandler RecoverUserGroupInfoSelection;
        public event EventHandler RecoverUserInfoSelection;

        private UserInfo m_currEditUser;

        private DataTable m_allGroupList;
        private DataTable m_userList;

        private int m_SelectedUserGroupIndex = 0;

        private int m_SelectedUserIndex = 0;

        private UserGroupInfo m_currEditUserGroup;
        
        public int SelectedUserGroupIndex
        {
            get { return m_SelectedUserGroupIndex; }
        }

        public int SelectedUserIndex
        {
            get { return m_SelectedUserIndex; }
            set { m_SelectedUserIndex = value; }
        }

        public UserManagementViewModel()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<AddUserGroupEvent>().Subscribe(OnUserGroupAddedOrModified, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<EditUserGroupEvent>().Subscribe(OnUserGroupAddedOrModified, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<DelUserGroupEvent>().Subscribe(OnUserGroupDeleted, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);//, Microsoft.Practices.Prism.Events.ThreadOption.BackgroundThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<AddUserEvent>().Subscribe(OnUserAddedOrModified, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<EditUserEvent>().Subscribe(OnUserAddedOrModified, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<DelUserEvent>().Subscribe(OnUserDeleted, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);//, Microsoft.Practices.Prism.Events.ThreadOption.BackgroundThread);
            Framework.Container.Instance.RegisterEventSubscriber(this);

        }

        private int FillAllGroup(uint selectedGroupId)
        {
            int selectedRowIndex = 0;

            if (m_allGroupList != null)
            {
                List<UserGroupInfo> list = null;
                try
                {
                    list = Framework.Container.Instance.VDAConfigService.GetAllUserGroup();
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "获取全部用户组");
                }

                if (list != null)
                {
                    m_allGroupList.Rows.Clear();
                    int index = 0;
                    if (Framework.Container.Instance.AuthenticationService.GetLoginUserRole() == E_VDA_USER_ROLE_TYPE.E_ROLE_TYPE_LEADER)
                    {
                        UserGroupInfo groupInfo = list.First(item => item.UserGroupID == Framework.Container.Instance.AuthenticationService.CurrUser.UserGroupID);
                        m_allGroupList.Rows.Add(groupInfo.UserGroupID
                                                , groupInfo.UserGroupName
                                                , groupInfo.UserGroupDescription
                                                , groupInfo
                                                );
                        if (selectedGroupId > 0 && groupInfo.UserGroupID == selectedGroupId)
                        {
                            selectedRowIndex = index;
                        }
                    }
                    else
                    {
                        list.ForEach(groupInfo =>
                            {
                                m_allGroupList.Rows.Add(groupInfo.UserGroupID
                                                        , groupInfo.UserGroupName
                                                        , groupInfo.UserGroupDescription
                                                        , groupInfo
                                                        );
                                if (selectedGroupId > 0 && groupInfo.UserGroupID == selectedGroupId)
                                {
                                    selectedRowIndex = index;
                                }
                                index++;
                            });
                    }
                }
            }
            return selectedRowIndex;
        }

        public void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<AddUserGroupEvent>().Unsubscribe(OnUserGroupAddedOrModified);
            Framework.Container.Instance.EvtAggregator.GetEvent<EditUserGroupEvent>().Unsubscribe(OnUserGroupAddedOrModified);
            Framework.Container.Instance.EvtAggregator.GetEvent<DelUserGroupEvent>().Unsubscribe(OnUserGroupDeleted);
            Framework.Container.Instance.EvtAggregator.GetEvent<AddUserEvent>().Unsubscribe(OnUserAddedOrModified);
            Framework.Container.Instance.EvtAggregator.GetEvent<EditUserEvent>().Unsubscribe(OnUserAddedOrModified);
            Framework.Container.Instance.EvtAggregator.GetEvent<DelUserEvent>().Unsubscribe(OnUserDeleted);
        }

        public DataTable AllGroupList
        {
            get
            {
                if (m_allGroupList == null)
                {
                    m_allGroupList = new DataTable("AllGroupList");
                    DataColumn UserGroupID = m_allGroupList.Columns.Add("UserGroupID", typeof(UInt32));
                    m_allGroupList.PrimaryKey = new DataColumn[] { UserGroupID };
                    m_allGroupList.Columns.Add("UserGroupName");
                    m_allGroupList.Columns.Add("UserGroupDescription");
                    m_allGroupList.Columns.Add("UserGroupInfo", typeof(UserGroupInfo));

                    FillAllGroup(0);
                }
                return m_allGroupList;
            }
            set { m_allGroupList = value; }
        }
        public DataTable UserList
        {
            get
            {
                if (m_userList == null)
                {
                    m_userList = new DataTable("UserList");
                    DataColumn UserID = m_userList.Columns.Add("UserID", typeof(UInt32));
                    m_userList.PrimaryKey = new DataColumn[] { UserID };
                    m_userList.Columns.Add("UserGroupID");
                    m_userList.Columns.Add("UserRoleType");
                    m_userList.Columns.Add("UserName");
                    m_userList.Columns.Add("UserNickName");
                    m_userList.Columns.Add("UserPwd");
                    m_userList.Columns.Add("CreateTime");
                    m_userList.Columns.Add("UpdateTime");
                    m_userList.Columns.Add("UserInfo", typeof(UserInfo));

                    FillUserByGroupID(CurrEditUserGroup.UserGroupID, 0);
                }
                return m_userList;
            }
            set { m_userList = value; }
        }

        public UserGroupInfo CurrEditUserGroup
        {
            get { return m_currEditUserGroup ?? new UserGroupInfo(); }
            set { m_currEditUserGroup = value; FillUserByGroupID(value.UserGroupID, 0); }
        }

        public void AddUserGroup()
        {
            FormNewUserGroup FormNewUserGroup = new FormNewUserGroup(0, "新建用户组", "新建用户组");
            FormNewUserGroup.ShowDialog();
        }

        public void EditUserGroup()
        {
            if (CurrEditUserGroup.UserGroupID == 1)
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox("无法编辑系统内置用户组！", Framework.Environment.PROGRAM_NAME, MessageBoxButtons.OK);
                return ;

            }

            FormNewUserGroup FormNewUserGroup = new FormNewUserGroup(CurrEditUserGroup.UserGroupID, CurrEditUserGroup.UserGroupName, CurrEditUserGroup.UserGroupDescription, true);
            FormNewUserGroup.ShowDialog();
        }

        public void DelUserGroup()
        {
            if (Framework.Container.Instance.AuthenticationService.CurrUser.UserGroupID == CurrEditUserGroup.UserGroupID)
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox("无法删除当前用户所在的用户组！", Framework.Environment.PROGRAM_NAME, MessageBoxButtons.OK);
                return;
            }

            if (Framework.Container.Instance.InteractionService.ShowMessageBox("是否要删除用户组【" + CurrEditUserGroup.UserGroupName + "】？", Framework.Environment.PROGRAM_NAME, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    IVX.Framework.Container.Instance.VDAConfigService.DelUserGroup(CurrEditUserGroup.UserGroupID);
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "删除用户组");
                }
            }

        }

        private void OnUserGroupAddedOrModified(uint userGroupId)
        {
            uint selectedUserGroupId = userGroupId;
            int selectedRowIndex = FillAllGroup(selectedUserGroupId);

            if (selectedRowIndex > 0)
            {
                m_SelectedUserGroupIndex = selectedRowIndex;
                if (RecoverUserGroupInfoSelection != null)
                {
                    RecoverUserGroupInfoSelection(this, EventArgs.Empty);
                }
            }
        }

        private void OnUserGroupDeleted(uint userGroupId)
        {
           FillAllGroup(0);
           // DataRow row = m_allGroupList.Rows.Find(userGroupId);
           //if (row != null)
           //{
           //    row.Delete();
           //    m_SelectedUserGroupIndex = 0;
           //    if (RecoverUserGroupInfoSelection != null)
           //    {
           //        RecoverUserGroupInfoSelection(this, EventArgs.Empty);
           //    }
           //}

        }

        void OnUserAddedOrModified(uint userId)
        {
            uint selectedUserId = userId;
            int selectedRowIndex = FillUserByGroupID(m_currEditUserGroup.UserGroupID, selectedUserId);

            if (selectedRowIndex > 0)
            {
                m_SelectedUserIndex = selectedRowIndex;
                if (RecoverUserInfoSelection != null)
                {
                    RecoverUserInfoSelection(this, EventArgs.Empty);
                }
            }
        }

        void OnUserDeleted(uint userId)
        {
            FillUserByGroupID(m_currEditUserGroup.UserGroupID, 0);
            //DataRow row = m_userList.Rows.Find(userId);
            //if (row != null)
            //{
            //    row.Delete();
            //    m_SelectedUserIndex = 0;
            //    if (RecoverUserInfoSelection != null)
            //    {
            //        RecoverUserInfoSelection(this, EventArgs.Empty);
            //    }
            //}

        }

        public UserInfo CurrEditUser
        {
            get { return m_currEditUser ?? new UserInfo(); }
            set { m_currEditUser = value; }
        }

        private int FillUserByGroupID(uint groupId, uint selectedUserId)
        {
            int selectedRowIndex = 0;
            if (m_userList == null)
            {
                return selectedRowIndex;
            }

            m_userList.Rows.Clear();

            List<UserInfo> list = null;
            try
            {
                list = Framework.Container.Instance.VDAConfigService.GetUserByGroupID(groupId);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "获取组用户列表");
            }

            if (list != null)
            {
                int index = 0;
                list.ForEach(userInfo => 
                    {
                        if (userInfo.UserID != 1) //界面上不显示superadmin用户
                        {
                            m_userList.Rows.Add(userInfo.UserID
                                                    , userInfo.UserGroupID
                                                    , userInfo.UserRoleType
                                                    , userInfo.UserName
                                                    , userInfo.UserNickName
                                                    , userInfo.UserPwd
                                                    , userInfo.CreateTime
                                                    , userInfo.UpdateTime
                                                    , userInfo
                                                    );
                            if (selectedUserId > 0 && userInfo.UserID == selectedUserId)
                            {
                                selectedRowIndex = index;
                            }
                            index++;
                        }
                    });
            }
            return selectedRowIndex;
        }

        public bool AddUser()
        {
            if (CurrEditUserGroup.UserGroupID == 1)
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox("无法为系统内置用户组添加用户！", Framework.Environment.PROGRAM_NAME, MessageBoxButtons.OK);
                return false;

            }

            FormNewUser FormNewUser = new FormNewUser(new UserInfo { UserName = string.Empty, UserGroupID = m_currEditUserGroup.UserGroupID, UserRoleType = 1 });
            FormNewUser.ShowDialog();
            return true;
        }
        public bool EditUser()
        {
            if (CurrEditUser.UserID == 1)
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox("无法编辑系统内置用户！", Framework.Environment.PROGRAM_NAME, MessageBoxButtons.OK);
                return false;

            }
            FormNewUser FormNewUser = new FormNewUser(CurrEditUser, true);
            FormNewUser.ShowDialog();
            return true;
        }
        public bool DelUser()
        {
            if (CurrEditUser.UserID == 1 || CurrEditUser.UserID == 2)
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox("无法删除系统内置用户！", Framework.Environment.PROGRAM_NAME, MessageBoxButtons.OK);
                return false;

            }

            if (Framework.Container.Instance.AuthenticationService.CurrUser.UserID == CurrEditUser.UserID)
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox("无法删除当前登录用户！", Framework.Environment.PROGRAM_NAME, MessageBoxButtons.OK);
                return false;
            }
            if (Framework.Container.Instance.InteractionService.ShowMessageBox("是否要删除用户【" + CurrEditUser.UserName + "】？", Framework.Environment.PROGRAM_NAME, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    IVX.Framework.Container.Instance.VDAConfigService.DelUser(CurrEditUser.UserID);
                    return true;
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "删除用户");
                    return false;
                }
            }
            else
                return true;
        }
    }
}
