using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using BOCOM.IVX.Properties;

namespace BOCOM.IVX.Controls
{
    public partial class ProgressSplash : Controls.DraggableForm
    {
        #region Fields

        private readonly bool m_ShowCancelBtn;
        private string m_StatusText = String.Empty;
        private readonly Thread m_InvokeThread;
        private readonly object m_ThreadPara;

        #endregion

        #region Constructors

        private ProgressSplash()
        {
            InitializeComponent();
        }

        private ProgressSplash(string statusText, bool showCancelBtn)
            : this()
        {
            this.m_StatusText = statusText;
            this.m_ShowCancelBtn = showCancelBtn;
        }

        public ProgressSplash(string statusText, bool showCancelBtn, Thread invokeThread, object threadPara)
            : this(statusText, showCancelBtn)
        {
            m_InvokeThread = invokeThread;
            m_ThreadPara = threadPara;
        }

        #endregion

        #region Private Methods

        private void InitUI()
        {
            if (String.IsNullOrEmpty(m_StatusText))
            {
                m_StatusText = string.Empty; // Resources.ProgressSplash_Text_Status;
            }
            
            label1.Text = m_StatusText;
            this.btnCancel.Text = Resources.Text_Cancel;
            int offsetTotal = 0;
            if (!m_ShowCancelBtn)
            {
                this.btnCancel.Visible = false;
                offsetTotal = -85;
                this.Size = new Size(this.Width + offsetTotal, this.Height);
                int labelWidth = this.Width - 15;
                this.pictureBox1.Width = labelWidth;
            }
           
            // Size size = TextRenderer.MeasureText(label1.Text, label1.Font);
            int offset = label1.Width - (this.Width - 7);
            if (offset > 0)
            {
                offsetTotal += offsetTotal;
                this.Width = this.Width + offset;
                this.pictureBox1.Width = pictureBox1.Width + offset;
            }

            if (offsetTotal != 0)
            {
                this.Location = new Point(this.Location.X - offsetTotal / 2, this.Location.Y);
            }
        }

        #endregion

        #region Public Methods

        public void SetLocation(Point p)
        {
            Point point = p;
            Screen screen = Screen.FromPoint(p);
            int maxX = screen.Bounds.X + screen.Bounds.Width;
            int maxY = screen.Bounds.Y + screen.Bounds.Height - 32;
            if ((point.X + this.Width) > maxX)
            {
                point.X = maxX - this.Width;
            }
            if ((point.Y + this.Height) > maxY)
            {
                point.Y = maxY - this.Height;
            }

            this.Location = point;
        }

        #endregion

        #region Event handlers

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if(m_InvokeThread != null)
            {
                try
                {
                    m_InvokeThread.Abort();
                }
                catch (ThreadAbortException)
                {
                    Debug.WriteLine("Abort m_InvokeThread ocurr exception.");
                }
            }
            this.DialogResult = DialogResult.Abort;
        }

        private void ProgressSplash_Load(object sender, EventArgs e)
        {
            InitUI();

            if(m_InvokeThread != null)
            {
                m_InvokeThread.Start(m_ThreadPara);
            }
        }

        #endregion
    }
}