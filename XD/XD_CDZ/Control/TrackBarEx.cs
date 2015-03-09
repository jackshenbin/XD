using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace BOCOM.IVX.Controls
{
    public partial class TrackBarEx : UserControl
    {
        private int _Max = 100;//进度条的最大值

        private int _Mth = 1;//进度条现在的值

        public int MaxValue
        {
            get
            {
                return this._Max;
            }
            set
            {
                if (value <= 0)
                    this._Max = 100;
                else
                    this._Max = value;
                this.SetBlood();
            }
        }

        public int Value
        {
            get
            {
                return this._Mth;
            }
            set
            {
                if (value >= this._Max) 
                    this._Mth = this._Max;
                else if (value <= 0) 
                    this._Mth = 0;
                else
                    this._Mth = value;
                this.SetBlood();
            }
        }


        public event EventHandler ValueChanged;
        public event EventHandler ValueChangedByMouse;

        private IContainer components = null;

        public TrackBarEx()
        {
            this.InitializeComponent();
        }

        private void TrackBarEx_Load(object sender, EventArgs e)
        {
            this.SetBlood();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TrackBarEx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::BOCOM.IVX.Controls.Properties.Resources.进度背景;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.DoubleBuffered = true;
            this.Name = "TrackBarEx";
            this.Size = new System.Drawing.Size(754, 7);
            this.Load += new System.EventHandler(this.TrackBarEx_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Blood_MouseDown);
            this.MouseEnter += new System.EventHandler(this.Blood_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.Blood_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Blood_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Blood_MouseUp);
            this.EnabledChanged += new EventHandler(TrackBarEx_EnabledChanged);
            this.ResumeLayout(false);

        }

        void TrackBarEx_EnabledChanged(object sender, EventArgs e)
        {
            if (!Enabled)
            {
                this.BackgroundImage.Dispose();
                this.BackgroundImage = global::BOCOM.IVX.Controls.Properties.Resources.进度背景;
                _Max = 100;
                _Mth = 0;
            }
        }

        public void SetBlood()
        {
            if (!Enabled)
                return;

            if (this.Width <= 0) 
                return;

            if (base.IsHandleCreated)
            {
                if (this.BackgroundImage != null)
                {
                    this.BackgroundImage.Dispose();
                    this.BackgroundImage = global::BOCOM.IVX.Controls.Properties.Resources.进度背景;
                }
                using (Graphics graphics = Graphics.FromImage(this.BackgroundImage))
                {
                    double zoom = (double)Properties.Resources.进度背景.Width / this.Width;
                    if (isMouseDown)
                    {
                        this._Mth = startPoint.X * this._Max / this.Width;
                        if (this._Mth >= this._Max) this._Mth = this._Max;
                        if (this._Mth <= 0) this._Mth = 0;
                        double num = ((double)this._Mth) / (this._Max * 1.0);//计算位置

                        graphics.DrawImage(Properties.Resources.进度,
                            new Rectangle(0, 2, Convert.ToInt32(zoom * (this.Width * num)), 2/*尺寸*/),
                            new Rectangle(0, 0, Properties.Resources.进度.Width, Properties.Resources.进度.Height),
                            GraphicsUnit.Pixel);
                        graphics.DrawImage(Properties.Resources.滑块,
                            new Rectangle(Convert.ToInt32(zoom * (this.Width * num - 7)), 0, Convert.ToInt32(zoom * 14), 7/*尺寸*/),
                            new Rectangle(0, 0, Properties.Resources.滑块.Width, Properties.Resources.滑块.Height),
                            GraphicsUnit.Pixel);
                    }
                    else
                    {
                        if (this._Max > 0 && this._Mth <= this._Max)
                        {
                            double num = ((double)this._Mth) / (this._Max * 1.0);//计算位置
                            graphics.DrawImage(Properties.Resources.进度,
                                new Rectangle(0, 2, Convert.ToInt32(zoom * (this.Width * num)), 2/*尺寸*/),
                                new Rectangle(0, 0, Properties.Resources.进度.Width, Properties.Resources.进度.Height),
                                GraphicsUnit.Pixel);
                            if (!isMouseOver)
                            {
                                graphics.DrawImage(Properties.Resources.进度滑块,
                                    new Rectangle(Convert.ToInt32(zoom * (this.Width * num - 7)), 2, Convert.ToInt32(zoom * 14), 2/*尺寸*/),
                                    new Rectangle(0, 0, Properties.Resources.进度滑块.Width, Properties.Resources.进度滑块.Height),
                                    GraphicsUnit.Pixel);
                            }
                            else
                            {
                                graphics.DrawImage(Properties.Resources.滑块,
                                    new Rectangle(Convert.ToInt32(zoom * (this.Width * num - 7)), 0, Convert.ToInt32(zoom * 14), 7/*尺寸*/),
                                    new Rectangle(0, 0, Properties.Resources.滑块.Width, Properties.Resources.滑块.Height),
                                    GraphicsUnit.Pixel);
                            }
                        }
                    }
                }
            }
        }
        bool isMouseOver = false;
        Point startPoint = new Point();
        private void Blood_MouseMove(object sender, MouseEventArgs e)
        {
            isMouseOver = true;
            if (isMouseDown) startPoint = e.Location;

            SetBlood();
            this.Cursor = Cursors.Hand;
        }

        private void Blood_MouseLeave(object sender, EventArgs e)
        {
            isMouseOver = false;
            SetBlood();
            this.Cursor = Cursors.Default;

        }

        private void Blood_MouseEnter(object sender, EventArgs e)
        {
            isMouseOver = true;
            SetBlood();
            this.Cursor = Cursors.Hand;

        }
        bool isMouseDown = false;
        private void Blood_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            startPoint = e.Location;
            SetBlood();

        }

        private void Blood_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            startPoint = new Point();
            SetBlood();
            if (ValueChangedByMouse != null) ValueChangedByMouse(this._Mth, e);
        }

    }
}
