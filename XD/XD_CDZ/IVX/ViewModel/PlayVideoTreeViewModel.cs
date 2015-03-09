using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using BOCOM.DataModel;
using BOCOM.IVX.Framework;
using Microsoft.Practices.Prism.Events;

namespace BOCOM.IVX.ViewModel
{
    public class PlayVideoTreeViewModel : VideoPictureTreeViewModelBase
    {

        public PlayVideoTreeViewModel()
            : base(TreeShowType.Camera)
        {

            Filter = TreeShowObjectFilter.NoUse ;

        }

    }

        
}
