using System;

namespace UtilsLib
{
    public class Utils
    {
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

        public static string GetHumanReadableProgress(double progress)
        {
            if (progress < 0.0)
                return "0 %";
            else if (progress > 100.0)
                return "100 %";
            else
                return ((int)(progress * 100)).ToString();
        }
    }
}
