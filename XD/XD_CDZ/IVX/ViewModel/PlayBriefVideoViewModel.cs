using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Framework;
using BOCOM.IVX.Protocol.Model;
using BOCOM.DataModel;
using BOCOM.IVX.Protocol;
using System.Drawing;
using Microsoft.Practices.Prism.Events;

namespace BOCOM.IVX.ViewModel
{
    public class PlayBriefVideoViewModel : ViewModelBase, IEventAggregatorSubscriber
    {
        public event Action<XtraSinglePlayer, uint> PlayPosChange;

        #region Fields

        private readonly XtraSinglePlayer m_player = null;

        private bool m_TimeOverlayer = true;
        private bool m_MoveObjOverlayer = false;
        private bool m_AvtionOverlayer = true;
        private bool m_AreaOverlayer = true;
        private uint m_currBriefTaskUnitID = 0;
        private E_VDA_BRIEF_DRAW_FILTER_TYPE DrawFilterType = E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_NULL;
        private VideoStatusType lastType = VideoStatusType.E_NULL;
        private bool lastActionOverlayer = true;
        private bool lastAreaOverlayer = true;

        private E_VDA_BRIEF_DENSITY m_briefDensityFilter = E_VDA_BRIEF_DENSITY.E_BRIEF_DENSITY_00;

        public List<TaskUnitInfo> SelectedFiles = new List<TaskUnitInfo>();

        private bool m_HasSelectedBriefObject;

        #endregion

        #region Properties

        public string TimeInfo { get; set; }

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
        public bool ConcentratedBtnEnable { get; set; }
        public bool DownLoadConcentratedBtnEnable { get; set; }
        public bool EditVideoBtnEnable { get; set; }
        public bool SetBriefBtnEnable { get; set; }

        public bool SaveObjectPicBtnEnable { get; set; }
        public bool PlayBackBtnEnable { get; set; }

        public bool ExportVideoBtnEnable { get; set; }

        public bool MarkBtnEnable { get; set; }

        public bool HidePlayBackWnd { get; set; }

        public bool HasSelectedBriefObject
        {
            get
            {
                return m_HasSelectedBriefObject;
            }
            set
            {
                if (m_HasSelectedBriefObject != value)
                {
                    m_HasSelectedBriefObject = value;
                    RaisePropertyChangedEvent("HasSelectedBriefObject");
                }
            }
        }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int NBriefDensityFilter
        {
            get { return (int)m_briefDensityFilter / 2; }
            set { m_briefDensityFilter = (E_VDA_BRIEF_DENSITY)(value * 2); }
        }
        public E_VDA_MOVEOBJ_TYPE BriefMoveObjTypeFilter { get; set; }
        public int BriefdwMoveObjColorFilter { get; set; }

