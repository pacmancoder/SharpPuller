using System;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;

namespace downloader_lib
{
    public class AsyncDownloader : IAsyncDownloader
    {
        private const int TIMESTAMPS_COUNT = 8;
        private const int DEFAULT_BLOCK_SIZE = 512;
        private const string REQUEST_METHOD_GET = "GET";
        private const string REQUEST_METHOD_HEAD = "HEAD";
        private const string REQUEST_AGENT = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0";

        private volatile bool allowedToRun_ = false;
        private volatile bool downloadInProgress_ = false;
        private volatile bool downloadStarted_ = false;
        private volatile bool downloadFinished_ = false;

        private string url_ = null;
        private string destPath_ = null;
        private int blockSize_ = DEFAULT_BLOCK_SIZE;

        private long fileSize_ = -1;
        private long downloadedSize_ = 0;
        private long prevDownloadedSize_ = -1;

        private long[] speedTimestamps_ = new long[TIMESTAMPS_COUNT];
        private int speedTimestampsHead_ = 0;
        private long prevTimeMeasure_ = -1;

        Task currentDownloadTask_;

        IFileSystem fileSystem_;

        public AsyncDownloader(string url, string destPath, IFileSystem fileSystem)
        {
            fileSystem_ = fileSystem;
            url_ = url;
            destPath_ = destPath;

            ValidateResponseHead();
        }

        public long DownloadSpeed
        {
            get
            {
                if (prevDownloadedSize_  < 0)
                {
                    prevDownloadedSize_ = DownloadedSize;
                    prevTimeMeasure_ = DateTime.Now.Ticks;
                }

                long timeDelta = DateTime.Now.Ticks - prevTimeMeasure_;
                long sizeDelta = DownloadedSize - prevDownloadedSize_;

                if (timeDelta == 0)
                {
                    speedTimestamps_[SpeedRingBufferIndex(speedTimestampsHead_++)] = 0;
                } else
                {
                    speedTimestamps_[SpeedRingBufferIndex(speedTimestampsHead_++)] = (long) (
                        (double) sizeDelta /
                        (timeDelta / (double)TimeSpan.TicksPerSecond));
                }

                double sum = 0;
                for (int i = 0; i < TIMESTAMPS_COUNT; i++)
                {
                    // For avoiding float ariphmetics overflow: (x + y) / 2 == (x / 2) + (y / 2);
                    sum += speedTimestamps_[SpeedRingBufferIndex(i + speedTimestampsHead_)] 
                        / (double) TIMESTAMPS_COUNT;
                }

                prevDownloadedSize_ = DownloadedSize;
                prevTimeMeasure_ = DateTime.Now.Ticks;

                return (long) sum;
            }
        }


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

        private class ResponseHeadData
        {
            public long contentLength;
        }

        private ResponseHeadData responseHead_ = null;
        private ResponseHeadData ResponseHead
        {
            get
            {
                if (responseHead_ == null)
                {
                    var request = (HttpWebRequest)WebRequest.Create(url_);
                    request.Method = REQUEST_METHOD_HEAD;
                    request.UserAgent = REQUEST_AGENT;

                    using (var response = request.GetResponse())
                    {
                        responseHead_ = new ResponseHeadData
                        {
                            contentLength = response.ContentLength,
                        };
                    }
                }

                return responseHead_;
            }
        }

        public long FileSize {
            get
            {
                return ResponseHead.contentLength;
            }
        }

        public bool Finished
        {
            get
            {
                return downloadFinished_;
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

        public string Url
        {
            get
            {
                return url_;
            }
        }
        public string DestPath
        {
            get
            {
                return destPath_;
            }
        }

        private void StartInternal(long offset)
        {
            var request = (HttpWebRequest)WebRequest.Create(url_);

            request.Method = REQUEST_METHOD_GET;
            request.UserAgent = REQUEST_AGENT;
            request.AddRange(offset);

            using (var response = request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var fs = fileSystem_.GetOutputStream(destPath_, true))
                    {
                        while (allowedToRun_)
                        {
                            var buffer = new byte[blockSize_];
                            var bytesRead = responseStream.Read(buffer, 0, buffer.Length);

                            if (bytesRead > 0)
                            {
                                fs.Write(buffer, 0, bytesRead);
                                DownloadedSize += bytesRead;
                            }
                            else
                            {
                                downloadFinished_ = true;
                                break;
                            }
                        }

                        fs.Flush();
                    }
                }
            }
            downloadInProgress_ = false;
        }

        public void Start()
        {
            if (currentDownloadTask_ != null && !currentDownloadTask_.IsCompleted)
            {
                return;
            }

            allowedToRun_ = true;
            downloadInProgress_ = true;
            downloadStarted_ = true;
            currentDownloadTask_ = Task.Run(() => StartInternal(DownloadedSize));
        }

        public void Pause()
        {
            allowedToRun_ = false;
            downloadInProgress_ = false;
        }

        public void Join()
        {
            if (currentDownloadTask_ != null && !currentDownloadTask_.IsCompleted)
            {
                currentDownloadTask_.Wait();
            }
        }

        private int SpeedRingBufferIndex(int index)
        {
            return index % TIMESTAMPS_COUNT;
        }

        private void ValidateResponseHead()
        {
            if (ResponseHead.contentLength < 0)
            {
                throw new ApplicationException($"Provided url \"{url_}\" is not a file location");
            }
        }
    }
}
