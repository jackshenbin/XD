using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.DataModel;
using BOCOM.IVX.Protocol.Model;
using BOCOM.IVX.Framework;
using System.Threading;
using System.Drawing;
using System.Diagnostics;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Common;

namespace BOCOM.IVX.Service
{
    /// <summary>
    /// 视频特征检索
    /// 支持多个检索并行， 即不需要记住当前检索用于过滤过期结果返回消息， 
    /// 如果需要过滤消息，由上层实现。 此处要实现的是， 同一个类型的检索同一时间只有一个
    /// </summary>
    public class VideoSearchService
    {
        #region Fields
        
        private TaskRunner<SearchSession, SearchSession> m_taskRunner;

        /// <summary>
        /// 每种类型的检索只能有一个
        /// </summary>
        private Dictionary<SearchType, SearchSession> m_DTSearchType2SearchSession;

        //private SearchSession m_SearchSession;

        //private int m_SearchParaId;

        private object m_SyncObjSearch = new object();

        private ManualResetEvent m_MRERequestImage;

        // 记录调过 StartSearch，是否已经掉Search
        private Dictionary<SearchType, bool> m_DTSearchType2InQueue;

        private Tuple<SearchResultRecord, bool> m_SyncRequsestImageRecord;

        #endregion
        
        private IVXProtocol IVXProtocol
        {
            get
            {
                return Framework.Container.Instance.IVXProtocol;
            }
        }

        #region Constructors
        
        public VideoSearchService()
        {
            m_DTSearchType2SearchSession = new Dictionary<SearchType, SearchSession>();

            m_DTSearchType2InQueue = new Dictionary<SearchType, bool>();
            m_DTSearchType2InQueue.Add(SearchType.Compare, false);
            m_DTSearchType2InQueue.Add(SearchType.Face, false);
            m_DTSearchType2InQueue.Add(SearchType.Normal, false);
            m_DTSearchType2InQueue.Add(SearchType.Vehicle, false);

            m_MRERequestImage = new ManualResetEvent(false);
            m_taskRunner = new TaskRunner<SearchSession, SearchSession>("VideoSearch", ApplyInsertTaskItemPolicy);
            
            Framework.Container.Instance.IVXProtocol.SearchItemResultReceived += new Action<SearchItemResult>(IVXProtocol_SearchItemResultReceived);
            Framework.Container.Instance.IVXProtocol.SearchItemImageReceived += new Action<SearchImageInfo>(IVXProtocol_SearchItemImageReceived);
        }

        #endregion

        #region Private helper functions

        /// <summary>
        /// TaskRunner 增加一个Task Item 时， 使用的策略，如
        /// 是否允许队列里有多个，还是按照一个规则清除掉队列里的Task Item
        /// </summary>
        /// <param name="taskItems"></param>
        /// <param name="taskItem"></param>
        /// <returns></returns>
        private bool ApplyInsertTaskItemPolicy(Queue<TaskItem<SearchSession, SearchSession>> taskItems, TaskItem<SearchSession, SearchSession> taskItem)
        {
            bool bRet = true;

            //if (taskItems != null && taskItems.Count > 0 && taskItem != null)
            //{
            //    TaskItem<SearchSession, SearchSession> taskItemSameType = null;
            //    foreach (TaskItem<SearchSession, SearchSession> item in taskItems)
            //    {
            //        if (item.Para.SearchPara.SearchType == taskItem.Para.SearchPara.SearchType)
            //        {
            //            taskItemSameType = item;
            //            break;
            //        }
            //    }

            //    if (taskItemSameType != null)
            //    {
            //        taskItems.
            //    }
            //}
            MyLog4Net.Container.Instance.Log.Info("VideoSearchService entering ApplyInsertTaskItemPolicy ...");
            if (taskItems != null)
            {
                foreach (TaskItem<SearchSession, SearchSession> t in taskItems)
                {
                    MyLog4Net.Container.Instance.Log.InfoFormat("VideoSearchService ApplyInsertTaskItemPolicy: removed taskitem with session {0}", t.Para.GetHashCode());
                }
                taskItems.Clear();
            }
            MyLog4Net.Container.Instance.Log.Info("VideoSearchService exited ApplyInsertTaskItemPolicy");
            return bRet;
        }

