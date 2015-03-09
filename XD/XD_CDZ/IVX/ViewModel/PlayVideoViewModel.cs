using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Framework;
using BOCOM.IVX.Protocol.Model;
using BOCOM.DataModel;
using BOCOM.IVX.Protocol;
using System.Drawing;
using BOCOM.IVX.Common;
using System.Windows.Forms;


namespace BOCOM.IVX.ViewModel
{
    public class PlayVideoViewModel : ViewModelBase, IEventAggregatorSubscriber
    {

        public event EventHandler DrawModeChange;
        
        public event Action<XtraSinglePlayer, uint> PlayPosChange;
        
        private Dictionary<int, XtraSinglePlayer> m_playerList = new Dictionary<int, XtraSinglePlayer>();

        private const int MAX_WINDOW_COUNT = 9;

        public string TimeInfo { get; set; }
        
        public XtraSinglePlayer CurrPlayer { get; set; }
        
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


        public PlayVideoViewModel()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<CompareDrawModeChangeEvent>().Subscribe(OnDrawModeChange);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayPosChangedEvent>().Subscribe(OnPlayPosChanged, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayReadyEvent>().Subscribe(OnPlayReady, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayFailedEvent>().Subscribe(OnPlayFailed, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            //Framework.Container.Instance.EvtAggregator.GetEvent<CameraSelectionChangedEvent>().Subscribe(OnCameraSelectionChanged);
            Framework.Container.Instance.EvtAggregator.GetEvent<OCXPlayVideoEvent>().Subscribe(OnOCXPlayVideo);
            Framework.Container.Instance.EvtAggregator.GetEvent<OCXStopAllPlayVideoEvent>().Subscribe(OnOCXStopAllPlayVideo);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitDeletedEvent>().Subscribe(OnTaskUnitDeleted, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);

            Framework.Container.Instance.RegisterEventSubscriber(this);

            PlayBtnImage = playvideoresource.播放5;
            PlayBtnCheckedImage = playvideoresource.播放3;
            PlayBtnMouseOverImage = playvideoresource.播放2;
            PlayBtnDisableImage = playvideoresource.播放5;
            PlayBtnOrigianlImage = playvideoresource.播放1;
            PlayBtnEnable = false;
            StopBtnEnable  = false ;
            PrivFrameBtnEnable  = false ;
            NextFrameBtnEnable  = false ;
            SlowBtnEnable  = false ;
            FastBtnEnable  = false ;
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
            Framework.Container.Instance.EvtAggregator.GetEvent<OCXPlayVideoEvent>().Unsubscribe(OnOCXPlayVideo);
            Framework.Container.Instance.EvtAggregator.GetEvent<OCXStopAllPlayVideoEvent>().Unsubscribe(OnOCXStopAllPlayVideo);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitDeletedEvent>().Unsubscribe(OnTaskUnitDeleted);
        }
        
        public void SetPlayer(int index, XtraSinglePlayer player)
        {
            if (index >= 1 && index <= MAX_WINDOW_COUNT )
            {
                m_playerList[index] = player; 
            }
        }

        private void OnDrawModeChange(E_VDA_SEARCH_MOVE_OBJ_RANGE_FILTER_TYPE mode)
        {
            if (DrawModeChange != null)
                DrawModeChange(mode, null);
        }

        public List<TaskUnitInfo> SelectedFiles = new List<TaskUnitInfo>();

        public void SetSelectedFiles(List<object> list)
        {
            OnCameraSelectionChanged(list);
        }

        private XtraSinglePlayer GetPlayerByPlayHandle(IntPtr handle)
        {
            XtraSinglePlayer player = null;
            foreach (KeyValuePair<int, XtraSinglePlayer> pair in m_playerList)
            {
                if (pair.Value.HWnd == handle)
                {
                    player = pair.Value;
                    break;
                }
            }
            return player;
        }

        private void OnCameraSelectionChanged(List<object> list)
        {
            SelectedFiles.Clear();
            foreach (object o in list)
            {
                if (o is TaskUnitInfo)
                {
                    if (!SelectedFiles.Contains((TaskUnitInfo)o))
                        SelectedFiles.Add((TaskUnitInfo)o);
                }
            }

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

        private void OnPlayReady(VideoStatusInfo info)
        {
            bool isFind = false;
            XtraSinglePlayer player = null;
            foreach (KeyValuePair<int, XtraSinglePlayer> pair in m_playerList)
            {
                if (pair.Value.HWnd == info.HWnd)
                {
                    player = pair.Value;
                    isFind = true;
                    break;
                }
            }


            if (!isFind)
                return;

            player.EnabledEx = true ;
            player.SetStatusText("");
            UpdateButtonStatus(info.HWnd);
        }

        private void OnPlayPosChanged(VideoStatusInfo info)
        {
            XtraSinglePlayer player = GetPlayerByPlayHandle(info.HWnd);

            if (player == null)
            {
                return;
            }

            if (CurrPlayer == player)
            {
                try
                {
                    VideoStatusInfo e = Framework.Container.Instance.VideoPlayService.GetPlayStatus(info.HWnd);
                    if (e.PlayStatePriv != e.PlayState)
                        SetPlayVideoBtnStatus(e);

                    TimeInfo = string.Format("{0}/{1}", new DateTime().AddSeconds(e.CurrPlayTime).ToString("HH:mm:ss"), new DateTime().AddSeconds(e.TotlePlayTime).ToString("HH:mm:ss"));
                    RaisePropertyChangedEvent("TimeInfo");
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "获取播放状态");
                }
            }

            if (info.PlayState == VideoStatusType.E_STOP)
            {
                //player.PlayVideoName = "";
                player.EnabledEx = false;
                player.SetStatusText("");
            }
            else
            {
                if (PlayPosChange != null)
                    PlayPosChange(player, info.PlayPercent);
            }
        }

        private void OnPlayFailed(VideoStatusInfo info)
        {
            bool isFind = false;
            XtraSinglePlayer player = null;
            foreach (KeyValuePair<int, XtraSinglePlayer> pair in m_playerList)
            {
                if (pair.Value.HWnd == info.HWnd)
                {
                    player = pair.Value;
                    isFind = true;
                    break;
                }
            }

            if (!isFind)
                return;

            CloseVideo(player);
        }

        private void OnOCXPlayVideo(uint taskUnitID)
        {
            SelectedFiles.Clear();
            try
            {
                TaskUnitInfo info = Framework.Container.Instance.TaskManagerService.GetTaskUnitById(taskUnitID);
                if (info != null)
                {
                    SelectedFiles.Add(info);
                    PlayVideo();
                }
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "根据编号获取任务单元");
            }
        }

        private void OnOCXStopAllPlayVideo(string obj)
        {
            foreach (KeyValuePair<int, XtraSinglePlayer> pair in m_playerList)
            {
                CloseVideo(pair.Value);
            }
        }
        private void OnTaskUnitDeleted(uint taskunitid)
        {
            foreach (KeyValuePair<int, XtraSinglePlayer> pair in m_playerList)
            {
            VideoStatusInfo e
                = Framework.Container.Instance.VideoPlayService.GetPlayStatus(pair.Value.HWnd);
            if (e.VideoTaskUnitID == taskunitid)
                CloseVideo(pair.Value);
            }

        }

        
        public void SetSelectedPlayer(XtraSinglePlayer player)
        {
            foreach (KeyValuePair<int, XtraSinglePlayer> pair in m_playerList)
            {
                if (pair.Value == player)
                {
                    pair.Value.Selected = true;
                    VideoStatusInfo e 
                        = Framework.Container.Instance.VideoPlayService.GetPlayStatus(pair.Value.HWnd);
                    TimeInfo = string.Format("{0}/{1}", new DateTime().AddSeconds(e.CurrPlayTime).ToString("HH:mm:ss"), new DateTime().AddSeconds(e.TotlePlayTime ).ToString("HH:mm:ss"));
                    RaisePropertyChangedEvent("TimeInfo");

                    SetPlayVideoBtnStatus(e);
                }
                else
                    pair.Value.Selected = false;
            }

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
                case  VideoStatusType.E_NULL:
                //case VideoStatusType.E_READY:
                    PlayBtnEnable = false;
                    StopBtnEnable = false ;
                    PrivFrameBtnEnable = false;
                    NextFrameBtnEnable = false;
                    SlowBtnEnable = false;
                    FastBtnEnable = false;
                    GrabBtnEnable = false;
                    EditVideoBtnEnable = false;
                    MarkBtnEnable = false;
                    GotoBtnEnable = false;
                    break;
                case  VideoStatusType.E_NORMAL:
                case VideoStatusType.E_PAUSE:
                case VideoStatusType.E_SPEED:
                case VideoStatusType.E_STEP:
                case VideoStatusType.E_STEP_BACK:
                    PlayBtnEnable = true;
                    StopBtnEnable = true;
                    PrivFrameBtnEnable = true ;
                    NextFrameBtnEnable = true ;
                    SlowBtnEnable = true ;
                    FastBtnEnable = true ;
                    GrabBtnEnable = true  ;
                    EditVideoBtnEnable = true;
                    MarkBtnEnable = false  ;
                    GotoBtnEnable = true ;
                    break;
                case VideoStatusType.E_STOP:
                    PlayBtnEnable = true ;
                    StopBtnEnable = false ;
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

        public void PlayOrPauseVideo(bool canReplay = true)
        {
            try
            {
                VideoStatusInfo e
                    = Framework.Container.Instance.VideoPlayService.GetPlayStatus(CurrPlayer.HWnd);

                if (e.PlayState == VideoStatusType.E_NORMAL)
                {
                    Framework.Container.Instance.VideoPlayService.VideoControl(CurrPlayer.HWnd, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_PAUSE, 0);
                    CurrPlayer.SetStatusText("暂停");
                }
                else if (e.PlayState == VideoStatusType.E_PAUSE)
                {
                    Framework.Container.Instance.VideoPlayService.VideoControl(CurrPlayer.HWnd, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_RESUME, 0);
                    CurrPlayer.SetStatusText(GetSpeedText((int)e.PlaySpeed));
                }
                else if (e.PlayState == VideoStatusType.E_STOP)
                {
                    if (canReplay)
                    {
                        Framework.Container.Instance.VideoPlayService.VideoControl(CurrPlayer.HWnd, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_START, 0);
                        CurrPlayer.EnabledEx = true;
                        CurrPlayer.SetStatusText("");

                    }
                }
                else if (e.PlayState == VideoStatusType.E_SPEED)
                {
                    Framework.Container.Instance.VideoPlayService.VideoControl(CurrPlayer.HWnd, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_PAUSE, 0);
                    CurrPlayer.SetStatusText("暂停");
                }
                else if (e.PlayState == VideoStatusType.E_STEP || e.PlayState == VideoStatusType.E_STEP_BACK)
                {
                    Framework.Container.Instance.VideoPlayService.VideoControl(CurrPlayer.HWnd, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_RESUME, 0);
                    CurrPlayer.SetStatusText(GetSpeedText((int)e.PlaySpeed));
                }

                VideoStatusInfo e1
                    = Framework.Container.Instance.VideoPlayService.GetPlayStatus(CurrPlayer.HWnd);
                SetPlayVideoBtnStatus(e1);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "播放或暂停视频");
            }

        }
        

        public void PlayVideo()
        {
            int startIndex = -1;
            foreach (KeyValuePair<int, XtraSinglePlayer> pair in m_playerList)
            {
                if(pair.Value == CurrPlayer)
                    startIndex = pair.Key;
            }
            if (startIndex < 0)
                return;


            foreach (TaskUnitInfo info in SelectedFiles)
            {
                //if (!CheckPlayStatus(m_playerList[startIndex].HWnd))
                //{
                //    continue;
                //}

                m_playerList[startIndex].PlayVideoName = info.TaskUnitName;
                m_playerList[startIndex].IsSuitWnd = false;
                m_playerList[startIndex].EnabledEx = false;
                m_playerList[startIndex].SetStatusText("");
                try
                {
                    Framework.Container.Instance.VideoPlayService.OpenVideo(m_playerList[startIndex].HWnd, info.TaskUnitID);
                    uint w = 0;
                    uint h = 0;
                    Framework.Container.Instance.VideoPlayService.GetPlayResolution(m_playerList[startIndex].HWnd, out w, out h);
                    m_playerList[startIndex].SetPlayResolution(w, h);

                    Framework.Container.Instance.VideoPlayService.PlayVideo(m_playerList[startIndex].HWnd, info.TaskUnitID);

                    VideoStatusInfo e
                        = Framework.Container.Instance.VideoPlayService.GetPlayStatus(m_playerList[startIndex].HWnd);
                    SetPlayVideoBtnStatus(e);
                }
                catch (SDKCallException ex)
                {
                    m_playerList[startIndex].PlayVideoName = "";
                    m_playerList[startIndex].EnabledEx = false;
                    SDKCallExceptionHandler.Handle(ex, "播放视频");
                }
                startIndex++;
                if (startIndex >= MAX_WINDOW_COUNT)
                    startIndex = 1;
            }
        }

        public void PauseVideo()
        {
            try
            {
                Framework.Container.Instance.VideoPlayService.VideoControl(CurrPlayer.HWnd, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_PAUSE, 0);
                CurrPlayer.SetStatusText("暂停");
                UpdateButtonStatus(CurrPlayer.HWnd);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "暂停视频");
            }
        }

