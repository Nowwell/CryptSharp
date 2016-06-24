using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp
{
    public static class CharArrayExtensions
    {
        public static int indexOf(this char[] array, char c)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == c) return i;
            }
            return -1;
        }
    }

    public static class StringExtensions
    {
        public static string Left(this string str, int count)
        {
            if (str.Length < count) return str;

            return str.Substring(0, count);
        }

        public static string Right(this string str, int count)
        {
            if (str.Length < count) return str;

            return str.Substring(-count, count);
        }

    }
}
