using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace BOCOM.IVX.Controls
{
    public partial class CustomizedButton : SimpleButton
    {
        public CustomizedButton()
        {
            InitializeComponent();

            LookAndFeel.UseDefaultLookAndFeel = false;

           Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
           Appearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
           Appearance.BorderColor = DataModel.Constant.COLOR_FONT;
           Appearance.ForeColor = DataModel.Constant.COLOR_FONT;
           LookAndFeel.UseDefaultLookAndFeel = false;
           LookAndFeel.SkinName = "High Contrast";
           LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
           Appearance.Options.UseBackColor = true;
           Appearance.Options.UseBorderColor = true;
           Appearance.Options.UseForeColor = true;
           ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
           Location = new System.Drawing.Point(249, 131);
           LookAndFeel.SkinName = "High Contrast";
           
           Name = "simpleButton13";
        }
    }
}
