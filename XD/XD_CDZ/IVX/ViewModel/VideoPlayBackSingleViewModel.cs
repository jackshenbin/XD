using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Framework;
using BOCOM.DataModel;
using BOCOM.IVX.Protocol;
using System.Drawing;

namespace BOCOM.IVX.ViewModel
{
    class VideoPlayBackSingleViewModel : ViewModelBase, IEventAggregatorSubscriber
    {
        private E_VDA_SEARCH_MOVE_OBJ_RANGE_FILTER_TYPE m_drawMode = E_VDA_SEARCH_MOVE_OBJ_RANGE_FILTER_TYPE.E_SEARCH_MOVE_OBJ_RANGE_FILTER_NOUSE;
        private readonly XtraSinglePlayer m_player;

        private DateTime m_currStartTime = new DateTime(0);
        private DateTime m_currEndTime = new DateTime(0);

        public event EventHandler DrawModeChange;
        public event Action<XtraSinglePlayer, uint, uint> PlayPosChange;
        public System.Drawing.Image PlayBtnImage { get; set; }
        public System.Drawing.Image PlayBtnCheckedImage { get; set; }
        public System.Drawing.Image PlayBtnMouseOverImage { get; set; }
        public System.Drawing.Image PlayBtnDisableImage { get; set; }
        public System.Drawing.Image PlayBtnOrigianlImage { get; set; }
        public bool PlayBtnEnable { get; set; }
        public bool StopBtnEnable { get; set; }
        public bool PrivFrameBtnEnable { get; set; }
        public bool NextFrameBtnEnable { get; set; }
        public bool SlowBtnEnable { get; set; }
        public bool FastBtnEnable { get; set; }
        public bool ConcentratedBtnEnable { get; set; }
        public bool DownLoadConcentratedBtnEnable { get; set; }
        public bool GrabBtnEnable { get; set; }
        public bool EditVideoBtnEnable { get; set; }
        public bool MarkBtnEnable { get; set; }
        public bool GotoBtnEnable { get; set; }

        public SearchResourceResultType SearchType
        {
            get;
            set;
        }
        public string TimeInfo { get; set; }



