using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace BOCOM.IVX.Controls
{
    public partial class AniPicture : UserControl
    {

        private static Image s_ImageResultBackGround;
        private static Bitmap s_BitmapResultBackGround;
        private static AnimatorImage s_AnimatorImageResultBackGround;

        public AniPicture()
        {
            InitializeComponent();

            if (s_ImageResultBackGround == null)
            {
                s_ImageResultBackGround = Properties.Resources.结果背景;
                s_BitmapResultBackGround = (Bitmap)s_ImageResultBackGround;
                s_AnimatorImageResultBackGround = new AnimatorImage(s_BitmapResultBackGround);
            }

            ias = s_AnimatorImageResultBackGround;// 获取资源，实例化动画类
            Image = s_BitmapResultBackGround;

            // 调试的时候发现 “下一页” 用s_ImageResultBackGround 赋值， 会导致异常， 改为使用 Properties.Resources.结果背景
            pictureBox1.Image = Properties.Resources.结果背景; // s_ImageResultBackGround;

            this.Size = new Size(100, 100);
            pictureBox1.MouseMove += new MouseEventHandler(pictureBox1_MouseMove);
            pictureBox1.MouseDown += new MouseEventHandler(pictureBox1_MouseDown);
            pictureBox1.MouseUp += new MouseEventHandler(pictureBox1_MouseUp);
            pictureBox1.MouseHover += new EventHandler(pictureBox1_MouseHover);
        }

        void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            this.OnMouseHover(e);
        }

        public Bitmap Image = null;
        public event EventHandler Pic_Click;
        public event EventHandler Pic_DoubleClick;
        public event MouseEventHandler Pic_MouseMove;
        public event MouseEventHandler Pic_MouseUp;
        public event MouseEventHandler Pic_MouseDown;
        private bool isChecked = false;

        private string m_toolTipContent;

        public bool Checked
        {
            get { return isChecked; }
            set
            {
                isChecked = value; 
                if (isChecked)
                {
                    if (BackColor != Color.LightGray) BackColor = Color.LightGray;
                    if (Pic_Click != null) Pic_Click(this, EventArgs.Empty);
                }
                else
                {
                    if (BackColor != Color.Gray) BackColor = Color.Gray;
                }
            }
        }

        public string ToolTipContent
        {
            get
            {
                return m_toolTipContent;
            }
            set
            {
                m_toolTipContent = value;
            }
        }

        public void StartAniPicture(Bitmap img)
        {
            pictureBox1.Image =Image = img;
            //pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            //ias = new AnimatorImage(img); // 获取资源，实例化动画类
            //ias.Delay = 5;
            //Image = img; 
            ////pictureBox1.Image = img;
            //// 通过委托在不同线程间访问控件
            //ias.DrawStarted += new EventHandler(ias_DrawStarted);
            //ias.DrawCompleted += new EventHandler(ias_DrawCompleted);
            //// Invalidate()方法底层并不涉及控件界面，只是发送消息，因此可以在不同线程间调用，即它是线程安全的
            //ias.Redraw += (s, e) => this.pictureBox1.Invalidate(e.ClipRectangle);

            //ias.DrawAnimator(animatorType);

        }

        public void DisposeAniPicture()
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }
            pictureBox1.Image = Image = null;
        }

        /// <summary>
        /// 绘制开始事件。
        /// </summary>
        public event EventHandler DrawStarted;
        /// <summary>
        /// 绘制完成事件。
        /// </summary>
        public event EventHandler DrawCompleted;

        void ias_DrawCompleted(object sender, EventArgs e)
        {
            if (DrawStarted != null)
            {
                DrawStarted.Invoke(sender, e);
            }
        }

        void ias_DrawStarted(object sender, EventArgs e)
        {
            if (DrawCompleted != null)
            {
                DrawCompleted.Invoke(sender, e);
            }
        }

        #region 私有字段

        private AnimatorImage ias; // 动画类实例引用
        private AnimateType animatorType = AnimateType.Animator10;// 动画类型


        #endregion
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // 将动画类中的内存输出位图绘制到DC上
            //e.Graphics.DrawImage(ias.OutBmp,  e.ClipRectangle, e.ClipRectangle, GraphicsUnit.Pixel);

        } 

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // if (Pic_Click != null) Pic_Click(this, e);
            Checked = true;
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (Pic_DoubleClick != null) Pic_DoubleClick(this, e);

        }

        private void button1_Enter(object sender, EventArgs e)
        {
        }

        private void button1_Leave(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(!isChecked) this.BackColor =   Color.DarkGray;
            if (Pic_MouseMove != null) Pic_MouseMove(this, e);
            //this.toolTip1.SetToolTip(pictureBox1, m_toolTipContent);
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            if(!isChecked) this.BackColor =   Color.Gray;

        }
        void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (Pic_MouseUp != null) Pic_MouseUp(this, e);
        }

        void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (Pic_MouseDown != null) Pic_MouseDown(this, e);
        }



    }
}
