using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using System.Data;
using BOCOM.DataModel;
using BOCOM.IVX.Framework;
using System.Diagnostics;
using System.Drawing;

namespace BOCOM.IVX.ViewModel
{
    class TaskUnitInfoViewModel : ViewModelBase,IEventAggregatorSubscriber
    {
        TaskUnitInfo m_oldTaskUnitInfo;
        TaskUnitInfo m_newTaskUnitInfo;
        List<E_VDA_ANALYZE_TYPE> analyzeTypeList = new List<E_VDA_ANALYZE_TYPE>();
        DevExpress.Utils.ImageCollection imageGroup;

        public bool ObjectAnalyseChecked 
        {
            get
            {
                return m_newTaskUnitInfo.AnalyzeStatus.ContainsKey(E_VDA_ANALYZE_TYPE.E_ANALYZE_OBJECT)
                    || analyzeTypeList.Contains(E_VDA_ANALYZE_TYPE.E_ANALYZE_OBJECT);
            }
            set 
            {
                if (value)
                {
                    if (!analyzeTypeList.Contains(E_VDA_ANALYZE_TYPE.E_ANALYZE_OBJECT))
                        analyzeTypeList.Add(E_VDA_ANALYZE_TYPE.E_ANALYZE_OBJECT);
                }
                else
                {
                    if (analyzeTypeList.Contains(E_VDA_ANALYZE_TYPE.E_ANALYZE_OBJECT))
                        analyzeTypeList.Remove(E_VDA_ANALYZE_TYPE.E_ANALYZE_OBJECT);
                }
            }
        }
        public bool ObjectAnalyseEnable 
        {
            get
            {
                return !m_newTaskUnitInfo.AnalyzeStatus.ContainsKey(E_VDA_ANALYZE_TYPE.E_ANALYZE_OBJECT);
            }
        }
        public Image ObjectAnalyseImage
        {
            get
            {
                return m_newTaskUnitInfo.AnalyzeStatus.ContainsKey(E_VDA_ANALYZE_TYPE.E_ANALYZE_OBJECT)?imageGroup.Images[17]:imageGroup.Images[18];
            }
        }
        public Image ObjectAnalyseStatusImage
        {
            get
            {
                if (m_newTaskUnitInfo.AnalyzeStatus.ContainsKey(E_VDA_ANALYZE_TYPE.E_ANALYZE_OBJECT))
                    return GetStatusImage(m_newTaskUnitInfo.AnalyzeStatus[E_VDA_ANALYZE_TYPE.E_ANALYZE_OBJECT]);
                else
                    return GetStatusImage(E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_NOUSE);
            }
        }

        public bool VicheilAnalyseChecked 
        {
            get
            {
                return m_newTaskUnitInfo.AnalyzeStatus.ContainsKey(E_VDA_ANALYZE_TYPE.E_ANALYZE_VEHICLE)
                     || analyzeTypeList.Contains(E_VDA_ANALYZE_TYPE.E_ANALYZE_VEHICLE);
            }
            set
            {
                if (value)
                {
                    if (!analyzeTypeList.Contains(E_VDA_ANALYZE_TYPE.E_ANALYZE_VEHICLE))
                        analyzeTypeList.Add(E_VDA_ANALYZE_TYPE.E_ANALYZE_VEHICLE);
                }
                else
                {
                    if (analyzeTypeList.Contains(E_VDA_ANALYZE_TYPE.E_ANALYZE_VEHICLE))
                        analyzeTypeList.Remove(E_VDA_ANALYZE_TYPE.E_ANALYZE_VEHICLE);
                }
            }
        }
        public bool VicheilAnalyseEnable 
        {
            get
            {
                return !m_newTaskUnitInfo.AnalyzeStatus.ContainsKey(E_VDA_ANALYZE_TYPE.E_ANALYZE_VEHICLE);
            }
        }
        public Image VicheilAnalyseImage
        {
            get
            {
                return m_newTaskUnitInfo.AnalyzeStatus.ContainsKey(E_VDA_ANALYZE_TYPE.E_ANALYZE_VEHICLE) ? imageGroup.Images[19] : imageGroup.Images[20];
            }
        }
        public Image VicheilAnalyseStatusImage
        {
            get
            {
                if (m_newTaskUnitInfo.AnalyzeStatus.ContainsKey(E_VDA_ANALYZE_TYPE.E_ANALYZE_VEHICLE))
                    return GetStatusImage(m_newTaskUnitInfo.AnalyzeStatus[E_VDA_ANALYZE_TYPE.E_ANALYZE_VEHICLE]);
                else
                    return GetStatusImage(E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_NOUSE);
            }
        }

