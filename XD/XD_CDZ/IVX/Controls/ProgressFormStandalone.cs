using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using BOCOM.IVX.Common;

namespace BOCOM.IVX.Controls
{
    public class ProgressFormStandalone
    {
        #region Fields

        private Form m_ProgressForm;
        private IProgressForm m_Progress;
        private string m_Title;
        private string m_StatusText;

        // private ManualResetEvent m_MRE = new ManualResetEvent(false);
        private bool m_FormLoaded = false;
        private Thread m_Thread;
        private bool m_ModernStyle = false;
        private Icon m_Icon;

        #endregion

        #region Properties

        public string Title
        {
            get
            {
                return m_Title;
            }
            set
            {
                m_Title = value;
            }
        }
        
        public string StatusText
        {
            get { return m_StatusText; }
            set { m_StatusText = value; }
        }
        #endregion

        #region Constructors

        public ProgressFormStandalone()
        {
            
        }

        public ProgressFormStandalone(string title, string status)
            : this()
        {
            this.m_Title = title;
            this.m_StatusText = status;
        }

        public ProgressFormStandalone(string title, string status, bool modernStyle, Icon icon)
            : this(title, status)
        {
            this.m_ModernStyle = modernStyle;
            m_Icon = icon;
        }
        #endregion

        #region Private helper functions

        private void ShowProgressForm()
        {
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Darkroom");

            //if (m_ModernStyle)
            //{
            //    m_ProgressForm = new ProgressFormModern();
            //}
            //else
            //{
                m_ProgressForm = new ProgressForm();
            // }
            m_ProgressForm.Icon = m_Icon;
            m_ProgressForm.StartPosition = FormStartPosition.CenterScreen;
            
            m_Progress = m_ProgressForm as IProgressForm;
            if (!String.IsNullOrEmpty(m_Title))
            {
                m_ProgressForm.Text = m_Title;
            }
            if (!String.IsNullOrEmpty(m_StatusText))
            {
                m_Progress.StatusText = m_StatusText;
            }
            m_ProgressForm.Load += new EventHandler(m_ProgressForm_Load);
            
            m_ProgressForm.ShowDialog();
            m_ProgressForm.Close();
        }

        void m_ProgressForm_Load(object sender, EventArgs e)
        {
            // m_MRE.Set();
            m_FormLoaded = true;
        }

        #endregion

        #region Public helper functions

        public void Show()
        {
            m_Thread = new Thread(ShowProgressForm);
            m_Thread.SetApartmentState(ApartmentState.STA);
            m_Thread.Start();
            while (!m_FormLoaded)
            {
                Thread.Sleep(100);
            }
            // m_MRE.WaitOne();
        }

        public void UpdateProgress(float percent)
        {
            if (percent > 0.01 && percent <= 1.01)
            {
                try
                {
                    if (m_ProgressForm.InvokeRequired)
                    {
                        m_ProgressForm.Invoke(new DelUpdateProgress(UpdateProgress), new object[] { percent });
                    }
                    else
                    {
                        m_Progress.Progress = (int)(m_Progress.Maximum * percent);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, TextUtil.FormatExceptionMsg(ex));
                }
            }
        }

        public void UpdateStatusText(string text)
        {
            if (m_ProgressForm.InvokeRequired)
            {
                m_ProgressForm.Invoke(new DelUpdateStatusText(UpdateStatusText), new object[] { text });
            }
            else
            {
                m_Progress.StatusText = text;
            }
        }
        
        public void Close()
        {
            if (m_ProgressForm.InvokeRequired)
            {
                m_ProgressForm.Invoke(new DelClose(Close));
            }
            else
            {
                m_ProgressForm.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        #endregion

        delegate void DelUpdateProgress(float percent);
        delegate void DelUpdateStatusText(string text);
        delegate void DelClose();
    }
}
