using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using BOCOM.DataModel;
using BOCOM.IVX.Protocol;

namespace BOCOM.IVX.Framework
{
    ///// <summary>
    ///// 视频检索请求完成，同步方式时， 返回了检索结果， 异步时仅发送了检索指令
    ///// </summary>
    //public class VAQueryCompletedEvent : CompositePresentationEvent<SearchPara>
    //{

    //}

    ///// <summary>
    ///// 视频检索请求事件
    ///// </summary>
    //public class VAQueryRequestEvent : CompositePresentationEvent<SearchPara>
    //{

    //}

    public class DeleteResourceEvent : CompositePresentationEvent<List<int>>
    {

    }

    public class SearchLogEvent : CompositePresentationEvent<LogReqInfo>
    {

    }

    public class SingleOrMixModeSwitchEvent : CompositePresentationEvent<bool>
    {

    }

    public class ClearSearchEvent : CompositePresentationEvent<object>
    {

    }

    public class PlayResourceEvent : CompositePresentationEvent<PlayResourceInfo>
    {

    }

    public class CompareDrawModeChangeEvent : CompositePresentationEvent<E_VDA_SEARCH_MOVE_OBJ_RANGE_FILTER_TYPE>
    {

    }

    public class UserLoginEvent : CompositePresentationEvent<LoginToken>
    {
    }

    public class UserLogOutEvent : CompositePresentationEvent<object>
    {
    }

    public class OCXAddFileEvent : CompositePresentationEvent<OCXAddFileInfo>
    {
    }

    public class ExpandLocalNodeEvent : CompositePresentationEvent<object>
    {
    }

    public class SetPlaywndFullEvent : CompositePresentationEvent<System.Windows.Forms.PreviewKeyDownEventArgs>
    {
    }

    public class PausePlayEvent : CompositePresentationEvent<System.Windows.Forms.PreviewKeyDownEventArgs>
    {
    }


    public class OpenImportNVRVideosFormEvent : CompositePresentationEvent<NVRAndChannelsInfo>
    {

    }

    public class NVRAndChannelsInfo
    {
        public string IP{get;set;}

        public int Port{get;set;}

        public string UserName{get;set;}
        
        public string Password{get;set;}
        
        public int Type{get;set;}

        public string Channels { get; set; }
    }


    public struct OCXAddFileInfo
    {
        public int id;
        public string name;
    }
    public class PlayResourceInfo
    {
        private Int32 _hrItem;		//资源节点句柄
        private string _szLocation;	//地点
        private Int64 _nTargetTs;			//目标时间
        private Int64 _nTargetAppearTs;			//目标出现时间
        private Int64 _nTargetDisappearTs;		//目标结束时间

        public Int32 hrItem
        {
            get { return _hrItem; }
            set { _hrItem = value; }
        }

        public string szLocation
        {
            get { return _szLocation; }
            set { _szLocation = value; }
        }

        public Int64 nTargetTs
        {
            get { return _nTargetTs; }
            set { _nTargetTs = value; }
        }

        public Int64 nTargetAppearTs
        {
            get { return _nTargetAppearTs; }
            set { _nTargetAppearTs = value; }
        }

        public Int64 nTargetDisappearTs
        {
            get { return _nTargetDisappearTs; }
            set { _nTargetDisappearTs = value; }
        }

    }
}
