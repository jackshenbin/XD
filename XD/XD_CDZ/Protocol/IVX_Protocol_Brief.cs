using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using BOCOM.IVX.Protocol.Model;
using BOCOM.DataModel;

namespace BOCOM.IVX.Protocol
{
    #region 基本结构体
    /// <summary>
    /// 摘要播放信息
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_BRIEF_PLAY_INFO
    {
        /// <summary>
        /// 任务单元ID
        /// </summary>
        public uint dwVideoTaskUnitID;
        /// <summary>
        /// 播放窗口的句柄
        /// </summary>
        public System.IntPtr hPlayWnd;
    }

    /// <summary>
    /// 摘要播放设置的信息
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_BRIEF_PLAY_PARAM
    {
        /// <summary>
        /// 对象密度，见定义:E_VDA_BRIEF_DENSITY
        /// </summary>
        public uint dwObjDensity;

        /// <summary>
        /// 是否摘要播放整个文件，如果整个文件，后续的时间段信息无效
        /// </summary>
        [MarshalAsAttribute(UnmanagedType.Bool)]
        public bool bIsBriefAllFile;

        /// <summary>
        /// 开始时间
        /// </summary>
        public uint dwStartTime;

        /// <summary>
        /// 结束时间
        /// </summary>
        public uint dwEndTime;

        /// <summary>
        /// 运动目标类型，见E_VDA_MOVEOBJ_TYPE
        /// </summary>
        public uint dwMoveObjType;

        /// <summary>
        /// 运动目标颜色，见E_VDA_MOVEOBJ_COLOR
        /// </summary>
        public uint dwMoveObjColor;
    }

    /// <summary>
    /// 摘要运动对象信息
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_BRIEF_MOVEOBJ_INFO
    {

        /// <summary>
        /// 运动目标标示
        /// </summary>
        public uint dwMoveObjID;

        /// <summary>
        /// 运动目标类型，见E_VDA_MOVEOBJ_TYPE
        /// </summary>
        public uint dwMoveObjType;

        /// <summary>
        /// 运动目标颜色
        /// </summary>
        public uint dwMoveObjColor;

        /// <summary>
        /// 目标出现的时间点（绝度时间，秒）
        /// </summary>
        public uint dwBeginTimeS;

        /// <summary>
        /// 目标结束的时间点（绝度时间，秒）
        /// </summary>
        public uint dwEndTimeS;
    }

