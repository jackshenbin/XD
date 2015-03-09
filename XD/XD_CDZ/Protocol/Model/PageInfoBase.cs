using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Protocol.Model
{
    public class PageInfoBase
    {
        public int Index { get; set; }

        public int CountPerPage { get; set; }

        public int CountInCurrentPage { get; set; }

        public int TotalPage { get; set; }

        public int TotalCount { get; set; }
    }
}
