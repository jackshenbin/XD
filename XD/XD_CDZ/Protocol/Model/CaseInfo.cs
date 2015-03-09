using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace BOCOM.IVX.Protocol.Model
{

    /// <summary>
    /// 案件信息
    /// </summary>
    [Serializable]
    public class CaseInfo : ICloneable, INotifyPropertyChanged
    {
        private string m_caseName = "";
        /// <summary>
        /// 案件ID
        /// </summary>
        public UInt32 CaseID { get; set; }
        /// <summary>
        /// 案件名，必填字段
        /// </summary>
        public string CaseName { get { return m_caseName; } set { m_caseName = value; } }
        /// <summary>
        /// 案件编号，公安业务统一编号
        /// </summary>
        public string CaseNo { get; set; }
        /// <summary>
        /// 案子发生时间
        /// </summary>
        public DateTime CaseHappenTime { get; set; }
        /// <summary>
        /// 案子发生地点
        /// </summary>
        public string CaseHappenAddr { get; set; }
        /// <summary>
        /// 备注，描述信息
        /// </summary>
        public string CaseDescription { get; set; }

        public UInt32 CaseType { get; set; }

        public UInt32 UserGroupId { get; set; }

        public object Clone()
        {
            CaseInfo newCase = new CaseInfo()
            {
                CaseDescription = this.CaseDescription,
                CaseHappenAddr = this.CaseHappenAddr,
                CaseHappenTime = this.CaseHappenTime,
                CaseID = this.CaseID,
                CaseName = this.CaseName,
                CaseNo = this.CaseNo,
                CaseType = this.CaseType,
                UserGroupId = this.UserGroupId
            };
            return newCase;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    };
    
}
