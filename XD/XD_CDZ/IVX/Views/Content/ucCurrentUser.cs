using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BOCOM.IVX.Views.Content
{
    public partial class ucCurrentUser : UserControl
    {
        public ucCurrentUser()
        {
            InitializeComponent();
        }

        private void ucCurrentUser_Load(object sender, EventArgs e)
        {
            //label1.Text = "您好！"+Framework.Environment.UserName;
            this.BackgroundImage = global::BOCOM.IVX.Properties.Resources.userheadbk;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

        }

        public void InitWnd()
        {
            label1.Text = "您好！" + Framework.Environment.UserName;
        }
    }
}
