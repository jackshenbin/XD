using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Protocol.Model
{
    public class SearchItem : SearchParaBase
    {
        public uint TaskUnitId { get; set; }

        public uint CameraId { get; set; }

    }
}
