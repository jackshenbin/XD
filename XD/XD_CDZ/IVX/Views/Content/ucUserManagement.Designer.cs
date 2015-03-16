namespace BOCOM.IVX.Views.Content
{
    partial class ucUserManagement
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucUserManagement));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.expandablePanel1 = new DevComponents.DotNetBar.ExpandablePanel();
            this.btnUserInfo = new DevComponents.DotNetBar.ButtonX();
            this.btnChangePass = new DevComponents.DotNetBar.ButtonX();
            this.btnAddUser = new DevComponents.DotNetBar.ButtonX();
            this.ucCurrentUser1 = new BOCOM.IVX.Views.Content.ucCurrentUser();
            this.superTabControl1 = new DevComponents.DotNetBar.SuperTabControl();
            this.superTabControlPanel1 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.dataGridViewX1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ucAddUserPanel1 = new BOCOM.IVX.Views.Content.ucAddUserPanel();
            this.bindingNavigatorEx1 = new DevComponents.DotNetBar.Controls.BindingNavigatorEx(this.components);
            this.bindingNavigatorAddNewItem = new DevComponents.DotNetBar.ButtonItem();
            this.bindingNavigatorCountItem = new DevComponents.DotNetBar.LabelItem();
            this.bindingNavigatorDeleteItem = new DevComponents.DotNetBar.ButtonItem();
            this.bindingNavigatorMoveFirstItem = new DevComponents.DotNetBar.ButtonItem();
            this.bindingNavigatorMovePreviousItem = new DevComponents.DotNetBar.ButtonItem();
            this.bindingNavigatorPositionItem = new DevComponents.DotNetBar.TextBoxItem();
            this.bindingNavigatorMoveNextItem = new DevComponents.DotNetBar.ButtonItem();
            this.bindingNavigatorMoveLastItem = new DevComponents.DotNetBar.ButtonItem();
            this.superTabItem1 = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel0 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.superTabItem0 = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel3 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.ucCurrUserPanel1 = new BOCOM.IVX.Views.Content.ucCurrUserPanel();
            this.superTabItem3 = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel2 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.ucChangeUserPass1 = new BOCOM.IVX.Views.Content.ucChangeUserPass();
            this.superTabItem2 = new DevComponents.DotNetBar.SuperTabItem();
            this.sideBar1 = new DevComponents.DotNetBar.SideBar();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.expandablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).BeginInit();
            this.superTabControl1.SuspendLayout();
            this.superTabControlPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigatorEx1)).BeginInit();
            this.superTabControlPanel3.SuspendLayout();
            this.superTabControlPanel2.SuspendLayout();
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
            this.splitContainer1.Size = new System.Drawing.Size(660, 592);
            this.splitContainer1.SplitterDistance = 185;
            this.splitContainer1.TabIndex = 0;
            // 
            // expandablePanel1
            // 
            this.expandablePanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.expandablePanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.expandablePanel1.Controls.Add(this.btnUserInfo);
            this.expandablePanel1.Controls.Add(this.btnChangePass);
            this.expandablePanel1.Controls.Add(this.btnAddUser);
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
            this.expandablePanel1.TabIndex = 5;
            this.expandablePanel1.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this.expandablePanel1.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandablePanel1.TitleStyle.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.expandablePanel1.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this.expandablePanel1.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandablePanel1.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.expandablePanel1.TitleStyle.GradientAngle = 90;
            this.expandablePanel1.TitleText = "权限管理";
            // 
            // btnUserInfo
            // 
            this.btnUserInfo.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnUserInfo.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnUserInfo.Location = new System.Drawing.Point(29, 111);
            this.btnUserInfo.Name = "btnUserInfo";
            this.btnUserInfo.Size = new System.Drawing.Size(113, 28);
            this.btnUserInfo.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnUserInfo.TabIndex = 1;
            this.btnUserInfo.Text = "查看个人信息";
            this.btnUserInfo.Click += new System.EventHandler(this.btnUserInfo_Click);
            // 
            // btnChangePass
            // 
            this.btnChangePass.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnChangePass.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnChangePass.Location = new System.Drawing.Point(29, 77);
            this.btnChangePass.Name = "btnChangePass";
            this.btnChangePass.Size = new System.Drawing.Size(113, 28);
            this.btnChangePass.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnChangePass.TabIndex = 1;
            this.btnChangePass.Text = "修改密码";
            this.btnChangePass.Click += new System.EventHandler(this.btnChangePass_Click);
            // 
            // btnAddUser
            // 
            this.btnAddUser.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAddUser.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAddUser.Location = new System.Drawing.Point(29, 43);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(113, 28);
            this.btnAddUser.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnAddUser.TabIndex = 1;
            this.btnAddUser.Text = "用户管理";
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
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
            this.superTabControl1.Controls.Add(this.superTabControlPanel1);
            this.superTabControl1.Controls.Add(this.superTabControlPanel0);
            this.superTabControl1.Controls.Add(this.superTabControlPanel3);
            this.superTabControl1.Controls.Add(this.superTabControlPanel2);
            this.superTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControl1.Location = new System.Drawing.Point(0, 0);
            this.superTabControl1.Name = "superTabControl1";
            this.superTabControl1.ReorderTabsEnabled = true;
            this.superTabControl1.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.superTabControl1.SelectedTabIndex = 0;
            this.superTabControl1.Size = new System.Drawing.Size(471, 592);
            this.superTabControl1.TabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.superTabControl1.TabIndex = 0;
            this.superTabControl1.TabLayoutType = DevComponents.DotNetBar.eSuperTabLayoutType.SingleLineFit;
            this.superTabControl1.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabItem0,
            this.superTabItem1,
            this.superTabItem2,
            this.superTabItem3});
            this.superTabControl1.TabsVisible = false;
            this.superTabControl1.Text = "superTabControl1";
            // 
            // superTabControlPanel1
            // 
            this.superTabControlPanel1.Controls.Add(this.dataGridViewX1);
            this.superTabControlPanel1.Controls.Add(this.bindingNavigatorEx1);
            this.superTabControlPanel1.Controls.Add(this.panel1);
            this.superTabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel1.Location = new System.Drawing.Point(0, 26);
            this.superTabControlPanel1.Name = "superTabControlPanel1";
            this.superTabControlPanel1.Size = new System.Drawing.Size(471, 566);
            this.superTabControlPanel1.TabIndex = 1;
            this.superTabControlPanel1.TabItem = this.superTabItem1;
            // 
            // dataGridViewX1
            // 
            this.dataGridViewX1.AllowUserToAddRows = false;
            this.dataGridViewX1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.dataGridViewX1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewX1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridViewX1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewX1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.Location = new System.Drawing.Point(0, 25);
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.ReadOnly = true;
            this.dataGridViewX1.RowTemplate.Height = 23;
            this.dataGridViewX1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewX1.Size = new System.Drawing.Size(471, 360);
            this.dataGridViewX1.TabIndex = 0;
            this.dataGridViewX1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewX1_DataError);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "id";
            this.Column1.HeaderText = "ID";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Visible = false;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "user_name";
            this.Column2.HeaderText = "用户名";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "passwd";
            this.Column3.HeaderText = "密码";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Visible = false;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "user_auth";
            this.Column4.DropDownHeight = 106;
            this.Column4.DropDownWidth = 121;
            this.Column4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Column4.HeaderText = "角色";
            this.Column4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Column4.IntegralHeight = false;
            this.Column4.ItemHeight = 16;
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "reg_time";
            this.Column5.HeaderText = "注册时间";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 200;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ucAddUserPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 385);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(471, 181);
            this.panel1.TabIndex = 2;
            // 
            // ucAddUserPanel1
            // 
            this.ucAddUserPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucAddUserPanel1.Location = new System.Drawing.Point(0, 0);
            this.ucAddUserPanel1.Name = "ucAddUserPanel1";
            this.ucAddUserPanel1.Size = new System.Drawing.Size(471, 181);
            this.ucAddUserPanel1.TabIndex = 0;
            // 
            // bindingNavigatorEx1
            // 
            this.bindingNavigatorEx1.AddNewRecordButton = this.bindingNavigatorAddNewItem;
            this.bindingNavigatorEx1.AntiAlias = true;
            this.bindingNavigatorEx1.CountLabel = this.bindingNavigatorCountItem;
            this.bindingNavigatorEx1.CountLabelFormat = "共 {0}";
            this.bindingNavigatorEx1.DeleteButton = this.bindingNavigatorDeleteItem;
            this.bindingNavigatorEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.bindingNavigatorEx1.DockSide = DevComponents.DotNetBar.eDockSide.Document;
            this.bindingNavigatorEx1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.bindingNavigatorEx1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem});
            this.bindingNavigatorEx1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigatorEx1.MoveFirstButton = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigatorEx1.MoveLastButton = this.bindingNavigatorMoveLastItem;
            this.bindingNavigatorEx1.MoveNextButton = this.bindingNavigatorMoveNextItem;
            this.bindingNavigatorEx1.MovePreviousButton = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigatorEx1.Name = "bindingNavigatorEx1";
            this.bindingNavigatorEx1.PositionTextBox = this.bindingNavigatorPositionItem;
            this.bindingNavigatorEx1.Size = new System.Drawing.Size(471, 25);
            this.bindingNavigatorEx1.Stretch = true;
            this.bindingNavigatorEx1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.bindingNavigatorEx1.TabIndex = 1;
            this.bindingNavigatorEx1.TabStop = false;
            this.bindingNavigatorEx1.Text = "bindingNavigatorEx1";
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.Text = "Add new";
            this.bindingNavigatorAddNewItem.Visible = false;
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Text = "共 {0}";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.Text = "Delete";
            this.bindingNavigatorDeleteItem.Visible = false;
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.BeginGroup = true;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.TextBoxWidth = 54;
            this.bindingNavigatorPositionItem.WatermarkColor = System.Drawing.SystemColors.GrayText;
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.BeginGroup = true;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // superTabItem1
            // 
            this.superTabItem1.AttachedControl = this.superTabControlPanel1;
            this.superTabItem1.GlobalItem = false;
            this.superTabItem1.Name = "superTabItem1";
            this.superTabItem1.Text = "superTabItem1";
            // 
            // superTabControlPanel0
            // 
            this.superTabControlPanel0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel0.Location = new System.Drawing.Point(0, 26);
            this.superTabControlPanel0.Name = "superTabControlPanel0";
            this.superTabControlPanel0.Size = new System.Drawing.Size(471, 566);
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
            // superTabControlPanel3
            // 
            this.superTabControlPanel3.Controls.Add(this.ucCurrUserPanel1);
            this.superTabControlPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel3.Location = new System.Drawing.Point(0, 0);
            this.superTabControlPanel3.Name = "superTabControlPanel3";
            this.superTabControlPanel3.Size = new System.Drawing.Size(471, 592);
            this.superTabControlPanel3.TabIndex = 0;
            this.superTabControlPanel3.TabItem = this.superTabItem3;
            // 
            // ucCurrUserPanel1
            // 
            this.ucCurrUserPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucCurrUserPanel1.Location = new System.Drawing.Point(0, 0);
            this.ucCurrUserPanel1.Name = "ucCurrUserPanel1";
            this.ucCurrUserPanel1.Size = new System.Drawing.Size(471, 592);
            this.ucCurrUserPanel1.TabIndex = 0;
            // 
            // superTabItem3
            // 
            this.superTabItem3.AttachedControl = this.superTabControlPanel3;
            this.superTabItem3.GlobalItem = false;
            this.superTabItem3.Name = "superTabItem3";
            this.superTabItem3.Text = "superTabItem3";
            // 
            // superTabControlPanel2
            // 
            this.superTabControlPanel2.Controls.Add(this.ucChangeUserPass1);
            this.superTabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel2.Location = new System.Drawing.Point(0, 0);
            this.superTabControlPanel2.Name = "superTabControlPanel2";
            this.superTabControlPanel2.Size = new System.Drawing.Size(471, 592);
            this.superTabControlPanel2.TabIndex = 0;
            this.superTabControlPanel2.TabItem = this.superTabItem2;
            // 
            // ucChangeUserPass1
            // 
            this.ucChangeUserPass1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucChangeUserPass1.Location = new System.Drawing.Point(0, 0);
            this.ucChangeUserPass1.Name = "ucChangeUserPass1";
            this.ucChangeUserPass1.Size = new System.Drawing.Size(471, 592);
            this.ucChangeUserPass1.TabIndex = 0;
            // 
            // superTabItem2
            // 
            this.superTabItem2.AttachedControl = this.superTabControlPanel2;
            this.superTabItem2.GlobalItem = false;
            this.superTabItem2.Name = "superTabItem2";
            this.superTabItem2.Text = "superTabItem2";
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
            // ucUserManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ucUserManagement";
            this.Size = new System.Drawing.Size(660, 592);
            this.Load += new System.EventHandler(this.ucUserManagement_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.expandablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).EndInit();
            this.superTabControl1.ResumeLayout(false);
            this.superTabControlPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigatorEx1)).EndInit();
            this.superTabControlPanel3.ResumeLayout(false);
            this.superTabControlPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private ucCurrentUser ucCurrentUser1;
        private DevComponents.DotNetBar.SuperTabControl superTabControl1;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel3;
        private DevComponents.DotNetBar.SuperTabItem superTabItem3;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel2;
        private DevComponents.DotNetBar.SuperTabItem superTabItem2;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel1;
        private DevComponents.DotNetBar.SuperTabItem superTabItem1;
        private DevComponents.DotNetBar.Controls.BindingNavigatorEx bindingNavigatorEx1;
        private DevComponents.DotNetBar.ButtonItem bindingNavigatorAddNewItem;
        private DevComponents.DotNetBar.LabelItem bindingNavigatorCountItem;
        private DevComponents.DotNetBar.ButtonItem bindingNavigatorDeleteItem;
        private DevComponents.DotNetBar.ButtonItem bindingNavigatorMoveFirstItem;
        private DevComponents.DotNetBar.ButtonItem bindingNavigatorMovePreviousItem;
        private DevComponents.DotNetBar.TextBoxItem bindingNavigatorPositionItem;
        private DevComponents.DotNetBar.ButtonItem bindingNavigatorMoveNextItem;
        private DevComponents.DotNetBar.ButtonItem bindingNavigatorMoveLastItem;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private System.Windows.Forms.Panel panel1;
        private ucAddUserPanel ucAddUserPanel1;
        private ucChangeUserPass ucChangeUserPass1;
        private DevComponents.DotNetBar.SideBar sideBar1;
        private ucCurrUserPanel ucCurrUserPanel1;
        private DevComponents.DotNetBar.ExpandablePanel expandablePanel1;
        private DevComponents.DotNetBar.ButtonX btnUserInfo;
        private DevComponents.DotNetBar.ButtonX btnChangePass;
        private DevComponents.DotNetBar.ButtonX btnAddUser;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel0;
        private DevComponents.DotNetBar.SuperTabItem superTabItem0;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
    }
}
