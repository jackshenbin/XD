using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.DataModel;
using System.Drawing;
using System.Windows.Forms;

namespace BOCOM.IVX.ViewModel.Search
{
    public class FaceSearchViewModel : VideoSearchSettingsViewModelBase
    {
        #region Fields
        
        private CompareSearchPattern m_Pattern;
        private E_ColorSimilarity m_colorSimilarity = E_ColorSimilarity.None;
        private Image m_Image;

        #endregion

        #region Properties

        protected override SearchType SearchType
        {
            get
            {
                return DataModel.SearchType.Face;
            }
        }

        public E_ColorSimilarity ColorSimilarity
        {
            get { return m_colorSimilarity; }
            set
            {
                m_colorSimilarity = value;
                m_SearchPara[BOCOM.DataModel.SDKConstant.dwColorSimilar] = value;
            }
        }

        public int NColorSimilarity
        {
            get { return (int)m_colorSimilarity; }
            set
            {
                if (NColorSimilarity != value)
                {
                    ColorSimilarity = (E_ColorSimilarity)value;
                    RaisePropertyChangedEvent("NColorSimilarity");
                }
            }
        }

        public int NPattern
        {
            get
            {
                return (int)m_Pattern;
            }
            set
            {
                if (NPattern != value)
                {
                    m_Pattern = (CompareSearchPattern)value;
                    m_SearchPara[BOCOM.DataModel.SDKConstant.dwAlgorithmFilterType] = m_Pattern + 1;
                }
            }
        }

        public Image Image
        {
            get
            {
                return m_Image;
            }
            set
            {
                if (m_Image != value)
                {
                    m_Image = value;
                    m_SearchPara[BOCOM.DataModel.SDKConstant.CompareImage] = value;
                    RaisePropertyChangedEvent("Image");
                }
            }
        }

        #endregion

        #region Constructors

        public FaceSearchViewModel()
        {
            m_SearchPara.SearchType = SearchType.Face;
            m_SearchPara.SortType = SortType.TimeAsc;

            // m_SearchPara[SDKConstant.dwSearchObjType] = MovingObjectType.All;
            // m_SearchPara[SDKConstant.dwColorSimilar] = m_colorSimilarity = Protocol.Model.ColorSimilarity.Middle;
        }
        public void GoToCompareSearch()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<BOCOM.IVX.Framework.GotoCompareSearchEvent>().Publish("");
            CompareImageInfo info = new CompareImageInfo
            {
                Image = null,
                RegionImage = null,
                ImageRectangle = new Rectangle(),
                ImageType =  ImageType.Face,
            };

            Framework.Container.Instance.EvtAggregator.GetEvent<BOCOM.IVX.Framework.SetCompareImageInfoEvent>().Publish(info);

        }

        #endregion

    }
}
