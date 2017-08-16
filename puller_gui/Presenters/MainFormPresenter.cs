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
    class MainFormPresenter
    {
        DownloadManager downloadManager = new DownloadManager();

        public void HookView(Views.IMainFormView view)
        {
            view_ = view;
        }

        public void AddFile(string url)
        {
            try
            {
                int fileId = downloadManager.AddFile(url);
                view_.AddFileToList(
                    fileId,
                    downloadManager.GetFileName(fileId),
                    UtilsLib.Utils.GetHumanReadableSize(downloadManager.GetFileSize(fileId)));
            }
            // other errors
            catch (Exception e)
            {
                view_.ShowError("Error on adding file", e.Message);
            }
        }

        public void RemoveFile(int fileId)
        {
            downloadManager.RemoveFile(fileId);
            view_.RemoveFileFromList(fileId);
        }

        public void StartDownload()
        {
            downloadManager.StartDownload();
            view_.SetStatus("Downloading...");
            view_.SetProgressTimerState(true);
            view_.SetButtonState(MainFormButton.Start, false);
            view_.SetButtonState(MainFormButton.Stop, true);
        }

        public void PauseDownload()
        {
            downloadManager.PauseDownload();
            view_.SetStatus("Paused");
            view_.SetProgressTimerState(false);
            view_.SetButtonState(MainFormButton.Start, true);
            view_.SetButtonState(MainFormButton.Stop, false);
        }

        public void CheckProgress()
        {
            view_.ShowProgress((int)(downloadManager.GetOverallProgress() * 1000));

            var fileIds = downloadManager.GetFileIdList();
            foreach (int id in fileIds)
            {
                view_.ModifyFileListItem(
                    id,
                    Utils.GetHumanReadableSize(downloadManager.GetFileDownloadedSize(id)),
                    downloadManager.GetFileDownloadSpeed(id),
                    Utils.GetHumanReadableProgress(downloadManager.GetFileProgress(id)),
                    downloadManager.GetFileStatus(id));
            }
                        
            if (downloadManager.AllDownloadsFinished)
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
    }
}
