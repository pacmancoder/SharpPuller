using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UtilsLib;

namespace downloader_lib
{
    public class DownloadManager
    {
        private string downloadFolder_;

        private Dictionary<int, FileWrapper> files_;
        private int nextId_;

        public DownloadManager()
        {
            // Get user's downloads folder path
            downloadFolder_ = Registry.GetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders",
                "{374DE290-123F-4565-9164-39C4925E467B}", "Downloads").ToString();
            nextId_ = 0;
            files_ = new Dictionary<int, FileWrapper>();
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
                // TODO: specific exception
                throw new ApplicationException("File already added!");
            }

            string defaultPath = downloadFolder_ + "\\" + Utils.ExtractFileName(url, "/");
            // TODO: check filename for incorrect symbols, replace with underscores !!!
            string resultPath = defaultPath;
            int fileSuffix = 1;
            while (System.IO.File.Exists(resultPath))
            {
                int separatorPos;
                if (defaultPath.LastIndexOf('.') > defaultPath.LastIndexOf('\\'))
                {
                    separatorPos = defaultPath.LastIndexOf('.');
                }
                else
                {
                    separatorPos = defaultPath.Length - 1;
                }

                resultPath = 
                    defaultPath.Substring(0, separatorPos) +
                    $" ({fileSuffix})" +
                    defaultPath.Substring(separatorPos, defaultPath.Length - separatorPos);
            }


            files_.Add(nextId_, new FileWrapper(url, resultPath));
            return nextId_++;
        }

        public void RemoveFile(int fileId)
        {
            // files_[fileId].StopDownload();
            files_.Remove(fileId);
        }

        public double GetOverallProgress()
        {
            double overallProgress = 0;
            foreach(var file in files_)
            {
                overallProgress += file.Value.Progress;
            }
            return overallProgress / files_.Count;
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

        public void StartDownload()
        {
            foreach(var file in files_)
            {
                file.Value.StartDownload();
            }
        }
        public void PauseDownload()
        {
            foreach (var file in files_)
            {
                file.Value.PauseDownload();
            }
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

        public List<int> GetFileIdList()
        {
            var list = new List<int>();
            foreach (var file in files_)
            {
                list.Add(file.Key);
            }
            return list;
        }

        public long GetFileDownloadSpeed(int id)
        {
            return files_[id].DownloadSpeed;
        }

        public double GetFileProgress(int id)
        {
            return files_[id].Progress;
        }
    }
}
