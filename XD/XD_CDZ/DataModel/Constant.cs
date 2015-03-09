using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.DataModel;
// using BOCOM.IVX.Protocol;
using System.Drawing;
using DataModel;
using System.Windows.Forms;

namespace BOCOM.DataModel
{
    public class Constant
    {
        public static readonly Size DEFAULTSIZE_THUMBNAIL = new Size(90, 90);
        private static TaskPriorityInfo[] s_TaskPriorityInfos;
        private static TaskStatusInfo[] s_TaskStatusInfos;
        private static AnalyzeStatusInfo[] s_AnalyzeStatusInfos;
        private static TaskUnitTypeInfo[] s_TaskUnitTypeInfos;
        private static UserRoleTypeInfo[] s_UserRoleTypeInfos;
        
        private static TaskUnitImportStatusInfo[] s_TaskUnitImportStatusInfos;
        
        private static LogTypeInfo[] s_LogTypeInfos;

        private static DownloadStatusInfo[] s_DownloadStatusInfos;

        private static LogLevelInfo[] s_LogLevelInfos;
        
        private static LogDetailTypeInfo[] s_LogDetailTypeInfos;
        
        private static AccessProtocolTypeInfo[] s_AccessProtocolTypeInfos;

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

        public readonly static VideoSupplierDeviceInfo VIDEOSUPPLIERDEVICEINFO_DUMMY = new VideoSupplierDeviceInfo()
        {
            DeviceName = "无",
            Id = 0
        };

        public static VehicleTypeInfo[] s_VehicleTypeInfos;

        public static SearchResultObjectTypeInfo[] s_SearchResultObjectTypeInfos;

        public static VehicleDetailTypeInfo[] s_VehicleDetailTypeInfos;

        public static VehiclePlateTypeInfo[] s_VehiclePlateTypeInfos;

        public static TaskPriorityInfo[] TaskPriorityInfos
        {
            get
            {
                if (s_TaskPriorityInfos == null)
                {
                    s_TaskPriorityInfos = new TaskPriorityInfo[]
                    {
                        new TaskPriorityInfo(TaskPriority.Highest, "最高"),
                        new TaskPriorityInfo(TaskPriority.High, "高"),
                        new TaskPriorityInfo(TaskPriority.Normal, "标准"),
                        new TaskPriorityInfo(TaskPriority.Low, "低"),
                        new TaskPriorityInfo(TaskPriority.Lowest, "最低")
                    };
                }
                return s_TaskPriorityInfos;
            }
        }

        public static AnalyzeStatusInfo[] AnalyzeStatusInfos
        {
            get
            {
                if (s_AnalyzeStatusInfos == null)
                {
                    s_AnalyzeStatusInfos = new AnalyzeStatusInfo[]
                    {
                        new AnalyzeStatusInfo(E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_NOUSE, ""),
                        new AnalyzeStatusInfo(E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_WAIT, "等待"),
                        new AnalyzeStatusInfo(E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_EXECUTING, "分析中"),
                        new AnalyzeStatusInfo(E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_FINISH, "√"),
                        new AnalyzeStatusInfo(E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_FAILED, "失败")
                    };
                }
                return s_AnalyzeStatusInfos;
            }
        }

        public static TaskStatusInfo[] TaskStatusInfos
        {
            get
            {
                if (s_TaskStatusInfos == null)
                {
                    s_TaskStatusInfos = new TaskStatusInfo[]
                            {
                                new TaskStatusInfo(E_VDA_TASK_STATUS.E_TASK_WAIT, "等待处理"),
                                new TaskStatusInfo(E_VDA_TASK_STATUS.E_TASK_EXECUTING, "正在处理"),
                                new TaskStatusInfo(E_VDA_TASK_STATUS.E_TASK_FINISH, "任务完成"),
                                new TaskStatusInfo(E_VDA_TASK_STATUS.E_TASK_FAILED, "任务失败")
                            };
                }
                return s_TaskStatusInfos;
            }
        }

        public static TaskUnitTypeInfo[] TaskUnitTypeInfos
        {
            get
            {
                if (s_TaskUnitTypeInfos == null)
                {
                    s_TaskUnitTypeInfos = new TaskUnitTypeInfo[]
                            {
                                new TaskUnitTypeInfo(E_VDA_TASK_UNIT_TYPE.E_TASKUNIT_TYPE_UNKNOW, "未知类型"),
                                new TaskUnitTypeInfo(E_VDA_TASK_UNIT_TYPE.E_TASKUNIT_TYPE_CLIENT_VIDEO_FILE, "本地视频"),
                                new TaskUnitTypeInfo(E_VDA_TASK_UNIT_TYPE.E_TASKUNIT_TYPE_FILESERVER_VIDEO_FILE, "服务器视频"),
                                new TaskUnitTypeInfo(E_VDA_TASK_UNIT_TYPE.E_TASKUNIT_TYPE_NETSTORE_VIDEO_FILE, "存储设备视频"),
                                new TaskUnitTypeInfo(E_VDA_TASK_UNIT_TYPE.E_TASKUNIT_TYPE_CLIENT_PIC_PACKAGE, "本地图片"),
                                new TaskUnitTypeInfo(E_VDA_TASK_UNIT_TYPE.E_TASKUNIT_TYPE_FILESERVER_PIC_PACKAGE, "服务器图片")
                            };
                }
                return s_TaskUnitTypeInfos;
            }
        }

