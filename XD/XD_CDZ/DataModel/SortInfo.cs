using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.DataModel
{
    public class SortInfo
    {
        public string FieldName { get; set; }

        public bool IsAscend { get; set; }

        public E_VDA_SEARCH_SORT_TYPE SortType { get; set; }
    }
}