        public void ContinueVideo()
        {
            try
            {
                VideoStatusInfo e = Framework.Container.Instance.VideoPlayService.GetPlayStatus(CurrPlayer.HWnd);
                Framework.Container.Instance.VideoPlayService.VideoControl(CurrPlayer.HWnd, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_RESUME, 0);
                CurrPlayer.SetStatusText(GetSpeedText((int)e.PlaySpeed));
                UpdateButtonStatus(CurrPlayer.HWnd);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "恢复播放视频");
            }
        }

        public void StopVideo()
        {
            try
            {
                Framework.Container.Instance.VideoPlayService.VideoControl(CurrPlayer.HWnd, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_STOP, 0);
                CurrPlayer.SetStatusText("停止");
                CurrPlayer.EnabledEx = false;
                UpdateButtonStatus(CurrPlayer.HWnd);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "停止播放视频");
            }
        }

        public void CloseVideo()
        {
            try
            {
                Framework.Container.Instance.VideoPlayService.StopVideo(CurrPlayer.HWnd);
                CurrPlayer.PlayVideoName = "";
                CurrPlayer.EnabledEx = false;
                CurrPlayer.SetStatusText("");
                TimeInfo = "00:00:00/00:00:00";
                UpdateButtonStatus(CurrPlayer.HWnd);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "关闭视频");
            }
        }

        public void CloseVideo(XtraSinglePlayer player)
        {
            try
            {
                Framework.Container.Instance.VideoPlayService.StopVideo(player.HWnd);
                player.PlayVideoName = "";
                player.EnabledEx = false;
                player.SetStatusText("");
                TimeInfo = "00:00:00/00:00:00";
                UpdateButtonStatus(player.HWnd);
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
                Framework.Container.Instance.VideoPlayService.VideoControl(CurrPlayer.HWnd, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_STEP_BACK, 0);
                CurrPlayer.SetStatusText("上一帧");
                UpdateButtonStatus(CurrPlayer.HWnd);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "视频上一帧");
            }
        }

        public void NextFrameVideo()
        {
            try
            {
                Framework.Container.Instance.VideoPlayService.VideoControl(CurrPlayer.HWnd, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_STEP, 0);
                CurrPlayer.SetStatusText("下一帧");
                UpdateButtonStatus(CurrPlayer.HWnd);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "视频下一帧");
            }
        }

        public void SlowVideo()
        {
            try
            {
                int speed = Framework.Container.Instance.VideoPlayService.SetPlaySpeedMinus(CurrPlayer.HWnd);
                CurrPlayer.SetStatusText(GetSpeedText(speed));
                UpdateButtonStatus(CurrPlayer.HWnd);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "视频慢放");
            }
        }
        string  GetSpeedText(int val)
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
                int speed = Framework.Container.Instance.VideoPlayService.SetPlaySpeedAdd(CurrPlayer.HWnd);
                CurrPlayer.SetStatusText(GetSpeedText(speed));
                UpdateButtonStatus(CurrPlayer.HWnd);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "视频快放",false);
            }
        }

        public void LoadPicture()
        {
            
        }
        public void GrabPicture()
        {
            try
            {
                Image img = Framework.Container.Instance.VideoPlayService.GrabPicture(CurrPlayer.HWnd);
                if (img == null)
                {
                    Framework.Container.Instance.InteractionService.ShowMessageBox("抓图失败", Framework.Environment.PROGRAM_NAME);
                    return;
                }
                string time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string type = "视频截图";
                string fileName = CurrPlayer.PlayVideoName.Replace(".", "_") + type + time + ".jpg";
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

            VideoStatusInfo videoStatusInfo = Framework.Container.Instance.VideoPlayService.GetPlayStatus(CurrPlayer.HWnd);
            
            if (videoStatusInfo != null)
            {
                if (videoStatusInfo.PlayState != VideoStatusType.E_STOP)
                    StopVideo();

                TaskUnitInfo taskunit = Framework.Container.Instance.TaskManagerService.GetTaskUnitById( videoStatusInfo.VideoTaskUnitID);

                try
                {
                    FormVideoEdit edit = new FormVideoEdit(taskunit
                        , taskunit.StartTime
                        , taskunit.StartTime.AddSeconds(videoStatusInfo.TotlePlayTime));
                    edit.ShowDialog();
                }
                catch (Exception ex)
                {
                    SDKCallExceptionHandler.Handle(ex, "导出视频", true);
                }

            }
        }

        public void MarkVideo()
        {
            
        }

        public void PosVideo(uint pos)
        {
            try
            {
                Framework.Container.Instance.VideoPlayService.SetPlayPos(CurrPlayer.HWnd, pos);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "播放定位");
            }
        }
        public void GotoCompareSearch()
        {
            Image img = null;
            VideoStatusInfo e = Framework.Container.Instance.VideoPlayService.GetPlayStatus(CurrPlayer.HWnd);
            if (e.PlayState != VideoStatusType.E_PAUSE)
            {
                PauseVideo();
            }

            try
            {
                img = Framework.Container.Instance.VideoPlayService.GrabPicture(CurrPlayer.HWnd);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "抓图");
            }
            if (img != null)
            {
                FormLoadImage image = new FormLoadImage(img, new Rectangle());
                image.ShowDialog();
            }
        }

    }
}
