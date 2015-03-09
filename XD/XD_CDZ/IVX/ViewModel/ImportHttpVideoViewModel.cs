using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using System.ComponentModel;
using BOCOM.IVX.Framework;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using BOCOM.DataModel;
using BOCOM.IVX.Protocol.Model;
using System.Collections;
using System.Data;

namespace BOCOM.IVX.ViewModel
{
    public class ImportHttpVideoViewModel : ImportVMBase
    {
        #region Fields

        private DataTable m_Table;

        private List<CameraInfo> m_Cameras;

        private CameraViewModel m_cameraVM;

        private bool m_HasFile;

        #endregion

        #region Properties

        public DataTable VAFileInfosTable
        {
            get
            {
                return m_Table;
            }
            set{}
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
                return m_cameraVM.Cameras;
            }
        }

        public bool HasFile
        {
            get { return m_HasFile; }
            set
            {
                if (m_HasFile != value)
                {
                    m_HasFile = value;
                    RaisePropertyChangedEvent("HasFile");
                }
            }
        }

        #endregion

        #region Constructors

        public ImportHttpVideoViewModel()
        {
            InitDataTable();
            m_cameraVM = new CameraViewModel();

            try
            {
                m_Cameras = Framework.Container.Instance.VDAConfigService.GetAllCameras();
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "获取点位列表");
            }
        }

        #endregion

        private void InitDataTable()
        {
            m_Table = new DataTable();
            DataColumn col = m_Table.Columns.Add("Id", typeof(int));
            m_Table.PrimaryKey = new DataColumn[] { col };
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

        private void AddFileRow(VAFileInfo fileInfo)
        {
            m_Table.Rows.Add( new object[] {fileInfo.GetHashCode(), 
                fileInfo.CameraId, 
                fileInfo.AdjustTime, 
                fileInfo.FileName, 
                fileInfo.FileFullName, 
                Common.Utils.GetByteSizeInUnit(fileInfo.FileSize),
                1,//(int)fileInfo.VAType,
                fileInfo,
                fileInfo.VATypeObject,
                fileInfo.VATypeFace,
                fileInfo.VATypeCar,
                fileInfo.VATypeBrief });

            HasFile = true;
        }

        public void AddFiles(List<object> files)
        {
            if (files != null && files.Count > 0)
            {
                string dateTime = DateTime.Now.ToString(DataModel.Constant.DATETIME_FORMAT);
                uint cameraId = m_cameraVM.Cameras[0].CameraID;
                //int nVaType = (int)E_VDA_ANALYZE_TYPE.E_ANALYZE_OBJECT;
                bool vatypeobject = true ;
                bool vatypeface = false;
                bool vatypecar = false;
                bool vatypebrief = false ;
                if (Framework.Environment.PRODUCT_TYPE
                    == Framework.Environment.E_PRODUCT_TYPE.ONLY_BRIEF)
                {
                    vatypeobject = false;
                    vatypebrief = true;
                }

                if (m_Table.Rows.Count > 0)
                {
                    VAFileInfo info = (VAFileInfo)m_Table.Rows[m_Table.Rows.Count - 1]["LocalVAFileInfo"];

                    dateTime = (string)m_Table.Rows[m_Table.Rows.Count - 1]["AdjustTime"];
                    cameraId = (uint)m_Table.Rows[m_Table.Rows.Count - 1]["CameraId"];
                    //nVaType = (int)m_Table.Rows[m_Table.Rows.Count - 1]["NVAType"];
                    vatypeobject = (bool)m_Table.Rows[m_Table.Rows.Count - 1]["NVATypeObject"];
                    vatypeface = (bool)m_Table.Rows[m_Table.Rows.Count - 1]["NVATypeFace"];
                    vatypecar = (bool)m_Table.Rows[m_Table.Rows.Count - 1]["NVATypeCar"];
                    vatypebrief = (bool)m_Table.Rows[m_Table.Rows.Count - 1]["NVATypeBrief"];

                }

                VAFileInfo fileInfo;
                foreach (object file in files)
                {
                    object[] info = file as object[];
                    string filefullname = info[0].ToString();
                    string filename = info[1].ToString();
                    ulong filesize = ulong.Parse(info[2].ToString());

                     fileInfo = new VAFileInfo(){
                            CameraId = cameraId,
                            AdjustTime = dateTime,
                            FileName = filename,
                             FileFullName = filefullname,
                              FileSize = filesize,
                               CameraName = "",
                            //VAType = (E_VDA_ANALYZE_TYPE)nVaType
                            VATypeObject = vatypeobject,
                            VATypeFace = vatypeface,
                            VATypeCar = vatypecar,
                            VATypeBrief = vatypebrief,

                     };
                                        
                    AddFileRow(fileInfo);
                }
            }
        }
        //private bool CheckName()
        //{
        //    string msg;
        //    string str = CreateTaskVM.TaskName;
        //    bool bRet = Common.TextUtil.ValidateNameText(ref str, false, "名称", 1, DataModel.Common.VDASDK_MAX_NAME_LEN, out msg);

        //    if (!bRet)
        //    {
        //        Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME,
        //               MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }
        //    return bRet;
        //}

        public bool ValidateFiles()
        {
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
                VAFileInfo fileInfo;
                List<string> files = new List<string>();
                foreach (DataRow row in m_Table.Rows)
                {
                    fileInfo = row["LocalVAFileInfo"] as VAFileInfo;
                    if (!files.Contains(fileInfo.FileFullName))
                    {
                        files.Add(fileInfo.FileFullName);
                    }
                    else
                    {
                        bRet = false;
                        Framework.Container.Instance.InteractionService.ShowMessageBox(
                            string.Format("不能导入重复的文件 ‘{0}", fileInfo.FileFullName), Framework.Environment.PROGRAM_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }
                }
            }
            return bRet;
        }
        
        public override bool Validate()
        {
            if (CreateTaskVM == null || !CreateTaskVM.IsVideoRemoteShare)
            {
                return true ;
            }

            bool bRetName = true;
            if (CreateTaskVM.IsNew)
            {
                bRetName = CreateTaskVM.ValidateTask();
            }
            bRetName = bRetName && ValidateFiles();
            return bRetName;
        }

        public override void Commit()
        {
            if (CreateTaskVM == null || !CreateTaskVM.IsVideoRemoteShare)
            {
                return;
            }
            else
            {
                uint taskId =0;
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
                    VAFileInfo[] localFiles = new VAFileInfo[count];
                    int i = 0;
                    foreach(DataRow row in m_Table.Rows)
                    {
                        localFiles[i] = (VAFileInfo)row["LocalVAFileInfo"];
                        localFiles[i].AdjustTime = (string)row["AdjustTime"];
                        localFiles[i].CameraId = (uint)row["CameraId"];
                        //localFiles[i].VAType = (E_VDA_ANALYZE_TYPE)((int)row["NVAType"]);
                        localFiles[i].VATypeBrief = (bool)row["NVATypeBrief"];
                        localFiles[i].VATypeCar = (bool)row["NVATypeCar"];
                        localFiles[i].VATypeFace = (bool)row["NVATypeFace"];
                        localFiles[i].VATypeObject = (bool)row["NVATypeObject"];
                        i++;
                    }

                    try
                    {
                        List<uint> ids = Framework.Container.Instance.TaskManagerService.AddRemoteVideoTaskUnits(taskId, localFiles);
                        if (ids == null)
                        {
                            string msg = string.Format("添加任务 '{0}' 的任务单元失败!", CreateTaskVM.TaskName);
                            Framework.Container.Instance.InteractionService.ShowMessageBox(
                                 msg, Framework.Environment.PROGRAM_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (SDKCallException ex)
                    {
                        Common.SDKCallExceptionHandler.Handle(ex, "添加远程任务单元");
                    }

                }
            }
        }

        public void DeleteFile()
        {
            if (SelectedFileInfo != null)
            {
                VAFileInfo fileInfo = SelectedFileInfo;
                DataRow row = m_Table.Rows.Find(fileInfo.GetHashCode());
                if (row != null)
                {
                    row.Delete();
                    HasFile = m_Table.Rows.Count > 0;
                }
            }
        }

    }

}
