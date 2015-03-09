using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using BOCOM.IVX.Protocol;
using System.IO;
using System.Drawing.Imaging;
using BOCOM.DataModel;
using System.Diagnostics;
using DataModel;
// using BOCOM.IVX.Protocol.IVXSDKProtocol;


namespace BOCOM.IVX.Protocol.Model
{
    public static class ModelParser
    {

        public static Image GetImage(IntPtr startAddress, int byteSize)
        {
            Image img = null;
            byte[] bytes = new byte[byteSize];
            IntPtr ptr = startAddress;
            Marshal.Copy(ptr, bytes, 0, byteSize);

            try
            {
                MemoryStream ms = new MemoryStream(bytes);
                Image imgTmp = Image.FromStream(ms);
                // 新创建一张Image， 从imgTmp构造， 因为用工具.NETMemoryProfiler 看到有时 bytes不能被回收
                img = new Bitmap(imgTmp);

                imgTmp.Dispose();
                ms.Dispose();
            }
            catch (Exception aex)
            {
                Debug.Assert(false, "Image.FromStream failed");
                img = null;
            }
            return img;
        }

        internal static TVDASDK_CASE_BASE Convert(CaseInfo caseInfo)
        {
            TVDASDK_CASE_BASE tCaseBase = new TVDASDK_CASE_BASE();

            tCaseBase.dwCaseHappenTime = ConvertLinuxTime(caseInfo.CaseHappenTime);
            tCaseBase.szCaseDescription = caseInfo.CaseDescription;
            tCaseBase.szCaseHappenAddr = caseInfo.CaseHappenAddr;
            tCaseBase.szCaseName = caseInfo.CaseName;
            tCaseBase.szCaseNo = caseInfo.CaseNo;
            tCaseBase.dwUserGroupID = caseInfo.UserGroupId;
            tCaseBase.dwCaseType = caseInfo.CaseType;

            return tCaseBase;
        }

        internal static CaseInfo Convert(TVDASDK_CASE_INFO ptCaseInfo)
        {
            CaseInfo caseInfo = new CaseInfo();

            caseInfo.CaseID = ptCaseInfo.dwCaseID;
            caseInfo.CaseHappenAddr = ptCaseInfo.tGroupBase.szCaseHappenAddr;
            caseInfo.CaseHappenTime = ModelParser.ConvertLinuxTime(ptCaseInfo.tGroupBase.dwCaseHappenTime);
            caseInfo.CaseDescription = ptCaseInfo.tGroupBase.szCaseDescription;
            caseInfo.CaseName = ptCaseInfo.tGroupBase.szCaseName;
            caseInfo.CaseNo = ptCaseInfo.tGroupBase.szCaseNo;
            caseInfo.CaseType = ptCaseInfo.tGroupBase.dwCaseType;
            caseInfo.UserGroupId = ptCaseInfo.tGroupBase.dwUserGroupID;

            return caseInfo;
        }

        internal static CameraInfo Convert(TVDASDK_CAMERA_INFO ptCameraInfo)
        {
            CameraInfo cameraInfo = new CameraInfo();

            cameraInfo.CameraID = ptCameraInfo.dwCameraID;
            cameraInfo.GroupID = ptCameraInfo.dwGroupID;
            cameraInfo.CameraName = ptCameraInfo.tCameraBase.szCameraName;
            cameraInfo.VideoSupplierDeviceID = ptCameraInfo.tCameraBase.dwVideoSupplierDeviceId;
            cameraInfo.VideoSupplierChannelID = ptCameraInfo.tCameraBase.szVideoSupplierChannelId;
            cameraInfo.PosCoordX = ptCameraInfo.tCameraBase.tPosCoord.fX;
            cameraInfo.PosCoordY = ptCameraInfo.tCameraBase.tPosCoord.fY;
            return cameraInfo;
        }
        public static DateTime ConvertLinuxTime(uint linuxtime)
        {
            DateTime retTime = Common.ZEROTIME.AddSeconds(linuxtime);
            if (retTime < Common.ZEROTIME.AddYears(1))
                return new DateTime().AddSeconds(linuxtime);
            else
                return retTime;
        }

        public static UInt32 ConvertLinuxTime(DateTime dnettime)
        {
            if (dnettime < Common.ZEROTIME)
                return (uint)(dnettime.Subtract(new DateTime()).TotalSeconds);
            else
            {
                if (dnettime > Common.MAXTIME)
                    return (uint)(Common.MAXTIME.Subtract(Common.ZEROTIME).TotalSeconds);
                else
                    return (uint)(dnettime.Subtract(Common.ZEROTIME).TotalSeconds);
            }
        }

