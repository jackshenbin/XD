using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using BOCOM.IVX.Protocol.Model;
using BOCOM.DataModel;
using System.Collections.Generic;

namespace BOCOM.IVX.Protocol
{
    #region 基本结构体


    /// <summary>
    /// 播放绘图相关接口 绘制的越界线信息
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_DRAW_PASSLINE
    {
        /// <summary>
        /// 越界线列表
        /// </summary>
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = Common.VDA_PASSLINE_MAXNUM)]
        public TVDASDK_IA_SEARCH_PASS_LINE[] atPassLineList;

        /// <summary>
        /// 越界线数量
        /// </summary>
        public UInt32 dwPassLineNum;
    };

    /// <summary>
    /// 绘制的闯入闯出区域信息
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TVDASDK_DRAW_BREAK_REGION
    {
        /// <summary>
        /// 闯入闯出区域
        /// </summary>
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = Common.VDA_BREAK_REGION_MAXNUM)]
        public TVDASDK_SEARCH_BREAK_REGION[] atBreakRegionList;

        /// <summary>
        /// 闯入闯出区域数量
        /// </summary>
        public UInt32 dwBreakRegionNum;
    };



    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TPDO_RECT
    {
        public UInt32 dwX;
        public UInt32 dwY;
        public UInt32 dwWidth;
        public UInt32 dwHeight;
    };

    //绘制的矩形信息
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TPDO_DRAW_RECT
    {
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = Common.PDO_DRAW_RECT_MAXNUM)]
        public TPDO_RECT[] atRectList;	//绘制的矩形列表
        public UInt32 dwRectNum;		//矩形数量
    };

    //文件路径
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    internal struct TPDO_FILE_PATH
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Common.PDO_FILEPATH_MAXLEN)]
        public string szFilePath; //文件路径
    };
    #endregion

    #region 回调定义

    /*===========================================================
    功  能：播放窗口鼠标动作通知回调函数
    参  数：lVodHandle - 播放标示句柄
		    dwOptType - 下载状态，见E_VDA_PLAY_WND_MOUSE_OPT_TYPE
		    dwX - 鼠标所在x轴窗口坐标
		    dwY - 鼠标所在y轴窗口坐标
		    dwUserData - 用户数据
    返回值：无
    ===========================================================*/
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    internal delegate void TfuncPlayWndMouseOptNtfCB(Int32 lVodHandle, UInt32 dwOptType, UInt32 dwX, UInt32 dwY, UInt32 dwUserData);

    //鼠标事件回调函数
    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    internal delegate void TFuncPDOMouseOptNtfCB(Int32 hPdoHandle, E_PDO_MOUSE_EVENT eMouseEvent,
                                                UInt32 dwX, UInt32 dwY, UInt32 dwUserData);

    #endregion

    internal partial class IVXSDKProtocol
    {
        #region 绘图相关


        /*===========================================================
        功  能：设置播放绘图类型（如画越界线，画闯入闯出区域，切换绘图类型时，会自动清除前面绘制的内容)
        参  数：lVodHandle - 播放标示句柄
		        dwDrawType - 搜索行为过滤类型 见E_VDA_SEARCH_MOVE_OBJ_RANGE_FILTER_TYPE
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_SetPlayDrawType(Int32 lVodHandle, UInt32 dwDrawType);

        //清除播放绘制内容
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_ClearPlayDraw(Int32 lVodHandle);

        /*===========================================================
        功  能：获取播放绘制的越界线信息
        参  数：lVodHandle - 播放标示句柄
                ptPlayPassline - 绘制的越界线信息
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_GetPlayDrawPassline(Int32 lVodHandle, out TVDASDK_DRAW_PASSLINE ptDrawPassline);

        /*===========================================================
        功  能：获取播放绘制的闯入闯出区域信息
        参  数：lVodHandle - 播放标示句柄
                ptDrawBreakRegion - 绘制的闯入闯出区域信息
        返回值：成功返回TRUE，失败返回FALSE
        ===========================================================*/
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_GetPlayDrawBreakRegion(Int32 lVodHandle, out TVDASDK_DRAW_BREAK_REGION ptDrawBreakRegion);

        //设置播放窗口鼠标动作通知回调参数
        [DllImport(DLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern bool VdaSdk_SetPlayWndMouseOptNtfCB(Int32 lVodHandle, TfuncPlayWndMouseOptNtfCB pfuncMouseOptNtf, UInt32 dwUserData);




        /*=================================================================
        功  能：开启显示图片叠加控制
        参  数：hWnd：显示图片的窗口句柄
                ptInitParam:初始化函数参数
                phPdoHandle:返回操作标示句柄
        返回值：成功返回PDO_OK，失败返回错误码
        =================================================================*/
        [DllImport(DRAWDLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 Pdo_Open(IntPtr hWnd, TFuncPDOMouseOptNtfCB pfuncMouseEventCb,
                                            UInt32 dwUserData, out UInt32 phPdoHandle);

        /*=================================================================
        功  能：关闭显示图片叠加控制
        参  数：hPdoHandle：标示句柄
        返回值：成功返回PDO_OK，失败返回错误码
        =================================================================*/
        [DllImport(DRAWDLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 Pdo_Close(UInt32 hPdoHandle);

        /*=================================================================
        功  能：设置要显示的本地图片文件
        参  数：hPdoHandle：标示句柄
                tPicFilePath：图片文件路径
                dwPicType：图片格式类型，见E_PDO_PIC_TYPE		
        返回值：成功返回PDO_OK，失败返回错误码
        =================================================================*/
        [DllImport(DRAWDLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 Pdo_DisplayPicFileSet(UInt32 hPdoHandle, TPDO_FILE_PATH tPicFilePath);

        /*=================================================================
        功  能：设置要显示的本地图片文件
        参  数：hPdoHandle：标示句柄
                pPicData：图片数据
                dwPicDataSize：图片数据大小
                dwPicType：图片格式类型，见E_PDO_PIC_TYPE		
        返回值：成功返回PDO_OK，失败返回错误码
        =================================================================*/
        [DllImport(DRAWDLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 Pdo_DisplayPicDataSet(UInt32 hPdoHandle, IntPtr pPicData,
                                                          UInt32 dwPicDataSize, UInt32 dwPicType);

        /*===========================================================
        功  能：设置绘图类型（如画线，画矩形等内容)
        参  数：hPdoHandle - 标示句柄
                dwDrawType - 搜索行为过滤类型 见E_PDO_DRAW_TYPE
        返回值：成功返回PDO_OK，失败返回错误码
        ===========================================================*/
        [DllImport(DRAWDLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 Pdo_DrawTypeSet(UInt32 hPdoHandle, UInt32 dwDrawType);

        /*===========================================================
        功  能：清除播放绘制内容
        参  数：hPdoHandle - 标示句柄
        返回值：成功返回PDO_OK，失败返回错误码
        ===========================================================*/
        [DllImport(DRAWDLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 Pdo_DrawClear(UInt32 hPdoHandle);

        /*===========================================================
        功  能：获取绘制的矩形信息
        参  数：hPdoHandle - 标示句柄
                ptDrawRect - 绘制的矩形信息
        返回值：成功返回PDO_OK，失败返回错误码
        ===========================================================*/
        [DllImport(DRAWDLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 Pdo_DrawRectGet(UInt32 hPdoHandle, out TPDO_DRAW_RECT ptDrawRect);
        /*===========================================================
        功  能：设置绘制的矩形信息
        参  数：hPdoHandle - 标示句柄
		        ptDrawRect - 绘制的矩形信息
        返回值：成功返回PDO_OK，失败返回错误码
        ===========================================================*/
        [DllImport(DRAWDLLPATH, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 Pdo_DrawRectSet(UInt32 hPdoHandle, ref TPDO_DRAW_RECT ptDrawRect);
        #endregion

    }


    public partial class IVXProtocol
    {


        #region 绘图相关


        /// <summary>
        /// 设置播放绘图类型（如画越界线，画闯入闯出区域，切换绘图类型时，会自动清除前面绘制的内容)
        /// </summary>
        /// <param name="vodHandle"></param>
        /// <param name="drawType">搜索行为过滤类型 见E_VDA_SEARCH_MOVE_OBJ_RANGE_FILTER_TYPE</param>
        /// <returns>成功返回TRUE，失败返回FALSE</returns>
        public bool SetPlayDrawType(Int32 vodHandle, E_VDA_SEARCH_MOVE_OBJ_RANGE_FILTER_TYPE drawType)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_SetPlayDrawType lVodHandle:" + vodHandle + ",dwDrawType:" + drawType);
            bool retVal = IVXSDKProtocol.VdaSdk_SetPlayDrawType(vodHandle, (uint)drawType);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_SetPlayDrawType ret:" + retVal);
            return retVal;

        }

        /// <summary>
        /// 清除播放绘制内容
        /// </summary>
        /// <param name="vodHandle">播放标示句柄</param>
        /// <returns></returns>
        public bool ClearPlayDraw(Int32 vodHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_ClearPlayDraw lVodHandle:" + vodHandle);
            bool retVal = IVXSDKProtocol.VdaSdk_ClearPlayDraw(vodHandle);
            //if (!retVal)
            //{
            //    // 调用失败，抛异常
            //    CheckError();
            //}
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_ClearPlayDraw ret:" + retVal);
            return retVal;
        }


        /// <summary>
        /// 获取播放绘制的越界线信息
        /// </summary>
        /// <param name="vodHandle">播放标示句柄</param>
        /// <returns>绘制的越界线信息</returns>
        public List<PassLine> GetPlayDrawPassline(Int32 vodHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_GetPlayDrawPassline lVodHandle:" + vodHandle);
            TVDASDK_DRAW_PASSLINE passline = new TVDASDK_DRAW_PASSLINE();
            bool retVal = IVXSDKProtocol.VdaSdk_GetPlayDrawPassline(vodHandle, out passline);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_GetPlayDrawPassline ret:{0},"
                , retVal));
            List<PassLine> ret = new List<PassLine>();
            for (int i = 0; i < passline.dwPassLineNum; i++)
            {
                TVDASDK_IA_SEARCH_PASS_LINE line = passline.atPassLineList[i];
                ret.Add(new PassLine
                {
                    PassLineType = line.dwPassLineType,
                    PassLineStart = new System.Drawing.Point((int)line.tPassLine.tStartPt.dwX, (int)line.tPassLine.tStartPt.dwY),
                    PassLineEnd = new System.Drawing.Point((int)line.tPassLine.tEndPt.dwX, (int)line.tPassLine.tEndPt.dwY),
                    DirectLineStart = new System.Drawing.Point((int)line.tDirectLine.tStartPt.dwX, (int)line.tDirectLine.tStartPt.dwY),
                    DirectLineEnd = new System.Drawing.Point((int)line.tDirectLine.tEndPt.dwX, (int)line.tDirectLine.tEndPt.dwY),
                });
            }
            return ret;
        }


        /// <summary>
        /// 获取播放绘制的闯入闯出区域信息
        /// </summary>
        /// <param name="vodHandle">播放标示句柄</param>
        /// <returns>绘制的闯入闯出区域信息</returns>
        public List<BreakRegion> GetPlayDrawBreakRegion(Int32 vodHandle)
        {

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_GetPlayDrawBreakRegion lVodHandle:" + vodHandle);
            TVDASDK_DRAW_BREAK_REGION ptDrawBreakRegion = new TVDASDK_DRAW_BREAK_REGION();
            bool retVal = IVXSDKProtocol.VdaSdk_GetPlayDrawBreakRegion(vodHandle, out ptDrawBreakRegion);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,string.Format("IVXSDKProtocol VdaSdk_GetPlayDrawBreakRegion ret:{0},"
                , retVal));
            List<BreakRegion> ret = new List<BreakRegion>();
            for (int i = 0; i < ptDrawBreakRegion.dwBreakRegionNum; i++)
            {
                TVDASDK_SEARCH_BREAK_REGION region = ptDrawBreakRegion.atBreakRegionList[i];
                BreakRegion newregion = new BreakRegion();
                newregion.RegionPointList = new List<System.Drawing.Point>();
                newregion.RegionType = region.dwRegionType;
                for (int j = 0; j < region.dwPointNum; j++)
                {
                    newregion.RegionPointList.Add(
                        new System.Drawing.Point((int)region.atRegionPointList[j].dwX, (int)region.atRegionPointList[j].dwY)
                        );
                }
                ret.Add(newregion);
            }
            return ret;

        }

        /// <summary>
        /// 设置播放窗口鼠标动作通知回调参数
        /// </summary>
        /// <param name="lVodHandle"></param>
        /// <param name="dwUserData"></param>
        /// <returns></returns>
        public bool SetPlayWndMouseOptNtfCB(Int32 lVodHandle, UInt32 dwUserData)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_SetPlayWndMouseOptNtfCB lVodHandle:" + lVodHandle);
            m_pfuncMouseOptNtf = OnPlayWndMouseOptNtfCB;
            bool retVal = IVXSDKProtocol.VdaSdk_SetPlayWndMouseOptNtfCB(lVodHandle, OnPlayWndMouseOptNtfCB, dwUserData);
            if (!retVal)
            {
                // 调用失败，抛异常
                CheckError();
            }
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol VdaSdk_SetPlayWndMouseOptNtfCB ret:" + retVal);
            return retVal;
        }





        /// <summary>
        /// 开启显示图片叠加控制
        /// </summary>
        /// <param name="hWnd">显示图片的窗口句柄</param>
        /// <param name="dwUserData"></param>
        /// <returns>返回操作标示句柄</returns>
        public UInt32 Pdo_Open(IntPtr hWnd, UInt32 dwUserData)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol Pdo_Open hWnd:" + hWnd);
            m_pfuncMouseEventCb = OnPDOMouseOptNtfCB;
            UInt32 phPdoHandle = 0;
            UInt32 retVal = IVXSDKProtocol.Pdo_Open(hWnd, null, dwUserData, out phPdoHandle);
            //if (0 != retVal)
            //{
            //    // 调用失败，抛异常
            //    CheckError();
            //}
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol Pdo_Open ret:" + retVal);
            return phPdoHandle;
        }


        /// <summary>
        /// 关闭显示图片叠加控制
        /// </summary>
        /// <param name="hPdoHandle">标示句柄</param>
        /// <returns>成功返回PDO_OK，失败返回错误码</returns>
        public UInt32 Pdo_Close(UInt32 hPdoHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol Pdo_Close hPdoHandle:" + hPdoHandle);
            UInt32 retVal = IVXSDKProtocol.Pdo_Close(hPdoHandle);
            //if (0 != retVal)
            //{
            //    // 调用失败，抛异常
            //    CheckError();
            //}
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol Pdo_Close ret:" + retVal);
            return retVal;

        }


        /// <summary>
        /// 设置要显示的本地图片文件
        /// </summary>
        /// <param name="hPdoHandle">标示句柄</param>
        /// <param name="picFilePath">图片文件路径</param>
        /// <returns>成功返回PDO_OK，失败返回错误码</returns>
        public UInt32 Pdo_DisplayPicFileSet(UInt32 hPdoHandle, string picFilePath)
        {
            TPDO_FILE_PATH tPicFilePath = new TPDO_FILE_PATH();
            tPicFilePath.szFilePath = picFilePath;
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol Pdo_DisplayPicFileSet hPdoHandle:" + hPdoHandle + ",PicFilePath:" + picFilePath);
            UInt32 retVal = IVXSDKProtocol.Pdo_DisplayPicFileSet(hPdoHandle, tPicFilePath);
            //if (0 != retVal)
            //{
            //    // 调用失败，抛异常
            //    CheckError();
            //}
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol Pdo_DisplayPicFileSet ret:" + retVal);
            return retVal;

        }


        /// <summary>
        /// 设置要显示的本地图片文件
        /// </summary>
        /// <param name="hPdoHandle">标示句柄</param>
        /// <param name="image">图片数据</param>
        /// <returns>成功返回PDO_OK，失败返回错误码</returns>
        public UInt32 Pdo_DisplayPicDataSet(UInt32 hPdoHandle, System.Drawing.Image image)
        {
            IntPtr pPicData = IntPtr.Zero;
            UInt32 dwPicDataSize = 0;
            if (image != null)
            {
                byte[] bytes = Model.ModelParser.ImageToJpegBytes(image);

                dwPicDataSize = (uint)bytes.Length;
                pPicData = Marshal.AllocHGlobal(bytes.Length);

                Marshal.Copy(bytes, 0, pPicData, bytes.Length);

            }
            UInt32 dwPicType = (uint)E_PDO_PIC_TYPE.E_PDO_PIC_JPG;//图片格式类型，见E_PDO_PIC_TYPE

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol Pdo_DisplayPicDataSet hPdoHandle:" + hPdoHandle);
            UInt32 retVal = IVXSDKProtocol.Pdo_DisplayPicDataSet(hPdoHandle, pPicData, dwPicDataSize, dwPicType);
            //if (0 != retVal)
            //{
            //    // 调用失败，抛异常
            //    CheckError();
            //}
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol Pdo_DisplayPicDataSet ret:" + retVal);
            
            if (pPicData!=IntPtr.Zero)
                Marshal.FreeHGlobal(pPicData);

            return retVal;
        }


        /// <summary>
        /// 设置绘图类型（如画线，画矩形等内容)
        /// </summary>
        /// <param name="hPdoHandle">标示句柄</param>
        /// <param name="dwDrawType">搜索行为过滤类型 见E_PDO_DRAW_TYPE</param>
        /// <returns>成功返回PDO_OK，失败返回错误码</returns>
        public UInt32 Pdo_DrawTypeSet(UInt32 hPdoHandle, E_PDO_DRAW_TYPE dwDrawType)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol Pdo_DrawTypeSet hPdoHandle:" + hPdoHandle + ",dwDrawType:" + dwDrawType);
            UInt32 retVal = IVXSDKProtocol.Pdo_DrawTypeSet(hPdoHandle, (uint)dwDrawType);
            //if (0 != retVal)
            //{
            //    // 调用失败，抛异常
            //    CheckError();
            //}
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol Pdo_DrawTypeSet ret:" + retVal);
            return retVal;

        }


        /// <summary>
        /// 清除播放绘制内容
        /// </summary>
        /// <param name="hPdoHandle">标示句柄</param>
        /// <returns>成功返回PDO_OK，失败返回错误码</returns>
        public UInt32 Pdo_DrawClear(UInt32 hPdoHandle)
        {
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol Pdo_DrawClear hPdoHandle:" + hPdoHandle);
            UInt32 retVal = IVXSDKProtocol.Pdo_DrawClear(hPdoHandle);
            //if (0 != retVal)
            //{
            //    // 调用失败，抛异常
            //    CheckError();
            //}
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol Pdo_DrawClear ret:" + retVal);
            return retVal;
        }


        /// <summary>
        /// 获取绘制的矩形信息
        /// </summary>
        /// <param name="hPdoHandle">标示句柄</param>
        /// <returns>绘制的矩形信息</returns>
        public List<System.Drawing.Rectangle> Pdo_DrawRectGet(UInt32 hPdoHandle)
        {
            TPDO_DRAW_RECT ptDrawRect = new TPDO_DRAW_RECT();

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol Pdo_DrawRectGet hPdoHandle:" + hPdoHandle);
            UInt32 retVal = IVXSDKProtocol.Pdo_DrawRectGet(hPdoHandle, out ptDrawRect);
            //if (0 != retVal)
            //{
            //    // 调用失败，抛异常
            //    CheckError();
            //}
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log,"IVXSDKProtocol Pdo_DrawRectGet ret:" + retVal);
            List<System.Drawing.Rectangle> ret = new List<System.Drawing.Rectangle>();
            for (int i = 0; i < ptDrawRect.dwRectNum; i++)
            {
                ret.Add(
                    new System.Drawing.Rectangle((int)ptDrawRect.atRectList[i].dwX, (int)ptDrawRect.atRectList[i].dwY, (int)ptDrawRect.atRectList[i].dwWidth, (int)ptDrawRect.atRectList[i].dwHeight)
                    );
            }
            return ret;
        }

        public UInt32 Pdo_DrawRectSet(UInt32 hPdoHandle, List<System.Drawing.Rectangle> rects)
        {
            TPDO_DRAW_RECT ptDrawRect = new TPDO_DRAW_RECT();
            ptDrawRect.dwRectNum = (uint)rects.Count;
            ptDrawRect.atRectList = new TPDO_RECT[Common.PDO_DRAW_RECT_MAXNUM];

            for (int i = 0; i < ptDrawRect.dwRectNum; i++)
            {
                ptDrawRect.atRectList[i] = new TPDO_RECT { dwHeight = (uint)rects[i].Height, dwWidth = (uint)rects[i].Width, dwX = (uint)rects[i].X, dwY = (uint)rects[i].Y };
            }

            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol Pdo_DrawRectSet hPdoHandle:" + hPdoHandle);
            UInt32 retVal = IVXSDKProtocol.Pdo_DrawRectSet(hPdoHandle, ref ptDrawRect);
            //if (0 != retVal)
            //{
            //    // 调用失败，抛异常
            //    CheckError();
            //}
            MyLog4Net.ILogExtension.DebugWithDebugView(MyLog4Net.Container.Instance.Log, "IVXSDKProtocol Pdo_DrawRectSet ret:" + retVal);

            return retVal;
        }

        #endregion
    }
}
