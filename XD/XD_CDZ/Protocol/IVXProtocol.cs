using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.Serialization;
using BOCOM.IVX.Protocol.Model;
using BOCOM.IVX.Protocol;
using BOCOM.DataModel;


namespace BOCOM.IVX.Protocol
{

    #region 委托定义
    /// <summary>
    /// 网络连接断开回调函数
    /// </summary>
    /// <param name="userData">用户参数</param>
    public delegate void DelegateDisConnectd(UInt32 userData);
    /// <summary>
    /// 任务状态回调函数
    /// </summary>
    /// <param name="taskID">任务ID</param>
    /// <param name="taskStatus">任务状态</param>
    /// <param name="userData">用户数据</param>
    public delegate void DelegateTaskStatus(UInt32 taskID, UInt32 taskStatus, UInt32 userData);

    /// <summary>
    /// 任务进度回调函数
    /// </summary>
    /// <param name="taskID">任务ID</param>
    /// <param name="taskProgress">任务进度，千分比</param>
    /// <param name="taskLeftTimeS">任务剩余时间</param>
    /// <param name="userData">用户数据</param>
    public delegate void DelegateTaskProgress(UInt32 taskID, UInt32 taskProgress, UInt32 taskLeftTimeS, UInt32 userData);


    /// <summary>
    /// 任务单元状态回调函数
    /// </summary>
    /// <param name="taskUnitID">任务单元ID</param>
    /// <param name="taskUnitImportStatus">任务单元导入状态</param>
    /// <param name="analyzeStatus">任务单元分析状态</param>
    /// <param name="analyzeStatusNumber">任务单元分析状态数目</param>
    /// <param name="userData">用户数据</param>
    public delegate void DelegateTaskUnitStatus(UInt32 taskUnitID, UInt32 taskUnitImportStatus,
                                                  Dictionary<E_VDA_ANALYZE_TYPE, E_VDA_TASK_UNIT_STATUS> analyzeStatus, UInt32 analyzeStatusNumber, UInt32 userData);

    /// <summary>
    /// 任务单元进度回调函数
    /// </summary>
    /// <param name="taskUnitID">任务单元ID</param>
    /// <param name="taskUnitProgress">任务单元总进度，千分比</param>
    /// <param name="taskUnitLeftTimeS">任务单元总剩余时间</param>
    /// <param name="userData">用户数据</param>
    public delegate void DelegateTaskUnitProgress(UInt32 taskUnitID, UInt32 taskUnitProgress,
                                                   UInt32 taskUnitLeftTimeS, UInt32 userData);


    /// <summary>
    /// 案件配置通知
    /// </summary>
    /// <param name="caseID">案件编号</param>
    /// <param name="cfgNotifyType">配置通知类型，见E_VDA_SDK_CFG_NOTIFY_TYPE定义</param>
    /// <param name="userData"></param>
    public delegate void DelegateCaseChanged(UInt32 caseID, E_VDA_SDK_CFG_NOTIFY_TYPE cfgNotifyType, UInt32 userData);



    /// <summary>
    /// 播放进度回调函数
    /// </summary>
    /// <param name="vodHandle">播放标示</param>
    /// <param name="playState">播放状态，见E_VDA_PLAY_STATUS</param>
    /// <param name="playPercent">播放进度，（百分比整数值，80表示80%）</param>
    /// <param name="curPlayTime">当前播放时间（绝对时间）</param>
    /// <param name="userData">用户数据</param>
    public delegate void DelegatePlayPos(Int32 vodHandle, E_VDA_PLAY_STATUS playState,
                                             UInt32 playPercent, UInt32 curPlayTime, UInt32 userData);

    /// <summary>
    /// 下载进度回调函数
    /// </summary>
    /// <param name="downloadHandle">下载标示</param>
    /// <param name="downLoadState">下载状态，见E_VDA_DOWNLOAD_STATUS</param>
    /// <param name="percent">下载进度，（百分比整数值，80表示80%）</param>
    /// <param name="userData">用户数据</param>
    public delegate void DelegateDownLoadVideoPos(Int32 downloadHandle, E_VDA_DOWNLOAD_STATUS downLoadState, UInt32 percent, UInt32 userData);

