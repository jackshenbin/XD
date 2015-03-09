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

    public partial class ucCDZStatPanel : UserControl
    {
        BindingSource bs;
        Timer timerFlash = new Timer();


        public event EventHandler AddUserComplete;
        public ucCDZStatPanel()
        {
            InitializeComponent();
            
            timerFlash.Interval = 10 * 1000;
            timerFlash.Stop();
            timerFlash.Tick += timerFlash_Tick;
        }

        void timerFlash_Tick(object sender, EventArgs e)
        {
            DrawChart(TextBoxDevid.Text);
        }

        private void ucCDZStatPanel_Load(object sender, EventArgs e)
        {
            //FillDevice();
            dateTimeInput1.Value = DateTime.Today.AddSeconds(-1);
            dateTimeInput2.Value = DateTime.Today.AddDays(1).AddSeconds(-1) ;
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
            labelSearchStat.Text = "";
            timerFlash.Stop();

            TextBoxDevid.Text = "";
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
            DrawChart("");


            if (row != null)
            {
                TextBoxDevid.Text = row["dev_id"].ToString();
                TextBoxUserid.Text = row["user_id"].ToString();
                TextBoxsFactoryid.Text = row["vender_id"].ToString();
                TextBoxsVersion.Text = row["software_ver"].ToString();
                comboBoxType.SelectedIndex = int.Parse(row["pile_type"].ToString()) - 1;
                TextBoxsSIM.Text = row["sim_id"].ToString();
                TextBoxsAddr.Text = row["address"].ToString();
                TextBoxsPosition.Text = row["position"].ToString();

                string sms_sqlstr2 = "SELECT * FROM pile_state_t where dev_id='" + TextBoxDevid.Text + "' order by date_time desc limit 1";
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

        private void DrawChart(string id)
        {
            string axissize_sqlstr = string.Format("SELECT (case when max(output_vol) is null then 250 else max(output_vol) end ) as maxval,(case when min(output_vol) is null then 230 else min(output_vol) end) as minval FROM pile_state_t "
                + " WHERE dev_id = '{0}' and date_time  BETWEEN '{1}' and '{2}'"
                , id, dateTimeInput1.Value.ToString("yyyy-MM-dd HH:mm:ss"), dateTimeInput2.Value.ToString("yyyy-MM-dd HH:mm:ss"));

            MySqlDataAdapter sms_daaxis = new MySqlDataAdapter(axissize_sqlstr, Framework.Environment.SMS_CONN);
            DataSet sms_dsaxis = new DataSet();
            sms_daaxis.Fill(sms_dsaxis, "T");

            chart1.ChartAreas[0].AxisY.Maximum = double.Parse(sms_dsaxis.Tables[0].Rows[0]["maxval"].ToString());
            chart1.ChartAreas[0].AxisY.Minimum = double.Parse(sms_dsaxis.Tables[0].Rows[0]["minval"].ToString());

            string sms_sqlstr = string.Format("SELECT DATE_FORMAT( date_time, '%Y-%m-%d %H:%i:%s' ) AS time, output_vol FROM pile_state_t "
                + " WHERE dev_id = '{0}' and date_time  BETWEEN '{1}' and '{2}'"
                , id, dateTimeInput1.Value.ToString("yyyy-MM-dd HH:mm:ss"), dateTimeInput2.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                //string sms_sqlstr = "select DATE_FORMAT(date_time,'%Y-%m-%d %H:%i:%s') as time,output_vol  from pile_state_t where dev_id='" + id + "' order by date_time limit 100";
                MySqlDataAdapter sms_dav = new MySqlDataAdapter(sms_sqlstr, Framework.Environment.SMS_CONN);
                DataSet sms_dsv = new DataSet();
                sms_dav.Fill(sms_dsv, "T");

                ////第二步：绑定一个数据源
                //if (sms_dsv.Tables[0].Rows.Count > 0)
                //    Chartlet1.BindChartData(sms_dsv);
                chart1.DataSource = sms_dsv;
                chart1.Series["Series1"].XValueMember = "time";
                chart1.Series["Series1"].YValueMembers = "output_vol";
                chart1.DataBind();

                sms_sqlstr = string.Format("SELECT DATE_FORMAT( date_time, '%Y-%m-%d %H:%i:%s' ) AS time, output_cur FROM pile_state_t "
        + " WHERE dev_id = '{0}' and date_time  BETWEEN '{1}' and '{2}' "
        , id, dateTimeInput1.Value.ToString("yyyy-MM-dd HH:mm:ss"), dateTimeInput2.Value.ToString("yyyy-MM-dd HH:mm:ss"));

                //sms_sqlstr = "select DATE_FORMAT(date_time,'%Y-%m-%d %H:%i:%s') as time,output_cur  from pile_state_t where dev_id=" + id + " order by date_time limit 100";
                MySqlDataAdapter sms_daa = new MySqlDataAdapter(sms_sqlstr, Framework.Environment.SMS_CONN);
                DataSet sms_dsa = new DataSet();
                sms_daa.Fill(sms_dsa, "T");

                ////第二步：绑定一个数据源
                //if (sms_dsa.Tables[0].Rows.Count > 0)
                //    Chartlet2.BindChartData(sms_dsa);

                chart2.DataSource = sms_dsa;
                chart2.Series["Series1"].XValueMember = "time";
                chart2.Series["Series1"].YValueMembers = "output_cur";
                chart2.DataBind();



                sms_sqlstr = string.Format("SELECT DATE_FORMAT( date_time, '%Y-%m-%d %H:%i:%s' ) AS time, total_degree FROM pile_state_t "
        + " WHERE dev_id = '{0}' and date_time  BETWEEN '{1}' and '{2}'"
        , id, dateTimeInput1.Value.ToString("yyyy-MM-dd HH:mm:ss"), dateTimeInput2.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                //sms_sqlstr = "select DATE_FORMAT(date_time,'%Y-%m-%d %H:%i:%s') as time,total_degree  from pile_state_t where dev_id=" + id + " order by date_time limit 100";
                MySqlDataAdapter sms_da_total_degree = new MySqlDataAdapter(sms_sqlstr, Framework.Environment.SMS_CONN);
                DataSet sms_ds_total_degree = new DataSet();
                sms_da_total_degree.Fill(sms_ds_total_degree, "T");

                ////第二步：绑定一个数据源
                //if (sms_dsa.Tables[0].Rows.Count > 0)
                //    Chartlet2.BindChartData(sms_dsa);

                chart3.DataSource = sms_ds_total_degree;
                chart3.Series["Series1"].XValueMember = "time";
                chart3.Series["Series1"].YValueMembers = "total_degree";
                chart3.DataBind();


                sms_sqlstr = string.Format("SELECT DATE_FORMAT( date_time, '%Y-%m-%d %H:%i:%s' ) AS time, work_state FROM pile_state_t "
                    + " WHERE dev_id = '{0}' and date_time  BETWEEN '{1}' and '{2}'"
                    , id, dateTimeInput1.Value.ToString("yyyy-MM-dd HH:mm:ss"), dateTimeInput2.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                //sms_sqlstr = "select DATE_FORMAT(date_time,'%Y-%m-%d %H:%i:%s') as time,work_state  from pile_state_t where dev_id=" + id + " order by date_time limit 100";
                MySqlDataAdapter sms_da_work_state = new MySqlDataAdapter(sms_sqlstr, Framework.Environment.SMS_CONN);
                DataSet sms_ds_work_state = new DataSet();
                sms_da_work_state.Fill(sms_ds_work_state, "T");

                ////第二步：绑定一个数据源
                //if (sms_dsa.Tables[0].Rows.Count > 0)
                //    Chartlet2.BindChartData(sms_dsa);

                chart4.DataSource = sms_ds_work_state;
                chart4.Series["Series1"].XValueMember = "time";
                chart4.Series["Series1"].YValueMembers = "work_state";
                chart4.DataBind();


                sms_sqlstr = string.Format("SELECT DATE_FORMAT( date_time, '%Y-%m-%d %H:%i:%s' ) AS time, conn_state FROM pile_state_t "
                    + " WHERE dev_id = '{0}' and date_time  BETWEEN '{1}' and '{2}'"
                    , id, dateTimeInput1.Value.ToString("yyyy-MM-dd HH:mm:ss"), dateTimeInput2.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                //sms_sqlstr = "select DATE_FORMAT(date_time,'%Y-%m-%d %H:%i:%s') as time,work_state  from pile_state_t where dev_id=" + id + " order by date_time limit 100";
                MySqlDataAdapter sms_da_conn_state = new MySqlDataAdapter(sms_sqlstr, Framework.Environment.SMS_CONN);
                DataSet sms_ds_conn_state = new DataSet();
                sms_da_conn_state.Fill(sms_ds_conn_state, "T");

                ////第二步：绑定一个数据源
                //if (sms_dsa.Tables[0].Rows.Count > 0)
                //    Chartlet2.BindChartData(sms_dsa);

                chart5.DataSource = sms_ds_conn_state;
                chart5.Series["Series1"].XValueMember = "time";
                chart5.Series["Series1"].YValueMembers = "conn_state";
                chart5.DataBind();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            string sms_sqlstr2 = "SELECT * FROM hd_pile_info_t where dev_id='" + TextBoxDevidSearch.Text+"'";
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
                labelSearchStat.Text = "无此充电桩记录";
            }

        }

        private void buttonSearchGraph_Click(object sender, EventArgs e)
        {
            DrawChart(TextBoxDevid.Text);
        }

        private void checkBoxFlash_CheckedChanged(object sender, EventArgs e)
        {
            timerFlash.Enabled = checkBoxFlash.Checked;
        }

    }
}