        public VideoPlayBackSingleViewModel(XtraSinglePlayer player)
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<CompareDrawModeChangeEvent>().Subscribe(OnDrawModeChange);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayPosChangedEvent>().Subscribe(OnPlayPosChanged,Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayReadyEvent>().Subscribe(OnPlayReady, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayFailedEvent>().Subscribe(OnPlayFailed, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            //Framework.Container.Instance.EvtAggregator.GetEvent<CameraSelectionChangedEvent>().Subscribe(OnCameraSelectionChanged);
            Framework.Container.Instance.EvtAggregator.GetEvent<SearchResoultPlaybackRequestEvent>().Subscribe(OnSearchResoultPlayBack);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitDeletedEvent>().Subscribe(OnTaskUnitDeleted, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.RegisterEventSubscriber(this);


            m_player = player;
            PlayBtnImage = playvideoresource.播放5;
            PlayBtnCheckedImage = playvideoresource.播放3;
            PlayBtnMouseOverImage = playvideoresource.播放2;
            PlayBtnDisableImage = playvideoresource.播放5;
            PlayBtnOrigianlImage = playvideoresource.播放1;
            PlayBtnEnable = false;
            StopBtnEnable = false;
            PrivFrameBtnEnable = false;
            NextFrameBtnEnable = false;
            SlowBtnEnable = false;
            FastBtnEnable = false;
            GrabBtnEnable = false;
            ConcentratedBtnEnable = false;
            DownLoadConcentratedBtnEnable = false;
            EditVideoBtnEnable = false;
            MarkBtnEnable = false;
            GotoBtnEnable = false;
        }
        public void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<CompareDrawModeChangeEvent>().Unsubscribe(OnDrawModeChange);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayPosChangedEvent>().Unsubscribe(OnPlayPosChanged);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayReadyEvent>().Unsubscribe(OnPlayReady);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayFailedEvent>().Unsubscribe(OnPlayFailed);
            //Framework.Container.Instance.EvtAggregator.GetEvent<CameraSelectionChangedEvent>().Unsubscribe(OnCameraSelectionChanged);
            Framework.Container.Instance.EvtAggregator.GetEvent<SearchResoultPlaybackRequestEvent>().Unsubscribe(OnSearchResoultPlayBack);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitDeletedEvent>().Unsubscribe(OnTaskUnitDeleted);
        }

        private void SetPlayVideoBtnStatus(VideoStatusInfo e)
        {
            switch (e.PlayState)
            {
                case VideoStatusType.E_NULL:
                case VideoStatusType.E_STOP:
                case VideoStatusType.E_PAUSE:
                case VideoStatusType.E_STEP:
                case VideoStatusType.E_STEP_BACK:
                //case VideoStatusType.E_READY:
                    PlayBtnCheckedImage = playvideoresource.播放3;
                    PlayBtnMouseOverImage = playvideoresource.播放2;
                    PlayBtnDisableImage = playvideoresource.播放5;
                    PlayBtnOrigianlImage = playvideoresource.播放1;
                    break;
                case VideoStatusType.E_NORMAL:
                case VideoStatusType.E_SPEED:
                    PlayBtnCheckedImage = playvideoresource.暂停3;
                    PlayBtnMouseOverImage = playvideoresource.暂停2;
                    PlayBtnDisableImage = playvideoresource.暂停5;
                    PlayBtnOrigianlImage = playvideoresource.暂停1;
                    break;
            }
            RaisePropertyChangedEvent("PlayBtnCheckedImage");
            RaisePropertyChangedEvent("PlayBtnMouseOverImage");
            RaisePropertyChangedEvent("PlayBtnDisableImage");
            RaisePropertyChangedEvent("PlayBtnOrigianlImage");


            switch (e.PlayState)
            {
                case VideoStatusType.E_NULL:
                //case VideoStatusType.E_READY:
                    PlayBtnImage = playvideoresource.播放5;
                    break;
                case VideoStatusType.E_STOP:
                    PlayBtnImage = playvideoresource.播放1;
                    break;
                case VideoStatusType.E_PAUSE:
                    PlayBtnImage = playvideoresource.播放1;
                    break;
                case VideoStatusType.E_STEP:
                    PlayBtnImage = playvideoresource.播放1;
                    break;
                case VideoStatusType.E_STEP_BACK:
                    PlayBtnImage = playvideoresource.播放1;
                    break;
                case VideoStatusType.E_NORMAL:
                    PlayBtnImage = playvideoresource.暂停1;
                    break;
                case VideoStatusType.E_SPEED:
                    PlayBtnImage = playvideoresource.暂停1;
                    break;
            }
            RaisePropertyChangedEvent("PlayBtnImage");


            switch (e.PlayState)
            {
                case VideoStatusType.E_NULL:
                //case VideoStatusType.E_READY:
                    PlayBtnEnable = false;
                    StopBtnEnable = false;
                    PrivFrameBtnEnable = false;
                    NextFrameBtnEnable = false;
                    SlowBtnEnable = false;
                    FastBtnEnable = false;
                    GrabBtnEnable = false;
                    EditVideoBtnEnable = false;
                    MarkBtnEnable = false;
                    GotoBtnEnable = false;
                    break;
                case VideoStatusType.E_NORMAL:
                case VideoStatusType.E_PAUSE:
                case VideoStatusType.E_SPEED:
                case VideoStatusType.E_STEP:
                case VideoStatusType.E_STEP_BACK:
                    PlayBtnEnable = true;
                    StopBtnEnable = true;
                    PrivFrameBtnEnable = true;
                    NextFrameBtnEnable = true;
                    SlowBtnEnable = true;
                    FastBtnEnable = true;
                    GrabBtnEnable = true;
                    EditVideoBtnEnable = true ;
                    MarkBtnEnable = true;
                    GotoBtnEnable = true;
                    break;
                case VideoStatusType.E_STOP:
                    PlayBtnEnable = true;
                    StopBtnEnable = false;
                    PrivFrameBtnEnable = false;
                    NextFrameBtnEnable = false;
                    SlowBtnEnable = false;
                    FastBtnEnable = false;
                    GrabBtnEnable = false;
                    EditVideoBtnEnable = true;
                    MarkBtnEnable = false;
                    GotoBtnEnable = false;
                    break;
            }
            RaisePropertyChangedEvent("PlayBtnEnable");
            RaisePropertyChangedEvent("StopBtnEnable");
            RaisePropertyChangedEvent("PrivFrameBtnEnable");
            RaisePropertyChangedEvent("NextFrameBtnEnable");
            RaisePropertyChangedEvent("SlowBtnEnable");
            RaisePropertyChangedEvent("FastBtnEnable");
            RaisePropertyChangedEvent("ConcentratedBtnEnable");
            RaisePropertyChangedEvent("DownLoadConcentratedBtnEnable");
            RaisePropertyChangedEvent("EditVideoBtnEnable");
            RaisePropertyChangedEvent("GrabBtnEnable");
            RaisePropertyChangedEvent("MarkBtnEnable");
            RaisePropertyChangedEvent("GotoBtnEnable");


        }

        private void OnDrawModeChange(E_VDA_SEARCH_MOVE_OBJ_RANGE_FILTER_TYPE mode)
        {
            if (SearchType == SearchResourceResultType.Normal)
            {
                if (DrawModeChange != null)
                    DrawModeChange(mode, null);
                m_drawMode = mode;
                try
                {
                    Framework.Container.Instance.GraphicDrawService.SetPlayDrawType(m_drawMode);
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "设置绘图模式");
                }
            }
        }

        public TaskUnitInfo CurrFile { get; set; }
        public uint PlayPos { get; set; }


        private void OnCameraSelectionChanged(List<object> list)
        {
            CurrFile = null;
            foreach(object o in list)
            {
                if(o is TaskUnitInfo)
                {
                    CurrFile = (TaskUnitInfo)o;
                    break;
                }
            }

            if (CurrFile == null)
                CurrFile = new TaskUnitInfo() { TaskUnitID = 0 };
        }
        
        private void UpdateButtonStatus(IntPtr hWnd)
        {
            try
            {
                VideoStatusInfo e = Framework.Container.Instance.VideoPlayService.GetPlayStatus(hWnd);
                SetPlayVideoBtnStatus(e);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "获取播放状态");
            }
        }


