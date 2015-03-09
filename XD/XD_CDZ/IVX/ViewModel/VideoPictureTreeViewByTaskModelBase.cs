using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using BOCOM.DataModel;
using BOCOM.IVX.Framework;

namespace BOCOM.IVX.ViewModel
{
    public class VideoPictureTreeViewByTaskModelBase:ViewModelBase,IEventAggregatorSubscriber
    {
        #region Events
        
        public event EventHandler<EventAddNodeArgs> TreeNodeAdded;
        public event EventHandler<EventDeleteNodeArgs> TreeNodeDeleted;
        public event EventHandler<EventEditNodeArgs> TreeNodeEdited;

        #endregion

        #region Fields

        private readonly VideoPictureResource m_TaskVideoRootFolder = new VideoPictureResource(ResourceType.CameraVideoFolder, "点位视频");

        private TreeShowType m_viewType = TreeShowType.All;

        private List<VideoPictureResource> m_RootResources;

        private Dictionary<uint, TaskUnitInfo> m_DTID2TaskUnitInfo;

        private SortedDictionary<string, TaskUnitInfo> m_DTDisplayIndex2TaskUnitInfo;

        private bool m_IsForVideoSearch = true;

        private List<object> m_SelectedObjects;

        private TreeShowObjectFilter m_filter = TreeShowObjectFilter.NoUse;

        #endregion

        #region Properties

        public TreeShowType ShowObjectType
        {
            get { return m_viewType; }
        }

        //public TreeShowObjectFilter Filter
        //{
        //    get { return m_filter; }
        //    set { m_filter = value; }
        //}
        
        public List<TaskInfo> TaskGroups { get; private set; }
        
        public List<VideoPictureResource> RootResources
        {
            get
            {
                return m_RootResources;
            }

        }
        
        public bool IsForVideoSearch
        {
            get
            {
                return m_IsForVideoSearch;
            }
            set
            {
                m_IsForVideoSearch = value;
            }
        }
        
        public List<object> SelectedObjects
        {
            get { return m_SelectedObjects; }
        }

        #endregion

        #region Constructors

