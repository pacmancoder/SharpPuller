using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace downloader_lib
{
    public class AsyncDownloaderFactory : IAsyncDownloaderFactory
    {
        public IAsyncDownloader GetDownloader(string url, string destPath, IFileSystem fileSystem)
        {
            return new AsyncDownloader(url, destPath, fileSystem);
        }
    }
}
