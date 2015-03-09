using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using BOCOM.DataModel;

namespace BOCOM.IVX.ViewModel
{
    public class EnteredCaseViewModel : ViewModelBase
    {

        public CaseInfo Case { get; private set; }

        public EnteredCaseViewModel(CaseInfo caseInfo)
        {
            Case = caseInfo;
        }

        public void ExitCase()
        {
            try
            {
                Framework.Container.Instance.CaseManagerService.ExitCase();
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "退出案件");
            }
        }

        public override void UnSubscribe()
        {
            
        }
    }
}
