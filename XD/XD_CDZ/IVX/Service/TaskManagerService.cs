using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Framework;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using BOCOM.DataModel;
using System.IO;
using BOCOM.IVX.Protocol.Model;

namespace BOCOM.IVX.Service
{
    public class TaskManagerService
    {
        //DelegateTaskStatus TaskStatus;
        //DelegateTaskProgress TaskProgress;
        //DelegateTaskUnitStatus TaskUnitStatus;
        //DelegateTaskUnitProgress TaskUnitProgress;

        public TaskManagerService()
        {
            Framework.Container.Instance.IVXProtocol.EvenTaskProgress += new DelegateTaskProgress(onTaskProgress);
            Framework.Container.Instance.IVXProtocol.EvenTaskStatus += new DelegateTaskStatus(onTaskStatus);
            Framework.Container.Instance.IVXProtocol.EvenTaskUnitProgress += new DelegateTaskUnitProgress(onTaskUnitProgress);
            Framework.Container.Instance.IVXProtocol.EvenTaskUnitStatus += new DelegateTaskUnitStatus(onTaskUnitStatus);
            Framework.Container.Instance.IVXProtocol.ResourceChanged += new Action<ResourceUpdateInfo>(IVXProtocol_ResourceChanged);

            //TaskStatus = onTaskStatus;
            //TaskProgress = onTaskProgress;
            //TaskUnitStatus = onTaskUnitStatus;
            //TaskUnitProgress = onTaskUnitProgress;
            //IVXProtocol.TaskStatusCBReg(TaskStatus, 0);
            //IVXProtocol.TaskProgressCBReg(TaskProgress, 0);
            //IVXProtocol.TaskUnitStatusCBReg(onTaskUnitStatus, 0);
            //IVXProtocol.TaskUnitProgressCBReg(onTaskUnitProgress, 0);
        }

        
        #region Private helper functions

        private List<LocalVideoFileImportInfo> GetLocalVideoFileImportInfos(VAFileInfo[] localFiles)
        {
            List<LocalVideoFileImportInfo> localVideoFileImportInfos = new List<LocalVideoFileImportInfo>();
            DateTime dt = DateTime.Now;
            VideoAnalyseInfo analyzeInfo;
            foreach (VAFileInfo fi in localFiles)
            {
                if (DateTime.TryParse(fi.AdjustTime, out dt))
                {
                    uint vatypecount = 0;
                    List<uint> vatypelist = new List<uint>();
                    if (fi.VATypeBrief)
                    {
                        vatypecount++;
                        vatypelist.Add((uint)E_VDA_ANALYZE_TYPE.E_ANALYZE_BRIEAF);
                    }
                    if (fi.VATypeCar)
                    {
                        vatypecount++;
                        vatypelist.Add((uint)E_VDA_ANALYZE_TYPE.E_ANALYZE_VEHICLE);
                    }
                    if (fi.VATypeFace)
                    {
                        vatypecount++;
                        vatypelist.Add((uint)E_VDA_ANALYZE_TYPE.E_ANALYZE_FACE);
                    }
                    if (fi.VATypeObject)
                    {
                        vatypecount++;
                        vatypelist.Add((uint)E_VDA_ANALYZE_TYPE.E_ANALYZE_OBJECT);
                    }

                    analyzeInfo = new VideoAnalyseInfo()
                    {
                        VideoAnalyzeTypeNum = vatypecount,
                        VideoAnalyzeType = vatypelist,
                    };

                    LocalVideoFileImportInfo lvfi = new LocalVideoFileImportInfo()
                    {
                        CameraID = fi.CameraId,
                        LocalFilePath = fi.FileFullName,
                        AdjustStartTime = dt,
                        VideoAnalyzeInfo = analyzeInfo,
                        FileSize = (ulong)(new FileInfo(fi.FileFullName)).Length,
                        TaskUnitName = Path.GetFileName(fi.FileFullName)
                    };
                    localVideoFileImportInfos.Add(lvfi);
                }
            }
            return localVideoFileImportInfos;
        }