        public bool FaceAnalyseChecked 
        {
            get
            {
                return m_newTaskUnitInfo.AnalyzeStatus.ContainsKey(E_VDA_ANALYZE_TYPE.E_ANALYZE_FACE)
                     || analyzeTypeList.Contains(E_VDA_ANALYZE_TYPE.E_ANALYZE_FACE);
            }
            set
            {
                if (value)
                {
                    if (!analyzeTypeList.Contains(E_VDA_ANALYZE_TYPE.E_ANALYZE_FACE))
                        analyzeTypeList.Add(E_VDA_ANALYZE_TYPE.E_ANALYZE_FACE);
                }
                else
                {
                    if (analyzeTypeList.Contains(E_VDA_ANALYZE_TYPE.E_ANALYZE_FACE))
                        analyzeTypeList.Remove(E_VDA_ANALYZE_TYPE.E_ANALYZE_FACE);
                }
            }
        }
        public bool FaceAnalyseEnable 
        {
            get
            {
                return !m_newTaskUnitInfo.AnalyzeStatus.ContainsKey(E_VDA_ANALYZE_TYPE.E_ANALYZE_FACE);
            }
        }
        public Image FaceAnalyseImage
        {
            get
            {
                return m_newTaskUnitInfo.AnalyzeStatus.ContainsKey(E_VDA_ANALYZE_TYPE.E_ANALYZE_FACE) ? imageGroup.Images[21] : imageGroup.Images[22];
            }
        }
        public Image FaceAnalyseStatusImage
        {
            get
            {
                if (m_newTaskUnitInfo.AnalyzeStatus.ContainsKey(E_VDA_ANALYZE_TYPE.E_ANALYZE_FACE))
                    return GetStatusImage(m_newTaskUnitInfo.AnalyzeStatus[E_VDA_ANALYZE_TYPE.E_ANALYZE_FACE]);
                else
                    return GetStatusImage(E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_NOUSE);
            }
        }

        public bool BriefAnalyseChecked 
        {
            get
            {
                return m_newTaskUnitInfo.AnalyzeStatus.ContainsKey(E_VDA_ANALYZE_TYPE.E_ANALYZE_BRIEAF)
                     || analyzeTypeList.Contains(E_VDA_ANALYZE_TYPE.E_ANALYZE_BRIEAF);
            }
            set
            {
                if (value)
                {
                    if (!analyzeTypeList.Contains(E_VDA_ANALYZE_TYPE.E_ANALYZE_BRIEAF))
                        analyzeTypeList.Add(E_VDA_ANALYZE_TYPE.E_ANALYZE_BRIEAF);
                }
                else
                {
                    if (analyzeTypeList.Contains(E_VDA_ANALYZE_TYPE.E_ANALYZE_BRIEAF))
                        analyzeTypeList.Remove(E_VDA_ANALYZE_TYPE.E_ANALYZE_BRIEAF);
                }
            }
        }
        public bool BriefAnalyseEnable 
        {
            get
            {
                return !m_newTaskUnitInfo.AnalyzeStatus.ContainsKey(E_VDA_ANALYZE_TYPE.E_ANALYZE_BRIEAF);
            }
        }
        public Image BriefAnalyseImage
        {
            get
            {
                return m_newTaskUnitInfo.AnalyzeStatus.ContainsKey(E_VDA_ANALYZE_TYPE.E_ANALYZE_BRIEAF) ? imageGroup.Images[23] : imageGroup.Images[24];
            }
        }
        public Image BriefAnalyseStatusImage
        {
            get
            {
                if (m_newTaskUnitInfo.AnalyzeStatus.ContainsKey(E_VDA_ANALYZE_TYPE.E_ANALYZE_BRIEAF))
                    return GetStatusImage(m_newTaskUnitInfo.AnalyzeStatus[E_VDA_ANALYZE_TYPE.E_ANALYZE_BRIEAF]);
                else
                    return GetStatusImage(E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_NOUSE);
            }
        }


