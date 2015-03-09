using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModel
{
    /// <summary>
    /// 输入总页数， 当前页（默认0），负责翻页，上一页、下一页是否可用等
    /// </summary>
    public class PageInfo : NotifyPropertyChangedModel
    {
        public event EventHandler SelectedPageNumberChanged;

        #region Fields

        private int m_TotalRecords;

        private int m_PageCount;

        private int m_CurrentPageCount;

        private int m_PageIndex;

        #endregion

        #region Properties

        public int TotalRecords
        {
            get
            {
                return m_TotalRecords;
            }
            set
            {
                if (m_TotalRecords != value)
                {
                    m_TotalRecords = value;
                    RaisePropertyChangedEvent("TotalRecords");
                }
            }
        }

        /// <summary>
        /// 页数
        /// </summary>
        public int Count
        {
            get { return m_PageCount; }
            set
            {
                if (m_PageCount != value)
                {
                    m_PageCount = value;
                    RaisePropertyChangedEvent("Count");
                }
            }
        }

        /// <summary>
        /// 当前页记录数
        /// </summary>
        public int CurrentPageCount
        {
            get { return m_CurrentPageCount; }
            set
            {
                if (m_CurrentPageCount != value)
                {
                    m_CurrentPageCount = value;
                    RaisePropertyChangedEvent("CurrentPageCount");
                }
            }
        }


        /// <summary>
        /// 从0开始计数
        /// </summary>
        public int Index
        {
            get { return m_PageIndex; }
            set
            {
                if (m_PageIndex != value)
                {
                    m_PageIndex = value;
                    RaisePropertyChangedEvent("Index");
                    RaisePropertyChangedEvent("DisplayIndex");
                    RaisePropertyChangedEvent("CanPageUp");
                    RaisePropertyChangedEvent("CanPageDown");
                    if (SelectedPageNumberChanged != null)
                    {
                        SelectedPageNumberChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// 用于显示， 从1开始计数
        /// </summary>
        public string DisplayIndex
        {
            get
            {
                return (m_PageIndex + 1).ToString();
            }
            set
            {
                
            }
        }

        /// <summary>
        /// 是否可以下一页
        /// </summary>
        public bool CanNextPage
        {
            get
            {
                return Index < Count - 1;
            }
            set
            {

            }
        }

        /// <summary>
        /// 是否可以上一页
        /// </summary>
        public bool CanPrePage
        {
            get
            {
                return Index < Count  && Index > 0;
            }
            set
            {

            }
        }

        #endregion

        #region Constructors

        public PageInfo(int pageCount, int totalRecords, int index = 0)
        {
            m_PageCount = pageCount;
            m_TotalRecords = totalRecords;
            m_PageIndex = index;
        }

        #endregion

        public void TurnNextPage()
        {
            if (CanNextPage)
            {
                ++Index;
            }
        }

        public void TurnPrePage()
        {
            if (CanPrePage)
            {
                --Index;
            }
        }

        public List<int> GetPageIds()
        {
            List<int> ids = new List<int>();

            for (int i = 1; i <= m_PageCount; i++)
            {
                ids.Add(i);
            }

            return ids;
        }
    }
}
