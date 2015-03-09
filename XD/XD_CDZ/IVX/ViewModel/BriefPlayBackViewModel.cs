using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Protocol.Model;
using BOCOM.DataModel;
using BOCOM.IVX.Framework;
using Microsoft.Practices.Prism.Events;
using BOCOM.IVX.Common;

namespace BOCOM.IVX.ViewModel
{
    public class BriefPlayBackViewModel : ViewModelBase,IEventAggregatorSubscriber
    {
        private DateTime m_currStartTime = new DateTime(0);
        private DateTime m_currEndTime = new DateTime(0);

        public string TimeInfo { get; set; }

        public BriefPlayBackViewModel(XtraSinglePlayer player)
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<BriefObjectPlayBackEvent>().Subscribe(OnBriefObjectPlayBack, ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayPosChangedEvent>().Subscribe(OnPlayPosChanged, ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayFailedEvent>().Subscribe(OnPlayFailed, ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayReadyEvent>().Subscribe(OnPlayReady, ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<OpenBriefPlaybackVideoEvent>().Subscribe(OnOpenBriefPlaybackVideo, ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitDeletedEvent>().Subscribe(OnTaskUnitDeleted, ThreadOption.WinFormUIThread);

            
            Framework.Container.Instance.RegisterEventSubscriber(this);

            PlayBtnImage = playvideoresource.播放5;
            PlayBtnCheckedImage = playvideoresource.播放3;
            PlayBtnMouseOverImage = playvideoresource.播放2;
            PlayBtnDisableImage = playvideoresource.播放5;
            PlayBtnOrigianlImage = playvideoresource.播放1;
            PlayBtnEnable = false;
            StopBtnEnable = false;
            GrabBtnEnable = false;
            GotoBtnEnable = false;
            EditVideoBtnEnable = false;
            //GotoBtnEnable = false;
            //ReplayBtnEnable = false;
            //PrivFrameBtnEnable  = false ;
            //NextFrameBtnEnable  = false ;
            //SlowBtnEnable  = false ;
            //FastBtnEnable  = false ;
            m_player = player;

        }
        private readonly XtraSinglePlayer m_player = null;



        public event Action<XtraSinglePlayer, uint,uint> PlayPosChange;


        public System.Drawing.Image PlayBtnImage { get; set; }
        public System.Drawing.Image PlayBtnCheckedImage { get; set; }
        public System.Drawing.Image PlayBtnMouseOverImage { get; set; }
        public System.Drawing.Image PlayBtnDisableImage { get; set; }
        public System.Drawing.Image PlayBtnOrigianlImage { get; set; }
        public bool PlayBtnEnable { get; set; }
        public bool StopBtnEnable { get; set; }
        public bool GrabBtnEnable { get; set; }
        public bool GotoBtnEnable { get; set; }

        ////public bool PrivFrameBtnEnable { get; set; }
        ////public bool NextFrameBtnEnable { get; set; }
        //public bool SlowBtnEnable { get; set; }
        //public bool FastBtnEnable { get; set; }
        //private bool m_gotoBtnEnable = false;
        //private bool m_replayBtnEnable = false;

        //public bool ReplayBtnEnable { get; set; }

        //public bool GotoBtnEnable { get; set; }
        public bool EditVideoBtnEnable { get; set; }




        public void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<BriefObjectPlayBackEvent>().Unsubscribe(OnBriefObjectPlayBack);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayPosChangedEvent>().Unsubscribe(OnPlayPosChanged);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayFailedEvent>().Unsubscribe(OnPlayFailed);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayReadyEvent>().Unsubscribe(OnPlayReady);
            Framework.Container.Instance.EvtAggregator.GetEvent<OpenBriefPlaybackVideoEvent>().Unsubscribe(OnOpenBriefPlaybackVideo);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitDeletedEvent>().Unsubscribe(OnTaskUnitDeleted);

        }

        private static bool CheckPlayStatus(IntPtr hWnd)
        {
            bool bRet = false;

            VideoStatusInfo statusinfo = null;
            try
            {
                statusinfo = Framework.Container.Instance.BriefVideoPlayService.GetPlayStatus(hWnd);
            }
            catch (SDKCallException ex)
            {
                SDKCallExceptionHandler.Handle(ex, "获取摘要播放状态");
                statusinfo = null;
            }

            if (statusinfo != null && statusinfo.PlayState != VideoStatusType.E_READY)
            {
                bRet = true;
            }
            else
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox("摘要播放服务正忙，请稍后再试。", Framework.Environment.PROGRAM_NAME, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
            }

            return bRet;
        }

        private void OnBriefObjectPlayBack(VodInfo info)
        {
            if (!CheckPlayStatus(m_player.HWnd))
            {
                return;
            }

            try
            {
                TaskUnitInfo taskunit = Framework.Container.Instance.TaskManagerService.GetTaskUnitById(info.VideoTaskUnitID);
                if (info != null)
                {
                    SelectedFile = taskunit;
                    PlayVideo(info.StartTime, info.EndTime);
                }
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "获取任务单元");
            }
        }

        private void OnOpenBriefPlaybackVideo(VodInfo info)
        {

            m_player.PlayVideoName = "";
            m_player.IsSuitWnd = false;
            m_player.EnabledEx = false ;
            m_player.SetStatusText("");
            try
            {
                Framework.Container.Instance.VideoPlayService.OpenVideo(m_player.HWnd, info.VideoTaskUnitID, info.StartTime, info.EndTime,true,true);
                uint w = 0;
                uint h = 0;
                Framework.Container.Instance.VideoPlayService.GetPlayResolution(m_player.HWnd, out w, out h);
                m_player.SetPlayResolution(w, h);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "摘要回放");
            }

            this.UpdateButtonStatus(m_player.HWnd);
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
            
            this.UpdateButtonStatus(info.HWnd);
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
                    if (info.StartPlayTime == 0 && info.EndPlayTime == 0)
                        PlayPosChange(player, info.PlayPercent, 1000);
                    else
                        PlayPosChange(player, info.CurrPlayTime - info.StartPlayTime, info.EndPlayTime - info.StartPlayTime);// info.PlayPercent);

               }
            }
        }

