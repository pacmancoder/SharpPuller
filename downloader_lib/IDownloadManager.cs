using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace downloader_lib
{
    public interface IDownloadManager
    {
        string DownloadsFolder { get; set; }
        double OverallProgress { get; }
        bool   AllDownloadsFinished { get; }

        int  AddFile(string url);
        void RemoveFile(int fileId);

        List<int> FileIdList { get; }

        string GetFileName(int id);
        string GetFilePath(int id);
        long   GetFileSize(int id);
        long   GetFileDownloadedSize(int id);
        string GetFileStatus(int id);
        long   GetFileDownloadSpeed(int id);
        double GetFileProgress(int id);

        void StartDownload();
        void PauseDownload();
    }
}
