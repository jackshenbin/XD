using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.DataModel
{

    public class CDZDevStatusInfo
    {
        public string DevID { get; set; }//充电桩编号
        public string UserID { get; set; }//用户编号
        public byte IsOnline { get; set; }//在线状态 1：在线；0：离线
        public byte ServiceStat { get; set; }//服务状态 1：服务状态 2：暂停服务 3：维护状态 4：测试状态
        public double ChongDianShuChuDianYa { get; set; }//充电输出电压 精确到小数点后一位
        public double ChongDianShuChuDianLiu { get; set; }//充电输出电流 精确到小数点后二位
        public bool ShuChuJiDianQiZhuangTai { get; set; }//输出继电器状态 布尔型, 变化上传;0关（未输出），1开（输出）
        public uint LianJieQueRenKaiGuanZhuangTai { get; set; }//连接确认开关状态 布尔型, 变化上传;0关（未好），1开（好）
        public bool ShiFouLianJieDianChi { get; set; }//是否连接电池 布尔型, 变化上传，0：否，1：是
        public UInt16 WorkStat { get; set; }//工作状态 0离线，1故障 2待机，3工作
        public byte DevType { get; set; }//0x02：单相交流离散桩 0x12：三相相交流离散桩 0x03：三相直流离散桩 0x13：单相直流离散桩
        //public Int16 JiaoLiuShuRuDianYaUXiang { get; set; }//交流输入电压U相 精确到小数点后一位
        //public Int16 JiaoLiuShuRuDianYaVXiang { get; set; }//交流输入电压V相 精确到小数点后一位
        //public Int16 JiaoLiuShuRuDianYaWXiang { get; set; }//交流输入电压W相 精确到小数点后一位
        //public Int16 JiaoLiuShuChuDianYaUXiang { get; set; }//交流输出电压U相 精确到小数点后一位
        //public Int16 JiaoLiuShuChuDianYaVXiang { get; set; }//交流输出电压V相 精确到小数点后一位
        //public Int16 JiaoLiuShuChuDianYaWXiang { get; set; }//交流输出电压W相 精确到小数点后一位
        //public Int16 JiaoLiuShuChuDianLiu { get; set; }//交流输出电流 精确到小数点后二位
        //public Int16 ZhiLiuShuChuDianYa { get; set; }//直流输出电压 精确到小数点后一位
        //public Int16 ZhiLiuShuChuDianLiu { get; set; }//直流输出电流 精确到小数点后二位
        public double YouGongZongDianDu { get; set; }//有功总电度 精确到小数点后一位
        //public Int32 JianZongDianDu { get; set; }//尖总电度 精确到小数点后一位
        //public Int32 FengZongDianDu { get; set; }//峰总电度 精确到小数点后一位
        //public Int32 PingZongDianDu { get; set; }//平总电度 精确到小数点后一位
        //public Int32 GuZongDianDu { get; set; }//谷总电度 精确到小数点后一位
        public string FactoryID { get; set; }//厂商编号
        public string DevSoftVersion { get; set; }//软件版本号
        public ushort CRC { get; set; }//唯一CRC校验码
    }

}
