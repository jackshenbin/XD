using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MySql.Data.MySqlClient;

public partial class login : System.Web.UI.Page
{
    MySqlConnection sms_conn;
    public int PageCount, PageSize, RecordCount, CurrentPage;
    protected string Action = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        string sms_connstr = System.Configuration.ConfigurationManager.AppSettings["sms_dbconn"]; //建立连接
        sms_conn = new MySqlConnection(sms_connstr);

        Init_WebControls();
    }
    public void Init_WebControls()
    {
        try
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Action"]))//获取form的Action中的参数
            {
                Action = Request.QueryString["Action"].Trim().ToLower();//去掉空格并变小写
            }
            switch (Action)
            {
                case "logout":
                    Session["userid"] = "";
                    Session["user"] = "";
                    Session["type"] = "";
                    break;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    //protected void Button_logoin(object sender, EventArgs e)
    //{
    //    string str_pwd = this.mypassword.Text.Trim().Replace("'", "''");
    //    string sqlstr = "select * from user_manage_t where user_name='" + username.Text + "'and passwd=?password";
    //    MySqlCommand cmd = new MySqlCommand(sqlstr, sms_conn);
    //    cmd.Parameters.Add(new MySqlParameter("?password", MySqlDbType.VarChar, 50));
    //    cmd.Parameters["?password"].Value = str_pwd;
    //    sms_conn.Open();
    //    MySqlDataReader dr = cmd.ExecuteReader();
    //    if (dr.Read() == true)
    //    {
    //        Session["userid"] = dr["id"].ToString();
    //        Session["user"] = dr["user_name"].ToString();//管理员用户,Session进行传值
    //        Session["type"] = dr["user_auth"].ToString();
    //        FormsAuthentication.RedirectFromLoginPage(username.Text, false);
    //        sms_conn.Close();
    //    }
    //    else
    //    {
    //        sms_conn.Close();
    //        message.Text = "您必须输入有效的用户名和密码！";
    //    }
    //}
    
}
