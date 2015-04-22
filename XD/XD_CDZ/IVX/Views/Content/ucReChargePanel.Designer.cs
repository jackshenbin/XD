namespace BOCOM.IVX.Views.Content
{
    partial class ucReChargePanel
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
            this.comboBoxMoneyType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.labelRet = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.textBoxUserName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.textBoxCardSerialNumber = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.buttonAddMoney = new DevComponents.DotNetBar.ButtonX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.checkBoxFrozen = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.btnReadCardNumber = new DevComponents.DotNetBar.ButtonX();
            this.textBoxWalletMoney = new DevComponents.Editors.DoubleInput();
            this.textBoxMoney = new DevComponents.Editors.DoubleInput();
            this.textBoxChargeMoney = new DevComponents.Editors.DoubleInput();
            this.textBoxCardID = new BOCOM.IVX.Controls.ucCardIDTextBox();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxWalletMoney)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxMoney)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxChargeMoney)).BeginInit();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxMoneyType
            // 
            this.comboBoxMoneyType.DisplayMember = "Text";
            this.comboBoxMoneyType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxMoneyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMoneyType.FormattingEnabled = true;
            this.comboBoxMoneyType.ItemHeight = 15;
            this.comboBoxMoneyType.Items.AddRange(new object[] {
            this.comboItem1});
            this.comboBoxMoneyType.Location = new System.Drawing.Point(178, 214);
            this.comboBoxMoneyType.Name = "comboBoxMoneyType";
            this.comboBoxMoneyType.Size = new System.Drawing.Size(80, 21);
            this.comboBoxMoneyType.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.comboBoxMoneyType.TabIndex = 3;
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "现金";
            // 
            // labelRet
            // 
            this.labelRet.AutoSize = true;
            // 
            // 
            // 
            this.labelRet.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelRet.Location = new System.Drawing.Point(259, 249);
            this.labelRet.Name = "labelRet";
            this.labelRet.Size = new System.Drawing.Size(0, 0);
            this.labelRet.TabIndex = 1;
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(128, 217);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(44, 18);
            this.labelX4.TabIndex = 1;
            this.labelX4.Text = "方式：";
            // 
            // textBoxUserName
            // 
            // 
            // 
            // 
            this.textBoxUserName.Border.Class = "TextBoxBorder";
            this.textBoxUserName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxUserName.Location = new System.Drawing.Point(178, 79);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.ReadOnly = true;
            this.textBoxUserName.Size = new System.Drawing.Size(140, 21);
            this.textBoxUserName.TabIndex = 2;
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(104, 82);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(68, 18);
            this.labelX3.TabIndex = 1;
            this.labelX3.Text = "用户姓名：";
            // 
            // textBoxCardSerialNumber
            // 
            // 
            // 
            // 
            this.textBoxCardSerialNumber.Border.Class = "TextBoxBorder";
            this.textBoxCardSerialNumber.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxCardSerialNumber.Location = new System.Drawing.Point(178, 52);
            this.textBoxCardSerialNumber.Name = "textBoxCardSerialNumber";
            this.textBoxCardSerialNumber.ReadOnly = true;
            this.textBoxCardSerialNumber.Size = new System.Drawing.Size(140, 21);
            this.textBoxCardSerialNumber.TabIndex = 2;
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(104, 55);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(68, 18);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "物理卡号：";
            // 
            // labelX5
            // 
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX5.Location = new System.Drawing.Point(0, 3);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(121, 23);
            this.labelX5.TabIndex = 1;
            this.labelX5.Text = "账户充值：";
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(104, 28);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(68, 18);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "用户卡号：";
            // 
            // buttonAddMoney
            // 
            this.buttonAddMoney.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonAddMoney.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonAddMoney.Location = new System.Drawing.Point(178, 249);
            this.buttonAddMoney.Name = "buttonAddMoney";
            this.buttonAddMoney.Size = new System.Drawing.Size(75, 23);
            this.buttonAddMoney.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonAddMoney.TabIndex = 0;
            this.buttonAddMoney.Text = "充值";
            this.buttonAddMoney.Click += new System.EventHandler(this.buttonAddMoney_Click);
            // 
            // labelX6
            // 
            this.labelX6.AutoSize = true;
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.Location = new System.Drawing.Point(104, 109);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(68, 18);
            this.labelX6.TabIndex = 1;
            this.labelX6.Text = "账户余额：";
            // 
            // labelX7
            // 
            this.labelX7.AutoSize = true;
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Location = new System.Drawing.Point(104, 163);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(68, 18);
            this.labelX7.TabIndex = 1;
            this.labelX7.Text = "是否冻结：";
            // 
            // labelX8
            // 
            this.labelX8.AutoSize = true;
            // 
            // 
            // 
            this.labelX8.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX8.Location = new System.Drawing.Point(104, 136);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(68, 18);
            this.labelX8.TabIndex = 1;
            this.labelX8.Text = "钱包余额：";
            // 
            // labelX9
            // 
            this.labelX9.AutoSize = true;
            // 
            // 
            // 
            this.labelX9.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX9.Location = new System.Drawing.Point(104, 190);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(68, 18);
            this.labelX9.TabIndex = 1;
            this.labelX9.Text = "充值金额：";
            // 
            // checkBoxFrozen
            // 
            // 
            // 
            // 
            this.checkBoxFrozen.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBoxFrozen.Enabled = false;
            this.checkBoxFrozen.Location = new System.Drawing.Point(178, 158);
            this.checkBoxFrozen.Name = "checkBoxFrozen";
            this.checkBoxFrozen.Size = new System.Drawing.Size(19, 23);
            this.checkBoxFrozen.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.checkBoxFrozen.TabIndex = 4;
            this.checkBoxFrozen.CheckedChanged += new System.EventHandler(this.checkBoxFrozen_CheckedChanged);
            // 
            // btnReadCardNumber
            // 
            this.btnReadCardNumber.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnReadCardNumber.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnReadCardNumber.Location = new System.Drawing.Point(368, 23);
            this.btnReadCardNumber.Name = "btnReadCardNumber";
            this.btnReadCardNumber.Size = new System.Drawing.Size(75, 23);
            this.btnReadCardNumber.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnReadCardNumber.TabIndex = 5;
            this.btnReadCardNumber.Text = "读卡";
            this.btnReadCardNumber.Click += new System.EventHandler(this.btnReadCardNumber_Click);
            // 
            // textBoxWalletMoney
            // 
            // 
            // 
            // 
            this.textBoxWalletMoney.BackgroundStyle.Class = "DateTimeInputBackground";
            this.textBoxWalletMoney.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxWalletMoney.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.textBoxWalletMoney.DisplayFormat = "0.##";
            this.textBoxWalletMoney.Enabled = false;
            this.textBoxWalletMoney.Increment = 1D;
            this.textBoxWalletMoney.IsInputReadOnly = true;
            this.textBoxWalletMoney.Location = new System.Drawing.Point(178, 133);
            this.textBoxWalletMoney.Name = "textBoxWalletMoney";
            this.textBoxWalletMoney.ShowUpDown = true;
            this.textBoxWalletMoney.Size = new System.Drawing.Size(80, 21);
            this.textBoxWalletMoney.TabIndex = 6;
            // 
            // textBoxMoney
            // 
            // 
            // 
            // 
            this.textBoxMoney.BackgroundStyle.Class = "DateTimeInputBackground";
            this.textBoxMoney.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxMoney.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.textBoxMoney.DisplayFormat = "0.##";
            this.textBoxMoney.Enabled = false;
            this.textBoxMoney.Increment = 1D;
            this.textBoxMoney.IsInputReadOnly = true;
            this.textBoxMoney.Location = new System.Drawing.Point(178, 106);
            this.textBoxMoney.Name = "textBoxMoney";
            this.textBoxMoney.ShowUpDown = true;
            this.textBoxMoney.Size = new System.Drawing.Size(80, 21);
            this.textBoxMoney.TabIndex = 6;
            // 
            // textBoxChargeMoney
            // 
            // 
            // 
            // 
            this.textBoxChargeMoney.BackgroundStyle.Class = "DateTimeInputBackground";
            this.textBoxChargeMoney.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxChargeMoney.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.textBoxChargeMoney.DisplayFormat = "0.##";
            this.textBoxChargeMoney.Increment = 1D;
            this.textBoxChargeMoney.Location = new System.Drawing.Point(178, 187);
            this.textBoxChargeMoney.Name = "textBoxChargeMoney";
            this.textBoxChargeMoney.ShowUpDown = true;
            this.textBoxChargeMoney.Size = new System.Drawing.Size(80, 21);
            this.textBoxChargeMoney.TabIndex = 6;
            this.textBoxChargeMoney.Value = 0.0001D;
            // 
            // textBoxCardID
            // 
            // 
            // 
            // 
            this.textBoxCardID.Border.Class = "TextBoxBorder";
            this.textBoxCardID.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxCardID.Location = new System.Drawing.Point(178, 23);
            this.textBoxCardID.MaxLength = 19;
            this.textBoxCardID.Name = "textBoxCardID";
            this.textBoxCardID.Size = new System.Drawing.Size(184, 21);
            this.textBoxCardID.TabIndex = 24;
            this.textBoxCardID.Value = "";
            this.textBoxCardID.WatermarkEnabled = false;
            this.textBoxCardID.TextChanged += new System.EventHandler(this.textBoxCardID_TextChanged);
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.textBoxCardID);
            this.panelEx1.Controls.Add(this.textBoxChargeMoney);
            this.panelEx1.Controls.Add(this.textBoxMoney);
            this.panelEx1.Controls.Add(this.textBoxWalletMoney);
            this.panelEx1.Controls.Add(this.btnReadCardNumber);
            this.panelEx1.Controls.Add(this.checkBoxFrozen);
            this.panelEx1.Controls.Add(this.comboBoxMoneyType);
            this.panelEx1.Controls.Add(this.labelX5);
            this.panelEx1.Controls.Add(this.labelRet);
            this.panelEx1.Controls.Add(this.buttonAddMoney);
            this.panelEx1.Controls.Add(this.labelX4);
            this.panelEx1.Controls.Add(this.labelX8);
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Controls.Add(this.textBoxUserName);
            this.panelEx1.Controls.Add(this.labelX9);
            this.panelEx1.Controls.Add(this.labelX7);
            this.panelEx1.Controls.Add(this.labelX6);
            this.panelEx1.Controls.Add(this.labelX3);
            this.panelEx1.Controls.Add(this.labelX2);
            this.panelEx1.Controls.Add(this.textBoxCardSerialNumber);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(607, 340);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 25;
            // 
            // ucReChargePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelEx1);
            this.Name = "ucReChargePanel";
            this.Size = new System.Drawing.Size(607, 340);
            this.Load += new System.EventHandler(this.ucChargePanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textBoxWalletMoney)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxMoney)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxChargeMoney)).EndInit();
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxMoneyType;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.DotNetBar.LabelX labelRet;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxUserName;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxCardSerialNumber;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX buttonAddMoney;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.LabelX labelX9;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxFrozen;
        private DevComponents.DotNetBar.ButtonX btnReadCardNumber;
        private DevComponents.Editors.DoubleInput textBoxWalletMoney;
        private DevComponents.Editors.DoubleInput textBoxMoney;
        private DevComponents.Editors.DoubleInput textBoxChargeMoney;
        private BOCOM.IVX.Controls.ucCardIDTextBox textBoxCardID;
        private DevComponents.DotNetBar.PanelEx panelEx1;
    }
}
