using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Assistant
{
    /// <summary>
    /// Helper functions for dealing with directories
    /// </summary>
    public static class DirectoryHelpers
    {
        /// <summary>
        /// Default delimiter for directory paths
        /// </summary>
        private const char Delimiter = '\\';

        /// <summary>
        /// Method for acquiring the proper directory name from a full path
        /// </summary>
        /// <param name="path">Full directory path</param>
        /// <returns></returns>
        public static string GetDirectoryName(string path)
        {
            // Make sure there is anything to do
            if (path.Length < 1)
                return string.Empty;

            // Get rid of potential forward slashes and trailing delimiters
            var normalizedPath = path.Replace('/', Delimiter).TrimEnd(new[] { Delimiter });

            // Find where the last part of the path starts
            var lastDelimiter = normalizedPath.LastIndexOf(Delimiter);

            if (lastDelimiter < 0)
                // If the path does not contain any delimiters, consider it a name...
                return normalizedPath;
            else
                // ...Otherwise return the last section of the path
                return normalizedPath.Substring(lastDelimiter + 1);
        }

        /// <summary>
        /// Method for determining the type of a directory
        /// </summary>
        /// <param name="path">Path representing the directory</param>
        /// <returns></returns>
        public static DirectoryType GetDirectoryType(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);

            // If DirectoryInfo sees no Parent, it should be a drive
            if (di.Parent == null)
                return DirectoryType.Drive;

            FileAttributes a = File.GetAttributes(path);

            if (a.HasFlag(FileAttributes.Directory))
                // If FileAttributes has the Directory flag, it is a folder...
                return DirectoryType.Folder;
            else
                // ...Otherwise it should be a file
                if (GetExtension(path) == ".mp3")
                    return DirectoryType.File | DirectoryType.MP3File;
                else
                    return DirectoryType.File;
        }

        public static List<string> GetContents(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);

            try
            {
                return di.GetDirectories().Select(d => d.FullName).Concat(di.GetFiles().Select(d => d.FullName)).ToList();
            }
            
            catch (UnauthorizedAccessException e)
            {
                return new List<string>();
            }
        }

        public static List<string> GetRootDirectoryContents()
        {
            return DriveInfo.GetDrives().Where(drive => drive.DriveType == DriveType.Fixed).Select(drive => drive.Name).ToList();
        }

        private static string GetExtension(string path)
        {
            return Path.GetExtension(path).ToLower();
        }
    }
}
