using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using BOCOM.DataModel;
using BOCOM.IVX.Protocol;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace BOCOM.IVX.ViewModel
{
    public class CreateTaskViewModel : ViewModelBase
    {
        #region Fields
                
        private TaskPriorityInfo[] m_priorityInfos;

        private TaskType m_TaskType = TaskType.VideoUpload;

        private TaskSubType m_TaskSubType = TaskSubType.VideoLocal;

        private TaskSubType m_LastVideoTaskSubType = TaskSubType.VideoLocal;

        private TaskSubType m_LastPicTaskSubType = TaskSubType.PicLocal;

        private VideoUploadSubType m_subVideoTaskType = VideoUploadSubType.Local;

        private PicUploadSubType m_subPicTaskType = PicUploadSubType.Local;

        private string m_TaskName = "新建任务1";

        private TaskPriority m_Priority;

        private TaskInfo m_TaskInfo;

        private bool m_IsNew = true;

        private List<ImportVMBase> m_ImportViewModels;

        private List<VideoSupplierDeviceInfo> m_VideoSupplierDevices;

        private VideoSupplierDeviceInfo m_SelectedVideoSupplierDevice;

        private string m_ImportPlatformVideoPageDescription;

        #endregion

        #region Properties

        public string TaskName
        {
            get { return m_TaskName; }
            set
            {
                m_TaskName = value;
                RaisePropertyChangedEvent("TaskName");
            }
        }

        public bool IsNew
        {
            get { return m_IsNew; }
            set { m_IsNew = value; }
        }

        public uint TaskId
        {
            get
            {
                if (m_TaskInfo == null)
                {
                    return 0;
                }
                else
                {
                    return m_TaskInfo.TaskID;
                }
            }
        }

        public TaskPriorityInfo[] PriorityInfos
        {
            get { return m_priorityInfos; }
        }

        public TaskPriority Priority
        {
            get
            {
                return m_Priority;
            }
            set
            {
                m_Priority = value;
            }
        }

        public TaskType SelectedTaskType
        {
            get { return m_TaskType; }
            set
            {
                if (m_TaskType != value)
                {
                    m_TaskType = value;
                    if (m_TaskType == TaskType.VideoUpload)
                    {
                        SelectedTaskSubType = m_LastVideoTaskSubType;
                    }
                    else
                    {
                        SelectedTaskSubType = m_LastPicTaskSubType;
                    }
                }
            }
        }

        public TaskSubType SelectedTaskSubType
        {
            get
            {
                return m_TaskSubType;
            }
            set
            {
                if (m_TaskSubType != value)
                {
                    bool parentTypeChanged = false;
                    if (((int)value) < 3)
                    {
                        m_LastVideoTaskSubType = value;
                        if (((int)m_TaskSubType) >= 3)
                        {
                            parentTypeChanged = true;
                        }
                    }
                    else
                    {
                        m_LastPicTaskSubType = value;
                        if (((int)m_TaskSubType) < 3)
                        {
                            parentTypeChanged = true;
                        }
                    }

                    string propertyName = string.Format("Is{0}", m_TaskSubType.ToString());
                    m_TaskSubType = value;
                    RaisePropertyChangedEvent(propertyName);
                    propertyName = string.Format("Is{0}", m_TaskSubType.ToString());
                    RaisePropertyChangedEvent(propertyName);

                    if (parentTypeChanged)
                    {
                        RaisePropertyChangedEvent("IsVideoTask");
                        RaisePropertyChangedEvent("IsPictureTask");
                    }
                }
            }
        }

        public VideoUploadSubType SelectedVideoUploadSubType
        {
            get
            {
                return m_subVideoTaskType;
            }
        }

        public PicUploadSubType SelectedPicUploadSubType
        {
            get
            {
                return m_subPicTaskType;
            }
        }

        public int NSelectedTaskType
        {
            get { return (int)m_TaskType; }
            set
            {
                TaskType tt = (TaskType)value;
                SelectedTaskType = tt;
            }
        }

        public bool IsVideoTask
        {
            get
            {
                return m_TaskType == TaskType.VideoUpload;
            }
            set
            {
            }
        }

        public bool IsPictureTask
        {
            get
            {
                return m_TaskType == TaskType.PicUpload;
            }
            set
            {
            }
        }

        public bool IsVideoLocal
        {
            get
            {
                return m_TaskSubType == TaskSubType.VideoLocal;
            }
            set
            {
                if (value)
                {
                    SelectedTaskSubType = TaskSubType.VideoLocal;
                }
            }
        }

        public bool IsVideoPlatform
        {
            get
            {
                return m_TaskSubType == TaskSubType.VideoPlatform;
            }
            set
            {
                if (value)
                {
                    SelectedTaskSubType = TaskSubType.VideoPlatform;
                }
                RaisePropertyChangedEvent("ShowSelectVideoSupplierDevicePage");
            }
        }

        public bool IsVideoRemoteShare
        {
            get
            {
                return m_TaskSubType == TaskSubType.VideoRemoteShare;
            }
            set
            {
                if (value)
                {
                    SelectedTaskSubType = TaskSubType.VideoRemoteShare;
                }
            }
        }
        
        public bool IsPicLocal
        {
            get
            {
                return m_TaskSubType == TaskSubType.PicLocal;
            }
            set
            {
                if (value)
                {
                    SelectedTaskSubType = TaskSubType.PicLocal;
                }
            }
        }
        
        public bool IsPicRemoteShare
        {
            get
            {
                return m_TaskSubType == TaskSubType.PicRemoteShare;
            }
            set
            {
                if (value)
                {
                    SelectedTaskSubType = TaskSubType.PicRemoteShare;
                }
            }
        }

        public List<VideoSupplierDeviceInfo> VideoSupplierDevices
        {
            get
            {
                return m_VideoSupplierDevices;
            }
        }

        public bool HasVideoSupplierDevice
        {
            get
            {
                return m_VideoSupplierDevices != null && m_VideoSupplierDevices.Count > 0;
            }
            set { }
        }

        public bool OnlyOneVideoSupplierDevice
        {
            get
            {
                return m_VideoSupplierDevices != null && m_VideoSupplierDevices.Count == 1;
            }
            set { }
        }

        public bool ShowSelectVideoSupplierDevicePage
        {
            get
            {
                return IsVideoPlatform && m_VideoSupplierDevices != null && m_VideoSupplierDevices.Count > 1;
            }
            set
            {

            }
        }

        public string ImportPlatformVideoPageDescription
        {
            get
            {
                string sRet = "每次导入视频数最多为 50 个";
                if (m_SelectedVideoSupplierDevice != null)
                {
                    sRet = string.Format("从设备 {0} 导入历史视频, {1}", m_SelectedVideoSupplierDevice.DeviceName, sRet);
                }
                return sRet;
            }
            set
            {
                
            }
        }

        public VideoSupplierDeviceInfo SelectedVideoSupplierDevice
        {
            get
            {
                return m_SelectedVideoSupplierDevice;
            }
            set
            {
                if (m_SelectedVideoSupplierDevice != value)
                {
                    m_SelectedVideoSupplierDevice = value;
                    RaisePropertyChangedEvent("ImportPlatformVideoPageDescription");
                }
            }
        }
        
        #endregion

        #region Constructors

        public CreateTaskViewModel()
        {
            m_priorityInfos = DataModel.Constant.TaskPriorityInfos;

            m_Priority = m_priorityInfos[2].Priority;
            m_ImportViewModels = new List<ImportVMBase>();

            List<TaskInfo> tasks = Framework.Container.Instance.TaskManagerService.GetAllTaskList();


            if (tasks != null && tasks.Count > 0)
            {
                List<string> names = new List<string>();
                foreach(TaskInfo task in tasks)
                {
                    if (!names.Contains(task.TaskName))
                    {
                        names.Add(task.TaskName);
                    }
                }
                this.TaskName = Common.TextUtil.GetNameWithIncreaseNO("新建任务", names, 1);
            }

            m_VideoSupplierDevices = Framework.Container.Instance.VDAConfigService.GetAllVideoSupplierDevice();

            if (m_VideoSupplierDevices != null && m_VideoSupplierDevices.Count == 1)
            {
                m_SelectedVideoSupplierDevice = m_VideoSupplierDevices[0];
            }
        }

        #endregion
        
        #region Public helper functions

        public void SetTask(TaskInfo taskInfo)
        {
            m_TaskInfo = taskInfo;
            IsNew = false;
            TaskName = taskInfo.TaskName;
            Priority = (TaskPriority)taskInfo.TaskPriorityLevel;
        }

        public void AddImportViewModel(ImportVMBase viewModel)
        {
            if (!m_ImportViewModels.Contains(viewModel))
            {
                m_ImportViewModels.Add(viewModel);
            }
        }

        public void Commit()
        {
            bool valid = true;
            if (IsNew)
            {
                valid = ValidateTask();
            }

            if (valid)
            {
                try
                {
                    m_ImportViewModels.ForEach(vm =>
                    {
                        if (vm.Validate())
                            vm.Commit();
                    });
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "添加任务");
                }
            }
        }

        public bool Validate()
        {
            try
            {
                bool ret = true ;
                m_ImportViewModels.ForEach(vm => 
                {
                    ret = ret && vm.Validate();
                });
                return ret;
            }
            catch (SDKCallException ex)
            {
                string msg = string.Format("添加任务检查出错, {0}", ex.Message);
                Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME,
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool ValidateTask()
        {
            bool bRetName = true;
            if (IsNew)
            {
                bRetName = CheckName();
            }
            return bRetName;
        }

        #endregion

        private bool CheckName()
        {
            string msg;
            string str = TaskName;
            bool bRet = Common.TextUtil.ValidateNameText(ref str, false, "任务名称", 1, DataModel.Common.VDASDK_MAX_NAME_LEN-1, out msg);
            TaskName = str;

            if (!bRet)
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME,
                       MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return bRet;
        }
    }
   
}
