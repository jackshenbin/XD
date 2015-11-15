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

    public partial class ucCDZRuntimeStatPanel : UserControl
    {
        Timer timerFlash = new Timer();


        public event EventHandler AddUserComplete;
        public ucCDZRuntimeStatPanel()
        {
            InitializeComponent();
            
            timerFlash.Interval = 10 * 1000;
            timerFlash.Stop();
            timerFlash.Tick += timerFlash_Tick;
            //Framework.Container.Instance.DevStateService.OnSetParamRet += DevStateService_OnSetParamRet;
            //Framework.Container.Instance.DevStateService.OnGetParamRet += DevStateService_OnGetParamRet;
            //Framework.Container.Instance.DevStateService.OnGetChargePriceRet += DevStateService_OnGetChargePriceRet;
            //Framework.Container.Instance.DevStateService.OnSetChargePriceRet += DevStateService_OnSetChargePriceRet;
        }



        void timerFlash_Tick(object sender, EventArgs e)
        {
        }

        private void ucCDZStatPanel_Load(object sender, EventArgs e)
        {
            DataSet sms_ds = new DataSet();
            DataTable tRoleServiceStat = new DataTable("tRoleServiceStat");
            tRoleServiceStat.Columns.Add("r_id");
            tRoleServiceStat.Columns.Add("r_type");

            tRoleServiceStat.Rows.Add(0, "未知状态");
            tRoleServiceStat.Rows.Add(1, "服务状态");
            tRoleServiceStat.Rows.Add(2, "暂停服务");
            tRoleServiceStat.Rows.Add(3, "维护状态");
            tRoleServiceStat.Rows.Add(4, "测试状态");

            sms_ds.Tables.Add(tRoleServiceStat);

            ServiceStat.DisplayMember = "r_type";
            ServiceStat.ValueMember = "r_id";
            ServiceStat.DataSource = new BindingSource(sms_ds, "tRoleServiceStat");


            DataTable tRoleWorkStat = new DataTable("tRoleWorkStat");
            tRoleWorkStat.Columns.Add("r_id");
            tRoleWorkStat.Columns.Add("r_type");

            tRoleWorkStat.Rows.Add(0, "离线");
            tRoleWorkStat.Rows.Add(1, "故障");
            tRoleWorkStat.Rows.Add(2, "待机");
            tRoleWorkStat.Rows.Add(3, "工作");

            sms_ds.Tables.Add(tRoleWorkStat);

            WorkStat.DisplayMember = "r_type";
            WorkStat.ValueMember = "r_id";
            WorkStat.DataSource = new BindingSource(sms_ds, "tRoleWorkStat");


            DataTable tRoleDevType = new DataTable("tRoleDevType");
            tRoleDevType.Columns.Add("r_id");
            tRoleDevType.Columns.Add("r_type");

            tRoleDevType.Rows.Add(0, "未知类型");
            tRoleDevType.Rows.Add(2, "单相交流离散桩");
            tRoleDevType.Rows.Add(18, "三相相交流离散桩");
            tRoleDevType.Rows.Add(3, "三相直流离散桩");
            tRoleDevType.Rows.Add(19, "单相直流离散桩");

            sms_ds.Tables.Add(tRoleDevType);

            DevType.DisplayMember = "r_type";
            DevType.ValueMember = "r_id";
            DevType.DataSource = new BindingSource(sms_ds, "tRoleDevType");

            DataTable tLianjie = new DataTable("tLianjie");
            tLianjie.Columns.Add("r_id");
            tLianjie.Columns.Add("r_type");

            tLianjie.Rows.Add(0, "未连");
            tLianjie.Rows.Add(1, "连接");
            tLianjie.Rows.Add(2, "可充");

            sms_ds.Tables.Add(tLianjie);

            LianJieQueRenKaiGuanZhuangTai.DisplayMember = "r_type";
            LianJieQueRenKaiGuanZhuangTai.ValueMember = "r_id";
            LianJieQueRenKaiGuanZhuangTai.DataSource = new BindingSource(sms_ds, "tLianjie");

        }



        public void InitWnd()
        {
            dataGridViewX1.DataSource = Framework.Container.Instance.DevStateService.DevStatTable;

        }

        private void buttonFlash_Click(object sender, EventArgs e)
        {
            dataGridViewX1.Refresh();
            //dataGridViewX1.DataSource = Framework.Container.Instance.DevStateService.CDZList;

        }



        private void dataGridViewX1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataGridViewX1_SelectionChanged(object sender, EventArgs e)
        {
        }


        private void dataGridViewX1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string Devid = dataGridViewX1["DevID", e.RowIndex].Value.ToString();

                formDevSetting dev = new formDevSetting(Devid);
                dev.ShowDialog();
            }
        }

        private void groupPanel1_Click(object sender, EventArgs e)
        {

        }

        private void btnChangeDevSetting_Click(object sender, EventArgs e)
        {
            if (dataGridViewX1.SelectedRows.Count == 1)
            {
                string Devid = dataGridViewX1.SelectedRows[0].Cells["DevID"].Value.ToString();

                formDevSetting dev = new formDevSetting(Devid);
                dev.ShowDialog();

            }
            else if (dataGridViewX1.SelectedRows.Count > 1)
            { 
            }

        }


    }
}
