using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.DataModel
{
    // class VideoProgressInfo
    //{
    //     int PlayVideoHandle{get;set;}
    //     IntPtr HWnd{get;set;}
    //     XtraSinglePlayer Player{get;set;}
    //     BOCOM.IVX.Protocol.E_VDA_PLAY_STATUS PlayState { get; set; }
    //     uint PlayPercent { get; set; }
    //     uint CurPlayTime { get; set; }
    //     uint UserData { get; set; } 
    //}

    public class VideoStatusInfo
    {
        public int PlayVideoHandle{get;set;}
        public uint VideoTaskUnitID { get; set; }
        public IntPtr HWnd{get;set;}
        //public XtraSinglePlayer Player{get;set;}
        public E_VDA_PLAY_STATUS OriginalPlayState { get; set; }
        public VideoStatusType PlayStatePriv { get; set; }
        public VideoStatusType PlayState { get; set; }
        public uint PlayPercent { get; set; }
        public E_VDA_PLAY_SPEED PlaySpeed { get; set; }
        public uint CurrPlayTime { get; set; }
        public uint TotlePlayTime { get; set; }
        public uint StartPlayTime { get; set; }
        public uint EndPlayTime { get; set; }
        public uint ErrorCode { get; set; } 
        public uint UserData { get; set; } 
    }

    public enum VideoStatusType
    {
        E_NULL = 0,		//无视频
        E_NORMAL,		//开始播放
        E_STOP,	        //停止播放
        E_PAUSE,	        //暂停播放
        E_SPEED,             //变速
        //E_SLOW,             //慢放
        //E_FAST,             //快放
        E_STEP,			//单帧前进
        E_STEP_BACK,		//单帧后退
        E_SET,		//
        E_READY,		//
    }
}
