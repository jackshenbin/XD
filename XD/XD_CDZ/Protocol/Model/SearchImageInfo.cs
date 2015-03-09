using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Protocol.Model
{
    public class SearchImageInfo
    {
        public  System.Drawing.Image ptImageData;
        
        public uint dwImageSize;
        
        /// DWORD->unsigned int
        public uint dwCameraID;

        /// DWORD->unsigned int
        public uint dwTaskUnitID;

        /// DWORD->unsigned int
        public uint dwMoveObjID;

    }
}
