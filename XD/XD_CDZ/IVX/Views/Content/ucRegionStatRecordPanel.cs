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

    public partial class ucRegionStatRecordPanel : UserControl
    {
        BindingSource bs;
        Timer timerFlash = new Timer();
        DataSet sms_ds;


        public event EventHandler AddUserComplete;
        public ucRegionStatRecordPanel()
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
        }

        private void GetDevTreeNodeByParent(int pid,ref List<int> nodelist)
        {
            string strsql = "select * from dev_tree_t where parent_id = "+pid;
            MySqlDataAdapter sms_da_pile = new MySqlDataAdapter(strsql, Framework.Environment.SMS_CONN);
            DataSet sms_ds_pile = new DataSet();
            sms_da_pile.Fill(sms_ds_pile, "T");
            if (sms_ds_pile.Tables[0].Rows.Count == 0)
            { nodelist.Add(pid); }
            else
            {
                foreach (DataRow item in sms_ds_pile.Tables[0].Rows)
                {
                    GetDevTreeNodeByParent(Convert.ToInt32( item["node_id"].ToString()),ref nodelist);
                }
            }
        }

        public void SetSelectdDevice(DataRow row)
        {
            if (row != null)
            {

                FillDevData(row);
                DrawChart(row);
            }
        }

        private void FillDevData(DataRow r)
        {
            List<int> list = new List<int>();
            GetDevTreeNodeByParent(Convert.ToInt32(r["node_id"].ToString()), ref list);
            string strnodelist = "";
            list.ForEach(item => strnodelist += item+",");
            strnodelist = strnodelist.TrimEnd(',');
            string sms_sqlstr2 = "SELECT  *, case IFNULL((select work_state from pile_state_t where  pile_state_t.dev_id = hd_pile_info_t.dev_id ORDER BY pile_state_t.date_time desc LIMIT 1 ),0) when 0 then '离线' when 1 then '故障' when 2 then '待机' when 3 then '工作' end  as work_state FROM hd_pile_info_t  where node_id in (" + strnodelist + ")";
            //string sms_sqlstr2 = "SELECT * FROM hd_pile_info_t where node_id=" + r["node_id"].ToString();
            MySqlDataAdapter sms_da_pile = new MySqlDataAdapter(sms_sqlstr2, Framework.Environment.SMS_CONN);
            DataSet sms_ds_pile = new DataSet();
            sms_da_pile.Fill(sms_ds_pile, "T");

            bs = new BindingSource(sms_ds_pile, "T");
            dataGridViewX1.DataSource = bs;

        }
        private void DrawChart(DataRow r)
        {
            List<int> list = new List<int>();
            GetDevTreeNodeByParent(Convert.ToInt32(r["node_id"].ToString()), ref list);
            string strnodelist = "";
            list.ForEach(item => strnodelist += item + ",");
            strnodelist = strnodelist.TrimEnd(',');

            string strformat = "select case work_state when 0 then '离线' when 1 then '故障' when 2 then '待机' when 3 then '工作' end as work_stat,count(dev_id) as devcount from (SELECT IFNULL((select work_state from pile_state_t where  pile_state_t.dev_id = hd_pile_info_t.dev_id ORDER BY pile_state_t.date_time desc LIMIT 1 ),0)  as work_state, hd_pile_info_t.dev_id FROM hd_pile_info_t  where node_id in (" + strnodelist + ")) as t group by work_state";
            //string strformat = "SELECT sum(total_pwr) as totalpwr,sum(spend_money) as spendmoney,month(start_time) as currmonth FROM pile_offline_records_t WHERE start_time BETWEEN '"
            //    + dateTimeInput1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + dateTimeInput2.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            //if (!string.IsNullOrWhiteSpace(TextBoxDevidSearch.Value)) strformat += " AND dev_id = '" + TextBoxDevidSearch.Value + "'";
            //if (!string.IsNullOrWhiteSpace(TextBoxCardSearch.Value)) strformat += " AND user_card = '" + TextBoxCardSearch.Value + "'";
            //strformat += "group by month(start_time)";

            MySqlDataAdapter sms_dav = new MySqlDataAdapter(strformat, Framework.Environment.SMS_CONN);
            DataSet sms_dsv = new DataSet();
            sms_dav.Fill(sms_dsv, "T");

            ////第二步：绑定一个数据源
            //if (sms_dsv.Tables[0].Rows.Count > 0)
            //    Chartlet1.BindChartData(sms_dsv);
            chart1.DataSource = sms_dsv;
            chart1.Series["Series1"].XValueMember = "work_stat";
            chart1.Series["Series1"].YValueMembers = "devcount";
           

            chart1.DataBind();



        }




        private void FillDevice(DevComponents.AdvTree.Node parentNode)
        {
            if (parentNode == null)
            {
                foreach (DataRow row in sms_ds.Tables[0].Rows)
                {
                    int level = int.Parse(row["level"].ToString());
                    if (level == 1)
                    {
                        DevComponents.AdvTree.Node n = new DevComponents.AdvTree.Node(row["node_name"].ToString());
                        n.ImageIndex = 4;
                        n.Tag = row;
                        n.NodeClick += n_NodeClick;
                        FillDevice(n);

                        advTree1.Nodes.Add(n);

                    }
                }
            }
            else
            {
                DataRow r = parentNode.Tag as DataRow;
                if (r != null)
                {
                    int level = int.Parse(r["level"].ToString());
                    if (level == 4)
                    {
                        //string sms_sqlstr2 = "SELECT * FROM hd_pile_info_t where node_id=" + r["node_id"].ToString();
                        //MySqlDataAdapter sms_da_pile = new MySqlDataAdapter(sms_sqlstr2, Framework.Environment.SMS_CONN);
                        //DataSet sms_ds_pile = new DataSet();
                        //sms_da_pile.Fill(sms_ds_pile, "T");

                        //foreach (DataRow item in sms_ds_pile.Tables[0].Rows)
                        //{
                        //    DevComponents.AdvTree.Node n = new DevComponents.AdvTree.Node(item["dev_id"].ToString());
                        //    n.Tag = item;
                        //    //n.Value = item["dev_id"].ToString();
                        //    if (item["pile_type"].ToString() == "2")
                        //        n.ImageIndex = 1;
                        //    else if (item["pile_type"].ToString() == "3")
                        //        n.ImageIndex = 2;
                        //    else
                        //        n.ImageIndex = 0;

                        //    n.NodeClick += n_NodeClick;
                        //    parentNode.Nodes.Add(n);

                        //}

                    }
                    else
                    {
                        foreach (DataRow item in sms_ds.Tables[0].Rows)
                        {
                            if (r["node_id"].ToString() == item["parent_id"].ToString())
                            {
                                DevComponents.AdvTree.Node n = new DevComponents.AdvTree.Node(item["node_name"].ToString());
                                if (level == 3)
                                    n.ImageIndex = 6;
                                else
                                    n.ImageIndex = 5;
                                n.Tag = item;
                                n.NodeClick += n_NodeClick;
                                FillDevice(n);
                                parentNode.Nodes.Add(n);
                            }
                        }
                    }
                }
            }
        }
        void n_NodeClick(object sender, EventArgs e)
        {

            DevComponents.AdvTree.Node n = sender as DevComponents.AdvTree.Node;
            if (n != null)
            {
                DataRow row = n.Tag as DataRow;
                if (row != null)
                {
                    SetSelectdDevice(row);
                }
            }
        }

        public void InitWnd()
        {
            DevTreeInit();


        }

        private void DevTreeInit()
        {
            string sms_sqlstr2 = "SELECT * FROM dev_tree_t";
            MySqlDataAdapter sms_da = new MySqlDataAdapter(sms_sqlstr2, Framework.Environment.SMS_CONN);
            sms_ds = new DataSet();
            sms_da.Fill(sms_ds, "T");

            advTree1.Nodes.Clear();
            FillDevice(null);
        }


    }
}
