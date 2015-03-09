using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using BOCOM.DataModel;
using BOCOM.IVX.Views.Search;
using BOCOM.IVX.Framework;
using Microsoft.Practices.Prism.Events;

namespace BOCOM.IVX.UILogics
{
    public class SearchResultViewController : IEventAggregatorSubscriber
    {
        #region Fields

        private PanelControl m_tabCtrlContainer;

        private SearchType m_currentSearchType;

        private Dictionary<SearchType, ucSearchResult> m_DTSearchType2SearchResultView;

        #endregion

        public SearchResultViewController(PanelControl tabControl, SearchType defaultSearchType, ucSearchResult defaultView)
        {
            this.m_tabCtrlContainer = tabControl;
            m_currentSearchType = defaultSearchType;
            m_DTSearchType2SearchResultView = new Dictionary<SearchType,ucSearchResult>();
            m_DTSearchType2SearchResultView.Add(defaultSearchType, defaultView);

            Framework.Container.Instance.EvtAggregator.GetEvent<SearchVideoFilerChangedEvent>().Subscribe(OnSearchVideoFilerChanged, ThreadOption.WinFormUIThread);
            Framework.Container.Instance.RegisterEventSubscriber(this);
        }

        public void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<SearchVideoFilerChangedEvent>().Unsubscribe(OnSearchVideoFilerChanged);
        }

        private ucSearchResult CreateResultView(SearchType type)
        {
            ucSearchResult resultView = new ucSearchResult();
            resultView.Init(type);

            return resultView;
        }

        private void DisplayResultView(SearchType type)
        {
            ucSearchResult resultView = null;

            if (m_DTSearchType2SearchResultView.ContainsKey(type))
            {
                resultView = m_DTSearchType2SearchResultView[type];
            }
            else
            {
                resultView = CreateResultView(type);
                m_tabCtrlContainer.Controls.Add(resultView);
                resultView.Dock = System.Windows.Forms.DockStyle.Fill;
                m_DTSearchType2SearchResultView.Add(type, resultView);
            }
            resultView.BringToFront();
        }

        private void OnSearchVideoFilerChanged(SearchResourceResultType type)
        {
            if (type != SearchResourceResultType.NoUse)
            {
                SearchType searchType = SearchType.Normal;
                if (type == SearchResourceResultType.Compare_Face || type == SearchResourceResultType.Compare_Normal)
                    searchType = SearchType.Compare;
                else if (type == SearchResourceResultType.Face)
                    searchType = SearchType.Face;
                else if (type == SearchResourceResultType.Normal)
                    searchType = SearchType.Normal;
                else if (type == SearchResourceResultType.Vehicle)
                    searchType = SearchType.Vehicle;


                if (m_currentSearchType != searchType)
                {
                    DisplayResultView(searchType);
                    m_currentSearchType = searchType;
                }
            }
        }
    }
}
