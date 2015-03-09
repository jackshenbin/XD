using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Protocol.Model
{
    public class SearchParaBase
    {
        public PageInfoBase PageInfo { get; set; }

        public SortType SortType { get; set; }

        public SearchParaBase()
        {
            PageInfo = new PageInfoBase();
            SortType = Model.SortType.TimeAsc;
        }
    

    }
}
