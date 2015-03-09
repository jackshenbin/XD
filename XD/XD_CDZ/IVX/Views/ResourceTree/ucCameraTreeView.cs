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
    public partial class ucCameraTreeView : DevExpress.XtraEditors.XtraUserControl
    {

        public bool ShowTitle
        {
            get
            {
                return this.ucResourceTreeViewBase1.ShowTitle;
            }
            set
            {
                this.ucResourceTreeViewBase1.ShowTitle = value;
            }
        }

        public bool MultiSelect
        {
            get
            {
                return ucResourceTreeViewBase1.MultiSelect;
            }
            set
            {
                ucResourceTreeViewBase1.MultiSelect = value;
            }
        }

        public List<object> SelectedObjects
        {
            get
            {
                return ucResourceTreeViewBase1.ViewModelbase.SelectedObjects;
            }
        }

        public ucCameraTreeView()
        {
            InitializeComponent();
        }

        private void ucCameraTreeView_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                ucResourceTreeViewBase1.ViewModelbase = new VideoPictureTreeViewByTaskModelBase(TreeShowType.Camera);
                ucResourceTreeViewBase1.InitRootFolders();
            }

        }
    }
}
