using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Framework;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using BOCOM.IVX.Protocol.Model;
using BOCOM.DataModel;

namespace BOCOM.IVX.Service
{
    public class BriefVideoPlayService 
    {
        private Dictionary<int, IntPtr> m_DTVideoHandleList = new Dictionary<int, IntPtr>();

        private Dictionary<int, VideoStatusInfo> m_DTVideoStatusList
            = new Dictionary<int, VideoStatusInfo>();


        public BriefVideoPlayService()
        {
            Framework.Container.Instance.IVXProtocol.EventBriefVideoPlayProgress += new Action<int, E_VDA_PLAY_STATUS, uint,uint,uint, uint>(IVXProtocol_EventBriefVideoPlayProgress);
            Framework.Container.Instance.IVXProtocol.EventBriefVideoWindowMouseClick += new Action<int, E_VDA_BRIEF_WND_MOUSE_OPT_TYPE, uint, uint, uint>(IVXProtocol_EventBriefVideoWindowMouseClick);
        }

        void IVXProtocol_EventBriefVideoWindowMouseClick(int vodHandle, E_VDA_BRIEF_WND_MOUSE_OPT_TYPE mouseOptType, uint x, uint y, uint userData)
        {
            if (m_DTVideoHandleList.ContainsKey(vodHandle))
            {
                BriefMouseClickInfo info = new BriefMouseClickInfo();
                info.MouseClickType = mouseOptType;
                info.BriefHandle = (uint)vodHandle;
                info.X = x;
                info.Y = y;
                info.UserData = userData;
                Framework.Container.Instance.EvtAggregator.GetEvent<BriefMouseClickChangedEvent>().Publish(info);
            }
        }

        void IVXProtocol_EventBriefVideoPlayProgress(int vodHandle, E_VDA_PLAY_STATUS playState, uint playPercent, uint curPlayTime,uint result, uint userData)
        {
            if(m_DTVideoHandleList.ContainsKey(vodHandle))
            {
                VideoStatusInfo info = m_DTVideoStatusList[vodHandle];
                info.CurrPlayTime = curPlayTime;
                info.OriginalPlayState = playState;
                info.PlayPercent = playPercent;
                info.UserData = userData;
                info.ErrorCode = result;

                if (playState == E_VDA_PLAY_STATUS.E_PLAY_STATUS_FAILED )
                {
                    info.PlayStatePriv = info.PlayState;
                    info.PlayState = VideoStatusType.E_NULL;
                    Framework.Container.Instance.EvtAggregator.GetEvent<PlayFailedEvent>().Publish(info);
                    return;
                }
                else if ( playState == E_VDA_PLAY_STATUS.E_PLAY_STATUS_SYNTH_ERROR)
                {
                    info.PlayStatePriv = info.PlayState;
                    info.PlayState = VideoStatusType.E_NULL;
                    Framework.Container.Instance.EvtAggregator.GetEvent<PlaySynthFailedEvent>().Publish(info);
                    return;
                }
                else if (playState == E_VDA_PLAY_STATUS.E_PLAY_STATUS_FINISH)
                {
                    info.PlayStatePriv = info.PlayState;
                    info.PlayState = VideoStatusType.E_STOP;
                }
                else if (playState == E_VDA_PLAY_STATUS.E_PLAY_STATUS_STARTPLAY_READY)
                {
                    info.PlayStatePriv = info.PlayState;
                    info.PlayState = VideoStatusType.E_NORMAL;
                    uint val = 0;
                    Framework.Container.Instance.IVXProtocol.BriefPlayControl(vodHandle, E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_GETTIME_RANGE, 0, out val);
                    info.TotlePlayTime = val;
                    Framework.Container.Instance.EvtAggregator.GetEvent<PlayReadyEvent>().Publish(info);
                    return;

                }
                Framework.Container.Instance.EvtAggregator.GetEvent<PlayPosChangedEvent>().Publish(info);

            }
        }

