using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.DataModel;
using System.Data;
using BOCOM.IVX.Protocol.Model;
using BOCOM.IVX.Framework;
using BOCOM.IVX.Common;
using DataModel;
using System.Diagnostics;
using System.Drawing;
using Microsoft.Practices.Prism.Events;
using BOCOM.IVX.Controls;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using BOCOM.IVX.Protocol;

namespace BOCOM.IVX.ViewModel.Search
{
    public class SearchResultSingleViewModel : ViewModelBase, IEventAggregatorSubscriber
    {
        #region Events
        
        public event Action<SearchResultSingleViewModel, SearchResultSingleSummary> SearchFinished;
        public event EventHandler SearchClosing;
        public event EventHandler SelectedRecordChanged;
        public event Action<SearchResultRecord> ThumbNailImageRetrieved;
        public event Action<SearchResultRecord> OriginalImageRetrieved;

        public  event EventHandler LostFocus;
        
        #endregion

        #region Fields
        
        private SearchItem m_currSearchItem;

        private uint m_searchSessionID = 0;

        private SearchSession m_SearchSession;

        private SearchResultSingleSummary m_ResultSummary;

        private string m_StatusText = "请稍候，正在检索 .....";

        private string m_Title;

        private PageInfo m_PageInfo;

        private PageInfo m_DetailViewPageInfo;

        private bool m_Selected;

        private bool m_IsSearching = true;

        private Dictionary<string, SearchResultRecord> m_DTKey2Record = new Dictionary<string, SearchResultRecord>();

        private EditImageForm m_EditImageForm;

        private bool m_ShowResultList = false;

        private SearchResultRecord m_SelectedResultRecord;

        #endregion

        #region Properties

        public SearchSession SearchSession
        {
            get { return m_SearchSession; }
        }

        public SearchItem CurrSearchItem
        {
            get { return m_currSearchItem; }
            set { m_currSearchItem = value; }
        }

        /// <summary>
        /// 表示当前结果展示面板是否被选中
        /// </summary>
        public bool Selected
        {
            get
            {
                return m_Selected;
            }
            set
            {
                m_Selected = value;
                //if (!m_Selected)
                //{
                //    if (LostFocus != null)
                //    {
                //        LostFocus(this, EventArgs.Empty);
                //    }
                //}
            }
        }

        /// <summary>
        /// 当前选中的结果项
        /// </summary>
        public SearchResultRecord SelectedResultRecord
        {
            get
            {
                return m_SelectedResultRecord;
            }
            set
            {
                m_SelectedResultRecord = value;

                Selected = m_SelectedResultRecord != null;
                if (SelectedRecordChanged != null)
                {
                    SelectedRecordChanged(this, EventArgs.Empty);
                }
            }
        }

        public SearchResultSingleSummary ResultSummary
        {
            get { return m_ResultSummary; }
        }

        public string StatusText
        {
            get
            {
                return m_StatusText;
            }
            set
            {
                MyLog4Net.Container.Instance.Log.DebugFormat("{0} set StatusText, old value {1}, new value {2}", this.GetHashCode(), m_StatusText, value);
                if (string.Compare(m_StatusText, value, true) != 0)
                {
                    m_StatusText = value;
                    // ShowProgressPanel = !string.IsNullOrEmpty(m_StatusText);
                    ShowResultList = string.IsNullOrEmpty(m_StatusText);
                    RaisePropertyChangedEvent("StatusText");
                }
            }
        }

        public string Title
        {
            get
            {
                return m_Title;
            }
            set
            {
                if (string.Compare(m_Title, value, true) != 0)
                {
                    m_Title = value;
                    RaisePropertyChangedEvent("Title");
                }
            }
        }

        public bool ShowProgressPanel
        {
            get
            {
                return !string.IsNullOrEmpty(m_StatusText);
            }
            set
            {
                //if (ShowProgressPanel != value)
                //{
                RaisePropertyChangedEvent("ShowProgressPanel");
                ShowResultList = !value;
                //}
            }
        }

        public bool ShowResultList
        {
            get
            {
                return m_ShowResultList;
            }
            set
            {
                if (m_ShowResultList != value)
                {
                    m_ShowResultList = value;
                    RaisePropertyChangedEvent("ShowResultList");
                }
            }
        }

