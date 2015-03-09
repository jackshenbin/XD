using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BOCOM.IVX.Protocol;
using System.Runtime.InteropServices;
using System.Diagnostics;
using BOCOM.DataModel;

namespace BOCOM.IVX.Controls
{
    public partial class XtraUserControlTimeLine : DevExpress.XtraEditors.XtraUserControl
    {
        public XtraUserControlTimeLine()
        {
            InitializeComponent();
        }
        public new event EventHandler MouseDoubleClick;

        private void XtraUserControlTimeLine_Load(object sender, EventArgs e)
        {
            axVdaTimeLine1.InitTimeline();
        }

        public void SetTimeLineObject(List<IVAResultRecord> snapshots, int total)
        {
            
        }

        void UpdateLabel(string str)
        {
            if (InvokeRequired)
            {
                try
                {
                    this.Invoke(new Action<string> (UpdateLabel), new object[] { str });
                }
                catch (Exception ex)
                {
                    Framework.Container.Instance.Log.Error("Invoke or BeginInvoke error: ", ex);
                    Debug.Assert(false, ex.Message);
                }
            }
            else
            {
                labelControl1.Text = str;
            }
        }

        public void UpdateImage(Image  img)
        {
            if (InvokeRequired && IsHandleCreated)
            {
                try
                {
                    this.Invoke(new Action<Image>(UpdateImage), new object[] { img });
                }
                catch (Exception ex)
                {
                    Framework.Container.Instance.Log.Error("Invoke or BeginInvoke error: ", ex);
                    Debug.Assert(false, ex.Message);
                }
            }
            else
            {
                pictureBox1.Image = img;
            }
        }
        public int SetTimeLineCurrentTime(Int64 currTime)
        {

            UpdateLabel(new DateTime((long)currTime * 10 * 1000).ToString("HH:mm:ss.fff") + "/" + new DateTime((long)VideoTotleTime * 10 * 1000).ToString("HH:mm:ss.fff"));
            return axVdaTimeLine1.SetCurTime((int)currTime);
        }
        private long VideoTotleTime = 0;
        public int SetTimeLineTotleTime(Int64 totleTime)
        {
            int ret = 1;
            if (VideoTotleTime != totleTime)
            {
                ret = axVdaTimeLine1.SetTotalTime((int)totleTime);
                VideoTotleTime = totleTime;
            }
            return ret;
        }
        private void zoomTrackBarControl1_EditValueChanged(object sender, EventArgs e)
        {
            axVdaTimeLine1.ZoomTimeline(zoomTrackBarControl1.Value);
        }

        private void checkButton1_CheckedChanged(object sender, EventArgs e)
        {   
            zoomTrackBarControl1.Enabled = !switchButton1.Checked;
            axVdaTimeLine1.SetFitAllObj(switchButton1.Checked ? 1 : 0);
        }

        private void axVdaTimeLine1_MouseMoveEvent(object sender, AxVdaTimeLineLib._DVdaTimeLineEvents_MouseMoveEvent e)
        {
            int time = axVdaTimeLine1.GetCurMouseTime();
            if (time > 0)
                UpdateLabel(new DateTime((long)time* 10 * 1000).ToString("HH:mm:ss.fff"));
        }

        private void axVdaTimeLine1_DblClick(object sender, EventArgs e)
        {
        }
    }
}
