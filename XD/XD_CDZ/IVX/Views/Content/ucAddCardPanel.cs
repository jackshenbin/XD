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

    public partial class ucAddCardPanel : UserControl
    {

        public event EventHandler AddCardComplete;
        public ucAddCardPanel()
        {
            InitializeComponent();

        }
        bool ValidateAddCard()
        {

            bool ret = true;

            if (textBoxCardID.Text == "")
            {
                labelRet.Text = "用户卡号不能为空";
                labelRet.ForeColor = Color.Red;
                ret = false;
            }
            if (textBoxCardSerialNumber.Text == "")
            {
                labelRet.Text = "物理卡号不能为空，请读卡获取";
                labelRet.ForeColor = Color.Red;
                ret = false;
            }
            if (textBoxUserName.Text == "")
            {
                labelRet.Text = "用户姓名不能为空";
                labelRet.ForeColor = Color.Red;
                ret = false;
            }
            if (textBoxPassword.Text == "")
            {
                labelRet.Text = "密码不能为空";
                labelRet.ForeColor = Color.Red;
                ret = false;
            }

            return ret;
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (!ValidateAddCard())
                return;

            string msg = string.Format("是否要创建新卡？" + Environment.NewLine
            + "姓名：{0}" + Environment.NewLine
            + "用户卡号：{1}" + Environment.NewLine
            + "物理卡号：{2}" + Environment.NewLine
            + "密码使能：{3}" + Environment.NewLine
            + "性别：{4}" + Environment.NewLine
            + "证件类型：{5}" + Environment.NewLine
            + "证件号码：{6}" + Environment.NewLine
            + "联系电话：{7}" + Environment.NewLine
            + "车牌号码：{8}" + Environment.NewLine
            + "通信地址：{9}" + Environment.NewLine
            , textBoxUserName.Text
            , textBoxCardID.Text
            , textBoxCardSerialNumber.Text
            , checkBoxPasswordable.Checked?"使用密码":"无需密码"
            , checkBoxMale.Checked?"男":"女"
            , comboBoxExIdentityType.SelectedItem.ToString()
            , textBoxIdentityNumber.Text
            , textBoxTelephone.Text
            , textBoxCarPlant.Text
            , textBoxAddress.Text);
            if (MessageBox.Show(msg, "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2) != DialogResult.OK)
                return;

            try
            {

            string ret = RFIDREAD.RFIDReader.NewCard(textBoxCardID.Text, textBoxPassword.Text,checkBoxPasswordable.Checked);
            if (ret == textBoxCardID.Text)
            {
                string sms_sqlstr = string.Format("INSERT INTO `user_card_list_t` (`user_card_id`,`phy_card`, `master_name`,`sex`, `license_plate`,  `phone_num`, `start_time`, `id_type`, `id_num`, `home_addr`, `mail_addr`,`total_balance`,`account_balance`,`elec_pkg_balance`) "
              + "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}','{11}','{12}','{13}')"
                , textBoxCardID.Text
                , textBoxCardSerialNumber.Text
                , textBoxUserName.Text
                , checkBoxMale.Checked ? 0 : 1
                , textBoxCarPlant.Text
                , textBoxTelephone.Text
                , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                , comboBoxExIdentityType.SelectedIndex
                , textBoxIdentityNumber.Text
                , textBoxAddress.Text
                , ""
                ,0
                ,0
                ,0);
                System.Diagnostics.Trace.WriteLine(sms_sqlstr);
                MySqlCommand sms_comm = new MySqlCommand(sms_sqlstr, Framework.Environment.SMS_CONN);
                sms_comm.Connection.Open();
                try
                {
                    sms_comm.ExecuteNonQuery();
                    labelRet.Text = "新建卡成功";
                    labelRet.ForeColor = Color.Blue;
                }
                catch (MySqlException)
                {
                    labelRet.Text = "新建卡写入数据库失败";
                    labelRet.ForeColor = Color.Red;
                }
                sms_comm.Connection.Close();

                if (AddCardComplete != null)
                    AddCardComplete(textBoxCardID.Text, null);
            }
            else
            { 
                labelRet.Text = "创建新卡失败";
                labelRet.ForeColor = Color.Red;
            }
            }
            catch (Exception ex)
            {
                
                labelRet.Text = "创建新卡失败: "+ex.Message;
                labelRet.ForeColor = Color.Red;
            }
        }

        private void btnReadCardNumber_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxCardSerialNumber.Text = "";
                textBoxCardID.Text = "";
                labelRet.Text = "";

                string uid = RFIDREAD.RFIDReader.ReadUID();
                textBoxCardSerialNumber.Text = uid;
                bool isold = RFIDREAD.RFIDReader.IsOldCard();
                if (isold)
                {
                    labelRet.Text = "卡已开，请先退卡";
                    labelRet.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            { 
                    labelRet.Text = "错误："+ex.Message;
                    labelRet.ForeColor = Color.Red;
            }
        }

        private void checkBoxPasswordable_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPassword.Text = checkBoxPasswordable.Checked ? "" : "666666";
             textBoxPassword.Enabled = checkBoxPasswordable.Checked;
       }

        private void ucAddCardPanel_Load(object sender, EventArgs e)
        {
            comboBoxExIdentityType.SelectedIndex = 0;
        }



    }
}
