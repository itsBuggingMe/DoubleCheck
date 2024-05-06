using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCheck
{
    internal class ConsoleLogger : IResultSaver
    {
        private StringBuilder _sb = new();
        public void Save(PotentialSolution solution)
        {
            lock (_sb)
            {
                _sb .Clear()
                    .Append('\n')
                    .Append(solution.Title)
                    .Append(": \"")
                    .Append(solution.Text)
                    .Append("\"\nScore: ")
                    .Append(solution.Score)
                    .Append("\nKeys: [ ");

                for (int i = 0; i < solution.Keys.Length; i++)
                {
                    _sb .Append(solution.Keys[i])
                        .Append(", ");
                }

                _sb .Remove(_sb.Length - 2, 2)
                    .Append(" ]");

                Console.WriteLine(_sb.ToString());
            }
        }
    }
}
