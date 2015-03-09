using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.DataModel
{
    public enum UIFunctionEnum
    {
        MyCaseList = 0x0000,
        NewCase = 0x0001,
        CurrCase = 0x0002,
        ModifyCase = 0x0003,
        Case,

        Search = 0x0100,
        SearchMotionObject = 0x0101,
        SearchFace = 0x0102,
        SearchVehicle = 0x0103,
        SearchByImage = 0x0104,

        LiveVideo = 0x0200,
        BriefVideo = 0x0201,

        Task,
        NewTask = 0x0300,
        NewTaskUnit = 0x0301,
        //RunningTasks = 0x0302,
        //FinishedTasks = 0x0303,
        VideoTasks = 0x0304,
        PictureTasks = 0x0305,
        TaskStatus = 0x0306,
        ImportVideos = 0x0307,
        ImportPictures = 0x0308,

        Configuration,
        PlatManagement = 0x0400,
        CameraManagement = 0x0401,
        UserManagement = 0x0402,
        CaseManagement = 0x0403,
        LogManagement = 0x0404,

        ClusterMonitor = 0x0500,
        MediaServerManagement = 0x0501,
        VDAServerManagement = 0x0502,
        VDAResultServerManagement = 0x0503,
        MediaRouterManagement = 0x0504,
        ClientRouterManagement = 0x0505,
        PASServerManagement = 0x0506,
        FtpHttpServerManagement = 0x0507,

        CaseExport = 0x0600,
        TagExport = 0x0601,
        Export = 0x0602,


        CmdImportVideo = 0x0900,
        CmdExportVideo = 0x0901,
        CmdPlayLiveVideo = 0x0902,
        CmdAddAnalysisType = 0x0903,
        ShowUserProfile = 0x0904,
        Backward = 0x0905,

        ShowDownloadListForm
    }

    public class CmdItemInfo
    {
        public UIFuncItemInfo Cmd { get; set; }
        public object Subject { get; set; }

        public CmdItemInfo(UIFuncItemInfo cmd, object subject)
        {
            Cmd = cmd;
            Subject = subject;
        }
    }

    public class UIFuncItemInfo
    {
        public static UIFuncItemInfo CASE = new UIFuncItemInfo(UIFunctionEnum.Case, "案件", null);
        public static UIFuncItemInfo MYCASELIST = new UIFuncItemInfo(UIFunctionEnum.MyCaseList, "我的案件", CASE);
        public static UIFuncItemInfo NEWCASE = new UIFuncItemInfo(UIFunctionEnum.NewCase, "新建案件", CASE);
        public static UIFuncItemInfo CURRCASE = new UIFuncItemInfo(UIFunctionEnum.CurrCase, "当前案件", CASE, true);
        public static UIFuncItemInfo MODIFYCASE = new UIFuncItemInfo(UIFunctionEnum.ModifyCase, "修改案件", CASE);
        
        //public static UIFuncItemInfo IMPORTVIDEOS = new UIFuncItemInfo(UIFunctionEnum.ImportVideos, "导入视频");
        //public static UIFuncItemInfo IMPORTPICTURES = new UIFuncItemInfo(UIFunctionEnum.ImportPictures, "导入图片");
        public static UIFuncItemInfo SEARCH = new UIFuncItemInfo(UIFunctionEnum.Search, "智能检索", null, true);
        public static UIFuncItemInfo SEARCHMOTIONOBJECT = new UIFuncItemInfo(UIFunctionEnum.SearchMotionObject, "运动物", SEARCH, true);
        public static UIFuncItemInfo SEARCHFACE = new UIFuncItemInfo(UIFunctionEnum.SearchFace, "人脸", SEARCH, true);
        public static UIFuncItemInfo SEARCHVEHICLE = new UIFuncItemInfo(UIFunctionEnum.SearchVehicle, "车辆", SEARCH, true);
        public static UIFuncItemInfo SEARCHBYIMAGE = new UIFuncItemInfo(UIFunctionEnum.SearchByImage, "以图搜图", SEARCH, true);

        public static UIFuncItemInfo LIVEVIDEO = new UIFuncItemInfo(UIFunctionEnum.LiveVideo, "视频播放", null, true);
        public static UIFuncItemInfo BRIEFVIDEO = new UIFuncItemInfo(UIFunctionEnum.BriefVideo , "摘要播放", null, true);

        public static UIFuncItemInfo CMDIMPORTVIDEO = new UIFuncItemInfo(UIFunctionEnum.CmdImportVideo, "导入");
        public static UIFuncItemInfo CMDEXPORTVIDEO = new UIFuncItemInfo(UIFunctionEnum.CmdExportVideo, "导出");
        public static UIFuncItemInfo CMDPLAYLIVEVIDEO = new UIFuncItemInfo(UIFunctionEnum.CmdPlayLiveVideo, "实时");
        public static UIFuncItemInfo CMDADDANALYSISTYPE =  new UIFuncItemInfo(UIFunctionEnum.CmdAddAnalysisType, "分析");

        public static UIFuncItemInfo TASK = new UIFuncItemInfo(UIFunctionEnum.Task, "任务", null, true);
        public static UIFuncItemInfo NEWTASK = new UIFuncItemInfo(UIFunctionEnum.NewTask, "新建任务", TASK, true);
        public static UIFuncItemInfo NEWTASKUNIT = new UIFuncItemInfo(UIFunctionEnum.NewTaskUnit, "新建任务单元", TASK);
        public static UIFuncItemInfo VIDEOSTASKS = new UIFuncItemInfo(UIFunctionEnum.VideoTasks, "按监控点视频任务", TASK);
        public static UIFuncItemInfo PICTURESTASKS = new UIFuncItemInfo(UIFunctionEnum.PictureTasks, "按监控点图片任务", TASK);
        public static UIFuncItemInfo TASKSTATUS = new UIFuncItemInfo(UIFunctionEnum.TaskStatus, "任务状态", TASK, true);

        public static UIFuncItemInfo CONFIGURATION = new UIFuncItemInfo(UIFunctionEnum.Configuration, "系统配置", null);
        public static UIFuncItemInfo PLATMANAGEMENT = new UIFuncItemInfo(UIFunctionEnum.PlatManagement, "平台管理", CONFIGURATION);
        public static UIFuncItemInfo CAMERAMANAGEMENT = new UIFuncItemInfo(UIFunctionEnum.CameraManagement , "监控点管理", CONFIGURATION);
        public static UIFuncItemInfo USERMANAGEMENT = new UIFuncItemInfo(UIFunctionEnum.UserManagement, "用户管理", CONFIGURATION);
        public static UIFuncItemInfo CASEMANAGEMENT = new UIFuncItemInfo(UIFunctionEnum.CaseManagement, "案件管理", CONFIGURATION);
        public static UIFuncItemInfo LOGMANAGEMENT = new UIFuncItemInfo(UIFunctionEnum.LogManagement, "日志管理", CONFIGURATION);

        public static UIFuncItemInfo CLUSTERMONITOR = new UIFuncItemInfo(UIFunctionEnum.ClusterMonitor, "集群管理", CONFIGURATION);
        public static UIFuncItemInfo MEDIASERVERMANAGEMENT = new UIFuncItemInfo(UIFunctionEnum.MediaServerManagement, "媒体服务器", CONFIGURATION);
        public static UIFuncItemInfo VDASERVERMANAGEMENT = new UIFuncItemInfo(UIFunctionEnum.VDAServerManagement, "分析存储服务器", CONFIGURATION);
        public static UIFuncItemInfo VDARESULTSERVERMANAGEMENT = new UIFuncItemInfo(UIFunctionEnum.VDAResultServerManagement, "检索比对服务器", CONFIGURATION);
        public static UIFuncItemInfo PASSERVERMANAGEMENT = new UIFuncItemInfo(UIFunctionEnum.PASServerManagement, "预分析服务器", CONFIGURATION);
        public static UIFuncItemInfo MEDIAROUTERMANAGEMENT = new UIFuncItemInfo(UIFunctionEnum.MediaRouterManagement, "媒体接入服务器", CONFIGURATION);
        public static UIFuncItemInfo CLIENTROUTERMANAGEMENT = new UIFuncItemInfo(UIFunctionEnum.ClientRouterManagement, "用户接入服务器", CONFIGURATION);
        public static UIFuncItemInfo FTPHTTPSERVERMANAGEMENT = new UIFuncItemInfo(UIFunctionEnum.FtpHttpServerManagement, "其他服务器", CONFIGURATION);

        public static UIFuncItemInfo SHOWUSERPROFILE = new UIFuncItemInfo(UIFunctionEnum.ShowUserProfile, "显示用户详细信息");

        public static UIFuncItemInfo BACKWARD = new UIFuncItemInfo(UIFunctionEnum.Backward, "后退");

        public static UIFuncItemInfo EXPORT = new UIFuncItemInfo(UIFunctionEnum.Export, "导出",null);
        public static UIFuncItemInfo CASEEXPORT = new UIFuncItemInfo(UIFunctionEnum.CaseExport, "案件导出",EXPORT);
        public static UIFuncItemInfo TAGEXPORT = new UIFuncItemInfo(UIFunctionEnum.TagExport, "标签导出", EXPORT);

        public static UIFuncItemInfo SHOWDOWNLOADLIST = new UIFuncItemInfo(UIFunctionEnum.ShowDownloadListForm, "下载查看");

        public string Caption { get; set; }

        public UIFunctionEnum Function { get; set; }

        public object Subject { get; set; }

        public UIFuncItemInfo Parent { get; private set; }

        public bool DependsOnCase { get; private set; }

        public UIFuncItemInfo(UIFunctionEnum func, string caption, UIFuncItemInfo parent=null, bool dependsOnCase = false)
        {
            Caption = caption;
            Function = func;
            Parent = parent;
            DependsOnCase = dependsOnCase;
        }

    }

}