        private List<RemoteVideoFileImportInfo> GetRemoteVideoFileImportInfos(VAFileInfo[] localFiles)
        {
            List<RemoteVideoFileImportInfo> remoteVideoFileImportInfos = new List<RemoteVideoFileImportInfo>();
            DateTime dt = DateTime.Now;
            VideoAnalyseInfo analyzeInfo;
            foreach (VAFileInfo fi in localFiles)
            {
                if (DateTime.TryParse(fi.AdjustTime, out dt))
                {
                    uint vatypecount = 0;
                    List<uint> vatypelist = new List<uint>();
                    if (fi.VATypeBrief)
                    {
                        vatypecount++;
                        vatypelist.Add((uint)E_VDA_ANALYZE_TYPE.E_ANALYZE_BRIEAF);
                    }
                    if (fi.VATypeCar)
                    {
                        vatypecount++;
                        vatypelist.Add((uint)E_VDA_ANALYZE_TYPE.E_ANALYZE_VEHICLE);
                    }
                    if (fi.VATypeFace)
                    {
                        vatypecount++;
                        vatypelist.Add((uint)E_VDA_ANALYZE_TYPE.E_ANALYZE_FACE);
                    }
                    if (fi.VATypeObject)
                    {
                        vatypecount++;
                        vatypelist.Add((uint)E_VDA_ANALYZE_TYPE.E_ANALYZE_OBJECT);
                    }

                    analyzeInfo = new VideoAnalyseInfo()
                    {
                        VideoAnalyzeTypeNum = vatypecount,
                        VideoAnalyzeType = vatypelist,
                    };

                    RemoteVideoFileImportInfo lvfi = new RemoteVideoFileImportInfo()
                    {
                        CameraID = fi.CameraId,
                        RemoteFileURL = fi.FileFullName,
                        AdjustStartTime = dt,
                        VideoAnalyzeInfo = analyzeInfo,
                        FileSize = fi.FileSize,
                        TaskUnitName = fi.FileName
                    };
                    remoteVideoFileImportInfos.Add(lvfi);
                }
            }
            return remoteVideoFileImportInfos;
        }
                
        private VideoAnalyseInfo GetAnalysisInfo(VAFileInfo fi)
        {
            uint vatypecount = 0;
            List<uint> vatypelist = new List<uint>();
            if (fi.VATypeBrief)
            {
                vatypecount++;
                vatypelist.Add((uint)E_VDA_ANALYZE_TYPE.E_ANALYZE_BRIEAF);
            }
            if (fi.VATypeCar)
            {
                vatypecount++;
                vatypelist.Add((uint)E_VDA_ANALYZE_TYPE.E_ANALYZE_VEHICLE);
            }
            if (fi.VATypeFace)
            {
                vatypecount++;
                vatypelist.Add((uint)E_VDA_ANALYZE_TYPE.E_ANALYZE_FACE);
            }
            if (fi.VATypeObject)
            {
                vatypecount++;
                vatypelist.Add((uint)E_VDA_ANALYZE_TYPE.E_ANALYZE_OBJECT);
            }

            VideoAnalyseInfo analyzeInfo = new VideoAnalyseInfo()
            {
                VideoAnalyzeTypeNum = vatypecount,
                VideoAnalyzeType = vatypelist,
            };

            return analyzeInfo;
        }


        CameraInfo GetCameraByDev(string ip, uint port, string camid,uint type)
        {
            CameraInfo cam = null;
            List<CameraInfo> list = Framework.Container.Instance.VDAConfigService.GetCameras(0);
            list.ForEach(item =>
                {
                    VideoSupplierDeviceInfo dev = Framework.Container.Instance.VDAConfigService.GetVideoSupplierDeviceByID(item.VideoSupplierDeviceID);
                    if (item.VideoSupplierChannelID == camid && ip == dev.IP && port == dev.Port && type == (uint)dev.ProtocolType)
                    {
                        cam = item;
                    }
                }
                );
            return cam;
        }
        
        private void RaiseTaskUnitAddedEvent(List<uint> taskUnitIds)
        {
            foreach (uint taskUnitId in taskUnitIds)
            {
                RaiseTaskUnitAddedEvent(taskUnitId);
            }
        }

