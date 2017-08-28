using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace downloader_lib
{
    public interface IAsyncDownloader
    {
        string Url { get; }
        string DestPath { get; }

        long DownloadSpeed  { get; }
        long DownloadedSize { get; }
        long FileSize       { get; }

        bool Finished           { get; }
        bool DownloadInProgress { get; }
        bool DownloadStarted    { get; }

        void Start();
        void Pause();
        void Join();
    }
}
