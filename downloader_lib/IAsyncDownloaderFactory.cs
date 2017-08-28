using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace downloader_lib
{
    public interface IAsyncDownloaderFactory
    {
        IAsyncDownloader GetDownloader(string url, string destPath, IFileSystem fileSystem);
    }
}
