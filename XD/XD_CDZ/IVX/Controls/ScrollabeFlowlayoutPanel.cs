using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Diagnostics;

namespace BOCOM.IVX.Controls
{
    public partial class ScrollabeFlowlayoutPanel : XtraScrollableControl
    {
        #region Fields

        public const int PICWIDTH_MIN = 100;
        public const int PICWIDTH_MAX = 130;

        public const int PADDING = 1;
        public const int MARGIN = 2;

        private int m_lastPicWidth;

        #endregion

        #region Properties

        public bool WrapContents
        {
            get
            {
                return this.flowLayoutPanel1.WrapContents;
            }
            set
            {
                this.flowLayoutPanel1.WrapContents = value;
            }
        }

        public AnchorStyles FlowlayoutPanelAnchor
        {
            get
            {
                
                return flowLayoutPanel1.Anchor;
            }
            set
            {
                flowLayoutPanel1.Anchor = value;
            }
        }

        public ControlCollection ControlsInFlowlayoutPanel
        {
            get
            {
                return flowLayoutPanel1.Controls;
            }
        }

        #endregion

        #region Constructors

        public ScrollabeFlowlayoutPanel()
        {
            InitializeComponent();
        }

        #endregion

        #region Public helper functions

        public void Init()
        {
            flowLayoutPanel1.Width = this.Width;
            this.Resize += new EventHandler(HScrollabeFlowlayoutPanel_Resize);
            //this.SetAutoSizeMode(AutoSizeMode.GrowOnly);
            if (flowLayoutPanel1.WrapContents)
            {
                // flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            }
        }

        public void UnInit()
        {
            this.Resize -= new EventHandler(HScrollabeFlowlayoutPanel_Resize);
            this.flowLayoutPanel1.Dispose();
            this.flowLayoutPanel1 = null;
        }

        public void AddControl(Control control, bool adjustContainerSize = false)
        {
            if (control != null)
            {
                this.flowLayoutPanel1.Controls.Add(control);

                if (flowLayoutPanel1.Controls.Count > 1)
                {
                    control.Size = flowLayoutPanel1.Controls[0].Size;
                }

                if (adjustContainerSize)
                {
                    AdjustFlowlayoutPanelSize();
                }
            }
        }

        public void AdjustFlowlayoutPanelSize()
        {
            if (flowLayoutPanel1.Controls.Count > 0)
            {
                if (WrapContents)
                {
                    AdjustFlowlayoutPanelHeight();
                }
                else
                {
                    if (flowLayoutPanel1.Controls.Count == 1)
                    {
                        this.flowLayoutPanel1.Height = flowLayoutPanel1.Controls[0].Height + 5;
                    }
                    Control control = flowLayoutPanel1.Controls[flowLayoutPanel1.Controls.Count - 1];
                    this.flowLayoutPanel1.Width = control.Right + PADDING;
                    if (this.flowLayoutPanel1.Width > this.Width)
                    {
                        this.HorizontalScroll.Visible = true;
                    }
                    else
                    {
                        this.HorizontalScroll.Visible = false;
                    }
                    this.VerticalScroll.Visible = false;
                }
            }
        }

        public void RemoveControl(Control control)
        {
            if (control != null && this.flowLayoutPanel1.Controls.Contains(control))
            {
                this.flowLayoutPanel1.Controls.Remove(control);
                if (WrapContents)
                {
                    AdjustFlowlayoutPanelHeight();
                }
                else
                {
                    this.flowLayoutPanel1.Width = control.Right + PADDING;
                }
            }
        }

        public void ClearControls()
        {
            if (flowLayoutPanel1.Controls.Count > 0)
            {
                this.ScrollControlIntoView(this.flowLayoutPanel1.Controls[0]);
                ControlCollection ctrls = flowLayoutPanel1.Controls;
                
                if (ctrls != null)
                {
                    MyLog4Net.Container.Instance.Log.DebugFormat("ucSearchResultItem.ClearControls: {0}", ctrls.Count);
                    int index = 0;
                    int total = ctrls.Count;
                    ucSearchResultItem ucSearchResultItem;
                    foreach (Control ctrl in ctrls)
                    {
                        ucSearchResultItem = ctrl as ucSearchResultItem;
                        if (ucSearchResultItem != null)
                        {
                            // MyLog4Net.Container.Instance.Log.DebugFormat("Calling ucSearchResultItem.Dispose {0}/{1}", index++, total);
                            ucSearchResultItem.UnInit();
                        }
                    }
                }
                this.flowLayoutPanel1.Controls.Clear();
            }

            if (WrapContents)
            {
                AdjustFlowlayoutPanelHeight();
            }
            else
            {
                this.flowLayoutPanel1.Width = 0; 
            }
        }

        #endregion

        #region Private helper functions

        private void AdjustFlowlayoutPanelHeight()
        {
            int bottom = 0;
            foreach (Control pic in flowLayoutPanel1.Controls)
            {
                if (bottom == 0)
                {
                    bottom = pic.Height + 2;
                }
                else if (bottom < pic.Bottom + 2)
                {
                    bottom = pic.Bottom + 2;
                }
            }
            flowLayoutPanel1.Height = bottom;
        }

        private int GetPicCtrlCountinOneRow()
        {
            int count = 0;

            int y = flowLayoutPanel1.Controls[0].Top;
            for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                if (flowLayoutPanel1.Controls[i].Top == y)
                {
                    count++;
                }
            }
            Debug.Assert(count != 0);
            MyLog4Net.Container.Instance.Log.Debug("GetPicCtrlCountinOneRow count =" + count.ToString());

            return count;
        }

