using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.DataModel
{
    public class StreamVAFileInfo
    {
        public string NetStoreDevIP { get; set; }
        public uint NetStoreDevPort { get; set; }
        public string NetStoreDevUserName { get; set; }
        public string NetStoreDevPassword { get; set; }
        public uint NetStoreDevType { get; set; }
        public string CameraName { get; set; }
        public string  CameraId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public UInt64 FileSize { get; set; }
        public bool VATypeObject { get; set; }
        public bool VATypeCar { get; set; }
        public bool VATypeFace { get; set; }
        public bool VATypeBrief { get; set; }
    }
}
