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

    public partial class ucCardStatChangePanel : UserControl
    {
        private double m_LastUncommitMoney = 0;
        public event EventHandler AddUserComplete;
        public ucCardStatChangePanel()
        {
            InitializeComponent();

        }

        private void ucChargePanel_Load(object sender, EventArgs e)
        {
        }

        private double GetLastUnCommitMoney(string cardid)
        { 
            string sms_sqlstr = "select * from pile_offline_records_t where  user_card='" + cardid + "' and exch_id=2";

            MySqlDataAdapter sms_da = new MySqlDataAdapter(sms_sqlstr, Framework.Environment.SMS_CONN);
            DataSet sms_ds = new DataSet();
            sms_da.Fill(sms_ds, "T");

            double retval = 0;
            if (sms_ds.Tables[0].Rows.Count > 0)
            {
                retval = Convert.ToDouble(sms_ds.Tables[0].Rows[0]["spend_money"].ToString());
            }
            return retval;

        }
        private void btnReadCardNumber_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxCardSerialNumber.Text = "";
                textBoxCardID.Value = "";
                labelRetRead.Text = "";
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
                        buttonUnfrozen.Enabled = checkBoxFrozen.Checked;
                        labelLastUncommitMoney.Text = info.bLockCard?GetLastUnCommitMoney(info.cardId).ToString("0.##"):"";
                        m_LastUncommitMoney = info.bLockCard?GetLastUnCommitMoney(info.cardId):0;
                        textBoxCardID.Value = info.cardId;
                        maskedTextBoxAdv1.Value = info.cardId;
                        buttonChangePass.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        labelRetRead.Text = "错误：" + ex.Message;
                        labelRetRead.ForeColor = Color.Red;

                    }

                }
                else
                {
                    labelRetRead.Text = "未开卡，请先开卡";
                    labelRetRead.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                labelRetRead.Text = "错误：" + ex.Message;
                labelRetRead.ForeColor = Color.Red;
            }

        }

        private void checkBoxFrozen_CheckedChanged(object sender, EventArgs e)
        {
        }


        private void textBoxCardID_TextChanged(object sender, EventArgs e)
        {
            GetUserInfo(textBoxCardID.Value, Convert.ToInt32(textBoxWalletMoney.Value * 100));
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
                float oldmoney = float.Parse(sms_ds.Tables[0].Rows[0]["elec_pkg_balance"].ToString());
                string phycardid = sms_ds.Tables[0].Rows[0]["phy_card"].ToString();
                string usercardid = sms_ds.Tables[0].Rows[0]["user_card_id"].ToString();
                if (money.ToString() != Convert.ToInt32(oldmoney * 100).ToString())
                {
                    string temp_sqlstr = "update user_card_list_t set elec_pkg_balance = '" + (money / 100f) + "' where  card_state=1 and user_card_id='" + cardid + "'";
                    MySqlCommand sms_comm = new MySqlCommand(temp_sqlstr, Framework.Environment.SMS_CONN);
                    sms_comm.Connection.Open();
                    try
                    {
                        sms_comm.ExecuteNonQuery();
                        labelRetRead.Text = "更新最新金额成功";
                        labelRetRead.ForeColor = Color.Blue;
                        sms_sqlstr = "INSERT INTO `money_change_info_t` (`phy_card`,`user_card_id`, `elec_pkg_balance`,`change_money`, `time`, `manager_id`, `manager_name`,`type`) "
                            + "VALUES ('" + phycardid + "', '" + usercardid + "', '" + (money / 100f) + "', '0', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + Framework.Environment.UserID + "', '" + Framework.Environment.UserName + "', '" + (int)DataModel.E_MONEY_CHANGE_TYPE.差额调整 + "')";

                        sms_comm.CommandText = sms_sqlstr;
                        sms_comm.ExecuteNonQuery();

                    }
                    catch (MySqlException)
                    {
                        labelRetRead.Text = "更新最新金额失败";
                        labelRetRead.ForeColor = Color.Red;

                    }
                    sms_comm.Connection.Close();

                }
            }


        }
        private void buttonUnfrozen_Click(object sender, EventArgs e)
        {
            if (textBoxCardID.Value == "")
            {
                labelRet.Text = "用户卡号不能为空";
                labelRet.ForeColor = Color.Red;
                return;
            }
            if (textBoxCardSerialNumber.Text == "")
            {
                labelRet.Text = "物理卡号不能为空，请读卡获取";
                labelRet.ForeColor = Color.Red;
                return;
            }
            if (textBoxWalletMoney.Value < m_LastUncommitMoney)
            { 
                labelRet.Text = "卡内余额不足，请先圈存";
                labelRet.ForeColor = Color.Red;
                return;
            }
            try
            {
                UnLockRecharge(m_LastUncommitMoney);
                RFIDREAD.RFIDReader.UnLock();
                btnReadCardNumber_Click(null, null);
                labelRet.Text = "解冻卡成功";
                labelRet.ForeColor = Color.Blue;
            }
            catch (Exception ex)
            {
                labelRet.Text = "解冻卡错误：" + ex.Message;
                labelRet.ForeColor = Color.Red;
            }
        }
        private void UnLockRecharge(double money)
        {
            decimal oldcashWallet = Convert.ToDecimal(textBoxWalletMoney.Value);
            decimal subcash = Convert.ToDecimal(money);

            decimal newcashwallet = oldcashWallet - subcash;
            //if (newcash < 0)
            //{
            //    labelRet.Text = "余额不足，请先充值";
            //    labelRet.ForeColor = Color.Red;

            //    return;
            //}

            try
            {
                RFIDREAD.RFIDReader.Charge(Convert.ToInt32(subcash * 100));

                labelRet.Text = "扣费成功";
                labelRet.ForeColor = Color.Blue;
                textBoxWalletMoney.Value = Convert.ToDouble(newcashwallet);
                m_LastUncommitMoney = 0;
                string userid = Framework.Environment.UserID.ToString();
                string username = Framework.Environment.UserName;
                string sms_sqlstr = "";
                sms_sqlstr = "INSERT INTO `money_change_info_t` (`phy_card`,`user_card_id`,`elec_pkg_balance`, `account_balance`,`change_money`, `time`, `manager_id`, `manager_name`,`type`) "
    + "VALUES ('" + textBoxCardSerialNumber.Text + "', '" + textBoxCardID.Value + "', '" + newcashwallet + "', '" + 0 + "', '" + subcash + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + userid + "', '" + username + "', '" + (int)DataModel.E_MONEY_CHANGE_TYPE.钱包扣费 + "')";

                MySqlCommand sms_comm = new MySqlCommand(sms_sqlstr, Framework.Environment.SMS_CONN);
                sms_comm.Connection.Open();
                try
                {
                    sms_comm.ExecuteNonQuery();
                    labelRet.Text = "扣费成功";
                    labelRet.ForeColor = Color.Blue;
                    sms_sqlstr = "update pile_offline_records_t set exch_id = 3 where  user_card='" + textBoxCardID.Value + "' and exch_id=2";
                    sms_comm.CommandText = sms_sqlstr;
                    sms_comm.ExecuteNonQuery();

                }
                catch (MySqlException)
                {
                    labelRet.Text = "扣费失败";
                    labelRet.ForeColor = Color.Red;
                    newcashwallet = 0;
                }
                sms_comm.Connection.Close();

            }
            catch (Exception ex)
            {
                labelRet.Text = "扣费错误：" + ex.Message;
                labelRet.ForeColor = Color.Red;
            }
        }

        public void InitWnd()
        {
            textBoxCardID.Value = "";
            textBoxCardSerialNumber.Text = "";
            textBoxMoney.Value = 0;
            textBoxUserName.Text = "";
            textBoxWalletMoney.Value = 0;
            maskedTextBoxAdv1.Value = "";
            buttonUnfrozen.Enabled = false;
            m_LastUncommitMoney = 0;
            labelRet.Text = "";
            buttonChangePass.Enabled = false;
        }

        private void buttonChangePass_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxCardID.Value))
            {
                formChangeCardPass form = new formChangeCardPass();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    labelRet.Text = "密码修改成功";
                    labelRet.ForeColor = Color.Blue;

                }
            }
        }

    }
}