        public static TaskUnitImportStatusInfo[] TaskUnitImportStatusInfos
        {
            get
            {
                if (s_TaskUnitImportStatusInfos == null)
                {
                    s_TaskUnitImportStatusInfos = new TaskUnitImportStatusInfo[]
                    {
                        new TaskUnitImportStatusInfo(E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_NOUSE,""),
                        new TaskUnitImportStatusInfo(E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_IMPORT_WAIT,"导入等待"),
                        new TaskUnitImportStatusInfo(E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_IMPORT_READY,"导入准备"),
                        new TaskUnitImportStatusInfo(E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_IMPORT_EXECUTING	,"导入中"),
                        new TaskUnitImportStatusInfo(E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_IMPORT_FINISH		,"导入完成"),
                        new TaskUnitImportStatusInfo(E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_IMPORT_FAILED			,"导入失败"),
                        new TaskUnitImportStatusInfo(E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_PREANALYSE_WAIT			,"预分析等待"),
                        new TaskUnitImportStatusInfo(E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_PREANALYSE_EXECUTING	,"预分析中"),
                        new TaskUnitImportStatusInfo(E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_PREANALYSE_FINISH	,"预分析完成"),
                        new TaskUnitImportStatusInfo(E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_PREANALYSE_FAILED		,"预分析失败"),
                        new TaskUnitImportStatusInfo(E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_WAIT			,"分析等待"),
                        new TaskUnitImportStatusInfo(E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_EXECUTING		,"分析中"),
                        new TaskUnitImportStatusInfo(E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_FINISH		,"分析完成"),
                        new TaskUnitImportStatusInfo(E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_FAILED		,"分析失败"),
                    };
                }
                return s_TaskUnitImportStatusInfos;
            }
        }

        public static UserRoleTypeInfo[] UserRoleTypeInfos
        {
            get
            {
                if (s_UserRoleTypeInfos == null)
                {
                    s_UserRoleTypeInfos = new UserRoleTypeInfo[]
                            {
                                new UserRoleTypeInfo(E_VDA_USER_ROLE_TYPE.E_ROLE_TYPE_NORMAL, "普通用户"),
                                new UserRoleTypeInfo(E_VDA_USER_ROLE_TYPE.E_ROLE_TYPE_LEADER, "组长"),
                                new UserRoleTypeInfo(E_VDA_USER_ROLE_TYPE.E_ROLE_TYPE_ADMIN, "管理员"),
                                new UserRoleTypeInfo(E_VDA_USER_ROLE_TYPE.E_ROLE_TYPE_SUPPER, "超级管理员"),
                            };
                }
                return s_UserRoleTypeInfos;
            }
        }

        public static VehicleTypeInfo[] VehicleTypeInfos
        {
            get
            {
                if (s_VehicleTypeInfos == null)
                {
                    s_VehicleTypeInfos = new VehicleTypeInfo[]
                    {
                        new VehicleTypeInfo() { Type = VehicleType.None, Name = "不限" },
                        new VehicleTypeInfo() { Type = VehicleType.Big, Name = "大车" },
                        new VehicleTypeInfo() { Type = VehicleType.Middle, Name = "中车" },
                        new VehicleTypeInfo() { Type = VehicleType.Small, Name = "小车" },
                        new VehicleTypeInfo() { Type = VehicleType.Other, Name = "其它" },
                    };
                }
                return s_VehicleTypeInfos;
            }
        }

        public static SearchResultObjectTypeInfo[] SearchResultObjectTypeInfos
        {
            get
            {
                if (s_SearchResultObjectTypeInfos == null)
                {
                    s_SearchResultObjectTypeInfos = new SearchResultObjectTypeInfo[]
                    {
                        new SearchResultObjectTypeInfo() { Type = SearchResultObjectType.None, Name = "不限" },
                        new SearchResultObjectTypeInfo() { Type = SearchResultObjectType.CAR, Name = "车" },
                        new SearchResultObjectTypeInfo() { Type = SearchResultObjectType.PEOPLE, Name = "人" },
                        new SearchResultObjectTypeInfo() { Type = SearchResultObjectType.Unknown, Name = "未知" },
                        new SearchResultObjectTypeInfo() { Type = SearchResultObjectType.FACE, Name = "人脸" },
                    };
                }
                return s_SearchResultObjectTypeInfos;
            }
        }