        private void OnPlayPosChanged(VideoStatusInfo info)
        {
            bool isFind = false;
            XtraSinglePlayer player = null;
            if (m_player.HWnd == info.HWnd)
            {
                player = m_player;
                isFind = true;
            }


            if (!isFind)
                return;

            UpdateButtonStatus(info.HWnd);
            TimeInfo = string.Format("{0}/{1}", new DateTime().AddSeconds(info.CurrPlayTime).ToString("HH:mm:ss"), new DateTime().AddSeconds(info.TotlePlayTime).ToString("HH:mm:ss"));
            RaisePropertyChangedEvent("TimeInfo");


            if (info.PlayState == VideoStatusType.E_STOP)
            {
                //player.PlayVideoName = "";
                player.EnabledEx = false;
                player.SetStatusText("");
            }
            else
            {
                if (PlayPosChange != null)
                {
                    if(info.StartPlayTime ==0 && info.EndPlayTime ==0)
                    PlayPosChange(player,  info.PlayPercent,1000);
                    else
                    PlayPosChange(player, info.CurrPlayTime - info.StartPlayTime, info.EndPlayTime - info.StartPlayTime);// info.PlayPercent);
                }
            }
        }

        private void OnPlayReady(VideoStatusInfo info)
        {
            bool isFind = false;
            XtraSinglePlayer player = null;
            if (m_player.HWnd == info.HWnd)
            {
                player = m_player;
                isFind = true;
            }


            if (!isFind)
                return;
            player.EnabledEx = true;
            player.SetStatusText("");
            try
            {
                player.PlayVideoName = Framework.Container.Instance.TaskManagerService.GetTaskUnitById(info.VideoTaskUnitID).TaskUnitName;
                UpdateButtonStatus(info.HWnd);
                //Framework.Container.Instance.GraphicDrawService.SetPlayDrawType(m_drawMode);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "根据编号获取任务单元");
            }
        }