        /// <summary>
        /// 根据空白宽度， 计算每个子控件的新宽度
        /// </summary>
        /// <param name="currentWidth"></param>
        /// <param name="itemCount"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="blank"></param>
        /// <returns></returns>
        private int CaculateNewWidth(int itemWidth, int containerWidth, int itemCount)
        {
            int width = itemWidth;
            int countPerRow = containerWidth / itemWidth;
            if (countPerRow <= 1)
            {
                // 只能容纳小于等于1个
                if (containerWidth > itemWidth)
                {
                    // 大于1个, 取 PICWIDTH_MAX , containerWidth 的小者
                    width = containerWidth > PICWIDTH_MAX ? PICWIDTH_MAX : containerWidth;
                }
                else
                {
                    // 小于一个的宽度， 取PICWIDTH_MIN , containerWidth 的大者
                    width = containerWidth > PICWIDTH_MIN ? containerWidth : PICWIDTH_MIN;
                }
            }
            else
            {
                int countPerRowMax = containerWidth / PICWIDTH_MIN;
                int countPerRowMin = containerWidth / PICWIDTH_MAX;
                if (countPerRowMin > itemCount || countPerRowMax == countPerRowMin)
                {
                    // 一行够放下全部， 或者最小宽度和最大宽度能放的Item 一样的多
                    width = PICWIDTH_MAX;
                }
                else
                {
                    // 放小的比放大的能多放， 不能按照item 最大宽度放
                    int blankTmp = 0;
                    int offset = 0;
                    if (countPerRowMax > itemCount)
                    {
                        //  一行可以全部放下
                        blankTmp = containerWidth - PICWIDTH_MIN * itemCount;
                        offset = blankTmp / itemCount;
                    }
                    else
                    {
                        // 如果放不下全部， 直接求余
                        blankTmp = containerWidth % PICWIDTH_MIN;
                        offset = blankTmp / countPerRowMax;
                    }

                    // 余量平分到每个Item
                    
                    int widthTmp = PICWIDTH_MIN + offset;
                    width = widthTmp > PICWIDTH_MAX ? PICWIDTH_MAX : widthTmp;
                    width = width - 4;
                }
            }

            return width;
        }

