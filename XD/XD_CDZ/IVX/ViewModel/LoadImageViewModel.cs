using System.ComponentModel;
using System.Drawing;
using BOCOM.IVX.Framework;
using BOCOM.DataModel;
using System.Collections.Generic;

namespace BOCOM.IVX.ViewModel
{
    public class LoadImageViewModel : ViewModelBase
    {
        #region Fields
        private System.Windows.Forms.Control m_PicPlayer;

        private Image m_baseImage = null;

        private Image m_currImage = null;

        private Rectangle m_imageRectangle = new Rectangle(350, 150, 200, 200);
        private int altsize = 20;

        #endregion

        #region Properties
        public Image BaseImage
        {
            get { return m_baseImage; }
            set
            {
                m_baseImage = value; 
            }
        }
        public Image CurrImage
        {
            get
            {
                if (m_currImage == null)
                    m_currImage = m_baseImage;
                return m_currImage; 
            }
            set
            {
                m_currImage = value;
                Framework.Container.Instance.GraphicDrawService.SetPic(m_currImage);
                Framework.Container.Instance.GraphicDrawService.SetPicDrawTypeRect();

            }
        }
        public Rectangle ImageRectangle
        {
            get
            {
                List<Rectangle> rects =  Framework.Container.Instance.GraphicDrawService.GetPicDrawRect();
                if (rects.Count > 0)
                    m_imageRectangle = rects[0];
                else
                    m_imageRectangle = new Rectangle();
                return m_imageRectangle;
            }
            set
            {
                List<Rectangle> rects = new List<Rectangle>();
                rects.Add(value);
                Framework.Container.Instance.GraphicDrawService.SetPicDrawRect(rects);
                m_imageRectangle = value;
            }
        }
        public ImageType CurrImageType { get; set; }
        #endregion

        #region Constructors

        public LoadImageViewModel(System.Windows.Forms.Control c)
        {
            m_PicPlayer = c;

            CurrImageType = ImageType.Common;
        }


        #endregion


        #region Public helper functions


        public Image GetRagionImage()
        {
            if (m_currImage == null)
                return null;
            Rectangle rect = ImageRectangle;

            Bitmap bitmap = new Bitmap(rect.Width + altsize, rect.Height + altsize);
            
            Graphics dc = Graphics.FromImage(bitmap);
            dc.DrawImage(m_currImage, new Rectangle(0, 0, bitmap.Width, bitmap.Height)
                , new Rectangle(rect.X - altsize / 2, rect.Y - altsize / 2, rect.Width + altsize, rect.Height + altsize), GraphicsUnit.Pixel);
            dc.DrawRectangle(new Pen(Color.Red, 3), altsize / 2, altsize / 2, rect.Width, rect.Height);
            dc.Save();
            return bitmap;
        }

        public void ReturnToBaseImage()
        {
            CurrImage = m_baseImage;
        }

        public bool Commit()
        {
            if (!Validate())
                return false;

            Framework.Container.Instance.EvtAggregator.GetEvent<NavigateEvent>().Publish(UIFuncItemInfo.SEARCH);

            Framework.Container.Instance.EvtAggregator.GetEvent<GotoCompareSearchEvent>().Publish("");
            CompareImageInfo info = new CompareImageInfo();
            info.Image= CurrImage;
            info.RegionImage= GetRagionImage();
            info.ImageRectangle= ImageRectangle ;
            info.ImageType = CurrImageType;
            Framework.Container.Instance.EvtAggregator.GetEvent<SetCompareImageInfoEvent>().Publish(info);

            return true;
           
        }
        private bool Validate()
        {
            if (CurrImage == null)
            {
                Framework.Container.Instance.InteractionService.ShowMessageBox("未设置比对图，请选择图片后继续。", Framework.Environment.PROGRAM_NAME, System.Windows.Forms.MessageBoxButtons.OK);
                return false;
            }
            if (ImageRectangle == new Rectangle(0, 0, CurrImage.Width, CurrImage.Height))
            {
                string msg = "未绘制比对框是否继续？";
                if (Framework.Container.Instance.InteractionService.ShowMessageBox(msg, Framework.Environment.PROGRAM_NAME, System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    return false;
            }

            return true;
        }

        public void InitPic()
        {
            Framework.Container.Instance.GraphicDrawService.HPicWnd = m_PicPlayer.Handle;
            Framework.Container.Instance.GraphicDrawService.OpenPic();
        }

        public void ClearPic()
        {
            Framework.Container.Instance.GraphicDrawService.ClosePic();
        }
        #endregion

    }

}


