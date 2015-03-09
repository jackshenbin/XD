namespace BOCOM.IVX.Controls
{
    partial class FormDownloadInfo
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumndstName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumntype = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnprogress = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnsrcVideoName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumndownloadPath = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnhrItem = new DevExpress.XtraGrid.Columns.GridColumn();
            this.toolTipController1 = new DevExpress.Utils.ToolTipController(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(384, 262);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.gridControl1.DoubleClick += new System.EventHandler(this.gridControl1_DoubleClick);
            // 
            // gridViewTaskUnit
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumndstName,
            this.gridColumntype,
            this.gridColumnprogress,
            this.gridColumnsrcVideoName,
            this.gridColumndownloadPath,
            this.gridColumnhrItem});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridViewTaskUnit";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsSelection.MultiSelect = true;
            this.gridView1.OptionsView.EnableAppearanceEvenRow = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumndstName
            // 
            this.gridColumndstName.Caption = "文件名";
            this.gridColumndstName.FieldName = "dstName";
            this.gridColumndstName.Name = "gridColumndstName";
            this.gridColumndstName.Visible = true;
            this.gridColumndstName.VisibleIndex = 0;
            this.gridColumndstName.Width = 200;
            // 
            // gridColumntype
            // 
            this.gridColumntype.Caption = "资源类型";
            this.gridColumntype.FieldName = "type";
            this.gridColumntype.Name = "gridColumntype";
            this.gridColumntype.Visible = true;
            this.gridColumntype.VisibleIndex = 1;
            this.gridColumntype.Width = 90;
            // 
            // gridColumnprogress
            // 
            this.gridColumnprogress.Caption = "进度";
            this.gridColumnprogress.FieldName = "progress";
            this.gridColumnprogress.Name = "gridColumnprogress";
            this.gridColumnprogress.Visible = true;
            this.gridColumnprogress.VisibleIndex = 2;
            this.gridColumnprogress.Width = 78;
            // 
            // gridColumnsrcVideoName
            // 
            this.gridColumnsrcVideoName.Caption = "所属资源";
            this.gridColumnsrcVideoName.FieldName = "srcVideoName";
            this.gridColumnsrcVideoName.Name = "gridColumnsrcVideoName";
            this.gridColumnsrcVideoName.Width = 166;
            // 
            // gridColumndownloadPath
            // 
            this.gridColumndownloadPath.Caption = "保存位置";
            this.gridColumndownloadPath.FieldName = "downloadPath";
            this.gridColumndownloadPath.Name = "gridColumndownloadPath";
            this.gridColumndownloadPath.Width = 158;
            // 
            // gridColumnhrItem
            // 
            this.gridColumnhrItem.Caption = "hrItem";
            this.gridColumnhrItem.FieldName = "hrItem";
            this.gridColumnhrItem.Name = "gridColumnhrItem";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Controls.Add(this.checkEdit1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 262);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(384, 30);
            this.panel1.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl1.Location = new System.Drawing.Point(84, 9);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(288, 14);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "共 0 个";
            // 
            // checkEdit1
            // 
            this.checkEdit1.Location = new System.Drawing.Point(3, 6);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "完整模式";
            this.checkEdit1.Size = new System.Drawing.Size(75, 19);
            this.checkEdit1.TabIndex = 1;
            this.checkEdit1.CheckedChanged += new System.EventHandler(this.checkEdit1_CheckedChanged);
            // 
            // FormDownloadInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(384, 292);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.panel1);
            this.LookAndFeel.SkinName = "Darkroom";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 330);
            this.Name = "FormDownloadInfo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "文件下载";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDownloadInfo_FormClosing);
            this.Load += new System.EventHandler(this.FormDownloadInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumndstName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumntype;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnprogress;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumndownloadPath;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnsrcVideoName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnhrItem;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.Utils.ToolTipController toolTipController1;
    }
}