using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;

namespace BOCOM.IVX.ViewModel
{
    public class LocalVideoResourceInfo
    {
        //private PTRESOURCE_ITEM_INRO m_ItemInfo;

        //public bool IsChecked { get; set; }

        public int Id { get; private set; }

        //public PTRESOURCE_ITEM_INRO ItemInfo
        //{
        //    get
        //    {
        //        Common.Utils.GetItemInfo(Id, out this.m_ItemInfo);
        //        return m_ItemInfo;
        //    }
        //}

        public LocalVideoResourceInfo(int id)
        {
            this.Id = id;
        }

    }
}
