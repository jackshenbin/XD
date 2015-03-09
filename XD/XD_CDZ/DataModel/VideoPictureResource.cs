using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.DataModel
{
    public class VideoPictureResource
    {
        private List<VideoPictureResource> m_Children;

        public ResourceType Type { get; private set; }

        public string Name { get; set; }

        public object Subject { get; set; }

        public int ImageIndex { get; private set; }

        public int SelectImageIndex { get; private set; }
        
        //public bool Expand { get; set; }

        public object TreeNode { get; set; }

        /// <summary>
        /// 在资源树上位置， 一级用3位整数表示， 用于在检索结果中标签页按照这个顺序排列
        /// </summary>
        public string DisplayIndex { get; set; }

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
            //Expand = false;
            TreeNode = null;
            SetImageByType(Type);
        }
        private void SetImageByType(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.CameraPicFolder:
                    ImageIndex = 16;
                    SelectImageIndex = 16;
                    break;
                case ResourceType.CameraVideoFolder:
                    ImageIndex = 13;
                    SelectImageIndex = 13;
                    break;
                case ResourceType.VideoFile:
                    ImageIndex = 0;
                    SelectImageIndex = 0;
                    break;
                case ResourceType.PicSet:
                    ImageIndex = 15;
                    SelectImageIndex = 15;
                    break;
                case ResourceType.NonCameraPicFolder:
                    ImageIndex = 11;
                    SelectImageIndex = 11;
                    break;
                case ResourceType.NonCameraVideoFolder:
                    ImageIndex = 11;
                    SelectImageIndex = 11;
                    break;
                case ResourceType.Camera:
                    ImageIndex = 14;
                    SelectImageIndex = 14;
                    break;
                case ResourceType.CameraGroup:
                    ImageIndex = 7;
                    SelectImageIndex = 7;
                    break;
                case ResourceType.VideoSupplierDevice:
                    ImageIndex = 3;
                    SelectImageIndex = 3;
                    break;
                case ResourceType.Task:
                    ImageIndex = 13;
                    SelectImageIndex = 13;
                    break;
            }
        }

        public void AddChild(VideoPictureResource child)
        {
            if (!m_Children.Contains(child))
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
        CameraVideoFolder,
        CameraGroup,
        Camera,
        NonCameraVideoFolder,
        CameraPicFolder,
        NonCameraPicFolder,
        VideoFile,
        PicSet,
        VideoSupplierDevice,
        Task,
    }
}