        //internal static TVDASDK_SEARCH_TARGET ToSearchTarget(this SearchItem searchItem)
        //{
        //    TVDASDK_SEARCH_TARGET searchTarget = SDKConstant.TVDASDK_SEARCH_TARGET_Empty;

        //    if (searchItem != null)
        //    {
        //        searchTarget = new TVDASDK_SEARCH_TARGET() { dwCameraID = searchItem.CameraId, dwTaskUnitID = searchItem.TaskUnitId };
        //    }
        //    return searchTarget;
        //}


        internal static TVDASDK_SEARCH_TASK_UNIT_LIST GetTargetList(SearchPara searchPara)
        {
            List<SearchItem> searchItems = null;

            //if (searchPara.DisplayMode == SearchResultDisplayMode.ThumbNailAllSearchItem)
            //{
            searchItems = searchPara.SearchItems;
            //}
            //else
            //{
            //    searchItems = new List<SearchItem>();
            //    searchItems.Add(searchPara.CurrentSearchItem);
            //}

            TVDASDK_SEARCH_TASK_UNIT_LIST targetsRet = new TVDASDK_SEARCH_TASK_UNIT_LIST();
            int count = searchItems.Count;

            IntPtr ptrStart = Marshal.AllocHGlobal(count * Marshal.SizeOf(typeof(uint)));
            IntPtr ptr;
            for (int index = 0; index < count; index++)
            {
                ptr = ptrStart + index * Marshal.SizeOf(typeof(uint));
                Marshal.StructureToPtr(searchItems[index].TaskUnitId, ptr, true);
            }
            targetsRet.pdwTaskUnitID = ptrStart;
            targetsRet.dwTaskUnitNum = (uint)count;
            return targetsRet;
        }

        internal static TVDASDK_SEARCH_MOBILEOBJ_FILTER GetMoveObjectFilter(SearchPara searchPara)
        {
            TVDASDK_SEARCH_MOBILEOBJ_FILTER filter = new TVDASDK_SEARCH_MOBILEOBJ_FILTER();
            filter.dwStartTimeS = ConvertLinuxTime(searchPara.StartTime);
            filter.dwEndTimeS = ConvertLinuxTime(searchPara.EndTime);
            filter.dwSearchObjType = (uint)((int)(searchPara[SDKConstant.dwSearchObjType]));
            Color color = (Color)searchPara[SDKConstant.dwSearchObjRGB];

            filter.dwSearchObjRGB = (uint)color.ToArgb();
            filter.bColorSearch = bool.Parse(searchPara[SDKConstant.bColorSearch].ToString());
            E_ColorSimilarity colorSimilarity = (E_ColorSimilarity)searchPara[SDKConstant.dwColorSimilar];
            filter.dwColorSimilar = GetColorSimilarity(color, colorSimilarity);

            filter.dwRangeFilterType = (uint)(E_VDA_SEARCH_MOVE_OBJ_RANGE_FILTER_TYPE)searchPara[SDKConstant.dwRangeFilterType];

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(
                "Move Object Search Filter: \r\n starttime: {0}, \r\n endtime: {1}, \r\n ObjType: {2}, \r\n Color: {3}, r\n ColorSimilarity: {4}, behavior filtertype: {5}",
                searchPara.StartTime.ToString(DataModel.Constant.DATETIME_FORMAT),
                searchPara.EndTime.ToString(DataModel.Constant.DATETIME_FORMAT),
                searchPara[SDKConstant.dwSearchObjType],
                color, filter.dwColorSimilar, searchPara[SDKConstant.dwRangeFilterType]);

            List<PassLine> lines = (List<PassLine>)searchPara[SDKConstant.ptSearchPassLineList];
            sb.Append("\r\nPassLine List:");
            int index = 1;
            if (lines != null && lines.Count > 0)
            {
                filter.ptSearchPassLineList = GetSearchPassLineList(lines);
                foreach (PassLine pl in lines)
                {
                    sb.AppendFormat("{9}: Type: {8}, PassLine: ({0}, {1}), ({2},{3}), direction line: ({4}, {5}), ({6},{7}) \r\n",
                        pl.PassLineStart.X, pl.PassLineStart.Y, pl.PassLineEnd.X, pl.PassLineEnd.Y,
                        pl.DirectLineStart.X, pl.DirectLineStart.Y, pl.DirectLineEnd.X, pl.DirectLineEnd.Y,
                        index++, pl.PassLineType);
                }
            }
            else
            {
                filter.ptSearchPassLineList = IntPtr.Zero;
            }

            filter.dwPassLineNum = (uint)(int)searchPara[SDKConstant.dwPassLineNum];

            List<BreakRegion> regions = (List<BreakRegion>)searchPara[SDKConstant.tSearchBreakRegion];

            sb.Append("\r\nBreakRegion List:");
            if (regions != null && regions.Count > 0)
            {
                index = 1;
                filter.tSearchBreakRegion = GetSearchBreakRegion(regions[0]);

                foreach (BreakRegion br in regions)
                {
                    sb.AppendFormat("\r\nRegion {0}, type: {1}", index++, br.RegionType);
                    foreach (Point p in br.RegionPointList)
                    {
                        sb.AppendFormat("({0}, {1}),", p.X, p.Y);
                    }
                }
            }
            else
            {
                filter.tSearchBreakRegion = new TVDASDK_SEARCH_BREAK_REGION();
            }
            MyLog4Net.Container.Instance.Log.Debug(sb.ToString());

            return filter;
        }

