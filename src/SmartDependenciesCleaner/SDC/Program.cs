using CommandLine;
using System;
using System.IO;
using System.Linq;

namespace SDC
{
    class Options
    {
        [Option('l', "list", HelpText = "Listar apenas! NÃO realiza o delete!")]
        public bool List { get; set; }

        [Option('p', "path", Required = true, HelpText = "Pasta principal")]
        public string Path { get; set; }

        [Option('s', "search", Required = true, HelpText = "Padrão para busca, você pode concatenar cada padrão de busca")]
        public string SearchPattern { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(opts => Run(opts))
                .WithNotParsed<Options>((errs) => HandleParseError(errs));
        }

        private static void Run(Options opts)
        {
            string[] files = GetFiles(opts.Path, opts.SearchPattern, SearchOption.AllDirectories);
            string[] directories = GetDirectories(opts.Path, opts.SearchPattern, SearchOption.AllDirectories);

            foreach (var path in files.Concat(directories))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);

                Console.WriteLine(path);

                if (directoryInfo.Attributes.HasFlag(FileAttributes.Directory))
                {
                    if (!opts.List)
                    {
                        directoryInfo.Delete(true);
                    }
                }
                else
                {
                    FileInfo fileInfo = new FileInfo(path);

                    if(!opts.List)
                    {
                        fileInfo.Delete();
                    }
                }
            }
        }

        private static void HandleParseError(object errs)
        {
            Console.WriteLine("Comando não bem formado, tente novamente...");
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
