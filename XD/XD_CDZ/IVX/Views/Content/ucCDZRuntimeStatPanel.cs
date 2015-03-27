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
        }

        void timerFlash_Tick(object sender, EventArgs e)
        {
        }

        private void ucCDZStatPanel_Load(object sender, EventArgs e)
        {
        }



        public void InitWnd()
        {
            dataGridViewX1.DataSource = Framework.Container.Instance.DevStateService.CDZList;

        }

        private void buttonFlash_Click(object sender, EventArgs e)
        {
            dataGridViewX1.Refresh();
            //dataGridViewX1.DataSource = Framework.Container.Instance.DevStateService.CDZList;

        }

    }
}
