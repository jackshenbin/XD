using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Protocol.Model;
using BOCOM.IVX.Framework;
using Microsoft.Practices.Prism.Events;
using BOCOM.DataModel;

namespace BOCOM.IVX.ViewModel
{
    public class SearchVideoTreeViewModel :ViewModelBase
    {
        public SearchType Filter { get; set; }

        public SearchVideoTreeViewModel(SearchType filter)
        {
            Filter = filter;

            Framework.Container.Instance.EvtAggregator.GetEvent<SearchVideoFilerChangedEvent>().Subscribe(OnSearchVideoFilerChanged);
            Framework.Container.Instance.RegisterEventSubscriber(this);

        }
        public  override void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<SearchVideoFilerChangedEvent>().Unsubscribe(OnSearchVideoFilerChanged);
        }
        private void OnSearchVideoFilerChanged(SearchResourceResultType type)
        {
            if (type != SearchResourceResultType.NoUse)
            {
                SearchType filter = SearchType.Normal;
                if (type == SearchResourceResultType.Face || type == SearchResourceResultType.Compare_Face)
                    filter = SearchType.Face;
                else if (type == SearchResourceResultType.Normal || type == SearchResourceResultType.Compare_Normal)
                    filter = SearchType.Normal;
                else if (type == SearchResourceResultType.Vehicle)
                    filter = SearchType.Vehicle;

                Filter = filter;
                RaisePropertyChangedEvent("Filter");
            }
        }
    }

        
}
