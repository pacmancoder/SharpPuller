using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;

namespace downloader_lib
{
    public class AsyncFileDownloader
    {

        private volatile bool allowedToRun_;
        private volatile bool downloadInProgress_;
        private volatile bool downloadStarted_;

        private string url_;
        private string destPath_;
        private int blockSize_;

        private long fileSize_;
        private long downloadedSize_;

        public long DownloadedSize
        {
            get
            {
                return Interlocked.Read(ref downloadedSize_);
            }
            private set
            {
                Interlocked.Exchange(ref downloadedSize_, value);
            }
        }

        public long FileSize {
            get
            {
                return fileSize_;
            }
        }

        public bool Finished
        {
            get
            {
                return FileSize == DownloadedSize;
            }
        }

        public bool DownloadInProgress
        {
            get
            {
                return downloadInProgress_;
            }
        }

        public bool DownloadStarted
        {
            get
            {
                return downloadStarted_;
            }
        }

        public AsyncFileDownloader(string url, string destPath, int blockSize)
        {
            allowedToRun_ = true;
            downloadInProgress_ = false;
            downloadStarted_ = false;

            url_ = url;
            destPath_ = destPath;
            blockSize_ = blockSize;
            fileSize_ = GetFileSize();

            DownloadedSize = 0;
        }

        private long GetFileSize()
        {
            var request = (HttpWebRequest)WebRequest.Create(url_);
            request.Method = "HEAD";

            using (var response = request.GetResponse())
            {
                if (response.ContentLength < 0)
                {
                    throw new ApplicationException($"Provided url \"{url_}\" is not a file location");
                }
                return response.ContentLength;
            }

        }

        private async Task Start(long offset)
        {
            if (!allowedToRun_)
                throw new InvalidOperationException();

            var request = (HttpWebRequest)WebRequest.Create(url_);
            request.Method = "GET";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
            request.AddRange(offset);

            using (var response = await request.GetResponseAsync())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var fs = new FileStream(destPath_, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    {
                        while (allowedToRun_)
                        {
                            var buffer = new byte[blockSize_];
                            var bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length);

                            if (bytesRead == 0) break;

                            await fs.WriteAsync(buffer, 0, bytesRead);
                            DownloadedSize += bytesRead;

                            if (Finished)
                            {
                                downloadStarted_ = false;
                            }
                        }

                        await fs.FlushAsync();
                    }
                }
            }
            downloadInProgress_ = false;
        }

        public Task Start()
        {
            allowedToRun_ = true;
            downloadInProgress_ = true;
            downloadStarted_ = true;
            return Start(DownloadedSize);
        }

        public void Pause()
        {
            allowedToRun_ = false;
            downloadInProgress_ = false;
        }
    }
}
