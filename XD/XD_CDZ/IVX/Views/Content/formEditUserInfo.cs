using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BOCOM.IVX.Views.Content
{
    public partial class formEditUserInfo : Form
    {
        int m_id = 0;
        public formEditUserInfo(int id, string usname, int user_auth)
        {
            InitializeComponent();
            m_id = id;
            textBoxUserName.Text = usname;
            comboBoxUserType.SelectedIndex = user_auth-1;
        }

        private void btnChangeType_Click(object sender, EventArgs e)
        {
            string sms_sqlstr = "update `user_manage_t` set `user_auth` = '" + (comboBoxUserType.SelectedIndex + 1) + "' where id ='"
    + m_id + "'";

            MySqlCommand sms_comm = new MySqlCommand(sms_sqlstr, Framework.Environment.SMS_CONN);
            sms_comm.Connection.Open();
            try
            {
                sms_comm.ExecuteNonQuery();
                textBoxPassword2.Text = "";

            }
            catch (MySqlException)
            {
                MessageBox.Show("修改用户类型失败", "用户信息", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            sms_comm.Connection.Close();
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnChangePass_Click(object sender, EventArgs e)
        {
            string sms_sqlstr = "update `user_manage_t` set `passwd` = '" + textBoxPassword2.Text + "' where id ='"
                + m_id + "'";

            MySqlCommand sms_comm = new MySqlCommand(sms_sqlstr, Framework.Environment.SMS_CONN);
            sms_comm.Connection.Open();
            try
            {
                sms_comm.ExecuteNonQuery();
                textBoxPassword2.Text = "";

            }
            catch (MySqlException)
            {
                MessageBox.Show("重置用户密码失败","用户信息",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
            sms_comm.Connection.Close();
            DialogResult = System.Windows.Forms.DialogResult.OK;

        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            string sms_sqlstr = "delete from `user_manage_t` where id ='"
                + m_id + "'";

            MySqlCommand sms_comm = new MySqlCommand(sms_sqlstr, Framework.Environment.SMS_CONN);
            sms_comm.Connection.Open();
            try
            {
                sms_comm.ExecuteNonQuery();
                textBoxPassword2.Text = "";

            }
            catch (MySqlException)
            {
                MessageBox.Show("删除用户失败", "用户信息", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            sms_comm.Connection.Close();
            DialogResult = System.Windows.Forms.DialogResult.OK;

        }
    }
}