        private static IntPtr GetSearchPassLineList(List<PassLine> lines)
        {
            IntPtr temp = IntPtr.Zero;
            temp = Marshal.AllocHGlobal(lines.Count * Marshal.SizeOf(typeof(TVDASDK_IA_SEARCH_PASS_LINE)));
            for (int i = 0; i < lines.Count; i++)
            {
                PassLine l = lines[i];
                TVDASDK_IA_SEARCH_PASS_LINE templine = new TVDASDK_IA_SEARCH_PASS_LINE();
                templine.dwPassLineType = l.PassLineType;
                templine.tDirectLine.tStartPt.dwX = (uint)l.DirectLineStart.X;
                templine.tDirectLine.tStartPt.dwY = (uint)l.DirectLineStart.Y;
                templine.tDirectLine.tEndPt.dwX = (uint)l.DirectLineEnd.X;
                templine.tDirectLine.tEndPt.dwY = (uint)l.DirectLineEnd.Y;
                templine.tPassLine.tStartPt.dwX = (uint)l.PassLineStart.X;
                templine.tPassLine.tStartPt.dwY = (uint)l.PassLineStart.Y;
                templine.tPassLine.tEndPt.dwX = (uint)l.PassLineEnd.X;
                templine.tPassLine.tEndPt.dwY = (uint)l.PassLineEnd.Y;
                Marshal.StructureToPtr(templine, temp + i * Marshal.SizeOf(typeof(TVDASDK_IA_SEARCH_PASS_LINE)), false);
            }
            return temp;
        }

        private static TVDASDK_SEARCH_BREAK_REGION GetSearchBreakRegion(BreakRegion region)
        {
            TVDASDK_SEARCH_BREAK_REGION temp = new TVDASDK_SEARCH_BREAK_REGION();
            temp.dwRegionType = region.RegionType;
            temp.dwPointNum = (uint)region.RegionPointList.Count;
            temp.atRegionPointList = new TVDASDK_SEARCH_POINT[Common.VDA_ONE_BREAK_REGION_POINT_MAXNUM];
            for (int i = 0; i < temp.dwPointNum; i++)
            {
                temp.atRegionPointList[i] = new TVDASDK_SEARCH_POINT();
                temp.atRegionPointList[i].dwX = (uint)region.RegionPointList[i].X;
                temp.atRegionPointList[i].dwY = (uint)region.RegionPointList[i].Y;
            }
            return temp;
        }

