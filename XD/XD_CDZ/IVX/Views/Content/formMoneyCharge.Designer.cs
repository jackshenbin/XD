namespace BOCOM.IVX.Views.Content
{
    partial class formMoneyCharge
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
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.textBoxMoney = new DevComponents.Editors.DoubleInput();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxMoney)).BeginInit();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX1.Location = new System.Drawing.Point(21, 28);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 38);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "金额";
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonX1.Location = new System.Drawing.Point(259, 28);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(75, 38);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 1;
            this.buttonX1.Text = "确认";
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
            this.textBoxMoney.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxMoney.Increment = 1D;
            this.textBoxMoney.IsInputReadOnly = true;
            this.textBoxMoney.Location = new System.Drawing.Point(102, 28);
            this.textBoxMoney.Name = "textBoxMoney";
            this.textBoxMoney.ShowUpDown = true;
            this.textBoxMoney.Size = new System.Drawing.Size(134, 38);
            this.textBoxMoney.TabIndex = 7;
            // 
            // formMoneyCharge
            // 
            this.AcceptButton = this.buttonX1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 98);
            this.Controls.Add(this.textBoxMoney);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.labelX1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "formMoneyCharge";
            this.Text = "formMoneyCharge";
            ((System.ComponentModel.ISupportInitialize)(this.textBoxMoney)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.Editors.DoubleInput textBoxMoney;
    }
}