    #endregion

    public partial class IVXProtocol
    {
        #region 事件
        /// <summary>
        /// 网络连接端口消息
        /// </summary>
        public event DelegateDisConnectd EventDisConnectd;
        /// <summary>
        /// 任务状态消息
        /// </summary>
        public event DelegateTaskStatus EvenTaskStatus;
        /// <summary>
        /// 任务进度消息
        /// </summary>
        public event DelegateTaskProgress EvenTaskProgress;
        /// <summary>
        /// 任务单元状态消息
        /// </summary>
        public event DelegateTaskUnitStatus EvenTaskUnitStatus;
        /// <summary>
        /// 任务单元进度消息
        /// </summary>
        public event DelegateTaskUnitProgress EvenTaskUnitProgress;
        /// <summary>
        /// 案件配置消息
        /// </summary>
        public event DelegateCaseChanged EventCaseChanged;
        /// <summary>
        /// 播放进度消息
        /// </summary>
        public event DelegatePlayPos EventPlayPos;
        /// <summary>
        /// 下载进度消息
        /// </summary>
        
        public event Action<Int32, UInt32, UInt32, UInt32, UInt32> VideoDownloadProgressUpdate;

        public event Action<Int32, UInt32, UInt32, UInt32> VideoDownloadStatusUpdate;

        public event Action<int, E_VDA_PLAY_STATUS, uint ,uint,uint, uint > EventBriefVideoPlayProgress;

        public event Action<int, E_VDA_EXPORT_STATUS, uint, uint> EventBriefVideoExportProgress;

        public event Action<int ,E_VDA_BRIEF_WND_MOUSE_OPT_TYPE, uint, uint, uint> EventBriefVideoWindowMouseClick;

        public event Action<SearchItemResult> SearchItemResultReceived;

        public event Action<SearchImageInfo> SearchItemImageReceived;

        public event Action<List<LogSearchResultInfo>> LogSearchItemReceived;

        public event Action<ResourceUpdateInfo> ResourceChanged;


        #endregion

        #region 字段

        private TfuncTaskStatusNtfCB m_TfuncTaskStatusNtfCB;
        private TfuncTaskProgressNtfCB m_TfuncTaskProgressNtfCB;
        private TfuncTaskUnitStatusNtfCB m_TfuncTaskUnitStatusNtfCB;
        private TfuncTaskUnitProgressNtfCB m_TfuncTaskUnitProgressNtfCB;

        private TfuncSearchResultTaskUnitPageNtfCB m_TfuncSearchResultByTaskUniNtfCB;

        private TfuncSearchVehicleResultTaskUnitPageNtfCB m_TfuncSearchVehicleResultTaskUnitPageNtfCB;

        private TfuncGetImagetNtfCB m_TfuncGetImagetNtfCB;

        private TfuncBriefPlayPosCB m_BriefPlayPosCB;

        private TfuncBriefExportPosCB m_BriefExportPosCB;

        private TfuncBriefWndMouseOptNtfCB m_BriefWndMouseOptNtfCB;

        private TfuncDisConnectNtfCB m_TfuncDisConnectNtfCB;

        private TfuncCaseCfgNtfCB m_TfuncCaseCfgNtfCB;

        private TfuncPlayPosCB m_TfuncPlayPosCB;

        private TfuncDownLoadVideoPosCB m_TfuncDownLoadVideoPosCB;

        private TfuncDownLoadVideoStatusCB m_TfuncDownLoadVideoStatusCB;

        private TfuncPlayWndMouseOptNtfCB m_pfuncMouseOptNtf;

        private TFuncPDOMouseOptNtfCB m_pfuncMouseEventCb;

        private TfuncLogSearchNtfCB m_pfuncLogSearchNtfCB;

        private TfuncResourceUpdateNtfCB m_pfuncResourceUpdateNtfCB;

        #endregion

