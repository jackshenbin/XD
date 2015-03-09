using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using BOCOM.IVX.Protocol.Model;
using BOCOM.DataModel;

namespace BOCOM.IVX.Protocol
{
    #region 基本结构体
    /// <summary>
        ///  运动物目标
        /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDA_OBJECT
    {
        /// <summary>
        /// 目标标识
        /// </summary>
        public Int32 hId;
        /// <summary>
        /// 目标时间
        /// </summary>
        public Int64 nObjTime;
        /// <summary>
        /// 目标出现时间
        /// </summary>
        public Int64 nObjAppearTime;
        /// <summary>
        /// 目标结束时间
        /// </summary>
        public Int64 nObjDisappearTime;
    };

    /// <summary>
        /// 智能分析检索对象_矩阵
        /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_SEARCH_RECT
    {

        /// <summary>
        /// 矩阵起始坐标X
        /// </summary>
        public UInt32 dwX;

        /// <summary>
        /// 矩阵起始坐标Y
        /// </summary>
        public UInt32 dwY;

        /// <summary>
        /// 矩阵宽度
        /// </summary>
        public UInt32 dwWidth;

        /// <summary>
        /// 矩阵高度
        /// </summary>
        public UInt32 dwHeight;
    }

    /// <summary>
        /// 智能分析检索结果_比对对象基本信息
        /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_SEARCH_OBJ_BASE
    {

        /// <summary>
        /// 检索对象ID
        /// </summary>
        public TVDASDK_SEARCH_OBJ_ID tObjID;

        /// <summary>
        /// 检索对象类型，见 E_VDA_SEARCH_OBJ_TYPE
        /// </summary>
        public UInt32 dwObjType;

        /// <summary>
        /// 检索对象出现的时间点
        /// </summary>
        public UInt32 dwBeginTime;

        /// <summary>
        /// 检索对象结束的时间点
        /// </summary>
        public UInt32 dwEndTime;
    }

    /// <summary>
    /// 智能分析检索结果_比对对象ID
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_SEARCH_OBJ_ID
    {

        /// <summary>
        ///  摄像机ID
        /// </summary>

        internal UInt32 dwCameraID;

        /// <summary>
        ///  任务单元ID
        /// </summary>
        internal UInt32 dwTaskUnitID;

        /// <summary>
        ///  目标ID
        /// </summary>
        internal UInt32 dwMoveObjID;
    }

