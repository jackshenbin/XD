using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using BOCOM.IVX.Protocol.Model;
using System.Text;
using System.Collections.Generic;
using BOCOM.DataModel;

namespace BOCOM.IVX.Protocol
{
    #region 基本结构体

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_GENERAL_PATH
    {
        public UInt32 dwGeneralPathType;	//通用路径类型：0：本地文件；1：远程文件；2本地目录；3；远程目录
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_URL_LEN)]
        public string szGeneralPath; //路径
    };

    // 任务详细信息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_TASK_INFO
    {
        public UInt32 dwTaskID;	//任务ID
        public TVDASDK_TASK_BASE tTaskBaseInfo; //任务基本信息

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_NAME_LEN)]
        public string szCreateUserName;	//创建任务用户名称
        public UInt32 dwCreateTime;		//创建任务时间
        public UInt32 dwCompleteTime;	//完成任务时间
        public UInt32 dwStatus;			//任务状态，见vdacomm.h中E_VDA_TASK_STATUS定义
        public UInt32 dwProgress;		//任务进度的千分比
        public UInt32 dwTotalLeftTimeS;		//任务总体估算剩余时间，秒
    };

    //任务基本信息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_TASK_BASE
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_NAME_LEN)]
        public string szTaskName; //任务名
        public UInt32 dwTaskPriorityLevel;	//任务优先级（1 - 10），1为最高级别，建议缺省任务优先级为5
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_DESCRIPTION_INFO_LEN)]
        public string szTaskDescription;//任务描述（备注）
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_TASK_STATUSCOUNT_INFO
    {
        public UInt32 dwTaskID; //任务ID 
        public UInt32 dwTotalCount; //任务单元总数 
        public UInt32 dwFailedCount; //失败任务单元数量 
        public UInt32 dwFinishCount; //已完成任务单元数量 
        public UInt32 dwProcessCount; //正在进行的任务单元数量 
    };

    // 任务单元详细信息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_TASK_UNIT_INFO
    {
        public UInt32 dwTaskID;	//所属任务ID
        public UInt32 dwTaskUnitID;	//任务单元ID
        public UInt32 dwCameraID;	//摄像机ID
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_TASK_UNIT_NAME_LEN)]
        public string szTaskUnitName; //任务单元名
        public UInt64 qwTaskUnitSize;		//任务单元大小,单位：字节
        public UInt32 dwTaskUnitType;		//任务单元类型，见vdacomm.h中E_VDA_TASK_UNIT_TYPE定义
        public TVDASDK_FILE_PATH tOriginFilePach; //任务单元原始路径，主要用于断点续传

        public UInt32 dwStartTime;		//任务单元开始时间
        public UInt32 dwEndTime;		//任务单元结束时间

        public UInt32 dwImportStatus;	//任务单元导入状态，见vdacomm.h中E_VDA_TASK_UNIT_IMPORT_STATUS定义
        public UInt32 dwVideoAnalyzeTypeNum;		//分析了几种算法
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.VDASDK_VIDEO_ANALYZE_TYPE_NUM)]
        public TVDASDK_TASK_UNIT_ANALYZE_STATUS[] atAnalyzeStatus;	//各类算法分析状态
        public UInt32 dwProgress;		//任务单元进度的千分比
        public UInt32 dwLeftTimeS;		//任务单元估算剩余时间，秒
    };


    // 任务单元状态
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_TASK_UNIT_ANALYZE_STATUS
    {
        public UInt32 dwAnalyzeType;	//分析算法类型，见定义
        public UInt32 dwAnalyzeStatus;	//分析状态，见vdacomm.h中E_VDA_TASK_UNIT_ANALYZE_STATUS定义
    };

    // 文件路径
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_FILE_PATH
    {
        public UInt32 dwFilePathType;	//文件路径类型：0：本地文件，1：远程文件
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_URL_LEN)]
        public string szFilePath; //路径
    };

    // 分析类型信息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TVDASDK_ANALYZE_TYPE_INFO
    {
        public UInt32 dwAnalyzeTypeNum;		//分析几种算法
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.VDASDK_ANALYZE_TYPE_MAXNUM)]
        public UInt32[] adwAnalyzeType; //具体分析的类型
    };
    // 本地视频文件导入信息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_LOCAL_VIDEO_FILE_IMPORT_INFO
    {
        public UInt32 dwCameraID;	//摄像机编号
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_TASK_UNIT_NAME_LEN)]
        public string szTaskUnitName; //任务单元名
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_FILEPATH_LEN)]
        public string szLocalFilePath; //文件路径
        public UInt64 qwFileSize;		//文件大小,单位：字节
        public UInt32 dwAdjustStartTime;			//校准的开始时间

        public TVDASDK_ANALYZE_TYPE_INFO tVideoAnalyzeInfo; // 视频分析信息
    };


    // 远程视频文件导入信息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal  struct TVDASDK_REMOTE_VIDEO_FILE_IMPORT_INFO
    {
        public UInt32 dwCameraID;	//摄像机编号
	    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_TASK_UNIT_NAME_LEN)]
        public string  szTaskUnitName; //任务单元名
	    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_URL_LEN)]
        public string  szRemoteFileURL; //文件URL
        public UInt64 qwFileSize;		//文件大小,单位：字节
        public UInt32 dwAdjustStartTime;			//校准的开始时间

	    public TVDASDK_ANALYZE_TYPE_INFO tVideoAnalyzeInfo; // 视频分析信息
    };

    //// 网络存储视频导入信息
    //typedef struct 
    //{
    //    DWORD dwCameraID;			//所属摄像机编号
    //    TCHAR szTaskUnitName[VDASDK_MAX_NAME_LEN]; //任务单元名
    //    DWORD dwStartTime;			//开始时间
    //    DWORD dwEndTime;			//结束时间
    //    DWORD dwAdjustStartTime;	//校准的开始时间

    //    //DWORD dwVideoSupplierDeviceId;	//关联的网络存储设备ID
    //    TCHAR szVideoSupplierChannelId[VDASDK_MAX_NAME_LEN]; //存储设备点位标示

    //    TVDASDK_ANALYZE_TYPE_INFO tVideoAnalyzeInfo; // 视频分析信息
    //}TVDASDK_NETSTORE_VIDEO_IMPORT_INFO;


    // 图片包信息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_PIC_PACKAGE_INFO
    {
        public UInt32 dwCameraID;			//所属摄像机编号
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_TASK_UNIT_NAME_LEN)]
        public string szTaskUnitName; //任务单元名
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_FILEPATH_LEN)]
        public string szPackageOriginPath; //图片包原始路径
        public UInt32 dwPackagePicFileNum;					//图片数量
        public UInt64 qwPackagePicFileSize;				//图片包中图片文件的总大小,单位：字节

        public UInt32 dwStartTime;			//开始时间
        public UInt32 dwEndTime;			//结束时间

        public TVDASDK_ANALYZE_TYPE_INFO tPicAnalyzeInfo; // 图片分析信息
    };

    #endregion

    #region 第三方平台导入视频相关结构体

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TVDASDK_NETSTORE_VIDEO_IMPORT_INFO
    {
        public uint dwCameraID;			//所属摄像机编号
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_TASK_UNIT_NAME_LEN)]
        public string szTaskUnitName; //任务单元名
        public uint dwStartTime;			//开始时间
        public uint dwEndTime;			//结束时间
        public uint dwAdjustStartTime;	//校准的开始时间
        public ulong qwFileSize;	        //文件大小,单位：字节
        public TVDASDK_ANALYZE_TYPE_INFO tVideoAnalyzeInfo; // 视频分析信息
    };
    
    // 网络存储设备基本信息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TVDASDK_NET_STORE_DEV_BASE
    {
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_NET_STORE_DEV_NAME)]
        //public string szDeviceName;	//设备名称

        //public UInt32 dwAccessProtocolType;					//接入厂商协议类型 E_VDA_ACCESS_PTOTOCOL_TYPE

        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_IPADDR_LEN)]
        //public string szDeviceIP;	//连接IP地址

        //public UInt32 dwDevicePort;							//连接端口号

        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_NAME_LEN)]
        //public string szLoginUser;		//登录用户

        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_PWD_LEN)]
        //public string szLoginPwd;		//登录密码

        public UInt32 dwDevicePort;							//连接端口号

        public UInt32 dwAccessProtocolType;					//接入厂商协议类型 E_VDA_ACCESS_PTOTOCOL_TYPE

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_NET_STORE_DEV_NAME)]
        public string szDeviceName;	//设备名称

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_IPADDR_LEN)]
        public string szDeviceIP;	//连接IP地址

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_NAME_LEN)]
        public string szLoginUser;		//登录用户

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_PWD_LEN)]
        public string szLoginPwd;		//登录密码
    };

    // 网络存储设备信息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_NET_STORE_DEV_INFO
    {
        public UInt32 dwVideoSupplierDeviceId;							//网络存储设备ID
        public TVDASDK_NET_STORE_DEV_BASE tNetStoreDevBase;	//网络存储设备基本信息
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TVDASDK_NET_STORE_DEV_LOGIN_INFO

    {
        public TVDASDK_NET_STORE_DEV_BASE tDevBase;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TVDASDK_NET_STORE_DEV_CHANNEL_INFO
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDA_MAX_FILEPATH_LEN)]
        public string szChannelId;  //通道Id
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDA_MAX_FILEPATH_LEN)]
        public string szChannelName;//通道名
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDA_MAX_FILEPATH_LEN)]
        public string szRest;		  //保留信息
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TVDASDK_NET_STORE_DEV_FILE_CONDITION
    {
        public uint dwStartTime; //平台文件开始时间
        public uint dwEndTime;   //平台文件结束时间
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDA_MAX_FILEPATH_LEN)]
        public string szChannelId;  //通道Id
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TVDASDK_NET_STORE_DEV_FILE_INFO
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDA_MAX_FILEPATH_LEN)]
        public string szFileId;	//第3方设备/平台上的文件标识
        public uint tStartTime;						//文件录像起始时间
        public uint tEndTime;						    //文件录像结束时间
        public ulong qwFileSize;						//文件大小
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDA_MAX_FILEPATH_LEN)]
        public string szRest;		//保留信息
    }

    #endregion

    #region 回调定义
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    internal unsafe delegate void TfuncTaskStatusNtfCB(UInt32 dwTaskID, UInt32 dwTaskStatus, UInt32 dwUserData);

    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    internal unsafe delegate void TfuncTaskProgressNtfCB(UInt32 dwTaskID, UInt32 dwTaskProgress, UInt32 dwTaskLeftTimeS, UInt32 dwUserData);

    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    internal unsafe delegate void TfuncTaskUnitStatusNtfCB(UInt32 dwTaskUnitID, UInt32 dwTaskUnitImportStatus, IntPtr/* TVDASDK_TASK_UNIT_ANALYZE_STATUS*  */ pAnalyzeStatus, UInt32 dwAnalyzeStatusNumber, UInt32 dwUserData);

    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    internal unsafe delegate void TfuncTaskUnitProgressNtfCB(UInt32 dwTaskUnitID, UInt32 dwTaskUnitProgress, UInt32 dwTaskUnitLeftTimeS, UInt32 dwUserData);

    #endregion

    internal partial class IVXSDKProtocol
    {
        #region 任务接口

        /// <summary>
        /// 查询任务信息列表
        /// </summary>
        /// <returns>-1表示失败，其他值表示返回的查询标示值。获取错误码调用VdaSdk_GetLastError</returns>
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 VdaSdk_QueryTaskList();

        /// <summary>
        /// 查询任务总数 
        /// </summary>
        /// <param name="lQueryHandle"></param>
        /// <returns></returns>
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 VdaSdk_GetTaskTotalNum(Int32 lQueryHandle);

        /// <summary>
        /// 查询下一个任务（遍历接口）
        /// </summary>
        /// <param name="lQueryHandle"></param>
        /// <param name="ptTaskInfo"></param>
        /// <returns></returns>
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_QueryNextTask(Int32 lQueryHandle, out TVDASDK_TASK_INFO ptTaskInfo);

        /// <summary>
        ///关闭任务查询
        /// </summary>
        /// <param name="lQueryHandle"></param>
        /// <returns></returns>
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_CloseTaskListQuery(Int32 lQueryHandle);

        /// <summary>
        /// 获取指定任务详细信息
        /// </summary>
        /// <param name="dwTaskID"></param>
        /// <param name="ptTaskInfo"></param>
        /// <returns></returns>
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_GetTaskByID(UInt32 dwTaskID, out TVDASDK_TASK_INFO ptTaskInfo);

        /// <summary>
        /// 查询任务状态统计详细信息
        /// </summary>
        /// <param name="dwTaskID">任务ID</param>
        /// <param name="ptTaskStatusCountInfo">任务状态统计详细信息</param>
        /// <returns>成功返回的TRUE，失败返回FALSE，获取错误码调用VdaSdk_GetLastError。</returns>
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_GetTaskStatusCount( UInt32 dwTaskID, ref TVDASDK_TASK_STATUSCOUNT_INFO ptTaskStatusCountInfo );

        /// <summary>
        /// 查询正在导入的任务单元ID列表
        /// </summary>
        /// <param name="dwMaxNum">任务单元最多返回个数</param>
        /// <param name="ptTaskUnitID">任务单元列表</param>
        /// <param name="pdwTaskUnitNum">任务单元数量</param>
        /// <returns>成功返回的TRUE，失败返回FALSE，获取错误码调用VdaSdk_GetLastError。</returns>
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_GetImportingTaskUnitList(UInt32 dwMaxNum, IntPtr ptTaskUnitID, out uint pdwTaskUnitNum);

        /// <summary>
        /// 查询任务单元信息列表
        /// </summary>
        /// <param name="dwTaskID">任务编号</param>
        /// <returns>1表示失败，其他值表示返回的查询标示值。获取错误码调用VdaSdk_GetLastError</returns>
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 VdaSdk_QueryTaskUnitList(UInt32 dwTaskID);

        /// <summary>
        /// 查询任务总数
        /// </summary>
        /// <param name="lQueryHandle"></param>
        /// <returns></returns>
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 VdaSdk_GetTaskUnitTotalNum(Int32 lQueryHandle);

        /// <summary>
        /// 查询下一个任务单元（遍历接口）
        /// </summary>
        /// <param name="lQueryHandle"></param>
        /// <param name="ptTaskUnitInfo"></param>
        /// <returns></returns>
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_QueryNextTaskUnit(Int32 lQueryHandle, out TVDASDK_TASK_UNIT_INFO ptTaskUnitInfo);

        /// <summary>
        /// 关闭任务单元查询
        /// </summary>
        /// <param name="lQueryHandle"></param>
        /// <returns></returns>
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_CloseTaskUnitListQuery(Int32 lQueryHandle);

        /// <summary>
        /// 获取指定任务单元详细信息
        /// </summary>
        /// <param name="dwTaskID"></param>
        /// <param name="dwTaskUnitID"></param>
        /// <param name="ptTaskUnitInfo"></param>
        /// <returns></returns>
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_GetTaskUnitByID(UInt32 dwTaskUnitID,
                         out TVDASDK_TASK_UNIT_INFO ptTaskUnitInfo);

        /// <summary>
        /// 新增任务
        /// </summary>
        /// <param name="tTaskBaseInfo">任务基本信息</param>
        /// <param name="pdwTaskID">返回任务编号</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_AddTask(TVDASDK_TASK_BASE tTaskBaseInfo, out UInt32 pdwTaskID);

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="dwTaskID"></param>
        /// <returns></returns>
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_DelTask(UInt32 dwTaskID);

        /// <summary>
        /// 修改任务
        /// </summary>
        /// <param name="dwTaskID"></param>
        /// <param name="tTaskBaseInfo"></param>
        /// <returns></returns>
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_MdfTask(UInt32 dwTaskID, TVDASDK_TASK_BASE tTaskBaseInfo);

        /// <summary>
        /// 修改任务优先级
        /// </summary>
        /// <param name="dwTaskID"></param>
        /// <param name="dwTaskPriorityLevel"></param>
        /// <returns></returns>
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_MdfTaskPriority(UInt32 dwTaskID, UInt32 dwTaskPriorityLevel);

        /// <summary>
        /// 注册任务状态回调函数和任务进度回调函数
        /// </summary>
        /// <param name="pfuncTaskStatusNtf"></param>
        /// <param name="dwUserData"></param>
        /// <returns></returns>
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_TaskStatusNtfCBReg(TfuncTaskStatusNtfCB pfuncTaskStatusNtf, UInt32 dwUserData);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pfuncTaskProgressNtf"></param>
        /// <param name="dwUserData"></param>
        /// <returns></returns>
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_TaskProgressNtfCBReg(TfuncTaskProgressNtfCB pfuncTaskProgressNtf, UInt32 dwUserData);

        /// <summary>
        /// 增加本地视频文件导入到任务中
        /// </summary>
        /// <param name="dwTaskID">任务编号</param>
        /// <param name="dwFileNum">本地上传文件信息列表</param>
        /// <param name="ptFileInfoList">返回任务单元编号列表</param>
        /// <param name="pdwTaskUnitIDList">成功返回TRUE，失败返回FALSE</param>
        /// <returns></returns>
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_AddLocalVideoFileImportToTask(UInt32 dwTaskID, UInt32 dwFileNum,
                                IntPtr/* TVDASDK_LOCAL_VIDEO_FILE_IMPORT_INFO* */ ptFileInfoList, IntPtr/* DWORD* */ pdwTaskUnitIDList);

        /*===========================================================
        功  能：增加文件服务器视频文件导入到任务中
        参  数：dwTaskID - 任务编号
                dwFileNum - 文件数量
                ptFileInfoList - 本地上传文件信息列表
                pdwTaskUnitIDList - 返回任务单元编号列表
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/

        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_AddRemoteVideoFileImportToTask(UInt32 dwTaskID,
                          UInt32 dwFileNum, IntPtr/* TVDASDK_REMOTE_VIDEO_FILE_IMPORT_INFO* */ ptFileInfoList,
                          IntPtr/* DWORD* */ pdwTaskUnitIDList);

        /*===========================================================
        功  能：增加网络存储视频导入到任务中
        参  数：dwTaskID - 任务编号
                dwVideoNum - 指定时间段视频数量
                ptVideoList - 视频列表
                pdwTaskUnitIDList - 任务单元数据
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_AddNetStoreVideoImportToTask(UInt32 dwTaskID,  UInt32 dwVideoNum,
                         IntPtr/* TVDASDK_NETSTORE_VIDEO_IMPORT_INFO* */ ptNetStoreVideoList, IntPtr/* DWORD* */ pdwTaskUnitIDList);


        /*===========================================================
        功  能：增加本地图片包导入到任务中(一次导入一个图片包）
        参  数：dwTaskID - 任务编号
                tPicPackageInfo - 图片包信息
                dwPicNum - 图片数量
                tFilePatch - 图片路径列表
                pdwTaskUnitID - 返回任务单元编号
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_AddLocalPicPackageImportToTask(UInt32 dwTaskID,
                          TVDASDK_PIC_PACKAGE_INFO tPicPackageInfo, UInt32 dwPathNum,
                          TVDASDK_GENERAL_PATH tGeneralPath, out IntPtr/* DWORD* */ pdwTaskUnitID);

        /*===========================================================
        功  能：增加文件服务器图片包导入到任务中(一次导入一个图片包）
        参  数：dwTaskID - 任务编号
                tPicPackageInfo - 图片包信息
                dwPicNum - 图片数量
                tFilePatch - 图片路径列表
                pdwTaskUnitID - 返回任务单元编号
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_AddRemotePicPackageImportToTask(UInt32 dwTaskID,
                          TVDASDK_PIC_PACKAGE_INFO tPicPackageInfo, UInt32 dwPathNum,
                          TVDASDK_GENERAL_PATH tGeneralPath, out IntPtr/* DWORD* */ pdwTaskUnitID);

        /*===========================================================
        功  能：删除任务单元
        参  数：dwTaskID - 任务编号
                dwTaskUnitID - 任务单元编号
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_DelTaskUnit(UInt32 dwTaskUnitID);

        /*===========================================================
        功  能：增加任务单元分析算法
        参  数：dwTaskUnitID - 任务单元编号
                tAnalyzeTypeInfo - 分析类型信息
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_AddTaskUnitAnalyzeType(UInt32 dwTaskUnitID, TVDASDK_ANALYZE_TYPE_INFO tAnalyzeTypeInfo);

        /*===========================================================
        功  能：修改任务单元矫正时间
        参  数：dwTaskUnitID 任务单元编号
		        dwAdjustTime 矫正时间
        返回值：成功返回TRUE，失败返回FALSE，获取错误码调用VdaSdk_GetLastError。
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_SetTaskUnitAdjustTime(UInt32 dwTaskUnitID, UInt32 dwAdjustTime);


        //注册任务单元状态回调函数和任务单元进度回调函数
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_TaskUnitStatusNtfCBReg(TfuncTaskUnitStatusNtfCB pfuncTaskUnitStatusNtf, UInt32 dwUserData);
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_TaskUnitProgressNtfCBReg(TfuncTaskUnitProgressNtfCB pfuncTaskUnitProgressNtf, UInt32 dwUserData);



        /*===========================================================
        功  能：查询指定监控点下的视频(任务单元）信息列表，包括多个任务汇总的资源列表
        参  数：dwCameraID - 监控点编号（传0表示所有获取没有关联监控点位的视频）
        返回值：-1表示失败，其他值表示返回的查询标示值。获取错误码调用VdaSdk_GetLastError
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 VdaSdk_QueryVideoTaskUnitListByCamera(UInt32 dwCameraID);

        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 VdaSdk_QueryVideoTaskUnitListByInvalidCamera();

        //查询下一个指定监控点下的视频(任务单元）信息（遍历接口）
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_QueryCameraNextVideoTaskUnit(Int32 lQueryHandle, out TVDASDK_TASK_UNIT_INFO ptTaskUnitInfo);
        //关闭指定监控点下的视频(任务单元）信息查询
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_CloseCameraVideoTaskUnitQuery(Int32 lQueryHandle);

        /*===========================================================
        功  能：查询指定监控点下的图片包(任务单元）信息列表，包括多个任务汇总的资源列表
        参  数：dwCameraID - 监控点编号（传0表示所有获取没有关联监控点位的视频）
        返回值：-1表示失败，其他值表示返回的查询标示值。获取错误码调用VdaSdk_GetLastError
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 VdaSdk_QueryPicPackageTaskUnitListByCamera(UInt32 dwCameraID);
        //查询下一个指定监控点下的图片包(任务单元）信息（遍历接口）
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_QueryCameraNextPicPackageTaskUnit(Int32 lQueryHandle, out TVDASDK_TASK_UNIT_INFO ptTaskUnitInfo);
        //关闭指定监控点下的图片包(任务单元）信息查询
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_CloseCameraPicPackageTaskUnitQuery(Int32 lQueryHandle);

        #endregion
    }

    public partial class IVXProtocol
    {
        #region 任务接口
        /// <summary>
        /// 查询任务信息列表
        /// </summary>
        /// <returns>-1表示失败，其他值表示返回的查询标示值</returns>
        public Int32 QueryTaskList()
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_QueryTaskList ");
            Int32 retVal = IVXSDKProtocol.VdaSdk_QueryTaskList();
            if (-1 == retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_QueryTaskList ret:" + retVal);
            return retVal;

        }

        /// <summary>
        /// 查询任务总数
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>任务总数</returns>
        public UInt32 GetTaskTotalNum(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_GetTaskTotalNum lQueryHandle:" + queryHandle);
            UInt32 retVal = IVXSDKProtocol.VdaSdk_GetTaskTotalNum(queryHandle);
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_GetTaskTotalNum ret:" + retVal);
            return retVal;
        }

        /// <summary>
        /// 查询下一个任务（遍历接口）
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>任务信息</returns>
        public TaskInfo QueryNextTask(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_QueryNextTask lQueryHandle:" + queryHandle);
            TVDASDK_TASK_INFO ptTaskInfo = new TVDASDK_TASK_INFO();
            bool retVal = IVXSDKProtocol.VdaSdk_QueryNextTask(queryHandle, out ptTaskInfo);

            TaskInfo taskInfo = null;
            // 不会有SDK调用失败的情况， 因为数据已经全部取到SDK了， 不需要再跟Server交互。所以不需要CheckError

            if (retVal)
            {
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_QueryNextTask ret:{0},"
                    + "dwTaskID:{1},"
                    + "chTaskName:{2},"
                    + "dwTaskPriorityLevel:{3},"
                    + "szTaskDescription:{4},"
                    + "szCreateUserName:{5},"
                    + "dwCreateTime:{6},"
                    + "dwCompleteTime:{7},"
                    + "dwStatus:{8},"
                    + "dwProgress:{9},"
                    + "dwTotalLeftTimeS:{10}"
                    , retVal
                    , ptTaskInfo.dwTaskID
                    , ptTaskInfo.tTaskBaseInfo.szTaskName
                    , ptTaskInfo.tTaskBaseInfo.dwTaskPriorityLevel
                    , ptTaskInfo.tTaskBaseInfo.szTaskDescription
                    , ptTaskInfo.szCreateUserName
                    , ptTaskInfo.dwCreateTime
                    , ptTaskInfo.dwCompleteTime
                    , ptTaskInfo.dwStatus
                    , ptTaskInfo.dwProgress
                    , ptTaskInfo.dwTotalLeftTimeS
                    ));
                taskInfo = new TaskInfo();
                taskInfo.CompleteTime = ModelParser.ConvertLinuxTime(ptTaskInfo.dwCompleteTime);
                taskInfo.CreateTime = ModelParser.ConvertLinuxTime(ptTaskInfo.dwCreateTime);
                taskInfo.Progress = ptTaskInfo.dwProgress;
                taskInfo.Status = ptTaskInfo.dwStatus;
                taskInfo.TaskID = ptTaskInfo.dwTaskID;
                taskInfo.TotalLeftTimeS = ptTaskInfo.dwTotalLeftTimeS;
                taskInfo.CreateUserName = ptTaskInfo.szCreateUserName;
                taskInfo.TaskPriorityLevel = ptTaskInfo.tTaskBaseInfo.dwTaskPriorityLevel;
                taskInfo.TaskDescription = ptTaskInfo.tTaskBaseInfo.szTaskDescription;
                taskInfo.TaskName = ptTaskInfo.tTaskBaseInfo.szTaskName;
            }
            return taskInfo;
        }

        /// <summary>
        /// 关闭任务查询
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool CloseTaskListQuery(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_CloseTaskListQuery lQueryHandle:" + queryHandle);
            bool retVal = IVXSDKProtocol.VdaSdk_CloseTaskListQuery(queryHandle);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_CloseTaskListQuery ret:" + retVal);
            return retVal;

        }

        /// <summary>
        /// 获取指定任务详细信息
        /// </summary>
        /// <param name="taskID">任务编号</param>
        /// <returns>任务信息</returns>
        public TaskInfo GetTaskByID(UInt32 taskID)
        {
            // MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_GetTaskByID taskID:" + taskID);
            TVDASDK_TASK_INFO ptTaskInfo = new TVDASDK_TASK_INFO();
            bool retVal = IVXSDKProtocol.VdaSdk_GetTaskByID(taskID, out ptTaskInfo);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
                // 如果不抛异常， 应该是记录不存在, 返回 null
                return null;
            }

            //MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_GetTaskByID ret:{0},"
            //    + "dwTaskID:{1},"
            //    + "chTaskName:{2},"
            //    + "dwTaskPriorityLevel:{3},"
            //    + "szTaskDescription:{4},"
            //    + "szCreateUserName:{5},"
            //    + "dwCreateTime:{6},"
            //    + "dwCompleteTime:{7},"
            //    + "dwStatus:{8},"
            //    + "dwProgress:{9},"
            //    + "dwTotalLeftTimeS:{10}"
            //    , retVal
            //    , ptTaskInfo.dwTaskID
            //    , ptTaskInfo.tTaskBaseInfo.szTaskName
            //    , ptTaskInfo.tTaskBaseInfo.dwTaskPriorityLevel
            //    , ptTaskInfo.tTaskBaseInfo.szTaskDescription
            //    , ptTaskInfo.szCreateUserName
            //    , ptTaskInfo.dwCreateTime
            //    , ptTaskInfo.dwCompleteTime
            //    , ptTaskInfo.dwStatus
            //    , ptTaskInfo.dwProgress
            //    , ptTaskInfo.dwTotalLeftTimeS
            //    ));
            TaskInfo taskInfo = new TaskInfo();
            taskInfo.CompleteTime = ModelParser.ConvertLinuxTime(ptTaskInfo.dwCompleteTime);
            taskInfo.CreateTime = ModelParser.ConvertLinuxTime(ptTaskInfo.dwCreateTime);
            taskInfo.Progress = ptTaskInfo.dwProgress;
            taskInfo.Status = ptTaskInfo.dwStatus;
            taskInfo.TaskID = ptTaskInfo.dwTaskID;
            taskInfo.TotalLeftTimeS = ptTaskInfo.dwTotalLeftTimeS;
            taskInfo.CreateUserName = ptTaskInfo.szCreateUserName;
            taskInfo.TaskPriorityLevel = ptTaskInfo.tTaskBaseInfo.dwTaskPriorityLevel;
            taskInfo.TaskDescription = ptTaskInfo.tTaskBaseInfo.szTaskDescription;
            taskInfo.TaskName = ptTaskInfo.tTaskBaseInfo.szTaskName;
            return retVal ? taskInfo : null;

        }


        /// <summary>
        /// 查询任务状态统计详细信息
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <param name="totalCount">任务总数</param>
        /// <param name="failedCount">失败数</param>
        /// <param name="processCount">进行数</param>
        /// <param name="finishCount">完成数</param>
        /// <returns>成功返回的TRUE，失败返回FALSE</returns>
        public bool GetTaskStatusCount(UInt32 taskID,out uint totalCount,out uint failedCount,out uint processCount,out uint finishCount)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_GetTaskStatusCount dwTaskID:" + taskID);
            TVDASDK_TASK_STATUSCOUNT_INFO ptTaskStatusCountInfo = new TVDASDK_TASK_STATUSCOUNT_INFO();
            bool retVal = IVXSDKProtocol.VdaSdk_GetTaskStatusCount(taskID, ref ptTaskStatusCountInfo);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            totalCount = ptTaskStatusCountInfo.dwTotalCount;
            failedCount = ptTaskStatusCountInfo.dwFailedCount;
            processCount = ptTaskStatusCountInfo.dwProcessCount;
            finishCount = ptTaskStatusCountInfo.dwFinishCount;
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_GetTaskStatusCount ret:{0},"
                + "dwTotalCount:{1},"
                + "dwFailedCount:{2},"
                + "dwProcessCount:{3},"
                + "dwFinishCount:{4},"
                , retVal
                , ptTaskStatusCountInfo.dwTotalCount
                , ptTaskStatusCountInfo.dwFailedCount
                , ptTaskStatusCountInfo.dwProcessCount
                , ptTaskStatusCountInfo.dwFinishCount
                ));
            return retVal;

        }

        /// <summary>
        /// 查询正在导入的任务单元ID列表
        /// </summary>
        /// <param name="taskUnitIDList">任务单元列表</param>
        /// <returns>成功返回的TRUE，失败返回FALSE</returns>
        public bool GetImportingTaskUnitList(out List<uint> taskUnitIDList)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_GetImportingTaskUnitList");

            IntPtr ptTaskUnitID = Marshal.AllocHGlobal(10* sizeof(uint));
            uint pdwTaskUnitNum = 0;

            bool retVal = IVXSDKProtocol.VdaSdk_GetImportingTaskUnitList(10, ptTaskUnitID, out pdwTaskUnitNum);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }

            StringBuilder sb = new StringBuilder();
                        taskUnitIDList = new List<uint>();
            sb.Clear();
            for (int i = 0; i < Math.Min( pdwTaskUnitNum,10); i++)
            {
                uint unitid = (uint)Marshal.PtrToStructure(ptTaskUnitID + Marshal.SizeOf(typeof(uint)) * i, typeof(uint));
                sb.AppendFormat("IVXSDKProtocol VdaSdk_GetImportingTaskUnitList pdwTaskUnitIDList[{0}]:{1}" + Environment.NewLine, i, unitid);
                if (!taskUnitIDList.Contains(unitid))
                    taskUnitIDList.Add(unitid);
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,sb.ToString());
            Marshal.FreeHGlobal(ptTaskUnitID);

            return retVal;
        }



        /// <summary>
        /// 查询任务单元信息列表
        /// </summary>
        /// <param name="taskID">任务编号</param>
        /// <returns>-1表示失败，其他值表示返回的查询标示值</returns>
        public Int32 QueryTaskUnitList(UInt32 taskID)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_QueryTaskUnitList dwTaskID:" + taskID);
            Int32 retVal = IVXSDKProtocol.VdaSdk_QueryTaskUnitList(taskID);
            if (-1 == retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_QueryTaskUnitList ret:" + retVal);
            return retVal;
        }

        /// <summary>
        /// 查询任务单元总数
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>任务单元总数</returns>
        public Int32 GetTaskUnitTotalNum(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_GetTaskUnitTotalNum lQueryHandle:" + queryHandle);
            Int32 retVal = IVXSDKProtocol.VdaSdk_GetTaskUnitTotalNum(queryHandle);
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_GetTaskUnitTotalNum ret:" + retVal);
            return retVal;
        }

        /// <summary>
        /// 查询下一个任务单元（遍历接口）
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>任务单元信息</returns>
        public TaskUnitInfo QueryNextTaskUnit(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_QueryNextTaskUnit lQueryHandle:" + queryHandle);
            TVDASDK_TASK_UNIT_INFO ptTaskUnitInfo = new TVDASDK_TASK_UNIT_INFO();
            bool retVal = IVXSDKProtocol.VdaSdk_QueryNextTaskUnit(queryHandle, out ptTaskUnitInfo);

            TaskUnitInfo taskUnitInfo = null;
            // 不会有SDK调用失败的情况， 因为数据已经全部取到SDK了， 不需要再跟Server交互。所以不需要CheckError

            if (retVal)
            {
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_QueryNextTaskUnit ret:{0},"
                    + "dwEndTime:{1},"
                    + "dwImportStatus:{2},"
                    + "dwLeftTimeS:{3},"
                    + "dwProgress:{4},"
                    + "dwStartTime:{5},"
                    + "dwTaskID:{6},"
                    + "dwTaskUnitID:{7},"
                    + "dwTaskUnitType:{8},"
                    + "dwVideoAnalyzeTypeNum:{9},"
                    + "dwCameraID:{10},"
                    , retVal
                    , ptTaskUnitInfo.dwEndTime
                    , ptTaskUnitInfo.dwImportStatus
                    , ptTaskUnitInfo.dwLeftTimeS
                    , ptTaskUnitInfo.dwProgress
                    , ptTaskUnitInfo.dwStartTime
                    , ptTaskUnitInfo.dwTaskID
                    , ptTaskUnitInfo.dwTaskUnitID
                    , ptTaskUnitInfo.dwTaskUnitType
                    , ptTaskUnitInfo.dwVideoAnalyzeTypeNum
                    , ptTaskUnitInfo.dwCameraID
                    ));



                //StringBuilder sb = new StringBuilder();
                //for (int i = 0; i < ptTaskUnitInfo.atAnalyzeStatus.Length; i++)
                //{
                //    TVDASDK_TASK_UNIT_ANALYZE_STATUS s = ptTaskUnitInfo.atAnalyzeStatus[i];

                //    sb.AppendFormat("IVXSDKProtocol VdaSdk_QueryNextTaskUnit ptTaskUnitInfo.atAnalyzeStatus[{0}] dwAnalyzeStatus:{1},dwAnalyzeType:{2}" + Environment.NewLine
                //                    , i
                //                    , s.dwAnalyzeStatus
                //                    , s.dwAnalyzeType);
                //}
                //MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,sb.ToString());


                taskUnitInfo = new TaskUnitInfo();
                taskUnitInfo.AnalyzeStatus = new Dictionary<E_VDA_ANALYZE_TYPE, E_VDA_TASK_UNIT_STATUS>();
                for (int i = 0; i < ptTaskUnitInfo.atAnalyzeStatus.Length; i++)
                {
                    TVDASDK_TASK_UNIT_ANALYZE_STATUS s = ptTaskUnitInfo.atAnalyzeStatus[i];
                    if (!taskUnitInfo.AnalyzeStatus.ContainsKey((E_VDA_ANALYZE_TYPE)s.dwAnalyzeType))
                        taskUnitInfo.AnalyzeStatus.Add((E_VDA_ANALYZE_TYPE)s.dwAnalyzeType, (E_VDA_TASK_UNIT_STATUS)s.dwAnalyzeStatus);
                }

                string anaStr = "";
                foreach (E_VDA_ANALYZE_TYPE type in taskUnitInfo.AnalyzeStatus.Keys)
                {
                    if (type > 0)
                    {
                        anaStr += "|" + (int)type + ":" + (int)taskUnitInfo.AnalyzeStatus[type];
                    }
                }
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, anaStr.Trim(new char[] { '|' }));


                taskUnitInfo.EndTime = ModelParser.ConvertLinuxTime(ptTaskUnitInfo.dwEndTime);
                taskUnitInfo.FilePath = ptTaskUnitInfo.tOriginFilePach.szFilePath;
                taskUnitInfo.CameraId = ptTaskUnitInfo.dwCameraID;
                taskUnitInfo.FilePathType = ptTaskUnitInfo.tOriginFilePach.dwFilePathType;
                taskUnitInfo.ImportStatus = ptTaskUnitInfo.dwImportStatus;
                taskUnitInfo.LeftTimeS = ptTaskUnitInfo.dwLeftTimeS;
                taskUnitInfo.Progress = ptTaskUnitInfo.dwProgress;
                taskUnitInfo.StartTime = ModelParser.ConvertLinuxTime(ptTaskUnitInfo.dwStartTime);
                taskUnitInfo.TaskID = ptTaskUnitInfo.dwTaskID;
                taskUnitInfo.TaskUnitID = ptTaskUnitInfo.dwTaskUnitID;
                taskUnitInfo.TaskUnitName = ptTaskUnitInfo.szTaskUnitName;
                taskUnitInfo.TaskUnitSize = ptTaskUnitInfo.qwTaskUnitSize;
                taskUnitInfo.TaskUnitType = ptTaskUnitInfo.dwTaskUnitType;
                taskUnitInfo.VideoAnalyzeTypeNum = ptTaskUnitInfo.dwVideoAnalyzeTypeNum;
            }
            return taskUnitInfo;
        }

        /// <summary>
        /// 关闭任务单元查询
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool CloseTaskUnitListQuery(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_CloseTaskUnitListQuery lQueryHandle:" + queryHandle);
            bool retVal = IVXSDKProtocol.VdaSdk_CloseTaskUnitListQuery(queryHandle);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_CloseTaskUnitListQuery ret:" + retVal);
            return retVal;
        }

        /// <summary>
        /// 获取指定任务单元详细信息
        /// </summary>
        /// <param name="taskID">任务编号</param>
        /// <param name="taskUnitID">任务单元编号</param>
        /// <returns>任务单元信息</returns>
        public TaskUnitInfo GetTaskUnitByID(UInt32 taskUnitID)
        {
            //MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_GetTaskUnitByID dwTaskUnitID:{0}", taskUnitID));
            TVDASDK_TASK_UNIT_INFO ptTaskUnitInfo = new TVDASDK_TASK_UNIT_INFO();
            bool retVal = IVXSDKProtocol.VdaSdk_GetTaskUnitByID(taskUnitID, out ptTaskUnitInfo);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
                // 如果不抛异常， 应该是记录不存在, 返回 null
                return null;
            }

            //MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_GetTaskUnitByID ret:{0},"
            //    + "dwEndTime:{1},"
            //    + "dwImportStatus:{2},"
            //    + "dwLeftTimeS:{3},"
            //    + "dwProgress:{4},"
            //    + "dwStartTime:{5},"
            //    + "dwTaskID:{6},"
            //    + "dwTaskUnitID:{7},"
            //    + "dwTaskUnitType:{8},"
            //    + "dwVideoAnalyzeTypeNum:{9}"
            //    , retVal
            //    , ptTaskUnitInfo.dwEndTime
            //    , ptTaskUnitInfo.dwImportStatus
            //    , ptTaskUnitInfo.dwLeftTimeS
            //    , ptTaskUnitInfo.dwProgress
            //    , ptTaskUnitInfo.dwStartTime
            //    , ptTaskUnitInfo.dwTaskID
            //    , ptTaskUnitInfo.dwTaskUnitID
            //    , ptTaskUnitInfo.dwTaskUnitType
            //    , ptTaskUnitInfo.dwVideoAnalyzeTypeNum
            //    ));

            TaskUnitInfo taskUnitInfo = new TaskUnitInfo();
            for (int i = 0; i < ptTaskUnitInfo.atAnalyzeStatus.Length; i++)
            {
                TVDASDK_TASK_UNIT_ANALYZE_STATUS s = ptTaskUnitInfo.atAnalyzeStatus[i];

                if (taskUnitInfo.AnalyzeStatus == null)
                {
                    taskUnitInfo.AnalyzeStatus = new Dictionary<E_VDA_ANALYZE_TYPE, E_VDA_TASK_UNIT_STATUS>();
                }

                if (s.dwAnalyzeType != 0 && !taskUnitInfo.AnalyzeStatus.ContainsKey((E_VDA_ANALYZE_TYPE)s.dwAnalyzeType))
                {
                    taskUnitInfo.AnalyzeStatus.Add((E_VDA_ANALYZE_TYPE)s.dwAnalyzeType, (E_VDA_TASK_UNIT_STATUS)s.dwAnalyzeStatus);
                }
            }
            taskUnitInfo.EndTime = ModelParser.ConvertLinuxTime(ptTaskUnitInfo.dwEndTime);
            taskUnitInfo.FilePath = ptTaskUnitInfo.tOriginFilePach.szFilePath;
            taskUnitInfo.CameraId = ptTaskUnitInfo.dwCameraID;
            taskUnitInfo.FilePathType = ptTaskUnitInfo.tOriginFilePach.dwFilePathType;
            taskUnitInfo.ImportStatus = ptTaskUnitInfo.dwImportStatus;
            taskUnitInfo.LeftTimeS = ptTaskUnitInfo.dwLeftTimeS;
            taskUnitInfo.Progress = ptTaskUnitInfo.dwProgress;
            taskUnitInfo.StartTime = ModelParser.ConvertLinuxTime(ptTaskUnitInfo.dwStartTime);
            taskUnitInfo.TaskID = ptTaskUnitInfo.dwTaskID;
            taskUnitInfo.TaskUnitID = ptTaskUnitInfo.dwTaskUnitID;
            taskUnitInfo.TaskUnitName = ptTaskUnitInfo.szTaskUnitName;
            taskUnitInfo.TaskUnitSize = ptTaskUnitInfo.qwTaskUnitSize;
            taskUnitInfo.TaskUnitType = ptTaskUnitInfo.dwTaskUnitType;
            taskUnitInfo.VideoAnalyzeTypeNum = ptTaskUnitInfo.dwVideoAnalyzeTypeNum;

            return retVal ? taskUnitInfo : null;

        }

        /// <summary>
        /// 新增任务
        /// </summary>
        /// <param name="taskInfo">任务信息</param>
        /// <returns>返回零失败，非零任务编号</returns>
        public uint AddTask(TaskInfo taskInfo)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_AddTask tTaskBaseInfo:"
            + "chTaskName:{0}"
            + "dwTaskPriorityLevel:{1}"
            + "szTaskDescription:{2}"
            , taskInfo.TaskName
            , taskInfo.TaskPriorityLevel
            , taskInfo.TaskDescription
            ));

            TVDASDK_TASK_BASE tTaskBaseInfo = new TVDASDK_TASK_BASE();
            tTaskBaseInfo.dwTaskPriorityLevel = taskInfo.TaskPriorityLevel;
            tTaskBaseInfo.szTaskDescription = taskInfo.TaskDescription;
            tTaskBaseInfo.szTaskName = taskInfo.TaskName;
            UInt32 taskID = 0;
            bool retVal = IVXSDKProtocol.VdaSdk_AddTask(tTaskBaseInfo, out taskID);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_AddTask ret:{0},pdwTaskID:{1}", retVal, taskID));
            return retVal ? taskID : 0;

        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="taskID">任务编号</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool DelTask(UInt32 taskID)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_DelTask dwTaskID:" + taskID);
            bool retVal = IVXSDKProtocol.VdaSdk_DelTask(taskID);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_DelTask ret:" + retVal);
            return retVal;

        }
        //
        /// <summary>
        /// 修改任务
        /// </summary>
        /// <param name="taskInfo">任务信息<</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool MdfTask(TaskInfo taskInfo)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_MdfTask tTaskBaseInfo:"
            + "chTaskName:{0}"
            + "dwTaskPriorityLevel:{1}"
            + "szTaskDescription:{2}"
            + "dwTaskID:{3}"
            , taskInfo.TaskName
            , taskInfo.TaskPriorityLevel
            , taskInfo.TaskDescription
            , taskInfo.TaskID
            ));

            TVDASDK_TASK_BASE tTaskBaseInfo = new TVDASDK_TASK_BASE();
            tTaskBaseInfo.dwTaskPriorityLevel = taskInfo.TaskPriorityLevel;
            tTaskBaseInfo.szTaskDescription = taskInfo.TaskDescription;
            tTaskBaseInfo.szTaskName = taskInfo.TaskName;

            bool retVal = IVXSDKProtocol.VdaSdk_MdfTask(taskInfo.TaskID, tTaskBaseInfo);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError(true );
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_MdfTask ret:{0}", retVal));
            return retVal;
        }

        /// <summary>
        /// 修改任务优先级
        /// </summary>
        /// <param name="taskID">任务编号</param>
        /// <param name="taskPriorityLevel">任务优先级</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool MdfTaskPriority(UInt32 taskID, UInt32 taskPriorityLevel)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_MdfTaskPriority dwTaskID:{0},dwTaskPriorityLevel:{1}", taskID, taskPriorityLevel));
            bool retVal = IVXSDKProtocol.VdaSdk_MdfTaskPriority(taskID, taskPriorityLevel);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_MdfTaskPriority ret:" + retVal);
            return retVal;
        }


        /// <summary>
        /// 注册任务状态回调函数
        /// </summary>
        /// <param name="onTaskStatus"></param>
        /// <param name="userData"></param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        //public  bool TaskStatusCBReg(DelegateTaskStatus onTaskStatus, UInt32 userData)
        //{
        //    MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_TaskStatusNtfCBReg dwUserData:" + userData);
        //    bool retVal = IVXSDKProtocol.VdaSdk_TaskStatusNtfCBReg(onTaskStatus, userData);
        //    MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_TaskStatusNtfCBReg ret:" + retVal);
        //    return retVal;

        //}

        /// <summary>
        /// 注册任务进度回调函数
        /// </summary>
        /// <param name="onTaskProgress"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        //bool TaskProgressCBReg(DelegateTaskProgress onTaskProgress, UInt32 userData)
        //{
        //    MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_TaskProgressNtfCBReg dwUserData:" + userData);
        //    bool retVal = IVXSDKProtocol.VdaSdk_TaskProgressNtfCBReg(onTaskProgress, userData);
        //    MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_TaskProgressNtfCBReg ret:" + retVal);
        //    return retVal;
        //}

        /// <summary>
        /// 增加本地视频文件导入到任务中
        /// </summary>
        /// <param name="taskID">任务编号</param>
        /// <param name="fileNum">文件数量</param>
        /// <param name="fileInfoList">本地上传文件信息列表</param>
        /// <param name="taskUnitIDList">返回任务单元编号列表</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool AddLocalVideoFileImportToTask(UInt32 taskID, UInt32 fileNum,
                                List<LocalVideoFileImportInfo> fileInfoList, out List<UInt32> taskUnitIDList)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < fileInfoList.Count; i++)
            {
                LocalVideoFileImportInfo info = fileInfoList[i];
                sb.AppendFormat("IVXSDKProtocol VdaSdk_AddLocalVideoFileImportToTask ptFileInfoList[{0}]"
                    + "dwAdjustStartTime:{1},"
                    + "dwCameraID:{2},"
                    + "qwFileSize:{3},"
                    + "szLocalFilePath:{4},"
                    + "szTaskUnitName:{5},"
                    + Environment.NewLine
                    , i
                    , fileInfoList[i].AdjustStartTime
                    , fileInfoList[i].CameraID
                    , fileInfoList[i].FileSize
                    , fileInfoList[i].LocalFilePath
                    , fileInfoList[i].TaskUnitName
                    );
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,sb.ToString());


            IntPtr ptFileInfoList = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TVDASDK_LOCAL_VIDEO_FILE_IMPORT_INFO)) * fileInfoList.Count);
            for (int i = 0; i < fileInfoList.Count; i++)
            {
                TVDASDK_LOCAL_VIDEO_FILE_IMPORT_INFO info = new TVDASDK_LOCAL_VIDEO_FILE_IMPORT_INFO();
                info.dwAdjustStartTime = ModelParser.ConvertLinuxTime(fileInfoList[i].AdjustStartTime);
                info.dwCameraID = fileInfoList[i].CameraID;
                info.qwFileSize = fileInfoList[i].FileSize;
                info.szLocalFilePath = fileInfoList[i].LocalFilePath;
                info.szTaskUnitName = fileInfoList[i].TaskUnitName;
                uint[] types = new uint[Common.VDASDK_ANALYZE_TYPE_MAXNUM];

                fileInfoList[i].VideoAnalyzeInfo.VideoAnalyzeType.ToArray().CopyTo(types, 0);

                info.tVideoAnalyzeInfo.adwAnalyzeType = types;

                info.tVideoAnalyzeInfo.dwAnalyzeTypeNum = fileInfoList[i].VideoAnalyzeInfo.VideoAnalyzeTypeNum;
                try
                {
                    Marshal.StructureToPtr(info, ptFileInfoList + Marshal.SizeOf(typeof(TVDASDK_LOCAL_VIDEO_FILE_IMPORT_INFO)) * i, true);
                }
                catch (COMException ex)
                {
                    string msg = string.Format("AddLocalVideoFileImportToTask error: {0}", ex.Message);
                    Debug.Assert(false, msg);
                }

            }

            IntPtr pdwTaskUnitIDList = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(uint)) * fileInfoList.Count);
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_AddLocalVideoFileImportToTask dwTaskID:{0},dwFileNum:{1}", taskID, fileNum));
            bool retVal = IVXSDKProtocol.VdaSdk_AddLocalVideoFileImportToTask(taskID, fileNum, ptFileInfoList, pdwTaskUnitIDList);
            if (!retVal)
            {
                if(pdwTaskUnitIDList!=IntPtr.Zero)
                Marshal.FreeHGlobal(pdwTaskUnitIDList);
                if(ptFileInfoList!=IntPtr.Zero)
                Marshal.FreeHGlobal(ptFileInfoList);
                // 调用失败，抛异常
                CheckError();
            }


            //IntPtr pdwTaskUnitIDList2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(UInt32)) * fileInfoList.Count);
            //bool retVal2 = IVXSDKProtocol.VdaSdk_AddRemoteVideoFileImportToTask(taskID, fileNum, ptFileInfoList, pdwTaskUnitIDList2);

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_AddLocalVideoFileImportToTask ret:" + retVal);
            taskUnitIDList = new List<uint>();
            sb.Clear();
            for (int i = 0; i < fileInfoList.Count; i++)
            {
                uint unitid = (uint)Marshal.PtrToStructure(pdwTaskUnitIDList + Marshal.SizeOf(typeof(uint)) * i, typeof(uint));
                sb.AppendFormat("IVXSDKProtocol VdaSdk_AddLocalVideoFileImportToTask pdwTaskUnitIDList[{0}]:{1}" + Environment.NewLine, i, unitid);
                if (!taskUnitIDList.Contains(unitid))
                    taskUnitIDList.Add(unitid);
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,sb.ToString());
            Marshal.FreeHGlobal(pdwTaskUnitIDList);
            Marshal.FreeHGlobal(ptFileInfoList);

            return retVal;

        }



        /// <summary>
        /// 增加文件视频文件导入到任务中
        /// </summary>
        /// <param name="dwTaskID">任务编号</param>
        /// <param name="dwFileNum">文件数量</param>
        /// <param name="ptFileInfoList">服务器文件信息列表</param>
        /// <param name="pdwTaskUnitIDList">返回任务单元编号列表</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool AddRemoteVideoFileImportToTask(UInt32 taskID,UInt32 fileNum,
                                List<RemoteVideoFileImportInfo> fileInfoList, out List<UInt32> taskUnitIDList)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_AddRemoteVideoFileImportToTask dwTaskID:{0},dwFileNum:{1}", taskID, fileNum));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < fileInfoList.Count; i++)
            {
                RemoteVideoFileImportInfo info = fileInfoList[i];
                sb.AppendFormat("IVXSDKProtocol VdaSdk_AddRemoteVideoFileImportToTask ptFileInfoList[{0}]"
                    + "dwAdjustStartTime:{1},"
                    + "dwCameraID:{2},"
                    + "qwFileSize:{3},"
                    + "szRemoteFilePath:{4},"
                    + "szTaskUnitName:{5},"
                    + Environment.NewLine
                    , i
                    , fileInfoList[i].AdjustStartTime
                    , fileInfoList[i].CameraID
                    , fileInfoList[i].FileSize
                    , fileInfoList[i].RemoteFileURL
                    , fileInfoList[i].TaskUnitName
                    );
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,sb.ToString());


            IntPtr ptFileInfoList = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TVDASDK_REMOTE_VIDEO_FILE_IMPORT_INFO)) * fileInfoList.Count);
            for (int i = 0; i < fileInfoList.Count; i++)
            {
                TVDASDK_REMOTE_VIDEO_FILE_IMPORT_INFO info = new TVDASDK_REMOTE_VIDEO_FILE_IMPORT_INFO();
                info.dwAdjustStartTime = ModelParser.ConvertLinuxTime(fileInfoList[i].AdjustStartTime);
                info.dwCameraID = fileInfoList[i].CameraID;
                info.qwFileSize = fileInfoList[i].FileSize;
                info.szRemoteFileURL = fileInfoList[i].RemoteFileURL;
                info.szTaskUnitName = fileInfoList[i].TaskUnitName;
                uint[] types = new uint[Common.VDASDK_ANALYZE_TYPE_MAXNUM];

                fileInfoList[i].VideoAnalyzeInfo.VideoAnalyzeType.ToArray().CopyTo(types, 0);

                info.tVideoAnalyzeInfo.adwAnalyzeType = types;

                info.tVideoAnalyzeInfo.dwAnalyzeTypeNum = fileInfoList[i].VideoAnalyzeInfo.VideoAnalyzeTypeNum;
                try
                {
                    Marshal.StructureToPtr(info, ptFileInfoList + Marshal.SizeOf(typeof(TVDASDK_REMOTE_VIDEO_FILE_IMPORT_INFO)) * i, true);
                }
                catch (COMException ex)
                {
                    string msg = string.Format("AddRemoteVideoFileImportToTask error: {0}", ex.Message);
                    Debug.Assert(false, msg);
                }

            }

            IntPtr pdwTaskUnitIDList = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(uint)) * fileInfoList.Count);
            bool retVal = IVXSDKProtocol.VdaSdk_AddRemoteVideoFileImportToTask(taskID, fileNum, ptFileInfoList, pdwTaskUnitIDList);
            if (!retVal)
            {
                if(pdwTaskUnitIDList!=IntPtr.Zero)
                Marshal.FreeHGlobal(pdwTaskUnitIDList);
                if(ptFileInfoList!=IntPtr.Zero)
                Marshal.FreeHGlobal(ptFileInfoList);
                // 调用失败，抛异常
                CheckError();
            }


            //IntPtr pdwTaskUnitIDList2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(UInt32)) * fileInfoList.Count);
            //bool retVal2 = IVXSDKProtocol.VdaSdk_AddRemoteVideoFileImportToTask(taskID, fileNum, ptFileInfoList, pdwTaskUnitIDList2);

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_AddRemoteVideoFileImportToTask ret:" + retVal);
            taskUnitIDList = new List<uint>();
            sb.Clear();
            for (int i = 0; i < fileInfoList.Count; i++)
            {
                uint unitid = (uint)Marshal.PtrToStructure(pdwTaskUnitIDList + Marshal.SizeOf(typeof(uint)) * i, typeof(uint));
                sb.AppendFormat("IVXSDKProtocol VdaSdk_AddRemoteVideoFileImportToTask pdwTaskUnitIDList[{0}]:{1}" + Environment.NewLine, i, unitid);
                if (!taskUnitIDList.Contains(unitid))
                    taskUnitIDList.Add(unitid);
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,sb.ToString());
            Marshal.FreeHGlobal(pdwTaskUnitIDList);
            Marshal.FreeHGlobal(ptFileInfoList);

            return retVal;

        }


        private VideoAnalyseInfo GetAnalysisInfo(VAFileInfo fi)
        {
            uint vatypecount = 0;
            List<uint> vatypelist = new List<uint>();
            if (fi.VATypeBrief)
            {
                vatypecount++;
                vatypelist.Add((uint)E_VDA_ANALYZE_TYPE.E_ANALYZE_BRIEAF);
            }
            if (fi.VATypeCar)
            {
                vatypecount++;
                vatypelist.Add((uint)E_VDA_ANALYZE_TYPE.E_ANALYZE_VEHICLE);
            }
            if (fi.VATypeFace)
            {
                vatypecount++;
                vatypelist.Add((uint)E_VDA_ANALYZE_TYPE.E_ANALYZE_FACE);
            }
            if (fi.VATypeObject)
            {
                vatypecount++;
                vatypelist.Add((uint)E_VDA_ANALYZE_TYPE.E_ANALYZE_OBJECT);
            }

            VideoAnalyseInfo analyzeInfo = new VideoAnalyseInfo()
            {
                VideoAnalyzeTypeNum = vatypecount,
                VideoAnalyzeType = vatypelist,
            };

            return analyzeInfo;
        }

        public bool AddNetStoreVideoImportToTask(UInt32 taskID, VAFileInfo[] vaFileInfos, out  List<UInt32> taskUnitIDList)
        {
            if (vaFileInfos == null && vaFileInfos.Length == 0)
            {
                taskUnitIDList = null;
                return false;
            }

            int count = vaFileInfos.Length;

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_AddNetStoreVideoImportToTask dwTaskID:{0},dwFileNum:{1}", taskID, count));            
            
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < vaFileInfos.Length; i++)
            {
                VAFileInfo info = vaFileInfos[i];
                sb.AppendFormat("IVXSDKProtocol VdaSdk_AddNetStoreVideoImportToTask netStoreVideoList[{0}]"
                    + "dwAdjustStartTime:{1},"
                    + "dwCameraID:{2},"
                    + "qwFileSize:{3},"
                    + "dwStartTime:{4},"
                    + "dwEndTime:{5},"
                    + "cameraName:{6},"
                    + Environment.NewLine
                    , i
                    , vaFileInfos[i].AdjustTime
                    , vaFileInfos[i].CameraId
                    , vaFileInfos[i].FileSize
                    , vaFileInfos[i].StartTime
                    , vaFileInfos[i].EndTime
                    , vaFileInfos[i].CameraName
                    );
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, sb.ToString());

            IntPtr ptFileInfoList = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TVDASDK_NETSTORE_VIDEO_IMPORT_INFO)) * count);

            for (int i = 0; i < count; i++)
            {
                TVDASDK_NETSTORE_VIDEO_IMPORT_INFO info = ModelParser.Convert(vaFileInfos[i]);

                try
                {
                    Marshal.StructureToPtr(info, ptFileInfoList + Marshal.SizeOf(typeof(TVDASDK_NETSTORE_VIDEO_IMPORT_INFO)) * i, true);
                }
                catch (COMException ex)
                {
                    string msg = string.Format("AddNetStoreVideoImportToTask error: {0}", ex.Message);
                    Debug.Assert(false, msg);
                }
            }

            IntPtr pdwTaskUnitIDList = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(uint)) * count);
            bool retVal = IVXSDKProtocol.VdaSdk_AddNetStoreVideoImportToTask(taskID, (uint)count, ptFileInfoList, pdwTaskUnitIDList);
            if (!retVal)
            {
                if (pdwTaskUnitIDList != IntPtr.Zero)
                    Marshal.FreeHGlobal(pdwTaskUnitIDList);
                if (ptFileInfoList != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptFileInfoList);
                // 调用失败，抛异常
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_AddNetStoreVideoImportToTask ret:" + retVal);
            taskUnitIDList = new List<uint>();
            sb.Clear();
            for (int i = 0; i < count; i++)
            {
                uint unitid = (uint)Marshal.PtrToStructure(pdwTaskUnitIDList + Marshal.SizeOf(typeof(uint)) * i, typeof(uint));
                sb.AppendFormat("IVXSDKProtocol VdaSdk_AddNetStoreVideoImportToTask pdwTaskUnitIDList[{0}]:{1}" + Environment.NewLine, i, unitid);
                if (!taskUnitIDList.Contains(unitid))
                    taskUnitIDList.Add(unitid);
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, sb.ToString());
            Marshal.FreeHGlobal(pdwTaskUnitIDList);
            Marshal.FreeHGlobal(ptFileInfoList);

            return retVal;
        }


        /// <summary>
        /// 删除任务单元
        /// </summary>
        /// <param name="taskUnitID">任务单元编号</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool DelTaskUnit(UInt32 taskUnitID)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_DelTaskUnit taskUnitID:" + taskUnitID);
            bool retVal = IVXSDKProtocol.VdaSdk_DelTaskUnit(taskUnitID);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_DelTaskUnit ret:" + retVal);
            return retVal;
        }

        /// <summary>
        /// 增加任务单元分析算法
        /// </summary>
        /// <param name="taskUnitID">任务单元编号</param>
        /// <param name="analyzeTypeList">分析类型信息</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool AddTaskUnitAnalyzeType(UInt32 taskUnitID, List<E_VDA_ANALYZE_TYPE> analyzeTypeList)
        {
            TVDASDK_ANALYZE_TYPE_INFO tAnalyzeTypeInfo = new TVDASDK_ANALYZE_TYPE_INFO();
            StringBuilder sb = new StringBuilder();

            uint[] types = new uint[Common.VDASDK_ANALYZE_TYPE_MAXNUM];

            for (int i = 0; i < Math.Min(analyzeTypeList.Count, types.Length); i++)
            {
                types[i] = (uint)analyzeTypeList[i];
            }

            tAnalyzeTypeInfo.adwAnalyzeType = types;

            tAnalyzeTypeInfo.dwAnalyzeTypeNum = (uint)analyzeTypeList.Count;

            for (int i = 0; i < analyzeTypeList.Count; i++)
            {
                sb.Append("index:" + i + ",type:" + analyzeTypeList[i].ToString());
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_AddTaskUnitAnalyzeType taskUnitID:{0}"
                + ",dwAnalyzeTypeNum:{1}"
                + ",adwAnalyzeType:{2}"
                , taskUnitID
                , tAnalyzeTypeInfo.dwAnalyzeTypeNum
                , sb.ToString()
                ));
            bool retVal = IVXSDKProtocol.VdaSdk_AddTaskUnitAnalyzeType(taskUnitID, tAnalyzeTypeInfo);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_AddTaskUnitAnalyzeType ret:" + retVal);
            return retVal;
        }


        /// <summary>
        /// 增加任务单元分析算法
        /// </summary>
        /// <param name="taskUnitID">任务单元编号</param>
        /// <param name="adjustTime">矫正时间</param>
        /// <returns>成功返回TRUE，失败返回FALSE，获取错误码调用VdaSdk_GetLastError。</returns>
        public bool SetTaskUnitAdjustTime(uint taskUnitID, DateTime adjustTime)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_SetTaskUnitAdjustTime taskUnitID:" + taskUnitID + ",adjustTime:" + adjustTime.ToString(DataModel.Constant.DATETIME_FORMAT));
            uint dwAdjustTime = Model.ModelParser.ConvertLinuxTime(adjustTime);
            bool retVal = IVXSDKProtocol.VdaSdk_SetTaskUnitAdjustTime(taskUnitID, dwAdjustTime);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError(true );
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_SetTaskUnitAdjustTime ret:" + retVal);
            return retVal;

        }


        /// <summary>
        /// 注册任务单元状态回调函数
        /// </summary>
        /// <param name="onTaskUnitStatus"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        //public  bool TaskUnitStatusCBReg(DelegateTaskUnitStatus onTaskUnitStatus, UInt32 userData)
        //{
        //    MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_TaskUnitStatusNtfCBReg dwUserData:" + userData);
        //    bool retVal = IVXSDKProtocol.VdaSdk_TaskUnitStatusNtfCBReg(onTaskUnitStatus, userData);
        //    MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_TaskUnitStatusNtfCBReg ret:" + retVal);
        //    return retVal;
        //}
        /// <summary>
        /// 注册任务单元进度回调函数
        /// </summary>
        /// <param name="onTaskUnitProgress"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        //public  bool TaskUnitProgressCBReg(DelegateTaskUnitProgress onTaskUnitProgress, UInt32 userData)
        //{
        //    MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_TaskUnitProgressNtfCBReg dwUserData:" + userData);
        //    bool retVal = IVXSDKProtocol.VdaSdk_TaskUnitProgressNtfCBReg(onTaskUnitProgress, userData);
        //    MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_TaskUnitProgressNtfCBReg ret:" + retVal);
        //    return retVal;
        //}

        /// <summary>
        /// 查询指定监控点下的视频(任务单元）信息列表，包括多个任务汇总的资源列表
        /// </summary>
        /// <param name="cameraID">监控点编号（传0表示所有获取没有关联监控点位的视频）</param>
        /// <returns>-1表示失败，其他值表示返回的查询标示值。获取错误码调用VdaSdk_GetLastError</returns>
        public Int32 QueryVideoTaskUnitListByCamera(UInt32 cameraID)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_QueryVideoTaskUnitListByCamera cameraID:" + cameraID);
            Int32 retVal = IVXSDKProtocol.VdaSdk_QueryVideoTaskUnitListByCamera(cameraID);
            if (-1 == retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_QueryVideoTaskUnitListByCamera ret:" + retVal);
            return retVal;

        }

        public Int32 QueryVideoTaskUnitListWithoutCamera()
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol QueryVideoTaskUnitListWithoutCamera");
            Int32 retVal = IVXSDKProtocol.VdaSdk_QueryVideoTaskUnitListByInvalidCamera();
            if (-1 == retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_QueryVideoTaskUnitListByCamera ret:" + retVal);
            return retVal;

        }

        /// <summary>
        /// 查询下一个指定监控点下的视频(任务单元）信息（遍历接口）
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>任务单元信息</returns>
        public TaskUnitInfo QueryCameraNextVideoTaskUnit(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_QueryCameraNextVideoTaskUnit lQueryHandle:" + queryHandle);
            TVDASDK_TASK_UNIT_INFO ptTaskUnitInfo = new TVDASDK_TASK_UNIT_INFO();
            bool retVal = IVXSDKProtocol.VdaSdk_QueryCameraNextVideoTaskUnit(queryHandle, out ptTaskUnitInfo);

            TaskUnitInfo taskUnitInfo = null;
            // 不会有SDK调用失败的情况， 因为数据已经全部取到SDK了， 不需要再跟Server交互。所以不需要CheckError

            if (retVal)
            {
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_QueryCameraNextVideoTaskUnit ret:{0},"
                    + "dwEndTime:{1},"
                    + "dwImportStatus:{2},"
                    + "dwLeftTimeS:{3},"
                    + "dwProgress:{4},"
                    + "dwStartTime:{5},"
                    + "dwTaskID:{6},"
                    + "dwTaskUnitID:{7},"
                    + "dwTaskUnitType:{8},"
                    + "dwVideoAnalyzeTypeNum:{9},"
                    , retVal
                    , ptTaskUnitInfo.dwEndTime
                    , ptTaskUnitInfo.dwImportStatus
                    , ptTaskUnitInfo.dwLeftTimeS
                    , ptTaskUnitInfo.dwProgress
                    , ptTaskUnitInfo.dwStartTime
                    , ptTaskUnitInfo.dwTaskID
                    , ptTaskUnitInfo.dwTaskUnitID
                    , ptTaskUnitInfo.dwTaskUnitType
                    , ptTaskUnitInfo.dwVideoAnalyzeTypeNum
                    ));

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < ptTaskUnitInfo.atAnalyzeStatus.Length; i++)
                {
                    TVDASDK_TASK_UNIT_ANALYZE_STATUS s = ptTaskUnitInfo.atAnalyzeStatus[i];

                    sb.AppendFormat("IVXSDKProtocol VdaSdk_QueryCameraNextVideoTaskUnit ptTaskUnitInfo.atAnalyzeStatus[{0}] dwAnalyzeStatus:{1},dwAnalyzeType:{2}" + Environment.NewLine
                                    , i
                                    , s.dwAnalyzeStatus
                                    , s.dwAnalyzeType);
                }
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,sb.ToString());


                taskUnitInfo = new TaskUnitInfo();
                taskUnitInfo.AnalyzeStatus = new Dictionary<E_VDA_ANALYZE_TYPE, E_VDA_TASK_UNIT_STATUS>();
                for (int i = 0; i < ptTaskUnitInfo.atAnalyzeStatus.Length; i++)
                {
                    TVDASDK_TASK_UNIT_ANALYZE_STATUS s = ptTaskUnitInfo.atAnalyzeStatus[i];
                    if (!taskUnitInfo.AnalyzeStatus.ContainsKey((E_VDA_ANALYZE_TYPE)s.dwAnalyzeStatus))
                        taskUnitInfo.AnalyzeStatus.Add((E_VDA_ANALYZE_TYPE)s.dwAnalyzeStatus, (E_VDA_TASK_UNIT_STATUS)s.dwAnalyzeType);
                }
                taskUnitInfo.EndTime = ModelParser.ConvertLinuxTime(ptTaskUnitInfo.dwEndTime);
                taskUnitInfo.FilePath = ptTaskUnitInfo.tOriginFilePach.szFilePath;
                taskUnitInfo.FilePathType = ptTaskUnitInfo.tOriginFilePach.dwFilePathType;
                taskUnitInfo.ImportStatus = ptTaskUnitInfo.dwImportStatus;
                taskUnitInfo.LeftTimeS = ptTaskUnitInfo.dwLeftTimeS;
                taskUnitInfo.Progress = ptTaskUnitInfo.dwProgress;
                taskUnitInfo.StartTime = ModelParser.ConvertLinuxTime(ptTaskUnitInfo.dwStartTime);
                taskUnitInfo.TaskID = ptTaskUnitInfo.dwTaskID;
                taskUnitInfo.TaskUnitID = ptTaskUnitInfo.dwTaskUnitID;
                taskUnitInfo.TaskUnitName = ptTaskUnitInfo.szTaskUnitName;
                taskUnitInfo.TaskUnitSize = ptTaskUnitInfo.qwTaskUnitSize;
                taskUnitInfo.TaskUnitType = ptTaskUnitInfo.dwTaskUnitType;
                taskUnitInfo.VideoAnalyzeTypeNum = ptTaskUnitInfo.dwVideoAnalyzeTypeNum;
            }
            return taskUnitInfo;
        }

        /// <summary>
        /// 关闭指定监控点下的视频(任务单元）信息查询
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool CloseCameraVideoTaskUnitQuery(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_CloseCameraVideoTaskUnitQuery lQueryHandle:" + queryHandle);
            bool retVal = IVXSDKProtocol.VdaSdk_CloseCameraVideoTaskUnitQuery(queryHandle);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_CloseCameraVideoTaskUnitQuery ret:" + retVal);
            return retVal;
        }




        #endregion
    }
}
