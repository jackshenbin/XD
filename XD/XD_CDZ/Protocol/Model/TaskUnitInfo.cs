using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BOCOM.IVX.Protocol.Model
{
    /// <summary>
    /// 任务单元详细信息
    /// </summary>
    [Serializable]
    public class TaskUnitInfo : ICloneable
    {
        /// <summary>
        /// 所属任务ID
        /// </summary>
        public UInt32 TaskID { get; set; }
        /// <summary>
        /// 任务单元ID
        /// </summary>
        public UInt32 TaskUnitID { get; set; }
        public UInt32 CameraId { get; set; }
        /// <summary>
        /// 任务单元名
        /// </summary>
        public string TaskUnitName { get; set; }
        /// <summary>
        /// 任务单元大小,单位：字节
        /// </summary>
        public UInt64 TaskUnitSize { get; set; }
        /// <summary>
        /// 任务单元类型，见vdacomm.h中E_VDA_TASK_UNIT_TYPE定义
        /// </summary>
        public UInt32 TaskUnitType { get; set; }
        /// <summary>
        /// 文件路径类型：0：本地文件，1：远程文件
        /// </summary>
        public UInt32 FilePathType { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 任务单元开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 任务单元结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 任务单元导入状态，见vdacomm.h中E_VDA_TASK_UNIT_IMPORT_STATUS定义
        /// </summary>
        public UInt32 ImportStatus { get; set; }
        /// <summary>
        /// 分析了几种算法
        /// </summary>
        public UInt32 VideoAnalyzeTypeNum { get; set; }
        /// <summary>
        /// 各类算法分析状态
        /// </summary>
        [Obsolete]
        public Dictionary<E_VDA_ANALYZE_TYPE, E_VDA_TASK_UNIT_STATUS> AnalyzeStatus { get; set; }
        /// <summary>
        /// 任务单元进度的千分比
        /// </summary>
        public UInt32 Progress { get; set; }
        /// <summary>
        /// 任务单元估算剩余时间，秒
        /// </summary>
        public UInt32 LeftTimeS { get; set; }

        public object Clone()
        {
            TaskUnitInfo newTaskUnit = new TaskUnitInfo()
            {
                TaskID = TaskID,
                TaskUnitID = TaskUnitID,
                TaskUnitName = TaskUnitName,
                TaskUnitSize = TaskUnitSize,
                TaskUnitType = TaskUnitType,
                FilePathType = FilePathType,
                FilePath = FilePath,
                StartTime = StartTime,
                EndTime = EndTime,
                ImportStatus = ImportStatus,
                VideoAnalyzeTypeNum = VideoAnalyzeTypeNum,
                AnalyzeStatus = new Dictionary<E_VDA_ANALYZE_TYPE, E_VDA_TASK_UNIT_STATUS>(),
                Progress = Progress,
                LeftTimeS = LeftTimeS,
            };
            foreach (E_VDA_ANALYZE_TYPE t in AnalyzeStatus.Keys)
            {
                newTaskUnit.AnalyzeStatus.Add(t, AnalyzeStatus[t]);
            }

            return newTaskUnit;
        }
    };
}
