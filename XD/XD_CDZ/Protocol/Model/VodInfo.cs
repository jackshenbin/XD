using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BOCOM.IVX.Protocol.Model
{
    /// <summary>
    /// 按任务单元点播信息
    /// </summary>
    [Serializable]
    public class VodInfo
    {
        /// <summary>
        /// 任务单元ID
        /// </summary>
        public UInt32 VideoTaskUnitID;
        /// <summary>
        /// 是否播放整个文件，如果整个文件，后续的时间段信息无效
        /// </summary>
        public bool IsPlayAllFile;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime;
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime;
        /// <summary>
        /// 播放窗口的句柄
        /// </summary>
        public IntPtr PlayWnd;
    };
}
