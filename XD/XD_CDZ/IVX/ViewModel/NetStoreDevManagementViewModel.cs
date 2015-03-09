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
    public class VideoSupplierDeviceManagementViewModel : IEventAggregatorSubscriber
    {
        public event EventHandler VideoSupplierDeviceAdded;
        public event EventHandler VideoSupplierDeviceDeleted;

        #region Fields

        private DataTable m_allVideoSupplierDeviceList;

        private VideoSupplierDeviceInfo m_currEditVideoSupplierDevice;

        #endregion

        #region Properties

        public DataTable AllVideoSupplierDeviceList
        {
            get 
            {
                if (m_allVideoSupplierDeviceList == null)
                {
                    m_allVideoSupplierDeviceList = new DataTable("AllVideoSupplierDeviceList");
                    DataColumn dwVideoSupplierDeviceID = m_allVideoSupplierDeviceList.Columns.Add("VideoSupplierDeviceID", typeof(UInt32));
                    m_allVideoSupplierDeviceList.PrimaryKey = new DataColumn[] { dwVideoSupplierDeviceID };
                    m_allVideoSupplierDeviceList.Columns.Add("ProtocolType", typeof(UInt32));
                    m_allVideoSupplierDeviceList.Columns.Add("DeviceName");
                    m_allVideoSupplierDeviceList.Columns.Add("DeviceIP");
                    m_allVideoSupplierDeviceList.Columns.Add("DevicePort", typeof(UInt32));
                    m_allVideoSupplierDeviceList.Columns.Add("LoginUser");
                    m_allVideoSupplierDeviceList.Columns.Add("LoginPwd");
                    m_allVideoSupplierDeviceList.Columns.Add("VideoSupplierDeviceInfo", typeof(VideoSupplierDeviceInfo));
                    FillAllVideoSupplierDevice();
                }
                return m_allVideoSupplierDeviceList;
            }
            set { m_allVideoSupplierDeviceList = value; }
        }

        public VideoSupplierDeviceInfo CurrEditVideoSupplierDevice
        {
            get { return m_currEditVideoSupplierDevice ?? new VideoSupplierDeviceInfo(); }
            set 
            { 
                if (m_currEditVideoSupplierDevice != value)
                {
                    m_currEditVideoSupplierDevice = value;
                }

            }
        }

        #endregion

        #region Constructors

        public VideoSupplierDeviceManagementViewModel()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<AddVideoSupplierDeviceEvent>().Subscribe(OnVideoSupplierDeviceAdded);
            Framework.Container.Instance.EvtAggregator.GetEvent<EditVideoSupplierDeviceEvent>().Subscribe(OnVideoSupplierDeviceModified);
            Framework.Container.Instance.EvtAggregator.GetEvent<DelVideoSupplierDeviceEvent>().Subscribe(OnVideoSupplierDeviceDeleted);
            Framework.Container.Instance.RegisterEventSubscriber(this);
        }

        #endregion

        #region Public helper functions

        public void AddVideoSupplierDevice()
        {
            FormNewVideoSupplierDevice FormNewVideoSupplierDevice = new FormNewVideoSupplierDevice(new VideoSupplierDeviceInfo { ProtocolType = DataModel.Constant.AccessProtocolTypeInfos[0].Type });
            FormNewVideoSupplierDevice.ShowDialog();
        }

        public void EditVideoSupplierDevice()
        {
            if (CurrEditVideoSupplierDevice.Id != 0)
            {
                FormNewVideoSupplierDevice FormNewVideoSupplierDevice = new FormNewVideoSupplierDevice(CurrEditVideoSupplierDevice, true);
                FormNewVideoSupplierDevice.ShowDialog();
            }
        }

        public void DelVideoSupplierDevice()
        {
            if (CurrEditVideoSupplierDevice.Id != 0)
            {
                string msg = string.Format("是否要删除网络存储设备【 {0} 】？", CurrEditVideoSupplierDevice.DeviceName);

                if (DialogResult.Yes == Framework.Container.Instance.InteractionService.ShowMessageBox(
                    msg, Framework.Environment.PROGRAM_NAME,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    try
                    {
                        IVX.Framework.Container.Instance.VDAConfigService.DelVideoSupplierDevice(CurrEditVideoSupplierDevice.Id);
                    }
                    catch (SDKCallException ex)
                    {
                        Common.SDKCallExceptionHandler.Handle(ex, "删除网络存储设备");
                    }
                }
            }
        }

        #endregion

        #region Private helper functions

        private void FillAllVideoSupplierDevice()
        {
            List<VideoSupplierDeviceInfo> list = null;
            try
            {
                list = Framework.Container.Instance.VDAConfigService.GetAllVideoSupplierDevice();
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "获取网络存储设备");
                list = null;
            }
            m_allVideoSupplierDeviceList.Rows.Clear();
            
            if (list != null)
            {
                list.ForEach(ptCameraGroupInfo => AddRow(ptCameraGroupInfo));
            }
        }

        private void AddRow(VideoSupplierDeviceInfo VideoSupplierDeviceInfo)
        {
            if ((int)VideoSupplierDeviceInfo.ProtocolType < 0)
            {
                VideoSupplierDeviceInfo.ProtocolType = E_VDA_NET_STORE_DEV_PROTOCOL_TYPE.E_DEV_PROTOCOL_CONTYPE_NONE;
            }
            m_allVideoSupplierDeviceList.Rows.Add(
                                VideoSupplierDeviceInfo.Id,
                                VideoSupplierDeviceInfo.ProtocolType,
                                VideoSupplierDeviceInfo.DeviceName,
                                VideoSupplierDeviceInfo.IP,
                                VideoSupplierDeviceInfo.Port,
                                VideoSupplierDeviceInfo.UserName,
                                VideoSupplierDeviceInfo.Password,
                                VideoSupplierDeviceInfo);

        }

        #endregion  

        #region Event handlers

        void OnVideoSupplierDeviceAdded(VideoSupplierDeviceInfo info)
        {
            AddRow(info);
            if (VideoSupplierDeviceAdded != null)
            {
                VideoSupplierDeviceAdded(info.Id, null);
            }
        }

        void OnVideoSupplierDeviceModified(VideoSupplierDeviceInfo info)
        {
            DataRow row = m_allVideoSupplierDeviceList.Rows.Find(info.Id);
            if (row != null)
            {
                row["ProtocolType"] = (uint)info.ProtocolType;
                row["DeviceName"] = info.DeviceName;
                row["DeviceIP"] = info.IP;
                row["DevicePort"] = info.Port;
                row["LoginUser"] = info.UserName;
                row["LoginPwd"] = info.Password;
                row["VideoSupplierDeviceInfo"] = info;
                CurrEditVideoSupplierDevice = info;
            }
        }

        void OnVideoSupplierDeviceDeleted(uint VideoSupplierDeviceID)
        {
            DataRow row = m_allVideoSupplierDeviceList.Rows.Find(VideoSupplierDeviceID);
            if (row != null)
            {
                row.Delete();
                if (VideoSupplierDeviceDeleted != null)
                {
                    VideoSupplierDeviceDeleted(VideoSupplierDeviceID, null);
                }
            }
        }

        #endregion

        public void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<AddVideoSupplierDeviceEvent>().Unsubscribe(OnVideoSupplierDeviceAdded);
            Framework.Container.Instance.EvtAggregator.GetEvent<EditVideoSupplierDeviceEvent>().Unsubscribe(OnVideoSupplierDeviceModified);
            Framework.Container.Instance.EvtAggregator.GetEvent<DelVideoSupplierDeviceEvent>().Unsubscribe(OnVideoSupplierDeviceDeleted);
        }
    }
}
