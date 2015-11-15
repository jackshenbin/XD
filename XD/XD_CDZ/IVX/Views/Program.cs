using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using BOCOM.IVX.Controls;
using System.Text;

namespace BOCOM.IVX
{
    static class Program2
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);


            //DevExpress.Skins.SkinManager.EnableFormSkins();
            //DevExpress.UserSkins.BonusSkins.Register();
            ////UserLookAndFeel.Default.SetSkinStyle("Metropolis Dark");
            //UserLookAndFeel.Default.SetSkinStyle("Darkroom");


            using (MainForm dlg = new MainForm())
            {
                Application.Run(dlg);
            }

        }
        static bool RegsvrVdaCutTimeLineOcx()
        {
            try
            {
                return (DllRegisterServerVdaCutTimeLine() >= 0);
            }
            catch
            {
                return false;
            }
        }

        [DllImport(@"lib\timetrack.ocx", EntryPoint = "DllRegisterServer")]
        public static extern int DllRegisterServerVdaCutTimeLine();


        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Framework.Environment.CheckMemeryUse();
            if (e.ExceptionObject is Exception)
            {
                MyLog4Net.Container.Instance.Log.Error("Application_ThreadException:", (Exception)e.ExceptionObject);

                MessageBox.Show(
                    string.Format("系统出现未处理异常："
                    + Environment.NewLine + "{0}"
                    + Environment.NewLine + Environment.NewLine + "请重新启动程序，如仍旧出现此对话框请联系管理员！", ((Exception)e.ExceptionObject).Message)
                    , "系统未处理异常"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Information);
            }
            else
            {
                MyLog4Net.Container.Instance.Log.Error("Application_ThreadException:无描述");

                MessageBox.Show(
                    string.Format("系统出现未处理异常："
                    + Environment.NewLine + "{0}"
                    + Environment.NewLine + Environment.NewLine + "请重新启动程序，如仍旧出现此对话框请联系管理员！", "无描述")
                    , "系统未处理异常"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Information);

            }
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
        
        // static public List<IMessageRet> FromList = new List<IMessageRet>();

    }
}