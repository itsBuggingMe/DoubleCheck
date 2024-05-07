using System.Runtime.CompilerServices;

namespace DoubleCheck
{
    internal struct VigenereCipher : ICipher<VigenereCipher>
    {
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
                    _table = new FChar[alphaLen];
                    Array.Fill(_table, FChar.None);

                    _alphabet = new FChar[alphaLen];
                    Array.Fill(_alphabet, FChar.None);

                    for (byte i = 0; i < alphabetKey.Length; i++)
                    {
                        FChar keyChar = alphabetKey[i];
                        _table[keyChar.Value] = new FChar(i);
                        _alphabet[i] = keyChar;
                    }

                    byte otherCounter = (byte)alphabetKey.Length;
                    byte alphaCounter = 0;
                    for (byte i = 0; i < alphaLen; i++)
                    {
                        if (_table[i] == FChar.None)
                            _table[i] = new FChar(otherCounter++);

                        if (_alphabet[i] == FChar.None)
                        {
                            while (alphabetKey.Contains((char)(alphaCounter + 'A')))
                                alphaCounter++;
                            _alphabet[i] = new FChar(alphaCounter++);
                        }
                    }
                    #endregion

                    generatedTables.Add(alphabetKey, new KeyCache(_table, _alphabet));
                }
            }
        }

        public readonly object[] GetCurrentKeys() => [Scrambler.AllWords[_at].ArrToString()];

        public readonly FChar UnScramble(ReadOnlySpan<FChar> characters, int index)
        {
            //normal imp
            FChar[] word = Scrambler.AllWords[_at];
            return _alphabet[mod26Table[_table[characters[index].Value].Value - _table[word[index % word.Length].Value].Value + alphaLenByte]];
        }

        private int _at;
        private readonly FChar[] _table;
        private readonly FChar[] _alphabet;

        private readonly record struct KeyCache(FChar[] Table, FChar[] Alphabet);


        #region Static
        private const int modLen = 26 * 26;
        private const int alphaLen = 26;
        private const byte alphaLenByte = 26;
        private static readonly Dictionary<string, KeyCache> generatedTables = new();
        private static readonly byte[] mod26Table = Enumerable.Range(0, modLen).Select(i => (byte)(i % 26)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ResetCounter(ref VigenereCipher thing) => thing._at = 0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool MoveNext(ref VigenereCipher thing) => ++thing._at < Scrambler.NumWords;
        #endregion Static
    }

}
