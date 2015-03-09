using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Protocol.Model
{

    /// <summary>
    /// 任务详细信息
    /// </summary>
    [Serializable]
    public class TaskInfo : ICloneable
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public UInt32 TaskID { get; set; }

        /// <summary>
        /// 任务名
        /// </summary>
        public string TaskName { get; set; }
        /// <summary>
        /// 任务优先级（1 - 10），1为最高级别，建议缺省任务优先级为5
        /// </summary>
        public UInt32 TaskPriorityLevel { get; set; }
        /// <summary>
        /// 任务描述（备注）
        /// </summary>
        public string TaskDescription { get; set; }
        /// <summary>
        /// 创建任务用户名称
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 创建任务时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 完成任务时间
        /// </summary>
        public DateTime CompleteTime { get; set; }
        /// <summary>
        /// 任务状态，见vdacomm.h中E_VDA_TASK_STATUS定义
        /// </summary>
        public UInt32 Status { get; set; }
        /// <summary>
        /// 任务进度的千分比
        /// </summary>
        public UInt32 Progress { get; set; }
        /// <summary>
        /// 任务总体估算剩余时间，秒
        /// </summary>
        public UInt32 TotalLeftTimeS { get; set; }

        public object Clone()
        {
            TaskInfo newTask = new TaskInfo()
            {
                TaskID = TaskID,
                TaskName = TaskName,
                TaskPriorityLevel = TaskPriorityLevel,
                TaskDescription = TaskDescription,
                CreateUserName = CreateUserName,
                CreateTime = CreateTime,
                CompleteTime = CompleteTime,
                Status = Status,
                Progress = Progress,
                TotalLeftTimeS = TotalLeftTimeS,

            };

            return newTask;
        }
    };
}
