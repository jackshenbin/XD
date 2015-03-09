using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.DataModel;
using BOCOM.IVX.Protocol;
using System.Drawing;
using DataModel;
using System.Windows.Forms;

namespace BOCOM.IVX.Common
{
    public class Constant
    {
        private static Cursor s_CameraCursor;
        private static Cursor s_ChangeCursor;

        internal readonly static int TASKUNIT_MAXIMUM_SEARCH = 10;

        public readonly static uint VOLUMESIZE_K = 1 << 10;
        public readonly static uint VOLUMESIZE_M = 1 << 20;
        public readonly static uint VOLUMESIZE_G = 1 << 30;

        public readonly static TimeSpan TIMERANGE_PLATFORMVIDEOSEARCH = new TimeSpan(1, 0, 0, 0);

        public static Cursor CameraCursor
        {
            get
            {
                if (s_CameraCursor == null)
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(Properties.Resources.CAM);
                    s_CameraCursor = new Cursor(ms);
                    ms.Dispose();
                }
                return s_CameraCursor;
            }
        }

        public static Cursor ChangeCursor
        {
            get
            {
                if (s_CameraCursor == null)
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(Properties.Resources.CHANGE);
                    s_ChangeCursor = new Cursor(ms);
                    ms.Dispose();
                }
                return s_ChangeCursor;
            }
        }

    }
}