        internal static TVDASDK_SEARCH_IMAGE_FILTER GetCompareSearchFilter(SearchPara searchPara)
        {
            TVDASDK_SEARCH_IMAGE_FILTER filter = new TVDASDK_SEARCH_IMAGE_FILTER();
            filter.dwStartTimeS = ConvertLinuxTime(searchPara.StartTime);
            filter.dwEndTimeS = ConvertLinuxTime(searchPara.EndTime);
            int nTmp = (int)(searchPara[SDKConstant.dwAlgorithmFilterType]);
            CompareSearchPattern compareSearchPattern = (CompareSearchPattern)nTmp;
            filter.dwAlgorithmFilterType = (uint)(nTmp);
            filter.dwObjFilterType = (uint)E_VDA_SEARCH_MOVE_OBJ_FILTER_TYPE.E_SEARCH_MOVE_OBJ_FILTER_ALL_MOVE_OBJ;

            E_ColorSimilarity colorSimilarity = (E_ColorSimilarity)searchPara[SDKConstant.dwColorSimilar];
            uint compareSimilarity = GetCompareSimilarity(compareSearchPattern, colorSimilarity);
            filter.dwColorSimilar = compareSimilarity;
            Image img = searchPara[SDKConstant.CompareImage] as Image;

            MyLog4Net.Container.Instance.Log.DebugFormat(
                "Compare Search Filter: \r\n starttime: {0}, \r\n endtime: {1}, \r\n searchpattern: {2}, \r\n ColorSimilarity: {3}",
                searchPara.StartTime.ToString(DataModel.Constant.DATETIME_FORMAT),
                searchPara.EndTime.ToString(DataModel.Constant.DATETIME_FORMAT),
                compareSearchPattern,
                compareSimilarity);

            if (img != null)
            {
                TVDASDK_SEARCH_IMAGE_INFO imageInfo = new TVDASDK_SEARCH_IMAGE_INFO();
                byte[] bytes = ImageToJpegBytes(img);

                imageInfo.dwImageSize = (uint)bytes.Length;
                imageInfo.ptImageData = Marshal.AllocHGlobal(bytes.Length);

                Marshal.Copy(bytes, 0, imageInfo.ptImageData, bytes.Length);
                filter.tImageInfo = imageInfo;
                Rectangle rect = (Rectangle)searchPara[SDKConstant.CompareImageRect];
                filter.tObjRect.dwX = (uint)rect.X;
                filter.tObjRect.dwY = (uint)rect.Y;
                filter.tObjRect.dwWidth = (uint)rect.Width;
                filter.tObjRect.dwHeight = (uint)rect.Height;

                MyLog4Net.Container.Instance.Log.DebugFormat(
                "Compare Search Filter: image size: {0}, compare rectangle: ({1}, {2}, {3}, {4})",
                imageInfo.dwImageSize, rect.X, rect.Y, rect.Width, rect.Height);
            }
            // filter.tSearchBreakRegion = 



            return filter;
        }

        internal static TVDASDK_SEARCH_FACEOBJ_FILTER GetFaceSearchFilter(SearchPara searchPara)
        {
            TVDASDK_SEARCH_FACEOBJ_FILTER filter = new TVDASDK_SEARCH_FACEOBJ_FILTER();
            filter.dwStartTimeS = ConvertLinuxTime(searchPara.StartTime);
            filter.dwEndTimeS = ConvertLinuxTime(searchPara.EndTime);

            MyLog4Net.Container.Instance.Log.DebugFormat("Face Search Filter: \r\n starttime: {0}, \r\n endtime: {1}",
                searchPara.StartTime.ToString(DataModel.Constant.DATETIME_FORMAT),
                searchPara.EndTime.ToString(DataModel.Constant.DATETIME_FORMAT));

            return filter;
        }

        internal static TVDASDK_SEARCH_VEHICLE_FILTER GetVehicleSearchFilter(SearchPara searchPara)
        {
            TVDASDK_SEARCH_VEHICLE_FILTER filter = new TVDASDK_SEARCH_VEHICLE_FILTER();
            filter.dwStartTimeS = ConvertLinuxTime(searchPara.StartTime);
            filter.dwEndTimeS = ConvertLinuxTime(searchPara.EndTime);

            filter.dwVehicleColor = (uint)(int)searchPara[SDKConstant.dwVehicleColor];
            filter.dwVehiclePlateColor = (uint)(int)searchPara[SDKConstant.dwVehiclePlateColor];

            filter.szVehiclePlateName = searchPara[SDKConstant.szVehiclePlateName].ToString();
            //if (string.IsNullOrEmpty(filter.szVehiclePlateName))
            //{
            //    filter.szVehiclePlateName = "%";
            //}
            //else
            //{
            //    filter.szVehiclePlateName = string.Format("*{0}*", filter.szVehiclePlateName);
            //}

            filter.dwVehicleLogo = (uint)(int)searchPara[SDKConstant.dwVehicleLogo];

            filter.dwVehicleDetailType = (uint)(int)searchPara[SDKConstant.dwVehicleDetailType];
            filter.dwVehiclePlateStruct = (uint)(int)searchPara[SDKConstant.dwVehiclePlateStruct];

            MyLog4Net.Container.Instance.Log.DebugFormat(
                "Vehicle Search Filter: \r\n starttime: {0}, \r\n endtime: {1}, \r\n vehicle color: {2}, \r\n plate color: {3}, \r\n plate number: {4}, \r\n vehicle brand: {5}, \r\n vehicle detailtype: {6}, \r\n vehicle PlateStruct: {7}",
                searchPara.StartTime.ToString(DataModel.Constant.DATETIME_FORMAT),
                searchPara.EndTime.ToString(DataModel.Constant.DATETIME_FORMAT),
                filter.dwVehicleColor, filter.dwVehiclePlateColor, filter.szVehiclePlateName, filter.dwVehicleLogo,
                filter.dwVehicleDetailType, filter.dwVehiclePlateStruct);

            return filter;
        }

