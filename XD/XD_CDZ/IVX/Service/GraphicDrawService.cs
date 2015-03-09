using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Framework;
using System.Threading;
using BOCOM.DataModel;
using System.Drawing;

namespace BOCOM.IVX.Service
{
    public class GraphicDrawService
    {

        private IntPtr m_hPlayWnd = IntPtr.Zero;

        private Image m_Image;

        public IntPtr HPlayWnd
        {
            get { return m_hPlayWnd; }
            set
            {
                if (value!= IntPtr.Zero && m_hPlayWnd != value)
                    m_hPlayWnd = value; 
            }
        }

        private IntPtr m_hPicWnd = IntPtr.Zero;

        public IntPtr HPicWnd
        {
          get { return m_hPicWnd; }
            set 
            {
                if (value != IntPtr.Zero && m_hPicWnd != value)
                    m_hPicWnd = value; 
            }
        }


        public bool SetPlayDrawType( E_VDA_SEARCH_MOVE_OBJ_RANGE_FILTER_TYPE type)
        {
            if (m_hPlayWnd != IntPtr.Zero)
            {
                int vodhandle = Framework.Container.Instance.VideoPlayService.GetPlayHandleByhWnd(m_hPlayWnd);
                if (vodhandle <= 0)
                    return false;

                bool ret = Framework.Container.Instance.IVXProtocol.SetPlayDrawType(vodhandle, type);
                return ret;
            }
            return false;
        }

        public bool ClearPlayDraw()
        {
            if (m_hPlayWnd != IntPtr.Zero)
            {
                int vodhandle = Framework.Container.Instance.VideoPlayService.GetPlayHandleByhWnd(m_hPlayWnd);
                if (vodhandle <= 0)
                    return false;

                bool ret = Framework.Container.Instance.IVXProtocol.ClearPlayDraw(vodhandle);
                return ret;
            }
            return false;
        }

        public List<PassLine> GetPlayDrawPassline()
        {
            if (m_hPlayWnd != IntPtr.Zero)
            {
                int vodhandle = Framework.Container.Instance.VideoPlayService.GetPlayHandleByhWnd(m_hPlayWnd);
                if (vodhandle <= 0)
                    return new List<PassLine>();

                List<PassLine> line = Framework.Container.Instance.IVXProtocol.GetPlayDrawPassline(vodhandle);
                return line;
            }
            return new List<PassLine>();
        }

        public List<BreakRegion> GetPlayDrawBreakRegion()
        {
            if (m_hPlayWnd != IntPtr.Zero)
            {
                int vodhandle = Framework.Container.Instance.VideoPlayService.GetPlayHandleByhWnd(m_hPlayWnd);
                List<BreakRegion> line = Framework.Container.Instance.IVXProtocol.GetPlayDrawBreakRegion(vodhandle);
                return line;
            }
            return new List<BreakRegion>();
        }


        public void Cleanup()
        {
            m_hPlayWnd = IntPtr.Zero;
            m_hPicWnd = IntPtr.Zero;
        }

        uint m_hPdoHandle = 0;
        public uint OpenPic()
        {
            m_hPdoHandle = Framework.Container.Instance.IVXProtocol.Pdo_Open(m_hPicWnd, 0);
            return m_hPdoHandle;
        }

        public void ClosePic()
        {
            Framework.Container.Instance.IVXProtocol.Pdo_Close(m_hPdoHandle);
            m_hPdoHandle = 0;
        }

        public void SetPic(Image img)
        {
            Framework.Container.Instance.IVXProtocol.Pdo_DisplayPicDataSet(m_hPdoHandle, img);
            m_Image = img;
        }

        public void SetPicDrawTypeRect()
        {
            Framework.Container.Instance.IVXProtocol.Pdo_DrawTypeSet(m_hPdoHandle, E_PDO_DRAW_TYPE.E_PDO_DRAW_RECT);
        }

        public List<Rectangle> GetPicDrawRect()
        {
            List<Rectangle> rects = Framework.Container.Instance.IVXProtocol.Pdo_DrawRectGet(m_hPdoHandle);

            if (rects == null || rects.Count == 0)
            {
                rects.Add(new Rectangle(new Point(0, 0), m_Image.Size));
            }

            return rects;
        }

        public void SetPicDrawRect(List<Rectangle> rects)
        {
            Framework.Container.Instance.IVXProtocol.Pdo_DrawRectSet(m_hPdoHandle, rects);
        }

    }
}
