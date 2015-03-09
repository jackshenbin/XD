using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Views.Content;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using BOCOM.DataModel;
using BOCOM.IVX.Framework;

namespace BOCOM.IVX.ViewModel
{
    public class ServerManagementViewModel : INotifyPropertyChanged, IEventAggregatorSubscriber
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler ServerDeleted;

        #region Fields

        private DataTable m_serverList;
        private ServerInfo m_Server;
        private ServerInfo m_AddedRealServer;
        
        private E_VDA_SERVER_TYPE m_serverType;

        private bool m_IsEmptyServer = false;

        #endregion

        #region Properties
        
        public DataView ServerList
        {
            get 
            {
                if (m_serverList == null)
                {
                    FillAllServer();
                }
                m_serverList.DefaultView.RowFilter = "Type = " + (int)m_serverType;
                return m_serverList.DefaultView;
            }
        }

        public ServerInfo Server
        {
            get 
            {
                if (m_serverList == null)
                {
                    FillAllServer();
                }

                if (m_Server != null)
                {
                    return m_Server;
                }

                m_serverList.DefaultView.RowFilter = "Type = " + (int)m_serverType;
                if (m_serverList.DefaultView.Count >= 1)
                {
                    m_Server = new ServerInfo
                    {
                        IpAddr = m_serverList.DefaultView[0]["IpAddr"].ToString(),
                        Type = uint.Parse(m_serverList.DefaultView[0]["Type"].ToString()),
                        Port = ushort.Parse(m_serverList.DefaultView[0]["Port"].ToString()),
                        ServerID = uint.Parse(m_serverList.DefaultView[0]["ServerID"].ToString()),
                        Description = m_serverList.DefaultView[0]["Description"].ToString()
                    };
                    m_AddedRealServer = m_Server;
                    m_IsEmptyServer = false;
                }
                else
                {
                    ushort port = Framework.Container.Instance.VDAConfigService.GetServerPortByServerType((E_VDA_SERVER_TYPE)m_serverType);

                    m_IsEmptyServer = true;
                    m_Server = new ServerInfo
                    {
                        IpAddr = "未设置",
                        Type = (uint)m_serverType,
                        Port = port,
                        ServerID = 0,
                        Description = ""
                    };
                }
                return m_Server;
            }
            set
            {
                m_Server = value;
            }
        }

        public ServerInfo SelectedServer { get; set; }

        #endregion

