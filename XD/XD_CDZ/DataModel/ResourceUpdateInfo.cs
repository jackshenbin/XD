using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DataModel;

namespace BOCOM.DataModel
{
    /// <summary>
    /// 资源更新信息 
    /// </summary>
    [Serializable]
    public class ResourceUpdateInfo 
    {
        /// <summary>
        /// 执行资源操作者名称
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 执行的资源操作类型
        /// </summary>
        public E_VDA_RESOURCE_OPERATE_TYPE OperateType { get; set; }
        /// <summary>
        /// 被执行的资源类型
        /// </summary>
        public E_VDA_RESOURCE_TYPE ResourceType { get; set; }
        /// <summary>
        /// 资源ID列表
        /// </summary>
        public List<UInt32> ResourceIDList { get; set; }
        /// <summary>
        /// 资源数量
        /// </summary>
        public UInt32 ResourceNum { get; set; }

        public UInt32 UserData { get; set; }
    };
}
