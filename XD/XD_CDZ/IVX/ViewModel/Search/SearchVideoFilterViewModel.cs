using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Protocol.Model;
using BOCOM.IVX.Framework;
using Microsoft.Practices.Prism.Events;

namespace BOCOM.IVX.ViewModel
{
    public class SearchVideoFilterViewModel : VideoPictureTreeViewModelBase
    {

        public SearchVideoFilterViewModel(TreeShowObjectFilter filter)
            : base(TreeShowType.Camera)
        {

            Filter = filter;

        }

    }

        
}