        //[DllImport("briefview.dll", CallingConvention = CallingConvention.StdCall)]
        //public static extern Int32 Bv_CreateBriefView(IntPtr hWnd, IntPtr ptBfvInitParam);
                
        
        private int OpenVideo(IntPtr hWnd, uint taskUnitID)
        {

            int playhandle = GetPlayHandleByhWnd(hWnd);
            if (playhandle == 0)
            {
                if (!Framework.Environment.CheckMemeryUse())
                {
                    throw new SDKCallException(0, "内存占用过大，请关闭视频播放或取消检索后再试。");
                }

                playhandle = Framework.Container.Instance.IVXProtocol.OpenBriefPlay(taskUnitID, hWnd, 0);

            }
            else
            {

                if (m_DTVideoStatusList[playhandle].VideoTaskUnitID != taskUnitID)
                {
                    if (!Framework.Environment.CheckMemeryUse())
                    {
                        throw new SDKCallException(0, "内存占用过大，请关闭视频播放或取消检索后再试。");
                    }

                    if (playhandle != 0)
                        StopVideo(hWnd);

                    playhandle = Framework.Container.Instance.IVXProtocol.OpenBriefPlay(taskUnitID, hWnd, 0);

                }
            }
            if (playhandle > 0)
            {
                VideoStatusInfo statinfo = new VideoStatusInfo();
                statinfo.VideoTaskUnitID = taskUnitID;
                statinfo.CurrPlayTime = 0;
                statinfo.HWnd = hWnd;
                statinfo.PlaySpeed = E_VDA_PLAY_SPEED.E_PLAYSPEED_NORMALSPEED;
                statinfo.PlayState = VideoStatusType.E_NULL;
                statinfo.PlayVideoHandle = playhandle;

                m_DTVideoStatusList[playhandle] = statinfo;
                m_DTVideoHandleList[playhandle] = hWnd;
            }

            return playhandle;
        }

        public void PlayVideo(IntPtr hWnd, uint taskUnitID, E_VDA_BRIEF_DENSITY objDensity = E_VDA_BRIEF_DENSITY.E_BRIEF_DENSITY_04, E_VDA_MOVEOBJ_TYPE moveObjType = E_VDA_MOVEOBJ_TYPE.E_VDA_MOVEOBJ_TYPE_ALL, uint moveObjColor=0)
        {
            //ClassLibrary1.Class1.PlayBriefVideo(hWnd, IntPtr.Zero);
            //return;

            int playhandle = OpenVideo(hWnd,taskUnitID );
            if (playhandle > 0)
            {
                uint val = 0;
                BriefPlayParam param = new BriefPlayParam();
                param.ObjDensity = objDensity;
                param.MoveObjColor = moveObjColor;
                param.MoveObjType = moveObjType;
                param.StartTime = new DateTime();
                param.EndTime = new DateTime();
                param.IsBriefAllFile = true ;

                Framework.Container.Instance.IVXProtocol.SetBriefPlayParam(playhandle, param); 
                Framework.Container.Instance.IVXProtocol.BriefPlayControl(playhandle, E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_START,
                    1, out val);
                Framework.Container.Instance.IVXProtocol.SetBriefEditMode(playhandle, false);
                Framework.Container.Instance.IVXProtocol.SwitchOnBriefWndMouseOptNtfCB(playhandle);

                VideoStatusInfo statinfo = new VideoStatusInfo();
                statinfo.CurrPlayTime = 0;
                statinfo.VideoTaskUnitID = taskUnitID;
                statinfo.HWnd = hWnd;
                statinfo.PlaySpeed = E_VDA_PLAY_SPEED.E_PLAYSPEED_NORMALSPEED;
                statinfo.PlayState = VideoStatusType.E_READY;
                statinfo.PlayVideoHandle = playhandle;


                m_DTVideoStatusList[playhandle] = statinfo;
                m_DTVideoHandleList[playhandle] = hWnd;
            }
        }

