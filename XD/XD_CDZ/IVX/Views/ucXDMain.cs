using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BOCOM.DataModel;
using MySql.Data.MySqlClient;

namespace BOCOM.IVX.Views
{
    public partial class ucXDMain : UserControl
    {
        public ucXDMain()
        {
            InitializeComponent();
            ucMainPage1.SwitchWnd += ucMainPage1_SwitchWnd;
        }

        void ucMainPage1_SwitchWnd(object sender, EventArgs e)
        {
            SwitchWnd((E_SWITCH_WND)sender);
        }


        public bool SwitchWnd(E_SWITCH_WND wnd)
        {
            if(Framework.Environment.UserID<=0)
                return false;


            switch (wnd)
            {
                case E_SWITCH_WND.SWITCH_WND_MAIN_PAGE:
                    superTabControl1.SelectedPanel = superTabControlPanel0;
                    break;
                case E_SWITCH_WND.SWITCH_WND_USER_MENAGEMENT:
                    superTabControl1.SelectedPanel = superTabControlPanel1;
                    break;
                case E_SWITCH_WND.SWITCH_WND_CARD_MENAGEMENT:
                    superTabControl1.SelectedPanel = superTabControlPanel2;
                    break;
                case E_SWITCH_WND.SWITCH_WND_DEVICE_MENAGEMENT:
                    superTabControl1.SelectedPanel = superTabControlPanel3;
                    break;
                case E_SWITCH_WND.SWITCH_WND_REPORT_MENAGEMENT:
                    superTabControl1.SelectedPanel = superTabControlPanel4;
                    break;
                default:
                    break;
            }

            return true;
        }


        public bool LoginWnd(string serverIP,string username, string password)
        {
            Framework.Environment.ServerIP = serverIP;
            Framework.Environment.UserName = username;
            string str_pwd = password;
            string sqlstr = "select * from user_manage_t where user_name='" + username + "'and passwd=?password";
            MySqlCommand cmd = new MySqlCommand(sqlstr, Framework.Environment.SMS_CONN);
            cmd.Parameters.Add(new MySqlParameter("?password", MySqlDbType.VarChar, 50));
            cmd.Parameters["?password"].Value = str_pwd;
            cmd.Connection.Open();

            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read() == true)
            {

                Framework.Environment.UserID =int.Parse(dr["id"].ToString());
                Framework.Environment.UserName =dr["user_name"].ToString();
                Framework.Environment.Password = password;
                Framework.Environment.UserType = int.Parse(dr["user_auth"].ToString());
                cmd.Connection.Close();
                
                Framework.Environment.ReadXDConfig();
                return true;
            }
            else
            {
                cmd.Connection.Close();

                throw new Exception( "您必须输入有效的用户名和密码！");
            }
        }


        public void InitWnd()
        {
            ucCardManagement1.InitWnd();
            ucDeviceManagement1.InitWnd();
            ucMainPage1.InitWnd();
            ucReportMenagement1.InitWnd();
            ucUserConfig1.InitWnd();
        }
    }
}
