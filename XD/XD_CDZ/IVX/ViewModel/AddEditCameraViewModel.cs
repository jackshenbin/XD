using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using System.Windows.Forms;
using BOCOM.IVX.Protocol.Model;
using BOCOM.DataModel;
using BOCOM.IVX.Common;

namespace BOCOM.IVX.ViewModel
{
    class AddEditCameraViewModel : ViewModelBase
    {
        #region Fields

        private CameraInfo m_oldCamera;

        private CameraInfo m_newCamera;

        private List<VideoSupplierDeviceInfo> m_VideoSupplierDevices;

        private VideoSupplierDeviceInfo m_SelectedVideoSupplierDevice;

        // private bool m_CorrespondingChannelOn;

        private List<VideoSupplierChannelInfo> m_Channels;

        private VideoSupplierChannelInfo m_SelectedChannel;

        #endregion

        #region Properties

        public CameraInfo OldCamera
        {
            get { return m_oldCamera ?? new CameraInfo(); }
            set { m_oldCamera = value; }
        }

        public CameraInfo NewCamera
        {
            get { return m_newCamera ?? new CameraInfo(); }
            set { m_newCamera = value; }
        }

        public List<VideoSupplierDeviceInfo> VideoSupplierDevices
        {
            get
            {
                return m_VideoSupplierDevices;
            }
        }

        public List<VideoSupplierChannelInfo> Channels
        {
            get
            {
                return m_Channels;
            }
        }

        public bool HasVideoSupplierDevice
        {
            get
            {
                return m_VideoSupplierDevices != null && m_VideoSupplierDevices.Count > 0;
            }
            set { }
        }

        public VideoSupplierDeviceInfo SelectedVideoSupplierDevice
        {
            get
            {
                return m_SelectedVideoSupplierDevice;
            }
            set
            {
                if (m_SelectedVideoSupplierDevice != value)
                {
                    m_SelectedVideoSupplierDevice = value;
                    RaisePropertyChangedEvent("ImportPlatformVideoPageDescription");
                    if (m_SelectedVideoSupplierDevice != null && m_SelectedVideoSupplierDevice != DataModel.Constant.VIDEOSUPPLIERDEVICEINFO_DUMMY)
                    {
                        try
                        {
                            m_Channels = Framework.Container.Instance.VDAConfigService.GetVideoSupplierChannels(m_SelectedVideoSupplierDevice);
                        }
                        catch (SDKCallException ex)
                        {
                            m_Channels = null;
                            SDKCallExceptionHandler.Handle(ex, "获取视频设备通道", true);
                        }
                    }
                    else
                    {
                        m_Channels = null;
                    }
                }
            }
        }

        public VideoSupplierChannelInfo SelectedChannel
        {
            get
            {
                return m_SelectedChannel;
            }
            set
            {
                if (m_SelectedChannel != value)
                {
                    m_SelectedChannel = value;
                    RaisePropertyChangedEvent("SelectedChannel");
                }
            }
        }

        //public bool CorrespondingChannelOn
        //{
        //    get
        //    {
        //        return m_CorrespondingChannelOn;
        //    }
        //    set
        //    {
        //        if (m_CorrespondingChannelOn != value)
        //        {
        //            m_CorrespondingChannelOn = value;
        //            RaisePropertyChangedEvent("CorrespondingChannelOn");
        //        }
        //    }
        //}

        public string CameraGroupName
        {
            get
            {
                string groupName = string.Empty;
                try
                {
                    CameraGroupInfo group = Framework.Container.Instance.VDAConfigService.GetCameraGroupByID(m_newCamera.GroupID);
                    if(group!=null) groupName= group.GroupName;
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "获取用户组名");
                }
                return groupName;
            }
        }

        public bool IsEditMode { get; set; }

        public string ErrorString { get; set; }

        #endregion
        
        #region Constructors

