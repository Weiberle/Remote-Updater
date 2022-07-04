using System.IO;

namespace RemoteUpdater.Sender.Helper
{
    internal static class FileSizeHelper
    {
        private static string[] sizes = { "B", "KB", "MB", "GB", "TB" };

        internal static string GetSize(string filePath)
        {
            double len = new FileInfo(filePath).Length;

            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            return string.Format("{0:0.##} {1}", len, sizes[order]);
        }
    }
}
