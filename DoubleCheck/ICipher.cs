using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCheck
{
    internal interface ICipher<TSelf> where TSelf : struct
    {
        FChar UnScramble(ReadOnlySpan<FChar> characters, int index);
        
        public object[] GetCurrentKeys();
        static abstract bool MoveNext(ref TSelf thing);
        static abstract void ResetCounter(ref TSelf thing);
    }
}
