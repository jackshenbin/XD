namespace BOCOM.IVX.Views.Content
{
    partial class formChangeCardPass
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
            this.labelRet = new DevComponents.DotNetBar.LabelX();
            this.buttonChangePass = new DevComponents.DotNetBar.ButtonX();
            this.textBoxPassword2 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.textBoxPasswordOld = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.textBoxPassword = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.checkBoxPasswordable = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // labelRet
            // 
            this.labelRet.AutoSize = true;
            // 
            // 
            // 
            this.labelRet.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelRet.Location = new System.Drawing.Point(109, 138);
            this.labelRet.Name = "labelRet";
            this.labelRet.Size = new System.Drawing.Size(0, 0);
            this.labelRet.TabIndex = 6;
            // 
            // buttonChangePass
            // 
            this.buttonChangePass.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonChangePass.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonChangePass.Location = new System.Drawing.Point(102, 107);
            this.buttonChangePass.Name = "buttonChangePass";
            this.buttonChangePass.Size = new System.Drawing.Size(75, 23);
            this.buttonChangePass.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonChangePass.TabIndex = 5;
            this.buttonChangePass.Text = "修改密码";
            this.buttonChangePass.Click += new System.EventHandler(this.buttonChangepass_Click);
            // 
            // textBoxPassword2
            // 
            // 
            // 
            // 
            this.textBoxPassword2.Border.Class = "TextBoxBorder";
            this.textBoxPassword2.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxPassword2.Location = new System.Drawing.Point(102, 73);
            this.textBoxPassword2.Name = "textBoxPassword2";
            this.textBoxPassword2.PasswordChar = '*';
            this.textBoxPassword2.Size = new System.Drawing.Size(153, 21);
            this.textBoxPassword2.TabIndex = 10;
            this.textBoxPassword2.TextChanged += new System.EventHandler(this.textBoxPassword_TextChanged);
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(21, 69);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 23);
            this.labelX3.TabIndex = 7;
            this.labelX3.Text = "确认密码";
            // 
            // labelX4
            // 
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(21, 15);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(75, 23);
            this.labelX4.TabIndex = 8;
            this.labelX4.Text = "原密码";
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(21, 42);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 9;
            this.labelX2.Text = "密码";
            // 
            // textBoxPasswordOld
            // 
            // 
            // 
            // 
            this.textBoxPasswordOld.Border.Class = "TextBoxBorder";
            this.textBoxPasswordOld.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxPasswordOld.Location = new System.Drawing.Point(102, 19);
            this.textBoxPasswordOld.Name = "textBoxPasswordOld";
            this.textBoxPasswordOld.PasswordChar = '*';
            this.textBoxPasswordOld.Size = new System.Drawing.Size(153, 21);
            this.textBoxPasswordOld.TabIndex = 11;
            // 
            // textBoxPassword
            // 
            // 
            // 
            // 
            this.textBoxPassword.Border.Class = "TextBoxBorder";
            this.textBoxPassword.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxPassword.Location = new System.Drawing.Point(102, 46);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(153, 21);
            this.textBoxPassword.TabIndex = 12;
            this.textBoxPassword.TextChanged += new System.EventHandler(this.textBoxPassword_TextChanged);
            // 
            // checkBoxPasswordable
            // 
            // 
            // 
            // 
            this.checkBoxPasswordable.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBoxPasswordable.Checked = true;
            this.checkBoxPasswordable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPasswordable.CheckValue = "Y";
            this.checkBoxPasswordable.Location = new System.Drawing.Point(261, 44);
            this.checkBoxPasswordable.Name = "checkBoxPasswordable";
            this.checkBoxPasswordable.Size = new System.Drawing.Size(100, 23);
            this.checkBoxPasswordable.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.checkBoxPasswordable.TabIndex = 14;
            this.checkBoxPasswordable.Text = "密码使能";
            this.checkBoxPasswordable.CheckedChanged += new System.EventHandler(this.checkBoxPasswordable_CheckedChanged);
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(183, 107);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(75, 23);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 15;
            this.buttonX1.Text = "取消修改";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // formChangeCardPass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 162);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.checkBoxPasswordable);
            this.Controls.Add(this.labelRet);
            this.Controls.Add(this.buttonChangePass);
            this.Controls.Add(this.textBoxPassword2);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.textBoxPasswordOld);
            this.Controls.Add(this.textBoxPassword);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "formChangeCardPass";
            this.Text = "修改智能卡密码";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelRet;
        private DevComponents.DotNetBar.ButtonX buttonChangePass;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxPassword2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxPasswordOld;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxPassword;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxPasswordable;
        private DevComponents.DotNetBar.ButtonX buttonX1;
    }
}