using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using BOCOM.DataModel;

namespace BOCOM.IVX.Protocol
{
    #region 基本结构体

    //智能分析日志检索
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TLOG_SEARCH_FILTER
    {
        public UInt32 qwLogSqlId; //日志查询流水号(大于等于0)
        public UInt32 dwLogCount; //本次日志检索条数(0为查询全部，建议值为：100)
        public UInt32 dwBeginTime;//开始时间(0则忽略)
        public UInt32 dwEndTime;  //结束时间(0则忽略)
        public UInt32 dwLogType; //日志类型，见E_VDA_LOG_TYPE
        public UInt32 dwLogLevel; //日志级别，见E_VDA_LOG_LEVEL
        public UInt32 dwSortKind; //排序类型，见E_VDA_LOG_SORT_TYPE
        public UInt32 dwOptType;  //操作者类型，见E_LOG_OPERATE_TYPE
        public UInt32 dwOptId;	  //关联到服务器ID
    };

    //智能分析日志检索结果
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TLOG_SEARCH_RESULT
    {
        public UInt32 qwLogSqlId;                                 //日志查询流水号(大于等于1)
        public UInt32 dwLevel;			                          //日志级别：见E_VDA_LOG_LEVEL
        public UInt32 dwLogDetail;                                //日志细分:见E_VDA_LOG_DETAIL
        public UInt32 dwLogType;								  //日志类型，见E_VDA_LOG_TYPE
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDA_MAX_DESCRIPTION_INFO_LEN)]
        public string szDescription;//日志描述
        public UInt32 dwOptType;                                  //操作者类型，见E_LOG_OPERATE_TYPE
        public UInt32 dwOptId;			                          //关联到服务器ID
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDA_MAX_NAME_LEN)]
        public string szOptName;			      //操作者名称
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDA_MAX_TIME_LEN)]
        public string szTime;                   //日志发生时间
    };


    #endregion

    #region 回调定义
    /*===========================================================
    功  能：注册日志检索回调通知
    参  数：	ptLogInfo - 日志信息
		    pLogNum - 日志数量
		    dwUserData - 用户数据
    返回值：成功返回TRUE，失败返回FALSE
    ===========================================================*/
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    internal delegate void TfuncLogSearchNtfCB(IntPtr /* TLOG_SEARCH_RESULT* */ ptLogInfo, UInt32 dwLogNum, UInt32 dwUserData);

    #endregion

    internal partial class IVXSDKProtocol
    {
        #region 日志相关
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_LogSearchNtfCBReg(TfuncLogSearchNtfCB pfuncLogSearchNtf, UInt32 dwUserData);
        /*===========================================================
        功  能：获取日志信息
        参  数：	tLogSearchFilter - 日志检索条件
                pdwLogQueryHandle - 日志检索句柄
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_LogSearch(TLOG_SEARCH_FILTER tLogSearchFilter);
        #endregion

    }


    public partial class IVXProtocol
    {
        #region 日志相关
        /*===========================================================
        功  能：获取日志信息
        参  数：	tLogSearchFilter - 日志检索条件
                pdwLogQueryHandle - 日志检索句柄
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        public bool LogSearch(LogSearchParam param)
        {
            TLOG_SEARCH_FILTER tLogSearchFilter = new TLOG_SEARCH_FILTER();
            tLogSearchFilter.dwBeginTime = Model.ModelParser.ConvertLinuxTime(param.BeginTime);
            tLogSearchFilter.dwEndTime = Model.ModelParser.ConvertLinuxTime(param.EndTime);
            tLogSearchFilter.dwLogCount = param.LogCount;
            tLogSearchFilter.dwLogLevel = (uint)param.LogLevel;
            tLogSearchFilter.dwLogType = (uint)param.LogType;
            tLogSearchFilter.dwOptId = param.OptId;
            tLogSearchFilter.dwOptType = (uint)param.OptType;
            tLogSearchFilter.dwSortKind = (uint)param.SortKind;
            tLogSearchFilter.qwLogSqlId = param.LogId;

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_LogSearch dwBeginTime:{0}"
                + ",dwEndTime:{1}"
                + ",dwLogCount:{2}"
                + ",dwLogLevel:{3}"
                + ",dwLogType:{4}"
                + ",dwOptId:{5}"
                + ",dwOptType:{6}"
                + ",dwSortKind:{7}"
                + ",qwLogSqlId:{8}"
                , tLogSearchFilter.dwBeginTime
                , tLogSearchFilter.dwEndTime
                , tLogSearchFilter.dwLogCount
                , tLogSearchFilter.dwLogLevel
                , tLogSearchFilter.dwLogType
                , tLogSearchFilter.dwOptId
                , tLogSearchFilter.dwOptType
                , tLogSearchFilter.dwSortKind
                , tLogSearchFilter.qwLogSqlId
                ));
            bool retVal = IVXSDKProtocol.VdaSdk_LogSearch(tLogSearchFilter);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_LogSearch ret:" + retVal);
            return retVal;

        }
        #endregion

    }
}