        public bool TimeOverlayer
        {
            get { return m_TimeOverlayer; }
            set
            {
                try
                {
                    Framework.Container.Instance.BriefVideoPlayService.SetOverlayEnable(
                        m_player.HWnd, E_VDA_BRIEF_PLAY_DRAW_TYPE.E_BRIEF_PLAY_DRAW_OBJ_TIME, value);
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "摘要视频设置叠加");
                }
                m_TimeOverlayer = value;
            }
        }
        public bool MoveObjOverlayer
        {
            get { return m_MoveObjOverlayer; }
            set
            {
                try
                {
                    Framework.Container.Instance.BriefVideoPlayService.SetOverlayEnable(
                        m_player.HWnd, E_VDA_BRIEF_PLAY_DRAW_TYPE.E_BRIEF_PLAY_DRAW_OBJ_FRAME, value);
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "摘要视频设置叠加");
                }
                m_MoveObjOverlayer = value;
            }
        }

        public bool AvtionOverlayer
        {
            get { return m_AvtionOverlayer; }
            set
            {
                try
                {
                    Framework.Container.Instance.BriefVideoPlayService.SetOverlayEnable(
                        m_player.HWnd, E_VDA_BRIEF_PLAY_DRAW_TYPE.E_BRIEF_PLAY_DRAW_ACTION_FILTER, value);
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "摘要视频设置叠加");
                }
                m_AvtionOverlayer = value;
            }
        }

        public bool AreaOverlayer
        {
            get { return m_AreaOverlayer; }
            set
            {
                try
                {
                    Framework.Container.Instance.BriefVideoPlayService.SetOverlayEnable(
                        m_player.HWnd, E_VDA_BRIEF_PLAY_DRAW_TYPE.E_BRIEF_PLAY_DRAW_AREA_FILTER, value);
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "摘要视频设置叠加");
                }
                m_AreaOverlayer = value;
            }
        }

        public bool PasslineDrawFilter
        {
            get { return DrawFilterType == E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_PASSLINE; }
            set
            {
                if (!value && DrawFilterType == E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_PASSLINE)
                    DrawFilterType = E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_NULL;
                if (value)
                    DrawFilterType = E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_PASSLINE;

                //DrawFilterType = value ? E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_PASSLINE : E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_NULL;
                //RaisePropertyChangedEvent("PasslineDrawFilter");
                //RaisePropertyChangedEvent("BreakAreaDrawFilter");
                //RaisePropertyChangedEvent("SheildDrawFilter");
                //RaisePropertyChangedEvent("InterestDrawFilter");
            }
        }
        public bool BreakAreaDrawFilter
        {
            get { return DrawFilterType == E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_BREAK_AREA; }
            set
            {
                if (!value && DrawFilterType == E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_BREAK_AREA)
                    DrawFilterType = E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_NULL;
                if (value)
                    DrawFilterType = E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_BREAK_AREA;

                //DrawFilterType = value ? E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_BREAK_AREA : E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_NULL;
                //RaisePropertyChangedEvent("PasslineDrawFilter");
                //RaisePropertyChangedEvent("BreakAreaDrawFilter");
                //RaisePropertyChangedEvent("SheildDrawFilter");
                //RaisePropertyChangedEvent("InterestDrawFilter");
            }
        }
        public bool SheildDrawFilter
        {
            get { return DrawFilterType == E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_AREA_FILTER_TYPE_SHEILD; }
            set
            {
                if (!value && DrawFilterType == E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_AREA_FILTER_TYPE_SHEILD)
                    DrawFilterType = E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_NULL;
                if (value)
                    DrawFilterType = E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_AREA_FILTER_TYPE_SHEILD;

                //DrawFilterType = value ? E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_AREA_FILTER_TYPE_SHEILD : E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_NULL;
                //RaisePropertyChangedEvent("PasslineDrawFilter");
                //RaisePropertyChangedEvent("BreakAreaDrawFilter");
                //RaisePropertyChangedEvent("SheildDrawFilter");
                //RaisePropertyChangedEvent("InterestDrawFilter");
            }
        }
        public bool InterestDrawFilter
        {
            get { return DrawFilterType == E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_AREA_FILTER_TYPE_INTEREST; }
            set
            {
                if (!value && DrawFilterType == E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_AREA_FILTER_TYPE_INTEREST)
                    DrawFilterType = E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_NULL;
                if (value)
                    DrawFilterType = E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_AREA_FILTER_TYPE_INTEREST;

                //RaisePropertyChangedEvent("PasslineDrawFilter");
                //RaisePropertyChangedEvent("BreakAreaDrawFilter");
                //RaisePropertyChangedEvent("SheildDrawFilter");
                //RaisePropertyChangedEvent("InterestDrawFilter");
            }
        }

        #endregion

        #region Constructors

        public PlayBriefVideoViewModel(XtraSinglePlayer player)
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayPosChangedEvent>().Subscribe(OnPlayPosChanged, ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayReadyEvent>().Subscribe(OnPlayReady, ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayFailedEvent>().Subscribe(OnPlayFailed, ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlaySynthFailedEvent>().Subscribe(OnPlaySynthFailed, ThreadOption.WinFormUIThread);
            //Framework.Container.Instance.EvtAggregator.GetEvent<CameraSelectionChangedEvent>().Subscribe(OnCameraSelectionChanged, ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<BriefMouseClickChangedEvent>().Subscribe(OnBriefMouseClickChanged, ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<OCXPlayBriefVideoEvent>().Subscribe(OnOCXPlayBriefVideo, ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<OCXStopPlayBriefVideoEvent>().Subscribe(OnOCXStopPlayBriefVideo, ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitDeletedEvent>().Subscribe(OnTaskUnitDeleted, ThreadOption.WinFormUIThread);

            Framework.Container.Instance.RegisterEventSubscriber(this);

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
            SaveObjectPicBtnEnable = false;
            SetBriefBtnEnable = false;
            PlayBackBtnEnable = false;
            ExportVideoBtnEnable = false;
            MarkBtnEnable = false;
            HidePlayBackWnd = true ;

            m_briefDensityFilter = E_VDA_BRIEF_DENSITY.E_BRIEF_DENSITY_04;
            BriefMoveObjTypeFilter = E_VDA_MOVEOBJ_TYPE.E_VDA_MOVEOBJ_TYPE_ALL;
            BriefdwMoveObjColorFilter = 0x00ffffff;

            m_player = player;
        }

        #endregion

        #region Private helper functions

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

        private void OnOCXPlayBriefVideo(uint taskUnitID)
        {
            SelectedFiles.Clear();

            TaskUnitInfo info = null;
            try
            {
                info = Framework.Container.Instance.TaskManagerService.GetTaskUnitById(taskUnitID);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "根据编号获取任务单元");
            }

            if (info != null)
            {
                SelectedFiles.Add(info);
                PlayBriefVideo();
            }
        }

        private void OnOCXStopPlayBriefVideo(string obj)
        {
            CloseBriefVideo();
        }

        private void OnTaskUnitDeleted(uint taskunitid)
        {
            VideoStatusInfo e
                = Framework.Container.Instance.BriefVideoPlayService.GetPlayStatus(m_player.HWnd);
            if (e.VideoTaskUnitID == taskunitid)
                CloseBriefVideo();

        }

        private void UpdateButtonStatus(IntPtr hWnd)
        {
            try
            {
                VideoStatusInfo e = Framework.Container.Instance.BriefVideoPlayService.GetPlayStatus(hWnd);
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
                    PlayPosChange(player, info.PlayPercent);
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
                VideoStatusInfo e = Framework.Container.Instance.BriefVideoPlayService.GetPlayStatus(info.HWnd);
                SetPlayVideoBtnStatus(e);

                //string newtime = string.Format("浓缩时长：{0}分{1}秒", e.TotlePlayTime / 60, e.TotlePlayTime % 60);
                //if (newtime != TimeInfo)
                //{
                //    TimeInfo = newtime;
                //    RaisePropertyChangedEvent("TimeInfo");
                //}
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "获取播放状态");
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

            CloseBriefVideo();
        }

        private void OnPlaySynthFailed(VideoStatusInfo info)
        {
            bool isFind = false;

            if (m_player.HWnd == info.HWnd)
            {
                isFind = true;
            }


            if (!isFind)
                return;

            CloseBriefVideo();
            //StopBriefVideo();
            if (info.ErrorCode == DataModel.Common.APPERR_BVODS_BRIEF_OBJECT_NULL)
            {
                Common.SDKCallExceptionHandler.Handle(new SDKCallException(info.ErrorCode, IVXProtocol.GetErrorMsg(info.ErrorCode)), "摘要合成", false);
                string msg = string.Format("您设置的条件下无摘要目标，请重新设置。");
                Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME,
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                PlayBriefVideo();

            }
            else
            {
                Common.SDKCallExceptionHandler.Handle(new SDKCallException(info.ErrorCode, IVXProtocol.GetErrorMsg(info.ErrorCode)), "摘要合成");
            }
            //Framework.Container.Instance.InteractionService.ShowMessageBox("摘要合成失败。", Framework.Environment.PROGRAM_NAME);

        }

        private void OnBriefMouseClickChanged(BriefMouseClickInfo info)
        {
            bool isselected = Framework.Container.Instance.BriefVideoPlayService.IsSelectBriefMoveObj(m_player.HWnd);
            switch (info.MouseClickType)
            {
                case E_VDA_BRIEF_WND_MOUSE_OPT_TYPE.E_BRIEF_WND_MOUSE_LCLICK:
                    try
                    {
                        if (Framework.Container.Instance.BriefVideoPlayService.IsSelectBriefMoveObj(m_player.HWnd))
                        {
                            PauseBriefVideo();
                        }

                        else
                        {
                            PlayOrPauseVideo(false);
                            RaisePropertyChangedEvent("PlayBackBtnEnable");
                        }
                    }
                    catch (SDKCallException ex)
                    {
                        Common.SDKCallExceptionHandler.Handle(ex, "获取摘要视频是否选中运动物");
                    }
                    //PlayBackBtnEnable =
                    //    Framework.Container.Instance.BriefVideoPlayService.IsSelectBriefMoveObj(m_player.HWnd);
                    //if (PlayBackBtnEnable)
                    //    Framework.Container.Instance.BriefVideoPlayService.GetSelectBriefMoveObjInfo(m_player.HWnd);
                    break;
                case E_VDA_BRIEF_WND_MOUSE_OPT_TYPE.E_BRIEF_WND_MOUSE_LDCLICK:
                    if (isselected)
                    {
                        ObjectPlayBack();
                    }
                    else
                    {
                        m_player.FullScreen();
                    }
                    break;
                case E_VDA_BRIEF_WND_MOUSE_OPT_TYPE.E_BRIEF_WND_MOUSE_RCLICK:
                    if (isselected)
                    {
                        Dictionary<string, DevExpress.XtraBars.ItemClickEventHandler> btnList
                            = new Dictionary<string, DevExpress.XtraBars.ItemClickEventHandler>();
                        btnList.Add("目标回放", barBtn_ItemClick1);
                        btnList.Add("保存目标图片", barBtn_ItemClick2);
                        btnList.Add("转到以图搜图", barBtn_ItemClick3);
                        m_player.ShowMenu(btnList, new Point ((int)info.X,(int)info.Y));
                    }
                    break;
                case E_VDA_BRIEF_WND_MOUSE_OPT_TYPE.E_BRIEF_WND_MOUSE_RDCLICK:
                    break;
            }
            HasSelectedBriefObject = isselected;

        }
        private void barBtn_ItemClick1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ObjectPlayBack();
        }

        private void barBtn_ItemClick2(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveObjectPic();
        }

        private void barBtn_ItemClick3(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GotoCompareSearch();
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
                    PrivFrameBtnEnable = false;
                    NextFrameBtnEnable = false;
                    SlowBtnEnable = false;
                    FastBtnEnable = false;
                    SetBriefBtnEnable = false;
                    //SaveObjectPicBtnEnable = false;
                    //PlayBackBtnEnable = false;
                    HasSelectedBriefObject = false;
                    ExportVideoBtnEnable = false;
                    MarkBtnEnable = false;
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
                    SetBriefBtnEnable = true;

                    //SaveObjectPicBtnEnable = true;
                    //PlayBackBtnEnable = true;
                    HasSelectedBriefObject = false;

                    ExportVideoBtnEnable = true;
                    MarkBtnEnable = true;
                    break;
                case VideoStatusType.E_STOP:
                    PlayBtnEnable = true;
                    StopBtnEnable = false;
                    PrivFrameBtnEnable = false;
                    NextFrameBtnEnable = false;
                    SlowBtnEnable = false;
                    FastBtnEnable = false;

                    //SaveObjectPicBtnEnable = false;
                    //PlayBackBtnEnable = false;
                    HasSelectedBriefObject = false;

                    SetBriefBtnEnable = false;

                    ExportVideoBtnEnable = false;
                    MarkBtnEnable = false;
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
            RaisePropertyChangedEvent("SetBriefBtnEnable");
            RaisePropertyChangedEvent("PlayBackBtnEnable");
            RaisePropertyChangedEvent("ExportVideoBtnEnable");
            RaisePropertyChangedEvent("MarkBtnEnable");
            RaisePropertyChangedEvent("SaveObjectPicBtnEnable");


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
                BOCOM.IVX.Common.SDKCallExceptionHandler.Handle(ex, "获取摘要播放状态");
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

        private void ResetBriefParam()
        {
            m_TimeOverlayer = true;
            m_MoveObjOverlayer = false;
            m_AvtionOverlayer = true;
            m_AreaOverlayer = true;
            RaisePropertyChangedEvent("TimeOverlayer");
            RaisePropertyChangedEvent("MoveObjOverlayer");
            RaisePropertyChangedEvent("AvtionOverlayer");
            RaisePropertyChangedEvent("AreaOverlayer");

            DrawFilterType = E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_NULL;
            RaisePropertyChangedEvent("PasslineDrawFilter");
            RaisePropertyChangedEvent("BreakAreaDrawFilter");
            RaisePropertyChangedEvent("SheildDrawFilter");
            RaisePropertyChangedEvent("InterestDrawFilter");

            m_briefDensityFilter = E_VDA_BRIEF_DENSITY.E_BRIEF_DENSITY_04;
            BriefMoveObjTypeFilter = E_VDA_MOVEOBJ_TYPE.E_VDA_MOVEOBJ_TYPE_ALL;
            BriefdwMoveObjColorFilter = 0x00FFFFFF;

            RaisePropertyChangedEvent("NBriefDensityFilter");
            RaisePropertyChangedEvent("BriefMoveObjTypeFilter");
            RaisePropertyChangedEvent("BriefdwMoveObjColorFilter");


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

        #endregion

        #region Public helper functions

        public void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayPosChangedEvent>().Unsubscribe(OnPlayPosChanged);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayReadyEvent>().Unsubscribe(OnPlayReady);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlayFailedEvent>().Unsubscribe(OnPlayFailed);
            Framework.Container.Instance.EvtAggregator.GetEvent<PlaySynthFailedEvent>().Unsubscribe(OnPlayFailed);
            //Framework.Container.Instance.EvtAggregator.GetEvent<CameraSelectionChangedEvent>().Unsubscribe(OnCameraSelectionChanged);
            Framework.Container.Instance.EvtAggregator.GetEvent<BriefMouseClickChangedEvent>().Unsubscribe(OnBriefMouseClickChanged);
            Framework.Container.Instance.EvtAggregator.GetEvent<OCXPlayBriefVideoEvent>().Unsubscribe(OnOCXPlayBriefVideo);
            Framework.Container.Instance.EvtAggregator.GetEvent<OCXStopPlayBriefVideoEvent>().Unsubscribe(OnOCXStopPlayBriefVideo);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitDeletedEvent>().Unsubscribe(OnTaskUnitDeleted);
        }

        public void SetSelectedFiles(List<object> list)
        {
            OnCameraSelectionChanged(list);
        }

        public void PlayBriefVideo()
        {
            if (!CheckPlayStatus(m_player.HWnd))
                return;

            if (SelectedFiles.Count > 0)
            {

                TaskUnitInfo info = SelectedFiles[0];
                m_player.PlayVideoName = info.TaskUnitName;
                m_player.EnabledEx = false;

                ResetBriefParam();

                try
                {
                    Framework.Container.Instance.BriefVideoPlayService.ClearDrawFilterGraph(m_player.HWnd, E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_NULL);
                    Framework.Container.Instance.BriefVideoPlayService.PlayVideo(
                        m_player.HWnd,
                        info.TaskUnitID,
                        m_briefDensityFilter,
                        BriefMoveObjTypeFilter,
                        (uint)Framework.Container.Instance.ColorService.GetObjectColorIndex(Color.FromArgb(BriefdwMoveObjColorFilter))
                        );
                    SetBriefDrawFilter(DrawFilterType);

                    TimeOverlayer = m_TimeOverlayer;
                    MoveObjOverlayer = m_MoveObjOverlayer;
                    AvtionOverlayer = m_AvtionOverlayer;
                    AreaOverlayer = m_AreaOverlayer;
                    m_currBriefTaskUnitID = info.TaskUnitID;

                    VodInfo vodinfo = new VodInfo();
                    vodinfo.VideoTaskUnitID = info.TaskUnitID;
                    vodinfo.StartTime = new DateTime();
                    vodinfo.IsPlayAllFile = true;
                    vodinfo.EndTime = new DateTime();

                    Framework.Container.Instance.EvtAggregator.GetEvent<OpenBriefPlaybackVideoEvent>().Publish(vodinfo);
                }
                catch (SDKCallException ex)
                {
                    m_player.PlayVideoName = "";
                    m_player.EnabledEx = false;
                    Common.SDKCallExceptionHandler.Handle(ex, "摘要播放");
                    // Framework.Container.Instance.InteractionService.ShowMessageBox("失败：[" + ex.ErrorCode + "]" + ex.Message, Framework.Environment.PROGRAM_NAME);
                }
                finally
                {
                    UpdateButtonStatus(m_player.HWnd);
                }
            }
        }
        public void PlayBriefVideo(List<object> list)
        {
            if (!CheckPlayStatus(m_player.HWnd))
                return;
            OnCameraSelectionChanged(list);

            PlayBriefVideo();
        }

        public void PlayOrPauseVideo(bool canReplay = true)
        {
            try
            {
                VideoStatusInfo e
                    = Framework.Container.Instance.BriefVideoPlayService.GetPlayStatus(m_player.HWnd);

                if (e.PlayState == VideoStatusType.E_NORMAL)
                {
                    Framework.Container.Instance.BriefVideoPlayService.VideoControl(m_player.HWnd, E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_PAUSE, 0);
                    m_player.SetStatusText("暂停");
                }
                else if (e.PlayState == VideoStatusType.E_PAUSE)
                {
                    Framework.Container.Instance.BriefVideoPlayService.VideoControl(m_player.HWnd, E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_RESUME, 0);
                    m_player.SetStatusText(GetSpeedText((int)e.PlaySpeed));
                }
                else if (e.PlayState == VideoStatusType.E_STOP)
                {
                    if (canReplay)
                    {
                        Framework.Container.Instance.BriefVideoPlayService.VideoControl(m_player.HWnd, E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_START, 0);
                        m_player.SetStatusText("");
                        m_player.EnabledEx = false;
                    }
                }
                else if (e.PlayState == VideoStatusType.E_SPEED)
                {
                    Framework.Container.Instance.BriefVideoPlayService.VideoControl(m_player.HWnd, E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_PAUSE, 0);
                    m_player.SetStatusText("暂停");
                }

                VideoStatusInfo e1
                    = Framework.Container.Instance.BriefVideoPlayService.GetPlayStatus(m_player.HWnd);
                SetPlayVideoBtnStatus(e1);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "播放或暂停摘要视频");
            }

        }

        public void PauseBriefVideo()
        {
            try
            {
                VideoStatusInfo e
                = Framework.Container.Instance.BriefVideoPlayService.GetPlayStatus(m_player.HWnd);

                if (e.PlayState != VideoStatusType.E_PAUSE && e.PlayState != VideoStatusType.E_STOP)
                {
                    Framework.Container.Instance.BriefVideoPlayService.VideoControl(m_player.HWnd, E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_PAUSE, 0);
                    m_player.SetStatusText("暂停");
                }
                VideoStatusInfo e1
                    = Framework.Container.Instance.BriefVideoPlayService.GetPlayStatus(m_player.HWnd);
                SetPlayVideoBtnStatus(e1);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "暂停摘要视频");
            }
        }

        public void ContinueBriefVideo()
        {
            try
            {
                VideoStatusInfo e
                = Framework.Container.Instance.BriefVideoPlayService.GetPlayStatus(m_player.HWnd);

                if (e.PlayState != VideoStatusType.E_NORMAL)
                    Framework.Container.Instance.BriefVideoPlayService.VideoControl(m_player.HWnd, E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_RESUME, 0);
                m_player.SetStatusText(GetSpeedText((int)e.PlaySpeed));

                VideoStatusInfo e1
                    = Framework.Container.Instance.BriefVideoPlayService.GetPlayStatus(m_player.HWnd);
                SetPlayVideoBtnStatus(e1);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "恢复播放摘要视频");
            }
        }

        public void StopBriefVideo()
        {
            try
            {
                Framework.Container.Instance.BriefVideoPlayService.VideoControl(m_player.HWnd, E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_STOP, 0);
                m_player.SetStatusText("停止");
                m_player.EnabledEx = false;

                VideoStatusInfo e
             = Framework.Container.Instance.BriefVideoPlayService.GetPlayStatus(m_player.HWnd);
                SetPlayVideoBtnStatus(e);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "停止播放摘要视频");
            }

        }

        public void CloseBriefVideo()
        {
            if (!CheckPlayStatus(m_player.HWnd))
                return;

            try
            {
                Framework.Container.Instance.BriefVideoPlayService.StopVideo(m_player.HWnd);
                m_player.SetStatusText("");
                m_player.EnabledEx = false;
                m_player.PlayVideoName = "";
                TimeInfo = "00:00:00/00:00:00";

                m_currBriefTaskUnitID = 0;
                VideoStatusInfo e
            = Framework.Container.Instance.BriefVideoPlayService.GetPlayStatus(m_player.HWnd);
                SetPlayVideoBtnStatus(e);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "关闭播放摘要视频");
            }
        }

        public void SlowBriefVideo()
        {
            try
            {
                int speed = Framework.Container.Instance.BriefVideoPlayService.SetPlaySpeedMinus(m_player.HWnd);
                m_player.SetStatusText(GetSpeedText(speed));
                VideoStatusInfo e
            = Framework.Container.Instance.BriefVideoPlayService.GetPlayStatus(m_player.HWnd);
                SetPlayVideoBtnStatus(e);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "摘要视频慢放");
            }
        }

        public void FastBriefVideo()
        {
            try
            {
                int speed = Framework.Container.Instance.BriefVideoPlayService.SetPlaySpeedAdd(m_player.HWnd);
                m_player.SetStatusText(GetSpeedText(speed));
                VideoStatusInfo e
            = Framework.Container.Instance.BriefVideoPlayService.GetPlayStatus(m_player.HWnd);
                SetPlayVideoBtnStatus(e);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "摘要视频快放");
            }
        }

        public void LoadPicture()
        {

        }

        public void GrabPicture()
        {

        }

        public void ObjectPlayBack()
        {
            try
            {
                HidePlayBackWnd = false ;
                RaisePropertyChangedEvent("HidePlayBackWnd");
                bool isselected = Framework.Container.Instance.BriefVideoPlayService.IsSelectBriefMoveObj(m_player.HWnd);
                if (isselected)
                {
                    BriefMoveobjInfo bfinfo = Framework.Container.Instance.BriefVideoPlayService.GetSelectBriefMoveObjInfo(m_player.HWnd);

                    VodInfo info = new VodInfo();
                    info.VideoTaskUnitID = m_currBriefTaskUnitID;
                    info.StartTime = bfinfo.BeginTimeS;
                    info.IsPlayAllFile = false;
                    info.EndTime = bfinfo.EndTimeS;
                    Framework.Container.Instance.EvtAggregator.GetEvent<BriefObjectPlayBackEvent>().Publish(info); ;
                }
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "回溯视频");
            }
        }

        public void SaveObjectPic()
        {
            try
            {
                bool isselected = Framework.Container.Instance.BriefVideoPlayService.IsSelectBriefMoveObj(m_player.HWnd);
                if (isselected)
                {
                    Image img = Framework.Container.Instance.BriefVideoPlayService.GetObjectPicture(m_player.HWnd);
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
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "截取目标图片");
            }
        }
        public void GotoCompareSearch()
        {
            bool isselected = Framework.Container.Instance.BriefVideoPlayService.IsSelectBriefMoveObj(m_player.HWnd);
            if (isselected)
            {
                VideoStatusInfo e = Framework.Container.Instance.BriefVideoPlayService.GetPlayStatus(m_player.HWnd);
                if (e.PlayState != VideoStatusType.E_PAUSE)
                {
                    PauseBriefVideo();
                }

                Image img = Framework.Container.Instance.BriefVideoPlayService.GetObjectPicture(m_player.HWnd);
                if (img == null)
                {
                    Framework.Container.Instance.InteractionService.ShowMessageBox("抓图失败", Framework.Environment.PROGRAM_NAME);
                    return;
                }

                else
                {
                    //FormLoadImage image = new FormLoadImage(img, new System.Drawing.Rectangle());
                    //image.ShowDialog();

                    // 需要复制一份Image
                    img = new Bitmap(img);
                    Framework.Container.Instance.EvtAggregator.GetEvent<GotoCompareSearchEvent>().Publish("");
                    CompareImageInfo info = new CompareImageInfo
                    {
                        Image = img,
                        RegionImage = img,
                        ImageRectangle = new Rectangle(new Point(), img.Size),
                        ImageType = ImageType.Object,
                    };
                    Framework.Container.Instance.EvtAggregator.GetEvent<SetCompareImageInfoEvent>().Publish(info);


                }
            }
        }

        public void EditVideo()
        {

        }

        public void MarkVideo()
        {
        }

        public void PosBriefVideo(uint pos)
        {
            try
            {
                Framework.Container.Instance.BriefVideoPlayService.SetPlayPos(m_player.HWnd, pos);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "摘要视频定位播放");
            }
        }

        public void SetBriefDrawFilter(E_VDA_BRIEF_DRAW_FILTER_TYPE type)
        {
            try
            {
                Framework.Container.Instance.BriefVideoPlayService.SetDrawFilter(m_player.HWnd, type);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "摘要视频设置过滤");
            }

        }

        public void ClearBriefDrawFilter(E_VDA_BRIEF_DRAW_FILTER_TYPE type)
        {
            try
            {
                Framework.Container.Instance.BriefVideoPlayService.ClearDrawFilterGraph(m_player.HWnd, type);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "摘要视频清除过滤条件");
            }
        }

        public void BeginBriefEdit()
        {
            try
            {
                VideoStatusInfo e
                    = Framework.Container.Instance.BriefVideoPlayService.GetPlayStatus(m_player.HWnd);
                lastType = e.PlayState;
                lastActionOverlayer = m_AvtionOverlayer;
                lastAreaOverlayer = m_AreaOverlayer;
                PauseBriefVideo();
                AreaOverlayer = true;
                AvtionOverlayer = true;
                Framework.Container.Instance.BriefVideoPlayService.BeginBriefEdit(m_player.HWnd);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "摘要视频切换设置模式");
            }
        }

        public void CancelBriefEdit()
        {
            try
            {
                //Framework.Container.Instance.BriefVideoPlayService.ClearDrawFilterGraph(m_player.HWnd, E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_PASSLINE);
                //Framework.Container.Instance.BriefVideoPlayService.ClearDrawFilterGraph(m_player.HWnd, E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_AREA_FILTER_TYPE_INTEREST);
                Framework.Container.Instance.BriefVideoPlayService.CancelBriefEdit(m_player.HWnd);

                switch (lastType)
                {
                    case VideoStatusType.E_NORMAL:
                    case VideoStatusType.E_SPEED:
                        ContinueBriefVideo();
                        break;
                    case VideoStatusType.E_PAUSE:
                        break;
                    case VideoStatusType.E_STOP:
                        break;

                }
                lastType = VideoStatusType.E_NULL;

                AreaOverlayer = lastAreaOverlayer;
                AvtionOverlayer = lastActionOverlayer;

            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "摘要视频清除检索设置");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void FinishBriefEdit()
        {
            try
            {
                Framework.Container.Instance.BriefVideoPlayService.FinishBriefEdit(
                    m_player.HWnd,
                    m_briefDensityFilter,
                    BriefMoveObjTypeFilter,
                    (uint)Framework.Container.Instance.ColorService.GetObjectColorIndex(Color.FromArgb(BriefdwMoveObjColorFilter)),
                    StartTime,
                    EndTime);
                Framework.Container.Instance.BriefVideoPlayService.VideoControl(m_player.HWnd, E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_START, 0);
                AreaOverlayer = lastAreaOverlayer;
                AvtionOverlayer = lastActionOverlayer;

                m_player.SetStatusText("");
                VideoStatusInfo e
                    = Framework.Container.Instance.BriefVideoPlayService.GetPlayStatus(m_player.HWnd);
                SetPlayVideoBtnStatus(e);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "摘要视频完成设置");
            }
        }

        #endregion
    }
}