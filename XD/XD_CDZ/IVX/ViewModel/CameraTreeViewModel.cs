using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Protocol.Model;
using BOCOM.DataModel;

namespace BOCOM.IVX.ViewModel
{
    public class CameraTreeViewModel
    {
        private DataTable m_allCameraList;

        public DataTable AllCameraList
        {
            get
            {
                if (m_allCameraList == null)
                {
                    m_allCameraList = new DataTable("AllCameraList");
                    DataColumn dwCameraID = m_allCameraList.Columns.Add("CameraID", typeof(UInt32));
                    m_allCameraList.PrimaryKey = new DataColumn[] { dwCameraID };
                    m_allCameraList.Columns.Add("GroupID", typeof(UInt32));
                    m_allCameraList.Columns.Add("CameraName");
                    m_allCameraList.Columns.Add("VideoSupplierDeviceID", typeof(UInt32));
                    m_allCameraList.Columns.Add("VideoSupplierDeviceCameraID");
                    m_allCameraList.Columns.Add("PosCoordX", typeof(float));
                    m_allCameraList.Columns.Add("PosCoordY", typeof(float));
                    m_allCameraList.Columns.Add("CameraInfo", typeof(CameraInfo));

                    FillAllCamera();
                }
                return m_allCameraList;
            }
            set { m_allCameraList = value; }
        }

        private void FillAllCamera()
        {
            m_allCameraList.Rows.Clear();

            List<CameraInfo> list = null;
            try
            {
                list = Framework.Container.Instance.VDAConfigService.GetAllCameras();
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "获取点位列表");
            }

            if (list != null)
            {
                list.ForEach(ptCameraInfo => m_allCameraList.Rows.Add(new object[]{ptCameraInfo.CameraID
                                            , ptCameraInfo.GroupID
                                            , ptCameraInfo.CameraName
                                            , ptCameraInfo.VideoSupplierDeviceID
                                            , ptCameraInfo.VideoSupplierChannelID
                                            , ptCameraInfo.PosCoordX
                                            , ptCameraInfo.PosCoordY
                                            , ptCameraInfo
                                            }));
            }
        }

        public DataTable GetTaskUnitByCameraID(uint camID)
        {
            DataTable TaskUnitList = new DataTable("TaskUnitList");
            DataColumn ID = TaskUnitList.Columns.Add("TaskUnitID", typeof(UInt32));
            TaskUnitList.PrimaryKey = new DataColumn[] { ID };
            TaskUnitList.Columns.Add("TaskID", typeof(UInt32));
            TaskUnitList.Columns.Add("TaskUnitName");
            TaskUnitList.Columns.Add("TaskUnitSize");
            TaskUnitList.Columns.Add("TaskUnitType");
            TaskUnitList.Columns.Add("FilePathType");
            TaskUnitList.Columns.Add("FilePath");
            TaskUnitList.Columns.Add("StartTime");
            TaskUnitList.Columns.Add("EndTime");
            TaskUnitList.Columns.Add("ImportStatus");
            TaskUnitList.Columns.Add("VideoAnalyzeTypeNum");
            TaskUnitList.Columns.Add("AnalyzeStatus");
            TaskUnitList.Columns.Add("Progress");
            TaskUnitList.Columns.Add("LeftTimeS");
            TaskUnitList.Columns.Add("TaskUnitInfo", typeof(TaskUnitInfo));

            TaskUnitList.Rows.Clear();
            List<TaskUnitInfo> list = null;
            try
            {
                list = Framework.Container.Instance.TaskManagerService.GetTaskUnitsByCameraID(camID);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "根据点位获取任务单元列表");
            }

            if (list != null)
            {
                list.ForEach(taskUnitInfo => TaskUnitList.Rows.Add(new object[]{
                                                            taskUnitInfo.TaskUnitID,
                                                            taskUnitInfo.TaskID,
                                                            taskUnitInfo.TaskUnitName,
                                                            taskUnitInfo.TaskUnitSize,
                                                            taskUnitInfo.TaskUnitType,
                                                            taskUnitInfo.FilePathType,
                                                            taskUnitInfo.FilePath,
                                                            taskUnitInfo.StartTime.ToString(Constant.DATETIME_FORMAT),
                                                            taskUnitInfo.EndTime.ToString(Constant.DATETIME_FORMAT),
                                                            taskUnitInfo.ImportStatus,
                                                            taskUnitInfo.VideoAnalyzeTypeNum,
                                                            taskUnitInfo.AnalyzeStatus,
                                                            taskUnitInfo.Progress,
                                                            taskUnitInfo.LeftTimeS,
                                                            taskUnitInfo }));
            }
            return TaskUnitList;
        }

        //public void CameraSelectionChanged(List<object> list)
        //{
        //    Framework.Container.Instance.EvtAggregator.GetEvent<BOCOM.IVX.Framework.CameraSelectionChangedEvent>().Publish(list);

        //}
    }
}
