namespace puller_gui
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pendingFilesListCaption = new System.Windows.Forms.Label();
            this.newFileTextBox = new System.Windows.Forms.TextBox();
            this.addFileButton = new System.Windows.Forms.Button();
            this.filesListContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.downloadProgressBar = new System.Windows.Forms.ProgressBar();
            this.pendingFilesList = new System.Windows.Forms.ListView();
            this.fileListHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sizeListHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.downloadedSizeListheader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.speedListHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.progressListHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusListHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.progressTimer = new System.Windows.Forms.Timer(this.components);
            this.showInFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filesListContextMenu.SuspendLayout();
            this.mainStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pendingFilesListCaption
            // 
            this.pendingFilesListCaption.AutoSize = true;
            this.pendingFilesListCaption.Location = new System.Drawing.Point(13, 13);
            this.pendingFilesListCaption.Name = "pendingFilesListCaption";
            this.pendingFilesListCaption.Size = new System.Drawing.Size(70, 13);
            this.pendingFilesListCaption.TabIndex = 1;
            this.pendingFilesListCaption.Text = "Pending files:";
            // 
            // newFileTextBox
            // 
            this.newFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.newFileTextBox.Location = new System.Drawing.Point(12, 244);
            this.newFileTextBox.Multiline = true;
            this.newFileTextBox.Name = "newFileTextBox";
            this.newFileTextBox.Size = new System.Drawing.Size(823, 80);
            this.newFileTextBox.TabIndex = 2;
            // 
            // addFileButton
            // 
            this.addFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addFileButton.Location = new System.Drawing.Point(841, 244);
            this.addFileButton.Name = "addFileButton";
            this.addFileButton.Size = new System.Drawing.Size(78, 81);
            this.addFileButton.TabIndex = 3;
            this.addFileButton.Text = "Add Files";
            this.addFileButton.UseVisualStyleBackColor = true;
            this.addFileButton.Click += new System.EventHandler(this.addFileButton_Click);
            // 
            // filesListContextMenu
            // 
            this.filesListContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem,
            this.showInFolderToolStripMenuItem});
            this.filesListContextMenu.Name = "filesListContextMenu";
            this.filesListContextMenu.Size = new System.Drawing.Size(153, 70);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.mainStatusStrip.Location = new System.Drawing.Point(0, 427);
            this.mainStatusStrip.Name = "mainStatusStrip";
            this.mainStatusStrip.Size = new System.Drawing.Size(930, 22);
            this.mainStatusStrip.TabIndex = 5;
            this.mainStatusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(39, 17);
            this.statusLabel.Text = "Ready";
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.startButton.Enabled = false;
            this.startButton.Location = new System.Drawing.Point(12, 389);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(78, 23);
            this.startButton.TabIndex = 6;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(96, 389);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(78, 23);
            this.stopButton.TabIndex = 6;
            this.stopButton.Text = "Pause";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // downloadProgressBar
            // 
            this.downloadProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadProgressBar.Location = new System.Drawing.Point(12, 331);
            this.downloadProgressBar.Maximum = 1000;
            this.downloadProgressBar.Name = "downloadProgressBar";
            this.downloadProgressBar.Size = new System.Drawing.Size(907, 52);
            this.downloadProgressBar.TabIndex = 7;
            // 
            // pendingFilesList
            // 
            this.pendingFilesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pendingFilesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.fileListHeader,
            this.sizeListHeader,
            this.downloadedSizeListheader,
            this.speedListHeader,
            this.progressListHeader,
            this.statusListHeader});
            this.pendingFilesList.ContextMenuStrip = this.filesListContextMenu;
            this.pendingFilesList.FullRowSelect = true;
            this.pendingFilesList.Location = new System.Drawing.Point(12, 29);
            this.pendingFilesList.Name = "pendingFilesList";
            this.pendingFilesList.Size = new System.Drawing.Size(905, 209);
            this.pendingFilesList.TabIndex = 8;
            this.pendingFilesList.UseCompatibleStateImageBehavior = false;
            this.pendingFilesList.View = System.Windows.Forms.View.Details;
            // 
            // fileListHeader
            // 
            this.fileListHeader.Text = "File";
            this.fileListHeader.Width = 300;
            // 
            // sizeListHeader
            // 
            this.sizeListHeader.Text = "Size";
            this.sizeListHeader.Width = 100;
            // 
            // downloadedSizeListheader
            // 
            this.downloadedSizeListheader.Text = "Downloaded Size";
            this.downloadedSizeListheader.Width = 100;
            // 
            // speedListHeader
            // 
            this.speedListHeader.Text = "Speed";
            this.speedListHeader.Width = 100;
            // 
            // progressListHeader
            // 
            this.progressListHeader.Text = "Progress";
            this.progressListHeader.Width = 100;
            // 
            // statusListHeader
            // 
            this.statusListHeader.Text = "Status";
            this.statusListHeader.Width = 200;
            // 
            // progressTimer
            // 
            this.progressTimer.Interval = 500;
            this.progressTimer.Tick += new System.EventHandler(this.progressTimer_Tick);
            // 
            // showInFolderToolStripMenuItem
            // 
            this.showInFolderToolStripMenuItem.Name = "showInFolderToolStripMenuItem";
            this.showInFolderToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.showInFolderToolStripMenuItem.Text = "Show in folder";
            this.showInFolderToolStripMenuItem.Click += new System.EventHandler(this.showInFolderToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 449);
            this.Controls.Add(this.pendingFilesList);
            this.Controls.Add(this.downloadProgressBar);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.mainStatusStrip);
            this.Controls.Add(this.addFileButton);
            this.Controls.Add(this.newFileTextBox);
            this.Controls.Add(this.pendingFilesListCaption);
            this.Name = "MainForm";
            this.Text = "Puller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.filesListContextMenu.ResumeLayout(false);
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label pendingFilesListCaption;
        private System.Windows.Forms.TextBox newFileTextBox;
        private System.Windows.Forms.Button addFileButton;
        private System.Windows.Forms.ContextMenuStrip filesListContextMenu;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.StatusStrip mainStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.ProgressBar downloadProgressBar;
        private System.Windows.Forms.ListView pendingFilesList;
        private System.Windows.Forms.ColumnHeader fileListHeader;
        private System.Windows.Forms.ColumnHeader sizeListHeader;
        private System.Windows.Forms.Timer progressTimer;
        private System.Windows.Forms.ColumnHeader downloadedSizeListheader;
        private System.Windows.Forms.ColumnHeader speedListHeader;
        private System.Windows.Forms.ColumnHeader progressListHeader;
        private System.Windows.Forms.ColumnHeader statusListHeader;
        private System.Windows.Forms.ToolStripMenuItem showInFolderToolStripMenuItem;
    }
}

