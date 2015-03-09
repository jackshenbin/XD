using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Framework;
using DevExpress.XtraEditors;
using BOCOM.DataModel;
using BOCOM.IVX.Views;
using Microsoft.Practices.Prism.Events;

namespace BOCOM.IVX.UILogics
{
    public class SearchVideoPlayViewController : IEventAggregatorSubscriber
    {
        private PanelControl m_Container;

        private Dictionary<DataModel.SearchResourceResultType, ucVideoPlayBackSingle> m_DTResultType2Player;

        public SearchVideoPlayViewController(PanelControl container, DataModel.SearchResourceResultType defaultSearchType, ucVideoPlayBackSingle defaultView)
        {
            m_Container = container;
            m_DTResultType2Player = new Dictionary<SearchResourceResultType, ucVideoPlayBackSingle>();
            m_DTResultType2Player.Add(defaultSearchType, defaultView);

            Framework.Container.Instance.EvtAggregator.GetEvent<SearchVideoFilerChangedEvent>().Subscribe(new Action<DataModel.SearchResourceResultType>(OnSearchTypeNavigate), ThreadOption.WinFormUIThread);
            Framework.Container.Instance.RegisterEventSubscriber(this);
        }
         
        private void OnSearchTypeNavigate(DataModel.SearchResourceResultType searchType)
        {
            ucVideoPlayBackSingle player = null;
            
            DataModel.SearchResourceResultType type = searchType;
            if(type == SearchResourceResultType.Compare_Normal ||
                type == SearchResourceResultType.Compare_Face)
            {
                type = SearchResourceResultType.NoUse;
            }

            if (m_DTResultType2Player.ContainsKey(type))
            {
                player = m_DTResultType2Player[type];
            }
            else
            {
                player = new ucVideoPlayBackSingle();
                player.SearchType = type;
                player.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
                player.Appearance.Options.UseBackColor = true;
                player.Dock = System.Windows.Forms.DockStyle.Fill;
                // player.Location = new System.Drawing.Point(2, 2);
                m_Container.Controls.Add(player);
                m_DTResultType2Player.Add(type, player);
            }

            player.BringToFront();

        }

        public void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<SearchVideoFilerChangedEvent>().Unsubscribe(new Action<DataModel.SearchResourceResultType>(OnSearchTypeNavigate));
        }
    }
}
