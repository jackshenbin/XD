using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.DataModel
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SearchResultPropertyAttribute : Attribute
    {
        private AvailableMode m_AvailableMode;

        /// <summary>
        /// 属性显示名
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 是否仅车辆结果属性
        /// </summary>
        public bool VehicleOnly { get; set; }

        public AvailableMode AvailableMode
        {
            get
            {
                return m_AvailableMode;
            }
            set
            {
                m_AvailableMode = value;
            }
        }

        public SearchResultPropertyAttribute()
        {
            //DisplayName = displayName;
            //VehicleOnly = vehicleOnly;
        }
    }

    public enum AvailableMode
    {
        All,
        Vehicle,
        NonVehicle
    }


    public class A
    {
        [SearchResultProperty(DisplayName="A", VehicleOnly=true)]
        public string KJK
        {
            get;
            set;
        }
    }
}
