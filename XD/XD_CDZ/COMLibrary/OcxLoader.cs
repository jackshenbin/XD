﻿using BOCOM.DataModel;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Views;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BOCOM.IVX.COMLibrary
{
    [Guid("8F72B3C0-0DEF-4995-9D9C-A7108CDD21E1"),
    ProgId("XDSDK"),
    ClassInterface(ClassInterfaceType.None),
    ComDefaultInterface(typeof(iXDInterface)),
    ComSourceInterfaces(typeof(XDEvents)),
    ComVisible(true)]
    public partial class OcxLoader : UserControl, iXDInterface, IObjectSafety
    {

        #region Fields

        private ucXDMain OcxMain;
        private bool m_isInited = false;
        private const string REGEXPATTERN_USERNAME = "[a-zA-Z0-9_]{1,20}$";
        private const string REGEXPATTERN_PASSWORD = @"[a-zA-Z0-9`~!@#$%^&*()_+-={}|\[\]:"";'<>?,.]{1,20}$";


        #endregion

        #region Constructors

        public OcxLoader()
        {
            MyLog4Net.Container.Instance.Log.Debug("COMLibrary begin OcxLoader");

            AppDomain.CurrentDomain.SetData("OCXContainer", this);
            InitializeComponent();
            m_isInited = false;
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Framework.Container.Instance.Cleanup(); //重新初始化后无法获取配置数据
            MyLog4Net.Container.Instance.Log.Debug("COMLibrary finish OcxLoader");
            Trace.WriteLine("COMLibrary finish OcxLoader");

            BackColor = System.Drawing.Color.DarkBlue;
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Framework.Environment.CheckMemeryUse();
            MyLog4Net.Container.Instance.Log.Error("Application_ThreadException:", e.Exception);

            MessageBox.Show(
                string.Format("系统出现未处理异常："
                + Environment.NewLine + "{0}"
                + Environment.NewLine + Environment.NewLine + "请重新启动程序，如仍旧出现此对话框请联系管理员！", e.Exception.Message)
                , "系统未处理异常"
                , MessageBoxButtons.OK
                , MessageBoxIcon.Information);

        }

        #endregion

        #region Events,Handles

        #endregion


        #region Implementation

        #endregion

        #region IObjectSafety implementations

        private const string _IID_IDispatch = "{00020400-0000-0000-C000-000000000046}";
        private const string _IID_IDispatchEx = "{a6ef9860-c720-11d0-9337-00a0c90dcaa9}";
        private const string _IID_IPersistStorage = "{0000010A-0000-0000-C000-000000000046}";
        private const string _IID_IPersistStream = "{00000109-0000-0000-C000-000000000046}";
        private const string _IID_IPersistPropertyBag = "{37D84F60-42CB-11CE-8135-00AA004BB851}";

        private const int INTERFACESAFE_FOR_UNTRUSTED_CALLER = 0x00000001;
        private const int INTERFACESAFE_FOR_UNTRUSTED_DATA = 0x00000002;
        private const int S_OK = 0;
        private const int E_FAIL = unchecked((int)0x80004005);
        private const int E_NOINTERFACE = unchecked((int)0x80004002);

        private bool _fSafeForScripting = true;
        private bool _fSafeForInitializing = true;

        //void IObjectSafety.GetInterfaceSafetyOptions(int riid, out int supportedOptions, out int enabledOptions)
        //{
        //    supportedOptions = 1;
        //    enabledOptions = 2;
        //}

        //void IObjectSafety.SetInterfaceSafetyOptions(int riid, int optionsSetMask, int enabledOptions)
        //{
        //    //throw new NotImplementedException();
        //}

        int IObjectSafety.GetInterfaceSafetyOptions(ref Guid riid, ref int pdwSupportedOptions, ref int pdwEnabledOptions)
        {
            int Rslt = E_FAIL;

            string strGUID = riid.ToString("B");
            pdwSupportedOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER | INTERFACESAFE_FOR_UNTRUSTED_DATA;
            switch (strGUID)
            {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForScripting == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForInitializing == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_DATA;
                    break;
                default:
                    Rslt = E_NOINTERFACE;
                    break;
            }
            Rslt = S_OK;

            return Rslt;
        }

        int IObjectSafety.SetInterfaceSafetyOptions(ref Guid riid, int dwOptionSetMask, int dwEnabledOptions)
        {
            int Rslt = E_FAIL;
            string strGUID = riid.ToString("B");
            switch (strGUID)
            {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_CALLER) && (_fSafeForScripting == true))
                        Rslt = S_OK;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_DATA) && (_fSafeForInitializing == true))
                        Rslt = S_OK;
                    break;
                default:
                    Rslt = E_NOINTERFACE;

                    break;
            }
            Rslt = S_OK;
            return Rslt;
        }

        #endregion

        #region Event handlers

 

        #endregion

        #region Private helper functions


        private string MakeRetMsg(int errCode, string errDiscription, string retInfo="")
        {
            string ret = string.Format("<Ret>"
                + "<RetMsg><ErrorCode>{0}</ErrorCode><Description>{1}</Description></RetMsg>"
                + "<RetInfo>{2}</RetInfo>"
                + "</Ret>"
                , errCode
                , errDiscription
                , retInfo);
            return ret;
        }




        #endregion

        #region iIVXInterface implementations

        public string VdaUninitialization()
        {
            MyLog4Net.Container.Instance.Log.Debug("COMLibrary VdaUninitialization");
            if (!m_isInited)
                return MakeRetMsg(-1, "未初始化");

            try
            {
                this.Controls.Clear();
                OcxMain = null;
                Framework.Container.Instance.Cleanup();
                m_isInited = false;
                MyLog4Net.Container.Instance.Log.Debug("COMLibrary finish VdaUninitialization");
                return MakeRetMsg(0, "");
            }
            catch (SDKCallException ex)
            {
                m_isInited = false;

                MyLog4Net.Container.Instance.Log.Debug("COMLibrary finish VdaUninitialization error :ret=[" + ex.ErrorCode + "]" + ex.Message);
                return MakeRetMsg((int)ex.ErrorCode, ex.Message);

            }
        }

        public string VdaSwitchForm(int formType)
        {
            MyLog4Net.Container.Instance.Log.Debug("COMLibrary VdaSwitchForm ");

            if (!m_isInited)
                return MakeRetMsg(-1,"未初始化");

            bool ret = OcxMain.SwitchWnd((E_SWITCH_WND)formType);

            if (ret)
            {
                MyLog4Net.Container.Instance.Log.Debug("COMLibrary finish VdaSwitchForm ");
                return MakeRetMsg(0, "");
            }
            else
            {
                MyLog4Net.Container.Instance.Log.Debug("COMLibrary finish VdaSwitchForm error :ret=[-1] 用户未登录" );
                return MakeRetMsg(-1, "用户未登录");

            }
        }



        public string VdaInitialization(string serverIp, string userName, string password)
        {
            MyLog4Net.Container.Instance.Log.Debug("COMLibrary  VdaInitialization ");
            Trace.WriteLine(string.Format("COMLibrary  VdaInitialization {0} {1} {2}", serverIp, userName, password));
            OcxMain = new ucXDMain();
            OcxMain.Dock = DockStyle.Fill;


            try
            {
                bool ret = OcxMain.LoginWnd(serverIp,userName, password);
                this.Controls.Add(OcxMain);
                OcxMain.InitWnd();
                m_isInited = true;
                MyLog4Net.Container.Instance.Log.Debug("COMLibrary finish VdaInitialization ");
                return MakeRetMsg(0, "");
            }
            catch (Exception ex)
            {
                m_isInited = false;

                MyLog4Net.Container.Instance.Log.Debug("COMLibrary finish VdaInitialization error :ret=[-1]" + ex.Message);
                return MakeRetMsg(-1, ex.Message);

            }

        }

        public string VdaGetVersion()
        {
            MyLog4Net.Container.Instance.Log.Debug("COMLibrary  VdaGetVersion ");

            return MakeRetMsg(0, "", Framework.Environment.Version);

        }

        #endregion

    }

}

