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
    public partial class ucCaseTreeView : DevExpress.XtraEditors.XtraUserControl, IEventAggregatorSubscriber
    {
        public ucCaseTreeView()
        {
            InitializeComponent();
            treeList2.ExpandAll();
            navBarItemCurrCase.Enabled = false;
            
            this.navBarItemMyCaseList.Tag = UIFuncItemInfo.MYCASELIST;
            this.navBarItemNewCase.Tag = UIFuncItemInfo.NEWCASE;
            this.navBarItemCurrCase.Tag = UIFuncItemInfo.CURRCASE;

            Framework.Container.Instance.EvtAggregator.GetEvent<EnterCaseEvent>().Subscribe(OnEnteringCase);
            Framework.Container.Instance.EvtAggregator.GetEvent<LeaveCaseEvent>().Subscribe(OnLeaveCase);

            Framework.Container.Instance.RegisterEventSubscriber(this);

        }

        private void ItemClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (e.Link.Item.Tag != null && e.Link.Item.Tag is UIFuncItemInfo)
            {
                UIFuncItemInfo func = (UIFuncItemInfo)e.Link.Item.Tag;
                Framework.Container.Instance.EvtAggregator.GetEvent<NavigateEvent>().Publish(func);
            }
        }

        public void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<EnterCaseEvent>().Unsubscribe(OnEnteringCase);
            Framework.Container.Instance.EvtAggregator.GetEvent<LeaveCaseEvent>().Unsubscribe(OnLeaveCase);
        }

        private void OnEnteringCase(string caseName)
        {
            this.navBarItemCurrCase.Enabled = true;
        }

        private void OnLeaveCase(string caseName)
        {
            this.navBarItemCurrCase.Enabled = false ;
        }

    }
}
