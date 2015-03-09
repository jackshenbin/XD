using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;

namespace BOCOM.IVX.Framework
{
    public class VAServerEvent : CompositePresentationEvent<VAServerMsg>
    {
    }

    public class VDSOfflineEvent : CompositePresentationEvent<VAServerMsg>
    {
    }

    public class ServerOfflineEvent : CompositePresentationEvent<VAServerMsg>
    {
    }

    public class UserReloginEvent : CompositePresentationEvent<VAServerMsg>
    {
    }

   public class VAServerMsg
    {
        public Int32 NCbType { get; set; }

        public Int32 NData1 { get; set; }

        public Int32 NData2 { get; set; }

        public Int32 NUserData { get; set; }
    }
}