        private void OnPlayFailed(VideoStatusInfo info)
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

            CloseVideo();
        }
        private void OnTaskUnitDeleted(uint taskUnitId)
        { 
            VideoStatusInfo e = Framework.Container.Instance.VideoPlayService.GetPlayStatus(m_player.HWnd);
            if(e.VideoTaskUnitID == taskUnitId)
            {
                CloseVideo();
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
            if (m_player.HWnd == info.HWnd)
            {
                player = m_player;
                isFind = true;
            }


            if (!isFind)
                return;
            player.EnabledEx = true;
            player.SetStatusText("");
            this.UpdateButtonStatus(info.HWnd);
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
                case VideoStatusType.E_READY:
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
                case VideoStatusType.E_READY:
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
                case VideoStatusType.E_READY:
                    PlayBtnEnable = false;
                    StopBtnEnable = false;
                    GrabBtnEnable = false;
                    GotoBtnEnable = false;
                    EditVideoBtnEnable = false;
                    break;
                case VideoStatusType.E_NORMAL:
                case VideoStatusType.E_PAUSE:
                case VideoStatusType.E_SPEED:
                case VideoStatusType.E_STEP:
                case VideoStatusType.E_STEP_BACK:
                    PlayBtnEnable = true;
                    StopBtnEnable = true;
                    GrabBtnEnable = true ;
                    GotoBtnEnable = true ;
                    EditVideoBtnEnable = true;
                    break;
                case VideoStatusType.E_STOP:
                    PlayBtnEnable = true;
                    StopBtnEnable = false;
                    GrabBtnEnable = false;
                    GotoBtnEnable = false;
                    EditVideoBtnEnable = true;
                    break;

            }
            RaisePropertyChangedEvent("PlayBtnEnable");
            RaisePropertyChangedEvent("StopBtnEnable");
            RaisePropertyChangedEvent("GrabBtnEnable");
            RaisePropertyChangedEvent("GotoBtnEnable");
            RaisePropertyChangedEvent("EditVideoBtnEnable");
        }
        public TaskUnitInfo SelectedFile = new TaskUnitInfo();

        public void PlayVideo(DateTime st, DateTime et)
        {
            if (SelectedFile != null)
            {
                bool valid = CheckPlayStatus(m_player.HWnd);
                if (!valid)
                {
                    return;
                }

                m_player.PlayVideoName = SelectedFile.TaskUnitName;
                m_player.IsSuitWnd = false;
                m_player.EnabledEx = false ;
                m_player.SetStatusText("");
                try
                {
                    m_currStartTime = st;
                    m_currEndTime = et;
                    Framework.Container.Instance.VideoPlayService.PlayVideoPartialFile(m_player.HWnd, SelectedFile.TaskUnitID, st, et);
                }
                catch (SDKCallException ex)
                {
                    m_player.PlayVideoName = "";
                    m_player.EnabledEx = false;
                    Common.SDKCallExceptionHandler.Handle(ex, "摘要回放");

                }
                UpdateButtonStatus(m_player.HWnd);
            }
        }

        public void PlayVideo()
        {
            if (SelectedFile != null)
            {
                bool valid = CheckPlayStatus(m_player.HWnd);
                if (!valid)
                {
                    return;
                }

                m_player.PlayVideoName = SelectedFile.TaskUnitName;
                m_player.EnabledEx = false ;
                m_player.SetStatusText("");
                try
                {
                    m_currStartTime = SelectedFile.StartTime;
                    m_currEndTime = SelectedFile.EndTime;

                    Framework.Container.Instance.VideoPlayService.PlayVideo(m_player.HWnd, SelectedFile.TaskUnitID);

                }
                catch (SDKCallException ex)
                {
                    m_player.PlayVideoName = "";
                    m_player.EnabledEx = false;
                    Common.SDKCallExceptionHandler.Handle(ex, "摘要回放");

                }

                UpdateButtonStatus(m_player.HWnd);

            }
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
                    m_player.SetStatusText("");
                }
                else if (e.PlayState == VideoStatusType.E_STOP)
                {
                    ReplayBackVideo();
                }
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "播放或暂停视频");
            }

            UpdateButtonStatus(m_player.HWnd);
        }
        private void PauseVideo()
        {
            try
            {

                Framework.Container.Instance.VideoPlayService.VideoControl(m_player.HWnd, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_PAUSE, 0);

                m_player.SetStatusText("暂停");
                UpdateButtonStatus(m_player.HWnd);
            }
            catch (SDKCallException ex)
            {
                m_player.PlayVideoName = "";
                m_player.EnabledEx = false;
                Common.SDKCallExceptionHandler.Handle(ex, "暂停视频");
            }

        }

        private void ContinueVideo()
        {
            try
            {
                Framework.Container.Instance.VideoPlayService.VideoControl(m_player.HWnd, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_RESUME, 0);

                m_player.SetStatusText("");
                UpdateButtonStatus(m_player.HWnd);
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
                UpdateButtonStatus(m_player.HWnd);
            }
            catch (SDKCallException ex)
            { 
                m_player.PlayVideoName = "";
                m_player.EnabledEx = false;
                Common.SDKCallExceptionHandler.Handle(ex, "停止播放视频");
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

                UpdateButtonStatus(m_player.HWnd);
            }
            catch (SDKCallException ex)
            { 
                m_player.PlayVideoName = "";
                m_player.EnabledEx = false;
                Common.SDKCallExceptionHandler.Handle(ex, "关闭播放视频");
            }
        }
        
        public void SlowVideo()
        {
            try
            {
                Framework.Container.Instance.VideoPlayService.SetPlaySpeedMinus(m_player.HWnd);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "调整播放速度");
            }

            UpdateButtonStatus(m_player.HWnd);
        }

        public void FastVideo()
        {
            try
            {
                Framework.Container.Instance.VideoPlayService.SetPlaySpeedAdd(m_player.HWnd);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "调整播放速度");
            }

            UpdateButtonStatus(m_player.HWnd);
        }

        public void LoadPicture()
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

        public void PosVideo(uint pos, uint max)
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

                //VideoStatusInfo e
                //    = Framework.Container.Instance.VideoPlayService.GetPlayStatus(m_player.HWnd);
                //uint newpos = (e.StartPlayTime + pos) * 1000 / e.TotlePlayTime;
                //newpos = Math.Min(newpos, 1000);
                //Framework.Container.Instance.VideoPlayService.SetPlayPos(m_player.HWnd, newpos);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "定位播放");
            }
        }


        public void GotoOriginalVideoPlay()
        {
            Framework.Container.Instance.InteractionService.ShowMessageBox("GotoOriginalVideoPlay", Framework.Environment.PROGRAM_NAME);
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
                SDKCallExceptionHandler.Handle(ex, "获取播放状态");
            }
            if (e != null)
            {
                //PlayVideo(new DateTime().AddSeconds(e.StartPlayTime), new DateTime().AddSeconds(e.EndPlayTime));
                PlayVideo(m_currStartTime, m_currEndTime);
            }
        }

        public void GotoCompareSearch()
        {
            System.Drawing.Image img = null;
            VideoStatusInfo e = Framework.Container.Instance.VideoPlayService.GetPlayStatus(m_player.HWnd);
            if (e.PlayState != VideoStatusType.E_PAUSE)
            {
                PauseVideo();
            }

            try
            {
                img = Framework.Container.Instance.VideoPlayService.GrabPicture(m_player.HWnd);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "抓图");
            }
            if (img != null)
            {
                FormLoadImage image = new FormLoadImage(img, new System.Drawing.Rectangle());
                image.ShowDialog();
            }
        }


        public void GrabPicture()
        {
            try
            {
                System.Drawing.Image img = Framework.Container.Instance.VideoPlayService.GrabPicture(m_player.HWnd);
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

    }


}
