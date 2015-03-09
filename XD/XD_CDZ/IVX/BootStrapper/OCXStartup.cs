using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BOCOM.IVX.Framework;
using BOCOM.IVX.Protocol;

namespace BOCOM.IVX.BootStrapper
{
    public class OCXStartup : Startup
    {
        public override bool Start(string[] args)
        {
            string userName="";
            string password="";
            string ip="";
            ushort port=60012;
            uint timeout=10;
            uint userData=0;

            foreach (string c in args)
            {
                try
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("OCXStartup args item: {0}", c));
                    string[] keyvalue = c.Split(new char[] { '=' }, 2);
                    switch (keyvalue[0].ToLower())
                    {
                        case "username": userName = keyvalue[1]; break;
                        case "password": password = keyvalue[1]; break;
                        case "ip": ip = keyvalue[1]; break;
                        case "port": port = ushort.Parse( keyvalue[1]); break;
                        case "timeout": timeout = uint.Parse(keyvalue[1]); break;
                        case "userdata": userData = uint.Parse(keyvalue[1]); break;
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            bool bRet = false;
            //using (FormLogin dlg = new FormLogin())
            //{
            //    Application.Run(dlg);
            //}
            Framework.Environment.SavePassword = false;
            try
            {
                //bRet = Framework.Container.Instance.AuthenticationService.Login(userName, password, ip, port, timeout, userData);
                //if (Framework.Environment.IsLoggedIn)
                //{
                //    bRet = true;
                //}
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "登录");
            }

            return bRet;
        }
    }
}