        internal void UpdatePicSize()
        {
            flowLayoutPanel1.HorizontalScroll.Visible = false;
            this.HorizontalScroll.Visible = false;
            // Debug.Assert(!this.HorizontalScroll.Visible && !this.flowLayoutPanel1.HorizontalScroll.Visible);

            int count = this.flowLayoutPanel1.Controls.Count;

            if (count > 0)
            {
                int offset = 2 * PADDING;
                System.Drawing.Size picSize = this.flowLayoutPanel1.Controls[0].Size;
                int ctrlCountInOneRow = GetPicCtrlCountinOneRow();
                int actualWidth = (picSize.Width + MARGIN) * ctrlCountInOneRow - MARGIN;
                int blank = flowLayoutPanel1.Width - offset - actualWidth;

                if (blank > 5)
                {
                    Debug.WriteLine(string.Format("VASnapshotList.UpdatePicSize: blank: {0}", blank));
                    int newWidth = picSize.Width + blank / ctrlCountInOneRow;
                    bool matched = false;
                    if (newWidth <= PICWIDTH_MAX)
                    {
                        Debug.WriteLine(string.Format("VASnapshotList.UpdatePicSize: blank: {0}, exceeding PICWIDTH_MAX if not increasing count in one row", blank));
                        matched = true;
                    }
                    else
                    {
                        Debug.WriteLine(string.Format("VASnapshotList.UpdatePicSize: blank: {0}, consider increasing count in one row", blank));
                        if (ctrlCountInOneRow < flowLayoutPanel1.Controls.Count)
                        {
                            newWidth = CaculateNewWidth(picSize.Width, flowLayoutPanel1.Width - 4, flowLayoutPanel1.Controls.Count);    // ((flowLayoutPanel1.Width - offset + MARGIN) / (ctrlCountInOneRow + 1) - MARGIN);
                            matched = true;
                        }
                    }
                    if (matched)
                    {
                        m_lastPicWidth = newWidth;
                        foreach (Control pic in flowLayoutPanel1.Controls)
                        {
                            pic.Width = newWidth;
                        }
                    }
                    Debug.WriteLine(string.Format("VASnapshotList.UpdatePicSize: blank: {0}, matched: {1}, newWidth: {2}", blank, matched, newWidth));
                }
               

                // 调整flowlayoutpanel的高度
                int bottom = 0;
                foreach (Control pic in flowLayoutPanel1.Controls)
                {
                    if (bottom == 0)
                    {
                        bottom = pic.Height + 2;
                    }
                    else if (bottom < pic.Bottom + 2)
                    {
                        bottom = pic.Bottom + 2;
                    }
                }
                flowLayoutPanel1.Height = bottom;
            }

            flowLayoutPanel1.HorizontalScroll.Visible = false;
            this.HorizontalScroll.Visible = false;

            // Debug.Assert(!this.HorizontalScroll.Visible && !this.flowLayoutPanel1.HorizontalScroll.Visible);
        }

        internal void ForceResize()
        {
            this.HScrollabeFlowlayoutPanel_Resize(this, EventArgs.Empty);
        }

        #endregion

        #region Event handlers

        void HScrollabeFlowlayoutPanel_Resize(object sender, EventArgs e)
        {
            if (flowLayoutPanel1.WrapContents)
            {
                int widthOri = flowLayoutPanel1.Width;
                flowLayoutPanel1.Width = 10;

                if (this.flowLayoutPanel1.Height > this.Height)
                {
                    this.VerticalScroll.Visible = true;
                }
                else
                {
                    this.VerticalScroll.Visible = false;
                }

                if (this.VerticalScroll.Visible)
                {
                    flowLayoutPanel1.Width = this.Width - 19; // -30; // 19;
                }
                else
                {
                    flowLayoutPanel1.Width = this.Width - 2;
                }
                //if (widthOri != flowLayoutPanel1.Width)
                //{
                    UpdatePicSize();
                // }
            }
            else
            {
                int widthOri = flowLayoutPanel1.Width;
                flowLayoutPanel1.Height = 10;
                if (this.HorizontalScroll.Visible)
                {
                    flowLayoutPanel1.Height = this.Height - 19;
                }
                else
                {
                    flowLayoutPanel1.Height = this.Height - 2;
                }
                this.VerticalScroll.Visible = false;
            }
        }

        #endregion
    }
}
