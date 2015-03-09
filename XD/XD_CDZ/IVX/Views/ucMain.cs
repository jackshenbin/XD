using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BOCOM.IVX.Views.Content;
using BOCOM.IVX.ViewModel;
using BOCOM.IVX.UILogics;
using BOCOM.IVX.Protocol;

namespace BOCOM.IVX.Views
{
    public partial class ucMain : UserControl
    {
        #region Fields

        private MainControlViewModel m_viewModel;
        
        #endregion

        public int SplitterPosition 
        {
            get 
            {
                return splitContainerControl2.SplitterPosition;
            }
            set
            {
                if (this.splitContainerControl2.PanelVisibility == SplitPanelVisibility.Both)
                    splitContainerControl2.SplitterPosition = value;
            }
        }

        #region Constructors

        public ucMain()
        {
            InitializeComponent();
            Framework.Container.Instance.MainControl = this;
            Framework.Container.Instance.ContentPageController = new ContentPageController(this.panelMain, this.labelControl3, this.splitContainerControl2);
            Framework.Container.Instance.NaviPanelController = new NaviPanelController(panelResTree);
            m_viewModel = new MainControlViewModel();

            // btnLogout.Tag = m_viewModel.LogoutCommand;
            //btnQuit.Tag = m_viewModel.QuitCommand;
            // btnShowUserProfile.Tag = m_viewModel.ShowUserProfileCommand;
        }

        public ucMain(bool showNavigationBar)
            : this()
        {
            if (!showNavigationBar)
            {
                this.splitContainerControl2.PanelVisibility = SplitPanelVisibility.Panel2;
                this.panelControl2.Visible = false;
            }
        }

        #endregion

        #region Event handlers


        private void xtraTabControl1_SizeChanged(object sender, EventArgs e)
        {
            //this.xtraTabControl2.Left = 4;
            //this.xtraTabControl2.Width = this.splitContainerControl1.Panel2.Width - 4;
        }

        private void ucMain_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                try
                {
                    this.naviBar1.Init(Framework.Environment.CaseType, Framework.Container.Instance.AuthenticationService.GetLoginUserRole());
                }
                catch (SDKCallException ex)
                {
                    Common.SDKCallExceptionHandler.Handle(ex, "获取用户角色");
                }
            }
            barStaticServerIP.Caption = m_viewModel.ServerIPInfo;
            barStaticSDKVersion.Caption = m_viewModel.SDKVersionInfo;
            barStaticServerVersion.Caption = m_viewModel.ServerVersionInfo;
        }

        //private void btn_Click(object sender, EventArgs e)
        //{
        //    m_viewModel.Execute(((Control)sender).Tag, sender);
        //}

        #endregion

        private void labelControl3_VisibleChanged(object sender, EventArgs e)
        {
            if (labelControl3.Visible)
            {
                if (panelMain.Top < labelControl3.Bottom)
                {
                    panelMain.Top += labelControl3.Height;
                    panelMain.Height = panelMain.Height - labelControl3.Height;
                }
            }
            else
            {
                if (panelMain.Top != labelControl3.Top)
                {
                    panelMain.Top = labelControl3.Top;
                    panelMain.Height = panelMain.Height + labelControl3.Height;
                }
            }
        }
       
    }
}
