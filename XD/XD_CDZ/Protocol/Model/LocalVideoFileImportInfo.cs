using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Protocol.Model
{

    /// <summary>
    /// 本地视频文件导入信息
    /// </summary>
    [Serializable]
    public class LocalVideoFileImportInfo
    {
        /// <summary>
        /// 摄像机编号
        /// </summary>
        public UInt32 CameraID { get; set; }
        /// <summary>
        /// 任务单元名
        /// </summary>
        public string TaskUnitName { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string LocalFilePath { get; set; }
        /// <summary>
        /// 文件大小,单位：字节
        /// </summary>
        public UInt64 FileSize { get; set; }
        /// <summary>
        /// 校准的开始时间
        /// </summary>
        public DateTime AdjustStartTime { get; set; }
        /// <summary>
        /// 视频分析信息
        /// </summary>
        public VideoAnalyseInfo VideoAnalyzeInfo { get; set; }
    };
}
