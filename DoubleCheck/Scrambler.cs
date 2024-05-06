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
        protected readonly int StartIndex;
        protected readonly int Count;
        protected readonly IResultSaver resultSaver;
        protected readonly string title;

        public Scrambler(int startIndex, int count, IResultSaver saver, string title)
        {
            if (startIndex < 0 || startIndex + count > AllWords.Length)
                throw new ArgumentException("Bad");

            resultSaver = saver;
            StartIndex = startIndex;
            Count = count;
            this.title = title;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetBiGramScore(Span<BChar> bChars)
        {
            int score = 0;
            int len = bChars.Length - 1;

            byte l = bChars[0]._char;
            for (int i = 1; i < len; i++)
            {
                byte r = bChars[i]._char;
                score += BiGramTable[l * 26 + r];
                l = r;
            }

            return score;
        }

        public abstract void TryAllCombos();

        public static int NumWords => AllWords.Length;
        public static readonly BChar[][] AllWords;
        public static readonly string[] AllWordsString;
        public static readonly int[] BiGramTable;

        static Scrambler()
        {
            string[] raw = File.ReadAllLines("words.txt");
            AllWords = raw
                .Select(s => 
                    s.Where(char.IsLetter).Select(c => (BChar)char.ToUpper(c)).ToArray()
                    )
                .Where(arr => arr.Length > 0)
                .ToArray();
            AllWordsString = raw
                .Select(s =>
                    s.Where(char.IsLetter).Select(c => char.ToUpper(c)).ToArray()
                    ).Select(carr => new string(carr))
                .Where(arr => arr.Length > 0)
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
                BChar left = char.ToUpper(rawScores[i].Item1[0]);
                BChar right = char.ToUpper(rawScores[i].Item1[1]);
                BiGramTable[left._char * 26 + right._char] = (int)(rawScores[i].Item2 / 2858953);//squash it down
            }
        }
    }
}
