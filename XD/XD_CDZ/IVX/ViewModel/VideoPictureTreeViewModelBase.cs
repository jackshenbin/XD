using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using BOCOM.DataModel;
using BOCOM.IVX.Framework;

namespace BOCOM.IVX.ViewModel
{
    public class VideoPictureTreeViewModelBase:ViewModelBase,IEventAggregatorSubscriber
    {
        #region Events
        
        public event EventHandler<EventAddNodeArgs> TreeNodeAdded;
        public event EventHandler<EventDeleteNodeArgs> TreeNodeDeleted;
        public event EventHandler<EventEditNodeArgs> TreeNodeEdited;

        #endregion

        #region Fields

        private readonly VideoPictureResource m_CameraVideoRootFolder = new VideoPictureResource(ResourceType.CameraVideoFolder, "点位视频");

        private readonly VideoPictureResource m_NoneCameraVideoRootFolder = new VideoPictureResource(ResourceType.NonCameraVideoFolder, "无点位视频", new CameraInfo { CameraID = 0 });

        private readonly VideoPictureResource m_CameraPicsRootFolder = new VideoPictureResource(ResourceType.CameraPicFolder, "点位图片");

        private readonly  VideoPictureResource m_NoneCameraPicsRootFolder = new VideoPictureResource(ResourceType.NonCameraPicFolder, "无点位图片");

        private TreeShowType m_viewType = TreeShowType.All;

        private List<VideoPictureResource> m_RootResources;

        private Dictionary<uint, TaskUnitInfo> m_DTID2TaskUnitInfo;

        private bool m_IsForVideoSearch = true;

        private List<object> m_SelectedObjects;

        private TreeShowObjectFilter m_filter = TreeShowObjectFilter.NoUse;

        #endregion

        #region Properties

        public TreeShowType ShowObjectType
        {
            get { return m_viewType; }
        }

        public TreeShowObjectFilter Filter
        {
            get { return m_filter; }
            set { m_filter = value; }
        }
        
        public List<CameraGroupInfo> CameraGroups { get; private set; }
        
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

        public VideoPictureTreeViewModelBase(TreeShowType viewType = TreeShowType.All)
        {
            m_DTID2TaskUnitInfo = new Dictionary<uint, TaskUnitInfo>();
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraAddedEvent>().Subscribe(OnCameraAdded);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraModifiedEvent>().Subscribe(OnCameraModified);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraDeletedEvent>().Subscribe(OnCameraDeleted);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraGroupAddedEvent>().Subscribe(OnCameraGroupAdded);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraGroupModifiedEvent>().Subscribe(OnCameraGroupModified);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraGroupDeletedEvent>().Subscribe(OnCameraGroupDeleted);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitAnalyseFinishedEvent>().Subscribe(OnTaskUnitAnalyseFinished, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitImportFinishedEvent>().Subscribe(OnTaskUnitImportFinished, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitAddedEvent>().Subscribe(OnTaskUnitAdded, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitDeletedEvent>().Subscribe(OnTaskUnitDeleted, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            
            Framework.Container.Instance.RegisterEventSubscriber(this);

            m_viewType = viewType;

            UpdateCameraGroups();

            FillupResources();
        }

        #endregion

        #region Private helper functions

        private void UpdateCameraGroups()
        {
            try
            {
                CameraGroups = Framework.Container.Instance.VDAConfigService.GetAllCameraGroup(0);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "获取播放状态");
            }
        }

        private void FillupResources()
        {
            m_RootResources = new List<VideoPictureResource>();
            if (m_viewType == TreeShowType.Video)
            {
                FillupVideoFolders();
            }
            else if (m_viewType == TreeShowType.Picture)
            {
                FillupPicFolders();
            }
            else if(m_viewType == TreeShowType.All)
            {
                FillupVideoFolders();
                FillupPicFolders();
            }
            else
            {
                FillupCameraResources();
            }
        }

        private void FillupCameraResources()
        {
            if (CameraGroups != null)
            {
                foreach (CameraGroupInfo cameragroup in CameraGroups)
                {
                    VideoPictureResource resource = new VideoPictureResource(ResourceType.CameraGroup, cameragroup.GroupName, cameragroup);
                    FillupCameraResources(resource, ResourceType.Camera, cameragroup);

                    m_RootResources.Add(resource);
                }
                // folder.Expand = true;
            }
        }

        private void FillupVideoFolders()
        {
            m_RootResources.Add(m_CameraVideoRootFolder);
            FillupCameraGroupResources(m_CameraVideoRootFolder, ResourceType.CameraGroup);
            
            m_RootResources.Add(m_NoneCameraVideoRootFolder);
        }

        private void FillupPicFolders()
        {
            m_RootResources.Add(m_CameraPicsRootFolder);
            FillupCameraGroupResources(m_CameraPicsRootFolder, ResourceType.CameraGroup);
                        
            m_RootResources.Add(m_NoneCameraPicsRootFolder);
        }

        private void FillupCameraGroupResources(VideoPictureResource folder, ResourceType resourceType)
        {
            if (CameraGroups != null /*&& TaskGroups.Count > 0*/)
            {
                foreach (CameraGroupInfo cameragroup in CameraGroups)
                {
                    VideoPictureResource resource =
                        new VideoPictureResource(resourceType, cameragroup.GroupName, cameragroup);
                    FillupCameraResources(resource, ResourceType.Camera,cameragroup);

                    folder.AddChild(resource);
                }
                //folder.Expand = true;
            }
        }

        private void FillupCameraResources(VideoPictureResource folder, ResourceType resourceType, CameraGroupInfo cameragroup)
        {
            List<CameraInfo> Cameras = new List<CameraInfo>();
            try
            { 
                Framework.Container.Instance.VDAConfigService.GetAllCameras(cameragroup.CameraGroupID, Cameras);
            }
            catch (SDKCallException ex)
            {
                Cameras = null;
                Common.SDKCallExceptionHandler.Handle(ex, "获取组监控点集合");
            }

            if (Cameras != null /*&& taskunits.Count > 0*/)
            {
                foreach (CameraInfo camera in Cameras)
                {
                    VideoPictureResource resource = new VideoPictureResource(resourceType, camera.CameraName, camera);
                    folder.AddChild(resource);
                }
                //folder.Expand = true;
            }
        }

        private void FillupCameraVideos(VideoPictureResource parentResource)
        {
            CameraInfo camera = parentResource.Subject as CameraInfo;
            List<TaskUnitInfo> taskUnits = null;
            try
            {
                taskUnits = Framework.Container.Instance.TaskManagerService.GetTaskUnitsByCameraID(camera.CameraID);
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
                            string.Format("{0}[{1} - {2}]", unit.TaskUnitName, unit.StartTime.ToString(DataModel.Constant.DATETIME_FORMAT),
                            unit.EndTime.ToString(DataModel.Constant.DATETIME_FORMAT)), unit);
                        parentResource.AddChild(child);
                    }
                }
                //parentResource.Expand = true;
            }
        }

