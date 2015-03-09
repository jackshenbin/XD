using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using BOCOM.DataModel;
using System.ComponentModel;
using System.Windows.Forms;

namespace BOCOM.IVX.ViewModel
{
    class EditTaskViewModel : ViewModelBase
    {
        private TaskInfo m_oldTask;
        private TaskInfo m_newTask;

        public TaskInfo OldTask
        {
            get { return m_oldTask??new TaskInfo(); }
            set { m_oldTask = value; }
        }

        public TaskInfo NewTask
        {
            get { return m_newTask ?? new TaskInfo(); }
            set { m_newTask = value; }
        }

        public uint TaskPriorityLevel
        {
            get { return Math.Max(m_newTask.TaskPriorityLevel - 3, 0); }
            set { m_newTask.TaskPriorityLevel = Math.Min(value + 3, 10); }
        }

        public TaskPriorityInfo[] PriorityInfos
        {
            get { return DataModel.Constant.TaskPriorityInfos; }
        }

        public string TaskName
        {
            get
            {
                return NewTask.TaskName;
            }
            set
            {
                NewTask.TaskName = value;
            }
        }

        public TaskPriority Priority
        {
            get
            {
                return (TaskPriority)NewTask.TaskPriorityLevel;
            }
            set
            {
                NewTask.TaskPriorityLevel = (uint)value;
            }
        }

        private bool HasChange()
        {
            bool bRet = false;
            NewTask.TaskName = NewTask.TaskName.Trim();

            if (String.CompareOrdinal(NewTask.TaskName, OldTask.TaskName) != 0)
            {
                bRet = true;
            }
            else if (NewTask.TaskPriorityLevel != OldTask.TaskPriorityLevel)
            {
                bRet = true;
            }

            return bRet;
        }

        private bool Validate()
        {
            string taskname = m_newTask.TaskName;
            string errMsg = "";
            bool result = Common.TextUtil.ValidateNameText(ref taskname, false, "任务名称", 1, DataModel.Common.VDA_MAX_NAME_LEN-1, out errMsg);
            m_newTask.TaskName = taskname;

            if (!result)
            {
                System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(errMsg));
                Framework.Container.Instance.InteractionService.ShowMessageBox(errMsg, Framework.Environment.PROGRAM_NAME,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return result;
            }

            return result;
        }

        public bool Commit()
        {
            bool result = true;
            if (HasChange())
            {
                result = false;
                if (Validate())
                {
                    try
                    {
                        result = Framework.Container.Instance.TaskManagerService.EditTask(m_newTask);
                        if (!result)
                        {
                            string msg = string.Format("修改任务 '{0}' 失败!", m_oldTask.TaskName);
                            Framework.Container.Instance.InteractionService.ShowMessageBox(
                                 msg, Framework.Environment.PROGRAM_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return result;
                    }
                    catch (SDKCallException ex)
                    {
                        Common.SDKCallExceptionHandler.Handle(ex, "修改任务");
                    }              

                }
            }
            return result;
        }

    }
}
