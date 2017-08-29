using System.IO;
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

    enum PauseType
    {
        Relaxed,
        Forced,
    }

    class FileWrapper
    {
        private static string tempExtension = ".part";

        private string url_;
        private string name_;

        string curentLocalPath;
        string localPath;

        IAsyncDownloader downloader_;
        IFileSystem fileSystem_;

        public FileWrapper(string url, string destPath, IAsyncDownloaderFactory downloaderFactory, IFileSystem fileSystem)
        {
            url_ = url;
            localPath = destPath;
            curentLocalPath = Utils.GenerateUniqueFileName(localPath + tempExtension);
            name_ = Path.GetFileName(localPath);

            downloader_ = downloaderFactory.GetDownloader(url, curentLocalPath, fileSystem);
            fileSystem_ = fileSystem;
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
                return curentLocalPath;
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

        public void PauseDownload(PauseType pauseType)
        {
            downloader_.Pause();

            if (pauseType == PauseType.Forced)
            {
                downloader_.Join();
            }
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
                    if (fileSystem_.Exists(curentLocalPath))
                    {
                        fileSystem_.Move(curentLocalPath, localPath);
                        curentLocalPath = localPath;
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
