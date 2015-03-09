using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Framework;
using BOCOM.IVX.Views.Content;
using System.Windows.Forms;
using BOCOM.DataModel;
using BOCOM.IVX.Protocol.Model;
using System.Diagnostics;

namespace BOCOM.IVX.ViewModel
{
    class MyCaseListViewModel : INotifyPropertyChanged, IEventAggregatorSubscriber
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public event EventHandler SelectedCaseChanged;

        #region Fields

        private DataTable m_DTCase;

        private CaseInfo m_SelectedCase;

        private bool m_EnterCaseEnabled;

        #endregion

        #region Properties

        public DataTable DTCase
        {
            get
            {
                if (m_DTCase == null)
                {
                    m_DTCase = new DataTable("AllCaseList");
                    DataColumn TaskID = m_DTCase.Columns.Add("CaseID", typeof(UInt32));
                    m_DTCase.PrimaryKey = new DataColumn[] { TaskID };
                    m_DTCase.Columns.Add("CaseName");
                    m_DTCase.Columns.Add("CaseNo");
                    m_DTCase.Columns.Add("CaseHappenTime", typeof(DateTime));
                    m_DTCase.Columns.Add("CaseHappenAddr");
                    m_DTCase.Columns.Add("CaseDescription");
                    m_DTCase.Columns.Add("Case", typeof(CaseInfo));
                    m_DTCase.Columns.Add("EnterCase", typeof(System.Drawing.Bitmap));
                    m_DTCase.Columns.Add("EditCase", typeof(System.Drawing.Bitmap));
                    m_DTCase.Columns.Add("DelCase", typeof(System.Drawing.Bitmap));

                    FillAllCase();
                }
                return m_DTCase;
            }
            set { m_DTCase = value; }
        }

        public int SelectedRowIndex
        {
            get;
            private set;
        }

        public bool EnterCaseEnabled
        {
            get
            {
                if (m_DTCase!=null && m_DTCase.Rows.Count > 0)
                {
                    return Framework.Container.Instance.EnteredCase == null ||
                        Framework.Container.Instance.EnteredCase.CaseID != SelectedCase.CaseID;
                }
                else
                    return false;
            }
            set
            {

            }
        }

