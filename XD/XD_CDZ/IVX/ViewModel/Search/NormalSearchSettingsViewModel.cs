using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.DataModel;
using BOCOM.IVX.Protocol.Model;
using System.Drawing;
using System.Windows.Forms;

namespace BOCOM.IVX.ViewModel.Search
{
    public class NormalSearchSettingsViewModel : VideoSearchSettingsViewModelBase
    {
        #region Fields

        private E_MovingObjectType m_objectType = E_MovingObjectType.All;

        private E_VDA_SEARCH_MOVE_OBJ_RANGE_FILTER_TYPE m_behaviorFilterType = E_VDA_SEARCH_MOVE_OBJ_RANGE_FILTER_TYPE.E_SEARCH_MOVE_OBJ_RANGE_FILTER_NOUSE;

        private E_ColorSimilarity m_colorSimilarity = E_ColorSimilarity.None;

        private Color m_Color = Color.Transparent;

        #endregion

        #region Properties

        public E_MovingObjectType ObjectType
        {
            get
            {
                return m_objectType;
            }
            set
            {
                m_objectType = value;
                m_SearchPara[SDKConstant.dwSearchObjType] = value;
            }
        }

        public int NObjectTypeIndex
        {
            get
            {
                return (int)m_objectType - 1;
            }
            set
            {
                if (NObjectTypeIndex != value)
                {
                    ObjectType = (E_MovingObjectType)(value + 1);
                    RaisePropertyChangedEvent("NObjectType");
                }
            }
        }

        public E_VDA_SEARCH_MOVE_OBJ_RANGE_FILTER_TYPE BehaviorFilterType
        {
            get { return m_behaviorFilterType; }
            set
            {
                m_behaviorFilterType = value;
                m_SearchPara[SDKConstant.dwRangeFilterType] = value;
            }
        }

        public int NBehaviorFilterType
        {
            get
            {
                return (int)m_behaviorFilterType;
            }
            set
            {
                if (NBehaviorFilterType != value)
                {
                    BehaviorFilterType = (E_VDA_SEARCH_MOVE_OBJ_RANGE_FILTER_TYPE)value;
                    ChangeDrawMode(m_behaviorFilterType);
                    RaisePropertyChangedEvent("NBehaviorFilterType");
                }
            }
        }

        void ChangeDrawMode(E_VDA_SEARCH_MOVE_OBJ_RANGE_FILTER_TYPE mode)
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<BOCOM.IVX.Framework.CompareDrawModeChangeEvent>().Publish(mode);
        }

        public E_ColorSimilarity ColorSimilarity
        {
            get { return m_colorSimilarity; }
            set
            {
                m_colorSimilarity = value;
                m_SearchPara[SDKConstant.dwColorSimilar] = value;
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

        public Color Color
        {
            get
            {
                return m_Color;
            }
            set
            {
                m_Color = value;
                m_SearchPara[SDKConstant.dwSearchObjRGB] = value;
                m_SearchPara[SDKConstant.bColorSearch] = m_Color != Color.Transparent;
                m_SearchPara.SortType = (m_Color != Color.Transparent) ? SortType.SimilarityDes : SortType.TimeAsc;
            }
        }

        #endregion

        public NormalSearchSettingsViewModel()
        {
            m_SearchPara.SearchType = SearchType.Normal;
            m_SearchPara.SortType = SortType.TimeAsc;

            m_SearchPara[SDKConstant.dwSearchObjType] = E_MovingObjectType.All;
            m_SearchPara[SDKConstant.dwRangeFilterType] = E_VDA_SEARCH_MOVE_OBJ_RANGE_FILTER_TYPE.E_SEARCH_MOVE_OBJ_RANGE_FILTER_NOUSE;
            m_SearchPara[SDKConstant.bColorSearch] = m_Color != Color.Transparent;
            m_SearchPara[SDKConstant.dwSearchObjRGB] = m_Color;
            m_SearchPara[SDKConstant.dwColorSimilar] = m_colorSimilarity;

        }

        protected override bool Validate(TaskUnitInfo[] taskUnits)
        {
            bool bRet = base.Validate(taskUnits);

            if (bRet)
            {
                List<PassLine> passlinelist = new List<PassLine>();
                List<BreakRegion> breakregionlist = new List<BreakRegion>();

                if (m_behaviorFilterType == E_VDA_SEARCH_MOVE_OBJ_RANGE_FILTER_TYPE.E_SEARCH_MOVE_OBJ_RANGE_FILTER_BREAK_REGION)
                {
                    try
                    {
                        breakregionlist = Framework.Container.Instance.GraphicDrawService.GetPlayDrawBreakRegion();
                        if (breakregionlist == null || breakregionlist.Count == 0)
                        {
                            bRet = false;
                        }
                    }
                    catch (Protocol.SDKCallException)
                    {
                        bRet = false;
                    }

                    if (!bRet)
                    {
                        breakregionlist = new List<BreakRegion>();
                        Framework.Container.Instance.InteractionService.ShowMessageBox("请绘制闯入闯出区域后再进行检索!", Framework.Environment.PROGRAM_NAME,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (m_behaviorFilterType == E_VDA_SEARCH_MOVE_OBJ_RANGE_FILTER_TYPE.E_SEARCH_MOVE_OBJ_RANGE_FILTER_PASS_LINE)
                {
                    try
                    {
                        passlinelist = Framework.Container.Instance.GraphicDrawService.GetPlayDrawPassline();
                        if (passlinelist == null || passlinelist.Count == 0)
                        {
                            bRet = false;
                        }
                    }
                    catch (Protocol.SDKCallException)
                    {
                        passlinelist = new List<PassLine>();
                        bRet = false;
                    }
                    if (!bRet)
                    {
                        Framework.Container.Instance.InteractionService.ShowMessageBox("请绘制越界线后再进行检索!", Framework.Environment.PROGRAM_NAME,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                if (bRet)
                {
                    m_SearchPara[SDKConstant.ptSearchPassLineList] = passlinelist;
                    m_SearchPara[SDKConstant.dwPassLineNum] = passlinelist.Count;
                    m_SearchPara[SDKConstant.tSearchBreakRegion] = breakregionlist;
                }
            }

            return bRet;
        }
    }
}