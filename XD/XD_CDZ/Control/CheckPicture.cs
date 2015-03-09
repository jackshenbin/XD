using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace BOCOM.IVX.Controls
{
    public partial class CheckedPictureBox : System.Windows.Forms.PictureBox
    {

        private int m_group = -1;

        public   CheckedPictureBox()
        {
            InitializeComponent();
            init();
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            // TODO: 在此处添加自定义绘制代码

            // 调用基类 OnPaint
            try
            {
                base.OnPaint(pe);
            }
            catch
            {
                this.Invalidate();
            }
        }

        [Category("Image")]
        public new event EventHandler Click;
        [Category("Image")]
        public new event EventHandler DoubleClick;
        [Category("Image")]
        public event EventHandler CheckedChanged;
        void init()
        {
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(CheckedPicture_MouseMove);
            this.MouseLeave += new EventHandler(CheckedPicture_MouseLeave);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(CheckedPicture_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(CheckedPicture_MouseDoubleClick);
            this.EnabledChanged += new EventHandler(CheckedPicture_EnabledChanged);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(CheckedPictureBox_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(CheckedPictureBox_MouseUp);
        }

        void CheckedPictureBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void CheckedPictureBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (CheckedImage != null && Image != CheckedImage) Image = CheckedImage;
        }

        void CheckedPicture_EnabledChanged(object sender, EventArgs e)
        {
            if (Enabled)
            {
                if (m_checked)
                {
                    if (m_checkedImage != null && Image != m_checkedImage) Image = m_checkedImage;
                }
                else
                {
                    if (Image != m_orignalImage) Image = m_orignalImage;
                }
            }
            else
            {
                if (m_disableImage != null && Image != m_disableImage) Image = m_disableImage;
            }
        }

        void CheckedPicture_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if(DoubleClick!=null)
                DoubleClick(sender, e);
        }

        void CheckedPicture_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (m_allowChecked)
            {
                if (CheckPictureType == eCheckPictureType.RadioButton && m_checked)
                {
                }
                else
                {
                    Checked = !m_checked;
                }
            }
            if (Click != null)
                Click(sender, e);
        }

        void CheckedPicture_MouseLeave(object sender, EventArgs e)
        {
            if (m_checked )
            {
                if (m_checkedImage != null && Image != m_checkedImage) Image = m_checkedImage;
            }
            else
            {
                if ( Image != m_orignalImage) Image = m_orignalImage;
            }
            if (!Enabled)
            {
                if (m_disableImage != null && Image != m_disableImage) Image = m_disableImage;

            }
        }

        void CheckedPicture_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!m_checked)
            {
                if (m_mouseOverImage != null && Image != m_mouseOverImage) Image = m_mouseOverImage;
            }
            if (!Enabled)
            {
                if (m_disableImage != null && Image != m_disableImage) Image = m_disableImage;

            }
        }
        private Image m_orignalImage = null;
        [Category("Image")]
        public Image OrignalImage
        {
            get { return m_orignalImage; }
            set { m_orignalImage = value; }
        }
        private Image m_checkedImage = null;

        [Category("Image")]
        public Image CheckedImage
        {
            get { return m_checkedImage; }
            set { m_checkedImage = value; }
        }
        private Image m_mouseOverImage = null;

        [Category("Image")]
        public Image MouseOverImage
        {
            get { return m_mouseOverImage; }
            set { m_mouseOverImage = value; }
        }
        private Image m_disableImage = null;

        [Category("Image")]
        public Image DisableImage
        {
            get { return m_disableImage; }
            set { m_disableImage = value; }
        }
        public void SetImage(Image image, Image checkedimage = null, Image mouseoverimage = null, Image disableimage = null)
        {
            Image = image;
            m_orignalImage = image;
            m_checkedImage = checkedimage;
            m_mouseOverImage = mouseoverimage;
            
            m_disableImage = disableimage;
            

        }
        public void SetCheckedImage(Image image)
        {
            m_checkedImage = image;
        }
        public void SetMouseOverImage(Image image)
        {
            m_mouseOverImage = image;
        }
        public void SetDisableImage(Image image)
        {
            m_disableImage = image;
        }
        private bool m_checked = false;

        [DefaultValue(false)]
        [Category("Image")]
        public bool Checked
        {
            get { return m_checked; }
            set 
            {
                if (!m_allowChecked)
                    return;

                if (m_checked != value)
                {
                    m_checked = value;
                    if (CheckedChanged != null)
                        CheckedChanged(this, null);
                }

                if (m_checked)
                {
                    if (m_checkedImage != null && Image != m_checkedImage) Image = m_checkedImage;
                    SetGroupItemUnchecked();
                }
                else
                {
                    if (Image != m_orignalImage) Image = m_orignalImage;
                }
                if (!Enabled)
                {
                    if (m_disableImage != null && Image != m_disableImage) Image = m_disableImage;

                }

            }
        }

        private bool m_allowChecked = false;

        [DefaultValue(false)]
        [Category("Image")]
        public bool AllowChecked
        {
            get { return m_allowChecked; }
            set 
            {
                m_allowChecked = value;
                if (!m_allowChecked)
                {
                    if (m_checked)
                    {
                        m_checked = false;
                        if (CheckedChanged != null)
                            CheckedChanged(this, null);
                        if (Image != m_orignalImage) Image = m_orignalImage;

                    }
                }
            }
        }

        [Category("Image")]
        public int Group 
        {
            get { return m_group; }
            set { m_group = value; }
        }

        public enum eCheckPictureType
        {
            CheckBox,
            RadioButton,
        }
        [DefaultValue(eCheckPictureType.CheckBox)]
        [Category("Image")]
        public eCheckPictureType CheckPictureType { get; set; }

        private void SetGroupItemUnchecked()
        {
            if (m_group == -1)
            {
                return;
            }
            else
            {
                IEnumerable<CheckedPictureBox> list = this.Parent.Controls.OfType<CheckedPictureBox>();
                foreach (CheckedPictureBox item in list)
                {
                    if (item.Group == m_group && item != this)
                    {
                        item.Checked = false;
                    }
                }
            }
        }

    }
}
