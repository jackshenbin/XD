using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.DataModel;
using System.Data;
using BOCOM.IVX.Protocol.Model;
using BOCOM.IVX.Framework;
using Microsoft.Practices.Prism.Events;
using DataModel;
using System.Drawing;
using System.Diagnostics;
using BOCOM.IVX.Protocol;
using System.Windows.Forms;
using BOCOM.IVX.Common;

namespace BOCOM.IVX.ViewModel.Search
{
    public class SearchResultViewModel : ViewModelBase
    {
        #region Events
        
        public event Action<SearchSession> SearchBegining;
        public event EventHandler SearchCloseing;
        public event Action<SearchResultSingleViewModel> SingleViewModelRemoved;

        #endregion

        #region Fields
        
        private SearchSession m_searchSession = new SearchSession(null);
        private SearchItem m_currSearchItem;

        private bool m_switchingSearchItem = false;

        private PageInfo m_searchItemPageInfo;

        private SearchResultSingleViewModel m_SelectedSingleVM;

        private bool m_HasSelectedResultRecord = false;

        private SearchType m_SearchType;

        private bool m_CanChangeDisplayMode = true;

        private System.Windows.Forms.Timer m_Timer;

        private Dictionary<SearchItem, SearchResultSingleViewModel> m_DTSearchItem2SearchResultSingleViewModel;
      
        #endregion

        #region Properties

        public SearchType SearchType
        {
            get { return m_SearchType; }
        }

        public SearchResultDisplayMode DisplayMode
        {
            get
            {
                return Framework.Environment.GetDisplayMode(m_SearchType);
            }
            private set
            {
                Framework.Environment.SetDisplayMode(m_SearchType, value);
            }
        }

        public bool CanChangeDisplayMode
        {
            get
            {
                return m_CanChangeDisplayMode;
            }
            set
            {
                if(m_searchSession == null || m_searchSession.SearchPara == null)
                {
                    return;
                }

                if (m_CanChangeDisplayMode != value)
                {
                    m_CanChangeDisplayMode = value;
                    RaisePropertyChangedEvent("CanChangeDisplayMode");

                    if (!m_CanChangeDisplayMode)
                    {
                        if (!m_Timer.Enabled)
                        {
                            m_Timer.Start();
                        }
                    }
                }
            }
        }

        public bool IsGridViewOneSearchItemDisplayMode
        {
            get
            {
                return DisplayMode == DataModel.SearchResultDisplayMode.GridViewOneSearchItem;
            }
            set
            {
                if (value && DisplayMode != DataModel.SearchResultDisplayMode.GridViewOneSearchItem)
                {
                    CanChangeDisplayMode = false;
                    DisplayMode = DataModel.SearchResultDisplayMode.GridViewOneSearchItem;
                    ResearchOnDisplayModeChanged();
                }
            }
        }

        public bool IsThumbNailOneSearchItemDisplayMode
        {
            get
            {
                return DisplayMode == DataModel.SearchResultDisplayMode.ThumbNailOneSearchItem;
            }
            set
            {
                if (value && DisplayMode != DataModel.SearchResultDisplayMode.ThumbNailOneSearchItem)
                {
                    CanChangeDisplayMode = false;

                    DisplayMode = DataModel.SearchResultDisplayMode.ThumbNailOneSearchItem;
                    ResearchOnDisplayModeChanged();
                }
            }
        }

        public bool IsThumNailAllSearchItemDisplayMode
        {
            get
            {
                return Framework.Environment.GetDisplayMode(m_SearchType) == DataModel.SearchResultDisplayMode.ThumbNailAllSearchItem;
            }
            set
            {
                if (value && DisplayMode != DataModel.SearchResultDisplayMode.ThumbNailAllSearchItem)
                {
                    CanChangeDisplayMode = false;
                    DisplayMode = DataModel.SearchResultDisplayMode.ThumbNailAllSearchItem;
                    ResearchOnDisplayModeChanged();
                }
            }
        }

        public SortType SortType
        {
            get
            {
                return Framework.Environment.SortType;
            }
            set
            {
                if (Framework.Environment.SortType != value)
                {
                    Framework.Environment.SortType = value;
                    m_searchSession.SearchPara.SortType = value;
                    //临时不支持检索结果后排序 Sort();
                }
            }
        }

