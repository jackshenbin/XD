using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Framework;
using BOCOM.DataModel;
using BOCOM.IVX.Views.Content;
using System.Diagnostics;
using System.Windows.Forms;
using BOCOM.IVX.Protocol.Model;

namespace BOCOM.IVX.ViewModel
{
    class VideoTaskStatusViewModel : ViewModelBase,IEventAggregatorSubscriber
    {
        #region Fields

        private CameraInfo m_currEditCamera;

        private DataTable m_taskUnitList;

        #endregion
        
        #region Properties

        public CameraInfo CurrEditCamera
        {
            get { return m_currEditCamera ?? new CameraInfo(); }
            set 
            {
                if(m_currEditCamera != value)
                {
                    m_currEditCamera = value; 
                    FillTaskUnitByCamID(value.CameraID);
                }
            }
        }


        public DataTable TaskUnitList
        {
            get
            {
                if (m_taskUnitList == null)
                {
                    m_taskUnitList = new DataTable("TaskUnitList");
                    DataColumn TaskUnitID = m_taskUnitList.Columns.Add("TaskUnitID", typeof(UInt32));
                    m_taskUnitList.PrimaryKey = new DataColumn[] { TaskUnitID };
                    m_taskUnitList.Columns.Add("TaskID");
                    m_taskUnitList.Columns.Add("TaskUnitName");
                    m_taskUnitList.Columns.Add("TaskUnitSize");
                    m_taskUnitList.Columns.Add("TaskUnitType");
                    m_taskUnitList.Columns.Add("FilePathType");
                    m_taskUnitList.Columns.Add("FilePath");
                    m_taskUnitList.Columns.Add("StartTime");
                    m_taskUnitList.Columns.Add("EndTime");
                    m_taskUnitList.Columns.Add("ImportStatus");
                    m_taskUnitList.Columns.Add("VideoAnalyzeTypeNum");
                    m_taskUnitList.Columns.Add("Progress", typeof(UInt32));
                    m_taskUnitList.Columns.Add("LeftTimeS", typeof(UInt32));
                    m_taskUnitList.Columns.Add("E_ANALYZE_OBJECT");
                    m_taskUnitList.Columns.Add("E_ANALYZE_VEHICLE");
                    m_taskUnitList.Columns.Add("E_ANALYZE_FACE");
                    m_taskUnitList.Columns.Add("E_ANALYZE_BRIEAF");
                    m_taskUnitList.Columns.Add("E_ANALYZE_VEHICLE_PIC");
                    m_taskUnitList.Columns.Add("E_ANALYZE_FACE_PIC");
                    m_taskUnitList.Columns.Add("TaskUnitInfo", typeof(TaskUnitInfo));

                    FillTaskUnitByCamID(CurrEditCamera.CameraID);
                }
                return m_taskUnitList;
            }
            set { m_taskUnitList = value; }
        }

               
        public DataTable TaskUnitImportStatus
        {
            get
            {
                DataTable t = new DataTable();
                t.Columns.Add("KEY");
                t.Columns.Add("NAME");
                t.Columns.Add("VALUE");
                t.Rows.Add(1, "E_TASKUNIT_IMPORT_WAIT", "等待导入");
                t.Rows.Add(2, "E_TASKUNIT_IMPORT", "导入中");
                t.Rows.Add(3, "E_TASKUNIT_IMPORT_COMPLETE", "导入完成");
                t.Rows.Add(4, "E_TASKUNIT_IMPORT_FAILED", "导入失败");
                return t;
            }
        }

        public string CurrCameraName
        {
            get 
            {
                if (string.IsNullOrEmpty(CurrEditCamera.CameraName))
                    return "";
                else
                    return  "【" + CurrEditCamera.CameraName + "】- ";
            }
        }
        #endregion

        #region Constructors

        public VideoTaskStatusViewModel()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitProgressStatusChangedEvent>().Subscribe(OnTaskUnitStatusChanged, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            //Framework.Container.Instance.EvtAggregator.GetEvent<CameraSelectionChangedEvent>().Subscribe(OnCameraSelectionChanged, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.RegisterEventSubscriber(this);
        }

        #endregion

        #region IEventAggregatorSubscriber implementation

        public void UnSubscribe()
        {
            Trace.WriteLine("TaskStatusViewModel.UnSubscribe");
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitProgressStatusChangedEvent>().Unsubscribe(OnTaskUnitStatusChanged);
            //Framework.Container.Instance.EvtAggregator.GetEvent<CameraSelectionChangedEvent>().Unsubscribe(OnCameraSelectionChanged);
        }

        #endregion

        #region Private helper functions
                
        private uint FillTaskUnitByCamID(uint camId)
        {
            if (m_taskUnitList == null)
                return 0;

            List<TaskUnitInfo> list = null;
            try
            {
                list = Framework.Container.Instance.TaskManagerService.GetTaskUnitsByCameraID(camId);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "抓图");
            }

            m_taskUnitList.Rows.Clear();
            foreach (TaskUnitInfo taskunitInfo in list)
            {
                AddTaskUnitInfoRow(taskunitInfo);
            }

            return (uint)list.Count;
        }