        public IVXProtocol(bool bCreateJVM)
        {
            if (Init(bCreateJVM))
            {
                m_TfuncTaskStatusNtfCB = TfuncTaskStatusNtfCB;
                m_TfuncTaskProgressNtfCB = TfuncTaskProgressNtfCB;
                m_TfuncTaskUnitStatusNtfCB = TfuncTaskUnitStatusNtfCB;
                m_TfuncTaskUnitProgressNtfCB = TfuncTaskUnitProgressNtfCB;
                m_TfuncCaseCfgNtfCB = TfuncCaseCfgNtfCB;
                m_BriefPlayPosCB = OnBriefPlayPosCB;
                m_BriefExportPosCB = OnBriefExportPosCB;
                m_BriefWndMouseOptNtfCB = OnBriefWndMouseOptNtfCB;

                m_TfuncSearchResultByTaskUniNtfCB = OnSearchResultTaskUnitNtfCB;
                m_TfuncGetImagetNtfCB = OnGetImagetNtfCB;

                m_pfuncLogSearchNtfCB = OnLogSearchNtfCB;

                m_TfuncSearchVehicleResultTaskUnitPageNtfCB = OnSearchVehicleResultTaskUnitNtfCB;
                m_pfuncResourceUpdateNtfCB = OnResourceUpdateNtfCB;

                bool retVal = false;
                retVal = IVXSDKProtocol.VdaSdk_TaskStatusNtfCBReg(m_TfuncTaskStatusNtfCB, 0);
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_TaskStatusNtfCBReg");
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_TaskStatusNtfCBReg");
                if (!retVal)
                {
                    // 调用失败，抛异常
                    CheckError();
                }

                retVal = IVXSDKProtocol.VdaSdk_TaskProgressNtfCBReg(m_TfuncTaskProgressNtfCB, 0);
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_TaskProgressNtfCBReg");
                if (!retVal)
                {
                    // 调用失败，抛异常
                    CheckError();
                }

                retVal = IVXSDKProtocol.VdaSdk_TaskUnitStatusNtfCBReg(m_TfuncTaskUnitStatusNtfCB, 0);
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_TaskUnitStatusNtfCBReg");
                if (!retVal)
                {
                    // 调用失败，抛异常
                    CheckError();
                }

                retVal = IVXSDKProtocol.VdaSdk_TaskUnitProgressNtfCBReg(m_TfuncTaskUnitProgressNtfCB, 0);
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_TaskUnitProgressNtfCBReg");
                if (!retVal)
                {
                    // 调用失败，抛异常
                    CheckError();
                }

                retVal = IVXSDKProtocol.VdaSdk_SearchResultTaskUnitPageNtfCBReg(m_TfuncSearchResultByTaskUniNtfCB);
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_SearchResultTaskUnitPageNtfCBReg");
                if (!retVal)
                {
                    // 调用失败，抛异常
                    CheckError();
                }

                retVal = IVXSDKProtocol.VdaSdk_SearchVehicleResultTaskUnitPageNtfCBReg(m_TfuncSearchVehicleResultTaskUnitPageNtfCB);
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_SearchResultTaskUnitPageNtfCBReg");
                if (!retVal)
                {
                    // 调用失败，抛异常
                    CheckError();
                }

                retVal = IVXSDKProtocol.VdaSdk_SearchGetImagetNtfCBReg(m_TfuncGetImagetNtfCB);
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_SearchGetImagetNtfCBReg");
                if (!retVal)
                {
                    // 调用失败，抛异常
                    CheckError();
                }

                retVal = IVXSDKProtocol.VdaSdk_LogSearchNtfCBReg(m_pfuncLogSearchNtfCB, 0);
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_LogSearchNtfCBReg");
                if (!retVal)
                {
                    // 调用失败，抛异常
                    CheckError();
                }

                retVal = IVXSDKProtocol.VdaSdk_ResourceUpdateNtfCBReg(m_pfuncResourceUpdateNtfCB, 0);
                MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_ResourceUpdateNtfCBReg");
                if (!retVal)
                {
                    // 调用失败，抛异常
                    CheckError();
                }


            }
        }

