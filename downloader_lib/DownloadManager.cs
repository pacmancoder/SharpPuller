using System;
using System.Collections.Generic;
using System.Linq;

using UtilsLib;

namespace downloader_lib
{
    public class DownloadManager : IDownloadManager
    {

        public DownloadManager(
            IAsyncDownloaderFactory downloaderFactory,
            IFileSystem fileSystem)
        {
            downloaderFactory_ = downloaderFactory;
            fileSystem_ = fileSystem;
        }

        public string DownloadsFolder
        {
            get
            {
                if (downloadsFolder_ == null)
                {
                    throw new InvalidOperationException("Downloads folder for download manager is not set");
                }
                return downloadsFolder_;
            }
            set
            {
                downloadsFolder_ = value;
            }
        }

        private bool FileAlreadyAdded(string url)
        {
            foreach (var file in files_)
            {
                if (file.Value.Url == url)
                {
                    return true;
                }
            }
            return false;
        }

        public int AddFile(string url)
        {
            if (FileAlreadyAdded(url))
            {
                throw new InvalidOperationException($"File with url {url} already added!");
            }

            string filePath = Utils.GenerateUniqueFileName(
                downloadsFolder_ + "\\" + Utils.ExtractFileNameFromUrl(url));

            files_.Add(nextId_, new FileWrapper(url, filePath, downloaderFactory_, fileSystem_));

            return nextId_++;
        }

        public void RemoveFile(int fileId)
        {
            files_[fileId].PauseDownload(PauseType.Forced);
            files_.Remove(fileId);
        }

        public string GetFileName(int id)
        {
            return files_[id].FileName;
        }

        public string GetFilePath(int id)
        {
            return files_[id].FilePath;
        }

        public long GetFileSize(int id)
        {
            return files_[id].Size;
        }
        public long GetFileDownloadedSize(int id)
        {
            return files_[id].DownloadedSize;
        }
        public string GetFileStatus(int id)
        {
            FileStatus status = files_[id].Status;
            switch (files_[id].Status)
            {
                case FileStatus.READY_TO_DOWNLOAD:
                    {
                        return "In queue";
                    }
                case FileStatus.IN_PROGRESS:
                    {
                        return "Downloading";
                    }
                case FileStatus.PAUSED:
                    {
                        return "Paused";
                    }
                case FileStatus.DOWNLOADED:
                    {
                        return "Finished";
                    }
            }
            return "Unknown";
        }

        public long GetFileDownloadSpeed(int id)
        {
            return files_[id].DownloadSpeed;
        }

        public double GetFileProgress(int id)
        {
            return files_[id].Progress;
        }

        public void StartDownload()
        {
            foreach (var file in files_)
            {
                file.Value.StartDownload();
            }
        }
        public void PauseDownload()
        {
            foreach (var file in files_)
            {
                file.Value.PauseDownload(PauseType.Relaxed);
            }
        }

        public void Shutdown()
        {
            foreach (var file in files_)
            {
                file.Value.PauseDownload(PauseType.Forced);
            }
        }

        public void StartDownload(int id)
        {
            files_[id].StartDownload();
        }

        public void PauseDownload(int id)
        {
            files_[id].PauseDownload(PauseType.Relaxed);
        }

        public bool AllDownloadsFinished
        {
            get
            {
                foreach (var file in files_)
                {
                    if (file.Value.Status != FileStatus.DOWNLOADED)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool DownloadInProgress
        {
            get
            {
                foreach (var file in files_)
                {
                    if (file.Value.Status == FileStatus.IN_PROGRESS)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public double OverallProgress
        {
            get
            {
                if (files_.Count == 0)
                {
                    return 1.0;
                }

                long sizeSum = 0;
                foreach (var file in files_)
                {
                    sizeSum += file.Value.Size;
                }

                double progressSum = 0;
                if (sizeSum != 0)
                {
                    foreach (var file in files_)
                    {
                        progressSum += (file.Value.Progress * file.Value.Size) / sizeSum;
                    }
                }
                if (progressSum > 1.0)
                {
                    return 1.0;
                }
                else
                {
                    return progressSum;
                }
            }
        }

        public List<int> FileIdList
        {
            get
            {
                return files_.Keys.ToList();
            }
        }

        public int FilesCount
        {
            get
            {
                return files_.Keys.Count;
            }
        }

        #region private properties

        private int NextId
        {
            get
            {
                return nextId_++;
            }
        }

        #endregion

        #region private fields

        IAsyncDownloaderFactory downloaderFactory_;
        IFileSystem fileSystem_;

        private Dictionary<int, FileWrapper> files_ = new Dictionary<int, FileWrapper>();
        private int nextId_ = 0;
        private string downloadsFolder_ = null;



        #endregion
    }
}