        public uint VideoControl(IntPtr hWnd, E_VDA_BRIEF_PLAYCTRL_TYPE ctrlType, uint ctrlValue)
        {
            int playhandle = GetPlayHandleByhWnd(hWnd);

            if (playhandle > 0)
            {
                uint outVal = 0;
                Framework.Container.Instance.IVXProtocol.BriefPlayControl(playhandle, ctrlType, ctrlValue, out outVal);


                if (ctrlType == E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_PAUSE)
                    m_DTVideoStatusList[playhandle].PlayState = VideoStatusType.E_PAUSE;
                else if (ctrlType == E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_RESUME)
                    m_DTVideoStatusList[playhandle].PlayState = VideoStatusType.E_NORMAL;
                else if (ctrlType == E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_START)
                {
                    VideoStatusInfo statinfo = m_DTVideoStatusList[playhandle];
                    statinfo.PlayState = VideoStatusType.E_READY;
                    statinfo.CurrPlayTime = 0;
                    //uint val = 0;
                    //Framework.Container.Instance.IVXProtocol.BriefPlayControl(playhandle, E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_GETTIME_RANGE, 0, out val);
                    //statinfo.TotlePlayTime = val;

                }
                //else if (ctrlType == E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_STEP)
                //    m_DTVideoStatusList[playhandle] = Model.VideoStatusType.E_STEP;
                //else if (ctrlType == E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_STEP_BACK)
                //    m_DTVideoStatusList[playhandle] = Model.VideoStatusType.E_STEP_BACK;
                else if (ctrlType == E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_STOP)
                    m_DTVideoStatusList[playhandle].PlayState = VideoStatusType.E_STOP;
                else if (ctrlType == E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_SETSPEED)
                {
                    m_DTVideoStatusList[playhandle].PlayState = VideoStatusType.E_SPEED;
                    m_DTVideoStatusList[playhandle].PlaySpeed = (E_VDA_PLAY_SPEED)ctrlValue;

                    //switch ((E_VDA_PLAY_SPEED)ctrlValue)
                    //{
                    //    case E_VDA_PLAY_SPEED.E_PLAYSPEED_NORMALSPEED:
                    //        m_DTVideoStatusList[playhandle] = Model.VideoStatusType.E_NORMAL;
                    //        break;
                    //    case E_VDA_PLAY_SPEED.E_PLAYSPEED_FAST2:
                    //    case E_VDA_PLAY_SPEED.E_PLAYSPEED_FAST4:
                    //    case E_VDA_PLAY_SPEED.E_PLAYSPEED_FAST8:
                    //    case E_VDA_PLAY_SPEED.E_PLAYSPEED_FAST16:
                    //        m_DTVideoStatusList[playhandle] = Model.VideoStatusType.E_FAST;
                    //        break;
                    //    case E_VDA_PLAY_SPEED.E_PLAYSPEED_SLOW2:
                    //    case E_VDA_PLAY_SPEED.E_PLAYSPEED_SLOW4:
                    //    case E_VDA_PLAY_SPEED.E_PLAYSPEED_SLOW8:
                    //    case E_VDA_PLAY_SPEED.E_PLAYSPEED_SLOW16:
                    //        m_DTVideoStatusList[playhandle] = Model.VideoStatusType.E_SLOW;
                    //        break;
                    //    default:
                    //        m_DTVideoStatusList[playhandle] = Model.VideoStatusType.E_NORMAL;
                    //        break;
                    //}
                }

                return outVal;
            }
            else
                return 0;
        }
        
        public uint GetPlayPos(IntPtr hWnd)
        {
            int playhandle = GetPlayHandleByhWnd(hWnd);

            if (playhandle > 0)
            {
                uint outVal = 0;
                Framework.Container.Instance.IVXProtocol.BriefPlayControl(playhandle, E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_GETPOS, 0, out outVal);
                return outVal;
            }
            return 0;
        }

        public void SetPlayPos(IntPtr hWnd,uint pos)
        {
            int playhandle = GetPlayHandleByhWnd(hWnd);

            if (playhandle > 0)
            {
                uint outVal = 0;
                Framework.Container.Instance.IVXProtocol.BriefPlayControl(playhandle, E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_SETPOS, pos, out outVal);
            }
        }

        public uint GetPlaySpeed(IntPtr hWnd)
        {
            int playhandle = GetPlayHandleByhWnd(hWnd);

            if (playhandle > 0)
            {
                uint outVal = 0;
                Framework.Container.Instance.IVXProtocol.BriefPlayControl(playhandle, E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_GETSPEED, 0, out outVal);
                return outVal;
            }
            return (uint)E_VDA_PLAY_SPEED.E_PLAYSPEED_NORMALSPEED;
        }

