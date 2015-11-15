using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Design;
using System.Threading;
using DevComponents.DotNetBar;

namespace BOCOM.IVX.Controls
{
    public partial class GridViewEx : DevComponents.DotNetBar.Controls.DataGridViewX
    {

        public GridViewEx()
        {
            InitializeComponent();
            this.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right)));
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.BackgroundColor = System.Drawing.SystemColors.Control;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();

            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(240)))));
            this.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Raised;
            this.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            this.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(240)))));
            
            this.Margin = new System.Windows.Forms.Padding(0);
            this.RowPostPaint += new DataGridViewRowPostPaintEventHandler(GridViewEx_RowPostPaint);
            RowHeadersDefaultCellStyle.Font = new Font(SystemFonts.CaptionFont, FontStyle.Bold);
            RowHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.RoyalBlue;
            
            this.KeyDown += new KeyEventHandler(GridViewEx_KeyDown);
            
        }

        void GridViewEx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C && e.Control)
            {
                Clipboard.Clear();
                StringBuilder sb = new StringBuilder();
                
                int startrow = RowCount;
                int startcolumn = ColumnCount;
                int endrow = 0;
                int endcolumn = 0;
                foreach(DataGridViewCell cell in this.SelectedCells)
                {
                    if (cell.RowIndex < startrow) startrow = cell.RowIndex;
                    if (this.Columns[cell.ColumnIndex].DisplayIndex < startcolumn) startcolumn = this.Columns[cell.ColumnIndex].DisplayIndex;
                    if (cell.RowIndex > endrow) endrow = cell.RowIndex;
                    if (this.Columns[cell.ColumnIndex].DisplayIndex > endcolumn) endcolumn = this.Columns[cell.ColumnIndex].DisplayIndex;
                    
                }
                for (int i = startrow; i <=endrow; i++)
                {
                    for (int j = startcolumn; j <=endcolumn; j++)
                    {
                        DataGridViewCell cell = this[getColumnId(j), i];
                        if (cell.Displayed)
                        {
                            if (this.SelectedCells.Contains(cell))
                            {
                                sb.Append(cell.FormattedValue.ToString() + "\t");
                            }
                            else
                            {
                                sb.Append("\t");
                            }
                        }
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(Environment.NewLine);
                }


                Clipboard.SetText(sb.ToString());
                e.Handled = true;
            }
        }

        int getColumnId(int displayId)
        {
            foreach (DataGridViewColumn c in this.Columns)
            {
                if (c.DisplayIndex == displayId) return c.Index;
            } return 0;
        }

        private bool bAsc = true;

        public List<DataGridViewCell> FindCell(object value)
        {
            List<DataGridViewCell> list = new List<DataGridViewCell>();
            for (int i = 0; i < this.Rows.Count; i++)
            {
                for (int j = 0; j < this.Columns.Count; j++)
                {
                    if (this[j, i].Value == value)
                    {
                        list.Add(this[j, i]);
                    }
                }
            }
            return list;
        }



        void GridViewEx_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if(!IsShowIndex) return;
            GridViewEx costomerDataGridView = sender as GridViewEx;
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                                   e.RowBounds.Location.Y,
                                   costomerDataGridView.RowHeadersWidth - 4,
                                   e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                costomerDataGridView.RowHeadersDefaultCellStyle.Font,
                rectangle,
                costomerDataGridView.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }


        bool isShowIndex = true;
       

        [Category("com")]
        [DefaultValue(true)]
        [Description("是否显示列序号")]
        public bool IsShowIndex
        {
            get
            {
                return isShowIndex;
            }
            set
            {
                isShowIndex = value;
            }
        }


        private void paseData()
        {
            GridCopy(this);
        }
            
             
        static bool GridCopy(DataGridView dg)
        {
            int RowCount = 0;
            int RowMax = 0;
            int RowMin = 0;
            int RowSel = 0;
            int ColumnSel = 0;


            RowSel = 0;
            ColumnSel = 0;

            string[] strTemp = Clipboard.GetText().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            RowCount = strTemp.Length;

            //未选中单元格，从第一个单元格开始复制
            if (dg.SelectedCells.Count == 0)
            {
                if (RowCount > dg.RowCount)
                {
                    string str = "cells_too_few_to_duplicate";
                    MessageBoxEx.Show(str, "Administrator", MessageBoxButtons.OK);
                    return false;
                }

                try
                {
                    foreach (string s in strTemp)
                    {
                        string[] cels = s.Split('\t');
                        for (int i = 0; i < cels.Length; i++)
                        {
                            dg[ColumnSel + i, RowSel].Value = cels[i];
                        }
                        RowSel++;
                    }
                }
                catch
                {
                    dg.EndEdit();
                    return false;
                }
                return true;
            }

            RowMin = dg.SelectedCells[0].RowIndex;
            RowMax = dg.SelectedCells[0].RowIndex;
            foreach (DataGridViewCell cell in dg.SelectedCells)
            {
                if (cell.RowIndex > RowMax) RowMax = cell.RowIndex;
                if (cell.RowIndex < RowMin) RowMin = cell.RowIndex;
            }

            RowSel = dg.SelectedCells[0].RowIndex;
            ColumnSel = dg.SelectedCells[0].ColumnIndex;
            foreach (DataGridViewCell cell in dg.SelectedCells)
            {
                if (cell.ColumnIndex < ColumnSel)
                {
                    ColumnSel = cell.ColumnIndex;
                }
                if (cell.RowIndex < RowSel)
                {
                    RowSel = cell.RowIndex;
                }
            }

            //从选中单元格到结束空间不够
            if (RowSel + RowCount > dg.RowCount)
            {
                string str = "选中的单元格数量过小";
                MessageBoxEx.Show(str, "Administrator", MessageBoxButtons.OK);
                return false;
            }


            //选中单元格空间不够
            if (RowMax - RowMin + 1 < RowCount)
            {
                for (int i = RowMin; i < RowMin + RowCount; i++)
                {
                    dg[ColumnSel, i].Selected = true;
                }
                string str = "选中的单元格数量过小";
                if (MessageBoxEx.Show(str, "Administrator", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
                {
                    for (int i = RowMin; i < RowMin + RowCount; i++)
                    {
                        dg[ColumnSel, i].Selected = false;
                    }
                    return false;
                }
            }

            try
            {
                foreach (string s in strTemp)
                {
                    string[] cels = s.Split('\t');
                    for (int i = 0; i < cels.Length; i++)
                    {
                        dg[ColumnSel + i, RowSel].Value = cels[i];
                    }
                    RowSel++;
                }
            }
            catch
            {
                dg.EndEdit();
                return false;
            }
            return true;
        }

        public string[] GetColumns()
        {

            if (this.Columns.Count == 0) return null;
            string[] strColumnNames = new string[this.Columns.Count];
            int iIndex = 0;

            foreach (System.Windows.Forms.DataGridViewColumn cl in this.Columns)
            {

                    strColumnNames[iIndex++] = cl.HeaderText;

            }
            return strColumnNames;
        }

        public void SetColumnsShow( DevComponents.DotNetBar.ButtonItem button )
        {
             foreach (DevComponents.DotNetBar.ButtonItem item in button.SubItems)
            {
                foreach (System.Windows.Forms.DataGridViewColumn cl in this.Columns)
                {
                    if (cl.HeaderText.CompareTo(item.Text) == 0)
                    {
                        cl.Visible = item.Checked;
                    }
                }

             }
           
            
        }


        private void GridViewEx_BackgroundColorChanged(object sender, EventArgs e)
        {
            //this.BackgroundColor = System.Drawing.Color.Gainsboro;//.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(240)))));
            this.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(240)))));

        }

    }



}
