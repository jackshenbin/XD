using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Protocol.Model;
using BOCOM.IVX.Framework;
using BOCOM.DataModel;

namespace BOCOM.IVX.ViewModel
{
    public class CameraViewModel : ViewModelBase,IEventAggregatorSubscriber
    {
        private List<CameraInfo> m_Cameras;

        public List<CameraInfo> Cameras
        {
          get { return m_Cameras; }
          set { m_Cameras = value; }
        }

        public CameraViewModel()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraAddedEvent>().Subscribe(OnCameraAdded);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraModifiedEvent>().Subscribe(OnCameraModified);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraDeletedEvent>().Subscribe(OnCameraDeleted);
            Framework.Container.Instance.RegisterEventSubscriber(this);
            FillupCameras();
        }

        void OnCameraAdded(CameraInfo info)
        {
            FillupCameras();
        }
        void OnCameraModified(CameraInfo info)
        {
            FillupCameras();
        }
        void OnCameraDeleted(uint camID)
        {
            FillupCameras();
        }

        private void FillupCameras()
        {
            try
            {
                m_Cameras = Framework.Container.Instance.VDAConfigService.GetAllCameras();
            }
            catch (SDKCallException ex)
            {
                Common.SDKCallExceptionHandler.Handle(ex, "获取全部点位");
            }              

            if (m_Cameras == null)
            {
                m_Cameras = new List<CameraInfo>();
            }

            CameraInfo dummy = new CameraInfo() { CameraID = 0, CameraName = "无" };
            m_Cameras.Insert(0, dummy);
        }


        public void UnSubscribe()
        {
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraAddedEvent>().Unsubscribe(OnCameraAdded);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraModifiedEvent>().Unsubscribe(OnCameraModified);
            Framework.Container.Instance.EvtAggregator.GetEvent<CameraDeletedEvent>().Unsubscribe(OnCameraDeleted);
        }
    }
}
