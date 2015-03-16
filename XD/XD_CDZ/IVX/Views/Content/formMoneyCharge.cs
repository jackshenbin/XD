using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BOCOM.IVX.Views.Content
{
    public partial class formMoneyCharge : Form
    {
        public formMoneyCharge(string title,bool isDigit)
        {
            InitializeComponent();
            Text = title;
            this.textBoxMoney.DisplayFormat = isDigit ? "0" : "0.##";
            
        }

        public double Value
        {
            get
            {
                return this.textBoxMoney.Value;
            }
            
        }
    }
}
