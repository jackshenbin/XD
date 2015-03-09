using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BOCOM.IVX.Protocol;
using System.Windows.Forms;
using BOCOM.DataModel;
using BOCOM.IVX.Common;

namespace BOCOM.IVX.ViewModel
{
    class ImportPlatformVideoViewModel : ImportVMBase
    {        
        #region Fields

        private DataTable m_Table;

        private List<CameraInfo> m_Cameras;

        private VideoSupplierDeviceInfo m_videoSupplierDevice;

        private List<VideoPictureResource> m_RootResources;

        private Dictionary<string, VAFileInfo> m_VAFileInfos;

        #endregion

        #region Properties

        public DataTable VAFileInfosTable
        {
            get
            {
                return m_Table;
            }
            set { }
        }

        public VAFileInfo SelectedFileInfo
        {
            get;
            set;
        }

        public List<CameraInfo> Cameras
        {
            get
            {
                return m_Cameras;
            }
        }
        
        public List<VideoPictureResource> RootResources
        {
            get
            {
                return m_RootResources;
            }
        }

        public List<VideoSupplierChannelInfo> Channels
        {
            get;
            set;
        }

        #endregion

        #region Constructors
        
        public ImportPlatformVideoViewModel()
        {
            m_VAFileInfos = new Dictionary<string, VAFileInfo>();
            InitDataTable();
            // m_cameraVM = new CameraViewModel();
            FillupResources();
        }

        #endregion

        #region Public helper functions
        
        public void DeleteFile()
        {
            if (SelectedFileInfo != null)
            {
                VAFileInfo fileInfo = SelectedFileInfo;
                DataRow row = m_Table.Rows.Find(fileInfo.FileName);
                if (row != null)
                {
                    m_VAFileInfos.Remove(fileInfo.FileName);
                    row.Delete();
                }
            }
        }

        public void RefreshCameras(VideoSupplierDeviceInfo videoSupplierDevice)
        {
            m_videoSupplierDevice = videoSupplierDevice;

            m_Cameras = null;

            if (m_videoSupplierDevice != null)
            {
                try
                {
                    m_Cameras = Framework.Container.Instance.VDAConfigService.GetCamerasByVideoSupplierDevice(videoSupplierDevice);
                }
                catch (SDKCallException ex)
                {
                    SDKCallExceptionHandler.Handle(ex, "获取视频存储设备管理监控点", true);
                }
            }
        }

        public List<VideoFileInfo> GetVideoFileInfos(CameraInfo camera, DateTime dtStart, DateTime dtEnd)
        {
            List<VideoFileInfo> files = null;

            if (camera != null)
            {
                try
                {
                    files = Framework.Container.Instance.VDAConfigService.GetVideoFiles(m_videoSupplierDevice, camera, dtStart, dtEnd);
                }
                catch (SDKCallException ex)
                {
                    //Framework.Container.Instance.InteractionService.ShowMessageBox("获取历史文件出错", "错误",
                    //    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SDKCallExceptionHandler.Handle(ex, "获取历史文件出错");
                }
            }

            return files;
        }

        public void RetrieveChildren(VideoPictureResource parentResource)
        {
            if (parentResource.Children == null || parentResource.Children.Count == 0)
            {
                if (parentResource.Type == ResourceType.Camera)
                {
                    FillupCameraVideos(parentResource);
                }
            }
        }

        public void AddFiles(List<VAFileInfo> files)
        {
            bool vatypeobject = true;
            bool vatypeface = false;
            bool vatypecar = false;
            bool vatypebrief = false;
            if (Framework.Environment.PRODUCT_TYPE
                == Framework.Environment.E_PRODUCT_TYPE.ONLY_BRIEF)
            {
                vatypeobject = false;
                vatypebrief = true;
            }
            if (m_Table.Rows.Count > 0)
            {
                VAFileInfo info = (VAFileInfo)m_Table.Rows[m_Table.Rows.Count - 1]["LocalVAFileInfo"];
                vatypeobject = (bool)m_Table.Rows[m_Table.Rows.Count - 1]["NVATypeObject"];
                vatypeface = (bool)m_Table.Rows[m_Table.Rows.Count - 1]["NVATypeFace"];
                vatypecar = (bool)m_Table.Rows[m_Table.Rows.Count - 1]["NVATypeCar"];
                vatypebrief = (bool)m_Table.Rows[m_Table.Rows.Count - 1]["NVATypeBrief"];

            }
            if (files != null && files.Count > 0)
            {
                foreach (VAFileInfo vaFileInfo in files)
                {
                    vaFileInfo.VATypeBrief = vatypebrief;
                    vaFileInfo.VATypeCar = vatypecar;
                    vaFileInfo.VATypeFace = vatypeface;
                    vaFileInfo.VATypeObject = vatypeobject;
                    if (!m_VAFileInfos.ContainsKey(vaFileInfo.FileName))
                    {
                        m_VAFileInfos.Add(vaFileInfo.FileName, vaFileInfo);
                        AddDataRow(vaFileInfo);
                    }
                }
            }
        }