        public PageInfo SearchItemPageInfo
        {
            get { return m_searchItemPageInfo; }
        }

        public bool CanNextVideo
        {
            get
            {
                bool bRet = false;

                if (m_searchSession != null && m_searchSession.SearchPara != null &&
                    m_searchSession.SearchPara.DisplayMode != DataModel.SearchResultDisplayMode.ThumbNailAllSearchItem)
                {
                    bRet = m_searchSession.SearchPara.CurrentSearchItemIndex < m_searchSession.SearchPara.SearchItems.Count - 1;
                }

                return bRet;
            }
        }

        public bool CanPreVideo
        {
            get
            {
                bool bRet = false;

                if (m_searchSession != null && m_searchSession.SearchPara != null &&
                    m_searchSession.SearchPara.DisplayMode != DataModel.SearchResultDisplayMode.ThumbNailAllSearchItem)
                {
                    bRet = m_searchSession.SearchPara.CurrentSearchItemIndex > 0;
                }

                return bRet;
            }
        }

        public SearchItem CurrSearchItem
        {
            get { return m_currSearchItem; }
            set { m_currSearchItem = value; }
        }

        /// <summary>
        /// 用户表示当前显示的检索资源项
        /// 取决于显示模式， 
        /// </summary>
        public SearchItem[] DisplaySearchItems
        {
            get
            {
                SearchItem[] items = new SearchItem[] { CurrSearchItem };

                if (IsThumNailAllSearchItemDisplayMode)
                {
                    items = m_searchSession.SearchPara.SearchItems.ToArray();
                }
                return items;
            }
        }

        public SearchSession SearchSession
        {
            get { return m_searchSession; }
            set { m_searchSession = value; }
        }

        public uint TaskUnitID
        {
            get { return m_currSearchItem.TaskUnitId; }
            set { m_currSearchItem.TaskUnitId = value; }
        }

        public SearchResultSingleViewModel SelectedSingleViewModel
        {
            get
            {
                return m_SelectedSingleVM;
            }
            internal set
            {
                if (m_SelectedSingleVM != value)
                {
                    m_SelectedSingleVM = value;
                }
                HasSelectedResultRecord = (m_SelectedSingleVM != null && m_SelectedSingleVM.SelectedResultRecord != null);
            }
        }

        public bool HasSelectedResultRecord
        {
            get 
            {
                return m_HasSelectedResultRecord;
            }
            set
            {
                if (HasSelectedResultRecord != value)
                {
                    m_HasSelectedResultRecord = value;
                    RaisePropertyChangedEvent("HasSelectedResultRecord");
                }
            }
        }

        public SearchResultSingleViewModel[] SearchResultSingleViewModels
        {
            get
            {
                return m_DTSearchItem2SearchResultSingleViewModel.Values.ToArray();
            }
        }

        #endregion

        #region Constructors
        
