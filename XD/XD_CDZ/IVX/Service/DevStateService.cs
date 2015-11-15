using BOCOM.DataModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using XDTCPProtocol;

namespace BOCOM.IVX.Service
{
    public class DevStateService
    {
        XdTcpHelper xd;
        public event EventHandler OnSetParamRet;
        public event EventHandler OnGetParamRet;
        public event EventHandler OnSetChargePriceRet;
        public event EventHandler OnGetChargePriceRet;
        public event EventHandler OnSetDevID;
        public event EventHandler OnSetServiceState;
        public event EventHandler OnSetBlackList;
        public event EventHandler OnDevStateChanged;

        DataTable m_devStatTable;

        public CDZDevStatusInfo GetDevByID(string devID)
        {
            if (m_devStatTable == null)
                return null;
            DataRow r = m_devStatTable.Rows.Find(devID);
            if (r != null)
            {
                return new CDZDevStatusInfo()
                {
                    ChongDianShuChuDianLiu = (double)(r["ChongDianShuChuDianLiu"]),
                    ChongDianShuChuDianYa = (double)(r["ChongDianShuChuDianYa"]),
                    CRC = (ushort)r["CRC"],
                    DevID = r["DevID"].ToString(),
                    DevSoftVersion = r["DevSoftVersion"].ToString(),
                    DevType = (byte)r["DevType"],
                    FactoryID = r["FactoryID"].ToString(),
                    IsOnline = (byte)(((bool)r["IsOnline"])?1:0),
                    LianJieQueRenKaiGuanZhuangTai = (uint)(r["LianJieQueRenKaiGuanZhuangTai"]),
                    ServiceStat = (byte)r["ServiceStat"],
                    ShiFouLianJieDianChi = (bool)(r["ShiFouLianJieDianChi"]),
                    ShuChuJiDianQiZhuangTai = (bool)(r["ShuChuJiDianQiZhuangTai"]),
                    UserID = r["UserID"].ToString(),
                    WorkStat = (ushort)r["WorkStat"],
                    YouGongZongDianDu = (double)(r["YouGongZongDianDu"]),
                };
            }
            else
                return null;
        }
        public DataTable DevStatTable
        {
            get 
            {
                if (m_devStatTable == null)
                {
                    m_devStatTable = new DataTable("DevStat");
                    DataColumn col= m_devStatTable.Columns.Add("DevID");
                    m_devStatTable.PrimaryKey = new DataColumn[] { col };
                    m_devStatTable.Columns.Add("UserID");
                    m_devStatTable.Columns.Add("IsOnline",typeof(bool));
                    m_devStatTable.Columns.Add("ServiceStat", typeof(byte));
                    m_devStatTable.Columns.Add("ChongDianShuChuDianYa", typeof(double));
                    m_devStatTable.Columns.Add("ChongDianShuChuDianLiu", typeof(double));
                    m_devStatTable.Columns.Add("ShuChuJiDianQiZhuangTai", typeof(bool));
                    m_devStatTable.Columns.Add("LianJieQueRenKaiGuanZhuangTai", typeof(uint));
                    m_devStatTable.Columns.Add("ShiFouLianJieDianChi", typeof(bool));
                    m_devStatTable.Columns.Add("WorkStat", typeof(UInt16));
                    m_devStatTable.Columns.Add("DevType", typeof(byte));
                    m_devStatTable.Columns.Add("YouGongZongDianDu", typeof(double));
                    m_devStatTable.Columns.Add("FactoryID");
                    m_devStatTable.Columns.Add("DevSoftVersion");
                    m_devStatTable.Columns.Add("CRC", typeof(UInt16));

                    FillAllDevStat();

                }
                return m_devStatTable; 
            }
            set { m_devStatTable = value; }
        }

