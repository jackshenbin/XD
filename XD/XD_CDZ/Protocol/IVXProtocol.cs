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


        #endregion

        #region 字段

        private TfuncDisConnectNtfCB m_TfuncDisConnectNtfCB;

        #endregion

        public IVXProtocol(bool bCreateJVM)
        {
            if (Init(bCreateJVM))
            {
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
        

        
        #endregion
    }

}