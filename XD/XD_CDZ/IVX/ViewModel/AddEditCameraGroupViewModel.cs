using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using System.Windows.Forms;
using BOCOM.DataModel;

namespace BOCOM.IVX.ViewModel
{
    class AddEditCameraGroupViewModel
    {
        #region Fields

        private CameraGroupInfo m_oldCameraGroup;

        private CameraGroupInfo m_newCameraGroup;

        #endregion

        #region Properties

        public CameraGroupInfo OldCameraGroup
        {
            get { return m_oldCameraGroup ?? new CameraGroupInfo(); }
            set { m_oldCameraGroup = value; }
        }

        public CameraGroupInfo NewCameraGroup
        {
            get { return m_newCameraGroup ?? new CameraGroupInfo(); }
            set { m_newCameraGroup = value; }
        }

        public bool IsEditMode { get; set; }

        public string ErrorString { get; set; }

        #endregion
        
        #region Constructors

        public AddEditCameraGroupViewModel(CameraGroupInfo cameraGroupInfo, bool isEditMode)
        {
            OldCameraGroup = cameraGroupInfo;
            NewCameraGroup = cameraGroupInfo.Clone() as CameraGroupInfo;
            IsEditMode = isEditMode;
        }

        #endregion
        
        #region Private helper functions

        private bool HasChange()
        {
            bool bRet = String.CompareOrdinal(OldCameraGroup.GroupName, NewCameraGroup.GroupName) != 0;
                            
            return bRet;
        }

        private bool CheckName()
        {
            string msg;
            string name = NewCameraGroup.GroupName;
            bool bRet = Common.TextUtil.ValidateNameText(ref name,false, "监控点组名称",1,DataModel.Common.VDA_MAX_NAME_LEN-1, out msg);
            NewCameraGroup.GroupName = name;

            if (!bRet)
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME,
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                NewCameraGroup.RaiseValidateFailEvent("GroupName");

                return bRet;
            }

            string descripation = NewCameraGroup.GroupDescription;
            bRet = Common.TextUtil.ValidateNameText(ref descripation,true, "监控点组描述",0,DataModel.Common.VDA_MAX_DESCRIPTION_INFO_LEN-1, out msg);
            NewCameraGroup.GroupDescription = descripation;

            if (!bRet)
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME,
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);

                NewCameraGroup.RaiseValidateFailEvent("GroupDescription");
                return bRet;
            }
            
            NewCameraGroup.GroupName = name;
            
            return bRet;
        }

        private bool Validate()
        {
            bool bRet = CheckName();
            return bRet;
        }

        private int CheckCameraGroup(CameraGroupInfo newinfo, CameraGroupInfo oldinfo)
        {
            return 0;
        }

        #endregion

        #region Public helper functions

        private bool AddCameraGroup()
        {
            bool bRet = Validate();

            if (bRet)
            {
                uint ret = 0;
                try
                {
                    ret = IVX.Framework.Container.Instance.VDAConfigService.AddCameraGroup(NewCameraGroup);
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "添加监控组");
                }
                bRet = ret > 0 ? true : false;

            }
            return bRet;
        }

        private bool EditCameraGroup()
        {
            bool bRet = Validate();

            if (bRet)
            {
                try
                {
                    bRet = false;
                    bRet = IVX.Framework.Container.Instance.VDAConfigService.EditCameraGroup(NewCameraGroup);
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "修改监控组");
                }
            }
            return bRet;
        }

        public bool Commit()
        {
            bool bRet = false;
            if (IsEditMode)
            {
                bRet = EditCameraGroup();
            }
            else
            {
                bRet = AddCameraGroup();
            }
            return bRet;
        }

        #endregion

    }
}
