using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraTreeList.Nodes;

namespace BOCOM.IVX.ViewModel
{
    public class LocalVideoResourceViewModel
    {
        #region Fields

        private List<LocalVideoResourceInfo> m_localVideoResourceInfos;
        private List<TreeListNode> m_checkedNodes = new List<TreeListNode>();
        private List<TreeListNode> m_selectedNodes = new List<TreeListNode>();

        #endregion

        #region Constructors

        public LocalVideoResourceViewModel()
        {
            m_localVideoResourceInfos = new List<LocalVideoResourceInfo>();
        }

        #endregion

        #region Public helper functions

        public void SetCheckedVideoResources(List<TreeListNode> checkedNodes)
        {
            m_checkedNodes = checkedNodes;
        }
        
        public void SetSelectedVideoResources(List<TreeListNode> selectedNodes)
        {
            m_selectedNodes = selectedNodes;
        }
        
        public LocalVideoResourceInfo AddItem(int id)
        {
            LocalVideoResourceInfo item = new LocalVideoResourceInfo(id);
            this.m_localVideoResourceInfos.Add(item);
            return item;
        }

        public void RemoveItem(LocalVideoResourceInfo item)
        {
            if (item != null && m_localVideoResourceInfos.Contains(item))
            {
                m_localVideoResourceInfos.Remove(item);
            }
        }
        
        public LocalVideoResourceInfo[] CheckedVideoResources
        {
            get
            {
                List<LocalVideoResourceInfo> videoResources = new List<LocalVideoResourceInfo>();

                foreach ( TreeListNode n in m_checkedNodes)
                {
                    if (n.Tag is LocalVideoResourceInfo)
                    {
                        videoResources.Add((LocalVideoResourceInfo)n.Tag);
                    }
                }

                return videoResources.ToArray();
            }
        }

        public LocalVideoResourceInfo[] SelectedVideoResources
        {
            get
            {
                List<LocalVideoResourceInfo> videoResources = new List<LocalVideoResourceInfo>();

                foreach ( TreeListNode n in m_selectedNodes)
                {
                    if (n.Tag is LocalVideoResourceInfo)
                    {
                        videoResources.Add((LocalVideoResourceInfo)n.Tag);
                        //if (videoResources.Count >= 3)
                        //{
                        //    break;
                        //}
                    }
                }

                return videoResources.ToArray();
            }
        }

        public LocalVideoResourceInfo GetResource(int id)
        {
            LocalVideoResourceInfo resource = m_localVideoResourceInfos.SingleOrDefault(r => r.Id == id);

            return resource;
        }

        public LocalVideoResourceInfo[] GetAllResources()
        {
            return m_localVideoResourceInfos.ToArray();
        }

        //public List<int> GetCheckedIds()
        //{
        //    List<int> ids = new List<int>();
        //    foreach (TreeListNode n in m_checkedNodes)
        //    {
        //        if (n.Tag is LocalVideoResourceInfo)
        //        {
        //            LocalVideoResourceInfo info = (LocalVideoResourceInfo)n.Tag;
        //            if (!ids.Contains(info.Id) && info.ItemInfo.eItemType == Protocol.E_ITEM_TYPE.ITEM_TYPE_FILE)
        //            {
        //                ids.Add(info.Id);
        //                //if (ids.Count >= 3)
        //                //{
        //                //    break;
        //                //}
        //            }
        //        }
        //    }

        //    return ids;
        //}
        
        //public List<int> GetSelectedIds()
        //{
        //    List<int> ids = new List<int>();
        //    foreach (TreeListNode n in m_selectedNodes)
        //    {
        //        if (n.Tag is LocalVideoResourceInfo)
        //        {
        //            LocalVideoResourceInfo info = (LocalVideoResourceInfo)n.Tag;
        //            if (!ids.Contains(info.Id) && info.ItemInfo.eItemType  == Protocol.E_ITEM_TYPE.ITEM_TYPE_FILE)
        //            {
        //                ids.Add(info.Id);
        //                if (ids.Count >= 3)
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //    return ids;
        //}

        #endregion

    }
}
