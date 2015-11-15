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

    public partial class ucAddUserPanel : UserControl
    {

        public event EventHandler AddUserComplete;
        public ucAddUserPanel()
        {
            InitializeComponent();
            comboBoxUserType.SelectedIndex = 2;

        }
        bool ValidateAddUser()
        {
            bool ret = true;
            string username = textBoxUserName.Text;
            bool ismatch = System.Text.RegularExpressions.Regex.IsMatch(username, @"[a-zA-z0-9]+");
            if (!ismatch )
            {
                labelRet.Text = "长度不能超过20字符，只能使用数字和字母";
                labelRet.ForeColor = Color.Red;
                ret = false;
            }
            if (username.Length>20 )
            {
                labelRet.Text = "长度不能超过20字符，只能使用数字和字母";
                labelRet.ForeColor = Color.Red;
                ret = false;
            }
            if ( username.Length<6)
            {
                labelRet.Text = "长度需要超过5字符，只能使用数字和字母";
                labelRet.ForeColor = Color.Red;
                ret = false;
            }
            string password = textBoxPassword.Text;
            if (password != textBoxPassword2.Text)
            {
                labelRet.Text = "两次输入的密码不同";
                labelRet.ForeColor = Color.Red;
                ret = false;

            }
            if (username.ToLower() == "admin")
            { 
                labelRet.Text = "不能以系统用户做用户名";
                labelRet.ForeColor = Color.Red;
                ret = false;
            }
            return ret;
        }

        private void buttonAddUser_Click(object sender, EventArgs e)
        {
            if (!ValidateAddUser())
                return;

            string sms_sqlstr = "INSERT INTO `user_manage_t` (`user_name`, `passwd`, `user_auth`, `reg_time`) "
    + "VALUES ('" + textBoxUserName.Text + "', '" + textBoxPassword.Text + "', '" + (comboBoxUserType.SelectedIndex + 1) + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

            MySqlCommand sms_comm = new MySqlCommand(sms_sqlstr, Framework.Environment.SMS_CONN);
            sms_comm.Connection.Open();
            try
            {
                sms_comm.ExecuteNonQuery();
                labelRet.Text = "添加用户成功";
                labelRet.ForeColor = Color.Blue;
                textBoxUserName.Text = "";
                textBoxPassword.Text = "";
                textBoxPassword2.Text = "";
                comboBoxUserType.SelectedIndex = 2;
            }
            catch (MySqlException)
            {
                labelRet.Text = "添加用户失败";
                labelRet.ForeColor = Color.Red;
            }
            sms_comm.Connection.Close();
            if(AddUserComplete!=null)
            {
                AddUserComplete(textBoxUserName.Text, null);
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

    }
}