        public static VehicleDetailTypeInfo[] VehicleDetailTypeInfos
        {
            get
            {
                if (s_VehicleDetailTypeInfos == null)
                {
                    s_VehicleDetailTypeInfos = new VehicleDetailTypeInfo[]
                    {
                        new VehicleDetailTypeInfo() { Type = VehicleDetailType.None, Name = "不限" },
                        new VehicleDetailTypeInfo() { Type = VehicleDetailType.Big, Name = "大型客车" },
                        new VehicleDetailTypeInfo() { Type = VehicleDetailType.Truck, Name = "大型货车" },
                        new VehicleDetailTypeInfo() { Type = VehicleDetailType.Middle, Name = "中型客车" },
                        new VehicleDetailTypeInfo() { Type = VehicleDetailType.Small, Name = "小型客车" },
                        new VehicleDetailTypeInfo() { Type = VehicleDetailType.Bicyle, Name = "两轮车" },
                        new VehicleDetailTypeInfo() { Type = VehicleDetailType.Other, Name = "其它" },
                        new VehicleDetailTypeInfo() { Type = VehicleDetailType.SmallTruck, Name = "小型货车" },
                    };
                }
                return s_VehicleDetailTypeInfos;
            }
        }

        public static VehiclePlateTypeInfo[] VehiclePlateTypeInfos
        {
            get
            {
                if (s_VehiclePlateTypeInfos == null)
                {
                    s_VehiclePlateTypeInfos = new VehiclePlateTypeInfo[]
                    {
                        new VehiclePlateTypeInfo() { Type = VehiclePlateType.None, Name = "不限" },
                        new VehiclePlateTypeInfo() { Type = VehiclePlateType.SingleRow, Name = "单排" },
                        new VehiclePlateTypeInfo() { Type = VehiclePlateType.DoubleRow, Name = "双排" },
                        new VehiclePlateTypeInfo() { Type = VehiclePlateType.Other, Name = "其它" },
                    };
                }
                return s_VehiclePlateTypeInfos;
            }
        }

        public static LogTypeInfo[] LogTypeInfos
        {
            get
            {
                if (s_LogTypeInfos == null)
                {
                    s_LogTypeInfos = new LogTypeInfo[]
                            {
                                new LogTypeInfo(E_VDA_LOG_TYPE.E_LOG_TYPE_NOUSE, "其他"),
                                new LogTypeInfo(E_VDA_LOG_TYPE.E_LOG_TYPE_MANAGER, "管理日志"),
                                new LogTypeInfo(E_VDA_LOG_TYPE.E_LOG_TYPE_OPERATE, "操作日志"),
                                new LogTypeInfo(E_VDA_LOG_TYPE.E_LOG_TYPE_SYSTEM, "系统日志"),
                            };
                }
                return s_LogTypeInfos;
            }
        }

        public static DownloadStatusInfo[] DownloadStatusInfos
        {
            get
            {
                if (s_DownloadStatusInfos == null)
                {
                    s_DownloadStatusInfos = new DownloadStatusInfo[]
                            {
                                new DownloadStatusInfo(VideoDownloadStatus.NOUSE, "未知"),
                                new DownloadStatusInfo(VideoDownloadStatus.Trans_Wait, "等待转码"),
                                new DownloadStatusInfo(VideoDownloadStatus.Trans_Normal, "正在转码"),
                                new DownloadStatusInfo(VideoDownloadStatus.Trans_Finish, "完成转码 "),
                                new DownloadStatusInfo(VideoDownloadStatus.Trans_Failed, "转码失败"),
                                new DownloadStatusInfo(VideoDownloadStatus.Download_Wait, "等待导出"),
                                new DownloadStatusInfo(VideoDownloadStatus.Download_Normal, "正在导出"),
                                new DownloadStatusInfo(VideoDownloadStatus.Download_Finish, "完成导出"),
                                new DownloadStatusInfo(VideoDownloadStatus.Download_Failed, "导出失败")
                            };
                }
                return s_DownloadStatusInfos;
            }
        }


        public static LogLevelInfo[] LogLevelInfos
        {
            get
            {
                if (s_LogLevelInfos == null)
                {
                    s_LogLevelInfos = new LogLevelInfo[]
                            {
                                new LogLevelInfo(E_VDA_LOG_LEVEL.E_LOG_LEVEL_NOUSE, "其他"),
                                new LogLevelInfo(E_VDA_LOG_LEVEL.E_LOG_LEVEL_COMMON, "普通"),
                                new LogLevelInfo(E_VDA_LOG_LEVEL.E_LOG_LEVEL_WARN, "警告"),
                                new LogLevelInfo(E_VDA_LOG_LEVEL.E_LOG_LEVEL_ERROR, "错误"),
                            };
                }
                return s_LogLevelInfos;
            }
        }

