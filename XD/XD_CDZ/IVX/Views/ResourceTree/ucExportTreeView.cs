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
    public partial class ucExportTreeView : DevExpress.XtraEditors.XtraUserControl
    {
        public ucExportTreeView()
        {
            InitializeComponent();

            this.navBarItemCaseExport.Tag = UIFuncItemInfo.CASEEXPORT;

            this.navBarItemTagExport.Tag = UIFuncItemInfo.TAGEXPORT;

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
