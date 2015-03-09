using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;

namespace BOCOM.IVX.Model
{
    public class LocalVAFileInfo
    {
        public string CameraName { get; set; }

        public uint CameraId { get; set; }

        public string AdjustTime { get; set; }

        public string FileName { get; set; }

        public E_VDA_ANALYZE_TYPE VAType { get; set; }

    }
}
