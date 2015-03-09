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

    public partial class ucCurrUserPanel : UserControl
    {
        public ucCurrUserPanel()
        {
            InitializeComponent();

        }

        private void ucCurrUserPanel_Load(object sender, EventArgs e)
        {
            string sms_sqlstr = "select id,user_name,`reg_time`,user_auth from user_manage_t where id=" 
                + Framework.Environment.UserID.ToString(); ;
            MySqlDataAdapter sms_da = new MySqlDataAdapter(sms_sqlstr, Framework.Environment.SMS_CONN);
            DataSet sms_ds = new DataSet();
            sms_da.Fill(sms_ds, "T");
            if (sms_ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = sms_ds.Tables[0].Rows[0];
                textBoxUserName.Text = row["user_name"].ToString();
                textBoxCreateTime.Text = row["reg_time"].ToString();
                textBoxLastTime.Text = row["reg_time"].ToString();
                comboBoxUserType.SelectedIndex = int.Parse(row["user_auth"].ToString())-1;
            }

        }



    }
}
