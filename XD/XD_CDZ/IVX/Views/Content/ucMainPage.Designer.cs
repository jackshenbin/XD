namespace BOCOM.IVX.Views.Content
{
    partial class ucMainPage
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucMainPage));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.expandablePanel1 = new DevComponents.DotNetBar.ExpandablePanel();
            this.buttonX3 = new DevComponents.DotNetBar.ButtonX();
            this.buttonX2 = new DevComponents.DotNetBar.ButtonX();
            this.btnUserMamagement = new DevComponents.DotNetBar.ButtonX();
            this.btnReportManagement = new DevComponents.DotNetBar.ButtonX();
            this.btnCardManagement = new DevComponents.DotNetBar.ButtonX();
            this.btnDeviceManagement = new DevComponents.DotNetBar.ButtonX();
            this.ucCurrentUser1 = new BOCOM.IVX.Views.Content.ucCurrentUser();
            this.sideBar1 = new DevComponents.DotNetBar.SideBar();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.expandablePanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.expandablePanel1);
            this.splitContainer1.Panel1.Controls.Add(this.ucCurrentUser1);
            this.splitContainer1.Panel1MinSize = 185;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.splitContainer1.Size = new System.Drawing.Size(660, 592);
            this.splitContainer1.SplitterDistance = 185;
            this.splitContainer1.TabIndex = 0;
            // 
            // expandablePanel1
            // 
            this.expandablePanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.expandablePanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.expandablePanel1.Controls.Add(this.buttonX3);
            this.expandablePanel1.Controls.Add(this.buttonX2);
            this.expandablePanel1.Controls.Add(this.btnUserMamagement);
            this.expandablePanel1.Controls.Add(this.btnReportManagement);
            this.expandablePanel1.Controls.Add(this.btnCardManagement);
            this.expandablePanel1.Controls.Add(this.btnDeviceManagement);
            this.expandablePanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.expandablePanel1.Location = new System.Drawing.Point(0, 59);
            this.expandablePanel1.Name = "expandablePanel1";
            this.expandablePanel1.Size = new System.Drawing.Size(185, 282);
            this.expandablePanel1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.expandablePanel1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandablePanel1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.expandablePanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.expandablePanel1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandablePanel1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.expandablePanel1.Style.GradientAngle = 90;
            this.expandablePanel1.TabIndex = 5;
            this.expandablePanel1.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this.expandablePanel1.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandablePanel1.TitleStyle.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.expandablePanel1.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this.expandablePanel1.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandablePanel1.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.expandablePanel1.TitleStyle.GradientAngle = 90;
            this.expandablePanel1.TitleText = "网站导航";
            // 
            // buttonX3
            // 
            this.buttonX3.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX3.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX3.Location = new System.Drawing.Point(29, 213);
            this.buttonX3.Name = "buttonX3";
            this.buttonX3.Size = new System.Drawing.Size(113, 28);
            this.buttonX3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX3.TabIndex = 1;
            this.buttonX3.Text = "关于我们";
            // 
            // buttonX2
            // 
            this.buttonX2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX2.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX2.Location = new System.Drawing.Point(29, 179);
            this.buttonX2.Name = "buttonX2";
            this.buttonX2.Size = new System.Drawing.Size(113, 28);
            this.buttonX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX2.TabIndex = 1;
            this.buttonX2.Text = "服务支持";
            // 
            // btnUserMamagement
            // 
            this.btnUserMamagement.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnUserMamagement.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnUserMamagement.Location = new System.Drawing.Point(29, 145);
            this.btnUserMamagement.Name = "btnUserMamagement";
            this.btnUserMamagement.Size = new System.Drawing.Size(113, 28);
            this.btnUserMamagement.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnUserMamagement.TabIndex = 1;
            this.btnUserMamagement.Text = "系统管理";
            this.btnUserMamagement.Click += new System.EventHandler(this.btnUserMamagement_Click);
            // 
            // btnReportManagement
            // 
            this.btnReportManagement.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnReportManagement.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnReportManagement.Location = new System.Drawing.Point(29, 111);
            this.btnReportManagement.Name = "btnReportManagement";
            this.btnReportManagement.Size = new System.Drawing.Size(113, 28);
            this.btnReportManagement.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnReportManagement.TabIndex = 1;
            this.btnReportManagement.Text = "综合查询";
            this.btnReportManagement.Click += new System.EventHandler(this.btnReportManagement_Click);
            // 
            // btnCardManagement
            // 
            this.btnCardManagement.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCardManagement.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCardManagement.Location = new System.Drawing.Point(29, 77);
            this.btnCardManagement.Name = "btnCardManagement";
            this.btnCardManagement.Size = new System.Drawing.Size(113, 28);
            this.btnCardManagement.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCardManagement.TabIndex = 1;
            this.btnCardManagement.Text = "智能卡管理";
            this.btnCardManagement.Click += new System.EventHandler(this.btnCardManagement_Click);
            // 
            // btnDeviceManagement
            // 
            this.btnDeviceManagement.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDeviceManagement.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDeviceManagement.Location = new System.Drawing.Point(29, 43);
            this.btnDeviceManagement.Name = "btnDeviceManagement";
            this.btnDeviceManagement.Size = new System.Drawing.Size(113, 28);
            this.btnDeviceManagement.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnDeviceManagement.TabIndex = 1;
            this.btnDeviceManagement.Text = "充电桩管理";
            this.btnDeviceManagement.Click += new System.EventHandler(this.btnDeviceManagement_Click);
            // 
            // ucCurrentUser1
            // 
            this.ucCurrentUser1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucCurrentUser1.BackgroundImage")));
            this.ucCurrentUser1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ucCurrentUser1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucCurrentUser1.Location = new System.Drawing.Point(0, 0);
            this.ucCurrentUser1.Name = "ucCurrentUser1";
            this.ucCurrentUser1.Size = new System.Drawing.Size(185, 59);
            this.ucCurrentUser1.TabIndex = 0;
            // 
            // sideBar1
            // 
            this.sideBar1.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.sideBar1.ExpandedPanel = null;
            this.sideBar1.Location = new System.Drawing.Point(0, 0);
            this.sideBar1.Name = "sideBar1";
            this.sideBar1.Size = new System.Drawing.Size(0, 0);
            this.sideBar1.TabIndex = 0;
            // 
            // ucMainPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ucMainPage";
            this.Size = new System.Drawing.Size(660, 592);
            this.Load += new System.EventHandler(this.ucMainPage_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.expandablePanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private ucCurrentUser ucCurrentUser1;
        private DevComponents.DotNetBar.SideBar sideBar1;
        private DevComponents.DotNetBar.ExpandablePanel expandablePanel1;
        private DevComponents.DotNetBar.ButtonX btnReportManagement;
        private DevComponents.DotNetBar.ButtonX btnCardManagement;
        private DevComponents.DotNetBar.ButtonX btnDeviceManagement;
        private DevComponents.DotNetBar.ButtonX buttonX3;
        private DevComponents.DotNetBar.ButtonX buttonX2;
        private DevComponents.DotNetBar.ButtonX btnUserMamagement;
    }
}
