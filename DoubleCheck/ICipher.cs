using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCheck
{
    internal interface ICipher<TSelf>
    {
        void UnScramble(ReadOnlySpan<BChar> characters, Span<BChar> output);
        public object[] GetCurrentKeys();
        static abstract bool MoveNext(ref TSelf thing);
    }
}
