using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.DataModel;
// using BOCOM.IVX.Protocol;
using System.Drawing;
using System.Windows.Forms;

namespace BOCOM.DataModel
{
    public class Constant
    {
        public static readonly Size DEFAULTSIZE_THUMBNAIL = new Size(90, 90);


        public static readonly string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public static readonly string DATETIME_ZERO = "0000-00-00 00:00:00";

        public static readonly Color COLOR_FONT = Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));

        public const string Common_SearchType = "searchType";
        public const string Common_TargetColor = "clrTargetColor";
        public const string Common_ColorSimilarRate = "dColorSimilarRate";
        public const string Common_OBJECT_TYPE = "eObjeType";

        public const string Common_bIsMixSearch = "bIsMixSearch";
        public const string Common_nTimeout = "nTimeout";
        public const string Common_ResourceList = "ResourceList";
        public const string Common_SearchBegin = "SearchBegin";
        public const string Common_SearchEnd = "SearchEnd";

        public const string Crossborder_PTBegin = "ptBegin"; //越界线起点
        public const string Crossborder_ptEnd = "ptEnd";					//越界线终点
        public const string Crossborder_eDirectionType = "eDirectionType";//越界线方向
        public const string Crossborder_ptDirectBegin = "ptDirectBegin";			//越界方向线起点
        public const string Crossborder_ptDirectEnd = "ptDirectEnd";

        public const string Crossframe_pPoint = "pPoint";					//闯入闯出顶点坐标数组
        public const string Crossframe_nPointSize = "nPointSize";					//闯入闯出顶点数量（一般就4个点）
        public const string Crossframe_eDirectionType = "eDirectionType";//闯入闯出方向

        public const string CommonCopare_dSimilarRate = "dSimilarRate";
        public const string CommonCopare_rtMainRectange = "rtMainRectange";	//主框（必须有一个主框）

        public const string CommonCopare_nSubRectangeSize = "nSubRectangeSize";  // 子框数组大小（可以为0，也可为多个）

        public const string CommonCopare_partSubRectange = "partSubRectange";	//子框（可以为0，也可为多个）
        public const string CommonCopare_eMethod = "eMethod";//比对方法
        public const string CommonCopare_nPicWidth = "nPicWidth";			//图像宽
        public const string CommonCopare_nPicHeight = "nPicHeight";			//图像高
        public const string CommonCopare_nPicData = "nPicData";			//图像数据（RGB）
        public const string CommonCopare_nPicDataSize = "nPicDataSize";		//图像数据大小

        public const string Vehicle_nPageNum = "nPageNum";					//页码（从1开始）
        public const string Vehicle_szCardNum = "szCardNum";	//车牌号码("00000000")
        public const string Vehicle_nCarType = "nCarType";					//车辆类型(-1:不限；1:小车；2:中车；3:大车；4:其它车型)
        public const string Vehicle_nCarDetailType = "nCarDetailType";				//车型细分(-1:不限；1:大型货车；2:大型客车；3:中型客车；4:小型客车；5两轮车；6其他)
        public const string Vehicle_nCarLogo = "nCarLogo";					//车标
        public const string Vehicle_nCardStruct = "nCardStruct";				//车牌结构(-1:不限；1:单行；2：双行；3其他)
        public const string Vehicle_clrCarColor = "clrCarColor";			//车身颜色
        public const string Vehicle_clrCardColor = "clrCardColor";			//车牌颜色
        public const string Vehicle_bSortKind = "bSortKind";					//排序类型(0:升序；1:降序)
        public const string Vehicle_szSortName = "szSortName";





    }
}

