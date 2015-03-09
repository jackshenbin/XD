using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Protocol.Model
{

    /// <summary>
    /// 任务单元状态
    /// </summary>
    [Serializable]
    public class TaskUnitAnalyseStatus
    {
        /// <summary>
        /// 分析算法类型，见定义
        /// </summary>
        public UInt32 AnalyseType { get; set; }
        /// <summary>
        /// 分析状态，见vdacomm.h中E_VDA_TASK_UNIT_ANALYZE_STATUS定义
        /// </summary>
        public UInt32 AnalyseStatus { get; set; }
    };

}