    /// <summary>
    /// 智能分析检索获取图片
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_SEARCH_GET_IMAGE_FILTER
    {
        /// <summary>
        /// 对象ID
        /// </summary>
        internal TVDASDK_SEARCH_OBJ_ID tObjID;

        /// <summary>
        /// 图片URL地址
        /// </summary>
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = Common.VDA_MAX_FILEPATH_LEN)]
        internal string szURLPath;
    }

    /// <summary>
    /// 智能分析检索对象_闯入闯出区域
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_SEARCH_BREAK_REGION
    {
        /// <summary>
        ///  闯入闯出区域类型，E_VDA_SEARCH_BREAK_REGION_TYPE
        /// </summary>
        public UInt32 dwRegionType;

        /// <summary>
        ///  区域边界点列表, TVDASDK_SEARCH_POINT
        /// </summary>
        //System.IntPtr ptRegionPointList;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = Common.VDA_ONE_BREAK_REGION_POINT_MAXNUM)]
        public 	TVDASDK_SEARCH_POINT[] atRegionPointList;			//单个区域边界点列表

        /// <summary>
        /// 区域边界点数量
        /// </summary>
        public UInt32 dwPointNum;
    }

    /// <summary>
    /// 智能分析检索结果_目标快照
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_SEARCH_SNAPSHOT
    {

        /// <summary>
        ///  当前绝对时间
        /// </summary>
        public UInt32 qwCurTime;

        /// <summary>
        ///  帧号
        /// </summary>
        public UInt32 dwFrameSeq;

        /// <summary>
        ///  目标发生的时间戳
        /// </summary>
        public UInt32 dwTimeStamp;

        /// <summary>
        ///  目标矩形
        /// </summary>
        public TVDASDK_SEARCH_RECT tObjRect;
    }

    /// <summary>
    /// 智能分析检索对象_点
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_SEARCH_POINT
    {

        /// <summary>
        ///  X坐标
        /// </summary>
        public UInt32 dwX;

        /// <summary>
        ///  Y坐标
        /// </summary>
        public UInt32 dwY;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_IA_SEARCH_PASS_LINE
    {

        /// <summary>
        ///  越界线类型，见E_VDA_SEARCH_PASS_LINE_TYPE
        /// </summary>
        public UInt32 dwPassLineType;

        /// <summary>
        ///  越界线
        /// </summary>
        public TVDASDK_SEARCH_LINE tPassLine;

        /// <summary>
        ///  方向线
        /// </summary>
        public TVDASDK_SEARCH_LINE tDirectLine;
    }

    /// <summary>
    /// 智能分析检索对象_线
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_SEARCH_LINE
    {

        /// <summary>
        ///  起始点
        /// </summary>
        public TVDASDK_SEARCH_POINT tStartPt;

        /// <summary>
        ///  结束点
        /// </summary>
        public TVDASDK_SEARCH_POINT tEndPt;
    }

    /// <summary>
    /// 智能分析检索结果_图像数据
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_SEARCH_IMAGE_INFO
    {
        /// <summary>
        /// 图片数据
        /// </summary>
        public IntPtr ptImageData;
        /// <summary>
        /// 图片大小
        /// </summary>
        public UInt32 dwImageSize;
    }

    ///// <summary>
    ///// 
    ///// </summary>
    //[StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    //internal struct TVDASDK_SEARCH_TARGET
    //{

    //    /// <summary>
    //    ///  DWORD->unsigned int
    //    /// </summary>
    //    public UInt32 dwCameraID;

    //    /// <summary>
    //    ///  DWORD->unsigned int
    //    /// </summary>
    //    public UInt32 dwTaskUnitID;
    //}

    /// <summary>
    /// 智能分析检索任务单元集合
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_SEARCH_TASK_UNIT_LIST
    {

        /// <summary>
        ///  任务单元ID 列表
        /// </summary>
        public System.IntPtr pdwTaskUnitID;

        /// <summary>
        ///  任务单元数目
        /// </summary>
        public UInt32 dwTaskUnitNum;
    }

    /// <summary>
    /// 运动物检索过滤条件
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_SEARCH_MOBILEOBJ_FILTER
    {
        /// <summary>
        /// 搜索开始时间点（从1970开始的秒数）
        /// </summary>
        public UInt32 dwStartTimeS;

        /// <summary>
        ///搜索结束时间点（从1970开始的秒数）
        /// </summary>
        public UInt32 dwEndTimeS;

        /// <summary>
        /// 检索对象类型，见E_VDA_SEARCH_MOVE_OBJ_FILTER_TYPE
        /// </summary>
        public UInt32 dwSearchObjType;

        /// <summary>
        /// 是否纯色比对
        /// </summary>
        [MarshalAsAttribute(UnmanagedType.Bool)]
        public bool bColorSearch;

        /// <summary>
        /// 纯色比对时的颜色值ARGB
        /// </summary>
        public UInt32 dwSearchObjRGB;

        /// <summary>
        /// 颜色相似度千分比(0 ~ 1000)
        /// </summary>
        public UInt32 dwColorSimilar;

        /// <summary>
        /// 启动区域类型，见E_VDA_SEARCH_MOVE_OBJ_RANGE_FILTER_TYPE
        /// </summary>
        public UInt32 dwRangeFilterType;

        /// <summary>
        ///  越界线列表
        /// </summary>
        public System.IntPtr ptSearchPassLineList;

        /// <summary>
        ///  绊线数量
        /// </summary>
        public UInt32 dwPassLineNum;

        /// <summary>
        ///  闯入闯出区域
        /// </summary>
        public TVDASDK_SEARCH_BREAK_REGION tSearchBreakRegion;
    }

    /// <summary>
    /// 人脸检索过滤条件
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_SEARCH_FACEOBJ_FILTER
    {

        /// <summary>
        ///  搜索开始时间点（从1970开始的秒数）
        /// </summary>
        public UInt32 dwStartTimeS;

        /// <summary>
        ///  搜索结束时间点（从1970开始的秒数）
        /// </summary>
        public UInt32 dwEndTimeS;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_SEARCH_IMAGE_FILTER
    {
        /// <summary>
        /// 搜索开始时间点（从1970开始的秒数）
        /// </summary>
        public UInt32 dwStartTimeS;
        /// <summary>
        /// 搜索结束时间点（从1970开始的秒数）	
        /// </summary>
        public UInt32 dwEndTimeS;
        /// <summary>
        /// 相似度千分比	
        /// </summary>
        public UInt32 dwColorSimilar;
        /// <summary>
        /// 算法过滤类型，见E_VDA_SEARCH_IMAGE_FILTER_TYPE
        /// </summary>
        public UInt32 dwAlgorithmFilterType;
        /// <summary>
        /// 对象过滤类型，见E_VDA_SEARCH_MOVE_OBJ_FILTER_TYPE，对于人脸当前参数无效
        /// </summary>
        public UInt32 dwObjFilterType;
        /// <summary>
        /// 图片信息
        /// </summary>
        public TVDASDK_SEARCH_IMAGE_INFO tImageInfo;
        /// <summary>
        /// 图片在大图中的位置
        /// </summary>
        public TVDASDK_SEARCH_RECT tObjRect;
    }

    /// <summary>
    /// 智能分析检索分页
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_SEARCH_RESULT_REQUIREMENT
    {
        /// <summary>
        ///  排序类型，见 E_VDA_SEARCH_SORT_TYPE
        /// </summary>
        public UInt32 dwResultSortType;

        /// <summary>
        ///  单页记录数量
        /// </summary>
        public UInt32 dwOnePageRecordNum;
    }

    /// <summary>
    /// 检索结果页面信息
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_SEARCH_RESULT_PAGE_INFO
    {
        /// <summary>
        /// 检索结果: 0 表示正常， 其它表示失败
        /// </summary>
        public UInt32 dwResult;

        /// <summary>
        /// 检索结果总数量
        /// </summary>
        public UInt32 dwResultTotalNum;

        /// <summary>
        /// 当前页序号
        /// </summary>
        public UInt32 dwCurPageIdx;

        /// <summary>
        /// 当前页的结果数量
        /// </summary>
        public UInt32 dwCurSearchResultNum;

        /// <summary>
        /// 页面总数目
        /// </summary>
        public UInt32 dwPageCount;
    }

    /// <summary>
    /// 检索详细信息结果
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_SEARCH_RESULT_OBJ_INFO
    {
        /// <summary>
        ///  比对对象基本信息
        /// </summary>
        public TVDASDK_SEARCH_OBJ_BASE tObjBase;

        /// <summary>
        ///  相似度千分比
        /// </summary>
        public UInt32 dwSimilar;

        /// <summary>
        ///  图片快照细信息
        /// </summary>
        public TVDASDK_SEARCH_SNAPSHOT tImageSnapshot;

        /// <summary>
        ///  原始图URL
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDA_MAX_FILEPATH_LEN)]
        public string szOriginalImageURL;

        /// <summary>
        ///  缩略图URL
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDA_MAX_FILEPATH_LEN)]
        public string szThumbImageURL;
    }

    /// <summary>
    /// 智能分析检索对象_大小
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_SEARCH_SIZE
    {
        /// <summary>
        ///  对象宽度
        /// </summary>
        public UInt32 dwWidth;

        /// <summary>
        ///  对象高度
        /// </summary>
        public UInt32 dwHeight;
    }

        
    //车牌检索过滤条件
    public struct TVDASDK_SEARCH_VEHICLE_FILTER
    {
	    public uint dwStartTimeS;								//搜索开始时间点（从1970开始的秒数）
	    public uint dwEndTimeS;								//搜索结束时间点（从1970开始的秒数）	
	    public uint dwVehicleType;							//车辆类型见E_VDA_SEARCH_VEHICLE_TYPE
	    public uint dwVehicleDetailType;						//车型细分见E_VDA_SEARCH_VEHICLE_DETAIL_TYPE
	    public uint dwVehicleColor;							//车身颜色见 E_VDA_SEARCH_VEHICLE_COLOR_TYPE
	    public uint dwVehicleLogo;							//车标
         [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDASDK_MAX_NAME_LEN)]
	    public string szVehiclePlateName;	//车牌号码
	    public uint dwVehiclePlateStruct;						//车牌结构见 E_VDA_SEARCH_VEHICLE_PLATE_STRUCT_TYPE
        public uint dwVehiclePlateColor;						//车牌颜色见 E_VDA_SEARCH_VEHICLE_PLATE_COLOR_TYPE
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_SEARCH_VEHICLE_RESULT_OBJ_INFO
    {
        internal TVDASDK_SEARCH_OBJ_ID tObjID;						//对象ID		
        internal TVDASDK_SEARCH_SNAPSHOT tImageSnapshot;				//图片快照细信息
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDA_MAX_NAME_LEN)]
        internal string szVehiclePlate;				//车牌号

        internal uint dwVehicleType;								//车型类型见E_VDA_SEARCH_VEHICLE_TYPE
        internal uint dwVehicleDetailType;							//车辆细分见E_VDA_SEARCH_VEHICLE_DETAIL_TYPE
        internal uint dwVehicleLogo;								//车身车标
        internal uint dwVehicleColor1;								//车身颜色1见E_VDA_SEARCH_VEHICLE_COLOR_TYPE
        internal uint dwVehicleColorSimilar1;						//车身颜色1相似度
        internal uint dwVehicleColor2;								//车身颜色2见E_VDA_SEARCH_VEHICLE_COLOR_TYPE
        internal uint dwVehicleColorSimilar2;						//车身颜色2相似度
        internal uint dwVehicleColor3;								//车身颜色3见E_VDA_SEARCH_VEHICLE_COLOR_TYPE
        internal uint dwVehicleColorSimilar3;						//车身颜色3相似度
        internal uint dwVehiclePlateColor;							//车牌颜色见E_VDA_SEARCH_VEHICLE_PLATE_COLOR_TYPE
        internal uint dwVehiclePlateStruct;							//车牌结构见E_VDA_SEARCH_VEHICLE_PLATE_STRUCT_TYPE

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDA_MAX_FILEPATH_LEN)]
        internal string szOriginalImageURL;		//原始图URL
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.VDA_MAX_FILEPATH_LEN)]
        internal string szThumbImageURL;		//缩略图URL
    }

    #endregion
          

    #region 回调定义

    /*===========================================================
    功  能：注册获取文件图片回调通知
    参  数：dwQueryHandle - 查询句柄
		    tObjID - 对象ID
		    tImageInfo - 图片信息
		    dwUserData - 用户数据
    返回值：成功返回TRUE，失败返回FALSE
    ===========================================================*/
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    internal delegate void TfuncGetImagetNtfCB(UInt32 dwSessionID, TVDASDK_SEARCH_GET_IMAGE_FILTER tSearchConditionFilter, TVDASDK_SEARCH_IMAGE_INFO tImageInfo, UInt32 dwUserData);

    /*===========================================================
    功  能：注册查询结果任务单元页回调通知
    参  数：dwQueryHandle - 查询句柄
            tResultPageInfo - 当前页信息
            ptSearchResultObjInfo - 查询结果详细信息
            dwUserData - 用户数据
    返回值：成功返回TRUE，失败返回FALSE
    ===========================================================*/
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    internal delegate void TfuncSearchResultTaskUnitPageNtfCB(UInt32 dwQueryHandle, UInt32 dwTaskUnitID, TVDASDK_SEARCH_RESULT_PAGE_INFO tResultPageInfo, IntPtr ptSearchResultObjInfoStart, UInt32 dwUserData);

    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    internal delegate void TfuncSearchVehicleResultTaskUnitPageNtfCB(UInt32 dwQueryHandle, UInt32 dwTaskUnitID, TVDASDK_SEARCH_RESULT_PAGE_INFO tResultPageInfo, IntPtr ptSearchResultObjInfoStart, UInt32 dwUserData);


    /*===========================================================
    功  能：注册查询结果摄像机页面回调通知
    参  数：dwQueryHandle - 查询句柄
            tResultPageInfo - 当前页信息
            ptSearchResultObjInfo - 查询结果详细信息
            dwUserData - 用户数据
    返回值：成功返回TRUE，失败返回FALSE
    ===========================================================*/
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    internal delegate void TfuncSearchResultCameraPageNtfCB(UInt32 dwQueryHandle, UInt32 dwCameraID, TVDASDK_SEARCH_RESULT_PAGE_INFO tResultPageInfo, IntPtr ptSearchResultObjInfoStart, UInt32 dwUserData);

    #endregion

    internal partial class IVXSDKProtocol
    {
        #region 检索相关

        /*===========================================================
        功  能：注册查询结果任务单元页回调通知
        参  数：dwQueryHandle - 查询句柄
		        tResultPageInfo - 当前页信息
		        ptSearchResultObjInfo - 查询结果详细信息
		        dwUserData - 用户数据
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern bool VdaSdk_SearchResultTaskUnitPageNtfCBReg(TfuncSearchResultTaskUnitPageNtfCB pfuncSearchResultTaskUnitPageNtf);

        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern bool VdaSdk_SearchVehicleResultTaskUnitPageNtfCBReg(TfuncSearchVehicleResultTaskUnitPageNtfCB pfuncSearchResultTaskUnitPageNtf);

        /*===========================================================
        功  能：注册查询结果摄像机页面回调通知
        参  数：dwQueryHandle - 查询句柄
                tResultPageInfo - 当前页信息
                ptSearchResultObjInfo - 查询结果详细信息
                dwUserData - 用户数据
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern bool VdaSdk_SearchResultCameraPageNtfCBReg(TfuncSearchResultCameraPageNtfCB pfuncSearchResultCameraPageNtf);

        /*===========================================================
        功  能：注册获取文件图片回调通知
        参  数：dwQueryHandle - 查询句柄
                tSearchConditionFilter - 获取图片查询条件
                tImageInfo - 图片信息
                dwUserData - 用户数据
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern bool VdaSdk_SearchGetImagetNtfCBReg(TfuncGetImagetNtfCB pfuncSearchGetImageNtf);

        /*===========================================================
        功  能：通过任务单元检索运动物特征
        参  数：tSearchTaskUnitList - 任务单元列表
                tSearchConditionFilter - 运动物检索过滤条件
                tSearchResultRequirement - 页面条件
                dwUserData - 用户数据
                pdwQueryHandle - 查询句柄
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern bool VdaSdk_StartMoveObjSearchByTaskUnitID(TVDASDK_SEARCH_TASK_UNIT_LIST tSearchTargetList, TVDASDK_SEARCH_MOBILEOBJ_FILTER tSearchConditionFilter, TVDASDK_SEARCH_RESULT_REQUIREMENT tSearchResultRequirement, UInt32 dwUserData, ref UInt32 pdwSessionID);

        /*===========================================================
        功  能：通过任务单元检索人脸
        参  数：tSearchTaskUnitList - 任务单元列表
                tSearchConditionFilter - 人脸检索过滤条件
                tSearchResultRequirement - 页面条件
                dwUserData - 用户数据
                pdwQueryHandle - 查询句柄
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern bool VdaSdk_StartFaceSearchByTaskUnitID(TVDASDK_SEARCH_TASK_UNIT_LIST tSearchTargetList, TVDASDK_SEARCH_FACEOBJ_FILTER tSearchConditionFilter, TVDASDK_SEARCH_RESULT_REQUIREMENT tSearchResultRequirement, UInt32 dwUserData, ref UInt32 pdwSessionID);

        /*===========================================================
        功  能：通过任务单元以图搜图
        参  数：tSearchTaskUnitList - 任务单元列表
                tSearchConditionFilter - 以图搜图检索过滤条件
                tSearchResultRequirement - 页面条件
                dwUserData - 用户数据
                pdwQueryHandle - 查询句柄
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern bool VdaSdk_StartImageSearchByTaskUnitID(TVDASDK_SEARCH_TASK_UNIT_LIST tSearchTargetList, TVDASDK_SEARCH_IMAGE_FILTER tSearchConditionFilter, TVDASDK_SEARCH_RESULT_REQUIREMENT tSearchResultRequirement, UInt32 dwUserData, ref UInt32 pdwSessionID);

        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern bool VdaSdk_StartVehicleSearchByTaskUnitID(TVDASDK_SEARCH_TASK_UNIT_LIST tSearchTargetList, TVDASDK_SEARCH_VEHICLE_FILTER tSearchConditionFilter, TVDASDK_SEARCH_RESULT_REQUIREMENT tSearchResultRequirement, UInt32 dwUserData, ref UInt32 pdwSessionID);

        /*===========================================================
        功  能：获取文件图片
        参  数：dwQueryHandle - 查询句柄
                tSearchConditionFilter - 查询条件
                dwUserData - 用户数据
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern bool VdaSdk_StartGetImage(UInt32 dwQueryHandle, TVDASDK_SEARCH_GET_IMAGE_FILTER tSearchConditionFilter, UInt32 dwUserData);
        // public static extern bool VdaSdk_StartGetImage(UInt32 dwQueryHandle, TVDASDK_SEARCH_GET_IMAGE_FILTER tSearchConditionFilter, UInt32 dwUserData);


        /*===========================================================
        功  能：按照任务单元ID查询指定分页请求
        参  数：dwQueryHandle - 查询句柄
                dwTaskUnitID - 任务单元ID
                dwPageIndex - 页面号
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern bool VdaSdk_StartSwitchPageByTaskUnitID(UInt32 dwSessionID, UInt32 dwTaskUnitID, UInt32 dwPageIndex);

        /*===========================================================
        功  能：关闭查询请求
        参  数：dwQueryHandle - 查询句柄		
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern bool VdaSdk_CloseSearchQueryHandle(UInt32 dwSessionID);

        #endregion
    }

    public partial class IVXProtocol
    {
        #region 检索相关

        /// <summary>
        /// 运动物检索请求
        /// </summary>
        /// <param name="searchPara">检索条件</param>
        /// <returns>检索唯一编号</returns>
        public UInt32 StartMoveObjectSearchByTaskUnit(SearchPara searchPara)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol StartMoveObjectSearchByTaskUnit");
            UInt32 searchID = 0;

            TVDASDK_SEARCH_TASK_UNIT_LIST targets = ModelParser.GetTargetList(searchPara);
            TVDASDK_SEARCH_MOBILEOBJ_FILTER filter = ModelParser.GetMoveObjectFilter(searchPara);

            TVDASDK_SEARCH_RESULT_REQUIREMENT pageAndSortInfo = ModelParser.GetPageAndSortSettings(searchPara);
            UInt32 userData = 0;
            bool result = IVXSDKProtocol.VdaSdk_StartMoveObjSearchByTaskUnitID(targets, filter, pageAndSortInfo, userData, ref searchID);

            if (!result)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol StartMoveObjectSearchByTaskUnit ret :" + searchID);

            return searchID;
        }

        /// <summary>
        /// 以图搜图检索请求
        /// </summary>
        /// <param name="searchPara">检索条件</param>
        /// <returns>检索唯一编号</returns>
        public UInt32 StartCompareSearchByTaskUnit(SearchPara searchPara)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol StartCompareSearchByTaskUnit");
            UInt32 searchID = 0;

            TVDASDK_SEARCH_TASK_UNIT_LIST targets = ModelParser.GetTargetList(searchPara);
            TVDASDK_SEARCH_IMAGE_FILTER filter = ModelParser.GetCompareSearchFilter(searchPara);

            TVDASDK_SEARCH_RESULT_REQUIREMENT pageAndSortInfo = ModelParser.GetPageAndSortSettings(searchPara);
            UInt32 userData = 0;
            bool result = IVXSDKProtocol.VdaSdk_StartImageSearchByTaskUnitID(targets, filter, pageAndSortInfo, userData, ref searchID);

            if (!result)
            {
                CheckError();
            }

            if (filter.tImageInfo.dwImageSize > 0)
            {
                Marshal.FreeHGlobal(filter.tImageInfo.ptImageData);
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol StartCompareSearchByTaskUnit ret :" + searchID);

            return searchID;
        }

        /// <summary>
        /// 人脸检索请求
        /// </summary>
        /// <param name="searchPara">检索条件</param>
        /// <returns>检索唯一编号</returns>
        public UInt32 StartFaceSearchByTaskUnit(SearchPara searchPara)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol StartFaceSearchByTaskUnit");
            UInt32 searchID = 0;

            TVDASDK_SEARCH_TASK_UNIT_LIST targets = ModelParser.GetTargetList(searchPara);
            TVDASDK_SEARCH_FACEOBJ_FILTER filter = ModelParser.GetFaceSearchFilter(searchPara);

            TVDASDK_SEARCH_RESULT_REQUIREMENT pageAndSortInfo = ModelParser.GetPageAndSortSettings(searchPara);
            UInt32 userData = 0;

            bool result = IVXSDKProtocol.VdaSdk_StartFaceSearchByTaskUnitID(targets, filter, pageAndSortInfo, userData, ref searchID);

            if (!result)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol StartFaceSearchByTaskUnit ret :" + searchID);

            return searchID;
        }

        /// <summary>
        /// 人脸检索请求
        /// </summary>
        /// <param name="searchPara">检索条件</param>
        /// <returns>检索唯一编号</returns>
        public UInt32 StartVehicleSearchByTaskUnit(SearchPara searchPara)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol StartVehicleSearchByTaskUnit");
            UInt32 searchID = 0;

            TVDASDK_SEARCH_TASK_UNIT_LIST targets = ModelParser.GetTargetList(searchPara);
            TVDASDK_SEARCH_VEHICLE_FILTER filter = ModelParser.GetVehicleSearchFilter(searchPara);

            TVDASDK_SEARCH_RESULT_REQUIREMENT pageAndSortInfo = ModelParser.GetPageAndSortSettings(searchPara);
            UInt32 userData = 0;
            bool result = IVXSDKProtocol.VdaSdk_StartVehicleSearchByTaskUnitID(targets, filter, pageAndSortInfo, userData, ref searchID);

            if (!result)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol StartVehicleSearchByTaskUnit ret :" + searchID);

            return searchID;
        }

        /// <summary>
        /// 获取目标物图片请求
        /// </summary>
        /// <param name="sessionID">检索唯一编号</param>
        /// <param name="camID">目标所属相机</param>
        /// <param name="taskUnitID">目标所属任务单元</param>
        /// <param name="id">目标编号</param>
        /// <param name="imgURL">请求的图片url</param>
        /// <returns></returns>
        public bool StartGetImage(UInt32 sessionID, UInt32 camID, UInt32 taskUnitID, UInt32 id, string imgURL)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_StartGetImage sessionID:{0},camID:{1},taskUnitID:{2},id:{3},imgURL:{4}", sessionID, camID, taskUnitID, id, imgURL));

            bool ret = false;
            TVDASDK_SEARCH_OBJ_ID info = new TVDASDK_SEARCH_OBJ_ID();
            info.dwCameraID = camID;
            info.dwMoveObjID = id;
            info.dwTaskUnitID = taskUnitID;

            TVDASDK_SEARCH_GET_IMAGE_FILTER imageFilter = new TVDASDK_SEARCH_GET_IMAGE_FILTER()
            {
                 tObjID = info, szURLPath = imgURL
            };

            UInt32 userData = 0;
            ret = IVXSDKProtocol.VdaSdk_StartGetImage(sessionID,  imageFilter, userData);

            if (!ret)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_StartGetImage ret" + ret);

            return ret;
        }

        /// <summary>
        /// 检索翻页请求
        /// </summary>
        /// <param name="sessionID"></param>
        /// <param name="taskUnitID"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public bool StartSwitchResultInfo(UInt32 sessionID, UInt32 taskUnitID, UInt32 pageIndex)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_StartSwitchPageByTaskUnitID sessionID:{0},taskUnitID:{1},pageIndex:{2}", sessionID, taskUnitID, pageIndex));

            bool ret = false;

                ret = IVXSDKProtocol.VdaSdk_StartSwitchPageByTaskUnitID(sessionID, taskUnitID, pageIndex);
                        
            if (!ret)
            {
                CheckError();
            }


            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_StartSwitchPageByTaskUnitID ret" + ret);

            return ret;
        }

        /// <summary>
        /// 结束检索
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public bool CloseSearchSession(UInt32 sessionID)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_CloseSearchQueryHandle sessionID:" + sessionID);

            bool ret = false;

            ret = IVXSDKProtocol.VdaSdk_CloseSearchQueryHandle(sessionID);
            if (!ret)
            {
                CheckError();
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol VdaSdk_CloseSearchQueryHandle ret:" + ret);

            return ret;
        }

        #endregion

    }
}
