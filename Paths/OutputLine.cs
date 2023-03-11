using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paths
{
    public class OutputLine : IComparable<OutputLine>
    {
        public bool IsSystem = true;
        public ConsoleColor Color;
        public string Output;

        public int CompareTo(OutputLine other)
        {
            if (other is null)
                return 1;

            // System vs User has precedence
            if ((IsSystem ^ other.IsSystem))
            {
                return IsSystem ? -1 : 1;
            }
            // Next is path equality
            if (Output is null)
            {
                if (other.Output is null)
                    return 0;
                else
                    return -1;
            }
            else
            {
                int result = Output.CompareTo(other.Output);
                if (result != 0)
                    return result;

                return (int)Color - (int)other.Color;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is OutputLine))
                return false;
            return CompareTo((OutputLine)obj) == 0;
        }
    }
}
