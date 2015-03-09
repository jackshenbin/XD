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
    public partial class ucSysconfigTreeView : DevExpress.XtraEditors.XtraUserControl
    {
        public ucSysconfigTreeView()
        {
            InitializeComponent();
            this.navBarItemCameraManagement.Tag = UIFuncItemInfo.CAMERAMANAGEMENT;

            this.navBarItemPlatManagement.Tag = UIFuncItemInfo.PLATMANAGEMENT;
            this.navBarItemUserManagement.Tag = UIFuncItemInfo.USERMANAGEMENT;


            this.navBarItemClusterMonitor.Tag = UIFuncItemInfo.CLUSTERMONITOR;
            this.navBarItemMediaServer.Tag = UIFuncItemInfo.MEDIASERVERMANAGEMENT;
            this.navBarItemVDAServer.Tag = UIFuncItemInfo.VDASERVERMANAGEMENT;
            this.navBarItemVDAResultServer.Tag = UIFuncItemInfo.VDARESULTSERVERMANAGEMENT;
            this.navBarItemMediaRouter.Tag = UIFuncItemInfo.MEDIAROUTERMANAGEMENT;
            this.navBarItemClientRouter.Tag = UIFuncItemInfo.CLIENTROUTERMANAGEMENT;

            this.navBarItemLogManagement.Tag = UIFuncItemInfo.LOGMANAGEMENT;

            this.navBarItemPASServer.Tag = UIFuncItemInfo.PASSERVERMANAGEMENT;

            this.navBarItemFtpHttpServer.Tag = UIFuncItemInfo.FTPHTTPSERVERMANAGEMENT;
        }
        private void ItemClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (e.Link.Item.Tag != null && e.Link.Item.Tag is UIFuncItemInfo)
            {
                UIFuncItemInfo func = (UIFuncItemInfo)e.Link.Item.Tag;
                Framework.Container.Instance.EvtAggregator.GetEvent<NavigateEvent>().Publish(func);
            }
        }

        private void ucSysconfigTreeView_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                if (Framework.Container.Instance.AuthenticationService.GetLoginUserRole() == E_VDA_USER_ROLE_TYPE.E_ROLE_TYPE_LEADER)
                {
                    navBarGroup8.Visible = false;

                }
            }
        }



    }
}