        public VideoPictureTreeViewByTaskModelBase(TreeShowType viewType = TreeShowType.All,TreeShowObjectFilter filter = TreeShowObjectFilter.NoUse)
        {
            m_DTID2TaskUnitInfo = new Dictionary<uint, TaskUnitInfo>();
            m_DTDisplayIndex2TaskUnitInfo = new SortedDictionary<string, TaskUnitInfo>();
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitAnalyseFinishedEvent>().Subscribe(OnTaskUnitAnalyseFinished, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitImportFinishedEvent>().Subscribe(OnTaskUnitImportFinished, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitAddedEvent>().Subscribe(OnTaskUnitAdded, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitDeletedEvent>().Subscribe(OnTaskUnitDeleted, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            //Framework.Container.Instance.EvtAggregator.GetEvent<TaskAddedEvent>().Subscribe(OnTaskAdded, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskModifiedEvent>().Subscribe(OnTaskModified, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskDeletedEvent>().Subscribe(OnTaskDeleted, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);

            Framework.Container.Instance.RegisterEventSubscriber(this);

            m_viewType = viewType;
            m_filter = filter;

            UpdateTaskGroups();

            FillupResources();
        }

        #endregion

        #region Private helper functions

        private void UpdateTaskGroups()
        {
            try
            {
                TaskGroups = Framework.Container.Instance.TaskManagerService.GetAllTaskList();
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "获取task");
            }
        }

        private void FillupResources()
        {
            m_RootResources = new List<VideoPictureResource>();
            FillupTaskResources();
        }

        private void FillupTaskResources()
        {
            if (TaskGroups != null)
            {
                foreach (TaskInfo task in TaskGroups)
                {
                    VideoPictureResource resource = new VideoPictureResource(ResourceType.Task, task.TaskName, task);
                    FillupTaskUnits(resource);
                    if(resource.Children.Count>0)
                        m_RootResources.Add(resource);
                }
                // folder.Expand = true;
            }
        }



        private void FillupTaskUnits(VideoPictureResource parentResource)
        {
            TaskInfo task = parentResource.Subject as TaskInfo;
            List<TaskUnitInfo> taskUnits = null;
            try
            {
                taskUnits = Framework.Container.Instance.TaskManagerService.GetTaskUintListByTaskID(task.TaskID);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "获取点位任务单元集合");
            }
            
            if (taskUnits != null /*&& taskUnits.Count > 0*/)
            {
                VideoPictureResource child;
                foreach (TaskUnitInfo unit in taskUnits)
                {
                    if (Match(unit, m_filter))
                    {
                        child = new VideoPictureResource(ResourceType.VideoFile,
                            string.Format("{0}", unit.TaskUnitName), unit);

                        //child = new VideoPictureResource(ResourceType.VideoFile,
                        //    string.Format("{0}[{1} - {2}]", unit.TaskUnitName, unit.StartTime.ToString(DataModel.Constant.DATETIME_FORMAT),
                        //    unit.EndTime.ToString(DataModel.Constant.DATETIME_FORMAT)), unit);
                        parentResource.AddChild(child);
                    }
                }
                //parentResource.Expand = true;
            }
        }

       
        private bool Match(TaskUnitInfo taskunit ,TreeShowObjectFilter filter)
        {
            switch ((E_VDA_TASK_UNIT_STATUS)taskunit.ImportStatus)
            {
                case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_NOUSE:
                case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_IMPORT_WAIT:
                case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_IMPORT_READY:
                case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_IMPORT_EXECUTING:
                case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_IMPORT_FAILED:
                case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_IMPORT_FINISH:
                case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_PREANALYSE_EXECUTING:
                case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_PREANALYSE_FAILED:
                case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_PREANALYSE_WAIT:
                    return false;
            }

            if (filter == TreeShowObjectFilter.NoUse)
                return true;

            bool retval = false;
            foreach (E_VDA_ANALYZE_TYPE type in taskunit.AnalyzeStatus.Keys)
            {
                E_VDA_TASK_UNIT_STATUS status = taskunit.AnalyzeStatus[type];
                if ((E_VDA_TASK_UNIT_STATUS)status != E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_FINISH)
                    continue;

                //switch ((E_VDA_TASK_UNIT_STATUS)status)
                //{
                //    case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_WAIT:
                //    case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_EXECUTING:
                //    case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_FAILED:
                //        continue;
                //}

                if ((filter & TreeShowObjectFilter.Brief)>0 && type == E_VDA_ANALYZE_TYPE.E_ANALYZE_BRIEAF)
                    retval = true;
                else if ((filter & TreeShowObjectFilter.Face)>0 && type ==E_VDA_ANALYZE_TYPE.E_ANALYZE_FACE)
                    retval = true;
                else if ((filter & TreeShowObjectFilter.Car)>0 && type == E_VDA_ANALYZE_TYPE.E_ANALYZE_VEHICLE)
                    retval = true;
                else if ((filter & TreeShowObjectFilter.Object)>0 && type == E_VDA_ANALYZE_TYPE.E_ANALYZE_OBJECT)
                    retval = true;
            }
            return retval;
        }

        private VideoPictureResource GetResourceByTaskUnit(VideoPictureResource parantres, TaskUnitInfo taskUnitInfo)
        {
            if (parantres.Subject is TaskUnitInfo)
            {
                TaskUnitInfo info = parantres.Subject as TaskUnitInfo;
                if (info.TaskUnitID == taskUnitInfo.TaskUnitID)
                    return parantres;
            }
            else
            {
                foreach (VideoPictureResource item in parantres.Children)
                {
                    VideoPictureResource ret = GetResourceByTaskUnit(item, taskUnitInfo);
                    if (ret != null)
                        return ret;
                }
            }
            return null;
        }

        private VideoPictureResource GetResourceByTask(VideoPictureResource parantres, TaskInfo taskInfo)
        {
            if (parantres.Subject is TaskInfo)
            {
                TaskInfo info = parantres.Subject as TaskInfo;
                if (info.TaskID == taskInfo.TaskID)
                    return parantres;
            }
            else
            {
                foreach (VideoPictureResource item in parantres.Children)
                {
                    VideoPictureResource ret = GetResourceByTask(item, taskInfo);
                    if (ret != null)
                        return ret;
                }
            }
            return null;
        }

        
        #endregion

        #region Public helper functions

        public void RetrieveChildren(VideoPictureResource parentResource)
        {
            //if(!parentResource.Expand)
            //{
            //    if (m_viewType == TreeShowType.Camera)
            //    {
            //        return;
            //    }

            //    if (parentResource.Type == ResourceType.Task)
            //    {
            //        FillupTaskUnits(parentResource);
            //    }
            //}
        }
        
        public void CameraSelectionChanged(List<object> list)
        {
            // Framework.Container.Instance.EvtAggregator.GetEvent<BOCOM.IVX.Framework.CameraSelectionChangedEvent>().Publish(list);
            m_SelectedObjects = list;
        }

        public void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitAnalyseFinishedEvent>().Unsubscribe(OnTaskUnitAnalyseFinished);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitImportFinishedEvent>().Unsubscribe(OnTaskUnitImportFinished);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitAddedEvent>().Unsubscribe(OnTaskUnitAdded);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitDeletedEvent>().Unsubscribe(OnTaskUnitDeleted);
            //Framework.Container.Instance.EvtAggregator.GetEvent<TaskAddedEvent>().Unsubscribe(OnTaskAdded);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskModifiedEvent>().Unsubscribe(OnTaskModified);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskDeletedEvent>().Unsubscribe(OnTaskDeleted);
        }

        public void UpdateCheckedResources(VideoPictureResource resource, bool isChecked)
        {
            // m_CheckedTaskUnitInfos.Clear();

            UpdateCheckedResourcesEx(resource, isChecked);

            if (IsForVideoSearch)
            {
                Framework.Container.Instance.SelectedTaskUnitsForSearch = m_DTDisplayIndex2TaskUnitInfo.Values.ToArray();
            }
        }

        public void UpdateCheckedResourcesEx(VideoPictureResource resource, bool isChecked)
        {
            if (resource != null)
            {
                if (resource.Type == ResourceType.VideoFile)
                {
                    TaskUnitInfo taskUnit = resource.Subject as TaskUnitInfo;
                    if (taskUnit != null)
                    {
                        if (isChecked && !m_DTID2TaskUnitInfo.ContainsKey(taskUnit.TaskUnitID))
                        {
                            m_DTID2TaskUnitInfo.Add(taskUnit.TaskUnitID, taskUnit);
                            m_DTDisplayIndex2TaskUnitInfo.Add(resource.DisplayIndex, taskUnit);
                        }
                        else if (!isChecked && m_DTID2TaskUnitInfo.ContainsKey(taskUnit.TaskUnitID))
                        {
                            m_DTID2TaskUnitInfo.Remove(taskUnit.TaskUnitID);
                            m_DTDisplayIndex2TaskUnitInfo.Remove(resource.DisplayIndex);
                        }
                    }
                }

                //if (resource.Children != null && resource.Children.Count > 0)
                //{
                //    foreach (VideoPictureResource child in resource.Children)
                //    {
                //        UpdateCheckedResourcesEx(child, isChecked);
                //    }
                //}
            }
        }

        #endregion

        #region Event handlers

        //VideoPictureResource OnTaskAdded(TaskInfo info)
        //{
        //    foreach (VideoPictureResource item in m_RootResources)
        //    {
        //        VideoPictureResource task = GetResourceByTask(item, info);
        //        if (task != null)
        //            return task;
        //    }


        //    VideoPictureResource resource =
        //        new VideoPictureResource(ResourceType.Task, info.TaskName, info);

        //    m_RootResources.Add(resource);

        //    if (resource != null)
        //    {
        //        if (TreeNodeAdded != null)
        //            TreeNodeAdded(null, new EventAddNodeArgs { NodeResource = resource });
        //    }
        //    return resource;
        //}

        void OnTaskModified(TaskInfo info)
        {
            VideoPictureResource task = null;
            foreach (VideoPictureResource child in m_RootResources)
            {
                task = GetResourceByTask(child, info);
                if (task != null)
                    break;

            }
            if (task != null)
            {
                task.Name = info.TaskName;
                task.Subject = info;

                if (/*task.Expand &&*/ TreeNodeEdited != null)
                {
                    TreeNodeEdited(null, new EventEditNodeArgs { NodeResource = task });
                }
            }
        }

        void OnTaskDeleted(uint taskID)
        {
            VideoPictureResource task = null;
            foreach (VideoPictureResource child in m_RootResources)
            {
                task = GetResourceByTask(child, new TaskInfo {  TaskID = taskID });
                if (task != null)
                    break;
            }
            if (task != null)
            {
                if (TreeNodeDeleted != null)
                    TreeNodeDeleted(null, new EventDeleteNodeArgs { NodeResource = task });
            }

        }


        private void OnTaskUnitAdded(TaskUnitInfo taskUnitInfo)
        {
            foreach (VideoPictureResource item in m_RootResources)
            {
                VideoPictureResource taskunitres = GetResourceByTaskUnit(item, taskUnitInfo);
                if (taskunitres != null)
                    return;
            }

            VideoPictureResource task = null;
            foreach (VideoPictureResource item in m_RootResources)
            {
                task = GetResourceByTask(item, new TaskInfo {  TaskID = taskUnitInfo.TaskID });
                if (task != null)
                    break;
            }
            if (task == null)
            {
                TaskInfo taskinfo = Framework.Container.Instance.TaskManagerService.GetTaskByID(taskUnitInfo.TaskID);
                task = new VideoPictureResource(ResourceType.Task, taskinfo.TaskName, taskinfo);
                VideoPictureResource child =null;
                if (Match(taskUnitInfo, m_filter))
                {
                    child = new VideoPictureResource(ResourceType.VideoFile,
                            string.Format("{0}", taskUnitInfo.TaskUnitName), taskUnitInfo);

                    task.AddChild(child);

                }
                if (task.Children.Count > 0)
                {
                    m_RootResources.Add(task);

                    if (TreeNodeAdded != null)
                        TreeNodeAdded(null, new EventAddNodeArgs { NodeResource = task });

                    if (TreeNodeAdded != null)
                        TreeNodeAdded(null, new EventAddNodeArgs { ParantTreeNode = task.TreeNode, NodeResource = child });
                }
            }
            else
            {
                if (Match(taskUnitInfo, m_filter))
                {
                    VideoPictureResource child = new VideoPictureResource(ResourceType.VideoFile,
                            string.Format("{0}", taskUnitInfo.TaskUnitName), taskUnitInfo);

                    task.AddChild(child);

                    if (TreeNodeAdded != null)
                        TreeNodeAdded(null, new EventAddNodeArgs { ParantTreeNode = task.TreeNode, NodeResource = child });
                }
            }
        }

        private void OnTaskUnitDeleted(uint taskUnitID)
        {
            VideoPictureResource taskunitres = null;
            foreach (VideoPictureResource item in m_RootResources)
            {
                taskunitres = GetResourceByTaskUnit(item, new TaskUnitInfo {  TaskUnitID = taskUnitID });
                if (taskunitres != null)
                    break;

            }

            if (taskunitres != null)
            {
                // 删除的任务单元， 需要在CheckedResources 集合中删除
                UpdateCheckedResources(taskunitres, false);

                if (TreeNodeDeleted != null)
                {
                    TreeNodeDeleted(null, new EventDeleteNodeArgs { NodeResource = taskunitres });
                }
            }

        }
        
        void OnTaskUnitAnalyseFinished(uint taskUnitId)
        {
            TaskUnitInfo taskUnitInfo = null;
            try
            {
                taskUnitInfo = Framework.Container.Instance.TaskManagerService.GetTaskUnitById(taskUnitId);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "根据编号获取任务单元");
                return;
            }
            VideoPictureResource taskunitres = null;
            foreach (VideoPictureResource item in m_RootResources)
            {
                taskunitres = GetResourceByTaskUnit(item, new TaskUnitInfo { TaskUnitID = taskUnitId });
                if (taskunitres != null)
                    break;
            }

            if (taskunitres != null)
            {
                string name = taskUnitInfo.TaskUnitName;
                //if (taskUnitInfo.CameraId != 0)
                //{
                //    name = string.Format("{0}[{1} - {2}]", taskUnitInfo.TaskUnitName, taskUnitInfo.StartTime.ToString(DataModel.Constant.DATETIME_FORMAT),
                //            taskUnitInfo.EndTime.ToString(DataModel.Constant.DATETIME_FORMAT));
                //}
                //else
                //{
                    //name = string.Format("{0}", taskUnitInfo.TaskUnitName);
                //}

                taskunitres.Name = name;
                taskunitres.Subject = taskUnitInfo;

                if (/*taskunitres.Expand &&*/ TreeNodeEdited != null)
                    TreeNodeEdited(null, new EventEditNodeArgs { NodeResource = taskunitres });
            }
            else
            {
                OnTaskUnitAdded(taskUnitInfo);
            }

        }