        private void RaiseTaskUnitAddedEvent(uint taskUnitId)
        {
            TaskUnitInfo taskUnit = Framework.Container.Instance.IVXProtocol.GetTaskUnitByID(taskUnitId);
            if (taskUnit != null)
            {
                Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitAddedEvent>().Publish(taskUnit);
            }
        }

        #endregion

        #region 任务接口
        
        public List<TaskInfo> GetAllTaskList()
        {
            List<TaskInfo> list = new List<TaskInfo>();

            //for (int i = 0; i < 10; i++)
            //{
            //    list.Add(new TaskInfo
            //    {
            //        CompleteTime = new DateTime(),
            //        CreateTime = DateTime.Now,
            //        CreateUserName = "user" + i,
            //        Progress = (uint)i*10,
            //        Status = (uint)i%4,
            //        TaskDescription = "sdfsfsfs",
            //        TaskID = (uint)i,
            //        TaskName = "task" + i,
            //        TaskPriorityLevel = 5,
            //        TotalLeftTimeS = (uint)i,

            //    });
            //}
            //return list;



            int lQueryHandle = Framework.Container.Instance.IVXProtocol.QueryTaskList();
            uint count = Framework.Container.Instance.IVXProtocol.GetTaskTotalNum(lQueryHandle);
            while (true)
            {
                TaskInfo taskInfo = Framework.Container.Instance.IVXProtocol.QueryNextTask(lQueryHandle);
                if (taskInfo!=null)
                {
                    list.Add(taskInfo);
                }
                else
                    break;
            }
            Framework.Container.Instance.IVXProtocol.CloseTaskListQuery(lQueryHandle);
            return list;
        }

        public List<TaskUnitInfo> GetTaskUintListByTaskID(uint taskID)
        {
            List<TaskUnitInfo> list = new List<TaskUnitInfo>();

            int lQueryHandle = Framework.Container.Instance.IVXProtocol.QueryTaskUnitList(taskID);
            int count = Framework.Container.Instance.IVXProtocol.GetTaskUnitTotalNum(lQueryHandle);
            while (true)
            {
                TaskUnitInfo taskUnitInfo = Framework.Container.Instance.IVXProtocol.QueryNextTaskUnit(lQueryHandle);
                if (taskUnitInfo!=null)
                {
                    list.Add(taskUnitInfo);
                }
                else
                    break;
            }
            Framework.Container.Instance.IVXProtocol.CloseTaskUnitListQuery(lQueryHandle);
            return list;
        }

        public TaskInfo GetTaskByID(uint taskID)
        {
            return Framework.Container.Instance.IVXProtocol.GetTaskByID(taskID);
        }

        public void GetTaskUnitCountByTaskID(uint taskID,out uint totalCount,out uint failedCount,out uint processCount,out uint finishCount)
        {
            Framework.Container.Instance.IVXProtocol.GetTaskStatusCount(taskID, out totalCount, out failedCount, out processCount, out finishCount);

            //List<TaskUnitInfo> taskunitlist = GetTaskUintListByTaskID(taskID);
            //totalCount = (uint)taskunitlist.Count;
            //uint failed = 0;
            //uint process = 0;
            //uint finish = 0;
            //taskunitlist.ForEach(item =>
            //{
            //    switch ((E_VDA_TASK_UNIT_STATUS)item.ImportStatus)
            //    {
            //        case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_NOUSE:
            //            break;
            //        case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_IMPORT_WAIT:
            //        case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_IMPORT_READY:
            //        case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_IMPORT_EXECUTING:
            //        case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_PREANALYSE_WAIT:
            //        case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_PREANALYSE_EXECUTING:
            //        case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_WAIT:
            //        case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_EXECUTING:
            //            process++;
            //            break;
            //        case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_FAILED:
            //        case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_IMPORT_FAILED:
            //        case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_PREANALYSE_FAILED:
            //            failed++;
            //            break;
            //        case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_FINISH:
            //        case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_IMPORT_FINISH:
            //        case E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_PREANALYSE_FINISH:
            //            finish++;
            //            break;
            //        default:
            //            break;
            //    }
            //});
            //processCount = process;
            //finishCount = finish;
            //failedCount = failed;

        }