        public VAFileInfo GetVAFileInfo(VideoFileInfo vFileInfo, CameraInfo camera)
        {
            string dtStart = vFileInfo.StartTime.ToString(DataModel.Constant.DATETIME_FORMAT);
            string dtEnd = vFileInfo.EndTime.ToString(DataModel.Constant.DATETIME_FORMAT);
            VAFileInfo vaFileInfo = new VAFileInfo()
            {
                CameraName = camera.CameraName,
                CameraId = camera.CameraID,
                FileSize = vFileInfo.Size,
                FileName = string.Format("{0}_{1} - {2}", camera.CameraName, dtStart, dtEnd),
                AdjustTime = vFileInfo.StartTime.ToString(DataModel.Constant.DATETIME_FORMAT),
                FileFullName = string.Format("{0}  -  {1}", vFileInfo.StartTime.ToString(DataModel.Constant.DATETIME_FORMAT),
                vFileInfo.EndTime.ToString(DataModel.Constant.DATETIME_FORMAT)),
                StartTime = vFileInfo.StartTime,
                EndTime = vFileInfo.EndTime
            };
            return vaFileInfo;
        }

        public bool ValidateTimeRange(DateTime dtStart, DateTime dtEnd)
        {
            bool valid = false;
            if (dtEnd <= dtStart)
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox("开始时间必须小于结束时间", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                TimeSpan span = dtEnd.Subtract(dtStart);
                if (span > Common.Constant.TIMERANGE_PLATFORMVIDEOSEARCH)
                {
                    Framework.Container.Instance.InteractionService.ShowMessageBox("查询时间范围不能超过1天", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    valid = true;
                }
            }
            return valid;
        }

        public override bool Validate()
        {
            if (CreateTaskVM == null || !CreateTaskVM.IsVideoPlatform)
            {
                return true;
            }

            bool bRet = false;

            if (m_Table.Rows.Count > Framework.Environment.MAX_TASKUNIT_UPLOAD_COUNT)
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox("一次导入分析的文件不能超过" + Framework.Environment.MAX_TASKUNIT_UPLOAD_COUNT + "个", Framework.Environment.PROGRAM_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (m_Table.Rows.Count <= 0)
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox("请添加需要导入分析的文件", Framework.Environment.PROGRAM_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                bRet = true;
            }

            return bRet;
        }

        public override void Commit()
        {
            if (CreateTaskVM == null || !CreateTaskVM.IsVideoPlatform)
            {
                return;
            }

            uint taskId = 0;
            if (CreateTaskVM.IsNew)
            {
                TaskInfo taskInfo = new TaskInfo() { TaskName = CreateTaskVM.TaskName, TaskPriorityLevel = (uint)CreateTaskVM.Priority };
                try
                {
                    taskId = Framework.Container.Instance.TaskManagerService.AddTask(taskInfo);
                    if (taskId <= 0)
                    {
                        string msg = string.Format("添加任务 '{0}' 失败!", CreateTaskVM.TaskName);
                        Framework.Container.Instance.InteractionService.ShowMessageBox(
                             msg, Framework.Environment.PROGRAM_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "添加任务");
                }
            }
            else
            {
                taskId = CreateTaskVM.TaskId;
            }

            int count = m_Table.Rows.Count;

            if (taskId > 0 && count > 0)
            {
                VAFileInfo vaFileInfo;
                for (int i = 0; i < m_VAFileInfos.Count; i++)
                {
                    DataRow row = m_Table.Rows[i];
                    vaFileInfo = row["LocalVAFileInfo"] as VAFileInfo;

                    object obj = row["AdjustTime"];
                    if (obj is System.DBNull)
                    {
                        vaFileInfo.AdjustTime = string.Empty;
                    }
                    else
                    {
                        vaFileInfo.AdjustTime = obj.ToString();
                    }

                    // vaFileInfoi].VAType = (E_VDA_ANALYZE_TYPE)((int)row["NVAType"]);
                    vaFileInfo.VATypeBrief = (bool)row["NVATypeBrief"];
                    vaFileInfo.VATypeCar = (bool)row["NVATypeCar"];
                    vaFileInfo.VATypeFace = (bool)row["NVATypeFace"];
                    vaFileInfo.VATypeObject = (bool)row["NVATypeObject"];
                    i++;
                }
                try
                {
                    List<uint> ids = Framework.Container.Instance.TaskManagerService.AddVideoSupplierDeviceTaskUnits(taskId, m_VAFileInfos.Values.ToArray());
                    if (ids == null)
                    {
                        string msg = string.Format("添加任务 '{0}' 的任务单元失败!", CreateTaskVM.TaskName);
                        Framework.Container.Instance.InteractionService.ShowMessageBox(
                             msg, Framework.Environment.PROGRAM_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "添加服务器");
                }
            }
        }

        #endregion

        #region Private helper functions
        
        private void InitDataTable()
        {
            m_Table = new DataTable();
            DataColumn col = m_Table.Columns.Add("Id", typeof(string));
            m_Table.PrimaryKey = new DataColumn[] { col };
            m_Table.Columns.Add("CameraName", typeof(string));
            m_Table.Columns.Add("CameraId", typeof(uint));
            m_Table.Columns.Add("AdjustTime", typeof(string));
            m_Table.Columns.Add("FileName", typeof(string));
            m_Table.Columns.Add("FileFullName", typeof(string));
            m_Table.Columns.Add("FileSize", typeof(string));
            m_Table.Columns.Add("NVAType", typeof(int));
            m_Table.Columns.Add("LocalVAFileInfo", typeof(VAFileInfo));
            m_Table.Columns.Add("NVATypeObject", typeof(bool));
            m_Table.Columns.Add("NVATypeFace", typeof(bool));
            m_Table.Columns.Add("NVATypeCar", typeof(bool));
            m_Table.Columns.Add("NVATypeBrief", typeof(bool));
        }

        private void AddDataRow(VAFileInfo vaFileInfo)
        {
            m_Table.Rows.Add(new object[]{
                vaFileInfo.FileName,
                vaFileInfo.CameraName,
                vaFileInfo.CameraId,
                vaFileInfo.AdjustTime,
                vaFileInfo.FileName,
                vaFileInfo.FileFullName,
                Common.Utils.GetByteSizeInUnit(vaFileInfo.FileSize),
                1, 
                vaFileInfo,
                vaFileInfo.VATypeObject,
                vaFileInfo.VATypeFace,
                vaFileInfo.VATypeCar,
                vaFileInfo.VATypeBrief});
        }

        private void FillupResources()
        {
            m_RootResources = new List<VideoPictureResource>();

            VideoPictureResource m_ResourceCameraVideoFolder = new VideoPictureResource(ResourceType.VideoSupplierDevice, "网络存储设备");
            m_RootResources.Add(m_ResourceCameraVideoFolder);
            FillupVideoSupplierDeviceResources(m_ResourceCameraVideoFolder, ResourceType.CameraGroup);


        }

        private void FillupVideoSupplierDeviceResources(VideoPictureResource folder, ResourceType resourceType)
        {
            List<VideoSupplierDeviceInfo> list = Framework.Container.Instance.VDAConfigService.GetAllVideoSupplierDevice();
            if (list != null && list.Count > 0)
            {
                foreach (VideoSupplierDeviceInfo info in list)
                {
                    VideoPictureResource resource =
                        new VideoPictureResource(resourceType, info.DeviceName, info);
                    FillupCameras(resource);

                    folder.AddChild(resource);
                }
            }
        }
            
        private void FillupCameras(VideoPictureResource parentResource)
        {
            VideoSupplierDeviceInfo info = parentResource.Subject as VideoSupplierDeviceInfo;
            List<CameraInfo> list = Framework.Container.Instance.VDAConfigService.GetCamerasByNetDevID(info.Id);
            if (list != null && list.Count > 0)
            {
                VideoPictureResource child;
                foreach (CameraInfo cam in list)
                {
                    child = new VideoPictureResource(ResourceType.Camera,
                        string.Format("{0}", cam.CameraName), cam);
                    parentResource.AddChild(child);
                }
            }
        }

        private void FillupCameraVideos(VideoPictureResource parentResource)
        {
            CameraInfo camera = parentResource.Subject as CameraInfo;
            //DIO.QueryStrmFiles
            List<TaskUnitInfo> taskUnits = null;// Framework.Container.Instance.TaskManagerService.GetTaskUnitsByCameraID(taskunit.CameraID);
            
            if (taskUnits != null && taskUnits.Count > 0)
            {
                VideoPictureResource child;
                foreach (TaskUnitInfo unit in taskUnits)
                {
                        child = new VideoPictureResource(ResourceType.VideoFile,
                            string.Format("{0}[{1} - {2}]", unit.TaskUnitName, unit.StartTime.ToString(DataModel.Constant.DATETIME_FORMAT),
                            unit.EndTime.ToString(DataModel.Constant.DATETIME_FORMAT)), unit);
                        parentResource.AddChild(child);
                }
            }
        }

        #endregion
    }
}
