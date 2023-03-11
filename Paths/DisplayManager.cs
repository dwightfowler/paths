using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paths
{
    public class DisplayManager
    {
        List<OutputLine> Lines;

        public DisplayManager()
        {
            this.Lines = new List<OutputLine>(16);
        }

        public void AddPath(bool isSystem, ConsoleColor color, string path)
        {
            Lines.Add(new OutputLine() { IsSystem = isSystem, Color = color, Output = path });
        }

        public IReadOnlyList<OutputLine> SortedPaths()
        {
            List<OutputLine> tmp = new (Lines);
            tmp.Sort();
            return tmp.AsReadOnly();
        }

        internal void WriteAll(bool sorted = false)
        {
            ConsoleColor savedColor = Console.ForegroundColor;
            IReadOnlyList<OutputLine> lines = sorted ? SortedPaths() : Lines;

            bool isSystem = true;
            string sortIndicator = sorted ? " (sorted)" : "";
            Console.WriteLine($"System Paths{sortIndicator}");

            foreach(var line in lines)
            {
                if (isSystem && !line.IsSystem)
                {
                    Console.ForegroundColor = savedColor;
                    Console.WriteLine($"User Paths{sortIndicator}");
                    isSystem = false;
                }
                Console.ForegroundColor = line.Color;
                Console.WriteLine(line.Output); 
            }

            Console.ForegroundColor = savedColor;
            Console.WriteLine(" ");
        }
    }
}
