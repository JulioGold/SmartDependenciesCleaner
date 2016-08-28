using System.IO;
using System.Linq;

namespace SDC
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] directories = GetDirectories(@"C:\InvalidPathToDontDoWrong", "*packages*|*node_modules*|*bin*|*obj*", SearchOption.AllDirectories);

            foreach (var directory in directories)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                directoryInfo.Delete(true);
            }

            string blabla = "";
        }

        /// <summary>
        /// Faz com que seja possível especificar mais de um searchPatter separado por | pipe
        /// </summary>
        /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
        /// <param name="searchPatterns">The search string to match against the names of subdirectories in path. This parameter can contain a combination of valid literal and wildcard characters (see Remarks), but doesn't support regular expressions.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include all subdirectories or only the current directory.</param>
        /// <returns>An array of the full names (including paths) of the subdirectories that match the specified criteria, or an empty array if no directories are found.</returns>
        private static string[] GetDirectories(string path, string searchPatterns, System.IO.SearchOption searchOption)
        {
            return searchPatterns.Split('|').SelectMany(pattern => Directory.GetDirectories(path, pattern, searchOption)).ToArray();
        }
    }
}