        public string Title
        {
            get 
            {
                if (m_newTaskUnitInfo.ImportStatus != (uint)E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_IMPORT_FINISH
                    && m_newTaskUnitInfo.ImportStatus != (uint)E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_FINISH
                    && m_newTaskUnitInfo.ImportStatus != (uint)E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_FAILED
                    && m_newTaskUnitInfo.ImportStatus != (uint)E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_PREANALYSE_FAILED
                    && m_newTaskUnitInfo.ImportStatus != (uint)E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_PREANALYSE_FINISH
                    && m_newTaskUnitInfo.ImportStatus != (uint)E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_IMPORT_FAILED)
                {
                    return m_newTaskUnitInfo.TaskUnitName + " [进度 " + m_newTaskUnitInfo.Progress / 10 + "%，剩余 " + m_newTaskUnitInfo.LeftTimeS + " 秒]";
                }
                else 
                {
                    return m_newTaskUnitInfo.TaskUnitName;
                }
            }
        }

        public string TaskUnitCameraName
        {
            get 
            {
                CameraInfo info = Framework.Container.Instance.VDAConfigService.GetCameraByID(m_newTaskUnitInfo.CameraId);
                if (info != null)
                    return info.CameraName;
                else
                    return "未关联监控点位";
            }
        }
        public uint ImportStatus
        {
            get
            {
                return m_newTaskUnitInfo.ImportStatus;
            }
        }
        public TaskUnitInfo OldTaskUnitInfo
        {
            get { return m_oldTaskUnitInfo??new TaskUnitInfo(); }
            set { m_oldTaskUnitInfo = value; }
        }

        public TaskUnitInfo NewTaskUnitInfo
        {
            get { return m_newTaskUnitInfo??new TaskUnitInfo(); }
            set { m_newTaskUnitInfo = value; }
        }


        public TaskUnitInfoViewModel()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitProgressStatusChangedEvent>().Subscribe(OnTaskUnitStatusChanged, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.RegisterEventSubscriber(this);

        }

        #region IEventAggregatorSubscriber implementation

        public void UnSubscribe()
        {
            Trace.WriteLine("TaskStatusViewModel.UnSubscribe");
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitProgressStatusChangedEvent>().Unsubscribe(OnTaskUnitStatusChanged);
        }

        #endregion


        public bool EditTaskUnitInfo()
        {
           bool ret2 = EditAdjustTime();
           bool ret1 = EditAnalyseType();
           return ret1 && ret2;
        }

