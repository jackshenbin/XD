using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.DataModel;
using System.Drawing;
using System.Windows.Forms;
using BOCOM.IVX.Framework;

namespace BOCOM.IVX.ViewModel.Search
{
    public class CompareSearchViewModel : VideoSearchSettingsViewModelBase
    {
        #region Fields
        
        private CompareSearchPattern m_Pattern;

        private E_ColorSimilarity m_colorSimilarity = E_ColorSimilarity.None;
        
        private Image m_Image;
        
        private Image m_RegionImage;

        private Rectangle m_ImageRectangle = Rectangle.Empty;

        #endregion

        #region Properties

        protected override SearchType SearchType
        {
            get
            {
                return DataModel.SearchType.Compare;
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
                    m_SearchPara[BOCOM.DataModel.SDKConstant.dwAlgorithmFilterType] = value + 1;

                }

                if(m_Pattern == CompareSearchPattern.Face)
                    Framework.Container.Instance.EvtAggregator.GetEvent<SearchVideoFilerChangedEvent>().Publish(SearchResourceResultType.Compare_Face);
                else
                    Framework.Container.Instance.EvtAggregator.GetEvent<SearchVideoFilerChangedEvent>().Publish(SearchResourceResultType.Compare_Normal);
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
                }
            }
        }

        public Image RegionImage
        {
            get { return m_RegionImage; }
            set
            {
                if (m_RegionImage != value)
                {
                    m_RegionImage = value;
                    RaisePropertyChangedEvent("RegionImage");
                }
            }
        }
        
        public Rectangle ImageRectangle
        {
            get { return m_ImageRectangle; }
            set
            {
                if (m_ImageRectangle != value)
                {
                    m_ImageRectangle = value;
                    m_SearchPara[BOCOM.DataModel.SDKConstant.CompareImageRect] = value;
                }
            }
        }


        #endregion

        #region Constructors

        public CompareSearchViewModel()
        {
            m_SearchPara.SearchType = SearchType.Compare;
            m_Pattern = CompareSearchPattern.Blob;
            m_SearchPara.SortType = SortType.SimilarityDes;

            m_SearchPara[BOCOM.DataModel.SDKConstant.dwAlgorithmFilterType] = m_Pattern + 1;
            m_SearchPara[BOCOM.DataModel.SDKConstant.dwSearchObjType] = E_MovingObjectType.All;

            m_SearchPara[BOCOM.DataModel.SDKConstant.dwColorSimilar] = m_colorSimilarity = E_ColorSimilarity.None;
            Framework.Container.Instance.EvtAggregator.GetEvent<SetCompareImageInfoEvent>().Subscribe(OnSetCompareImageInfo);
            Framework.Container.Instance.RegisterEventSubscriber(this);

        }
        public override void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<SetCompareImageInfoEvent>().Unsubscribe(OnSetCompareImageInfo);
        }

        #endregion


        public void LoadImage()
        {
            FormLoadImage image;
            if (Image != null)
                image = new FormLoadImage(Image, m_ImageRectangle, (m_Pattern == CompareSearchPattern.Face) ? ImageType.Face : ImageType.Object);
            else
                image = new FormLoadImage();
            image.ShowDialog();

        }

        public void OnSetCompareImageInfo(CompareImageInfo info)
        {
            if (info.Image != null)
            {
                Image = info.Image;
                RegionImage = info.RegionImage;
                ImageRectangle = info.ImageRectangle;
            }

            if (info != null)
            {
                if (info.ImageType == ImageType.Face)
                {
                    NPattern = 2;
                }
                else
                {
                    if (m_Pattern != CompareSearchPattern.Blob && m_Pattern != CompareSearchPattern.Texture)
                        NPattern = 0;
                }
                RaisePropertyChangedEvent("NPattern");
            }
        }

        protected override bool Validate(TaskUnitInfo[] taskUnits)
        {
            bool bRet = base.Validate(taskUnits);

            if (m_ImageRectangle == Rectangle.Empty)
            {
                bRet = false;
                Framework.Container.Instance.InteractionService.ShowMessageBox("请导入图片后再进行检索!", Framework.Environment.PROGRAM_NAME,
                  System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }

            return bRet;
        }
    }
}
