﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace puller_gui.Presenters
{
    public interface IMainFormPresenter
    {
        void HookView(Views.IMainFormView view);
        void OnClosing();
        void AddFiles(string[] urls);
        void RemoveFile(int fileId);
        void ShowFileInFolder(int fileId);
        void StartDownload();
        void PauseDownload();
        void CheckProgress();
    }
}
