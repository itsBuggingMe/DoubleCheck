using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace DoubleCheck
{
    internal class OneLayerScramble<TChecker, TCipher>(TChecker checker, TCipher cipher, FChar[] cypherText, IResultSaver saver, string title) 
        : Scrambler(saver, title) where TCipher : struct, ICipher<TCipher> where TChecker : IChecker
    {
        public override void TryAllCombos()
        {
            ReadOnlySpan<FChar> ctext = cypherText;

            int checkIndex = checker.GetIndex();

            ReadOnlySpan<FChar> match = checker.GetMatch();
            int matchLength = match.Length;

            while (TCipher.MoveNext(ref cipher))
            {
                int checkStart = 0;
                FChar currentChar;

                do
                {
                    currentChar = cipher.UnScramble(ctext, checkIndex + checkStart);

                    if (checkStart == matchLength)
                    {
                        SaveCurrentState();
                        break;
                    }
                } while (match[checkIndex++] == currentChar);
            }


            ////11_000 per pair, chosen through the scientific process of guess and check
            //const int CharThreshold = 14_000;
            //int threshold = ctext.Length * CharThreshold;
            // bi-gram analysis for score
            //int score = GetBiGramScore(bChar);
        }

        private StringBuilder sb = new StringBuilder();

        private void SaveCurrentState()
        {
            for(int i = 0; i < cypherText.Length; i++)
                sb.Append(cipher.UnScramble(cypherText, i));
            resultSaver.Save(new PotentialSolution(title, sb.ToString(), 0, cipher.GetCurrentKeys()));
        }
    }
}
