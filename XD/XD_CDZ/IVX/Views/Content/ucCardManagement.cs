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
    public partial class ucCardManagement : UserControl
    {
        public ucCardManagement()
        {
            InitializeComponent();
        }

        private void btnReCharge_Click(object sender, EventArgs e)
        {
            superTabControl1.SelectedTab = superTabItem1;
            ucReChargePanel1.InitWnd();
        }

        private void btnWalletCharge_Click(object sender, EventArgs e)
        {
            superTabControl1.SelectedTab = superTabItem2;
            ucWalletChargePanel1.InitWnd();
        }

        private void btnAddCard_Click(object sender, EventArgs e)
        {
            superTabControl1.SelectedTab = superTabItem7;
            ucAddCardPanel1.InitWnd();
        }

        private void btnQuitCard_Click(object sender, EventArgs e)
        {
            superTabControl1.SelectedTab = superTabItem8;
            ucQuitCardPanel1.InitWnd();
        }

        private void ucCardManagement_Load(object sender, EventArgs e)
        {
            superTabControlPanel0.BackgroundImage = Framework.Environment.DefaultImage;

        }

        private void buttonCardStat_Click(object sender, EventArgs e)
        {
            superTabControl1.SelectedTab = superTabItem6;
            ucCardStatPanel1.InitWnd();
        }
        public void InitWnd()
        {
            ucCurrentUser1.InitWnd();
            superTabControl1.SelectedTab = superTabItem0;
            switch (Framework.Environment.UserType)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                case 4:
                    btnAddCard.Enabled = false;
                    btnReCharge.Enabled = false;
                    btnQuitCard.Enabled = false;
                    btnWalletCharge.Enabled = false;
                    break;
                default:
                    break;
            }

        }

        private void btnCardRecord_Click(object sender, EventArgs e)
        {
            superTabControl1.SelectedTab = superTabItem9;

        }

        private void expandablePanel1_Click(object sender, EventArgs e)
        {

        }

        private void btnWalletChargeBack_Click(object sender, EventArgs e)
        {
            superTabControl1.SelectedTab = superTabItem3;
            ucWalletChargeBackPanel1.InitWnd();
        }

        private void btnCharge_Click(object sender, EventArgs e)
        {
            superTabControl1.SelectedTab = superTabItem4;
            ucChargePanel1.InitWnd();
        }

        private void btnWalletReCharge_Click(object sender, EventArgs e)
        {
            superTabControl1.SelectedTab = superTabItem5;
            ucWalletReChargePanel1.InitWnd();
        }

        private void btnStatChanege_Click(object sender, EventArgs e)
        {
            superTabControl1.SelectedTab = superTabItem10;
            ucCardStatChangePanel1.InitWnd();

        }

    }
}
