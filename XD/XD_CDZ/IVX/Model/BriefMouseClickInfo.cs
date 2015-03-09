using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;

namespace BOCOM.IVX.Model
{
    public class BriefMouseClickInfo
    {
        public uint BriefHandle { get; set; } 
        public IntPtr HWnd { get; set; }
        public XtraSinglePlayer Player { get; set; }
        public E_VDA_BRIEF_WND_MOUSE_OPT_TYPE MouseClickType { get; set; } 
        public uint X { get; set; } 
        public uint Y { get; set; } 
        public uint UserData { get; set; } 

    }
}
