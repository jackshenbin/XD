using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Protocol.Model;
using BOCOM.IVX.Framework;
using BOCOM.DataModel;

namespace BOCOM.IVX.ViewModel
{
    public class VideoPictureTreeViewModel:IEventAggregatorSubscriber
    {
        public event EventHandler CameraChanged;

        #region Fields
        
        private VideoPictureResource m_ResourceCameraVideoFolder;

        private TreeShowType m_viewType = TreeShowType.All;

        private List<VideoPictureResource> m_RootResources;

        private List<TaskUnitInfo> m_CheckedTaskUnitInfos;

        private bool m_IsForVideoSearch = true;

        #endregion

        #region Properties
        
        public List<CameraInfo> Cameras { get; private set; }
        
        public List<VideoPictureResource> RootResources
        {
            get
            {
                return m_RootResources;
            }

        }

        public VideoPictureTreeViewModel(TreeShowType viewType = TreeShowType.All)
        {
            m_CheckedTaskUnitInfos = new List<TaskUnitInfo>();
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraAddedEvent>().Subscribe(OnCameraAdded);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraModifiedEvent>().Subscribe(OnCameraModified);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraDeletedEvent>().Subscribe(OnCameraDeleted);
            Framework.Container.Instance.RegisterEventSubscriber(this);

            m_viewType = viewType;
            Cameras = Framework.Container.Instance.VDAConfigService.GetAllCameras();
            FillupResources();
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

        #endregion

        #region Private helper functions

        private void FillupResources()
        {
            m_RootResources = new List<VideoPictureResource>();
            if (m_viewType == TreeShowType.Camera)
            {
                FillupVideoFolders();
            }
            else if (m_viewType == TreeShowType.Picture)
            {
                FillupPicFolders();
            }
            else
            {
                FillupVideoFolders();
                FillupPicFolders();
            }
        }

        private void FillupVideoFolders()
        {
            m_ResourceCameraVideoFolder = new VideoPictureResource(ResourceType.CameraVideoFolder, "点位视频");
            m_RootResources.Add(m_ResourceCameraVideoFolder);
            FillupCameraResources(m_ResourceCameraVideoFolder, ResourceType.VideoCamera);

            VideoPictureResource resource = new VideoPictureResource(ResourceType.NonCameraVideoFolder, "无点位视频");
            m_RootResources.Add(resource);
        }

        private void FillupPicFolders()
        {
            VideoPictureResource resource = new VideoPictureResource(ResourceType.CameraPicFolder, "点位图片");
                m_RootResources.Add(resource);
                FillupCameraResources(m_ResourceCameraVideoFolder, ResourceType.PicCamera);

                m_RootResources.Add(new VideoPictureResource(
                    ResourceType.NonCameraPicFolder, "无点位图片"));
        }

        private void FillupCameraResources(VideoPictureResource folder, ResourceType resourceType)
        {
            if (Cameras != null && Cameras.Count > 0)
            {
                foreach (CameraInfo camera in Cameras)
                {
                    VideoPictureResource resource =
                        new VideoPictureResource(resourceType, camera.CameraName, camera);
                    folder.AddChild(resource);
                }
            }
        }

        private void FillupCameraVideos(VideoPictureResource parentResource)
        {
            CameraInfo camera = parentResource.Subject as CameraInfo;
            List<TaskUnitInfo> taskUnits = Framework.Container.Instance.TaskManagerService.GetTaskUnitsByCameraID(camera.CameraID);
            if (taskUnits != null && taskUnits.Count > 0)
            {
                VideoPictureResource child;
                foreach (TaskUnitInfo unit in taskUnits)
                {
                    child = new VideoPictureResource(ResourceType.File,
                        string.Format("{0}[{1} - {2}]",unit.TaskUnitName, unit.StartTime.ToString(Common.Constant.DATETIME_FORMAT),
                        unit.EndTime.ToString(DataModel.Constant.DATETIME_FORMAT)), unit);
                    parentResource.AddChild(child);
                }
            }
        }

        private void FillupNonCameraVideos(VideoPictureResource parentResource)
        {
            CameraInfo camera = parentResource.Subject as CameraInfo;
            List<TaskUnitInfo> taskUnits = Framework.Container.Instance.TaskManagerService.GetTaskUnitsWithoutCamera();
            if (taskUnits != null && taskUnits.Count > 0)
            {
                VideoPictureResource child;
                foreach (TaskUnitInfo unit in taskUnits)
                {
                    child = new VideoPictureResource(ResourceType.File,
                        string.Format("{0}", unit.TaskUnitName), unit);
                    parentResource.AddChild(child);
                }
            }
        }

        #endregion

        #region Public helper functions

        public void RetrieveChildren(VideoPictureResource parentResource)
        {
            if (parentResource.Type == ResourceType.VideoCamera)
            {
                FillupCameraVideos(parentResource);
            }
            else if (parentResource.Type == ResourceType.NonCameraVideoFolder)
            {
                FillupNonCameraVideos(parentResource);
            }
        }
        
        public void CameraSelectionChanged(List<object> list)
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<BOCOM.IVX.Framework.CameraSelectionChangedEvent>().Publish(list);

        }

        public void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraAddedEvent>().Unsubscribe(OnCameraAdded);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraModifiedEvent>().Unsubscribe(OnCameraModified);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraDeletedEvent>().Unsubscribe(OnCameraDeleted);
        }

        public void UpdateCheckedResources(VideoPictureResource resource, bool isChecked)
        {
            UpdateCheckedResourcesEx(resource, isChecked);

            if (IsForVideoSearch)
            {
                Framework.Container.Instance.SelectedTaskUnitsForSearch = m_CheckedTaskUnitInfos.ToArray();
            }
        }

        public void UpdateCheckedResourcesEx(VideoPictureResource resource, bool isChecked)
        {
            if (resource != null)
            {
                if (resource.Type == ResourceType.File)
                {
                    TaskUnitInfo taskUnit = resource.Subject as TaskUnitInfo;
                    if (taskUnit != null)
                    {
                        if (isChecked && !m_CheckedTaskUnitInfos.Contains(taskUnit))
                        {
                            m_CheckedTaskUnitInfos.Add(taskUnit);
                        }
                        else if (!isChecked && m_CheckedTaskUnitInfos.Contains(taskUnit))
                        {
                            m_CheckedTaskUnitInfos.Remove(taskUnit);
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
            Cameras = Framework.Container.Instance.VDAConfigService.GetAllCameras();

            FillupResources();
            if (CameraChanged != null)
                CameraChanged(info, null);
        }

        void OnCameraModified(CameraInfo info)
        {
            Cameras = Framework.Container.Instance.VDAConfigService.GetAllCameras();

            FillupResources();
            if (CameraChanged != null)
                CameraChanged(info, null);
        }

        void OnCameraDeleted(uint camID)
        {
            Cameras = Framework.Container.Instance.VDAConfigService.GetAllCameras();

            FillupResources();
            if (CameraChanged != null)
                CameraChanged(null, null);
        }

        #endregion

    }

        
}
