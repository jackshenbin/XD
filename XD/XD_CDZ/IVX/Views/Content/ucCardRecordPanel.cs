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

    public partial class ucCardRecordPanel : UserControl
    {
        BindingSource bs;
        Timer timerFlash = new Timer();


        public event EventHandler AddUserComplete;
        public ucCardRecordPanel()
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
            dateTimeInput1.Value = DateTime.Today.AddDays(1).AddMonths(-1).AddSeconds(-1);
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


        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sms_sqlstr2 = string.Format("SELECT * FROM money_change_info_t WHERE time BETWEEN '{0}' AND '{1}' and user_card_id = '{2}';"
                , dateTimeInput1.Value.ToString("yyyy-MM-dd HH:mm:ss")
                , dateTimeInput2.Value.ToString("yyyy-MM-dd HH:mm:ss")
                , TextBoxCardSearch.Value);
            MySqlDataAdapter sms_da2 = new MySqlDataAdapter(sms_sqlstr2, Framework.Environment.SMS_CONN);
            DataSet sms_ds2 = new DataSet();
            sms_da2.Fill(sms_ds2, "T");

            DataTable tRole = new DataTable("tRole");
            tRole.Columns.Add("r_id");
            tRole.Columns.Add("r_type");

            foreach (var item in Enum.GetValues(typeof(DataModel.E_MONEY_CHANGE_TYPE)))
            {
                tRole.Rows.Add((int)item, item.ToString());
            }
            //tRole.Rows.Add((int)DataModel.E_MONEY_CHANGE_TYPE.充值, DataModel.E_MONEY_CHANGE_TYPE.充值.ToString());
            //tRole.Rows.Add((int)DataModel.E_MONEY_CHANGE_TYPE.扣费, DataModel.E_MONEY_CHANGE_TYPE.扣费.ToString());
            //tRole.Rows.Add((int)DataModel.E_MONEY_CHANGE_TYPE.差额调整, DataModel.E_MONEY_CHANGE_TYPE.差额调整.ToString());
            //tRole.Rows.Add((int)DataModel.E_MONEY_CHANGE_TYPE.充电使用, DataModel.E_MONEY_CHANGE_TYPE.充电使用.ToString());
            //tRole.Rows.Add((int)DataModel.E_MONEY_CHANGE_TYPE.圈存, DataModel.E_MONEY_CHANGE_TYPE.圈存.ToString());
            //tRole.Rows.Add((int)DataModel.E_MONEY_CHANGE_TYPE.冲正, DataModel.E_MONEY_CHANGE_TYPE.冲正.ToString());
            //tRole.Rows.Add((int)DataModel.E_MONEY_CHANGE_TYPE.钱包扣费, DataModel.E_MONEY_CHANGE_TYPE.钱包扣费.ToString());

            sms_ds2.Tables.Add(tRole);

            Column8.DisplayMember = "r_type";
            Column8.ValueMember = "r_id";
            Column8.DataSource = new BindingSource(sms_ds2, "tRole");
            bs = new BindingSource(sms_ds2, "T");
            dataGridViewX1.DataSource = bs;

        }


        private void btnReadCardNumber_Click(object sender, EventArgs e)
        {
            try
            {
                TextBoxCardSearch.Value = "";
                labelRet.Text = "";

                bool isold = RFIDREAD.RFIDReader.IsOldCard();
                if (isold)
                {
                    try
                    {
                        RFIDREAD.CardInfo info = RFIDREAD.RFIDReader.ReadCardInfo();
                        TextBoxCardSearch.Value = info.cardId; ;
                    }
                    catch (Exception ex)
                    {
                        labelRet.Text = "错误：" + ex.Message;
                        labelRet.ForeColor = Color.Red;

                    }

                }
                else
                {
                    labelRet.Text = "已注销卡";
                    labelRet.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                labelRet.Text = "错误：" + ex.Message;
                labelRet.ForeColor = Color.Red;
            }

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Excel files (*.xls)|*.xls";
            dlg.FilterIndex = 0;
            dlg.RestoreDirectory = true;
            dlg.CreatePrompt = true;
            dlg.FileName = null;
            dlg.Title = "选择保存文件的路径";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Common.ExcelExport.ReportToExcel(dataGridViewX1, dlg.FileName);
                //string file = Common.ExcelExport.ExportToCVS(dataGridViewX1);
                //if (!string.IsNullOrEmpty(file))
                //    System.IO.File.WriteAllText(dlg.FileName, file,Encoding.GetEncoding("gbk"));
            }
        }

        private void dataGridViewX1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }

    
}
