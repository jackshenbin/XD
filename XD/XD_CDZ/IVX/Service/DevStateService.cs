using BOCOM.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using XDTCPProtocol;

namespace BOCOM.IVX.Service
{
    public class DevStateService
    {
        XdTcpHelper xd;

        public void Start()
        {
            xd = new XdTcpHelper();
            xd.OnConnected += xd_OnConnected;
            xd.OnDisConnected += xd_OnDisConnected;
            xd.OnReceiveData += xd_OnReceiveData;
            //xd.Open("192.168.3.250", 5188);
            xd.Open("127.0.0.1", 9999);
            xd.OnReceiveLogin += xd_OnReceiveLogin;
            xd.OnReceiveGetDevChargeInfo += xd_OnReceiveGetDevChargeInfo;
            xd.OnReceiveGetDevVersion += xd_OnReceiveGetDevVersion;
            xd.OnReceiveNoteDevChargeStatus += xd_OnReceiveNoteDevChargeStatus;
            xd.OnReceiveNoteDevStatus += xd_OnReceiveNoteDevStatus;
            xd.OnReceiveSubscribrDevChargeStatus += xd_OnReceiveSubscribrDevChargeStatus;
            xd.OnReceiveSubscribrDevStatus += xd_OnReceiveSubscribrDevStatus;

            Login();

            SubscibeDevStat();
        }
        void xd_OnReceiveData(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine( sender + System.Environment.NewLine);
        }

        void xd_OnReceiveSubscribrDevStatus(SubscribeDevStatusRet obj)
        {
        }

        void xd_OnReceiveSubscribrDevChargeStatus(SubscribeDevChargeStatusRet obj)
        {
        }

        void xd_OnReceiveNoteDevStatus(DevStatusNote obj)
        {
            CDZDevStatusInfo cdz = m_CDZList.Find(item => item.DevID == new string(obj.DevID));
            if (cdz == null)
            {
                cdz = new CDZDevStatusInfo() { DevID = new string(obj.DevID), };
                m_CDZList.Add(cdz);
            }

            string msg = string.Format("xd_OnReceiveNoteDevStatus devID:{0}"
                    + ",IsOnline:{1}"
                    + ",ServiceStat:{2}"
                    + ",UserID:{3}"
                    , obj.DevID
                    , obj.IsOnline
                    , obj.ServiceStat
                    , obj.UserID
                    );
            System.Diagnostics.Trace.WriteLine(msg + System.Environment.NewLine);
            cdz.IsOnline = obj.IsOnline;
            cdz.ServiceStat = obj.ServiceStat;
            cdz.UserID = new string(obj.UserID);
        }

        void xd_OnReceiveNoteDevChargeStatus(DevChargeStatusNote obj)
        {
            CDZDevStatusInfo cdz = m_CDZList.Find(item => item.DevID == new string(obj.DevID));
            if (cdz == null)
            {
                cdz = new CDZDevStatusInfo() { DevID = new string(obj.DevID), };
                m_CDZList.Add(cdz);
            }

            string msg = string.Format("xd_OnReceiveNoteDevChargeStatus devID:{0}"
                + ",ChongDianShuChuDianLiu:{1}"
                + ",ChongDianShuChuDianYa:{2}"
                + ",LianJieQueRenKaiGuanZhuangTai:{3}"
                + ",ShiFouLianJieDianChi:{4}"
                + ",ShuChuJiDianQiZhuangTai:{5}"
                + ",WorkStat:{6}"
                + ",YouGongZongDianDu:{7}"
                , obj.DevID
                , obj.ChongDianShuChuDianLiu
                , obj.ChongDianShuChuDianYa
                , obj.LianJieQueRenKaiGuanZhuangTai
                , obj.ShiFouLianJieDianChi
                , obj.ShuChuJiDianQiZhuangTai
                , obj.WorkStat
                , obj.YouGongZongDianDu
                );
            System.Diagnostics.Trace.WriteLine(msg + System.Environment.NewLine);
            cdz.ChongDianShuChuDianLiu =  obj.ChongDianShuChuDianLiu/100d;
            cdz.ChongDianShuChuDianYa = obj.ChongDianShuChuDianYa/10d;
            cdz.LianJieQueRenKaiGuanZhuangTai = obj.LianJieQueRenKaiGuanZhuangTai==1;
            cdz.ShiFouLianJieDianChi = obj.ShiFouLianJieDianChi==1;
            cdz.ShuChuJiDianQiZhuangTai = obj.ShuChuJiDianQiZhuangTai==1;
            cdz.WorkStat = obj.WorkStat;
            cdz.YouGongZongDianDu = obj.YouGongZongDianDu/10d;
        }

        void xd_OnReceiveGetDevVersion(GetDevVersionRet obj)
        {
            CDZDevStatusInfo cdz = m_CDZList.Find(item => item.DevID == new string(obj.DevID));
            if (cdz == null)
            {
                cdz = new CDZDevStatusInfo() { DevID = new string(obj.DevID), };
                m_CDZList.Add(cdz);
            }

            string msg = string.Format("xd_OnReceiveGetDevVersion devID:{0}"
                + ",FactoryID:{1}"
                + ",DevSoftVersion:{2}"
                + ",CRC:{3}"
                , obj.DevID
                , obj.FactoryID
                , obj.DevSoftVersion
                , obj.CRC
                );
            System.Diagnostics.Trace.WriteLine(msg + System.Environment.NewLine);
            cdz.FactoryID = new string(obj.FactoryID);
            cdz.DevSoftVersion = new string(obj.DevSoftVersion);
            cdz.CRC = obj.CRC;

        }

