using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Reflection;
using BOCOM.DataModel;
using System.Diagnostics;

namespace BOCOM.IVX.Controls
{
    public partial class ucEditImage : DevExpress.XtraEditors.XtraUserControl
    {
        public event EventHandler ZoomRateChanged;

        #region Fields

        private SearchResultRecord m_Record;

        private int m_zoomRate;

        // 当前的图片
        private Image m_CurImage = null;

        private SearchResultSingleSummary m_resultSummary;

        private List<Tuple<PropertyInfo, string>> m_properties;

        private bool m_Loaded = false;

        #endregion

        #region Properties

        public int ZoomRate
        {
            get { return m_zoomRate; }

            private set
            {
                if (m_zoomRate != value)
                {
                    m_zoomRate = value;
                    if (ZoomRateChanged != null)
                    {
                        ZoomRateChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        internal SearchResultRecord Record
        {
            get
            {
                return m_Record;
            }
            set
            {
                if (m_Record != value)
                {
                    m_Record = value;
                    if (m_Record != null)
                    {
                        ShowProperties();
                        ShowThumbImage(true);
                    }
                }
            }
        }

        internal Image ThumbImage
        {
            get
            {
                return this.picEditThumb.Image;
            }
            set
            {
                //if (this.picEditThumb.Image != null)
                //{
                //    this.picEditThumb.Image.Dispose();
                //}
               
                this.picEditThumb.Image = value;
            }
        }

        internal Image OriginalImage
        {
            get
            {
                return this.picEditOriginal.Image;
            }
            set
            {
                //if (this.picEditOriginal.Image != null)
                //{
                //    this.picEditOriginal.Image.Dispose();
                //}

                this.picEditOriginal.Image = value;
            }
        }

        public List<Tuple<PropertyInfo, string>> Properties
        {
            get { return m_properties; }
        }

        #endregion

        #region Constructors

        public ucEditImage()
        {
            InitializeComponent();

            picEditOriginal.Properties.ShowScrollBars = true;

            this.picEditOriginal.Width = 890;
            this.picEditOriginal.Location = new System.Drawing.Point(7, 11);
        }

        #endregion

        #region Private helper functions

        private List<Tuple<PropertyInfo, string>> GetProperties(SearchResultRecord record)
        {
            List<Tuple<PropertyInfo, string>> psRet = new List<Tuple<PropertyInfo, string>>();

            PropertyInfo[] properties = record.GetType().GetProperties();

            if (properties != null && properties.Length > 0)
            {
                string description;
                SearchResultPropertyAttribute attr;
                foreach (PropertyInfo p in properties)
                {
                    object[] objs = p.GetCustomAttributes(typeof(SearchResultPropertyAttribute), false);
                    if (objs != null && objs.Length > 0)
                    {
                        attr = (SearchResultPropertyAttribute)objs[0];
                        if (record.IsVehicleSearchResult)
                        {
                            if (attr.AvailableMode == AvailableMode.NonVehicle)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            if (attr.AvailableMode == AvailableMode.Vehicle)
                            {
                                continue;
                            }
                        }

                        description = ((SearchResultPropertyAttribute)objs[0]).DisplayName;
                        if (string.CompareOrdinal(description, "相似度") == 0 && !m_resultSummary.IsSimilaritySearch)
                        {
                            continue;
                        }
                        psRet.Add(new Tuple<PropertyInfo, string>(p, description));
                    }
                }
            }
            return psRet;
        }

        //private void ShowCirularProgress(bool show)
        //{
        //    //ShowCirularProgress(circularProgress1, picEditOriginal, show);
        //    //ShowCirularProgress(circularProgress2, picEditThumb, show);
        //}

        //private void UpdateCircularPosition(CircularProgress circular)
        //{
        //    //int x = circular.Parent.Width / 2;
        //    //int y = circular.Parent.Height / 2;
        //    //circular.Location = new System.Drawing.Point(x - circular.Width / 2, y - circular.Height / 2);
        //}

        #endregion

        #region Internal helper functions

        internal void SwitchRecord(SearchResultSingleSummary summary, SearchResultRecord record)
        {
            m_resultSummary = summary;
            Record = record;
        }

        internal void ClearImage()
        {
            this.picEditOriginal.Image = null;
            this.picEditThumb.Image = null;
            this.gridControl1.DataSource = null;
        }

        internal void ShowProperties()
        {
            if (m_properties == null)
            {
                m_properties = GetProperties(m_Record);
            }

            if (m_properties != null && m_properties.Count > 0)
            {
                List<Tuple<string, string>> valList = new List<Tuple<string, string>>();

                Tuple<string, string> t;
                foreach (Tuple<PropertyInfo, string> p in m_properties)
                {
                    object obj = p.Item1.GetValue(m_Record, null);
                    if (obj != null)
                    {
                        t = new Tuple<string, string>(p.Item2, obj.ToString());
                        valList.Add(t);
                    }
                    else
                    {
                        Debug.Assert(false);
                    }
                }

                gridControl1.DataSource = valList;
            }

        }

        internal void Original()
        {
            if (this.picEditOriginal.Image != null)
            {
                picEditOriginal.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Clip;

                ZoomRate = 100;
                picEditOriginal.Properties.ZoomPercent = m_zoomRate;
            }
        }

        internal void ZoomIn()
        {
            picEditOriginal.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Clip;
            ZoomRate = Math.Min((int)(m_zoomRate * 1.2), 1000);
            picEditOriginal.Properties.ZoomPercent = m_zoomRate;
        }

        internal void ZoomOut()
        {
            picEditOriginal.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Clip;
            ZoomRate = Math.Max((int)(m_zoomRate / 1.2), 5);
            picEditOriginal.Properties.ZoomPercent = m_zoomRate;
        }

        internal void AutoFit()
        {
            if (picEditOriginal.Image != null)
            {
                picEditOriginal.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;

                int scaleW = (int)((float)picEditOriginal.Width / (float)picEditOriginal.Image.Width * 100);
                int scaleH = (int)((float)picEditOriginal.Height / (float)picEditOriginal.Image.Height * 100);
                ZoomRate = scaleW > scaleH ? scaleH : scaleW;
            }
        }

        internal void ShowOriginalImage()
        {
            this.picEditOriginal.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;

            OriginalImage = m_Record.OriginalPic;
            ThumbImage = m_Record.ObjectPic;

            AutoFit();
        }


        internal void ShowThumbImage(bool showCircular)
        {
            this.picEditThumb.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;

            // this.picEditThumb.Image = m_Record.OriginalPic;
        }


        internal void ShowImage()
        {
            ShowThumbImage(false);

            //if (picEditThumb.Image == null)
            //{
            //    ShowThumbImage(false);
            //}

            //ShowCirularProgress(circularProgress1, picEditOriginal, false);
            //m_CurImage = this.picEditOriginal.Image = m_Record.Ori; //
            //AutoFit();

            //ICacheItem cacheItem = m_Record as ICacheItem;
            //if (cacheItem != null)
            //{
            //    Framework.Container.Instance.CacheMgr.Register(cacheItem);
            //}
        }

        internal void ClearThumbImage(bool showAnimationPic)
        {
            this.picEditThumb.Image = null;
            // ShowCirularProgress(circularProgress2, picEditThumb, showAnimationPic);
        }

        internal void ClearOriginalImage(bool showAnimationPic)
        {
            this.picEditOriginal.Image = null;
            // ShowCirularProgress(circularProgress1, picEditOriginal, showAnimationPic);
        }

        internal void ClearImage(PictureEdit picEdit, bool showCircular)
        {
            picEdit.Image = null;

            if (showCircular)
            {
                if (picEdit == this.picEditThumb)
                {
                    // ShowCirularProgress(circularProgress2, picEditThumb, showCircular);
                }
                else
                {
                    // ShowCirularProgress(circularProgress1, picEditOriginal, showCircular);
                }
            }
        }

        //internal void ShowCirularProgress(CircularProgress circular, PictureEdit picEdit, bool show)
        //{
        //    picEdit.Visible = !show;

        //    if (show)
        //    {
        //        circular.Parent.BackColor = picEdit.BackColor;
        //        circular.BringToFront();
        //    }

        //    UpdateCircularPosition(circular);
        //    circular.Visible = show;
        //    circular.IsRunning = show;
        //}

        #endregion

        #region Event handlers

        void picEditOriginal_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                ZoomIn();
            else
                ZoomOut();
        }

        private void picEditThumb_SizeChanged(object sender, EventArgs e)
        {
            if (m_Loaded)
            {
                // UpdateCircularPosition(this.circularProgress2);
            }
        }

        private void picEditOriginal_SizeChanged(object sender, EventArgs e)
        {

        }

        private void ucEditImage_Load(object sender, EventArgs e)
        {
            //UpdateCircularPosition(this.circularProgress1);
            //UpdateCircularPosition(this.circularProgress2);
        }

        private void panelControl1_SizeChanged(object sender, EventArgs e)
        {
            // UpdateCircularPosition(this.circularProgress1);
        }

        #endregion

        private void picEditOriginal_Resize(object sender, EventArgs e)
        {
            if (picEditOriginal.Image != null) // && this.ParentForm != null) 
            {
                if (picEditOriginal.Properties.SizeMode == DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom)
                {
                    picEditOriginal.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
                    int scaleW = (int)((float)picEditOriginal.Width / (float)picEditOriginal.Image.Width * 100);
                    int scaleH = (int)((float)picEditOriginal.Height / (float)picEditOriginal.Image.Height * 100);
                    ZoomRate = scaleW > scaleH ? scaleH : scaleW;
                }
            }
        }
    }
}
