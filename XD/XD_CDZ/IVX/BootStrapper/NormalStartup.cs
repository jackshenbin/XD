using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BOCOM.IVX.BootStrapper
{
    public class NormalStartup : Startup
    {
        public override bool Start()
        {
            bool bRet = false;

            while (true)
            {

                if (Framework.Environment.IsLoggedIn)
                {
                    using (MainForm dlg = new MainForm())
                    {
                            Application.Run(dlg);
                    }
                    if (!Framework.Environment.IsBeingLogout)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            return bRet;
        }

    }
}
