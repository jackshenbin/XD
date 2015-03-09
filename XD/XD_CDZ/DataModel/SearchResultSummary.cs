using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Collections;
using System.Diagnostics;
// using BOCOM.IVX.Logic;
using System.Data;

namespace BOCOM.DataModel
{

    /// <summary>
    /// 检索条件对应的检索结果
    /// </summary>
    public class SearchResultSingleSummary
    {
        #region Fields
        
        private int m_totalCount = 0;
        private int m_totalPage = 0;

        private int m_currPageIndex = 0;
        private uint m_taskUnitID = 0;
        private int m_currPageTotleCount = 0;
        private List<SearchResultRecord> m_searchResultList = new List<SearchResultRecord>();
        private uint m_searchSessionID = 0;
        private DataTable m_allResultList = null;

        #endregion

        #region Properties

        public int TotalPage
        {
            get { return m_totalPage; }
            set { m_totalPage = value; }
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

        public int CurrPageTotalCount
        {
            get { return m_currPageTotleCount; }
            set { m_currPageTotleCount = value; }
        }

        public int TotalCount
        {
            get { return m_totalCount; }
            set
            {
                m_totalCount = value;
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
                    m_allResultList.Columns.Add("PlateNO");
                    m_allResultList.Columns.Add("ThumbNailPic", typeof(Image));
                    m_allResultList.Columns.Add("VehicleType");
                    m_allResultList.Columns.Add("VehicleDetailType");
                    m_allResultList.Columns.Add("VehicleBrand");
                    m_allResultList.Columns.Add("VehiclePlateType");
                    m_allResultList.Columns.Add("VehicleBodyColor");
                    m_allResultList.Columns.Add("VehicleBodyColor1");
                    m_allResultList.Columns.Add("VehicleBodyColor2");
                    m_allResultList.Columns.Add("PlateColorName");
                    m_allResultList.Columns.Add("SearchResultRecord", typeof(SearchResultRecord));
                    FillAllResult();
                }
                return m_allResultList;
            }
            set { m_allResultList = value; }
        }

        public bool IsSimilaritySearch
        {
            get;
            set;
        }

        #endregion

        #region Private helper functions
        
        private uint FillAllResult()
        {
            if (m_allResultList == null)
                return 0;

            m_allResultList.Rows.Clear();
            if (m_searchResultList != null)
            {
                m_searchResultList.ForEach(ptInfo => AddRow(ptInfo));
                return (uint)m_searchResultList.Count;
            }
            return 0;
        }

        private void AddRow(SearchResultRecord info)
        {
            // 测试环境， 多个服务器使用同一个大数据， 会产生重复 Id 的SearchResultRecord， 需要过滤掉
            if (m_allResultList.Rows.Find(info.ID) == null)
            {
                m_allResultList.Rows.Add(info.ID,
                                         info.TaskUnitID,
                                         info.CameraID,
                                         info.ID,
                                         info.TimeStamp,
                                         info.TargetTs,
                                         info.TargetAppearTs,
                                         info.TargetDisappearTs,
                                         info.Similarate,
                                         info.ThumbPicPath,
                                         info.OrgPicPath,
                                         info.ObjectRect,
                                         info.ObjectTypeInfo,
                                         info.PlateNO,
                                         info.ThumbNailPic,
                                         info.VehicleTypeInfo,
                                         info.VehicleDetailTypeInfo,
                                         info.VehicleBrandInfo,
                                         info.VehiclePlateTypeInfo,
                                         info.VehicleBodyColor,
                                         info.VehicleBodyColor1,
                                         info.VehicleBodyColor2,
                                         info.PlateColorName,
                                         info);
                Debug.WriteLine(String.Format("{0}, VehicleType: {1}, VehicleDetailType: {2}",
                    info.PlateNO, info.VehicleType, info.VehicleDetailType));
            }
            else
            {
                Debug.Assert(false, string.Format("Duplicated SearchResultRecord", info.ID));
            }
        }

        #endregion

        public void ClearResult()
        {
            if (m_searchResultList != null)
            {
                foreach (SearchResultRecord record in m_searchResultList)
                {
                    record.Dispose();
                }
                m_searchResultList.Clear();
            }

            if (m_allResultList != null)
            {
                m_allResultList.Clear();
            }
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