        private void CheckError(bool includeRecordNotFound = false)
        {
            uint errorCode = 0;
            string error = GetLastError(out errorCode);
            if (errorCode > 0)
            {
                if (!includeRecordNotFound && errorCode == IVXSDKProtocol.ERRORCODE_RECORDNOTEXIST)
                {
                    Trace.Write(string.Format("CheckError: Reocrd not exist"));
                }
                else
                {
                    Trace.Write(string.Format("SDKCallException errorCode:{0},errorString:{1}", errorCode, error));
                    if (string.IsNullOrEmpty(error))
                    {
                        Debug.Assert(false, "Failed but cannot get error message!");
                    }
                    throw new SDKCallException(errorCode, error);
                }
            }
            else
            {
                Debug.Assert(false, "No valid error code!");
            }
        }

        #region 回调响应

        void TfuncDisConnectNtfCB(UInt32 dwUserData)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"TfuncDisConnectNtfCB ");

            if (EventDisConnectd != null)
                EventDisConnectd(dwUserData);
        }
        
        void TfuncTaskStatusNtfCB(UInt32 dwTaskID, UInt32 dwTaskStatus, UInt32 dwUserData)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("TfuncTaskStatusNtfCB dwTaskID:{0}"
                + ",dwTaskStatus:{1}"
                + ",dwUserData:{2}"
                , dwTaskID
                , dwTaskStatus
                , dwUserData));

            if (EvenTaskStatus != null)
                EvenTaskStatus(dwTaskID, dwTaskStatus, dwUserData);
        }
        
        void TfuncTaskProgressNtfCB(UInt32 dwTaskID, UInt32 dwTaskProgress, UInt32 dwTaskLeftTimeS, UInt32 dwUserData)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("TfuncTaskProgressNtfCB dwTaskID:{0}"
                + ",dwTaskProgress:{1}"
                + ",dwTaskLeftTimeS:{2}"
                + ",dwUserData:{3}"
                , dwTaskID
                , dwTaskProgress
                , dwTaskLeftTimeS
                , dwUserData));

            if (EvenTaskProgress != null)
                EvenTaskProgress(dwTaskID, dwTaskProgress, dwTaskLeftTimeS, dwUserData);
        }
        
        void TfuncTaskUnitStatusNtfCB(UInt32 dwTaskUnitID, UInt32 dwTaskUnitImportStatus, IntPtr/* TVDASDK_TASK_UNIT_ANALYZE_STATUS*  */ pAnalyzeStatus, UInt32 dwAnalyzeStatusNumber, UInt32 dwUserData)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("TfuncTaskUnitStatusNtfCB dwTaskUnitID:{0}"
                + ",dwTaskUnitImportStatus:{1}"
                + ",dwAnalyzeStatusNumber:{2}"
                + ",dwUserData:{3}"
                , dwTaskUnitID
                , dwTaskUnitImportStatus
                , dwAnalyzeStatusNumber
                , dwUserData));

            if (EvenTaskUnitStatus != null)
            {
                Dictionary<E_VDA_ANALYZE_TYPE, E_VDA_TASK_UNIT_STATUS> list = new Dictionary<E_VDA_ANALYZE_TYPE, E_VDA_TASK_UNIT_STATUS>();
                for (int i = 0; i < dwAnalyzeStatusNumber; i++)
                {
                    TVDASDK_TASK_UNIT_ANALYZE_STATUS status = new TVDASDK_TASK_UNIT_ANALYZE_STATUS();
                    status = (TVDASDK_TASK_UNIT_ANALYZE_STATUS)Marshal.PtrToStructure(pAnalyzeStatus + Marshal.SizeOf(typeof(TVDASDK_TASK_UNIT_ANALYZE_STATUS)) * i, typeof(TVDASDK_TASK_UNIT_ANALYZE_STATUS));
                    if (!list.ContainsKey((E_VDA_ANALYZE_TYPE)status.dwAnalyzeType))
                        list.Add((E_VDA_ANALYZE_TYPE)status.dwAnalyzeType, (E_VDA_TASK_UNIT_STATUS)status.dwAnalyzeStatus);
                }

                EvenTaskUnitStatus(dwTaskUnitID, dwTaskUnitImportStatus, list, dwAnalyzeStatusNumber, dwUserData);
            }
        }
        
        void TfuncTaskUnitProgressNtfCB(UInt32 dwTaskUnitID, UInt32 dwTaskUnitProgress, UInt32 dwTaskUnitLeftTimeS, UInt32 dwUserData)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("TfuncTaskUnitProgressNtfCB dwTaskUnitID:{0}"
                + ",dwTaskUnitProgress:{1}"
                + ",dwTaskUnitLeftTimeS:{2}"
                + ",dwUserData:{3}"
                , dwTaskUnitID
                , dwTaskUnitProgress
                , dwTaskUnitLeftTimeS
                , dwUserData));

            if (EvenTaskUnitProgress != null)
                EvenTaskUnitProgress(dwTaskUnitID, dwTaskUnitProgress, dwTaskUnitLeftTimeS, dwUserData);
        }
        
        void TfuncCaseCfgNtfCB(UInt32 dwCaseID, UInt32 dwCfgNotifyType, UInt32 dwUserData)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("TfuncCaseCfgNtfCB dwCaseID:{0}"
                + ",dwCfgNotifyType:{1}"
                + ",dwUserData:{2}"
                , dwCaseID
                , dwCfgNotifyType
                , dwUserData));
                
            if (EventCaseChanged != null)
                EventCaseChanged(dwCaseID, (E_VDA_SDK_CFG_NOTIFY_TYPE)dwCfgNotifyType, dwUserData);
        }
        
        void TfuncPlayPosCB(Int32 lVodHandle, UInt32 dwPlayState, UInt32 dwPlayPercent, UInt32 dwCurPlayTime, UInt32 dwUserData)
        {
            //MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("TfuncPlayPosCB lVodHandle:{0}"
            //    +",dwPlayState:{1}"
            //    + ",dwPlayPercent:{2}"
            //    + ",dwCurPlayTime:{3}"
            //    +",dwUserData:{4}"
            //    , lVodHandle
            //    ,  dwPlayState
            //    , dwPlayPercent
            //    , dwCurPlayTime
            //    ,  dwUserData));
            if (EventPlayPos != null)
                EventPlayPos(lVodHandle, (E_VDA_PLAY_STATUS)dwPlayState, dwPlayPercent, dwCurPlayTime, dwUserData);
        }
        
        void TfuncDownLoadVideoPosCB(Int32 lDlHandle, UInt32 dwTransProgress, UInt32 dwExportProgress, UInt32 dwCombineProgress, UInt32 dwUserData)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("TfuncDownLoadVideoPosCB lDlHandle:{0}"
                + ",dwTransProgress:{1}"
                + ",dwExportProgress:{2}"
                + ",dwCombineProgress:{3}"
                + ",dwUserData:{4}"
                , lDlHandle
                , dwTransProgress
                , dwExportProgress
                , dwCombineProgress
                , dwUserData));

            if (VideoDownloadProgressUpdate != null)
            {
                VideoDownloadProgressUpdate(lDlHandle, dwTransProgress, dwExportProgress, dwCombineProgress, dwUserData);
            }
        }

        void TfuncDownLoadVideoStatusCB(Int32 lDlHandle, UInt32 dwDownLoadStatus, UInt32 nResult, UInt32 dwUserData)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, string.Format("TfuncDownLoadVideoStatusCB lDlHandle:{0}"
                + ",dwDownLoadStatus:{1}"
                + ",dwUserData:{2}"
                , lDlHandle
                , dwDownLoadStatus
                , dwUserData));

            if (VideoDownloadStatusUpdate != null)
            {
                VideoDownloadStatusUpdate(lDlHandle, dwDownLoadStatus, nResult, dwUserData);
            }
        }

        void OnBriefPlayPosCB(int lBriefHandle, uint dwPlayState, uint dwPlayProgress, uint dwCurPlayTime,uint dwResult, uint dwUserData)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("OnBriefPlayPosCB lBriefHandle:{0}"
                +",dwPlayState:{1}"
                +",dwPlayProgress:{2}"
                + ",dwCurPlayTime:{3}"
                + ",dwResult:{4}"
                +",dwUserData:{5}"
                ,lBriefHandle
                ,  dwPlayState
                ,  dwPlayProgress
                , dwCurPlayTime
                , dwResult
                ,  dwUserData));

            if (EventBriefVideoPlayProgress != null)
            {
                EventBriefVideoPlayProgress(lBriefHandle, (E_VDA_PLAY_STATUS)dwPlayState, dwPlayProgress,dwCurPlayTime,dwResult, dwUserData);
            }
        }

        void OnBriefExportPosCB(int lBriefHandle, uint dwExportState, uint dwProgress, uint dwUserData)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("OnBriefExportPosCB lBriefHandle:{0}"
                + ",dwExportState:{1}"
                + ",dwProgress:{2}"
                + ",dwUserData:{3}"
                , lBriefHandle
                , dwExportState
                , dwProgress
                , dwUserData));

            if (EventBriefVideoExportProgress != null)
            {
                EventBriefVideoExportProgress(lBriefHandle, (E_VDA_EXPORT_STATUS)dwExportState, dwProgress, dwUserData);
            }
        }

        void OnBriefWndMouseOptNtfCB(int lBriefHandle, uint dwOptType, uint dwX, uint dwY, uint dwUserData)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("OnBriefWndMouseOptNtfCB lBriefHandle:{0}"
                + ",dwOptType:{1}"
                + ",dwX:{2}"
                + ",dwY:{3}"
                + ",dwUserData:{4}"
                , lBriefHandle
                , dwOptType
                , dwX
                , dwY
                , dwUserData));
            
            if (EventBriefVideoWindowMouseClick != null)
            {
                EventBriefVideoWindowMouseClick(lBriefHandle, (E_VDA_BRIEF_WND_MOUSE_OPT_TYPE)dwOptType, dwX, dwY, dwUserData);
            }
        }

        private void OnSearchResultTaskUnitNtfCB(uint dwQueryHandle, uint dwTaskUnitID, TVDASDK_SEARCH_RESULT_PAGE_INFO tResultPageInfo, IntPtr ptSearchResultObjInfoStart, uint dwUserData)
        {
            MyLog4Net.Container.Instance.Log.InfoFormat("OnSearchResultTaskUnitNtfCB dwQueryHandle:{0} ,dwTaskUnitID:{1}, totlecount: {2}, result: {3}"
                , dwQueryHandle, dwTaskUnitID, tResultPageInfo.dwResultTotalNum, tResultPageInfo.dwResult);
            
            SearchItemResult result = ModelParser.GetSearchItemResult(dwQueryHandle, dwTaskUnitID, tResultPageInfo, ptSearchResultObjInfoStart);
            if (SearchItemResultReceived != null)
            {
                SearchItemResultReceived(result);
            }
        }

        private void OnSearchVehicleResultTaskUnitNtfCB(uint dwQueryHandle, uint dwTaskUnitID, TVDASDK_SEARCH_RESULT_PAGE_INFO tResultPageInfo, IntPtr ptSearchResultObjInfoStart, uint dwUserData)
        {
            MyLog4Net.Container.Instance.Log.InfoFormat("OnSearchVehicleResultTaskUnitNtfCB dwQueryHandle:{0} ,dwTaskUnitID:{1}, totlecount: {2}, result: {3}"
                , dwQueryHandle, dwTaskUnitID,  tResultPageInfo.dwResultTotalNum, tResultPageInfo.dwResult);

            SearchItemResult result = ModelParser.GetVehicleSearchItemResult(dwQueryHandle, dwTaskUnitID, tResultPageInfo, ptSearchResultObjInfoStart);
            if (SearchItemResultReceived != null)
            {
                SearchItemResultReceived(result);
            }
        }

        private void OnGetImagetNtfCB(uint dwSessionID, TVDASDK_SEARCH_GET_IMAGE_FILTER tSearchConditionFilter, TVDASDK_SEARCH_IMAGE_INFO tImageInfo, uint dwUserData)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("OnGetImagetNtfCB dwSessionID:{0}"
                + ",szURLPath:{1}"
                + ",dwCameraID:{2}"
                + ",dwMoveObjID:{3}"
                + ",dwTaskUnitID:{4}"
                + ",ImageSize:{5}"
                + ",dwUserData:{6}"
                , dwSessionID
                , tSearchConditionFilter.szURLPath
                , tSearchConditionFilter.tObjID.dwCameraID
                , tSearchConditionFilter.tObjID.dwMoveObjID
                , tSearchConditionFilter.tObjID.dwTaskUnitID
                , tImageInfo.dwImageSize
                , dwUserData));

            if (tImageInfo.dwImageSize <= 0)
            {
                return;
            }

            if (SearchItemImageReceived != null)
            {
                SearchImageInfo info = new SearchImageInfo();
                info.dwCameraID = tSearchConditionFilter.tObjID.dwCameraID;
                info.dwImageSize = tImageInfo.dwImageSize;
                info.dwMoveObjID = tSearchConditionFilter.tObjID.dwMoveObjID;
                info.dwTaskUnitID = tSearchConditionFilter.tObjID.dwTaskUnitID;
                info.ImageURL = tSearchConditionFilter.szURLPath;
                info.Image = ModelParser.GetImage(tImageInfo.ptImageData, (int)tImageInfo.dwImageSize);

                SearchItemImageReceived(info);
            }

        }

        private void OnPlayWndMouseOptNtfCB(Int32 lVodHandle, UInt32 dwOptType, UInt32 dwX, UInt32 dwY, UInt32 dwUserData)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"OnPlayWndMouseOptNtfCB ");

        }
        private void OnPDOMouseOptNtfCB(Int32 hPdoHandle, E_PDO_MOUSE_EVENT eMouseEvent,
                                               UInt32 dwX, UInt32 dwY, UInt32 dwUserData)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"OnPDOMouseOptNtfCB ");


        }

        private void OnLogSearchNtfCB(IntPtr ptLogInfoList, UInt32 dwLogNum, UInt32 dwUserData)
        {
            List<LogSearchResultInfo> logList = new List<LogSearchResultInfo>();
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"OnLogSearchNtfCB ");
            if(LogSearchItemReceived!=null)
            {   
                for(int i=0;i<dwLogNum;i++)
                {
                    TLOG_SEARCH_RESULT ptLogInfo = (TLOG_SEARCH_RESULT)Marshal.PtrToStructure(ptLogInfoList + i * Marshal.SizeOf(typeof(TLOG_SEARCH_RESULT)), typeof(TLOG_SEARCH_RESULT));
                LogSearchResultInfo info = new LogSearchResultInfo();
                info.Description = ptLogInfo.szDescription;
                info.Level = (E_VDA_LOG_LEVEL)ptLogInfo.dwLevel;
                info.LogDetail = (E_VDA_LOG_DETAIL) ptLogInfo.dwLogDetail;
                info.LogId = ptLogInfo.qwLogSqlId;
                info.LogTime = ptLogInfo.szTime;
                info.LogType = (E_VDA_LOG_TYPE) ptLogInfo.dwLogType;
                info.OptId = ptLogInfo.dwOptId;
                info.OptName = ptLogInfo.szOptName;
                info.OptType = (E_LOG_OPERATE_TYPE) ptLogInfo.dwOptType;
                logList.Add(info);
                }
                

                LogSearchItemReceived(logList);
                
            }
        }


        private void OnResourceUpdateNtfCB(string szOperatorName, UInt32 dwOperateType,
                                                UInt32 dwResourceType, IntPtr ptResourceID, UInt32 dwResourceNum, UInt32 dwUserData)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "OnResourceUpdateNtfCB dwOperateType:" + (E_VDA_RESOURCE_OPERATE_TYPE)dwOperateType + " ,dwResourceType:" + (E_VDA_RESOURCE_TYPE)dwResourceType);
            if (ResourceChanged != null)
            {
                ResourceUpdateInfo info = new ResourceUpdateInfo();
                info.OperateType = (E_VDA_RESOURCE_OPERATE_TYPE)dwOperateType;
                info.OperatorName = szOperatorName;
                info.ResourceNum = dwResourceNum;
                info.ResourceIDList = new List<uint>();
                for (int i = 0; i < dwResourceNum; i++)
                {
                    uint unitid = (uint)Marshal.PtrToStructure(ptResourceID + Marshal.SizeOf(typeof(uint)) * i, typeof(uint));
                    if (!info.ResourceIDList.Contains(unitid))
                        info.ResourceIDList.Add(unitid);
                }

                info.ResourceType = (E_VDA_RESOURCE_TYPE)dwResourceType;
                info.UserData = dwUserData;
                ResourceChanged(info);
            }
        }

        
        #endregion
    }

}