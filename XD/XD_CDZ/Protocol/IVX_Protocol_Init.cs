using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using BOCOM.DataModel;

namespace BOCOM.IVX.Protocol
{
    #region 基本结构体

    // 用户信息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TUSER_INFO
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDA_MAX_NAME_LEN)]
        public string szUserName;	//用户名
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDA_MAX_PWD_LEN)]
        public string szPassword;	//密码
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDA_MAX_NAME_LEN)]
        public string szDeptName;	//所属组织名称
        public Int32 nDeptId;					//所属组织编号
        public Int32 nUserGroupId;				//用户组或者角色（1:管理员,2:普通用户）
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDA_MAX_CFGDATA_LEN)]
        public string szCreateTime;//创建时间
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDA_MAX_CFGDATA_LEN)]
        public string szModifyTime;//修改时间
    };

    //登陆信息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_LOGIN_INFO
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_IPADDR_LEN)]
        public string szDevIp;	//设备IP地址
        public UInt16 wDevPort;							//端口
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_NAME_LEN)]
        public string szUserName;	//用户名称
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_PWD_LEN)]
        public string szAuthPwd;	//认证密码
    };
    #endregion

    #region 回调定义

    /// <summary>
    /// 网络连接断开回调函数
    /// </summary>
    /// <param name="dwUserData"></param>
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    internal unsafe delegate void TfuncDisConnectNtfCB(UInt32 dwUserData);

    #endregion

    internal partial class IVXSDKProtocol
    {
        #region sdk初始化
        /************************************************************************
		 ** 初始化和连接接口
		 ***********************************************************************/
        //初始化
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_Init(bool bCreateJVM);
        //退出
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_Cleanup();

        //获取SDK的版本信息
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern string VdaSdk_GetSdkVersion();
        //获取错误码
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 VdaSdk_GetLastError();
        //获取错误码描述
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern string VdaSdk_GetErrorMsg(UInt32 dwErrCode);


        /*===========================================================
        功  能：登陆请求
        参  数：ptLoginInfo - 登陆信息，具体见VDASDK_LOGIN_INFO定义
                dwTimeoutS - 超时时间（秒）
                pFuncDisConnectNtf: 断开连接通知回调函数
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_Login(TVDASDK_LOGIN_INFO tLoginInfo, UInt32 dwTimeoutS,
                                      TfuncDisConnectNtfCB pFuncDisConnectNtf, UInt32 dwUserData);

        /*===========================================================
        功  能：注销登陆
        参  数：lLoginID - 登陆ID，VdaSdk_DevLogin的返回值
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_Logout();

        /*===========================================================
        功  能：获取登录用户类型
        参  数：无
        返回值：用户角色类型，见E_VDA_USER_ROLE_TYPE定义
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_GetLoginUserType(out UInt32 pdwUserType);

        /*===========================================================
        功  能：获取登录中心服务器的版本
        参  数：无
        返回值：版本字符串
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern string VdaSdk_GetServerVersion();

        // 获取当前用户信息(收到WM_VDA_CURRENTUSER消息后即可获取)
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_GetCurUserInfo(out TUSER_INFO pUserInfo);



        #endregion
        //#region 获取拓扑信息接口
        ///************************************************************************
        // * 获取拓扑信息接口
        // ***********************************************************************/
        ///*===========================================================
        //功  能：获取案件拓扑摄像机组
        //参  数：无
        //返回值：-1表示失败，其他值表示返回的查询标示值。获取错误码调用VdaSdk_GetLastError
        //===========================================================*/
        //[DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        //public static extern Int32 VdaSdk_QueryCaseCameraGroupList(UInt32 dwParentCameraGroupID);
        ////查询下一个拓扑（遍历接口）
        //[DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        //public static extern bool VdaSdk_QueryNextCaseCameraGroup(Int32 lQueryHandle,
        //                                                     out TVDASDK_CAMERA_GROUP_INFO ptCameraGroupInfo);
        ////关闭任务关联拓扑查询
        //[DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        //public static extern bool VdaSdk_CloseCaseCameraGroupQuery(Int32 lQueryHandle);

        ///*===========================================================
        //功  能：获取案件拓扑摄像机
        //参  数：dwCameraGroupID - 监控点分组ID（传0表示查询用户关联的所有监控点）
        //返回值：-1表示失败，其他值表示返回的查询标示值。获取错误码调用VdaSdk_GetLastError
        //===========================================================*/
        //[DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        //public static extern Int32 VdaSdk_QueryCaseCameraList(UInt32 dwCameraGroupID);
        ////查询下一个监控点（遍历接口）
        //[DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        //public static extern bool VdaSdk_QueryNextCaseCamera(Int32 lQueryHandle, out TVDASDK_CAMERA_INFO ptCameraInfo);
        ////关闭监控点查询
        //[DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        //public static extern bool VdaSdk_CloseCaseCameraQuery(Int32 lQueryHandle);


        //#endregion

    }


    public partial class IVXProtocol
    {

        #region sdk初始化
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        private bool Init(bool bCreateJVM)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_Init");

            bool retVal = IVXSDKProtocol.VdaSdk_Init(bCreateJVM);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_Init ret:" + retVal);
            return retVal;
        }

        /// <summary>
        /// 退出,清理资源
        /// </summary>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool Cleanup()
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_Cleanup");
            bool retVal = IVXSDKProtocol.VdaSdk_Cleanup();
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_Cleanup ret:" + retVal);
            return retVal;
        }


        /// <summary>
        /// 获取SDK的版本信息
        /// </summary>
        /// <returns>版本信息</returns>
        public static string GetSdkVersion()
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_GetSdkVersion");
            string retVal = IVXSDKProtocol.VdaSdk_GetSdkVersion();
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_GetSdkVersion ret:" + retVal);
            return retVal;
        }

        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <param name="errCode">输出参数,错误码编号</param>
        /// <returns>错误信息</returns>
        public string GetLastError(out UInt32 errCode)
        {
            errCode = IVXSDKProtocol.VdaSdk_GetLastError();
            MyLog4Net.ILogExtension.ErrorWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_GetLastError ret:" + errCode);

            string retVal = IVXSDKProtocol.VdaSdk_GetErrorMsg(errCode);
            MyLog4Net.ILogExtension.ErrorWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_GetErrorMsg ret:" + retVal);
            return retVal;
        }

        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <param name="errCode">输出参数,错误码编号</param>
        /// <returns>错误信息</returns>
        public static string GetErrorMsg(UInt32 errCode)
        {
            string retVal = IVXSDKProtocol.VdaSdk_GetErrorMsg(errCode);
            MyLog4Net.ILogExtension.ErrorWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_GetErrorMsg ret:" + retVal);
            return retVal;
        }

        /// <summary>
        /// 登陆请求
        /// </summary>
        /// <param name="devIp">ip</param>
        /// <param name="devPort">端口</param>
        /// <param name="userName">用户名</param>
        /// <param name="authPwd">密码</param>
        /// <param name="timeOutS">超时时间（秒）</param>
        /// <param name="disConnectd">断开连接通知回调函数</param>
        /// <param name="userData">用户参数</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool Login(string devIp,
                                        UInt16 devPort,
                                        string userName,
                                        string authPwd,
                                        UInt32 timeOutS,
                                        UInt32 userData)
        {
            TVDASDK_LOGIN_INFO info = new TVDASDK_LOGIN_INFO();
            info.szDevIp = devIp;
            info.szAuthPwd = authPwd;
            info.szUserName = userName;
            info.wDevPort = devPort;
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_Login chDevIp:{0},wDevPort:{1},szUserName:{2},szAuthPwd:{3},dwTimeoutS:{4},dwUserData:{5}",
                            devIp, devPort, userName, authPwd, timeOutS, userData));

            m_TfuncDisConnectNtfCB = TfuncDisConnectNtfCB;

            bool retVal = IVXSDKProtocol.VdaSdk_Login(info, timeOutS, m_TfuncDisConnectNtfCB, userData);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_Login ret:" + retVal);
            return retVal;
        }

        /// <summary>
        /// 注销登陆
        /// </summary>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool Logout()
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_Logout");
            bool retVal = IVXSDKProtocol.VdaSdk_Logout();
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_Logout ret:" + retVal);
            return retVal;

        }


        /// <summary>
        /// 获取登录用户类型
        /// </summary>
        /// <returns>用户角色类型，见E_VDA_USER_ROLE_TYPE定义</returns>
        public E_VDA_USER_ROLE_TYPE GetLoginUserType()
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_GetLoginUserType");
            uint dwUserType;
            bool retVal = IVXSDKProtocol.VdaSdk_GetLoginUserType(out dwUserType);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_GetLoginUserType ret:" + retVal);
            return (E_VDA_USER_ROLE_TYPE)dwUserType;
        }


        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <param name="ptUserInfo"></param>
        /// <returns>用户信息</returns>

        /// <summary>
        /// 获取登录中心服务器的版本
        /// </summary>
        /// <returns>版本字符串</returns>
        public string GetServerVersion()
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_GetServerVersion");
            string retVal = IVXSDKProtocol.VdaSdk_GetServerVersion();
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_GetServerVersion ret:" + retVal);
            return retVal;
        }




        #endregion
 
    }

}
