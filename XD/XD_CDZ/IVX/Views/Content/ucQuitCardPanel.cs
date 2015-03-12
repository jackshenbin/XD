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

    public partial class ucQuitCardPanel : UserControl
    {
        BindingSource bs;

        public event EventHandler QuitCardComplete;
        public ucQuitCardPanel()
        {
            InitializeComponent();

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
                    try
                    {
                        RFIDREAD.CardInfo info = RFIDREAD.RFIDReader.ReadCardInfo();
                        textBoxWalletMoney.Value = info.money / 100f;
                        checkBoxFrozen.Checked = info.bLockCard;
                        textBoxCardID.Text = info.cardId; ;
                    }
                    catch (Exception ex)
                    {
                        labelRet.Text = "发现错误：" + ex.Message;
                        labelRet.ForeColor = Color.Red;

                    }

                }
                else
                {
                    labelRet.Text = "未开卡，请先开卡";
                    labelRet.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            { 
                labelRet.Text = "发现错误：" + ex.Message;
                labelRet.ForeColor = Color.Red;
            }

        }



        private void textBoxCardID_TextChanged(object sender, EventArgs e)
        {
            GetUserInfo(textBoxCardID.Text, Convert.ToInt32(textBoxWalletMoney.Value * 100));
        }

        private void GetUserInfo(string cardid, int money)
        {
            string sms_sqlstr = "select * from user_card_list_t where  card_state=1 and user_card_id='" + cardid + "'";

            MySqlDataAdapter sms_da = new MySqlDataAdapter(sms_sqlstr, Framework.Environment.SMS_CONN);
            DataSet sms_ds = new DataSet();
            sms_da.Fill(sms_ds, "T");

            if (sms_ds.Tables[0].Rows.Count > 0)
            {
                textBoxUserName.Text = sms_ds.Tables[0].Rows[0]["master_name"].ToString();
                textBoxWalletMoney.Text = sms_ds.Tables[0].Rows[0]["elec_pkg_balance"].ToString();
                textBoxMoney.Text = sms_ds.Tables[0].Rows[0]["account_balance"].ToString();
                textBoxCreatDate.Text = sms_ds.Tables[0].Rows[0]["start_time"].ToString();
                float oldmoney = float.Parse(sms_ds.Tables[0].Rows[0]["elec_pkg_balance"].ToString());
                string usercardid = sms_ds.Tables[0].Rows[0]["user_card_id"].ToString();
                string phycardid = sms_ds.Tables[0].Rows[0]["phy_card"].ToString();
                if (money.ToString() != Convert.ToInt32(oldmoney * 100).ToString())
                {
                    string temp_sqlstr = "update user_card_list_t set elec_pkg_balance = '" + (money / 100f) + "' where  card_state=1 and user_card_id='" + cardid + "'";
                    MySqlCommand sms_comm = new MySqlCommand(temp_sqlstr, Framework.Environment.SMS_CONN);
                    sms_comm.Connection.Open();
                    try
                    {
                        sms_comm.ExecuteNonQuery();
                        labelRet.Text = "更新最新金额成功";
                        labelRet.ForeColor = Color.Blue;
                        sms_sqlstr = "INSERT INTO `money_change_info_t` (`phy_card`,`user_card_id`, `elec_pkg_balance`,`change_money`, `time`, `manager_id`, `manager_name`,`type`) "
                            + "VALUES ('" + phycardid + "', '" + usercardid + "', '" + (money / 100f) + "', '0', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + Framework.Environment.UserID + "', '" + Framework.Environment.UserName + "', '" + 3 + "')";

                        sms_comm.CommandText = sms_sqlstr;
                        sms_comm.ExecuteNonQuery();

                    }
                    catch (MySqlException)
                    {
                        labelRet.Text = "更新最新金额失败";
                        labelRet.ForeColor = Color.Red;

                    }
                    sms_comm.Connection.Close();

                }
            }


        }
        bool ValidateQuitCard()
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

            return ret;
        }

        private void buttonQuitCard_Click(object sender, EventArgs e)
        {
            if (!ValidateQuitCard())
                return;

            string msg = string.Format("是否真的要退卡？"+Environment.NewLine
                + "用户卡号：{0}"+Environment.NewLine
                + "姓名：{1}" + Environment.NewLine
                + "物理卡号：{2}" + Environment.NewLine
                + "账户余额：{3}" + Environment.NewLine
                + "钱包余额：{4}" + Environment.NewLine
                + "是否冻结：{5}" + Environment.NewLine
                + "建卡日期：{6}" + Environment.NewLine
                ,textBoxCardID.Text
                ,textBoxUserName.Text
                ,textBoxCardSerialNumber.Text
                ,textBoxMoney.Text
                ,textBoxWalletMoney.Text
                ,checkBoxFrozen.Checked?"冻结":"未冻结"
                ,textBoxCreatDate.Text);
            if (MessageBox.Show(msg, "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2) != DialogResult.OK)
                return;

            try
            {
                RFIDREAD.RFIDReader.QuitCard();
                string sms_sqlstr = "update user_card_list_t set card_state = 4 where card_state <4 and  user_card_id='" + textBoxCardID.Text + "'";
                MySqlCommand sms_comm = new MySqlCommand(sms_sqlstr, Framework.Environment.SMS_CONN);
                sms_comm.Connection.Open();
                try
                {
                    sms_comm.ExecuteNonQuery();
                    labelRet.Text = "退卡成功";
                    labelRet.ForeColor = Color.Blue;

                }
                catch (MySqlException)
                {
                    labelRet.Text = "退卡失败";
                    labelRet.ForeColor = Color.Red;
                }
                sms_comm.Connection.Close();
            }
            catch (Exception ex)
            { 
                labelRet.Text = "退卡失败:"+ex.Message;
                labelRet.ForeColor = Color.Red;
            }
        }


    }
}
