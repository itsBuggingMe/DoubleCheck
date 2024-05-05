using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCheck
{
    internal class OneLayerScramble<TCipher, TChecker>(TCipher cipher, BChar[] cypherText) 
        : Scrambler(0, 0) where TCipher : struct, ICipher<TCipher> where TChecker : IChecker
    {
        private TCipher _cipher = cipher;

        public override void TryAllCombos()
        {
            ReadOnlySpan<BChar> ctext = cypherText;
            Span<BChar> bChar = stackalloc BChar[ctext.Length];

            while (TCipher.MoveNext(ref _cipher))
            {
                _cipher.UnScramble(ctext, bChar);
                if (TChecker.Check(bChar))
                {
                    Console.WriteLine("Found");
                    Console.WriteLine(bChar.ToArray().ArrToString());
                }
            }
        }
    }
}
