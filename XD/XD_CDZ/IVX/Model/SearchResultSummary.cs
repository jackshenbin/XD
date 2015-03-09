using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using BOCOM.IVX.ViewModel;
using BOCOM.IVX.Protocol;
using System.Drawing;
using System.Collections;
using System.Diagnostics;
using BOCOM.IVX.Controls;
// using BOCOM.IVX.Logic;
using BOCOM.IVX.Protocol.Model;
using System.Data;

namespace BOCOM.IVX.Model
{

    /// <summary>
    /// 检索条件对应的检索结果
    /// </summary>
    public class SearchResultSingleSummary
    {
        private int m_totleCount = 0;
        private int m_totlePage = 0;

        private int m_currPageIndex = 0;
        private uint m_taskUnitID = 0;
        private int m_currPageTotleCount = 0;
        private List<SearchResultRecord> m_searchResultList = new List<SearchResultRecord>();
        private uint m_searchSessionID = 0;
        private DataTable m_allResultList = null;


         public int TotlePage
        {
            get { return m_totlePage; }
            set { m_totlePage = value; }
        }
       public uint TaskUnitID
        {
            get { return m_taskUnitID; }
            set { m_taskUnitID = value; }
        }

       public List<SearchResultRecord> SearchResultList
        {
            get { return m_searchResultList; }
            set { m_searchResultList = value; }
        }

        public int CurrPageTotleCount
        {
            get { return m_currPageTotleCount; }
            set { m_currPageTotleCount = value; }
        }

        public int TotleCount
        {
            get { return m_totleCount; }
            set
            {
                m_totleCount = value;
            }
        }

        public int CurrPageIndex
        {
            get { return m_currPageIndex; }
            set
            {
                 m_currPageIndex = value;
            }
        }
        public uint SearchSessionID
        {
            get { return m_searchSessionID; }
            set { m_searchSessionID = value; }
        }
        public DataTable AllSearchResultList
        {
            get
            {
                if (m_allResultList == null)
                {
                    m_allResultList = new DataTable("AllResultLiat");
                    DataColumn ID = m_allResultList.Columns.Add("ID", typeof(UInt32));
                    m_allResultList.PrimaryKey = new DataColumn[] { ID };
                    m_allResultList.Columns.Add("TaskUnitID", typeof(UInt32));
                    m_allResultList.Columns.Add("CameraID");
                    m_allResultList.Columns.Add("FrameSeq");
                    m_allResultList.Columns.Add("TimeStamp");
                    m_allResultList.Columns.Add("TargetTs");
                    m_allResultList.Columns.Add("TargetAppearTs");
                    m_allResultList.Columns.Add("TargetDisappearTs");
                    m_allResultList.Columns.Add("Distance");
                    m_allResultList.Columns.Add("ThumbPicPath");
                    m_allResultList.Columns.Add("OrgPicPath");
                    m_allResultList.Columns.Add("ObjectRect");
                    m_allResultList.Columns.Add("ObjectType");
                    m_allResultList.Columns.Add("CardNum");
                    m_allResultList.Columns.Add("CarType");
                    m_allResultList.Columns.Add("CarLogo");
                    m_allResultList.Columns.Add("CarStruct");
                    m_allResultList.Columns.Add("CarColor1");
                    m_allResultList.Columns.Add("CarColor2");
                    m_allResultList.Columns.Add("CarColor3");
                    m_allResultList.Columns.Add("CardColor");
                    m_allResultList.Columns.Add("SearchResultRecord", typeof(SearchResultRecord));
                    FillAllResult();
                }
                return m_allResultList;
            }
            set { m_allResultList = value; }
        }

        private uint FillAllResult()
        {
            if (m_allResultList == null)
                return 0;

            m_allResultList.Rows.Clear();

            m_searchResultList.ForEach(ptInfo => AddRow(ptInfo));
            return (uint)m_searchResultList.Count;
        }

        private void AddRow(SearchResultRecord info)
        {
            m_allResultList.Rows.Add(info.ID,
                                    info.TaskUnitID,
                                    info.CameraID,
                                    info.FrameSeq,
                                    info.TimeStamp,
                                    info.TargetAppearTs,
                                    info.TargetDisappearTs,
                                    info.TargetDisappearTs,
                                    info.Distance,
                                    info.ThumbPicPath,
                                    info.OrgPicPath,
                                    info.ObjectRect,
                                    info.ObjectType,
                                    info.CardNum,
                                    info.CarType,
                                    info.CarLogo,
                                    info.CarStruct,
                                    info.CarColor1,
                                    info.CarColor2,
                                    info.CarColor3,
                                    info.CardColor,
                                    info);
        }
    }

    public class SearchResultSummary
    {
        private Dictionary<uint, SearchResultSingleSummary> dtSearchResultSingleSummaryList
            = new Dictionary<uint, SearchResultSingleSummary>();

        private uint m_searchSessionID = 0;
        private SearchPara m_SearchPara = null;
        private uint m_taskUnitID = 0;
        private SearchType m_searchType = SearchType.Normal;

        public Dictionary<uint, SearchResultSingleSummary> DtSearchResultSingleSummaryList
        {
            get { return dtSearchResultSingleSummaryList; }
            set { dtSearchResultSingleSummaryList = value; }
        }
        public SearchPara SearchPara
        {
            get { return m_SearchPara ?? new SearchPara(SearchType.Normal); }
            set { m_SearchPara = value; }
        }

        public uint TaskUnitID
        {
            get { return m_taskUnitID; }
            set { m_taskUnitID = value; }
        }


        public uint SearchSessionID
        {
            get { return m_searchSessionID; }
            set { m_searchSessionID = value; }
        }

        public SearchType SearchType
        {
            get { return m_searchType; }
            set { m_searchType = value; }
        }


    }

}
