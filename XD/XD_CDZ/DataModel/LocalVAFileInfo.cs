using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.DataModel
{
    public class VAFileInfo
    {
        public string CameraName { get; set; }

        public uint CameraId { get; set; }

        public string AdjustTime { get; set; }

        public string FileName { get; set; }

        public string FileFullName { get; set; }

        public UInt64 FileSize { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public VideoAnalyseInfo VideoAnalyzeInfo { get; set; }

        public bool VATypeObject { get; set; }

        public bool VATypeCar { get; set; }

        public bool VATypeFace { get; set; }

        public bool VATypeBrief { get; set; }
    }
}
