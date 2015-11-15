namespace BOCOM.IVX.Views.Content
{
    partial class ucCDZRuntimeStatPanel
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnChangeDevSetting = new DevComponents.DotNetBar.ButtonX();
            this.labelX19 = new DevComponents.DotNetBar.LabelX();
            this.buttonFlash = new DevComponents.DotNetBar.ButtonX();
            this.dataGridViewX1 = new BOCOM.IVX.Controls.GridViewEx();
            this.DevID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WorkStat = new DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn();
            this.UserID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsOnline = new DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn();
            this.ServiceStat = new DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn();
            this.ChongDianShuChuDianYa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChongDianShuChuDianLiu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ShuChuJiDianQiZhuangTai = new DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn();
            this.LianJieQueRenKaiGuanZhuangTai = new DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn();
            this.ShiFouLianJieDianChi = new DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn();
            this.DevType = new DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn();
            this.YouGongZongDianDu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FactoryID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DevSoftVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CRC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupPanel1
            // 
            this.groupPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.btnChangeDevSetting);
            this.groupPanel1.Controls.Add(this.labelX19);
            this.groupPanel1.Controls.Add(this.buttonFlash);
            this.groupPanel1.Controls.Add(this.dataGridViewX1);
            this.groupPanel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupPanel1.Location = new System.Drawing.Point(3, 3);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(847, 986);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel1.TabIndex = 25;
            this.groupPanel1.Click += new System.EventHandler(this.groupPanel1_Click);
            // 
            // btnChangeDevSetting
            // 
            this.btnChangeDevSetting.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnChangeDevSetting.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnChangeDevSetting.Location = new System.Drawing.Point(88, 39);
            this.btnChangeDevSetting.Name = "btnChangeDevSetting";
            this.btnChangeDevSetting.Size = new System.Drawing.Size(143, 25);
            this.btnChangeDevSetting.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnChangeDevSetting.TabIndex = 30;
            this.btnChangeDevSetting.Text = "修改充电桩参数";
            this.btnChangeDevSetting.Click += new System.EventHandler(this.btnChangeDevSetting_Click);
            // 
            // labelX19
            // 
            this.labelX19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.labelX19.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX19.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX19.Location = new System.Drawing.Point(0, 0);
            this.labelX19.Name = "labelX19";
            this.labelX19.Size = new System.Drawing.Size(158, 23);
            this.labelX19.TabIndex = 29;
            this.labelX19.Text = "充电桩实时状态：";
            // 
            // buttonFlash
            // 
            this.buttonFlash.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonFlash.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonFlash.Location = new System.Drawing.Point(5, 39);
            this.buttonFlash.Name = "buttonFlash";
            this.buttonFlash.Size = new System.Drawing.Size(70, 25);
            this.buttonFlash.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonFlash.TabIndex = 28;
            this.buttonFlash.Text = "刷新";
            this.buttonFlash.Click += new System.EventHandler(this.buttonFlash_Click);
            // 
            // dataGridViewX1
            // 
            this.dataGridViewX1.AllowUserToAddRows = false;
            this.dataGridViewX1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.dataGridViewX1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewX1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewX1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(240)))));
            this.dataGridViewX1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewX1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewX1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DevID,
            this.WorkStat,
            this.UserID,
            this.IsOnline,
            this.ServiceStat,
            this.ChongDianShuChuDianYa,
            this.ChongDianShuChuDianLiu,
            this.ShuChuJiDianQiZhuangTai,
            this.LianJieQueRenKaiGuanZhuangTai,
            this.ShiFouLianJieDianChi,
            this.DevType,
            this.YouGongZongDianDu,
            this.FactoryID,
            this.DevSoftVersion,
            this.CRC});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.Location = new System.Drawing.Point(5, 76);
            this.dataGridViewX1.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.ReadOnly = true;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewX1.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewX1.RowTemplate.Height = 23;
            this.dataGridViewX1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewX1.Size = new System.Drawing.Size(708, 450);
            this.dataGridViewX1.TabIndex = 27;
            this.dataGridViewX1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewX1_CellMouseDoubleClick);
            this.dataGridViewX1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewX1_DataError);
            this.dataGridViewX1.SelectionChanged += new System.EventHandler(this.dataGridViewX1_SelectionChanged);
            // 
            // DevID
            // 
            this.DevID.DataPropertyName = "DevID";
            this.DevID.HeaderText = "充电桩编号";
            this.DevID.Name = "DevID";
            this.DevID.ReadOnly = true;
            this.DevID.Width = 150;
            // 
            // WorkStat
            // 
            this.WorkStat.DataPropertyName = "WorkStat";
            this.WorkStat.DropDownHeight = 106;
            this.WorkStat.DropDownWidth = 121;
            this.WorkStat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.WorkStat.HeaderText = "当前状态";
            this.WorkStat.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.WorkStat.IntegralHeight = false;
            this.WorkStat.ItemHeight = 16;
            this.WorkStat.Name = "WorkStat";
            this.WorkStat.ReadOnly = true;
            this.WorkStat.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.WorkStat.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // UserID
            // 
            this.UserID.DataPropertyName = "UserID";
            this.UserID.HeaderText = "用户编号";
            this.UserID.Name = "UserID";
            this.UserID.ReadOnly = true;
            this.UserID.Visible = false;
            this.UserID.Width = 150;
            // 
            // IsOnline
            // 
            this.IsOnline.Checked = true;
            this.IsOnline.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.IsOnline.CheckValue = "N";
            this.IsOnline.DataPropertyName = "IsOnline";
            this.IsOnline.HeaderText = "在线状态";
            this.IsOnline.Name = "IsOnline";
            this.IsOnline.ReadOnly = true;
            this.IsOnline.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsOnline.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.IsOnline.Visible = false;
            // 
            // ServiceStat
            // 
            this.ServiceStat.DataPropertyName = "ServiceStat";
            this.ServiceStat.DropDownHeight = 106;
            this.ServiceStat.DropDownWidth = 121;
            this.ServiceStat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ServiceStat.HeaderText = "服务状态";
            this.ServiceStat.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ServiceStat.IntegralHeight = false;
            this.ServiceStat.ItemHeight = 16;
            this.ServiceStat.Name = "ServiceStat";
            this.ServiceStat.ReadOnly = true;
            this.ServiceStat.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ServiceStat.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ServiceStat.Visible = false;
            // 
            // ChongDianShuChuDianYa
            // 
            this.ChongDianShuChuDianYa.DataPropertyName = "ChongDianShuChuDianYa";
            this.ChongDianShuChuDianYa.HeaderText = "充电输出电压";
            this.ChongDianShuChuDianYa.Name = "ChongDianShuChuDianYa";
            this.ChongDianShuChuDianYa.ReadOnly = true;
            this.ChongDianShuChuDianYa.Width = 102;
            // 
            // ChongDianShuChuDianLiu
            // 
            this.ChongDianShuChuDianLiu.DataPropertyName = "ChongDianShuChuDianLiu";
            this.ChongDianShuChuDianLiu.HeaderText = "充电输出电流";
            this.ChongDianShuChuDianLiu.Name = "ChongDianShuChuDianLiu";
            this.ChongDianShuChuDianLiu.ReadOnly = true;
            // 
            // ShuChuJiDianQiZhuangTai
            // 
            this.ShuChuJiDianQiZhuangTai.Checked = true;
            this.ShuChuJiDianQiZhuangTai.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.ShuChuJiDianQiZhuangTai.CheckValue = "N";
            this.ShuChuJiDianQiZhuangTai.DataPropertyName = "ShuChuJiDianQiZhuangTai";
            this.ShuChuJiDianQiZhuangTai.HeaderText = "输出继电器状态";
            this.ShuChuJiDianQiZhuangTai.Name = "ShuChuJiDianQiZhuangTai";
            this.ShuChuJiDianQiZhuangTai.ReadOnly = true;
            this.ShuChuJiDianQiZhuangTai.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ShuChuJiDianQiZhuangTai.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ShuChuJiDianQiZhuangTai.Visible = false;
            // 
            // LianJieQueRenKaiGuanZhuangTai
            // 
            this.LianJieQueRenKaiGuanZhuangTai.DataPropertyName = "LianJieQueRenKaiGuanZhuangTai";
            this.LianJieQueRenKaiGuanZhuangTai.DropDownHeight = 106;
            this.LianJieQueRenKaiGuanZhuangTai.DropDownWidth = 121;
            this.LianJieQueRenKaiGuanZhuangTai.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LianJieQueRenKaiGuanZhuangTai.HeaderText = "连接确认开关状态";
            this.LianJieQueRenKaiGuanZhuangTai.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LianJieQueRenKaiGuanZhuangTai.IntegralHeight = false;
            this.LianJieQueRenKaiGuanZhuangTai.ItemHeight = 16;
            this.LianJieQueRenKaiGuanZhuangTai.Name = "LianJieQueRenKaiGuanZhuangTai";
            this.LianJieQueRenKaiGuanZhuangTai.ReadOnly = true;
            this.LianJieQueRenKaiGuanZhuangTai.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.LianJieQueRenKaiGuanZhuangTai.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LianJieQueRenKaiGuanZhuangTai.Width = 126;
            // 
            // ShiFouLianJieDianChi
            // 
            this.ShiFouLianJieDianChi.Checked = true;
            this.ShiFouLianJieDianChi.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.ShiFouLianJieDianChi.CheckValue = "N";
            this.ShiFouLianJieDianChi.DataPropertyName = "ShiFouLianJieDianChi";
            this.ShiFouLianJieDianChi.HeaderText = "是否连接电池";
            this.ShiFouLianJieDianChi.Name = "ShiFouLianJieDianChi";
            this.ShiFouLianJieDianChi.ReadOnly = true;
            this.ShiFouLianJieDianChi.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ShiFouLianJieDianChi.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ShiFouLianJieDianChi.Visible = false;
            // 
            // DevType
            // 
            this.DevType.DataPropertyName = "DevType";
            this.DevType.DropDownHeight = 106;
            this.DevType.DropDownWidth = 121;
            this.DevType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DevType.HeaderText = "充电桩类型";
            this.DevType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.DevType.IntegralHeight = false;
            this.DevType.ItemHeight = 16;
            this.DevType.Name = "DevType";
            this.DevType.ReadOnly = true;
            this.DevType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DevType.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // YouGongZongDianDu
            // 
            this.YouGongZongDianDu.DataPropertyName = "YouGongZongDianDu";
            this.YouGongZongDianDu.HeaderText = "有功总电度";
            this.YouGongZongDianDu.Name = "YouGongZongDianDu";
            this.YouGongZongDianDu.ReadOnly = true;
            // 
            // FactoryID
            // 
            this.FactoryID.DataPropertyName = "FactoryID";
            this.FactoryID.HeaderText = "厂商编号";
            this.FactoryID.Name = "FactoryID";
            this.FactoryID.ReadOnly = true;
            // 
            // DevSoftVersion
            // 
            this.DevSoftVersion.DataPropertyName = "DevSoftVersion";
            this.DevSoftVersion.HeaderText = "软件版本";
            this.DevSoftVersion.Name = "DevSoftVersion";
            this.DevSoftVersion.ReadOnly = true;
            // 
            // CRC
            // 
            this.CRC.DataPropertyName = "CRC";
            dataGridViewCellStyle3.Format = "X4";
            this.CRC.DefaultCellStyle = dataGridViewCellStyle3;
            this.CRC.HeaderText = "唯一CRC校验码";
            this.CRC.Name = "CRC";
            this.CRC.ReadOnly = true;
            this.CRC.Width = 108;
            // 
            // ucCDZRuntimeStatPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.Controls.Add(this.groupPanel1);
            this.Name = "ucCDZRuntimeStatPanel";
            this.Size = new System.Drawing.Size(853, 1001);
            this.Load += new System.EventHandler(this.ucCDZStatPanel_Load);
            this.groupPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private BOCOM.IVX.Controls.GridViewEx dataGridViewX1;
        private DevComponents.DotNetBar.ButtonX buttonFlash;
        private DevComponents.DotNetBar.LabelX labelX19;
        private System.Windows.Forms.DataGridViewTextBoxColumn DevID;
        private DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn WorkStat;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserID;
        private DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn IsOnline;
        private DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn ServiceStat;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChongDianShuChuDianYa;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChongDianShuChuDianLiu;
        private DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn ShuChuJiDianQiZhuangTai;
        private DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn LianJieQueRenKaiGuanZhuangTai;
        private DevComponents.DotNetBar.Controls.DataGridViewCheckBoxXColumn ShiFouLianJieDianChi;
        private DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn DevType;
        private System.Windows.Forms.DataGridViewTextBoxColumn YouGongZongDianDu;
        private System.Windows.Forms.DataGridViewTextBoxColumn FactoryID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DevSoftVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn CRC;
        private DevComponents.DotNetBar.ButtonX btnChangeDevSetting;

    }
}
