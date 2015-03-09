using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.DataModel;
using System.Diagnostics;
using BOCOM.IVX.Framework;

namespace BOCOM.IVX.Service
{
    public class VideoDownloadService
    {
        private Dictionary<int, DownloadInfo> m_DTSessionId2DownloadInfo;

        private object m_SyncObj = new object();

        #region Constructors
        
        public VideoDownloadService()
        {
            m_DTSessionId2DownloadInfo = new Dictionary<int, DownloadInfo>();

            Framework.Container.Instance.IVXProtocol.VideoDownloadProgressUpdate += new Action<int, uint, uint, uint, uint>(IVXProtocol_VideoDownloadProgressUpdate);
            Framework.Container.Instance.IVXProtocol.VideoDownloadStatusUpdate += new Action<int, uint, uint, uint>(IVXProtocol_VideoDownloadStatusUpdate);
        }

        #endregion

        private DownloadInfo GetDownloadInfo(int sessionId)
        {
            DownloadInfo downloadInfo = null;

            lock (m_SyncObj)
            {
                if (m_DTSessionId2DownloadInfo.ContainsKey(sessionId))
                {
                    downloadInfo = m_DTSessionId2DownloadInfo[sessionId];
                }
            }

            return downloadInfo;
        }

        private void AddDownloadInfo(int sessionId, DownloadInfo downloadInfo)
        {
            lock (m_SyncObj)
            {
                Debug.Assert(!m_DTSessionId2DownloadInfo.ContainsKey(sessionId));

                if (!m_DTSessionId2DownloadInfo.ContainsKey(sessionId))
                {
                    m_DTSessionId2DownloadInfo.Add(sessionId, downloadInfo);
                }
            }
        }
        
        #region Public helper functions

        public int StartDownload(DownloadInfo downloadInfo)
        {
            TaskUnitInfo taskunitinfo = Framework.Container.Instance.TaskManagerService.GetTaskUnitById(downloadInfo.VideoTaskUnitID);
            if (taskunitinfo != null)
            {
                if (downloadInfo.StartTime < DataModel.Common.ZEROTIME)
                    downloadInfo.StartTime = taskunitinfo.StartTime;

                if (downloadInfo.EndTime < DataModel.Common.ZEROTIME)
                    downloadInfo.EndTime = taskunitinfo.EndTime;
            }
            int sessionId = Framework.Container.Instance.IVXProtocol.DownloadVideoByTaskUnit(downloadInfo, 0);

            if (sessionId > 0)
            {
                AddDownloadInfo(sessionId, downloadInfo);
            }

            return sessionId;
        }

        public void StopDownload(int sessionId)
        {
            lock (m_SyncObj)
            {
                if (m_DTSessionId2DownloadInfo.ContainsKey(sessionId))
                {
                    Framework.Container.Instance.IVXProtocol.StopDownloadVideo(sessionId);
                    m_DTSessionId2DownloadInfo.Remove(sessionId);
                }
            }
        }

        //public void GetDownloadStatusnProgress(int sessionId, out VideoDownloadStatus status, out uint percent)
        //{
        //    status = VideoDownloadStatus.NOUSE;
        //    percent = 0;

        //    lock (m_SyncObj)
        //    {
        //        if (m_DTSessionId2DownloadInfo.ContainsKey(sessionId))
        //        {
        //            DownloadInfo downloadInfo = m_DTSessionId2DownloadInfo[sessionId];
        //            Framework.Container.Instance.IVXProtocol.GetDownloadVideoPos(sessionId, out status, out percent);

        //            downloadInfo.TotalProgress = percent;
        //            downloadInfo.Status = status;
        //        }
        //    }
        //}

        public List<DownloadInfo> GetAllDownloadInfos()
        {
            List<DownloadInfo> downloadInfos = null;
            lock (m_SyncObj)
            {
                downloadInfos = m_DTSessionId2DownloadInfo.Values.ToList();
            }
            return downloadInfos;
        }

        #endregion

        #region Event handlers
        
        void IVXProtocol_VideoDownloadStatusUpdate(int sessionId, uint nStatus, uint nResult, uint userData)
        {
            DownloadInfo downloadInfo = GetDownloadInfo(sessionId);

            if (downloadInfo != null)
            {
                downloadInfo.Status = (VideoDownloadStatus)nStatus;
                downloadInfo.ErrorCode = nResult;
                
                Framework.Container.Instance.EvtAggregator.GetEvent<VideoDownloadStatusUpdateEvent>().Publish(downloadInfo);
            }
        }

        void IVXProtocol_VideoDownloadProgressUpdate(int sessionId, uint dwTransProgress, uint dwExportProgress, uint dwCombineProgress, uint userData)
        {
            DownloadInfo downloadInfo = GetDownloadInfo(sessionId);

            if (downloadInfo != null)
            {
                downloadInfo.ConversionProgress = dwTransProgress;
                downloadInfo.ExportProgress = dwExportProgress;
                downloadInfo.ComposeProgress = dwCombineProgress;

                Framework.Container.Instance.EvtAggregator.GetEvent<VideoDownloadProgressUpdateEvent>().Publish(downloadInfo);
            }
        }

        #endregion
    }
}
