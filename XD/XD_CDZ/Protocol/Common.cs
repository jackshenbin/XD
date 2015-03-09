using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BOCOM.IVX.Protocol
{

    internal partial class IVXSDKProtocol
    {
        #region 常量定义
        
        public static readonly int ERRORCODE_RECORDNOTEXIST = 0x10000 + 202;

#if DEBUG
        const string DLLPATH = @"lib\vdaclientsdk.dll";
        const string DRAWDLLPATH = @"lib\picoverlay.dll";
#else
        const string DLLPATH = @"lib\vdaclientsdk.dll";
        const string DRAWDLLPATH = @"lib\picoverlay.dll";
#endif

        #endregion
    }
}
