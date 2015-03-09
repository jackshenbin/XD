namespace BOCOM.IVX.Views.ResourceTree
{
    partial class ucExportTreeView
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
            this.navBarItemCaseExport = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItemTagExport = new DevExpress.XtraNavBar.NavBarItem();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl2)).BeginInit();
            this.SuspendLayout();
            // 
            // navBarControl2
            // 
            this.navBarControl2.ActiveGroup = this.navBarGroup8;
            this.navBarControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.navBarControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navBarControl2.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.navBarGroup8});
            this.navBarControl2.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] {
            this.navBarItemCaseExport,
            this.navBarItemTagExport});
            this.navBarControl2.Location = new System.Drawing.Point(2, 2);
            this.navBarControl2.LookAndFeel.SkinName = "Metropolis Dark";
            this.navBarControl2.Name = "navBarControl2";
            this.navBarControl2.OptionsNavPane.ExpandedWidth = 222;
            this.navBarControl2.Size = new System.Drawing.Size(222, 449);
            this.navBarControl2.StoreDefaultPaintStyleName = true;
            this.navBarControl2.TabIndex = 3;
            this.navBarControl2.Text = "navBarControl2";
            // 
            // navBarGroup8
            // 
            this.navBarGroup8.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.navBarGroup8.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.navBarGroup8.Appearance.Options.UseFont = true;
            this.navBarGroup8.Appearance.Options.UseForeColor = true;
            this.navBarGroup8.Caption = "导出";
            this.navBarGroup8.Expanded = true;
            this.navBarGroup8.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.SmallIconsText;
            this.navBarGroup8.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItemCaseExport),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItemTagExport)});
            this.navBarGroup8.Name = "navBarGroup8";
            this.navBarGroup8.TopVisibleLinkIndex = 1;
            // 
            // navBarItemCaseExport
            // 
            this.navBarItemCaseExport.Caption = "案件导出";
            this.navBarItemCaseExport.Name = "navBarItemCaseExport";
            this.navBarItemCaseExport.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.ItemClicked);
            // 
            // navBarItemTagExport
            // 
            this.navBarItemTagExport.Caption = "标签导出";
            this.navBarItemTagExport.Name = "navBarItemTagExport";
            this.navBarItemTagExport.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.ItemClicked);
            // 
            // ucExportTreeView
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.navBarControl2);
            this.Name = "ucExportTreeView";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(226, 453);
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraNavBar.NavBarControl navBarControl2;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup8;
        private DevExpress.XtraNavBar.NavBarItem navBarItemCaseExport;
        private DevExpress.XtraNavBar.NavBarItem navBarItemTagExport;
    }
}
