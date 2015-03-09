using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Framework;
using BOCOM.IVX.Protocol.Model;
using BOCOM.DataModel;
using System.Diagnostics;

namespace BOCOM.IVX.Service
{
    public class VDAConfigService
    {
        //public event EventHandler<CameraEventArgs> CameraAdded;
        //public event EventHandler<CameraEventArgs> CameraDeleted;
        //public event EventHandler<CameraEventArgs> CameraModified;

        //public event EventHandler<ServerEventArgs> ServerAdded;
        //public event EventHandler<ServerEventArgs> ServerDeleted;
        //public event EventHandler<ServerEventArgs> ServerModified;

        public VDAConfigService()
        {
            Framework.Container.Instance.IVXProtocol.ResourceChanged += new Action<ResourceUpdateInfo>(IVXProtocol_ResourceChanged);

        }

        void IVXProtocol_ResourceChanged(ResourceUpdateInfo obj)
        {
            switch (obj.ResourceType)
            {
                case E_VDA_RESOURCE_TYPE.E_RESOURCE_TYPE_NOUSE:
                    break;
                case E_VDA_RESOURCE_TYPE.E_RESOURCE_TYPE_SERVER:
                    if (obj.OperateType == E_VDA_RESOURCE_OPERATE_TYPE.E_RESOURCE_OPERATE_TYPE_ADD)
                    {
                        obj.ResourceIDList.ForEach(item =>
                        {
                            ServerInfo info = GetServerById(item);
                            if (info != null)
                                Framework.Container.Instance.EvtAggregator.GetEvent<ServerAddedEvent>().Publish(info);
                        }
                            );
                    }
                    else if (obj.OperateType == E_VDA_RESOURCE_OPERATE_TYPE.E_RESOURCE_OPERATE_TYPE_DEL)
                    {
                        obj.ResourceIDList.ForEach(item =>
                        {
                            Framework.Container.Instance.EvtAggregator.GetEvent<ServerDeletedEvent >().Publish(item);
                        }
                            );
                    }
                    else if (obj.OperateType == E_VDA_RESOURCE_OPERATE_TYPE.E_RESOURCE_OPERATE_TYPE_MDF)
                    {
                        obj.ResourceIDList.ForEach(item =>
                        {
                            ServerInfo info = GetServerById(item);
                            if (info != null)
                                Framework.Container.Instance.EvtAggregator.GetEvent<ServerModifiedEvent>().Publish(info);
                        }
                            );
                    }

                    break;
                case E_VDA_RESOURCE_TYPE.E_RESOURCE_TYPE_NET_STORE:
                    break;
                case E_VDA_RESOURCE_TYPE.E_RESOURCE_TYPE_CAMERA_GROUP:
                    if (obj.OperateType == E_VDA_RESOURCE_OPERATE_TYPE.E_RESOURCE_OPERATE_TYPE_ADD)
                    {
                        obj.ResourceIDList.ForEach(item =>
                        {
                            CameraGroupInfo info = GetCameraGroupByID(item);
                            if (info != null)
                                Framework.Container.Instance.EvtAggregator.GetEvent<CameraGroupAddedEvent>().Publish(info);
                        }
                            );
                    }
                    else if (obj.OperateType == E_VDA_RESOURCE_OPERATE_TYPE.E_RESOURCE_OPERATE_TYPE_DEL)
                    {
                        obj.ResourceIDList.ForEach(item =>
                        {
                            Framework.Container.Instance.EvtAggregator.GetEvent<CameraGroupDeletedEvent>().Publish(item);
                        }
                            );
                    }
                    else if (obj.OperateType == E_VDA_RESOURCE_OPERATE_TYPE.E_RESOURCE_OPERATE_TYPE_MDF)
                    {
                        obj.ResourceIDList.ForEach(item =>
                        {
                            CameraGroupInfo info = GetCameraGroupByID(item);
                            if (info != null)
                                Framework.Container.Instance.EvtAggregator.GetEvent<CameraGroupModifiedEvent>().Publish(info);
                        }
                            );
                    }


                    break;
                case E_VDA_RESOURCE_TYPE.E_RESOURCE_TYPE_CAMERA:
                    if (obj.OperateType == E_VDA_RESOURCE_OPERATE_TYPE.E_RESOURCE_OPERATE_TYPE_ADD)
                    {
                        obj.ResourceIDList.ForEach(item =>
                        {
                            CameraInfo info = GetCameraByID(item);
                            if (info != null)
                                Framework.Container.Instance.EvtAggregator.GetEvent<CameraAddedEvent>().Publish(info);
                        }
                            );
                    }
                    else if (obj.OperateType == E_VDA_RESOURCE_OPERATE_TYPE.E_RESOURCE_OPERATE_TYPE_DEL)
                    {
                        obj.ResourceIDList.ForEach(item =>
                        {
                            Framework.Container.Instance.EvtAggregator.GetEvent<CameraDeletedEvent>().Publish(item);
                        }
                            );
                    }
                    else if (obj.OperateType == E_VDA_RESOURCE_OPERATE_TYPE.E_RESOURCE_OPERATE_TYPE_MDF)
                    {
                        obj.ResourceIDList.ForEach(item =>
                        {
                            CameraInfo info = GetCameraByID(item);
                            if (info != null)
                                Framework.Container.Instance.EvtAggregator.GetEvent<CameraModifiedEvent>().Publish(info);
                        }
                            );
                    }


                    break;
                case E_VDA_RESOURCE_TYPE.E_RESOURCE_TYPE_USER_GROUP:
                    if (obj.OperateType == E_VDA_RESOURCE_OPERATE_TYPE.E_RESOURCE_OPERATE_TYPE_ADD)
                    {
                        obj.ResourceIDList.ForEach(item =>
                        {
                            UserGroupInfo info = Framework.Container.Instance.IVXProtocol.GetUserGroupByID(item);
                            if (info != null)
                                Framework.Container.Instance.EvtAggregator.GetEvent<AddUserGroupEvent>().Publish(item);
                        }
                            );
                    }
                    else if (obj.OperateType == E_VDA_RESOURCE_OPERATE_TYPE.E_RESOURCE_OPERATE_TYPE_DEL)
                    {
                        obj.ResourceIDList.ForEach(item =>
                        {
                            Framework.Container.Instance.EvtAggregator.GetEvent<DelUserGroupEvent>().Publish(item);
                        }
                            );
                    }
                    else if (obj.OperateType == E_VDA_RESOURCE_OPERATE_TYPE.E_RESOURCE_OPERATE_TYPE_MDF)
                    {
                        obj.ResourceIDList.ForEach(item =>
                        {
                            UserGroupInfo info = Framework.Container.Instance.IVXProtocol.GetUserGroupByID(item);
                            if (info != null)
                                Framework.Container.Instance.EvtAggregator.GetEvent<EditUserGroupEvent>().Publish(item);
                        }
                            );
                    }

                    break;
                case E_VDA_RESOURCE_TYPE.E_RESOURCE_TYPE_USER:
                    if (obj.OperateType == E_VDA_RESOURCE_OPERATE_TYPE.E_RESOURCE_OPERATE_TYPE_ADD)
                    {
                        obj.ResourceIDList.ForEach(item =>
                        {
                            UserInfo info = Framework.Container.Instance.IVXProtocol.GetUserByID(item);
                            if (info != null)
                                Framework.Container.Instance.EvtAggregator.GetEvent<AddUserEvent>().Publish(item);
                        }
                            );
                    }
                    else if (obj.OperateType == E_VDA_RESOURCE_OPERATE_TYPE.E_RESOURCE_OPERATE_TYPE_DEL)
                    {
                        obj.ResourceIDList.ForEach(item =>
                        {
                            Framework.Container.Instance.EvtAggregator.GetEvent<DelUserEvent>().Publish(item);
                        }
                            );
                    }
                    else if (obj.OperateType == E_VDA_RESOURCE_OPERATE_TYPE.E_RESOURCE_OPERATE_TYPE_MDF)
                    {
                        obj.ResourceIDList.ForEach(item =>
                        {
                            UserInfo info = Framework.Container.Instance.IVXProtocol.GetUserByID(item);
                            if (info != null)
                                Framework.Container.Instance.EvtAggregator.GetEvent<EditUserEvent>().Publish(item);
                        }
                            );
                    }
                    break;
                case E_VDA_RESOURCE_TYPE.E_RESOURCE_TYPE_CASE:
                    break;
                case E_VDA_RESOURCE_TYPE.E_RESOURCE_TYPE_TASK:
                    break;
                case E_VDA_RESOURCE_TYPE.E_RESOURCE_TYPE_TASKUNIT:
                    break;
                default:
                    break;
            }
        }
        
