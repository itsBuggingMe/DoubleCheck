using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCheck
{
    internal struct Transposition(int max, int offsetStart, int offsetMax) : ICipher<Transposition>
    {
        private int skipNum = 0;
        private readonly int max = max;
        private int offset = offsetStart;
        private readonly int offsetMax = offsetMax;
        public static bool MoveNext(ref Transposition thing)
        {
            thing.skipNum++;
            if(thing.skipNum >= thing.max)
            {
                thing.offset++;

                if(thing.offset >= thing.offsetMax)
                {
                    return false;
                }
            }

            return true;
        }

        public readonly void UnScramble(ReadOnlySpan<BChar> characters, Span<BChar> output)
        {
            int characterReadLoc = skipNum + offset + characters.Length;

            for(int i = 0; i < output.Length; i++)
            {
                characterReadLoc %= characters.Length;
                output[i] = characters[characterReadLoc];
                characterReadLoc += skipNum;
            }
        }
    }
}
