using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel;

namespace BOCOM.DataModel
{
    /// <summary>
    /// 用户组信息
    /// </summary>
    [Serializable]
    public class UserGroupInfo : NotifyPropertyChangedModel
    {
        private string m_Name;
        /// <summary>
        /// 所属组ID
        /// </summary>
        public UInt32 UserGroupID { get; set; }
        /// <summary>
        /// 用户组名
        /// </summary>
        public string UserGroupName
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }
        /// <summary>
        /// 备注，描述信息
        /// </summary>
        public string UserGroupDescription { get; set; }
    };
}