        private void FillupNonCameraVideos(VideoPictureResource parentResource)
        {
            List<TaskUnitInfo> taskUnits = null;
            try
            {
                taskUnits = Framework.Container.Instance.TaskManagerService.GetTaskUnitsWithoutCamera();
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "获取非点位任务单元集合");
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

        private VideoPictureResource GetResourceByCamera(VideoPictureResource parantres, CameraInfo cameraInfo)
        {
            if (parantres.Subject is CameraInfo)
            {
                CameraInfo info = parantres.Subject as CameraInfo;
                if (info.CameraID == cameraInfo.CameraID)
                    return parantres;
            }
            else
            {
                foreach (VideoPictureResource item in parantres.Children)
                {
                    VideoPictureResource ret = GetResourceByCamera(item, cameraInfo);
                    if (ret != null)
                        return ret;
                }
            }
            return null;
        }

        private VideoPictureResource GetResourceByCameraGroup(VideoPictureResource parantres, CameraGroupInfo cameragroupInfo)
        {
            if (parantres.Subject is CameraGroupInfo)
            {
                CameraGroupInfo info = parantres.Subject as CameraGroupInfo;
                if (info.CameraGroupID == cameragroupInfo.CameraGroupID)
                    return parantres;
            }
            else
            {
                foreach (VideoPictureResource item in parantres.Children)
                {
                    VideoPictureResource ret = GetResourceByCameraGroup(item, cameragroupInfo);
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

            //    if (parentResource.Type == ResourceType.Camera)
            //    {
            //        FillupCameraVideos(parentResource);
            //    }
            //    else if (parentResource.Type == ResourceType.NonCameraVideoFolder)
            //    {
            //        FillupNonCameraVideos(parentResource);
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
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraAddedEvent>().Unsubscribe(OnCameraAdded);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraModifiedEvent>().Unsubscribe(OnCameraModified);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraDeletedEvent>().Unsubscribe(OnCameraDeleted);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraGroupAddedEvent>().Unsubscribe(OnCameraGroupAdded);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraGroupModifiedEvent>().Unsubscribe(OnCameraGroupModified);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraGroupDeletedEvent>().Unsubscribe(OnCameraGroupDeleted);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitAnalyseFinishedEvent>().Unsubscribe(OnTaskUnitAnalyseFinished);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitImportFinishedEvent>().Unsubscribe(OnTaskUnitImportFinished);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitAddedEvent>().Unsubscribe(OnTaskUnitAdded);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitDeletedEvent>().Unsubscribe(OnTaskUnitDeleted);
            //Framework.Container.Instance.EvtAggregator.GetEvent<TaskAddedEvent>().Unsubscribe(OnTaskAdded);
            //Framework.Container.Instance.EvtAggregator.GetEvent<TaskDeletedEvent>().Unsubscribe(OnTaskDeleted);
        }

        public void UpdateCheckedResources(VideoPictureResource resource, bool isChecked)
        {
            // m_CheckedTaskUnitInfos.Clear();

            UpdateCheckedResourcesEx(resource, isChecked);

            if (IsForVideoSearch)
            {
                Framework.Container.Instance.SelectedTaskUnitsForSearch = m_DTID2TaskUnitInfo.Values.ToArray();
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
                        }
                        else if (!isChecked && m_DTID2TaskUnitInfo.ContainsKey(taskUnit.TaskUnitID))
                        {
                            m_DTID2TaskUnitInfo.Remove(taskUnit.TaskUnitID);
                        }
                    }
                }

                if (resource.Children != null && resource.Children.Count > 0)
                {
                    foreach (VideoPictureResource child in resource.Children)
                    {
                        UpdateCheckedResourcesEx(child, isChecked);
                    }
                }
            }
        }

