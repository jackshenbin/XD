using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using BOCOM.IVX.Protocol.Model;
using BOCOM.DataModel;

namespace BOCOM.IVX.Protocol
{
    #region 基本结构体

    /// <summary>
    /// 案件基本信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_CASE_BASE
    {
        /// <summary>
        /// 案件名，必填字段
        /// <summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_NAME_LEN)]
        public string szCaseName;

        /// <summary>
        /// 案件编号，公安业务统一编号
        /// <summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_NAME_LEN)]
        public string szCaseNo;

        /// <summary>
        /// 案件类型，保留，暂未定义
        /// <summary>
        public uint dwCaseType;

        /// <summary>
        /// 案子发生时间
        /// <summary>
        public UInt32 dwCaseHappenTime;

        /// <summary>
        /// 案子发生地点
        /// <summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_ADDR_NAME_LEN)]
        public string szCaseHappenAddr;

        public uint dwUserGroupID;
        /// <summary>
        /// 备注，描述信息
        /// <summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_DESCRIPTION_INFO_LEN)]
        public string szCaseDescription;

    };

    /// <summary>
    /// 案件信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_CASE_INFO
    {
        /// <summary>
        /// 案件ID
        /// <summary>
        public UInt32 dwCaseID;

        /// <summary>
        /// 案件基本信息
        /// <summary>
        public TVDASDK_CASE_BASE tGroupBase;

    };

    #endregion

    #region 回调定义

    /*===========================================================
    功  能：案件配置通知
    参  数：dwCaseID - 案件编号
            dwCfgNotifyType - 配置通知类型，见E_VDA_SDK_CFG_NOTIFY_TYPE定义
    ===========================================================*/
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    internal unsafe delegate void TfuncCaseCfgNtfCB(UInt32 dwCaseID, UInt32 dwCfgNotifyType, UInt32 dwUserData);
    #endregion

