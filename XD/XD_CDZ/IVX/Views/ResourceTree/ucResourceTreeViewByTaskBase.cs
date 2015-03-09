using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BOCOM.DataModel;
using DevExpress.XtraBars;
using BOCOM.IVX.Framework;
using BOCOM.IVX.ViewModel;
using DevExpress.XtraTreeList.Nodes;
using System.Diagnostics;

namespace BOCOM.IVX.Views.ResourceTree
{
    public partial class ucResourceTreeViewByTaskBase : DevExpress.XtraEditors.XtraUserControl
    {
        #region Fields
        
        private bool isDragDrop = false;
        private Point startPoint = new Point();
        private DevExpress.XtraTreeList.Nodes.TreeListNode dragNode = null;

        private VideoPictureTreeViewByTaskModelBase m_viewModelbase;

        private bool m_ShowImage = true;

        #endregion

        #region Properties

        public VideoPictureTreeViewByTaskModelBase ViewModelbase
        {
            get { return m_viewModelbase; }
            set
            {
                if (value != null)
                {
                    m_viewModelbase = value;
                    //m_viewModelbase.TaskUnitDeleted+=new EventHandler<EventDeleteNodeArgs>(m_viewModelbase_TaskUnitDeleted); 
                    //m_viewModelbase.TaskUnitAdded+=new EventHandler<EventAddNodeArgs>(m_viewModelbase_TaskUnitAdded);
                    //m_viewModelbase.TaskUnitEdited += new EventHandler<EventEditNodeArgs>(m_viewModelbase_TaskUnitEdited);
                    m_viewModelbase.TreeNodeAdded += new EventHandler<EventAddNodeArgs>(m_viewModelbase_TaskUnitAdded);
                    m_viewModelbase.TreeNodeDeleted += new EventHandler<EventDeleteNodeArgs>(m_viewModelbase_TaskUnitDeleted);
                    m_viewModelbase.TreeNodeEdited += new EventHandler<EventEditNodeArgs>(m_viewModelbase_TaskUnitEdited);
                }
            }
        }

        [Category("Appearance")]
        public string Caption
        {
            get
            {
                return labelControl1.Text;
            }
            set
            {
                labelControl1.Text = value;
            }
        }

        [Category("Appearance")]
        public bool ShowCheckBoxes
        {
            get
            {
                return treeList1.OptionsView.ShowCheckBoxes;
            }
            set
            {
                treeList1.OptionsView.ShowCheckBoxes = value;
                if (m_viewModelbase != null)
                    m_viewModelbase.IsForVideoSearch = value;
            }
        }

        public bool ShowTitle
        {
            get
            {
                return labelControl1.Visible;
            }
            set
            {
                labelControl1.Visible = value;
            }
        }

        public bool ShowImage
        {
            get
            {
                return m_ShowImage;
            }
            set
            {
                m_ShowImage = value;
                if (!m_ShowImage)
                {
                    treeList1.SelectImageList = null;
                }
            }
        }

        public bool MultiSelect
        {
            get
            {
                return this.treeList1.OptionsSelection.MultiSelect;
            }
            set
            {
                this.treeList1.OptionsSelection.MultiSelect = value;
            }
        }

        #endregion

        #region Constructors

        public ucResourceTreeViewByTaskBase()
        {
            InitializeComponent();
        }

        #endregion

        #region Public helper functions
        
        public void InitRootFolders()
        {
            this.treeList1.BeginUnboundLoad();

            foreach (VideoPictureResource root in m_viewModelbase.RootResources)
            {
                AddNode(root, -1);
            }

            this.treeList1.EndUnboundLoad();
        }

        #endregion

        #region Private helper functions

        private void AddNode(VideoPictureResource vpResource, int parentNodeId)
        {
            TreeListNode node = treeList1.AppendNode(new object[] { vpResource, vpResource.Name, vpResource.Type, vpResource.Subject }, 
                parentNodeId, ShowImage?vpResource.ImageIndex : -1, ShowImage?vpResource.SelectImageIndex : -1, -1);
            
            bool isLeaf = false;
            if (m_viewModelbase.ShowObjectType == TreeShowType.Camera)
            {
                isLeaf = vpResource.Type == ResourceType.Task;
            }
            else
            {
                isLeaf = (vpResource.Type == ResourceType.VideoFile || vpResource.Type == ResourceType.PicSet);
            }

            node.HasChildren = !isLeaf;
            node.Tag = vpResource;
            vpResource.TreeNode = node;
            vpResource.DisplayIndex = Common.Utils.GetNodeDisplayIndex(node);

            if (vpResource.Children.Count > 0 && node.Nodes.Count == 0)
            {
                foreach (VideoPictureResource child in vpResource.Children)
                {
                    AddNode(child, node.Id);
                }
            }

        }


        private void FillupChildNodes(TreeListNode parentNode)
        {
            VideoPictureResource resource = parentNode[0] as VideoPictureResource;
            if (resource != null)
            {
                m_viewModelbase.RetrieveChildren(resource);
                if (resource.Children.Count > 0 && parentNode.Nodes.Count == 0)
                {
                    foreach (VideoPictureResource child in resource.Children)
                    {
                        AddNode(child, parentNode.Id);
                    }
                }
            }
        }

        #endregion

        #region Event handlers

