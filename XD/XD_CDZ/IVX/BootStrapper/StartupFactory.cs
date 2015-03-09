using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.BootStrapper
{
    public class StartupFactory
    {
        public static Startup GetStartup(object args)
        {
            Startup startup = null;
            if (args == null || (args is string[] && ((string[])args).Length == 0))
            {
                startup = new NormalStartup();
            }
            else if (args is string && args.ToString() == "OCX")
            {
                startup = new OCXStartup();
            }
            else if (args is string[] && ((string[])args).Length > 0)
            {
                startup = new CmdlineStartup((string[])args);
            }
            return startup;
        }
    }
}
