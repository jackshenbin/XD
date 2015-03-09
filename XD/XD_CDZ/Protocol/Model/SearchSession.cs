using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Protocol.Model
{
    public class SearchSession
    {
        private SearchPara m_SearchPara;

        public SearchPara SearchPara
        {
            get
            {
                return m_SearchPara;
            }
        }

        public uint SearchId { get; set; }

        public SearchSession(SearchPara searchPara)
        {
            m_SearchPara = searchPara;
        }
    }
}
