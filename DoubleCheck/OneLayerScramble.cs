using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCheck
{
    internal class OneLayerScramble<TCipher>(TCipher cipher, BChar[] cypherText, IResultSaver saver, string title) 
        : Scrambler(0, 0, saver, title) where TCipher : struct, ICipher<TCipher>
    {
        private TCipher _cipher = cipher;

        public override void TryAllCombos()
        {
            ReadOnlySpan<BChar> ctext = cypherText;
            BChar[] bChar = new BChar[ctext.Length];

            //11_000 per pair, chosen through the scientific process of guess and check
            const int CharThreshold = 14_000;
            int threshold = ctext.Length * CharThreshold;

            while (TCipher.MoveNext(ref _cipher))
            {
                _cipher.UnScramble(ctext, bChar);

                // bi-gram analysis for score
                int score = GetBiGramScore(bChar);

                if (score > threshold)
                {
                    resultSaver.Save(new PotentialSolution(title, bChar.ArrToString(), score, _cipher.GetCurrentKeys()));
                }
            }
        }
    }
}