        void OnTaskUnitImportFinished(uint taskUnitId)
        {
            try
            {
                TaskUnitInfo taskUnitInfo = Framework.Container.Instance.TaskManagerService.GetTaskUnitById(taskUnitId);
                OnTaskUnitAdded(taskUnitInfo);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "根据编号获取任务单元");
            }
            
        }

        private void UpdateResourceByTaskID(uint ID, VideoPictureResource res)
        {
            if (res.Type == ResourceType.Task && res.Subject is TaskInfo)
            {
                TaskInfo camera = res.Subject as TaskInfo;
                if (ID == camera.TaskID)
                {
                    res.Children.Clear();
                    FillupTaskUnits(res);
                    return;
                }
            }

            res.Children.ForEach(item => UpdateResourceByTaskID(ID,item));

        }


        #endregion
    }

    #region Other classes

    public enum TreeShowType
    {
        All,
        Video,
        Picture,
        Camera
    }

    [FlagsAttribute]
    public enum TreeShowObjectFilter
    {
        NoUse = 0,
        Object =1,
        Face = 2,
        Car = 4,
        Brief = 8,
    }

    public class EventAddNodeArgs : EventArgs
    {
        public object ParantTreeNode { get; set; }
        public VideoPictureResource NodeResource { get; set; }
    }
    public class EventDeleteNodeArgs : EventArgs
    {
        public VideoPictureResource NodeResource { get; set; }
    }
    public class EventEditNodeArgs : EventArgs
    {
        public VideoPictureResource NodeResource { get; set; }
    }
    #endregion
        
}
