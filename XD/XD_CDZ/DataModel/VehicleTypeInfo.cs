using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.DataModel;

namespace DataModel
{

    public class ModelInfoBase
    {
        public virtual string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class VehicleTypeInfo : ModelInfoBase
    {
        public VehicleType Type { get; set; }

    }

    public class SearchResultObjectTypeInfo : ModelInfoBase
    {
        public SearchResultObjectType Type { get; set; }
    }

    public class VehicleDetailTypeInfo : ModelInfoBase
    {
        public VehicleDetailType Type { get; set; }
    }

    public class VehiclePlateTypeInfo : ModelInfoBase
    {
        public VehiclePlateType Type { get; set; }
    }
}
