using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using BOCOM.IVX.Protocol.Model;
using BOCOM.DataModel;

namespace BOCOM.IVX.Protocol
{
    #region 基本结构体
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_TASK_UNIT_VOD_INFO
    {
        public UInt32 dwVideoTaskUnitID;	//任务单元ID
        public bool bIsPlayAllFile;		//是否播放整个文件，如果整个文件，后续的时间段信息无效
        public UInt32 dwStartTime;			//开始时间
        public UInt32 dwEndTime;			//结束时间
        public IntPtr hPlayWnd;				//播放窗口的句柄
    };

    //按任务单元下载信息
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_TASK_UNIT_DOWNLOAD_INFO
    {
        public UInt32 dwVideoTaskUnitID;	//任务单元ID
        public bool bIsDownloadAllFile;	//是否下载整个文件，如果整个文件，后续的时间段信息无效
        public UInt32 dwStartTime;			//开始时间（从1970年1月1日开始的秒数）
        public UInt32 dwEndTime;			//结束时间（从1970年1月1日开始的秒数）
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_FILEPATH_LEN)]
        public string szLocalSaveFilePath; //本地保存文件路径
    };

    //窗口坐标
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_WINDOW_POINT
    {
        public UInt32 dwX;		//X坐标
        public UInt32 dwY;		//Y坐标
    };

    #endregion

    #region 回调定义

    /*===========================================================
    功  能：播放进度回调函数
    参  数：lVodHandle - 播放标示
        dwPlayState - 播放状态，见E_VDA_PLAY_STATUS
        dwPlayPercent - 播放进度，（百分比整数值，80表示80%）
        dwCurPlayTime - 当前播放时间（绝对时间）
        dwUserData - 用户数据
    返回值：-1表示失败，其他值表示返回的点播标示值。
    ===========================================================*/
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    internal unsafe delegate void TfuncPlayPosCB(Int32 lVodHandle, UInt32 dwPlayState,
                                             UInt32 dwPlayPercent, UInt32 dwCurPlayTime, UInt32 dwUserData);

    /// <summary>
    /// 下载进度回调函数 
    /// </summary>
    /// <param name="lDlHandle">下载标识</param>
    /// <param name="dwTransProgress">下载转码进度进度，（千分比整数值，801表示80.1%） </param>
    /// <param name="dwExportProgress">下载导出进度，（千分比整数值，801表示80.1%） </param>
    /// <param name="dwCombineProgress">下载组合进度进度，（千分比整数值，801表示80.1%）</param>
    /// <param name="dwUserData">用户数据</param>
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    internal unsafe delegate void TfuncDownLoadVideoPosCB(Int32 lDlHandle, UInt32 dwTransProgress, UInt32 dwExportProgress, UInt32 dwCombineProgress, UInt32 dwUserData);

    /// <summary>
    /// 下载进度回调函数
    /// </summary>
    /// <param name="lDlHandle">下载标识</param>
    /// <param name="dwDownLoadStatus">下载状态，见E_VDA_DOWNLOAD_STATUS</param>
    /// <param name="dwUserData">用户数据</param>
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    internal unsafe delegate void TfuncDownLoadVideoStatusCB(Int32 lDlHandle, UInt32 dwDownLoadStatus, UInt32 nResult, UInt32 dwUserData);

    #endregion

    internal partial class IVXSDKProtocol
    {
        #region 点播及下载导出接口
        /************************************************************************
         * 点播及下载导出接口
         ***********************************************************************/
        /*===========================================================
        功  能：点播指定任务单元的视频
        参  数：ptVodInfoByTaskUnit - 点播信息
                pfuncPlayPos - 进度回调函数，传Null标示不需要回调进度
                dwUserData - 用户数据
        返回值：-1表示失败，其他值表示返回的点播标示值。
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 VdaSdk_PlayBackByTaskUnit(ref TVDASDK_TASK_UNIT_VOD_INFO ptVodInfoByTaskUnit,
                                                          TfuncPlayPosCB pfuncPlayPos, UInt32 dwUserData);
        /*===========================================================
        功  能：播放控制
        参  数：lVodHandle - 点播标示句柄
                dwControlCode - 播放控制类型，见E_VDA_PLAYCTRL_TYPE定义
                dwInValue - 播放控制输入参数
                pdwOutValue - 播放控制输出参数，如获取的进度等
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_PlayBackControl(Int32 lVodHandle, UInt32 dwControlCode, UInt32 dwInValue, out UInt32 pdwOutValue);
        //停止播放
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_StopPlayBack(Int32 lVodHandle);

        // 下载指定任务单元的视频
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 VdaSdk_DownloadVideoByTaskUnit(ref TVDASDK_TASK_UNIT_DOWNLOAD_INFO ptDownloadInfo,
                                                          TfuncDownLoadVideoPosCB pfuncDownLoadPos, TfuncDownLoadVideoStatusCB pfuncDownLoadStatus, UInt32 dwUserData);
        // 查询视频下载进度
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_GetDownloadVideoPos(Int32 lDlHandle, out UInt32 pdwDownLoadState, out UInt32 pdwPercent);

        // 停止视频下载
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_StopDownloadVideo(Int32 lDlHandle);


        /*===========================================================
        功  能：获取播放视频的分辨率
        参  数：lVodHandle - 播放标示句柄
		        pdwWidth - 分辨率宽
		        pdwHeight：分辨率高
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_GetPlayResolution(Int32 lVodHandle, out UInt32 pdwWidth, out UInt32 pdwHeight );
        /*===========================================================
        功  能：抓图并获取数据
        参  数：lVodHandle - 点播标示句柄
		        dwPicType - 抓取图片的类型，见E_VDA_GRAB_PIC_TYPE
		        pPicBuf - 图片缓存区（只有在输入缓存区大小足够前提下，才会输出图片数据）
		        dwBufLen - 输入图片缓存区大小
		        dwPicDataLen - 实际图片数据大小
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_GrabPictureData(Int32 lVodHandle, UInt32 dwPicType, IntPtr pPicBuf, UInt32 dwBufLen, out UInt32 dwPicDataLen );


        /*===========================================================
        功  能：判断是否选中图形
        参  数：lVodHandle - 播放标示句柄
		        tPt - 窗口坐标
        返回值：返回TRUE表示已经选中图形(越界线或闯入\闯出区),返回FALSE表示没有选中
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_IsPlayHitGraph(Int32 lVodHandle, TVDASDK_WINDOW_POINT tPt);

        #endregion

    }


    public partial class IVXProtocol
    {

        #region 点播及下载导出接口

        /// <summary>
        /// 点播指定任务单元的视频
        /// </summary>
        /// <param name="vodInfo">点播信息</param>
        /// <param name="userData">用户数据</param>
        /// <returns>-1表示失败，其他值表示返回的点播标示值。</returns>
        public Int32 PlayBackByTaskUnit(VodInfo vodInfo, UInt32 userData)
        {
            TVDASDK_TASK_UNIT_VOD_INFO info = new TVDASDK_TASK_UNIT_VOD_INFO();
            info.bIsPlayAllFile = vodInfo.IsPlayAllFile;
            info.dwEndTime = ModelParser.ConvertLinuxTime(vodInfo.EndTime);
            info.dwStartTime = ModelParser.ConvertLinuxTime(vodInfo.StartTime);
            info.dwVideoTaskUnitID = vodInfo.VideoTaskUnitID;
            info.hPlayWnd = vodInfo.PlayWnd;
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_PlayBackByTaskUnit dwVideoTaskUnitID:{0}"
                + ",hPlayWnd:{1}"
                + ",bIsPlayAllFile:{2}"
                + ",dwStartTime:{3}"
                + ",dwEndTime:{4}"
                , info.dwVideoTaskUnitID
                , info.hPlayWnd
                , info.bIsPlayAllFile
                , info.dwStartTime
                , info.dwEndTime
                ));

            m_TfuncPlayPosCB = TfuncPlayPosCB;

            int retVal = IVXSDKProtocol.VdaSdk_PlayBackByTaskUnit(ref info, m_TfuncPlayPosCB, userData);
            if (-1 == retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }


            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_PlayBackByTaskUnit ret:" + retVal);
            return retVal;

        }

        /// <summary>
        /// 播放控制
        /// </summary>
        /// <param name="vodHandle">点播标示句柄</param>
        /// <param name="controlCode">播放控制类型，见E_VDA_PLAYCTRL_TYPE定义</param>
        /// <param name="inValue">播放控制输入参数</param>
        /// <param name="outValue">播放控制输出参数，如获取的进度等</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool PlayBackControl(Int32 vodHandle, E_VDA_PLAYCTRL_TYPE controlCode, UInt32 inValue, out UInt32 outValue)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_PlayBackControl vodHandle:{0}"
                + ",controlCode:{1}"
                + ",inValue:{2}"
                , vodHandle
                , controlCode
                , inValue
                ));

            bool retVal = IVXSDKProtocol.VdaSdk_PlayBackControl(vodHandle, (uint)controlCode, inValue, out outValue);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }


            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_PlayBackControl ret:" + retVal + ",outValue:" + outValue);
            return retVal;

        }

        /// <summary>
        /// 停止播放
        /// </summary>
        /// <param name="vodHandle">点播标示句柄</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool StopPlayBack(Int32 vodHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_StopPlayBack vodHandle:{0}"
                , vodHandle
                ));

            bool retVal = IVXSDKProtocol.VdaSdk_StopPlayBack(vodHandle);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_StopPlayBack ret:" + retVal);
            return retVal;
        }


        /// <summary>
        /// 下载指定任务单元的视频
        /// </summary>
        /// <param name="downloadInfo">下载信息</param>
        /// <param name="userData"></param>
        /// <returns>下载句柄</returns>
        public Int32 DownloadVideoByTaskUnit(DownloadInfo downloadInfo, UInt32 userData)
        {
            TVDASDK_TASK_UNIT_DOWNLOAD_INFO info = new TVDASDK_TASK_UNIT_DOWNLOAD_INFO();
            info.bIsDownloadAllFile = downloadInfo.IsDownloadAllFile;
            info.dwEndTime = ModelParser.ConvertLinuxTime(downloadInfo.EndTime);
            info.dwStartTime = ModelParser.ConvertLinuxTime(downloadInfo.StartTime);
            info.dwVideoTaskUnitID = downloadInfo.VideoTaskUnitID;
            info.szLocalSaveFilePath = downloadInfo.LocalSaveFilePath;
            
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_DownloadVideoByTaskUnit dwVideoTaskUnitID:{0}"
                + ",szLocalSaveFilePath:{1}"
                + ",bIsDownloadAllFile:{2}"
                + ",dwStartTime:{3}"
                + ",dwEndTime:{4}"
                , info.dwVideoTaskUnitID
                , info.szLocalSaveFilePath
                , info.bIsDownloadAllFile
                , info.dwStartTime
                , info.dwEndTime
                ));

            m_TfuncDownLoadVideoPosCB = TfuncDownLoadVideoPosCB;
            m_TfuncDownLoadVideoStatusCB = TfuncDownLoadVideoStatusCB;

            int retVal = IVXSDKProtocol.VdaSdk_DownloadVideoByTaskUnit(ref info, m_TfuncDownLoadVideoPosCB, m_TfuncDownLoadVideoStatusCB, userData);
            if (-1 == retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_DownloadVideoByTaskUnit ret:" + retVal);
            
            return retVal;
        }

        /// <summary>
        /// 查询视频下载进度
        /// </summary>
        /// <param name="downloadHandle">下载句柄</param>
        /// <param name="downLoadState">下载状态</param>
        /// <param name="percent">下载进度</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool GetDownloadVideoPos(Int32 downloadHandle, out VideoDownloadStatus downLoadState, out UInt32 percent)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_GetDownloadVideoPos downloadHandle:{0}"
                , downloadHandle
                ));
            uint pwdDownLoadState = 0;
            bool retVal = IVXSDKProtocol.VdaSdk_GetDownloadVideoPos(downloadHandle, out pwdDownLoadState, out percent);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            
            downLoadState = (VideoDownloadStatus)pwdDownLoadState;
            
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_GetDownloadVideoPos ret:{0}"
                + ",downLoadState:"
                + ",percent:"
                , retVal
                , pwdDownLoadState
                , percent
                ));
            return retVal;

        }

        /// <summary>
        /// 停止视频下载
        /// </summary>
        /// <param name="downloadHandle">下载句柄</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool StopDownloadVideo(Int32 downloadHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_StopDownloadVideo downloadHandle:{0}"
                , downloadHandle
                ));

            bool retVal = IVXSDKProtocol.VdaSdk_StopDownloadVideo(downloadHandle);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_StopDownloadVideo ret:{0}"
                , retVal
                ));
            return retVal;
        }



        /// <summary>
        /// 抓图并获取数据
        /// </summary>
        /// <param name="vodHandle">点播标示句柄</param>
        /// <returns>图片</returns>
        public System.Drawing.Image GrabPicture(Int32 vodHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_GrabPictureData vodHandle:{0}"
                , vodHandle
                ));
           uint pictype = (uint)E_VDA_GRAB_PIC_TYPE.E_GRAB_PIC_BMP;
            uint buflen = 10*1024*1024;
            IntPtr picbuf = Marshal.AllocHGlobal((int)buflen);
            uint picdatalen =0;
            bool retVal = IVXSDKProtocol.VdaSdk_GrabPictureData(vodHandle,pictype,picbuf,buflen,out picdatalen);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            System.Drawing.Image img = ModelParser.GetImage(picbuf, (int)picdatalen);

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_GrabPictureData ret:{0},picdatalen:{1}"
                , retVal
                , picdatalen
                ));
            if(picbuf!=IntPtr.Zero)
                Marshal.FreeHGlobal(picbuf);
            return img;

        }

        #endregion

        /// <summary>
        /// 获取播放视频的分辨率
        /// </summary>
        /// <param name="lVodHandle">播放标示句柄</param>
        /// <param name="pdwWidth">分辨率宽</param>
        /// <param name="pdwHeight">分辨率高</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool GetPlayResolution(Int32 lVodHandle, out UInt32 pdwWidth, out UInt32 pdwHeight)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_GetPlayResolution lVodHandle:{0}"
                , lVodHandle
                ));

            bool retVal = IVXSDKProtocol.VdaSdk_GetPlayResolution(lVodHandle,out  pdwWidth,out pdwHeight);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }


            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_GetPlayResolution ret:{0}"
                + ",pdwWidth:{1}"
                + ",pdwHeight:{2}"
                , retVal
                , pdwWidth
                , pdwHeight
                ));
            return retVal;

        }

        /// <summary>
        /// 判断是否选中图形(越界线或闯入\闯出区)
        /// </summary>
        /// <param name="lVodHandle">播放标示句柄</param>
        /// <param name="tPt">窗口坐标</param>
        /// <returns>返回TRUE表示已经选中图形,返回FALSE表示没有选中</returns>
        public bool IsPlayHitGraph(Int32 lVodHandle, System.Drawing.Point point)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_IsPlayHitGraph lVodHandle:{0},x:{1},y:{2}"
                , lVodHandle
                ,point.X
                ,point.Y
                ));
            TVDASDK_WINDOW_POINT tPt = new TVDASDK_WINDOW_POINT() { dwX = (uint)point.X, dwY = (uint)point.Y };
            bool retVal = IVXSDKProtocol.VdaSdk_IsPlayHitGraph(lVodHandle, tPt);

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("IVXSDKProtocol VdaSdk_IsPlayHitGraph ret:{0}"
                , retVal
                ));
            return retVal;

        }

    }
}
