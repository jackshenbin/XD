using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.DataModel;
using System.Drawing;
using System.Windows.Forms;

namespace BOCOM.IVX.ViewModel.Search
{
    public class CarSearchViewModel : VideoSearchSettingsViewModelBase
    {
        #region Fields

        private string m_PlateNumber;

        private int m_Brand;

        private int m_NVehicleDetailType;

        private int m_NVehiclePlateType;

        private Color m_VehicleColor;

        private Color m_PlateColor;

        #endregion

        #region Properties

        protected override SearchType SearchType
        {
            get
            {
                return DataModel.SearchType.Vehicle;
            }
        }

        public string PlateNumber
        {
            get { return m_PlateNumber; }
            set
            {
                m_PlateNumber = value;
                m_SearchPara[SDKConstant.szVehiclePlateName] = value;
            }
        }


        public int Brand
        {
            get { return m_Brand; }
            set
            {
                m_Brand = value;
                m_SearchPara[SDKConstant.dwVehicleLogo] = value;
            }
        }

        public int NVehicleDetailType
        {
            get
            {
                return m_NVehicleDetailType;
            }
            set
            {
                if (m_NVehicleDetailType != value)
                {
                    m_NVehicleDetailType = value;
                    m_SearchPara[SDKConstant.dwVehicleDetailType] = value;
                    RaisePropertyChangedEvent("NVehicleDetailType");
                }
            }
        }

        public int NVehiclePlateType
        {
            get
            {
                return m_NVehiclePlateType;
            }
            set
            {
                if (m_NVehiclePlateType != value)
                {
                    m_NVehiclePlateType = value;
                    m_SearchPara[SDKConstant.dwVehiclePlateStruct] = value;
                    RaisePropertyChangedEvent("NVehiclePlateType");
                }
            }
        }

        public Color PlateColor
        {
            get { return m_PlateColor; }
            set
            {
                m_PlateColor = value;
                m_SearchPara[SDKConstant.dwVehiclePlateColor] = Framework.Container.Instance.ColorService.GetPlateColorIndex(value);
                
            }
        }

        public Color VehicleColor
        {
            get { return m_VehicleColor; }
            set
            {
                m_VehicleColor = value;
                m_SearchPara[SDKConstant.dwVehicleColor] = Framework.Container.Instance.ColorService.GetVehicleColorIndex(value);
            }
        }

        #endregion

        #region Constructors

        public CarSearchViewModel()
        {
            m_SearchPara.SearchType = SearchType.Vehicle;
            m_SearchPara.SortType = SortType.TimeAsc;
            m_SearchPara[SDKConstant.dwVehicleType] = DataModel.VehicleType.None;
            m_SearchPara[SDKConstant.dwVehicleDetailType] = DataModel.VehicleDetailType.None;
            m_SearchPara[SDKConstant.dwVehicleColor] = 0;
            m_SearchPara[SDKConstant.dwVehicleLogo] = -1;
            m_SearchPara[SDKConstant.szVehiclePlateName] = string.Empty;
            m_SearchPara[SDKConstant.dwVehiclePlateStruct] = 0;
            m_SearchPara[SDKConstant.dwVehiclePlateColor] = 0;
        }

        #endregion
    }

}
