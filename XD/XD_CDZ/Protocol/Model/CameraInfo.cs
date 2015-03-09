using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BOCOM.IVX.Protocol.Model
{
    /// <summary>
    /// 监控点位信息 
    /// </summary>
    [Serializable]
    public class CameraInfo : ICloneable
    {
        /// <summary>
        /// 所属组ID
        /// </summary>
        public uint GroupID { get; set; }

        /// <summary>
        /// 监控点ID
        /// </summary>
        public uint CameraID { get; set; }

        /// <summary>
        /// 监控点名
        /// </summary>
        public string CameraName { get; set; }

        /// <summary>
        /// 关联的网络设备ID
        /// </summary>
        public uint NetStoreDevID { get; set; }

        /// <summary>
        /// 存储设备点位标示
        /// </summary>
        public string NetStoreDevCameraID { get; set; }

        /// <summary>
        /// 地理位置坐标	X
        /// </summary>
        public float PosCoordX { get; set; }

        /// <summary>
        /// 地理位置坐标	Y
        /// </summary>
        public float PosCoordY { get; set; }

        public object Clone()
        {
            CameraInfo newCamera = new CameraInfo()
            {
                CameraID = CameraID,
                CameraName = CameraName,
                GroupID = GroupID,
                NetStoreDevCameraID = NetStoreDevCameraID,
                NetStoreDevID = NetStoreDevID,
                PosCoordX = PosCoordX,
                PosCoordY = PosCoordY,
            };

            return newCamera;
        }
    };
}
