using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Protocol.Model
{
    /// <summary>
    /// 监控点位分组基本信息
    /// </summary>
    [Serializable]
    public class CameraGroupInfo
    {
        /// <summary>
        /// 组ID
        /// </summary>
        public UInt32 CameraGroupID { get; set; }
        /// <summary>
        /// 所属上级组ID
        /// </summary>
        public UInt32 ParentGroupID { get; set; }
        /// <summary>
        /// 组名
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 备注，描述信息
        /// </summary>
        public string GroupDescription { get; set; }

        public object Clone()
        {
            CameraGroupInfo newGroup = new CameraGroupInfo()
            {
                CameraGroupID = CameraGroupID,
                GroupName = GroupName,
                ParentGroupID = ParentGroupID,
                GroupDescription = GroupDescription,
            };

            return newGroup;
        }

    };

}
