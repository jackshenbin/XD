using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Protocol.Model;

namespace BOCOM.IVX.Model
{
    public class LogSearchResultSummary
    {
        public event EventHandler SelectedPageNumberChanged;
        public event EventHandler SortFieldChanged;

        private LogResInfoSum m_PageInfo;

        private LogReqInfo m_SearchPara;

        #region Properties

        public int PageNumber
        {
            get
            {
                return m_PageInfo.iPageNum;
            }
            set
            {
                if (m_PageInfo.iPageNum != value && value > 0 && value <= m_PageInfo.iPageCount)
                {
                    m_PageInfo.iPageNum = value;
                    m_SearchPara.iPageNum = value;
                    if (SelectedPageNumberChanged != null)
                    {
                        SelectedPageNumberChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        public bool IsSortASC
        {
            get
            {
                bool asc = m_SearchPara.iTypeOfSort == 0;
                return asc;
            }
            set
            {
                m_SearchPara.iTypeOfSort = value ? 0 : 1;
            }
        }

        public string SortFieldName
        {
            get
            {
                return m_SearchPara.szSortName;
            }
            set
            {
                m_SearchPara.szSortName = value;
            }
        }

        public LogReqInfo SearchPara
        {
            get
            {
                return m_SearchPara;
            }
        }

        public LogResInfoSum PageInfo
        {
            get
            {
                return m_PageInfo;
            }
        }

        public List<LogRecord> Records { get; set; }

        #endregion

        public void RaiseSortFieldChangeEvent()
        {
            if (SortFieldChanged != null)
            {
                m_SearchPara.iPageNum = 1;
                SortFieldChanged(this, EventArgs.Empty);
            }
        }

        public LogSearchResultSummary(LogReqInfo searchPara, LogResInfoSum pageInfo)
        {
            m_SearchPara = searchPara;
            m_PageInfo = pageInfo;
        }
    }
}
