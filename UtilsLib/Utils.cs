using System;
using Microsoft.Win32;
using System.IO;

namespace UtilsLib
{
    public class Utils
    {

        // Formatting

        public static string GetHumanReadableSize(long sizeInBytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            double size = sizeInBytes;
            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }

            return String.Format("{0:0.##} {1}", size, sizes[order]);
        }

        public static string GetHumanReadableSpeed(long speedInBytesPerSecond)
        {
            return GetHumanReadableSize(speedInBytesPerSecond) + "/s";
        }

        public static string GetHumanReadableProgress(double progress)
        {
            if (progress < 0.0)
                return "0 %";
            else if (progress > 100.0)
                return "100 %";
            else
                return ((int)(progress * 100)).ToString() + " %";
        }

        public static string ExtractFileNameFromUrl(string path)
        {
            // Extracts the last part of the url
            string filename = path.Substring(path.LastIndexOf("/") + 1);
            // deny empty filenames
            if (filename.Length == 0)
            {
                return "unnamed";
            }
            // remove invalid chars
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }

        // FS

        public static string GetDefaultDownloadsFolder()
        {
            return Registry.GetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders",
                "{374DE290-123F-4565-9164-39C4925E467B}", "Downloads").ToString();
        }

        public static string GenerateUniqueFileName(string expectedPath)
        {
            string resultPath = expectedPath;
            int fileSuffix = 1;
            while (System.IO.File.Exists(resultPath))
            {
                int separatorPos;
                // respect extension while inserting file suffix
                if (expectedPath.LastIndexOf('.') > expectedPath.LastIndexOf('\\'))
                {
                    separatorPos = expectedPath.LastIndexOf('.');
                }
                else
                {
                    separatorPos = expectedPath.Length - 1;
                }

                resultPath =
                    expectedPath.Substring(0, separatorPos) +
                    $" ({fileSuffix++})" +
                    expectedPath.Substring(separatorPos, expectedPath.Length - separatorPos);
            }
            return resultPath;
        }
    }
}
