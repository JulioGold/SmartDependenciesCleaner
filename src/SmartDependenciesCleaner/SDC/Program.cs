using System.IO;
using System.Linq;

namespace SDC
{
    class Program
    {
        static void Main(string[] args)
        {
            string pattern = "*packages*|*node_modules*|*bin*|*obj*|package-lock.json";
            string[] files = GetFiles(@"C:\temp\InvalidPathToDontDoWrong", pattern, SearchOption.AllDirectories);
            string[] directories = GetDirectories(@"C:\temp\InvalidPathToDontDoWrong", pattern, SearchOption.AllDirectories);

            foreach (var path in files.Concat(directories))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);

                if (directoryInfo.Attributes.HasFlag(FileAttributes.Directory))
                {
                    directoryInfo.Delete(true);
                }
                else
                {
                    FileInfo fileInfo = new FileInfo(path);
                    fileInfo.Delete();
                }
            }
        }

        /// <summary>
        /// Faz com que seja possível especificar mais de um searchPattern separado por | pipe
        /// </summary>
        /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
        /// <param name="searchPatterns">The search string to match against the names of subdirectories in path. This parameter can contain a combination of valid literal and wildcard characters (see Remarks), but doesn't support regular expressions.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include all subdirectories or only the current directory.</param>
        /// <returns>An array of the full names (including paths) of the subdirectories that match the specified criteria, or an empty array if no directories are found.</returns>
        private static string[] GetDirectories(string path, string searchPatterns, System.IO.SearchOption searchOption)
        {
            return searchPatterns.Split('|').SelectMany(pattern => Directory.GetDirectories(path, pattern, searchOption)).ToArray();
        }

        /// <summary>
        /// Faz com que seja possível especificar mais de um searchPattern separado por | pipe
        /// </summary>
        /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
        /// <param name="searchPatterns">The search string to match against the names of subdirectories in path. This parameter can contain a combination of valid literal and wildcard characters (see Remarks), but doesn't support regular expressions.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include all subdirectories or only the current directory.</param>
        /// <returns>An array of the full names that match the specified criteria, or an empty array if no files are found.</returns>
        private static string[] GetFiles(string path, string searchPatterns, System.IO.SearchOption searchOption)
        {
            return searchPatterns.Split('|').SelectMany(pattern => Directory.GetFiles(path, pattern, searchOption)).ToArray();
        }
    }
}
