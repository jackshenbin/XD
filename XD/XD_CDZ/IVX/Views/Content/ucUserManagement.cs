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
    public partial class ucUserManagement : UserControl
    {
        string sms_sqlstr = "";
        BindingSource bs;
        public ucUserManagement()
        {
            InitializeComponent();
        }
        public void UserListInit()
        {
            //sms_sqlstr = "select id,user_name,`passwd`,if(user_auth=1,'管理员',if(user_auth=2,'发卡员',if(user_auth=3,'操作员',''))) as type,reg_time from user_manage_t";
            sms_sqlstr = "select id,user_name,`passwd`,CONVERT(user_auth,char(1)) as user_auth,reg_time from user_manage_t";
            MySqlDataAdapter sms_da = new MySqlDataAdapter(sms_sqlstr, Framework.Environment.SMS_CONN);
            DataSet sms_ds = new DataSet();
            sms_da.Fill(sms_ds, "T");
            DataTable tRole = new DataTable("tRole");
            tRole.Columns.Add("r_id");
            tRole.Columns.Add("r_type");

            tRole.Rows.Add(1, "管理员");
            tRole.Rows.Add(2, "发卡员");
            tRole.Rows.Add(3, "操作员");
            tRole.Rows.Add(4, "报表查看");
            sms_ds.Tables.Add(tRole);

            Column4.DisplayMember = "r_type";
            Column4.ValueMember = "r_id";
            Column4.DataSource = new BindingSource(sms_ds,"tRole");
            bs = new BindingSource(sms_ds, "T");
            dataGridViewX1.DataSource = bs;
            //dataGridViewX1.Columns[0].Visible = false;
            //dataGridViewX1.Columns[1].HeaderText = "用户名";
            //dataGridViewX1.Columns[2].HeaderText = "密码";
            //dataGridViewX1.Columns[3].HeaderText = "角色";
            //dataGridViewX1.Columns[3].DataPropertyName = "";
            //dataGridViewX1.Columns[4].HeaderText = "注册时间";


            bindingNavigatorEx1.BindingSource = bs;

        }


        private void btnAddUser_Click(object sender, EventArgs e)
        {
            superTabControl1.SelectedPanel = superTabControlPanel1;
        }

        private void btnChangePass_Click(object sender, EventArgs e)
        {
            superTabControl1.SelectedPanel = superTabControlPanel2;

        }

        private void btnUserInfo_Click(object sender, EventArgs e)
        {
            superTabControl1.SelectedPanel = superTabControlPanel3;
        }

        private void ucUserManagement_Load(object sender, EventArgs e)
        {
            superTabControlPanel0.BackgroundImage = Framework.Environment.DefaultImage;
        }
        public void InitWnd()
        {
            ucCurrentUser1.InitWnd();

            superTabControl1.SelectedPanel = superTabControlPanel0;
            switch (Framework.Environment.UserType)
            {
                case 1:
                    UserListInit();
                    break;
                case 2:
                case 3:
                case 4:
                    btnAddUser.Enabled = false;
                    break;
                default:
                    break;
            }

        }

        private void dataGridViewX1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

    }
}
