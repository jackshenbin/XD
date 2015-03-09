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

    public partial class ucChangeUserPass : UserControl
    {
        string sms_sqlstr = "";

        public event EventHandler ChangePassComplete;
        public ucChangeUserPass()
        {
            InitializeComponent();
        }
        bool ValidateChangepass()
        {
            bool ret = true;

            sms_sqlstr = "select id,user_name,`passwd`,user_auth from user_manage_t where id="
                + Framework.Environment.UserID.ToString(); ;
            MySqlDataAdapter sms_da = new MySqlDataAdapter(sms_sqlstr, Framework.Environment.SMS_CONN);
            DataSet sms_ds = new DataSet();
            sms_da.Fill(sms_ds, "T");
            if (sms_ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = sms_ds.Tables[0].Rows[0];
                string oldpass = row["passwd"].ToString();
                if (oldpass == textBoxPasswordOld.Text)
                {
                    string newpass = textBoxPassword.Text;
                    string confermnewpass = textBoxPassword2.Text;
                    if (newpass != confermnewpass)
                    {
                        labelRet.Text = "两次输入的密码不同";
                        labelRet.ForeColor = Color.Red;
                        ret = false;

                    }
                }
                else
                {
                    labelRet.Text = "原密码不正确";
                    labelRet.ForeColor = Color.Red;
                    ret = false;

                }
            }
            else
            {
                labelRet.Text = "无此用户";
                labelRet.ForeColor = Color.Red;
                ret = false;
            }


            return ret;
        }

        private void buttonChangepass_Click(object sender, EventArgs e)
        {
            if (!ValidateChangepass())
                return;

            string sms_sqlstr = "update `user_manage_t` set `passwd` = '" + textBoxPassword.Text + "' where id ='" 
                + Framework.Environment.UserID.ToString() + "'";

            MySqlCommand sms_comm = new MySqlCommand(sms_sqlstr, Framework.Environment.SMS_CONN);
            sms_comm.Connection.Open();
            try
            {
                sms_comm.ExecuteNonQuery();
                labelRet.Text = "修改用户密码成功";
                labelRet.ForeColor = Color.Blue;
                textBoxPasswordOld.Text = "";
                textBoxPassword.Text = "";
                textBoxPassword2.Text = "";

            }
            catch (MySqlException)
            {
                labelRet.Text = "修改用户密码失败";
                labelRet.ForeColor = Color.Red;
            }
            sms_comm.Connection.Close();
            if (ChangePassComplete != null)
            {
                ChangePassComplete(textBoxUserName.Text, null);
            }
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            if (textBoxPassword.Text.Length >= 8)
            {
                bool m1 = System.Text.RegularExpressions.Regex.IsMatch(textBoxPassword.Text, @"[a-zA-z]");
                bool m2 = System.Text.RegularExpressions.Regex.IsMatch(textBoxPassword.Text, @"[0-9]");

                if (m1 && m2)
                    stepIndicator2.CurrentStep = 3;
                else
                    stepIndicator2.CurrentStep = 2;
            }
            else if (textBoxPassword.Text.Length < 6)
            { stepIndicator2.CurrentStep = 0; }
            else
            {
                stepIndicator2.CurrentStep = 1;
            }
        }

        private void ucChangeUserPass_Load(object sender, EventArgs e)
        {
            textBoxUserName.Text = Framework.Environment.UserName;

        }

    }
}
