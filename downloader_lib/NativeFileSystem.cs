using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace downloader_lib
{
    public class NativeFileSystem : IFileSystem
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public Stream GetOutputStream(string path, bool append)
        {
            return new FileStream(path, FileMode.Append, FileAccess.Write);
        }

        public void Move(string src, string dest)
        {
            File.Move(src, dest);
        }
    }
}