        private  void SetPlaySpeed(IntPtr hWnd,uint speed)
        {
            int playhandle = GetPlayHandleByhWnd(hWnd);

            if (playhandle > 0)
            {
                uint outVal = 0;
                if (m_DTVideoStatusList[playhandle].PlayState == VideoStatusType.E_PAUSE
                    || m_DTVideoStatusList[playhandle].PlayState == VideoStatusType.E_STEP
                    || m_DTVideoStatusList[playhandle].PlayState == VideoStatusType.E_STEP_BACK)
                {
                    Framework.Container.Instance.IVXProtocol.BriefPlayControl(playhandle, E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_RESUME, 0, out outVal);
                }

                Framework.Container.Instance.IVXProtocol.BriefPlayControl(playhandle, E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_SETSPEED, speed, out outVal);
                m_DTVideoStatusList[playhandle].PlayState = VideoStatusType.E_SPEED;
                m_DTVideoStatusList[playhandle].PlaySpeed = (E_VDA_PLAY_SPEED)speed;

                //switch ((E_VDA_PLAY_SPEED)speed)
                //{
                //    case E_VDA_PLAY_SPEED.E_PLAYSPEED_NORMALSPEED:
                //        m_DTVideoStatusList[playhandle] = Model.VideoStatusType.E_NORMAL;
                //        break;
                //    case E_VDA_PLAY_SPEED.E_PLAYSPEED_FAST2:
                //    case E_VDA_PLAY_SPEED.E_PLAYSPEED_FAST4:
                //    case E_VDA_PLAY_SPEED.E_PLAYSPEED_FAST8:
                //    case E_VDA_PLAY_SPEED.E_PLAYSPEED_FAST16:
                //        m_DTVideoStatusList[playhandle] = Model.VideoStatusType.E_FAST;
                //        break;
                //    case E_VDA_PLAY_SPEED.E_PLAYSPEED_SLOW2:
                //    case E_VDA_PLAY_SPEED.E_PLAYSPEED_SLOW4:
                //    case E_VDA_PLAY_SPEED.E_PLAYSPEED_SLOW8:
                //    case E_VDA_PLAY_SPEED.E_PLAYSPEED_SLOW16:
                //        m_DTVideoStatusList[playhandle] = Model.VideoStatusType.E_SLOW;
                //        break;
                //    default:
                //        m_DTVideoStatusList[playhandle] = Model.VideoStatusType.E_NORMAL;
                //        break;
                //}

            }
        }

        public int SetPlaySpeedAdd(IntPtr hWnd)
        {
            int speed = (int)GetPlaySpeed(hWnd);
            speed++;
            if (speed>(int)E_VDA_PLAY_SPEED.E_PLAYSPEED_FAST2)
            {
                speed = (int)E_VDA_PLAY_SPEED.E_PLAYSPEED_FAST2;
            }
            SetPlaySpeed(hWnd,(uint)speed);
            return speed;
        }

        public int SetPlaySpeedMinus(IntPtr hWnd)
        { 
            int speed = (int)GetPlaySpeed(hWnd);
            speed--;
            if (speed<(int)E_VDA_PLAY_SPEED.E_PLAYSPEED_SLOW2)
            {
                speed = (int)E_VDA_PLAY_SPEED.E_PLAYSPEED_SLOW2;
            }
            SetPlaySpeed(hWnd,(uint)speed);
            return speed;
        }

        public DateTime GetPlayTime(IntPtr hWnd)
        {
            int playhandle = GetPlayHandleByhWnd(hWnd);

            if (playhandle > 0)
            {
                uint outVal = 0;
                Framework.Container.Instance.IVXProtocol.BriefPlayControl(playhandle, E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_GETTIME_RANGE, 0, out outVal);
                return ModelParser.ConvertLinuxTime(outVal);
                 
            }
            return DateTime.MinValue;
        }
        
        public void StopVideo(IntPtr hWnd)
        {
            int playhandle = GetPlayHandleByhWnd(hWnd);

            if (playhandle > 0)
            {
                Framework.Container.Instance.IVXProtocol.CloseBriefPlay(playhandle);
                m_DTVideoHandleList.Remove(playhandle);
                m_DTVideoStatusList.Remove(playhandle);
            }
        }
        
        public VideoStatusInfo GetPlayStatus(IntPtr hWnd)
        {
            int handle = GetPlayHandleByhWnd(hWnd);
            if(handle>0)
                return m_DTVideoStatusList[handle];
            else
                return new VideoStatusInfo();
        }

        public List<int> GetPlayHandlesByTaskUnitId(uint taskUnitId)
        {
            List<int> handles = new List<int>();
            foreach (KeyValuePair<int, VideoStatusInfo> kv in m_DTVideoStatusList)
            {
                if (kv.Value.VideoTaskUnitID == taskUnitId)
                {
                    Debug.Assert(!handles.Contains(kv.Key));
                    if (!handles.Contains(kv.Key))
                    {
                        handles.Add(kv.Key);
                    }
                }
            }
            return handles;
        }

        private int GetPlayHandleByhWnd(IntPtr hWnd)
        {
            int playhandle = 0;
            foreach (KeyValuePair<int, IntPtr> o in m_DTVideoHandleList)
            {
                if (o.Value == hWnd)
                {
                    playhandle = o.Key;
                    break;
                }
            }
            return playhandle;
        }

