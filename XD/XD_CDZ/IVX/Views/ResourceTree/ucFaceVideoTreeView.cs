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
    public partial class ucFaceVideoTreeView : DevExpress.XtraEditors.XtraUserControl
    {

        public ucFaceVideoTreeView()
        {
            InitializeComponent();
        }

        private void ucFaceVideoTreeView_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                ucResourceTreeViewBase1.ViewModelbase = new VideoPictureTreeViewByTaskModelBase(TreeShowType.Video, TreeShowObjectFilter.Face );
                ucResourceTreeViewBase1.InitRootFolders();
            }
        }
        public void UpdateCheckedStat()
        {
            ucResourceTreeViewBase1.ViewModelbase.UpdateCheckedResources(null, false);
        }



    }
}