        private void LogSearchItems(SearchPara searchPara)
        {
            List<SearchItem> searchItems = null;

            //if (searchPara.DisplayMode == SearchResultDisplayMode.ThumbNailAllSearchItem)
            //{
            searchItems = searchPara.SearchItems;
            //}
            //else
            //{
            //    searchItems = new List<SearchItem>();
            //    searchItems.Add(searchPara.CurrentSearchItem);
            //}

            MyLog4Net.Container.Instance.Log.DebugFormat("Search Items: count {0}/{1}", searchItems.Count, searchPara.SearchItems.Count);
            int index = 1;
            TaskUnitInfo taskUnitInfo;
            foreach (SearchItem item in searchItems)
            {
                taskUnitInfo = Framework.Container.Instance.TaskManagerService.GetTaskUnitById(item.TaskUnitId);
                Debug.Assert(taskUnitInfo != null);
                if (taskUnitInfo != null)
                {
                    MyLog4Net.Container.Instance.Log.DebugFormat("{0}: {1}", index++, taskUnitInfo.TaskUnitName);
                }
            }
        }

        private SearchSession Search(SearchSession searchSession)
        {
            MyLog4Net.Container.Instance.Log.InfoFormat("VideoSearchService entering Search: session {0}, searchType {1}, result display mode {2} ...", 
                searchSession.GetHashCode(), searchSession.SearchPara.SearchType, searchSession.SearchPara.DisplayMode);
            lock (this.m_SyncObjSearch)
            {
                CloseSearch(searchSession.SearchPara.SearchType);

                try
                {
                    LogSearchItems(searchSession.SearchPara);

                    if (searchSession.SearchPara.SearchType == SearchType.Normal)
                    {
                        searchSession.SearchId = Framework.Container.Instance.IVXProtocol.StartMoveObjectSearchByTaskUnit(searchSession.SearchPara);
                    }
                    else if (searchSession.SearchPara.SearchType == SearchType.Compare)
                    {
                        searchSession.SearchId = Framework.Container.Instance.IVXProtocol.StartCompareSearchByTaskUnit(searchSession.SearchPara);
                    }

                    else if (searchSession.SearchPara.SearchType == SearchType.Face)
                    {
                        searchSession.SearchId = Framework.Container.Instance.IVXProtocol.StartFaceSearchByTaskUnit(searchSession.SearchPara);
                    }

                    else if (searchSession.SearchPara.SearchType == SearchType.Vehicle)
                    {
                        searchSession.SearchId = Framework.Container.Instance.IVXProtocol.StartVehicleSearchByTaskUnit(searchSession.SearchPara);
                    }

                    m_DTSearchType2SearchSession.Add(searchSession.SearchPara.SearchType, searchSession);
                    m_DTSearchType2InQueue[searchSession.SearchPara.SearchType] = false;
                }
                catch (Exception ex)
                {
                    searchSession.Exception = ex;
                    SDKCallExceptionHandler.Handle(ex, "Search", false);
                }
                MyLog4Net.Container.Instance.Log.InfoFormat("VideoSearchService exited Search: session {0}, searchType {1}, seachID {2}",
                    searchSession.GetHashCode(), searchSession.SearchPara.SearchType, searchSession.SearchId);
            }
            return searchSession;
        }
        
        private void UpdateSearchResultRecords(List<SearchResultRecord> records)
        {
            if (records != null)
            {
                foreach (SearchResultRecord record in records)
                {
                    record.VehicleBrandInfo = Framework.Container.Instance.VehicleInfoService.GetBrandInfo(record.VehicleBrand);
                    record.VehicleBodyColorName1 = Framework.Container.Instance.ColorService.GetVehicleColorName(record.VehicleBodyColor1);
                    record.VehicleBodyColorName2 = Framework.Container.Instance.ColorService.GetVehicleColorName(record.VehicleBodyColor2);
                    record.VehicleBodyColorName3 = Framework.Container.Instance.ColorService.GetVehicleColorName(record.VehicleBodyColor3);
                    record.VehicleBodyColor = string.Format("{0}, {1}, {2}", record.VehicleBodyColorName1, record.VehicleBodyColorName2, record.VehicleBodyColorName3);
                    record.VehicleBodyColor = record.VehicleBodyColor.Replace(", 不限", string.Empty);

                    record.PlateColorName = Framework.Container.Instance.ColorService.GetPlateColorName(record.PlateColor);
                }
            }
        }