        public static byte[] ImageToJpegBytes(Image img)
        {
            byte[] bytes = null;

            ImageFormat format = img.RawFormat;

            using (MemoryStream ms = new MemoryStream())
            {
                //if(format.Equals(ImageFormat.Jpeg) ||
                //    format.Equals(ImageFormat.Bmp) ||
                //    format.Equals(ImageFormat.Gif) ||
                //    format.Equals(ImageFormat.Icon))
                {
                    img.Save(ms, ImageFormat.Jpeg);
                    bytes = new byte[ms.Length];

                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Read(bytes, 0, bytes.Length);
                }
            }

            return bytes;
        }

        private static uint GetCompareSimilarity(CompareSearchPattern compareSearchPattern, E_ColorSimilarity colorSimilarity)
        {
            uint nRet = 0;

            //if (compareSearchPattern == CompareSearchPattern.Texture)
            //{
            //    nRet = 800;
            //}
            //else if (compareSearchPattern == CompareSearchPattern.Blob)
            //{
            if (colorSimilarity == E_ColorSimilarity.High)
            {
                nRet = 800;
            }
            else if (colorSimilarity == E_ColorSimilarity.Middle)
            {
                nRet = 500;
            }
            else if (colorSimilarity == E_ColorSimilarity.Low)
            {
                nRet = 200;
            }
            else
            {
                nRet = 1;
            }
            // }
            // 临时调试用
            // nRet = 0;
            return nRet;
        }

        private static uint GetColorSimilarity(Color color, E_ColorSimilarity similarity)
        {
            uint nRet = 0;

            double defaultRate = 0.8d;
            switch ((uint)color.ToArgb())
            {
                // Color.White
                case 0xFFFFFFFF: defaultRate = 0.5d;
                    break;
                // Color.Silver
                case 0xFFC0C0C0: defaultRate = 0.8d;
                    break;
                // Color.Black
                case 0xFF000000: defaultRate = 0.93d;
                    break;
                // Color.Red
                case 0xFFFF0000: defaultRate = 0.8d;
                    break;
                // Color.Puple
                case 0xFF800080: defaultRate = 0.8d;
                    break;
                // Color.Blue
                case 0xFF0000FF: defaultRate = 0.7d;
                    break;
                case 0xFFFFFF00: defaultRate = 0.77d;
                    break;
                // Color.Green
                case 0xFF008000: defaultRate = 0.75d;
                    break;
                // Color.Gray
                case 0xFF808080: defaultRate = 0.8d;
                    break;
                // Color.Bule
                case 0xFFFFC0CB: defaultRate = 0.71d;
                    break;
                default: defaultRate = 0.8d;
                    break;
            }
            double retVal = 0;
            switch (similarity)
            {
                case E_ColorSimilarity.High:
                    retVal = 0.6; //defaultRate + 0.05; 
                    break;
                case E_ColorSimilarity.Middle:
                    retVal = 0.5; // defaultRate; 
                    break;
                case E_ColorSimilarity.Low:
                    retVal = 0.1; // defaultRate - 0.05; 
                    break;
                case E_ColorSimilarity.None:
                    retVal = 0.001;
                    break;
                default:
                    retVal = 0.8d;
                    break;
            }
            nRet = (uint)(retVal * 1000);
            return nRet;
        }

        internal static TVDASDK_SEARCH_RESULT_REQUIREMENT GetPageAndSortSettings(SearchPara searchPara)
        {
            TVDASDK_SEARCH_RESULT_REQUIREMENT settings = new TVDASDK_SEARCH_RESULT_REQUIREMENT();
            settings.dwOnePageRecordNum = (uint)searchPara.PageInfo.CountPerPage;
            settings.dwResultSortType = (uint)searchPara.SortType;

            return settings;
        }

