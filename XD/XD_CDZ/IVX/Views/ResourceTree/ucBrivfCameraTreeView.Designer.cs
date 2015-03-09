namespace BOCOM.IVX.Views.ResourceTree
{
    partial class ucBrivfCameraTreeView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ucResourceTreeViewBase1 = new BOCOM.IVX.Views.ResourceTree.ucResourceTreeViewByTaskBase();
            this.SuspendLayout();
            // 
            // ucResourceTreeViewBase1
            // 
            this.ucResourceTreeViewBase1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ucResourceTreeViewBase1.Appearance.Options.UseBackColor = true;
            this.ucResourceTreeViewBase1.Caption = " 摘要相关监控点";
            this.ucResourceTreeViewBase1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucResourceTreeViewBase1.Location = new System.Drawing.Point(0, 0);
            this.ucResourceTreeViewBase1.MultiSelect = true;
            this.ucResourceTreeViewBase1.Name = "ucResourceTreeViewBase1";
            this.ucResourceTreeViewBase1.Padding = new System.Windows.Forms.Padding(2);
            this.ucResourceTreeViewBase1.ShowCheckBoxes = false;
            this.ucResourceTreeViewBase1.ShowImage = true;
            this.ucResourceTreeViewBase1.ShowTitle = true;
            this.ucResourceTreeViewBase1.Size = new System.Drawing.Size(501, 523);
            this.ucResourceTreeViewBase1.TabIndex = 0;
            this.ucResourceTreeViewBase1.ViewModelbase = null;
            // 
            // ucBrivfCameraTreeView
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ucResourceTreeViewBase1);
            this.Name = "ucBrivfCameraTreeView";
            this.Size = new System.Drawing.Size(501, 523);
            this.Load += new System.EventHandler(this.ucBrivfCameraTreeView_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ucResourceTreeViewByTaskBase ucResourceTreeViewBase1;
    }
}
