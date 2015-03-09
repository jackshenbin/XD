using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using System.Windows.Forms;
using BOCOM.DataModel;

namespace BOCOM.IVX.ViewModel
{
    class AddEditCaseViewModel : AddEditViewModelBase
    {
        //#region Fields

        //private CaseInfo m_oldCase;

        //private CaseInfo m_newCase;

        //#endregion

        #region Properties

        public CaseInfo OldCase { get; private set; }

        public CaseInfo NewCase { get; private set; }

        public bool IsEditMode { get; private set; }

        public string Caption
        {
            get;
            private set;
        }

        public string ErrorString { get; set; }

        #endregion

        #region Constructors
                
        public AddEditCaseViewModel(CaseInfo caseInfo, bool isEditMode = false)
            : base("案件", isEditMode)
        {
            OldCase = caseInfo;
            NewCase = (CaseInfo)caseInfo.Clone();
            IsEditMode = isEditMode;

            if (isEditMode)
            {
                Caption = string.Format("修改案件【{0}】", NewCase.CaseName);
            }
            else
            {
                NewCase.UserGroupId = Framework.Container.Instance.AuthenticationService.CurrUser.UserGroupID;
                Caption = "新增案件";
            }
        }

        #endregion

        #region Private helper functions

        private bool CheckName()
        {
            string msg;
            string casename = NewCase.CaseName;
            bool bRet = Common.TextUtil.ValidateNameText(ref casename,false, "案件名称",1,BOCOM.DataModel.Common.VDA_MAX_NAME_LEN-1, out msg);
            NewCase.CaseName = casename;

            if (!bRet)
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME,
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return bRet;
            }

            string casedesc = NewCase.CaseDescription;
            bRet = Common.TextUtil.ValidateNameText(ref casedesc, true, "案件描述", 0, BOCOM.DataModel.Common.VDA_MAX_DESCRIPTION_INFO_LEN - 1, out msg);
            NewCase.CaseDescription = casedesc;

            if (!bRet)
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME,
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return bRet;
            }


            return bRet;
        }

        private bool CheckNo()
        {
            string msg;
            string input = NewCase.CaseNo;
            bool bRet = Common.TextUtil.ValidateNameText(ref input, false, "统一编号", 1, BOCOM.DataModel.Common.VDA_MAX_NAME_LEN-1, out msg);
            NewCase.CaseNo = input;

            if (!bRet)
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME,
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return bRet;
        }


        #endregion

        #region Protected helper functions
        
        protected override bool Validate()
        {
            bool bRetName = true;
            bool bRetNo = true;
            if (IsEditMode)
            {
                if (String.CompareOrdinal(OldCase.CaseName, NewCase.CaseName) != 0)
                {
                    bRetName = CheckName();
                }
                if (bRetName &&
                    String.CompareOrdinal(OldCase.CaseNo, NewCase.CaseNo) != 0)
                {
                    bRetNo = CheckNo();
                }
            }
            else
            {
                bRetName = CheckName();
                if (bRetName)
                {
                    bRetNo = CheckNo();
                }
            }
            return bRetName && bRetNo;
        }

        protected override bool AddEx()
        {
            bool bRet = false;

            try
            {
                NewCase.CaseType = 1;
                IVX.Framework.Container.Instance.CaseManagerService.AddCase(NewCase);
                bRet = true;
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "添加案件");
            }

            return bRet;
        }

        protected override bool EditEx()
        {
            bool bRet = false;
            try
            {
                IVX.Framework.Container.Instance.CaseManagerService.EditCase(NewCase);
                bRet = true;
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "修改案件");
            }

            return bRet;
        }

        #endregion


    }
}
