using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BOCOM.IVX.COMLibrary
{
    [Guid("7302FA7E-B19A-40BF-B0C1-622420B9C705"),
    InterfaceType(ComInterfaceType.InterfaceIsDual),
    ComVisible(true)]
    public interface iXDInterface
    {
        #region 基础操作
        /// <summary>
        /// 控件初始化。
        /// </summary>
        /// <param name="serverIp">服务器地址</param>
        /// <param name="userName">用户姓名</param>
        /// <param name="password">用户密码</param>
        /// <returns>xml返回
        /// <Ret>
        /// <RetMsg><ErrorCode></ErrorCode><Description></Description></RetMsg>
        /// <RetInfo></RetInfo>
        /// </Ret>
        /// 
        /// </returns>
        string VdaInitialization(string serverIp, string userName, string password);



        /// <summary>
        /// 控件反初始化
        /// </summary>
        /// <returns>xml返回
        /// <Ret>
        /// <RetMsg><ErrorCode></ErrorCode><Description></Description></RetMsg>
        /// <RetInfo></RetInfo>
        /// </Ret>
        /// 
        /// </returns>
        string VdaUninitialization();

        /// <summary>
        /// 页面切换
        /// </summary>
        /// <param name="formType">页面类型见E_SWITCH_WND </param>
        /// <returns>xml返回
        /// <Ret>
        /// <RetMsg><ErrorCode></ErrorCode><Description></Description></RetMsg>
        /// <RetInfo>版本号</RetInfo>
        /// </Ret>
        /// 
        /// </returns>
        string VdaSwitchForm(int formType);

        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <returns>xml返回
        /// <Ret>
        /// <RetMsg><ErrorCode></ErrorCode><Description></Description></RetMsg>
        /// <RetInfo></RetInfo>
        /// </Ret>
        /// 
        /// </returns>
        string VdaGetVersion();
        #endregion
        
    }



    [ComVisible(true)]
    [Guid("BB5FC768-5D87-4694-8E39-C8C6C3B538A1")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface XDEvents
    {

    }

    /// <summary>
    /// IObjectSafety接口.net定义
    /// </summary>
    [ComImport, GuidAttribute("CB5BDC81-93C1-11CF-8F20-00805F2CD064")]//uuid
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]//继承了IUnknown
    public interface IObjectSafety
    {
        [PreserveSig]
        int GetInterfaceSafetyOptions(
            ref Guid riid,
            [MarshalAs(UnmanagedType.U4)] ref int pdwSupportedOptions,
            [MarshalAs(UnmanagedType.U4)] ref int pdwEnabledOptions);

        [PreserveSig()]
        int SetInterfaceSafetyOptions(
            ref Guid riid,
            [MarshalAs(UnmanagedType.U4)] int dwOptionSetMask,
            [MarshalAs(UnmanagedType.U4)] int dwEnabledOptions);
    }


}