        #endregion

        #region Event handlers
        
        void OnCameraAdded(CameraInfo info)
        {
            foreach (VideoPictureResource item in m_RootResources)
            {
                VideoPictureResource camres = GetResourceByCamera(item, info);
                if (camres != null)
                    return;
            }

            VideoPictureResource groupres = null;
            foreach (VideoPictureResource child in m_RootResources)
            {
                groupres = GetResourceByCameraGroup(child, new CameraGroupInfo { CameraGroupID = info.GroupID });
                if (groupres != null)
                {
                    break;
                }
            }

            if (groupres != null)
            {
                VideoPictureResource resource =
                    new VideoPictureResource(ResourceType.Camera, info.CameraName, info);
                groupres.AddChild(resource);

                //if (groupres.Expand)
                //{
                    if (TreeNodeAdded != null)
                        TreeNodeAdded(null, new EventAddNodeArgs { ParantTreeNode = groupres.TreeNode, NodeResource = resource });
                //}
            }
            else
            {
                foreach (VideoPictureResource child in m_RootResources)
                {
                    if (child.Type == ResourceType.CameraVideoFolder || child.Type == ResourceType.CameraPicFolder)
                    { 
                        CameraGroupInfo group = Framework.Container.Instance.VDAConfigService.GetCameraGroupByID(info.GroupID);
                        VideoPictureResource resource =
                            new VideoPictureResource(ResourceType.CameraGroup, group.GroupName, group);
                        FillupCameraResources(resource, ResourceType.Camera, group);

                        child.AddChild(resource);
                        //if (child.Expand)
                        //{
                            if (TreeNodeAdded != null)
                                TreeNodeAdded(null, new EventAddNodeArgs { ParantTreeNode = child.TreeNode, NodeResource = resource });
                        //}
                    }
                }
            }
        }

        void OnCameraModified(CameraInfo info)
        {
            VideoPictureResource camres = null;
            foreach (VideoPictureResource child in m_RootResources)
            {
                camres = GetResourceByCamera(child,info);
                if (camres != null)
                    break;

            }
            if (camres != null)
            {
                camres.Name = info.CameraName;
                camres.Subject = info;

                if (/*camres.Expand &&*/ TreeNodeEdited != null)
                {
                    TreeNodeEdited(null, new EventEditNodeArgs { NodeResource = camres });
                }
            }
            else
            {
                OnCameraAdded(info);
            }
        }

        void OnCameraDeleted(uint camID)
        {
            VideoPictureResource camres = null;
            foreach (VideoPictureResource child in m_RootResources)
            {
                camres = GetResourceByCamera(child, new CameraInfo {   CameraID = camID });
                if (camres != null)
                    break;
            }
            if (camres != null/* && camres.Expand*/)
            {
                if (TreeNodeDeleted != null)
                    TreeNodeDeleted(null, new EventDeleteNodeArgs { NodeResource = camres });
            }

        }

        void OnCameraGroupAdded(CameraGroupInfo info)
        {
            foreach (VideoPictureResource item in m_RootResources)
            {
                VideoPictureResource camres = GetResourceByCameraGroup(item, info);
                if (camres != null)
                    return;
            }

            //VideoPictureResource rootres = null;
            foreach (VideoPictureResource child in m_RootResources)
            {
                if(child.Type == ResourceType.CameraPicFolder || child.Type == ResourceType.CameraVideoFolder)
                {            

                    VideoPictureResource resource =
                        new VideoPictureResource(ResourceType.CameraGroup, info.GroupName, info);
                    FillupCameraResources(resource, ResourceType.Camera, info);

                    child.AddChild(resource);

                //    if (child.Expand)
                //{
                    if (TreeNodeAdded != null)
                        TreeNodeAdded(null, new EventAddNodeArgs { ParantTreeNode = child.TreeNode, NodeResource = resource });
                //}


                }
            }
        }

