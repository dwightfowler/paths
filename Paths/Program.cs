using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Utility.CommandLine;

namespace Paths
{
    class Program
    {
        private static List<string> foundPaths = new List<string>();
        private static DisplayManager dispMgr = new DisplayManager();

        [Argument('s', "sort", "Sort the entries")]
        private static bool Sort { get; set; }
        [Argument('h', "help", "Display this help message")]
        private static bool Help { get; set; }

        static int Main(string[] args)
        {
            Arguments.Populate();

            ConsoleColor forecolor = Console.ForegroundColor;
            string paths;
            try
            {
                paths = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
                generatePaths(paths, true);

                paths = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
                generatePaths(paths, false);

                dispMgr.WriteAll(Sort);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = forecolor;
                Console.Write(ex.Message);
                return 1;
            }

            return 0;
        }

        private static void generatePaths(string paths, bool isSystem)
        {
            Regex setvar = new Regex("%[^%]+%");
            string[] pathList = paths.Split(';');
            foreach (String path in pathList)
            {
                if (path == "")
                    continue;

                ConsoleColor color = ConsoleColor.DarkGreen;
                if (DuplicatePathExists(path))
                {
                    color = ConsoleColor.DarkYellow;
                }

                if (Directory.Exists(path))
                {
                    dispMgr.AddPath(isSystem, color, path);
                    continue;
                }

                color = ConsoleColor.DarkRed;
                Match m = setvar.Match(path);
                if (m.Length <= 0)
                {
                    dispMgr.AddPath(isSystem, color, path);
                    continue;
                }

                string sVar = path.Substring(m.Index, m.Length);
                string sVarVal = sVar.Trim('%');
                sVarVal = Environment.GetEnvironmentVariable(sVarVal);
                bool varExists = !String.IsNullOrEmpty(sVarVal);
                string dispPath = $"{path} = ";
                if (varExists)
                {
                    sVar = path.Replace(sVar, sVarVal);
                    color = Directory.Exists(sVar) ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed;
                    dispMgr.AddPath(isSystem, color, dispPath + sVar);
                }
                else
                    dispMgr.AddPath(isSystem, color, dispPath + $"Environment var does not exist {sVar}: {sVarVal}");
            }
        }

        private static bool DuplicatePathExists(string path)
        {
            int idxFound = foundPaths.BinarySearch(path);
            bool exists = idxFound >= 0;
            if (!exists)
            {
                foundPaths.Add(path);
                foundPaths.Sort();
            }
            return exists;
        }
    }
}