        public uint AddTask(TaskInfo taskInfo)
        {
            uint taskID = Framework.Container.Instance.IVXProtocol.AddTask(taskInfo);
            if (taskID > 0)
            {
                TaskInfo taskInfoNew = Framework.Container.Instance.IVXProtocol.GetTaskByID(taskID);
                Framework.Container.Instance.EvtAggregator.GetEvent<TaskAddedEvent>().Publish(taskInfoNew);
            }
            return taskID;
        }

        public bool EditTask(TaskInfo taskInfo)
        {
            bool bRet = Framework.Container.Instance.IVXProtocol.MdfTask(taskInfo);
            if (bRet)
            {
                Framework.Container.Instance.EvtAggregator.GetEvent<TaskModifiedEvent>().Publish(taskInfo);
            }
            return bRet;
        }

        public bool DelTask(uint taskID)
        {
            List<TaskUnitInfo> taskunitlist = GetTaskUintListByTaskID(taskID);
            taskunitlist.ForEach(item => Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitDeletedEvent>().Publish(item.TaskUnitID));
            
            bool bRet = Framework.Container.Instance.IVXProtocol.DelTask(taskID);
            if (bRet)
            {
                Framework.Container.Instance.EvtAggregator.GetEvent<TaskDeletedEvent>().Publish(taskID);
            }
            return bRet;
        }

        public List<UInt32> AddLocalVideoTaskUnits(UInt32 taskId, VAFileInfo[] localFiles)
        {

            List<LocalVideoFileImportInfo> fileInfoList = GetLocalVideoFileImportInfos(localFiles);

            List<uint> taskUnitIds;
            bool result = Framework.Container.Instance.IVXProtocol.AddLocalVideoFileImportToTask(taskId, (uint)fileInfoList.Count,
                fileInfoList, out taskUnitIds);

            Debug.Assert(result, "添加TaskUnit 失败");

            if (result)
            {
                RaiseTaskUnitAddedEvent(taskUnitIds);
            }

            return result ? taskUnitIds : null;
 
        }

        public List<UInt32> AddRemoteVideoTaskUnits(UInt32 taskId, VAFileInfo[] localFiles)
        {

            List<RemoteVideoFileImportInfo> fileInfoList = GetRemoteVideoFileImportInfos(localFiles);

            List<uint> taskUnitIds;
            bool result = Framework.Container.Instance.IVXProtocol.AddRemoteVideoFileImportToTask(taskId, (uint)fileInfoList.Count,
                fileInfoList, out taskUnitIds);

            Debug.Assert(result, "添加TaskUnit 失败");

            if (result)
            {
                RaiseTaskUnitAddedEvent(taskUnitIds);
            }

            return result ? taskUnitIds : null;
 
        }

        public List<UInt32> AddVideoSupplierDeviceTaskUnits(UInt32 taskId, VAFileInfo[] vaFileInfos)
        {

            List<uint> taskUnitIds = null;
            if (taskId > 0 && vaFileInfos != null && vaFileInfos.Length > 0)
            {
                foreach (VAFileInfo vaFile in vaFileInfos)
                {
                    vaFile.VideoAnalyzeInfo = GetAnalysisInfo(vaFile);
                }

                bool result = Framework.Container.Instance.IVXProtocol.AddNetStoreVideoImportToTask(taskId, vaFileInfos, out taskUnitIds);

                Debug.Assert(result, "添加TaskUnit 失败");

                if (result)
                {
                    RaiseTaskUnitAddedEvent(taskUnitIds);
                }
            }
            return taskUnitIds;
 
        }

        public TaskUnitInfo GetTaskUnitById(uint id)
        {
            return Framework.Container.Instance.IVXProtocol.GetTaskUnitByID(id);
        }

        public bool DelTaskUnit(uint taskUnitID)
        {
            bool bRet = Framework.Container.Instance.IVXProtocol.DelTaskUnit(taskUnitID);
            if (bRet)
            {
                Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitDeletedEvent>().Publish(taskUnitID);
            }

            return bRet;
        }

        public bool EditTaskUnitAnalyseType(uint taskUnitID, List<E_VDA_ANALYZE_TYPE> types)
        {
            return Framework.Container.Instance.IVXProtocol.AddTaskUnitAnalyzeType(taskUnitID, types);
        }

