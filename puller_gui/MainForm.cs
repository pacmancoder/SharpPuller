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
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            presenter_ = new Presenters.MainFormPresenter();
            presenter_.HookView(this);
        }



        public void AddFileToList(int id, string fileName, string size)
        {
            var row = new ListViewItem(fileName);
            row.SubItems.Add(size);
            row.Tag = id;
            pendingFilesList.Items.Add(row);
        }

        public void RemoveFileFromList(int id)
        {
            for (int i = 0; i < pendingFilesList.Items.Count; i++)
            {
                if ((int) pendingFilesList.Items[i].Tag == id)
                {
                    pendingFilesList.Items.RemoveAt(i);
                    break;
                }
            }
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

        Presenters.MainFormPresenter presenter_ = null;

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
                presenter_.RemoveFile((int)item.Tag);
            }
        }
    }
}