        #region 监控点组

        public List<CameraGroupInfo> GetAllCameraGroup(uint parentGroupId, bool caseMode=false)
        {
            caseMode = false; //3.0.4.X版本暂时不使用case类型接口，强制赋值为false

            List<CameraGroupInfo> list = new List<CameraGroupInfo>();

            int queryHandle = 0;
            uint count = 0;
            if (caseMode)
            {
                queryHandle = Framework.Container.Instance.IVXProtocol.QueryCaseCameraGroupList(parentGroupId);
            }
            else
            {
                queryHandle = Framework.Container.Instance.IVXProtocol.QueryCameraGroupList(parentGroupId);
            }

            count = Framework.Container.Instance.IVXProtocol.GetCameraGroupNum(queryHandle);

            CameraGroupInfo ptCameraGroupInfo = null;

            while (true)
            {
                if (caseMode)
                {
                    ptCameraGroupInfo = Framework.Container.Instance.IVXProtocol.QueryNextCaseCameraGroup(queryHandle);
                }
                else
                {
                    ptCameraGroupInfo = Framework.Container.Instance.IVXProtocol.QueryNextCameraGroup(queryHandle);
                }
                if (ptCameraGroupInfo != null)
                {
                    list.Add(ptCameraGroupInfo);
                }
                else
                {
                    break;
                }
            }

            if (caseMode)
            {
                Framework.Container.Instance.IVXProtocol.CloseCaseCameraGroupQuery(queryHandle);
            }
            else
            {
                Framework.Container.Instance.IVXProtocol.CloseCameraGroupQuery(queryHandle);
            }

            if (list != null)
            {
                return list.ToList();
            }
            else
            {
                return null;
            }
        }
        
