using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;


namespace BOCOM.IVX.Controls
{
    public class AutoHistoryComboBox : ComboBoxEdit
    {
        // private DevExpress.XtraEditors.Repository.RepositoryItemComboBox fProperties;
        private List<string> m_HisItems;

        public int MaxRecord { get; set; }

        public AutoHistoryComboBox()
            : base()
        {
            m_HisItems = new List<string>();
            this.Properties.CaseSensitiveSearch = true;
            this.TextChanged += new EventHandler(AutoHistoryComboBox_TextChanged);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
        }

        void AutoHistoryComboBox_TextChanged(object sender, EventArgs e)
        {
            string text = Text.Trim(); //.ToLower();

            Properties.Items.BeginUpdate();
            Properties.Items.Clear();

            if (string.IsNullOrEmpty(text))
            {
                Properties.Items.AddRange(m_HisItems);
            }
            else
            {
                m_HisItems.ForEach(s =>
                    {
                        if (s.StartsWith(text))
                        {
                            Properties.Items.Add(s);
                        }
                    });
            }

            //if (Properties.Items.Count == 0)
            //{
            //    Properties.AutoComplete = false;
            //}
            //else
            //{
            //    Properties.AutoComplete = true;
            //}

            Properties.Items.EndUpdate();
        }

        public void Save()
        {
            string text = Text.Trim();

            if (!Properties.Items.Contains(text))
            {
                if (Properties.Items.Count < MaxRecord)
                {
                    Properties.Items.Insert(0, text);
                    m_HisItems.Insert(0, text);
                }
                else
                {
                    Properties.Items.RemoveAt(Properties.Items.Count - 1);
                    m_HisItems.Remove(text);
                }
            }
            else
            {
                if (Properties.Items.IndexOf(text) != 0)
                {
                    Properties.Items.Remove(text);
                    Properties.Items.Insert(0, text);
                    m_HisItems.Insert(0, text);
                }
            }
        }

        private void InitializeComponent()
        {
            //this.fProperties = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            //((System.ComponentModel.ISupportInitialize)(this.fProperties)).BeginInit();
            //this.SuspendLayout();
            //// 
            //// fProperties
            //// 
            //this.fProperties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            //this.fProperties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            //this.fProperties.Appearance.Options.UseBackColor = true;
            //this.fProperties.Appearance.Options.UseForeColor = true;
            //this.fProperties.AppearanceDropDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            //this.fProperties.AppearanceDropDown.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            //this.fProperties.AppearanceDropDown.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            //this.fProperties.AppearanceDropDown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            //this.fProperties.AppearanceDropDown.Options.UseBackColor = true;
            //this.fProperties.AppearanceDropDown.Options.UseBorderColor = true;
            //this.fProperties.AppearanceDropDown.Options.UseForeColor = true;
            //this.fProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            //new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            //this.fProperties.LookAndFeel.SkinName = "DevExpress Dark Style";
            //this.fProperties.LookAndFeel.UseDefaultLookAndFeel = false;
            //this.fProperties.Name = "fProperties";
            //((System.ComponentModel.ISupportInitialize)(this.fProperties)).EndInit();
            //this.ResumeLayout(false);

        }

    }
}
