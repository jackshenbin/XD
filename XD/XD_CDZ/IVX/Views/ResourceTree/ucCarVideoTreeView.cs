using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BOCOM.DataModel;
using DevExpress.XtraBars;
using BOCOM.IVX.Framework;
using BOCOM.IVX.ViewModel;
using DevExpress.XtraTreeList.Nodes;

namespace BOCOM.IVX.Views.ResourceTree
{
    public partial class ucCarVideoTreeView : DevExpress.XtraEditors.XtraUserControl
    {
        public ucCarVideoTreeView()
        {
            InitializeComponent();
        }

        private void ucCarVideoTreeView_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                ucResourceTreeViewBase1.ViewModelbase = new VideoPictureTreeViewByTaskModelBase(TreeShowType.Video,TreeShowObjectFilter.Car );
                ucResourceTreeViewBase1.InitRootFolders();
            }
        }
        public void UpdateCheckedStat()
        {
            ucResourceTreeViewBase1.ViewModelbase.UpdateCheckedResources(null, false);
        }



    }
}
