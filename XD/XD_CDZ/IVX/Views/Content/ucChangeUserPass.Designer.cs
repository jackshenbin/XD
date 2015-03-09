namespace BOCOM.IVX.Views.Content
{
    partial class ucChangeUserPass
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
            this.stepIndicator2 = new DevComponents.DotNetBar.Controls.StepIndicator();
            this.labelRet = new DevComponents.DotNetBar.LabelX();
            this.textBoxPassword2 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.textBoxPassword = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.textBoxUserName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.buttonChangePass = new DevComponents.DotNetBar.ButtonX();
            this.textBoxPasswordOld = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // stepIndicator2
            // 
            this.stepIndicator2.CurrentStep = 0;
            this.stepIndicator2.Location = new System.Drawing.Point(341, 83);
            this.stepIndicator2.Name = "stepIndicator2";
            this.stepIndicator2.Size = new System.Drawing.Size(84, 18);
            this.stepIndicator2.StepCount = 3;
            this.stepIndicator2.TabIndex = 4;
            this.stepIndicator2.Text = "stepIndicator2";
            // 
            // labelRet
            // 
            // 
            // 
            // 
            this.labelRet.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelRet.Location = new System.Drawing.Point(272, 141);
            this.labelRet.Name = "labelRet";
            this.labelRet.Size = new System.Drawing.Size(193, 23);
            this.labelRet.TabIndex = 1;
            // 
            // textBoxPassword2
            // 
            // 
            // 
            // 
            this.textBoxPassword2.Border.Class = "TextBoxBorder";
            this.textBoxPassword2.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxPassword2.Location = new System.Drawing.Point(178, 107);
            this.textBoxPassword2.Name = "textBoxPassword2";
            this.textBoxPassword2.Size = new System.Drawing.Size(153, 21);
            this.textBoxPassword2.TabIndex = 2;
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(97, 103);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 23);
            this.labelX3.TabIndex = 1;
            this.labelX3.Text = "确认密码";
            // 
            // textBoxPassword
            // 
            // 
            // 
            // 
            this.textBoxPassword.Border.Class = "TextBoxBorder";
            this.textBoxPassword.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxPassword.Location = new System.Drawing.Point(178, 80);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(153, 21);
            this.textBoxPassword.TabIndex = 2;
            this.textBoxPassword.TextChanged += new System.EventHandler(this.textBoxPassword_TextChanged);
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(97, 76);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "密码";
            // 
            // textBoxUserName
            // 
            // 
            // 
            // 
            this.textBoxUserName.Border.Class = "TextBoxBorder";
            this.textBoxUserName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxUserName.Enabled = false;
            this.textBoxUserName.Location = new System.Drawing.Point(178, 25);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(153, 21);
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
            this.labelX5.Size = new System.Drawing.Size(152, 23);
            this.labelX5.TabIndex = 1;
            this.labelX5.Text = "修改个人密码";
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(97, 21);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "用户名";
            // 
            // buttonChangePass
            // 
            this.buttonChangePass.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonChangePass.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonChangePass.Location = new System.Drawing.Point(178, 141);
            this.buttonChangePass.Name = "buttonChangePass";
            this.buttonChangePass.Size = new System.Drawing.Size(75, 23);
            this.buttonChangePass.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonChangePass.TabIndex = 0;
            this.buttonChangePass.Text = "修改密码";
            this.buttonChangePass.Click += new System.EventHandler(this.buttonChangepass_Click);
            // 
            // textBoxPasswordOld
            // 
            // 
            // 
            // 
            this.textBoxPasswordOld.Border.Class = "TextBoxBorder";
            this.textBoxPasswordOld.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxPasswordOld.Location = new System.Drawing.Point(178, 53);
            this.textBoxPasswordOld.Name = "textBoxPasswordOld";
            this.textBoxPasswordOld.Size = new System.Drawing.Size(153, 21);
            this.textBoxPasswordOld.TabIndex = 2;
            this.textBoxPasswordOld.TextChanged += new System.EventHandler(this.textBoxPassword_TextChanged);
            // 
            // labelX4
            // 
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(97, 49);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(75, 23);
            this.labelX4.TabIndex = 1;
            this.labelX4.Text = "原密码";
            // 
            // ucChangeUserPass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.stepIndicator2);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.labelRet);
            this.Controls.Add(this.buttonChangePass);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.textBoxPassword2);
            this.Controls.Add(this.textBoxUserName);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.textBoxPasswordOld);
            this.Controls.Add(this.textBoxPassword);
            this.Name = "ucChangeUserPass";
            this.Size = new System.Drawing.Size(489, 187);
            this.Load += new System.EventHandler(this.ucChangeUserPass_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.StepIndicator stepIndicator2;
        private DevComponents.DotNetBar.LabelX labelRet;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxPassword2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxPassword;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxUserName;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX buttonChangePass;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxPasswordOld;
        private DevComponents.DotNetBar.LabelX labelX4;
    }
}
