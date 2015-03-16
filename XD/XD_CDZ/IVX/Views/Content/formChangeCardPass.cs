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
    public partial class formChangeCardPass : Form
    {
        public formChangeCardPass()
        {
            InitializeComponent();
        }

        bool ValidateChangepass()
        {
            bool ret = true;
            RFIDREAD.CardInfo info = RFIDREAD.RFIDReader.ReadCardInfo();
            string oldpass = "";

            if (true)//(oldpass == textBoxPasswordOld.Text)
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


            return ret;
        }

        private void buttonChangepass_Click(object sender, EventArgs e)
        {
            if (!ValidateChangepass())
                return;

            try
            {
            RFIDREAD.RFIDReader.ChangePassword(textBoxPassword.Text, checkBoxPasswordable.Checked);
                labelRet.Text = "修改用户密码成功";
                labelRet.ForeColor = Color.Blue;
                this.Close();
            }
            catch (Exception ex)
            {
                labelRet.Text = "修改用户密码失败";
                labelRet.ForeColor = Color.Red;
            }
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            string s = ((TextBox)sender).Text;
            for (int i = s.Length - 1; i >= 0; i--)
            {
                if (!char.IsDigit(s, i))
                {
                    s = s.Remove(i, 1);
                }
            }
            ((TextBox)sender).Text = s;

        }
        private void checkBoxPasswordable_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPassword.Text = checkBoxPasswordable.Checked ? "" : "666666";
            textBoxPassword2.Text = checkBoxPasswordable.Checked ? "" : "666666";
            textBoxPassword.Enabled = checkBoxPasswordable.Checked;
            textBoxPassword2.Enabled = checkBoxPasswordable.Checked;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
