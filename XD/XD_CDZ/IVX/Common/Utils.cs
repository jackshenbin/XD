using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using BOCOM.IVX.Protocol;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using BOCOM.DataModel;
using System.Drawing;
using DataModel;

namespace BOCOM.IVX.Common
{
    public class Utils
    {
        public static string GetSearchObjectTypeName(SearchResultObjectType objType)
        {
            string sRet = "未知";

            if (objType == SearchResultObjectType.CAR)
            {
                sRet = "车";
            }
            else if (objType == SearchResultObjectType.PEOPLE)
            {
                sRet = "人";
            }
            else if (objType == SearchResultObjectType.FACE)
            {
                sRet = "人脸";
            }

            return sRet;
        }

        public static IntPtr Convert2UnmanagedIntArray(List<int> ints)
        {
            IntPtr ptr = IntPtr.Zero;

            if (ints != null && ints.Count > 0)
            {
                //List<int> ids = Framework.Container.Instance.LocalVideoResourceViewModel.GetCheckedIds();
                
                //ptr = Marshal.AllocHGlobal(4 * ints.Count);
                //for (int i = 0; i < ints.Count; i++)
                //{
                //    Marshal.WriteInt32(ptr + 4 * i, ints[i]);
                //}
            }

            return ptr;
        }

        public static double GetSimilarRate(int type)
        {
            return (10 + 1 - type) / 10;
        }
        
        public static int SetSimilarRate(double value)
        {
            return 10- (int)(value*10)  + 1 ;
        }
        