        public bool EditTaskUnitAdjustTime(uint taskUnitID, DateTime adjustTime)
        {
            bool bRet = Framework.Container.Instance.IVXProtocol.SetTaskUnitAdjustTime(taskUnitID, adjustTime);
            if (bRet)
            {   //修改时间后需要通知界面更新数据，暂时用状态更新来通知
                Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitProgressStatusChangedEvent>().Publish(taskUnitID);
            }
            return bRet;
        }

        public List<TaskUnitInfo> GetTaskUnitsByCameraID(uint cameraId)
        {
            List<TaskUnitInfo> taskUnits = new List<TaskUnitInfo>();
            //taskUnits.Add(new TaskUnitInfo { TaskUnitID = taskId * 10 + 1, TaskUnitName = "asdf" });
            //return taskUnits;

            int queryHandle = Framework.Container.Instance.IVXProtocol.QueryVideoTaskUnitListByCamera(cameraId);

            int count = Framework.Container.Instance.IVXProtocol.GetTaskUnitTotalNum(queryHandle);
            while (true)
            {
                TaskUnitInfo taskUnitInfo = Framework.Container.Instance.IVXProtocol.QueryNextTaskUnit(queryHandle);
                if (taskUnitInfo != null)
                {
                    taskUnits.Add(taskUnitInfo);
                }
                else
                    break;
            }

            Framework.Container.Instance.IVXProtocol.CloseTaskUnitListQuery(queryHandle);

            return taskUnits;
        }

        public List<TaskUnitInfo> GetTaskUnitsWithoutCamera()
        {
            List<TaskUnitInfo> taskUnits = new List<TaskUnitInfo>();
            //taskUnits.Add(new TaskUnitInfo { TaskUnitID = taskId * 10 + 1, TaskUnitName = "asdf" });
            //return taskUnits;

            int queryHandle = Framework.Container.Instance.IVXProtocol.QueryVideoTaskUnitListWithoutCamera();

            int count = Framework.Container.Instance.IVXProtocol.GetTaskUnitTotalNum(queryHandle);
            while (true)
            {
                TaskUnitInfo taskUnitInfo = Framework.Container.Instance.IVXProtocol.QueryNextTaskUnit(queryHandle);
                if (taskUnitInfo != null)
                {
                    taskUnits.Add(taskUnitInfo);
                }
                else
                    break;
            }

            Framework.Container.Instance.IVXProtocol.CloseTaskUnitListQuery(queryHandle);

            return taskUnits;
        }

        public List<uint> GetLocalUploadTaskUnit()
        {
            List<uint> taskunitidlist = new List<uint> ();
            Framework.Container.Instance.IVXProtocol.GetImportingTaskUnitList(out taskunitidlist);

            //List<TaskUnitInfo> taskunits = new List<TaskUnitInfo>();
            //List<TaskInfo> tasks = GetAllTaskList();
            //foreach (TaskInfo item in tasks)
            //{
            //    List<TaskUnitInfo> temp = GetTaskUintListByTaskID(item.TaskID);
            //    foreach (TaskUnitInfo taskunititem in temp)
            //    {
            //        if(taskunititem.ImportStatus == (uint)E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_IMPORT_EXECUTING
            //            && taskunititem.TaskUnitType == (uint)E_VDA_TASK_UNIT_TYPE.E_TASKUNIT_TYPE_CLIENT_VIDEO_FILE)
            //        {
            //            taskunits.Add(taskunititem);
            //        }
            //    }
            //}
            return taskunitidlist;
        }

        #endregion

        #region Event handlers

        void onTaskStatus(UInt32 taskID, UInt32 taskStatus, UInt32 userData)
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskStatusChangedEvent>().Publish(taskID);
        }

