using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BOCOM.IVX.Controls;

namespace BOCOM.IVX
{
    public partial class XtraUserControlColorViewer : DevExpress.XtraEditors.XtraUserControl
    {
        public XtraUserControlColorViewer()
        {
            InitializeComponent();
        }
        public event EventHandler ItemChange;
        public Color Value
        {
            get
            {
                CheckedPictureBox pic = null;
                foreach (Control c in flowLayoutPanel1.Controls)
                {
                     pic = (CheckedPictureBox)c;
                     if (pic.Checked) break;
                     else pic = null;
                }
                if (pic != null)
                {
                    string id = pic.Name.Replace("checkedPictureBox", "");
                    if (id != "Null")
                        return GetColorById(int.Parse(id));
                }
                return Color.Transparent;
            }
            //set
            //{
            //    foreach (DevExpress.XtraEditors.Controls.ImageComboBoxItem item in colorComboBoxEx1.Properties.Items)
            //    {
            //        if (int.Parse(item.Value.ToString()) == value.ToArgb())
            //        {
            //            colorComboBoxEx1.SelectedItem = item;
            //            break;
            //        }
            //    }
            //}
        }

        bool isPlateColor = false;
        public bool IsPlateColor
        {
            get { return isPlateColor; }
            set
            {
                isPlateColor = value;
                if (isPlateColor)
                {
                    checkedPictureBox2.Visible = false;
                    checkedPictureBox4.Visible = false;
                    checkedPictureBox5.Visible = false;
                    checkedPictureBox8.Visible = false;
                    checkedPictureBox9.Visible = false;
                    checkedPictureBox10.Visible = false;
                    checkedPictureBox11.Visible = false;
                }
                else
                { 
                    checkedPictureBox2.Visible = true ;
                    checkedPictureBox4.Visible = true;
                    checkedPictureBox5.Visible = true;
                    checkedPictureBox8.Visible = true;
                    checkedPictureBox9.Visible = true;
                    checkedPictureBox10.Visible = true;
                    checkedPictureBox11.Visible = true;
                }
            }
        }
        private Color GetColorById(int id)
        {
            Color c = Color.Transparent;
            switch (id)
            {
                case 1: c = Color.White; break;
                case 2: c = Color.Silver; break;
                case 3: c = Color.Black; break;
                case 4: c = Color.Red; break;
                case 5: c = Color.Purple; break;
                case 6: c = Color.Blue ; break;
                case 7: c = Color.Yellow; break;
                case 8: c = Color.Green; break;
                case 9: c = Color.Brown; break;
                case 10: c = Color.Pink; break;
                case 11: c = Color.Gray; break;
                default : c = Color.Transparent; break;
            }
            return c;
        }

        private void checkedPictureBox12_CheckedChanged(object sender, EventArgs e)
        {
            CheckedPictureBox check = (CheckedPictureBox)sender;
            if (check.Checked)
            {
                foreach (Control c in flowLayoutPanel1.Controls)
                {
                    if (c is CheckedPictureBox )
                    {
                       
                        CheckedPictureBox pic = (CheckedPictureBox)c;
                        if (pic.Name != check.Name && pic.Checked)
                            pic.Checked = false;
                    }
                }
            }
            if (ItemChange!=null) ItemChange(sender, e);
        }
    }
}
