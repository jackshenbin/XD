using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BOCOM.IVX.Framework;
using Microsoft.Practices.Prism.Events;
using BOCOM.DataModel;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Common;

namespace BOCOM.IVX.ViewModel
{
    public class DownloadInfoListViewModel :ViewModelBase
    {
        #region Fields
        
        private DataTable m_DTDownloadInfo;

        #endregion

        #region Properties

        public DownloadInfo SelectedItem
        {
            get;
            set;
        }

        public DataTable DTDownloadInfo
        {
            get { return m_DTDownloadInfo; }
        }

        public int RecordCount
        {
            get
            {
                return m_DTDownloadInfo.Rows.Count;
            }
            set
            {
                RaisePropertyChangedEvent("RecordCount");
            }
        }

        #endregion

        #region Private helper functions
        
        private void Init()
        {
            m_DTDownloadInfo = new DataTable();
            DataColumn column = m_DTDownloadInfo.Columns.Add("SessionId", typeof(int));
            m_DTDownloadInfo.PrimaryKey = new DataColumn[] { column };

            m_DTDownloadInfo.Columns.Add("ItemName", typeof(string));
            m_DTDownloadInfo.Columns.Add("FileNmae", typeof(string));
            m_DTDownloadInfo.Columns.Add("Status", typeof(string));
            m_DTDownloadInfo.Columns.Add("Progress", typeof(string));
            m_DTDownloadInfo.Columns.Add("DownloadPath", typeof(string));
            m_DTDownloadInfo.Columns.Add("Cancel", typeof(string));
            m_DTDownloadInfo.Columns.Add("DownloadInfo", typeof(DownloadInfo));

            FillUpTable();
        }

        private void FillUpTable()
        {
            List<DownloadInfo> downloadInfos = Framework.Container.Instance.VideoDownloadService.GetAllDownloadInfos();

            if (downloadInfos != null && downloadInfos.Count > 0)
            {
                foreach (DownloadInfo downloadInfo in downloadInfos)
                {
                    AddRow(downloadInfo);
                }
            }
        }

        private void AddRow(DownloadInfo downloadInfo)
        {
            TaskUnitInfo tuInfo = Framework.Container.Instance.TaskManagerService.GetTaskUnitById(downloadInfo.VideoTaskUnitID);

            if (tuInfo != null)
            {
                string temp = "";
                switch (downloadInfo.Status)
                {
                    case VideoDownloadStatus.NOUSE:
                        break;
                    case VideoDownloadStatus.Trans_Wait:
                    case VideoDownloadStatus.Trans_Normal:
                    case VideoDownloadStatus.Trans_Finish:
                    case VideoDownloadStatus.Download_Wait:
                    case VideoDownloadStatus.Download_Normal:
                        temp = "导出中";
                        break;
                    case VideoDownloadStatus.Download_Finish:
                        temp = "导出完成";
                        break;
                    case VideoDownloadStatus.Trans_Failed:
                    case VideoDownloadStatus.Download_Failed:
                        temp = "导出失败";
                        break;
                    default:
                        break;
                }

                System.IO.FileInfo fi = new System.IO.FileInfo(downloadInfo.LocalSaveFilePath);

                m_DTDownloadInfo.Rows.Add(new object[]{
                    downloadInfo.SessionId,
                    tuInfo.TaskUnitName,
                    fi.Name,
                    temp,
                    string.Format("{0}%", ((float)downloadInfo.ComposeProgress/10)),
                    downloadInfo.LocalSaveFilePath,
                    "取消",
                    downloadInfo});
                RecordCount = m_DTDownloadInfo.Rows.Count;
            }
        }

        private void RemoveRow(DownloadInfo downloadInfo)
        {
            DataRow row = m_DTDownloadInfo.Rows.Find(downloadInfo.SessionId);
            if (row != null)
            {
                m_DTDownloadInfo.Rows.Remove(row);
                RecordCount = m_DTDownloadInfo.Rows.Count;
            }
        }

        #endregion

        #region Constructors

        public DownloadInfoListViewModel()
        {
            Init();
            Framework.Container.Instance.EvtAggregator.GetEvent<VideoDownloadStatusUpdateEvent>().Subscribe(OnDownloadInfoStatusUpdate, ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<VideoDownloadProgressUpdateEvent>().Subscribe(OnDownloadInfoProgressUpdate, ThreadOption.WinFormUIThread);
            Framework.Container.Instance.RegisterEventSubscriber(this);
        }

        #endregion

        #region Public helper functions

        public override void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<VideoDownloadStatusUpdateEvent>().Unsubscribe(OnDownloadInfoStatusUpdate);
            Framework.Container.Instance.EvtAggregator.GetEvent<VideoDownloadProgressUpdateEvent>().Unsubscribe(OnDownloadInfoProgressUpdate);
        }

        public void StopDownload()
        {
            if (SelectedItem != null)
            {
                try
                {
                    Framework.Container.Instance.VideoDownloadService.StopDownload(SelectedItem.SessionId);
                    m_DTDownloadInfo.Clear();
                    FillUpTable();
                }
                catch (SDKCallException ex)
                {
                    SDKCallExceptionHandler.Handle(ex, "取消视频导出", true);
                }
            }
        }

        #endregion

        #region Event helper functions

        private void OnDownloadInfoStatusUpdate(DownloadInfo downloadInfo)
        {
            DataRow row = m_DTDownloadInfo.Rows.Find(downloadInfo.SessionId);
            if (row != null)
            {
                string temp = "";
                switch (downloadInfo.Status)
                {
                    case VideoDownloadStatus.NOUSE:
                        break;
                    case VideoDownloadStatus.Trans_Wait:
                    case VideoDownloadStatus.Trans_Normal:
                    case VideoDownloadStatus.Trans_Finish:
                    case VideoDownloadStatus.Download_Wait:
                    case VideoDownloadStatus.Download_Normal:
                        temp = "导出中";
                        break;
                    case VideoDownloadStatus.Download_Finish:
                        temp = "导出完成";
                        break;
                    case VideoDownloadStatus.Trans_Failed:
                    case VideoDownloadStatus.Download_Failed:
                        temp = "导出失败";
                        break;
                    default:
                        break;
                }
                row["Status"] = temp;
                row["Progress"] = string.Format("{0}%", ((float)downloadInfo.ComposeProgress / 10));
            }
            else
            {
                AddRow(downloadInfo);
            }
        }

        private void OnDownloadInfoProgressUpdate(DownloadInfo downloadInfo)
        {
            DataRow row = m_DTDownloadInfo.Rows.Find(downloadInfo.SessionId);
            if (row != null)
            {
                string temp = "";
                switch (downloadInfo.Status)
                {
                    case VideoDownloadStatus.NOUSE:
                        break;
                    case VideoDownloadStatus.Trans_Wait:
                    case VideoDownloadStatus.Trans_Normal:
                    case VideoDownloadStatus.Trans_Finish:
                    case VideoDownloadStatus.Download_Wait:
                    case VideoDownloadStatus.Download_Normal:
                        temp = "导出中";
                        break;
                    case VideoDownloadStatus.Download_Finish:
                        temp = "导出完成";
                        break;
                    case VideoDownloadStatus.Trans_Failed:
                    case VideoDownloadStatus.Download_Failed:
                        temp = "导出失败";
                        break;
                    default:
                        break;
                }
                row["Status"] = temp;
                row["Progress"] = string.Format("{0}%", ((float)downloadInfo.ComposeProgress / 10));
            }
            else
            {
                AddRow(downloadInfo);
            }
        }

        #endregion
    }
}
