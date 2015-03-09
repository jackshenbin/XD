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
    /// ocx用的检索结果返回结构体 车牌
    /// </summary>
    public class CarSearchResultRecord 
    {
        public uint CameraID { get; set; }//摄像机号

        public uint TaskUnitID { get; set; }//资源节点句柄

        public uint ID { get; set; }//号

        public string ThumbPicPath { get; set; }//缩略图数据地址（jpg）

        public string OrgPicPath { get; set; }//原始图数据地址（jpg）

        public DateTime TargetTs { get; set; }//目标时间

        public System.Drawing.Rectangle ObjectRect { get; set; }//目标区域

        public string PlateNO { get; set; }	//车牌号码	

        public uint VehicleType { get; set; }					//车辆类型

        public uint VehicleDetailType { get; set; }

        public string VehicleBrand { get; set; }					//车标

        public uint VehiclePlateType { get; set; }					//车牌结构

        public string VehicleBodyColor1 { get; set; }			//车身颜色

        public string VehicleBodyColor2 { get; set; }			//车身颜色

        public string VehicleBodyColor3 { get; set; }			//车身颜色

        public string  PlateColor { get; set; }			//车牌颜色


    }
}