        private void AddTaskUnitInfoRow(TaskUnitInfo taskunitInfo)
        {
            m_taskUnitList.Rows.Add(taskunitInfo.TaskUnitID
                                               , taskunitInfo.TaskID
                                               , taskunitInfo.TaskUnitName
                                               , taskunitInfo.TaskUnitSize
                                               , taskunitInfo.TaskUnitType
                                               , taskunitInfo.FilePathType
                                               , taskunitInfo.FilePath
                                               , taskunitInfo.StartTime
                                               , taskunitInfo.EndTime
                                               , taskunitInfo.ImportStatus
                                               , taskunitInfo.VideoAnalyzeTypeNum
                                               , ((decimal)taskunitInfo.Progress) / 10
                                               , taskunitInfo.LeftTimeS
                                               , GetAnalyseStatus(taskunitInfo, E_VDA_ANALYZE_TYPE.E_ANALYZE_OBJECT)
                                               , GetAnalyseStatus(taskunitInfo, E_VDA_ANALYZE_TYPE.E_ANALYZE_VEHICLE)
                                               , GetAnalyseStatus(taskunitInfo, E_VDA_ANALYZE_TYPE.E_ANALYZE_FACE)
                                               , GetAnalyseStatus(taskunitInfo, E_VDA_ANALYZE_TYPE.E_ANALYZE_BRIEAF)
                                               , GetAnalyseStatus(taskunitInfo, E_VDA_ANALYZE_TYPE.E_ANALYZE_VEHICLE_PIC)
                                               , GetAnalyseStatus(taskunitInfo, E_VDA_ANALYZE_TYPE.E_ANALYZE_FACE_PIC)
                                               , taskunitInfo
                                               );
        }

        private string GetAnalyseStatus(TaskUnitInfo taskunitInfo, E_VDA_ANALYZE_TYPE type)
        {
            if (taskunitInfo.AnalyzeStatus.ContainsKey((E_VDA_ANALYZE_TYPE)type))
            {
                return taskunitInfo.AnalyzeStatus[(E_VDA_ANALYZE_TYPE)type].ToString();
            }
            else
            {
                return E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_WAIT.ToString();
            }
        }
        

        #endregion
        
        #region Public helper functions

        #endregion

        #region Event handlers

        void OnCameraSelectionChanged(List<object> list)
        {
            foreach (object o in list)
            {
                if (o is TaskUnitInfo)
                {
                    try
                    {
                        CameraInfo info = Framework.Container.Instance.VDAConfigService.GetCameraByID(((TaskUnitInfo)o).CameraId);
                        if (info != null)
                        {
                            CurrEditCamera = info;
                        }
                        else
                        {
                            CurrEditCamera = new CameraInfo();
                        }
                    }
                    catch (SDKCallException ex)
                    {
                        Common.SDKCallExceptionHandler.Handle(ex, "根据编号获取点位");
                    }
                    break;
                }
                if (o is CameraInfo)
                {
                    CurrEditCamera = (CameraInfo)o;
                    break;
                }
            }
            RaisePropertyChangedEvent("CurrCameraName");

        }

        void OnTaskUnitStatusChanged(uint taskUnitId)
        {
            if (m_taskUnitList == null)
            {
                return;
            }
            DataRow row = m_taskUnitList.Rows.Find(taskUnitId);
            try
            {
                TaskUnitInfo taskUnitInfo = Framework.Container.Instance.TaskManagerService.GetTaskUnitById(taskUnitId);

                if (row != null && taskUnitInfo != null)
                {
                    row["VideoAnalyzeTypeNum"] = taskUnitInfo.VideoAnalyzeTypeNum;
                    row["ImportStatus"] = taskUnitInfo.ImportStatus;
                    row["Progress"] = ((decimal)taskUnitInfo.Progress) / 10;
                    row["LeftTimeS"] = taskUnitInfo.LeftTimeS;
                    row["E_ANALYZE_OBJECT"] = GetAnalyseStatus(taskUnitInfo, E_VDA_ANALYZE_TYPE.E_ANALYZE_OBJECT);
                    row["E_ANALYZE_VEHICLE"] = GetAnalyseStatus(taskUnitInfo, E_VDA_ANALYZE_TYPE.E_ANALYZE_VEHICLE);
                    row["E_ANALYZE_FACE"] = GetAnalyseStatus(taskUnitInfo, E_VDA_ANALYZE_TYPE.E_ANALYZE_FACE);
                    row["E_ANALYZE_BRIEAF"] = GetAnalyseStatus(taskUnitInfo, E_VDA_ANALYZE_TYPE.E_ANALYZE_BRIEAF);
                    row["E_ANALYZE_VEHICLE_PIC"] = GetAnalyseStatus(taskUnitInfo, E_VDA_ANALYZE_TYPE.E_ANALYZE_VEHICLE_PIC);
                    row["E_ANALYZE_FACE_PIC"] = GetAnalyseStatus(taskUnitInfo, E_VDA_ANALYZE_TYPE.E_ANALYZE_FACE_PIC);
                    row["TaskUnitInfo"] = taskUnitInfo;
                }
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "根据编号获取任务单元");
            }
        }

        #endregion
    }

}
