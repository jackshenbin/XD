using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BOCOM.IVX.Views.Content
{
    public partial class ucMainPage : UserControl
    {
        public ucMainPage()
        {
            InitializeComponent();
        }

        public event EventHandler SwitchWnd;

        private void btnDeviceManagement_Click(object sender, EventArgs e)
        {
            if (SwitchWnd != null)
                SwitchWnd(BOCOM.DataModel.E_SWITCH_WND.SWITCH_WND_DEVICE_MENAGEMENT, e);
        }

        private void btnCardManagement_Click(object sender, EventArgs e)
        {
            if (SwitchWnd != null)
                SwitchWnd(BOCOM.DataModel.E_SWITCH_WND.SWITCH_WND_CARD_MENAGEMENT, e);
        }

        private void btnReportManagement_Click(object sender, EventArgs e)
        {
            if (SwitchWnd != null)
                SwitchWnd(BOCOM.DataModel.E_SWITCH_WND.SWITCH_WND_REPORT_MENAGEMENT, e);
        }

        private void btnUserMamagement_Click(object sender, EventArgs e)
        {
            if (SwitchWnd != null)
                SwitchWnd(BOCOM.DataModel.E_SWITCH_WND.SWITCH_WND_USER_MENAGEMENT, e);
        }

        private void ucMainPage_Load(object sender, EventArgs e)
        {
            splitContainer1.Panel2.BackgroundImage = Framework.Environment.DefaultImage;
        }

        public void InitWnd()
        {
            ucCurrentUser1.InitWnd();
        }

    }
}
