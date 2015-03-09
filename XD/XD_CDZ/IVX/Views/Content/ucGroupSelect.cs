using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BOCOM.IVX.Views.Content
{
    public partial class ucGroupSelect : UserControl
    {
        DataSet sms_ds;
        public ucGroupSelect()
        {
            InitializeComponent();

        }
        public event EventHandler GroupSelected;

        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string GroupTreeName { get; set; }

        private void ucGroupSelect_Load(object sender, EventArgs e)
        {
        }

        public void Init()
        { 
            string sms_sqlstr2 = "SELECT * FROM dev_tree_t";
            MySqlDataAdapter sms_da = new MySqlDataAdapter(sms_sqlstr2, Framework.Environment.SMS_CONN);
            sms_ds = new DataSet();
            sms_da.Fill(sms_ds, "T");

            flowLayoutPanel1.Controls.Clear();
            GroupID = 0;
            GroupName = "";
            GroupTreeName = "";
            foreach (DataRow item in sms_ds.Tables[0].Rows)
            {
                if (item["level"].ToString() == "1")
                {
                    DevComponents.DotNetBar.ButtonX btn = new DevComponents.DotNetBar.ButtonX();
                    btn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
                    btn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
                    btn.Name = "btn_" + item["node_id"].ToString();
                    btn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
                    btn.Text = item["node_name"].ToString();
                    SizeF size = btn.CreateGraphics().MeasureString(btn.Text, DefaultFont);
                    btn.Size = new System.Drawing.Size((int)size.Width+40, 23);
                    btn.Tag = item;
                    btn.Click += btn_Click;
                    flowLayoutPanel1.Controls.Add(btn);
                }
            }
        }

        void level4_btn_Click(object sender, EventArgs e)
        {             
            DataRow row = ((DevComponents.DotNetBar.ButtonX)sender).Tag as DataRow;

            if (row != null)
            {
                GroupID = int.Parse(row["node_id"].ToString());
                GroupName = row["node_name"].ToString();
                GroupTreeName += "." + row["node_name"].ToString();
                if (GroupSelected != null)
                    GroupSelected(GroupID, e);
            }
        }

        void btn_Click(object sender, EventArgs e)
        {
            DataRow row = ((DevComponents.DotNetBar.ButtonX)sender).Tag as DataRow;

            if (row != null)
            {
                GroupTreeName += "." + row["node_name"].ToString();
                flowLayoutPanel1.Controls.Clear();
                foreach (DataRow item in sms_ds.Tables[0].Rows)
                {
                    if (item["parent_id"].ToString() == row["node_id"].ToString())
                    {
                        DevComponents.DotNetBar.ButtonX btn = new DevComponents.DotNetBar.ButtonX();
                        btn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
                        btn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
                        btn.Name = "btn_" + item["node_id"].ToString();
                        btn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
                        btn.Text = item["node_name"].ToString();
                        SizeF size = btn.CreateGraphics().MeasureString(btn.Text, DefaultFont);
                        btn.Size = new System.Drawing.Size((int)size.Width + 40, 23);
                        btn.Tag = item;
                        if(item["level"].ToString()=="4")
                            btn.Click+=level4_btn_Click;
                        else
                            btn.Click += btn_Click;
                        flowLayoutPanel1.Controls.Add(btn);
                    }
                }

            }
        }



    }
}
