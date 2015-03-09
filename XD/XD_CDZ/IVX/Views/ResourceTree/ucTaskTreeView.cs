using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BOCOM.DataModel;
using BOCOM.IVX.Framework;

namespace BOCOM.IVX.Views.ResourceTree
{
    public partial class ucTaskTreeView : DevExpress.XtraEditors.XtraUserControl
    {
        public ucTaskTreeView()
        {
            InitializeComponent();
            treeList2.ExpandAll();
            this.navBarItemNewTask.Tag = UIFuncItemInfo.NEWTASK;
            this.navBarItemTaskStatus.Tag = UIFuncItemInfo.TASKSTATUS;

        }
        private void ItemClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (e.Link.Item.Tag != null && e.Link.Item.Tag is UIFuncItemInfo)
            {
                UIFuncItemInfo func = (UIFuncItemInfo)e.Link.Item.Tag;
                Framework.Container.Instance.EvtAggregator.GetEvent<NavigateEvent>().Publish(func);
            }
        }


    }
}