        private void OnPlayFailed(VideoStatusInfo info)
        {
            bool isFind = false;
            if (m_player.HWnd == info.HWnd)
            {
                isFind = true;
            }

            if (!isFind)
                return;

            CloseVideo();
        }

        public void PlayOrPauseVideo()
        {
            try
            {
                VideoStatusInfo e
                    = Framework.Container.Instance.VideoPlayService.GetPlayStatus(m_player.HWnd);

                if (e.PlayState == VideoStatusType.E_NORMAL)
                {
                    Framework.Container.Instance.VideoPlayService.VideoControl(m_player.HWnd, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_PAUSE, 0);
                    m_player.SetStatusText("暂停");
                }
                else if (e.PlayState == VideoStatusType.E_PAUSE)
                {
                    Framework.Container.Instance.VideoPlayService.VideoControl(m_player.HWnd, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_RESUME, 0);
                    m_player.SetStatusText(GetSpeedText((int)e.PlaySpeed));
                }
                else if (e.PlayState == VideoStatusType.E_STOP)
                {
                    if (e.StartPlayTime == 0 && e.EndPlayTime == 0)
                    {
                        Framework.Container.Instance.VideoPlayService.VideoControl(m_player.HWnd, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_START, 0);
                        m_player.EnabledEx = true ;
                        m_player.SetStatusText("");
                    }
                    else
                    {
                        ReplayBackVideo();
                    }
                }
                else if (e.PlayState == VideoStatusType.E_SPEED)
                {
                    Framework.Container.Instance.VideoPlayService.VideoControl(m_player.HWnd, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_PAUSE, 0);
                    m_player.SetStatusText("暂停");
                }
                else if (e.PlayState == VideoStatusType.E_STEP || e.PlayState == VideoStatusType.E_STEP_BACK)
                {
                    Framework.Container.Instance.VideoPlayService.VideoControl(m_player.HWnd, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_RESUME, 0);
                    m_player.SetStatusText(GetSpeedText((int)e.PlaySpeed));
                }

                VideoStatusInfo e1
                    = Framework.Container.Instance.VideoPlayService.GetPlayStatus(m_player.HWnd);
                SetPlayVideoBtnStatus(e1);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "播放或暂停视频");
            }
        }


        private void OnSearchResoultPlayBack(Tuple<SearchResultRecord, SearchType> payLoad)
        {
            if (payLoad != null && payLoad.Item1 != null)
            {
                bool matched = false;
                if (payLoad.Item2 == DataModel.SearchType.Normal)
                {
                    matched = SearchType == SearchResourceResultType.Normal;
                }
                else if (payLoad.Item2 == DataModel.SearchType.Face)
                {
                    matched = SearchType == SearchResourceResultType.Face;
                }
                else if (payLoad.Item2 == DataModel.SearchType.Vehicle)
                {
                    matched = SearchType == SearchResourceResultType.Vehicle;
                }
                else if (payLoad.Item2 == DataModel.SearchType.Compare)
                {
                    matched = SearchType == SearchResourceResultType.NoUse ;//SearchResourceResultType.Compare_Normal;
                }

                if (matched)
                {
                    PlayVideo(payLoad.Item1);
                }
            }

        }
        private void OnTaskUnitDeleted(uint taskUnitId)
        {
            VideoStatusInfo e = Framework.Container.Instance.VideoPlayService.GetPlayStatus(m_player.HWnd);
            if (e.VideoTaskUnitID == taskUnitId)
            {
                CloseVideo();
            }
        }

        

