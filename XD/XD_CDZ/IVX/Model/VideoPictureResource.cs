using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Model
{
    public class VideoPictureResource
    {
        private List<VideoPictureResource> m_Children;

        public ResourceType Type { get; private set; }

        public string Name { get; private set; }

        public object Subject { get; private set; }

        public List<VideoPictureResource> Children
        {
            get
            {
                return m_Children;
            }
        }

        public VideoPictureResource(ResourceType rType, string name, object subject = null)
        {
            Type = rType;
            Name = name;
            Subject = subject;
            m_Children = new List<VideoPictureResource>();
        }

        public void AddChild(VideoPictureResource child)
        {
            if(!m_Children.Contains(child))
            {
                m_Children.Add(child);
            }
        }

        public void RemoveChild(VideoPictureResource child)
        {
            if (m_Children.Contains(child))
            {
                m_Children.Remove(child);
            }
        }
    }

    public enum ResourceType
    {
        Folder,
        VideoCamera,
        NonCameraVideoFolder,
        PicCamera,
        NonCameraPicFolder,
        File
    }
}
