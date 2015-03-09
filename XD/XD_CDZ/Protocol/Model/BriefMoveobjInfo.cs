using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.DataModel;

namespace BOCOM.IVX.Protocol.Model
{
    public class BriefMoveobjInfo
    {
        public uint MoveObjID;		//运动目标标示
        public E_VDA_MOVEOBJ_TYPE MoveObjType;	//运动目标类型，见E_VDA_MOVEOBJ_TYPE
        public uint MoveObjColor;	//运动目标颜色，见定义//???

        public DateTime BeginTimeS;	//目标出现的时间点（绝度时间，秒）
        public DateTime EndTimeS;	//目标结束的时间点（绝度时间，秒）
    }
}
