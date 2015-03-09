using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BOCOM.DataModel;
using DevExpress.XtraEditors;

namespace BOCOM.IVX.Controls
{
    public partial class ucSearchResultItem : UserControl
    {
        public event EventHandler Pic_Click;
        public event EventHandler Pic_DoubleClick;
        public event MouseEventHandler Pic_MouseUp;
        public event EventHandler<MouseEventArgs> Pic_MouseDown;

        protected bool isDoDrag = false;
        protected SearchResultRecord m_SearchResultRecord;
        protected System.Drawing.Point startPoint = new System.Drawing.Point();

        private bool m_Checked;

        public bool Checked
        {
            get
            {
                return m_Checked;
            }
            set
            {
                if (m_Checked != value)
                {
                    m_Checked = value;
                    Invalidate();
                }
            }
        }

        public ucSearchResultItem()
        {
            InitializeComponent();
        }

        public void Show(SearchResultRecord item)
        {
            m_SearchResultRecord = item;
            this.pictureBox1.Image = item.ThumbNailPic;
            this.labelControl1.Text = string.Format("目标类型 {0}", Common.Utils.GetSearchObjectTypeName(item.ObjectType));
            this.labelControl2.Text = item.TargetTs.ToString(DataModel.Constant.DATETIME_FORMAT);
        }

        public void UnInit()
        {
            this.pictureBox1.Image = null;
            m_SearchResultRecord = null;
            MyLog4Net.Container.Instance.Log.DebugFormat("ucSearchResultItem disposed: ", this.GetHashCode());
            
            this.GiveFeedback -= new System.Windows.Forms.GiveFeedbackEventHandler(this.pictureBox1_GiveFeedback);
            this.pictureBox1.Click -= new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.GiveFeedback -= new System.Windows.Forms.GiveFeedbackEventHandler(this.pictureBox1_GiveFeedback);
            this.pictureBox1.DoubleClick -= new System.EventHandler(this.pictureBox1_DoubleClick);
            this.pictureBox1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);

            this.PreviewKeyDown -= new System.Windows.Forms.PreviewKeyDownEventHandler(this.ucSearchResultItem_PreviewKeyDown);
            this.Resize -= new System.EventHandler(this.ucSearchResultItem_Resize);

            this.Controls.Clear();

            labelControl1.Dispose();
            labelControl1 = null;

            labelControl2.Dispose();
            labelControl2 = null;

            this.pictureBox1.Dispose();
            this.pictureBox1 = null;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            System.Drawing.Pen p = null;
            if (Checked)
            {
                p = new Pen(new SolidBrush(Color.LightGray));
                p.Width = 2;
            }
            else
            {
                p = new Pen(new SolidBrush(Color.Gray));
                p.Width = 1;
            }

            Graphics g = e.Graphics;
            g.DrawRectangle(p, new Rectangle(1, 1, this.Width - 3, this.Height - 3));
            p.Dispose();

            base.OnPaint(e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (this.Pic_Click != null)
            {
                Pic_Click(this, e);
            }
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (Pic_DoubleClick != null)
            {
                Pic_DoubleClick(this, e);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.Pic_MouseDown != null)
            {
                isDoDrag = true;
                startPoint = e.Location;

                Pic_MouseDown(this, e);
            }
        }

        private void ucSearchResultItem_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            
        }

        private void ucSearchResultItem_Resize(object sender, EventArgs e)
        {
            Common.UIUtil.HorizontalCentralizeControl(this, labelControl1);
            Common.UIUtil.HorizontalCentralizeControl(this, labelControl2);
        }

        private void pictureBox1_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = false;
            Cursor.Current = Common.Constant.CameraCursor;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isDoDrag = false;
            if (Pic_MouseUp != null)
            {
                Pic_MouseUp(this, e);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_SearchResultRecord != null &&
                e.Button == System.Windows.Forms.MouseButtons.Left && isDoDrag)
            {
                if (Math.Abs(startPoint.X - e.Location.X) > 5 || Math.Abs(startPoint.Y - e.Location.Y) > 5)
                {
                    DoDragDrop(m_SearchResultRecord, DragDropEffects.Move | DragDropEffects.Link);
                }
            }
        }
    }
}
