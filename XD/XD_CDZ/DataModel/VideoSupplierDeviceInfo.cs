using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using DataModel;

namespace BOCOM.DataModel
{
    /// <summary>
    /// 网络存储设备信息
    /// </summary>
    [Serializable]
    public class VideoSupplierDeviceInfo : NotifyPropertyChangedModel
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// 接入厂商协议类型
        /// </summary>
        public E_VDA_NET_STORE_DEV_PROTOCOL_TYPE ProtocolType { get; set; }
        /// <summary>
        /// 连接IP地址
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 连接端口号
        /// </summary>
        public UInt32 Port { get; set; }
        /// <summary>
        /// 登录用户
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 网络存储设备ID
        /// </summary>
        public UInt32 Id { get; set; }

        public int LoginSessionId { get; set; }

        public object Clone()
        {
            VideoSupplierDeviceInfo newinfo = new VideoSupplierDeviceInfo()
            {
                DeviceName = this.DeviceName,
                ProtocolType = this.ProtocolType,
                IP = this.IP,
                Port = this.Port,
                UserName = this.UserName,
                Password = this.Password,
                Id = this.Id,
                LoginSessionId = this.LoginSessionId
            };
            return newinfo;
        }

        public override string ToString()
        {
            return this.DeviceName;
        }

    };

    public class VideoSupplierChannelInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ReservedDescription { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class VideoFileInfo
    {
        public string Id { get; set; }	//第3方设备/平台上的文件标识

        public DateTime StartTime { get; set; }					//文件录像起始时间

        public DateTime EndTime { get; set; }	    //文件录像结束时间

        public ulong Size { get; set; }						//文件大小

        public string ReservedDescription { get; set; }		//保留信息
    }

    public class AccessProtocolTypeInfo
    {
        public E_VDA_NET_STORE_DEV_PROTOCOL_TYPE Type { get; set; }

        public uint NType
        {
            get
            {
                return (uint)Type;
            }
        }

        public string Name { get; set; }

        public AccessProtocolTypeInfo(E_VDA_NET_STORE_DEV_PROTOCOL_TYPE type, string name)
        {
            Type = type;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }


}
