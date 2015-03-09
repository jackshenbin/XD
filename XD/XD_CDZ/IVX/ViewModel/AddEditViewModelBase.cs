using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using System.Windows.Forms;

namespace BOCOM.IVX.ViewModel
{
    public class AddEditViewModelBase : ViewModelBase
    {

        private string m_ModelName;

        public bool IsEditMode { get; protected set; }

        public AddEditViewModelBase(string modelName, bool isEditMode)
        {
            m_ModelName = modelName;
            IsEditMode = isEditMode;
        }

        private bool Add()
        {
            bool bRet = Validate();

            if (bRet)
            {
                    bRet = AddEx();
            }
            return bRet;
        }

        private bool Edit()
        {
            bool bRet = Validate();

            if (bRet)
            {
                    bRet = EditEx();
            }
            return bRet;
        }

        protected virtual bool Validate()
        {
            throw new NotImplementedException();
        }

        protected virtual bool AddEx()
        {
            throw new NotImplementedException();
        }

        protected virtual bool EditEx()
        {
            throw new NotImplementedException();
        }

        public bool Commit()
        {
            bool bRet = false;
            if (IsEditMode)
            {
                bRet = Edit();
            }
            else
            {
                bRet = Add();
            }
            return bRet;
        }
    }
}
