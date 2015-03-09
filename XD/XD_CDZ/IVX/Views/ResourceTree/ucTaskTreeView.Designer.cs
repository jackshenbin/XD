namespace BOCOM.IVX.Views.ResourceTree
{
    partial class ucTaskTreeView
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
            this.navBarGroup1 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarItemNewTask = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItemTaskStatus = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItemFinishedTasks = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItemRunningTask = new DevExpress.XtraNavBar.NavBarItem();
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
            "新建任务"}, -1);
            this.treeList2.AppendNode(new object[] {
            "任务状态"}, -1);
            this.treeList2.AppendNode(new object[] {
            "未完成任务"}, 1);
            this.treeList2.AppendNode(new object[] {
            "已完成任务"}, 1);
            this.treeList2.EndUnboundLoad();
            this.treeList2.OptionsBehavior.Editable = false;
            this.treeList2.OptionsMenu.EnableColumnMenu = false;
            this.treeList2.OptionsMenu.EnableFooterMenu = false;
            this.treeList2.OptionsMenu.ShowAutoFilterRowItem = false;
            this.treeList2.OptionsPrint.UsePrintStyles = true;
            this.treeList2.Size = new System.Drawing.Size(241, 410);
            this.treeList2.TabIndex = 5;
            this.treeList2.Visible = false;
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "任务";
            this.treeListColumn1.FieldName = "任务";
            this.treeListColumn1.MinWidth = 70;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            // 
            // navBarControl4
            // 
            this.navBarControl4.ActiveGroup = this.navBarGroup1;
            this.navBarControl4.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.navBarControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navBarControl4.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.navBarGroup1});
            this.navBarControl4.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] {
            this.navBarItemRunningTask,
            this.navBarItemFinishedTasks,
            this.navBarItemNewTask,
            this.navBarItemTaskStatus});
            this.navBarControl4.Location = new System.Drawing.Point(2, 2);
            this.navBarControl4.LookAndFeel.SkinName = "Metropolis Dark";
            this.navBarControl4.Name = "navBarControl4";
            this.navBarControl4.OptionsNavPane.ExpandedWidth = 241;
            this.navBarControl4.Size = new System.Drawing.Size(241, 410);
            this.navBarControl4.StoreDefaultPaintStyleName = true;
            this.navBarControl4.TabIndex = 6;
            this.navBarControl4.Text = "navBarControl4";
            // 
            // navBarGroup1
            // 
            this.navBarGroup1.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.navBarGroup1.Appearance.Options.UseForeColor = true;
            this.navBarGroup1.Caption = "任务管理";
            this.navBarGroup1.Expanded = true;
            this.navBarGroup1.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.SmallIconsText;
            this.navBarGroup1.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItemNewTask),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItemTaskStatus),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItemFinishedTasks),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItemRunningTask)});
            this.navBarGroup1.Name = "navBarGroup1";
            // 
            // navBarItemNewTask
            // 
            this.navBarItemNewTask.Caption = "新建任务";
            this.navBarItemNewTask.Name = "navBarItemNewTask";
            this.navBarItemNewTask.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.ItemClicked);
            // 
            // navBarItemTaskStatus
            // 
            this.navBarItemTaskStatus.Caption = "任务状态";
            this.navBarItemTaskStatus.Name = "navBarItemTaskStatus";
            this.navBarItemTaskStatus.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.ItemClicked);
            // 
            // navBarItemFinishedTasks
            // 
            this.navBarItemFinishedTasks.Caption = "已经完成的任务";
            this.navBarItemFinishedTasks.Name = "navBarItemFinishedTasks";
            this.navBarItemFinishedTasks.Visible = false;
            // 
            // navBarItemRunningTask
            // 
            this.navBarItemRunningTask.Caption = "正在进行的任务";
            this.navBarItemRunningTask.Name = "navBarItemRunningTask";
            this.navBarItemRunningTask.Visible = false;
            // 
            // ucTaskTreeView
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.navBarControl4);
            this.Controls.Add(this.treeList2);
            this.Name = "ucTaskTreeView";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(245, 414);
            ((System.ComponentModel.ISupportInitialize)(this.treeList2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTreeList.TreeList treeList2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraNavBar.NavBarControl navBarControl4;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup1;
        private DevExpress.XtraNavBar.NavBarItem navBarItemNewTask;
        private DevExpress.XtraNavBar.NavBarItem navBarItemTaskStatus;
        private DevExpress.XtraNavBar.NavBarItem navBarItemFinishedTasks;
        private DevExpress.XtraNavBar.NavBarItem navBarItemRunningTask;
    }
}