        void OnCameraGroupModified(CameraGroupInfo info)
        {
            VideoPictureResource camres = null;
            foreach (VideoPictureResource child in m_RootResources)
            {
                camres = GetResourceByCameraGroup(child,info);
                if (camres != null)
                    break;

            }
            if (camres != null)
            {
                camres.Name = info.GroupName;
                camres.Subject = info;

                if (/*camres.Expand &&*/ TreeNodeEdited != null)
                {
                    TreeNodeEdited(null, new EventEditNodeArgs { NodeResource = camres });
                }
            }
            else
            {
                OnCameraGroupAdded(info);
            }
        }

        void OnCameraGroupDeleted(uint camgroupID)
        {
            VideoPictureResource camres = null;
            foreach (VideoPictureResource child in m_RootResources)
            {
                camres = GetResourceByCameraGroup(child, new CameraGroupInfo { CameraGroupID = camgroupID });
                if (camres != null)
                    break;
            }
            if (camres != null /*&& camres.Expand*/)
            {
                if (TreeNodeDeleted != null)
                    TreeNodeDeleted(null, new EventDeleteNodeArgs { NodeResource = camres });
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

            VideoPictureResource camres = null;
            foreach (VideoPictureResource item in m_RootResources)
            {
                camres = GetResourceByCamera(item, new CameraInfo { CameraID = taskUnitInfo.CameraId });
                if (camres != null)
                    break;
            }
            if (camres != null /*&& camres.Expand*/)
            {
                if (Match(taskUnitInfo, m_filter))
                {
                    VideoPictureResource child;
                    if (taskUnitInfo.CameraId != 0)
                    {
                        child = new VideoPictureResource(ResourceType.VideoFile,
                            string.Format("{0}[{1} - {2}]", taskUnitInfo.TaskUnitName, taskUnitInfo.StartTime.ToString(DataModel.Constant.DATETIME_FORMAT),
                            taskUnitInfo.EndTime.ToString(DataModel.Constant.DATETIME_FORMAT)), taskUnitInfo);
                    }
                    else
                    {
                        child = new VideoPictureResource(ResourceType.VideoFile,
                            string.Format("{0}", taskUnitInfo.TaskUnitName), taskUnitInfo);

                    }
                    camres.AddChild(child);

                    if (TreeNodeAdded != null)
                        TreeNodeAdded(null, new EventAddNodeArgs { ParantTreeNode = camres.TreeNode, NodeResource = child });
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
                if (taskUnitInfo.CameraId != 0)
                {
                    name = string.Format("{0}[{1} - {2}]", taskUnitInfo.TaskUnitName, taskUnitInfo.StartTime.ToString(DataModel.Constant.DATETIME_FORMAT),
                            taskUnitInfo.EndTime.ToString(DataModel.Constant.DATETIME_FORMAT));
                }
                else
                {
                    name = string.Format("{0}", taskUnitInfo.TaskUnitName);
                }

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

        private void UpdateResourceByCameraId(uint camID, VideoPictureResource res)
        {
            if (res.Type == ResourceType.NonCameraVideoFolder && camID == 0)
            {
                res.Children.Clear();
                FillupNonCameraVideos(res);
                return;
            }

            if (res.Type == ResourceType.Camera && res.Subject is CameraInfo)
            {
                CameraInfo camera = res.Subject as CameraInfo;
                if (camID == camera.CameraID)
                {
                    res.Children.Clear();
                    FillupCameraVideos(res);
                    return;
                }
            }

            res.Children.ForEach(item => UpdateResourceByCameraId(camID,item));

        }


        #endregion
    }

    //#region Other classes

    //public enum TreeShowType
    //{
    //    All,
    //    Video,
    //    Picture,
    //    Camera
    //}

    //[FlagsAttribute]
    //public enum TreeShowObjectFilter
    //{
    //    NoUse = 0,
    //    Object = 1,
    //    Face = 2,
    //    Car = 4,
    //    Brief = 8,
    //}

    //public class EventAddNodeArgs : EventArgs
    //{
    //    public object ParantTreeNode { get; set; }
    //    public VideoPictureResource NodeResource { get; set; }
    //}
    //public class EventDeleteNodeArgs : EventArgs
    //{
    //    public VideoPictureResource NodeResource { get; set; }
    //}
    //public class EventEditNodeArgs : EventArgs
    //{
    //    public VideoPictureResource NodeResource { get; set; }
    //}
    //#endregion
        
}
