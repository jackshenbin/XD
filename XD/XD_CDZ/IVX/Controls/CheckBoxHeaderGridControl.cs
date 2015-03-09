using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Diagnostics;
using DevExpress.XtraEditors.Repository;

namespace BOCOM.IVX.Controls
{
    public partial class CheckBoxHeaderGridControl : GridControl
    {
        #region Fields
        
        private GridView m_GridView;
        private ICollection<GridColumn> m_CheckBoxColumns;

        // private RepositoryItemCheckEdit m_respositoryEditorCheckBox = new RepositoryItemCheckEdit();

        private Dictionary<GridColumn, RepositoryItemCheckEdit> m_DTColumn2CheckEdit = new Dictionary<GridColumn,RepositoryItemCheckEdit>();

        private int m_changingrowHandle;

        private GridColumn m_changingColumn;
        
        private bool m_changingCellValue;

        #endregion

        #region Constructors

        public CheckBoxHeaderGridControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Public helper functions
        
        public void Init(ICollection<GridColumn> checkBoxColumns)
        {
            m_GridView = Views[0] as GridView;
            if (m_GridView == null)
            {
                throw new InvalidOperationException("No gridview initialized");
            }

            m_CheckBoxColumns = checkBoxColumns;

            if(checkBoxColumns != null && checkBoxColumns.Count > 0)
            {
                RepositoryItemCheckEdit checkEdit;
                foreach(GridColumn column in checkBoxColumns)
                {
                    checkEdit = new RepositoryItemCheckEdit();
                    checkEdit.Caption = string.Empty;
                    checkEdit.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
                    m_DTColumn2CheckEdit.Add(column, checkEdit);
                }
           
                m_GridView.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(GridView_CellValueChanging);
                m_GridView.MouseDown += new MouseEventHandler(GridView_MouseDown);
                m_GridView.CustomDrawColumnHeader += new ColumnHeaderCustomDrawEventHandler(GridView_CustomDrawColumnHeader);
                m_GridView.RowCountChanged += new EventHandler(GridView_RowCountChanged);
            }
        }

        private void GridView_RowCountChanged(object sender, EventArgs e)
        {
            m_changingrowHandle = -1;
            m_changingColumn = null;
            this.Invalidate();
        }

        #endregion

        #region Private helper functions

        private int getCheckedCount(GridColumn column)
        {
            int count = 0;
            bool bVal;
            for (int i = 0; i < m_GridView.DataRowCount; i++)
            {
                if (m_changingrowHandle == i && column == m_changingColumn)
                {
                    bVal = m_changingCellValue;
                }
                else
                {
                    bVal = (bool)m_GridView.GetRowCellValue(i, m_GridView.Columns[column.FieldName]);
                }

                if (bVal)
                {
                    count++;
                }
                else
                {
                    Debug.WriteLine("kasjdkf");
                }
            }
            return count;
        }

        private void CheckAll(GridColumn column)
        {
            for (int i = 0; i < m_GridView.DataRowCount; i++)
            {
                m_GridView.SetRowCellValue(i, m_GridView.Columns[column.FieldName], true);
            }
        }

        private void UnChekAll(GridColumn column)
        {
            for (int i = 0; i < m_GridView.DataRowCount; i++)
            {
                m_GridView.SetRowCellValue(i, m_GridView.Columns[column.FieldName], false);
            }
        }

        private void DrawCheckBox(Graphics g, Rectangle r, RepositoryItemCheckEdit checkEdit, CheckState state)
        {
            DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo info;
            DevExpress.XtraEditors.Drawing.CheckEditPainter painter;
            DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs args;
            checkEdit.AllowGrayed = true;

            info = checkEdit.CreateViewInfo() as DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo;
            if (state == CheckState.Unchecked)
            {
                info.EditValue = false;
            }
            else if (state == CheckState.Checked)
            {
                info.EditValue = true;
            }
            else
            {
                info.EditValue = null;
            }

            painter = checkEdit.CreatePainter() as DevExpress.XtraEditors.Drawing.CheckEditPainter;

            info.Bounds = r;
            info.PaintAppearance.ForeColor = Color.FromArgb(180, 180, 180);
            info.CalcViewInfo(g);
            args = new DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs(info, new DevExpress.Utils.Drawing.GraphicsCache(g), r);
            painter.Draw(args);
            args.Cache.Dispose();
        }

        #endregion

        #region Event handlers

        void GridView_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (m_CheckBoxColumns == null || m_CheckBoxColumns.Count == 0)
            {
                return;
            }

            if (m_CheckBoxColumns.Contains(e.Column))
            {
                e.Info.InnerElements.Clear();
                e.Info.Appearance.ForeColor = Color.FromArgb(180, 180, 180);
                e.Painter.DrawObject(e.Info);
                CheckState state = CheckState.Checked;
                int count = getCheckedCount(e.Column);

                Debug.WriteLine("### count: " + count);

                if (count == 0)
                {
                    state = CheckState.Unchecked;
                }
                else if (count != m_GridView.DataRowCount)
                {
                    state = CheckState.Indeterminate;
                }
                RepositoryItemCheckEdit checkEdit = m_DTColumn2CheckEdit[e.Column];
                DrawCheckBox(e.Graphics, e.Bounds, checkEdit, state);
                e.Handled = true;
            }
        }

        void GridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_CheckBoxColumns == null || m_CheckBoxColumns.Count == 0)
            {
                return;
            }

            if (e.Clicks == 1 && e.Button == MouseButtons.Left)
            {
                GridHitInfo info;
                Point pt = m_GridView.GridControl.PointToClient(Control.MousePosition);
                info = m_GridView.CalcHitInfo(pt);

                if (info != null && info.Column != null && m_CheckBoxColumns.Contains(info.Column))
                {
                    if (info.InColumn)
                    {
                        m_changingrowHandle = -1;
                        m_changingColumn = null;
                        if (getCheckedCount(info.Column) == m_GridView.DataRowCount)
                        {
                            UnChekAll(info.Column);
                            m_GridView.Invalidate();
                        }
                        else
                        {
                            CheckAll(info.Column);
                            m_GridView.Invalidate();
                        }
                    }
                    else
                    {

                    }
                }

            }
        }

        void GridView_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (m_CheckBoxColumns == null || m_CheckBoxColumns.Count == 0)
            {
                return;
            }

            if (m_CheckBoxColumns.Contains(e.Column))
            {
                m_changingColumn = e.Column;
                m_changingrowHandle = e.RowHandle;
                m_changingCellValue = (bool)e.Value;
                m_GridView.Invalidate();
            }
        }

        #endregion
    }
}
