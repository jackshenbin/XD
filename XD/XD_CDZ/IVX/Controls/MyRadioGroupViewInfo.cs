using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Repository;
using System.Windows.Forms;
using System.Drawing;

namespace BOCOM.IVX.Controls
{
    class MyRadioGroupViewInfo : RadioGroupViewInfo
    {
        public MyRadioGroupViewInfo(RepositoryItem item) : base(item) { }

        protected override void CalcRadioGroupItemsInfo()
        {
            base.CalcRadioGroupItemsInfo();
            int x = 0;
            int index = 0;
            int offset = 0;
            int captionWidth;
            foreach (RadioGroupItemViewInfo item in ItemsInfo)
            {
                index++;
                Rectangle newBounds = item.Bounds;
                newBounds.X = x;
                Rectangle newGlyphRect = item.GlyphRect;
                newGlyphRect.X = x;
                x += newGlyphRect.Width + 2; // (偏移2， 文字离按钮太近了)
                Rectangle newCaptionRect = item.CaptionRect;
                newCaptionRect.X = x;
                captionWidth = TextRenderer.MeasureText(item.Caption, this.OwnerControl.Font).Width;
                newCaptionRect.Width = captionWidth;
                x += newCaptionRect.Width;
                newBounds.Width = newGlyphRect.Width + newCaptionRect.Width;
         
                item.Bounds = newBounds;
                item.CaptionRect = newCaptionRect;
                item.GlyphRect = newGlyphRect;

                // x = x + 5;
            }
        }
    }
}