        void onTaskProgress(UInt32 taskID, UInt32 taskProgress, UInt32 taskLeftTimeS, UInt32 userData)
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskStatusChangedEvent>().Publish(taskID);
        }

        void onTaskUnitStatus(UInt32 taskUnitID, UInt32 taskUnitImportStatus,
                                                  Dictionary<E_VDA_ANALYZE_TYPE, E_VDA_TASK_UNIT_STATUS> analyzeStatus, UInt32 analyzeStatusNumber, UInt32 userData)
        {
            if (taskUnitImportStatus == (uint)E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_FINISH)
                Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitAnalyseFinishedEvent>().Publish(taskUnitID);
            if (taskUnitImportStatus == (uint)E_VDA_TASK_UNIT_STATUS.E_TASK_UNIT_ANALYSE_WAIT)
                Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitImportFinishedEvent>().Publish(taskUnitID);

            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitProgressStatusChangedEvent>().Publish(taskUnitID);

        }

        void onTaskUnitProgress(UInt32 taskUnitID, UInt32 taskUnitProgress,
                                                   UInt32 taskUnitLeftTimeS, UInt32 userData)
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitProgressStatusChangedEvent>().Publish(taskUnitID);
        }

        void IVXProtocol_ResourceChanged(ResourceUpdateInfo obj)
        {
            switch (obj.ResourceType)
            {
                case E_VDA_RESOURCE_TYPE.E_RESOURCE_TYPE_NOUSE:
                    break;
                case E_VDA_RESOURCE_TYPE.E_RESOURCE_TYPE_SERVER:
                    break;
                case E_VDA_RESOURCE_TYPE.E_RESOURCE_TYPE_NET_STORE:
                    break;
                case E_VDA_RESOURCE_TYPE.E_RESOURCE_TYPE_CAMERA_GROUP:
                    break;
                case E_VDA_RESOURCE_TYPE.E_RESOURCE_TYPE_CAMERA:
                    break;
                case E_VDA_RESOURCE_TYPE.E_RESOURCE_TYPE_USER_GROUP:
                    break;
                case E_VDA_RESOURCE_TYPE.E_RESOURCE_TYPE_USER:
                    break;
                case E_VDA_RESOURCE_TYPE.E_RESOURCE_TYPE_CASE:
                    break;
                case E_VDA_RESOURCE_TYPE.E_RESOURCE_TYPE_TASK:
                    if (obj.OperateType == E_VDA_RESOURCE_OPERATE_TYPE.E_RESOURCE_OPERATE_TYPE_ADD)
                    {
                        obj.ResourceIDList.ForEach(item => 
                            {
                                TaskInfo info = GetTaskByID(item);
                                if(info!=null)
                                    Framework.Container.Instance.EvtAggregator.GetEvent<TaskAddedEvent>().Publish(info);
                            }
                            );
                    }
                    else if (obj.OperateType == E_VDA_RESOURCE_OPERATE_TYPE.E_RESOURCE_OPERATE_TYPE_DEL)
                    { 
                        obj.ResourceIDList.ForEach(item => 
                            {
                                Framework.Container.Instance.EvtAggregator.GetEvent<TaskDeletedEvent>().Publish(item);
                            }
                            );
                    }
                    else if (obj.OperateType == E_VDA_RESOURCE_OPERATE_TYPE.E_RESOURCE_OPERATE_TYPE_MDF)
                    { 
                        obj.ResourceIDList.ForEach(item => 
                            {
                                TaskInfo info = GetTaskByID(item);
                                if(info!=null)
                                    Framework.Container.Instance.EvtAggregator.GetEvent<TaskModifiedEvent>().Publish(info);
                            }
                            );
                    }
                    break;
                case E_VDA_RESOURCE_TYPE.E_RESOURCE_TYPE_TASKUNIT:
                    if (obj.OperateType == E_VDA_RESOURCE_OPERATE_TYPE.E_RESOURCE_OPERATE_TYPE_ADD)
                    {
                        obj.ResourceIDList.ForEach(item =>
                        {
                            TaskUnitInfo info = GetTaskUnitById(item);
                            if (info != null)
                                Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitAddedEvent>().Publish(info);
                        }
                            );
                    }
                    else if (obj.OperateType == E_VDA_RESOURCE_OPERATE_TYPE.E_RESOURCE_OPERATE_TYPE_DEL)
                    {
                        obj.ResourceIDList.ForEach(item =>
                        {
                            Framework.Container.Instance.EvtAggregator.GetEvent<TaskUnitDeletedEvent>().Publish(item);
                        }
                            );
                    }

                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
