using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BOCOM.IVX.Views.Content;
using BOCOM.IVX.Framework;
using BOCOM.IVX.Views;

namespace BOCOM.IVX
{
    public partial class MainForm : Form
    {
        #region Fields

        //private MainFormViewModel m_viewModel;

        #endregion

        #region Constructors

        ucXDMain c ;
        public MainForm()
        {
            InitializeComponent();
            //m_viewModel = new MainFormViewModel(this.Text);
            //m_viewModel.CloseRequest += new EventHandler(m_viewModel_CloseRequest);
        }


        #endregion

        #region Event handlers

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Framework.Container.Instance.VVMDataBindings.AddBinding(this, "Text", m_viewModel, "ComposedCaption");

            c = new ucXDMain();
            this.panel1.Controls.Add(c);
            c.Dock = DockStyle.Fill;
            this.Text = "XD " + Framework.Environment.VersionDetail;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Controls.Clear();
            c = null;
            Framework.Container.Instance.Cleanup();

        }

        #endregion

        private void buttonItem1_Click(object sender, EventArgs e)
        {
            c.SwitchWnd(DataModel.E_SWITCH_WND.SWITCH_WND_USER_MENAGEMENT);
        }

        private void buttonItem2_Click(object sender, EventArgs e)
        {
            c.SwitchWnd(DataModel.E_SWITCH_WND.SWITCH_WND_CARD_MENAGEMENT);

        }

        private void buttonItem3_Click(object sender, EventArgs e)
        {
            c.SwitchWnd(DataModel.E_SWITCH_WND.SWITCH_WND_DEVICE_MENAGEMENT);

        }

        private void buttonItem4_Click(object sender, EventArgs e)
        {
            c.SwitchWnd(DataModel.E_SWITCH_WND.SWITCH_WND_REPORT_MENAGEMENT);

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            bool ret = c.LoginWnd(textBoxItemIP.Text,textBoxItemUser.Text, textBoxItemPass.Text);
            if (ret)
            {
                c.InitWnd();
            }
        }

        private void buttonItem5_Click(object sender, EventArgs e)
        {
            c.SwitchWnd(DataModel.E_SWITCH_WND.SWITCH_WND_MAIN_PAGE);

        }

    }
}