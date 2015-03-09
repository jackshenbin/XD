using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Framework;
using BOCOM.DataModel;
using BOCOM.IVX.Protocol;
using System.Drawing;
using System.Xml;
using System.Windows.Forms;
using BOCOM.IVX.Common;

namespace BOCOM.IVX.ViewModel
{
    class VideoEditViewModel : ViewModelBase, IEventAggregatorSubscriber
    {

        private readonly XtraSinglePlayer m_player;
        private readonly ucTimeTrack m_track;

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
        public bool GrabBtnEnable { get; set; }
        public bool EditVideoBtnEnable { get; set; }
        public string TimeInfo { get; set; }

        private DateTime m_startTime = new DateTime(0);
        private DateTime m_endTime = new DateTime(int.MaxValue);
        public DateTime StartTime 
        {
            get { return m_startTime; }
            set 
            {
                if (m_currFile != null)
                {
                    if (value >= m_currFile.StartTime)
                        m_startTime = value;
                    else
                        m_startTime = m_currFile.StartTime;
                   
                }
            }
        }
        public DateTime EndTime 
        {
            get { return m_endTime; }
            set
            {
                if (m_currFile != null)
                {
                    if (value <= m_currFile.EndTime)
                        m_endTime = value;
                    else
                        m_endTime = m_currFile.EndTime;
                }
            }
        }

        public DateTime RelativeStartTime 
        {
            get
            {
                if (m_currFile == null)
                    return new DateTime(0);
                else
                    return m_startTime.Subtract(new TimeSpan(m_currFile.StartTime.Ticks));
            }
            set 
            {
                if (m_currFile != null)
                    StartTime = m_currFile.StartTime.Add(new TimeSpan(value.Ticks));
            }
        }
        public DateTime RelativeEndTime 
        {
            get
            {
                if (m_currFile == null)
                    return new DateTime(0);
                else
                    return m_endTime.Subtract(new TimeSpan(m_currFile.StartTime.Ticks));
            }
            set
            {
                if (m_currFile != null)
                    EndTime = m_currFile.StartTime.Add(new TimeSpan(value.Ticks));
            }
        }

        List<VideoSection> TimeSection = new List<VideoSection>();