        public static LogDetailTypeInfo[] LogDetailTypeInfos
        {
            get
            {
                if (s_LogDetailTypeInfos == null)
                {
                    s_LogDetailTypeInfos = new LogDetailTypeInfo[]
                            {
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_NOUSE, "其他"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_TASK_MANAGE, "任务管理"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_USER_LOGIN, "用户登录"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_USER_LOGOUT, "用户登出"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_START_SERACH, "开始检索"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_STOP_SERACH, "停止检索"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_START_PLAYBACK, "开始点播"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_CLOSE_PLAYBACK, "关闭点播"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_START_BRIEF_VOD, "开始摘要点播"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_CLOSE_BRIEF_VOD, "关闭摘要点播"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_ENTER_CASE, "进入案件"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_LEAVE_CASE, "离开案件"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_CAMERA_MANAGE, "监控点管理"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_CAMERA_GROUP_MANAGE, "监控点组管理"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_CASE_MANAGE, "案件管理"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_NET_STORE_MANAGE, "网络设备管理"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_USER_MANAGE, "用户管理"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_USER_GROUP_MANAGE, "用户组管理"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_SERVER_MANAGE, "服务器管理"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_TASKUNIT_STATUS, "任务单元状态"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_BRIEF_VOD_STATUS, "摘要点播状态"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_PLAYBACK_STATUS, "视频点播状态"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_IAS_STATUS, "智能检索状态"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_SERVER_STATUS, "服务器状态"),
                                new LogDetailTypeInfo(E_VDA_LOG_DETAIL.E_LOG_DETAIL_EXPORT_STATUS, "导出状态"),                            
                            };
                }
                return s_LogDetailTypeInfos;
            }
        }

        public static AccessProtocolTypeInfo[] AccessProtocolTypeInfos
        {
            get
            {
                if (s_AccessProtocolTypeInfos == null)
                {
                    s_AccessProtocolTypeInfos = new AccessProtocolTypeInfo[]
                            {
                                // new AccessProtocolTypeInfo(E_VDA_NET_STORE_DEV_PROTOCOL_TYPE.E_DEV_PROTOCOL_CONTYPE_NONE,		"其他协议类型"),
                                //new AccessProtocolTypeInfo(E_VDA_NET_STORE_DEV_PROTOCOL_TYPE.E_DEV_PROTOCOL_CONTYPE_ONVIF_PROTOCOL, "OnVif协议接入"),
                                //new AccessProtocolTypeInfo(E_VDA_NET_STORE_DEV_PROTOCOL_TYPE.E_DEV_PROTOCOL_CONTYPE_GB28181_PROTOCOL, "GB28181协议接入"),
                                //new AccessProtocolTypeInfo(E_VDA_NET_STORE_DEV_PROTOCOL_TYPE.E_DEV_PROTOCOL_CONTYPE_BCSYS_PLAT,	"博康系统平台"),
                                //new AccessProtocolTypeInfo(E_VDA_NET_STORE_DEV_PROTOCOL_TYPE.E_DEV_PROTOCOL_CONTYPE_H3C_PLAT, "华三平台"),
                                //new AccessProtocolTypeInfo(E_VDA_NET_STORE_DEV_PROTOCOL_TYPE.E_DEV_PROTOCOL_CONTYPE_HK_PLAT, "海康平台"),
                                //new AccessProtocolTypeInfo(E_VDA_NET_STORE_DEV_PROTOCOL_TYPE.E_DEV_PROTOCOL_CONTYPE_NETPOSA_PLAT, "东方网力平台"),
                                //new AccessProtocolTypeInfo(E_VDA_NET_STORE_DEV_PROTOCOL_TYPE.E_DEV_PROTOCOL_CONTYPE_HT_PLAT, "汇通平台"),
                                new AccessProtocolTypeInfo(E_VDA_NET_STORE_DEV_PROTOCOL_TYPE.E_DEV_PROTOCOL_CONTYPE_HK_DEV, "海康设备接入"),
                                //new AccessProtocolTypeInfo(E_VDA_NET_STORE_DEV_PROTOCOL_TYPE.E_DEV_PROTOCOL_CONTYPE_DH_DEV, "大华设备接入"),
                                //new AccessProtocolTypeInfo(E_VDA_NET_STORE_DEV_PROTOCOL_TYPE.E_DEV_PROTOCOL_CONTYPE_SANYO_IPC_DEV, "三洋IPC"),
                            };
                }
                return s_AccessProtocolTypeInfos;
            }
        }

    }
}

