using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCheck
{
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal readonly struct FChar(int value)
    {
        public readonly int Value = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator FChar(char value) => new FChar(ToByte(value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator char(FChar value) => ToChar(value.Value);

        public static char ToChar(int value) => (char)('A' + value);
        public static int ToByte(char value) => (int)(value - 'A');

        public override string ToString()
        {
            return ToChar(Value).ToString();
        }

        public static FChar None = new FChar(int.MaxValue);
    }
}
