using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCheck
{
    internal interface IResultSaver
    {
        public void Save(PotentialSolution solution);
    }

    internal record class PotentialSolution(string Title, string Text, int Score, object[] Keys);
}
