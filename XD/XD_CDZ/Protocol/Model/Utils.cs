using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Protocol.Model
{
    public class Utils
    {
        public static DateTime GetTime(ulong seconds)
        {
            DateTime dt = new DateTime((long)seconds * 1000);
            return dt;
        }
    }
}
