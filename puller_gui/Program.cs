using downloader_lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilsLib;

namespace puller_gui
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            DownloadManager downloadManager = new DownloadManager(
                new AsyncDownloaderFactory(), new NativeFileSystem());
            downloadManager.DownloadsFolder = Utils.GetDefaultDownloadsFolder();
            Presenters.MainFormPresenter mainFormPresenter = 
                new Presenters.MainFormPresenter(downloadManager);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(mainFormPresenter));
        }
    }
}
