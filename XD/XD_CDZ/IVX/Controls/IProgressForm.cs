using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BOCOM.IVX.Controls
{
    public interface IProgressForm
    {
        string StatusText{get;set;}

        int Maximum{get;set;}

        int Progress{get;set;}
    }
}