        public SearchResultViewModel(SearchType searchType)
        {
            m_SearchType = searchType;
            m_searchItemPageInfo = new PageInfo(0, 0, 0);
            m_DTSearchItem2SearchResultSingleViewModel = new Dictionary<SearchItem, SearchResultSingleViewModel>();
            m_searchItemPageInfo.SelectedPageNumberChanged += new EventHandler(SearchItemPageInfo_SelectedPageNumberChanged);
            
            Framework.Container.Instance.EvtAggregator.GetEvent<SearchBeginEvent>().Subscribe(OnSearchBegin, ThreadOption.PublisherThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitDeletedEvent>().Subscribe(OnTaskUnitDeleted, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.RegisterEventSubscriber(this);

            m_Timer = new Timer();
            m_Timer.Interval = 2000;
            m_Timer.Tick += new EventHandler(Timer_Tick);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            m_Timer.Stop();
            CanChangeDisplayMode = true;
        }
        
        #endregion

        private void Sort()
        {
            if (m_searchSession != null && m_searchSession.SearchPara != null)
            {
                try
                {
                    Framework.Container.Instance.VideoSearchService.StartSearch(m_searchSession.SearchPara);
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "检索排序");
                }
            }
        }

        private void AddSearchResultSingleViewModel(SearchItem item)
        {
            SearchResultSingleViewModel singleVM = new SearchResultSingleViewModel(m_searchSession, item);
            singleVM.SelectedRecordChanged += new EventHandler(singleVM_SelectedRecordChanged);
            m_DTSearchItem2SearchResultSingleViewModel.Add(item, singleVM);
        }

        private void ClearSingleViewModels()
        {
            foreach (SearchResultSingleViewModel singleVM in m_DTSearchItem2SearchResultSingleViewModel.Values)
            {
                ClearSingleViewModel(singleVM);
            }
            m_DTSearchItem2SearchResultSingleViewModel.Clear();

            SelectedSingleViewModel = null;
        }

        private void ClearSingleViewModel(SearchResultSingleViewModel singleVM)
        {
            singleVM.SelectedRecordChanged -= new EventHandler(singleVM_SelectedRecordChanged);
            singleVM.UnInit();
        }

        void singleVM_SelectedRecordChanged(object sender, EventArgs e)
        {
            if (m_SelectedSingleVM == sender)
            {
                HasSelectedResultRecord = (m_SelectedSingleVM != null && m_SelectedSingleVM.SelectedResultRecord != null);
            }

            // 之前 不是采用 TabPage时的代码， 多文件的结果可以同时分行看到， 选中一个文件的结果， 要清除之前的选中
            foreach (SearchResultSingleViewModel singleVM in m_DTSearchItem2SearchResultSingleViewModel.Values)
            {
                if (singleVM != SelectedSingleViewModel)
                {
                    singleVM.Selected = false;
                }
            }
        }

        #region Public helper functions
        
        public void Clear()
        {
            try
            {
                Framework.Container.Instance.VideoSearchService.CloseSearch(m_searchSession.SearchId);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "清除检索");
            }

            m_searchSession = new SearchSession(null) ;
            ClearSingleViewModels();

            m_currSearchItem = null;
            if (SearchCloseing != null)
            {
                SearchCloseing(null, null);
            }
        }
        
        public override void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<SearchBeginEvent>().Unsubscribe(OnSearchBegin);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitDeletedEvent>().Unsubscribe(OnTaskUnitDeleted);
        }

        public void GoToCompareSearch()
        {
            SearchResultRecord record = SelectedSingleViewModel.SelectedResultRecord;
            Image image = record.ThumbNailPic;
            Image fullimage = record.OriginalPic;
            Rectangle rect = record.ObjectRect;
            if (image == null)
            {
                try
                {
                    image = Framework.Container.Instance.VideoSearchService.RequestImage(m_searchSession.SearchId, record, false);
                    record.ThumbNailPic = image;
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "获取缩略图");
                }
            }

