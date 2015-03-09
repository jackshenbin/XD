using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using System.Windows.Forms;
using BOCOM.DataModel;

namespace BOCOM.IVX.ViewModel
{
    class AddEditVideoSupplierDeviceViewModel : ViewModelBase
    {
        #region Fields

        private VideoSupplierDeviceInfo m_oldVideoSupplierDevice;

        private VideoSupplierDeviceInfo m_newVideoSupplierDevice;

        #endregion

        #region Properties

        public VideoSupplierDeviceInfo OldVideoSupplierDevice
        {
            get { return m_oldVideoSupplierDevice ?? new VideoSupplierDeviceInfo(); }
            set { m_oldVideoSupplierDevice = value; }
        }

        public VideoSupplierDeviceInfo NewVideoSupplierDevice
        {
            get { return m_newVideoSupplierDevice ?? new VideoSupplierDeviceInfo(); }
            set { m_newVideoSupplierDevice = value; }
        }

        public bool IsEditMode { get; set; }

        public string ErrorString { get; set; }

        public AccessProtocolTypeInfo[] AccessProtocol
        {
            get { return DataModel.Constant.AccessProtocolTypeInfos; }
        }

        public E_VDA_NET_STORE_DEV_PROTOCOL_TYPE ProtocolType
        {
            get { return (E_VDA_NET_STORE_DEV_PROTOCOL_TYPE)NewVideoSupplierDevice.ProtocolType; }
            set 
            {
                NewVideoSupplierDevice.ProtocolType = value;
                UpdateVideoSupplierDeviceName(); 
            }
        }

        public string DeviceName
        {
            get { return NewVideoSupplierDevice.DeviceName; }
            set { NewVideoSupplierDevice.DeviceName = value; }
        }

        public string  DeviceIP
        {
            get { return NewVideoSupplierDevice.IP; }
            set 
            {
                NewVideoSupplierDevice.IP = value; 
                UpdateVideoSupplierDeviceName(); 
            }
        }

        public uint DevicePort
        {
            get { return NewVideoSupplierDevice.Port; }
            set 
            {
                NewVideoSupplierDevice.Port = value; 
                UpdateVideoSupplierDeviceName(); 
            }
        }

        
        #endregion
        
        #region Constructors

        public AddEditVideoSupplierDeviceViewModel(VideoSupplierDeviceInfo VideoSupplierDeviceInfo, bool isEditMode)
        {
            OldVideoSupplierDevice = VideoSupplierDeviceInfo;
            NewVideoSupplierDevice = VideoSupplierDeviceInfo.Clone() as VideoSupplierDeviceInfo;
            IsEditMode = isEditMode;
        }

        #endregion
        
        #region Private helper functions

        private bool CheckValue()
        {
            string msg;
            bool bRet = Common.TextUtil.ValidateIPAddress(NewVideoSupplierDevice.IP);
            if (!bRet)
            {
                msg = "输入地址不符合IP规范";
                Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME,
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return bRet;
            }
            string user = NewVideoSupplierDevice.UserName;
            bRet = Common.TextUtil.ValidateNameText(ref user, false, "登录用户", 1, DataModel.Common.VDA_MAX_NAME_LEN-1, out msg);
            NewVideoSupplierDevice.UserName = user;

            if (!bRet)
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME,
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                NewVideoSupplierDevice.RaiseValidateFailEvent("UserName");
                return bRet;
            }
            string pass = NewVideoSupplierDevice.Password;
            bRet = Common.TextUtil.ValidateNameText(ref pass, false, "登录密码", 1, DataModel.Common.VDA_MAX_PWD_LEN-1, out msg);
            NewVideoSupplierDevice.Password = pass;

            if (!bRet)
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME,
                       MessageBoxButtons.OK, MessageBoxIcon.Warning);

                NewVideoSupplierDevice.RaiseValidateFailEvent("Password");
                return bRet;
            }
            return bRet;
        }

        private bool Validate()
        {
            bool bRet = CheckValue();
            return bRet;
        }


        #endregion

        #region Public helper functions

        private bool AddVideoSupplierDevice()
        {
            bool bRet = Validate();

            if (bRet)
            {
                uint ret = 0;
                try
                {
                    ret = IVX.Framework.Container.Instance.VDAConfigService.AddVideoSupplierDevice(NewVideoSupplierDevice);
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "添加网络存储设备");
                }
                bRet = ret > 0 ? true : false;

            }
            return bRet;
        }

        private bool EditVideoSupplierDevice()
        {
            bool bRet = Validate();

            if (bRet)
            {
                try
                {
                    bRet = IVX.Framework.Container.Instance.VDAConfigService.EditVideoSupplierDevice(NewVideoSupplierDevice);
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "修改网络存储设备");
                }
            }
            return bRet;
        }
        
        public bool Commit()
        {
            bool bRet = false;
            UpdateVideoSupplierDeviceName();
            if (IsEditMode)
            {
                bRet = EditVideoSupplierDevice();
            }
            else
            {
                bRet = AddVideoSupplierDevice();
            }
            return bRet;
        }

        public void UpdateVideoSupplierDeviceName()
        {
            AccessProtocolTypeInfo info = AccessProtocol.First(item => item.Type == NewVideoSupplierDevice.ProtocolType);
            NewVideoSupplierDevice.DeviceName = string.Format("{0}【{1}:{2}】", info.Name, NewVideoSupplierDevice.IP, NewVideoSupplierDevice.Port);

            RaisePropertyChangedEvent("DeviceName");
        }

        #endregion

    }
}
