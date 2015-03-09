using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.DataModel
{
    public class LogSearchParam
    {
        public UInt32 LogId; //日志查询流水号(大于等于0)
        public UInt32 LogCount; //本次日志检索条数(0为查询全部，建议值为：100)
        public DateTime BeginTime;//开始时间(0则忽略)
        public DateTime EndTime;  //结束时间(0则忽略)
        public E_VDA_LOG_TYPE LogType; //日志类型，见E_VDA_LOG_TYPE
        public E_VDA_LOG_LEVEL LogLevel; //日志级别，见E_VDA_LOG_LEVEL
        public E_VDA_LOG_SORT_TYPE SortKind; //排序类型，见E_VDA_LOG_SORT_TYPE
        public E_LOG_OPERATE_TYPE OptType;  //操作者类型，见E_LOG_OPERATE_TYPE
        public UInt32 OptId;	  //关联到服务器ID
    }
}
