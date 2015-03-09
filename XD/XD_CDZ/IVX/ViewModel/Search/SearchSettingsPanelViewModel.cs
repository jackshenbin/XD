using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.DataModel;
using System.ComponentModel;
using BOCOM.IVX.Protocol.Model;
using BOCOM.IVX.Framework;

namespace BOCOM.IVX.ViewModel.Search
{
    public class SearchSettingsPanelViewModel : ViewModelBase
    {
        private SearchType m_SelectedSearchCat = SearchType.Normal;

        public int PageCount { get; set; }

        public SearchType SelectedSearchCat
        {
            get
            {
                return m_SelectedSearchCat;
            }
            set
            {
                m_SelectedSearchCat = value;
                //SearchResourceResultType type = SearchResourceResultType.Normal;
                //if (m_SelectedSearchCat == SearchType.Compare)
                //    type = SearchResourceResultType.NoUse;
                //else if (m_SelectedSearchCat == SearchType.Face)
                //    type = SearchResourceResultType.Face;
                //else if (m_SelectedSearchCat == SearchType.Normal)
                //    type = SearchResourceResultType.Normal;
                //else if (m_SelectedSearchCat == SearchType.Vehicle)
                //    type = SearchResourceResultType.Vehicle;

                // Framework.Container.Instance.EvtAggregator.GetEvent<SearchVideoFilerChangedEvent>().Publish(type);
            }
        }

        public SearchSettingsPanelViewModel()
        {
            // Framework.Container.Instance.EvtAggregator.GetEvent<GotoCompareSearchEvent>().Subscribe(OnGotoCompareSearch/*, Microsoft.Practices.Prism.Events.ThreadOption.WinFormUIThread*/);
            Framework.Container.Instance.RegisterEventSubscriber(this);

        }
        public override void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<GotoCompareSearchEvent>().Unsubscribe(OnGotoCompareSearch);
        }

        private void OnGotoCompareSearch(string val)
        {
            //SelectedSearchCat = SearchType.Compare;
            //RaisePropertyChangedEvent("SelectedSearchCat");

        }
    }
}