    /// <summary>
    /// 摘要本地导出信息
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_BRIEF_LOCAL_EXPORT_INFO
    {
        /// <summary>
        /// 本地保存文件路径
        /// </summary>
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_FILEPATH_LEN)]
        public string szLocalSaveFilePath;
    }

    #endregion

    #region 回调定义
    /*===========================================================
    功  能：摘要播放进度回调函数
    参  数：lVodHandle - 播放标示
		    dwPlayState - 播放状态，见E_VDA_PLAY_STATUS
		    dwPlayProgress - 播放进度，（千分比整数值，801表示80.1%）
		    dwResult - 错误码，由合成失败或播放失败时带出		    
            dwUserData - 用户数据
    返回值：-1表示失败，其他值表示返回的点播标示值。
    ===========================================================*/
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    internal delegate void TfuncBriefPlayPosCB(Int32 lBriefHandle, UInt32 dwPlayState, UInt32 dwPlayProgress, UInt32 dwCurPlayTime, UInt32 dwResult, UInt32 dwUserData);
 
    /*===========================================================
    功  能：摘要窗口鼠标动作通知回调函数
    参  数：lBriefHandle - 播放标示句柄
		    dwOptType - 下载状态，见E_VDA_BRIEF_WND_MOUSE_OPT_TYPE
		    dwX - 鼠标所在x轴窗口坐标
		    dwY - 鼠标所在y轴窗口坐标
		    dwUserData - 用户数据
    返回值：无
    ===========================================================*/
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    internal delegate void TfuncBriefWndMouseOptNtfCB(Int32 lBriefHandle, UInt32 dwOptType, UInt32 dwX, UInt32 dwY, UInt32 dwUserData);

    /*===========================================================
    功  能：摘要导出进度回调函数
    参  数：lBriefHandle - 播放标示句柄
            dwExportState - 下载状态，见E_VDA_EXPORT_STATUS
            dwProgress - 下载进度，（千分比整数值，801表示80.1%）
            dwUserData - 用户数据
    返回值：无
    ===========================================================*/
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    internal delegate void TfuncBriefExportPosCB(Int32 lBriefHandle, UInt32 dwExportState, UInt32 dwProgress, UInt32 dwUserData);


    #endregion

    internal partial class IVXSDKProtocol
    {
        #region 摘要相关


        /*===========================================================
		功  能：摘要播放指定任务单元的视频
		参  数：ptBriefPlayInfo - 点播信息
				pfuncPlayPos - 进度回调函数，传Null标示不需要回调进度
				dwUserData - 用户数据
		返回值：-1表示失败，其他值表示返回的摘要播放标示值。
		===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern int VdaSdk_OpenBriefPlay(ref TVDASDK_BRIEF_PLAY_INFO ptBriefPlayInfo, TfuncBriefPlayPosCB pfuncPlayPos, uint dwUserData);

        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_CloseBriefPlay(int lBriefHandle);

        /*===========================================================
        功  能：设置摘要播放参数
        参  数：lBriefHandle - 播放标示句柄
                tBriefPalyParam - 摘要播放参数
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_SetBriefPlayParam(int lBriefHandle, TVDASDK_BRIEF_PLAY_PARAM tBriefPalyParam);

        /*===========================================================
        功  能：摘要播放控制
        参  数：lBriefHandle - 点播标示句柄
                dwControlCode - 播放控制类型，见E_VDA_BRIEF_PLAYCTRL_TYPE定义
                dwInValue - 播放控制输入参数
                pdwOutValue - 播放控制输出参数，如获取的进度等
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_BriefPlayControl(int lBriefHandle, uint dwControlCode, uint dwInValue, out uint pdwOutValue);

        /*===========================================================
        功  能：设置摘要编辑模式(是否进行绘图操作的开关）
        参  数：lBriefHandle - 播放标示句柄
                bIsEditMode - 是否编辑模式
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_SetBriefEditMode(int lBriefHandle, [MarshalAsAttribute(UnmanagedType.Bool)] bool bIsEditMode);

        /*===========================================================
        功  能：设置摘要绘图过滤器类型（如画绊线，画闯入闯出区域，感兴趣区域和屏蔽区域)
        参  数：lBriefHandle - 播放标示句柄
                dwDrawFilterType - 行为过滤类型 见E_VDA_BRIEF_DRAW_FILTER_TYPE
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_SetBriefDrawFilterType(int lBriefHandle, uint dwDrawFilterType);

        //清除行为过滤绘画内容
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_ClearActionDrawFilter(int lBriefHandle);

        //清除区域过滤绘画内容
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_ClearAreaDrawFilter(int lBriefHandle);

        /*===========================================================
        功  能：设置摘要播放时的画图的叠加信息开关
        参  数：lBriefHandle - 播放标示句柄
                dwBriefPlayDrawType - 行为过滤类型 见E_VDA_BRIEF_PLAY_DRAW_TYPE
                bIsDraw - 开关状态
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_SetBriefPlayDrawType(int lBriefHandle, uint dwBriefPlayDrawType, [MarshalAsAttribute(UnmanagedType.Bool)] bool bIsDraw);

        /*===========================================================
        功  能：判断是否选中摘要运动目标
        参  数：lBriefHandle - 播放标示句柄
        返回值：成功返回TRUE，选中，失败返回FALSE，未选中
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_IsSelectBriefMoveObj(int lBriefHandle);

        /*===========================================================
        功  能：获取当前选中的摘要运动目标信息
        参  数：lBriefHandle - 播放标示句柄
                ptBriefMoveObjInfo - 选中的运动目标相关信息
        返回值：成功返回TRUE，选中，失败返回FALSE，未选中
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_GetSelectBriefMoveObjInfo(int lBriefHandle, out TVDASDK_BRIEF_MOVEOBJ_INFO ptBriefMoveObjInfo);

        /*===========================================================
        功  能：抓取摘要图片数据
        参  数：lBriefHandle - 播放标示句柄
		        dwPicType - 抓取图片的类型，见E_VDA_GRAB_PIC_TYPE
		        pPicBuf - 图片缓存区（只有在输入缓存区大小足够前提下，才会输出图片数据）
		        dwBufLen - 输入图片缓存区大小
		        dwPicDataLen - 实际图片数据大小
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool  VdaSdk_GrabBriefPictureData(Int32 lBriefHandle, UInt32 dwPicType, IntPtr pPicBuf, UInt32 dwBufLen, out UInt32 dwPicDataLen );


        /*===========================================================
        功  能：判断是否选中图形
        参  数：lBriefHandle - 摘要播放标示句柄
		        tPt - 窗口坐标
        返回值：返回TRUE表示已经选中图形(越界线或闯入\闯出区),返回FALSE表示没有选中
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_IsBriefPlayHitGraph(Int32 lBriefHandle, TVDASDK_WINDOW_POINT tPt);

        /*===========================================================
        功  能：摘要窗口鼠标动作通知回调函数
        参  数：lBriefHandle - 播放标示句柄
                dwOptType - 下载状态，见E_VDA_BRIEF_WND_MOUSE_OPT_TYPE
                dwX - 鼠标所在x轴窗口坐标
                dwY - 鼠标所在y轴窗口坐标
                dwUserData - 用户数据
        返回值：无
        ===========================================================*/
        //设置摘要窗口鼠标动作通知回调参数
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern int VdaSdk_SetBriefWndMouseOptNtfCB(int lBriefHandle, TfuncBriefWndMouseOptNtfCB pfuncMouseOptNtf, uint dwUserData);

        /*===========================================================
        功  能：摘要导出进度回调函数
        参  数：lBriefHandle - 播放标示句柄
               dwExportState - 下载状态，见E_VDA_EXPORT_STATUS
               dwProgress - 下载进度，（千分比整数值，801表示80.1%）
               dwUserData - 用户数据
        返回值：无
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern int VdaSdk_BriefLocalExport(int lBriefHandle, ref TVDASDK_BRIEF_LOCAL_EXPORT_INFO ptExportInfo, TfuncBriefExportPosCB pfuncExportPos, uint dwUserData);

        // 查询摘要导出进度
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_GetBriefExportPos(int lBriefHandle, ref uint pdwExportState, ref uint pdwProgress);

        // 停止摘要本地导出
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_StopBriefExport(int lBriefHandle);



        #endregion

    }


    public partial class IVXProtocol
    {

        #region 摘要相关

        /// <summary>
        /// 摘要播放指定任务单元的视频
        /// </summary>
        /// <param name="taskUnitId">任务单元</param>
        /// <param name="windowHandle">窗口句柄</param>
        /// <param name="dwUserData">用户数据</param>
        /// <returns>-1表示失败，其他值表示返回的摘要播放标示值</returns>
        public int OpenBriefPlay(uint taskUnitId, IntPtr windowHandle, uint dwUserData)
        {
            int nRet = -1;

            TVDASDK_BRIEF_PLAY_INFO ptBriefPlayInfo = new TVDASDK_BRIEF_PLAY_INFO()
            {
                dwVideoTaskUnitID = taskUnitId,
                hPlayWnd = windowHandle
            };
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_OpenBriefPlay taskUnitId:{0}"
                            + ",windowHandle:{1}"
                            , ptBriefPlayInfo.dwVideoTaskUnitID
                            , ptBriefPlayInfo.hPlayWnd
                            ));

            nRet = IVXSDKProtocol.VdaSdk_OpenBriefPlay(ref ptBriefPlayInfo, m_BriefPlayPosCB, dwUserData);

            if (nRet == -1)
            {
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_OpenBriefPlay ret:{0}"
                , nRet
                ));

            return nRet;
        }

        /// <summary>
        /// 结束摘要播放
        /// </summary>
        /// <param name="sessionId">摘要播放标示值</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool CloseBriefPlay(int sessionId)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_CloseBriefPlay sessionId:{0}"
                            , sessionId
                            ));
            bool bRet = IVXSDKProtocol.VdaSdk_CloseBriefPlay(sessionId);

            if (!bRet)
            {
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_CloseBriefPlay ret:{0}"
                , bRet
                ));

            return bRet;
        }

        /// <summary>
        /// 设置摘要播放参数
        /// </summary>
        /// <param name="sessionId">播放标示句柄</param>
        /// <param name="param">摘要播放参数</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool SetBriefPlayParam(int sessionId, BriefPlayParam param)
        {
            TVDASDK_BRIEF_PLAY_PARAM tBriefPlayParam = new TVDASDK_BRIEF_PLAY_PARAM();
            tBriefPlayParam.bIsBriefAllFile = param.IsBriefAllFile;
            tBriefPlayParam.dwMoveObjColor = param.MoveObjColor;
            tBriefPlayParam.dwMoveObjType = (uint)param.MoveObjType;
            tBriefPlayParam.dwObjDensity = (uint)param.ObjDensity;
            tBriefPlayParam.dwEndTime = ModelParser.ConvertLinuxTime(param.EndTime);
            tBriefPlayParam.dwStartTime = ModelParser.ConvertLinuxTime(param.StartTime);

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_SetBriefPlayParam sessionId:{0}"
                + ",bIsBriefAllFile:{1}"
                + ",dwMoveObjColor:{2}"
                + ",dwMoveObjType:{3}"
                 + ",dwObjDensity:{4}"
                + ",dwEndTime:{5}"
                + ",dwStartTime:{6}"
                           , sessionId
                           , tBriefPlayParam.bIsBriefAllFile
                           , tBriefPlayParam.dwMoveObjColor
                           , tBriefPlayParam.dwMoveObjType
                           , tBriefPlayParam.dwObjDensity
                           , tBriefPlayParam.dwEndTime
                           , tBriefPlayParam.dwStartTime
                            ));

            bool bRet = IVXSDKProtocol.VdaSdk_SetBriefPlayParam(sessionId, tBriefPlayParam);

            if (!bRet)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_SetBriefPlayParam ret:{0}"
                , bRet
                ));
            return bRet;
        }

        /// <summary>
        /// 摘要播放控制
        /// </summary>
        /// <param name="sessionId">点播标示句柄</param>
        /// <param name="dwControlCode">播放控制类型，见E_VDA_BRIEF_PLAYCTRL_TYPE定义</param>
        /// <param name="dwInValue">播放控制输入参数</param>
        /// <param name="outValue">播放控制输出参数，如获取的进度等</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool BriefPlayControl(int sessionId, E_VDA_BRIEF_PLAYCTRL_TYPE dwControlCode, uint dwInValue, out uint outValue)
        {

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_BriefPlayControl sessionId:{0}"
                            + ",dwControlCode:{1}"
                            + ",dwInValue:{2}"
                            , sessionId
                            , dwControlCode
                            , dwInValue
                            ));
            bool bRet = IVXSDKProtocol.VdaSdk_BriefPlayControl(sessionId, (uint)dwControlCode, dwInValue, out outValue);

            if (!bRet)
            {
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_BriefPlayControl ret:{0}"
                + "outValue:{1}"
                , bRet
                , outValue
                ));

            return bRet;
        }

        /// <summary>
        /// 设置摘要编辑模式(是否进行绘图操作的开关）
        /// </summary>
        /// <param name="sessionId">播放标示句柄</param>
        /// <param name="bIsEditMode">是否编辑模式</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool SetBriefEditMode(int sessionId, [MarshalAsAttribute(UnmanagedType.Bool)]bool bIsEditMode)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_SetBriefEditMode sessionId:{0}"
                            + ",bIsEditMode:{1}"
                            , sessionId
                            , bIsEditMode
                            ));
            bool bRet = IVXSDKProtocol.VdaSdk_SetBriefEditMode(sessionId, bIsEditMode);

            if (!bRet)
            {
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_SetBriefEditMode ret:{0}"
                , bRet
                ));

            return bRet;
        }

        /// <summary>
        /// 设置摘要绘图过滤器类型（如画绊线，画闯入闯出区域，感兴趣区域和屏蔽区域)
        /// </summary>
        /// <param name="sessionId">播放标示句柄</param>
        /// <param name="drawFilterType">行为过滤类型 见E_VDA_BRIEF_DRAW_FILTER_TYPE</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool SetBriefDrawFilterType(int sessionId, E_VDA_BRIEF_DRAW_FILTER_TYPE drawFilterType)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_SetBriefDrawFilterType sessionId:{0}"
                            + ",drawFilterType:{1}"
                            , sessionId
                            , drawFilterType
                            ));
            bool bRet = IVXSDKProtocol.VdaSdk_SetBriefDrawFilterType(sessionId, (uint)drawFilterType);

            if (!bRet)
            {
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_SetBriefDrawFilterType ret:{0}"
                , bRet
                ));

            return bRet;
        }

        /// <summary>
        /// 判断是否选中摘要运动目标
        /// </summary>
        /// <param name="sessionId">播放标示句柄</param>
        /// <returns>成功返回TRUE，选中，失败返回FALSE，未选中</returns>
        public bool IsSelectBriefMoveObj(int sessionId)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_IsSelectBriefMoveObj sessionId:{0}"
                            , sessionId
                            ));
            bool bRet = IVXSDKProtocol.VdaSdk_IsSelectBriefMoveObj(sessionId);

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_IsSelectBriefMoveObj ret:{0}"
                , bRet
                ));

            return bRet;
        }

        /// <summary>
        /// 获取当前选中的摘要运动目标信息
        /// </summary>
        /// <param name="sessionId">播放标示句柄</param>
        /// <returns>选中的运动目标相关信息</returns>
        public BriefMoveobjInfo GetSelectBriefMoveObjInfo(int sessionId)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_GetSelectBriefMoveObjInfo sessionId:{0}"
                            , sessionId
                            ));
            TVDASDK_BRIEF_MOVEOBJ_INFO ptBriefMoveObjInfo;

            bool bRet = IVXSDKProtocol.VdaSdk_GetSelectBriefMoveObjInfo(sessionId, out ptBriefMoveObjInfo);

            if (!bRet)
            {
                CheckError();
                // 如果不抛异常， 应该是记录不存在, 返回 null
                return null;
            }
            BriefMoveobjInfo retVal = new BriefMoveobjInfo();
            retVal.BeginTimeS = ModelParser.ConvertLinuxTime(ptBriefMoveObjInfo.dwBeginTimeS);
            retVal.EndTimeS = ModelParser.ConvertLinuxTime(ptBriefMoveObjInfo.dwEndTimeS);
            retVal.MoveObjColor = ptBriefMoveObjInfo.dwMoveObjColor;
            retVal.MoveObjID = ptBriefMoveObjInfo.dwMoveObjID;
            retVal.MoveObjType = (E_VDA_MOVEOBJ_TYPE)ptBriefMoveObjInfo.dwMoveObjType;
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_GetSelectBriefMoveObjInfo ret:{0}"
                + ",BeginTimeS:{1}"
                + ",EndTimeS:{2}"
                + ",MoveObjColor:{3}"
                + ",MoveObjID:{4}"
                + ",MoveObjType:{5}"
                , bRet
                , ptBriefMoveObjInfo.dwBeginTimeS
                , ptBriefMoveObjInfo.dwEndTimeS
                , ptBriefMoveObjInfo.dwMoveObjColor
                , ptBriefMoveObjInfo.dwMoveObjID
                , ptBriefMoveObjInfo.dwMoveObjType
                ));

            return retVal;
        }

        /// <summary>
        /// 设置摘要播放时的画图的叠加信息开关
        /// </summary>
        /// <param name="sessionId">播放标示句柄</param>
        /// <param name="briefPlayDrawType">叠加类型 见E_VDA_BRIEF_PLAY_DRAW_TYPE</param>
        /// <param name="bIsDraw">开关状态</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool SetBriefPlayDrawType(int sessionId, E_VDA_BRIEF_PLAY_DRAW_TYPE briefPlayDrawType, bool bIsDraw)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_SetBriefPlayDrawType sessionId:{0}"
                            + ",briefPlayDrawType:{1}"
                            + ",bIsDraw:{2}"
                            , sessionId
                            , briefPlayDrawType
                            , bIsDraw
                            ));
            bool bRet = IVXSDKProtocol.VdaSdk_SetBriefPlayDrawType(sessionId, (uint)briefPlayDrawType, bIsDraw);

            if (!bRet)
            {
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_SetBriefPlayDrawType ret:{0}"
                , bRet
                ));

            return bRet;
        }

        /// <summary>
        /// 摘要导出
        /// </summary>
        /// <param name="sessionId">播放标示句柄</param>
        /// <param name="ptExportInfo">导出信息</param>
        /// <param name="dwUserData">用户数据</param>
        /// <returns>-1表示失败，其他值表示返回的摘要导出标示值</returns>
        public int BriefLocalExport(int sessionId, string szLocalSaveFilePath, uint dwUserData)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_BriefLocalExport sessionId:{0},szLocalSaveFilePath:{1}"
                            , sessionId
                            , szLocalSaveFilePath
                            ));
            TVDASDK_BRIEF_LOCAL_EXPORT_INFO info = new TVDASDK_BRIEF_LOCAL_EXPORT_INFO();
            info.szLocalSaveFilePath = szLocalSaveFilePath;
            int nRet = IVXSDKProtocol.VdaSdk_BriefLocalExport(sessionId, ref info, m_BriefExportPosCB, dwUserData);

            if (nRet == -1)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_BriefLocalExport ret:{0}"
                , nRet
                ));
            return nRet;
        }

        /// <summary>
        /// 查询摘要导出进度
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="pdwExportState"></param>
        /// <param name="pdwProgress"></param>
        /// <returns></returns>
        public bool GetBriefExportPos(int sessionId, ref uint pdwExportState, ref uint pdwProgress)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_GetBriefExportPos sessionId:{0}"
                            , sessionId
                            ));
            bool bRet = IVXSDKProtocol.VdaSdk_GetBriefExportPos(sessionId, ref pdwExportState, ref pdwProgress);

            if (!bRet)
            {
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_GetBriefExportPos ret:{0}"
                , bRet
                ));

            return bRet;
        }

        /// <summary>
        /// 停止摘要本地导出
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public bool StopBriefExport(int sessionId)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_StopBriefExport sessionId:{0}"
                            , sessionId
                            ));
            bool bRet = IVXSDKProtocol.VdaSdk_StopBriefExport(sessionId);

            if (!bRet)
            {
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_StopBriefExport ret:{0}"
                , bRet
                ));

            return bRet;
        }

        /// <summary>
        /// 清除区域过滤绘画内容
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public bool ClearAreaDrawFilter(int sessionId)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_ClearAreaDrawFilter sessionId:{0}"
                            , sessionId
                            ));
            bool bRet = IVXSDKProtocol.VdaSdk_ClearAreaDrawFilter(sessionId);

            //if (!bRet)
            //{
            //    CheckError();
            //}

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_ClearAreaDrawFilter ret:{0}"
                , bRet
                ));
            return bRet;
        }

        /// <summary>
        /// 清除行为过滤绘画内容
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public bool ClearActionDrawFilter(int sessionId)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_ClearActionDrawFilter sessionId:{0}"
                            , sessionId
                            ));
            bool bRet = IVXSDKProtocol.VdaSdk_ClearActionDrawFilter(sessionId);

            //if (!bRet)
            //{
            //    CheckError();
            //}

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_ClearActionDrawFilter ret:{0}"
                , bRet
                ));
            return bRet;
        }

        /// <summary>
        /// 摘要窗口鼠标动作通知回调函数
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public int SwitchOnBriefWndMouseOptNtfCB(int sessionId)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_SetBriefWndMouseOptNtfCB sessionId:{0}"
                            , sessionId
                            ));
            uint userData = 0;
            int nRet = IVXSDKProtocol.VdaSdk_SetBriefWndMouseOptNtfCB(sessionId, m_BriefWndMouseOptNtfCB, userData);

            if (nRet == -1)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_SetBriefWndMouseOptNtfCB ret:{0}"
                , nRet
                ));
            return nRet;
        }

        /// <summary>
        /// 抓取摘要图片数据
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public System.Drawing.Image GrabBriefPictureData(Int32 sessionId)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol GrabBriefPictureData vodHandle:{0}"
                , sessionId
                ));
            uint pictype = (uint)E_VDA_GRAB_PIC_TYPE.E_GRAB_PIC_BMP;
            uint buflen = 10 * 1024 * 1024;
            IntPtr picbuf = Marshal.AllocHGlobal((int)buflen);
            uint picdatalen = 0;
            bool retVal = IVXSDKProtocol.VdaSdk_GrabBriefPictureData(sessionId, pictype, picbuf, buflen, out picdatalen);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            System.Drawing.Image img = ModelParser.GetImage(picbuf, (int)picdatalen);

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol GrabBriefPictureData ret:{0},picdatalen:{1}"
                , retVal
                , picdatalen
                ));
            if (picbuf != IntPtr.Zero)
                Marshal.FreeHGlobal(picbuf);
            return img;
        }

        /// <summary>
        /// 判断是否选中图形(越界线或闯入\闯出区)
        /// </summary>
        /// <param name="lBriefHandle">摘要播放标示句柄</param>
        /// <param name="point">窗口坐标</param>
        /// <returns>返回TRUE表示已经选中图形,返回FALSE表示没有选中</returns>
        public bool IsBriefPlayHitGraph(Int32 lBriefHandle, System.Drawing.Point point)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_IsBriefPlayHitGraph lBriefHandle:{0},x:{1},y:{2}"
                , lBriefHandle
                , point.X
                , point.Y
                ));
            TVDASDK_WINDOW_POINT tPt = new TVDASDK_WINDOW_POINT() { dwX = (uint)point.X, dwY = (uint)point.Y };
            bool retVal = IVXSDKProtocol.VdaSdk_IsBriefPlayHitGraph(lBriefHandle, tPt);

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_IsBriefPlayHitGraph ret:{0}"
                , retVal
                ));
            return retVal;

        }


        #endregion

    }
}
