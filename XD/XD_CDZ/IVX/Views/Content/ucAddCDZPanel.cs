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

    public partial class ucAddCDZPanel : UserControl
    {

        ucGroupSelect group = new ucGroupSelect();

        public event EventHandler AddDevComplete;
        public ucAddCDZPanel()
        {
            InitializeComponent();
            this.Controls.Add(group);
            group.GroupSelected+=group_GroupSelected;
            group.Visible = false;

        }
        bool ValidateAddDevice()
        {

            bool ret = true;

            if (TextBoxDevid.Text == "")
            {
                labelRet.Text = "充电桩编号不能为空";
                labelRet.ForeColor = Color.Red;
                ret = false;
            }
            return ret;
        }

        private bool ShowInfo()
        {
            string msg = string.Format("充电桩编号：{0}"+Environment.NewLine
                +"充电桩类型：{1}"+Environment.NewLine
                +"区域：{2}"+Environment.NewLine
                +"厂商编号：{3}"+Environment.NewLine
                +"软件版本号：{4}"+Environment.NewLine
                +"SIM卡号：{5}"+Environment.NewLine
                +"安装地址：{6}"+Environment.NewLine
                +"备注：{7}"+Environment.NewLine
                ,TextBoxDevid.Text
                ,comboBoxType.SelectedItem.ToString()
                ,buttonGroup.Text//,comboBoxExProvince.SelectedItem.ToString()+comboBoxExCity.SelectedItem.ToString()+comboBoxExDistrict.SelectedItem.ToString()+comboBoxExCounty.SelectedItem.ToString()
                ,TextBoxsFactoryid.Text
                ,TextBoxsVersion.Text
                ,TextBoxsSIM.Text
                ,TextBoxsAddr.Text
                ,TextBoxsPosition.Text);
            return (MessageBox.Show(msg, "确认充电桩注册数据", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK) ? true : false;
        }

        private void buttonAddDevice_Click(object sender, EventArgs e)
        {
            if (!ValidateAddDevice())
                return;

            if (!ShowInfo())
                return;

            string sms_sqlstr = string.Format("INSERT INTO `hd_pile_info_t` VALUES ('{0}', '{1}', '{2}', '{3}','{4}', '{5}', '{6}', '{7}','{8}','{9}')"
                , TextBoxDevid.Text
                , TextBoxUserid.Text
                , TextBoxsFactoryid.Text
                , TextBoxsVersion.Text
                , comboBoxType.SelectedIndex+1
                , TextBoxsSIM.Text
                , TextBoxsAddr.Text
                , TextBoxsPosition.Text
                , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                , buttonGroup.Tag.ToString());
            MySqlCommand sms_comm = new MySqlCommand(sms_sqlstr, Framework.Environment.SMS_CONN);
            sms_comm.Connection.Open();
            try
            {
                sms_comm.ExecuteNonQuery();
                labelRet.Text = "添加充电桩成功";
                labelRet.ForeColor = Color.Blue;

            }
            catch (MySqlException)
            {
                labelRet.Text = "添加充电桩写入数据库失败";
                labelRet.ForeColor = Color.Red;
            }
            sms_comm.Connection.Close();

            if (AddDevComplete != null)
                AddDevComplete(null, null);
        }

        private void ucAddCDZPanel_Load(object sender, EventArgs e)
        {
            comboBoxType.SelectedIndex = 1;
            buttonGroup.Tag = "0";
        }

        private void buttonGroup_Click(object sender, EventArgs e)
        {
            group.Location = buttonGroup.Location;
            group.Init();
            group.Visible = true;
            group.BringToFront();
        }

        void group_GroupSelected(object sender, EventArgs e)
        {
            buttonGroup.Tag = group.GroupID;
            buttonGroup.Text = group.GroupTreeName.Trim('.') ;
            SizeF size = buttonGroup.CreateGraphics().MeasureString(buttonGroup.Text, DefaultFont);
            buttonGroup.Width = (int)size.Width+40;
            labelX12.Left = buttonGroup.Left + buttonGroup.Width + 5;
            group.Visible = false;
        }

        private void ucAddCDZPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if(group.Visible)
                group.Visible = false;

        }



    }
}
