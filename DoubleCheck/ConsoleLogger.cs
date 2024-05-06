using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCheck
{
    internal class FileLogger(string path) : IResultSaver
    {
        private readonly string path = path;
        private readonly StringBuilder _sb = new();
        private volatile int solutionsGotten = 0;
        const int writeThreshold = 100;
        public void Save(PotentialSolution solution)
        {
            lock (_sb)
            {
                _sb .Append(solution.Title)
                    .Append(',')
                    .Append(solution.Text)
                    .Append(',')
                    .Append(solution.Score)
                    .Append(',');
                for (int i = 0; i < solution.Keys.Length; i++)
                {
                    _sb .Append(solution.Keys[i])
                        .Append(',');
                }

                _sb.Append('\n');
                solutionsGotten++;

                if(solutionsGotten > writeThreshold)
                {
                    File.AppendAllLines(path, [_sb.ToString()]);
                    _sb.Clear();
                    solutionsGotten = 0;
                }
            }
        }
    }
}
