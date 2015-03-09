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
    public partial class ucLoadingStatus : UserControl
    {
        private bool m_Running = true;

        [Bindable(true)]
        public string StatusText
        {
            get
            {
                return this.label1.Text;
            }
            set
            {
                this.label1.Text = value;
            }
        }

        [Bindable(true)]
        public bool Running
        {
            get
            {
                return this.m_Running;
            }
            set
            {
                m_Running = value;
                this.pictureBox1.Visible = value;
            }
        }

        public ucLoadingStatus()
        {
            InitializeComponent();
        }
    }
}
