using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BOCOM.IVX.Controls
{
    public partial class ucCardIDTextBox : DevComponents.DotNetBar.Controls.TextBoxX
    {
        public ucCardIDTextBox()
        {
            this.TextChanged += ucCardIDTextBox_TextChanged;
            this.MaxLength = 16+3;
            
        }



        public string Value
        {
            get
            {
                string temp = base.Text;
                for (int i = temp.Length-1; i >=0; i--)
                {
                    if (temp[i] == ' ')
                        temp = temp.Remove(i, 1);
                }
                return temp;
            }
            set
            {
                base.Text = ConvetToCardID(value);
            }
        }

        void ucCardIDTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.Text = ConvetToCardID(this.Text);
                this.SelectionStart = this.Text.Length;
            }
            catch
            {

            }
        }

        private static string ConvetToCardID(string s)
        {
            for (int i = s.Length - 1;i>=0 ; i--)
            {
                if (!char.IsDigit(s, i))
                {
                    s=s.Remove(i, 1);
                }
            }
            string ss = "";
            if (s.Length > 0)
            {
                ss = s[0].ToString();
                string str_temp = "";
                for (int i = 1; i < s.Length; i++)
                {
                    if ((i % 4 == 0) && (s[i] != ' '))
                        str_temp = " " + s[i];
                    else
                        str_temp = s[i].ToString();
                    ss = ss + str_temp;
                }
            }
            return ss;
        }
    }
}
