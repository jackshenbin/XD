using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;


namespace XDTCPProtocol
{
    public class XdTcpHelper
    {
        static byte workflow = 0;

        TcpHelper m_tcpManager = new TcpHelper();

        bool m_isLogin = false;
        string m_userName = "";
        int m_userType = 0;

        public event EventHandler OnConnected;
        public event EventHandler OnDisConnected;
        public event EventHandler OnReceiveData;

        public event Action<HB> OnReceiveHB;
        public event Action<LoginRet> OnReceiveLogin;
        public event Action<SubscribeDevStatusRet> OnReceiveSubscribrDevStatus;
        public event Action<DevStatusNote> OnReceiveNoteDevStatus;
        public event Action<SubscribeDevChargeStatusRet> OnReceiveSubscribrDevChargeStatus;
        public event Action<DevChargeStatusNote> OnReceiveNoteDevChargeStatus;
        public event Action<GetDevChargeInfoRet> OnReceiveGetDevChargeInfo;
        public event Action<GetDevVersionRet> OnReceiveGetDevVersion;
        public event Action<SetDevParamRet> OnReceiveSetDevParam;
        public event Action<GetDevParamRet> OnReceiveGetDevParam;
        public event Action<SetChargePriceRet> OnReceiveSetChargePrice;
        public event Action<GetChargePriceRet> OnReceiveGetChargePrice;


        public void Open(string ip, int port)
        {
            m_tcpManager.OnConnectFail += m_tcpManager_OnConnectFail;
            m_tcpManager.OnConnectSuccess += m_tcpManager_OnConnectSuccess;
            m_tcpManager.OnReceiveData += m_tcpManager_OnReceiveData;
            m_tcpManager.NewTcpHelper(ip, port);
            //m_tcpManager.OnSendData += m_tcpManager_OnSendData;

        }

        public void Close()
        {
            m_isLogin = false;
            m_userName = "";
            m_userType = 0;
            m_tcpManager.Stop();
        }

