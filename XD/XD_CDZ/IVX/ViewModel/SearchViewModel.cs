using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Framework;
using BOCOM.IVX.Protocol;
using BOCOM.DataModel;
using BOCOM.IVX.Protocol.Model;
using BOCOM.IVX.Common;

namespace BOCOM.IVX.ViewModel
{
    class SearchViewModel : IEventAggregatorSubscriber
    {
        public SearchViewModel()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraSelectionChangedEvent>().Subscribe(CameraSelectionChanged);
            Framework.Container.Instance.RegisterEventSubscriber(this);

        }

        public void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraSelectionChangedEvent>().Unsubscribe(CameraSelectionChanged);
        }

        public event EventHandler EventCameraSelectionChanged;
        public List<CameraInfo> SelectedCameras =new List<CameraInfo>();
        
        void CameraSelectionChanged(List<object> list)
        {
            lock (SelectedCameras)
            {
                SelectedCameras.Clear();
                foreach (object item in list)
                {
                    if (item is CameraInfo)
                    {
                        if (!SelectedCameras.Contains((CameraInfo)item))
                        {
                            SelectedCameras.Add((CameraInfo)item);
                        }
                    }
                }
                NormalSearchPara[DataModel.Constant.Common_ResourceList] = SelectedCameras;
                if (EventCameraSelectionChanged!=null)
                    EventCameraSelectionChanged(null, null);
            }
        }

        Dictionary<string, object> m_normalSearchPara;
        public Dictionary<string, object> NormalSearchPara
        {
            get { return m_normalSearchPara??new Dictionary<string, object>(); }
            set 
            {
                m_normalSearchPara = value;
                if (m_normalSearchPara.ContainsKey(DataModel.Constant.Common_SearchType))
                    ChangeDrawMode((E_VDA_SEARCH_MOVE_OBJ_RANGE_FILTER_TYPE)m_normalSearchPara[DataModel.Constant.Common_SearchType]);
            }
        }

        Dictionary<string, object> m_carSearchPara;
        public Dictionary<string, object> CarSearchPara
        {
            get { return m_carSearchPara ?? new Dictionary<string, object>(); }
            set { m_carSearchPara = value; }
        }

        Dictionary<string, object> m_FaceSearchPara;
        public Dictionary<string, object> FaceSearchPara
        {
            get { return m_FaceSearchPara ?? new Dictionary<string, object>(); }
            set { m_FaceSearchPara = value; }
        }
        
        Dictionary<string, object> m_compareSearchPara;
        public Dictionary<string, object> CompareSearchPara
        {
            get { return m_compareSearchPara ?? new Dictionary<string, object>(); }
            set { m_compareSearchPara = value; }
        }

        public bool NormalSearch()
        {
            StringBuilder sb = new StringBuilder();
            SelectedCameras.ForEach(item => sb.AppendFormat("[{0}]{1},", item.CameraID, item.CameraName));
            Framework.Container.Instance.InteractionService.ShowMessageBox(sb.ToString()
                , Framework.Environment.PROGRAM_NAME, System.Windows.Forms.MessageBoxButtons.OK
                , System.Windows.Forms.MessageBoxIcon.Asterisk);

            sb.Clear();
            foreach (string k in m_normalSearchPara.Keys)
            {
                sb.AppendLine(k +":"+ m_normalSearchPara[k].ToString());
            }

            Framework.Container.Instance.InteractionService.ShowMessageBox(sb.ToString()
                , Framework.Environment.PROGRAM_NAME, System.Windows.Forms.MessageBoxButtons.OK
                , System.Windows.Forms.MessageBoxIcon.Asterisk);
            
            return true;
        }

        void ChangeDrawMode(E_VDA_SEARCH_MOVE_OBJ_RANGE_FILTER_TYPE mode)
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<BOCOM.IVX.Framework.CompareDrawModeChangeEvent>().Publish(mode);
        }



    }
}
