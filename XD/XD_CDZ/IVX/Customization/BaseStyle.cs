using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace BOCOM.IVX.Customization
{
    public class BaseStyle
    {
        public virtual Image HeadImage
        {
            get
            {
                return null;//global::BOCOM.IVX.Properties.Resources.头1副本;
            }
        }

        public virtual Image ImageInLoginForm
        {
            get
            {
                return global::BOCOM.IVX.Properties.Resources.bocom;
            }
        }

        public virtual bool ShowImageInLoginForm
        {
            get
            {
                return true;
            }
        }

    }
}
