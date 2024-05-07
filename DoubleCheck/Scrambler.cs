using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCheck
{
    internal abstract class Scrambler
    {
        protected readonly IResultSaver resultSaver;
        protected readonly string title;

        public Scrambler(IResultSaver saver, string title)
        {
            resultSaver = saver;
            this.title = title;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetBiGramScore(Span<FChar> bChars)
        {
            int score = 0;
            int len = bChars.Length - 1;

            int l = bChars[0].Value;
            for (int i = 1; i < len; i++)
            {
                int r = bChars[i].Value;
                score += BiGramTable[l * 26 + r];
                l = r;
            }

            return score;
        }

        public abstract void TryAllCombos();

        public static int NumWords => AllWords.Length;
        public static readonly FChar[][] AllWords;
        public static readonly string[] AllWordsString;
        public static readonly int[] BiGramTable;

        static Scrambler()
        {
            //TODO: Remove doubles
            //TODO: reimplement custom checkerse

            string[] raw = File.ReadAllLines("words.txt");
            AllWords = raw
                .Select(s => 
                    s.Where(char.IsLetter).Select(c => (FChar)char.ToUpper(c)).ToArray()
                    )
                .Where(arr => arr.Length > 0 && arr.Length < 26)
                .ToArray();
            AllWordsString = raw
                .Select(s =>
                    s.Where(char.IsLetter).Select(c => char.ToUpper(c)).ToArray()
                    ).Select(carr => new string(carr))
                .Where(arr => arr.Length > 0 && arr.Length < 26)
                .ToArray();

            BiGramTable = new int[26 * 26 * 26];

            (string, long)[] rawScores =
                File.ReadAllLines("count_2l.txt")
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => s.Split('\t'))
                .Select(s => (s[0], long.Parse(s[1])))
                .ToArray();

            for (int i = 0; i < rawScores.Length; i++)
            {
                FChar left = char.ToUpper(rawScores[i].Item1[0]);
                FChar right = char.ToUpper(rawScores[i].Item1[1]);
                BiGramTable[left.Value * 26 + right.Value] = (int)(rawScores[i].Item2 / 2858953);//squash it down
            }
        }
    }
}
