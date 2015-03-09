using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using BOCOM.DataModel;

namespace BOCOM.IVX.ViewModel
{
    public class VideoVATypeViewModel : ViewModelBase
    {
        public readonly static VideoVATypeInfo OBJECT =
           new VideoVATypeInfo() { VAType = E_VDA_ANALYZE_TYPE.E_ANALYZE_OBJECT, Name = "运动物" };
        public readonly static VideoVATypeInfo BRIEF =
            new VideoVATypeInfo() { VAType = E_VDA_ANALYZE_TYPE.E_ANALYZE_BRIEAF, Name = "摘要" };
        public readonly static VideoVATypeInfo FACE =
            new VideoVATypeInfo() { VAType = E_VDA_ANALYZE_TYPE.E_ANALYZE_FACE, Name = "人脸" };
        public readonly static VideoVATypeInfo VEHICLE =
            new VideoVATypeInfo() { VAType = E_VDA_ANALYZE_TYPE.E_ANALYZE_VEHICLE, Name = "车辆" };

        private List<VideoVATypeInfo> m_videoVATypeInfos;

        public List<VideoVATypeInfo> VideoVATypeInfos
        {
            get { return m_videoVATypeInfos; }
            set { m_videoVATypeInfos = value; }
        }

        public VideoVATypeViewModel()
        {
            m_videoVATypeInfos = new List<VideoVATypeInfo>
            {
                FACE,
                OBJECT,
                VEHICLE,
                BRIEF
            };
        }
    }

    [Serializable]
    public class VideoVATypeInfo
    {
       public E_VDA_ANALYZE_TYPE VAType { get; set; }

        public string Name { get; set; }

        public int NVAType
        {
            get
            {
                return (int)VAType;
            }
            set
            {
                VAType = (E_VDA_ANALYZE_TYPE)value;
            }
        }

    }
}
