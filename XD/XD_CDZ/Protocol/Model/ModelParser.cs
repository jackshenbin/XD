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







    }
}
