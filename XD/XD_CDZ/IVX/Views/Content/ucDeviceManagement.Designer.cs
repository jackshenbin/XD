namespace BOCOM.IVX.Views.Content
{
    partial class ucDeviceManagement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucDeviceManagement));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.advTree1 = new DevComponents.AdvTree.AdvTree();
            this.columnHeader1 = new DevComponents.AdvTree.ColumnHeader();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.node1 = new DevComponents.AdvTree.Node();
            this.node5 = new DevComponents.AdvTree.Node();
            this.node6 = new DevComponents.AdvTree.Node();
            this.node2 = new DevComponents.AdvTree.Node();
            this.node3 = new DevComponents.AdvTree.Node();
            this.node4 = new DevComponents.AdvTree.Node();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle4 = new DevComponents.DotNetBar.ElementStyle();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.expandablePanel1 = new DevComponents.DotNetBar.ExpandablePanel();
            this.btnSearchDeviceStat = new DevComponents.DotNetBar.ButtonX();
            this.btnRealtimeStat = new DevComponents.DotNetBar.ButtonX();
            this.buttonDelDevice = new DevComponents.DotNetBar.ButtonX();
            this.btnAddDevice = new DevComponents.DotNetBar.ButtonX();
            this.ucCurrentUser1 = new BOCOM.IVX.Views.Content.ucCurrentUser();
            this.superTabControl1 = new DevComponents.DotNetBar.SuperTabControl();
            this.superTabControlPanel0 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.superTabItem0 = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel10 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.ucCDZRuntimeStatPanel1 = new BOCOM.IVX.Views.Content.ucCDZRuntimeStatPanel();
            this.superTabItem4 = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel9 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.ucDelCDZPanel1 = new BOCOM.IVX.Views.Content.ucDelCDZPanel();
            this.superTabItem3 = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel2 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.ucCDZStatPanel1 = new BOCOM.IVX.Views.Content.ucCDZStatPanel();
            this.superTabItem2 = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel1 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.ucAddCDZPanel1 = new BOCOM.IVX.Views.Content.ucAddCDZPanel();
            this.superTabItem1 = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel4 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.superTabControlPanel3 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.superTabControlPanel8 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.superTabControlPanel7 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.superTabControlPanel6 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.superTabControlPanel5 = new DevComponents.DotNetBar.SuperTabControlPanel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advTree1)).BeginInit();
            this.expandablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).BeginInit();
            this.superTabControl1.SuspendLayout();
            this.superTabControlPanel10.SuspendLayout();
            this.superTabControlPanel9.SuspendLayout();
            this.superTabControlPanel2.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.advTree1);
            this.splitContainer1.Panel1.Controls.Add(this.expandablePanel1);
            this.splitContainer1.Panel1.Controls.Add(this.ucCurrentUser1);
            this.splitContainer1.Panel1MinSize = 185;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.superTabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(871, 507);
            this.splitContainer1.SplitterDistance = 185;
            this.splitContainer1.TabIndex = 2;
            // 
            // advTree1
            // 
            this.advTree1.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTree1.AllowDrop = true;
            this.advTree1.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTree1.BackgroundStyle.Class = "TreeBorderKey";
            this.advTree1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.advTree1.Columns.Add(this.columnHeader1);
            this.advTree1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advTree1.ExpandWidth = 4;
            this.advTree1.ImageList = this.imageList1;
            this.advTree1.Location = new System.Drawing.Point(0, 246);
            this.advTree1.Name = "advTree1";
            this.advTree1.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node1,
            this.node2,
            this.node3,
            this.node4});
            this.advTree1.NodesConnector = this.nodeConnector1;
            this.advTree1.NodeStyle = this.elementStyle4;
            this.advTree1.PathSeparator = ";";
            this.advTree1.Size = new System.Drawing.Size(185, 261);
            this.advTree1.Styles.Add(this.elementStyle4);
            this.advTree1.Styles.Add(this.elementStyle1);
            this.advTree1.TabIndex = 6;
            this.advTree1.Text = "advTree1";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Name = "columnHeader1";
            this.columnHeader1.Text = "充电桩列表";
            this.columnHeader1.Width.Absolute = 150;
            this.columnHeader1.Width.AutoSize = true;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "cdz.jpg");
            this.imageList1.Images.SetKeyName(1, "cdz交流.jpg");
            this.imageList1.Images.SetKeyName(2, "cdz直流.jpg");
            this.imageList1.Images.SetKeyName(3, "department.jpg");
            this.imageList1.Images.SetKeyName(4, "地区.png");
            this.imageList1.Images.SetKeyName(5, "区域.png");
            this.imageList1.Images.SetKeyName(6, "区域2.png");
            this.imageList1.Images.SetKeyName(7, "充电桩充电.png");
            this.imageList1.Images.SetKeyName(8, "充电桩待机.png");
            this.imageList1.Images.SetKeyName(9, "充电桩故障.png");
            this.imageList1.Images.SetKeyName(10, "充电桩离线.png");
            this.imageList1.Images.SetKeyName(11, "充电桩暂停.png");
            // 
            // node1
            // 
            this.node1.Expanded = true;
            this.node1.ImageIndex = 6;
            this.node1.Name = "node1";
            this.node1.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node5});
            this.node1.Text = "node1";
            // 
            // node5
            // 
            this.node5.Expanded = true;
            this.node5.ImageIndex = 4;
            this.node5.Name = "node5";
            this.node5.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node6});
            this.node5.Text = "区";
            // 
            // node6
            // 
            this.node6.ImageIndex = 7;
            this.node6.Name = "node6";
            this.node6.Text = "node";
            // 
            // node2
            // 
            this.node2.Expanded = true;
            this.node2.ImageIndex = 5;
            this.node2.Name = "node2";
            this.node2.Text = "node2";
            // 
            // node3
            // 
            this.node3.Expanded = true;
            this.node3.ImageIndex = 2;
            this.node3.Name = "node3";
            this.node3.Text = "node3";
            // 
            // node4
            // 
            this.node4.Expanded = true;
            this.node4.ImageIndex = 1;
            this.node4.Name = "node4";
            this.node4.Text = "node4";
            // 
            // nodeConnector1
            // 
            this.nodeConnector1.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle4
            // 
            this.elementStyle4.BackColor = System.Drawing.Color.White;
            this.elementStyle4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(228)))), ((int)(((byte)(240)))));
            this.elementStyle4.BackColorGradientAngle = 90;
            this.elementStyle4.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle4.BorderBottomWidth = 1;
            this.elementStyle4.BorderColor = System.Drawing.Color.DarkGray;
            this.elementStyle4.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle4.BorderLeftWidth = 1;
            this.elementStyle4.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle4.BorderRightWidth = 1;
            this.elementStyle4.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle4.BorderTopWidth = 1;
            this.elementStyle4.CornerDiameter = 4;
            this.elementStyle4.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle4.Description = "Gray";
            this.elementStyle4.Name = "elementStyle4";
            this.elementStyle4.PaddingBottom = 1;
            this.elementStyle4.PaddingLeft = 1;
            this.elementStyle4.PaddingRight = 1;
            this.elementStyle4.PaddingTop = 1;
            this.elementStyle4.TextColor = System.Drawing.Color.Black;
            // 
            // elementStyle1
            // 
            this.elementStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(230)))), ((int)(((byte)(247)))));
            this.elementStyle1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(168)))), ((int)(((byte)(228)))));
            this.elementStyle1.BackColorGradientAngle = 90;
            this.elementStyle1.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle1.BorderBottomWidth = 1;
            this.elementStyle1.BorderColor = System.Drawing.Color.DarkGray;
            this.elementStyle1.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle1.BorderLeftWidth = 1;
            this.elementStyle1.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle1.BorderRightWidth = 1;
            this.elementStyle1.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle1.BorderTopWidth = 1;
            this.elementStyle1.CornerDiameter = 4;
            this.elementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle1.Description = "Blue";
            this.elementStyle1.Name = "elementStyle1";
            this.elementStyle1.PaddingBottom = 1;
            this.elementStyle1.PaddingLeft = 1;
            this.elementStyle1.PaddingRight = 1;
            this.elementStyle1.PaddingTop = 1;
            this.elementStyle1.TextColor = System.Drawing.Color.Black;
            // 
            // expandablePanel1
            // 
            this.expandablePanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.expandablePanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.expandablePanel1.Controls.Add(this.btnSearchDeviceStat);
            this.expandablePanel1.Controls.Add(this.btnRealtimeStat);
            this.expandablePanel1.Controls.Add(this.buttonDelDevice);
            this.expandablePanel1.Controls.Add(this.btnAddDevice);
            this.expandablePanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.expandablePanel1.Location = new System.Drawing.Point(0, 59);
            this.expandablePanel1.Name = "expandablePanel1";
            this.expandablePanel1.Size = new System.Drawing.Size(185, 187);
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
            this.expandablePanel1.TitleText = "充电桩管理";
            // 
            // btnSearchDeviceStat
            // 
            this.btnSearchDeviceStat.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSearchDeviceStat.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSearchDeviceStat.Location = new System.Drawing.Point(29, 43);
            this.btnSearchDeviceStat.Name = "btnSearchDeviceStat";
            this.btnSearchDeviceStat.Size = new System.Drawing.Size(113, 28);
            this.btnSearchDeviceStat.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSearchDeviceStat.TabIndex = 1;
            this.btnSearchDeviceStat.Text = "充电桩查询";
            this.btnSearchDeviceStat.Click += new System.EventHandler(this.btnDeviceStat_Click);
            // 
            // btnRealtimeStat
            // 
            this.btnRealtimeStat.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnRealtimeStat.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnRealtimeStat.Location = new System.Drawing.Point(29, 145);
            this.btnRealtimeStat.Name = "btnRealtimeStat";
            this.btnRealtimeStat.Size = new System.Drawing.Size(113, 28);
            this.btnRealtimeStat.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnRealtimeStat.TabIndex = 1;
            this.btnRealtimeStat.Text = "充电桩实时状态";
            this.btnRealtimeStat.Click += new System.EventHandler(this.btnRealtimeStat_Click);
            // 
            // buttonDelDevice
            // 
            this.buttonDelDevice.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonDelDevice.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonDelDevice.Location = new System.Drawing.Point(29, 111);
            this.buttonDelDevice.Name = "buttonDelDevice";
            this.buttonDelDevice.Size = new System.Drawing.Size(113, 28);
            this.buttonDelDevice.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonDelDevice.TabIndex = 1;
            this.buttonDelDevice.Text = "充电桩注销";
            this.buttonDelDevice.Click += new System.EventHandler(this.btnDelDevice_Click);
            // 
            // btnAddDevice
            // 
            this.btnAddDevice.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAddDevice.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAddDevice.Location = new System.Drawing.Point(29, 77);
            this.btnAddDevice.Name = "btnAddDevice";
            this.btnAddDevice.Size = new System.Drawing.Size(113, 28);
            this.btnAddDevice.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnAddDevice.TabIndex = 1;
            this.btnAddDevice.Text = "充电桩注册";
            this.btnAddDevice.Click += new System.EventHandler(this.btnAddDevice_Click);
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
            this.superTabControl1.Controls.Add(this.superTabControlPanel0);
            this.superTabControl1.Controls.Add(this.superTabControlPanel10);
            this.superTabControl1.Controls.Add(this.superTabControlPanel9);
            this.superTabControl1.Controls.Add(this.superTabControlPanel2);
            this.superTabControl1.Controls.Add(this.superTabControlPanel1);
            this.superTabControl1.Controls.Add(this.superTabControlPanel4);
            this.superTabControl1.Controls.Add(this.superTabControlPanel3);
            this.superTabControl1.Controls.Add(this.superTabControlPanel8);
            this.superTabControl1.Controls.Add(this.superTabControlPanel7);
            this.superTabControl1.Controls.Add(this.superTabControlPanel6);
            this.superTabControl1.Controls.Add(this.superTabControlPanel5);
            this.superTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControl1.Location = new System.Drawing.Point(0, 0);
            this.superTabControl1.Name = "superTabControl1";
            this.superTabControl1.ReorderTabsEnabled = true;
            this.superTabControl1.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.superTabControl1.SelectedTabIndex = 0;
            this.superTabControl1.Size = new System.Drawing.Size(682, 507);
            this.superTabControl1.TabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.superTabControl1.TabIndex = 2;
            this.superTabControl1.TabLayoutType = DevComponents.DotNetBar.eSuperTabLayoutType.SingleLineFit;
            this.superTabControl1.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabItem0,
            this.superTabItem1,
            this.superTabItem2,
            this.superTabItem3,
            this.superTabItem4});
            this.superTabControl1.TabsVisible = false;
            this.superTabControl1.Text = "superTabControl1";
            // 
            // superTabControlPanel0
            // 
            this.superTabControlPanel0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel0.Location = new System.Drawing.Point(0, 26);
            this.superTabControlPanel0.Name = "superTabControlPanel0";
            this.superTabControlPanel0.Size = new System.Drawing.Size(682, 481);
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
            // superTabControlPanel10
            // 
            this.superTabControlPanel10.Controls.Add(this.ucCDZRuntimeStatPanel1);
            this.superTabControlPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel10.Location = new System.Drawing.Point(0, 0);
            this.superTabControlPanel10.Name = "superTabControlPanel10";
            this.superTabControlPanel10.Size = new System.Drawing.Size(682, 507);
            this.superTabControlPanel10.TabIndex = 2;
            this.superTabControlPanel10.TabItem = this.superTabItem4;
            // 
            // ucCDZRuntimeStatPanel1
            // 
            this.ucCDZRuntimeStatPanel1.AutoScroll = true;
            this.ucCDZRuntimeStatPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucCDZRuntimeStatPanel1.Location = new System.Drawing.Point(0, 0);
            this.ucCDZRuntimeStatPanel1.Name = "ucCDZRuntimeStatPanel1";
            this.ucCDZRuntimeStatPanel1.Size = new System.Drawing.Size(682, 507);
            this.ucCDZRuntimeStatPanel1.TabIndex = 0;
            // 
            // superTabItem4
            // 
            this.superTabItem4.AttachedControl = this.superTabControlPanel10;
            this.superTabItem4.GlobalItem = false;
            this.superTabItem4.Name = "superTabItem4";
            this.superTabItem4.Text = "superTabItem4";
            // 
            // superTabControlPanel9
            // 
            this.superTabControlPanel9.Controls.Add(this.ucDelCDZPanel1);
            this.superTabControlPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel9.Location = new System.Drawing.Point(0, 0);
            this.superTabControlPanel9.Name = "superTabControlPanel9";
            this.superTabControlPanel9.Size = new System.Drawing.Size(682, 507);
            this.superTabControlPanel9.TabIndex = 0;
            this.superTabControlPanel9.TabItem = this.superTabItem3;
            // 
            // ucDelCDZPanel1
            // 
            this.ucDelCDZPanel1.AutoScroll = true;
            this.ucDelCDZPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucDelCDZPanel1.Location = new System.Drawing.Point(0, 0);
            this.ucDelCDZPanel1.Name = "ucDelCDZPanel1";
            this.ucDelCDZPanel1.Size = new System.Drawing.Size(682, 507);
            this.ucDelCDZPanel1.TabIndex = 0;
            // 
            // superTabItem3
            // 
            this.superTabItem3.AttachedControl = this.superTabControlPanel9;
            this.superTabItem3.GlobalItem = false;
            this.superTabItem3.Name = "superTabItem3";
            this.superTabItem3.Text = "superTabItem3";
            // 
            // superTabControlPanel2
            // 
            this.superTabControlPanel2.Controls.Add(this.ucCDZStatPanel1);
            this.superTabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel2.Location = new System.Drawing.Point(0, 0);
            this.superTabControlPanel2.Name = "superTabControlPanel2";
            this.superTabControlPanel2.Size = new System.Drawing.Size(682, 507);
            this.superTabControlPanel2.TabIndex = 0;
            this.superTabControlPanel2.TabItem = this.superTabItem2;
            // 
            // ucCDZStatPanel1
            // 
            this.ucCDZStatPanel1.AutoScroll = true;
            this.ucCDZStatPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucCDZStatPanel1.Location = new System.Drawing.Point(0, 0);
            this.ucCDZStatPanel1.Name = "ucCDZStatPanel1";
            this.ucCDZStatPanel1.Size = new System.Drawing.Size(682, 507);
            this.ucCDZStatPanel1.TabIndex = 0;
            // 
            // superTabItem2
            // 
            this.superTabItem2.AttachedControl = this.superTabControlPanel2;
            this.superTabItem2.GlobalItem = false;
            this.superTabItem2.Name = "superTabItem2";
            this.superTabItem2.Text = "superTabItem2";
            // 
            // superTabControlPanel1
            // 
            this.superTabControlPanel1.Controls.Add(this.ucAddCDZPanel1);
            this.superTabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel1.Location = new System.Drawing.Point(0, 0);
            this.superTabControlPanel1.Name = "superTabControlPanel1";
            this.superTabControlPanel1.Size = new System.Drawing.Size(682, 507);
            this.superTabControlPanel1.TabIndex = 1;
            this.superTabControlPanel1.TabItem = this.superTabItem1;
            // 
            // ucAddCDZPanel1
            // 
            this.ucAddCDZPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucAddCDZPanel1.Location = new System.Drawing.Point(0, 0);
            this.ucAddCDZPanel1.Name = "ucAddCDZPanel1";
            this.ucAddCDZPanel1.Size = new System.Drawing.Size(682, 507);
            this.ucAddCDZPanel1.TabIndex = 0;
            // 
            // superTabItem1
            // 
            this.superTabItem1.AttachedControl = this.superTabControlPanel1;
            this.superTabItem1.GlobalItem = false;
            this.superTabItem1.Name = "superTabItem1";
            this.superTabItem1.Text = "superTabItem1";
            // 
            // superTabControlPanel4
            // 
            this.superTabControlPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel4.Location = new System.Drawing.Point(0, 0);
            this.superTabControlPanel4.Name = "superTabControlPanel4";
            this.superTabControlPanel4.Size = new System.Drawing.Size(682, 507);
            this.superTabControlPanel4.TabIndex = 0;
            // 
            // superTabControlPanel3
            // 
            this.superTabControlPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel3.Location = new System.Drawing.Point(0, 0);
            this.superTabControlPanel3.Name = "superTabControlPanel3";
            this.superTabControlPanel3.Size = new System.Drawing.Size(682, 507);
            this.superTabControlPanel3.TabIndex = 0;
            // 
            // superTabControlPanel8
            // 
            this.superTabControlPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel8.Location = new System.Drawing.Point(0, 0);
            this.superTabControlPanel8.Name = "superTabControlPanel8";
            this.superTabControlPanel8.Size = new System.Drawing.Size(682, 507);
            this.superTabControlPanel8.TabIndex = 0;
            // 
            // superTabControlPanel7
            // 
            this.superTabControlPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel7.Location = new System.Drawing.Point(0, 0);
            this.superTabControlPanel7.Name = "superTabControlPanel7";
            this.superTabControlPanel7.Size = new System.Drawing.Size(682, 507);
            this.superTabControlPanel7.TabIndex = 0;
            // 
            // superTabControlPanel6
            // 
            this.superTabControlPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel6.Location = new System.Drawing.Point(0, 0);
            this.superTabControlPanel6.Name = "superTabControlPanel6";
            this.superTabControlPanel6.Size = new System.Drawing.Size(682, 507);
            this.superTabControlPanel6.TabIndex = 0;
            // 
            // superTabControlPanel5
            // 
            this.superTabControlPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel5.Location = new System.Drawing.Point(0, 0);
            this.superTabControlPanel5.Name = "superTabControlPanel5";
            this.superTabControlPanel5.Size = new System.Drawing.Size(682, 507);
            this.superTabControlPanel5.TabIndex = 0;
            // 
            // ucDeviceManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ucDeviceManagement";
            this.Size = new System.Drawing.Size(871, 507);
            this.Load += new System.EventHandler(this.ucDeviceManagement_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.advTree1)).EndInit();
            this.expandablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).EndInit();
            this.superTabControl1.ResumeLayout(false);
            this.superTabControlPanel10.ResumeLayout(false);
            this.superTabControlPanel9.ResumeLayout(false);
            this.superTabControlPanel2.ResumeLayout(false);
            this.superTabControlPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private ucCurrentUser ucCurrentUser1;
        private DevComponents.DotNetBar.ExpandablePanel expandablePanel1;
        private DevComponents.DotNetBar.ButtonX btnAddDevice;
        private DevComponents.DotNetBar.SuperTabControl superTabControl1;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel1;
        private DevComponents.DotNetBar.SuperTabItem superTabItem1;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel2;
        private DevComponents.DotNetBar.SuperTabItem superTabItem2;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel4;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel3;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel8;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel7;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel6;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel5;
        private ucAddCDZPanel ucAddCDZPanel1;
        private ucCDZStatPanel ucCDZStatPanel1;
        private DevComponents.DotNetBar.ButtonX btnSearchDeviceStat;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel0;
        private DevComponents.DotNetBar.SuperTabItem superTabItem0;
        private DevComponents.AdvTree.AdvTree advTree1;
        private DevComponents.AdvTree.ColumnHeader columnHeader1;
        private DevComponents.AdvTree.Node node1;
        private DevComponents.AdvTree.Node node2;
        private DevComponents.AdvTree.Node node3;
        private DevComponents.AdvTree.Node node4;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle4;
        private System.Windows.Forms.ImageList imageList1;
        private DevComponents.AdvTree.Node node5;
        private DevComponents.AdvTree.Node node6;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.DotNetBar.ButtonX buttonDelDevice;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel9;
        private DevComponents.DotNetBar.SuperTabItem superTabItem3;
        private ucDelCDZPanel ucDelCDZPanel1;
        private DevComponents.DotNetBar.ButtonX btnRealtimeStat;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel10;
        private ucCDZRuntimeStatPanel ucCDZRuntimeStatPanel1;
        private DevComponents.DotNetBar.SuperTabItem superTabItem4;
    }
}
