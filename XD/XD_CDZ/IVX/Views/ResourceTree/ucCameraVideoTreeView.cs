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

namespace BOCOM.IVX.Views.ResourceTree
{
    public partial class ucCameraVideoTreeView : DevExpress.XtraEditors.XtraUserControl
    {

        public ucCameraVideoTreeView()
        {
            InitializeComponent();
            // barManager1.SetPopupContextMenu(this.treeList2, this.popupMenu1);
            treeList2.ExpandAll();
        }


    }
}
