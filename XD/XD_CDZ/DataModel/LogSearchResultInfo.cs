using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.DataModel
{
    public class LogSearchResultInfo
    {
        public UInt32 LogId;                                 //日志查询流水号(大于等于1)
        public E_VDA_LOG_LEVEL Level;			                          //日志级别：见E_VDA_LOG_LEVEL
        public E_VDA_LOG_DETAIL LogDetail;                                //日志细分:见E_VDA_LOG_DETAIL
        public E_VDA_LOG_TYPE LogType;								  //日志类型，见E_VDA_LOG_TYPE
        public string Description;//日志描述
        public E_LOG_OPERATE_TYPE OptType;                                  //操作者类型，见E_LOG_OPERATE_TYPE
        public UInt32 OptId;			                          //关联到服务器ID
        public string OptName;			      //操作者名称
        public string LogTime;                   //日志发生时间

    }


    public class LogTypeInfo
    {
        public E_VDA_LOG_TYPE Type { get; set; }

        public uint NType
        {
            get
            {
                return (uint)Type;
            }
        }

        public string Name { get; set; }

        public LogTypeInfo(E_VDA_LOG_TYPE type, string name)
        {
            Type = type;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class LogLevelInfo
    {
        public E_VDA_LOG_LEVEL Type { get; set; }

        public uint NType
        {
            get
            {
                return (uint)Type;
            }
        }

        public string Name { get; set; }

        public LogLevelInfo(E_VDA_LOG_LEVEL type, string name)
        {
            Type = type;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class LogDetailTypeInfo
    {
        public E_VDA_LOG_DETAIL Type { get; set; }

        public uint NType
        {
            get
            {
                return (uint)Type;
            }
        }

        public string Name { get; set; }

        public LogDetailTypeInfo(E_VDA_LOG_DETAIL type, string name)
        {
            Type = type;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }


}