        public VideoEditViewModel(XtraSinglePlayer player,ucTimeTrack track)
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayPosChangedEvent>().Subscribe(OnPlayPosChanged,Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayReadyEvent>().Subscribe(OnPlayReady, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayFailedEvent>().Subscribe(OnPlayFailed, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.RegisterEventSubscriber(this);
            m_player = player;
            m_track = track;
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
            EditVideoBtnEnable = false;
        }
        public void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayPosChangedEvent>().Unsubscribe(OnPlayPosChanged);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayReadyEvent>().Unsubscribe(OnPlayReady);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayFailedEvent>().Unsubscribe(OnPlayFailed);
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
                    EditVideoBtnEnable = true;
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


        }

        private TaskUnitInfo m_currFile = null;
        public TaskUnitInfo CurrFile 
        {
            get { return m_currFile; }
            set { m_currFile = value; m_startTime = m_currFile.StartTime; m_endTime = m_currFile.EndTime; }
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
                    PlayPosChange(player,  info.CurrPlayTime,info.TotlePlayTime);
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
                    Framework.Container.Instance.VideoPlayService.PlayVideo(m_player.HWnd, CurrFile.TaskUnitID);

                    //Framework.Container.Instance.VideoPlayService.VideoControl(m_player.HWnd, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_START, 0);
                    m_player.EnabledEx = true ;
                    m_player.SetStatusText("");
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

        public void PlayVideo(DateTime dtStart, DateTime dtEnd)
        {
            //if (!CheckPlayStatus(m_player.HWnd))
            //{
            //    return;
            //}
            VideoStatusInfo last_e = Framework.Container.Instance.VideoPlayService.GetPlayStatus(m_player.HWnd);

            m_player.PlayVideoName = CurrFile.TaskUnitName;
            m_player.IsSuitWnd = false;
            m_player.EnabledEx = false;
            m_player.SetStatusText("");
            try
            {
                // Framework.Container.Instance.GraphicDrawService.HPlayWnd = m_player.HWnd;
                Framework.Container.Instance.VideoPlayService.OpenVideo(m_player.HWnd, CurrFile.TaskUnitID);
                uint w = 0;
                uint h = 0;
                Framework.Container.Instance.VideoPlayService.GetPlayResolution(m_player.HWnd, out w, out h);
                m_player.SetPlayResolution(w, h);

                Framework.Container.Instance.VideoPlayService.PlayVideoPartialFile(m_player.HWnd, CurrFile.TaskUnitID, dtStart, dtEnd);

            }
            catch (SDKCallException ex)
            {
                m_player.PlayVideoName = "";
                m_player.EnabledEx = false;
                Common.SDKCallExceptionHandler.Handle(ex, "播放视频");
            }
            UpdateButtonStatus(m_player.HWnd);
        }

        public void PlayVideo()
        {
            //if (!CheckPlayStatus(m_player.HWnd))
            //{
            //    return;
            //}

            m_player.PlayVideoName = CurrFile.TaskUnitName;
            m_player.IsSuitWnd = false;
            m_player.EnabledEx = false;
            m_player.SetStatusText("");
            try
            {
                // Framework.Container.Instance.GraphicDrawService.HPlayWnd = m_player.HWnd;
                Framework.Container.Instance.VideoPlayService.OpenVideo(m_player.HWnd, CurrFile.TaskUnitID);
                uint w = 0;
                uint h = 0;
                Framework.Container.Instance.VideoPlayService.GetPlayResolution(m_player.HWnd, out w, out h);
                m_player.SetPlayResolution(w, h);

                Framework.Container.Instance.VideoPlayService.PlayVideo(m_player.HWnd, CurrFile.TaskUnitID);

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
                Common.SDKCallExceptionHandler.Handle(ex, "快放");
            }
            UpdateButtonStatus(m_player.HWnd);
        }

        public void PosVideo(uint pos,uint max)
        {
            try
            {
                if (max != 1000)
                {
                    VideoStatusInfo e
                        = Framework.Container.Instance.VideoPlayService.GetPlayStatus(m_player.HWnd);

                    uint newpos = pos * 1000 / e.TotlePlayTime;
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

        public void ReplaySectionVideo()
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
                PlayVideo(new DateTime().AddSeconds(e.StartPlayTime), new DateTime().AddSeconds(e.EndPlayTime));
            }
        }

        private List<VideoSection> GetAllSection()
        {
            string xml = m_track.GetAllCutSection();
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xml);

            List<VideoSection> timeList = new List<VideoSection>();
            XmlNode CutSection = xDoc.SelectSingleNode("CutSection");
            foreach (XmlElement e in CutSection.ChildNodes)
            {
                XmlNode BeginTime = e.SelectSingleNode("BeginTime");
                XmlNode EndTime = e.SelectSingleNode("EndTime");
                timeList.Add(new VideoSection { startTime = UInt64.Parse(BeginTime.InnerText), endTime = UInt64.Parse(EndTime.InnerText) });
            }
            //timeList.Add(CurrentTimeSection);
            timeList.Sort((x, y) => x.startTime.CompareTo(y.startTime));
            return timeList;
        }

        public  bool ExportEditedVideo(string savePath)
        {
            //VideoStatusInfo videoStatusInfo = Framework.Container.Instance.VideoPlayService.GetPlayStatus(m_player.HWnd);
            //List<VideoSection> section = GetAllSection();
            //if (section.Count > 0)
            //{
                //DateTime start = StartTime;
                //DateTime end = new DateTime(0).AddMilliseconds(section[0].endTime);
                //string fileName = "";// Framework.Environment.VideoSavePath + @"\剪辑视频" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".h264";
                
                //using (SaveFileDialog dlg = new SaveFileDialog())
                //{
                //    dlg.InitialDirectory = Framework.Environment.VideoSavePath;
                //    dlg.FileName = string.Format("导出视频_{0}_{1}", CurrFile.TaskUnitName.Replace('.', '_'), DateTime.Now.ToString("yyyyMMddHHmmss") + ".h264");
                //    dlg.Filter = "*.*|*.*";

                //    if (dlg.ShowDialog() != DialogResult.OK)
                //    {
                //        return false;
                //    }
                //    fileName = dlg.FileName;
                //}

            System.IO.FileInfo fi = new System.IO.FileInfo(savePath);
                if (fi.Extension.ToLower() != ".h264")
                {
                    savePath += ".h264";
                }

                DownloadInfo downloadInfo = new DownloadInfo()
                {
                    VideoTaskUnitID = CurrFile.TaskUnitID,
                    IsDownloadAllFile = false,
                    LocalSaveFilePath = savePath,
                    StartTime = StartTime,
                    EndTime = EndTime,
                };

                try
                {
                    downloadInfo.SessionId = Framework.Container.Instance.VideoDownloadService.StartDownload(downloadInfo);
                    if (downloadInfo.SessionId > 0)
                    {
                        //Framework.Container.Instance.EvtAggregator.GetEvent<NavigateEvent>().Publish(UIFuncItemInfo.SHOWDOWNLOADLIST);
                        Framework.Container.Instance.EvtAggregator.GetEvent<AddVideoDownloadEvent>().Publish(downloadInfo);
                        return true;
                    }
                    
                }
                catch (SDKCallException ex)
                {
                    SDKCallExceptionHandler.Handle(ex, "导出视频", true);
                }
            //}
                return false;
            
        }



    }
        public class VideoSection
        {
            public UInt64 startTime;
            public UInt64 endTime;
        }

}
