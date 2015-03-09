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

namespace BOCOM.IVX.Service
{
    public class CaseManagerService
    {

        public CaseManagerService()
        {
 
        }

        #region 案件

        public List<CaseInfo> GetAllCase()
        {
            List<CaseInfo> list = new List<CaseInfo>();
            //for (int i = 0; i < 10; i++)
            //{
            //    list.Add(new CaseInfo()
            //    {
            //        CaseDescription = "aaaaa",
            //        CaseHappenAddr = "sfasdf",
            //        CaseHappenTime = DateTime.Now,
            //        CaseID = (uint)i,
            //        CaseName = "case" + i,
            //        CaseNo = "case" + i,
            //    }
            //    );
            //}
            //return list;

            int lQueryHandle = Framework.Container.Instance.IVXProtocol.QueryCaseList();
            while (true)
            {
                CaseInfo ptCaseInfo = Framework.Container.Instance.IVXProtocol.QueryNextCase(lQueryHandle);
                if (ptCaseInfo != null)
                {
                    list.Add(ptCaseInfo);
                }
                else
                {
                    break;
                }
            }
            Framework.Container.Instance.IVXProtocol.CloseCaseQuery(lQueryHandle);
            return list;
        }

        public uint AddCase(CaseInfo tCaseBase)
        {
            uint CaseId = Framework.Container.Instance.IVXProtocol.AddCase(tCaseBase);
            if (CaseId > 0)
            {
                CaseInfo Case = Framework.Container.Instance.IVXProtocol.GetCaseByID(CaseId);
                Framework.Container.Instance.EvtAggregator.GetEvent<CaseAddedEvent>().Publish(Case);
            }

            return CaseId;
        }

        public bool EditCase(CaseInfo tCaseBase)
        {
            bool bRet = Framework.Container.Instance.IVXProtocol.MdfCase(tCaseBase);

            if (bRet)
            {
                Framework.Container.Instance.EvtAggregator.GetEvent<CaseModifiedEvent>().Publish(tCaseBase);
            }

            return bRet;
        }

        public bool DelCase(uint dwCaseID)
        {
            bool bRet = true;

            CaseInfo Case = Framework.Container.Instance.IVXProtocol.GetCaseByID(dwCaseID);

            if (Case != null)
            {
                bRet = Framework.Container.Instance.IVXProtocol.DelCase(dwCaseID);

                if (bRet)
                {
                    Framework.Container.Instance.EvtAggregator.GetEvent<CaseDeletedEvent>().Publish(dwCaseID);
                }
            }
            return bRet;
        }

        public void EnterCase(CaseInfo caseInfo)
        {
            Framework.Container.Instance.IVXProtocol.EnterCase(caseInfo.CaseID);
            Framework.Container.Instance.EnteredCase = caseInfo;

        }

        public void ExitCase()
        {
            if (Framework.Container.Instance.EnteredCase != null)
            {
                Framework.Container.Instance.LeaveCase();
            }
        }

        #endregion



    }
}
