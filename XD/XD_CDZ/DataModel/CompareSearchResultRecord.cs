using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DataModel;
using System.ComponentModel;
using System.Diagnostics;

namespace BOCOM.DataModel
{
    /// <summary>
    /// ocx用的检索结果返回结构体 比对
    /// </summary>
    public class CompareSearchResultRecord 
    {
        public uint CameraID { get; set; }//摄像机号

        public uint TaskUnitID { get; set; }//资源节点句柄

        public uint ID { get; set; }//号

        public uint Distance { get; set; }//距离（即目标特征相似度）

        public DateTime TargetAppearTs { get; set; }//目标出现时间

        public DateTime TargetDisappearTs { get; set; }//目标结束时间

        public uint ObjectType { get; set; }//目标类型（人、车、不确定）

        public string ThumbPicPath { get; set; }//缩略图数据地址（jpg）

        public string OrgPicPath { get; set; }//原始图数据地址（jpg）

        public DateTime TargetTs { get; set; }//目标时间

        public System.Drawing.Rectangle ObjectRect { get; set; }//目标区域


    }
}
