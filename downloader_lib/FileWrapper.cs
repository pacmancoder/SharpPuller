using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace downloader_lib
{
    enum FileStatus
    {
        READY_TO_DOWNLOAD,
        IN_PROGRESS,
        PAUSED,
        DOWNLOADED,
    }

    class FileWrapper
    {
        public static int PROGRESS_MAX = 1000;
        public static int BLOCK_SIZE = 4096;

        private string url_;
        private string name_;
        AsyncFileDownloader downloader_;

        public FileWrapper(string url, string destFolder)
        {
            name_ = null;
            url_ = url;
            downloader_ = new AsyncFileDownloader(url, destFolder + "/" + Name, BLOCK_SIZE);
        }

        public string url
        {
            get
            {
                return url_;
            }
        }

        public string Name
        {
            get
            {
                if (name_ == null)
                {
                    name_ = url_.Substring(url.LastIndexOf('/') + 1);
                }
                return name_;
            }
        }

        public double Progress
        {
            get
            {
                if (downloader_.FileSize == 0)
                {
                    return 1.0;
                }
                return (double)downloader_.DownloadedSize / downloader_.FileSize;
            }
        }

        public long Size
        {
            get
            {
                return downloader_.FileSize;
            }
        }

        public long DownloadedSize
        {
            get
            {
                return downloader_.DownloadedSize;
            }
        }

        public void StartDownload()
        {
            downloader_.Start();
        }

        public void PauseDownload()
        {
            downloader_.Pause();
        }

        public FileStatus Status
        {
            get
            {
                if (downloader_.Finished)
                {
                    return FileStatus.DOWNLOADED;
                }
                else if (downloader_.DownloadInProgress)
                {
                    return FileStatus.IN_PROGRESS;
                }
                else if (downloader_.DownloadStarted && !downloader_.DownloadInProgress)
                {
                    return FileStatus.PAUSED;
                }
                else
                {
                    return FileStatus.READY_TO_DOWNLOAD;
                }
            }
        }
    }
}
