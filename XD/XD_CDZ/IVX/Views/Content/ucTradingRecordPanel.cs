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

    public partial class ucTradingRecordPanel : UserControl
    {
        BindingSource bs;
        Timer timerFlash = new Timer();


        public event EventHandler AddUserComplete;
        public ucTradingRecordPanel()
        {
            InitializeComponent();
            
            timerFlash.Interval = 10 * 1000;
            timerFlash.Stop();
            timerFlash.Tick += timerFlash_Tick;
        }

        void timerFlash_Tick(object sender, EventArgs e)
        {
            //DrawChart(TextBoxDevid.Text);
        }

        private void ucCDZStatPanel_Load(object sender, EventArgs e)
        {
            //FillDevice();
            dateTimeInput1.Value = DateTime.Today.AddSeconds(-1); ;
            dateTimeInput2.Value = DateTime.Today.AddDays(1).AddSeconds(-1);
        }

        //private void FillDevice()
        //{
        //    string sms_sqlstr2 = "SELECT * FROM hd_pile_info_t";
        //    MySqlDataAdapter sms_da = new MySqlDataAdapter(sms_sqlstr2, sms_conn);
        //    DataSet sms_ds = new DataSet();
        //    sms_da.Fill(sms_ds, "T");

        //    advTree1.Nodes.Clear();
        //    foreach (DataRow item in sms_ds.Tables[0].Rows)
        //    {
        //        DevComponents.AdvTree.Node n = new DevComponents.AdvTree.Node(item["user_id"].ToString());
        //        n.Tag = item;
        //        //n.Value = item["dev_id"].ToString();
        //        if (item["pile_type"].ToString() == "2")
        //            n.ImageIndex = 1;
        //        else if (item["pile_type"].ToString() == "3")
        //            n.ImageIndex = 2;
        //        else
        //            n.ImageIndex = 0;

        //        n.NodeClick += n_NodeClick;
        //        advTree1.Nodes.Add(n);

        //    }
        //}

        public void SetSelectdDevice(DataRow row)
        {
            //labelSearchStat.Text = "";
            //timerFlash.Stop();

            //TextBoxDevid.Text = "";
            //TextBoxUserid.Text = "";
            //TextBoxsFactoryid.Text = "";
            //TextBoxsVersion.Text = "";
            //comboBoxType.SelectedIndex = 0;
            //TextBoxsSIM.Text = "";
            //TextBoxsAddr.Text = "";
            //TextBoxsPosition.Text = "";
            //TextBoxsOutvol.Text = "";
            //TextBoxsOutcur.Text = "";
            //comboBoxRelaystat.SelectedIndex = 0;
            //comboBoxConnstat.SelectedIndex = 0;
            //TextBoxsTotaldegree.Text = "";
            //comboBoxBattery.SelectedIndex = 0;
            //comboBoxWorkstat.SelectedIndex = 0;


            //if (row != null)
            //{
            //    TextBoxDevid.Text = row["dev_id"].ToString();
            //    TextBoxUserid.Text = row["user_id"].ToString();
            //    TextBoxsFactoryid.Text = row["vender_id"].ToString();
            //    TextBoxsVersion.Text = row["software_ver"].ToString();
            //    comboBoxType.SelectedIndex = int.Parse(row["pile_type"].ToString()) - 1;
            //    TextBoxsSIM.Text = row["sim_id"].ToString();
            //    TextBoxsAddr.Text = row["address"].ToString();
            //    TextBoxsPosition.Text = row["position"].ToString();

            //    string sms_sqlstr2 = "SELECT * FROM pile_state_t where dev_id='" + TextBoxDevid.Text + "' order by date_time desc limit 1";
            //    MySqlDataAdapter sms_da2 = new MySqlDataAdapter(sms_sqlstr2, Framework.Environment.SMS_CONN);
            //    DataSet sms_ds2 = new DataSet();
            //    sms_da2.Fill(sms_ds2, "T");
            //    if (sms_ds2.Tables[0].Rows.Count > 0)
            //    {
            //        DataRow r = sms_ds2.Tables[0].Rows[0];
            //        TextBoxsOutvol.Text = r["output_vol"].ToString();
            //        TextBoxsOutcur.Text = r["output_cur"].ToString();
            //        comboBoxRelaystat.SelectedIndex = int.Parse(r["relay_state"].ToString());
            //        comboBoxConnstat.SelectedIndex = int.Parse(r["conn_state"].ToString());
            //        TextBoxsTotaldegree.Text = r["total_degree"].ToString();
            //        comboBoxBattery.SelectedIndex = int.Parse(r["battery"].ToString());
            //        comboBoxWorkstat.SelectedIndex = int.Parse(r["work_state"].ToString());


            //    }
            //}
        }

        private void DrawChart()
        {

            string strformat = "SELECT sum(total_pwr) as totalpwr,sum(spend_money) as spendmoney,month(start_time) as currmonth FROM pile_offline_records_t WHERE start_time BETWEEN '"
                + dateTimeInput1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + dateTimeInput2.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            if (!string.IsNullOrWhiteSpace(TextBoxDevidSearch.Value)) strformat += " AND dev_id = '" + TextBoxDevidSearch.Value + "'";
            if (!string.IsNullOrWhiteSpace(TextBoxCardSearch.Value)) strformat += " AND user_card = '" + TextBoxCardSearch.Value + "'";
            strformat += "group by month(start_time)";

            MySqlDataAdapter sms_dav = new MySqlDataAdapter(strformat, Framework.Environment.SMS_CONN);
                DataSet sms_dsv = new DataSet();
                sms_dav.Fill(sms_dsv, "T");

                ////第二步：绑定一个数据源
                //if (sms_dsv.Tables[0].Rows.Count > 0)
                //    Chartlet1.BindChartData(sms_dsv);
                chart1.DataSource = sms_dsv;
                chart1.Series["Series1"].XValueMember = "currmonth";
                chart1.Series["Series1"].YValueMembers = "totalpwr";
           
                chart1.Series["Series2"].XValueMember = "currmonth";
                chart1.Series["Series2"].YValueMembers = "spendmoney";
                chart1.DataBind();



        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            string strformat = "SELECT distinct dev_id,user_card,phy_card,start_time,end_time,total_pwr,spend_money,elec_wallet_ballance,serial_sn FROM pile_offline_records_t WHERE start_time BETWEEN '"
                +dateTimeInput1.Value.ToString("yyyy-MM-dd HH:mm:ss")+"' AND '"+dateTimeInput2.Value.ToString("yyyy-MM-dd HH:mm:ss")+"'";
            if(!string.IsNullOrWhiteSpace(TextBoxDevidSearch.Value)) strformat += " AND dev_id = '"+TextBoxDevidSearch.Value+"'";
            if(!string.IsNullOrWhiteSpace(TextBoxCardSearch.Value)) strformat+= " AND user_card = '"+TextBoxCardSearch.Value+"'";
//string sms_sqlstr2 = string.Format("SELECT * FROM pile_offline_records_t WHERE"
//           + "exch_time BETWEEN '{0}' AND '{1}' AND dev_id = '{2}' AND user_card = '{3}' AND phy_card = '{4}'";

//            string sms_sqlstr2 = "SELECT * FROM hd_pile_info_t where dev_id='" + TextBoxDevidSearch.Text+"'";
            MySqlDataAdapter sms_da2 = new MySqlDataAdapter(strformat, Framework.Environment.SMS_CONN);
            DataSet sms_ds2 = new DataSet();
            sms_da2.Fill(sms_ds2, "T");

            //DataTable tRole = new DataTable("tRole");
            //tRole.Columns.Add("r_id");
            //tRole.Columns.Add("r_type");

            //tRole.Rows.Add(1, "充值");
            //tRole.Rows.Add(2, "扣费");
            //tRole.Rows.Add(3, "差额调整");
            //tRole.Rows.Add(4, "充电使用");
            //tRole.Rows.Add(5, "圈钱");
            //tRole.Rows.Add(6, "充正");
            //sms_ds2.Tables.Add(tRole);

            //Column8.DisplayMember = "r_type";
            //Column8.ValueMember = "r_id";
            //Column8.DataSource = new BindingSource(sms_ds2, "tRole");
            bs = new BindingSource(sms_ds2, "T");
            dataGridViewX1.DataSource = bs;

        }

        private void buttonSearchGraph_Click(object sender, EventArgs e)
        {
            DrawChart();
        }

        private void checkBoxFlash_CheckedChanged(object sender, EventArgs e)
        {
            timerFlash.Enabled = checkBoxFlash.Checked;
        }

    }
}
