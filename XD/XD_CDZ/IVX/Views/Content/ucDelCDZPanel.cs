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

    public partial class ucDelCDZPanel : UserControl
    {


        public event EventHandler DelDevComplete;
        public ucDelCDZPanel()
        {
            InitializeComponent();

        }

        private void ucCDZStatPanel_Load(object sender, EventArgs e)
        {
            //FillDevice();
        }
        bool ValidateDelDevice()
        {

            bool ret = true;

            if (TextBoxDevid.Value == "")
            {
                labelRet.Text = "充电桩编号不能为空";
                labelRet.ForeColor = Color.Red;
                ret = false;
            }
            return ret;
        }

        public void SetSelectdDevice(DataRow row)
        {
            labelSearchStat.Text = "";

            TextBoxDevid.Value = "";
            TextBoxUserid.Text = "";
            TextBoxsFactoryid.Text = "";
            TextBoxsVersion.Text = "";
            comboBoxType.SelectedIndex = 0;
            TextBoxsSIM.Text = "";
            TextBoxsAddr.Text = "";
            TextBoxsPosition.Text = "";
            TextBoxsOutvol.Text = "";
            TextBoxsOutcur.Text = "";
            comboBoxRelaystat.SelectedIndex = 0;
            comboBoxConnstat.SelectedIndex = 0;
            TextBoxsTotaldegree.Text = "";
            comboBoxBattery.SelectedIndex = 0;
            comboBoxWorkstat.SelectedIndex = 0;


            if (row != null)
            {
                TextBoxDevid.Value = row["dev_id"].ToString();
                TextBoxUserid.Text = row["user_id"].ToString();
                TextBoxsFactoryid.Text = row["vender_id"].ToString();
                TextBoxsVersion.Text = row["software_ver"].ToString();
                comboBoxType.SelectedIndex = int.Parse(row["pile_type"].ToString()) - 1;
                TextBoxsSIM.Text = row["sim_id"].ToString();
                TextBoxsAddr.Text = row["address"].ToString();
                TextBoxsPosition.Text = row["position"].ToString();

                string sms_sqlstr2 = "SELECT * FROM pile_state_t where dev_id='" + TextBoxDevid.Value + "' order by date_time desc limit 1";
                MySqlDataAdapter sms_da2 = new MySqlDataAdapter(sms_sqlstr2, Framework.Environment.SMS_CONN);
                DataSet sms_ds2 = new DataSet();
                sms_da2.Fill(sms_ds2, "T");
                if (sms_ds2.Tables[0].Rows.Count > 0)
                {
                    DataRow r = sms_ds2.Tables[0].Rows[0];
                    TextBoxsOutvol.Text = r["output_vol"].ToString();
                    TextBoxsOutcur.Text = r["output_cur"].ToString();
                    comboBoxRelaystat.SelectedIndex = int.Parse(r["relay_state"].ToString());
                    comboBoxConnstat.SelectedIndex = int.Parse(r["conn_state"].ToString());
                    TextBoxsTotaldegree.Text = r["total_degree"].ToString();
                    comboBoxBattery.SelectedIndex = int.Parse(r["battery"].ToString());
                    comboBoxWorkstat.SelectedIndex = int.Parse(r["work_state"].ToString());


                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            string sms_sqlstr2 = "SELECT * FROM hd_pile_info_t where dev_id='" + TextBoxDevidSearch.Value + "'";
            MySqlDataAdapter sms_da2 = new MySqlDataAdapter(sms_sqlstr2, Framework.Environment.SMS_CONN);
            DataSet sms_ds2 = new DataSet();
            sms_da2.Fill(sms_ds2, "T");
            if (sms_ds2.Tables[0].Rows.Count > 0)
            {
                DataRow r = sms_ds2.Tables[0].Rows[0];
                SetSelectdDevice(r);
            }
            else
            {
                labelSearchStat.Text = "无此桩号记录";
            }

        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (!ValidateDelDevice())
                return;


            string msg = string.Format("是否真的要注销充电桩？" + Environment.NewLine
                + "充电桩编号：{0}" + Environment.NewLine
                + "充电桩桩类型：{1}" + Environment.NewLine
                + "工作状态：{2}" + Environment.NewLine
                + "厂商编号：{3}" + Environment.NewLine
                + "软件版本号：{4}" + Environment.NewLine
                + "SIM卡号：{5}" + Environment.NewLine
                + "安装地址：{6}" + Environment.NewLine
                , TextBoxDevid.Value
                , comboBoxType.SelectedItem.ToString()
                , comboBoxWorkstat.SelectedItem.ToString()
                , TextBoxsFactoryid.Text
                , TextBoxsVersion.Text
                , TextBoxsSIM.Text
                , TextBoxsAddr.Text);
            if (MessageBox.Show(msg, "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2) != DialogResult.OK)
                return;

            string sms_sqlstr = "delete FROM hd_pile_info_t where dev_id='" + TextBoxDevid.Value + "'";

            MySqlCommand sms_comm = new MySqlCommand(sms_sqlstr, Framework.Environment.SMS_CONN);
            sms_comm.Connection.Open();
            try
            {
                sms_comm.ExecuteNonQuery();
                labelRet.Text = "注销充电桩成功";
                labelRet.ForeColor = Color.Blue;
                Framework.Container.Instance.DevStateService.DeleteDev(TextBoxDevid.Value);
            }
            catch (MySqlException)
            {
                labelRet.Text = "注销充电桩写入数据库失败";
                labelRet.ForeColor = Color.Red;
            }
            sms_comm.Connection.Close();

            if (DelDevComplete != null)
                DelDevComplete(null, null);
            
        }


        public void InitWnd()
        { 
            TextBoxDevid.Value = "";
            comboBoxType.SelectedIndex = 0;
            comboBoxWorkstat.SelectedIndex = 0;
            TextBoxsFactoryid.Text = "";
            TextBoxsVersion.Text= "";
            TextBoxsSIM.Text="";
            TextBoxsAddr.Text = "";
            TextBoxDevid1.Text = "";
            TextBoxDevidSearch.Value = "";
            TextBoxsOutcur.Text = "";
            TextBoxsOutvol.Text = "";
            TextBoxsPosition.Text = "";
            TextBoxsTotaldegree.Text = "";
            TextBoxsVersion.Text = "";
            TextBoxUserid.Text = "";
            labelRet.Text = "";

        }
    }
}
