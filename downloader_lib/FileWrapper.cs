using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsLib;

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

        private static string tempExtension = ".part";

        private string url_;
        private string name_;
        AsyncFileDownloader downloader_;

        string tempLocalPath;
        string localPath;

        public FileWrapper(string url, string destPath)
        {
            name_ = Utils.ExtractFileName(destPath, "\\");
            url_ = url;

            tempLocalPath = destPath + tempExtension;
            localPath = destPath;

            downloader_ = new AsyncFileDownloader(url, tempLocalPath, BLOCK_SIZE);
        }

        public string FileName
        {
            get
            {
                return name_;
            }
        }

        public string FilePath
        {
            get
            {
                if (Status == FileStatus.DOWNLOADED)
                {
                    return localPath;
                }
                else
                {
                    return tempLocalPath;
                }
            }
        }

        public string Url
        {
            get
            {
                return url_;
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

        public long DownloadSpeed
        {
            get
            {
                return downloader_.DownloadSpeed;
            }
        }

        public FileStatus Status
        {
            get
            {
                if (downloader_.Finished)
                {
                    if (System.IO.File.Exists(tempLocalPath))
                    {
                        System.IO.File.Move(tempLocalPath, localPath);
                        // TODO: if dest found - rename!
                    }

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
