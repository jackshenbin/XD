using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Framework;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace BOCOM.IVX.Service
{
    public class ResourceManagerService
    {
        private bool m_IsRunning = true;
        private object m_SynObjRunning = new object();


        public ResourceManagerService()
        {
 
        }



    }
}
