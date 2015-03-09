using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using BOCOM.DataModel;
using BOCOM.IVX.Protocol.Model;
using DataModel;
using System.Collections.Generic;

namespace BOCOM.IVX.Protocol
{
    #region 基本结构体
    // 地理位置坐标
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_POS_COORD
    {
        public float fX;
        public float fY;
    };

    // 监控点位基本信息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_CAMERA_BASE
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_NAME_LEN)]
        public string szCameraName; //监控点名

        public UInt32 dwVideoSupplierDeviceId;	//关联的网络设备ID
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_NAME_LEN)]
        public string szVideoSupplierChannelId; //存储设备点位标示

        public TVDASDK_POS_COORD tPosCoord; //地理位置坐标	
    };

    // 监控点位信息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_CAMERA_INFO
    {
        public UInt32 dwGroupID;	//所属组ID
        public UInt32 dwCameraID;	//监控点ID
        public TVDASDK_CAMERA_BASE tCameraBase; //监控点位基本信息
    };


    // 监控点位分组基本信息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_CAMERA_GROUP_BASE
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_NAME_LEN)]
        public string szGroupName; //组名
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_DESCRIPTION_INFO_LEN)]
        public string szGroupDescription; //备注，描述信息
    };

    // 监控点位分组基本信息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_CAMERA_GROUP_INFO
    {
        public UInt32 dwCameraGroupID;		//组ID
        public UInt32 dwParentGroupID;		//所属上级组ID
        public TVDASDK_CAMERA_GROUP_BASE tGroupBase; //分组基本信息
    };



    // 服务器基本信息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_SERVER_BASE
    {
        public UInt32 dwType;	//服务器类型，见vdacomm.h中E_VDA_SERVER_TYPE定义
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_IPADDR_LEN)]
        public string szIpAddr; //服务器IP地址
        public UInt16 wPort;	////服务器端口号
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDA_MAX_FILEPATH_LEN)]
        public string  szRest; //保留信息
    };

    // 中心服务器单元基本信息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_SERVER_INFO
    {
        public UInt32 dwServerID;	//服务器ID
        public TVDASDK_SERVER_BASE tServerBase; //服务器基本信息
    };

    // 用户组基本信息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_USER_GROUP_BASE
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_NAME_LEN)]
        public string szUserGroupName; //用户组名
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_DESCRIPTION_INFO_LEN)]
        public string szUserGroupDescription; //备注，描述信息
    };

    // 用户组信息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_USER_GROUP_INFO
    {
        public UInt32 dwUserGroupID;	//所属组ID
        public TVDASDK_USER_GROUP_BASE tUserGroupBase; //用户组基本信息
    };

    // 用户基本信息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_USER_BASE
    {
        public UInt32 dwUserRoleType;	//用户角色类型，见vdacomm.h中E_VDA_USER_ROLE_TYPE定义
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_NAME_LEN)]
        public string szUserName; //用户名
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_NAME_LEN)]
        public string szUserNickName; //用户昵称
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_PWD_LEN)]
        public string szUserPwd; //用户密码
    };

    // 用户信息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_USER_INFO
    {
        public UInt32 dwUserGroupID;	//所属组ID
        public UInt32 dwUserID;			//用户ID
        public TVDASDK_USER_BASE tUserBase; //用户基本信息
        public UInt32 dwCreateTime;		//用户创建时间
        public UInt32 dwUpdateTime;		//用户信息最后一次修改时间
    };


    #endregion

    #region 回调定义

    /// <summary>
    /// 资源更新回调函数
    /// </summary>
    /// <param name="szOperatorName">执行资源操作者名称</param>
    /// <param name="dwOperateType">执行的资源操作类型，见E_VDA_RESOURCE_OPERATE_TYPE</param>
    /// <param name="dwResourceType">被执行的资源类型，见E_VDA_RESOURCE_TYPE</param>
    /// <param name="ptResourceID">资源ID列表</param>
    /// <param name="dwResourceNum">资源数量</param>
    /// <param name="dwUserData"></param>
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    internal unsafe delegate void TfuncResourceUpdateNtfCB(string szOperatorName, UInt32 dwOperateType,
                                                UInt32 dwResourceType, IntPtr ptResourceID, UInt32 dwResourceNum, UInt32 dwUserData);

    #endregion

    internal partial class IVXSDKProtocol
    {
        #region Case 相关的 Camera, CameraGroup 配置方法

        /*===========================================================
		功  能：查询监控点位分组列表
		参  数：dwParentGroupID - 父组编号（传0表示所有分组）
		返回值：-1表示失败，其他值表示返回的查询标示值。
		===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 VdaSdk_QueryCaseCameraGroupList(UInt32 dwParentGroupID);

        //查询下一个监控点分组（遍历接口）
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_QueryNextCaseCameraGroup(Int32 lQueryHandle, out TVDASDK_CAMERA_GROUP_INFO ptCameraGroupInfo);

        //关闭监控点分组查询
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_CloseCaseCameraGroupQuery(Int32 lQueryHandle);

        /*===========================================================
     功  能：查询监控点位列表
     参  数：dwGroupID - 分组编号（传0表示所有点位）
     返回值：-1表示失败，其他值表示返回的查询标示值。获取错误码调用VdaSdk_GetLastError
     ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 VdaSdk_QueryCaseCameraList(UInt32 dwGroupID);

        //查询下一个监控点（遍历接口）
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_QueryNextCaseCamera(Int32 lQueryHandle, out TVDASDK_CAMERA_INFO ptCameraInfo);
        //关闭监控点查询
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_CloseCaseCameraQuery(Int32 lQueryHandle);

        #endregion

        #region 系统Camera, CameraGroup 配置方法（与Case 无关）

        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 VdaSdk_QueryCameraGroupList(UInt32 dwParentGroupID);

        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_QueryNextCameraGroup(Int32 lQueryHandle, out TVDASDK_CAMERA_GROUP_INFO ptCameraGroupInfo);

        //关闭监控点分组查询
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_CloseCameraGroupQuery(Int32 lQueryHandle);

        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 VdaSdk_QueryCameraList(UInt32 dwParentGroupID);

        //查询下一个监控点（遍历接口）
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_QueryNextCamera(Int32 lQueryHandle, out TVDASDK_CAMERA_INFO ptCameraInfo);
        //关闭监控点查询
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_CloseCameraQuery(Int32 lQueryHandle);

        #endregion

        #region Camera, Camera Group 通用配置方法

        //查询监控点分组总数
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 VdaSdk_GetCameraGroupNum(Int32 lQueryHandle);

        //获取指定监控点分组详细信息
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_GetCameraGroupByID(UInt32 dwCameraGroupID,
                                                out TVDASDK_CAMERA_GROUP_INFO ptCameraGroupInfo);

        /*===========================================================
        功  能：新增监控点位分组
        参  数：dwParentGroupID - 父组编号
                tUserGroupBase - 用户组信息
                pdwUserGroupID - 返回用户组编号
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_AddCameraGroup(UInt32 dwParentGroupID, TVDASDK_CAMERA_GROUP_BASE tCameraGroupBase, out UInt32 pdwCameraGroupID);
        //修改监控点位分组
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_MdfCameraGroup(UInt32 dwCameraGroupID, TVDASDK_CAMERA_GROUP_BASE tCameraGroupBase);
        //删除监控点位分组
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_DelCameraGroup(UInt32 dwCameraGroupID);

        //查询监控点总数
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 VdaSdk_GetCameraNum(Int32 lQueryHandle);

        //获取指定监控点详细信息
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_GetCameraByID(UInt32 dwCameraID,
                                                out TVDASDK_CAMERA_INFO ptCameraInfo);

        /*===========================================================
        功  能：新增监控点
        参  数：tCameraInfo - 监控点信息
                pdwCameraID - 返回监控点编号
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_AddCamera(UInt32 dwGroupID, TVDASDK_CAMERA_BASE tCameraBase, out UInt32 pdwCameraID);
        //修改监控点
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_MdfCamera(UInt32 dwCameraID, TVDASDK_CAMERA_BASE tCameraBase);
        //删除监控点
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_DelCamera(UInt32 dwCameraID);

        #endregion

        #region 服务器配置

        /*===========================================================
        功  能：查询指定类型服务器信息列表
        参  数：dwServerType - 服务器类型，见E_VDA_SERVER_TYPE定义
        返回值：-1表示失败，其他值表示返回的查询标示值。获取错误码调用VdaSdk_GetLastError
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 VdaSdk_QueryServerList(UInt32 dwServerType);
        //查询指定类型服务器总数
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 VdaSdk_GetServerNum(Int32 lQueryHandle);
        //查询下一个服务器（遍历接口）
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_QueryNextServer(Int32 lQueryHandle, out TVDASDK_SERVER_INFO ptServerInfo);
        //关闭服务器查询
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_CloseServerQuery(Int32 lQueryHandle);
        //获取指定服务器详细信息
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_GetServerByID(UInt32 dwServerID,
                                                out TVDASDK_SERVER_INFO ptServerInfo);

        /*===========================================================
        功  能：新增服务器
        参  数：tCameraInfo - 服务器信息
                pdwCameraID - 返回监控点编号
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_AddServer(TVDASDK_SERVER_BASE tServerBase, out UInt32 pdwServerID);
        //修改服务器
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_MdfServer(UInt32 dwServerID, TVDASDK_SERVER_BASE tServerBase);
        //删除服务器
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_DelServer(UInt32 dwServerID);
        //启用/停用服务器
        //暂略

        #endregion

        #region 用户、用户组相关

        /*===========================================================
        功  能：查询用户组列表
        参  数：
        返回值：-1表示失败，其他值表示返回的查询标示值。获取错误码调用VdaSdk_GetLastError
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 VdaSdk_QueryUserGroupList();
        //查询用户组总数
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 VdaSdk_GetUserGroupNum(Int32 lQueryHandle);
        //查询下一个用户组（遍历接口）
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_QueryNextUserGroup(Int32 lQueryHandle, out TVDASDK_USER_GROUP_INFO ptUserGroupInfo);
        //关闭用户组查询
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_CloseUserGroupQuery(Int32 lQueryHandle);
        //获取指定用户组详细信息
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_GetUserGroupByID(UInt32 dwUserGroupID,
                                                out TVDASDK_USER_GROUP_INFO ptUserGroupInfo);

        /*===========================================================
        功  能：新增用户组
        参  数：tUserGroupBase - 用户组信息
                pdwUserGroupID - 返回用户组编号
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_AddUserGroup(TVDASDK_USER_GROUP_BASE tUserGroupBase, out UInt32 pdwUserGroupID);
        //修改用户组
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_MdfUserGroup(UInt32 dwUserGroupID, TVDASDK_USER_GROUP_BASE tUserGroupBase);
        //删除用户组
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_DelUserGroup(UInt32 dwUserGroupID);

        /*===========================================================
        功  能：查询用户列表
        参  数：dwUserGroupID : 所属用户组
        返回值：-1表示失败，其他值表示返回的查询标示值。获取错误码调用VdaSdk_GetLastError
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 VdaSdk_QueryUserList(UInt32 dwUserGroupID);
        //查询用户总数
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 VdaSdk_GetUserNum(Int32 lQueryHandle);
        //查询下一个用户（遍历接口）
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_QueryNextUser(Int32 lQueryHandle, out TVDASDK_USER_INFO ptUserInfo);
        //关闭用户查询
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_CloseUserQuery(Int32 lQueryHandle);
        //获取指定用户详细信息
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_GetUserByID(UInt32 dwUserID,
                                                out TVDASDK_USER_INFO ptUserInfo);

        /*===========================================================
        功  能：新增用户
        参  数：dwUserGroupID : 所属用户组
                tUserBase - 用户组信息
                pdwUserID - 返回用户组编号
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_AddUser(UInt32 dwUserGroupID, TVDASDK_USER_BASE tUserBase, out UInt32 pdwUserID);
        //修改用户
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_MdfUser(UInt32 dwUserID, TVDASDK_USER_BASE tUserBase);
        //删除用户
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_DelUser(UInt32 dwUserID);

        #endregion

        #region 网络存储设备

          /*===========================================================
        功  能：登录网络设备或平台
        参  数：tLoginInfo - 平台登录信息
        返回值：成功返回登录句柄，否则返回-1，获取错误码调用VdaSdk_GetLastError
        */
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern int VdaSdk_LoginNetStoreDevice(TVDASDK_NET_STORE_DEV_LOGIN_INFO tLoginInfo);
        
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_LogoutNetStoreDevice(int loginHandle);

        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern int VdaSdk_GetDeviceChannelList(int loginHandle);

        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_QueryNextDeviceChannel(int loginHandle, out TVDASDK_NET_STORE_DEV_CHANNEL_INFO ptChannelInfo );
        
        //关闭设备通道查询
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_ColseDeviceChannelQuery(int loginHandle);

         [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern int VdaSdk_GetDeviceVideoFileList(int loginHandle, TVDASDK_NET_STORE_DEV_FILE_CONDITION tFileCondition);

        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_QueryNextDeviceVideoFile(int loginHandle, out TVDASDK_NET_STORE_DEV_FILE_INFO ptFileInfo);
        
        //关闭设备通道查询
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_ColseDeviceVideoFilelQuery(int loginHandle);

        /*===========================================================
        功  能：查询网络存储设备列表
        参  数：
        返回值：-1表示失败，其他值表示返回的查询标示值。获取错误码调用VdaSdk_GetLastError
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 VdaSdk_QueryNetStoreDevList();

        //查询网络存储设备总数
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 VdaSdk_GetNetStoreDevNum(Int32 lQueryHandle);

        //查询下一个网络存储设备（遍历接口）
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_QueryNextNetStoreDev(Int32 lQueryHandle, out TVDASDK_NET_STORE_DEV_INFO ptNetStoreDevInfo);

        //关闭网络存储设备查询
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_CloseNetStoreDevQuery(Int32 lQueryHandle);
        
        //获取指定网络存储设备详细信息
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_GetNetStoreDevByID(UInt32 dwVideoSupplierDeviceId, out TVDASDK_NET_STORE_DEV_INFO ptNetStoreDevInfo);

        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern int VdaSdk_QueryNetStoreDevCameraList(uint deviceId);

        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_QueryNextNetStoreDevCamera(int queryHandle, out TVDASDK_CAMERA_INFO ptCameraInfo);

        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_CloseNetStoreDevCameraQuery(int queryHandle);

        /*===========================================================
        功  能：新增网络存储设备
        参  数：dwUserGroupID : 所属用户组
		        tUserBase - 用户组信息
		        pdwUserID - 返回用户组编号
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_AddNetStoreDev(TVDASDK_NET_STORE_DEV_BASE tNetStoreDevBase, out UInt32 pdwVideoSupplierDeviceId);
        //修改用户
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_MdfNetStoreDev(UInt32 dwVideoSupplierDeviceId, TVDASDK_NET_STORE_DEV_BASE tNetStoreDevBase);
        //删除用户
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_DelNetStoreDev(UInt32 dwVideoSupplierDeviceId);

        #endregion

        //注册资源更新通知回调
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_ResourceUpdateNtfCBReg(TfuncResourceUpdateNtfCB pfuncResourceUpdateNtf, UInt32 dwUserData);
    }


    public partial class IVXProtocol
    {
        #region Case 相关的 Camera, CameraGroup 配置方法

        /// <summary>
        /// 查询监控点位分组列表
        /// </summary>
        /// <param name="parentGroupID">父组编号（传0表示所有分组）</param>
        /// <returns>-1表示失败，其他值表示返回的查询标示值。</returns>
        public Int32 QueryCaseCameraGroupList(UInt32 parentGroupID)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol QueryCaseCameraGroupList parentGroupID:{0}"
                , parentGroupID
                ));

            int retVal = IVXSDKProtocol.VdaSdk_QueryCaseCameraGroupList(parentGroupID);
            if (-1 == retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }


            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol QueryCaseCameraGroupList ret:{0}"
                , retVal
                ));
            return retVal;
        }

        /// <summary>
        /// 查询下一个监控点分组（遍历接口）
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>监控点分组信息</returns>
        public CameraGroupInfo QueryNextCaseCameraGroup(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol QueryNextCaseCameraGroup queryHandle:{0}"
                , queryHandle
                ));
            TVDASDK_CAMERA_GROUP_INFO info = new TVDASDK_CAMERA_GROUP_INFO();

            bool retVal = IVXSDKProtocol.VdaSdk_QueryNextCaseCameraGroup(queryHandle, out info);

            CameraGroupInfo cameraGroupInfo = null;
            // 不会有SDK调用失败的情况， 因为数据已经全部取到SDK了， 不需要再跟Server交互。所以不需要CheckError

            if (retVal)
            {
                cameraGroupInfo = new CameraGroupInfo();
                cameraGroupInfo.CameraGroupID = info.dwCameraGroupID;
                cameraGroupInfo.ParentGroupID = info.dwParentGroupID;
                cameraGroupInfo.GroupDescription = info.tGroupBase.szGroupDescription;
                cameraGroupInfo.GroupName = info.tGroupBase.szGroupName;
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol QueryNextCaseCameraGroup ret:{0}"
                    + ",dwCameraGroupID:{1}"
                    + ",dwParentGroupID:{2}"
                    + ",szGroupDescription:{3}"
                    + ",szGroupName:{4}"
                    , retVal
                    , info.dwCameraGroupID
                    , info.dwParentGroupID
                    , info.tGroupBase.szGroupDescription
                    , info.tGroupBase.szGroupName
                    ));
            }
            return cameraGroupInfo;
        }

        /// <summary>
        /// 关闭监控点分组查询
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool CloseCaseCameraGroupQuery(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol CloseCaseCameraGroupQuery queryHandle:{0}"
                , queryHandle
                ));

            bool retVal = IVXSDKProtocol.VdaSdk_CloseCaseCameraGroupQuery(queryHandle);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }


            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol CloseCaseCameraGroupQuery ret:{0}"
                , retVal
                ));
            return retVal;
        }

        /// <summary>
        /// 查询监控点位列表
        /// </summary>
        /// <param name="groupID">分组编号（传0表示所有点位）</param>
        /// <returns>-1表示失败，其他值表示返回的查询标示值</returns>
        public Int32 QueryCaseCameraList(UInt32 groupID)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol QueryCaseCameraList dwGroupID:" + groupID);
            Int32 retVal = IVXSDKProtocol.VdaSdk_QueryCaseCameraList(groupID);

            if (-1 == retVal)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol QueryCaseCameraList ret:" + retVal);
            return retVal;

        }

        /// <summary>
        /// 查询下一个监控点（遍历接口）
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>监控点信息</returns>
        public CameraInfo QueryNextCaseCamera(Int32 queryHandle)
        {

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol QueryNextCaseCamera lQueryHandle:" + queryHandle);
            TVDASDK_CAMERA_INFO ptCameraInfo; // = new IVXSDKProtocol.TVDASDK_CAMERA_INFO();
            bool retVal = IVXSDKProtocol.VdaSdk_QueryNextCaseCamera(queryHandle, out ptCameraInfo);

            CameraInfo cameraInfo = null;
            // 不会有SDK调用失败的情况， 因为数据已经全部取到SDK了， 不需要再跟Server交互。所以不需要CheckError

            if (retVal)
            {
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol QueryNextCaseCamera ret:{0},"
                    + "dwCameraID:{1},"
                    + "dwGroupID:{2},"
                    + "chCameraName:{3},"
                    + "dwVideoSupplierDeviceId:{4},"
                    + "szVideoSupplierChannelId:{5},"
                    + "tPosCoord.fX:{6},"
                    + "tPosCoord.fY:{7},"
                    + Environment.NewLine
                    , retVal
                    , ptCameraInfo.dwCameraID
                    , ptCameraInfo.dwGroupID
                    , ptCameraInfo.tCameraBase.szCameraName
                    , ptCameraInfo.tCameraBase.dwVideoSupplierDeviceId
                    , ptCameraInfo.tCameraBase.szVideoSupplierChannelId
                    , ptCameraInfo.tCameraBase.tPosCoord.fX
                    , ptCameraInfo.tCameraBase.tPosCoord.fY
                    ));
                cameraInfo = ModelParser.Convert(ptCameraInfo);
            }
            return cameraInfo;
        }

        /// <summary>
        /// 关闭监控点查询
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool CloseCaseCameraQuery(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol CloseCaseCameraQuery lQueryHandle:" + queryHandle);
            bool retVal = IVXSDKProtocol.VdaSdk_CloseCaseCameraQuery(queryHandle);

            if (!retVal)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol CloseCaseCameraQuery ret:" + retVal);
            return retVal;

        }

        #endregion

        #region  系统Camera, CameraGroup 配置方法（与Case 无关）


        /// <summary>
        /// 查询监控点位分组列表
        /// </summary>
        /// <param name="parentGroupID">父组编号（传0表示所有分组）</param>
        /// <returns>-1表示失败，其他值表示返回的查询标示值。</returns>
        public Int32 QueryCameraGroupList(UInt32 parentGroupID)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_QueryCameraGroupList parentGroupID:{0}"
                , parentGroupID
                ));

            int retVal = IVXSDKProtocol.VdaSdk_QueryCameraGroupList(parentGroupID);
            if (-1 == retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }


            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_QueryCameraGroupList ret:{0}"
                , retVal
                ));
            return retVal;
        }

        /// <summary>
        /// 查询下一个监控点分组（遍历接口）
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>监控点分组信息</returns>
        public CameraGroupInfo QueryNextCameraGroup(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_QueryNextCameraGroup queryHandle:{0}"
                , queryHandle
                ));
            TVDASDK_CAMERA_GROUP_INFO info = new TVDASDK_CAMERA_GROUP_INFO();

            bool retVal = IVXSDKProtocol.VdaSdk_QueryNextCameraGroup(queryHandle, out info);

            CameraGroupInfo cameraGroupInfo = null;
            // 不会有SDK调用失败的情况， 因为数据已经全部取到SDK了， 不需要再跟Server交互。所以不需要CheckError

            if (retVal)
            {
                cameraGroupInfo = new CameraGroupInfo();
                cameraGroupInfo.CameraGroupID = info.dwCameraGroupID;
                cameraGroupInfo.ParentGroupID = info.dwParentGroupID;
                cameraGroupInfo.GroupDescription = info.tGroupBase.szGroupDescription;
                cameraGroupInfo.GroupName = info.tGroupBase.szGroupName;
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_QueryNextCameraGroup ret:{0}"
                    + ",dwCameraGroupID:{1}"
                    + ",dwParentGroupID:{2}"
                    + ",szGroupDescription:{3}"
                    + ",szGroupName:{4}"
                    , retVal
                    , info.dwCameraGroupID
                    , info.dwParentGroupID
                    , info.tGroupBase.szGroupDescription
                    , info.tGroupBase.szGroupName
                    ));
            }
            return cameraGroupInfo;
        }

        /// <summary>
        /// 关闭监控点分组查询
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool CloseCameraGroupQuery(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_CloseCameraGroupQuery queryHandle:{0}"
                , queryHandle
                ));

            bool retVal = IVXSDKProtocol.VdaSdk_CloseCameraGroupQuery(queryHandle);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }


            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_CloseCameraGroupQuery ret:{0}"
                , retVal
                ));
            return retVal;
        }

        /// <summary>
        /// 查询监控点位列表
        /// </summary>
        /// <param name="groupID">分组编号（传0表示所有点位）</param>
        /// <returns>-1表示失败，其他值表示返回的查询标示值</returns>
        public Int32 QueryCameraList(UInt32 groupID)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_QueryCameraList dwGroupID:" + groupID);
            Int32 retVal = IVXSDKProtocol.VdaSdk_QueryCameraList(groupID);

            if (-1 == retVal)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_QueryCameraList ret:" + retVal);
            return retVal;

        }

        /// <summary>
        /// 查询下一个监控点（遍历接口）
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>监控点信息</returns>
        public CameraInfo QueryNextCamera(Int32 queryHandle)
        {

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_QueryNextCamera lQueryHandle:" + queryHandle);
            TVDASDK_CAMERA_INFO ptCameraInfo; // = new IVXSDKProtocol.TVDASDK_CAMERA_INFO();
            bool retVal = IVXSDKProtocol.VdaSdk_QueryNextCamera(queryHandle, out ptCameraInfo);

            CameraInfo cameraInfo = null;
            // 不会有SDK调用失败的情况， 因为数据已经全部取到SDK了， 不需要再跟Server交互。所以不需要CheckError

            if (retVal)
            {
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_QueryNextCamera ret:{0},"
                    + "dwCameraID:{1},"
                    + "dwGroupID:{2},"
                    + "chCameraName:{3},"
                    + "dwVideoSupplierDeviceId:{4},"
                    + "szVideoSupplierChannelId:{5},"
                    + "tPosCoord.fX:{6},"
                    + "tPosCoord.fY:{7},"
                    + Environment.NewLine
                    , retVal
                    , ptCameraInfo.dwCameraID
                    , ptCameraInfo.dwGroupID
                    , ptCameraInfo.tCameraBase.szCameraName
                    , ptCameraInfo.tCameraBase.dwVideoSupplierDeviceId
                    , ptCameraInfo.tCameraBase.szVideoSupplierChannelId
                    , ptCameraInfo.tCameraBase.tPosCoord.fX
                    , ptCameraInfo.tCameraBase.tPosCoord.fY
                    ));
                cameraInfo = new CameraInfo();

                cameraInfo.CameraID = ptCameraInfo.dwCameraID;
                cameraInfo.GroupID = ptCameraInfo.dwGroupID;
                cameraInfo.CameraName = ptCameraInfo.tCameraBase.szCameraName;
                cameraInfo.VideoSupplierDeviceID = ptCameraInfo.tCameraBase.dwVideoSupplierDeviceId;
                cameraInfo.VideoSupplierChannelID = ptCameraInfo.tCameraBase.szVideoSupplierChannelId;
                cameraInfo.PosCoordX = ptCameraInfo.tCameraBase.tPosCoord.fX;
                cameraInfo.PosCoordY = ptCameraInfo.tCameraBase.tPosCoord.fY;
            }
            return cameraInfo;
        }

        /// <summary>
        /// 关闭监控点查询
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool CloseCameraQuery(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_CloseCameraQuery lQueryHandle:" + queryHandle);
            bool retVal = IVXSDKProtocol.VdaSdk_CloseCameraQuery(queryHandle);

            if (!retVal)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_CloseCameraQuery ret:" + retVal);
            return retVal;

        }

        #endregion

        #region 其它通用配置方法

        /// <summary>
        /// 获取指定监控点分组详细信息
        /// </summary>
        /// <param name="cameraGroupID">监控点分组编号</param>
        /// <returns>监控点分组详细信息</returns>
        public CameraGroupInfo GetCameraGroupByID(UInt32 cameraGroupID)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_GetCameraGroupByID cameraGroupID:{0}"
                , cameraGroupID
                ));
            TVDASDK_CAMERA_GROUP_INFO info = new TVDASDK_CAMERA_GROUP_INFO();

            bool retVal = IVXSDKProtocol.VdaSdk_GetCameraGroupByID(cameraGroupID, out info);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
                // 如果不抛异常， 应该是记录不存在, 返回 null
                return null;
            }

            CameraGroupInfo cameraGroupInfo = new CameraGroupInfo();
            cameraGroupInfo.CameraGroupID = info.dwCameraGroupID;
            cameraGroupInfo.ParentGroupID = info.dwParentGroupID;
            cameraGroupInfo.GroupDescription = info.tGroupBase.szGroupDescription;
            cameraGroupInfo.GroupName = info.tGroupBase.szGroupName;
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_GetCameraGroupByID ret:{0}"
                + ",dwCameraGroupID:{1}"
                + ",dwParentGroupID:{2}"
                + ",szGroupDescription:{3}"
                + ",szGroupName:{4}"
                , retVal
                , info.dwCameraGroupID
                , info.dwParentGroupID
                , info.tGroupBase.szGroupDescription
                , info.tGroupBase.szGroupName
                ));
            return cameraGroupInfo;

        }

        /// <summary>
        /// 查询监控点分组总数
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>监控点分组总数</returns>
        public UInt32 GetCameraGroupNum(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_GetCameraGroupNum queryHandle:{0}"
                , queryHandle
                ));

            uint retVal = IVXSDKProtocol.VdaSdk_GetCameraGroupNum(queryHandle);

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_GetCameraGroupNum ret:{0}"
                , retVal
                ));
            return retVal;
        }

        /// <summary>
        /// 新增监控点位分组
        /// </summary>
        /// <param name="cameraGroupInfo">用户组信息</param>
        /// <returns>用户组编号</returns>
        public UInt32 AddCameraGroup(CameraGroupInfo cameraGroupInfo)
        {
            uint cameraGroupID = 0;

            TVDASDK_CAMERA_GROUP_BASE tCameraGroupBase = new TVDASDK_CAMERA_GROUP_BASE();
            tCameraGroupBase.szGroupName = cameraGroupInfo.GroupName;
            tCameraGroupBase.szGroupDescription = cameraGroupInfo.GroupDescription;

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_AddCameraGroup ParentGroupID:{0},"
                + "szGroupName:{1},"
                + "szGroupDescription:{2},"
                + Environment.NewLine
                , cameraGroupInfo.ParentGroupID
                , tCameraGroupBase.szGroupName
                , tCameraGroupBase.szGroupDescription
                ));
            bool retVal = IVXSDKProtocol.VdaSdk_AddCameraGroup(cameraGroupInfo.ParentGroupID, tCameraGroupBase, out cameraGroupID);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_AddCameraGroup ret:{0},cameraGroupID:{1}", retVal, cameraGroupID));
            return retVal ? cameraGroupID : 0;

        }

        /// <summary>
        /// 修改监控点位分组
        /// </summary>
        /// <param name="cameraGroupInfo">用户组信息</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool MdfCameraGroup(CameraGroupInfo cameraGroupInfo)
        {
            TVDASDK_CAMERA_GROUP_BASE tCameraGroupBase = new TVDASDK_CAMERA_GROUP_BASE();
            tCameraGroupBase.szGroupName = cameraGroupInfo.GroupName;
            tCameraGroupBase.szGroupDescription = cameraGroupInfo.GroupDescription;

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_MdfCameraGroup CameraGroupID:{0},"
                + "szGroupName:{1},"
                + "szGroupDescription:{2},"
                + Environment.NewLine
                , cameraGroupInfo.CameraGroupID
                , tCameraGroupBase.szGroupName
                , tCameraGroupBase.szGroupDescription
                ));
            bool retVal = IVXSDKProtocol.VdaSdk_MdfCameraGroup(cameraGroupInfo.CameraGroupID, tCameraGroupBase);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError(true  );
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_MdfCameraGroup ret:{0}", retVal));
            return retVal;
        }

        /// <summary>
        /// 删除监控点位分组
        /// </summary>
        /// <param name="cameraGroupID">用户组编号</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool DelCameraGroup(UInt32 cameraGroupID)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_DelCameraGroup cameraGroupID:{0}"
                , cameraGroupID
                ));

            bool retVal = IVXSDKProtocol.VdaSdk_DelCameraGroup(cameraGroupID);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }


            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_DelCameraGroup ret:{0}"
                , retVal
                ));
            return retVal;

        }

        /// <summary>
        /// 查询监控点总数
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>监控点总数</returns>
        public UInt32 GetCameraNum(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_GetCameraNum lQueryHandle:" + queryHandle);
            UInt32 retVal = IVXSDKProtocol.VdaSdk_GetCameraNum(queryHandle);

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_GetCameraNum ret:" + retVal);
            return retVal;
        }

        /// <summary>
        /// 获取指定监控点详细信息
        /// </summary>
        /// <param name="cameraID">监控点编号</param>
        /// <returns>监控点信息</returns>
        public CameraInfo GetCameraByID(UInt32 cameraID)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_GetCameraByID dwCameraID:" + cameraID);
            TVDASDK_CAMERA_INFO ptCameraInfo; // = new IVXSDKProtocol.TVDASDK_CAMERA_INFO();
            bool retVal = IVXSDKProtocol.VdaSdk_GetCameraByID(cameraID, out ptCameraInfo);
            if (!retVal)
            {
                CheckError();
                // 如果不抛异常， 应该是记录不存在, 返回 null
                return null;
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_GetCameraByID ret:{0},"
                + "dwCameraID:{1},"
                + "dwGroupID:{2},"
                + "chCameraName:{3},"
                + "dwVideoSupplierDeviceId:{4},"
                + "szVideoSupplierChannelId:{5},"
                + "tPosCoord.fX:{6},"
                + "tPosCoord.fY:{7},"
                + Environment.NewLine
                , retVal
                , ptCameraInfo.dwCameraID
                , ptCameraInfo.dwGroupID
                , ptCameraInfo.tCameraBase.szCameraName
                , ptCameraInfo.tCameraBase.dwVideoSupplierDeviceId
                , ptCameraInfo.tCameraBase.szVideoSupplierChannelId
                , ptCameraInfo.tCameraBase.tPosCoord.fX
                , ptCameraInfo.tCameraBase.tPosCoord.fY
                ));

            CameraInfo cameraInfo = new CameraInfo();

            cameraInfo.CameraID = ptCameraInfo.dwCameraID;
            cameraInfo.GroupID = ptCameraInfo.dwGroupID;
            cameraInfo.CameraName = ptCameraInfo.tCameraBase.szCameraName;
            cameraInfo.VideoSupplierDeviceID = ptCameraInfo.tCameraBase.dwVideoSupplierDeviceId;
            cameraInfo.VideoSupplierChannelID = ptCameraInfo.tCameraBase.szVideoSupplierChannelId;
            cameraInfo.PosCoordX = ptCameraInfo.tCameraBase.tPosCoord.fX;
            cameraInfo.PosCoordY = ptCameraInfo.tCameraBase.tPosCoord.fY;

            return retVal ? cameraInfo : null;
        }

        /// <summary>
        /// 新增监控点
        /// </summary>
        /// <param name="cameraInfo"></param>
        /// <returns>零表示失败，非零表示监控点编号</returns>
        public UInt32 AddCamera(CameraInfo cameraInfo)
        {
            uint cameraID = 0;

            TVDASDK_CAMERA_BASE tCameraBase = new TVDASDK_CAMERA_BASE();
            tCameraBase.dwVideoSupplierDeviceId = cameraInfo.VideoSupplierDeviceID;
            tCameraBase.szCameraName = cameraInfo.CameraName;
            tCameraBase.szVideoSupplierChannelId = cameraInfo.VideoSupplierChannelID;
            tCameraBase.tPosCoord.fX = 0;// cameraInfo.PosCoordX;
            tCameraBase.tPosCoord.fY = 0;// cameraInfo.PosCoordY;
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_AddCamera dwGroupID:{0},"
                + "chCameraName:{1},"
                + "dwVideoSupplierDeviceId:{2},"
                + "szVideoSupplierChannelId:{3},"
                + "tPosCoord.fX:{4},"
                + "tPosCoord.fY:{5},"
                + Environment.NewLine
                , cameraInfo.GroupID
                , tCameraBase.szCameraName
                , tCameraBase.dwVideoSupplierDeviceId
                , tCameraBase.szVideoSupplierChannelId
                , tCameraBase.tPosCoord.fX
                , tCameraBase.tPosCoord.fY
                ));
            bool retVal = IVXSDKProtocol.VdaSdk_AddCamera(cameraInfo.GroupID, tCameraBase, out cameraID);

            if (!retVal)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_AddCamera ret:{0},pdwCameraID:{1}", retVal, cameraID));
            return retVal ? cameraID : 0;
        }

        /// <summary>
        /// 修改监控点
        /// </summary>
        /// <param name="cameraInfo">监控点信息</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool MdfCamera(CameraInfo cameraInfo)
        {
            TVDASDK_CAMERA_BASE tCameraBase = new TVDASDK_CAMERA_BASE();
            tCameraBase.dwVideoSupplierDeviceId = cameraInfo.VideoSupplierDeviceID;
            tCameraBase.szCameraName = cameraInfo.CameraName;
            tCameraBase.szVideoSupplierChannelId = cameraInfo.VideoSupplierChannelID;
            tCameraBase.tPosCoord.fX = 0;// cameraInfo.PosCoordX;
            tCameraBase.tPosCoord.fY = 0;// cameraInfo.PosCoordY;

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_MdfCamera dwCameraID:{0},"
                + "chCameraName:{1},"
                + "dwVideoSupplierDeviceId:{2},"
                + "szVideoSupplierChannelId:{3},"
                + "tPosCoord.fX:{4},"
                + "tPosCoord.fY:{5},"
                + Environment.NewLine
                , cameraInfo.CameraID
                , tCameraBase.szCameraName
                , tCameraBase.dwVideoSupplierDeviceId
                , tCameraBase.szVideoSupplierChannelId
                , tCameraBase.tPosCoord.fX
                , tCameraBase.tPosCoord.fY
                ));
            bool retVal = IVXSDKProtocol.VdaSdk_MdfCamera(cameraInfo.CameraID, tCameraBase);
            if (!retVal)
            {
                CheckError(true );
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_MdfCamera ret:{0}", retVal));

            return retVal;
        }

        /// <summary>
        /// 删除监控点
        /// </summary>
        /// <param name="cameraID">监控点编号</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool DelCamera(UInt32 cameraID)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_DelCamera dwCameraID:" + cameraID);
            bool retVal = IVXSDKProtocol.VdaSdk_DelCamera(cameraID);

            if (!retVal)
            {
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_DelCamera ret:" + retVal);
            return retVal;

        }

        /// <summary>
        /// 查询指定类型服务器信息列表
        /// </summary>
        /// <param name="serverType">服务器类型</param>
        /// <returns>-1表示失败，其他值表示返回的查询标示值</returns>
        public Int32 QueryServerList(E_VDA_SERVER_TYPE serverType)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_QueryServerList dwServerType:" + serverType);
            int retVal = IVXSDKProtocol.VdaSdk_QueryServerList((uint)serverType);

            if (retVal <= 0)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_QueryServerList ret:" + retVal);
            return retVal;
        }

        /// <summary>
        /// 查询指定类型服务器总数
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>服务器总数</returns>
        public UInt32 GetServerNum(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_GetServerNum lQueryHandle:" + queryHandle);
            uint retVal = IVXSDKProtocol.VdaSdk_GetServerNum(queryHandle);

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_GetServerNum ret:" + retVal);
            return retVal;
        }

        /// <summary>
        /// 查询下一个服务器（遍历接口）
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>服务器信息</returns>
        public ServerInfo QueryNextServer(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_QueryNextServer lQueryHandle:" + queryHandle);
            TVDASDK_SERVER_INFO ptServerInfo = new TVDASDK_SERVER_INFO();
            bool retVal = IVXSDKProtocol.VdaSdk_QueryNextServer(queryHandle, out ptServerInfo);

            ServerInfo serverInfo = null;
            // 不会有SDK调用失败的情况， 因为数据已经全部取到SDK了， 不需要再跟Server交互。所以不需要CheckError

            if (retVal)
            {
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_QueryNextServer ret:{0},"
                    + "dwServerID:{1},"
                    + "dwType:{2},"
                    + "szIpAddr:{3},"
                    + "wPort:{4},"
                    + "szRest:{5},"
                    , retVal
                    , ptServerInfo.dwServerID
                    , ptServerInfo.tServerBase.dwType
                    , ptServerInfo.tServerBase.szIpAddr
                    , ptServerInfo.tServerBase.wPort
                    , ptServerInfo.tServerBase.szRest
                    ));

                serverInfo = new ServerInfo();
                serverInfo.ServerID = ptServerInfo.dwServerID;
                serverInfo.IpAddr = ptServerInfo.tServerBase.szIpAddr;
                serverInfo.Port = ptServerInfo.tServerBase.wPort;
                serverInfo.Type = ptServerInfo.tServerBase.dwType;
                serverInfo.Description = ptServerInfo.tServerBase.szRest;
            }

            return serverInfo;
        }

        /// <summary>
        /// 关闭服务器查询
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool CloseServerQuery(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_CloseServerQuery lQueryHandle:" + queryHandle);
            bool retVal = IVXSDKProtocol.VdaSdk_CloseServerQuery(queryHandle);

            if (!retVal)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_CloseServerQuery ret:" + retVal);
            return retVal;
        }

        /// <summary>
        /// 获取指定服务器详细信息
        /// </summary>
        /// <param name="serverID">服务器编号</param>
        /// <returns>服务器信息</returns>
        /// <exception cref="SDKCallException"></exception>
        public ServerInfo GetServerByID(UInt32 serverID)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_GetServerByID dwServerID:" + serverID);
            TVDASDK_SERVER_INFO ptServerInfo = new TVDASDK_SERVER_INFO();
            bool retVal = IVXSDKProtocol.VdaSdk_GetServerByID(serverID, out ptServerInfo);

            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
                // 如果不抛异常， 应该是记录不存在, 返回 null
                return null;
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_GetServerByID ret:{0},"
                + "dwServerID:{1},"
                + "dwType:{2},"
                + "szIpAddr:{3},"
                + "wPort:{4},"
                + "szRest:{5},"
                , retVal
                , ptServerInfo.dwServerID
                , ptServerInfo.tServerBase.dwType
                , ptServerInfo.tServerBase.szIpAddr
                , ptServerInfo.tServerBase.wPort
                , ptServerInfo.tServerBase.szRest
                ));

            ServerInfo serverInfo = new ServerInfo();
            serverInfo.ServerID = ptServerInfo.dwServerID;
            serverInfo.IpAddr = ptServerInfo.tServerBase.szIpAddr;
            serverInfo.Port = ptServerInfo.tServerBase.wPort;
            serverInfo.Type = ptServerInfo.tServerBase.dwType;
            serverInfo.Description = ptServerInfo.tServerBase.szRest;

            return retVal ? serverInfo : null;
        }
        
        /// <summary>
        /// 新增服务器
        /// </summary>
        /// <param name="serverInfo">服务器信息</param>
        /// <returns>返回零表示失败，非零表示服务器编号</returns>
        public UInt32 AddServer(ServerInfo serverInfo)
        {
            uint serverID = 0;
            TVDASDK_SERVER_BASE tServerBase = new TVDASDK_SERVER_BASE();
            tServerBase.dwType = serverInfo.Type;
            tServerBase.szIpAddr = serverInfo.IpAddr;
            tServerBase.wPort = serverInfo.Port;
            tServerBase.szRest = serverInfo.Description;

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_AddServer dwType:{0},"
                + "szIpAddr:{1},"
                + "wPort:{2},"
                + "szRest:{3},"
                , tServerBase.dwType
                , tServerBase.szIpAddr
                , tServerBase.wPort
                , tServerBase.szRest
                ));

            bool retVal = IVXSDKProtocol.VdaSdk_AddServer(tServerBase, out serverID);

            if (!retVal)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_AddServer ret:{0},pdwServerID:{1}", retVal, serverID));
            return retVal ? serverID : 0;

        }

        /// <summary>
        /// 修改服务器
        /// </summary>
        /// <param name="serverInfo">服务器信息</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        /// <exception cref="SDKCallException">调用SDK异常</exception>
        public bool MdfServer(ServerInfo serverInfo)
        {
            TVDASDK_SERVER_BASE tServerBase = new TVDASDK_SERVER_BASE();
            tServerBase.dwType = serverInfo.Type;
            tServerBase.szIpAddr = serverInfo.IpAddr;
            tServerBase.wPort = serverInfo.Port;
            tServerBase.szRest = serverInfo.Description;

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_MdfServer dwServerID:{0}"
                + "dwType:{1},"
                + "szIpAddr:{2},"
                + "wPort:{3},"
                + "szRest:{4},"
                , serverInfo.ServerID
                , tServerBase.dwType
                , tServerBase.szIpAddr
                , tServerBase.wPort
                , tServerBase.szRest
                ));

            bool retVal = IVXSDKProtocol.VdaSdk_MdfServer(serverInfo.ServerID, tServerBase);

            if (!retVal)
            {
                CheckError(true);
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_MdfServer ret:{0}", retVal));
            return retVal;
        }

        /// <summary>
        /// 删除服务器
        /// </summary>
        /// <param name="serverID">服务器编号</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool DelServer(UInt32 serverID)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_DelServer dwServerID:" + serverID);

            bool retVal = IVXSDKProtocol.VdaSdk_DelServer(serverID);


            if (!retVal)
            {
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_DelServer ret:" + retVal);

            return retVal;

        }

        /// <summary>
        /// 查询用户组列表
        /// </summary>
        /// <returns>-1表示失败，其他值表示返回的查询标示值</returns>
        public Int32 QueryUserGroupList()
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_QueryUserGroupList ");
            Int32 retVal = IVXSDKProtocol.VdaSdk_QueryUserGroupList();

            if (retVal <= 0)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_QueryUserGroupList ret:" + retVal);
            return retVal;
        }

        /// <summary>
        /// 查询用户组总数
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>用户组总数</returns>
        public UInt32 GetUserGroupNum(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_GetUserGroupNum lQueryHandle:" + queryHandle);
            UInt32 retVal = IVXSDKProtocol.VdaSdk_GetUserGroupNum(queryHandle);

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_GetUserGroupNum ret:" + retVal);
            return retVal;
        }

        /// <summary>
        /// 查询下一个用户组（遍历接口）
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>用户组信息</returns>
        public UserGroupInfo QueryNextUserGroup(Int32 queryHandle)
        {
            TVDASDK_USER_GROUP_INFO ptUserGroupInfo = new TVDASDK_USER_GROUP_INFO();
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_QueryNextUserGroup lQueryHandle:{0}", queryHandle));
            bool retVal = IVXSDKProtocol.VdaSdk_QueryNextUserGroup(queryHandle, out ptUserGroupInfo);

            UserGroupInfo userGroupInfo = null;
            // 不会有SDK调用失败的情况， 因为数据已经全部取到SDK了， 不需要再跟Server交互。所以不需要CheckError

            if (retVal)
            {
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_QueryNextUserGroup ret:{0},"
                    + "dwUserGroupID:{1},"
                    + "szUserGroupDescription:{2},"
                    + "szUserGroupName:{3},"
                    , retVal
                    , ptUserGroupInfo.dwUserGroupID
                    , ptUserGroupInfo.tUserGroupBase.szUserGroupDescription
                    , ptUserGroupInfo.tUserGroupBase.szUserGroupName
                    ));

                userGroupInfo = new UserGroupInfo();
                userGroupInfo.UserGroupID = ptUserGroupInfo.dwUserGroupID;
                userGroupInfo.UserGroupDescription = ptUserGroupInfo.tUserGroupBase.szUserGroupDescription;
                userGroupInfo.UserGroupName = ptUserGroupInfo.tUserGroupBase.szUserGroupName;
            }

            return userGroupInfo;
        }

        /// <summary>
        /// 关闭用户组查询
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool CloseUserGroupQuery(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_CloseUserGroupQuery lQueryHandle:" + queryHandle);
            bool retVal = IVXSDKProtocol.VdaSdk_CloseUserGroupQuery(queryHandle);

            if (!retVal)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_CloseUserGroupQuery ret:" + retVal);
            return retVal;
        }
        //
        /// <summary>
        /// 获取指定用户组详细信息
        /// </summary>
        /// <param name="userGroupID">用户组编号</param>
        /// <returns>用户组信息</returns>
        public UserGroupInfo GetUserGroupByID(UInt32 userGroupID)
        {
            TVDASDK_USER_GROUP_INFO ptUserGroupInfo = new TVDASDK_USER_GROUP_INFO();
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_GetUserGroupByID dwUserGroupID:{0}", userGroupID));
            bool retVal = IVXSDKProtocol.VdaSdk_GetUserGroupByID(userGroupID, out ptUserGroupInfo);
            if (!retVal)
            {
                CheckError();
                // 如果不抛异常， 应该是记录不存在, 返回 null
                return null;
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_GetUserGroupByID ret:{0},"
                + "dwUserGroupID:{1},"
                + "szUserGroupDescription:{2},"
                + "szUserGroupName:{3},"
                , retVal
                , ptUserGroupInfo.dwUserGroupID
                , ptUserGroupInfo.tUserGroupBase.szUserGroupDescription
                , ptUserGroupInfo.tUserGroupBase.szUserGroupName
                ));

            UserGroupInfo userGroupInfo = new UserGroupInfo();
            userGroupInfo.UserGroupID = ptUserGroupInfo.dwUserGroupID;
            userGroupInfo.UserGroupDescription = ptUserGroupInfo.tUserGroupBase.szUserGroupDescription;
            userGroupInfo.UserGroupName = ptUserGroupInfo.tUserGroupBase.szUserGroupName;
            return retVal ? userGroupInfo : null;
        }

        /// <summary>
        /// 新增用户组
        /// </summary>
        /// <param name="userGroupInfo">用户组信息</param>
        /// <returns>返回零表示失败，非零表示用户组编号</returns>
        public uint AddUserGroup(UserGroupInfo userGroupInfo)
        {
            TVDASDK_USER_GROUP_BASE tUserGroupBase = new TVDASDK_USER_GROUP_BASE();
            tUserGroupBase.szUserGroupName = userGroupInfo.UserGroupName;
            tUserGroupBase.szUserGroupDescription = userGroupInfo.UserGroupDescription;
            UInt32 pdwUserGroupID = 0;
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_AddUserGroup szUserGroupName:{0},szUserGroupDescription:{1}"
                , userGroupInfo.UserGroupName, userGroupInfo.UserGroupDescription));

            bool retVal = IVXSDKProtocol.VdaSdk_AddUserGroup(tUserGroupBase, out pdwUserGroupID);

            if (!retVal)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_AddUserGroup ret:" + retVal);
            return retVal ? pdwUserGroupID : 0;
        }

        /// <summary>
        /// 修改用户组
        /// </summary>
        /// <param name="userGroupInfo">用户组信息</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool MdfUserGroup(UserGroupInfo userGroupInfo)
        {
            // return false;
            TVDASDK_USER_GROUP_BASE tUserGroupBase = new TVDASDK_USER_GROUP_BASE();
            tUserGroupBase.szUserGroupName = userGroupInfo.UserGroupName;
            tUserGroupBase.szUserGroupDescription = userGroupInfo.UserGroupDescription;
            UInt32 pdwUserGroupID = userGroupInfo.UserGroupID;
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_MdfUserGroup pdwUserGroupID:{0}"
                + ",szUserGroupName:{1}"
                + ",szUserGroupDescription{2}"
                , pdwUserGroupID
                , tUserGroupBase.szUserGroupName
                , tUserGroupBase.szUserGroupDescription
                ));

            bool retVal = IVXSDKProtocol.VdaSdk_MdfUserGroup(pdwUserGroupID, tUserGroupBase);

            if (!retVal)
            {
                CheckError(true );
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_MdfUserGroup ret:" + retVal);
            return retVal;

        }

        /// <summary>
        /// 删除用户组
        /// </summary>
        /// <param name="userGroupID">用户组编号</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool DelUserGroup(UInt32 userGroupID)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_DelUserGroup dwUserGroupID:" + userGroupID);
            bool retVal = IVXSDKProtocol.VdaSdk_DelUserGroup(userGroupID);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_DelUserGroup ret:" + retVal);
            return retVal;

        }

        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <param name="userGroupID">所属用户组</param>
        /// <returns>-1表示失败，其他值表示返回的查询标示值</returns>
        public Int32 QueryUserList(UInt32 userGroupID)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_QueryUserList dwUserGroupID:" + userGroupID);
            int retVal = IVXSDKProtocol.VdaSdk_QueryUserList(userGroupID);
            if (-1 == retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_QueryUserList ret:" + retVal);
            return retVal;
        }

        /// <summary>
        /// 查询用户总数
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>用户总数</returns>
        public UInt32 GetUserNum(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_GetUserNum lQueryHandle:" + queryHandle);
            uint retVal = IVXSDKProtocol.VdaSdk_GetUserNum(queryHandle);
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_GetUserNum ret:" + retVal);
            return retVal;
        }

        /// <summary>
        /// 查询下一个用户（遍历接口）
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>用户信息</returns>
        public UserInfo QueryNextUser(Int32 queryHandle)
        {
            TVDASDK_USER_INFO ptUserInfo = new TVDASDK_USER_INFO();

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_QueryNextUser lQueryHandle:" + queryHandle);
            bool retVal = IVXSDKProtocol.VdaSdk_QueryNextUser(queryHandle, out ptUserInfo);

            UserInfo userInfo = null;
            // 不会有SDK调用失败的情况， 因为数据已经全部取到SDK了， 不需要再跟Server交互。所以不需要CheckError

            if (retVal)
            {
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_QueryNextUser ret:{0}"
                    + ",dwUserGroupID:{1}"
                    + ",dwUserID:{2}"
                    + ",dwCreateTime:{3}"
                    + ",dwUpdateTime:{4}"
                    + ",dwUserRoleType:{5}"
                    + ",szUserName:{6}"
                    + ",szUserNickName:{7}"
                    + ",szUserPwd:{8}"
                    , retVal
                    , ptUserInfo.dwUserGroupID
                    , ptUserInfo.dwUserID
                    , ptUserInfo.dwCreateTime
                    , ptUserInfo.dwUpdateTime
                    , ptUserInfo.tUserBase.dwUserRoleType
                    , ptUserInfo.tUserBase.szUserName
                    , ptUserInfo.tUserBase.szUserNickName
                    , ptUserInfo.tUserBase.szUserPwd
                    ));
                userInfo = new UserInfo();
                userInfo.CreateTime = ModelParser.ConvertLinuxTime(ptUserInfo.dwCreateTime);
                userInfo.UpdateTime = ModelParser.ConvertLinuxTime(ptUserInfo.dwUpdateTime);
                userInfo.UserGroupID = ptUserInfo.dwUserGroupID;
                userInfo.UserID = ptUserInfo.dwUserID;
                userInfo.UserName = ptUserInfo.tUserBase.szUserName;
                userInfo.UserNickName = ptUserInfo.tUserBase.szUserNickName;
                userInfo.UserPwd = ptUserInfo.tUserBase.szUserPwd;
                userInfo.UserRoleType = ptUserInfo.tUserBase.dwUserRoleType;
            }
            return userInfo;
        }

        /// <summary>
        /// 关闭用户查询
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool CloseUserQuery(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_CloseUserQuery lQueryHandle:" + queryHandle);
            bool retVal = IVXSDKProtocol.VdaSdk_CloseUserQuery(queryHandle);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_CloseUserQuery ret:" + retVal);
            return retVal;
        }
        //
        /// <summary>
        /// 获取指定用户详细信息
        /// </summary>
        /// <param name="userID">用户编号</param>
        /// <returns>用户信息</returns>
        public UserInfo GetUserByID(UInt32 userID)
        {
            TVDASDK_USER_INFO ptUserInfo = new TVDASDK_USER_INFO();

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_GetUserByID dwUserGroupID:" + userID);
            bool retVal = IVXSDKProtocol.VdaSdk_GetUserByID(userID, out ptUserInfo);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
                // 如果不抛异常， 应该是记录不存在, 返回 null
                return null;
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_GetUserByID ret:{0}"
                + ",dwUserGroupID:{1}"
                + ",dwUserID:{2}"
                + ",dwCreateTime:{3}"
                + ",dwUpdateTime:{4}"
                + ",dwUserRoleType:{5}"
                + ",szUserName:{6}"
                + ",szUserNickName:{7}"
                + ",szUserPwd:{8}"
                , retVal
                , ptUserInfo.dwUserGroupID
                , ptUserInfo.dwUserID
                , ptUserInfo.dwCreateTime
                , ptUserInfo.dwUpdateTime
                , ptUserInfo.tUserBase.dwUserRoleType
                , ptUserInfo.tUserBase.szUserName
                , ptUserInfo.tUserBase.szUserNickName
                , ptUserInfo.tUserBase.szUserPwd
                ));
            UserInfo userInfo = new UserInfo();
            userInfo.CreateTime = ModelParser.ConvertLinuxTime(ptUserInfo.dwCreateTime);
            userInfo.UpdateTime = ModelParser.ConvertLinuxTime(ptUserInfo.dwUpdateTime);
            userInfo.UserGroupID = ptUserInfo.dwUserGroupID;
            userInfo.UserID = ptUserInfo.dwUserID;
            userInfo.UserName = ptUserInfo.tUserBase.szUserName;
            userInfo.UserNickName = ptUserInfo.tUserBase.szUserNickName;
            userInfo.UserPwd = ptUserInfo.tUserBase.szUserPwd;
            userInfo.UserRoleType = ptUserInfo.tUserBase.dwUserRoleType;

            return retVal ? userInfo : null;
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns>返回零表示失败，非零表示用户编号</returns>
        public uint AddUser(UserInfo userInfo)
        {
            TVDASDK_USER_BASE tUserBase = new TVDASDK_USER_BASE();
            tUserBase.dwUserRoleType = userInfo.UserRoleType;
            tUserBase.szUserName = userInfo.UserName;
            tUserBase.szUserNickName = userInfo.UserNickName;
            tUserBase.szUserPwd = userInfo.UserPwd;
            UInt32 dwUserGroupID = userInfo.UserGroupID;

            uint dwUserID = 0;
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_AddUser dwUserGroupID:{0}"
                + ",dwUserRoleType:{1}"
                + ",szUserName:{2}"
                + ",szUserNickName:{3}"
                + ",szUserPwd:{4}"
                , dwUserGroupID
                , tUserBase.dwUserRoleType
                , tUserBase.szUserName
                , tUserBase.szUserNickName
                , tUserBase.szUserPwd
                ));

            bool retVal = IVXSDKProtocol.VdaSdk_AddUser(dwUserGroupID, tUserBase, out dwUserID);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_AddUser ret:" + retVal);
            return retVal ? dwUserID : 0;

        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool MdfUser(UserInfo userInfo)
        {
            TVDASDK_USER_BASE tUserBase = new TVDASDK_USER_BASE();
            tUserBase.dwUserRoleType = userInfo.UserRoleType;
            tUserBase.szUserName = userInfo.UserName;
            tUserBase.szUserNickName = userInfo.UserNickName;
            tUserBase.szUserPwd = userInfo.UserPwd;

            UInt32 dwUserID = userInfo.UserID;

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_MdfUser dwUserID:{0}"
                + ",dwUserRoleType:{1}"
                + ",szUserName:{2}"
                + ",szUserNickName:{3}"
                + ",szUserPwd:{4}"
                , dwUserID
                , tUserBase.dwUserRoleType
                , tUserBase.szUserName
                , tUserBase.szUserNickName
                , tUserBase.szUserPwd
                ));

            bool retVal = IVXSDKProtocol.VdaSdk_MdfUser(dwUserID, tUserBase);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError(true );
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_MdfUser ret:" + retVal);
            return retVal;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userID">用户编号</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool DelUser(UInt32 userID)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_DelUser dwUserID:" + userID);
            bool retVal = IVXSDKProtocol.VdaSdk_DelUser(userID);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_DelUser ret:" + retVal);
            return retVal;
        }

        #endregion

        #region 网络存储设备

        /*===========================================================
        功  能：登录网络设备或平台
        参  数：tLoginInfo - 平台登录信息
        返回值：成功返回登录句柄，否则返回-1，获取错误码调用VdaSdk_GetLastError
        */
        
        public int LoginVideoSupplierDevice(VideoSupplierDeviceInfo device)
        {
            if(device == null)
            {
                return 0;
            }

             MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, 
                string.Format("Entering LoginVideoSupplierDevice {0} ...", device.DeviceName));

            TVDASDK_NET_STORE_DEV_BASE dev = new TVDASDK_NET_STORE_DEV_BASE()
            {
                dwAccessProtocolType = (uint)device.ProtocolType,
                dwDevicePort = device.Port,
                szDeviceIP = device.IP,
                szDeviceName = device.DeviceName,
                szLoginUser = device.UserName,
                szLoginPwd = device.Password
            };

            TVDASDK_NET_STORE_DEV_LOGIN_INFO loginInfo = new TVDASDK_NET_STORE_DEV_LOGIN_INFO()
            {
                tDevBase = dev
            };

            int result = IVXSDKProtocol.VdaSdk_LoginNetStoreDevice(loginInfo);

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,
                string.Format("LoginVideoSupplierDevice {0}, session Id: {1}", device.DeviceName, result));

            if (result < 0)
            {
                CheckError();
            }
            device.LoginSessionId = result;
            return result;
        }

        public bool LogoutVideoSupplierDevice(VideoSupplierDeviceInfo device)
        {
            bool bRet = false;

            if(device == null)
            {
                return bRet;
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, 
                string.Format("Entering LogoutVideoSupplierDevice {0} ...", device.DeviceName));
            if (device != null && device.LoginSessionId > 0)
            {
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, 
                    string.Format("LogoutVideoSupplierDevice {0}, sessionId {1}", device.DeviceName, device.LoginSessionId));
                bRet = IVXSDKProtocol.VdaSdk_LogoutNetStoreDevice(device.LoginSessionId);

                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, 
                    string.Format("LogoutVideoSupplierDevice {0}, sessionId {1}, result: {2}", device.DeviceName, device.LoginSessionId, bRet));

                if (!bRet)
                {
                    CheckError();
                }
                device.LoginSessionId = 0;
            }

             MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, 
                string.Format("Leave LogoutVideoSupplierDevice {0} ...", device.DeviceName));
            return bRet;
        }
        
        /// <summary>
        /// 查询网络存储设备列表
        /// </summary>
        /// <returns>-1表示失败，其他值表示返回的查询标示值。</returns>
        public Int32 QueryVideoSupplierDeviceList()
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_QueryNetStoreDevList ");
            int retVal = IVXSDKProtocol.VdaSdk_QueryNetStoreDevList();

            if (retVal <= 0)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_QueryNetStoreDevList ret:" + retVal);
            return retVal;

        }

        /// <summary>
        /// 查询网络存储设备总数
        /// </summary>
        /// <param name="queryHandle">网络存储设备查询唯一标示</param>
        /// <returns></returns>
        public UInt32 GetVideoSupplierDeviceNum(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_GetNetStoreDevNum lQueryHandle:" + queryHandle);
            uint retVal = IVXSDKProtocol.VdaSdk_GetNetStoreDevNum(queryHandle);
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_GetNetStoreDevNum ret:" + retVal);
            return retVal;

        }

        /// <summary>
        /// 查询下一个网络存储设备（遍历接口）
        /// </summary>
        /// <param name="queryHandle">网络存储设备查询唯一标示</param>
        /// <returns></returns>
        public VideoSupplierDeviceInfo QueryNextVideoSupplierDevice(Int32 queryHandle)
        {

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_QueryNextNetStoreDev lQueryHandle:" + queryHandle);
            TVDASDK_NET_STORE_DEV_INFO ptNetStoreDevInfo = new TVDASDK_NET_STORE_DEV_INFO();
            bool retVal = IVXSDKProtocol.VdaSdk_QueryNextNetStoreDev(queryHandle, out ptNetStoreDevInfo);

            VideoSupplierDeviceInfo info = null;
            if (retVal)
            {
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_QueryNextNetStoreDev ret:{0}"
                    + ",dwVideoSupplierDeviceId:{1}"
                    + ",dwAccessProtocolType:{2}"
                    + ",dwDevicePort:{3}"
                    + ",szDeviceIP:{4}"
                    + ",szDeviceName:{5}"
                    + ",szLoginPwd:{6}"
                    + ",szLoginUser:{7}"
                    , retVal
                    , ptNetStoreDevInfo.dwVideoSupplierDeviceId
                    , ptNetStoreDevInfo.tNetStoreDevBase.dwAccessProtocolType
                    , ptNetStoreDevInfo.tNetStoreDevBase.dwDevicePort
                    , ptNetStoreDevInfo.tNetStoreDevBase.szDeviceIP
                    , ptNetStoreDevInfo.tNetStoreDevBase.szDeviceName
                    , ptNetStoreDevInfo.tNetStoreDevBase.szLoginPwd
                    , ptNetStoreDevInfo.tNetStoreDevBase.szLoginUser

                    ));
                info = ModelParser.Convert(ptNetStoreDevInfo);
                
            }
            return info;

        }

        /// <summary>
        /// 关闭网络存储设备查询
        /// </summary>
        /// <param name="queryHandle">网络存储设备查询唯一标示</param>
        /// <returns></returns>
        public bool CloseVideoSupplierDeviceQuery(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_CloseVideoSupplierDeviceQuery lQueryHandle:" + queryHandle);
            bool retVal = IVXSDKProtocol.VdaSdk_CloseNetStoreDevQuery(queryHandle);

            if (!retVal)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_CloseNetStoreDevQuery ret:" + retVal);
            return retVal;

        }

        /// <summary>
        /// 获取指定网络存储设备详细信息
        /// </summary>
        /// <param name="VideoSupplierDeviceId">网络存储设备编号</param>
        /// <returns></returns>
        public VideoSupplierDeviceInfo GetVideoSupplierDeviceByID(UInt32 VideoSupplierDeviceId)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_GetNetStoreDevByID VideoSupplierDeviceId:" + VideoSupplierDeviceId);
            TVDASDK_NET_STORE_DEV_INFO ptNetStoreDevInfo = new TVDASDK_NET_STORE_DEV_INFO();
            bool retVal = IVXSDKProtocol.VdaSdk_GetNetStoreDevByID(VideoSupplierDeviceId, out ptNetStoreDevInfo);

            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
                // 如果不抛异常， 应该是记录不存在, 返回 null
                return null;
            }

            VideoSupplierDeviceInfo info = null;

            if (retVal)
            {
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_GetNetStoreDevByID ret:{0}"
                    + ",dwVideoSupplierDeviceId:{1}"
                    + ",dwAccessProtocolType:{2}"
                    + ",dwDevicePort:{3}"
                    + ",szDeviceIP:{4}"
                    + ",szDeviceName:{5}"
                    + ",szLoginPwd:{6}"
                    + ",szLoginUser:{7}"
                    , retVal
                    , ptNetStoreDevInfo.dwVideoSupplierDeviceId
                    , ptNetStoreDevInfo.tNetStoreDevBase.dwAccessProtocolType
                    , ptNetStoreDevInfo.tNetStoreDevBase.dwDevicePort
                    , ptNetStoreDevInfo.tNetStoreDevBase.szDeviceIP
                    , ptNetStoreDevInfo.tNetStoreDevBase.szDeviceName
                    , ptNetStoreDevInfo.tNetStoreDevBase.szLoginPwd
                    , ptNetStoreDevInfo.tNetStoreDevBase.szLoginUser

                    ));
                info = ModelParser.Convert(ptNetStoreDevInfo);
            }
            return info;

        }

        /// <summary>
        /// 新增网络存储设备
        /// </summary>
        /// <param name="netStoreDev">网络存储设备信息</param>
        /// <returns>返回零表示失败，非零表示网络存储设备编号</returns>
        public UInt32 AddVideoSupplierDevice(VideoSupplierDeviceInfo netStoreDev)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_AddNetStoreDev "
                + ",dwAccessProtocolType:{0}"
                + ",dwDevicePort:{1}"
                + ",szDeviceIP:{2}"
                + ",szDeviceName:{3}"
                + ",szLoginPwd:{4}"
                + ",szLoginUser:{5}"
                , netStoreDev.ProtocolType
                , netStoreDev.Port
                , netStoreDev.IP
                , netStoreDev.DeviceName
                , netStoreDev.Password
                , netStoreDev.UserName

                ));

            UInt32 pdwVideoSupplierDeviceId = 0;

            TVDASDK_NET_STORE_DEV_BASE tNetStoreDevBase = ModelParser.Convert(netStoreDev);

            bool retVal = IVXSDKProtocol.VdaSdk_AddNetStoreDev(tNetStoreDevBase, out pdwVideoSupplierDeviceId);

            if (!retVal)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_AddNetStoreDev ret:" + retVal + ",pdwVideoSupplierDeviceId:" + pdwVideoSupplierDeviceId);
            return pdwVideoSupplierDeviceId;

        }

        /// <summary>
        /// 修改网络存储设备
        /// </summary>
        /// <param name="netStoreDev">网络存储设备信息</param>
        /// <returns></returns>
        public bool MdfVideoSupplierDevice(VideoSupplierDeviceInfo netStoreDev)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_MdfNetStoreDev "
                + ",VideoSupplierDeviceId:{0}"
                + ",dwAccessProtocolType:{1}"
                + ",dwDevicePort:{2}"
                + ",szDeviceIP:{3}"
                + ",szDeviceName:{4}"
                + ",szLoginPwd:{5}"
                + ",szLoginUser:{6}"
                , netStoreDev.Id
                , netStoreDev.ProtocolType
                , netStoreDev.Port
                , netStoreDev.IP
                , netStoreDev.DeviceName
                , netStoreDev.Password
                , netStoreDev.UserName

                ));

            UInt32 pdwVideoSupplierDeviceId = netStoreDev.Id;
            TVDASDK_NET_STORE_DEV_BASE tNetStoreDevBase = ModelParser.Convert(netStoreDev);
          
            bool retVal = IVXSDKProtocol.VdaSdk_MdfNetStoreDev(pdwVideoSupplierDeviceId, tNetStoreDevBase);

            if (!retVal)
            {
                CheckError(true );
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_MdfNetStoreDev ret:" + retVal);
            return retVal;

        }

        /// <summary>
        /// 删除网络存储设备
        /// </summary>
        /// <param name="dwVideoSupplierDeviceId">网络存储设备编号</param>
        /// <returns></returns>
        public bool DelVideoSupplierDevice(UInt32 VideoSupplierDeviceId)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_DelNetStoreDev VideoSupplierDeviceId:" + VideoSupplierDeviceId);
            bool retVal = IVXSDKProtocol.VdaSdk_DelNetStoreDev(VideoSupplierDeviceId);

            if (!retVal)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_DelNetStoreDev ret:" + retVal);
            return retVal;

        }

        public List<VideoSupplierChannelInfo> GetVideoSupplierChannels(VideoSupplierDeviceInfo device)
        {
            if (device == null || device.LoginSessionId <= 0)
            {
                throw new ArgumentNullException("null device or not login");
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,
                string.Format("Entering GetVideoSupplierChannels {0} ...", device.DeviceName));

            int queryHandle = IVXSDKProtocol.VdaSdk_GetDeviceChannelList(device.LoginSessionId);
            if (queryHandle < 0)
            {
                CheckError();
            }
            List<VideoSupplierChannelInfo> channels = new List<VideoSupplierChannelInfo>();
            
            TVDASDK_NET_STORE_DEV_CHANNEL_INFO netChannel;
            VideoSupplierChannelInfo channelInfo;
            while (IVXSDKProtocol.VdaSdk_QueryNextDeviceChannel(queryHandle, out netChannel))
            {
                if (!string.IsNullOrEmpty(netChannel.szChannelId))
                {
                    channelInfo = ModelParser.Convert(netChannel);
                    channels.Add(channelInfo);
                }
            }

            bool result = IVXSDKProtocol.VdaSdk_ColseDeviceChannelQuery(queryHandle);
            if (!result)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,
               string.Format("Leave GetVideoSupplierChannels {0}", device.DeviceName));

            return channels;
        }

        public List<VideoFileInfo> GetVideoFiles(VideoSupplierDeviceInfo device, string channelId, DateTime dtStart, DateTime dtEnd)
        {
             if (device == null || device.LoginSessionId <= 0 || string.IsNullOrEmpty(channelId))
            {
                throw new ArgumentNullException("null device or not login");
            }

             MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,
                string.Format("Entering GetVideoFiles {0}, ChannelID: {1}, StartTime: {2}, EndTime: {3} ...", 
                    device.DeviceName, channelId, dtStart, dtEnd));

            List<VideoFileInfo> videoFiles = null;

            TVDASDK_NET_STORE_DEV_FILE_CONDITION condition = new TVDASDK_NET_STORE_DEV_FILE_CONDITION()
            {
                szChannelId = channelId,
                dwStartTime = ModelParser.ConvertLinuxTime(dtStart),
                dwEndTime = ModelParser.ConvertLinuxTime(dtEnd)
            };

            int queryHandle = IVXSDKProtocol.VdaSdk_GetDeviceVideoFileList(device.LoginSessionId, condition);
            if (queryHandle < 0)
            {
                CheckError();
            }
            
            videoFiles = new List<VideoFileInfo>();

            TVDASDK_NET_STORE_DEV_FILE_INFO netFileInfo;
            VideoFileInfo videoFileInfo;

            while (IVXSDKProtocol.VdaSdk_QueryNextDeviceVideoFile(queryHandle, out netFileInfo))
            {
                if (!string.IsNullOrEmpty(netFileInfo.szFileId))
                {
                    videoFileInfo = ModelParser.Convert(netFileInfo);
                    videoFiles.Add(videoFileInfo);
                }
            }

            bool result = IVXSDKProtocol.VdaSdk_ColseDeviceVideoFilelQuery(queryHandle);
            if (!result)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,
                string.Format("Leave GetVideoFiles {0}, ChannelID: {1}, StartTime: {2}, EndTime: {3}",
                    device.DeviceName, channelId, dtStart, dtEnd));

            return videoFiles;
        }

        public List<CameraInfo> GetCamerasByVideoSupplierDevice(VideoSupplierDeviceInfo device)
        {
            if (device == null || device.LoginSessionId <= 0)
            {
                throw new ArgumentNullException("null device or not login");
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,
                string.Format("Entering GetCamerasByVideoSupplierDevice {0} ...", device.DeviceName));

            List<CameraInfo> cameras = null;

            int queryHandle = IVXSDKProtocol.VdaSdk_QueryNetStoreDevCameraList(device.Id);
            if (queryHandle < 0)
            {
                CheckError();
            }
            
            cameras = new List<CameraInfo>();
            CameraInfo camera;
            TVDASDK_CAMERA_INFO tCameraInfo;
            IntPtr ptr;

            while (IVXSDKProtocol.VdaSdk_QueryNextNetStoreDevCamera(queryHandle, out tCameraInfo))
            {
                //object obj = Marshal.PtrToStructure(ptr, typeof(TVDASDK_CAMERA_INFO));
                //tCameraInfo = (TVDASDK_CAMERA_INFO)obj;
                if (tCameraInfo.dwCameraID > 0)
                {
                    camera = ModelParser.Convert(tCameraInfo);
                    cameras.Add(camera);
                }
            }

            bool result = IVXSDKProtocol.VdaSdk_CloseNetStoreDevCameraQuery(queryHandle);
            if (!result)
            {
                CheckError();
            }

             MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,
                string.Format("Leave GetCamerasByVideoSupplierDevice {0}", device.DeviceName));

            return cameras;
        }

        #endregion
    }
}
