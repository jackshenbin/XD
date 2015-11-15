using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace BOCOM.DataModel
{
    //public enum CanvasDrawMode
    //{
    //    ClearAll = 0,
    //    Line = 1,
    //    Rect,
    //    Frame,
    //}

    //public enum SearchCategory
    //{
    //    Common = 0,
    //    Face,
    //    Car,
    //    Compare
    //}

    public enum E_MovingObjectType
    {
        All = 1,
        Car,
        Person
    }

    //public enum BehaviorFilterType
    //{
    //    None = 0,
    //    CrossLine,
    //    Region
    //}

     /// <summary>
     ///  检索条件以图搜图算法过滤类型
     /// </summary>
    public enum CompareSearchPattern
    {

        //E_SEARCH_IMAGE_FILTER_NOUSE = 0,	//无效类型        
        //E_SEARCH_IMAGE_FILTER_BLOB,			//按颜色算法特征过滤
        //E_SEARCH_IMAGE_FILTER_SURF,			//按纹理算法特征过滤
        //E_SEARCH_IMAGE_FILTER_FACE			//按人脸算法特征过滤

        /// <summary>
        /// 颜色
        /// <summary>
        Blob = 0,
        /// <summary>
        /// 纹理
        /// <summary>
        Texture,
        /// <summary>
        /// 人脸
        /// <summary>
        Face
    }

    public enum E_ColorSimilarity
    {
        None = 0,
        High,
        Middle,
        Low
    }

    public enum SearchResultDisplayMode
    {
        /// <summary>
        /// 表格，同一时间仅显示一个文件结果
        /// </summary>
        GridViewOneSearchItem,
        
        /// <summary>
        /// 缩略图， 同一时间仅显示一个文件结果
        /// </summary>
        ThumbNailOneSearchItem,
                
        /// <summary>
        /// 缩略图， 同时显示全部文件的检索结果。一个文件显示一行
        /// </summary>
        ThumbNailAllSearchItem
    }

    public enum SearchType
    {
        Normal,
        Compare,
        Face,
        Vehicle
    }
    
    public enum SearchResourceResultType
    {
        NoUse,
        Normal,
        Compare_Normal,
        Compare_Face,
        Face,
        Vehicle
    }
    
    public enum SortType
    {
        None = 0,
        SimilarityDes,
        TimeAsc,
        TimeDesc
    }

    public enum SearchResultObjectType
    {
        None = 0,
        CAR,
        PEOPLE,
        Unknown,
        FACE,
    }

    public enum CarType
    {
        All,
        Big,
        Middle,
        Small
    }



    

    public enum VehicleType
    {
        // 1:不限；
        None = 0,
        // 1:小车；
        Small = 1,
        // 2:中车；
        Middle = 2,
        // 3:大车；
        Big = 3,
        // 4:其它车型
        Other
    }

    public enum VehicleDetailType
    {
        // 不限
        None = 0,
        // 大型客车
        Big = 1,
        // 大型货车
        Truck = 2,
        // 中性客车
        Middle,
        // 小型客车
        Small,
        // 两轮车
        Bicyle, // 
        // 其它
        Other, 
        // 小型货车
        SmallTruck
    }

    public enum VehiclePlateType
    {
        // 1:不限；
        None = 0,
        // 1:单行；
        SingleRow = 1,
        // 2：双行；
        DoubleRow = 2,
        // 3其他
        Other = 3
    }

    /// <summary>
    /// 下载状态
    /// </summary>
    public enum VideoDownloadStatus
    {
        NOUSE = 0, //未知的导出状态 
        Trans_Wait = 1, //等待转码 
        Trans_Normal, //正在转码 
        Trans_Finish, //完成转码 
        Trans_Failed, //转码失败 
        Download_Wait, //等待导出 
        Download_Normal, //正在导出 
        Download_Finish, //完成导出 
        Download_Failed, //导出失败 
    };

     public struct ColorName
    {
        private Color m_Color;

        private string m_Name;

        public Color Color
        {
            get
            {
                return m_Color;
            }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public ColorName(Color color, string name)
        {
            m_Color = color;
            m_Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    ///  Color 和在不同业务中表示该Color的 整数值
    /// </summary>
     public struct ColorID
     {
         private Color m_Color;

         private int m_ID;

         public Color Color
         {
             get
             {
                 return m_Color;
             }
         }

         public int ID
         {
             get
             {
                 return m_ID;
             }
         }

         public ColorID(int colorID, Color color)
         {
             m_Color = color;
             m_ID = colorID;
         }
     }
}
