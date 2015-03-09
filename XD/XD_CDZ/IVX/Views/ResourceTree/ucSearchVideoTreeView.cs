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
    public partial class ucSearchVideoTreeView : DevExpress.XtraEditors.XtraUserControl
    {
        private SearchVideoTreeViewModel m_viewModel;

        public ucSearchVideoTreeView()
        {
            InitializeComponent();
        }


        public void SetShowType(SearchType filter)
        {
            switch (filter)
            {
                case SearchType.Normal:
                    ucObjectVideoTreeView1.BringToFront();
                    ucObjectVideoTreeView1.UpdateCheckedStat();
                    break;
                case SearchType.Vehicle:
                    ucCarVideoTreeView1.BringToFront();
                    ucCarVideoTreeView1.UpdateCheckedStat();
                    break;
                case SearchType.Face:
                    ucFaceVideoTreeView1.BringToFront();
                    ucFaceVideoTreeView1.UpdateCheckedStat();
                    break;
                case SearchType.Compare:
                    ucObjectVideoTreeView1.BringToFront();
                    ucObjectVideoTreeView1.UpdateCheckedStat();
                    break;
            }
        }

        private void ucSearchVideoTreeView_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                m_viewModel = new SearchVideoTreeViewModel(SearchType.Normal);
                m_viewModel.PropertyChanged += new PropertyChangedEventHandler(m_viewModel_PropertyChanged);
                
            }
        }

        void m_viewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Filter")
            {
                SetShowType(m_viewModel.Filter);
            }
        }
    }
}
