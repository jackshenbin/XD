using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Protocol.Model
{
    public class LogResInfo
    {
        public int iIDOfLog;					//日志ID

        public string szLogTime;	//日志时间

        public LogBasicInfo lbiOfLog;		//日志基本信息

        public string szLogDscp;	//日志时间
    };
    public class  LogBasicInfo
    {
        public int iTypeOfLog;					//日志类型
        public int iLevelOfLog;				//日志级别
        public int iDetailOfLog;				//日志细分
        public string szUserName;
    }
    public class LogReqInfo
    {
        public int iIDOfLog;					//日志ID
        public int iPageNum;					//请求的页码

        public string szBeginTime;	//日志开始时间

        public string szEndTime;	//日志结束时间

        public LogBasicInfo lbiOfLog;		//日志基本信息

        public string szSortName;	//排序名

        public int iTypeOfSort;				//排序类型
    };

    public class LogResInfoSum
    {
        public int iPageCount;					//日志页码数
        public int iPageNum;					//日志当前页码
        public int iRecords;					//日志记录总数
        public int iPageRecord;				//日志当前页的记录数
    }

}