        private void treeList1_SelectionChanged(object sender, EventArgs e)
        {
            List<object> camlist = new List<object>();
            foreach (DevExpress.XtraTreeList.Nodes.TreeListNode n in treeList1.Selection)
            {
                object o = n["Context"];
                if(o!=null && !camlist.Contains(o))
                    camlist.Add(o);
            }
            m_viewModelbase.CameraSelectionChanged(camlist);
            
        }

        private void treeList1_BeforeExpand(object sender, DevExpress.XtraTreeList.BeforeExpandEventArgs e)
        {
            //FillupChildNodes(e.Node);
        }
        
        private void treeList1_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            VideoPictureResource resource = e.Node.Tag as VideoPictureResource;
            if (resource != null)
            {
                if (e.Node.Checked )
                {
                    int count = CalcCheckedNodeCount(treeList1.Nodes);

                    if (count > Common.Constant.TASKUNIT_MAXIMUM_SEARCH)
                    {
                        string msg = string.Format("一次检索最多不能超过 {0} 路视频", Common.Constant.TASKUNIT_MAXIMUM_SEARCH);
                        Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME);
                        e.Node.Checked = false;
                    }
                }

                m_viewModelbase.UpdateCheckedResources(resource, e.Node.Checked);
            }
        }

        private int CalcCheckedNodeCount(TreeListNodes nodes)
        {
            int count =0;
            foreach(TreeListNode n in nodes)
            {
                VideoPictureResource resource = n.Tag as VideoPictureResource;

                if (n.HasChildren)
                    count += CalcCheckedNodeCount(n.Nodes);
                else
                    if (n.Checked && resource.Type == ResourceType.VideoFile)
                        count++;
            }
            return count;
        }
        
        private void treeList1_MouseDown(object sender, MouseEventArgs e)
        {
            DevExpress.XtraTreeList.TreeListHitInfo info =  treeList1.CalcHitInfo(e.Location);
            if (info.Node != null)
            {
                object o = info.Node["Context"];
                if (o is TaskUnitInfo)
                {
                    isDragDrop = true;
                    startPoint = e.Location;
                    dragNode = info.Node;
                }
            }
        }

        private void treeList1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragDrop)
            {
                if (Math.Abs(startPoint.X - e.Location.X) > 5 || Math.Abs(startPoint.Y - e.Location.Y) > 5)
                {
                    List<object> camlist = new List<object>();
                    if (dragNode.HasChildren)
                    {
                        foreach (DevExpress.XtraTreeList.Nodes.TreeListNode node in dragNode.Nodes)
                        {
                            object o = node["Context"];
                            if (!camlist.Contains(o))
                                camlist.Add(o);
                        }
                    }
                    else
                    {
                        object o = dragNode["Context"];
                        if (!camlist.Contains(o))
                            camlist.Add(o);

 
                    }
                    treeList1.DoDragDrop(camlist, DragDropEffects.Move | DragDropEffects.Link);
                    isDragDrop = false;
                    startPoint = new Point();
                    dragNode = null;
                }
            }

        }

        private void treeList1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragDrop = false;
            startPoint = new Point();
            dragNode = null;

        }

        private void treeList1_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            // Sets the custom cursor based upon the effect.
            e.UseDefaultCursors = false;
            System.IO.MemoryStream ms;
            if ((e.Effect & DragDropEffects.Move) == DragDropEffects.Move)
                ms = new System.IO.MemoryStream(Properties.Resources.CAM);
            else
                ms = new System.IO.MemoryStream(Properties.Resources.CHANGE);


            Cursor.Current = new Cursor(ms);

        }
        
        void m_viewModelbase_TaskUnitEdited(object sender, EventEditNodeArgs e)
        {
            VideoPictureResource child = e.NodeResource as VideoPictureResource;
            if (child != null && child.TreeNode != null)
            {
                TreeListNode node = child.TreeNode as TreeListNode;
                node[0] = child;
                node[1] = child.Name;
                node[3] = child.Subject;
                node.Tag = child;
                node.HasChildren = child.Type != ResourceType.VideoFile;

            }
        }

        void m_viewModelbase_TaskUnitAdded(object sender, EventAddNodeArgs e)
        {

            TreeListNode pnode = e.ParantTreeNode as TreeListNode;

            VideoPictureResource child = e.NodeResource as VideoPictureResource;
            if (child != null && child.TreeNode == null)
            {
                AddNode(child, pnode!=null?pnode.Id:-1);
            }
        }

        void m_viewModelbase_TaskUnitDeleted(object sender, EventDeleteNodeArgs e)
        {
            if (e.NodeResource != null)
            {
                TreeListNode node = e.NodeResource.TreeNode as TreeListNode;
                if (node != null )
                {
                    if(node.ParentNode == null)
                    {
                        treeList1.Nodes.Remove(node);
                    }
                    else
                    {
                        node.ParentNode.Nodes.Remove(node);
                    }
                }
            }
        }
        
        #endregion

        private void ucResourceTreeViewBase_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                if (m_viewModelbase != null)
                {
                    InitRootFolders();
                }
            }
        }

        private void treeList1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //DevExpress.XtraTreeList.TreeListHitInfo info = treeList1.CalcHitInfo(e.Location);
            //if (info.Node != null)
            //{
            //    object o = info.Node["Context"];
            //    if (o is TaskUnitInfo)
            //    {
            //    }
            //}

        }

    }
}
