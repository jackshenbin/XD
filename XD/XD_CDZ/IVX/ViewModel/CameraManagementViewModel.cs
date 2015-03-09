using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Framework;
using System.Windows.Forms;
using BOCOM.DataModel;

namespace BOCOM.IVX.ViewModel
{
    public class CameraManagementViewModel : IEventAggregatorSubscriber
    {
        public event EventHandler CameraGroupAdded;
        public event EventHandler CameraGroupDeleted;

        public event EventHandler CameraAdded;
        public event EventHandler CameraDeleted;

        #region Fields

        private DataTable m_allCameraList;

        private CameraInfo m_currEditCamera;

        private DataTable m_allCameraGroupList;

        private CameraGroupInfo m_currEditCameraGroup;

        #endregion

        #region Properties

        public DataTable AllCameraList
        {
            get 
            {
                if (m_allCameraList == null)
                {
                    m_allCameraList = new DataTable("AllCameraList");
                    DataColumn dwCameraID = m_allCameraList.Columns.Add("CameraID", typeof(UInt32));
                    m_allCameraList.PrimaryKey = new DataColumn[] { dwCameraID };
                    m_allCameraList.Columns.Add("GroupID",typeof(UInt32));
                    m_allCameraList.Columns.Add("CameraName");
                    m_allCameraList.Columns.Add("VideoSupplierDeviceID", typeof(UInt32));
                    m_allCameraList.Columns.Add("VideoSupplierDeviceCameraID");
                    m_allCameraList.Columns.Add("PosCoordX", typeof(float));
                    m_allCameraList.Columns.Add("PosCoordY", typeof(float));
                    m_allCameraList.Columns.Add("Camera", typeof(CameraInfo));
                    FillAllCamera(CurrEditCameraGroup.CameraGroupID);
                }
                return m_allCameraList;
            }
            set { m_allCameraList = value; }
        }

        public CameraInfo CurrEditCamera
        {
            get { return m_currEditCamera ?? new CameraInfo(); }
            set { m_currEditCamera = value; }
        }

        public DataTable AllCameraGroupList
        {
            get 
            {
                if (m_allCameraGroupList == null)
                {
                    m_allCameraGroupList = new DataTable("AllCameraGroupList");
                    DataColumn dwCameraGroupID = m_allCameraGroupList.Columns.Add("CameraGroupID", typeof(UInt32));
                    m_allCameraGroupList.PrimaryKey = new DataColumn[] { dwCameraGroupID };
                    m_allCameraGroupList.Columns.Add("ParentGroupID", typeof(UInt32));
                    m_allCameraGroupList.Columns.Add("GroupName");
                    m_allCameraGroupList.Columns.Add("GroupDescription");
                    m_allCameraGroupList.Columns.Add("CameraGroup", typeof(CameraGroupInfo));
                    FillAllCameraGroup();
                }
                return m_allCameraGroupList;
            }
            set { m_allCameraGroupList = value; }
        }

        public CameraGroupInfo CurrEditCameraGroup
        {
            get { return m_currEditCameraGroup ?? new CameraGroupInfo(); }
            set 
            { 
                if (m_currEditCameraGroup != value)
                {
                    m_currEditCameraGroup = value;
                    FillAllCamera(value.CameraGroupID);
                }

            }
        }

        #endregion

        #region Constructors

