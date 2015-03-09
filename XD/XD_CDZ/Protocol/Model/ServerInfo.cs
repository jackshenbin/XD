using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace BOCOM.IVX.Protocol.Model
{
    /// <summary>
    /// 中心服务器单元基本信息
    /// </summary>
    [Serializable]
    public class ServerInfo : ICloneable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string m_IP;

        /// <summary>
        /// 服务器ID
        /// </summary>
        public UInt32 ServerID { get; set; }
        /// <summary>
        /// 服务器类型，见vdacomm.h中E_VDA_SERVER_TYPE定义
        /// </summary>
        public UInt32 Type { get; set; }
        /// <summary>
        /// 服务器IP地址
        /// </summary>
        public string IpAddr
        {
            get
            {
                return m_IP;
            }
            set
            {
                m_IP = value;
            }
        }
        /// <summary>
        /// 服务器端口号
        /// </summary>
        public UInt16 Port { get; set; }

        public object Clone()
        {
            ServerInfo newServer = new ServerInfo()
            {
                ServerID = this.ServerID,
                IpAddr = this.IpAddr,
                Port = this.Port,
                Type = this.Type
            };
            return newServer;
        }
    };
}
