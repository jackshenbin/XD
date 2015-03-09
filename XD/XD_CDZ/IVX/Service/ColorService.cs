using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.DataModel;
using System.Drawing;

namespace BOCOM.IVX.Service
{
    public class ColorService
    {
        public readonly static ColorName COLOR_TRANSPARENT = new ColorName(Color.Transparent, "不限");
        public readonly static ColorName COLOR_WHITE   =  new ColorName(Color.White, "白色");
        public readonly static ColorName COLOR_SILVER  =  new ColorName(Color.Silver,  "银色");
        public readonly static ColorName COLOR_BLACK  =   new ColorName(Color.Black, "黑色");
        public readonly static ColorName COLOR_RED      =   new ColorName(Color.Red, "红色");
        public readonly static ColorName COLOR_PURPL  =   new ColorName(Color.Purple, "紫色");
        public readonly static ColorName COLOR_BLUE     =  new ColorName(Color.Blue, "蓝色");
        public readonly static ColorName COLOR_ORANGE = new ColorName(Color.Orange, "橘黄");
        public readonly static ColorName COLOR_YELLOW= new ColorName(Color.Yellow, "黄色");
        public readonly static ColorName COLOR_GREEN  =  new ColorName(Color.Green, "绿色");
        public readonly static ColorName COLOR_BROWN =  new ColorName(Color.Brown, "褐色");
        public readonly static ColorName COLOR_PINK      =  new ColorName(Color.Pink, "粉色");
        public readonly static ColorName COLOR_GRAY    =  new ColorName(Color.Gray, "灰色");

        private readonly static ColorName[] COLORNAMES_MOVEOBJ = new ColorName[]{
           COLOR_TRANSPARENT,
           COLOR_RED,        
           COLOR_GREEN,      
           COLOR_BLUE,       
           COLOR_ORANGE,     
           COLOR_YELLOW,     
           COLOR_PURPL,      
           COLOR_WHITE,      
           COLOR_BLACK,      
        };                   

        private readonly static ColorName[] COLORNAMES_VEHICLEBODY = new ColorName[]{
           COLOR_TRANSPARENT,
           COLOR_WHITE,
           COLOR_SILVER,
           COLOR_BLACK,
           COLOR_RED,    
           COLOR_PURPL,
           COLOR_BLUE,  
           COLOR_YELLOW,
           COLOR_GREEN,
           COLOR_BROWN,
           COLOR_PINK ,  
           COLOR_GRAY
        };

        private readonly static ColorName[] COLORNAMES_VEHICLEPLATE = new ColorName[]{
           COLOR_TRANSPARENT,
           COLOR_BLUE,  
           COLOR_BLACK,
           COLOR_YELLOW,
           COLOR_WHITE,
        };

        private static int GetColorIndex(ColorName[] colors, Color color)
        {
            int index = -1;
            for (int i = 0; i < colors.Length; i++)
            {
                if (colors[i].Color.ToArgb() == color.ToArgb())
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        public ColorName[] GetVehicleColors()
        {
            return COLORNAMES_VEHICLEBODY;
        }

        public ColorName[] GetMoveObjColors()
        {
            return COLORNAMES_MOVEOBJ;
        }

        public ColorName[] GetPlateColors()
        {
            return COLORNAMES_VEHICLEPLATE;
        }

        public ColorName GetPlateColorName(int id)
        {
            return COLORNAMES_VEHICLEPLATE[id];
        }

        public ColorName GetVehicleColorName(int id)
        {
            return COLORNAMES_VEHICLEBODY[id];
        }

        public int GetVehicleColorIndex(Color color)
        {
            return GetColorIndex(COLORNAMES_VEHICLEBODY, color);
        }

        public int GetPlateColorIndex(Color color)
        {
            return GetColorIndex(COLORNAMES_VEHICLEPLATE, color);
        }

        public int GetObjectColorIndex(Color color)
        {
            return GetColorIndex(COLORNAMES_MOVEOBJ, color);
        }
    }
}
