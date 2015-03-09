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
using BOCOM.DataModel;
using BOCOM.IVX.Protocol.Model;

namespace BOCOM.IVX.Service
{
    public class VideoPlayService
    {

        #region Fields
        
        private Dictionary<int, IntPtr> m_DTVideoHandleList = new Dictionary<int, IntPtr>();

        private Dictionary<int, VideoStatusInfo> m_DTVideoStatusList = new Dictionary<int, VideoStatusInfo>();
        
        #endregion

        #region Constructors
        
        public VideoPlayService()
        {

            Framework.Container.Instance.IVXProtocol.EventPlayPos += new DelegatePlayPos(IVXProtocol_EventPlayPos);
        }

        #endregion

        #region Public helper functions
        
        public int OpenVideo(IntPtr hWnd, uint taskUnitID, DateTime startTime = new DateTime(), DateTime endTime = new DateTime(), bool isFullFile = true ,bool needClose = false)
        {
            int playhandle = GetPlayHandleByhWnd(hWnd);
            if (playhandle == 0)
            {
                if (!Framework.Environment.CheckMemeryUse())
                {
                    throw new SDKCallException(0, "内存占用过大，请关闭视频播放或取消检索后再试。");
                }

                TaskUnitInfo taskunit = Framework.Container.Instance.TaskManagerService.GetTaskUnitById(taskUnitID);
                VodInfo info = new VodInfo();
                info.VideoTaskUnitID = taskUnitID;
                info.PlayWnd = hWnd;
                info.StartTime = startTime; 
                info.EndTime = endTime;
                info.IsPlayAllFile = false;
                if (startTime == DateTime.MinValue && endTime == DateTime.MinValue)
                {
                    info.IsPlayAllFile = true;
                }
                if (!isFullFile)
                {
                    info.EndTime = new DateTime();
                    info.StartTime = new DateTime();
                    info.IsPlayAllFile = true;
                }
                playhandle = Framework.Container.Instance.IVXProtocol.PlayBackByTaskUnit(info, 0);
            }
            else
            {

                if (m_DTVideoStatusList[playhandle].VideoTaskUnitID != taskUnitID || needClose)
                {
                    if (!Framework.Environment.CheckMemeryUse())
                    {
                        throw new SDKCallException(0, "内存占用过大，请关闭视频播放或取消检索后再试。");
                    }

                    if (playhandle != 0 )
                        StopVideo(hWnd);
                    playhandle = 0;
                    VodInfo info = new VodInfo();
                    info.VideoTaskUnitID = taskUnitID;
                    info.PlayWnd = hWnd;
                    info.StartTime = startTime;
                    info.EndTime = endTime;
                    info.IsPlayAllFile = false;
                    if (startTime == DateTime.MinValue && endTime == DateTime.MinValue)
                    {
                        info.IsPlayAllFile = true;
                    }
                    if (!isFullFile)
                    {
                        info.EndTime = new DateTime();
                        info.StartTime = new DateTime();
                        info.IsPlayAllFile = true;
                    }

                    playhandle = Framework.Container.Instance.IVXProtocol.PlayBackByTaskUnit(info, 0);
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
                //Framework.Container.Instance.IVXProtocol.PlayBackControl(playhandle, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_GETTIME_RANGE, 0, out outval);
                //statinfo.TotlePlayTime = outval;
                //statinfo.StartPlayTime = Protocol.Model.ModelParser.ConvertLinuxTime(startTime);
                //statinfo.EndPlayTime = Protocol.Model.ModelParser.ConvertLinuxTime(endTime);

                m_DTVideoStatusList[playhandle] = statinfo;

                m_DTVideoHandleList[playhandle] = hWnd;
            }

            return playhandle;
        }

        public void PlayVideo(IntPtr hWnd,uint taskUnitID,DateTime startTime = new DateTime(),DateTime endTime = new DateTime())
        {
            int playhandle = OpenVideo(hWnd,taskUnitID,startTime,endTime);
               
            if (playhandle > 0)
            {
                uint outval = 0;
                Framework.Container.Instance.IVXProtocol.PlayBackControl(playhandle, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_START, 0, out outval);
                VideoStatusInfo statinfo = new VideoStatusInfo();
                statinfo.VideoTaskUnitID = taskUnitID;
                statinfo.CurrPlayTime = 0;
                statinfo.HWnd = hWnd;
                statinfo.PlaySpeed = E_VDA_PLAY_SPEED.E_PLAYSPEED_NORMALSPEED;
                statinfo.PlayState = VideoStatusType.E_NORMAL;
                statinfo.PlayVideoHandle = playhandle;
                //Framework.Container.Instance.IVXProtocol.PlayBackControl(playhandle, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_GETTIME_RANGE, 0, out outval);
                //statinfo.TotlePlayTime = outval;
                //statinfo.StartPlayTime = Protocol.Model.ModelParser.ConvertLinuxTime(startTime);
                //statinfo.EndPlayTime = Protocol.Model.ModelParser.ConvertLinuxTime(endTime);

                m_DTVideoStatusList[playhandle]=statinfo;
                m_DTVideoHandleList[playhandle]= hWnd;
                PlayReady(playhandle);
            }
        }
        
        /// <summary>
        /// 用本地播放定位实现播放文件中一段时间视频的功能
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="taskUnitID"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public void PlayVideoPartialFile(IntPtr hWnd, uint taskUnitID, DateTime startTime = new DateTime(), DateTime endTime = new DateTime())
        {
            int playhandle = OpenVideo(hWnd, taskUnitID, startTime, endTime,false);
            try
            {
                uint outval = 0;
                Framework.Container.Instance.IVXProtocol.PlayBackControl(playhandle, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_STOP, 0, out outval);
            }
            catch (SDKCallException)
            { }
            if (playhandle > 0)
            {
                uint outval = 0;
                VideoStatusInfo statinfo = new VideoStatusInfo();
                statinfo.CurrPlayTime = 0;
                statinfo.VideoTaskUnitID = taskUnitID;
                statinfo.HWnd = hWnd;
                statinfo.PlaySpeed = E_VDA_PLAY_SPEED.E_PLAYSPEED_NORMALSPEED;
                statinfo.OriginalPlayState = E_VDA_PLAY_STATUS.E_PLAY_STATUS_NORMAL;
                statinfo.PlayStatePriv = VideoStatusType.E_NULL;
                statinfo.PlayState = VideoStatusType.E_NORMAL;
                statinfo.PlayVideoHandle = playhandle;
                Framework.Container.Instance.IVXProtocol.PlayBackControl(playhandle, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_GETTIME_RANGE, 0, out outval);
                statinfo.TotlePlayTime = outval;

                TaskUnitInfo taskunitinfo = Framework.Container.Instance.TaskManagerService.GetTaskUnitById(taskUnitID);
                if (startTime > DataModel.Common.ZEROTIME)
                {
                    statinfo.StartPlayTime = (uint)Math.Max(startTime.Subtract(taskunitinfo.StartTime).TotalSeconds, 0);
                }
                else
                    statinfo.StartPlayTime = (uint)Math.Max(startTime.Subtract(new DateTime()).TotalSeconds,0);

                if (endTime > DataModel.Common.ZEROTIME)
                    statinfo.EndPlayTime = (uint)endTime.Subtract(taskunitinfo.StartTime).TotalSeconds;
                else
                    statinfo.EndPlayTime = (uint)endTime.Subtract(new DateTime()).TotalSeconds;


                Debug.Assert(statinfo.StartPlayTime <= statinfo.TotlePlayTime && statinfo.EndPlayTime <= statinfo.TotlePlayTime);

                uint startpos = 1000 * statinfo.StartPlayTime / statinfo.TotlePlayTime;
                Framework.Container.Instance.IVXProtocol.PlayBackControl(playhandle, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_PLAY_BY_SEEK, startpos, out outval);


                m_DTVideoStatusList[playhandle] = statinfo;
                m_DTVideoHandleList[playhandle] = hWnd;

                PlayReady(playhandle);
            }
        }

        public uint VideoControl(IntPtr hWnd,E_VDA_PLAYCTRL_TYPE ctrlType,uint ctrlValue)
        {
            int playhandle = GetPlayHandleByhWnd(hWnd);

            if (playhandle > 0)
            {
                uint outVal = 0;
                Framework.Container.Instance.IVXProtocol.PlayBackControl(playhandle, ctrlType, ctrlValue, out outVal);


                if (ctrlType == E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_PAUSE)
                    m_DTVideoStatusList[playhandle].PlayState = VideoStatusType.E_PAUSE;
                else if (ctrlType == E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_RESUME)
                    m_DTVideoStatusList[playhandle].PlayState = VideoStatusType.E_NORMAL;
                else if (ctrlType == E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_START)
                    m_DTVideoStatusList[playhandle].PlayState = VideoStatusType.E_NORMAL;
                //{

                //    VideoStatusInfo statinfo = m_DTVideoStatusList[playhandle];
                //    statinfo.PlayState = VideoStatusType.E_READY;
                //    statinfo.CurrPlayTime = 0;
                //    //uint val = 0;
                //    //Framework.Container.Instance.IVXProtocol.BriefPlayControl(playhandle, E_VDA_BRIEF_PLAYCTRL_TYPE.E_BRIEF_PLAYCTRL_GETTIME_RANGE, 0, out val);
                //    //statinfo.TotlePlayTime = val;

                //}
                else if (ctrlType == E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_STEP)
                    m_DTVideoStatusList[playhandle].PlayState = VideoStatusType.E_STEP;
                else if (ctrlType == E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_STEP_BACK)
                    m_DTVideoStatusList[playhandle].PlayState = VideoStatusType.E_STEP_BACK;
                else if (ctrlType == E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_STOP)
                    m_DTVideoStatusList[playhandle].PlayState = VideoStatusType.E_STOP;
                else if (ctrlType == E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_SETSPEED)
                {
                    m_DTVideoStatusList[playhandle].PlayState = VideoStatusType.E_SPEED;
                    m_DTVideoStatusList[playhandle].PlaySpeed = (E_VDA_PLAY_SPEED)ctrlValue;
                    //switch ((E_VDA_PLAY_SPEED)ctrlValue)
                    //{
                    //    case E_VDA_PLAY_SPEED.E_PLAYSPEED_NORMALSPEED:
                    //        m_DTVideoStatusList[playhandle].PlaySpeed = Model.VideoStatusType.E_NORMAL;
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
                Framework.Container.Instance.IVXProtocol.PlayBackControl(playhandle, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_GETPOS, 0, out outVal);
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
                Framework.Container.Instance.IVXProtocol.PlayBackControl(playhandle, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_SETPOS, pos, out outVal);
            }
        }

        public int GetPlaySpeed(IntPtr hWnd)
        {
            int playhandle = GetPlayHandleByhWnd(hWnd);

            if (playhandle > 0)
            {
                uint outVal = 0;
                Framework.Container.Instance.IVXProtocol.PlayBackControl(playhandle, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_GETSPEED, 0, out outVal);
                return (int)outVal;
            }
            return (int)E_VDA_PLAY_SPEED.E_PLAYSPEED_NORMALSPEED;
        }

        /// <summary>
        /// 快放，最快到8倍
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public int SetPlaySpeedAdd(IntPtr hWnd)
        {
            int speed = GetPlaySpeed(hWnd);
            speed++;
            if (speed>(int)E_VDA_PLAY_SPEED.E_PLAYSPEED_FAST8)
            {
                speed = (int)E_VDA_PLAY_SPEED.E_PLAYSPEED_FAST8;
            }
            SetPlaySpeed(hWnd,(uint)speed);
            return speed;
        }
        /// <summary>
        /// 慢放，最低到1/2慢放
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public int SetPlaySpeedMinus(IntPtr hWnd)
        { 
            int speed = GetPlaySpeed(hWnd);
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
                Framework.Container.Instance.IVXProtocol.PlayBackControl(playhandle, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_GETPOS, 0, out outVal);
                return ModelParser.ConvertLinuxTime(outVal);
                 
            }
            return DateTime.MinValue;
        }
        
        public void StopVideo(IntPtr hWnd)
        {
            int playhandle = GetPlayHandleByhWnd(hWnd);
            if (playhandle <= 0)
                return;

            StopVideo(playhandle);
        }

        private  void StopVideo(int playhandle)
        {
            if (m_DTVideoHandleList.ContainsKey(playhandle))
            {
                IntPtr windowHandle = m_DTVideoHandleList[playhandle];

                Framework.Container.Instance.IVXProtocol.StopPlayBack(playhandle);
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
        
        public int GetPlayHandleByhWnd(IntPtr hWnd)
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
        
        public System.Drawing.Image GrabPicture(IntPtr hWnd)
        {
            int playhandle = GetPlayHandleByhWnd(hWnd);

            if (playhandle > 0)
            {
                return Framework.Container.Instance.IVXProtocol.GrabPicture(playhandle);
            }
            else
                return null;
        }

        public bool GrabPictureToFile(IntPtr hWnd,string path)
        {
            System.Drawing.Image img = GrabPicture(hWnd);
            try
            {
                img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch
            {
                return false; 
            }
            return true;
        }
        
        public bool GetPlayResolution(IntPtr hWnd, out UInt32 Width, out UInt32 Height)
        {
            int playhandle = GetPlayHandleByhWnd(hWnd);
            Width = 0;
            Height = 0;
            if (playhandle > 0)
            {
                return Framework.Container.Instance.IVXProtocol.GetPlayResolution(playhandle,out Width,out Height);
            }
            else
                return false;
 
        }


        public bool IsPlayHitGraph(IntPtr hWnd , System.Drawing.Point point)
        {
            int playhandle = GetPlayHandleByhWnd(hWnd);
            if (playhandle > 0)
            {
                return Framework.Container.Instance.IVXProtocol.IsPlayHitGraph(playhandle, point);
            }
            else
                return false;

        }
        #endregion

        #region Private helper functions
        
        private  void SetPlaySpeed(IntPtr hWnd,uint speed)
        {
            int playhandle = GetPlayHandleByhWnd(hWnd);

            if (playhandle > 0)
            {
                uint outVal = 0;
                if (m_DTVideoStatusList[playhandle].PlayState == VideoStatusType.E_PAUSE
                    ||m_DTVideoStatusList[playhandle].PlayState == VideoStatusType.E_STEP
                    ||m_DTVideoStatusList[playhandle].PlayState == VideoStatusType.E_STEP_BACK)
                { 
                    Framework.Container.Instance.IVXProtocol.PlayBackControl(playhandle, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_RESUME, 0, out outVal);
                }
                Framework.Container.Instance.IVXProtocol.PlayBackControl(playhandle, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_SETSPEED, speed, out outVal);
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

        private void PlayReady(int vodHandle)
        {
            if (m_DTVideoHandleList.ContainsKey(vodHandle))
            {
                VideoStatusInfo info = m_DTVideoStatusList[vodHandle];

                info.PlayStatePriv = info.PlayState;
                info.PlayState = VideoStatusType.E_NORMAL;
                uint val = 0;
                Framework.Container.Instance.IVXProtocol.PlayBackControl(vodHandle, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_GETTIME_RANGE, 0, out val);
                info.TotlePlayTime = val;
                Framework.Container.Instance.EvtAggregator.GetEvent<PlayReadyEvent>().Publish(info);
            }
        }

        #endregion

        void IVXProtocol_EventPlayPos(int vodHandle, E_VDA_PLAY_STATUS playState, uint playPercent, uint curPlayTime, uint userData)
        {
            if (m_DTVideoHandleList.ContainsKey(vodHandle))
            {
                //BOCOM.IVX.Model.VideoProgressInfo info = new Model.VideoProgressInfo();

                VideoStatusInfo info = m_DTVideoStatusList[vodHandle];
                info.CurrPlayTime = curPlayTime;
                info.OriginalPlayState = playState;
                info.PlayPercent = playPercent;
                info.UserData = userData;

                if (playState == E_VDA_PLAY_STATUS.E_PLAY_STATUS_FAILED)
                {
                    info.PlayStatePriv = info.PlayState;
                    info.PlayState = VideoStatusType.E_NULL;
                    Framework.Container.Instance.EvtAggregator.GetEvent<PlayFailedEvent>().Publish(info);
                    return;
                }
                else if (playState == E_VDA_PLAY_STATUS.E_PLAY_STATUS_FINISH)
                {
                    info.PlayStatePriv = info.PlayState;
                    info.PlayState = VideoStatusType.E_STOP;
                }
                //else if (playState == E_VDA_PLAY_STATUS.E_PLAY_STATUS_STARTPLAY_READY)
                //{
                //    info.PlayStatePriv = info.PlayState;
                //    info.PlayState = VideoStatusType.E_NORMAL;
                //    uint val = 0;
                //    Framework.Container.Instance.IVXProtocol.PlayBackControl(vodHandle, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_GETTIME_RANGE, 0, out val);
                //    info.TotlePlayTime = val;
                //    Framework.Container.Instance.EvtAggregator.GetEvent<PlayReadyEvent>().Publish(info);
                //    return;

                //}

                if (info.TotlePlayTime != 0)
                {


                    uint endpos = 1000 * info.EndPlayTime / info.TotlePlayTime;

                    if (endpos > 0 && playPercent > endpos)
                    {
                        uint outval = 0;
                        Framework.Container.Instance.IVXProtocol.PlayBackControl(vodHandle, E_VDA_PLAYCTRL_TYPE.E_PLAYCTRL_STOP, 0, out outval);
                        info.PlayStatePriv = info.PlayState;
                        info.PlayState = VideoStatusType.E_STOP;
                    }

                }

                Framework.Container.Instance.EvtAggregator.GetEvent<PlayPosChangedEvent>().Publish(info);

            }
        }

    }
}
