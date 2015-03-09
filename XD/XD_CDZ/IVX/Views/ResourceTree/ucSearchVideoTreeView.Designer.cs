namespace BOCOM.IVX.Views.ResourceTree
{
    partial class ucSearchVideoTreeView
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
            this.ucObjectVideoTreeView1 = new BOCOM.IVX.Views.ResourceTree.ucObjectVideoTreeView();
            this.ucFaceVideoTreeView1 = new BOCOM.IVX.Views.ResourceTree.ucFaceVideoTreeView();
            this.ucCarVideoTreeView1 = new BOCOM.IVX.Views.ResourceTree.ucCarVideoTreeView();
            this.SuspendLayout();
            // 
            // ucObjectVideoTreeView1
            // 
            this.ucObjectVideoTreeView1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ucObjectVideoTreeView1.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucObjectVideoTreeView1.Appearance.Options.UseBackColor = true;
            this.ucObjectVideoTreeView1.Appearance.Options.UseFont = true;
            this.ucObjectVideoTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucObjectVideoTreeView1.Location = new System.Drawing.Point(0, 0);
            this.ucObjectVideoTreeView1.Name = "ucObjectVideoTreeView1";
            this.ucObjectVideoTreeView1.Size = new System.Drawing.Size(250, 354);
            this.ucObjectVideoTreeView1.TabIndex = 2;
            // 
            // ucFaceVideoTreeView1
            // 
            this.ucFaceVideoTreeView1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ucFaceVideoTreeView1.Appearance.Options.UseBackColor = true;
            this.ucFaceVideoTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucFaceVideoTreeView1.Location = new System.Drawing.Point(0, 0);
            this.ucFaceVideoTreeView1.Name = "ucFaceVideoTreeView1";
            this.ucFaceVideoTreeView1.Size = new System.Drawing.Size(250, 354);
            this.ucFaceVideoTreeView1.TabIndex = 1;
            // 
            // ucCarVideoTreeView1
            // 
            this.ucCarVideoTreeView1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ucCarVideoTreeView1.Appearance.Options.UseBackColor = true;
            this.ucCarVideoTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucCarVideoTreeView1.Location = new System.Drawing.Point(0, 0);
            this.ucCarVideoTreeView1.Name = "ucCarVideoTreeView1";
            this.ucCarVideoTreeView1.Size = new System.Drawing.Size(250, 354);
            this.ucCarVideoTreeView1.TabIndex = 0;
            // 
            // ucSearchVideoTreeView
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ucObjectVideoTreeView1);
            this.Controls.Add(this.ucFaceVideoTreeView1);
            this.Controls.Add(this.ucCarVideoTreeView1);
            this.Name = "ucSearchVideoTreeView";
            this.Size = new System.Drawing.Size(250, 354);
            this.Load += new System.EventHandler(this.ucSearchVideoTreeView_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ucCarVideoTreeView ucCarVideoTreeView1;
        private ucFaceVideoTreeView ucFaceVideoTreeView1;
        private ucObjectVideoTreeView ucObjectVideoTreeView1;

    }
}
