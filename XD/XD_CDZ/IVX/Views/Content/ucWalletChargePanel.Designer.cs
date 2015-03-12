namespace BOCOM.IVX.Views.Content
{
    partial class ucWalletChargePanel
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
            this.labelRet = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.btnAddWallet = new DevComponents.DotNetBar.ButtonX();
            this.textBoxCardID = new DevComponents.DotNetBar.Controls.MaskedTextBoxAdv();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.checkBoxFrozen = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.textBoxMoney = new DevComponents.Editors.DoubleInput();
            this.textBoxWalletMoney = new DevComponents.Editors.DoubleInput();
            this.textBoxCardSerialNumber = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX11 = new DevComponents.DotNetBar.LabelX();
            this.labelX14 = new DevComponents.DotNetBar.LabelX();
            this.textBoxUserName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX13 = new DevComponents.DotNetBar.LabelX();
            this.labelX12 = new DevComponents.DotNetBar.LabelX();
            this.btnSubWallet = new DevComponents.DotNetBar.ButtonX();
            this.buttonUnfrozen = new DevComponents.DotNetBar.ButtonX();
            this.textBoxChargeMoney = new DevComponents.Editors.DoubleInput();
            this.btnReadCardNumber = new DevComponents.DotNetBar.ButtonX();
            this.ucCardIDTextBox1 = new BOCOM.IVX.Controls.ucCardIDTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxMoney)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxWalletMoney)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxChargeMoney)).BeginInit();
            this.SuspendLayout();
            // 
            // labelRet
            // 
            // 
            // 
            // 
            this.labelRet.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelRet.Location = new System.Drawing.Point(354, 235);
            this.labelRet.Name = "labelRet";
            this.labelRet.Size = new System.Drawing.Size(220, 23);
            this.labelRet.TabIndex = 1;
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
            this.labelX5.Text = "圈钱&&充正：";
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(80, 28);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "用户卡号：";
            // 
            // btnAddWallet
            // 
            this.btnAddWallet.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAddWallet.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAddWallet.Location = new System.Drawing.Point(181, 235);
            this.btnAddWallet.Name = "btnAddWallet";
            this.btnAddWallet.Size = new System.Drawing.Size(75, 23);
            this.btnAddWallet.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnAddWallet.TabIndex = 0;
            this.btnAddWallet.Text = "圈钱";
            this.btnAddWallet.Click += new System.EventHandler(this.btnAddWallet_Click);
            // 
            // textBoxCardID
            // 
            // 
            // 
            // 
            this.textBoxCardID.BackgroundStyle.Class = "TextBoxBorder";
            this.textBoxCardID.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxCardID.BeepOnError = true;
            this.textBoxCardID.ButtonClear.Visible = true;
            this.textBoxCardID.Location = new System.Drawing.Point(181, 26);
            this.textBoxCardID.Mask = "0000 0000 0000 0000";
            this.textBoxCardID.Name = "textBoxCardID";
            this.textBoxCardID.PromptChar = ' ';
            this.textBoxCardID.Size = new System.Drawing.Size(184, 25);
            this.textBoxCardID.SkipLiterals = false;
            this.textBoxCardID.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.textBoxCardID.TabIndex = 26;
            this.textBoxCardID.Text = "";
            this.textBoxCardID.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.textBoxCardID.WatermarkEnabled = false;
            this.textBoxCardID.TextChanged += new System.EventHandler(this.textBoxCardID_TextChanged);
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(80, 53);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 23);
            this.labelX3.TabIndex = 1;
            this.labelX3.Text = "用户姓名：";
            // 
            // labelX9
            // 
            // 
            // 
            // 
            this.labelX9.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX9.Location = new System.Drawing.Point(80, 188);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(75, 23);
            this.labelX9.TabIndex = 1;
            this.labelX9.Text = "转账金额：";
            // 
            // checkBoxFrozen
            // 
            // 
            // 
            // 
            this.checkBoxFrozen.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBoxFrozen.Enabled = false;
            this.checkBoxFrozen.Location = new System.Drawing.Point(181, 163);
            this.checkBoxFrozen.Name = "checkBoxFrozen";
            this.checkBoxFrozen.Size = new System.Drawing.Size(19, 23);
            this.checkBoxFrozen.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.checkBoxFrozen.TabIndex = 38;
            this.checkBoxFrozen.CheckedChanged += new System.EventHandler(this.checkBoxFrozen_CheckedChanged);
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
            this.textBoxMoney.Location = new System.Drawing.Point(181, 111);
            this.textBoxMoney.Name = "textBoxMoney";
            this.textBoxMoney.ShowUpDown = true;
            this.textBoxMoney.Size = new System.Drawing.Size(100, 21);
            this.textBoxMoney.TabIndex = 36;
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
            this.textBoxWalletMoney.Location = new System.Drawing.Point(181, 138);
            this.textBoxWalletMoney.Name = "textBoxWalletMoney";
            this.textBoxWalletMoney.ShowUpDown = true;
            this.textBoxWalletMoney.Size = new System.Drawing.Size(100, 21);
            this.textBoxWalletMoney.TabIndex = 37;
            // 
            // textBoxCardSerialNumber
            // 
            // 
            // 
            // 
            this.textBoxCardSerialNumber.Border.Class = "TextBoxBorder";
            this.textBoxCardSerialNumber.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxCardSerialNumber.Location = new System.Drawing.Point(181, 84);
            this.textBoxCardSerialNumber.Name = "textBoxCardSerialNumber";
            this.textBoxCardSerialNumber.ReadOnly = true;
            this.textBoxCardSerialNumber.Size = new System.Drawing.Size(152, 21);
            this.textBoxCardSerialNumber.TabIndex = 33;
            // 
            // labelX11
            // 
            this.labelX11.AutoSize = true;
            // 
            // 
            // 
            this.labelX11.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX11.Location = new System.Drawing.Point(87, 85);
            this.labelX11.Name = "labelX11";
            this.labelX11.Size = new System.Drawing.Size(68, 18);
            this.labelX11.TabIndex = 29;
            this.labelX11.Text = "物理卡号：";
            // 
            // labelX14
            // 
            this.labelX14.AutoSize = true;
            // 
            // 
            // 
            this.labelX14.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX14.Location = new System.Drawing.Point(87, 168);
            this.labelX14.Name = "labelX14";
            this.labelX14.Size = new System.Drawing.Size(68, 18);
            this.labelX14.TabIndex = 30;
            this.labelX14.Text = "是否冻结：";
            // 
            // textBoxUserName
            // 
            // 
            // 
            // 
            this.textBoxUserName.Border.Class = "TextBoxBorder";
            this.textBoxUserName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxUserName.Location = new System.Drawing.Point(181, 57);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.ReadOnly = true;
            this.textBoxUserName.Size = new System.Drawing.Size(152, 21);
            this.textBoxUserName.TabIndex = 35;
            // 
            // labelX13
            // 
            this.labelX13.AutoSize = true;
            // 
            // 
            // 
            this.labelX13.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX13.Location = new System.Drawing.Point(87, 141);
            this.labelX13.Name = "labelX13";
            this.labelX13.Size = new System.Drawing.Size(68, 18);
            this.labelX13.TabIndex = 31;
            this.labelX13.Text = "钱包余额：";
            // 
            // labelX12
            // 
            this.labelX12.AutoSize = true;
            // 
            // 
            // 
            this.labelX12.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX12.Location = new System.Drawing.Point(87, 114);
            this.labelX12.Name = "labelX12";
            this.labelX12.Size = new System.Drawing.Size(68, 18);
            this.labelX12.TabIndex = 32;
            this.labelX12.Text = "账户余额：";
            // 
            // btnSubWallet
            // 
            this.btnSubWallet.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSubWallet.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSubWallet.Location = new System.Drawing.Point(262, 235);
            this.btnSubWallet.Name = "btnSubWallet";
            this.btnSubWallet.Size = new System.Drawing.Size(75, 23);
            this.btnSubWallet.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSubWallet.TabIndex = 0;
            this.btnSubWallet.Text = "充正";
            this.btnSubWallet.Click += new System.EventHandler(this.btnSubWallet_Click);
            // 
            // buttonUnfrozen
            // 
            this.buttonUnfrozen.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonUnfrozen.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonUnfrozen.Location = new System.Drawing.Point(212, 163);
            this.buttonUnfrozen.Name = "buttonUnfrozen";
            this.buttonUnfrozen.Size = new System.Drawing.Size(75, 23);
            this.buttonUnfrozen.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonUnfrozen.TabIndex = 39;
            this.buttonUnfrozen.Text = "解冻";
            this.buttonUnfrozen.Click += new System.EventHandler(this.buttonUnfrozen_Click);
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
            this.textBoxChargeMoney.Location = new System.Drawing.Point(181, 192);
            this.textBoxChargeMoney.Name = "textBoxChargeMoney";
            this.textBoxChargeMoney.ShowUpDown = true;
            this.textBoxChargeMoney.Size = new System.Drawing.Size(80, 21);
            this.textBoxChargeMoney.TabIndex = 40;
            this.textBoxChargeMoney.Value = 0.0001D;
            // 
            // btnReadCardNumber
            // 
            this.btnReadCardNumber.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnReadCardNumber.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnReadCardNumber.Location = new System.Drawing.Point(371, 26);
            this.btnReadCardNumber.Name = "btnReadCardNumber";
            this.btnReadCardNumber.Size = new System.Drawing.Size(75, 23);
            this.btnReadCardNumber.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnReadCardNumber.TabIndex = 41;
            this.btnReadCardNumber.Text = "读卡";
            this.btnReadCardNumber.Click += new System.EventHandler(this.btnReadCardNumber_Click);
            // 
            // ucCardIDTextBox1
            // 
            // 
            // 
            // 
            this.ucCardIDTextBox1.Border.Class = "TextBoxBorder";
            this.ucCardIDTextBox1.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ucCardIDTextBox1.Location = new System.Drawing.Point(380, 116);
            this.ucCardIDTextBox1.MaxLength = 19;
            this.ucCardIDTextBox1.Name = "ucCardIDTextBox1";
            this.ucCardIDTextBox1.Size = new System.Drawing.Size(170, 21);
            this.ucCardIDTextBox1.TabIndex = 42;
            this.ucCardIDTextBox1.Value = "";
            this.ucCardIDTextBox1.TextChanged += new System.EventHandler(this.ucCardIDTextBox1_TextChanged);
            // 
            // ucWalletChargePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ucCardIDTextBox1);
            this.Controls.Add(this.btnReadCardNumber);
            this.Controls.Add(this.textBoxChargeMoney);
            this.Controls.Add(this.buttonUnfrozen);
            this.Controls.Add(this.checkBoxFrozen);
            this.Controls.Add(this.textBoxMoney);
            this.Controls.Add(this.textBoxWalletMoney);
            this.Controls.Add(this.textBoxCardSerialNumber);
            this.Controls.Add(this.labelX11);
            this.Controls.Add(this.labelX14);
            this.Controls.Add(this.textBoxUserName);
            this.Controls.Add(this.labelX13);
            this.Controls.Add(this.labelX12);
            this.Controls.Add(this.textBoxCardID);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.labelRet);
            this.Controls.Add(this.btnSubWallet);
            this.Controls.Add(this.btnAddWallet);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.labelX9);
            this.Controls.Add(this.labelX3);
            this.Name = "ucWalletChargePanel";
            this.Size = new System.Drawing.Size(764, 340);
            this.Load += new System.EventHandler(this.ucWalletChargePanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textBoxMoney)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxWalletMoney)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxChargeMoney)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelRet;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX btnAddWallet;
        private DevComponents.DotNetBar.Controls.MaskedTextBoxAdv textBoxCardID;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX9;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxFrozen;
        private DevComponents.Editors.DoubleInput textBoxMoney;
        private DevComponents.Editors.DoubleInput textBoxWalletMoney;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxCardSerialNumber;
        private DevComponents.DotNetBar.LabelX labelX11;
        private DevComponents.DotNetBar.LabelX labelX14;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxUserName;
        private DevComponents.DotNetBar.LabelX labelX13;
        private DevComponents.DotNetBar.LabelX labelX12;
        private DevComponents.DotNetBar.ButtonX btnSubWallet;
        private DevComponents.DotNetBar.ButtonX buttonUnfrozen;
        private DevComponents.Editors.DoubleInput textBoxChargeMoney;
        private DevComponents.DotNetBar.ButtonX btnReadCardNumber;
        private Controls.ucCardIDTextBox ucCardIDTextBox1;
    }
}
