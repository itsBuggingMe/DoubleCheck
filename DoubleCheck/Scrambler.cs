using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCheck
{
    internal abstract class Scrambler
    {
        protected readonly int StartIndex;
        protected readonly int Count;

        public Scrambler(int startIndex, int count)
        {
            if (startIndex < 0 || startIndex + count > AllWords.Length)
                throw new ArgumentException("Bad");

            StartIndex = startIndex;
            Count = count;
        }

        public abstract void TryAllCombos();

        public static readonly BChar[][] AllWords;
        public static int NumWords => AllWords.Length;

        static Scrambler()
        {
            AllWords = File.ReadAllLines("words.txt")
                .Select(s => 
                    s.Where(char.IsLetter).Select(c => (BChar)char.ToUpper(c)).ToArray()
                    )
                .Where(arr => arr.Length > 0)
                .ToArray();
        }
    }

    internal interface IChecker
    {
        static abstract bool Check(Span<BChar> bChars);
    }
}
