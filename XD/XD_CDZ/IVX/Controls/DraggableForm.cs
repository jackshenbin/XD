using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace BOCOM.IVX.Controls.Controls
{
    public partial class DraggableForm : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        public DraggableForm()
        {
            InitializeComponent();
        }

        protected void Form_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        protected void Form_MouseMove(object sender, MouseEventArgs e)
        { 
            //if (m_MouseDown)
            //{
            //    this.Top = Control.MousePosition.Y - m_PointMouseDown.Y;
            //    this.Left = Control.MousePosition.X - m_PointMouseDown.X;
            //}
        }

        protected void Form_MouseUp(object sender, MouseEventArgs e)
        {
           //m_MouseDown = false;
        }
    }
}