        public static System.Drawing.Image GetSearchOriginalImage(byte[] nPicData, int nPicDataSize, int nPicWidth, int nPicHeight)
        {
            IntPtr imghead = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BITMAPFILEHEADER)) + Marshal.SizeOf(typeof(BITMAPINFOHEADER)));
            
            BITMAPFILEHEADER h1;
            h1.bfType1 = (byte)'B';
            h1.bfType2 = (byte)'M';
            h1.bfSize = Marshal.SizeOf(typeof(BITMAPFILEHEADER)) + Marshal.SizeOf(typeof(BITMAPINFOHEADER)) + nPicDataSize;
            h1.bfReserved1 = 0;
            h1.bfReserved2 = 0;
            h1.bfOffBits = Marshal.SizeOf(typeof(BITMAPFILEHEADER)) + Marshal.SizeOf(typeof(BITMAPINFOHEADER));

            BITMAPINFOHEADER h2;
            h2.biSize = Marshal.SizeOf(typeof(BITMAPINFOHEADER));
            h2.biWidth = nPicWidth;
            h2.biHeight = nPicHeight;
            h2.biPlanes = 1;
            h2.biBitCount = 24;
            h2.biCompression = 0;
            h2.biSizeImage = 0;
            h2.biXPelsPerMeter = 0;
            h2.biYPelsPerMeter = 0;
            h2.biClrUsed = 0;
            h2.biClrImportant = 0;

            Marshal.StructureToPtr(h1, imghead, true);
            Marshal.StructureToPtr(h2, imghead + Marshal.SizeOf(typeof(BITMAPFILEHEADER)), true);
            byte[] retimagehead = new byte[Marshal.SizeOf(typeof(BITMAPFILEHEADER)) + Marshal.SizeOf(typeof(BITMAPINFOHEADER))];

            byte[] retimage = new byte[Marshal.SizeOf(typeof(BITMAPFILEHEADER)) + Marshal.SizeOf(typeof(BITMAPINFOHEADER)) + nPicDataSize];
            Marshal.Copy(imghead, retimagehead, 0, retimagehead.Length);
            Array.Copy(retimagehead, retimage, retimagehead.Length);
            Array.Copy(nPicData, 0, retimage, retimagehead.Length, nPicDataSize);

            System.IO.MemoryStream ms = new System.IO.MemoryStream(retimage);
            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
            return image;
            }
            catch (ArgumentException aex)
            {
            return null;
            }
            finally
            {
                if(imghead != IntPtr.Zero)Marshal.FreeHGlobal(imghead);
            }
        }

        public static string GetMd5(string fileName, bool isSharedFile)
        {
            string md5 = string.Empty;

            //if (!string.IsNullOrEmpty(fileName) && (isSharedFile || File.Exists(fileName)))
            //{
            //    char[] chs = new char[256];
            //    int nSize;
            //    bool succeed = ICASProtocol.Vda_GetFileMd5(fileName, 256, chs, out nSize, isSharedFile);
            //    if (succeed)
            //    {
            //        md5 = new string(chs, 0, nSize);
            //    }
            //}

            return md5;
        }
                
        //public static Int32 GetItemInfo(Int32 hFromItem, out PTRESOURCE_ITEM_INRO pItemInfo)
        //{
        //    //Framework.Container.Instance.Log.DebugFormat("Common.Utils Start Vda_GetItemInfo: {0}", hFromItem);
        //    //int nResult = ICASProtocol.Vda_GetItemInfo(hFromItem, out pItemInfo);
        //    //Framework.Container.Instance.Log.DebugFormat("Common.Utils Leave Vda_GetItemInfo: {0}", hFromItem);
        //    pItemInfo = new PTRESOURCE_ITEM_INRO(); ; 
        //    return 0;
        //}

        public static bool CopyArray(IntPtr ptrOri, out IntPtr ptrNew, int count, int sizeofItem, Func<IntPtr, bool> deleteFun, bool isByte=false)
        {
            ptrNew = Marshal.AllocHGlobal(count * sizeofItem);
            if (!isByte)
            {
                int[] ids = new int[count];
                Marshal.Copy(ptrOri, ids, 0, count);
                Marshal.Copy(ids, 0, ptrNew, count);
            }
            else
            {
                byte[] ids = new byte[count];
                Marshal.Copy(ptrOri, ids, 0, count);
                Marshal.Copy(ids, 0, ptrNew, count);
            }
            bool result = true;
            if (deleteFun != null)
            {
                result = deleteFun(ptrOri);
            }
            return result;
        }

        [DllImport("kernel32")]
        public static extern void GlobalMemoryStatusEx(ref MEMORY_STATUS_EX meminfo);

        public static double GetMemoryUsage()
        {
            MEMORY_STATUS_EX memInfo = new MEMORY_STATUS_EX();
            memInfo.dwLength = (uint)Marshal.SizeOf(typeof(MEMORY_STATUS_EX));
            GlobalMemoryStatusEx(ref memInfo);
            if (memInfo.ullTotalPhys <= 0) 
                return 0;
            return  (1-(double)memInfo.ullAvailPhys/memInfo.ullTotalPhys)*100;
        }

        public static void CheckandInvoke(Control ctrl, Delegate delegator, object[] paras)
        {
            if (ctrl.IsDisposed)
                return;

            if (ctrl.InvokeRequired)
            {
                ctrl.Invoke(delegator, paras);
                return;
            }

            delegator.DynamicInvoke(paras);
        }

        public static void CheckandBeginInvoke(Control ctrl, Delegate delegator, object[] paras)
        {
            if (ctrl.IsDisposed)
                return;

            if (ctrl.InvokeRequired)
            {
                ctrl.BeginInvoke(delegator, paras);
                return;
            }

            delegator.DynamicInvoke(paras);
        }

        public static System.Drawing.Image ResizeImage(System.Drawing.Image image, int newWidth, int newHeight)
        {
            System.Drawing.Image imgResized = null;

            if (image != null && newWidth > 0 && newHeight > 0)
            {
                float widthScale = ((float)newWidth) / (float)image.Width;
                float heightScale = ((float)newHeight) / (float)image.Height;

                float scale = widthScale > heightScale ? heightScale : widthScale;

                int width = (int)(image.Width * scale);
                int height = (int)(image.Height * scale);

                imgResized = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(imgResized);
                g.DrawImage(image, new Rectangle(0, 0, width, height), 
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                g.Dispose();
            }

            return imgResized;
        }

        public static void HandleInputValidateFailEvent(NotifyPropertyChangedModel dataSource, string dataMember)
        {
            Control ctrl = Framework.Container.Instance.VVMDataBindings.GetControl(dataSource, dataMember);
            if (ctrl != null)
            {
                ctrl.Focus();
            }
        }

        //public static string GetNodeDisplayIndex(TreeListNode node)
        //{
        //    string sRet = string.Empty;

        //    if (node != null)
        //    {
        //        if (node.ParentNode == null)
        //        {
        //            sRet = node.TreeList.Nodes.IndexOf(node).ToString("000");
        //        }
        //        else
        //        {
        //            sRet = node.ParentNode.Nodes.IndexOf(node).ToString("000");
        //        }

        //        TreeListNode parent = node.ParentNode;
        //        while (parent != null)
        //        {
        //            if (parent.ParentNode == null)
        //            {
        //                sRet = string.Format("{0}{1}", parent.TreeList.Nodes.IndexOf(parent).ToString("000"), sRet);
        //            }
        //            else
        //            {
        //                sRet = string.Format("{0}{1}", parent.ParentNode.Nodes.IndexOf(parent).ToString("000"), sRet);
        //            }
        //            parent = parent.ParentNode;
        //        }
        //    }

        //    return sRet;
        //}

        /// <summary>
        /// 换算成 K, M, G Byte
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string GetByteSizeInUnit(ulong size)
        {
            string sRet;

            if (size >= Constant.VOLUMESIZE_G)
            {
                sRet = string.Format("{0} GB", size >> 30);
            }
            else if (size >= Constant.VOLUMESIZE_M)
            {
                sRet = string.Format("{0} MB", size >> 20);
            }
            else if (size >= Constant.VOLUMESIZE_K)
            {
                sRet = string.Format("{0} KB", size >> 10);
            }
            else
            {
                sRet = string.Format("{0} Byte", size);
            }

            return sRet;
        }

        #region Other structs
        //定义内存的信息结构 
        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_STATUS_EX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public UInt64 ullTotalPhys;
            public UInt64 ullAvailPhys;
            public UInt64 ullTotalPageFile;
            public UInt64 ullAvailPageFile;
            public UInt64 ullTotalVirtual;
            public UInt64 ullAvailVirtual;
            public UInt64 ullAvailExtendedVirtual;
        } ;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BITMAPFILEHEADER
        {
            public byte bfType1;
            public byte bfType2;
            public Int32 bfSize;
            public Int16 bfReserved1;
            public Int16 bfReserved2;
            public Int32 bfOffBits;
        } ;
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BITMAPINFOHEADER
        {
            public Int32 biSize;
            public Int32 biWidth;
            public Int32 biHeight;
            public Int16 biPlanes;
            public Int16 biBitCount;
            public Int32 biCompression;
            public Int32 biSizeImage;
            public Int32 biXPelsPerMeter;
            public Int32 biYPelsPerMeter;
            public Int32 biClrUsed;
            public Int32 biClrImportant;
        } ;

        #endregion
    }

}
