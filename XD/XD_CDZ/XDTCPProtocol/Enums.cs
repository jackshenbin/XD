using System;
using System.Collections.Generic;
using System.Text;

namespace XDTCPProtocol
{
    /// <summary>
    /// 摘要:协议类型
    /// </summary>
    public enum EnumProtocolType
    {
        REQ_USER_LOGIN = 0x0001, //用户登录
        RET_USER_LOGIN = 0x0081,
        REQ_HEART_BEAT = 0x0002,//心跳协议
        RET_HEART_BEAT = 0x0082,
        REQ_SUBSCTIBR_DEV_STATUS = 0x0003,//定制状态信息命令
        RET_SUBSCTIBR_DEV_STATUS = 0x0083,
        NOTE_DEV_STATUS = 0x0084,//充电桩状态数据
        REQ_SUBSCTIBR_DEV_CHARGE_STATUS = 0x0005,//定制充电实时监测数据 
        RET_SUBSCTIBR_DEV_CHARGE_STATUS = 0x0085,
        NOTE_DEV_CHARGE_STATUS = 0x0086,//充电实时监测数据
        REQ_GET_DEV_CHARGE_INFO = 0x0007,//获取充电桩充电信息
        RET_GET_DEV_CHARGE_INFO = 0x0087,
        REQ_GET_DEV_VERSION = 0x0008,//获取充电桩软件版本
        RET_GET_DEV_VERSION = 0x0088,

        REQ_GET_DEV_PARAM = 0x0009,//获取充电桩参数
        RET_GET_DEV_PARAM = 0x0089,
        REQ_SET_DEV_PARAM = 0x0021,
        RET_SET_DEV_PARAM = 0x00A1,

        REQ_GET_CHARGE_PRICE = 0x000A,//获取充电桩费率
        RET_GET_CHARGE_PRICE = 0x008A,
        REQ_SET_CHARGE_PRICE = 0x0022,
        RET_SET_CHARGE_PRICE = 0x00A2,

        TIMEOUT = 0xEEEE,
    }


}
