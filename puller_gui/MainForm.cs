using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using puller_gui.Presenters;
using puller_gui.Views;

namespace puller_gui
{
    public partial class MainForm : Form, Views.IMainFormView
    {

        private class FileListRowTag
        {
            public int fileId;
            public int listIndex;
        }

        public MainForm(Presenters.IMainFormPresenter presenter)
        {
            InitializeComponent();

            presenter_ = presenter;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            presenter_.HookView(this);
        }



        public void AddFileToList(int id, string fileName, string size)
        {
            var row = new ListViewItem(fileName);
            row.SubItems.Add(size); // size
            row.SubItems.Add("0 B"); // downloaded size
            row.SubItems.Add("0 B/s"); // speed
            row.SubItems.Add("0 %"); // progress
            row.SubItems.Add("In queue"); // status
            pendingFilesList.Items.Add(row);

            row.Tag = new FileListRowTag { fileId = id, listIndex = pendingFilesList.Items.Count - 1 };
            fileIdToListIndex.Add(id, pendingFilesList.Items.Count - 1);
        }

        public void RemoveFileFromList(int id)
        {
            var rowIndex = -1;
            for (int i = 0; i < pendingFilesList.Items.Count; i++)
            {
                var rowInfo = (FileListRowTag) pendingFilesList.Items[i].Tag;
                if (rowInfo.fileId == id)
                {
                    rowIndex = i;                    
                    break;
                }
            }

            for (int i = rowIndex; i < pendingFilesList.Items.Count; i++)
            {
                var rowInfo = (FileListRowTag)pendingFilesList.Items[i].Tag;
                rowInfo.listIndex--;
                fileIdToListIndex[rowInfo.fileId]--;
            }

            fileIdToListIndex.Remove(id);
            pendingFilesList.Items.RemoveAt(rowIndex);
        }

        public void ModifyFileListItem(int id, string downloadedSize, string speed, string progress, string status)
        {
                var listItem = pendingFilesList.Items[fileIdToListIndex[id]];
                listItem.SubItems[2].Text = downloadedSize;
                listItem.SubItems[3].Text = speed;
                listItem.SubItems[4].Text = progress;
                listItem.SubItems[5].Text = status;
        }

        public void SetStatus(string message)
        {
            statusLabel.Text = message;
        }

        public void ShowError(string caption, string message)
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ShowMessage(string caption, string message)
        {
            MessageBox.Show(message, caption);
        }

        public void ShowProgress(int progress)
        {
            MethodInvoker methodInvoker = new MethodInvoker(() => {
                if (progress < 0)
                {
                    downloadProgressBar.Value = 0;
                }
                else if (progress > 1000)
                {
                    downloadProgressBar.Value = 1000;
                }
                else
                {
                    downloadProgressBar.Value = progress;
                }
            });

            if (downloadProgressBar.InvokeRequired)
            {
                downloadProgressBar.Invoke(methodInvoker);
            }
            else
            {
                methodInvoker.Invoke();
            }
        }

        public void SetButtonState(Views.MainFormButton button, bool enabled, string customText = null)
        {
            Button guiButton = null;
            switch (button)
            {
                case Views.MainFormButton.Add:
                {
                    guiButton = addFileButton;
                    break;
                }
                case Views.MainFormButton.Start:
                {
                    guiButton = startButton;
                    break;
                }
                case Views.MainFormButton.Stop:
                {
                    guiButton = stopButton;
                    break;
                }
            }

            if (guiButton != null)
            {
                guiButton.Enabled = enabled;
                if (customText != null)
                {
                    guiButton.Text = customText;
                }
            }
        }

        public void SetProgressTimerState(bool enabled)
        {
            progressTimer.Enabled = enabled;
        }

        private void addFileButton_Click(object sender, EventArgs e)
        {
            presenter_.AddFile(newFileTextBox.Text);
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            presenter_.StartDownload();
        }

        private void progressTimer_Tick(object sender, EventArgs e)
        {
            presenter_.CheckProgress();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            presenter_.PauseDownload();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in pendingFilesList.SelectedItems)
            {
                var rowInfo = (FileListRowTag)item.Tag;
                presenter_.RemoveFile(rowInfo.fileId);
            }
        }

        Presenters.IMainFormPresenter presenter_;

        Dictionary<int, int> fileIdToListIndex = new Dictionary<int, int>();
    }
}
