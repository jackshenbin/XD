using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Framework
{
    public class CacheManager
    {
        private List<ICacheItem> m_CacheItems;
        private static readonly int MAXIMUMCOUNT_DEFAULT = 5;

        private int m_MaximumCount = MAXIMUMCOUNT_DEFAULT;

        public CacheManager()
        {
            m_CacheItems = new List<ICacheItem>();
        }

        private void ClearExceededItems()
        {
            while (m_CacheItems.Count >= m_MaximumCount)
            {
                ICacheItem item = m_CacheItems[0];
                item.Clear();
                m_CacheItems.Remove(item);
            }
        }

        public void Register(ICacheItem item)
        {
            if (item != null)
            {
                if (m_CacheItems.Contains(item))
                {
                    m_CacheItems.Remove(item);
                    m_CacheItems.Add(item);
                }
                else
                {
                    ClearExceededItems();
                    m_CacheItems.Add(item);
                }
            }

        }

        public bool HasItem(ICacheItem item)
        {
            bool bRet = false;

            if (item != null)
            {
                bRet = m_CacheItems.Contains(item);
            }

            return bRet;
        }

        public void UnRegister(ICacheItem item)
        {
            if (item != null && m_CacheItems.Contains(item))
            {
                m_CacheItems.Remove(item);
            }
        }
    }
}
