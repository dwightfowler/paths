using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Paths
{
    class Program
    {
        private static List<string> foundPaths = new List<string>();

        static void Main(string[] args)
        {
            ConsoleColor forecolor = Console.ForegroundColor;
            Regex setvar = new Regex("%[^%]+%");
            string? paths;
            try
            {
                Console.ForegroundColor = forecolor;
                Console.WriteLine("System Paths");
                paths = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
                printPaths(paths ?? "Machine path not found");

                Console.ForegroundColor = forecolor;
                Console.WriteLine("User Paths");
                paths = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
                printPaths(paths ?? "User path not found");
            }
            finally
            {
                Console.ForegroundColor = forecolor;
                Console.Write(" ");
            }
        }

        private static void printPaths(string paths)
        {
            Regex setvar = new Regex("%[^%]+%");
            string[] pathList = paths.Split(';');
            foreach (String path in pathList)
            {
                if (path == "")
                    continue;

                ConsoleColor color = ConsoleColor.DarkGreen;
                if (DuplicatesExist(path))
                {
                    color = ConsoleColor.DarkYellow;
                }

                if (Directory.Exists(path))
                {
                    Console.ForegroundColor = color;
                    Console.WriteLine(path);
                    continue;
                }

                color = ConsoleColor.DarkRed;
                Match m = setvar.Match(path);
                if (m.Length <= 0)
                {
                    Console.ForegroundColor = color;
                    Console.WriteLine(path);
                    continue;
                }

                string sVar = path.Substring(m.Index, m.Length);
                string? sVarVal = sVar.Trim('%');
                sVarVal = Environment.GetEnvironmentVariable(sVarVal);
                bool varExists = !String.IsNullOrEmpty(sVarVal);
                color = varExists ? ConsoleColor.DarkYellow : ConsoleColor.DarkRed;
                Console.ForegroundColor = color;
                Console.Write($"{path} = ");
                if (varExists)
                {
                    sVar = path.Replace(sVar, sVarVal);
                    color = Directory.Exists(sVar) ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed;
                    Console.ForegroundColor = color;
                    Console.WriteLine(sVar);
                }
                else
                    Console.WriteLine($"Environment var does not exist %{sVar}%: {sVarVal}");
            }
        }

        private static bool DuplicatesExist(string path)
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
