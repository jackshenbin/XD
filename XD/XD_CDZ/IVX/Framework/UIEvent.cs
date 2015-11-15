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

    public class DelVideoSupplierDeviceEvent : CompositePresentationEvent<uint> { }

    public class AddLocalVideoTaskEvent : CompositePresentationEvent<string> { }

    public class TaskStatusChangedEvent : CompositePresentationEvent<uint> { }



    public class TaskDeletedEvent : CompositePresentationEvent<uint> { }

    public class CaseDeletedEvent : CompositePresentationEvent<uint> { }

    public class CameraDeletedEvent : CompositePresentationEvent<uint> { }

    public class CameraGroupDeletedEvent : CompositePresentationEvent<uint> { }
    public class ServerDeletedEvent : CompositePresentationEvent<uint> { }

    public class TaskUnitProgressStatusChangedEvent : CompositePresentationEvent<uint> { }

    public class TaskUnitAnalyseFinishedEvent : CompositePresentationEvent<uint> { }

    public class TaskUnitImportFinishedEvent : CompositePresentationEvent<uint> { }


    public class TaskUnitDeletedEvent : CompositePresentationEvent<uint> { }

    //public class CameraSelectionChangedEvent : CompositePresentationEvent<List<object>> { }


    public class OCXPlayVideoEvent : CompositePresentationEvent<uint> { }

    public class OCXStopAllPlayVideoEvent : CompositePresentationEvent<string> { }

    public class OCXPlayBriefVideoEvent : CompositePresentationEvent<uint> { }

    public class OCXStopPlayBriefVideoEvent : CompositePresentationEvent<string > { }


    public class SwitchPageBeginEvent : CompositePresentationEvent<Tuple<uint, uint>> { }


    public class GotoCompareSearchEvent : CompositePresentationEvent<string> { }


    public class LogSearchBeginingEvent : CompositePresentationEvent<string> { }

    public class SearchVideoFilerChangedEvent : CompositePresentationEvent<SearchResourceResultType> { }


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
