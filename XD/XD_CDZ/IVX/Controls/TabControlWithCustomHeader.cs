using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraTab;
using DevExpress.XtraEditors;

namespace BOCOM.IVX.Controls
{
    public partial class TabControlWithCustomHeader : UserControl
    {
        private SimpleButton[] m_HeadButtons;
        private SimpleButton m_btnLast;

        private readonly static Color S_SelectedButtonColor = Color.FromArgb(117, 117, 117);
        private readonly static Color S_NoneSelectedButtonColor = Color.FromArgb(57, 57, 57);

        public bool ShowHeader
        {
            get
            {
                return this.xtraTabControl1.Dock != DockStyle.Fill;
            }
            set
            {
                if (!value)
                {
                    this.xtraTabControl1.Dock = DockStyle.Fill;
                }
            }
        }

        protected XtraTabControl TabControl
        {
            get
            {
                return xtraTabControl1;
            }
        }

        public TabControlWithCustomHeader()
        {
            InitializeComponent();
            m_HeadButtons = new SimpleButton[]
            {
                simpleButton1,
                simpleButton2,
                simpleButton3,
                simpleButton4,
                simpleButton5,
                simpleButton6,
            };
            xtraTabControl1.SelectedPageChanged += new TabPageChangedEventHandler(xtraTabControl1_SelectedPageChanged);

        }

        public void AddPage(XtraTabPage page)
        {
            int index = xtraTabControl1.TabPages.Count;
            if (index < m_HeadButtons.Length)
            {
                m_HeadButtons[index].Tag = page;
                m_HeadButtons[index].Visible = true;
                m_HeadButtons[index].Text = page.Text;
                this.xtraTabControl1.TabPages.Add(page);
            }
            if (m_btnLast == null)
            {
                m_btnLast = m_HeadButtons[0];
                m_btnLast.Appearance.BackColor = m_btnLast.Appearance.BackColor2 = S_SelectedButtonColor;
            }
        }

        void button_Click(object sender, EventArgs e)
        {
            SimpleButton btn = (SimpleButton)sender;
            this.xtraTabControl1.SelectedTabPage = btn.Tag as XtraTabPage;
            if (btn != m_btnLast)
            {
                btn.Appearance.BackColor = btn.Appearance.BackColor2 = S_SelectedButtonColor;
                if (m_btnLast != null)
                {
                    m_btnLast.Appearance.BackColor = m_btnLast.Appearance.BackColor2 = S_NoneSelectedButtonColor;
                }
                m_btnLast = btn;
            }
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            if (m_btnLast != null)
            {
                m_btnLast.Appearance.BackColor = m_btnLast.Appearance.BackColor2 = S_NoneSelectedButtonColor;
            }

            foreach (SimpleButton btn in m_HeadButtons)
            {
                if (btn.Tag == e.Page)
                {
                    btn.Appearance.BackColor2 = S_SelectedButtonColor;
                    m_btnLast = btn;
                    break;
                }
            }
        }

    }
}
