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
    public partial class ucReportMenagement : UserControl
    {
        public ucReportMenagement()
        {
            InitializeComponent();
        }
        public void InitWnd()
        {
            ucCurrentUser1.InitWnd();
            superTabControlPanel0.BackgroundImage = Framework.Environment.DefaultImage;
            superTabControl1.SelectedTab = superTabItem0;

            switch (Framework.Environment.UserType)
            {
                case 1:
                    break;
                case 2:
                case 3:
                    btnTradingRecord.Enabled = false;
                    btnCardRecord.Enabled = false;
                    break;
                case 4:
                    break;
                default:
                    break;
            }

        }
        private void UnCheckAllButton()
        {
            btnRegionStat.Checked = false;
            btnCardRecord.Checked = false;
            btnTradingRecord.Checked = false;

        }

        private void btnTradingRecord_Click(object sender, EventArgs e)
        {
            UnCheckAllButton();
            btnTradingRecord.Checked = true;

            superTabControl1.SelectedTab = superTabItem1;

        }

        private void btnCardRecord_Click(object sender, EventArgs e)
        {
            UnCheckAllButton();
            btnCardRecord.Checked = true;
            superTabControl1.SelectedTab = superTabItem3;

        }

        private void btnRegionStat_Click(object sender, EventArgs e)
        {
            UnCheckAllButton();
            btnRegionStat.Checked = true;
            superTabControl1.SelectedTab = superTabItem2;
            ucRegionStatRecordPanel1.InitWnd();
        }

    }
}
