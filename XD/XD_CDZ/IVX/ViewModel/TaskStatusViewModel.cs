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
    class TaskStatusViewModel : ViewModelBase
    {
        public event EventHandler<TaskChangedEventArgs> TaskAdded;
        public event EventHandler<TaskChangedEventArgs> TaskDeleted;

        public event EventHandler<TaskUnitChangedEventArgs> TaskUnitAdded;
        public event EventHandler TaskUnitDeleted;

        #region Fields

        private DataTable m_taskList;

        private TaskInfo m_currEditTask;

        private DataTable m_taskUnitList;

        private TaskUnitInfo m_currEditTaskUnit;

        #endregion
        
        #region Properties

        public DataTable AllTaskList
        {
            get
            {
                if (m_taskList == null)
                {
                    m_taskList = new DataTable("AllTaskList");
                    DataColumn TaskID = m_taskList.Columns.Add("TaskID", typeof(UInt32));
                    m_taskList.PrimaryKey = new DataColumn[] { TaskID };
                    m_taskList.Columns.Add("TaskName");
                    m_taskList.Columns.Add("TaskPriorityLevel");
                    m_taskList.Columns.Add("TaskDescription");
                    m_taskList.Columns.Add("CreateUserName");
                    m_taskList.Columns.Add("CreateTime");
                    m_taskList.Columns.Add("CompleteTime");
                    m_taskList.Columns.Add("Status");
                    m_taskList.Columns.Add("Progress", typeof(decimal));
                    m_taskList.Columns.Add("TotalLeftTimeS");
                    m_taskList.Columns.Add("TotalCount", typeof(UInt32));
                    m_taskList.Columns.Add("FailedCount", typeof(UInt32));
                    m_taskList.Columns.Add("FinishCount", typeof(UInt32));
                    m_taskList.Columns.Add("ProcessCount", typeof(UInt32));
                    m_taskList.Columns.Add("TaskInfo", typeof(TaskInfo));

                    FillAllTask();
                }
                return m_taskList;
            }
            set { m_taskList = value; }
        }

        public TaskInfo CurrEditTask
        {
            get
            {
                return m_currEditTask ?? new TaskInfo();
            }
            set
            {
                if (m_currEditTask != value)
                {
                    m_currEditTask = value;
                    FillTaskUnitByTaskID(value.TaskID);
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
                    m_taskUnitList.Columns.Add("Progress", typeof(decimal));
                    m_taskUnitList.Columns.Add("LeftTimeS", typeof(UInt32));
                    m_taskUnitList.Columns.Add("E_ANALYZE_OBJECT");
                    m_taskUnitList.Columns.Add("E_ANALYZE_VEHICLE");
                    m_taskUnitList.Columns.Add("E_ANALYZE_FACE");
                    m_taskUnitList.Columns.Add("E_ANALYZE_BRIEAF");
                    m_taskUnitList.Columns.Add("E_ANALYZE_VEHICLE_PIC");
                    m_taskUnitList.Columns.Add("E_ANALYZE_FACE_PIC");
                    m_taskUnitList.Columns.Add("TaskUnitInfo", typeof(TaskUnitInfo));

                    FillTaskUnitByTaskID(CurrEditTask.TaskID);
                }
                return m_taskUnitList;
            }
            set { m_taskUnitList = value; }
        }

        public TaskUnitInfo CurrEditTaskUnit
        {
            get { return m_currEditTaskUnit ?? new TaskUnitInfo(); }
            set { m_currEditTaskUnit = value; }
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
        
        #endregion

        #region Constructors

        public TaskStatusViewModel()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskStatusChangedEvent>().Subscribe(OnTaskStatusChanged, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitProgressStatusChangedEvent>().Subscribe(OnTaskUnitStatusChanged, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskAddedEvent>().Subscribe(OnTaskAdded, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskModifiedEvent>().Subscribe(OnTaskModified, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskDeletedEvent>().Subscribe(OnTaskDeleted, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitAddedEvent>().Subscribe(OnTaskUnitAdded, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitDeletedEvent>().Subscribe(OnTaskUnitDeleted, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            
            Framework.Container.Instance.EvtAggregator.GetEvent<PreLeaveCaseEvent>().Subscribe(OnPreLeaveCase);

            Framework.Container.Instance.RegisterEventSubscriber(this);
        }

        #endregion

        #region IEventAggregatorSubscriber implementation

        public override void UnSubscribe()
        {
            Trace.WriteLine("TaskStatusViewModel.UnSubscribe");
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskStatusChangedEvent>().Unsubscribe(OnTaskStatusChanged);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitProgressStatusChangedEvent>().Unsubscribe(OnTaskUnitStatusChanged);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskModifiedEvent>().Unsubscribe(OnTaskModified);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskDeletedEvent>().Unsubscribe(OnTaskDeleted);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskAddedEvent>().Unsubscribe(OnTaskAdded);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitAddedEvent>().Unsubscribe(OnTaskUnitAdded);
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitDeletedEvent>().Unsubscribe(OnTaskUnitDeleted);

            Framework.Container.Instance.EvtAggregator.GetEvent<PreLeaveCaseEvent>().Unsubscribe(OnPreLeaveCase);
        }

        #endregion

        #region Private helper functions

        private uint FillTaskUnitByTaskID(uint taskId)
        {
            if (m_taskUnitList == null)
                return 0;
            m_taskUnitList.Rows.Clear();
            
            List<TaskUnitInfo> list = null;
            try
            {
                list = Framework.Container.Instance.TaskManagerService.GetTaskUintListByTaskID(taskId);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "跟任务编号获取任务单元集合");
            }

            if (list != null)
            {
                foreach (TaskUnitInfo taskunitInfo in list)
                {
                    AddTaskUnitInfoRow(taskunitInfo);
                }
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

        private uint GetAnalyseStatus(TaskUnitInfo taskunitInfo, E_VDA_ANALYZE_TYPE type)
        {
            if (taskunitInfo.AnalyzeStatus.ContainsKey((E_VDA_ANALYZE_TYPE)type))
            {
                return (uint)taskunitInfo.AnalyzeStatus[(E_VDA_ANALYZE_TYPE)type];
            }
            else
            {
                return (uint)E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_NOUSE;
            }
        }
        
        private void FillAllTask()
        {
            if (m_taskList == null)
                return;

            m_taskList.Rows.Clear();

            List<TaskInfo> list = null;
            try
            {
                list = Framework.Container.Instance.TaskManagerService.GetAllTaskList();
            }
            catch (SDKCallException ex)
            {
                list = null;
                Common.SDKCallExceptionHandler.Handle(ex, "获取全部任务列表");
            }
            if (list != null)
            {
                foreach (TaskInfo taskInfo in list)
                {
                    AddTaskRow(taskInfo);
                }
            }
        }

        private void AddTaskRow(TaskInfo taskInfo)
        {
            Debug.Assert(taskInfo.TaskID != 0);
                        
            uint total = 0;
            uint failed = 0;
            uint process = 0;
            uint finish = 0;

            Framework.Container.Instance.TaskManagerService.GetTaskUnitCountByTaskID(
                taskInfo.TaskID,
                out total,
                out failed,
                out process,
                out finish);

            m_taskList.Rows.Add(taskInfo.TaskID
                                           , taskInfo.TaskName
                                           , taskInfo.TaskPriorityLevel
                                           , taskInfo.TaskDescription
                                           , taskInfo.CreateUserName
                                           , taskInfo.CreateTime
                                           , taskInfo.CompleteTime
                                           , taskInfo.Status
                                           , ((decimal)taskInfo.Progress) / 10
                                           , Common.TextUtil.GetFormatedLastTime(taskInfo.TotalLeftTimeS)
                                           , total
                                           , failed
                                           , finish
                                           , process
                                           , taskInfo
                                           );
        }

        #endregion
        
        #region Public helper functions

        public bool AddTask()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<NavigateEvent>().Publish(UIFuncItemInfo.NEWTASK);
            return true;
        }

        public bool EditTask()
        {
            if (CurrEditTask.TaskID > 0)
            {
                FormTaskEdit FormTaskEdit = new FormTaskEdit(CurrEditTask);
                FormTaskEdit.ShowDialog();
                return true;
            }
            else
                return false;
        }

        public bool DelTask()
        {
            uint id = CurrEditTask.TaskID;
            if (id > 0)
            {
                bool result = false;
                if (Framework.Container.Instance.InteractionService.ShowMessageBox("是否要删除任务【" + CurrEditTask.TaskName + "】？", Framework.Environment.PROGRAM_NAME, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    bool confirmed = true;
                    try
                    {
                        // 检查是否有任务单元相关视频在播放
                        List<TaskUnitInfo> taskUnitInfos = Framework.Container.Instance.TaskManagerService.GetTaskUintListByTaskID(CurrEditTask.TaskID);
                        confirmed = CheckRunningWorks(taskUnitInfos);
                        if (confirmed)
                        {
                            result = Framework.Container.Instance.TaskManagerService.DelTask(id);
                            if (!result)
                            {
                                Framework.Container.Instance.InteractionService.ShowMessageBox("删除任务失败！",
                                    Framework.Environment.PROGRAM_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (SDKCallException ex)
                    {
                        Common.SDKCallExceptionHandler.Handle(ex, "删除任务");
                    }
                }
                return result;
            }
            else
                return false;
        }
                
        public bool AddTaskUnit()
        {
            if (CurrEditTask.TaskID > 0)
            {
                UIFuncItemInfo.NEWTASKUNIT.Subject = CurrEditTask;
                Framework.Container.Instance.EvtAggregator.GetEvent<NavigateEvent>().Publish(UIFuncItemInfo.NEWTASKUNIT);
                return true;
            }
            else
                return false;
        }

        public bool EditTaskUnit()
        {
            if (CurrEditTaskUnit.TaskUnitID > 0)
            {
                FormTaskUnitInfo FormTaskUnitInfo = new Views.Content.FormTaskUnitInfo(CurrEditTaskUnit);
                FormTaskUnitInfo.ShowDialog();
                return true;
            }
            else
                return false;
        }
        
        private List<int> GetPlayHandles(List<TaskUnitInfo> taskunits)
        {
            List<int> handles = new List<int>();

            foreach (TaskUnitInfo taskUnitInfo in taskunits)
            {
                List<int> tmp = Framework.Container.Instance.VideoPlayService.GetPlayHandlesByTaskUnitId(taskUnitInfo.TaskUnitID);
                if (tmp != null && tmp.Count > 0)
                {
                    handles.AddRange(tmp);
                }
            }

            return handles;
        }

        private bool CheckRunningWorks(List<TaskUnitInfo> taskunits)
        {
            bool confirmed = true;

            if (taskunits != null && taskunits.Count > 0)
            {
                foreach (TaskUnitInfo taskUnitInfo in taskunits)
                {
                    confirmed = CheckRunningWorks(taskUnitInfo);
                    if (!confirmed)
                    {
                        break;
                    }
                }
            }
            return confirmed;
        }

        private bool CheckRunningWorks(TaskUnitInfo taskunit)
        {
            bool confirmed = true;
            List<int> videoHandles = Framework.Container.Instance.VideoPlayService.GetPlayHandlesByTaskUnitId(taskunit.TaskUnitID);
            List<int> briefVideoHandles = Framework.Container.Instance.BriefVideoPlayService.GetPlayHandlesByTaskUnitId(taskunit.TaskUnitID);
            bool hasVideoWork = videoHandles != null && videoHandles.Count > 0;
            bool hasBriefVideoWork = briefVideoHandles != null && briefVideoHandles.Count > 0;

            if (hasVideoWork || hasBriefVideoWork)
            {
                confirmed = false;
                string msg = string.Empty;
                //if (hasVideoWork && hasBriefVideoWork)
                //{
                msg = string.Format("任务单元【{0}】正在播放视频或摘要视频， 删除将停止视频或摘要视频， 是否确定删除？",
                    taskunit.TaskUnitName);
                //}
                //else if (hasVideoWork)
                //{
                //    msg = string.Format("任务单元【{0}】正在播放视频， 删除将停止视频， 是否确定删除？",
                //        CurrEditTaskUnit.TaskUnitName);
                //}
                //else if (hasBriefVideoWork)
                //{
                //    msg = string.Format("任务单元【{0}】正在播放摘要视频， 删除将停止摘要视频， 是否确定删除？",
                //        CurrEditTaskUnit.TaskUnitName);
                //}

                if (Framework.Container.Instance.InteractionService.ShowMessageBox(
                    msg, Framework.Environment.PROGRAM_NAME, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    confirmed = true;

                }
            }
            return confirmed;
        }


        public bool DelTaskUnit()
        {
            uint id = CurrEditTaskUnit.TaskUnitID;
            if (id > 0)
            {
                bool result = false;
                bool confirm = false;
                // List<int> videoHandles = null;
                if (Framework.Container.Instance.InteractionService.ShowMessageBox("是否要删除任务单元【" + CurrEditTaskUnit.TaskUnitName + "】？", Framework.Environment.PROGRAM_NAME, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    confirm = CheckRunningWorks(CurrEditTaskUnit);
                }

                if (confirm)
                {
                    //if (videoHandles != null && videoHandles.Count > 0)
                    //{
                    //    foreach (int handle in videoHandles)
                    //    {
                    //        Framework.Container.Instance.VideoPlayService.CloseVideo(handle);
                    //    }
                    //}
                    try
                    {
                        result = Framework.Container.Instance.TaskManagerService.DelTaskUnit(CurrEditTaskUnit.TaskUnitID);
                        if (!result)
                        {
                            Framework.Container.Instance.InteractionService.ShowMessageBox("删除任务失败！",
                                Framework.Environment.PROGRAM_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (SDKCallException ex)
                    {
                        Common.SDKCallExceptionHandler.Handle(ex, "删除任务单元");
                    }
                }
                return result;
            }
            else
                return false;
        }

        public bool DelTaskUnit(List<TaskUnitInfo> taskunits)
        {
            bool result = false;
            if (taskunits.Count > 0)
            {
                bool confirm = false;
                List<int> videoHandles = null;
                string msg = "是否要删除【" + taskunits[0].TaskUnitName + "】等 " + taskunits.Count + " 个任务单元？";
                StringBuilder sb = new StringBuilder();
                if (Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    confirm = CheckRunningWorks(taskunits);
                }

                if (confirm)
                {
                    //if (videoHandles != null && videoHandles.Count > 0)
                    //{
                    //    foreach (int handle in videoHandles)
                    //    {
                    //        Framework.Container.Instance.VideoPlayService.StopVideo(handle);
                    //    }
                    //}
                    foreach (TaskUnitInfo info in taskunits)
                    {
                        string name = info.TaskUnitName;
                        try
                        {
                            Framework.Container.Instance.TaskManagerService.DelTaskUnit(info.TaskUnitID);
                        }
                        catch (SDKCallException)
                        {
                            sb.AppendLine("【" + name + "】");
                        }
                    }
                    if (sb.Length > 0)
                    {
                        result = false;
                        Framework.Container.Instance.InteractionService.ShowMessageBox("删除任务失败！如下文件删除失败：" + System.Environment.NewLine + sb,
                            Framework.Environment.PROGRAM_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        #endregion

        #region Event handlers

        private void OnTaskStatusChanged(uint taskId)
        {
            DataRow row = m_taskList.Rows.Find(taskId);
            try
            {
                TaskInfo taskInfo = Framework.Container.Instance.TaskManagerService.GetTaskByID(taskId);
                if (row != null && taskInfo != null)
                {
                    uint total = 0;
                    uint failed = 0;
                    uint process = 0;
                    uint finish = 0;

                    Framework.Container.Instance.TaskManagerService.GetTaskUnitCountByTaskID(
                        taskInfo.TaskID,
                        out total,
                        out failed,
                        out process,
                        out finish);

                    row["Status"] = taskInfo.Status;
                     row["CompleteTime"] = taskInfo.CompleteTime.ToString(DataModel.Constant.DATETIME_FORMAT);
                     row["Status"] = taskInfo.Status;
                     row["Progress"] =   ((decimal)taskInfo.Progress) / 10;
                     row["TotalLeftTimeS"] = Common.TextUtil. GetFormatedLastTime(taskInfo.TotalLeftTimeS);
                     row["TotalCount"] = total;
                     row["FailedCount"] = failed;
                     row["FinishCount"] = finish;
                     row["ProcessCount"] = process;
                     row["TaskInfo"] = taskInfo;
                }
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "根据编号获取任务");
            }
        }

        private void OnTaskAdded(TaskInfo taskInfo)
        {
            DataRow row = m_taskList.Rows.Find(taskInfo.TaskID);
            if (row == null)
            {
                AddTaskRow(taskInfo);
                CurrEditTask = taskInfo;
                if (TaskAdded != null)
                {
                    TaskAdded(this, new TaskChangedEventArgs(taskInfo));
                }
            }
        }

        private void OnTaskModified(TaskInfo taskInfo)
        {
            if (taskInfo != null)
            {
                DataRow row = m_taskList.Rows.Find(taskInfo.TaskID);
                if (row != null)
                {
                    uint total = 0;
                    uint failed = 0;
                    uint process = 0;
                    uint finish = 0;

                    Framework.Container.Instance.TaskManagerService.GetTaskUnitCountByTaskID(
                        taskInfo.TaskID,
                        out total,
                        out failed,
                        out process,
                        out finish);

                    row["TaskName"] = taskInfo.TaskName;
                    row["TaskPriorityLevel"] = taskInfo.TaskPriorityLevel;
                    row["TotalCount"] = total;
                    row["FailedCount"] = failed;
                    row["FinishCount"] = finish;
                    row["ProcessCount"] = process;
                    row["TaskInfo"] = taskInfo;
                    CurrEditTask = taskInfo;
                }
            }
        }

        private void OnTaskDeleted(uint taskID)
        {
            DataRow row = m_taskList.Rows.Find(taskID);
            if (row != null)
            {
                row.Delete();
                if (TaskDeleted != null)
                {
                    TaskDeleted(this, new TaskChangedEventArgs(CurrEditTask));
                }
            }
        }



        private void OnTaskUnitAdded(TaskUnitInfo taskUnitInfo)
        {
            if (m_taskUnitList == null)
            {
                return;
            }

            if (taskUnitInfo.TaskID == CurrEditTask.TaskID)
            {
                DataRow row = m_taskUnitList.Rows.Find(taskUnitInfo.TaskUnitID);
                if (row == null)
                {
                    // 属于当前选中的任务， 任务单元中还没有包含
                    AddTaskUnitInfoRow(taskUnitInfo);
                    // CurrEditTaskUnit = taskUnitInfo;
                    if (TaskUnitAdded != null)
                    {
                        TaskUnitAdded(this, new TaskUnitChangedEventArgs(taskUnitInfo));
                    }
                }
            }
        }

        private void OnTaskUnitDeleted(uint taskUnitID)
        {

            if (m_taskUnitList == null)
            {
                return;
            }
            DataRow row = m_taskUnitList.Rows.Find(taskUnitID);
            if (row != null)
            {
                row.Delete();
                if (TaskUnitDeleted != null)
                {
                    TaskUnitDeleted(this, EventArgs.Empty);
                }
            }

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
                    if (CurrEditTaskUnit.TaskUnitID == taskUnitInfo.TaskUnitID)
                    {
                        CurrEditTaskUnit = taskUnitInfo;
                    }



                    DataRow rowtask = m_taskList.Rows.Find(taskUnitInfo.TaskID);
                    try
                    {
                        TaskInfo taskInfo = Framework.Container.Instance.TaskManagerService.GetTaskByID(taskUnitInfo.TaskID);
                        if (rowtask != null && taskInfo != null)
                        {
                            uint total = 0;
                            uint failed = 0;
                            uint process = 0;
                            uint finish = 0;

                            Framework.Container.Instance.TaskManagerService.GetTaskUnitCountByTaskID(
                                taskInfo.TaskID,
                                out total,
                                out failed,
                                out process,
                                out finish);
                            rowtask["Status"] = taskInfo.Status;
                            rowtask["CompleteTime"] = taskInfo.CompleteTime.ToString(DataModel.Constant.DATETIME_FORMAT);
                            rowtask["Status"] = taskInfo.Status;
                            rowtask["Progress"] = ((decimal)taskInfo.Progress) / 10;
                            rowtask["TotalLeftTimeS"] = Common.TextUtil.GetFormatedLastTime(taskInfo.TotalLeftTimeS);
                            rowtask["TotalCount"] = total;
                            rowtask["FailedCount"]= failed;
                            rowtask["FinishCount"] = finish;
                            rowtask["ProcessCount"] = process;

                            rowtask["TaskInfo"] = taskInfo;
                        }
                    }
                    catch (SDKCallException ex)
                    {
                        Common.SDKCallExceptionHandler.Handle(ex, "根据编号获取任务",false);
                    }


                }
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "根据编号获取任务单元");
            }
        }

        private void OnPreLeaveCase(string caseName)
        {
            if (m_taskList != null)
            {
                m_taskList.Rows.Clear();
            }
            // m_taskUnitList.Rows.Clear();
        }

        #endregion
    }

    #region Other classes

    public class TaskChangedEventArgs : EventArgs
    {
        public TaskInfo Task{get; private set;}

        public TaskChangedEventArgs(TaskInfo taskInfo)
            : base()
        {
            Task = taskInfo;
        }
    }
    
    public class TaskUnitChangedEventArgs : EventArgs
    {
        public TaskUnitInfo TaskUnit { get; private set; }

        public TaskUnitChangedEventArgs(TaskUnitInfo taskUnit)
            : base()
        {
            TaskUnit = taskUnit;
        }
    }

    #endregion
}
