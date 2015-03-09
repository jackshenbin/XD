namespace BOCOM.IVX.Views.ResourceTree
{
    partial class ucSysconfigTreeView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.navBarControl2 = new DevExpress.XtraNavBar.NavBarControl();
            this.navBarGroup8 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarItemClusterMonitor = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItemVDAResultServer = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItemMediaServer = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItemPASServer = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItemVDAServer = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItemMediaRouter = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItemClientRouter = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItemFtpHttpServer = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarGroup7 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarItemCameraManagement = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItemPlatManagement = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarGroup9 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarItemUserManagement = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarGroup10 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarItemLogManagement = new DevExpress.XtraNavBar.NavBarItem();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl2)).BeginInit();
            this.SuspendLayout();
            // 
            // navBarControl2
            // 
            this.navBarControl2.ActiveGroup = this.navBarGroup8;
            this.navBarControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.navBarControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navBarControl2.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.navBarGroup8,
            this.navBarGroup7,
            this.navBarGroup9,
            this.navBarGroup10});
            this.navBarControl2.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] {
            this.navBarItemCameraManagement,
            this.navBarItemClusterMonitor,
            this.navBarItemUserManagement,
            this.navBarItemLogManagement,
            this.navBarItemPlatManagement,
            this.navBarItemMediaServer,
            this.navBarItemVDAServer,
            this.navBarItemVDAResultServer,
            this.navBarItemMediaRouter,
            this.navBarItemClientRouter,
            this.navBarItemPASServer,
            this.navBarItemFtpHttpServer});
            this.navBarControl2.Location = new System.Drawing.Point(2, 2);
            this.navBarControl2.LookAndFeel.SkinName = "Metropolis Dark";
            this.navBarControl2.Name = "navBarControl2";
            this.navBarControl2.OptionsNavPane.ExpandedWidth = 241;
            this.navBarControl2.Size = new System.Drawing.Size(241, 548);
            this.navBarControl2.StoreDefaultPaintStyleName = true;
            this.navBarControl2.TabIndex = 2;
            this.navBarControl2.Text = "navBarControl2";
            // 
            // navBarGroup8
            // 
            this.navBarGroup8.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.navBarGroup8.Appearance.Options.UseForeColor = true;
            this.navBarGroup8.Caption = "服务器管理";
            this.navBarGroup8.Expanded = true;
            this.navBarGroup8.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.SmallIconsText;
            this.navBarGroup8.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItemClusterMonitor),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItemVDAResultServer),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItemMediaServer),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItemPASServer),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItemVDAServer),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItemMediaRouter),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItemClientRouter),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItemFtpHttpServer)});
            this.navBarGroup8.Name = "navBarGroup8";
            this.navBarGroup8.TopVisibleLinkIndex = 1;
            // 
            // navBarItemClusterMonitor
            // 
            this.navBarItemClusterMonitor.Caption = "集群资源监控";
            this.navBarItemClusterMonitor.Name = "navBarItemClusterMonitor";
            this.navBarItemClusterMonitor.Visible = false;
            this.navBarItemClusterMonitor.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.ItemClicked);
            // 
            // navBarItemVDAResultServer
            // 
            this.navBarItemVDAResultServer.Caption = "检索比对服务器";
            this.navBarItemVDAResultServer.Name = "navBarItemVDAResultServer";
            this.navBarItemVDAResultServer.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.ItemClicked);
            // 
            // navBarItemMediaServer
            // 
            this.navBarItemMediaServer.Caption = "媒体服务器";
            this.navBarItemMediaServer.Name = "navBarItemMediaServer";
            this.navBarItemMediaServer.Visible = false;
            this.navBarItemMediaServer.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.ItemClicked);
            // 
            // navBarItemPASServer
            // 
            this.navBarItemPASServer.Caption = "预分析服务器";
            this.navBarItemPASServer.Name = "navBarItemPASServer";
            this.navBarItemPASServer.Visible = false;
            this.navBarItemPASServer.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.ItemClicked);
            // 
            // navBarItemVDAServer
            // 
            this.navBarItemVDAServer.Caption = "分析存储服务器";
            this.navBarItemVDAServer.Name = "navBarItemVDAServer";
            this.navBarItemVDAServer.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.ItemClicked);
            // 
            // navBarItemMediaRouter
            // 
            this.navBarItemMediaRouter.Caption = "媒体接入网关";
            this.navBarItemMediaRouter.Name = "navBarItemMediaRouter";
            this.navBarItemMediaRouter.Visible = false;
            this.navBarItemMediaRouter.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.ItemClicked);
            // 
            // navBarItemClientRouter
            // 
            this.navBarItemClientRouter.Caption = "用户接入网关";
            this.navBarItemClientRouter.Name = "navBarItemClientRouter";
            this.navBarItemClientRouter.Visible = false;
            this.navBarItemClientRouter.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.ItemClicked);
            // 
            // navBarItemFtpHttpServer
            // 
            this.navBarItemFtpHttpServer.Caption = "其他服务器";
            this.navBarItemFtpHttpServer.Name = "navBarItemFtpHttpServer";
            this.navBarItemFtpHttpServer.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.ItemClicked);
            // 
            // navBarGroup7
            // 
            this.navBarGroup7.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.navBarGroup7.Appearance.Options.UseForeColor = true;
            this.navBarGroup7.Caption = "监控点管理";
            this.navBarGroup7.Expanded = true;
            this.navBarGroup7.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.SmallIconsText;
            this.navBarGroup7.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItemCameraManagement),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItemPlatManagement)});
            this.navBarGroup7.Name = "navBarGroup7";
            // 
            // navBarItemCameraManagement
            // 
            this.navBarItemCameraManagement.Caption = "监控点管理";
            this.navBarItemCameraManagement.Name = "navBarItemCameraManagement";
            this.navBarItemCameraManagement.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.ItemClicked);
            // 
            // navBarItemPlatManagement
            // 
            this.navBarItemPlatManagement.Caption = "平台管理";
            this.navBarItemPlatManagement.Name = "navBarItemPlatManagement";
            this.navBarItemPlatManagement.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.ItemClicked);
            // 
            // navBarGroup9
            // 
            this.navBarGroup9.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.navBarGroup9.Appearance.Options.UseForeColor = true;
            this.navBarGroup9.Caption = "用户管理";
            this.navBarGroup9.Expanded = true;
            this.navBarGroup9.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItemUserManagement)});
            this.navBarGroup9.Name = "navBarGroup9";
            // 
            // navBarItemUserManagement
            // 
            this.navBarItemUserManagement.Caption = "用户管理";
            this.navBarItemUserManagement.Name = "navBarItemUserManagement";
            this.navBarItemUserManagement.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.ItemClicked);
            // 
            // navBarGroup10
            // 
            this.navBarGroup10.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.navBarGroup10.Appearance.Options.UseForeColor = true;
            this.navBarGroup10.Caption = "日志管理";
            this.navBarGroup10.Expanded = true;
            this.navBarGroup10.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.SmallIconsText;
            this.navBarGroup10.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItemLogManagement)});
            this.navBarGroup10.Name = "navBarGroup10";
            // 
            // navBarItemLogManagement
            // 
            this.navBarItemLogManagement.Caption = "日志管理";
            this.navBarItemLogManagement.Name = "navBarItemLogManagement";
            this.navBarItemLogManagement.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.ItemClicked);
            // 
            // ucSysconfigTreeView
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.navBarControl2);
            this.Name = "ucSysconfigTreeView";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(245, 552);
            this.Load += new System.EventHandler(this.ucSysconfigTreeView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraNavBar.NavBarControl navBarControl2;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup7;
        private DevExpress.XtraNavBar.NavBarItem navBarItemCameraManagement;
        private DevExpress.XtraNavBar.NavBarItem navBarItemPlatManagement;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup8;
        private DevExpress.XtraNavBar.NavBarItem navBarItemClusterMonitor;
        private DevExpress.XtraNavBar.NavBarItem navBarItemMediaServer;
        private DevExpress.XtraNavBar.NavBarItem navBarItemVDAServer;
        private DevExpress.XtraNavBar.NavBarItem navBarItemVDAResultServer;
        private DevExpress.XtraNavBar.NavBarItem navBarItemMediaRouter;
        private DevExpress.XtraNavBar.NavBarItem navBarItemClientRouter;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup9;
        private DevExpress.XtraNavBar.NavBarItem navBarItemUserManagement;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup10;
        private DevExpress.XtraNavBar.NavBarItem navBarItemLogManagement;
        private DevExpress.XtraNavBar.NavBarItem navBarItemPASServer;
        private DevExpress.XtraNavBar.NavBarItem navBarItemFtpHttpServer;

    }
}
