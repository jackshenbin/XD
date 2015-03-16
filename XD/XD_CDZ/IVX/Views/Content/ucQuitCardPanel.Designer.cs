namespace BOCOM.IVX.Views.Content
{
    partial class ucQuitCardPanel
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
            this.textBoxUserName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.buttonQuitCard = new DevComponents.DotNetBar.ButtonX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX11 = new DevComponents.DotNetBar.LabelX();
            this.textBoxCardSerialNumber = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnReadCardNumber = new DevComponents.DotNetBar.ButtonX();
            this.labelX12 = new DevComponents.DotNetBar.LabelX();
            this.labelX13 = new DevComponents.DotNetBar.LabelX();
            this.labelX14 = new DevComponents.DotNetBar.LabelX();
            this.labelX15 = new DevComponents.DotNetBar.LabelX();
            this.textBoxCreatDate = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.textBoxMoney = new DevComponents.Editors.DoubleInput();
            this.textBoxWalletMoney = new DevComponents.Editors.DoubleInput();
            this.checkBoxFrozen = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.textBoxCardID = new BOCOM.IVX.Controls.ucCardIDTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxMoney)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxWalletMoney)).BeginInit();
            this.SuspendLayout();
            // 
            // labelRet
            // 
            this.labelRet.AutoSize = true;
            // 
            // 
            // 
            this.labelRet.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelRet.Location = new System.Drawing.Point(271, 239);
            this.labelRet.Name = "labelRet";
            this.labelRet.Size = new System.Drawing.Size(193, 23);
            this.labelRet.TabIndex = 1;
            // 
            // textBoxUserName
            // 
            // 
            // 
            // 
            this.textBoxUserName.Border.Class = "TextBoxBorder";
            this.textBoxUserName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxUserName.Location = new System.Drawing.Point(178, 68);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.ReadOnly = true;
            this.textBoxUserName.Size = new System.Drawing.Size(152, 21);
            this.textBoxUserName.TabIndex = 2;
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
            this.labelX5.Size = new System.Drawing.Size(96, 23);
            this.labelX5.TabIndex = 1;
            this.labelX5.Text = "注销卡片：";
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(108, 68);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(44, 18);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "姓名：";
            // 
            // buttonQuitCard
            // 
            this.buttonQuitCard.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonQuitCard.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonQuitCard.Location = new System.Drawing.Point(178, 239);
            this.buttonQuitCard.Name = "buttonQuitCard";
            this.buttonQuitCard.Size = new System.Drawing.Size(75, 23);
            this.buttonQuitCard.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonQuitCard.TabIndex = 0;
            this.buttonQuitCard.Text = "注销";
            this.buttonQuitCard.Click += new System.EventHandler(this.buttonQuitCard_Click);
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(84, 39);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(68, 18);
            this.labelX4.TabIndex = 1;
            this.labelX4.Text = "用户卡号：";
            // 
            // labelX11
            // 
            this.labelX11.AutoSize = true;
            // 
            // 
            // 
            this.labelX11.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX11.Location = new System.Drawing.Point(84, 96);
            this.labelX11.Name = "labelX11";
            this.labelX11.Size = new System.Drawing.Size(68, 18);
            this.labelX11.TabIndex = 1;
            this.labelX11.Text = "物理卡号：";
            // 
            // textBoxCardSerialNumber
            // 
            // 
            // 
            // 
            this.textBoxCardSerialNumber.Border.Class = "TextBoxBorder";
            this.textBoxCardSerialNumber.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxCardSerialNumber.Location = new System.Drawing.Point(178, 95);
            this.textBoxCardSerialNumber.Name = "textBoxCardSerialNumber";
            this.textBoxCardSerialNumber.ReadOnly = true;
            this.textBoxCardSerialNumber.Size = new System.Drawing.Size(152, 21);
            this.textBoxCardSerialNumber.TabIndex = 2;
            // 
            // btnReadCardNumber
            // 
            this.btnReadCardNumber.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnReadCardNumber.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnReadCardNumber.Location = new System.Drawing.Point(368, 39);
            this.btnReadCardNumber.Name = "btnReadCardNumber";
            this.btnReadCardNumber.Size = new System.Drawing.Size(75, 23);
            this.btnReadCardNumber.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnReadCardNumber.TabIndex = 0;
            this.btnReadCardNumber.Text = "读卡";
            this.btnReadCardNumber.Click += new System.EventHandler(this.btnReadCardNumber_Click);
            // 
            // labelX12
            // 
            this.labelX12.AutoSize = true;
            // 
            // 
            // 
            this.labelX12.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX12.Location = new System.Drawing.Point(84, 125);
            this.labelX12.Name = "labelX12";
            this.labelX12.Size = new System.Drawing.Size(68, 18);
            this.labelX12.TabIndex = 1;
            this.labelX12.Text = "账户余额：";
            // 
            // labelX13
            // 
            this.labelX13.AutoSize = true;
            // 
            // 
            // 
            this.labelX13.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX13.Location = new System.Drawing.Point(84, 152);
            this.labelX13.Name = "labelX13";
            this.labelX13.Size = new System.Drawing.Size(68, 18);
            this.labelX13.TabIndex = 1;
            this.labelX13.Text = "钱包余额：";
            // 
            // labelX14
            // 
            this.labelX14.AutoSize = true;
            // 
            // 
            // 
            this.labelX14.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX14.Location = new System.Drawing.Point(84, 179);
            this.labelX14.Name = "labelX14";
            this.labelX14.Size = new System.Drawing.Size(68, 18);
            this.labelX14.TabIndex = 1;
            this.labelX14.Text = "是否冻结：";
            // 
            // labelX15
            // 
            this.labelX15.AutoSize = true;
            // 
            // 
            // 
            this.labelX15.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX15.Location = new System.Drawing.Point(84, 206);
            this.labelX15.Name = "labelX15";
            this.labelX15.Size = new System.Drawing.Size(68, 18);
            this.labelX15.TabIndex = 1;
            this.labelX15.Text = "建卡日期：";
            // 
            // textBoxCreatDate
            // 
            // 
            // 
            // 
            this.textBoxCreatDate.Border.Class = "TextBoxBorder";
            this.textBoxCreatDate.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxCreatDate.Location = new System.Drawing.Point(178, 203);
            this.textBoxCreatDate.Name = "textBoxCreatDate";
            this.textBoxCreatDate.ReadOnly = true;
            this.textBoxCreatDate.Size = new System.Drawing.Size(152, 21);
            this.textBoxCreatDate.TabIndex = 2;
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
            this.textBoxMoney.Location = new System.Drawing.Point(178, 122);
            this.textBoxMoney.Name = "textBoxMoney";
            this.textBoxMoney.ShowUpDown = true;
            this.textBoxMoney.Size = new System.Drawing.Size(100, 21);
            this.textBoxMoney.TabIndex = 7;
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
            this.textBoxWalletMoney.Location = new System.Drawing.Point(178, 149);
            this.textBoxWalletMoney.Name = "textBoxWalletMoney";
            this.textBoxWalletMoney.ShowUpDown = true;
            this.textBoxWalletMoney.Size = new System.Drawing.Size(100, 21);
            this.textBoxWalletMoney.TabIndex = 8;
            // 
            // checkBoxFrozen
            // 
            // 
            // 
            // 
            this.checkBoxFrozen.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBoxFrozen.Enabled = false;
            this.checkBoxFrozen.Location = new System.Drawing.Point(178, 174);
            this.checkBoxFrozen.Name = "checkBoxFrozen";
            this.checkBoxFrozen.Size = new System.Drawing.Size(19, 23);
            this.checkBoxFrozen.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.checkBoxFrozen.TabIndex = 10;
            // 
            // textBoxCardID
            // 
            // 
            // 
            // 
            this.textBoxCardID.Border.Class = "TextBoxBorder";
            this.textBoxCardID.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxCardID.Location = new System.Drawing.Point(178, 37);
            this.textBoxCardID.MaxLength = 19;
            this.textBoxCardID.Name = "textBoxCardID";
            this.textBoxCardID.Size = new System.Drawing.Size(184, 21);
            this.textBoxCardID.TabIndex = 25;
            this.textBoxCardID.Value = "";
            this.textBoxCardID.WatermarkEnabled = false;
            this.textBoxCardID.TextChanged += new System.EventHandler(this.textBoxCardID_TextChanged);
            // 
            // ucQuitCardPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxCardID);
            this.Controls.Add(this.checkBoxFrozen);
            this.Controls.Add(this.textBoxMoney);
            this.Controls.Add(this.textBoxWalletMoney);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.labelRet);
            this.Controls.Add(this.btnReadCardNumber);
            this.Controls.Add(this.buttonQuitCard);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.textBoxCardSerialNumber);
            this.Controls.Add(this.textBoxCreatDate);
            this.Controls.Add(this.labelX15);
            this.Controls.Add(this.labelX11);
            this.Controls.Add(this.labelX14);
            this.Controls.Add(this.textBoxUserName);
            this.Controls.Add(this.labelX13);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.labelX12);
            this.Name = "ucQuitCardPanel";
            this.Size = new System.Drawing.Size(647, 320);
            ((System.ComponentModel.ISupportInitialize)(this.textBoxMoney)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxWalletMoney)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelRet;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxUserName;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX buttonQuitCard;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX11;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxCardSerialNumber;
        private DevComponents.DotNetBar.ButtonX btnReadCardNumber;
        private DevComponents.DotNetBar.LabelX labelX12;
        private DevComponents.DotNetBar.LabelX labelX13;
        private DevComponents.DotNetBar.LabelX labelX14;
        private DevComponents.DotNetBar.LabelX labelX15;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxCreatDate;
        private DevComponents.Editors.DoubleInput textBoxMoney;
        private DevComponents.Editors.DoubleInput textBoxWalletMoney;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxFrozen;
        private BOCOM.IVX.Controls.ucCardIDTextBox textBoxCardID;
    }
}