        public AddEditCameraViewModel(CameraInfo cameraInfo, bool isEditMode)
        {
            OldCamera = cameraInfo;
            NewCamera = cameraInfo.Clone() as CameraInfo;
            IsEditMode = isEditMode;

            m_VideoSupplierDevices = Framework.Container.Instance.VDAConfigService.GetAllVideoSupplierDevice();
            if (m_VideoSupplierDevices == null)
            {
                m_VideoSupplierDevices = new List<VideoSupplierDeviceInfo>();
            }
            
            m_VideoSupplierDevices.Insert(0, DataModel.Constant.VIDEOSUPPLIERDEVICEINFO_DUMMY);
            
            bool matched = false;

            if (cameraInfo.VideoSupplierDeviceID > 0 && m_VideoSupplierDevices != null && m_VideoSupplierDevices.Count > 0)
            {
                foreach (VideoSupplierDeviceInfo device in m_VideoSupplierDevices)
                {
                    if (device.Id == cameraInfo.VideoSupplierDeviceID)
                    {
                        SelectedVideoSupplierDevice = device;
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    SelectedVideoSupplierDevice = m_VideoSupplierDevices[0];
                }
            }
        }

        #endregion
        
        #region Private helper functions

        private bool HasChange()
        {
            bool bRet = String.CompareOrdinal(OldCamera.CameraName, NewCamera.CameraName) != 0;
                            
            return bRet;
        }

        private bool CheckName()
        {
            string msg;
            bool bRet = Common.TextUtil.ValidateIfEmptyString(NewCamera.CameraName, "名称", out msg);

            if (!bRet)
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME,
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return bRet;
        }

        private bool Validate()
        {
            bool bRet = true;
            if (IsEditMode)
            {
                if (String.CompareOrdinal(OldCamera.CameraName, NewCamera.CameraName) != 0)
                {
                    bRet = CheckName();
                }
            }
            else
            {
                bRet = CheckName();
            }
            return bRet;
        }

        private int CheckCamera(CameraInfo newinfo, CameraInfo oldinfo)
        {
            return 0;
        }

        #endregion

        #region Public helper functions
        
        public VideoSupplierChannelInfo GetChannel(string id)
        {
            VideoSupplierChannelInfo channelRet = null;

            if (m_Channels != null && m_Channels.Count > 0)
            {
                foreach (VideoSupplierChannelInfo channel in m_Channels)
                {
                    if (string.Compare(id, channel.Id, true) == 0)
                    {
                        channelRet = channel;
                        break;
                    }
                }
            }
            return channelRet;
        }

        private bool AddCamera()
        {
            bool bRet = false;

            uint ret = 0;

            try
            {
                ret = IVX.Framework.Container.Instance.VDAConfigService.AddCamera(NewCamera);
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "添加监控点位");
            }
            bRet = ret > 0 ? true : false;

            return bRet;
        }

        private bool EditCamera()
        {
            bool bRet = false;
            try
            {
                bRet = IVX.Framework.Container.Instance.VDAConfigService.EditCamera(NewCamera);
            }
            catch (SDKCallException ex)
            {
                string msg = string.Format("修改点位失败, 原因: {0}", ex.Message);
                Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME,
                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                bRet = false;
            }
            return bRet;
        }

        public bool Commit()
        {
             bool bRet = Validate();

             if (bRet)
             {
                 if (m_SelectedChannel != null)
                 {
                     NewCamera.VideoSupplierDeviceID = m_SelectedVideoSupplierDevice.Id;
                     NewCamera.VideoSupplierChannelID = m_SelectedChannel.Id;
                 }
                 else
                 {
                     NewCamera.VideoSupplierDeviceID = 0;
                     NewCamera.VideoSupplierChannelID = string.Empty;
                 }

                 if (IsEditMode)
                 {
                     bRet = EditCamera();
                 }
                 else
                 {
                     bRet = AddCamera();
                 }
             }

         
            return bRet;
        }

        #endregion

    }
}
