using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Drawing;

namespace BOCOM.DataModel
{

    public class CompareImageInfo
    {
        public Image Image = null;
        public Image RegionImage = null;
        public Rectangle ImageRectangle = new Rectangle();
        public ImageType ImageType = ImageType.Common;
    };


    public enum ImageType
    {
        Common,
        Face,
        Object,
        Car,

    
    }
    
}