        public CaseInfo SelectedCase
        {
            get { return m_SelectedCase ?? new CaseInfo() { CaseHappenTime = DateTime.Now }; }
            set
            {
                if (m_SelectedCase != value)
                {
                    m_SelectedCase = value;

                    if (SelectedCaseChanged != null)
                    {
                        DataRow row = m_DTCase.Rows.Find(m_SelectedCase.CaseID);
                        SelectedRowIndex = m_DTCase.Rows.IndexOf(row);
                        SelectedCaseChanged(this, EventArgs.Empty);
                    }
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("CurrEditCase"));
                        PropertyChanged(this, new PropertyChangedEventArgs("CaseName"));
                        PropertyChanged(this, new PropertyChangedEventArgs("CaseNo"));
                        PropertyChanged(this, new PropertyChangedEventArgs("CaseHappenAddr"));
                        PropertyChanged(this, new PropertyChangedEventArgs("CaseHappenTime"));
                        PropertyChanged(this, new PropertyChangedEventArgs("Description"));
                        PropertyChanged(this, new PropertyChangedEventArgs("EnterCaseEnabled"));
                    }
                }
            }
        }

        public string CaseName
        {
            get
            {
                string text = string.Empty;

                if (m_SelectedCase != null)
                {
                    text = m_SelectedCase.CaseName;
                }
                return text;
            }
            set{}
        }
        
        public string CaseNo 
        {
            get
            {
                string text = string.Empty;

                if (m_SelectedCase != null)
                {
                    text = m_SelectedCase.CaseNo;
                }
                return text;
            }
            set { }
        }

        public string CaseHappenAddr
        {
            get
            {
                string text = string.Empty;

                if (m_SelectedCase != null)
                {
                    text = m_SelectedCase.CaseHappenAddr;
                }
                return text;
            }
            set { }
        }

        public DateTime CaseHappenTime
        {
            get
            {
                DateTime dt = DateTime.MinValue;

                if (m_SelectedCase != null)
                {
                    dt = m_SelectedCase.CaseHappenTime;
                }
                return dt;
            }
            set { }
        }

        public string Description
        {
            get
            {
                string text = string.Empty;

                if (m_SelectedCase != null)
                {
                    text = m_SelectedCase.CaseDescription;
                }
                return text;
            }
            set { }
        }

        #endregion

        #region Constructors

        public MyCaseListViewModel()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<LeaveCaseEvent>().Subscribe(OnLeaveCase);
            Framework.Container.Instance.EvtAggregator.GetEvent<CaseAddedEvent>().Subscribe(CaseManagerService_CaseAdded);
            Framework.Container.Instance.EvtAggregator.GetEvent<CaseModifiedEvent>().Subscribe(CaseManagerService_CaseModified);
            Framework.Container.Instance.EvtAggregator.GetEvent<CaseDeletedEvent>().Subscribe(CaseManagerService_CaseDeleted);
            Framework.Container.Instance.RegisterEventSubscriber(this);
        }

        #endregion

        #region Public helper functions

        public void EnterCase()
        {
            CaseInfo info = SelectedCase;

            if (Framework.Container.Instance.EnteredCase!=null && info.CaseID == Framework.Container.Instance.EnteredCase.CaseID)
            {
                Debug.Assert(false, "重复进入同一个案件！");
                return;
            }

            if (Framework.Container.Instance.EnteredCase != null)
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
            
            if (info != null && info.CaseID > 0)
            {
                try
                {
                    Framework.Container.Instance.CaseManagerService.EnterCase(info);
                    PropertyChanged(this, new PropertyChangedEventArgs("EnterCaseEnabled"));
                }
                catch (SDKCallException ex)
                {
                    string operation = string.Format("进入案件 '{0}'", info.CaseName);
                    Common.SDKCallExceptionHandler.Handle(ex, operation);
                    return;
                }
                string caseName = info.CaseName;
                Framework.Container.Instance.EvtAggregator.GetEvent<EnterCaseEvent>().Publish(caseName);
                UIFuncItemInfo.CURRCASE.Subject = info;
                Framework.Container.Instance.EvtAggregator.GetEvent<NavigateEvent>().Publish(UIFuncItemInfo.CURRCASE);
            }
        }

        public void AddCase()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<NavigateEvent>().Publish(UIFuncItemInfo.NEWCASE);
        }

        public void EditCase()
        {
            if (SelectedCase != null)
            {
                UIFuncItemInfo.MODIFYCASE.Subject = SelectedCase;
                Framework.Container.Instance.EvtAggregator.GetEvent<NavigateEvent>().Publish(UIFuncItemInfo.MODIFYCASE);
            }
        }

        public void DelCase()
        {

            if (Framework.Container.Instance.EnteredCase != null &&
                Framework.Container.Instance.EnteredCase.CaseID == SelectedCase.CaseID)
            {
                if (Framework.Container.Instance.InteractionService.ShowMessageBox(
                    "不能删除当前进入的案件，是否要退出当前案件并删除？", Framework.Environment.PROGRAM_NAME,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    try
                    {
                        Framework.Container.Instance.CaseManagerService.ExitCase();
                    }
                    catch (SDKCallException ex)
                    {
                        Common.SDKCallExceptionHandler.Handle(ex, "退出案件");
                    }
                else
                    return;

            }
            else
            {
                string msg1 = string.Format("是否要删除案件【 {0} 】？", SelectedCase.CaseName);

                if (DialogResult.No == Framework.Container.Instance.InteractionService.ShowMessageBox(
                    msg1, Framework.Environment.PROGRAM_NAME,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    return;
                }
            }

            try
            {
                IVX.Framework.Container.Instance.CaseManagerService.DelCase(SelectedCase.CaseID);
                SelectedCase = new CaseInfo() { CaseHappenTime = DateTime.Now};
             }
            catch (SDKCallException ex)
            {
                string operation = string.Format("删除案件 '{0}'", SelectedCase.CaseName);
                Common.SDKCallExceptionHandler.Handle(ex, operation);
            }
        }

        #endregion

        #region Private helper functions

        private void AddCaseRow(CaseInfo caseInfo)
        {
                m_DTCase.Rows.Add(new object[]{caseInfo.CaseID,
                caseInfo.CaseName,
                caseInfo.CaseNo,
                caseInfo.CaseHappenTime,
                caseInfo.CaseHappenAddr,
                caseInfo.CaseDescription,
                caseInfo,
                Properties.Resources.进入案件1,
                Properties.Resources.编辑案件1,
                Properties.Resources.删除案件1});
        }

        private void FillAllCase()
        {
            List<CaseInfo> list = null;

            try
            {
                list = Framework.Container.Instance.CaseManagerService.GetAllCase();
                //m_allCaseList.Clear();
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "获取案件列表");
            }

            if (list != null)
            {
                list.ForEach(item => {
                    if(item.CaseType!= 0) // 判断是否是组内默认案件
                        AddCaseRow(item);
                });
            }
            
        }

        #endregion

        #region Event handlers

        void CaseManagerService_CaseModified(CaseInfo caseInfo)
        {
            if (caseInfo != null)
            {
                DataRow row = m_DTCase.Rows.Find(caseInfo.CaseID);
                if (row != null)
                {
                   row["CaseName"] = caseInfo.CaseName;
                   row["CaseNo"] = caseInfo.CaseNo;
                   row["CaseHappenTime"] = caseInfo.CaseHappenTime;
                   row["CaseHappenAddr"] = caseInfo.CaseHappenAddr;
                   row["CaseDescription"] = caseInfo.CaseDescription;
                   row["Case"] = caseInfo;
                   m_SelectedCase = caseInfo;

                   if (PropertyChanged != null)
                   {
                       PropertyChanged(this, new PropertyChangedEventArgs("CurrEditCase"));
                       PropertyChanged(this, new PropertyChangedEventArgs("CaseName"));
                       PropertyChanged(this, new PropertyChangedEventArgs("CaseNo"));
                       PropertyChanged(this, new PropertyChangedEventArgs("CaseHappenAddr"));
                       PropertyChanged(this, new PropertyChangedEventArgs("CaseHappenTime"));
                       PropertyChanged(this, new PropertyChangedEventArgs("Description"));
                   }
                }
            }
        }

        void CaseManagerService_CaseDeleted(uint caseID)
        {
            DataRow row = m_DTCase.Rows.Find(caseID);
              if (row != null)
              {
                  row.Delete();
              }
        }

        void CaseManagerService_CaseAdded(CaseInfo caseInfo)
        {
            //if (caseInfo.UserGroupId == Framework.Container.Instance.AuthenticationService.CurrLoginUserInfo.UserGroupId)
            //{
                AddCaseRow(caseInfo);
                SelectedCase = caseInfo;
            //}
        }

        private void OnLeaveCase(string caseName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs("EnterCaseEnabled"));
        }

        #endregion


        public void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<LeaveCaseEvent>().Unsubscribe(OnLeaveCase);
            Framework.Container.Instance.EvtAggregator.GetEvent<CaseAddedEvent>().Unsubscribe(CaseManagerService_CaseAdded);
            Framework.Container.Instance.EvtAggregator.GetEvent<CaseModifiedEvent>().Unsubscribe(CaseManagerService_CaseModified);
            Framework.Container.Instance.EvtAggregator.GetEvent<CaseDeletedEvent>().Unsubscribe(CaseManagerService_CaseDeleted);
        }
    }
}
