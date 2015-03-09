using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.DataModel;

namespace BOCOM.IVX.UILogics
{
    public class NaviRecord
    {
        /// <summary>
        /// 每个功能大类对应的最近打开的小类
        /// </summary>
        private Dictionary<UIFuncItemInfo, UIFuncItemInfo> m_DTCategory2SubItem;

        /// <summary>
        /// 功能大类对应的全部小类
        /// </summary>
        private Dictionary<UIFuncItemInfo, List<UIFuncItemInfo>> m_DTCategory2SubItems;

        private Dictionary<UIFuncItemInfo, int> m_DTCategory2SplitPosition;

        private UIFuncItemInfo m_curFuncItemInfo;

        public NaviRecord()
        {
            m_DTCategory2SplitPosition = new Dictionary<UIFuncItemInfo, int>();
            m_DTCategory2SplitPosition.Add(UIFuncItemInfo.CASE, 220);
            m_DTCategory2SplitPosition.Add(UIFuncItemInfo.TASK, 0);
            m_DTCategory2SplitPosition.Add(UIFuncItemInfo.SEARCH, 220);
            m_DTCategory2SplitPosition.Add(UIFuncItemInfo.LIVEVIDEO, 220);
            m_DTCategory2SplitPosition.Add(UIFuncItemInfo.BRIEFVIDEO, 220);
            m_DTCategory2SplitPosition.Add(UIFuncItemInfo.EXPORT, 220);
            m_DTCategory2SplitPosition.Add(UIFuncItemInfo.CONFIGURATION, 220);


            m_DTCategory2SubItem = new Dictionary<UIFuncItemInfo, UIFuncItemInfo>();
            m_DTCategory2SubItem.Add(UIFuncItemInfo.CASE, UIFuncItemInfo.MYCASELIST);
            m_DTCategory2SubItem.Add(UIFuncItemInfo.CONFIGURATION, UIFuncItemInfo.LOGMANAGEMENT);
            m_DTCategory2SubItem.Add(UIFuncItemInfo.TASK, UIFuncItemInfo.TASKSTATUS);
            m_DTCategory2SubItem.Add(UIFuncItemInfo.EXPORT, UIFuncItemInfo.TAGEXPORT);

            m_DTCategory2SubItems = new Dictionary<UIFuncItemInfo, List<UIFuncItemInfo>>();
            List<UIFuncItemInfo> items = new List<UIFuncItemInfo>(){
                UIFuncItemInfo.MYCASELIST,
                UIFuncItemInfo.CURRCASE
            };
            m_DTCategory2SubItems.Add(UIFuncItemInfo.CASE, items);

            items = new List<UIFuncItemInfo>(){
                UIFuncItemInfo.NEWTASK,
                UIFuncItemInfo.TASKSTATUS
            };
            m_DTCategory2SubItems.Add(UIFuncItemInfo.TASK, items);

            items = new List<UIFuncItemInfo>(){
                UIFuncItemInfo.PLATMANAGEMENT,
                UIFuncItemInfo.CAMERAMANAGEMENT,
                UIFuncItemInfo.USERMANAGEMENT,
                UIFuncItemInfo.CASEMANAGEMENT,
                UIFuncItemInfo.LOGMANAGEMENT,
                UIFuncItemInfo.CLUSTERMONITOR,
                UIFuncItemInfo.MEDIASERVERMANAGEMENT,
                UIFuncItemInfo.VDASERVERMANAGEMENT,
                UIFuncItemInfo.VDARESULTSERVERMANAGEMENT,
                UIFuncItemInfo.MEDIAROUTERMANAGEMENT,
                UIFuncItemInfo.CLIENTROUTERMANAGEMENT
            };
            m_DTCategory2SubItems.Add(UIFuncItemInfo.CONFIGURATION, items);

            items = new List<UIFuncItemInfo>(){
                UIFuncItemInfo.TAGEXPORT,
                UIFuncItemInfo.CASEEXPORT
            };
            m_DTCategory2SubItems.Add(UIFuncItemInfo.EXPORT, items);

        }

        /// <summary>
        /// 如果传入的是功能大类, 返回该大类最近操作过的该大类下的小类功能
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public UIFuncItemInfo GetSubItem(UIFuncItemInfo item)
        {
            UIFuncItemInfo subItem = item;

            if (item != null && m_DTCategory2SubItem.ContainsKey(item))
            {
                subItem = m_DTCategory2SubItem[item];
            }

            return subItem;
        }

        public void RegisterSubItem(UIFuncItemInfo subItem)
        {
            if (subItem != null && m_curFuncItemInfo != subItem)
            {
                if (subItem.Function == UIFunctionEnum.NewTask)
                {
                    return;
                }

                foreach (UIFuncItemInfo item in m_DTCategory2SubItems.Keys)
                {
                    if (m_DTCategory2SubItems[item].Contains(subItem))
                    {
                        m_DTCategory2SubItem[item] = subItem;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 更新大类对应最近使用小类功能字典
        /// </summary>
        /// <param name="subItem"></param>
        public void RegisterSubItem(UIFuncItemInfo subItem, int oldSplitPosition, out int splitPosition)
        {
            splitPosition = -1;
            if (subItem != null && m_curFuncItemInfo != subItem)
            {
                RegisterSubItem(subItem);

                // 需要记住旧的 splitposition
                UIFuncItemInfo catItemCur = null;
                UIFuncItemInfo catItemNew = subItem.Parent ?? subItem;
                if (m_curFuncItemInfo != null)
                {

                    catItemCur = m_curFuncItemInfo.Parent ?? m_curFuncItemInfo;
                    if (catItemCur != catItemNew)
                    {
                        if (!m_DTCategory2SplitPosition.ContainsKey(catItemCur))
                        {
                            m_DTCategory2SplitPosition.Add(catItemCur, oldSplitPosition);
                        }
                        else
                        {
                            m_DTCategory2SplitPosition[catItemCur] = oldSplitPosition;
                        }

                    }
                }
                if (m_DTCategory2SplitPosition.ContainsKey(catItemNew))
                {
                    splitPosition = m_DTCategory2SplitPosition[catItemNew];
                }
                m_curFuncItemInfo = subItem;
            }
        }
        //public void RegisterSubItem(UIFuncItemInfo subItem, int oldSplitPosition, out int splitPosition)
        //{
        //    splitPosition = -1;
        //    if (subItem != null && m_curFuncItemInfo != subItem)
        //    {
        //        RegisterSubItem(subItem);

        //         需要记住旧的 splitposition
        //        if (m_curFuncItemInfo != null)
        //        {
        //            UIFuncItemInfo catItemCur = m_curFuncItemInfo.Parent ?? m_curFuncItemInfo;
        //            UIFuncItemInfo catItemNew = subItem.Parent ?? subItem;

        //            if (catItemCur != catItemNew)
        //            {
        //                if (!m_DTCategory2SplitPosition.ContainsKey(catItemCur))
        //                {
        //                    m_DTCategory2SplitPosition.Add(catItemCur, oldSplitPosition);
        //                }
        //                else
        //                {
        //                    m_DTCategory2SplitPosition[catItemCur] = oldSplitPosition;
        //                }

        //                if (m_DTCategory2SplitPosition.ContainsKey(catItemNew))
        //                {
        //                    splitPosition = m_DTCategory2SplitPosition[catItemNew];
        //                }
        //            }
        //        }
        //        m_curFuncItemInfo = subItem;
        //    }
        //}

    }
}