        internal static SearchItemResult GetSearchItemResult(uint dwQueryHandle, uint dwTaskUnitID, TVDASDK_SEARCH_RESULT_PAGE_INFO tResultPageInfo, IntPtr ptSearchResultObjInfoStart)
        {
            SearchItemResult siResult = new SearchItemResult() { SearchId = dwQueryHandle, TaskUnitId = dwTaskUnitID };
            siResult.Succeed = tResultPageInfo.dwResult == 0;

            PageInfoBase pageInfo = new PageInfoBase()
            {
                Index = (int)tResultPageInfo.dwCurPageIdx,
                CountInCurrentPage = (int)tResultPageInfo.dwCurSearchResultNum,
                TotalCount = (int)tResultPageInfo.dwResultTotalNum,
                TotalPage = (int)tResultPageInfo.dwPageCount
            };
            siResult.PageInfo = pageInfo;

            if (siResult.Succeed)
            {
                List<SearchResultRecord> resultRecords = GetSearchResultRecords(siResult.PageInfo.CountInCurrentPage, ptSearchResultObjInfoStart);
                siResult.ResultRecords = resultRecords;
            }
            else
            {
                siResult.ResultRecords = new List<SearchResultRecord>();
            }

            return siResult;
        }

        internal static SearchItemResult GetVehicleSearchItemResult(uint dwQueryHandle, uint dwTaskUnitID, TVDASDK_SEARCH_RESULT_PAGE_INFO tResultPageInfo, IntPtr ptSearchResultObjInfoStart)
        {
            SearchItemResult siResult = new SearchItemResult() { SearchId = dwQueryHandle, TaskUnitId = dwTaskUnitID };
            siResult.Succeed = tResultPageInfo.dwResult == 0;

            PageInfoBase pageInfo = new PageInfoBase()
            {
                Index = (int)tResultPageInfo.dwCurPageIdx,
                CountInCurrentPage = (int)tResultPageInfo.dwCurSearchResultNum,
                TotalCount = (int)tResultPageInfo.dwResultTotalNum,
                TotalPage = (int)tResultPageInfo.dwPageCount
            };
            siResult.PageInfo = pageInfo;

            if (siResult.Succeed)
            {
                List<SearchResultRecord> resultRecords = GetVehicleSearchResultRecords(siResult.PageInfo.CountInCurrentPage, ptSearchResultObjInfoStart);
                siResult.ResultRecords = resultRecords;
            }

            return siResult;
        }

        private static List<SearchResultRecord> GetSearchResultRecords(int count, IntPtr ptSearchResultObjInfoStart)
        {
            int unitSize = Marshal.SizeOf(typeof(TVDASDK_SEARCH_RESULT_OBJ_INFO));
            List<SearchResultRecord> records = new List<SearchResultRecord>();

            IntPtr ptr;
            SearchResultRecord record;
            TVDASDK_SEARCH_RESULT_OBJ_INFO resultInfo = new TVDASDK_SEARCH_RESULT_OBJ_INFO();
            for (int i = 0; i < count; i++)
            {
                ptr = (IntPtr)(i * unitSize + (int)ptSearchResultObjInfoStart);
                resultInfo = (TVDASDK_SEARCH_RESULT_OBJ_INFO)Marshal.PtrToStructure(ptr, typeof(TVDASDK_SEARCH_RESULT_OBJ_INFO));
                record = GetResultRecord(resultInfo);
                records.Add(record);
            }

            return records;
        }

        private static List<SearchResultRecord> GetVehicleSearchResultRecords(int count, IntPtr ptSearchResultObjInfoStart)
        {
            int unitSize = Marshal.SizeOf(typeof(TVDASDK_SEARCH_VEHICLE_RESULT_OBJ_INFO));
            List<SearchResultRecord> records = new List<SearchResultRecord>();

            IntPtr ptr;
            SearchResultRecord record;
            TVDASDK_SEARCH_VEHICLE_RESULT_OBJ_INFO resultInfo = new TVDASDK_SEARCH_VEHICLE_RESULT_OBJ_INFO();
            for (int i = 0; i < count; i++)
            {
                ptr = (IntPtr)(i * unitSize + (int)ptSearchResultObjInfoStart);
                resultInfo = (TVDASDK_SEARCH_VEHICLE_RESULT_OBJ_INFO)Marshal.PtrToStructure(ptr, typeof(TVDASDK_SEARCH_VEHICLE_RESULT_OBJ_INFO));
                record = GetResultRecord(resultInfo);
                records.Add(record);

                Debug.WriteLine(String.Format("ModelParser.GetVehicleSearchResultRecords: {0}, VehicleType: {1}, VehicleDetailType: {2}",
               record.PlateNO, record.VehicleType, record.VehicleDetailType));
            }

            return records;
        }