        private  bool EditAnalyseType()
        {
            bool ret = false;
            try
            {
                ret = Framework.Container.Instance.TaskManagerService.EditTaskUnitAnalyseType(NewTaskUnitInfo.TaskUnitID, analyzeTypeList);
                analyzeTypeList.Clear();
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "修改任务单元分析类型");
            }
            return ret;

        }
        private  bool EditAdjustTime()
        {
            bool ret = false;
            try
            {
                ret = Framework.Container.Instance.TaskManagerService.EditTaskUnitAdjustTime(NewTaskUnitInfo.TaskUnitID, NewTaskUnitInfo.StartTime);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "修改任务单元校准时间");
            }
            return ret;

        }
        public void SetImageGroup(DevExpress.Utils.ImageCollection imageCollection1)
        {
            imageGroup = imageCollection1;
        }

        //public DataTable TaskUnitImportStatus
        //{
        //    get
        //    {
        //        DataTable t = new DataTable();
        //        t.Columns.Add("KEY");
        //        t.Columns.Add("NAME");
        //        t.Columns.Add("VALUE");
        //        t.Rows.Add(0, "E_TASKUNIT_IMPORT_NOUSE", "等待导入");
        //        t.Rows.Add(1, "E_TASKUNIT_IMPORT_WAIT", "等待导入");
        //        t.Rows.Add(2, "E_TASKUNIT_IMPORT_READY", "导入准备");
        //        t.Rows.Add(3, "E_TASKUNIT_IMPORT_EXECUTING", "导入中");
        //        t.Rows.Add(4, "E_TASKUNIT_IMPORT_FINISH", "导入完成");
        //        t.Rows.Add(5, "E_TASKUNIT_IMPORT_FAILED", "导入失败");
        //        t.Rows.Add(6, "E_TASKUNIT_ANALYZE_WAIT", "等待导入中");
        //        t.Rows.Add(7, "E_TASKUNIT_ANALYZE", "分析中");
        //        t.Rows.Add(8, "E_TASKUNIT_ANALYZE_COMPLETE", "完成");
        //        t.Rows.Add(9, "E_TASKUNIT_ANALYZE_FAILED", "失败");

        //        return t;
        //    }
        //}
        private Image GetStatusImage(E_VDA_TASK_UNIT_STATUS s)
        {
            Image i = imageGroup.Images[25];
            switch (s)
            {
                //case E_VDA_TASK_UNIT_STATUS.E_TASKUNIT_NO_ANALYZE:
                //    i = imageCollection1.Images[25];
                //    break;
                case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_WAIT:
                    i = imageGroup.Images[30];
                    break;
                case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_EXECUTING:
                    i = imageGroup.Images[27];
                    break;
                case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_FINISH:
                    i = imageGroup.Images[29];
                    break;
                case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_FAILED:
                    i = imageGroup.Images[28];
                    break;
                default:
                    i = imageGroup.Images[25];
                    break;
            }
            return i;
        }

        void OnTaskUnitStatusChanged(uint taskUnitId)
        {
            if (m_newTaskUnitInfo.TaskUnitID != taskUnitId)
            {
                return;
            }
            try
            {
                TaskUnitInfo taskUnitInfo = Framework.Container.Instance.TaskManagerService.GetTaskUnitById(taskUnitId);

                if (taskUnitInfo != null)
                {
                    m_newTaskUnitInfo.VideoAnalyzeTypeNum = taskUnitInfo.VideoAnalyzeTypeNum;
                    m_newTaskUnitInfo.ImportStatus = taskUnitInfo.ImportStatus;
                    m_newTaskUnitInfo.Progress = taskUnitInfo.Progress;
                    m_newTaskUnitInfo.LeftTimeS = taskUnitInfo.LeftTimeS;
                    m_newTaskUnitInfo.AnalyzeStatus = taskUnitInfo.AnalyzeStatus;

                    RaisePropertyChangedEvent("ObjectAnalyseChecked");
                    RaisePropertyChangedEvent("ObjectAnalyseEnable");
                    RaisePropertyChangedEvent("VicheilAnalyseChecked");
                    RaisePropertyChangedEvent("VicheilAnalyseEnable");
                    RaisePropertyChangedEvent("FaceAnalyseChecked");
                    RaisePropertyChangedEvent("FaceAnalyseEnable");
                    RaisePropertyChangedEvent("BriefAnalyseChecked");
                    RaisePropertyChangedEvent("BriefAnalyseEnable");
                    RaisePropertyChangedEvent("ImportStatus");
                    RaisePropertyChangedEvent("Title");

                    RaisePropertyChangedEvent("ObjectAnalyseImage");
                    RaisePropertyChangedEvent("ObjectAnalyseStatusImage");
                    RaisePropertyChangedEvent("VicheilAnalyseImage");
                    RaisePropertyChangedEvent("VicheilAnalyseStatusImage");
                    RaisePropertyChangedEvent("FaceAnalyseImage");
                    RaisePropertyChangedEvent("FaceAnalyseStatusImage");
                    RaisePropertyChangedEvent("BriefAnalyseImage");
                    RaisePropertyChangedEvent("BriefAnalyseStatusImage");
                }
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "根据编号获取任务单元");
            }
        }



    }
}
