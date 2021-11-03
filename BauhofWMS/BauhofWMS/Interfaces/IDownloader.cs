using System;
using System.Collections.Generic;
using System.Text;


namespace BauhofWMS
{
    public interface IDownloader
    {
        void DownloadFile(string url);
        event EventHandler<DownloadEventArgs> OnFileDownloaded;

        void LaunchLocalFile(string fileName);
    }
}