        private static SearchResultRecord GetResultRecord(TVDASDK_SEARCH_VEHICLE_RESULT_OBJ_INFO resultInfo)
        {
            //int color1 = (int)resultInfo.dwVehicleColor1;
            //uint simimar = (uint)resultInfo.dwVehicleColorSimilar1;

            //if (resultInfo.dwVehicleColorSimilar2 > simimar)
            //{
            //    color1 = (int)resultInfo.dwVehicleColor2;
            //    simimar = (uint)resultInfo.dwVehicleColorSimilar2;
            //}

            //if (resultInfo.dwVehicleColorSimilar3 > simimar)
            //{
            //    color1 = (int)resultInfo.dwVehicleColor3;
            //    simimar = (uint)resultInfo.dwVehicleColorSimilar3;
            //}

            SearchResultRecord record = new SearchResultRecord()
            {
                IsVehicleSearchResult = true,
                ObjectType = SearchResultObjectType.CAR,
                CameraID = (uint)resultInfo.tObjID.dwCameraID,
                TaskUnitID = (uint)resultInfo.tObjID.dwTaskUnitID,
                ID = resultInfo.tObjID.dwMoveObjID,
                Distance = resultInfo.dwVehicleColorSimilar1,

                TargetTs = ConvertLinuxTime(resultInfo.tImageSnapshot.qwCurTime),

                ThumbPicPath = resultInfo.szThumbImageURL,
                OrgPicPath = resultInfo.szOriginalImageURL,
                ObjectRect = new Rectangle((int)resultInfo.tImageSnapshot.tObjRect.dwX, (int)resultInfo.tImageSnapshot.tObjRect.dwY,
                    (int)resultInfo.tImageSnapshot.tObjRect.dwWidth, (int)resultInfo.tImageSnapshot.tObjRect.dwHeight),

                PlateNO = resultInfo.szVehiclePlate,
                VehicleBrand = (int)resultInfo.dwVehicleLogo, //  random.Next(0, 124), // 
                VehicleType = (VehicleType)resultInfo.dwVehicleType, //  (VehicleType)(random.Next(0, 4)),  //
                VehicleDetailType = (VehicleDetailType)resultInfo.dwVehicleDetailType, //  (VehicleDetailType)(random.Next(0, 6)), //   
                VehiclePlateType = (VehiclePlateType)resultInfo.dwVehiclePlateStruct, // (VehiclePlateType)(random.Next(0, 3)), //      

                VehicleBodyColor1 = (int)resultInfo.dwVehicleColor1,
                VehicleBodyColor2 = (int)resultInfo.dwVehicleColor2,
                VehicleBodyColor3 = (int)resultInfo.dwVehicleColor3,

                PlateColor = (int)resultInfo.dwVehiclePlateColor // random.Next(0, 5) //       

            };

            Debug.WriteLine(String.Format("ModelParser.GetResultRecord: {0}, VehicleType: {1}, VehicleDetailType: {2}",
               record.PlateNO, record.VehicleType, record.VehicleDetailType));


            record.TargetAppearTs = record.TargetTs;
            record.TargetDisappearTs = record.TargetTs.AddSeconds(5);

            return record;
        }

        private static SearchResultRecord GetResultRecord(TVDASDK_SEARCH_RESULT_OBJ_INFO resultInfo)
        {
            SearchResultRecord record = new SearchResultRecord()
            {
                ObjectType = (SearchResultObjectType)resultInfo.tObjBase.dwObjType,
                CameraID = resultInfo.tObjBase.tObjID.dwCameraID,
                TaskUnitID = resultInfo.tObjBase.tObjID.dwTaskUnitID,
                ID = resultInfo.tObjBase.tObjID.dwMoveObjID,
                Distance = resultInfo.dwSimilar,
                TargetAppearTs = ConvertLinuxTime(resultInfo.tObjBase.dwBeginTime),
                TargetDisappearTs = ConvertLinuxTime(resultInfo.tObjBase.dwEndTime),
                TargetTs = ConvertLinuxTime(resultInfo.tImageSnapshot.qwCurTime),
                ThumbPicPath = resultInfo.szThumbImageURL,
                OrgPicPath = resultInfo.szOriginalImageURL,
                ObjectRect = new Rectangle((int)resultInfo.tImageSnapshot.tObjRect.dwX, (int)resultInfo.tImageSnapshot.tObjRect.dwY,
                    (int)resultInfo.tImageSnapshot.tObjRect.dwWidth, (int)resultInfo.tImageSnapshot.tObjRect.dwHeight),


                IsVehicleSearchResult = false,
                PlateColor = 0,
                VehicleType = 0,
                VehiclePlateType = 0,
                VehicleDetailType = 0,
                VehicleBrand = -1,
                PlateNO = "",
                VehicleBodyColor1 = 0,
                VehicleBodyColor2 = 0,
                VehicleBodyColor3 = 0,
                TimeStamp = 0,
            };
            return record;
        }