    internal partial class IVXSDKProtocol
    {
        #region 案件相关接口
        /************************************************************************
		 * 案件相关接口
		 ***********************************************************************/
        /*===========================================================
        功  能：进入案件/退出案件
        参  数：dwCaseID - 案件编号
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_EnterCase(UInt32 dwCaseID);
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_LeaveCase(UInt32 dwCaseID);

        /*===========================================================
        功  能：获取案件列表（相同用户组共享案件列表）
        参  数：无
        返回值：-1表示失败，其他值表示返回的查询标示值。获取错误码调用VdaSdk_GetLastError
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 VdaSdk_QueryCaseList();
        //查询下一个案件（遍历接口）
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_QueryNextCase(Int32 lQueryHandle, out TVDASDK_CASE_INFO ptCaseInfo);
        //关闭案件查询
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_CloseCaseQuery(Int32 lQueryHandle);
        //获取指定案件详细信息
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_GetCaseByID(UInt32 dwCaseID, out TVDASDK_CASE_INFO ptCaseInfo);

        /*===========================================================
        功  能：新增案件
        参  数：tCaseBase - 案件基本信息
                pdwCaseID - 返回案件编号
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_AddCase(TVDASDK_CASE_BASE tCaseBase, out uint pdwCaseID);
        //删除案件
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_DelCase(UInt32 dwCaseID);
        //修改案件
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_MdfCase(UInt32 dwCaseID, TVDASDK_CASE_BASE tCaseBase);


        //注册通知函数
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_CaseCfgNtfReg(TfuncCaseCfgNtfCB pFuncCaseCfgNtf, UInt32 dwUserData);

        /*===========================================================
        功  能：获取指定案件资源统计信息
        参  数：dwCaseID：案件编号
                pdwVideoTaskUnitCount：视频资源数量（任务单元）
                pdwPicPackageTaskUnitCount：图片包资源数量（任务单元）
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_GetTaskUnitCountByCase(UInt32 dwCaseID,
                                                         out UInt32 pdwVideoTaskUnitCount, out UInt32 pdwPicPackageTaskUnitCount);
        /*===========================================================
        功  能：获取指定案件标注统计信息
        参  数：dwCaseID：案件编号
                pdwVideoMarkCount：视频标注数量
                pdwPicPackageMarkCount：图片包标注数量
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_GetMarkCountByCase(UInt32 dwCaseID,
                                                out UInt32 pdwVideoMarkCount, out UInt32 pdwPicPackageMarkCount);

        #endregion

    }

    public partial class IVXProtocol
    {

        #region 案件相关接口
        /// <summary>
        /// 进入案件
        /// </summary>
        /// <param name="caseID">案件编号</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool EnterCase(UInt32 caseID)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_EnterCase caseID:" + caseID);
            bool retVal = IVXSDKProtocol.VdaSdk_EnterCase(caseID);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_EnterCase ret:" + retVal);


            return retVal;
        }

        /// <summary>
        /// 退出案件
        /// </summary>
        /// <param name="caseID">案件编号</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool LeaveCase(UInt32 caseID)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_LeaveCase dwCaseID:" + caseID);
            bool retVal = IVXSDKProtocol.VdaSdk_LeaveCase(caseID);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_LeaveCase ret:" + retVal);
            return retVal;
        }

        /// <summary>
        /// 获取案件列表（相同用户组共享案件列表）
        /// </summary>
        /// <returns>-1表示失败，其他值表示返回的查询标示值。获取错误码调用VdaSdk_GetLastError</returns>
        public Int32 QueryCaseList()
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_QueryCaseList ");
            int retVal = IVXSDKProtocol.VdaSdk_QueryCaseList();
            if (retVal == -1)
            {
                // 调用失败，抛异常
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_QueryCaseList ret:" + retVal);
            return retVal;
        }

        /// <summary>
        /// 查询下一个案件（遍历接口）
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns>案件信息</returns>
        public CaseInfo QueryNextCase(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_QueryNextCase lQueryHandle:" + queryHandle);
            TVDASDK_CASE_INFO ptCaseInfo;
            bool retVal = IVXSDKProtocol.VdaSdk_QueryNextCase(queryHandle, out ptCaseInfo);

            CaseInfo caseInfo = null;
            // 不会有SDK调用失败的情况， 因为数据已经全部取到SDK了， 不需要再跟Server交互。所以不需要CheckError

            if (retVal)
            {
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_QueryNextCase ret:{0},"
                    + "dwCaseID:{1},"
                    + "szCaseHappenAddr:{2},"
                    + "dwCaseHappenTime:{3},"
                    + "szCaseDescription:{4},"
                    + "szCaseName:{5},"
                    + "szCaseNo:{6},"
                    + Environment.NewLine
                    , retVal
                    , ptCaseInfo.dwCaseID
                    , ptCaseInfo.tGroupBase.szCaseHappenAddr
                    , ptCaseInfo.tGroupBase.dwCaseHappenTime
                    , ptCaseInfo.tGroupBase.szCaseDescription
                    , ptCaseInfo.tGroupBase.szCaseName
                    , ptCaseInfo.tGroupBase.szCaseNo
                    ));

                caseInfo = ModelParser.Convert(ptCaseInfo);
            }

            return caseInfo;
        }

        /// <summary>
        /// 关闭案件查询
        /// </summary>
        /// <param name="queryHandle">查询标示值</param>
        /// <returns></returns>
        public bool CloseCaseQuery(Int32 queryHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_CloseCaseQuery lQueryHandle:" + queryHandle);
            bool retVal = IVXSDKProtocol.VdaSdk_CloseCaseQuery(queryHandle);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_CloseCaseQuery ret:" + retVal);
            return retVal;

        }

        /// <summary>
        /// 获取指定案件详细信息
        /// </summary>
        /// <param name="caseID">案件编号</param>
        /// <returns>案件信息</returns>
        public CaseInfo GetCaseByID(UInt32 caseID)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_GetCaseByID caseID:" + caseID);
            TVDASDK_CASE_INFO ptCaseInfo;
            bool retVal = IVXSDKProtocol.VdaSdk_GetCaseByID(caseID, out ptCaseInfo);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
                // 如果不抛异常， 应该是记录不存在, 返回 null
                return null;
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_GetCaseByID ret:{0},"
                + "dwCaseID:{1},"
                + "szCaseHappenAddr:{2},"
                + "dwCaseHappenTime:{3},"
                + "szCaseDescription:{4},"
                + "szCaseName:{5},"
                + "szCaseNo:{6},"
                + Environment.NewLine
                , retVal
                , ptCaseInfo.dwCaseID
                , ptCaseInfo.tGroupBase.szCaseHappenAddr
                , ptCaseInfo.tGroupBase.dwCaseHappenTime
                , ptCaseInfo.tGroupBase.szCaseDescription
                , ptCaseInfo.tGroupBase.szCaseName
                , ptCaseInfo.tGroupBase.szCaseNo
                ));

            CaseInfo caseInfo = ModelParser.Convert(ptCaseInfo);

            return retVal ? caseInfo : null;
        }

        /// <summary>
        /// 新增案件
        /// </summary>
        /// <param name="caseInfo">案件信息</param>
        /// <returns>案件编号</returns>
        public UInt32 AddCase(CaseInfo caseInfo)
        {
            uint caseID = 0;

            TVDASDK_CASE_BASE tCaseBase = ModelParser.Convert(caseInfo);
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_AddCase szCaseName:{0},"
                + "szCaseNo:{1},"
                + "dwCaseHappenTime:{2},"
                + "szCaseDescription:{3},"
                + "szCaseHappenAddr:{4},"
                + Environment.NewLine
                , tCaseBase.szCaseName
                , tCaseBase.szCaseNo
                , tCaseBase.dwCaseHappenTime
                , tCaseBase.szCaseDescription
                , tCaseBase.szCaseHappenAddr
                ));
            bool retVal = IVXSDKProtocol.VdaSdk_AddCase(tCaseBase, out caseID);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_AddCase ret:{0},caseID:{1}", retVal, caseID));
            return retVal ? caseID : 0;

        }

        /// <summary>
        /// 删除案件
        /// </summary>
        /// <param name="caseID">案件编号</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool DelCase(UInt32 caseID)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_DelCase caseID:" + caseID);
            bool retVal = IVXSDKProtocol.VdaSdk_DelCase(caseID);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_DelCase ret:" + retVal);
            return retVal;

        }

        /// <summary>
        /// 修改案件
        /// </summary>
        /// <param name="caseInfo">案件信息</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool MdfCase(CaseInfo caseInfo)
        {
            TVDASDK_CASE_BASE tCaseBase = ModelParser.Convert(caseInfo);

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_MdfCase caseID:{0},"
                + "szCaseName:{1},"
                + "szCaseNo:{2},"
                + "dwCaseHappenTime:{3},"
                + "szCaseDescription:{4},"
                + "szCaseHappenAddr:{5},"
                + Environment.NewLine
                , caseInfo.CaseID
                , tCaseBase.szCaseName
                , tCaseBase.szCaseNo
                , tCaseBase.dwCaseHappenTime
                , tCaseBase.szCaseDescription
                , tCaseBase.szCaseHappenAddr
                ));
            bool retVal = IVXSDKProtocol.VdaSdk_MdfCase(caseInfo.CaseID, tCaseBase);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_MdfCase ret:{0}", retVal));
            return retVal;
        }


        ////注册通知函数
        //[DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        //public static extern bool VdaSdk_CaseCfgNtfReg(TfuncCaseCfgNtfCB pFuncCaseCfgNtf, UInt32 dwUserData);

        /// <summary>
        /// 获取指定案件资源统计信息
        /// </summary>
        /// <param name="caseID">案件编号</param>
        /// <param name="videoTaskUnitCount">视频资源数量（任务单元）</param>
        /// <param name="picPackageTaskUnitCount">图片包资源数量（任务单元）</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool GetTaskUnitCountByCase(UInt32 caseID,
                                                         out UInt32 videoTaskUnitCount, out UInt32 picPackageTaskUnitCount)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_GetTaskUnitCountByCase caseID:" + caseID);
            bool retVal = IVXSDKProtocol.VdaSdk_GetTaskUnitCountByCase(caseID, out videoTaskUnitCount, out picPackageTaskUnitCount);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_GetTaskUnitCountByCase ret:{0}"
                + ",videoTaskUnitCount:{1}"
                + ",picPackageTaskUnitCount:{2}"
                , retVal
                , videoTaskUnitCount
                , picPackageTaskUnitCount
                ));
            return retVal;

        }

        /// <summary>
        /// 获取指定案件标注统计信息
        /// </summary>
        /// <param name="caseID">案件编号</param>
        /// <param name="videoMarkCount">视频标注数量</param>
        /// <param name="picPackageMarkCount">图片包标注数量</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool GetMarkCountByCase(UInt32 caseID,
                                                out UInt32 videoMarkCount, out UInt32 picPackageMarkCount)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_GetMarkCountByCase caseID:" + caseID);
            bool retVal = IVXSDKProtocol.VdaSdk_GetMarkCountByCase(caseID, out videoMarkCount, out picPackageMarkCount);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_GetMarkCountByCase ret:{0}"
                + ",videoTaskUnitCount:{1}"
                + ",picPackageTaskUnitCount:{2}"
                , retVal
                , videoMarkCount
                , picPackageMarkCount
                ));
            return retVal;
        }
        #endregion

    }
}