        void m_tcpManager_OnReceiveData(object sender, TcpClientEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Len:" + e.DataSize + ",Data:");
            for (int i = 0; i < e.DataSize; i++)
            {
                sb.Append(e.Data[i].ToString("X2"));
            }
            if (OnReceiveData != null)
                OnReceiveData(sb.ToString(), null);

            int index = 0;
            while (true)
            {
                if (index > e.DataSize - 1)
                    break;

                if (e.Data[index] == 0x58 && e.Data[index + 1] == 0x44)
                {
                    IntPtr pdata = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(HEAD)));
                    Marshal.Copy(e.Data, 0, pdata, Marshal.SizeOf(typeof(HEAD)));
                    HEAD hd = (HEAD)Marshal.PtrToStructure(pdata, typeof(HEAD));
                    if (!CheckCRC(e.Data))
                    {
                        return;
                    }
                    int len = e.Data[index+3] + (e.Data[index+4] << 8) - 8;
                    byte[] body = new byte[len];
                    Array.Copy(e.Data, index+11, body, 0, len);
                    ProcessProtocol(hd, body);
                    index = index + 11 + len+2;
                }
                else
                {
                    index++; 
                }
            }
            
        }
        void ProcessProtocol(HEAD hd, byte[] body)
        {
            switch ((EnumProtocolType)hd.messagetype)
            {
                case EnumProtocolType.RET_HEART_BEAT:
                    OnReceiveData_HB(body);
                    break;
                case EnumProtocolType.RET_USER_LOGIN:
                    OnReceiveData_LOGIN(body);
                    break;
                case EnumProtocolType.RET_SUBSCTIBR_DEV_STATUS:
                    OnReceiveData_SubscribrDevStatus(body);
                    break;
                case EnumProtocolType.NOTE_DEV_STATUS:
                    OnReceiveData_NoteDevStatus(body);
                    break;
                case EnumProtocolType.RET_SUBSCTIBR_DEV_CHARGE_STATUS:
                    OnReceiveData_SubscribrDevChargeStatus(body);
                    break;
                case EnumProtocolType.NOTE_DEV_CHARGE_STATUS:
                    OnReceiveData_NoteDevChargeStatus(body);
                    break;
                case EnumProtocolType.RET_GET_DEV_CHARGE_INFO:
                    OnReceiveData_GetDevChargeInfo(body);
                    break;
                case EnumProtocolType.RET_GET_DEV_VERSION:
                    OnReceiveData_GetDevVersion(body);
                    break;
                case EnumProtocolType.RET_SET_DEV_PARAM:
                    OnReceiveData_SetDevParam(body);
                    break;
                case EnumProtocolType.RET_GET_DEV_PARAM:
                    OnReceiveData_GetDevParam(body);
                    break;
                case EnumProtocolType.RET_GET_CHARGE_PRICE:
                    OnReceiveData_GetChargePrice(body);
                    break;
                case EnumProtocolType.RET_SET_CHARGE_PRICE:
                    OnReceiveData_SetChargePrice(body);
                    break;
                default:
                    break;
            }
        }

        private void OnReceiveData_SetChargePrice(byte[] body)
        {
            try
            {
                IntPtr pdata = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SetChargePriceRet)));
                Marshal.Copy(body, 0, pdata, Marshal.SizeOf(typeof(SetChargePriceRet)));
                SetChargePriceRet msg = (SetChargePriceRet)Marshal.PtrToStructure(pdata, typeof(SetChargePriceRet));

                if (OnReceiveSetChargePrice != null)
                    OnReceiveSetChargePrice(msg);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
        }

        private void OnReceiveData_GetChargePrice(byte[] body)
        {
            try
            {
                IntPtr pdata = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(GetChargePriceRet)));
                Marshal.Copy(body, 0, pdata, Marshal.SizeOf(typeof(GetChargePriceRet)));
                GetChargePriceRet msg = (GetChargePriceRet)Marshal.PtrToStructure(pdata, typeof(GetChargePriceRet));

                if (OnReceiveGetChargePrice != null)
                    OnReceiveGetChargePrice(msg);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
        }

        private void OnReceiveData_GetDevVersion(byte[] body)
        {
            try
            {
                IntPtr pdata = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(GetDevVersionRet)));
                Marshal.Copy(body, 0, pdata, Marshal.SizeOf(typeof(GetDevVersionRet)));
                GetDevVersionRet msg = (GetDevVersionRet)Marshal.PtrToStructure(pdata, typeof(GetDevVersionRet));

                if (OnReceiveGetDevVersion != null)
                    OnReceiveGetDevVersion(msg);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
        }

        private void OnReceiveData_SetDevParam(byte[] body)
        {
            try
            {
                IntPtr pdata = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SetDevParamRet)));
                Marshal.Copy(body, 0, pdata, Marshal.SizeOf(typeof(SetDevParamRet)));
                SetDevParamRet msg = (SetDevParamRet)Marshal.PtrToStructure(pdata, typeof(SetDevParamRet));

                if (OnReceiveSetDevParam != null)
                    OnReceiveSetDevParam(msg);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
        }
        private void OnReceiveData_GetDevParam(byte[] body)
        {
            try
            {
                IntPtr pdata = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(GetDevParamRet)));
                Marshal.Copy(body, 0, pdata, Marshal.SizeOf(typeof(GetDevParamRet)));
                GetDevParamRet msg = (GetDevParamRet)Marshal.PtrToStructure(pdata, typeof(GetDevParamRet));

                if (OnReceiveGetDevParam != null)
                    OnReceiveGetDevParam(msg);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
        }

        private void OnReceiveData_GetDevChargeInfo(byte[] body)
        {
            try
            {

                IntPtr pdata = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(GetDevChargeInfoRet)));
                Marshal.Copy(body, 0, pdata, Marshal.SizeOf(typeof(GetDevChargeInfoRet)));
                GetDevChargeInfoRet msg = (GetDevChargeInfoRet)Marshal.PtrToStructure(pdata, typeof(GetDevChargeInfoRet));

                if (OnReceiveGetDevChargeInfo != null)
                    OnReceiveGetDevChargeInfo(msg);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
        }

        private void OnReceiveData_NoteDevChargeStatus(byte[] body)
        {
            try
            {

                IntPtr pdata = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(DevChargeStatusNote)));
                Marshal.Copy(body, 0, pdata, Marshal.SizeOf(typeof(DevChargeStatusNote)));
                DevChargeStatusNote msg = (DevChargeStatusNote)Marshal.PtrToStructure(pdata, typeof(DevChargeStatusNote));

                if (OnReceiveNoteDevChargeStatus != null)
                    OnReceiveNoteDevChargeStatus(msg);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
        }

        private void OnReceiveData_SubscribrDevChargeStatus(byte[] body)
        {
            try
            {

                IntPtr pdata = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SubscribeDevChargeStatusRet)));
                Marshal.Copy(body, 0, pdata, Marshal.SizeOf(typeof(SubscribeDevChargeStatusRet)));
                SubscribeDevChargeStatusRet msg = (SubscribeDevChargeStatusRet)Marshal.PtrToStructure(pdata, typeof(SubscribeDevChargeStatusRet));

                if (OnReceiveSubscribrDevChargeStatus != null)
                    OnReceiveSubscribrDevChargeStatus(msg);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }

        }

        private void OnReceiveData_NoteDevStatus(byte[] body)
        {
            try
            {

                IntPtr pdata = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(DevStatusNote)));
                Marshal.Copy(body, 0, pdata, Marshal.SizeOf(typeof(DevStatusNote)));
                DevStatusNote msg = (DevStatusNote)Marshal.PtrToStructure(pdata, typeof(DevStatusNote));

                if (OnReceiveNoteDevStatus != null)
                    OnReceiveNoteDevStatus(msg);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }

        }

        private void OnReceiveData_SubscribrDevStatus(byte[] body)
        {
            try
            {

                IntPtr pdata = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SubscribeDevStatusRet)));
                Marshal.Copy(body, 0, pdata, Marshal.SizeOf(typeof(SubscribeDevStatusRet)));
                SubscribeDevStatusRet msg = (SubscribeDevStatusRet)Marshal.PtrToStructure(pdata, typeof(SubscribeDevStatusRet));

                if (OnReceiveSubscribrDevStatus != null)
                    OnReceiveSubscribrDevStatus(msg);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }

        }
        private void OnReceiveData_HB(byte[] data)
        {
            try
            {

                IntPtr pdata = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(HB)));
                Marshal.Copy(data, 0, pdata, Marshal.SizeOf(typeof(HB)));
                HB hb = (HB)Marshal.PtrToStructure(pdata, typeof(HB));

                if (OnReceiveHB != null)
                    OnReceiveHB(hb);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }

        }
        private void OnReceiveData_LOGIN(byte[] data)
        {
            try
            {

                IntPtr pdata = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(LoginRet)));
                Marshal.Copy(data, 0, pdata, Marshal.SizeOf(typeof(LoginRet)));
                LoginRet hb = (LoginRet)Marshal.PtrToStructure(pdata, typeof(LoginRet));

                if (hb.ret == 0)
                {
                    m_userName = new string(hb.ClientName);
                    m_userType = hb.ClientType;
                    m_isLogin = true;
                    new System.Threading.Thread(ThHBSend).Start();
                }
                if (OnReceiveLogin != null)
                    OnReceiveLogin(hb);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }

        }

        void ThHBSend()
        {
            int index = 0;
            while (m_isLogin)
            {
                System.Threading.Thread.Sleep(1000);
                index++;
                if (index % 10 == 0)
                    SendHB();
            }
        }

        void m_tcpManager_OnConnectFail(object sender, TcpClientEventArgs e)
        {
            m_isLogin = false;
            if (OnDisConnected != null)
                OnDisConnected(e.Description, null);
        }

        void m_tcpManager_OnConnectSuccess(object sender, TcpClientEventArgs e)
        {
            if (OnConnected != null)
                OnConnected(e.Description, null);
        }


        private void SendHB()
        {
            HB hb = new HB { ClientName = m_userName.ToCharArray(Common.MAX_NAME_LEN), ClientType = (byte)m_userType };
            //IntPtr phb = Marshal.AllocHGlobal(Marshal.SizeOf(hb));
            ////System.Net.IPAddress.HostToNetworkOrder()
            //Marshal.StructureToPtr(hb,phb,false);
            //byte[] bhb = new byte[Marshal.SizeOf(hb)];
            //Marshal.Copy(phb, bhb, 0, Marshal.SizeOf(hb));
            ////byte[] hb = new byte[] { 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x01 };
            //Marshal.FreeHGlobal(phb);
            Send(hb, typeof(HB), EnumProtocolType.REQ_HEART_BEAT);
            System.Diagnostics.Trace.WriteLine("SendHB");
        }

        public void SendLogin(string userName, int userType)
        {
            LoginReq login = new LoginReq { ClientName = userName.ToCharArray(Common.MAX_NAME_LEN), ClientType = (byte)userType, ClientMac = GetMacAddress().ToCharArray(Common.MAX_NAME_LEN) };
            //IntPtr plogin = Marshal.AllocHGlobal(Marshal.SizeOf(login));
            //Marshal.StructureToPtr(login, plogin, false);
            //byte[] blogin = new byte[Marshal.SizeOf(login)];
            //Marshal.Copy(plogin, blogin, 0, Marshal.SizeOf(login));
            //Marshal.FreeHGlobal(plogin);
            Send(login, typeof(LoginReq), EnumProtocolType.REQ_USER_LOGIN);
            System.Diagnostics.Trace.WriteLine("SendLogin username:" + userName + ",usertype:" + userType);

        }

        public void SendSubscribeDevStatus()
        {
            SubscribeDevStatusReq msg = new SubscribeDevStatusReq { Flag = 1 };
            Send(msg, typeof(SubscribeDevStatusReq), EnumProtocolType.REQ_SUBSCTIBR_DEV_STATUS);
            System.Diagnostics.Trace.WriteLine("SendSubscribeDevStatus");

        }

        public void SendSubscribeDevChargeStatus()
        {
            SubscribeDevChargeStatusReq msg = new SubscribeDevChargeStatusReq { Flag = 1 };
            Send(msg, typeof(SubscribeDevChargeStatusReq), EnumProtocolType.REQ_SUBSCTIBR_DEV_CHARGE_STATUS);
            System.Diagnostics.Trace.WriteLine("SendSubscribeDevChargeStatus");

        }

        public void SendGetDevChargeInfo(string devID)
        {
            GetDevChargeInfoReq msg = new GetDevChargeInfoReq { DevID = devID.ToCharArray(Common.MAX_DEV_ID_LEN) };
            Send(msg, typeof(GetDevChargeInfoReq), EnumProtocolType.REQ_GET_DEV_CHARGE_INFO);
            System.Diagnostics.Trace.WriteLine("SendGetDevChargeInfo devid:" + devID);
        }

        public void SendGetDevVersion(string devID)
        {
            GetDevVersionReq msg = new GetDevVersionReq { DevID = devID.ToCharArray(Common.MAX_DEV_ID_LEN) };
            Send(msg, typeof(GetDevVersionReq), EnumProtocolType.REQ_GET_DEV_VERSION);
            System.Diagnostics.Trace.WriteLine("SendGetDevVersion devid:" + devID);
        }

        public void SendSetDevParam(SetDevParamReq param)
        {
            Send(param, typeof(SetDevParamReq), EnumProtocolType.REQ_SET_DEV_PARAM);
            System.Diagnostics.Trace.WriteLine("SendSetDevParam devid:" + param.DevID);

        }
        public void SendGetDevParam(string devID)
        {
            GetDevParamReq msg = new GetDevParamReq { DevID = devID.ToCharArray(Common.MAX_DEV_ID_LEN) };
            Send(msg, typeof(GetDevParamReq), EnumProtocolType.REQ_GET_DEV_PARAM);
            System.Diagnostics.Trace.WriteLine("SendGetDevParam devid:" + devID);
        }

        public void SendGetChargePrice(string devID)
        {
            GetChargePriceReq msg = new GetChargePriceReq { DevID = devID.ToCharArray(Common.MAX_DEV_ID_LEN) };
            Send(msg, typeof(GetChargePriceReq), EnumProtocolType.REQ_GET_CHARGE_PRICE);
            System.Diagnostics.Trace.WriteLine("SendGetChargePrice devid:" + devID);
        }

        public void SendSetChargePrice(string devID,  UInt32 TaperPrice, UInt32 PeakPrice, UInt32 FlatPrice, UInt32 ValleyPrice)
        {
            SetChargePriceReq msg = new SetChargePriceReq
            {
                DevID = devID.ToCharArray(Common.MAX_DEV_ID_LEN),
                FlatPrice = FlatPrice,
                PeakPrice = PeakPrice,
                TaperPrice = TaperPrice,
                ValleyPrice = ValleyPrice,
            };
            Send(msg, typeof(SetChargePriceReq), EnumProtocolType.REQ_SET_CHARGE_PRICE);
            System.Diagnostics.Trace.WriteLine("SendSetChargePrice devid:" + devID);
        }


        private void Send(object st, Type sttype, EnumProtocolType protocoltype)
        {

            IntPtr plogin = Marshal.AllocHGlobal(Marshal.SizeOf(st));
            Marshal.StructureToPtr(st, plogin, false);
            byte[] body = new byte[Marshal.SizeOf(st)];
            Marshal.Copy(plogin, body, 0, Marshal.SizeOf(st));
            Marshal.FreeHGlobal(plogin);

            HEAD hd = CreateHeaderByProtocolType(protocoltype);
            hd.len = (short)(8 + body.Length);
            short crc = (short)((hd.len & 0xff) + ((hd.len >> 8) & 0xff) + hd.messagetype + hd.recvaddr + hd.recvtype + hd.sendaddr + hd.sendtype + hd.version + hd.workflow);
            for (int i = 0; i < body.Length; i++)
            {
                crc += (short)body[i];
            }
            //hd.len = System.Net.IPAddress.HostToNetworkOrder(hd.len);
            IntPtr phd = Marshal.AllocHGlobal(Marshal.SizeOf(hd));
            Marshal.StructureToPtr(hd, phd, false);
            byte[] protocol = new byte[Marshal.SizeOf(hd) + body.Length + 2];
            Marshal.Copy(phd, protocol, 0, Marshal.SizeOf(hd));
            Array.Copy(body, 0, protocol, Marshal.SizeOf(hd), body.Length);
            crc = System.Net.IPAddress.HostToNetworkOrder(crc);
            protocol[protocol.Length - 2] = (byte)((crc >> 8) & 0xff);
            protocol[protocol.Length - 1] = (byte)(crc & 0xff);
            Marshal.FreeHGlobal(phd);
            m_tcpManager.SendData(protocol);
            //System.Diagnostics.Trace.WriteLine(string.Format("�������ݣ�{0}", protocol));

        }
        /// <summary>
        /// ժҪ:����Э������,������ͷ
        /// </summary>
        /// <param name="ptype">Э������</param>
        /// <returns>Э�����ͷ</returns>
        private static HEAD CreateHeaderByProtocolType(EnumProtocolType ptype)
        {
            HEAD h = new HEAD();
            h.flag1 = 0x58;
            h.flag2 = 0x44;
            h.messagetype = (byte)ptype;
            h.version = 0x01;
            h.workflow = unchecked(workflow++);
            h.recvaddr = 0;
            h.recvtype = 0;
            h.sendaddr = 0;
            h.sendtype = 0;

            return h;
        }
        private bool CheckCRC(byte[] data)
        {
            return true;
        }

        private static string GetMacAddress()
        {
            string mac = "10BF483ED5AC";
            //System.Management.ManagementClass mc = new System.Management.ManagementClass("Win32_NetworkAdapterConfiguration");
            //System.Management.ManagementObjectCollection moc = mc.GetInstances();
            //foreach (System.Management.ManagementObject item in moc)
            //{
            //    if ((bool)item["IPEnable"])
            //    {
            //        mac = item["MacAddress"].ToString();
            //        break;
            //    }
            //}
            //moc = null;
            //mc = null;
            return mac;
        }

        public float Convert2Float(int val, uint sit = 1)
        {
            if (val == 0)
                return 0f;
            if (sit > 5)
                return 0f;
            double beichushu = Math.Pow(10, (double)sit);
            return (float)(val / beichushu);
        }


    }
    public static class IStringExtension
    {
        public static char[] ToCharArray(this string str, int len)
        {
            char[] ch = new char[len];
            Array.Clear(ch, 0, len);
            char[] temp = str.ToCharArray();
            if (temp.Length == len)
                return temp;
            else if (temp.Length > len)
            {
                Array.Copy(temp, ch, len);
                return ch;
            }
            else
            {
                Array.Copy(temp, ch, temp.Length);
                return ch;
            }
        }
    }
    public class Common
    {
        public const int MAX_NAME_LEN = 16;
        public const int MAX_DEV_ID_LEN = 16;
        public const int MAX_USER_ID_LEN = 32;
        public const int MAX_PASSWORD_LEN = 6;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HB
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.MAX_NAME_LEN)]
        public char[] ClientName;//���㲹0������Ӣ�ĺ�����
        public byte ClientType;//1��ϵͳ����Ա 2��������Ա 3���豸ά��Ա
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LoginReq
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.MAX_NAME_LEN)]
        public char[] ClientName;//���㲹0������Ӣ�ĺ�����
        public byte ClientType;//1��ϵͳ����Ա 2��������Ա 3���豸ά��Ա
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.MAX_NAME_LEN)]
        public char[] ClientMac;//�ͻ���MAC��ַ
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LoginRet
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.MAX_NAME_LEN)]
        public char[] ClientName;//���㲹0������Ӣ�ĺ�����
        public byte ClientType;//1��ϵͳ����Ա 2��������Ա 3���豸ά��Ա
        public byte ret;//0��ע��ɹ� 1��ע��ʧ�� 3���޴��û� 4��MAC��ַ����

    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SubscribeDevStatusReq
    {
        public byte Flag;//1�����ƣ�0���˶�
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SubscribeDevStatusRet
    {
        public byte retFlag;//0�����ƻ����˶��ɹ���1�����ƻ����˶�ʧ��
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DevStatusNote
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.MAX_DEV_ID_LEN)]
        public char[] DevID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.MAX_USER_ID_LEN)]
        public char[] UserID;//�û����
        public byte IsOnline;//����״̬ 1�����ߣ�0������
        public byte ServiceStat;//����״̬ 1������״̬ 2����ͣ���� 3��ά��״̬ 4������״̬
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SubscribeDevChargeStatusReq
    {
        public byte Flag;//1�����ƣ�0���˶�
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SubscribeDevChargeStatusRet
    {
        public byte retFlag;//0�����ƻ����˶��ɹ���1�����ƻ����˶�ʧ��

    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DevChargeStatusNote
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.MAX_DEV_ID_LEN)]
        public char[] DevID;//���׮���
        public UInt16 ChongDianShuChuDianYa;//��������ѹ ��ȷ��С�����һλ
        public UInt16 ChongDianShuChuDianLiu;//���������� ��ȷ��С������λ
        public byte ShuChuJiDianQiZhuangTai;//����̵���״̬ ������, �仯�ϴ�;0�أ�δ�������1���������
        public byte LianJieQueRenKaiGuanZhuangTai;//����ȷ�Ͽ���״̬ ������, �仯�ϴ�;0�أ�δ�ã���1�����ã�
        public UInt32 YouGongZongDianDu;//�й��ܵ�� ��ȷ��С�����һλ
        public byte ShiFouLianJieDianChi;//�Ƿ����ӵ�� ������, �仯�ϴ���0����1����
        public byte WorkStat;//����״̬ 0���ߣ�1���� 2������3����

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GetDevChargeInfoReq
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.MAX_DEV_ID_LEN)]
        public char[] DevID;//���׮���
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GetDevChargeInfoRet
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.MAX_DEV_ID_LEN)]
        public char[] DevID;//���׮���
        public byte DevType;//0x02�����ཻ����ɢ׮ 0x12�������ཻ����ɢ׮ 0x03������ֱ����ɢ׮ 0x13������ֱ����ɢ׮
        public Int16 JiaoLiuShuRuDianYaUXiang;//���������ѹU�� ��ȷ��С�����һλ
        public Int16 JiaoLiuShuRuDianYaVXiang;//���������ѹV�� ��ȷ��С�����һλ
        public Int16 JiaoLiuShuRuDianYaWXiang;//���������ѹW�� ��ȷ��С�����һλ
        public Int16 JiaoLiuShuChuDianYaUXiang;//���������ѹU�� ��ȷ��С�����һλ
        public Int16 JiaoLiuShuChuDianYaVXiang;//���������ѹV�� ��ȷ��С�����һλ
        public Int16 JiaoLiuShuChuDianYaWXiang;//���������ѹW�� ��ȷ��С�����һλ
        public Int16 JiaoLiuShuChuDianLiu;//����������� ��ȷ��С������λ
        public Int16 ZhiLiuShuChuDianYa;//ֱ�������ѹ ��ȷ��С�����һλ
        public Int16 ZhiLiuShuChuDianLiu;//ֱ��������� ��ȷ��С������λ
        public Int32 YouGongZongDianDu;//�й��ܵ�� ��ȷ��С�����һλ
        public Int32 JianZongDianDu;//���ܵ�� ��ȷ��С�����һλ
        public Int32 FengZongDianDu;//���ܵ�� ��ȷ��С�����һλ
        public Int32 PingZongDianDu;//ƽ�ܵ�� ��ȷ��С�����һλ
        public Int32 GuZongDianDu;//���ܵ�� ��ȷ��С�����һλ

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GetDevVersionReq
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.MAX_DEV_ID_LEN)]
        public char[] DevID;//���׮���
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GetDevVersionRet
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.MAX_DEV_ID_LEN)]
        public char[] DevID;//���׮���
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.MAX_NAME_LEN)]
        public char[] FactoryID;//���̱��
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.MAX_NAME_LEN)]
        public char[] DevSoftVersion;//����汾��
        public Int32 CRC;//ΨһCRCУ����
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SetDevParamReq
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.MAX_DEV_ID_LEN)]
        public char[] DevID;//���׮���
        public byte DevAddrEnable;//�豸��ַʹ�� 0-������Ч��1-���øò���
        public Int16 DevAddr;
        public byte StationEnable;					//վ����ַʹ��(0:��Ч1:��Ч)
        public Int16 Station;						//վ����ַ(Ĭ����0x000A)
        public byte ControlEnable;					//��������ʹ��(0:��Ч1:��Ч)
        public byte Control;							//�������� (0:��ʹ��1:ʹ��)
        public byte ELockEnable;						//������ʹ��(0:��Ч1:��Ч)
        public byte ELock;							//������ (0:ȫ������1:������2:��ͷ��3:ȫ����)
        public byte RatioEnable;					//ռ�ձ�ʹ��(0:��Ч1:��Ч)
        public Int16 Ratio;							//ռ�ձ� (��ȷ��С�������λ)
        public byte PasswordEnable;					//ά������ʹ��(0:��Ч1:��Ч)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.MAX_PASSWORD_LEN)]
        public char[] Password;						//ά������ (0~9ascii��)(6λ�����ַ��������һλ��0)
        public byte ModelEnable;					//�˻�ģʽʹ��(0:��Ч1:��Ч)
        public byte Model;							//�˻�ģʽ (0:����ģʽ1:�˻�ģʽ)
        public byte ContrastEnable;					//�Աȶ�ʹ��(0:��Ч1:��Ч)
        public byte Contrast;						//�Աȶ� (Ĭ����52)
        public byte BackLightEnable;				//����ʱ��ʹ��(0:��Ч1:��Ч)
        public byte BackLight;						//����ʱ�� (��λ����Ĭ����1����)

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SetDevParamRet
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.MAX_DEV_ID_LEN)]
        public char[] DevID;//���׮���
        public byte DevAddrEnable;//�豸��ַʹ�� 0-������Ч��1-���øò���
        public byte StationEnable;					//վ����ַʹ��(0���ɹ���1��ʧ��)
        public byte ControlEnable;					//��������ʹ��(0���ɹ���1��ʧ��)
        public byte ELockEnable;						//������ʹ��(0���ɹ���1��ʧ��)
        public byte RatioEnable;					//ռ�ձ�ʹ��(0���ɹ���1��ʧ��)
        public byte PasswordEnable;					//ά������ʹ��(0���ɹ���1��ʧ��)
        public byte ModelEnable;					//�˻�ģʽʹ��(0���ɹ���1��ʧ��)
        public byte ContrastEnable;					//�Աȶ�ʹ��(0���ɹ���1��ʧ��)
        public byte BackLightEnable;				//����ʱ��ʹ��(0���ɹ���1��ʧ��)

    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GetDevParamReq
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.MAX_DEV_ID_LEN)]
        public char[] DevID;//���׮���

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GetDevParamRet
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.MAX_DEV_ID_LEN)]
        public char[] DevID;//���׮���
        public Int16 DevAddr;
        public Int16 Station;						//վ����ַ(Ĭ����0x000A)
        public byte Control;							//�������� (0:��ʹ��1:ʹ��)
        public byte ELock;							//������ (0:ȫ������1:������2:��ͷ��3:ȫ����)
        public Int16 Ratio;							//ռ�ձ� (��ȷ��С�������λ)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.MAX_PASSWORD_LEN)]
        public char[] Password;						//ά������ (0~9ascii��)(6λ�����ַ��������һλ��0)
        public byte Model;							//�˻�ģʽ (0:����ģʽ1:�˻�ģʽ)
        public byte Contrast;						//�Աȶ� (Ĭ����52)
        public byte BackLight;						//����ʱ�� (��λ����Ĭ����1����)

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GetChargePriceRet
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.MAX_DEV_ID_LEN)]
        public char[] DevID;//���׮���
        public UInt32 TaperPrice;
        public UInt32 PeakPrice;
        public UInt32 FlatPrice;
        public UInt32 ValleyPrice;
    };
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SetChargePriceReq
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.MAX_DEV_ID_LEN)]
        public char[] DevID;//���׮���
        public UInt32 TaperPrice;
        public UInt32 PeakPrice;
        public UInt32 FlatPrice;
        public UInt32 ValleyPrice;
    };
    public struct GetChargePriceReq
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.MAX_DEV_ID_LEN)]
        public char[] DevID;//���׮���

    }

    public struct SetChargePriceRet
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Common.MAX_DEV_ID_LEN)]
        public char[] DevID;//���׮���
        public byte RetFlag;//�ɹ���ʶ 0���ɹ���1��ʧ��
    }

}
