using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Protocol.Model
{
    /// <summary>
    /// 用户组信息
    /// </summary>
    [Serializable]
    public class UserGroupInfo
    {
        /// <summary>
        /// 所属组ID
        /// </summary>
        public UInt32 UserGroupID { get; set; }
        /// <summary>
        /// 用户组名
        /// </summary>
        public string UserGroupName { get; set; }
        /// <summary>
        /// 备注，描述信息
        /// </summary>
        public string UserGroupDescription { get; set; }
    };
}