        private void FillAllDevStat()
        {
            if (m_devStatTable == null)
                return;

            m_devStatTable.Rows.Clear();

            string sms_sqlstr = "select * from hd_pile_info_t ";

            MySqlDataAdapter sms_da = new MySqlDataAdapter(sms_sqlstr, Framework.Environment.SMS_CONN);
            DataSet sms_ds = new DataSet();
            sms_da.Fill(sms_ds, "T");

            try
            {
                foreach (DataRow item in sms_ds.Tables[0].Rows)
                {
                    m_devStatTable.Rows.Add(item["dev_id"].ToString()//DevID
                               , item["user_id"].ToString()//UserID
                               , false//IsOnline
                               , 0//ServiceStat
                               , 0//ChongDianShuChuDianYa
                               , 0//ChongDianShuChuDianLiu
                               , false//ShuChuJiDianQiZhuangTai
                               , 0//LianJieQueRenKaiGuanZhuangTai
                               , false//ShiFouLianJieDianChi
                               , 0//WorkStat
                               , Convert.ToInt32( item["pile_type"].ToString())//DevType
                               , 0//YouGongZongDianDu
                               , item["vender_id"].ToString()//FactoryID
                               , item["software_ver"].ToString()//DevSoftVersion
                               , 0//CRC
                               );

                }
            }
            catch (Exception ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "获取全部设备");
            }
        }

        public void Start()
        {
            xd = new XdTcpHelper();
            if (xd.Open(Framework.Environment.TCPIP, Framework.Environment.TCPPORT))
            {
                xd.OnConnected += xd_OnConnected;
                xd.OnDisConnected += xd_OnDisConnected;
                xd.OnReceiveData += xd_OnReceiveData;
                //xd.Open("127.0.0.1", 9999);
                xd.OnReceiveLogin += xd_OnReceiveLogin;
                xd.OnReceiveGetDevChargeInfo += xd_OnReceiveGetDevChargeInfo;
                xd.OnReceiveGetDevVersion += xd_OnReceiveGetDevVersion;
                xd.OnReceiveNoteDevChargeStatus += xd_OnReceiveNoteDevChargeStatus;
                xd.OnReceiveNoteDevStatus += xd_OnReceiveNoteDevStatus;
                xd.OnReceiveSubscribrDevChargeStatus += xd_OnReceiveSubscribrDevChargeStatus;
                xd.OnReceiveSubscribrDevStatus += xd_OnReceiveSubscribrDevStatus;
                xd.OnReceiveSetDevParam += xd_OnReceiveSetDevParamRet;
                xd.OnReceiveGetDevParam += xd_OnReceiveGetDevParam;
                xd.OnReceiveGetChargePrice += xd_OnReceiveGetChargePrice;
                xd.OnReceiveSetChargePrice += xd_OnReceiveSetChargePrice;
                xd.OnReceiveSetBlackList += xd_OnReceiveSetBlackList;
                xd.OnReceiveSetDevID += xd_OnReceiveSetDevID;
                xd.OnReceiveSetServiceState += xd_OnReceiveSetServiceState;
                Login();
            }
            else
            {
                throw new Exception("无法连接管理服务器！");
 
            }
        }

        void xd_OnReceiveSetServiceState(SetServiceStateRet obj)
        {
            if (OnSetServiceState != null)
                OnSetServiceState(obj, null);
        }

        void xd_OnReceiveSetDevID(SetDevIDRet obj)
        {
            if (OnSetDevID != null)
                OnSetDevID(obj, null);
        }

        void xd_OnReceiveSetBlackList(SetDevBlackListRet obj)
        {
            if (OnSetBlackList != null)
                OnSetBlackList(obj, null);
        }

        void xd_OnReceiveSetChargePrice(SetChargePriceRet obj)
        {
            if (OnSetChargePriceRet != null)
                OnSetChargePriceRet(obj, null);
        }

        void xd_OnReceiveGetChargePrice(GetChargePriceRet obj)
        {
            if (OnGetChargePriceRet != null)
                OnGetChargePriceRet(obj, null);
        }

        void xd_OnReceiveGetDevParam(GetDevParamRet obj)
        {
            if (OnGetParamRet != null)
                OnGetParamRet(obj, null);
        }

        void xd_OnReceiveSetDevParamRet(SetDevParamRet obj)
        {
            if (OnSetParamRet != null)
                OnSetParamRet(obj, null);
        }
        void xd_OnReceiveData(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine(sender + System.Environment.NewLine);
        }