        internal static VideoSupplierDeviceInfo Convert(TVDASDK_NET_STORE_DEV_INFO netDevInfo)
        {
            VideoSupplierDeviceInfo deviceInfo = new VideoSupplierDeviceInfo()
            {
                Id = netDevInfo.dwVideoSupplierDeviceId,
                ProtocolType = (E_VDA_NET_STORE_DEV_PROTOCOL_TYPE)netDevInfo.tNetStoreDevBase.dwAccessProtocolType,
                IP = netDevInfo.tNetStoreDevBase.szDeviceIP,
                DeviceName = netDevInfo.tNetStoreDevBase.szDeviceName,
                Port = netDevInfo.tNetStoreDevBase.dwDevicePort,
                Password = netDevInfo.tNetStoreDevBase.szLoginPwd,
                UserName = netDevInfo.tNetStoreDevBase.szLoginUser
            };
            return deviceInfo;
        }

        internal static TVDASDK_NET_STORE_DEV_BASE Convert(VideoSupplierDeviceInfo deviceInfo)
        {
            TVDASDK_NET_STORE_DEV_BASE tNetStoreDevBase = new TVDASDK_NET_STORE_DEV_BASE()
                {
                    dwAccessProtocolType = (uint)deviceInfo.ProtocolType,
                    dwDevicePort = deviceInfo.Port,
                    szDeviceIP = deviceInfo.IP,
                    szDeviceName = deviceInfo.DeviceName,
                    szLoginPwd = deviceInfo.Password,
                    szLoginUser = deviceInfo.UserName
                };
            return tNetStoreDevBase;
        }

        internal static VideoSupplierChannelInfo Convert(TVDASDK_NET_STORE_DEV_CHANNEL_INFO netChannel)
        {
            VideoSupplierChannelInfo channelInfo = new VideoSupplierChannelInfo()
            {
                Id = netChannel.szChannelId,
                Name = netChannel.szChannelName,
                ReservedDescription = netChannel.szRest
            };
            return channelInfo;
        }

        internal static VideoFileInfo Convert(TVDASDK_NET_STORE_DEV_FILE_INFO netFileInfo)
        {
            VideoFileInfo fileInfo = new VideoFileInfo()
            {
                Id = netFileInfo.szFileId,
                Size = netFileInfo.qwFileSize,
                StartTime = ConvertLinuxTime(netFileInfo.tStartTime),
                EndTime = ConvertLinuxTime(netFileInfo.tEndTime),
                ReservedDescription = netFileInfo.szRest
            };
            return fileInfo;
        }

        internal static TVDASDK_NETSTORE_VIDEO_IMPORT_INFO Convert(VAFileInfo vaFileInfo)
        {
             TVDASDK_NETSTORE_VIDEO_IMPORT_INFO info = new TVDASDK_NETSTORE_VIDEO_IMPORT_INFO();
            uint nStartTime = ModelParser.ConvertLinuxTime( vaFileInfo.StartTime);
            info.dwAdjustStartTime = nStartTime;
            if(!string.IsNullOrEmpty(vaFileInfo.AdjustTime))
            {
                DateTime dt;
                if (DateTime.TryParse(vaFileInfo.AdjustTime, out dt))
                {
                    info.dwAdjustStartTime = ModelParser.ConvertLinuxTime(dt);
                }
            }

            info.dwCameraID = vaFileInfo.CameraId;
            info.qwFileSize = vaFileInfo.FileSize;
            info.dwStartTime = nStartTime;
            info.dwEndTime = ModelParser.ConvertLinuxTime(vaFileInfo.EndTime);
            info.szTaskUnitName = string.Format("{0}_{1}-{2}", vaFileInfo.CameraName, vaFileInfo.StartTime.ToString("yyyyMMddHHmmss"), vaFileInfo.EndTime.ToString("yyyyMMddHHmmss"));
                
            uint[] types = new uint[Common.VDASDK_ANALYZE_TYPE_MAXNUM];

            vaFileInfo.VideoAnalyzeInfo.VideoAnalyzeType.ToArray().CopyTo(types, 0);

            info.tVideoAnalyzeInfo.adwAnalyzeType = types;

            info.tVideoAnalyzeInfo.dwAnalyzeTypeNum = vaFileInfo.VideoAnalyzeInfo.VideoAnalyzeTypeNum;

            return info;
        }
    }
}
