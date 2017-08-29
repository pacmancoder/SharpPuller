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

        public void ShowFileInFolder(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            // combine the arguments together
            // it doesn't matter if there is a space after ','
            string argument = "/select, \"" + filePath.Replace('/', '\\') + "\"";

            System.Diagnostics.Process.Start("explorer.exe", argument);
        }

        public void Move(string src, string dest)
        {
            File.Move(src, dest);
        }
    }
}