        void xd_OnReceiveSubscribrDevStatus(SubscribeDevStatusRet obj)
        {
        }

        void xd_OnReceiveSubscribrDevChargeStatus(SubscribeDevChargeStatusRet obj)
        {
        }

        void xd_OnReceiveNoteDevStatus(DevStatusNote obj)
        {
            DataRow row = m_devStatTable.Rows.Find(new string(obj.DevID));
            if (row == null)
            {
                row = m_devStatTable.Rows.Add(new string(obj.DevID)//DevID
                   , ""//UserID
                   , false//IsOnline
                   , 0//ServiceStat
                   , 0//ChongDianShuChuDianYa
                   , 0//ChongDianShuChuDianLiu
                   , false//ShuChuJiDianQiZhuangTai
                   , 0//LianJieQueRenKaiGuanZhuangTai
                   , false//ShiFouLianJieDianChi
                   , 0//WorkStat
                   , 0//DevType
                   , 0//YouGongZongDianDu
                   , ""//FactoryID
                   , ""//DevSoftVersion
                   , 0//CRC
                   );

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
            row["IsOnline"] = obj.IsOnline==1?true:false;
            row["ServiceStat"] = obj.ServiceStat;
            row["UserID"] = new string(obj.UserID);
        }

        void xd_OnReceiveNoteDevChargeStatus(DevChargeStatusNote obj)
        {
            DataRow row = m_devStatTable.Rows.Find(new string(obj.DevID));
            if (row == null)
            {
                row = m_devStatTable.Rows.Add(new string(obj.DevID)//DevID
                   , ""//UserID
                   , false//IsOnline
                   , 0//ServiceStat
                   , 0//ChongDianShuChuDianYa
                   , 0//ChongDianShuChuDianLiu
                   , false//ShuChuJiDianQiZhuangTai
                   , 0//LianJieQueRenKaiGuanZhuangTai
                   , false//ShiFouLianJieDianChi
                   , 0//WorkStat
                   , 0//DevType
                   , 0//YouGongZongDianDu
                   , ""//FactoryID
                   , ""//DevSoftVersion
                   , 0//CRC
                   );

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
            row["ChongDianShuChuDianLiu"] = obj.ChongDianShuChuDianLiu / 100d;
            row["ChongDianShuChuDianYa"] = obj.ChongDianShuChuDianYa / 10d;
            row["LianJieQueRenKaiGuanZhuangTai"] = obj.LianJieQueRenKaiGuanZhuangTai;
            row["ShiFouLianJieDianChi"] = obj.ShiFouLianJieDianChi == 1;
            row["ShuChuJiDianQiZhuangTai"] = obj.ShuChuJiDianQiZhuangTai == 1;
            row["WorkStat"] = obj.WorkStat;
            row["YouGongZongDianDu"] = obj.YouGongZongDianDu / 10d;
            if (OnDevStateChanged != null)
                OnDevStateChanged(new string(obj.DevID), null);
        }

        void xd_OnReceiveGetDevVersion(GetDevVersionRet obj)
        {
            DataRow row = m_devStatTable.Rows.Find(new string(obj.DevID));
            if (row == null)
            {
                row = m_devStatTable.Rows.Add(new string(obj.DevID)//DevID
                   , ""//UserID
                   , false//IsOnline
                   , 0//ServiceStat
                   , 0//ChongDianShuChuDianYa
                   , 0//ChongDianShuChuDianLiu
                   , false//ShuChuJiDianQiZhuangTai
                   , 0//LianJieQueRenKaiGuanZhuangTai
                   , false//ShiFouLianJieDianChi
                   , 0//WorkStat
                   , 0//DevType
                   , 0//YouGongZongDianDu
                   , ""//FactoryID
                   , ""//DevSoftVersion
                   , 0//CRC
                   );

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
            row["FactoryID"]= new string(obj.FactoryID);
            row["DevSoftVersion"] = new string(obj.DevSoftVersion);
            row["CRC"] = obj.CRC;
        }

        void xd_OnReceiveGetDevChargeInfo(GetDevChargeInfoRet obj)
        {
            DataRow row = m_devStatTable.Rows.Find(new string(obj.DevID));
            if (row == null)
            {
                m_devStatTable.Rows.Add(new string(obj.DevID)//DevID
                   , ""//UserID
                   , false//IsOnline
                   , 0//ServiceStat
                   , 0//ChongDianShuChuDianYa
                   , 0//ChongDianShuChuDianLiu
                   , false//ShuChuJiDianQiZhuangTai
                   , 0//LianJieQueRenKaiGuanZhuangTai
                   , false//ShiFouLianJieDianChi
                   , 0//WorkStat
                   , 0//DevType
                   , 0//YouGongZongDianDu
                   , ""//FactoryID
                   , ""//DevSoftVersion
                   , 0//CRC
                   );

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
            if (Framework.Environment.IsCloseMainForm)
                return;

            System.Diagnostics.Trace.WriteLine("xd_OnDisConnected :" + sender + System.Environment.NewLine);
            System.Threading.Thread.Sleep(30 * 1000);
            Start();
        }

        void xd_OnConnected(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("xd_OnConnected :" + sender + System.Environment.NewLine);
        }

        void xd_OnReceiveLogin(LoginRet obj)
        {
            float a = 0f;
            a = xd.Convert2Float(obj.ClientType, 3);
            
            if(obj.ret == 0)
                SubscibeDevStat();

            System.Diagnostics.Trace.WriteLine("xd_OnReceiveLogin ClientName:" + obj.ClientName + ",ClientType:" + obj.ClientType + ",ret:" + obj.ret + System.Environment.NewLine);
        }
        public void Stop()
        {
            if (xd != null)
            {
                xd.OnConnected -= xd_OnConnected;
                xd.OnDisConnected -= xd_OnDisConnected;
                xd.OnReceiveData -= xd_OnReceiveData;
                xd.OnReceiveLogin -= xd_OnReceiveLogin;
                xd.OnReceiveGetDevChargeInfo -= xd_OnReceiveGetDevChargeInfo;
                xd.OnReceiveGetDevVersion -= xd_OnReceiveGetDevVersion;
                xd.OnReceiveNoteDevChargeStatus -= xd_OnReceiveNoteDevChargeStatus;
                xd.OnReceiveNoteDevStatus -= xd_OnReceiveNoteDevStatus;
                xd.OnReceiveSubscribrDevChargeStatus -= xd_OnReceiveSubscribrDevChargeStatus;
                xd.OnReceiveSubscribrDevStatus -= xd_OnReceiveSubscribrDevStatus;
                xd.OnReceiveSetDevParam -= xd_OnReceiveSetDevParamRet;
                xd.OnReceiveGetDevParam -= xd_OnReceiveGetDevParam;
                xd.OnReceiveGetChargePrice -= xd_OnReceiveGetChargePrice;
                xd.OnReceiveSetChargePrice -= xd_OnReceiveSetChargePrice;
                xd.OnReceiveSetBlackList -= xd_OnReceiveSetBlackList;
                xd.OnReceiveSetDevID -= xd_OnReceiveSetDevID;
                xd.OnReceiveSetServiceState -= xd_OnReceiveSetServiceState;

                xd.Close();
                xd = null;
            }
        }

        //private List<CDZDevStatusInfo> m_CDZList = new List<CDZDevStatusInfo>();

        //public List<CDZDevStatusInfo> CDZList
        //{
        //    get { return m_CDZList; }
        //    set { m_CDZList = value; }
        //}

        private bool Login()
        {
            xd.SendLogin(Framework.Environment.UserName, Framework.Environment.UserType);
            return true;
        }

        private bool SubscibeDevStat()
        {
            xd.SendSubscribeDevStatus();
            xd.SendSubscribeDevChargeStatus();
            DataTable tb = DevStatTable;
            return true;
        }

        public bool SetDevParamDevAddr(string devID, Int16 addr)
        {
            SetDevParamReq msg = new SetDevParamReq()
            {
                DevID = devID.ToCharArray(XDTCPProtocol.Common.MAX_DEV_ID_LEN),
                DevAddr = addr,
                BackLightEnable = 0,
                ContrastEnable = 0,
                ControlEnable = 0,
                DevAddrEnable = 1,
                ELockEnable = 0,
                ModelEnable = 0,
                PasswordEnable = 0,
                RatioEnable = 0,
                StationEnable = 0,

            };
            xd.SendSetDevParam(msg);
            return true;
        }
        public bool SetDevParamStation(string devID, Int16 station)
        {
            SetDevParamReq msg = new SetDevParamReq()
            {
                DevID = devID.ToCharArray(XDTCPProtocol.Common.MAX_DEV_ID_LEN),

                Station = station,
                BackLightEnable = 0,
                ContrastEnable = 0,
                ControlEnable = 0,
                DevAddrEnable = 0,
                ELockEnable = 0,
                ModelEnable = 0,
                PasswordEnable = 0,
                RatioEnable = 0,
                StationEnable = 1,

            };
            xd.SendSetDevParam(msg);
            return true;
        }
        public bool SetDevParamControl(string devID, byte control)
        {
            SetDevParamReq msg = new SetDevParamReq()
            {
                DevID = devID.ToCharArray(XDTCPProtocol.Common.MAX_DEV_ID_LEN),

                Control = control,
                BackLightEnable = 0,
                ContrastEnable = 0,
                ControlEnable = 1,
                DevAddrEnable = 0,
                ELockEnable = 0,
                ModelEnable = 0,
                PasswordEnable = 0,
                RatioEnable = 0,
                StationEnable = 0,

            };
            xd.SendSetDevParam(msg);
            return true;
        }
        public bool SetDevParamELock(string devID, byte elock)
        {
            SetDevParamReq msg = new SetDevParamReq()
            {
                DevID = devID.ToCharArray(XDTCPProtocol.Common.MAX_DEV_ID_LEN),

                ELock = elock,
                BackLightEnable = 0,
                ContrastEnable = 0,
                ControlEnable = 0,
                DevAddrEnable = 0,
                ELockEnable = 1,
                ModelEnable = 0,
                PasswordEnable = 0,
                RatioEnable = 0,
                StationEnable = 0,

            };
            xd.SendSetDevParam(msg);
            return true;
        }
        public bool SetDevParamRatio(string devID, byte ratio)
        {
            SetDevParamReq msg = new SetDevParamReq()
            {
                DevID = devID.ToCharArray(XDTCPProtocol.Common.MAX_DEV_ID_LEN),

                Ratio = ratio,
                BackLightEnable = 0,
                ContrastEnable = 0,
                ControlEnable = 0,
                DevAddrEnable = 0,
                ELockEnable = 0,
                ModelEnable = 0,
                PasswordEnable = 0,
                RatioEnable = 1,
                StationEnable = 0,

            };
            xd.SendSetDevParam(msg);
            return true;
        }
        public bool SetDevParamPassword(string devID, string pass)
        {
            SetDevParamReq msg = new SetDevParamReq()
            {
                DevID = devID.ToCharArray(XDTCPProtocol.Common.MAX_DEV_ID_LEN),

                Password = pass.ToCharArray(XDTCPProtocol.Common.MAX_PASSWORD_LEN),
                BackLightEnable = 0,
                ContrastEnable = 0,
                ControlEnable = 0,
                DevAddrEnable = 0,
                ELockEnable = 0,
                ModelEnable = 0,
                PasswordEnable = 1,
                RatioEnable = 0,
                StationEnable = 0,

            };
            xd.SendSetDevParam(msg);
            return true;
        }
        public bool SetDevParamModel(string devID, byte model)
        {
            SetDevParamReq msg = new SetDevParamReq()
            {
                DevID = devID.ToCharArray(XDTCPProtocol.Common.MAX_DEV_ID_LEN),

                Model = model,
                BackLightEnable = 0,
                ContrastEnable = 0,
                ControlEnable = 0,
                DevAddrEnable = 0,
                ELockEnable = 0,
                ModelEnable = 1,
                PasswordEnable = 0,
                RatioEnable = 0,
                StationEnable = 0,

            };
            xd.SendSetDevParam(msg);
            return true;
        }
        public bool SetDevParamContrast(string devID, byte contrast)
        {
            SetDevParamReq msg = new SetDevParamReq()
            {
                DevID = devID.ToCharArray(XDTCPProtocol.Common.MAX_DEV_ID_LEN),

                Contrast = contrast,
                BackLightEnable = 0,
                ContrastEnable = 1,
                ControlEnable = 0,
                DevAddrEnable = 0,
                ELockEnable = 0,
                ModelEnable = 0,
                PasswordEnable = 0,
                RatioEnable = 0,
                StationEnable = 0,

            };
            xd.SendSetDevParam(msg);
            return true;
        }
        public bool SetDevParamBackLight(string devID, byte backLight)
        {
            SetDevParamReq msg = new SetDevParamReq()
            {
                DevID = devID.ToCharArray(XDTCPProtocol.Common.MAX_DEV_ID_LEN),

                BackLight = backLight,
                BackLightEnable = 1,
                ContrastEnable = 0,
                ControlEnable = 0,
                DevAddrEnable = 0,
                ELockEnable = 0,
                ModelEnable = 0,
                PasswordEnable = 0,
                RatioEnable = 0,
                StationEnable = 0,

            };
            xd.SendSetDevParam(msg);
            return true;
        }

        public bool SetDevParam(string devID, 
            Int16 addr, bool addrEnable,
            Int16 station, bool stationEnable,
            bool control, bool controlEnable,
            byte elock, bool elockEnable,
            float ratio, bool ratioEnable,
            string pass, bool passEnable,
            bool model, bool modelEnable,
            byte contrast, bool contrastEnable,
            byte backLight, bool backLightEnable
            )
        {
            SetDevParamReq msg = new SetDevParamReq()
            {
                DevID = devID.ToCharArray(XDTCPProtocol.Common.MAX_DEV_ID_LEN),
                DevAddr = addr,
                Station = station,
                Control = (byte)(control?1:0),
                ELock = elock,
                Ratio = Convert.ToInt16( ratio*100),
                Password = pass.ToCharArray(XDTCPProtocol.Common.MAX_PASSWORD_LEN),
                Model = (byte)(model?1:0),
                Contrast = contrast,
                BackLight = backLight,

                BackLightEnable = (byte)(backLightEnable ? 1 : 0),
                ContrastEnable = (byte)(contrastEnable ? 1 : 0),
                ControlEnable = (byte)(controlEnable ? 1 : 0),
                DevAddrEnable = (byte)(addrEnable?1:0),
                ELockEnable = (byte)(elockEnable ? 1 : 0),
                ModelEnable = (byte)(modelEnable ? 1 : 0),
                PasswordEnable = (byte)(passEnable ? 1 : 0),
                RatioEnable = (byte)(ratioEnable ? 1 : 0),
                StationEnable = (byte)(stationEnable ? 1 : 0),

            };
            xd.SendSetDevParam(msg);
            return true;

        }

        public bool GetDevParam(string devID)
        {
            xd.SendGetDevParam(devID);
            return true;
        }

        public bool GetChargePrice(string devID)
        {
            xd.SendGetChargePrice(devID);
            return true;
        }
        public bool SetChargePrice(string devID, float TaperPrice, float PeakPrice, float FlatPrice, float ValleyPrice)
        {
            xd.SendSetChargePrice(devID,
                Convert.ToUInt32( TaperPrice*100000),
                Convert.ToUInt32( PeakPrice*100000),
                Convert.ToUInt32( FlatPrice*100000),
                Convert.ToUInt32( ValleyPrice*100000));
            return true;
        }

        public bool SetServiceState(string devID, int serviceState)
        {
            xd.SendSetServiceState(devID, serviceState);
            return true;
        }

        public bool SetDevID(string devID, string newDevID)
        {
            xd.SendSetDevID(devID, newDevID);
            return true;
        }

        public bool SetBlackList(string devID, List<string> blackList)
        {
            xd.SendSetBlackList(devID, blackList);
            return true;
        }

        public void DeleteDev(string devID)
        {
            DataRow r = m_devStatTable.Rows.Find(devID);
            if (r != null)
                r.Delete();
        }
    }
}