            if (fullimage == null)
            {
                try
                {
                    fullimage = Framework.Container.Instance.VideoSearchService.RequestImage(m_searchSession.SearchId, record, true);
                    record.OriginalPic = fullimage;
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "获取原始图");
                }
                if (fullimage == null)
                {
                    MyLog4Net.Container.Instance.Log.Warn("GoToCompareSearch: No original image retrieved, cannot go to compare search");
                    return;
                }
            }

            DataModel.ImageType it = ImageType.Common;
            if (record.ObjectType == SearchResultObjectType.CAR)
                it = ImageType.Car;
            else if (record.ObjectType == SearchResultObjectType.FACE)
                it = ImageType.Face;
            else
                it = ImageType.Object;

            if (image != null)
            {
                // 需要复制一份Image
                image = new Bitmap(image);
                fullimage = new Bitmap(fullimage);
                Framework.Container.Instance.EvtAggregator.GetEvent<GotoCompareSearchEvent>().Publish("");
                CompareImageInfo info = new CompareImageInfo
                {
                    Image = fullimage,
                    RegionImage = image,
                    ImageRectangle = rect,
                    ImageType = it,
                };
                if (!Framework.Container.Instance.CacheMgr.HasItem(record))
                {
                    record.Clear();
                }
                Framework.Container.Instance.EvtAggregator.GetEvent<SetCompareImageInfoEvent>().Publish(info);
            }
        }

        public void PlayVideo()
        {
            if (m_SelectedSingleVM != null && m_SelectedSingleVM.SelectedResultRecord != null)
            {
                Framework.Container.Instance.EvtAggregator.GetEvent<SearchResoultPlaybackRequestEvent>().Publish(
                    new Tuple<SearchResultRecord, SearchType>(m_SelectedSingleVM.SelectedResultRecord, m_SearchType));
            }
        }

        public void DownloadVideo()
        {
            if (m_SelectedSingleVM != null)
            {
                SearchResultRecord record = SelectedSingleViewModel.SelectedResultRecord;

                try
                {
                    FormVideoEdit edit = new FormVideoEdit(Framework.Container.Instance.TaskManagerService.GetTaskUnitById(record.TaskUnitID)
                        , record.TargetAppearTs
                        , record.TargetDisappearTs);

                    edit.ShowDialog();
                }
                catch (Exception ex)
                { SDKCallExceptionHandler.Handle(ex, "导出视频", true); }
            }
        }

        #endregion

        #region Event handlers

        void OnSearchBegin(SearchSession session)
        {
            if (m_SearchType != session.SearchPara.SearchType)
            {
                return;
            }

            // 该事件响应需要同步执行, 
            if (Framework.Container.Instance.MainControl.InvokeRequired)
            {
                Framework.Container.Instance.MainControl.Invoke(new Action<SearchSession>(OnSearchBegin), new object[] { session });
                return;
            }

            try
            {
                m_searchSession = session;
                // 临时操作 不支持后排序，查询完成后直接显示原始排序方式
                SortType = m_searchSession.SearchPara.SortType;
                RaisePropertyChangedEvent("SortType");
                m_currSearchItem = session.SearchPara.CurrentSearchItem;

                // 先清除界面， 再清除ViewModels
                if (SearchCloseing != null)
                {
                    SearchCloseing(null, null);
                }

                ClearSingleViewModels();

                // 生成每个SearchItem 将要显示的View需要绑定的ViewModel
                foreach (SearchItem item in m_searchSession.SearchPara.SearchItems)
                {
                    AddSearchResultSingleViewModel(item);
                }

                if (SearchBegining != null)
                {
                    SearchBegining(session);
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }

        private void OnTaskUnitDeleted(uint taskUnitID)
        {
            if (m_searchSession != null && m_searchSession.SearchPara != null)
            {
                SearchItem searchItem = m_searchSession.SearchPara.GetSeachItem(taskUnitID);
                if (searchItem != null)
                {
                    if (m_DTSearchItem2SearchResultSingleViewModel.ContainsKey(searchItem))
                    {
                        Clear();
                    }
                }
            }
        }

        void SearchItemPageInfo_SelectedPageNumberChanged(object sender, EventArgs e)
        {
            if (!m_switchingSearchItem)
            {
                m_searchSession.SearchPara.CurrentSearchItemIndex = m_searchItemPageInfo.Index;
                m_searchSession.SearchPara.CurrentSearchItem.IsFinished = false;
                try
                {
                    Framework.Container.Instance.VideoSearchService.SwitchSearchItem(m_searchSession);
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "检索切换文件");
                }
            }
        }

        private void ResearchOnDisplayModeChanged()
        {
            if (m_searchSession != null && m_searchSession.SearchPara != null)
            {
                SearchResultDisplayMode displayMode = Framework.Environment.GetDisplayMode(m_SearchType);
                m_searchSession.SearchPara.DisplayMode = displayMode;
                bool multiItem = m_searchSession.SearchPara.SearchItems.Count > 1;
                m_searchSession.SearchPara.PageInfo.CountPerPage = Framework.Environment.GetDefaultCountPerPage(multiItem, displayMode);

                m_searchSession.SearchPara.SearchItems.ForEach(item =>
                    item.PageInfo.CountInCurrentPage = m_searchSession.SearchPara.PageInfo.CountPerPage);

                try
                {
                    Framework.Container.Instance.VideoSearchService.StartSearch(m_searchSession.SearchPara);
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "切换显示模式重新检索");
                }
            }
        }

        #endregion

    }
}
