﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Customization
{
    public sealed class NeuterStyle : BaseStyle
    {
        public override System.Drawing.Image HeadImage
        {
            get
            {
                return null;// global::BOCOM.IVX.Properties.Resources.头NEUTERPRODUCT;
            }
        }

        public override bool ShowImageInLoginForm
        {
            get
            {
                return false;
            }
        }
    }
}
