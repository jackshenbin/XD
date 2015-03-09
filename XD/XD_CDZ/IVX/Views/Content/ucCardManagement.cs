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

        private void btnCharge_Click(object sender, EventArgs e)
        {
            superTabControl1.SelectedTab = superTabItem1;
        }

        private void btnWalletCharge_Click(object sender, EventArgs e)
        {
            superTabControl1.SelectedTab = superTabItem2;
        }

        private void btnAddCard_Click(object sender, EventArgs e)
        {
            superTabControl1.SelectedTab = superTabItem3;

        }

        private void btnQuitCard_Click(object sender, EventArgs e)
        {
            superTabControl1.SelectedTab = superTabItem4;

        }

        private void ucCardManagement_Load(object sender, EventArgs e)
        {
            superTabControlPanel0.BackgroundImage = Framework.Environment.DefaultImage;

        }

        private void buttonCardStat_Click(object sender, EventArgs e)
        {
            superTabControl1.SelectedTab = superTabItem5;

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
                    btnCharge.Enabled = false;
                    btnQuitCard.Enabled = false;
                    btnWalletCharge.Enabled = false;
                    break;
                default:
                    break;
            }

        }

        private void btnCardRecord_Click(object sender, EventArgs e)
        {
            superTabControl1.SelectedTab = superTabItem6;

        }

    }
}
