using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Globalization;
using Aspose.Cells;

namespace BOCOM.IVX.Common
{
    public class ExcelExport
    {
        public static string ExportToCVS(DataGridView view1)
        {
            StringBuilder strLine = new StringBuilder();
            try
            {
                //Write in the headers of the columns.
                for (int i = 0; i < view1.Columns.Count; i++)
                {
                    if (!view1.Columns[i].Visible)
                        continue;

                    strLine.Append(view1.Columns[i].HeaderText + "\t");
                }

                strLine.AppendLine();
                //Write in the content of the columns.
                for (int j = 0; j < view1.Rows.Count; j++)
                {
                    string Line = "";
                    for (int k = 0; k < view1.Columns.Count; k++)
                    {
                        if (!view1.Columns[k].Visible)
                            continue;
                        if (view1.Rows[j].Cells[k].Value == null)
                            Line += " \t";
                        else
                        {
                            string m = view1.Rows[j].Cells[k].Value.ToString().Trim();
                            Line += m.Replace(",", "，") + "\t";
                        }
                    }
                    strLine.AppendLine(Line);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exporting Error", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            return strLine.ToString();
        }

        public static void ReportToExcel(DataGridView view1, string filename)
        {
            //获取用户选择的excel文件名称
            string path = filename;
            {
                Workbook wb = new Workbook();
                Worksheet ws = wb.Worksheets[0];
                Cells cell = ws.Cells;
                //定义并获取导出的数据源
                string[,] _ReportDt = new string[view1.RowCount,view1.ColumnCount];
                //设置行高
                cell.SetRowHeight(0, 20);
                      
                int i=0;
                //设置Execl列名
                foreach (DataGridViewColumn item in view1.Columns)
	            {
		            if(!item.Visible)
                        continue;
                                        
                    cell[0, i++].PutValue(item.HeaderText);
	            }

                int x=0;
                int y=1;

                //设置单元格内容
                foreach (DataGridViewRow itemr in view1.Rows)
                {
                    foreach (DataGridViewColumn itemc in view1.Columns)
                    {
                        if (!itemc.Visible)
                            continue;
                        if (view1[itemc.Index, itemr.Index].Value == null)
                        {
                            cell[y,x++].PutValue(""); 
                        }
                        else
                        {
                            cell[y,x++].PutValue(view1[itemc.Index, itemr.Index].Value.ToString());
                        }
                    }
                    y++;
                    x = 0;
                }

                //保存excel表格
                wb.Save(path);
            }
        }

    }
}