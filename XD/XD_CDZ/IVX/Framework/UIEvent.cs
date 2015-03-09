using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using BOCOM.DataModel;
using BOCOM.IVX.Protocol;

namespace BOCOM.IVX.Framework
{
    public class NavigateEvent : CompositePresentationEvent<UIFuncItemInfo> { }

    public class CmdExecuteEvent : CompositePresentationEvent<CmdItemInfo> { }

    public class EnterCaseEvent : CompositePresentationEvent<string> { }

    public class PreLeaveCaseEvent : CompositePresentationEvent<string> { }
        
    public class LeaveCaseEvent : CompositePresentationEvent<string> { }
    
    public class LogoutEvent : CompositePresentationEvent<bool> { }

    public class QuitEvent : CompositePresentationEvent<string> { }

    public class ShowDialogEvent : CompositePresentationEvent<UIFuncItemInfo> { }

    public class AddUserGroupEvent : CompositePresentationEvent<uint> { }

    public class EditUserGroupEvent : CompositePresentationEvent<uint> { }

    public class DelUserGroupEvent : CompositePresentationEvent<uint> { }

    public class AddUserEvent : CompositePresentationEvent<uint> { }

    public class EditUserEvent : CompositePresentationEvent<uint> { }

    public class DelUserEvent : CompositePresentationEvent<uint> { }

    public class AddVideoSupplierDeviceEvent : CompositePresentationEvent<VideoSupplierDeviceInfo> { }

    public class EditVideoSupplierDeviceEvent : CompositePresentationEvent<VideoSupplierDeviceInfo> { }

    public class DelVideoSupplierDeviceEvent : CompositePresentationEvent<uint> { }

    public class AddLocalVideoTaskEvent : CompositePresentationEvent<string> { }

    public class TaskStatusChangedEvent : CompositePresentationEvent<uint> { }

    public class TaskAddedEvent : CompositePresentationEvent<TaskInfo> { }

    public class TaskModifiedEvent : CompositePresentationEvent<TaskInfo> { }

    public class TaskDeletedEvent : CompositePresentationEvent<uint> { }

    public class CaseAddedEvent : CompositePresentationEvent<CaseInfo> { }

    public class CaseModifiedEvent : CompositePresentationEvent<CaseInfo> { }

    public class CaseDeletedEvent : CompositePresentationEvent<uint> { }

    public class CameraAddedEvent : CompositePresentationEvent<CameraInfo> { }

    public class CameraModifiedEvent : CompositePresentationEvent<CameraInfo> { }

    public class CameraDeletedEvent : CompositePresentationEvent<uint> { }

    public class CameraGroupAddedEvent : CompositePresentationEvent<CameraGroupInfo> { }

    public class CameraGroupModifiedEvent : CompositePresentationEvent<CameraGroupInfo> { }

    public class CameraGroupDeletedEvent : CompositePresentationEvent<uint> { }

    public class ServerAddedEvent : CompositePresentationEvent<ServerInfo> { }

    public class ServerModifiedEvent : CompositePresentationEvent<ServerInfo> { }

    public class ServerDeletedEvent : CompositePresentationEvent<uint> { }

    public class TaskUnitProgressStatusChangedEvent : CompositePresentationEvent<uint> { }

    public class TaskUnitAnalyseFinishedEvent : CompositePresentationEvent<uint> { }

    public class TaskUnitImportFinishedEvent : CompositePresentationEvent<uint> { }

    public class TaskUnitAddedEvent : CompositePresentationEvent<TaskUnitInfo> { }

    public class TaskUnitDeletedEvent : CompositePresentationEvent<uint> { }

    //public class CameraSelectionChangedEvent : CompositePresentationEvent<List<object>> { }

    public class BriefMouseClickChangedEvent : CompositePresentationEvent<BriefMouseClickInfo> { }

    public class SearchItemResultReceivedEvent : CompositePresentationEvent<SearchItemResult> { }

    public class PlayPosChangedEvent : CompositePresentationEvent<VideoStatusInfo> { }

    public class PlaySynthFailedEvent : CompositePresentationEvent<VideoStatusInfo> { }

    public class PlayFailedEvent : CompositePresentationEvent<VideoStatusInfo> { }

    public class PlayReadyEvent : CompositePresentationEvent<VideoStatusInfo> { }

    public class OCXPlayVideoEvent : CompositePresentationEvent<uint> { }

    public class OCXStopAllPlayVideoEvent : CompositePresentationEvent<string> { }

    public class OCXPlayBriefVideoEvent : CompositePresentationEvent<uint> { }

    public class OCXStopPlayBriefVideoEvent : CompositePresentationEvent<string > { }

    public class SearchFinishedEvent : CompositePresentationEvent<SearchResultSingleSummary> { }

    public class SearchResultRecordSelectedEvent : CompositePresentationEvent<Tuple<SearchItem, SearchResultRecord>> { }

    public class SearchItemImageReceivedEvent : CompositePresentationEvent<SearchImageInfo> { }

    public class SearchBeginEvent : CompositePresentationEvent<SearchSession> { }

    public class SearchCloseEvent : CompositePresentationEvent<SearchSession> { }

    public class SwitchPageBeginEvent : CompositePresentationEvent<Tuple<uint, uint>> { }

    public class BriefObjectPlayBackEvent : CompositePresentationEvent<VodInfo> { }

    public class GotoCompareSearchEvent : CompositePresentationEvent<string> { }

    public class SetCompareImageInfoEvent : CompositePresentationEvent<CompareImageInfo> { }

    public class OpenBriefPlaybackVideoEvent : CompositePresentationEvent<VodInfo> { }

    public class LogSearchReceivedEvent : CompositePresentationEvent<List<LogSearchResultInfo>> { }

    public class LogSearchBeginingEvent : CompositePresentationEvent<string> { }

    public class SearchVideoFilerChangedEvent : CompositePresentationEvent<SearchResourceResultType> { }

    public class SearchResoultPlaybackRequestEvent : CompositePresentationEvent<Tuple<SearchResultRecord, SearchType>> { }

    public class VideoDownloadStatusUpdateEvent : CompositePresentationEvent<DownloadInfo> { }

    public class VideoDownloadProgressUpdateEvent : CompositePresentationEvent<DownloadInfo> { }
    public class AddVideoDownloadEvent : CompositePresentationEvent<DownloadInfo> { }
    public class DelVideoDownloadEvent : CompositePresentationEvent<DownloadInfo> { }
    public class FinishVideoDownloadEvent : CompositePresentationEvent<DownloadInfo> { }

    public class ShowDownloadListFormEvent : CompositePresentationEvent<string> { }


    public class VideoClosedEventArgs : EventArgs
    {
        public IntPtr WindowHandle { get; set; }

        public VideoClosedEventArgs(IntPtr playHandle)
            : base()
        {
            WindowHandle = playHandle;
        }
    }
}
