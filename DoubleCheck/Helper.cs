using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DoubleCheck.Program;

namespace DoubleCheck
{
    internal static class Helper
    {
        #region Extensions
        public static bool StartsWith(this Span<FChar> bString, string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] != bString[i])
                    return false;
            }

            return true;
        }

        public static string ArrToString(this FChar[] arr)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var r in arr)
            {
                stringBuilder.Append((char)r);
            }
            return stringBuilder.ToString();
        }

        public static string ToString(this FChar[] arr)
        {
            StringBuilder sb = new StringBuilder(arr.Length);
            for (int i = 0; i < arr.Length; i++)
                sb.Append((char)arr[i]);
            return sb.ToString();
        }

        public static int Checksum(this Span<FChar> bChars)
        {
            int s = 0;
            for (int i = 0; i < bChars.Length; i++)
                s += bChars[i];
            return s;
        }

        public static FChar[] AsBCharArr(this string line)
        {
            FChar[] arr = new FChar[line.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = line[i];
            }
            return arr;
        }
        #endregion Extensions
    }
}
