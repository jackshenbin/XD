using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BOCOM.IVX.ViewModel;
using DevExpress.XtraTreeList.Nodes;
using BOCOM.DataModel;

namespace BOCOM.IVX.Views.ResourceTree
{
    public partial class ucBrivfCameraTreeView : DevExpress.XtraEditors.XtraUserControl
    {

        public ucBrivfCameraTreeView()
        {
            InitializeComponent();

        }

        private void ucBrivfCameraTreeView_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                ucResourceTreeViewBase1.ViewModelbase = new VideoPictureTreeViewByTaskModelBase(TreeShowType.Video,TreeShowObjectFilter.Brief );
                ucResourceTreeViewBase1.InitRootFolders();
            }

        }


    }
}
