using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using System.Windows.Forms;
using BOCOM.DataModel;

namespace BOCOM.IVX.ViewModel
{
    class AddEditServerViewModel
    {
        #region Fields

        private ServerInfo m_newServer;
        private ServerInfo m_oldServer;
        
        #endregion
        
        #region Properties

        public ServerInfo NewServer
        {
            get { return m_newServer ?? new ServerInfo(); }
            set { m_newServer = value; }
        }

        public ServerInfo OldServer
        {
            get { return m_oldServer ?? new ServerInfo(); }
            set { m_oldServer = value; }
        }

        public string ErrorString { get; set; }

        public bool IsEditMode { get; set; }

        #endregion

        public AddEditServerViewModel(ServerInfo serverInfo, bool isEditMode)
        {
            OldServer = serverInfo;
            NewServer = serverInfo.Clone() as ServerInfo;
            IsEditMode = isEditMode;
        }

        private bool HasChange()
        {
            bool bRet = String.CompareOrdinal(OldServer.IpAddr, NewServer.IpAddr) != 0 ||
                OldServer.Port != NewServer.Port;
            
            return bRet;
        }

        #region Public helper functions

        private bool CheckName()
        {
            bool bRet = Common.TextUtil.ValidateIPAddress(NewServer.IpAddr);

            if (!bRet)
            {
                string msg = "[" + NewServer.IpAddr + "] 不是有效的IP地址，请重新输入。";

                Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME,
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return bRet;
        }

        private bool Validate()
        {
            bool bRet = true;
            if (IsEditMode)
            {
                if (String.CompareOrdinal(OldServer.IpAddr, NewServer.IpAddr) != 0)
                {
                    bRet = CheckName();
                }
            }
            else
            {
                bRet = CheckName();
            }
            return bRet;
        }

        private bool AddServer()
        {
            bool bRet = Validate();

            if (bRet)
            {
                bRet = false;
                try
                {
                    IVX.Framework.Container.Instance.VDAConfigService.AddServer(NewServer);
                    bRet = true;
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "添加服务器");
                }              
            }
            return bRet;
        }

        private bool EditServer()
        {
            bool bRet = Validate();

            if (bRet)
            {
                bRet = false;
                try
                {
                    IVX.Framework.Container.Instance.VDAConfigService.EditServer(NewServer);
                    bRet = true;
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "修改服务器");
                }
            }
            return bRet;
        }

        public bool Commit()
        {
            bool bRet = false;
            if (IsEditMode)
            {
                bRet = EditServer();
            }
            else
            {
                bRet = AddServer();
            }
            return bRet;
        }

        #endregion
    }
}
