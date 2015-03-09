using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace BOCOM.DataModel
{
    public interface IVAResultRecord : IDisposable
    {
        IntPtr IdPtr { get; set; }

        int Id { get; }

        int VideoSourceId {get;}

        int SearchId { get; set; }

        /// <summary>
        /// 以图搜图时的源图片
        /// </summary>
        Image SourcePic { get; set; }

        Image ThumbnailPic
        {
            get;
            set;
        }

        Image OriginalPic
        {
            get;
            set;
        }

        void RetrieveImage();
    }



}