        public CameraGroupInfo GetCameraGroupByID(uint camgroupId)
        {
            CameraGroupInfo camgroup = Framework.Container.Instance.IVXProtocol.GetCameraGroupByID(camgroupId);
            return camgroup;
        }
        
        public uint AddCameraGroup(CameraGroupInfo tCameraGroupBase)
        {
            uint cameraGroupId = Framework.Container.Instance.IVXProtocol.AddCameraGroup(tCameraGroupBase);
            if (cameraGroupId > 0)
            {
                CameraGroupInfo cameraGroup = Framework.Container.Instance.IVXProtocol.GetCameraGroupByID(cameraGroupId);
                Framework.Container.Instance.EvtAggregator.GetEvent<CameraGroupAddedEvent>().Publish(cameraGroup);
            }

            return cameraGroupId;
        }

        public bool EditCameraGroup(CameraGroupInfo tCameraGroupBase)
        {
            bool bRet = Framework.Container.Instance.IVXProtocol.MdfCameraGroup(tCameraGroupBase);

            if (bRet)
            {
                Framework.Container.Instance.EvtAggregator.GetEvent<CameraGroupModifiedEvent>().Publish(tCameraGroupBase);
            }

            return bRet;
        }

        public bool DelCameraGroup(uint dwCameraGroupID)
        {
            bool bRet = true;

            CameraGroupInfo cameraGroup = Framework.Container.Instance.IVXProtocol.GetCameraGroupByID(dwCameraGroupID);
            List<CameraInfo> cameras = GetCameras(dwCameraGroupID, false);

            if (cameraGroup != null)
            {
                bRet = Framework.Container.Instance.IVXProtocol.DelCameraGroup(dwCameraGroupID);

                if (bRet )
                {
                    Framework.Container.Instance.EvtAggregator.GetEvent<CameraGroupDeletedEvent>().Publish(dwCameraGroupID);
                    if (cameras != null && cameras.Count > 0)
                    {
                        foreach (CameraInfo camera in cameras)
                        {
                            // bRet = DelCamera(taskunit.CameraID);
                            Framework.Container.Instance.EvtAggregator.GetEvent<CameraDeletedEvent>().Publish(camera.CameraID);
                            // Debug.Assert(bRet);
                        }
                    }
                }
            }
            return bRet;
        }