        private SearchSession GetSearchSession(int id)
        {
            SearchSession session = null;
            foreach (KeyValuePair<SearchType, SearchSession> kv in m_DTSearchType2SearchSession)
            {
                if (kv.Value.SearchId == id)
                {
                    session = kv.Value;
                    break;
                }
            }
            return session;
        }

        #endregion

        #region Public helper functions
        
        public bool StartSearch(SearchPara para)
        {
            if (!Framework.Environment.CheckMemeryUse())
            {
                throw new SDKCallException(0, "内存占用过大，请关闭视频播放或取消检索后再试。");
            }

            MyLog4Net.Container.Instance.Log.Info("VideoSearchService entering StartSearch ...");
            bool bRet = true;

            lock (m_SyncObjSearch)
            {
                // 先停掉当前的检索
                //if (m_SearchSession != null)
                //{
                //    Framework.Container.Instance.IVXProtocol.CloseSearchSession(m_SearchSession.SearchId);
                //    m_SearchSession = null;
                //}
                try
                { 
                    CloseSearch(para.SearchType);
                }
                catch (SDKCallException ex)
                {
                    SDKCallExceptionHandler.Handle(ex, "CloseSearchSession", false);
                }
                
                // 放入任务队列
                SearchSession searchSession = new SearchSession(para) { };
                // 状态复位
                para.ResetSearchItemStatus();

                para.DisplayMode = Framework.Environment.GetDisplayMode(para.SearchType);

                // m_DTSearchType2SearchSession.Add(para.SearchType, searchSession);
                m_DTSearchType2InQueue[para.SearchType] = true;

                TaskItem<SearchSession, SearchSession> taskItem =
                    new TaskItem<SearchSession, SearchSession> { FuncToRun = Search, Callback = OnSearchResult, Para = searchSession };

                MyLog4Net.Container.Instance.Log.InfoFormat("VideoSearchService: Insert task item session {0}", searchSession.GetHashCode());
                m_taskRunner.AddTask(taskItem);

                MyLog4Net.Container.Instance.Log.Info("VideoSearchService leave StartSearch");
            }
            return bRet;
        }

        public bool CloseSearch(uint searchSessionID)
        {
            MyLog4Net.Container.Instance.Log.InfoFormat("VideoSearchService entering CancelSearch {0} ...", searchSessionID);
            lock (m_SyncObjSearch)
            {
                // Debug.Assert(m_SearchSession != null && m_SearchSession.SearchId == searchSessionID);
                //Framework.Container.Instance.IVXProtocol.CloseSearchSession(searchSessionID);
                //m_SearchSession = null;
                //m_SearchParaId = 0;
                SearchSession session = GetSearchSession((int)searchSessionID);

                if (session != null)
                {
                    try
                    { 
                        CloseSearch(session.SearchPara.SearchType);
                    }
                    catch (SDKCallException ex)
                    {
                        SDKCallExceptionHandler.Handle(ex, "CloseSearchSession", false);
                    }
                }
            }

            MyLog4Net.Container.Instance.Log.InfoFormat("VideoSearchService leave CancelSearch {0}", searchSessionID);

            return true;
        }