        public void PlayVideo(DateTime dtStart, DateTime dtEnd)
        {
            //if (!CheckPlayStatus(m_player.HWnd))
            //{
            //    return;
            //}
            bool needResetDrawType = true ;
            VideoStatusInfo last_e = Framework.Container.Instance.VideoPlayService.GetPlayStatus(m_player.HWnd);
            if (last_e != null && last_e.VideoTaskUnitID == CurrFile.TaskUnitID)
            {
                needResetDrawType = false;
            }

            m_player.PlayVideoName = CurrFile.TaskUnitName;
            m_player.IsSuitWnd = false;
            m_player.EnabledEx = false;
            m_player.SetStatusText("");
            try
            {
                if (SearchType == SearchResourceResultType.Normal)
                {
                    Framework.Container.Instance.GraphicDrawService.HPlayWnd = m_player.HWnd;
                }
                Framework.Container.Instance.VideoPlayService.OpenVideo(m_player.HWnd, CurrFile.TaskUnitID);
                uint w = 0;
                uint h = 0;
                Framework.Container.Instance.VideoPlayService.GetPlayResolution(m_player.HWnd, out w, out h);
                m_player.SetPlayResolution(w, h);

                m_currStartTime = dtStart;
                m_currEndTime = dtEnd;

                Framework.Container.Instance.VideoPlayService.PlayVideoPartialFile(m_player.HWnd, CurrFile.TaskUnitID, dtStart, dtEnd);

                if (needResetDrawType) 
                    Framework.Container.Instance.GraphicDrawService.SetPlayDrawType(m_drawMode);
            }
            catch (SDKCallException ex)
            {
                m_player.PlayVideoName = "";
                m_player.EnabledEx = false;
                Common.SDKCallExceptionHandler.Handle(ex, "播放视频");
            }
            UpdateButtonStatus(m_player.HWnd);
        }

