using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.DataModel;

namespace BOCOM.IVX.Protocol.Model
{
    public class BriefPlayParam
    {
        public E_VDA_BRIEF_DENSITY ObjDensity; //对象密度，见定义:E_VDA_BRIEF_DENSITY
        public E_VDA_MOVEOBJ_TYPE MoveObjType;	//运动目标类型，见E_VDA_MOVEOBJ_TYPE
        public uint MoveObjColor;	//运动目标颜色，见定义//???
    }
}
