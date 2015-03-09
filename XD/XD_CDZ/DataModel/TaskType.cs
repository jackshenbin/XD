using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.DataModel
{
    #region TaskType

    public enum TaskType
    {
        VideoUpload = 0,
        PicUpload
    }

    #endregion

    #region TaskPriority

    public enum TaskPriority
    {
        Highest = 1,
        High = 3,
        Normal = 5,
        Low = 7,
        Lowest = 10
    }

    #endregion

    #region VideoUploadSubType

    public enum VideoUploadSubType
    {
        Local =0,
        Platform,
        RemoteShare
    }

    #endregion

    #region PicUploadSubType
    
    public enum PicUploadSubType
    {
        Local = 0,
        RemoteShare
    }

    #endregion

    #region TaskSubType

    public enum TaskSubType
    {
        VideoLocal = 0,
        VideoPlatform,
        VideoRemoteShare,
        PicLocal,
        PicRemoteShare
    }

    #endregion

    #region TaskPriorityInfo

    public class TaskPriorityInfo
    {
        public TaskPriority Priority { get; set; }
        
        public uint NPriority
        {
            get
            {
                return (uint)Priority;
            }
        }

        public string Name { get; set; }

        public TaskPriorityInfo(TaskPriority priority, string name)
        {
            Priority = priority;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    #endregion

    #region TaskStatusInfo

    public class TaskStatusInfo
    {
        public E_VDA_TASK_STATUS Status { get; set; }

        public uint NStatus
        {
            get
            {
                return (uint)Status;
            }
        }

        public string Name { get; set; }

        public TaskStatusInfo(E_VDA_TASK_STATUS status, string name)
        {
            Status = status;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    #endregion

    #region AnalyzeStatusInfo

    public class AnalyzeStatusInfo
    {
        public E_VDA_TASK_UNIT_STATUS Status { get; set; }

        public uint NStatus
        {
            get
            {
                return (uint)Status;
            }
        }

        public string Name { get; set; }

        public AnalyzeStatusInfo(E_VDA_TASK_UNIT_STATUS status, string name)
        {
            Status = status;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    #endregion

    #region TaskUnitTypeInfo

    public class TaskUnitTypeInfo
    {
        public E_VDA_TASK_UNIT_TYPE Type { get; set; }

        public uint NType
        {
            get
            {
                return (uint)Type;
            }
        }

        public string Name { get; set; }

        public TaskUnitTypeInfo(E_VDA_TASK_UNIT_TYPE type, string name)
        {
            Type = type;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    #endregion

    #region TaskUnitImportStatusInfo

    public class TaskUnitImportStatusInfo
    {
        public E_VDA_TASK_UNIT_STATUS Status { get; set; }

        public uint NStatus
        {
            get
            {
                return (uint)Status;
            }
        }

        public string Name { get; set; }

        public TaskUnitImportStatusInfo(E_VDA_TASK_UNIT_STATUS status, string name)
        {
            Status = status;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    #endregion
}