        public void PlayVideo(SearchResultRecord record)
        {
            try
            {
                TaskUnitInfo taskUnit = Framework.Container.Instance.TaskManagerService.GetTaskUnitById(record.TaskUnitID);
                CurrFile = taskUnit;
                DateTime dtStart = record.TargetAppearTs.AddSeconds(-5);
                DateTime dtEnd = record.TargetDisappearTs.AddSeconds(10);
                //if (dtStart < taskUnit.StartTime)
                //{
                //    dtStart = taskUnit.StartTime;
                //}

                PlayVideo(dtStart, /*record.TargetDisappearTs*/dtEnd); // TODO: 因为不知道TaskUnitInfo的结束时间， 加5秒无法判断是否越界。暂时不加5
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "获取任务单元");
            }
        }

        public void PlayVideo()
        {
            //if (!CheckPlayStatus(m_player.HWnd))
            //{
            //    return;
            //}
            bool needResetDrawType = true;
            VideoStatusInfo last_e = Framework.Container.Instance.VideoPlayService.GetPlayStatus(m_player.HWnd);
            if (last_e != null && last_e.VideoTaskUnitID == CurrFile.TaskUnitID && SearchType != SearchResourceResultType.Normal)
            {
                needResetDrawType = false;
            }


            m_player.PlayVideoName = CurrFile.TaskUnitName;
            m_player.IsSuitWnd = false;
            m_player.EnabledEx = false;
            m_player.SetStatusText("");
            try
            {
                if (SearchType == SearchResourceResultType.Normal)
                {
                    Framework.Container.Instance.GraphicDrawService.HPlayWnd = m_player.HWnd;
                }
                Framework.Container.Instance.VideoPlayService.OpenVideo(m_player.HWnd, CurrFile.TaskUnitID);
                uint w = 0;
                uint h = 0;
                Framework.Container.Instance.VideoPlayService.GetPlayResolution(m_player.HWnd, out w, out h);
                m_player.SetPlayResolution(w, h);

                m_currStartTime = CurrFile.StartTime;
                m_currEndTime = CurrFile.EndTime;

                Framework.Container.Instance.VideoPlayService.PlayVideo(m_player.HWnd, CurrFile.TaskUnitID);

                if(needResetDrawType)
                    Framework.Container.Instance.GraphicDrawService.SetPlayDrawType(m_drawMode);
            }
            catch (SDKCallException ex)
            {
                m_player.PlayVideoName = "";
                m_player.EnabledEx = false;
                Common.SDKCallExceptionHandler.Handle(ex, "播放视频");
            }
            UpdateButtonStatus(m_player.HWnd);
        }

        public void PauseVideo()
        {
            try
            {
                Framework.Container.Instance.VideoPlayService.VideoControl(m_player.HWnd, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_PAUSE, 0);
                m_player.SetStatusText("暂停");
                VideoStatusInfo e = Framework.Container.Instance.VideoPlayService.GetPlayStatus(m_player.HWnd);
                SetPlayVideoBtnStatus(e);
            }
            catch (SDKCallException ex)
            {
                m_player.PlayVideoName = "";
                m_player.EnabledEx = false;
                Common.SDKCallExceptionHandler.Handle(ex, "暂停视频");
            }
        }

        public void ContinueVideo()
        {
            try
            { 
                VideoStatusInfo e = Framework.Container.Instance.VideoPlayService.GetPlayStatus(m_player.HWnd);
                Framework.Container.Instance.VideoPlayService.VideoControl(m_player.HWnd, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_RESUME, 0);
                m_player.SetStatusText(GetSpeedText((int)e.PlaySpeed));
                VideoStatusInfo e1 = Framework.Container.Instance.VideoPlayService.GetPlayStatus(m_player.HWnd);
                SetPlayVideoBtnStatus(e1);
            }
            catch (SDKCallException ex)
            {
                m_player.PlayVideoName = "";
                m_player.EnabledEx = false;
                Common.SDKCallExceptionHandler.Handle(ex, "恢复播放视频");
            }
        }

        public void StopVideo()
        {
            try
            {
                Framework.Container.Instance.VideoPlayService.VideoControl(m_player.HWnd, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_STOP, 0);
                m_player.SetStatusText("停止");
                m_player.EnabledEx = false;
                VideoStatusInfo e = Framework.Container.Instance.VideoPlayService.GetPlayStatus(m_player.HWnd);
                SetPlayVideoBtnStatus(e);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "停止视频");
            }
        }

        public void CloseVideo()
        {
            try
            {
                Framework.Container.Instance.VideoPlayService.StopVideo(m_player.HWnd);
                m_player.PlayVideoName = "";
                m_player.EnabledEx = false;
                m_player.SetStatusText("");
                TimeInfo = "00:00:00/00:00:00";
                VideoStatusInfo e = Framework.Container.Instance.VideoPlayService.GetPlayStatus(m_player.HWnd);
                SetPlayVideoBtnStatus(e);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "关闭视频");
            }
        }
        
        public void PrivFrameVideo()
        {
            try
            {
                Framework.Container.Instance.VideoPlayService.VideoControl(m_player.HWnd, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_STEP_BACK, 0);
                m_player.SetStatusText("上一帧");
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "上一帧");
            }
            UpdateButtonStatus(m_player.HWnd);
        }

        public void NextFrameVideo()
        {
            try
            {
                Framework.Container.Instance.VideoPlayService.VideoControl(m_player.HWnd, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_STEP, 0);
                m_player.SetStatusText("下一帧");
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "下一帧");
            }
            UpdateButtonStatus(m_player.HWnd);
        }

        public void SlowVideo()
        {
            try
            {
                int speed = Framework.Container.Instance.VideoPlayService.SetPlaySpeedMinus(m_player.HWnd);
                m_player.SetStatusText(GetSpeedText(speed));
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "慢放");
            }
            UpdateButtonStatus(m_player.HWnd);
        }

        string GetSpeedText(int val)
        {
            string strspeed = "";
            switch ((E_VDA_PLAY_SPEED)val)
            {
                case E_VDA_PLAY_SPEED.E_PLAYSPEED_SLOW16: strspeed = "1/16 倍"; break;
                case E_VDA_PLAY_SPEED.E_PLAYSPEED_SLOW8: strspeed = "1/8 倍"; break;
                case E_VDA_PLAY_SPEED.E_PLAYSPEED_SLOW4: strspeed = "1/4 倍"; break;
                case E_VDA_PLAY_SPEED.E_PLAYSPEED_SLOW2: strspeed = "1/2 倍"; break;
                case E_VDA_PLAY_SPEED.E_PLAYSPEED_NORMALSPEED: strspeed = ""; break;
                case E_VDA_PLAY_SPEED.E_PLAYSPEED_FAST2: strspeed = "2 倍"; break;
                case E_VDA_PLAY_SPEED.E_PLAYSPEED_FAST4: strspeed = "4 倍"; break;
                case E_VDA_PLAY_SPEED.E_PLAYSPEED_FAST8: strspeed = "8 倍"; break;
                case E_VDA_PLAY_SPEED.E_PLAYSPEED_FAST16: strspeed = "16 倍"; break;
            }
            return strspeed;
        }
        public void FastVideo()
        {
            try
            {
                int speed = Framework.Container.Instance.VideoPlayService.SetPlaySpeedAdd(m_player.HWnd);
                m_player.SetStatusText(GetSpeedText(speed));
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "快放",false);
            }
            UpdateButtonStatus(m_player.HWnd);
        }

        public void LoadPicture()
        {

        }
        public void GrabPicture()
        {
            try
            {

                Image img = Framework.Container.Instance.VideoPlayService.GrabPicture(m_player.HWnd);
                if (img == null)
                {
                    Framework.Container.Instance.InteractionService.ShowMessageBox("抓图失败", Framework.Environment.PROGRAM_NAME);
                    return;
                }
                string time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string type = "视频截图";
                string fileName = m_player.PlayVideoName.Replace(".", "_") + type + time + ".jpg";
                bool needdSave = true;

                System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
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

                    img.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);

                }
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "抓图");
            }
        }


        public void Concentrated()
        {

        }

        public void DownLoadConcentrated()
        {

        }

        public void EditVideo()
        {

            VideoStatusInfo videoStatusInfo = Framework.Container.Instance.VideoPlayService.GetPlayStatus(m_player.HWnd);

            if (videoStatusInfo != null)
            {
                if (videoStatusInfo.PlayState != VideoStatusType.E_STOP)
                    StopVideo();

                TaskUnitInfo taskunit = Framework.Container.Instance.TaskManagerService.GetTaskUnitById(videoStatusInfo.VideoTaskUnitID);

                try
                {
                    FormVideoEdit edit = new FormVideoEdit(taskunit
                        , m_currStartTime
                        , m_currEndTime);
                    edit.ShowDialog();
                }
                catch (Exception ex)
                {
                    BOCOM.IVX.Common.SDKCallExceptionHandler.Handle(ex, "导出视频", true);
                }

                //return;



            }
        }

        public void MarkVideo()
        {

        }

        public void PosVideo(uint pos,uint max)
        {
            try
            {
                if (max != 1000)
                {
                    VideoStatusInfo e
                        = Framework.Container.Instance.VideoPlayService.GetPlayStatus(m_player.HWnd);

                    uint newpos = (e.StartPlayTime + pos) * 1000 / e.TotlePlayTime;
                    newpos = Math.Min(newpos, 1000);
                    Framework.Container.Instance.VideoPlayService.SetPlayPos(m_player.HWnd, newpos);
                }
                else
                {
                    Framework.Container.Instance.VideoPlayService.SetPlayPos(m_player.HWnd, pos);
                }
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "定位播放");
            }
        }

        public void GotoCompareSearch()
        {
            VideoStatusInfo e = Framework.Container.Instance.VideoPlayService.GetPlayStatus(m_player.HWnd);
            if (e.PlayState != VideoStatusType.E_PAUSE)
            {
                PauseVideo();
            }

            try
            {
                Image img = Framework.Container.Instance.VideoPlayService.GrabPicture(m_player.HWnd);
                FormLoadImage image = new FormLoadImage(img, new Rectangle());
                image.ShowDialog();
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "抓图");
            }
        }

        public void ReplayBackVideo()
        {
            VideoStatusInfo e = null;
            try
            {
                e = Framework.Container.Instance.VideoPlayService.GetPlayStatus(m_player.HWnd);
            }
            catch (SDKCallException ex)
            {
                BOCOM.IVX.Common.SDKCallExceptionHandler.Handle(ex, "获取播放状态");
            }
            if (e != null)
            {
                PlayVideo(m_currStartTime, m_currEndTime);
            }
        }

        public void FullScreenPlayWnd(System.Drawing.Point wndPoint)
        {
            if (!Framework.Container.Instance.VideoPlayService.IsPlayHitGraph(m_player.HWnd, wndPoint))
                m_player.FullScreen();
        }

    }

}
