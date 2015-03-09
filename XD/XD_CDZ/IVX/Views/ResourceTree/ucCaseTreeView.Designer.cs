namespace BOCOM.IVX.Views.ResourceTree
{
    partial class ucCaseTreeView
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
            this.treeList2 = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.navBarControl4 = new DevExpress.XtraNavBar.NavBarControl();
            this.navBarGroup16 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarItemNewCase = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItemMyCaseList = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItemCurrCase = new DevExpress.XtraNavBar.NavBarItem();
            ((System.ComponentModel.ISupportInitialize)(this.treeList2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl4)).BeginInit();
            this.SuspendLayout();
            // 
            // treeList2
            // 
            this.treeList2.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1});
            this.treeList2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList2.Location = new System.Drawing.Point(2, 2);
            this.treeList2.Name = "treeList2";
            this.treeList2.BeginUnboundLoad();
            this.treeList2.AppendNode(new object[] {
            "新建案件"}, -1);
            this.treeList2.AppendNode(new object[] {
            "我的案件"}, -1);
            this.treeList2.AppendNode(new object[] {
            "青州路抢劫案"}, 1);
            this.treeList2.AppendNode(new object[] {
            "xx路杀人案"}, 1);
            this.treeList2.AppendNode(new object[] {
            "当前案件"}, -1);
            this.treeList2.EndUnboundLoad();
            this.treeList2.OptionsBehavior.Editable = false;
            this.treeList2.OptionsMenu.EnableColumnMenu = false;
            this.treeList2.OptionsMenu.EnableFooterMenu = false;
            this.treeList2.OptionsMenu.ShowAutoFilterRowItem = false;
            this.treeList2.OptionsPrint.UsePrintStyles = true;
            this.treeList2.Size = new System.Drawing.Size(246, 350);
            this.treeList2.TabIndex = 5;
            this.treeList2.Visible = false;
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "案件";
            this.treeListColumn1.FieldName = "点位";
            this.treeListColumn1.MinWidth = 70;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            // 
            // navBarControl4
            // 
            this.navBarControl4.ActiveGroup = this.navBarGroup16;
            this.navBarControl4.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.navBarControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navBarControl4.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.navBarGroup16});
            this.navBarControl4.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] {
            this.navBarItemMyCaseList,
            this.navBarItemNewCase,
            this.navBarItemCurrCase});
            this.navBarControl4.Location = new System.Drawing.Point(2, 2);
            this.navBarControl4.LookAndFeel.SkinName = "Metropolis Dark";
            this.navBarControl4.Name = "navBarControl4";
            this.navBarControl4.OptionsNavPane.ExpandedWidth = 246;
            this.navBarControl4.Size = new System.Drawing.Size(246, 350);
            this.navBarControl4.StoreDefaultPaintStyleName = true;
            this.navBarControl4.TabIndex = 6;
            this.navBarControl4.Text = "navBarControl4";
            // 
            // navBarGroup16
            // 
            this.navBarGroup16.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.navBarGroup16.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.navBarGroup16.Appearance.Options.UseFont = true;
            this.navBarGroup16.Appearance.Options.UseForeColor = true;
            this.navBarGroup16.Caption = "案件管理";
            this.navBarGroup16.Expanded = true;
            this.navBarGroup16.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.SmallIconsText;
            this.navBarGroup16.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItemNewCase),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItemMyCaseList),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItemCurrCase)});
            this.navBarGroup16.Name = "navBarGroup16";
            // 
            // navBarItemNewCase
            // 
            this.navBarItemNewCase.Caption = "新建案件";
            this.navBarItemNewCase.Name = "navBarItemNewCase";
            this.navBarItemNewCase.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.ItemClicked);
            // 
            // navBarItemMyCaseList
            // 
            this.navBarItemMyCaseList.Caption = "我的案件";
            this.navBarItemMyCaseList.Name = "navBarItemMyCaseList";
            this.navBarItemMyCaseList.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.ItemClicked);
            // 
            // navBarItemCurrCase
            // 
            this.navBarItemCurrCase.Caption = "当前案件";
            this.navBarItemCurrCase.Name = "navBarItemCurrCase";
            this.navBarItemCurrCase.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.ItemClicked);
            // 
            // ucCaseTreeView
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.navBarControl4);
            this.Controls.Add(this.treeList2);
            this.Name = "ucCaseTreeView";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(250, 354);
            ((System.ComponentModel.ISupportInitialize)(this.treeList2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTreeList.TreeList treeList2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraNavBar.NavBarControl navBarControl4;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup16;
        private DevExpress.XtraNavBar.NavBarItem navBarItemNewCase;
        private DevExpress.XtraNavBar.NavBarItem navBarItemMyCaseList;
        private DevExpress.XtraNavBar.NavBarItem navBarItemCurrCase;
    }
}
