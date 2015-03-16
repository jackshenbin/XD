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

    public partial class ucWalletReChargePanel : UserControl
    {

        public ucWalletReChargePanel()
        {
            InitializeComponent();

        }
        bool ValidateAddMoney()
        {
            bool ret = true;
            if (textBoxCardID.Value == "")
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


            decimal oldcash = 0;
            decimal addcash = 0;
            try
            {
                oldcash = Convert.ToDecimal(textBoxWalletMoney.Value);
                addcash = Convert.ToDecimal(textBoxChargeMoney.Value);
                if (addcash < 0)
                {
                    labelRet.Text = " 圈钱金额错误";
                    labelRet.ForeColor = Color.Red;
                    ret = false;
                }
            }
            catch (Exception)
            {

                labelRet.Text = " 圈钱金额错误";
                labelRet.ForeColor = Color.Red;
                ret = false;
            }

            return ret;
        }

        bool ValidateDeductMoney()
        {
            bool ret = true;
            if (textBoxCardID.Value == "")
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


            decimal oldcash = 0;
            decimal deductcash = 0;
            try
            {
                oldcash = Convert.ToDecimal(textBoxWalletMoney.Value);
                deductcash = Convert.ToDecimal(textBoxChargeMoney.Value);
                if (deductcash < 0)
                {
                    labelRet.Text = " 充正金额错误";
                    labelRet.ForeColor = Color.Red;
                    ret = false;
                }
                else
                {
                    if (oldcash - deductcash < 0)
                    {
                        labelRet.Text = " 充正金额不足";
                        labelRet.ForeColor = Color.Red;
                        ret = false;
                    }
                }
            }
            catch (Exception)
            {

                labelRet.Text = " 扣费金额错误";
                labelRet.ForeColor = Color.Red;
                ret = false;
            }

            return ret;
        }

        private void btnReadCardNumber_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxCardSerialNumber.Text = "";
                textBoxCardID.Value = "";
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
                        textBoxCardID.Value = info.cardId; ;
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

        private void checkBoxFrozen_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void buttonUnfrozen_Click(object sender, EventArgs e)
        {
            if (textBoxCardID.Value == "")
            {
                labelRet.Text = "用户卡号不能为空";
                labelRet.ForeColor = Color.Red;
                return ;
            }
            if (textBoxCardSerialNumber.Text == "")
            {
                labelRet.Text = "物理卡号不能为空，请读卡获取";
                labelRet.ForeColor = Color.Red;
                return ;
            }


            try
            {
                RFIDREAD.RFIDReader.UnLock();
                btnReadCardNumber_Click(null, null);
            }
            catch (Exception ex)
            { 
                labelRet.Text = "解冻卡错误："+ex.Message;
                labelRet.ForeColor = Color.Red;
            }
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
                            + "VALUES ('" + phycardid + "', '" + usercardid + "', '" + (money / 100f) + "', '0', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + Framework.Environment.UserID + "', '" + Framework.Environment.UserName + "', '" + (int)DataModel.E_MONEY_CHANGE_TYPE.差额调整 + "')";

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

        //private void btnAddWallet_Click(object sender, EventArgs e)
        //{
        //    if (!ValidateAddMoney())
        //        return;

        //    decimal oldcashWallet = Convert.ToDecimal(textBoxWalletMoney.Value);
        //    decimal oldcash = Convert.ToDecimal(textBoxMoney.Value);
        //    decimal addcash = Convert.ToDecimal(textBoxChargeMoney.Value);

        //    decimal newcashwallet = oldcashWallet + addcash;
        //    decimal newcash = oldcash - addcash;
        //    if (newcash < 0)
        //    {
        //        labelRet.Text = "余额不足，请先充值";
        //        labelRet.ForeColor = Color.Red;

        //        return ;
        //    }
        //    try
        //    {
        //        RFIDREAD.RFIDReader.ReCharge(Convert.ToInt32(addcash * 100));

        //        string sms_sqlstr = "update user_card_list_t set elec_pkg_balance = " + newcashwallet.ToString() + " , account_balance = " + newcash.ToString() + " where card_state=1 and user_card_id='" + textBoxCardID.Value + "'";
        //        MySqlCommand sms_comm = new MySqlCommand(sms_sqlstr, Framework.Environment.SMS_CONN);
        //        sms_comm.Connection.Open();
        //        try
        //        {
        //            sms_comm.ExecuteNonQuery();
        //            labelRet.Text = "圈钱成功";
        //            labelRet.ForeColor = Color.Blue;
        //            textBoxMoney.Value = Convert.ToDouble(newcash);
        //            textBoxWalletMoney.Value = Convert.ToDouble(newcashwallet);
        //            textBoxChargeMoney.Value = 0;

        //            string userid = Framework.Environment.UserID.ToString();
        //            string username = Framework.Environment.UserName;
        //            sms_sqlstr = "INSERT INTO `money_change_info_t` (`phy_card`,`user_card_id`,`elec_pkg_balance`, `account_balance`,`change_money`, `time`, `manager_id`, `manager_name`,`type`) "
        //                + "VALUES ('" + textBoxCardSerialNumber.Text + "', '" + textBoxCardID.Value + "', '" + newcashwallet + "', '" + newcash + "', '" + addcash + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + userid + "', '" + username + "', '" + 5 + "')";

        //            sms_comm.CommandText = sms_sqlstr;
        //            sms_comm.ExecuteNonQuery();

        //        }
        //        catch (MySqlException)
        //        {
        //            labelRet.Text = "圈钱失败";
        //            labelRet.ForeColor = Color.Red;
        //            newcashwallet = 0;
        //        }
        //        sms_comm.Connection.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //            labelRet.Text = "圈钱错误："+ex.Message;
        //            labelRet.ForeColor = Color.Red;
        //    }

        //}

        private void btnSubWallet_Click(object sender, EventArgs e)
        {
            if (!ValidateDeductMoney())
                return;

            decimal oldcashWallet = Convert.ToDecimal(textBoxWalletMoney.Value);
            decimal subcash = Convert.ToDecimal(textBoxChargeMoney.Value);

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
                    textBoxChargeMoney.Value = 0;
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
                    labelRet.Text = "扣费错误："+ex.Message;
                    labelRet.ForeColor = Color.Red;
            }
        }

        private void ucWalletChargePanel_Load(object sender, EventArgs e)
        {

        }

        private void ucCardIDTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void InitWnd()
        {
            textBoxCardID.Value = "";
            textBoxCardSerialNumber.Text = "";
            textBoxChargeMoney.Value = 0;
            textBoxMoney.Value = 0;
            textBoxUserName.Text = "";
            textBoxWalletMoney.Value = 0;
            labelRet.Text = "";
        }

    }
}
