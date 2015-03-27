namespace BOCOM.IVX.Views.Content
{
    partial class ucReportMenagement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucReportMenagement));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.expandablePanel1 = new DevComponents.DotNetBar.ExpandablePanel();
            this.btnCardRecord = new DevComponents.DotNetBar.ButtonX();
            this.btnRegionStat = new DevComponents.DotNetBar.ButtonX();
            this.btnTradingRecord = new DevComponents.DotNetBar.ButtonX();
            this.ucCurrentUser1 = new BOCOM.IVX.Views.Content.ucCurrentUser();
            this.superTabControl1 = new DevComponents.DotNetBar.SuperTabControl();
            this.superTabControlPanel0 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.superTabItem0 = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel2 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.superTabItem2 = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel3 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.ucCardRecordPanel1 = new BOCOM.IVX.Views.Content.ucCardRecordPanel();
            this.superTabItem3 = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel1 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.ucTradingRecordPanel1 = new BOCOM.IVX.Views.Content.ucTradingRecordPanel();
            this.superTabItem1 = new DevComponents.DotNetBar.SuperTabItem();
            this.ucRegionStatRecordPanel1 = new BOCOM.IVX.Views.Content.ucRegionStatRecordPanel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.expandablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).BeginInit();
            this.superTabControl1.SuspendLayout();
            this.superTabControlPanel2.SuspendLayout();
            this.superTabControlPanel3.SuspendLayout();
            this.superTabControlPanel1.SuspendLayout();
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
            this.splitContainer1.Panel2.Controls.Add(this.superTabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(854, 401);
            this.splitContainer1.SplitterDistance = 185;
            this.splitContainer1.TabIndex = 3;
            // 
            // expandablePanel1
            // 
            this.expandablePanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.expandablePanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.expandablePanel1.Controls.Add(this.btnCardRecord);
            this.expandablePanel1.Controls.Add(this.btnRegionStat);
            this.expandablePanel1.Controls.Add(this.btnTradingRecord);
            this.expandablePanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.expandablePanel1.Location = new System.Drawing.Point(0, 59);
            this.expandablePanel1.Name = "expandablePanel1";
            this.expandablePanel1.Size = new System.Drawing.Size(185, 176);
            this.expandablePanel1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.expandablePanel1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandablePanel1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.expandablePanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.expandablePanel1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandablePanel1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.expandablePanel1.Style.GradientAngle = 90;
            this.expandablePanel1.TabIndex = 6;
            this.expandablePanel1.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this.expandablePanel1.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandablePanel1.TitleStyle.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.expandablePanel1.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this.expandablePanel1.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandablePanel1.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.expandablePanel1.TitleStyle.GradientAngle = 90;
            this.expandablePanel1.TitleText = "综合查询";
            // 
            // btnCardRecord
            // 
            this.btnCardRecord.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCardRecord.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCardRecord.Location = new System.Drawing.Point(29, 111);
            this.btnCardRecord.Name = "btnCardRecord";
            this.btnCardRecord.Size = new System.Drawing.Size(113, 28);
            this.btnCardRecord.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCardRecord.TabIndex = 1;
            this.btnCardRecord.Text = "卡片操作记录查询";
            this.btnCardRecord.Click += new System.EventHandler(this.btnCardRecord_Click);
            // 
            // btnRegionStat
            // 
            this.btnRegionStat.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnRegionStat.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnRegionStat.Location = new System.Drawing.Point(29, 77);
            this.btnRegionStat.Name = "btnRegionStat";
            this.btnRegionStat.Size = new System.Drawing.Size(113, 28);
            this.btnRegionStat.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnRegionStat.TabIndex = 1;
            this.btnRegionStat.Text = "区域状态查询";
            this.btnRegionStat.Click += new System.EventHandler(this.btnRegionStat_Click);
            // 
            // btnTradingRecord
            // 
            this.btnTradingRecord.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnTradingRecord.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnTradingRecord.Location = new System.Drawing.Point(29, 43);
            this.btnTradingRecord.Name = "btnTradingRecord";
            this.btnTradingRecord.Size = new System.Drawing.Size(113, 28);
            this.btnTradingRecord.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnTradingRecord.TabIndex = 1;
            this.btnTradingRecord.Text = "交易记录查询";
            this.btnTradingRecord.Click += new System.EventHandler(this.btnTradingRecord_Click);
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
            // superTabControl1
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.superTabControl1.ControlBox.CloseBox.Name = "";
            // 
            // 
            // 
            this.superTabControl1.ControlBox.MenuBox.Name = "";
            this.superTabControl1.ControlBox.Name = "";
            this.superTabControl1.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabControl1.ControlBox.MenuBox,
            this.superTabControl1.ControlBox.CloseBox});
            this.superTabControl1.ControlBox.Visible = false;
            this.superTabControl1.Controls.Add(this.superTabControlPanel2);
            this.superTabControl1.Controls.Add(this.superTabControlPanel1);
            this.superTabControl1.Controls.Add(this.superTabControlPanel0);
            this.superTabControl1.Controls.Add(this.superTabControlPanel3);
            this.superTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControl1.Location = new System.Drawing.Point(0, 0);
            this.superTabControl1.Name = "superTabControl1";
            this.superTabControl1.ReorderTabsEnabled = true;
            this.superTabControl1.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.superTabControl1.SelectedTabIndex = 0;
            this.superTabControl1.Size = new System.Drawing.Size(665, 401);
            this.superTabControl1.TabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.superTabControl1.TabIndex = 1;
            this.superTabControl1.TabLayoutType = DevComponents.DotNetBar.eSuperTabLayoutType.SingleLineFit;
            this.superTabControl1.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabItem0,
            this.superTabItem1,
            this.superTabItem2,
            this.superTabItem3});
            this.superTabControl1.TabsVisible = false;
            this.superTabControl1.Text = "superTabControl1";
            // 
            // superTabControlPanel0
            // 
            this.superTabControlPanel0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel0.Location = new System.Drawing.Point(0, 26);
            this.superTabControlPanel0.Name = "superTabControlPanel0";
            this.superTabControlPanel0.Size = new System.Drawing.Size(665, 375);
            this.superTabControlPanel0.TabIndex = 0;
            this.superTabControlPanel0.TabItem = this.superTabItem0;
            // 
            // superTabItem0
            // 
            this.superTabItem0.AttachedControl = this.superTabControlPanel0;
            this.superTabItem0.GlobalItem = false;
            this.superTabItem0.Name = "superTabItem0";
            this.superTabItem0.Text = "superTabItem0";
            // 
            // superTabControlPanel2
            // 
            this.superTabControlPanel2.Controls.Add(this.ucRegionStatRecordPanel1);
            this.superTabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel2.Location = new System.Drawing.Point(0, 26);
            this.superTabControlPanel2.Name = "superTabControlPanel2";
            this.superTabControlPanel2.Size = new System.Drawing.Size(665, 375);
            this.superTabControlPanel2.TabIndex = 0;
            this.superTabControlPanel2.TabItem = this.superTabItem2;
            // 
            // superTabItem2
            // 
            this.superTabItem2.AttachedControl = this.superTabControlPanel2;
            this.superTabItem2.GlobalItem = false;
            this.superTabItem2.Name = "superTabItem2";
            this.superTabItem2.Text = "superTabItem2";
            // 
            // superTabControlPanel3
            // 
            this.superTabControlPanel3.Controls.Add(this.ucCardRecordPanel1);
            this.superTabControlPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel3.Location = new System.Drawing.Point(0, 0);
            this.superTabControlPanel3.Name = "superTabControlPanel3";
            this.superTabControlPanel3.Size = new System.Drawing.Size(665, 401);
            this.superTabControlPanel3.TabIndex = 0;
            this.superTabControlPanel3.TabItem = this.superTabItem3;
            // 
            // ucCardRecordPanel1
            // 
            this.ucCardRecordPanel1.AutoScroll = true;
            this.ucCardRecordPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucCardRecordPanel1.Location = new System.Drawing.Point(0, 0);
            this.ucCardRecordPanel1.Name = "ucCardRecordPanel1";
            this.ucCardRecordPanel1.Size = new System.Drawing.Size(665, 401);
            this.ucCardRecordPanel1.TabIndex = 0;
            // 
            // superTabItem3
            // 
            this.superTabItem3.AttachedControl = this.superTabControlPanel3;
            this.superTabItem3.GlobalItem = false;
            this.superTabItem3.Name = "superTabItem3";
            this.superTabItem3.Text = "superTabItem3";
            // 
            // superTabControlPanel1
            // 
            this.superTabControlPanel1.Controls.Add(this.ucTradingRecordPanel1);
            this.superTabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel1.Location = new System.Drawing.Point(0, 26);
            this.superTabControlPanel1.Name = "superTabControlPanel1";
            this.superTabControlPanel1.Size = new System.Drawing.Size(665, 375);
            this.superTabControlPanel1.TabIndex = 1;
            this.superTabControlPanel1.TabItem = this.superTabItem1;
            // 
            // ucTradingRecordPanel1
            // 
            this.ucTradingRecordPanel1.AutoScroll = true;
            this.ucTradingRecordPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucTradingRecordPanel1.Location = new System.Drawing.Point(0, 0);
            this.ucTradingRecordPanel1.Name = "ucTradingRecordPanel1";
            this.ucTradingRecordPanel1.Size = new System.Drawing.Size(665, 375);
            this.ucTradingRecordPanel1.TabIndex = 0;
            // 
            // superTabItem1
            // 
            this.superTabItem1.AttachedControl = this.superTabControlPanel1;
            this.superTabItem1.GlobalItem = false;
            this.superTabItem1.Name = "superTabItem1";
            this.superTabItem1.Text = "superTabItem1";
            // 
            // ucRegionStatRecordPanel1
            // 
            this.ucRegionStatRecordPanel1.AutoScroll = true;
            this.ucRegionStatRecordPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucRegionStatRecordPanel1.Location = new System.Drawing.Point(0, 0);
            this.ucRegionStatRecordPanel1.Name = "ucRegionStatRecordPanel1";
            this.ucRegionStatRecordPanel1.Size = new System.Drawing.Size(665, 375);
            this.ucRegionStatRecordPanel1.TabIndex = 0;
            // 
            // ucReportMenagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ucReportMenagement";
            this.Size = new System.Drawing.Size(854, 401);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.expandablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).EndInit();
            this.superTabControl1.ResumeLayout(false);
            this.superTabControlPanel2.ResumeLayout(false);
            this.superTabControlPanel3.ResumeLayout(false);
            this.superTabControlPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private ucCurrentUser ucCurrentUser1;
        private DevComponents.DotNetBar.ExpandablePanel expandablePanel1;
        private DevComponents.DotNetBar.ButtonX btnCardRecord;
        private DevComponents.DotNetBar.ButtonX btnRegionStat;
        private DevComponents.DotNetBar.ButtonX btnTradingRecord;
        private DevComponents.DotNetBar.SuperTabControl superTabControl1;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel0;
        private DevComponents.DotNetBar.SuperTabItem superTabItem0;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel3;
        private DevComponents.DotNetBar.SuperTabItem superTabItem3;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel2;
        private DevComponents.DotNetBar.SuperTabItem superTabItem2;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel1;
        private DevComponents.DotNetBar.SuperTabItem superTabItem1;
        private ucTradingRecordPanel ucTradingRecordPanel1;
        private ucCardRecordPanel ucCardRecordPanel1;
        private ucRegionStatRecordPanel ucRegionStatRecordPanel1;
    }
}
