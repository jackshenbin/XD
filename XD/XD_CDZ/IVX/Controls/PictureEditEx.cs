using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BOCOM.IVX.Controls
{
    public partial class PictureEditEx : System.Windows.Forms.PictureBox
    {
        public PictureEditEx()
        {
            InitializeComponent();
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            // TODO: 在此处添加自定义绘制代码

            // 调用基类 OnPaint
            try
            {
                base.OnPaint(pe);
            }
            catch
            {
                this.Invalidate();
            }
        }
    }
}