        public ServerManagementViewModel(E_VDA_SERVER_TYPE serverType)
        {
            m_serverType = serverType;

            //Framework.Container.Instance.VDAConfigService.ServerAdded += new EventHandler<Framework.ServerEventArgs>(VDAConfigService_ServerAdded);
            //Framework.Container.Instance.VDAConfigService.ServerModified += new EventHandler<Framework.ServerEventArgs>(VDAConfigService_ServerModified);
            //Framework.Container.Instance.VDAConfigService.ServerDeleted += new EventHandler<Framework.ServerEventArgs>(VDAConfigService_ServerDeleted);
            Framework.Container.Instance.EvtAggregator.GetEvent<ServerAddedEvent>().Subscribe(VDAConfigService_ServerAdded,Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<ServerModifiedEvent>().Subscribe(VDAConfigService_ServerModified,Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<ServerDeletedEvent>().Subscribe(VDAConfigService_ServerDeleted,Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.RegisterEventSubscriber(this);
        }

        #region Private helper functions

        private void  FillAllServer()
        {
            m_serverList = new DataTable("ServerList");
            DataColumn dwServerID = m_serverList.Columns.Add("ServerID", typeof(UInt32));
            m_serverList.PrimaryKey = new DataColumn[] { dwServerID };
            m_serverList.Columns.Add("Type", typeof(UInt32));
            m_serverList.Columns.Add("IpAddr");
            m_serverList.Columns.Add("Port", typeof(UInt16));
            m_serverList.Columns.Add("Description");
            m_serverList.Columns.Add("Server", typeof(ServerInfo));
            
            List<ServerInfo> list = null;
            
            try
            {
               list = Framework.Container.Instance.VDAConfigService.GetAllServer();
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "获取全部服务");
            }
            if (list != null)
            {
                list.ForEach(ptServerInfo =>
                    {
                        AddRow(ptServerInfo);
                    });
            }
        }

        private void AddRow(ServerInfo server)
        {
            m_serverList.Rows.Add(server.ServerID, server.Type, server.IpAddr, server.Port,server.Description, server);
        }

        #endregion

        #region Public helper functions

        public bool AddServer(uint dwType)
        {
            ushort port = Framework.Container.Instance.VDAConfigService.GetServerPortByServerType((E_VDA_SERVER_TYPE)dwType);
            
            using (FormNewServer dlg = new FormNewServer(new ServerInfo { Type = dwType, Port = port }))
            {
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog();
            }
            return true;
        }

        public bool EditServer(uint dwType)
        {
            ServerInfo oldserver = SelectedServer;
            if (oldserver != null)
            {
                using (FormNewServer dlg = new FormNewServer(oldserver, true))
                {
                    dlg.StartPosition = FormStartPosition.CenterParent;
                    dlg.ShowDialog();
                }
            }

            return true;
        }

        public bool DelServer(uint dwType)
        {
            bool bRet = true;
            ServerInfo server = SelectedServer;

            if (server == null || server.ServerID == 0)
                return false ;

            string msg = String.Format("是否确定要删除{0}【{1}】", GetServerTypeName(dwType), server.IpAddr);
            if (dwType == (uint)E_VDA_SERVER_TYPE.E_SERVER_TYPE_IDS)
            { 
                msg = string.Format("删除{0}【{1}】将导致该服务器上的任务单元丢失，且无法恢复。"
                    + System.Environment.NewLine + "是否确认要删除？", GetServerTypeName(dwType), server.IpAddr);            
            }

            if (DialogResult.Yes == Framework.Container.Instance.InteractionService.ShowMessageBox(
                msg, Framework.Environment.PROGRAM_NAME,
                MessageBoxButtons.YesNo, (dwType == (uint)E_VDA_SERVER_TYPE.E_SERVER_TYPE_IDS) ? MessageBoxIcon.Error : MessageBoxIcon.Question))
            {
                try
                {
                        bRet = IVX.Framework.Container.Instance.VDAConfigService.DelServer(server.ServerID);
                }
                catch (SDKCallException ex)
                {
                    bRet = false;
                    Common.SDKCallExceptionHandler.Handle(ex, "删除服务器");
                }
            }
            return bRet;
        }

        public void SaveServer()
        {
            if (!Validate())
                return;
            if (m_Server.Type == (uint)E_VDA_SERVER_TYPE.E_SERVER_TYPE_SMDS
                || m_Server.Type == (uint)E_VDA_SERVER_TYPE.E_SERVER_TYPE_SSDS)
            {
                if (DialogResult.No == Framework.Container.Instance.InteractionService.ShowMessageBox(
                    string.Format("设置服务器后需要手动启动所有的分析服务器，是否要保存服务器数据【{0}:{1}】？", m_Server.IpAddr, m_Server.Port), Framework.Environment.PROGRAM_NAME,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    return;
                }
            }


            if (m_IsEmptyServer)
            {
                try
                {
                    uint serverId = Framework.Container.Instance.VDAConfigService.AddServer(m_Server);
                    if (serverId > 0)
                    {
                        m_AddedRealServer = Framework.Container.Instance.VDAConfigService.GetServerById(serverId);
                        m_IsEmptyServer = false;
                    }
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "添加服务器");
                }
            }
            else
            {
                if (m_AddedRealServer != null)
                {
                    m_AddedRealServer.IpAddr = m_Server.IpAddr;
                    m_AddedRealServer.Port = m_Server.Port;
                    try
                    {
                        Framework.Container.Instance.VDAConfigService.EditServer(m_AddedRealServer);
                    }
                    catch (SDKCallException ex)
                    {
                        Common.SDKCallExceptionHandler.Handle(ex, "修改服务器");
                    }
                }
                else
                {
                    try
                    { 
                        Framework.Container.Instance.VDAConfigService.EditServer(m_Server);
                    }
                    catch (SDKCallException ex)
                    {
                        Common.SDKCallExceptionHandler.Handle(ex, "编辑服务器");
                    }
                }
            }
        }

        public static string GetServerTypeName(uint dwType)
        {
            string strType = "";
            switch ((E_VDA_SERVER_TYPE)dwType)
            {
                case E_VDA_SERVER_TYPE.E_SERVER_TYPE_ALL:			//所有类型的服务器
                    strType = "所有类型的服务器";
                    break;
                case E_VDA_SERVER_TYPE.E_SERVER_TYPE_MSS:           //媒体存储服务器
                    strType = "媒体存储服务器";
                    break;
                case E_VDA_SERVER_TYPE.E_SERVER_TYPE_IAS:			//智能分析服务器
                    strType = "智能分析服务器";
                    break;
                case E_VDA_SERVER_TYPE.E_SERVER_TYPE_SMS:			//检索比对服务器
                    strType = "检索比对服务器";
                    break;
                case E_VDA_SERVER_TYPE.E_SERVER_TYPE_SSS:			//结构化检索服务器
                    strType = "结构化检索服务器";
                    break;
                case E_VDA_SERVER_TYPE.E_SERVER_TYPE_MGW:			//媒体接入网关
                    strType = "媒体接入网关";
                    break;
                case E_VDA_SERVER_TYPE.E_SERVER_TYPE_UGW:			//用户接入网关
                    strType = "用户接入网关";
                    break;
                case E_VDA_SERVER_TYPE.	E_SERVER_TYPE_PAS:		//预分析服务器
                    strType = "预分析服务器";
                    break;

            }
            return strType;
        }

        #endregion

        #region Event handlers

        void VDAConfigService_ServerDeleted(uint serverID)
        {

            DataRow row = m_serverList.Rows.Find(serverID);

            if (row != null)
            {
                row.Delete();
                if (ServerDeleted != null)
                    ServerDeleted(serverID, null);
            }
        }

        void VDAConfigService_ServerModified(ServerInfo info)
        {
            if (info.Type != (uint)m_serverType)
            {
                return;
            }

            DataRow row = m_serverList.Rows.Find(info.ServerID);

            if (row != null)
            {
                row["IpAddr"] = info.IpAddr;
                row["Port"] = info.Port;
                row["Server"] = info;
                SelectedServer = info;
            }
        }

        void VDAConfigService_ServerAdded(ServerInfo info)
        {
            if (info.Type != (uint)m_serverType)
            {
                return;
            }

            DataRow row = m_serverList.Rows.Find(info.ServerID);

            if (row == null)
            {
                AddRow(info);
            }
        }

        private bool CheckIPPort(ServerInfo info)
        {
            bool bRet = Common.TextUtil.ValidateIPAddress(info.IpAddr);
            
            if (!bRet)
            {            
                string msg = "["+ info.IpAddr +"] 不是有效的IP地址，请重新输入。";

                Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME,
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return bRet;
        }

        private bool Validate()
        {
            bool bRet = true;
            if (!m_IsEmptyServer)
            {
                if ((String.CompareOrdinal(m_Server.IpAddr, m_AddedRealServer.IpAddr) != 0) 
                    || (m_Server.Port != m_AddedRealServer.Port))
                {
                    bRet = CheckIPPort(m_Server);
                }
            }
            else
            {
                bRet = CheckIPPort(m_Server);
            }
            return bRet;
        }

        #endregion

        public void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<ServerAddedEvent>().Unsubscribe(VDAConfigService_ServerAdded);
            Framework.Container.Instance.EvtAggregator.GetEvent<ServerModifiedEvent>().Unsubscribe(VDAConfigService_ServerModified);
            Framework.Container.Instance.EvtAggregator.GetEvent<ServerDeletedEvent>().Unsubscribe(VDAConfigService_ServerDeleted);
        }
    }
}
