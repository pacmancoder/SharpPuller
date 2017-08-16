using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace puller_gui.Views
{
    public enum MainFormButton
    {
        Start,
        Stop,
        Add,
    }

    interface IMainFormView
    {
        void AddFileToList(int id, string fileName, string size);
        void RemoveFileFromList(int id);
        void ModifyFileListItem(int id, string downloadedSize, string speed, string progress, string status);
        void SetStatus(string message);
        void ShowError(string caption, string message);
        void ShowMessage(string caption, string message);
        void ShowProgress(int progress);
        void SetButtonState(MainFormButton button, bool enabled, string customText = null);
        void SetProgressTimerState(bool enabled);
    }
}
