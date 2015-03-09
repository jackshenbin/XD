namespace BOCOM.IVX.Views
{
    partial class ucMain
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
            this.components = new System.ComponentModel.Container();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barStaticServerIP = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticSDKVersion = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticServerVersion = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barStaticItem3 = new DevExpress.XtraBars.BarStaticItem();
            this.naviBar1 = new BOCOM.IVX.Views.Navigation.NaviBar();
            this.splitContainerControl2 = new DevExpress.XtraEditors.SplitContainerControl();
            this.panelResTree = new DevExpress.XtraEditors.PanelControl();
            this.panelMain = new DevExpress.XtraEditors.PanelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.ucUserProfile1 = new BOCOM.IVX.Views.Navigation.ucUserProfile();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).BeginInit();
            this.splitContainerControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelResTree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.AllowCustomization = false;
            this.barManager1.AllowQuickCustomization = false;
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barStaticSDKVersion,
            this.barStaticItem3,
            this.barStaticServerVersion,
            this.barStaticServerIP});
            this.barManager1.MaxItemId = 5;
            this.barManager1.ShowFullMenusAfterDelay = false;
            this.barManager1.ShowShortcutInScreenTips = false;
            this.barManager1.StatusBar = this.bar3;
            this.barManager1.UseAltKeyForMenu = false;
            this.barManager1.UseF10KeyForMenu = false;
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticServerIP),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticSDKVersion),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticServerVersion)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.DrawSizeGrip = true;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // barStaticServerIP
            // 
            this.barStaticServerIP.Caption = "服务器地址：127.0.0.1";
            this.barStaticServerIP.Id = 4;
            this.barStaticServerIP.Name = "barStaticServerIP";
            this.barStaticServerIP.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticSDKVersion
            // 
            this.barStaticSDKVersion.Caption = "SDK 版本:V3.0.2.0";
            this.barStaticSDKVersion.Id = 1;
            this.barStaticSDKVersion.Name = "barStaticSDKVersion";
            this.barStaticSDKVersion.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticServerVersion
            // 
            this.barStaticServerVersion.Caption = "服务器版本：V3.0.2.0";
            this.barStaticServerVersion.Id = 3;
            this.barStaticServerVersion.Name = "barStaticServerVersion";
            this.barStaticServerVersion.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1095, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 521);
            this.barDockControlBottom.Size = new System.Drawing.Size(1095, 27);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 521);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1095, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 521);
            // 
            // barStaticItem3
            // 
            this.barStaticItem3.Caption = "admin";
            this.barStaticItem3.Id = 2;
            this.barStaticItem3.Name = "barStaticItem3";
            this.barStaticItem3.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // naviBar1
            // 
            this.naviBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.naviBar1.Appearance.BackColor = System.Drawing.Color.Black;
            this.naviBar1.Appearance.Options.UseBackColor = true;
            this.naviBar1.Location = new System.Drawing.Point(227, 2);
            this.naviBar1.LookAndFeel.SkinName = "Metropolis Dark";
            this.naviBar1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.naviBar1.Name = "naviBar1";
            this.naviBar1.Size = new System.Drawing.Size(857, 61);
            this.naviBar1.TabIndex = 6;
            // 
            // splitContainerControl2
            // 
            this.splitContainerControl2.Appearance.BackColor = System.Drawing.Color.Black;
            this.splitContainerControl2.Appearance.Options.UseBackColor = true;
            this.splitContainerControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl2.Location = new System.Drawing.Point(2, 2);
            this.splitContainerControl2.Name = "splitContainerControl2";
            this.splitContainerControl2.Panel1.Controls.Add(this.panelResTree);
            this.splitContainerControl2.Panel1.Padding = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.splitContainerControl2.Panel1.Text = "Panel1";
            this.splitContainerControl2.Panel2.Appearance.BackColor = System.Drawing.Color.Black;
            this.splitContainerControl2.Panel2.Appearance.BackColor2 = System.Drawing.Color.Black;
            this.splitContainerControl2.Panel2.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.splitContainerControl2.Panel2.Appearance.Options.UseBackColor = true;
            this.splitContainerControl2.Panel2.Appearance.Options.UseBorderColor = true;
            this.splitContainerControl2.Panel2.Controls.Add(this.panelMain);
            this.splitContainerControl2.Panel2.Controls.Add(this.labelControl3);
            this.splitContainerControl2.Panel2.Padding = new System.Windows.Forms.Padding(0, 0, 2, 2);
            this.splitContainerControl2.Panel2.Text = "我的案件";
            this.splitContainerControl2.Size = new System.Drawing.Size(1091, 452);
            this.splitContainerControl2.SplitterPosition = 226;
            this.splitContainerControl2.TabIndex = 0;
            this.splitContainerControl2.Text = "splitContainerControl2";
            // 
            // panelResTree
            // 
            this.panelResTree.Appearance.BackColor = System.Drawing.Color.Black;
            this.panelResTree.Appearance.Options.UseBackColor = true;
            this.panelResTree.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelResTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelResTree.Location = new System.Drawing.Point(2, 2);
            this.panelResTree.Name = "panelResTree";
            this.panelResTree.Size = new System.Drawing.Size(224, 448);
            this.panelResTree.TabIndex = 1;
            // 
            // panelMain
            // 
            this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMain.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.panelMain.Appearance.Options.UseBackColor = true;
            this.panelMain.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelMain.Location = new System.Drawing.Point(0, 36);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(858, 415);
            this.panelMain.TabIndex = 0;
            // 
            // labelControl3
            // 
            this.labelControl3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl3.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl3.Location = new System.Drawing.Point(0, 2);
            this.labelControl3.LookAndFeel.SkinName = "Darkroom";
            this.labelControl3.LookAndFeel.UseDefaultLookAndFeel = false;
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 13, 3, 3);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Padding = new System.Windows.Forms.Padding(6, 0, 20, 0);
            this.labelControl3.Size = new System.Drawing.Size(858, 33);
            this.labelControl3.TabIndex = 1;
            this.labelControl3.Text = "labelControl3";
            this.labelControl3.VisibleChanged += new System.EventHandler(this.labelControl3_VisibleChanged);
            // 
            // panelControl2
            // 
            this.panelControl2.Appearance.BackColor = System.Drawing.Color.Black;
            this.panelControl2.Appearance.Options.UseBackColor = true;
            this.panelControl2.Appearance.Options.UseBorderColor = true;
            this.panelControl2.Appearance.Options.UseFont = true;
            this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl2.Controls.Add(this.ucUserProfile1);
            this.panelControl2.Controls.Add(this.naviBar1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.LookAndFeel.SkinName = "Metropolis Dark";
            this.panelControl2.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.panelControl2.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1095, 65);
            this.panelControl2.TabIndex = 7;
            // 
            // ucUserProfile1
            // 
            this.ucUserProfile1.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.ucUserProfile1.Appearance.Options.UseBackColor = true;
            this.ucUserProfile1.Location = new System.Drawing.Point(5, 0);
            this.ucUserProfile1.Name = "ucUserProfile1";
            this.ucUserProfile1.Size = new System.Drawing.Size(220, 65);
            this.ucUserProfile1.TabIndex = 7;
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.splitContainerControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 65);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Padding = new System.Windows.Forms.Padding(2);
            this.panelControl1.Size = new System.Drawing.Size(1095, 456);
            this.panelControl1.TabIndex = 0;
            // 
            // ucMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "ucMain";
            this.Size = new System.Drawing.Size(1095, 548);
            this.Load += new System.EventHandler(this.ucMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).EndInit();
            this.splitContainerControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelResTree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarStaticItem barStaticSDKVersion;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraEditors.PanelControl panelMain;
        private Navigation.NaviBar naviBar1;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl2;
        private DevExpress.XtraEditors.PanelControl panelResTree;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private Navigation.ucUserProfile ucUserProfile1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraBars.BarStaticItem barStaticServerVersion;
        private DevExpress.XtraBars.BarStaticItem barStaticServerIP;
        private DevExpress.XtraBars.BarStaticItem barStaticItem3;
        private DevExpress.XtraEditors.LabelControl labelControl3;
    }
}