        public void CloseSearch(SearchType searchType)
        {
            MyLog4Net.Container.Instance.Log.InfoFormat("VideoSearchService entering CancelSearch, type {0} ...", searchType);

            lock (m_SyncObjSearch)
            {
                if (m_DTSearchType2SearchSession.ContainsKey(searchType))
                {
                    SearchSession session = m_DTSearchType2SearchSession[searchType];
                    Framework.Container.Instance.EvtAggregator.GetEvent<SearchCloseEvent>().Publish(session);

                    try
                    {
                        MyLog4Net.Container.Instance.Log.InfoFormat("VideoSearchService entering CancelSearch, sessionId {0} ...", session.SearchId);
                        IVXProtocol.CloseSearchSession(session.SearchId);
                    }
                    finally
                    {
                        m_DTSearchType2SearchSession.Remove(searchType);
                    }
                }
            }

            MyLog4Net.Container.Instance.Log.InfoFormat("VideoSearchService leave CancelSearch, type {0}", searchType);
        }

        public void CloseSearch()
        {
            lock (m_SyncObjSearch)
            {
                if (m_DTSearchType2SearchSession.Count > 0)
                {
                    SearchType[] searchTypes = m_DTSearchType2SearchSession.Keys.ToArray();
                    foreach (SearchType searchType in searchTypes)
                    {
                        try
                        {
                            CloseSearch(searchType);
                        }
                        catch (SDKCallException ex)
                        {
                            SDKCallExceptionHandler.Handle(ex, "CloseSearchSession", false);
                        }
                        m_DTSearchType2SearchSession.Remove(searchType);
                    }
                }
            }
        }

        /// <summary>
        /// 切换检索文件
        /// </summary>
        /// <param name="searchSession"></param>
        public void SwitchSearchItem(SearchSession searchSession)
        {
            MyLog4Net.Container.Instance.Log.InfoFormat("VideoSearchService entering SwitchSearchItem {0}...", searchSession.SearchId);

            TaskItem<SearchSession, SearchSession> taskItem =
               new TaskItem<SearchSession, SearchSession> { FuncToRun = Search, Callback = OnSearchResult, Para = searchSession };

            m_taskRunner.AddTask(taskItem);

            MyLog4Net.Container.Instance.Log.InfoFormat("VideoSearchService leave SwitchSearchItem {0}", searchSession.SearchId);
        }

        public void SwitchSearchPage(uint searchSessionID, uint taskUnitID, uint pageIndex)
        {
            MyLog4Net.Container.Instance.Log.InfoFormat("VideoSearchService entering SwitchSearchPage, searchSessionID:{0}, taskUnitID:{1}, pageIndex:{2}" 
                , searchSessionID,taskUnitID,pageIndex);

            Framework.Container.Instance.EvtAggregator.GetEvent<SwitchPageBeginEvent>().Publish(new Tuple<uint, uint>(searchSessionID, taskUnitID));

            Framework.Container.Instance.IVXProtocol.StartSwitchResultInfo(searchSessionID, taskUnitID, pageIndex);

            MyLog4Net.Container.Instance.Log.InfoFormat("VideoSearchService leave SwitchSearchPage, searchSessionID:{0}, taskUnitID:{1}, pageIndex:{2}"
                , searchSessionID, taskUnitID, pageIndex);
        }

        public void StartRequestImage(uint searchSessionID, SearchResultRecord item, bool isOriginalImage)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("StartRequestImage searchSessionID:{0},taskUnitID:{1},objID:{2},imageType:{3}"
                , searchSessionID, item.TaskUnitID, item.ID, isOriginalImage));

