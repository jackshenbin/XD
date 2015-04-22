using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using System.Xml;
using System.Windows.Forms;
using BOCOM.IVX.Controls;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using BOCOM.DataModel;
using BOCOM.IVX.Views;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace BOCOM.IVX.Framework
{
    public class Environment
    {
        #region Fields

        public enum E_PRODUCT_TYPE
        {
            RELEASE=0,
            NO_LOG=1,
            SH_PRODUCT=2,
            ONLY_BRIEF =3,
            WITH_BIG_DATA =4,
        }
        public static readonly string PROGRAM_NAME = "IVX";
        public static readonly E_PRODUCT_TYPE PRODUCT_TYPE = E_PRODUCT_TYPE.RELEASE;
        public static long MAX_VIRTUAL_SIZE = (long)1600 * 1024 * 1024;
        public static long MAX_PRIVATE_SIZE = (long)700 * 1024 * 1024;
        public static readonly int MAX_TASKUNIT_UPLOAD_COUNT = 50;


        private static Configuration s_Config;
        private static bool s_IsSearchMixedMode = false;
        private static string s_Version;
        private static LoginToken s_Token;
        private static bool s_RememberPassword = false;
        private static Image m_defaultImage = null;

        private static bool s_Loggedin;

        private static bool s_IsBeingLogout;

        private static Control s_MainControl;

        private static Dictionary<SearchType, SearchResultDisplayMode> s_DTSearchType2DisplayMode = new Dictionary<SearchType, SearchResultDisplayMode>();

        private static MySqlConnection m_sms_conn;
        private static string sms_dbconn = "Host={0};UserName=rootcdz;Password=rootcdz;Database=xd_data;Port=3306;CharSet=gbk;Allow Zero Datetime=true";

        #endregion

        #region Properties

        public static MySqlConnection SMS_CONN
        {
            get 
            {
                if (m_sms_conn == null)
                {
                    try
                    {
                        string sms_connstr = string.Format(sms_dbconn,Framework.Environment.ServerIP);
                        m_sms_conn = new MySqlConnection(sms_connstr);
                    }
                    catch
                    { }
                }
                return m_sms_conn;
            }
        }

        public static int UserID
        {
            get
            {
                return s_Token.UserID;
            }
            set
            {
                s_Token.UserID = value;
            }

        }

        public static int UserType
        {
            get
            {
                return s_Token.UserType;
            }
            set
            {
                s_Token.UserType = value;
            }

        }

        public static string ServerIP
        {
            get
            {
                return s_Token.ServerIP;
            }
            set
            {
                s_Token.ServerIP = value;
            }
        }

        public static string UserName 
        {
            get
            {
                return s_Token.UserName;
            }
            set
            {
                s_Token.UserName = value;
            }
        }

        public static string Password
        {
            get
            {
                return s_Token.Password;
            }
            set
            {
                s_Token.Password = value;
            }
        }


        public static bool IsLoggedIn
        {
            get
            {
                return s_Loggedin;
            }
        }

        public static string VideoSavePath { get; set; }

        public static bool SavePassword
        {
            get
            {
                return s_RememberPassword;
            }
            set
            {
                if (s_RememberPassword != value)
                {
                    s_RememberPassword = value;
                    if (!s_RememberPassword)
                    {
                        Password = string.Empty;
                    }
                }
            }
        }

        public static string PictureSavePath { get; set; }

        public static string VersionDetail
        {
            get
            {
                if (String.IsNullOrEmpty(s_Version))
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    Version v = assembly.GetName().Version;
                    string fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
                    string dateTime = (new DateTime(2000, 1, 1)).AddDays(v.Build).AddSeconds(v.Revision * 2).ToString("yyyyMMddHHmmss");
                    //dllimport加载dll非常耗时，启动时显示sdk版本，触发加载dll
                    //string sdkver = Protocol.IVXProtocol.GetSdkVersion();
                    s_Version = string.Format("Ver:{0} ({1})", fileVersion, dateTime);
                }
                return s_Version;
            }
        }
        public static string Version
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                return   FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
            }
        }

        public static string TCPIP { get; set; }

        public static int TCPPORT { get; set; }
        public static bool IsSearchMixedMode
        {
            get
            {
                return s_IsSearchMixedMode;
            }
            set
            {
                s_IsSearchMixedMode = value;
            }
        }

        public static string CurrentDirectory
        {
            get;
            set;
        }

        public static string RecentLoadVideoFolder
        {
            get
            {
                return s_Config.AppSettings.Settings["RecendLoadVideoFolder"].Value;
            }
            set
            {
                try
                {
                    s_Config.AppSettings.Settings["RecendLoadVideoFolder"].Value = value;
                    s_Config.Save();
                }
                catch (ConfigurationErrorsException ex)
                { }
            }
        }
        public static string IR_APP_ID
        {
            get
            {
                return s_Config.AppSettings.Settings["IR_APP_ID"].Value;
            }
        }
        public static string IR_APP_KEY
        {
            get
            {
                return s_Config.AppSettings.Settings["IR_APP_KEY"].Value;
            }
        }
        /// <summary>
        /// 表示是否正在登出 （注销）
        /// </summary>
        public static bool IsBeingLogout
        {
            get
            {
                return s_IsBeingLogout;
            }
            set
            {
                s_IsBeingLogout = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsCloseMainForm { get; set; }

        public static int DefaultPageIndex
        {
            get
            {
                return 0;
            }
        }
        public static Image DefaultImage
        {
            get
            {
                if (m_defaultImage == null)
                {
                    try
                    {
                        System.Reflection.Assembly asm = System.Reflection.Assembly.GetCallingAssembly();

                        string mainformimg = System.IO.Path.Combine(System.IO.Directory.GetParent(asm.Location).FullName, "mainpage.jpg");

                        m_defaultImage = Image.FromFile(mainformimg);
                    }
                    catch
                    {
                        m_defaultImage = new Bitmap(10, 10);
                    }
                }
                return m_defaultImage;
            }
        }

        //public static int DefaultCountPerPage
        //{
        //    get
        //    {
        //        int count = 25;

        //        if (SearchResultDisplayMode == DataModel.SearchResultDisplayMode.GridViewOneSearchItem)
        //        {
        //            count = 50;
        //        }
        //        else if (SearchResultDisplayMode == DataModel.SearchResultDisplayMode.ThumbNailAllSearchItem)
        //        {
        //            count = 10;
        //        }
        //        return count;
        //    }
        //}
        
        public static SortType SortType
        {
            get;
            set;
        }

        //public static SearchResultDisplayMode SearchResultDisplayMode
        //{
        //    get
        //    {
        //        return s_SearchResultDisplayMode;
        //    }
        //    set
        //    {
        //        s_SearchResultDisplayMode = value;
        //    }
        //}

        public static SearchResultDisplayMode GetDisplayMode(SearchType searchType)
        {
            return s_DTSearchType2DisplayMode[searchType];
        }

        public static void SetDisplayMode(SearchType searchType, SearchResultDisplayMode mode)
        {
            s_DTSearchType2DisplayMode[searchType] = mode;
        }

        //public static string GetWebFileService { set; get; }

        //public static string GetFtpFileService { set; get; }

        #endregion

        #region Constructors

        public static void Init()
        {
        }

        static Environment()
        {

            Trace.WriteLine("Entering Envionment static constructor ...");
            try
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                
                string logpath = asm.Location + ".config";
                Trace.WriteLine("ReadConfig:" + logpath);
                if (!File.Exists(logpath))
                {
                    File.WriteAllText(logpath, Properties.Resources.IVX_exe);
                }

                string path = asm.Location;
                path = Path.GetDirectoryName(path);
                CurrentDirectory = path;
                Trace.WriteLine("set CurrentDirectory:" + path);

                s_Config = ConfigurationManager.OpenExeConfiguration(asm.Location);

                int ntype = 0;
                string producttypepath = CurrentDirectory + "\\Resource.dll";
                if (System.IO.File.Exists(producttypepath))
                {
                    string strtype = System.IO.File.ReadAllText(producttypepath);
                    try
                    {
                        ntype = int.Parse(strtype);
                        if (ntype > Enum.GetNames(typeof(E_PRODUCT_TYPE)).Length - 1 || ntype < 0)
                            ntype = 0;
                    }
                    catch { }
                }
                PRODUCT_TYPE = (E_PRODUCT_TYPE)ntype;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                MyLog4Net.Container.Instance.Log.Error("OpenExeConfiguration error in Environment: ", ex);
            }

            s_Token = new LoginToken(0,string.Empty, string.Empty, string.Empty,0);

            Framework.Environment.ReadConfig();
            Trace.WriteLine("begin Vda_Initialize");

            Framework.Container.Instance.EvtAggregator.GetEvent<UserLoginEvent>().Subscribe(OnUserLoggedIn);
            Framework.Container.Instance.EvtAggregator.GetEvent<UserLogOutEvent>().Subscribe(OnUserLogout);
            Trace.WriteLine("end Environment");

        }
        #endregion

        #region Public helper functions
        public static bool CheckMemeryUse()
        {
            long VirtualMemorySize64 = Process.GetCurrentProcess().VirtualMemorySize64 ;
            long PrivateMemorySize64 = Process.GetCurrentProcess().PrivateMemorySize64 ;

            bool ret = false;
            if ((VirtualMemorySize64> Framework.Environment.MAX_VIRTUAL_SIZE
                && PrivateMemorySize64 > Framework.Environment.MAX_PRIVATE_SIZE)
                || VirtualMemorySize64> 1900 * 1024 * 1024)
            { ret = false; }
            else
            { ret = true; }

            MyLog4Net.Container.Instance.Log.DebugFormat("CheckMemeryUse ret={2},VirtualMemorySize64:{0},PrivateMemorySize64:{1}", VirtualMemorySize64, PrivateMemorySize64, ret);
            return ret;
        }
        public static int GetDefaultCountPerPage(bool multiSearchItem, SearchResultDisplayMode displayMode)
        {
            int count = 25;

            if (displayMode == DataModel.SearchResultDisplayMode.GridViewOneSearchItem)
            {
                count = 50;
            }
            else if (displayMode == DataModel.SearchResultDisplayMode.ThumbNailAllSearchItem)
            {
                //if (multiSearchItem)
                //{
                //    count = 10;
                //}
            }
            return count;
        }

        public static bool SaveConfig()
        {
            try
            {
                //XmlDocument xDoc = new XmlDocument();
                //Assembly asm = Assembly.GetExecutingAssembly();
                //xDoc.Load(asm.Location + ".config");
                //XmlNode xNode = xDoc.SelectSingleNode("//userSettings");
                //XmlElement xElemServerIP = (XmlElement)xNode.SelectSingleNode("//setting[@name='ServerIP']");
                //xElemServerIP.SelectSingleNode("value").InnerText = Framework.Environment.ServerIP;
                //XmlElement xElemUserName = (XmlElement)xNode.SelectSingleNode("//setting[@name='UserName']");
                //xElemUserName.SelectSingleNode("value").InnerText = Framework.Environment.UserName;
                //XmlElement xElemPassword = (XmlElement)xNode.SelectSingleNode("//setting[@name='Password']");
                //if (!Framework.Environment.SavePassword)
                //    Framework.Environment.Password = "";
                //xElemPassword.SelectSingleNode("value").InnerText = DesKey.Encrypt(Framework.Environment.Password);
                //XmlElement xElemShareSimulate = (XmlElement)xNode.SelectSingleNode("//setting[@name='ShareSimulate']");
                //xElemShareSimulate.SelectSingleNode("value").InnerText = Framework.Environment.ShareSimulate.ToString();
                //XmlElement xElemBvgShareSimulateType = (XmlElement)xNode.SelectSingleNode("//setting[@name='BvgShareSimulateType']");
                //xElemBvgShareSimulateType.SelectSingleNode("value").InnerText = Framework.Environment.BvgShareSimulateType.ToString();
                //XmlElement xElemBvgShareSimuPath = (XmlElement)xNode.SelectSingleNode("//setting[@name='BvgShareSimuPath']");
                //xElemBvgShareSimuPath.SelectSingleNode("value").InnerText = Framework.Environment.BvgShareSimuPath;
                //XmlElement xElemAcepedVideoFile = (XmlElement)xNode.SelectSingleNode("//setting[@name='AcepedVideoFile']");
                //xElemAcepedVideoFile.SelectSingleNode("value").InnerText = Framework.Environment.AcepedVideoFile;
                //XmlElement xElemVideoSavePath = (XmlElement)xNode.SelectSingleNode("//setting[@name='VideoSavePath']");
                //xElemVideoSavePath.SelectSingleNode("value").InnerText = Framework.Environment.VideoSavePath;
                //XmlElement xElemSavePassword = (XmlElement)xNode.SelectSingleNode("//setting[@name='SavePassword']");
                //xElemSavePassword.SelectSingleNode("value").InnerText = Framework.Environment.SavePassword.ToString();
                //XmlElement xElemPictureSavePath = (XmlElement)xNode.SelectSingleNode("//setting[@name='PictureSavePath']");
                //xElemPictureSavePath.SelectSingleNode("value").InnerText = Framework.Environment.PictureSavePath;
                //XmlElement xElemisNeedSearchPlay = (XmlElement)xNode.SelectSingleNode("//setting[@name='isNeedSearchPlay']");
                //xElemisNeedSearchPlay.SelectSingleNode("value").InnerText = Framework.Environment.isNeedSearchPlay.ToString();
                //XmlElement xElemFileUploadSize = (XmlElement)xNode.SelectSingleNode("//setting[@name='FileUploadSize']");
                //xElemFileUploadSize.SelectSingleNode("value").InnerText = Framework.Environment.FileUploadSize.ToString();
                //xDoc.Save(asm.Location + ".config");

                //try
                //{
                //    s_Config = ConfigurationManager.OpenExeConfiguration(asm.Location);
                //}
                //catch (Exception ex)
                //{
                //    MyLog4Net.Container.Instance.Log.Error("OpenExeConfiguration error in Environment: ", ex);
                //}

                return true;
            }
            catch
            {
                MessageBox.Show("配置文件损坏，系统参数可能无法保存。", Framework.Environment.PROGRAM_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }
        public static void ReadXDConfig()
        {
            string sms_sqlstr = "select * from xd_config";

            MySqlDataAdapter sms_da = new MySqlDataAdapter(sms_sqlstr, Framework.Environment.SMS_CONN);
            System.Data.DataSet sms_ds = new System.Data.DataSet();
            sms_da.Fill(sms_ds, "T");
            foreach (System.Data.DataRow item in sms_ds.Tables[0].Rows)
	        {
		        switch (item["type"].ToString())
	            {
                    case "block1Key":
                        RFIDREAD.protocol.block1Key = gethexFromeText(item["value"].ToString());
                        
                        break;
                    case "block16key":
                        RFIDREAD.protocol.block16Key = gethexFromeText(item["value"].ToString());
                        break;
                    case "block32key":
                        RFIDREAD.protocol.block32Key = gethexFromeText(item["value"].ToString());
                        break;
                    case "tagMode":
                        RFIDREAD.protocol.tagMode = int.Parse(item["value"].ToString());
                        break;
                    case "tcpip":
                        TCPIP = item["value"].ToString();
                        break;
                    case "tcpport":
                        TCPPORT = int.Parse(item["value"].ToString());
                        break;
		            default:
                        break;
	            }
	        }
        }
        static byte[] gethexFromeText(string s)
        {
            byte[] bs = new byte[s.Length/2];
            for (int i = 0; i < s.Length/2; i++)
            {
                string t = s.Substring(i*2, 2);
                bs[i] = byte.Parse(t, System.Globalization.NumberStyles.HexNumber);
                
            }
            
            return bs;
        }

        public static void ReadConfig()
        {
            //Framework.Environment.ServerIP = Properties.Settings.Default.ServerIP;
            //Framework.Environment.UserName = Properties.Settings.Default.UserName;
            //Framework.Environment.Password = DesKey.Decrypt(Properties.Settings.Default.Password.Trim());
            //Framework.Environment.BvgShareSimulateType = Properties.Settings.Default.BvgShareSimulateType;
            //Framework.Environment.ShareSimulate = Properties.Settings.Default.ShareSimulate;
            //Framework.Environment.BvgShareSimuPath = Properties.Settings.Default.BvgShareSimuPath;
            //Framework.Environment.AcepedVideoFile = Properties.Settings.Default.AcepedVideoFile;
            //Framework.Environment.PictureSavePath = Properties.Settings.Default.PictureSavePath;
            //Framework.Environment.CaseType = Properties.Settings.Default.CaseType;
            ////Framework.Environment.GetWebFileService = Properties.Settings.Default.GetWebFilesServiceURL;
            ////Framework.Environment.GetFtpFileService = Properties.Settings.Default.GetFtpFilesServiceURL;
            //Framework.Environment.MAX_VIRTUAL_SIZE = Properties.Settings.Default.MAX_VIRTUAL_SIZE*1024*1024;
            //Framework.Environment.MAX_PRIVATE_SIZE = Properties.Settings.Default.MAX_PRIVATE_SIZE*1024*1024;
            //if (!Directory.Exists(Framework.Environment.PictureSavePath))
            //{
            //    Framework.Environment.PictureSavePath = string.Empty;
            //}
            //if (string.IsNullOrEmpty(Framework.Environment.PictureSavePath))
            //{
            //    Framework.Environment.PictureSavePath = CurrentDirectory + @"\Download\PictureSavePath";
            //    if (!System.IO.Directory.Exists(Framework.Environment.PictureSavePath))
            //    {
            //        System.IO.Directory.CreateDirectory(Framework.Environment.PictureSavePath);
            //    }
            //}
            //Framework.Environment.VideoSavePath = Properties.Settings.Default.VideoSavePath;
            //if (!Directory.Exists(Framework.Environment.VideoSavePath))
            //{
            //    Framework.Environment.VideoSavePath = string.Empty;
            //}

            //if (string.IsNullOrEmpty(Framework.Environment.VideoSavePath))
            //{
            //    Framework.Environment.VideoSavePath = CurrentDirectory + @"\Download\VideoSavePath";
            //    if (!System.IO.Directory.Exists(Framework.Environment.VideoSavePath))
            //    {
            //        System.IO.Directory.CreateDirectory(Framework.Environment.VideoSavePath);
            //    }
            //}
            //Framework.Environment.SavePassword = Properties.Settings.Default.SavePassword;
            //Framework.Environment.isNeedSearchPlay = Properties.Settings.Default.isNeedSearchPlay;
            //Framework.Environment.FileUploadSize = Properties.Settings.Default.FileUploadSize;

            //if (Framework.Environment.FileUploadSize > 1000 || Framework.Environment.FileUploadSize < 10)
            //{
            //    Framework.Environment.FileUploadSize = 10;
            //}
        }

        public static Control GetMainControl(bool showNavigationBar)
        {
            if (s_MainControl == null)
            {
                //s_MainControl = new ucMain(showNavigationBar);
            }
            return s_MainControl;
        }

        public static void Reset()
        {
            if(s_MainControl!=null)
                s_MainControl.Dispose();
            s_MainControl = null;
            s_Loggedin = false;
        }

        #endregion

        #region Private helper functions

       private static void OnUserLoggedIn(LoginToken token)
        {
            s_Token = token;
            s_Loggedin = true;
            SaveConfig();

        }

        private static void OnUserLogout(object o)
        {
            s_Loggedin = false;
        }

        #endregion


    }
}
