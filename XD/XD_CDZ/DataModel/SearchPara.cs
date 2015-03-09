using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.DataModel
{
    public class SearchPara : SearchParaBase, ICloneable
    {
        public event EventHandler CurrentSearchItemChanged;

        #region Fileds

        public static readonly DateTime ZEROTIME = new DateTime(1970, 1, 1).ToLocalTime();
        public static readonly DateTime MAXTIME = new DateTime(1970, 1, 1).ToLocalTime().AddSeconds(uint.MaxValue);

        private SearchResultDisplayMode m_DisplayMode;
        private TimeRange m_TimeRange;
        private FilterCondition m_FilterCondition;
        private int m_currentSearchItemIndex = 0;

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
                return SearchItems[m_currentSearchItemIndex];
            }
            //set
            //{
            //    if (m_CurrentSearchItem != value)
            //    {
            //        m_CurrentSearchItem = value;
            //        if (CurrentSearchItemChanged != null)
            //        {
            //            CurrentSearchItemChanged(this, EventArgs.Empty);
            //        }
            //    }
            //}
        }

        public int CurrentSearchItemIndex
        {
            get
            {
                return m_currentSearchItemIndex;
            }
            set
            {
                if (m_currentSearchItemIndex != value)
                {
                    m_currentSearchItemIndex = value;
                    if (CurrentSearchItemChanged != null)
                    {
                        CurrentSearchItemChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        public FilterCondition FilterCondition
        {
            get
            {
                return m_FilterCondition;
            }
            set
            {
                m_FilterCondition = value;
            }
        }
        
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

        /// <summary>
        ///  标示该检索是否有相似度信息
        /// </summary>
        public bool IsSimilaritySearch
        {
            get
            {
                bool bRet = false;
                if (SearchType == DataModel.SearchType.Normal)
                {
                    if (m_FilterCondition != null &&
                    m_FilterCondition[SDKConstant.bColorSearch] != null &&
                    (bool)m_FilterCondition[SDKConstant.bColorSearch])
                    {
                        bRet = true;
                    }
                }
                else if (SearchType == DataModel.SearchType.Compare)
                {
                    bRet = true;
                }
                return bRet;
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

        #region Constructors

        public SearchPara()
        {
            SearchItems = new List<SearchItem>();
            m_FilterCondition = new FilterCondition();
            m_TimeRange = new TimeRange() { DTStart = ZEROTIME, DTEnd = MAXTIME};
            SortType = SortType.TimeAsc;
            PageInfo = new PageInfoBase();
        }
        
        public SearchPara(SearchType searchType)
            : this()
        {
            SearchType = searchType;
        }

        #endregion

        public SearchItem GetSeachItem(uint id)
        {
            SearchItem searchItem = null;

            if (SearchItems != null)
            {
                foreach (SearchItem item in SearchItems)
                {
                    if (item.TaskUnitId == id)
                    {
                        searchItem = item;
                        break;
                    }
                }
            }

            return searchItem;
        }

        public void ResetSearchItemStatus()
        {
            //if (m_DisplayMode == SearchResultDisplayMode.ThumbNailAllSearchItem)
            //{
                SearchItems.ForEach(si => si.IsFinished = false);
            //}
            //else
            //{
            //    CurrentSearchItem.IsFinished = false;
            //}
        }

        public object Clone()
        {
            SearchPara searchParaNew = new SearchPara(SearchType)
            {
                DisplayMode = this.DisplayMode,
                SearchItems = this.SearchItems,
                CurrentSearchItemIndex = this.CurrentSearchItemIndex,
                SortType = this.SortType,
                StartTime = this.StartTime,
                EndTime = this.EndTime,
            };

            if (this.FilterCondition != null)
            {
                ICollection<string> keys = this.FilterCondition.GetAllKeys();
                if (keys != null)
                {
                    foreach (string key in keys)
                    {
                        searchParaNew[key] = this[key];
                    }
                }
            }

            return searchParaNew;
        }
    }

}
