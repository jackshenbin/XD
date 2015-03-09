using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.ViewModel
{
    public class DownloadItemViewModel
    {
        private  Dictionary<int, DownloadItemInfo> downloadFileList = new Dictionary<int, DownloadItemInfo>();

        public void UpdateProgress(DownloadItemInfo info)
        {
            if (downloadFileList.ContainsKey(info.hrItem))
            {
                downloadFileList[info.hrItem] = info;
            }
            else
            {
                downloadFileList.Add(info.hrItem, info);
            }
           //form.UpdateDownloadProgress(info.hrItem, info.progress);
            ShowDownloadInfoWnd();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hrItem"></param>
        /// <param name="progress">-1标示下载失败，100标示下载完成</param>
        public void UpdateProgress(int hrItem,int progress)
        {

            if (downloadFileList.ContainsKey(hrItem))
            {
                DownloadItemInfo info = downloadFileList[hrItem];
                info.progress = progress;
                form.UpdateDownloadProgress(hrItem, progress);
                if (Framework.Environment.PRODUCT_TYPE == Framework.Environment.E_PRODUCT_TYPE.SH_PRODUCT)
                {
                    if (progress == 100)
                    {
                        string filename = info.downloadPath + "\\" + info.dstName;
                        int type = 0;
                        switch (info.type)
                        {
                            case DownloadType.视频截图:
                            case DownloadType.结果图片:
                                type = 1026;
                                break;
                            case DownloadType.浓缩导出:
                            case DownloadType.视频剪辑:
                                type = 1036;
                                break;
                        }
                        IR_SDK.IR_UploadVideoFile(filename, type);
                        MyLog4Net.Container.Instance.Log.Debug("IR_UploadVideoFile:" + filename + " type:" + type);

                    }
                }

            }
        }
        public List<DownloadItemInfo> GetAllDownloadItemInfo()
        {
            return downloadFileList.Values.ToList();
        }
        public DownloadItemInfo GetDownloadItemInfoById(int hrItem)
        {
            if (downloadFileList.ContainsKey(hrItem))
                return downloadFileList[hrItem];
            else
                return null;
        }
        public string GetDownloadTypeName(DownloadType type)
        {
            return type.ToString();
        }
        private BOCOM.IVX.Controls.FormDownloadInfo form = new Controls.FormDownloadInfo();

        public void ShowDownloadInfoWnd()
        {
            if (form.IsDisposed)
                return;

            if (form.InvokeRequired)
            {
                try
                {
                    form.Invoke(new Action(ShowDownloadInfoWnd));
                    return;
                }
                catch
                {
                }
            }

            if (!form.Visible)
                form.Show();
            form.BringToFront();
        }
    }

       public   enum DownloadType
        {
            浓缩导出,
            结果图片,
            视频截图,
            视频剪辑,
            摘要导出,
        }
   
    public class DownloadItemInfo
    {
        public int hrItem;
        public string srcVideoName;
        public string dstName;
        public int progress;
        public string downloadPath;
        public DownloadType type;

    };

}
