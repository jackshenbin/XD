using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;

namespace BOCOM.IVX.Controls
{
    public class FilterableTreeList : TreeList
    {
        private string m_FilterText;

        private object m_columnName2Filter;

        private bool m_isMatchInContainMode = true;

        public object ColumnName2Filter
        {
            get { return m_columnName2Filter; }
            set { m_columnName2Filter = value; }
        }

        public bool IsMatchInContainMode
        {
            get { return m_isMatchInContainMode; }
            set { m_isMatchInContainMode = value; }
        }

        public string FilterText
        {
            get
            {
                return m_FilterText;
            }
            set
            {
                if (string.Compare(m_FilterText, value, true) != 0)
                {
                    m_FilterText = value;
                }
            }
        }

        public FilterableTreeList()
            : base()
        {
            OptionsBehavior.EnableFiltering = true;
            FilterNode += new FilterNodeEventHandler(FilterableTreeList_FilterNode);
        }

        #region Private helper functions

        private bool IsMatch(TreeListNode node)
        {
            bool matched = true;

            if (!string.IsNullOrEmpty(m_FilterText))
            {
                string nodeVal = node.GetDisplayText(m_columnName2Filter);
                if (m_isMatchInContainMode)
                {
                    matched = nodeVal.Contains(this.m_FilterText);
                }
                else
                {
                    matched = nodeVal.StartsWith(this.m_FilterText);
                }
            }

            return matched;
        }

        private bool HasMatchedChildNode(TreeListNode node)
        {
            bool matched = false;
            foreach (TreeListNode child in node.Nodes)
            {
                if (IsMatch(child))
                {
                    matched = true;
                    break;
                }
            }
            if (!matched)
            {
                //Recursive check child node
                foreach (TreeListNode child in node.Nodes)
                {
                    if (HasMatchedChildNode(child))
                    {
                        matched = true;
                        break;
                    }
                }
            }
            return matched;
        }

        #endregion

        void FilterableTreeList_FilterNode(object sender, FilterNodeEventArgs e)
        {
            bool matched = IsMatch(e.Node);
            
            e.Node.Visible = true;
            
            if (matched)
            {
                e.Node.Visible = true;
            }
            else
            {
                if (e.Node.HasChildren)
                {
                    if (HasMatchedChildNode(e.Node))
                    {
                        e.Node.Visible = true;
                    }
                    else
                    {
                        e.Node.Visible = false;
                    }
                }
                else
                {
                    e.Node.Visible = false;
                }
            }
            e.Handled = true;
        }

    }
}