            string url = isOriginalImage? item.OrgPicPath : item.ThumbPicPath;
            Framework.Container.Instance.IVXProtocol.StartGetImage(searchSessionID, item.CameraID, item.TaskUnitID, item.ID, url);
        }

        /// <summary>
        /// 同步等到获取到图片后返回
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isOriginalImage"></param>
        /// <returns></returns>
        public Image RequestImage(uint searchSessionID, SearchResultRecord item, bool isOriginalImage, int timeout = 5*1000)
        {
            Image imgRet = null;
            lock (m_MRERequestImage)
            {
                m_SyncRequsestImageRecord = new Tuple<SearchResultRecord, bool>(item, isOriginalImage);
                StartRequestImage(searchSessionID, item, isOriginalImage);
            }

            bool succeed = m_MRERequestImage.WaitOne(timeout);

            if (succeed)
            {
                lock (m_MRERequestImage)
                {
                    imgRet = isOriginalImage ? m_SyncRequsestImageRecord.Item1.OriginalPic : m_SyncRequsestImageRecord.Item1.ThumbNailPic;
                    m_MRERequestImage.Reset();
                    m_SyncRequsestImageRecord = null;
                }
            }

            return imgRet;
        }


        #endregion

        #region Event handlers
        
        private void OnSearchResult(SearchSession searchSession)
        {
            bool matched = false;


            lock(m_SyncObjSearch)
            {
                // TODO: 考虑将 m_SearchSession 赋值放到 StartSearch
                // 因为这个消息回来的时候, 这次检索可能已经被取消
                SearchType searchType = searchSession.SearchPara.SearchType;

                if(!m_DTSearchType2InQueue[searchType] &&
                    m_DTSearchType2SearchSession.ContainsKey(searchType) &&
                    m_DTSearchType2SearchSession[searchType] == searchSession)
                {
                    matched = true;
                }
            }

            if (matched)
            {
                Framework.Container.Instance.EvtAggregator.GetEvent<SearchBeginEvent>().Publish(searchSession);
            }
        }

        void IVXProtocol_SearchItemResultReceived(SearchItemResult obj)
        {
            lock (m_SyncObjSearch)
            {
                SearchSession searchSession = GetSearchSession((int)obj.SearchId);
                if (searchSession == null)
                {
                    return;
                }

                //if (searchSession.SearchPara.DisplayMode != SearchResultDisplayMode.ThumbNailAllSearchItem)
                //{
                //    if (obj.TaskUnitId != searchSession.SearchPara.CurrentSearchItem.TaskUnitId)
                //    {
                //        return;
                //    }
                //}

                SearchResultSingleSummary result = new SearchResultSingleSummary();
                result.IsSimilaritySearch = searchSession.SearchPara.IsSimilaritySearch;
                result.CurrPageIndex = obj.PageInfo.Index;
                result.CurrPageTotalCount = obj.PageInfo.CountInCurrentPage;
                result.SearchResultList = obj.ResultRecords;

                if (searchSession.SearchPara.SearchType == SearchType.Vehicle)
                {
                    UpdateSearchResultRecords(result.SearchResultList);
                }

                result.SearchSessionID = obj.SearchId;
                result.TaskUnitID = obj.TaskUnitId;
                result.TotalCount = obj.PageInfo.TotalCount;
                result.TotalPage = obj.PageInfo.TotalPage;

                SearchItem searchItem = searchSession.SearchPara.GetSeachItem(obj.TaskUnitId);
                lock (searchItem)
                {
                    searchItem.IsFinished = true;
                    searchItem.ResultSummary = result;
                    MyLog4Net.Container.Instance.Log.InfoFormat("Fire SearchFinishedEvent in VideoSearchService, taskUnitID: {0}", result.TaskUnitID);
                    Framework.Container.Instance.EvtAggregator.GetEvent<SearchFinishedEvent>().Publish(result);
                }
            }
        }

        void IVXProtocol_SearchItemImageReceived(SearchImageInfo obj)
        {
            bool matched = false;
            lock (m_MRERequestImage)
            {

                if (m_SyncRequsestImageRecord != null)
                {
                   if (m_SyncRequsestImageRecord.Item2)
                   {
                       if(string.Compare(m_SyncRequsestImageRecord.Item1.OrgPicPath, obj.ImageURL, true) == 0)
                       {
                           m_SyncRequsestImageRecord.Item1.OriginalPic = obj.Image;
                           matched = true;
                           m_MRERequestImage.Set();
                       }
                   }
                   else
                   {
                      if(string.Compare(m_SyncRequsestImageRecord.Item1.ThumbPicPath, obj.ImageURL, true) == 0)
                       {
                           matched = true;
                           m_SyncRequsestImageRecord.Item1.ThumbNailPic = obj.Image;
                           m_MRERequestImage.Set();
                       }
                   }
                }
            }

            if (!matched)
            {
                Framework.Container.Instance.EvtAggregator.GetEvent<SearchItemImageReceivedEvent>().Publish(obj);
            }
        }


        #endregion
    }
}
