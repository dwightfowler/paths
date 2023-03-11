using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Paths
{
    class Program
    {
        /// <summary>
        /// Command line parameters:
        /// <code>
        ///     -a                      add current path to PATH environment variable<br/>
        ///     -a .                    add current path to PATH environment variable<br/>
        ///     -a \existing\path       add given path to end of PATH environment variable<br/>
        ///     -a 5 \existing\path     add given path at n-th line of PATH environment variable<br/>
        ///     -d                      delete non-existent paths<br/>
        ///     -d 2                    delete n-th path in PATH environment variable<br/>
        ///     -h                      hide System and User headers<br/>
        ///     -u                      show just the user PATH environment variable<br/>
        ///     -s                      show just the sytem PATH environment variable<br/>
        ///     -n                      include line numbers<br/>
        ///     \existing\path          returns Exit Code 0 if given path is in PATH environment variable, otherwise Exit Code 1<br/>
        /// </code>
        /// </summary>
        static void Main(string[] args)
        {
            ConsoleColor forecolor = Console.ForegroundColor;
            Regex setvar = new Regex("%[^%]+%");
            string paths;
            try
            {
                Console.ForegroundColor = forecolor;
                Console.WriteLine("System Paths");
                paths = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
                printPaths(paths);

                Console.ForegroundColor = forecolor;
                Console.WriteLine("User Paths");
                paths = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
                printPaths(paths);
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
                ConsoleColor color = ConsoleColor.DarkGreen;
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
                string sVarVal = sVar.Trim('%');
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
                    Console.WriteLine();
            }
        }
    }
}