        public void BeginBriefEdit(IntPtr hWnd)
        {
            int handle = GetPlayHandleByhWnd(hWnd);
            if (handle > 0)
                Framework.Container.Instance.IVXProtocol.SetBriefEditMode(handle, true);
        }
        public void CancelBriefEdit(IntPtr hWnd)
        {
            int handle = GetPlayHandleByhWnd(hWnd);
            if (handle > 0)
                Framework.Container.Instance.IVXProtocol.SetBriefEditMode(handle, false );

        }
        public void FinishBriefEdit(
            IntPtr hWnd,
            E_VDA_BRIEF_DENSITY objDensity,
            E_VDA_MOVEOBJ_TYPE moveObjType,
            uint moveObjColor,
            DateTime startTime,
            DateTime endTime)
        {
            int handle = GetPlayHandleByhWnd(hWnd);
            if (handle > 0)
            {
                BriefPlayParam param = new BriefPlayParam();
                param.ObjDensity = objDensity;
                param.MoveObjColor = moveObjColor;
                param.MoveObjType = moveObjType;
                param.StartTime = startTime;
                param.EndTime = endTime;
                param.IsBriefAllFile = false;
                if (startTime == DateTime.MinValue && endTime == DateTime.MinValue)
                    param.IsBriefAllFile = true ;

                Framework.Container.Instance.IVXProtocol.SetBriefPlayParam(handle, param);
                Framework.Container.Instance.IVXProtocol.SetBriefEditMode(handle, false );
            }
        }

        public void SetOverlayEnable(IntPtr hWnd,E_VDA_BRIEF_PLAY_DRAW_TYPE overlayType,bool isShow)
        { 
            int handle = GetPlayHandleByhWnd(hWnd);
            if (handle > 0)
            {
                Framework.Container.Instance.IVXProtocol.SetBriefPlayDrawType(handle, overlayType,isShow);
            }
            
        }

        public void SetDrawFilter(IntPtr hWnd, E_VDA_BRIEF_DRAW_FILTER_TYPE fileterType)
        { 
            int handle = GetPlayHandleByhWnd(hWnd);
            if (handle > 0)
            {
                Framework.Container.Instance.IVXProtocol.SetBriefDrawFilterType(handle, fileterType);
            }
        }
        public void ClearDrawFilterGraph(IntPtr hWnd, E_VDA_BRIEF_DRAW_FILTER_TYPE fileterType)
        {
            int handle = GetPlayHandleByhWnd(hWnd);
            if (handle > 0)
            {
                switch (fileterType)
                {
                    case E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_NULL:
                        Framework.Container.Instance.IVXProtocol.ClearActionDrawFilter(handle);
                        Framework.Container.Instance.IVXProtocol.ClearAreaDrawFilter(handle);
                        break;
                    case E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_PASSLINE:
                    case E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_ACTION_FILTER_TYPE_BREAK_AREA:
                        Framework.Container.Instance.IVXProtocol.ClearActionDrawFilter(handle);
                        break;
                    case E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_AREA_FILTER_TYPE_INTEREST:
                    case E_VDA_BRIEF_DRAW_FILTER_TYPE.E_BRIEF_AREA_FILTER_TYPE_SHEILD:
                        Framework.Container.Instance.IVXProtocol.ClearAreaDrawFilter(handle);
                        break;
                }
            }

        }

        public BriefMoveobjInfo GetSelectBriefMoveObjInfo(IntPtr hWnd)
        { 
            int handle = GetPlayHandleByhWnd(hWnd);
            if (handle > 0)
            {
                if (Framework.Container.Instance.IVXProtocol.IsSelectBriefMoveObj(handle))
                {
                    return Framework.Container.Instance.IVXProtocol.GetSelectBriefMoveObjInfo(handle);
                }
                else

                    return null;
            }
            else
                return null;
            
        }
        public bool IsSelectBriefMoveObj(IntPtr hWnd)
        { 
            int handle = GetPlayHandleByhWnd(hWnd);
            if (handle > 0)
            {
                return Framework.Container.Instance.IVXProtocol.IsSelectBriefMoveObj(handle);
            }
            else
                return false ;
            
        }
        public System.Drawing.Image GetObjectPicture(IntPtr hWnd)
        {
            int playhandle = GetPlayHandleByhWnd(hWnd);

            if (playhandle > 0)
            {
                return Framework.Container.Instance.IVXProtocol.GrabBriefPictureData(playhandle);
            }
            else
                return null;
        }

        public void Cleanup()
        {
            try
            {
                foreach (IntPtr o in m_DTVideoHandleList.Values.ToList())
                {
                    StopVideo(o);
                }
            }
            catch (SDKCallException)
            { }
            m_DTVideoHandleList.Clear();
        }



    }
}
