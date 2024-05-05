using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCheck
{
    internal struct VigenereCipher : ICipher<VigenereCipher>
    {
        private int _at;
        private readonly BChar[] _table;
        private readonly BChar[] _alphabet;

        private readonly record struct KeyCache(BChar[] Table, BChar[] Alphabet);

        public VigenereCipher(string alphabetKey)
        {
            lock (generatedTables)
            {
                if (generatedTables.TryGetValue(alphabetKey, out KeyCache record))
                {
                    _table = record.Table;
                    _alphabet = record.Alphabet;
                }
                else
                {
                    #region Table+Alphabet
                    BChar[] newTable = new BChar[alphaLen];
                    Array.Fill(newTable, BChar.None);

                    BChar[] newAlphabet = new BChar[alphaLen];
                    Array.Fill(newAlphabet, BChar.None);

                    for (byte i = 0; i < alphabetKey.Length; i++)
                    {
                        BChar keyChar = alphabetKey[i];
                        newTable[keyChar._char] = new BChar(i);
                        newAlphabet[i] = keyChar;
                    }

                    byte otherCounter = (byte)alphabetKey.Length;
                    byte alphaCounter = 0;
                    for (byte i = 0; i < alphaLen; i++)
                    {
                        if (newTable[i] == BChar.None)
                            newTable[i] = new BChar(otherCounter++);

                        if (newAlphabet[i] == BChar.None)
                        {
                            while (alphabetKey.Contains((char)(alphaCounter + 'A')))
                                alphaCounter++;
                            newAlphabet[i] = new BChar(alphaCounter++);
                        }
                    }
                    #endregion

                    generatedTables.Add(alphabetKey, new KeyCache(newTable, newAlphabet));
                    _table = newTable;
                    _alphabet = newAlphabet;
                }
            }
            _at = 0;
        }

        public readonly unsafe void UnScramble(ReadOnlySpan<BChar> characters, Span<BChar> output)
        {
            fixed (BChar* charPtr = &characters.GetPinnableReference())
            fixed (BChar* outPtr = &output.GetPinnableReference())
            fixed (BChar* tablePtr = _table)
            fixed (byte* modTablePtr = mod26Table)
            fixed (BChar* alphabetPtr = _alphabet)
            {//new with ptr - removes CLR bounds check ~30% faster (unsafe though)
                var at = Scrambler.AllWords[_at];
                int len = at.Length;
                int outputLen = output.Length;
                int modCtr = 0;

                for (int i = 0; i < outputLen; i++)
                {
                    outPtr[i] = alphabetPtr[modTablePtr[tablePtr[charPtr[i]._char]._char - tablePtr[at[modCtr]._char]._char + alphaLenByte]];

                    if (++modCtr == len)
                        modCtr = 0;
                }
            }
            /*
            var at = Scrambler.AllWords[_at];
            int len = at.Length;
            int outputLen = output.Length;
            int modCtr = 0;

            for (int i = 0; i < outputLen; i++)
            {
                //byte keyStreamIndex = _table[at[modCtr].AsIndex()].AsIndex();//keystream

                output[i] = _alphabet[mod26Table[_table[characters[i]._char]._char - _table[at[modCtr]._char]._char + alphaLenByte]];

                // a b c d e f g h i j k l m n o p q r s t u v w x y z

                // alpha table
                // 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5
                // k r y p t o s a b c d e f g h i j l m n q u v w x z

                // inv alpha table:
                // a  b  c  d  e  f  g  h  i  j  k  l  m  n  o  p  q  r  s  t  u  v  w  x  y  z
                // 8  9  10 11 12 13 14 15 16 17 1  18 19 20 6  4  21 2  7  5  22 23 24 25 3  26
                // h  i  j  k  l  m  n  o  p  q  a  r  s  t  f  d  u  b  g  e  v  w  x  y  c  z 

                //modCtr = (modCtr + 1) % len; -> ~20% slower

                if (++modCtr == len)
                    modCtr = 0;
            }*/
        }


        private const int modLen = 26 * 26;
        private const int alphaLen = 26;
        private const byte alphaLenByte = 26;
        private static readonly Dictionary<string, KeyCache> generatedTables = new();
        private static readonly byte[] mod26Table;
        public static bool MoveNext(ref VigenereCipher thing)
        {
            thing._at++;
            return thing._at < Scrambler.NumWords;
        }
        static VigenereCipher()
        {
            //mod table
            mod26Table = new byte[modLen];
            for (int i = 0; i < mod26Table.Length; i++)
            {
                mod26Table[i] = (byte)(i % 26);
            }
        }
    }

}
