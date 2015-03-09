using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace BOCOM.IVX.Protocol.Model
{
    public enum CanvasDrawMode
    {
        ClearAll = 0,
        Line = 1,
        Rect,
        Frame,
    }

    public enum SearchCategory
    {
        Common = 0,
        Face,
        Car,
        Compare
    }

    public enum MovingObjectType
    {
        All = 1,
        Person,
        Car
    }

    public enum BehaviorFilterType
    {
        None = 0,
        CrossLine,
        Region
    }

     /// <summary>
     ///  检索条件以图搜图算法过滤类型
     /// </summary>
    public enum CompareSearchPattern
    {
        /// <summary>
        /// 颜色
        /// <summary>
        Blob = 1,
        /// <summary>
        /// 纹理
        /// <summary>
        Texture,
        /// <summary>
        /// 人脸
        /// <summary>
        Face
    }

    public enum ColorSimilarity
    {
        None = 0,
        High,
        Middle,
        Low
    }

    public enum SearchResultDisplayMode
    {
        Gridview,
        ImageLine,
        ImageGridview,
    }

    public enum SearchType
    {
        Normal,
        Compare,
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

    public struct CarTypeInfo
    {
        public CarType CarType { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public struct CarBrandInfo
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public Image Image { get; set; }
    }
}
