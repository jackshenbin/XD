using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BOCOM.IVX.DataModel
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
        public UInt32 VideoTaskUnitID;
        /// <summary>
        /// 是否下载整个文件，如果整个文件，后续的时间段信息无效
        /// </summary>
        public bool IsDownloadAllFile;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime;
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime;
        /// <summary>
        /// 本地保存文件路径
        /// </summary>
        public string LocalSaveFilePath;
    };
}