        public bool IsSearching
        {
            get
            {
                return m_IsSearching;
            }
            set
            {
                if (m_IsSearching != value)
                {
                    m_IsSearching = value;
                    RaisePropertyChangedEvent("IsSearching");
                }
            }
        }

        /// <summary>
        /// 结果的分页， 一页多条记录
        /// </summary>
        public PageInfo ResultPageInfo
        {
            get
            {
                return m_PageInfo;
            }
        }

        /// <summary>
        /// 用于显示原图时的分页信息， 每次只显示一张图
        /// </summary>
        public PageInfo DetailViewPageInfo
        {
            get { return m_DetailViewPageInfo; }
        }


        #endregion

        #region Constructors

        public SearchResultSingleViewModel(SearchSession session, SearchItem searchItem)
        {
            TaskUnitInfo info = null;
            try
            {
                info = Framework.Container.Instance.TaskManagerService.GetTaskUnitById(searchItem.TaskUnitId);
            }
            catch (SDKCallException ex)
            {
                info = null;
                Common.SDKCallExceptionHandler.Handle(ex, "获取任务单元");
            }

            if (info != null)
            {
                CameraInfo camera = null;
                try
                {
                    camera = Framework.Container.Instance.VDAConfigService.GetCameraByID(info.CameraId);
                }
                catch (SDKCallException ex)
                {
                    camera = null;
                    Common.SDKCallExceptionHandler.Handle(ex, "获取监控点");
                }

                if (camera != null)
                {
                    Title = string.Format("{0} ({1}-{2}", camera.CameraName,
                        info.StartTime.ToString(DataModel.Constant.DATETIME_FORMAT),
                        info.EndTime.ToString(DataModel.Constant.DATETIME_FORMAT));
                }
                Title = info.TaskUnitName;
            }

            // 注册事件的时候， 可能事件已经 Fire 过了
            lock (searchItem)
            {
                Framework.Container.Instance.EvtAggregator.GetEvent<SearchFinishedEvent>().Subscribe(OnSearchFinished, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
                Framework.Container.Instance.EvtAggregator.GetEvent<SearchItemImageReceivedEvent>().Subscribe(OnSearchItemImageReceived, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
                Framework.Container.Instance.EvtAggregator.GetEvent<SearchResultRecordSelectedEvent>().Subscribe(OnSearchResultRecordSelectedEvent, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
                Framework.Container.Instance.EvtAggregator.GetEvent<SwitchPageBeginEvent>().Subscribe(OnSwitchPage, ThreadOption.PublisherThread);
                Framework.Container.Instance.RegisterEventSubscriber(this);
                m_currSearchItem = searchItem;

                m_searchSessionID = session.SearchId;
                m_SearchSession = session;

                if (searchItem.IsFinished)
                {
                    MyLog4Net.Container.Instance.Log.DebugFormat("Fire OnSearchFinished event in SearchResultSingleViewModel constructor, ResultSummary taskUnitID: {0}", searchItem.ResultSummary.TaskUnitID);
                    // 需要补上SearchBegin, SearchFinished 事件要做的事情
                    OnSearchFinished(searchItem.ResultSummary);
                }
            }
        }

        #endregion

        #region Private helper functions

        private void UpdatePageInfo(SearchResultSingleSummary singleSummary)
        {
            if (m_PageInfo == null)
            {
                m_PageInfo = new PageInfo(singleSummary.TotalPage, singleSummary.TotalCount, singleSummary.CurrPageIndex);
                m_PageInfo.CurrentPageCount = singleSummary.CurrPageTotalCount;
                m_PageInfo.SelectedPageNumberChanged += new EventHandler(ResultPageInfo_SelectedPageNumberChanged);
            }
            else
            {
                m_PageInfo.TotalRecords = singleSummary.TotalCount;
                m_PageInfo.CurrentPageCount = singleSummary.CurrPageTotalCount;
                m_PageInfo.Count = singleSummary.TotalPage;
                m_PageInfo.Index = singleSummary.CurrPageIndex;
            }

            if (m_DetailViewPageInfo == null)
            {
                m_DetailViewPageInfo = new PageInfo(singleSummary.CurrPageTotalCount, singleSummary.CurrPageTotalCount, 0);
            }
            else
            {
                m_DetailViewPageInfo.TotalRecords = singleSummary.CurrPageTotalCount;
                m_DetailViewPageInfo.Count = singleSummary.CurrPageTotalCount;
                m_DetailViewPageInfo.Index = 0;
            }

        }

        private string GetRecordKey(SearchResultRecord record)
        {
           return GetRecordKey(record.CameraID, record.TaskUnitID, record.ID);
        }

        private string GetRecordKey(uint cameraID, uint taskUnitID, uint recordID)
        {
            string key = string.Format("{0}#{1}#{2}", cameraID, taskUnitID, recordID);
            return key;
        }

        private void BuildSearchResultRecordTable()
        {
            m_DTKey2Record = new Dictionary<string, SearchResultRecord>();
            if (m_ResultSummary != null && m_ResultSummary.SearchResultList != null)
            {
                string key;
                foreach (SearchResultRecord record in m_ResultSummary.SearchResultList)
                {
                    key = GetRecordKey(record);
                    Debug.Assert(!m_DTKey2Record.ContainsKey(key), string.Format("Duplicated SearchResultRecord key: {0}", key));
                    if (!m_DTKey2Record.ContainsKey(key))
                    {
                        m_DTKey2Record.Add(key, record);
                    }
                }
            }
        }

        #endregion

        #region Public helper functions

        public void Clear()
        {
            if (m_EditImageForm != null)
            {
                m_EditImageForm.Close();
                // m_EditImageForm = null;
            }

            if (m_ResultSummary != null)
            {
                m_ResultSummary.ClearResult();
            }
            if (m_DTKey2Record != null)
            {
                m_DTKey2Record.Clear();
            }

            SelectedResultRecord = null;
        }

        public void UnInit()
        {
            Clear();

            if (m_PageInfo != null)
            {
                m_PageInfo.SelectedPageNumberChanged -= new EventHandler(ResultPageInfo_SelectedPageNumberChanged);
            }

            Framework.Container.Instance.UnRegisterEventSubscriber(this);
        }

        public void StartRequestImage(SearchResultRecord item, bool isOriginalImage)
        {
            try
            {
                Framework.Container.Instance.VideoSearchService.StartRequestImage(m_searchSessionID, item, isOriginalImage);
            }
            catch (SDKCallException ex)
            {
                string operation = isOriginalImage ? "请求原始图" : "请求缩略图";
                Common.SDKCallExceptionHandler.Handle(ex, operation);
            }
        }

        public void ShowOriginalImage()
        {
            int index = 0;
            SearchResultRecord record = SelectedResultRecord;
            if(record != null)
            {
                index = m_ResultSummary.SearchResultList.IndexOf(record);
            }
            DetailViewPageInfo.Index = index;

            // 用using 在Clear的时候m_EditImageForm 为null
            m_EditImageForm = new EditImageForm(this);
            m_EditImageForm.FormClosed += new FormClosedEventHandler(EditImageForm_FormClosed);
            m_EditImageForm.ShowDialog();
        }

        public override void UnSubscribe()
        {
            MyLog4Net.Container.Instance.Log.DebugFormat("SearchResultSingleViewModel ({0}), UnSubscribe", this.GetHashCode());

            Framework.Container.Instance.EvtAggregator.GetEvent<SearchFinishedEvent>().Unsubscribe(OnSearchFinished);
            // Framework.Container.Instance.EvtAggregator.GetEvent<SearchBeginEvent>().Unsubscribe(OnSearchBegin);
            Framework.Container.Instance.EvtAggregator.GetEvent<SearchItemImageReceivedEvent>().Unsubscribe(OnSearchItemImageReceived);

            Framework.Container.Instance.EvtAggregator.GetEvent<SearchResultRecordSelectedEvent>().Unsubscribe(OnSearchResultRecordSelectedEvent);
            Framework.Container.Instance.EvtAggregator.GetEvent<SwitchPageBeginEvent>().Unsubscribe(OnSwitchPage);
        }

        public void RetrieveThumbnailImages()
        {
            if (m_ResultSummary.SearchResultList != null && m_ResultSummary.SearchResultList.Count > 0)
            {
                MyLog4Net.Container.Instance.Log.DebugFormat("### RetrieveThumbnailImages, taskunitname: {0}, taskUnitID: {1}", Title, m_currSearchItem.TaskUnitId);
                try
                {
                    foreach (SearchResultRecord record in m_ResultSummary.SearchResultList)
                    {
                        Framework.Container.Instance.VideoSearchService.StartRequestImage(m_searchSessionID, record, false);
                    }
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "获取缩略图", false);
                }
            }
        }

        public void ExportAllPics(bool originalPic)
        {
            string path = Framework.Environment.PictureSavePath;
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.ShowNewFolderButton = true;
                dialog.Description = "设置需要批量导出的文件夹";
                dialog.SelectedPath = path;
                if (dialog.ShowDialog() != DialogResult.OK)
                { return; }

                path = dialog.SelectedPath;
            }

            ProgressFormStandalone pf = new ProgressFormStandalone("图片批量导出", "正在请求图片。。。");
            pf.Show();
            int index = 0;
            if (m_ResultSummary != null && m_ResultSummary.SearchResultList != null && m_ResultSummary.SearchResultList.Count > 0)
            {
                int count =  m_ResultSummary.SearchResultList.Count;
                float unit = 1.0f / (3f * count);
                float progress = 0.0f;
                string operation = originalPic ? "请求原始图" : "请求缩略图";
                foreach (SearchResultRecord record in m_ResultSummary.SearchResultList)
                {

                    index ++;

                    progress += unit * 2;

                    if (originalPic)
                    {
                        if (record.OriginalPic == null)
                        {
                            pf.UpdateStatusText(string.Format("正在获取 {0} / {1} 图片 ...", index, count));
                            pf.UpdateProgress(progress);
                            try
                            {
                                record.OriginalPic = Framework.Container.Instance.VideoSearchService.RequestImage(m_searchSessionID, record, originalPic);
                            }
                            catch (SDKCallException ex)
                            {
                                Common.SDKCallExceptionHandler.Handle(ex, operation);
                            }
                        }
                    }
                    else
                    {
                        if (record.ThumbNailPic == null)
                        {
                            pf.UpdateStatusText(string.Format("正在获取 {0} / {1} 图片 ...", index, count));
                            pf.UpdateProgress(progress);
                            try
                            {
                                record.ThumbNailPic = Framework.Container.Instance.VideoSearchService.RequestImage(m_searchSessionID, record, originalPic);
                            }
                            catch (SDKCallException ex)
                            {
                                Common.SDKCallExceptionHandler.Handle(ex, operation);
                            }
                        }
                    }
                }
                progress = 0.67f;
                pf.UpdateStatusText("开始导出图片。。。");
                pf.UpdateProgress(progress);

                // string path = Framework.Environment.PictureSavePath + "\\" + DateTime.Now.ToString("MMddHHmmss") + "\\";
                
                if (!System.IO.Directory.Exists(path))
                 {
                     System.IO.Directory.CreateDirectory(path);
                 }
                if(!path.EndsWith("\\"))
                {
                    path = path+"\\";
                }

                string resourcename = GetResourceNameByTaskUnitID(m_ResultSummary.TaskUnitID);

                string fileName;
                Image img;
                foreach (SearchResultRecord record in m_ResultSummary.SearchResultList)
                {
                    img = originalPic ? record.OriginalPic : record.ThumbNailPic;
                    if (img != null)
                    {
                        string type = (originalPic) ? "原始图" : "缩略图";

                        // string FileName = "原始图" + index + ".jpg";
                        fileName = resourcename.Replace(".", "_") + type + record.ID.ToString() + ".jpg";

                        using (Image imgTmp = new Bitmap(img))
                        {
                            try
                            {
                                 pf.UpdateStatusText(string.Format("正在保存图片 ‘{0}’ ...", fileName));
                                progress += unit;
                                pf.UpdateProgress(progress);

                                imgTmp.Save(Path.Combine(path, fileName));

                                if (originalPic)
                                {
                                    // 需要清除， 避免内存占用过大
                                    if (!Framework.Container.Instance.CacheMgr.HasItem(record))
                                    {
                                        record.Clear();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                string msg = string.Format("保存图片出错: {0}", ex.Message);
                                Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME,
                                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                                msg = msg = string.Format("保存图片 '{0}' 出错", fileName);
                                MyLog4Net.Container.Instance.Log.Error(msg, ex);
                            }
                        }
                    }
                }
            }

            pf.UpdateStatusText("导出图片完成");
            pf.Close();
        }

        public void ExportPic(bool originalPic)
        {
            SearchResultRecord record = m_SelectedResultRecord;

            if (record == null)
            {
                return;
            }

            string resourcename = GetResourceNameByTaskUnitID(record.TaskUnitID);
            
            string type = (originalPic) ? "原始图" : "缩略图";
            string fileName = resourcename.Replace(".", "_") + type + record.ID.ToString() + ".jpg";
            bool needdSave = true;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;

            sfd.Filter = "JPG文件|*.jpg";
            sfd.FileName = fileName;
            sfd.InitialDirectory = Framework.Environment.PictureSavePath;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                fileName = sfd.FileName;
            }
            else
            {
                needdSave = false;
            }

            if (needdSave)
            {
                // 保存文件
                Trace.WriteLine("btnSave_Click, FileName:" + fileName);
                Image srcpic = null;
                if (originalPic)
                {
                    if (record.OriginalPic == null)
                    {
                        try
                        {
                            record.OriginalPic =
                                Framework.Container.Instance.VideoSearchService.RequestImage(m_searchSessionID, record, originalPic);
                        }
                        catch (SDKCallException ex)
                        {
                            Common.SDKCallExceptionHandler.Handle(ex, "获取原始图");
                        }
                    }
                    srcpic = record.OriginalPic;
                }
                else
                {
                    if (record.ThumbNailPic == null)
                    {
                        try
                        {
                            record.ThumbNailPic =
                                Framework.Container.Instance.VideoSearchService.RequestImage(m_searchSessionID, record, originalPic);
                        }
                        catch (SDKCallException ex)
                        {
                            Common.SDKCallExceptionHandler.Handle(ex, "获取缩略图");
                        }
                    }
                    srcpic = record.ThumbNailPic;
                }

                if (srcpic != null)
                {
                    using (Image imgTmp = new Bitmap(srcpic))
                    {
                        imgTmp.Save(fileName, ImageFormat.Jpeg);
                    }
                }
            }
        }

        private static string GetResourceNameByTaskUnitID(uint TaskUnitID)
        {
            string resourcename = string.Empty;

            // 没有关联camera， 使用TaskUnit Name
            TaskUnitInfo taskUnitInfo = null;
            try
            {
                taskUnitInfo = Framework.Container.Instance.TaskManagerService.GetTaskUnitById(TaskUnitID);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "获取任务单元");
            }

            resourcename = taskUnitInfo.TaskUnitName;
            return resourcename;
        }
        
        public void ExportVideo()
        {
            SearchResultRecord record = m_SelectedResultRecord;


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

        public void SearchResoultPlayBack()
        {
            SearchResultRecord record = m_SelectedResultRecord;
            if (record != null)
            {
                Framework.Container.Instance.EvtAggregator.GetEvent<SearchResoultPlaybackRequestEvent>().Publish(
                    new Tuple<SearchResultRecord, SearchType> (record, m_SearchSession.SearchPara.SearchType));
            }

        }

        #endregion

        internal void HandleSearchFinishedEvent(SearchResultSingleSummary singleSummary)
        {
            m_ResultSummary = singleSummary;
            BuildSearchResultRecordTable();

            UpdatePageInfo(singleSummary);

            IsSearching = false;

            if (singleSummary.SearchResultList != null && singleSummary.SearchResultList.Count > 0)
            {
                StatusText = string.Empty;
            }
            else
            {
                StatusText = "无匹配的结果";
            }

            if (SearchFinished != null)
            {
                SearchFinished(this, singleSummary);
            }
        }

        #region Event handlers
        
        void OnSearchFinished(SearchResultSingleSummary singleSummary)
        {
            if (singleSummary == null)
                return;
            if (m_searchSessionID != singleSummary.SearchSessionID)
                return;
            
            if (m_currSearchItem.TaskUnitId != singleSummary.TaskUnitID)
            {
                return;
            }

            MyLog4Net.Container.Instance.Log.InfoFormat("SearchResultSingleViewModel ({0}) OnSearchFinished, taskUnitName: {1}, singleSummary taskUnitID: {2}, m_currSearchItem.TaskUnitId: {3}, total count: {4}",
               this.GetHashCode(), Title, singleSummary.TaskUnitID, 
               m_currSearchItem.TaskUnitId, singleSummary.TotalCount);

            if (this.m_PageInfo == null || IsSearching)
            {
                HandleSearchFinishedEvent(singleSummary);
                MyLog4Net.Container.Instance.Log.InfoFormat("Called HandleSearchFinishedEvent from single view model: item {0}", this.m_currSearchItem.TaskUnitId);
            }
            else
            {
                // Debug.Assert(false, "SearchFinished has been fired in view");
            }
        }

        void OnSwitchPage(Tuple<uint, uint> tuple)
        {
            if (m_currSearchItem.TaskUnitId == tuple.Item2)
            {
                Clear();
                IsSearching = true;
                StatusText = "请稍候，正在检索 .....";
                try
                {
                    ResultPageInfo.CurrentPageCount = 0;
                }
                catch (NullReferenceException ex)
                {
                    SDKCallExceptionHandler.Handle(ex, "切换页面", false);
                }
            }
        }
        
        //void OnSearchBegin(SearchSession session)
        //{
        //    m_searchSessionID = session.SearchId;
        //    //m_currSearchItem = summary.SearchPara.CurrentSearchItem;
        //}

        void OnSearchItemImageReceived(SearchImageInfo info)
        {
            string key = GetRecordKey(info.dwCameraID, info.dwTaskUnitID, info.dwMoveObjID);
            if (m_DTKey2Record.ContainsKey(key))
            {
                SearchResultRecord record = m_DTKey2Record[key];
                if (string.Compare(record.ThumbPicPath, info.ImageURL, true) == 0)
                {
                    MyLog4Net.Container.Instance.Log.DebugFormat("### OnSearchItemImageReceived, taskunitname: {0}, taskUnitID: {1}", Title, m_currSearchItem.TaskUnitId);
                    if (record.ThumbNailPic == null)
                    {
                        record.ThumbNailPic = info.Image;

                        DataRow dataRow = this.m_ResultSummary.AllSearchResultList.Rows.Find(info.dwMoveObjID);

                        if (dataRow != null)
                        {
                            dataRow["ThumbNailPic"] = info.Image;
                        }

                        if (ThumbNailImageRetrieved != null)
                        {
                            ThumbNailImageRetrieved(record);
                        }
                    }
                }
                else
                {
                    int result = string.Compare(record.OrgPicPath, info.ImageURL, true);
                    // Debug.Assert(result == 0);
                    if (result == 0)
                    {
                        if (record.OriginalPic == null)
                        {
                            record.OriginalPic = info.Image;
                            
                            Framework.Container.Instance.CacheMgr.Register(record);

                            if (OriginalImageRetrieved != null)
                            {
                                if (record.ObjectRect != Rectangle.Empty)
                                {
                                    // 先提取目标图
                                    Image img = new Bitmap(record.ObjectRect.Width, record.ObjectRect.Height);
                                    Graphics g = Graphics.FromImage(img);
                                    g.DrawImage(record.OriginalPic, new Rectangle(0, 0, record.ObjectRect.Width, record.ObjectRect.Height), record.ObjectRect, GraphicsUnit.Pixel);
                                    record.ObjectPic = img;
                                    g.Dispose();
                                    // 再在原图上画目标框
                                    g = Graphics.FromImage(record.OriginalPic);
                                    g.DrawRectangle(new Pen(new SolidBrush(Color.Red), 2), record.ObjectRect);
                                    g.Dispose();
                                }
                                OriginalImageRetrieved(record);
                            }
                        }
                    }
                }
              
            }
            //if (ImageDownloaded != null)
            //{
            //    ImageDownloaded(m_searchSessionID, info.dwCameraID, info.dwTaskUnitID, (int)info.dwMoveObjID, info.ptImageData);
            //}
        }

        private void OnSearchResultRecordSelectedEvent(Tuple<SearchItem, SearchResultRecord> t)
        {
            if (t.Item1 != m_currSearchItem)
            {
                if (LostFocus != null)
                {
                    LostFocus(this, EventArgs.Empty);
                }
            }
            else
            {
            }
        }

        void ResultPageInfo_SelectedPageNumberChanged(object sender, EventArgs e)
        {
            try
            {
                Framework.Container.Instance.VideoSearchService.SwitchSearchPage(m_searchSessionID,
                    m_currSearchItem.TaskUnitId, (uint)m_PageInfo.Index);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "跳转检索页");
            }

        }

        void EditImageForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_EditImageForm.Dispose();
            m_EditImageForm = null;
        }

        #endregion

    }
}
