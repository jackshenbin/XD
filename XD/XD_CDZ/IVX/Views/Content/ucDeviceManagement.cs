﻿using System;
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
    public partial class ucDeviceManagement : UserControl
    {
        BindingSource bs;
        DataSet sms_ds;

        public ucDeviceManagement()
        {
            InitializeComponent(); 

        }
        private void UnCheckAllButton()
        {
            buttonDelDevice.Checked = false;
            btnAddDevice.Checked = false;
            btnRealtimeStat.Checked = false;
            btnSearchDeviceStat.Checked = false;

        }

        private void btnAddDevice_Click(object sender, EventArgs e)
        {
            UnCheckAllButton();
            btnAddDevice.Checked = true;
            superTabControl1.SelectedTab = superTabItem1;
            ucAddCDZPanel1.InitWnd();
        }

        private void btnDeviceStat_Click(object sender, EventArgs e)
        {
            UnCheckAllButton();
            btnSearchDeviceStat.Checked = true;
            superTabControl1.SelectedTab = superTabItem2;
            ucCDZStatPanel1.InitWnd();

        }

        private void ucDeviceManagement_Load(object sender, EventArgs e)
        {
            superTabControlPanel0.BackgroundImage = Framework.Environment.DefaultImage;
            ucAddCDZPanel1.AddDevComplete += ucAddCDZPanel1_AddDevComplete;
            ucDelCDZPanel1.DelDevComplete += ucDelCDZPanel1_DelDevComplete;
            Framework.Container.Instance.DevStateService.OnDevStateChanged+=DevStateService_OnDevStateChanged;// += DevStateService_OnSetServiceState;
        }

        void DevStateService_OnDevStateChanged(object sender, EventArgs e)
        {
            string devid = sender.ToString();
            BOCOM.DataModel.CDZDevStatusInfo info = Framework.Container.Instance.DevStateService.GetDevByID(devid);
            if (info != null)
            {
                UpdateDevServiceStatic(devid, info.WorkStat);
            }
        }

        private void UpdateDevServiceStatic(string devid, uint workstatic)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string, uint>(UpdateDevServiceStatic), devid, workstatic);
            }
            else
            {
                switch (workstatic)
                {
                    case 0://离线，
                        advTree1.FindNodeByCellText(devid).ImageIndex = 10;
                        break;
                    case 1://故障
                        advTree1.FindNodeByCellText(devid).ImageIndex = 9;
                        break;

                    case 2://待机，
                        advTree1.FindNodeByCellText(devid).ImageIndex = 8;
                        break;

                    case 3://工作
                        advTree1.FindNodeByCellText(devid).ImageIndex = 7;
                        break;

                    default:
                        advTree1.FindNodeByCellText(devid).ImageIndex = 10;
                        break;

                }

            }
        }

        void ucDelCDZPanel1_DelDevComplete(object sender, EventArgs e)
        {
            advTree1.Nodes.Clear();
            FillDevice(null);
        }

        void ucAddCDZPanel1_AddDevComplete(object sender, EventArgs e)
        {
            advTree1.Nodes.Clear();
            FillDevice(null);
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
                        string sms_sqlstr2 = "SELECT * FROM hd_pile_info_t where node_id="+r["node_id"].ToString();
                        MySqlDataAdapter sms_da_pile = new MySqlDataAdapter(sms_sqlstr2, Framework.Environment.SMS_CONN);
                        DataSet sms_ds_pile = new DataSet();
                        sms_da_pile.Fill(sms_ds_pile, "T");

                        foreach (DataRow item in sms_ds_pile.Tables[0].Rows)
                        {
                            DevComponents.AdvTree.Node n = new DevComponents.AdvTree.Node(item["dev_id"].ToString());
                            n.Tag = item;
                            string devid = item["dev_id"].ToString();
                            BOCOM.DataModel.CDZDevStatusInfo info = Framework.Container.Instance.DevStateService.GetDevByID(devid);
                            if (info != null)
                            {
                                switch (info.WorkStat)
                                {
                                    case 0://离线，
                                        n.ImageIndex = 10;
                                        break;
                                    case 1://故障
                                        n.ImageIndex = 9;
                                        break;

                                    case 2://待机，
                                        n.ImageIndex = 8;
                                        break;

                                    case 3://工作
                                        n.ImageIndex = 7;
                                        break;

                                    default:
                                        n.ImageIndex = 10;
                                        break;

                                }
                            }
                            else
                            {
                                n.ImageIndex = 10;

                            }

                            n.NodeClick += n_NodeClick;
                            parentNode.Nodes.Add(n);

                        }

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
            if (superTabControl1.SelectedTab != superTabItem2 && superTabControl1.SelectedTab != superTabItem3)
                superTabControl1.SelectedTab = superTabItem2;


            DevComponents.AdvTree.Node n = sender as DevComponents.AdvTree.Node;
            if (n != null)
            {
                DataRow row = n.Tag as DataRow;
                if (row != null)
                {
                    ucCDZStatPanel1.SetSelectdDevice(row);
                    ucDelCDZPanel1.SetSelectdDevice(row);
                }
            }
        }

        private void btnDelDevice_Click(object sender, EventArgs e)
        {
            UnCheckAllButton();
            buttonDelDevice.Checked = true;
            superTabControl1.SelectedTab = superTabItem3;
            ucDelCDZPanel1.InitWnd();

        }

        public void InitWnd()
        {
            DevTreeInit();

            ucCurrentUser1.InitWnd();
            superTabControl1.SelectedTab = superTabItem0;

            switch (Framework.Environment.UserType)
	        {
                case 1:
                    break;
                case 2:
                case 4:
                    buttonDelDevice.Enabled = false;
                    btnAddDevice.Enabled = false;
                    break;
                case 3:
                    break;
		        default:
                    break;
	        }

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

        private void btnRealtimeStat_Click(object sender, EventArgs e)
        {
            UnCheckAllButton();
            btnRealtimeStat.Checked = true;
            superTabControl1.SelectedTab = superTabItem4;
            ucCDZRuntimeStatPanel1.InitWnd();

        }


    }
}