        public CameraManagementViewModel()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraAddedEvent>().Subscribe(OnCameraAdded, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraModifiedEvent>().Subscribe(OnCameraModified, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraDeletedEvent>().Subscribe(OnCameraDeleted, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraGroupAddedEvent>().Subscribe(OnCameraGroupAdded, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraGroupModifiedEvent>().Subscribe(OnCameraGroupModified, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraGroupDeletedEvent>().Subscribe(OnCameraGroupDeleted, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread);
            Framework.Container.Instance.RegisterEventSubscriber(this);
        }

        #endregion

        #region Public helper functions

        public void AddCamera()
        {
            if (CurrEditCameraGroup.CameraGroupID != 0)
            {
                FormNewCamera dlg = new FormNewCamera(new CameraInfo() { GroupID = CurrEditCameraGroup.CameraGroupID });
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog();
            }
        }

        public void EditCamera()
        {
            if (CurrEditCamera.CameraID != 0)
            {
                FormNewCamera dlg = new FormNewCamera(CurrEditCamera, true);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog();
            }
        }

        public void DelCamera()
        {
            if (CurrEditCamera.CameraID != 0)
            {
                string msg = string.Format("是否要删除监控点【 {0} 】？", CurrEditCamera.CameraName);

                if (DialogResult.Yes == Framework.Container.Instance.InteractionService.ShowMessageBox(
                    msg, Framework.Environment.PROGRAM_NAME,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    try
                    {
                        IVX.Framework.Container.Instance.VDAConfigService.DelCamera(CurrEditCamera.CameraID);
                    }
                    catch (SDKCallException ex)
                    {
                        Common.SDKCallExceptionHandler.Handle(ex, "删除监控点");
                    }
                }
            }
        }

        public void AddCameraGroup()
        {
           
            FormNewCameraGroup FormNewCameraGroup = new FormNewCameraGroup(new CameraGroupInfo() {  ParentGroupID = 0 });
            FormNewCameraGroup.ShowDialog();
        }

        public void EditCameraGroup()
        {
            if (CurrEditCameraGroup.CameraGroupID != 0)
            {
                FormNewCameraGroup FormNewCameraGroup = new FormNewCameraGroup(CurrEditCameraGroup, true);
                FormNewCameraGroup.ShowDialog();
            }
        }

        public void DelCameraGroup()
        {
            if (CurrEditCameraGroup.CameraGroupID != 0)
            {
                string msg = string.Format("注意删除监控组会导致监控组下全部的监控点位删除。是否要删除监控组【 {0} 】？", CurrEditCameraGroup.GroupName);

                if (DialogResult.Yes == Framework.Container.Instance.InteractionService.ShowMessageBox(
                    msg, Framework.Environment.PROGRAM_NAME,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    try
                    {
                        IVX.Framework.Container.Instance.VDAConfigService.DelCameraGroup(CurrEditCameraGroup.CameraGroupID);
                    }
                    catch (SDKCallException ex)
                    {
                        Common.SDKCallExceptionHandler.Handle(ex, "删除监控组");
                    }
                }
            }
        }

        #endregion

        #region Private helper functions

        private void FillAllCamera(uint cameraGroupID)
        {
            if (m_allCameraList == null)
            {
                return;
            }

            List<CameraInfo> list = new List<CameraInfo>();
            try
            {
                list = Framework.Container.Instance.VDAConfigService.GetCameras(cameraGroupID, false);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "获取监控点位");
            }
            
            m_allCameraList.Rows.Clear();

            list.ForEach(ptCameraInfo => AddRow(ptCameraInfo));
            
        }

        private void AddRow(CameraInfo cameraInfo)
        {
            m_allCameraList.Rows.Add(
                                            cameraInfo.CameraID,
                                            cameraInfo.GroupID,
                                            cameraInfo.CameraName,
                                            cameraInfo.VideoSupplierDeviceID,
                                            cameraInfo.VideoSupplierChannelID,
                                            cameraInfo.PosCoordX,
                                            cameraInfo.PosCoordY,
                                            cameraInfo);
        }

        private void FillAllCameraGroup()
        {
            List<CameraGroupInfo> list = null;
            try
            {
                list = Framework.Container.Instance.VDAConfigService.GetAllCameraGroup(0);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "获取点位组");
                list = null;
            }
            m_allCameraGroupList.Rows.Clear();
            
            if (list != null)
            {
                list.ForEach(ptCameraGroupInfo => AddRow(ptCameraGroupInfo));
            }
        }

        private void AddRow(CameraGroupInfo cameraGroupInfo)
        {
            m_allCameraGroupList.Rows.Add(
                                            cameraGroupInfo.CameraGroupID,
                                            cameraGroupInfo.ParentGroupID,
                                            cameraGroupInfo.GroupName,
                                            cameraGroupInfo.GroupDescription,
                                            cameraGroupInfo);
        }

        #endregion

        #region Event handlers

        void OnCameraAdded(CameraInfo info)
        {
            DataRow row = m_allCameraList.Rows.Find(info.CameraID);
            if (row == null)
            {
                AddRow(info);
                if (CameraAdded != null)
                {
                    CameraAdded(info.CameraID, EventArgs.Empty);
                }
            }
        }

        void OnCameraModified(CameraInfo info)
        {
            DataRow row = m_allCameraList.Rows.Find(info.CameraID);
            if (row != null)
            {
                row["CameraName"] = info.CameraName;
                row["Camera"] = info;
                CurrEditCamera = info;
            }
        }

        void OnCameraDeleted(uint camID)
        {
            DataRow row = m_allCameraList.Rows.Find(camID);
            if (row != null)
            {
                row.Delete();
                if (CameraDeleted != null)
                {
                    CameraDeleted(camID, null);
                }
            }
        }

        void OnCameraGroupAdded(CameraGroupInfo info)
        {
            DataRow row = m_allCameraGroupList.Rows.Find(info.CameraGroupID);
            if (row == null)
            {
                AddRow(info);
                if (CameraGroupAdded != null)
                {
                    CameraGroupAdded(info.CameraGroupID, null);
                }
            }
        }

        void OnCameraGroupModified(CameraGroupInfo info)
        {
            DataRow row = m_allCameraGroupList.Rows.Find(info.CameraGroupID);
            if (row != null)
            {
                row["GroupName"] = info.GroupName;
                row["CameraGroup"] = info;
                CurrEditCameraGroup = info;
            }
        }

        void OnCameraGroupDeleted(uint camGroupID)
        {
            DataRow row = m_allCameraGroupList.Rows.Find(camGroupID);
            if (row != null)
            {
                row.Delete();
                if (CameraGroupDeleted != null)
                {
                    CameraGroupDeleted(camGroupID, null);
                }
            }
        }

        #endregion

        public void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraAddedEvent>().Unsubscribe(OnCameraAdded);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraModifiedEvent>().Unsubscribe(OnCameraModified);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraDeletedEvent>().Unsubscribe(OnCameraDeleted);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraGroupAddedEvent>().Unsubscribe(OnCameraGroupAdded);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraGroupModifiedEvent>().Unsubscribe(OnCameraGroupModified);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraGroupDeletedEvent>().Unsubscribe(OnCameraGroupDeleted);
        }
    }
}
