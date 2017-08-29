using System.IO;

namespace downloader_lib
{
    public interface IFileSystem
    {
        void Move(string src, string dest);
        bool Exists(string path);
        void ShowFileInFolder(string path);
        Stream GetOutputStream(string path, bool append);
    }
}
