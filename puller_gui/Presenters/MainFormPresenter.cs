using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using puller_gui.Views;
using downloader_lib;
using UtilsLib;

namespace puller_gui.Presenters
{
    class MainFormPresenter : IMainFormPresenter
    {

        public MainFormPresenter(IDownloadManager downloadManager, IFileSystem fileSystem)
        {
            downloadManager_ = downloadManager;
            fileSystem_ = fileSystem;
        }

        public void HookView(Views.IMainFormView view)
        {
            view_ = view;
        }

        public void OnClosing()
        {
            downloadManager_.Shutdown();
        }

        public void AddFiles(string[] urls)
        {
            foreach (var url in urls)
            {
                try
                {
                    int fileId = downloadManager_.AddFile(url);
                    view_.AddFileToList(
                        fileId,
                        downloadManager_.GetFileName(fileId),
                        UtilsLib.Utils.GetHumanReadableSize(downloadManager_.GetFileSize(fileId)));
                    if (downloadManager_.DownloadInProgress)
                    {
                        downloadManager_.StartDownload(fileId);
                    } else
                    {
                        view_.SetButtonState(MainFormButton.Start, true);
                    }
                }
                catch (Exception e) // top-level error handling
                {
                    view_.ShowError("Error on adding file", $"File {url}: " + e.Message);
                }
            }
        }

        public void RemoveFile(int fileId)
        {
            downloadManager_.RemoveFile(fileId);
            view_.RemoveFileFromList(fileId);

            if (downloadManager_.FilesCount == 0)
            {
                view_.SetButtonState(MainFormButton.Start, false);
            }
        }

        public void ShowFileInFolder(int fileId)
        {
            fileSystem_.ShowFileInFolder(downloadManager_.GetFilePath(fileId));
        }

        public void StartDownload()
        {
            downloadManager_.StartDownload();
            view_.SetStatus("Downloading...");
            view_.SetProgressTimerState(true);
            view_.SetButtonState(MainFormButton.Start, false);
            view_.SetButtonState(MainFormButton.Stop, true);
        }

        public void PauseDownload()
        {
            downloadManager_.PauseDownload();
            view_.SetStatus("Paused");
            view_.SetProgressTimerState(false);
            view_.SetButtonState(MainFormButton.Start, true);
            view_.SetButtonState(MainFormButton.Stop, false);
        }

        public void CheckProgress()
        {
            view_.ShowProgress((int)(downloadManager_.OverallProgress * 1000));

            foreach (int id in downloadManager_.FileIdList)
            {
                view_.ModifyFileListItem(
                    id,
                    Utils.GetHumanReadableSize(downloadManager_.GetFileDownloadedSize(id)),
                    Utils.GetHumanReadableSpeed(downloadManager_.GetFileDownloadSpeed(id)),
                    Utils.GetHumanReadableProgress(downloadManager_.GetFileProgress(id)),
                    downloadManager_.GetFileStatus(id));
            }
                        
            if (downloadManager_.AllDownloadsFinished)
            {
                ProcessDownloadFinished();
            }
        }

        private void ProcessDownloadFinished()
        {
            view_.SetStatus("Ready");
            view_.SetProgressTimerState(false);
            view_.SetButtonState(MainFormButton.Start, true);
            view_.SetButtonState(MainFormButton.Stop, false);
            view_.ShowMessage("Success", "All downloads were finished!");
        }

        Views.IMainFormView view_;
        IDownloadManager downloadManager_;
        IFileSystem fileSystem_;
    }
}
