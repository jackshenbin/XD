using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using BOCOM.DataModel;

namespace BOCOM.IVX.Controls
{
    public partial class FormDownloadInfo : DevExpress.XtraEditors.XtraForm
    {
        public FormDownloadInfo()
        {
            InitializeComponent();
            tableDownloadInfoSource.Columns.Add("dstName", typeof(string));
            tableDownloadInfoSource.Columns.Add("type", typeof(string));
            tableDownloadInfoSource.Columns.Add("progress", typeof(string));
            tableDownloadInfoSource.Columns.Add("downloadPath", typeof(string));
            tableDownloadInfoSource.Columns.Add("srcVideoName", typeof(string));
            tableDownloadInfoSource.Columns.Add("hrItem", typeof(int));
            this.Width = 400;
            this.Height = 330;

        }

        DataTable tableDownloadInfoSource = new DataTable();
        private  void UpdateDownloadItemInfo()
        {

            //foreach (BOCOM.IVX.ViewModel.DownloadItemInfo info in Framework.Container.Instance.DownloadItemViewModel.GetAllDownloadItemInfo())
            //{
            //    string progress = "";
            //    if (info.progress < 0)
            //        progress = "下载失败";
            //    else if (info.progress == 100)
            //        progress = "下载完成";
            //    else
            //        progress = info.progress+"%";

            //    tableDownloadInfoSource.Rows.Add(new object[] { 
            //        info.dstName,
            //        Framework.Container.Instance.DownloadItemViewModel.GetDownloadTypeName(info.type),
            //        progress, 
            //        info.downloadPath,
            //        info.srcVideoName,
            //        info.hrItem 
            //    });

            //}
            gridControl1.DataSource = tableDownloadInfoSource;
            UpdateSummary();
        }

        private void FormDownloadInfo_Load(object sender, EventArgs e)
        {
            UpdateDownloadItemInfo();

        }


        public void UpdateDownloadProgress(int hrItem, int progress)
        {
            if (InvokeRequired)
            {
                try
                {
                    this.Invoke(new Action<int, int>(UpdateDownloadProgress), new object[] { hrItem, progress });
                    return;
                }
                catch
                { 
                }
            }

            DataRow[] rows= tableDownloadInfoSource.Select("hrItem=" + hrItem);
            if (rows != null && rows.Length > 0)
            {
                string p = "";
                if (progress < 0)
                    p = "下载失败";
                else if (progress == 100)
                    p = "下载完成";
                else
                    p = progress + "%";

                rows[0]["progress"] = p;
            }
            else
            {
                //BOCOM.IVX.ViewModel.DownloadItemInfo info = Framework.Container.Instance.DownloadItemViewModel.GetDownloadItemInfoById(hrItem);
                //if (info != null)
                //{
                //    string p = "";
                //    if (info.progress < 0)
                //        p = "下载失败";
                //    else if (info.progress == 100)
                //        p = "下载完成";
                //    else
                //        p = info.progress + "%";

                //    tableDownloadInfoSource.Rows.Add(new object[] { 
                //        info.dstName,
                //        Framework.Container.Instance.DownloadItemViewModel.GetDownloadTypeName(info.type),
                //        p, 
                //        info.downloadPath,
                //        info.srcVideoName,
                //        info.hrItem 
                //        });
                //}

            }
            UpdateSummary();

        }

        private void UpdateSummary()
        {
            labelControl1.Text = "共 " + tableDownloadInfoSource.Rows.Count + " 个";
            DataRow[] rs = tableDownloadInfoSource.Select("type='" + DownloadType.结果图片.ToString() + "'");
            string temp = DownloadType.结果图片.ToString() + ":" + rs.Length + Environment.NewLine;
            rs = tableDownloadInfoSource.Select("type='" + DownloadType.浓缩导出.ToString() + "'");
            temp += DownloadType.浓缩导出.ToString() + ":" + rs.Length + Environment.NewLine;
            rs = tableDownloadInfoSource.Select("type='" + DownloadType.视频剪辑.ToString() + "'");
            temp += DownloadType.视频剪辑.ToString() + ":" + rs.Length + Environment.NewLine;
            rs = tableDownloadInfoSource.Select("type='" + DownloadType.视频截图.ToString() + "'");
            temp += DownloadType.视频截图.ToString() + ":" + rs.Length;
            labelControl1.ToolTip = temp;
        }
        private void FormDownloadInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            gridColumndownloadPath.Visible = gridColumnsrcVideoName.Visible = checkEdit1.Checked;
            if (!checkEdit1.Checked)
            {
                this.Width = 400;
                this.Height = 330;
            }
            else
            { 
                this.Width = 700;
                this.Height = 450;
            }
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            System.Drawing.Point p = gridControl1.PointToClient(Control.MousePosition);
            GridHitInfo hitInfo = gridView1.CalcHitInfo(p);

            if (hitInfo.InRow || hitInfo.InRowCell)
            {
                object obj = gridView1.GetRow(hitInfo.RowHandle);
                if (obj != null)
                {
                    DataRowView row = obj as DataRowView;
                    System.Diagnostics.Process.Start("Explorer.exe", "/select," +
                     row["downloadPath"].ToString() +"\\"+ row["dstName"].ToString());
                }
            }

        }


    }
}
