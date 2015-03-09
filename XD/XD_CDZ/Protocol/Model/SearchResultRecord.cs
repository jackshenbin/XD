using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Protocol.Model
{
    public class SearchResultRecord
    {
        public uint CameraID { get; set; }//摄像机号

        public uint TaskUnitID { get; set; }//资源节点句柄

        public uint ID { get; set; }//号

        public uint Distance { get; set; }//距离（即目标特征相似度）

        public DateTime TargetAppearTs { get; set; }//目标出现时间

        public DateTime TargetDisappearTs { get; set; }//目标结束时间

        public SearchResultObjectType ObjectType { get; set; }//目标类型（人、车、不确定）

        public string ThumbPicPath { get; set; }//缩略图数据地址（jpg）

        public string OrgPicPath { get; set; }//原始图数据地址（jpg）

        public DateTime TargetTs { get; set; }//目标时间

        public uint FrameSeq { get; set; }//帧号
        
        public uint TimeStamp { get; set; }

        public System.Drawing.Rectangle ObjectRect { get; set; }//目标区域


        public string CardNum { get; set; }	//车牌号码	
        public Int32 CarType { get; set; }					//车辆类型
        public Int32 CarLogo { get; set; }					//车标
        public Int32 CarStruct { get; set; }					//车牌结构
        public Int32 CarColor1 { get; set; }			//车身颜色
        public Int32 CarColor2 { get; set; }			//车身颜色
        public Int32 CarColor3 { get; set; }			//车身颜色
        public Int32 CardColor { get; set; }			//车牌颜色
    }
}