        void xd_OnReceiveGetDevChargeInfo(GetDevChargeInfoRet obj)
        {
            CDZDevStatusInfo cdz = m_CDZList.Find(item => item.DevID == new string(obj.DevID));
            if (cdz == null)
            {
                cdz = new CDZDevStatusInfo() { DevID = new string(obj.DevID), };
                m_CDZList.Add(cdz);
            }

            string msg = string.Format("xd_OnReceiveGetDevChargeInfo devID:{0}"
                + ",DevType:{1}"
                + ",FengZongDianDu:{2}"
                + ",GuZongDianDu:{3}"
                + ",JianZongDianDu:{4}"
                + ",JiaoLiuShuChuDianLiu:{5}"
                + ",JiaoLiuShuChuDianYaUXiang:{6}"
                + ",JiaoLiuShuChuDianYaVXiang:{7}"
                + ",JiaoLiuShuChuDianYaWXiang:{8}"
                + ",JiaoLiuShuRuDianYaUXiang:{9}"
                + ",JiaoLiuShuRuDianYaVXiang:{10}"
                + ",JiaoLiuShuRuDianYaWXiang:{11}"
                + ",PingZongDianDu:{12}"
                + ",YouGongZongDianDu:{13}"
                + ",ZhiLiuShuChuDianLiu:{14}"
                + ",ZhiLiuShuChuDianYa:{15}"
                , obj.DevID
                , obj.DevType
                , obj.FengZongDianDu
                , obj.GuZongDianDu
                , obj.JianZongDianDu
                , obj.JiaoLiuShuChuDianLiu
                , obj.JiaoLiuShuChuDianYaUXiang
                , obj.JiaoLiuShuChuDianYaVXiang
                , obj.JiaoLiuShuChuDianYaWXiang
                , obj.JiaoLiuShuRuDianYaUXiang
                , obj.JiaoLiuShuRuDianYaVXiang
                , obj.JiaoLiuShuRuDianYaWXiang
                , obj.PingZongDianDu
                , obj.YouGongZongDianDu
                , obj.ZhiLiuShuChuDianLiu
                , obj.ZhiLiuShuChuDianYa
                );
            System.Diagnostics.Trace.WriteLine(msg + System.Environment.NewLine);
            //cdz.DevType = obj.DevType;
            //cdz.FengZongDianDu = obj.FengZongDianDu;
            //cdz.GuZongDianDu = obj.GuZongDianDu;
            //cdz.JianZongDianDu = obj.JianZongDianDu;
            //cdz.JiaoLiuShuChuDianLiu = obj.JiaoLiuShuChuDianLiu;
            //cdz.JiaoLiuShuChuDianYaUXiang = obj.JiaoLiuShuChuDianYaUXiang;
            //cdz.JiaoLiuShuChuDianYaVXiang = obj.JiaoLiuShuChuDianYaVXiang;
            //cdz.JiaoLiuShuChuDianYaWXiang = obj.JiaoLiuShuChuDianYaWXiang;
            //cdz.JiaoLiuShuRuDianYaUXiang = obj.JiaoLiuShuRuDianYaUXiang;
            //cdz.JiaoLiuShuRuDianYaVXiang = obj.JiaoLiuShuRuDianYaVXiang;
            //cdz.JiaoLiuShuRuDianYaWXiang = obj.JiaoLiuShuRuDianYaWXiang;
            //cdz.PingZongDianDu = obj.PingZongDianDu;
            //cdz.YouGongZongDianDu = obj.YouGongZongDianDu/10d;
            //cdz.ZhiLiuShuChuDianLiu = obj.ZhiLiuShuChuDianLiu;
            //cdz.ZhiLiuShuChuDianYa = obj.ZhiLiuShuChuDianYa;

        }

        void xd_OnDisConnected(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("xd_OnDisConnected :" + sender + System.Environment.NewLine);
        }

        void xd_OnConnected(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("xd_OnConnected :" + sender + System.Environment.NewLine);
        }

        void xd_OnReceiveLogin(LoginRet obj)
        {
            float a = 0f;
            a = xd.Convert2Float(obj.ClientType, 3);
            System.Diagnostics.Trace.WriteLine("xd_OnReceiveLogin ClientName:" + obj.ClientName + ",ClientType:" + obj.ClientType + ",ret:" + obj.ret + System.Environment.NewLine);
        }
        public void Stop()
        {
            if (xd != null)
            {
                xd.Close();
                xd = null;
            }
        }

        private List<CDZDevStatusInfo> m_CDZList = new List<CDZDevStatusInfo>();

        public List<CDZDevStatusInfo> CDZList
        {
            get { return m_CDZList; }
            set { m_CDZList = value; }
        }

        private bool Login()
        {
            xd.SendLogin(Framework.Environment.UserName, Framework.Environment.UserType);
            return true;
        }

        private bool SubscibeDevStat()
        {
            xd.SendSubscribeDevStatus();
            xd.SendSubscribeDevChargeStatus();
            return true;
        }
    }
}
