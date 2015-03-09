using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Protocol.Model
{
    public class SearchPara : SearchParaBase
    {
        public event EventHandler CurrentSearchItemChanged;

        #region Fileds
        
        private SearchResultDisplayMode m_DisplayMode;
        private TimeRange m_TimeRange;
        private FilterCondition m_FilterCondition;

        private SearchItem m_CurrentSearchItem;

        #endregion

        #region Properties

        public SearchType SearchType
        {
            get;
            set;
        }

        public SearchResultDisplayMode DisplayMode
        {
            get
            {
                return m_DisplayMode;
            }
            set
            {
                m_DisplayMode = value;
            }
        }

        public uint CurrReqestTaskUnitID { get; set; }

        public List<SearchItem> SearchItems { get; set; }

        public SearchItem CurrentSearchItem
        {
            get
            {
                m_CurrentSearchItem = m_CurrentSearchItem?? SearchItems[0];
                return m_CurrentSearchItem;
            }
            set
            {
                if (m_CurrentSearchItem != value)
                {
                    m_CurrentSearchItem = value;
                    if (CurrentSearchItemChanged != null)
                    {
                        CurrentSearchItemChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        public FilterCondition FilterCondition { get; set; }
        
        public DateTime StartTime
        {
            get
            {
                return m_TimeRange.DTStart;
            }
            set
            {
                m_TimeRange.DTStart = value;
            }
        }

        public DateTime EndTime
        {
            get
            {
                return m_TimeRange.DTEnd;
            }
            set
            {
                m_TimeRange.DTEnd = value;
            }
        }

        public object this[string key]
        {
            get
            {
                return m_FilterCondition[key];
            }
            set
            {
                m_FilterCondition[key] = value;
            }
        }

        #endregion

        public SearchPara()
        {
            SearchItems = new List<SearchItem>();
            m_FilterCondition = new FilterCondition();
            m_TimeRange = new TimeRange() { DTStart = DateTime.MinValue, DTEnd = DateTime.MaxValue };
            SortType = Model.SortType.TimeAsc;
            PageInfo = new PageInfoBase();
        }
        
        public SearchPara(SearchType searchType)
            : this()
        {
            SearchType = searchType;
        }


    }

}
