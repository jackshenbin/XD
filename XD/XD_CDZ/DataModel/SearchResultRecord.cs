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

    public class SearchResultRecord : IDisposable, ICacheItem
    {
        /// <summary>
        /// 表明是否是车辆检索结果，与 ObjectType 为Car 有区别
        /// 车辆检索结果包含车牌等信息
        /// </summary>
        public bool IsVehicleSearchResult { get; set; }

        public uint CameraID { get; set; }//摄像机号

        public uint TaskUnitID { get; set; }//资源节点句柄

        public uint ID { get; set; }//号

        [SearchResultProperty(DisplayName = "目标类型", VehicleOnly = false, AvailableMode = AvailableMode.All)]
        public SearchResultObjectTypeInfo ObjectTypeInfo
        {
            get
            {
                return Constant.SearchResultObjectTypeInfos[(int)ObjectType];
            }
        }

        [SearchResultProperty(DisplayName = "时间", VehicleOnly = false, AvailableMode = AvailableMode.All)]
        public DateTime TargetTs { get; set; }//目标时间

        // [ReadOnlyAttribute(true)]
         [SearchResultProperty(DisplayName = "出现时间", VehicleOnly = false, AvailableMode = AvailableMode.NonVehicle)]
        public DateTime TargetAppearTs { get; set; }//目标出现时间

        [SearchResultProperty(DisplayName = "消失时间", VehicleOnly = false, AvailableMode = AvailableMode.NonVehicle)]
        public DateTime TargetDisappearTs { get; set; }//目标结束时间

        public SearchResultObjectType ObjectType { get; set; }//目标类型（人、车、不确定）
             
        [SearchResultProperty(DisplayName = "相似度", VehicleOnly = false, AvailableMode = AvailableMode.All)]
        public string Similarate
        {
            get
            {
                string sRet = string.Format("{0}%", ((float)Distance) / 10);

                return sRet;
            }
        }

        public uint Distance { get; set; }//距离（即目标特征相似度）

        public string ThumbPicPath { get; set; }//缩略图数据地址（jpg）

        public string OrgPicPath { get; set; }//原始图数据地址（jpg）

        public Image OriginalPic { get; set; }

        public Image ThumbNailPic { get; set; }

        public Image ObjectPic { get; set; }
        
        public uint TimeStamp { get; set; }

        public System.Drawing.Rectangle ObjectRect { get; set; }//目标区域

        [SearchResultProperty(DisplayName = "车牌号码", VehicleOnly = true, AvailableMode = AvailableMode.Vehicle)]
        public string PlateNO { get; set; }	//车牌号码	

        public VehicleType VehicleType { get; set; }					//车辆类型

        // [SearchResultProperty(DisplayName = "车辆类型", VehicleOnly = true)]
        public VehicleTypeInfo VehicleTypeInfo
        {
            get
            {
                return Constant.VehicleTypeInfos[(int)VehicleType];
            }
        }

        public VehicleDetailType VehicleDetailType { get; set; }

        [SearchResultProperty(DisplayName = "车辆细分", VehicleOnly = true, AvailableMode = AvailableMode.Vehicle)]
        public VehicleDetailTypeInfo VehicleDetailTypeInfo
        {
            get
            {
                int index = (int)VehicleDetailType;
                Debug.Assert(index >= 0 && index < Constant.VehicleDetailTypeInfos.Length);
                if(index >= 0 && index < Constant.VehicleDetailTypeInfos.Length)
                {
                    return Constant.VehicleDetailTypeInfos[index];
                }
                else
                {
                    return Constant.VehicleDetailTypeInfos[Constant.VehicleDetailTypeInfos.Length - 1];
                }
            }
        }
                
        public Int32 VehicleBrand { get; set; }					//车标

        [SearchResultProperty(DisplayName = "车标", VehicleOnly = true, AvailableMode = AvailableMode.Vehicle)]
        public VehicleBrandInfo VehicleBrandInfo
        {
            get;
            set;
        }

        public VehiclePlateType VehiclePlateType { get; set; }					//车牌结构

        [SearchResultProperty(DisplayName = "车牌类型", VehicleOnly = true, AvailableMode = AvailableMode.Vehicle)]
        public VehiclePlateTypeInfo VehiclePlateTypeInfo
        {
            get
            {
                return Constant.VehiclePlateTypeInfos[(int)VehiclePlateType];
            }
        }

        public Int32 VehicleBodyColor1 { get; set; }			//车身颜色

        [SearchResultProperty(DisplayName = "车身颜色", VehicleOnly = true, AvailableMode = AvailableMode.Vehicle)]
        public string VehicleBodyColor { get; set; }
        
        public ColorName VehicleBodyColorName1 { get; set; }			//车身颜色

        public ColorName VehicleBodyColorName2 { get; set; }			//车身颜色

        public ColorName VehicleBodyColorName3 { get; set; }			//车身颜色

        public Int32 PlateColor { get; set; }			//车身颜色

        [SearchResultProperty(DisplayName = "车牌颜色", VehicleOnly = true, AvailableMode = AvailableMode.Vehicle)]
        public ColorName PlateColorName { get; set; }			//车身颜色
        
        public Int32 VehicleBodyColor2 { get; set; }			//车身颜色
        
        public Int32 VehicleBodyColor3 { get; set; }			//车牌颜色

        public void Dispose()
        {
            if (OriginalPic != null)
            {
                OriginalPic.Dispose();
                OriginalPic = null;
            }

            if (ThumbNailPic != null)
            {
                ThumbNailPic.Dispose();
                ThumbNailPic = null;
            }

            if (ObjectPic != null)
            {
                ObjectPic.Dispose();
                ObjectPic = null;
            }
        }

        public void Clear()
        {
            if (OriginalPic != null)
            {
                OriginalPic.Dispose();
                OriginalPic = null;
            }
        }
    }
}
