using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.DataModel
{
    /// <summary>
    /// 按任务单元下载信息
    /// </summary>
    [Serializable]
    public class DownloadInfo
    {
        /// <summary>
        /// 任务单元ID
        /// </summary>
        public UInt32 VideoTaskUnitID { get; set; }

        public int SessionId { get; set; }

        public uint ConversionProgress{get;set;}
        
        public uint ExportProgress {get;set;}

        public uint ComposeProgress { get; set; }

        //public uint TotalProgress { get; set;}

        public VideoDownloadStatus Status
        {
            get;
            set;
        }

        public uint ErrorCode { get; set; }

        /// <summary>
        /// 是否下载整个文件，如果整个文件，后续的时间段信息无效
        /// </summary>
        public bool IsDownloadAllFile { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 本地保存文件路径
        /// </summary>
        public string LocalSaveFilePath { get; set; }
    };
}
