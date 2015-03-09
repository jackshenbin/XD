using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BOCOM.DataModel
{
    #region 枚举类型定义
    public enum E_SWITCH_WND
    {
        SWITCH_WND_MAIN_PAGE=0,
        SWITCH_WND_USER_MENAGEMENT=1,
        SWITCH_WND_CARD_MENAGEMENT,
        SWITCH_WND_DEVICE_MENAGEMENT,
        SWITCH_WND_REPORT_MENAGEMENT,
    }
    public enum E_LOGIN_RESULT
    {
        LOGIN_SUCCESS = 0,		//登录成功
        LOGIN_IPPORT_ERROR,		//IP地址或端口号不正确
        LOGIN_USERNAME_INPUT,	//请输入用户名......
        LOGIN_PASSWORD_INPUT,	//请输入密码......
        //LOGIN_CONNECTING,			//正在连接服务器.......
        //LOGIN_CHECKING,			//服务器连接成功，正在验证用户......
        LOGIN_ERROR,			//连接不上服务器
        LOGIN_CHECK_ERROR,		//用户验证失败
        LOGIN_USER_ERROR,		//用户名不存在
        LOGIN_PASSWORD_ERROR,	//用户密码错误
        LOGIN_USER_TOOMANY,		//超过服务器支持的连接数
        LOGIN_USER_EXIST,		//该用户已经登录
    };
    public enum E_OBJECT_TYPE
    {
        SEARCH_ALL_OBJECT = 0,	//所有目标（或不确定）
        SEARCH_VEHICLE_OBJECT,	//车
        SEARCH_HUMAN_OBJECT		//人
    };

    /// <summary>
    /// 任务状态
    /// </summary>
    public enum E_VDA_TASK_STATUS
    {
        /// <summary>
        /// 任务未知状态
        /// </summary>
        E_TASK_NOUSE = 0,		
        /// <summary>
        /// 任务等待
        /// </summary>
        E_TASK_WAIT = 1,		
        /// <summary>
        /// 任务执行
        /// </summary>
        E_TASK_EXECUTING = 2,		
        /// <summary>
        /// 任务完成
        /// </summary>
        E_TASK_FINISH = 3,		
        /// <summary>
        /// 任务失败
        /// </summary>
        E_TASK_FAILED = 4,		
    };

    /// <summary>
    /// 任务单元类型
    /// </summary>
    public enum E_VDA_TASK_UNIT_TYPE
    {
        /// <summary>
        /// 未知类型
        /// </summary>
        E_TASKUNIT_TYPE_UNKNOW = 0,
        /// <summary>
        /// 客户端导入视频文件
        /// </summary>
        E_TASKUNIT_TYPE_CLIENT_VIDEO_FILE = 1,
        /// <summary>
        /// 服务器导入视频文件
        /// </summary>
        E_TASKUNIT_TYPE_FILESERVER_VIDEO_FILE = 2,
        /// <summary>
        /// 网络存储导入视频文件
        /// </summary>
        E_TASKUNIT_TYPE_NETSTORE_VIDEO_FILE = 3,
        /// <summary>
        /// 客户端导入图片包
        /// </summary>
        E_TASKUNIT_TYPE_CLIENT_PIC_PACKAGE = 4,
        /// <summary>
        /// 服务器导入图片包
        /// </summary>
        E_TASKUNIT_TYPE_FILESERVER_PIC_PACKAGE = 5,
    };

    /// <summary>
    /// 任务单元导入状态
    /// </summary>
    public enum E_VDA_TASK_UNIT_STATUS
    {
        /// <summary>
        /// 任务单元未知状态
        /// </summary>
        E_TASK_UNIT_NOUSE = 0,	
        /// <summary>
        /// 任务单元导入等待
        /// </summary>
        E_TASK_UNIT_IMPORT_WAIT = 1,	
        /// <summary>
        /// 任务单元导入准备
        /// </summary>
        E_TASK_UNIT_IMPORT_READY = 2,   	
        /// <summary>
        /// 任务单元导入开始
        /// </summary>
        E_TASK_UNIT_IMPORT_EXECUTING = 3,	
        /// <summary>
        /// 任务单元导入完成
        /// </summary>
        E_TASK_UNIT_IMPORT_FINISH = 4,	
        /// <summary>
        /// 任务单元导入失败
        /// </summary>
        E_TASK_UNIT_IMPORT_FAILED = 5,	
        /// <summary>
        /// 任务单元预分析等待
        /// </summary>
        E_TASK_UNIT_PREANALYSE_WAIT = 6,	
        /// <summary>
        /// 任务单元预分析开始
        /// </summary>
        E_TASK_UNIT_PREANALYSE_EXECUTING = 7,	
        /// <summary>
        /// 任务单元预分析完成
        /// </summary>
        E_TASK_UNIT_PREANALYSE_FINISH = 8,	
        /// <summary>
        /// 任务单元预分析失败
        /// </summary>
        E_TASK_UNIT_PREANALYSE_FAILED = 9,	
        /// <summary>
        /// 任务单元分析等待
        /// </summary>
        E_TASK_UNIT_ANALYSE_WAIT = 10,	
        /// <summary>
        /// 任务单元分析开始
        /// </summary>
        E_TASK_UNIT_ANALYSE_EXECUTING = 11,	
        /// <summary>
        /// 任务单元分析完成
        /// </summary>
        E_TASK_UNIT_ANALYSE_FINISH = 12,	
        /// <summary>
        /// 任务单元分析失败
        /// </summary>
        E_TASK_UNIT_ANALYSE_FAILED = 13,	
    };


    /// <summary>
    /// 全分析类型
    /// </summary>
    [Serializable]
    public enum E_VDA_ANALYZE_TYPE
    {
        /// <summary>
        /// 目标检测(运动物分析算法)
        /// </summary>
        E_ANALYZE_OBJECT = 0x00000001,
        /// <summary>
        /// 车牌识别
        /// </summary>
        E_ANALYZE_VEHICLE = 0x00000002,
        /// <summary>
        /// 人脸检测
        /// </summary>
        E_ANALYZE_FACE = 0x00000004,
        /// <summary>
        /// 浓缩摘要
        /// </summary>
        E_ANALYZE_BRIEAF = 0x00000008,
        /// <summary>
        /// 车牌图片识别
        /// </summary>
        E_ANALYZE_VEHICLE_PIC = 0x00000010,
        /// <summary>
        /// 人脸图片检测
        /// </summary>
        E_ANALYZE_FACE_PIC = 0x00000020,
    };

    /*
    /// <summary>
    /// 任务单元分析状态
    /// </summary>
    public enum E_VDA_TASK_UNIT_ANALYZE_STATUS
    {
        /// <summary>
        /// 无分析任务
        /// </summary>
        E_TASKUNIT_NO_ANALYZE = 0,
        /// <summary>
        /// 等待导入中
        /// </summary>
        E_TASKUNIT_ANALYZE_WAIT = 1,
        /// <summary>
        /// 分析中
        /// </summary>
        E_TASKUNIT_ANALYZE,
        /// <summary>
        /// 完成
        /// </summary>
        E_TASKUNIT_ANALYZE_COMPLETE,
        /// <summary>
        /// 分析失败
        /// </summary>
        E_TASKUNIT_ANALYZE_FAILED,

    };
    */

    /// <summary>
    /// 服务器类型
    /// </summary>
    public enum E_VDA_SERVER_TYPE
    {
        /// <summary>
        /// 所有类型的服务器
        /// </summary>
        E_SERVER_TYPE_ALL = 0,
        /// <summary>
        /// 媒体存储服务器
        /// </summary>
        E_SERVER_TYPE_MSS = 11,
        /// <summary>
        /// 智能分析服务器
        /// </summary>
        E_SERVER_TYPE_IAS = 12,
        /// <summary>
        /// 检索比对服务器
        /// </summary>
        E_SERVER_TYPE_SMS = 13,
        /// <summary>
        /// 结构化检索服务器
        /// </summary>
        E_SERVER_TYPE_SSS = 14,
        /// <summary>
        /// 媒体接入网关
        /// </summary>
        E_SERVER_TYPE_MGW = 15,
        /// <summary>
        /// 用户接入网关
        /// </summary>
        E_SERVER_TYPE_UGW = 16,
        /// <summary>
        /// 预分析服务器
        /// </summary>
        E_SERVER_TYPE_PAS = 17,
        /// <summary>
        /// 分析调度服务器 
        /// </summary>
        E_SERVER_TYPE_IDS = 18, 
        /// <summary>
        /// 非结构化检索调度服务器
        /// </summary>
        E_SERVER_TYPE_SMDS = 19, 
        /// <summary>
        /// 结构化检索调度服务器 
        /// </summary>
        E_SERVER_TYPE_SSDS = 20, 
        E_SERVER_TYPE_FTP = 21, 
        E_SERVER_TYPE_HTTP = 22, 
    };

    /// <summary>
    /// 用户角色类型
    /// </summary>
    public enum E_VDA_USER_ROLE_TYPE
    {
        /// <summary>
        /// 普通用户（组员）
        /// </summary>
        E_ROLE_TYPE_NORMAL = 1,
        /// <summary>
        /// 组长
        /// </summary>
        E_ROLE_TYPE_LEADER,
        /// <summary>
        /// 管理员
        /// </summary>
        E_ROLE_TYPE_ADMIN,
        /// <summary>
        /// 超级管理员
        /// </summary>
        E_ROLE_TYPE_SUPPER,
    };

    /// <summary>
    /// 会话状态
    /// </summary>
    public enum E_VDA_SESSION_STATUS
    {
        /// <summary>
        /// 会话离线
        /// </summary>
        E_SESSION_STATUS_OFFLINE = 0,
        /// <summary>
        /// 会话在线
        /// </summary>
        E_SESSION_STATUS_ONLINE,
    };

    /// <summary>
    /// 配置通知类型定义
    /// </summary>
    public enum E_VDA_SDK_CFG_NOTIFY_TYPE
    {
        /// <summary>
        /// 新增通知
        /// </summary>
        E_CFG_NOTIFY_TYPE_ADD = 1,
        /// <summary>
        /// 修改通知
        /// </summary>
        E_CFG_NOTIFY_TYPE_MDF = 2,
        /// <summary>
        /// 删除通知
        /// </summary>
        E_CFG_NOTIFY_TYPE_DEL = 3,
    };

    /// <summary>
    /// 播放速度
    /// </summary>
    public enum E_VDA_PLAY_SPEED
    {
        E_PLAYSPEED_SLOW16 = 1,
        E_PLAYSPEED_SLOW8,
        E_PLAYSPEED_SLOW4,
        E_PLAYSPEED_SLOW2,
        E_PLAYSPEED_NORMALSPEED,
        E_PLAYSPEED_FAST2,
        E_PLAYSPEED_FAST4,
        E_PLAYSPEED_FAST8,
        E_PLAYSPEED_FAST16,
    };


    /// <summary>
    /// 播放控制类型
    /// </summary>
    public enum E_VDA_PLAYCTRL_TYPE
    {
        E_PLAYCTRL_START = 1,		//开始播放
        E_PLAYCTRL_STOP = 2,	    //停止播放

        E_PLAYCTRL_PAUSE = 3,	        //暂停播放
        E_PLAYCTRL_RESUME = 4,			//恢复播放
        // 	E_PLAYCTRL_FAST,	        //快放
        // 	E_PLAYCTRL_SLOW,	        //慢放

        E_PLAYCTRL_SETSPEED = 11,	//设置播放速度, 见E_VDA_PLAY_SPEED
        E_PLAYCTRL_GETSPEED = 12,		//获取播放速度
        E_PLAYCTRL_STEP = 13,			//单帧前进
        E_PLAYCTRL_STEP_BACK = 14,		//单帧后退

        E_PLAYCTRL_SETTIME = 15,			//按绝对时间定位（必须是按时间范围播放）
        E_PLAYCTRL_GETTIME = 16,			//获取回放绝对时间
        E_PLAYCTRL_SETPOS = 17,	        //定位回放的进度（千分比整数值，801表示80.1%）
        E_PLAYCTRL_GETPOS = 18,	        //获取回放的进度
        E_PLAYCTRL_GETTIME_RANGE = 19,			//获取回放总时间
    	E_PLAYCTRL_PLAY_BY_SEEK = 20,	//定位后播放
    };

    /// <summary>
    /// 播放状态
    /// </summary>
    public enum E_VDA_PLAY_STATUS
    {
        E_PLAY_STATUS_NORMAL = 1,		//正常播放
        E_PLAY_STATUS_FINISH,	        //结束播放
        E_PLAY_STATUS_FAILED,	        //播放失败
        E_PLAY_STATUS_STARTPLAY_READY,	//开始播放准备就绪(StartPlay时触发)
        E_PLAY_STATUS_SYNTH_ERROR,		//合成失败
    };

    /// <summary>
    /// 下载状态
    /// </summary>
    public enum E_VDA_DOWNLOAD_STATUS
    {
        E_DOWNLOAD_STATUS_NOUSE = 0, //未知的导出状态 
        E_DOWNLOAD_STATUS_TRANS_CODE_WAIT = 1, //等待转码 
        E_DOWNLOAD_STATUS_TRANS_CODE_NORMAL, //正在转码 
        E_DOWNLOAD_STATUS_TRANS_CODE_FINISH, //完成转码 
        E_DOWNLOAD_STATUS_TRANS_CODE_FAILED, //转码失败 
        E_DOWNLOAD_STATUS_DOWN_LOAD_WAIT, //等待导出 
        E_DOWNLOAD_STATUS_DOWN_LOAD_NORMAL, //正在导出 
        E_DOWNLOAD_STATUS_DOWN_LOAD_FINISH, //完成导出 
        E_DOWNLOAD_STATUS_DOWN_LOAD_FAILED, //导出失败 
    };

    /// <summary>
    /// 导出结果
    /// </summary>
    public enum E_VDA_EXPORT_STATUS
    {
        E_EXPORT_STATUS_NORMAL = 1,		//正常导出
        E_EXPORT_STATUS_FINISH,	        //结束导出
        E_EXPORT_STATUS_FAILED,	        //导出失败
    }

    /// <summary>
    /// 抓图保存图片格式
    /// </summary>
    public enum E_VDA_GRAB_PIC_TYPE
    {
        E_GRAB_PIC_BMP = 0,	//bmp格式
        E_GRAB_PIC_JPG = 1,	//Jpg格式
    };


    /// <summary>
    /// 运动对象类型（用于人车分类）
    /// </summary>
    public enum E_VDA_MOVEOBJ_TYPE
    {

        E_VDA_MOVEOBJ_TYPE_ALL = 0,	    //0全部
        E_VDA_MOVEOBJ_TYPE_PEOPLE,		//人
        E_VDA_MOVEOBJ_TYPE_CAR,			//车
        E_VDA_MOVEOBJ_TYPE_UNKNOWN,     //未知
    }

    //运动目标颜色
    public  enum E_VDA_MOVEOBJ_COLOR
    {	
	    E_VDA_MOVEOBJ_COLOR_NULL = 0,	//无
	    E_VDA_MOVEOBJ_COLOR_RED,		//红
	    E_VDA_MOVEOBJ_COLOR_GREEN,		//绿
	    E_VDA_MOVEOBJ_COLOR_BLUE,		//蓝
	    E_VDA_MOVEOBJ_COLOR_ORANGE,	//橘黄
	    E_VDA_MOVEOBJ_COLOR_YELLOW,	//黄
	    E_VDA_MOVEOBJ_COLOR_PURPLE,	//紫
	    E_VDA_MOVEOBJ_COLOR_WHITE,		//白
	    E_VDA_MOVEOBJ_COLOR_BLACK,		//黑
    };
    /// <summary>
    /// 摘要播放对象密度
    /// </summary>
    public enum E_VDA_BRIEF_DENSITY
    {
        E_BRIEF_DENSITY_00 = 0,	//摘要密度0.0
        E_BRIEF_DENSITY_01,		//摘要密度0.1
        E_BRIEF_DENSITY_02,		//摘要密度0.2
        E_BRIEF_DENSITY_03,		//摘要密度0.3
        E_BRIEF_DENSITY_04,		//摘要密度0.4
        E_BRIEF_DENSITY_05,		//摘要密度0.5
        E_BRIEF_DENSITY_06,		//摘要密度0.6
        E_BRIEF_DENSITY_07,		//摘要密度0.7
        E_BRIEF_DENSITY_08,		//摘要密度0.8
        E_BRIEF_DENSITY_09,		//摘要密度0.9
        E_BRIEF_DENSITY_10,		//摘要密度1.0
    }

    /// <summary>
    /// 摘要播放控制类型
    /// </summary>
    public enum E_VDA_BRIEF_PLAYCTRL_TYPE
    {

        E_BRIEF_PLAYCTRL_START = 1,	//开始播放（进行摘要合成并播放）
        E_BRIEF_PLAYCTRL_STOP = 2,	    //停止播放

        E_BRIEF_PLAYCTRL_PAUSE = 3,	        //暂停播放
        E_BRIEF_PLAYCTRL_RESUME = 4,		//恢复播放

        E_BRIEF_PLAYCTRL_SETSPEED = 11,		//设置播放速度, 见E_VDA_PLAY_SPEED(目前复用视频播放的速度定义）
        E_BRIEF_PLAYCTRL_GETSPEED = 12,		//获取播放速度

        E_BRIEF_PLAYCTRL_SETPOS = 13,	        //定位回放的进度（千分比整数值，801表示80.1%）
        E_BRIEF_PLAYCTRL_GETPOS = 14,	        //获取回放的进度
        E_BRIEF_PLAYCTRL_GETTIME_RANGE = 19,	//获取播放时长(秒）
    }

    /// <summary>
    /// 摘要行为过滤类型
    /// </summary>
    public enum E_VDA_BRIEF_DRAW_FILTER_TYPE
    {
        E_BRIEF_ACTION_FILTER_TYPE_NULL = 0,		//空类型
        E_BRIEF_ACTION_FILTER_TYPE_PASSLINE = 1,	//越界线
        E_BRIEF_ACTION_FILTER_TYPE_BREAK_AREA = 2,	//闯入闯出区域
        E_BRIEF_AREA_FILTER_TYPE_SHEILD = 3,		//屏蔽区
        E_BRIEF_AREA_FILTER_TYPE_INTEREST = 4,		//兴趣区
    }

    /// <summary>
    /// 摘要播放叠加画图信息类型
    /// </summary>
    public enum E_VDA_BRIEF_PLAY_DRAW_TYPE
    {

        E_BRIEF_PLAY_DRAW_OBJ_FRAME = 0,	//叠加目标框
        E_BRIEF_PLAY_DRAW_OBJ_TIME = 1,			//叠加目标时间
        E_BRIEF_PLAY_DRAW_ACTION_FILTER = 2,	//叠加行为过滤
        E_BRIEF_PLAY_DRAW_AREA_FILTER = 3,		//叠加区域过滤
    }

    /// <summary>
    /// 摘要窗口鼠标操作类型
    /// </summary>
    public enum E_VDA_BRIEF_WND_MOUSE_OPT_TYPE
    {

        E_BRIEF_WND_MOUSE_UNKNOW = 0,	//鼠标无效操作
        E_BRIEF_WND_MOUSE_LCLICK = 1,	//鼠标左键单击
        E_BRIEF_WND_MOUSE_LDCLICK = 2,		//鼠标左键双击
        E_BRIEF_WND_MOUSE_RCLICK = 3,		//鼠标右键单击
        E_BRIEF_WND_MOUSE_RDCLICK = 4,		//鼠标右键双击
    }

    /// <summary>
    /// 播放窗口鼠标操作类型
    /// </summary>
    public enum E_VDA_PLAY_WND_MOUSE_OPT_TYPE
    {
        E_PLAY_WND_MOUSE_UNKNOW = 0,	//鼠标无效操作
        E_PLAY_WND_MOUSE_LCLICK = 1,	//鼠标左键单击
        E_PLAY_WND_MOUSE_LDCLICK = 2,		//鼠标左键双击
        E_PLAY_WND_MOUSE_RCLICK = 3,		//鼠标右键单击
        E_PLAY_WND_MOUSE_RDCLICK = 4,		//鼠标右键双击
    };
    /// <summary>
    /// 智能分析检索闯入闯出区域类型
    /// </summary>
    public enum E_VDA_SEARCH_BREAK_REGION_TYPE
    {
        E_SEARCH_BREAK_REGION_TYPE_NOUSE = 0,		//未知类型
        E_SEARCH_BREAK_REGION_TYPE_IN,				//闯入
        E_SEARCH_BREAK_REGION_TYPE_OUT,				//闯出
        E_SEARCH_BREAK_REGION_TYPE_DOUBLE,			//双向
    }

    /// <summary>
    /// 智能分析检索越界线类型
    /// </summary>
    public enum E_VDA_SEARCH_PASS_LINE_TYPE
    {
        E_SEARCH_PASS_LINE_TYPE_NOUSE = 0,		//未知类型
        E_SEARCH_PASS_LINE_TYPE_SINGLE,			//单向
        E_SEARCH_PASS_LINE_TYPE_DOUBLE,			//双向
    }

    /// <summary>
    /// 智能分析检索对象类型
    /// </summary>
    public enum E_VDA_SEARCH_OBJ_TYPE
    {
        E_SEARCH_OBJECT_TYPE_NOUSE = 0,			//无效类型
        E_SEARCH_OBJECT_TYPE_CAR,				//车辆
        E_SEARCH_OBJECT_TYPE_PEOPLE,			//人脸	
        E_SEARCH_OBJECT_TYPE_UNKNOW_MOVE_OBJ,	//未知运动物
        E_SEARCH_OBJECT_TYPE_FACE,				//人脸
    }

    /// <summary>
    /// 检索结果排序类型
    /// </summary>
    public enum E_VDA_SEARCH_SORT_TYPE
    {
        /// <summary>
        /// 未知类型
        /// </summary>
        E_SEARCH_SORT_TYPE_NOUSE = 0,
        /// <summary>
        /// 相似度排序
        /// </summary>
        E_SEARCH_SORT_TYPE_SIMILAR,
        /// <summary>
        /// 时间升序
        /// </summary>
        E_SEARCH_SORT_TYPE_TIME_ASC,
        /// <summary>
        /// 时间降序
        /// </summary>
        E_SEARCH_SORT_TYPE_TIME_DESC,
    }

    /// <summary>
    /// 检索条件对象过滤类型
    /// </summary>
    public enum E_VDA_SEARCH_MOVE_OBJ_RANGE_FILTER_TYPE
    {
        E_SEARCH_MOVE_OBJ_RANGE_FILTER_NOUSE = 0,				//无效类型
        E_SEARCH_MOVE_OBJ_RANGE_FILTER_PASS_LINE,				//越界线过滤
        E_SEARCH_MOVE_OBJ_RANGE_FILTER_BREAK_REGION,			//闯入闯出区域过滤
    }
    /// <summary>
    /// 检索条件以图搜图算法过滤类型
    /// </summary>
    public enum E_VDA_SEARCH_IMAGE_FILTER_TYPE
    {
        /// <summary>
        /// 无效类型
        /// <summary>
        E_SEARCH_IMAGE_FILTER_NOUSE = 0,
        /// <summary>
        /// 按颜色算法特征过滤
        /// <summary>
        E_SEARCH_IMAGE_FILTER_BLOB,
        /// <summary>
        /// 按纹理算法特征过滤	
        /// <summary>
        E_SEARCH_IMAGE_FILTER_SURF,
        /// <summary>
        /// 按人脸算法特征过滤
        /// <summary>
        E_SEARCH_IMAGE_FILTER_FACE
    };

    /// <summary>
    /// 检索条件对象过滤类型
    /// <summary>
    public enum E_VDA_SEARCH_MOVE_OBJ_FILTER_TYPE
    {
        /// <summary>
        /// 无效类型
        /// <summary>
        E_SEARCH_MOVE_OBJ_FILTER_NOUSE = 0,
        /// <summary>
        /// 按全部运动物目标过滤
        /// <summary>
        E_SEARCH_MOVE_OBJ_FILTER_ALL_MOVE_OBJ,
        /// <summary>
        /// 按车过滤
        /// <summary>
        E_SEARCH_MOVE_OBJ_FILTER_CAR,
        /// <summary>
        /// 按人过滤	
        /// <summary>
        E_SEARCH_MOVE_OBJ_FILTER_PEOPLE
    };


    public enum DownloadType
    {
        浓缩导出,
        结果图片,
        视频截图,
        视频剪辑,
        摘要导出,
    }



    public enum E_PDO_MOUSE_EVENT
    {
        E_PDO_MOUSE_UNKNOW = 0,		//鼠标无效操作
        E_PDO_MOUSE_LCLICK = 1,		//鼠标左键单击
        E_PDO_MOUSE_LDCLICK = 2,	//鼠标左键双击
        E_PDO_MOUSE_RCLICK = 3,		//鼠标右键单击
        E_PDO_MOUSE_RDCLICK = 4,	//鼠标右键双击
    };

    // 图片文件格式
    public enum E_PDO_PIC_TYPE
    {
        E_PDO_PIC_UNKNOWN = 0, //不确定
        E_PDO_PIC_JPG = 1,	//Jpg格式
        E_PDO_PIC_BMP = 2,	//bmp格式,暂不支持
    };

    //叠加画图类型
    public enum E_PDO_DRAW_TYPE
    {
        E_PDO_DRAW_NONE = 0,		//不画任何内容
        E_PDO_DRAW_RECT = 1,		//画矩形
        E_PDO_DRAW_PASS_LINE,		//越界线过滤
        E_PDO_DRAW_BREAK_REGION,			//闯入闯出区域过滤
    };

    // 配置通知类型定义
    public enum E_VDA_RESOURCE_OPERATE_TYPE
    {
        E_RESOURCE_OPERATE_TYPE_NOUSE = 0,//未知类型
        E_RESOURCE_OPERATE_TYPE_ADD,	 //增加资源(服务器、相机、任务单元等)
        E_RESOURCE_OPERATE_TYPE_DEL,	 //删除资源(服务器、相机、任务单元等)
        E_RESOURCE_OPERATE_TYPE_MDF,	 //修改资源(服务器、相机、任务单元等)
    };
    //资源类型
    public enum E_VDA_RESOURCE_TYPE
    {
	    E_RESOURCE_TYPE_NOUSE	= 0,//无效类型
	    E_RESOURCE_TYPE_SERVER,		//服务器
	    E_RESOURCE_TYPE_NET_STORE,	//网络设备	
	    E_RESOURCE_TYPE_CAMERA_GROUP,//相机组
	    E_RESOURCE_TYPE_CAMERA,		//相机
	    E_RESOURCE_TYPE_USER_GROUP,	//用户组
	    E_RESOURCE_TYPE_USER,		//用户
	    E_RESOURCE_TYPE_CASE,		//案件
	    E_RESOURCE_TYPE_TASK,		//任务
	    E_RESOURCE_TYPE_TASKUNIT,	//任务单元
    };

    //////////////////////////////////////////////////////////////////////////
    //						日志枚举类型									//
    //////////////////////////////////////////////////////////////////////////
    //日志类型
    public enum E_VDA_LOG_TYPE
    {
        E_LOG_TYPE_NOUSE = 0,		//无效类型
        E_LOG_TYPE_SYSTEM,				//系统日志
        E_LOG_TYPE_OPERATE,				//操作日志
        E_LOG_TYPE_MANAGER,				//管理日志	
    };
    //日志级别
    public enum E_VDA_LOG_LEVEL
    {
        E_LOG_LEVEL_NOUSE = 0,		//无效类型
        E_LOG_LEVEL_COMMON,				//普通级别
        E_LOG_LEVEL_WARN,				//警告级别
        E_LOG_LEVEL_ERROR,				//错误级别	
    };

    //日志排序方式
    public enum E_VDA_LOG_SORT_TYPE
    {
        E_LOG_SORT_TYPE_NOUSE = 0,    //无效类型
        E_LOG_SORT_TYPE_TIME_ASC,		//按时间升序
        E_LOG_SORT_TYPE_TIME_DESC,		//按时间降序
    };

    //日志操作者类型
    public enum E_LOG_OPERATE_TYPE
    {
        E_LOG_OPERATE_TYPE_INVALID = 0,     //默认无效值
        E_LOG_OPERATE_TYPE_USER,			//用户
        E_LOG_OPERATE_TYPE_VDM,				//管理服务器
        E_LOG_OPERATE_TYPE_MSS,				//媒体存储服务器
        E_LOG_OPERATE_TYPE_IAS,				//智能分析服务器
        E_LOG_OPERATE_TYPE_SMS,				//检索比对服务器
        E_LOG_OPERATE_TYPE_SSS,				//结构化检索服务器
        E_LOG_OPERATE_TYPE_MSW,				//媒体接入网关服务器
        E_LOG_OPERATE_TYPE_UGW,				//用户接入网关服务器
        E_LOG_OPERATE_TYPE_PAS,				//预分析服务器服务器
        E_LOG_OPERATE_TYPE_IDS,				//分析调度服务器
        E_LOG_OPERATE_TYPE_SMDS,			//非结构化检索调度服务器
        E_LOG_OPERATE_TYPE_SSDS,			//结构化检索调度服务器	

    };

    //日志细分
    public enum E_VDA_LOG_DETAIL
    {
        E_LOG_DETAIL_NOUSE = 0,		  //无效类型
        E_LOG_DETAIL_TASK_MANAGE,		  //任务管理
        E_LOG_DETAIL_USER_LOGIN,		  //用户登录
        E_LOG_DETAIL_USER_LOGOUT,		  //用户登出
        E_LOG_DETAIL_START_SERACH,		  //开始检索
        E_LOG_DETAIL_STOP_SERACH,		  //停止检索
        E_LOG_DETAIL_START_PLAYBACK,	  //开始点播
        E_LOG_DETAIL_CLOSE_PLAYBACK,	  //关闭点播
        E_LOG_DETAIL_START_BRIEF_VOD,	  //开始摘要点播
        E_LOG_DETAIL_CLOSE_BRIEF_VOD,	  //关闭摘要点播
        E_LOG_DETAIL_ENTER_CASE,		  //进入案件
        E_LOG_DETAIL_LEAVE_CASE,		  //离开案件
        E_LOG_DETAIL_CAMERA_MANAGE,		  //相机管理
        E_LOG_DETAIL_CAMERA_GROUP_MANAGE, //相机组管理
        E_LOG_DETAIL_CASE_MANAGE,		  //案件管理
        E_LOG_DETAIL_NET_STORE_MANAGE,	  //网络设备管理
        E_LOG_DETAIL_USER_MANAGE,		  //用户管理
        E_LOG_DETAIL_USER_GROUP_MANAGE,	  //用户组管理
        E_LOG_DETAIL_SERVER_MANAGE,		  //服务器管理
        E_LOG_DETAIL_TASKUNIT_STATUS,	  //任务单元状态
        E_LOG_DETAIL_BRIEF_VOD_STATUS,	  //摘要点播状态
        E_LOG_DETAIL_PLAYBACK_STATUS,	  //视频点播状态
        E_LOG_DETAIL_IAS_STATUS,		  //智能检索状态
        E_LOG_DETAIL_SERVER_STATUS,		  //服务器状态
	    E_LOG_DETAIL_EXPORT_STATUS,		  //导出状态
    }


    //public enum E_VDA_ACCESS_PTOTOCOL_TYPE
    //{
    //    E_VDA_ACCESS_PTOTOCOL_TYPE_NOUSE = 0,		// 其他协议类型
    //    E_VDA_ACCESS_PTOTOCOL_TYPE_RTSP_CLIENT,		// rtsp协议客户端接入库
    //    E_VDA_ACCESS_PTOTOCOL_TYPE_TCPIP_CLIENT,		// tcpip
    //    E_VDA_ACCESS_PTOTOCOL_TYPE_NETPOSA,				// 东方网力SDK接入
    //    E_VDA_ACCESS_PTOTOCOL_TYPE_HK,					// 海康SDK接入
    //    E_VDA_ACCESS_PTOTOCOL_TYPE_DH,				// 大华网力SDK接入
    //    E_VDA_ACCESS_PTOTOCOL_TYPE_SANYO_IPC,			// 三洋IPC
    //    E_VDA_ACCESS_PTOTOCOL_TYPE_BOCOM,				// VIS系统
    //    E_VDA_ACCESS_PTOTOCOL_TYPE_BKSP,				// 博康系统平台
    //    E_VDA_ACCESS_PTOTOCOL_TYPE_ONVIF,				// OnVif协议接入
    //    E_VDA_ACCESS_PTOTOCOL_TYPE_GB28181,				// GB28181协议接入
    //    E_VDA_ACCESS_PTOTOCOL_TYPE_SNMP,                //snmp协议
    //    E_VDA_ACCESS_PTOTOCOL_TYPE_H3C,				//华三sdk
    //    E_VDA_ACCESS_PTOTOCOL_TYPE_HKPLAT = 1024            //海康平台
    //};


    //网络存储设备接入协议类型
    public enum E_VDA_NET_STORE_DEV_PROTOCOL_TYPE
    {
        E_DEV_PROTOCOL_CONTYPE_NONE = -1,//OnVif协议接入

	    E_DEV_PROTOCOL_CONTYPE_ONVIF_PROTOCOL = 0,//OnVif协议接入
	    E_DEV_PROTOCOL_CONTYPE_GB28181_PROTOCOL,  //GB28181协议接入

	    E_DEV_PROTOCOL_CONTYPE_BCSYS_PLAT = 32,	  //博康系统
	    E_DEV_PROTOCOL_CONTYPE_H3C_PLAT,		  //华三平台
	    E_DEV_PROTOCOL_CONTYPE_HK_PLAT,			  //海康平台
	    E_DEV_PROTOCOL_CONTYPE_NETPOSA_PLAT,	  //东方网力平台
	    E_DEV_PROTOCOL_CONTYPE_HT_PLAT,			  //汇通平台

	    E_DEV_PROTOCOL_CONTYPE_HK_DEV = 1024,	  //海康设备接入
	    E_DEV_PROTOCOL_CONTYPE_DH_DEV,			  //大华设备接入
	    E_DEV_PROTOCOL_CONTYPE_SANYO_IPC_DEV	  //三洋IPC
    };
    #endregion

    #region 车辆检索相关结构体

    /// <summary>
    /// 车牌检索车型类型
    /// </summary>
    public enum E_VDA_SEARCH_VEHICLE_TYPE
    {
	    E_SEARCH_VEHICLE_TYPE_NOUSE	= 0,		//无效类型	
	    E_SEARCH_VEHICLE_TYPE_SMALL	= 1,		//小型车
	    E_SEARCH_VEHICLE_TYPE_MIDDLE	= 2,		//中型车
	    E_SEARCH_VEHICLE_TYPE_LARGE	= 3,		//大型车
	    E_SEARCH_VEHICLE_TYPE_OTHER	= 4,		//其他
    }

    /// <summary>
    /// 车牌检索车型细分
    /// </summary>
    public enum E_VDA_SEARCH_VEHICLE_DETAIL_TYPE
    {
	    E_SEARCH_VEHICLE_DETAIL_TYPE_NOUSE			= 0,	//无效类型	
	    E_SEARCH_VEHICLE_DETAIL_TYPE_LARGE_TRUCTK	= 1,	//大型货车
	    E_SEARCH_VEHICLE_DETAIL_TYPE_LARGE_BUS		= 2,	//大型客车
	    E_SEARCH_VEHICLE_DETAIL_TYPE_MIDDLE_BUS		= 3,	//中型客车
	    E_SEARCH_VEHICLE_DETAIL_TYPE_SMALL_BUS		= 4,	//小型客车
	    E_SEARCH_VEHICLE_DETAIL_TYPE_BIKE			= 5,	//两轮车
	    E_SEARCH_VEHICLE_DETAIL_TYPE_OTHER			= 6,	//其他
        E_SEARCH_VEHICLE_DETAIL_TYPE_SMALL_TRUCK = 7,	//其他
    }

    /// <summary>
    /// 车牌检索车身颜色
    /// </summary>
    public enum E_VDA_SEARCH_VEHICLE_COLOR_TYPE
    {
        E_SEARCH_VEHICLE_COLOR_TYPE_NOUSE = 0,		//无效类型	
        E_SEARCH_VEHICLE_COLOR_TYPE_WHITE = 1,		//白色
        E_SEARCH_VEHICLE_COLOR_TYPE_SILVER = 2,			//银色
        E_SEARCH_VEHICLE_COLOR_TYPE_BLACK = 3,		//黑色
        E_SEARCH_VEHICLE_COLOR_TYPE_RED = 4,			//红色
        E_SEARCH_VEHICLE_COLOR_TYPE_PURPLE = 5,			//紫色
        E_SEARCH_VEHICLE_COLOR_TYPE_BLUE = 6,		//蓝色
        E_SEARCH_VEHICLE_COLOR_TYPE_YELLOW = 7,			//黄色
        E_SEARCH_VEHICLE_COLOR_TYPE_GREEN = 8,		//绿色
        E_SEARCH_VEHICLE_COLOR_TYPE_BROWN = 9,		//褐色
        E_SEARCH_VEHICLE_COLOR_TYPE_PINK = 10,		//粉红色
        E_SEARCH_VEHICLE_COLOR_TYPE_GRAY = 11,		//灰色
        E_SEARCH_VEHICLE_COLOR_TYPE_OTHER = 12		//其它颜色
    }

    ///// <summary>
    ///// 车牌检索车身颜色
    ///// </summary>
    //public enum E_VDA_SEARCH_VEHICLE_COLOR_DETAIL_TYPE
    //{
    //    E_SEARCH_VEHICLE_COLOR_TYPE_NOUSE   = 0x00FFFFFF,		//全部	
    //    E_SEARCH_VEHICLE_COLOR_TYPE_WHITE = 0xFFFFFFFF,		//白色
    //    E_SEARCH_VEHICLE_COLOR_TYPE_SILVER = 0xFFC0C0C0,		//银色
    //    E_SEARCH_VEHICLE_COLOR_TYPE_BLACK = 0xFF000000,		//黑色
    //    E_SEARCH_VEHICLE_COLOR_TYPE_RED = 0xFFFF0000,		//红色
    //    E_SEARCH_VEHICLE_COLOR_TYPE_PURPLE = 0xFF800080,		//紫色
    //    E_SEARCH_VEHICLE_COLOR_TYPE_BLUE = 0xFF0000FF,		//蓝色
    //    E_SEARCH_VEHICLE_COLOR_TYPE_YELLOW = 0xFFFFFF00,		//黄色
    //    E_SEARCH_VEHICLE_COLOR_TYPE_GREEN = 0xFF008000,		//绿色
    //    E_SEARCH_VEHICLE_COLOR_TYPE_BROWN = 0xFFA52A2A,		//褐色
    //    E_SEARCH_VEHICLE_COLOR_TYPE_PINK = 0xFFFFC0CB,		//粉红色
    //    E_SEARCH_VEHICLE_COLOR_TYPE_GRAY = 0xFF808080,		//灰色
    //}

    /// <summary>
    /// 车牌检索车牌颜色
    /// </summary>
    public enum E_VDA_SEARCH_VEHICLE_PLATE_COLOR_TYPE
    {
	    E_SEARCH_VEHICLE_PLATE_COLOR_TYPE_NOUSE		= 0,		//无效类型	
	    E_SEARCH_VEHICLE_PLATE_COLOR_TYPE_BLUE		= 1,		//蓝色
	    E_SEARCH_VEHICLE_PLATE_COLOR_TYPE_BLACK		= 2,		//黑色
	    E_SEARCH_VEHICLE_PLATE_COLOR_TYPE_YELLOW	= 3,		//黄色
	    E_SEARCH_VEHICLE_PLATE_COLOR_TYPE_WHITE		= 4,		//白色	
	    E_SEARCH_VEHICLE_PLATE_COLOR_TYPE_OTHER		= 5,		//其他
    }

    ///// <summary>
    ///// 车牌检索车牌颜色
    ///// </summary>
    //public enum E_VDA_SEARCH_VEHICLE_PLATE_COLOR_DETAIL_TYPE
    //{
        //E_SEARCH_VEHICLE_PLATE_COLOR_TYPE_NOUSE = 0x00FFFFFF;		//全部	
        //E_SEARCH_VEHICLE_PLATE_COLOR_TYPE_BLUE = 0xFF0000FF,		//蓝色
        //E_SEARCH_VEHICLE_PLATE_COLOR_TYPE_BLACK = 0xFF000000,		//黑色
        //E_SEARCH_VEHICLE_PLATE_COLOR_TYPE_YELLOW = 0xFFFFFF00,		//黄色
        //E_SEARCH_VEHICLE_PLATE_COLOR_TYPE_WHITE = 0xFFFFFFFF,		//白色	
    //}

    /// <summary>
    /// 车牌检索车牌结构
    /// </summary>
    public enum E_VDA_SEARCH_VEHICLE_PLATE_STRUCT_TYPE
    {
        E_SEARCH_VEHICLE_PLATE_STRUCT_TYPE_NOUSE = 0,		//无效类型	
        E_SEARCH_VEHICLE_PLATE_STRUCT_TYPE_SINGLE = 1,		//其他
        E_SEARCH_VEHICLE_PLATE_STRUCT_TYPE_DOUBLE = 2,		//其他
        E_SEARCH_VEHICLE_PLATE_STRUCT_TYPE_OTHER = 3,		//其他
    }




    #endregion

    public class Common
    {
        #region 常量定义
        public const int DEFAULE_CASE_ID = 1;

        public const int VDA_MAX_IPADDR_LEN = 32;
        public const int VDA_MAX_MACADDR_LEN = 32;
        public const int VDA_MAX_NAME_LEN = 32;
        public const int VDA_MAX_PWD_LEN = VDA_MAX_NAME_LEN;
        public const int VDA_MAX_VERSION_LEN = 32;
        public const int VDA_MAX_CFGDATA_LEN = 256;
        public const int VDA_MAX_FILEPATH_LEN = 256;
        public const int VDA_MAX_URL_LEN = 512;
        public const int VDA_MAX_DESCRIPTION_INFO_LEN = 256;	//描述信息文本长度
        public const int VDASDK_MAX_ADDR_NAME_LEN = (2 * VDA_MAX_NAME_LEN);  //地址名称长度

        public const int VDA_MAX_TIME_LEN = 32;

        public const int MAXFILE_PERDIR = 64;

        public const int VDA_VIDEO_ANALYZE_TYPE_NUM = 4;	//视频算法分析类型数量
        public const int VDA_PIC_ANALYZE_TYPE_NUM = 2;	//图片算法分析类型数量

        public const int VDASDK_MAX_IPADDR_LEN = VDA_MAX_IPADDR_LEN;
        public const int VDASDK_MAX_MACADDR_LEN = VDA_MAX_MACADDR_LEN;
        
        public const int VDASDK_MAX_NAME_LEN = VDA_MAX_NAME_LEN;

        public const int VDASDK_MAX_NET_STORE_DEV_NAME = 128;
        
        public const int VDASDK_MAX_PWD_LEN = VDA_MAX_PWD_LEN;
        public const int VDASDK_MAX_FILEPATH_LEN = VDA_MAX_FILEPATH_LEN;
        public const int VDASDK_MAX_URL_LEN = VDA_MAX_URL_LEN;
        public const int VDASDK_MAX_DESCRIPTION_INFO_LEN = VDA_MAX_DESCRIPTION_INFO_LEN;

        public const int VDASDK_VIDEO_ANALYZE_TYPE_NUM = VDA_VIDEO_ANALYZE_TYPE_NUM;	//视频算法分析类型数量
        public const int VDASDK_PIC_ANALYZE_TYPE_NUM = VDA_PIC_ANALYZE_TYPE_NUM;	//图片算法分析类型数量
        public const int VDASDK_ANALYZE_TYPE_MAXNUM = VDASDK_VIDEO_ANALYZE_TYPE_NUM;	//最大分析类型数量

        public const int VDASDK_MAX_TASK_UNIT_NAME_LEN = 255;

        public const int VDA_PASSLINE_MAXNUM = 8;	                //越界线最大数量
        public const int VDA_ONE_BREAK_REGION_POINT_MAXNUM = 64;	        //单个区域边界点最大数量
        public const int VDA_BREAK_REGION_MAXNUM = 1;	            //闯入闯出区域最大数量


        public const int PDO_DRAW_RECT_MAXNUM = 8;	//绘制矩形的最大数量
        public const int PDO_FILEPATH_MAXLEN = 256;


        public static readonly DateTime ZEROTIME = new DateTime(1970, 1, 1).ToLocalTime();
        public static readonly DateTime MAXTIME = new DateTime(1970, 1, 1).ToLocalTime().AddSeconds(uint.MaxValue);

        public static readonly int APPERR_BVODS_BRIEF_OBJECT_NULL = 0x10000 + 1012;//没有摘要目标

        #endregion

    }
    public class SDKConstant
    {
        public static readonly string dwStartTimeS = "dwStartTimeS";

        public static readonly string dwEndTimeS = "dwEndTimeS";

        public static readonly string dwSearchObjType = "dwSearchObjType";

        public static readonly string bColorSearch = "bColorSearch";

        public static readonly string dwSearchObjRGB = "dwSearchObjRGB";

        public static readonly string dwColorSimilar = "dwColorSimilar";

        public static readonly string dwRangeFilterType = "dwRangeFilterType";

        public static readonly string ptSearchPassLineList = "ptSearchPassLineList";

        public static readonly string dwAlgorithmFilterType = "dwAlgorithmFilterType";

        public static readonly string CompareImage = "CompareImage";

        public static readonly string CompareImageRect = "CompareImageRect";
        /// <summary>
        /// 绊线数量
        /// </summary>
        public static readonly string dwPassLineNum = "dwPassLineNum";

        /// <summary>
        /// 闯入闯出区域, TVDASDK_SEARCH_BREAK_RE_SEARCH_BREAK_REGIONGION
        /// </summary>
        public static readonly string tSearchBreakRegion = "tSearchBreakRegion";

        public static readonly string dwVehicleDetailType = "dwVehicleDetailType";
        public static readonly string dwVehicleColor = "dwVehicleColor";
        public static readonly string dwVehicleLogo = "dwVehicleLogo";
        public static readonly string szVehiclePlateName = "szVehiclePlateName";
        public static readonly string dwVehiclePlateStruct = "dwVehiclePlateStruct";
        public static readonly string dwVehiclePlateColor = "dwVehiclePlateColor";

        //internal static TVDASDK_SEARCH_TARGET TVDASDK_SEARCH_TARGET_Empty =
        //    new TVDASDK_SEARCH_TARGET() { dwCameraID =0, dwTaskUnitID = 0};

        //internal static TVDASDK_SEARCH_RESULT_OBJ_INFO TVDASDK_SEARCH_RESULT_OBJ_INFO_Empty =
        //    new TVDASDK_SEARCH_RESULT_OBJ_INFO { };

        public static readonly string dwVehicleType = "dwVehicleType";

    }

}
