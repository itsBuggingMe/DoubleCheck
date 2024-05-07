using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCheck
{
    internal interface IChecker
    {
        public int GetIndex();
        public ReadOnlySpan<FChar> GetMatch();
    }
}
