using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Protocol.Model
{
    /// <summary>
    /// 视频分析信息
    /// </summary>
    public class VideoAnalyseInfo
    {
        /// <summary>
        /// 分析几种算法
        /// </summary>
        public UInt32 VideoAnalyzeTypeNum { get; set; }
        /// <summary>
        /// 具体分析的类型
        /// </summary>
        public List<UInt32> VideoAnalyzeType { get; set; }
    };
}
