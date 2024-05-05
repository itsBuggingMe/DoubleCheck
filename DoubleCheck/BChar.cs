using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCheck
{
    internal readonly struct BChar
    {
        private readonly byte _char;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BChar(byte value)
        {
            _char = value;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator BChar(char value) => new BChar(ToByte(value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator char(BChar value) => ToChar(value._char);

        public static char ToChar(byte value) => (char)('A' + value);
        public static byte ToByte(char value) => (byte)(value - 'A');

        public override string ToString()
        {
            return ToChar(_char).ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int AsIndex()
        {
            return _char;
        }

        public static BChar None = new BChar(byte.MaxValue);
    }

    internal static class Extensions
    {
        public static string ToString(this BChar[] arr)
        {
            StringBuilder sb = new StringBuilder(arr.Length);
            for(int i = 0; i < arr.Length; i++)
                sb.Append((char)arr[i]);
            return sb.ToString();
        }
    }
}