        #endregion

        #region 监控点

        public List<CameraInfo> GetAllCameras(bool caseMode = true)
        {
            caseMode = false; //3.0.4.X版本暂时不使用case类型接口，强制赋值为false

            List<CameraInfo> cameras = new List<CameraInfo>();

            List<CameraGroupInfo> groups = GetAllCameraGroup(0, caseMode);
            if (groups != null && groups.Count > 0)
            {
                foreach (CameraGroupInfo g in groups)
                {
                    List<CameraInfo> camerasTmp = GetCameras(g.CameraGroupID, caseMode);
                    if (camerasTmp != null)
                    {
                        cameras.AddRange(camerasTmp);
                    }
                }
            }

            // GetAllCameras(0, cameras);

            return cameras;
        }

        /// <summary>
        /// 获取直接子级 taskunits
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public List<CameraInfo> GetCameras(uint groupID, bool caseMode = true)
        {
            caseMode = false; //3.0.4.X版本暂时不使用case类型接口，强制赋值为false


            List<CameraInfo> list = new List<CameraInfo>();
            int queryHandle = 0;
            uint count = 0;
            if (caseMode)
            {
                queryHandle = Framework.Container.Instance.IVXProtocol.QueryCaseCameraList(groupID);
            }
            else
            {
                queryHandle = Framework.Container.Instance.IVXProtocol.QueryCameraList(groupID);
            }
            
            count = Framework.Container.Instance.IVXProtocol.GetCameraNum(queryHandle);

            CameraInfo ptCameraInfo = null;

            while (true)
            {
                if (caseMode)
                {
                    ptCameraInfo = Framework.Container.Instance.IVXProtocol.QueryNextCaseCamera(queryHandle);
                }
                else
                {
                    ptCameraInfo = Framework.Container.Instance.IVXProtocol.QueryNextCamera(queryHandle);
                }

                if (ptCameraInfo != null)
                {
                    list.Add(ptCameraInfo);
                }
                else
                {
                    break;
                }
            }

            if (caseMode)
            {
                Framework.Container.Instance.IVXProtocol.CloseCaseCameraQuery(queryHandle);
            }
            else
            {
                Framework.Container.Instance.IVXProtocol.CloseCameraQuery(queryHandle);
            }

            if (list != null)
            {
                return list.ToList();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取直接和间接子 taskunits
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public void GetAllCameras(uint groupID, List<CameraInfo> cameras)
        {
            List<CameraInfo> camerasTmp = GetCameras(groupID);
            if (camerasTmp != null && camerasTmp.Count > 0)
            {
                cameras.AddRange(camerasTmp);
            }

            List<CameraGroupInfo> groups = GetAllCameraGroup(groupID);
            if (groups != null && groups.Count > 0)
            {
                foreach (CameraGroupInfo g in groups)
                {
                    GetAllCameras(g.CameraGroupID, cameras);
                }
            }
        }

        public CameraInfo GetCameraByID(uint camId)
        {
            CameraInfo cam = null;
            try
            {
                if(camId>0)
                    cam = Framework.Container.Instance.IVXProtocol.GetCameraByID(camId);
            }
            catch (SDKCallException ex)
            {
                // Framework.Container.
            }
            return cam;
        }

        public List<CameraInfo> GetCamerasByNetDevID(uint netDevID)
        {
            List<CameraInfo> cameras = new List<CameraInfo>();

            cameras = GetAllCameras(false);

            List<CameraInfo> ret = new List<CameraInfo>();
            cameras.ForEach(item=>
            {
                if (item.VideoSupplierDeviceID == netDevID)
                { ret.Add(item); }
            });
            return ret;
        }

        public uint AddCamera(CameraInfo tCameraBase)
        {
            uint cameraId = Framework.Container.Instance.IVXProtocol.AddCamera(tCameraBase);
            if (cameraId > 0)
            {
                CameraInfo camera = Framework.Container.Instance.IVXProtocol.GetCameraByID(cameraId);
                Framework.Container.Instance.EvtAggregator.GetEvent<CameraAddedEvent>().Publish(camera);
            }

            return cameraId;
        }

        public bool EditCamera( CameraInfo tCameraBase)
        {
            bool bRet = Framework.Container.Instance.IVXProtocol.MdfCamera(tCameraBase);

            //if (bRet)
            //{
            //    Framework.Container.Instance.EvtAggregator.GetEvent<CameraModifiedEvent>().Publish(tCameraBase);
            //}

            return bRet;
        }

        public bool DelCamera(uint dwCameraID)
        {
            bool bRet = true;

            CameraInfo camera = Framework.Container.Instance.IVXProtocol.GetCameraByID(dwCameraID);

            if (camera != null)
            {
                bRet = Framework.Container.Instance.IVXProtocol.DelCamera(dwCameraID);

                if (bRet )
                {
                    Framework.Container.Instance.EvtAggregator.GetEvent<CameraDeletedEvent>().Publish(dwCameraID);
                }
            }
            return bRet;
        }

        public CameraInfo GetCameraByVideoSupplierDevice(string strDevIp, int iDevPort, string strChannelId)
        {
            VideoSupplierDeviceInfo dev = GetVideoSupplierDeviceByID(strDevIp, iDevPort);

            CameraInfo ret = null;
            if(dev!=null && dev.Id>0)
            {
                List<CameraInfo> cams = GetCamerasByNetDevID(dev.Id);
                foreach(CameraInfo item in cams)
                {
                    if (item.VideoSupplierChannelID == strChannelId)
                    {
                        ret = item;
                    }
                }
            }
            return ret;
        }

        #endregion

        #region 服务器

        public string GetFtpFileServiceURL()
        {
            List<ServerInfo> list = new List<ServerInfo>();

            int handle = Framework.Container.Instance.IVXProtocol.QueryServerList(E_VDA_SERVER_TYPE.E_SERVER_TYPE_FTP);

            while (true)
            {
                ServerInfo ptServerInfo = Framework.Container.Instance.IVXProtocol.QueryNextServer(handle);
                if (ptServerInfo != null)
                {
                    list.Add(ptServerInfo);
                }
                else
                    break;
            }
            Framework.Container.Instance.IVXProtocol.CloseServerQuery(handle);

            string ftpurl = "";
            if (list.Count > 0)
                ftpurl = list[0].Description;
            return ftpurl;
        }

        public string GetHttpFileServiceURL()
        {
            List<ServerInfo> list = new List<ServerInfo>();

            int handle = Framework.Container.Instance.IVXProtocol.QueryServerList(E_VDA_SERVER_TYPE.E_SERVER_TYPE_HTTP);

            while (true)
            {
                ServerInfo ptServerInfo = Framework.Container.Instance.IVXProtocol.QueryNextServer(handle);
                if (ptServerInfo != null)
                {
                    list.Add(ptServerInfo);
                }
                else
                    break;
            }
            Framework.Container.Instance.IVXProtocol.CloseServerQuery(handle);

            string httpurl = "";
            if (list.Count > 0)
                httpurl = list[0].Description;
            return httpurl;
        }

        public List<ServerInfo> GetAllServer()
        {
            List<ServerInfo> list = new List<ServerInfo>();

            int handle = Framework.Container.Instance.IVXProtocol.QueryServerList(0);
         
            while (true)
            {
                ServerInfo ptServerInfo = Framework.Container.Instance.IVXProtocol.QueryNextServer(handle);
                if (ptServerInfo!=null)
                {
                    list.Add(ptServerInfo);
                }
                else
                    break;
            }
            Framework.Container.Instance.IVXProtocol.CloseServerQuery(handle);
            return list;

        }

        public ServerInfo GetServerById(uint serverId)
        {
            ServerInfo server = Framework.Container.Instance.IVXProtocol.GetServerByID(serverId);
            return server;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverInfo"></param>
        /// <returns></returns>
        /// <exception cref="SDKCallException"></exception>
        public uint AddServer(ServerInfo serverInfo)
        {
            uint serverId = Framework.Container.Instance.IVXProtocol.AddServer(serverInfo);

            if (serverId > 0)
            {
                ServerInfo server = Framework.Container.Instance.IVXProtocol.GetServerByID(serverId);
                Framework.Container.Instance.EvtAggregator.GetEvent<ServerAddedEvent>().Publish(server);
            }

            return serverId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverInfo"></param>
        /// <returns></returns>
        /// <exception cref="SDKCallException"></exception>
        public bool EditServer(ServerInfo serverInfo)
        {
            bool bRet = Framework.Container.Instance.IVXProtocol.MdfServer(serverInfo);

            if (bRet )
            {
                Framework.Container.Instance.EvtAggregator.GetEvent<ServerModifiedEvent>().Publish(serverInfo);
            }
            
            return bRet;
        }

        public bool DelServer(uint pdwServerID)
        {
            bool bRet = true;

            ServerInfo server = Framework.Container.Instance.IVXProtocol.GetServerByID(pdwServerID);

            if (server != null)
            {
                bRet = Framework.Container.Instance.IVXProtocol.DelServer(pdwServerID);

                if (bRet )
                {
                    Framework.Container.Instance.EvtAggregator.GetEvent<ServerDeletedEvent>().Publish(pdwServerID);
                }
            }
            return bRet;
        }


        public ushort GetServerPortByServerType(E_VDA_SERVER_TYPE dwType)
        {
            ushort port = (ushort)((uint)dwType - 10 + 9001);
            if (dwType == E_VDA_SERVER_TYPE.E_SERVER_TYPE_SMS)
                port = 3306;

            return port;
        }

        #endregion

        #region 用户组
        public List<UserGroupInfo> GetAllUserGroup()
        {
            List<UserGroupInfo> list = new List<UserGroupInfo>();

            //for (int i = 0; i < 10; i++)
            //{
            //    list.Add(new UserGroupInfo
            //    {
            //        UserGroupDescription = "UserGroupDescription UserGroupInfo" + i,
            //        UserGroupID = (uint)i,
            //        UserGroupName = "UserGroupInfo" + i
            //    });
            //}
            //return list;



            int lQueryHandle = Framework.Container.Instance.IVXProtocol.QueryUserGroupList();
            while (true)
            {
                UserGroupInfo groupInfo = Framework.Container.Instance.IVXProtocol.QueryNextUserGroup(lQueryHandle);
                if (groupInfo!=null)
                {
                    list.Add(groupInfo);
                }
                else
                    break;
            }
            Framework.Container.Instance.IVXProtocol.CloseUserGroupQuery(lQueryHandle);
            return list;
        }

        public uint AddUserGroup(UserGroupInfo userGroupInfo)
        {
            uint ret = Framework.Container.Instance.IVXProtocol.AddUserGroup(userGroupInfo);
            Framework.Container.Instance.EvtAggregator.GetEvent<AddUserGroupEvent>().Publish(ret);
            return ret;
        }

        public bool EditUserGroup(UserGroupInfo userGroupInfo)
        {
            bool ret = Framework.Container.Instance.IVXProtocol.MdfUserGroup(userGroupInfo);
            Framework.Container.Instance.EvtAggregator.GetEvent<EditUserGroupEvent>().Publish(userGroupInfo.UserGroupID);
            return ret;
        }

        public bool DelUserGroup(uint groupID)
        {
            List<UserInfo> users = GetUserByGroupID(groupID);

            bool ret = Framework.Container.Instance.IVXProtocol.DelUserGroup(groupID);
            Framework.Container.Instance.EvtAggregator.GetEvent<DelUserGroupEvent>().Publish(groupID);

            if (users != null && users.Count > 0)
            {
                foreach (UserInfo user in users)
                {
                    Framework.Container.Instance.EvtAggregator.GetEvent<DelUserEvent>().Publish(user.UserID);
                }
            }

            return ret;
        }
        public string GetUserGroupNameById(uint groupID)
        {
            UserGroupInfo info = Framework.Container.Instance.IVXProtocol.GetUserGroupByID(groupID);
            return (info == null) ? groupID.ToString() : info.UserGroupName;
        }
        #endregion

        #region 用户
        public List<UserInfo> GetAllUser()
        {
            List<UserGroupInfo> usergrouplist = GetAllUserGroup();

            List<UserInfo> UserList = new List<UserInfo>();
            foreach (UserGroupInfo ug in usergrouplist)
            {
                List<UserInfo> ul= GetUserByGroupID(ug.UserGroupID);
                UserList.AddRange(ul);
            }

            return UserList;
        }

        public List<UserInfo> GetUserByGroupID(uint groupID)
        {
            List<UserInfo> list = new List<UserInfo>();

            //for (int i = 0; i < 10; i++)
            //{
            //    list.Add(new UserInfo
            //    {
            //        CreateTime = DateTime.Now,
            //        UpdateTime = DateTime.Now,
            //        UserID = (uint)i,
            //        UserGroupID = groupID,
            //        UserName = "UserInfo" + i,
            //        UserNickName = "u" + i,
            //        UserPwd = "12345",
            //        UserRoleType = (uint)i%3,
            //    });
            //}
            //return list;


            if (groupID == 0)
                return new List<UserInfo>();

            int lQueryHandle = Framework.Container.Instance.IVXProtocol.QueryUserList(groupID);
            while (true)
            {
                UserInfo userInfo = Framework.Container.Instance.IVXProtocol.QueryNextUser(lQueryHandle);
                if (userInfo!=null)
                {
                    list.Add(userInfo);
                }
                else
                    break;
            }
            Framework.Container.Instance.IVXProtocol.CloseUserQuery(lQueryHandle);
            return list;
        }

        public uint AddUser(UserInfo userInfo)
        {
            userInfo.CreateTime = DateTime.Now;
            userInfo.UpdateTime = DateTime.Now;
            uint ret = Framework.Container.Instance.IVXProtocol.AddUser(userInfo);
            Framework.Container.Instance.EvtAggregator.GetEvent<AddUserEvent>().Publish(ret);
            return ret;

        }

        public bool EditUser(UserInfo userInfo)
        {
            userInfo.UpdateTime = DateTime.Now;
            bool ret = Framework.Container.Instance.IVXProtocol.MdfUser(userInfo);
            Framework.Container.Instance.EvtAggregator.GetEvent<EditUserEvent>().Publish(userInfo.UserID);
            return ret;
        }

        public bool DelUser(uint userID)
        {
            bool ret = Framework.Container.Instance.IVXProtocol.DelUser(userID);
            Framework.Container.Instance.EvtAggregator.GetEvent<DelUserEvent>().Publish(userID);
            return ret;
        }

        #endregion

        #region 网络存储设备

        public List<VideoSupplierDeviceInfo> GetAllVideoSupplierDevice()
        {
            List<VideoSupplierDeviceInfo> list = new List<VideoSupplierDeviceInfo>();

            int lQueryHandle = Framework.Container.Instance.IVXProtocol.QueryVideoSupplierDeviceList();
            while (true)
            {
                VideoSupplierDeviceInfo info = Framework.Container.Instance.IVXProtocol.QueryNextVideoSupplierDevice(lQueryHandle);
                if (info!=null)
                {
                    list.Add(info);
                }
                else
                    break;
            }

            Framework.Container.Instance.IVXProtocol.CloseVideoSupplierDeviceQuery(lQueryHandle);
            return list;
        }

        public VideoSupplierDeviceInfo GetVideoSupplierDeviceByID(uint VideoSupplierDeviceID)
        {
            VideoSupplierDeviceInfo info = null;
            try
            {
                if (VideoSupplierDeviceID > 0)
                     info = Framework.Container.Instance.IVXProtocol.GetVideoSupplierDeviceByID(VideoSupplierDeviceID);
            }
            catch (SDKCallException ex)
            {
                // Framework.Container.
            }
            return info;
        }

        public VideoSupplierDeviceInfo GetVideoSupplierDeviceByID(string strDevIp, int iDevPort)
        {
                        
            List<VideoSupplierDeviceInfo> devs = GetAllVideoSupplierDevice();
            VideoSupplierDeviceInfo dev = devs.First(item=>item.IP == strDevIp && item.Port == (uint)iDevPort);

            return dev;
        }

        public uint AddVideoSupplierDevice(VideoSupplierDeviceInfo VideoSupplierDeviceInfo)
        {
            uint VideoSupplierDeviceID = Framework.Container.Instance.IVXProtocol.AddVideoSupplierDevice(VideoSupplierDeviceInfo);
            if (VideoSupplierDeviceID > 0)
            {
                VideoSupplierDeviceInfo dev = Framework.Container.Instance.IVXProtocol.GetVideoSupplierDeviceByID(VideoSupplierDeviceID);
                Framework.Container.Instance.EvtAggregator.GetEvent<AddVideoSupplierDeviceEvent>().Publish(dev);
            }
            return VideoSupplierDeviceID;

        }

        public bool EditVideoSupplierDevice(VideoSupplierDeviceInfo VideoSupplierDeviceInfo)
        {
            bool ret = Framework.Container.Instance.IVXProtocol.MdfVideoSupplierDevice(VideoSupplierDeviceInfo);
            if (ret)
            {
                Framework.Container.Instance.EvtAggregator.GetEvent<EditVideoSupplierDeviceEvent>().Publish(VideoSupplierDeviceInfo);
            }
            return ret;
        }

        public bool DelVideoSupplierDevice(uint VideoSupplierDeviceID)
        {
            bool bRet = true;

            VideoSupplierDeviceInfo dev = Framework.Container.Instance.IVXProtocol.GetVideoSupplierDeviceByID(VideoSupplierDeviceID);

            if (dev != null)
            {
                bRet = Framework.Container.Instance.IVXProtocol.DelVideoSupplierDevice(VideoSupplierDeviceID);

                if (bRet)
                {
                    Framework.Container.Instance.EvtAggregator.GetEvent<DelVideoSupplierDeviceEvent>().Publish(VideoSupplierDeviceID);
                }
            }
            return bRet;
        }

        public void LoginVideoSupplierDevice(VideoSupplierDeviceInfo device)
        {
            if (device != null)
            {
                Framework.Container.Instance.IVXProtocol.LoginVideoSupplierDevice(device);
            }
        }

        public bool LogoutVideoSupplierDevice(VideoSupplierDeviceInfo device)
        {
            bool result = false;
            if (device != null)
            {
                result = Framework.Container.Instance.IVXProtocol.LogoutVideoSupplierDevice(device);
            }
            return result;
        }

        public List<VideoSupplierChannelInfo> GetVideoSupplierChannels(VideoSupplierDeviceInfo device)
        {
            List<VideoSupplierChannelInfo> channels = null;
            
            if (device != null)
            {
                if (device.LoginSessionId <= 0)
                {
                    LoginVideoSupplierDevice(device);
                }
                channels = Framework.Container.Instance.IVXProtocol.GetVideoSupplierChannels(device);
                LogoutVideoSupplierDevice(device);
            }

            return channels;
        }

        public List<VideoFileInfo> GetVideoFiles(VideoSupplierDeviceInfo device, CameraInfo camera, DateTime dtStart, DateTime dtEnd)
        {
            List<VideoFileInfo> files = null;

            if (device != null && camera != null && camera.VideoSupplierDeviceID == device.Id && !string.IsNullOrEmpty(camera.VideoSupplierChannelID))
            {
                if (device.LoginSessionId <= 0)
                {
                    LoginVideoSupplierDevice(device);
                }
                
                files = Framework.Container.Instance.IVXProtocol.GetVideoFiles(device, camera.VideoSupplierChannelID, dtStart, dtEnd);
                LogoutVideoSupplierDevice(device);
            }

            return files;
        }

        public List<CameraInfo> GetCamerasByVideoSupplierDevice(VideoSupplierDeviceInfo device)
        {
            List<CameraInfo> cameras = null;

            if (device != null)
            {
                if (device.LoginSessionId <= 0)
                {
                    LoginVideoSupplierDevice(device);
                }
                cameras = Framework.Container.Instance.IVXProtocol.GetCamerasByVideoSupplierDevice(device);
                LogoutVideoSupplierDevice(device);
            }

            return cameras;
        }
        
        #endregion

        #region 版本信息
        public string GetSDKVersion()
        {
            return IVXProtocol.GetSdkVersion();
        }
        public string GetServerVersion()
        {
            return Framework.Container.Instance.IVXProtocol.GetServerVersion();
        }
        #endregion

    }
}
