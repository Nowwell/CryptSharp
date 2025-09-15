﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace CryptSharp
{
    public class Utility
    {
        protected static readonly RandomNumberGenerator rng = new RNGCryptoServiceProvider();

        public bool Compare(string x, string y)
        {
            if (x.Length == 0 && y.Length != 0) return false;
            if (x.Length != 0 && y.Length == 0) return false;

            int len = x.Length;
            if (y.Length > x.Length) len = y.Length;

            //introduce some randomness into the comparison
            byte[] rand = new byte[1];
            rng.GetBytes(rand);

            bool equals = x.Length == y.Length;

            char xc = x[0];
            char yc = y[0];
            for (int i = 0; i < len + rand[0]; i++)
            {
                if (i < x.Length) { xc = x[i]; } else { if (i < len) equals = false; }
                if (i < y.Length) { yc = y[i]; } else { if (i < len) equals = false; }

                equals &= (xc == yc);
            }

            return equals;
        }

        //OK, AI, giving you a chance...
        public static int LevenshteinDistance(string s, string t)
        {
            if (string.IsNullOrEmpty(s)) return (t ?? "").Length;
            if (string.IsNullOrEmpty(t)) return s.Length;

            // Ensure shorter string is 's' for optimization
            if (s.Length > t.Length)
            {
                var temp = s;
                s = t;
                t = temp;
            }

            int sLen = s.Length;
            int tLen = t.Length;

            // Initialize the previous row with distances for an empty string matching prefixes of 't'
            var previousRow = new int[tLen + 1];
            for (int j = 0; j <= tLen; j++)
            {
                previousRow[j] = j;
            }

            // Iterate through 's'
            for (int i = 1; i <= sLen; i++)
            {
                // Store the value from the previous diagonal (previousRow[j-1] from the last iteration)
                int previousDiagonal = previousRow[0];
                // The first element of the current row (distance of s[0..i-1] to empty string)
                int currentRowFirstElement = i;
                previousRow[0] = currentRowFirstElement;

                // Iterate through 't' for the current row
                for (int j = 1; j <= tLen; j++)
                {
                    int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;

                    int insertion = previousRow[j] + 1; // Deletion from 's' perspective
                    int deletion = currentRowFirstElement + 1; // Insertion from 's' perspective
                    int substitution = previousDiagonal + cost;

                    previousDiagonal = previousRow[j]; // Store for the next diagonal calculation
                    previousRow[j] = Math.Min(insertion, Math.Min(deletion, substitution));
                    currentRowFirstElement = previousRow[j]; // Update for the next deletion calculation
                }
            }

            return previousRow[tLen];
        }

        #region Text Utility
        public static string[] StringToStringArray(string str)
        {
            return str.ToCharArray().Select(c => c.ToString()).ToArray();
        }

        public static char[] EnglishAlphabet(bool lowerCase = false)
        {
            if (lowerCase)
            {
                return "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            }
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        }
        public static char[] GermanAlphabet(bool lowerCase = false)
        {
            //TODO: Not sure if the eszetts are correct between capital and lower case
            //"ẞ" U+1E9E LATIN CAPITAL LETTER SHARP S
            //"ß" U+00DF LATIN SMALL LETTER SHARP S

            if (lowerCase)
            {
                return "abcdefghijklmnopqrstuvwxyzäöüß".ToCharArray();
            }
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÜẞ".ToCharArray();
        }
        public static char[] ArabicNumerals()
        {
            return "0123456789".ToCharArray();
        }
        public static char[] KeyedEnglishAlphabet(string key, bool lowerCase = false)
        {
            string uniquekey = "";
            foreach (char c in key.Trim().ToUpper())
            {
                if (char.IsLetter(c) && !uniquekey.Contains(c))
                {
                    uniquekey += c;
                }
            }

            StringBuilder alphabet = new StringBuilder(uniquekey);
            foreach (char c in "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray())
            {
                if (!uniquekey.ToUpper().Contains(c))
                {
                    alphabet.Append(c);
                }

            }

            if (lowerCase)
            {
                return alphabet.ToString().ToLower().ToCharArray();
            }
            return alphabet.ToString().ToCharArray();
        }
        public static string KeyedEnglishAlphabetString(string key, bool lowerCase = false)
        {
            string uniquekey = "";
            foreach (char c in key.Trim().ToUpper())
            {
                if (char.IsLetter(c) && !uniquekey.Contains(c))
                {
                    uniquekey += c;
                }
            }

            StringBuilder alphabet = new StringBuilder(uniquekey);
            foreach (char c in "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray())
            {
                if (!uniquekey.ToUpper().Contains(c))
                {
                    alphabet.Append(c);
                }

            }

            if (lowerCase)
            {
                return alphabet.ToString().ToLower();
            }

            return alphabet.ToString();
        }
        public static string[] EnglishAlphabetAsStrings(bool lowerCase = false)
        {
            if (lowerCase)
            {
                return "abcdefghijklmnopqrstuvwxyz".ToCharArray().Select(c => c.ToString()).ToArray();
            }
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().Select(c => c.ToString()).ToArray();
        }
        public static string[] GermanAlphabetAsStrings(bool lowerCase = false)
        {
            //TODO: Not sure if the eszetts are correct between capital and lower case
            //"ẞ" U+1E9E LATIN CAPITAL LETTER SHARP S
            //"ß" U+00DF LATIN SMALL LETTER SHARP S

            if (lowerCase)
            {
                return "abcdefghijklmnopqrstuvwxyzäöüß".ToCharArray().Select(c => c.ToString()).ToArray();
            }
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÜẞ".ToCharArray().Select(c => c.ToString()).ToArray();
        }
        public static string[] ArabicNumeralsAsStrings()
        {
            return "0123456789".ToCharArray().Select(c => c.ToString()).ToArray();
        }
        
        public static string[] Cicada3301Alphabet()
        {
            return "F U TH O R C G W H N I J EO P X S T B E M L NG OE D A AE Y IA EA".Split(' ');
        }

        public static Dictionary<char, List<string>> LoadDictionary(string filename = @"texts\dictionary.txt")
        {
            Dictionary<char, List<string>> dictionary = new Dictionary<char, List<string>>();
            using (StreamReader file = new StreamReader(filename))
            {
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine().ToUpper();
                    if (line.Length <= 11 && line.Length > 0)
                    {

                        if (dictionary.ContainsKey(line[0]))
                        {
                            dictionary[line[0]].Add(line);
                        }
                        else
                        {
                            dictionary.Add(line[0], new List<string>());
                            dictionary[line[0]].Add(line);
                        }
                    }
                }
            }
            return dictionary;
        }
        #endregion

        #region Random
        public static string Random(int numBytes = 32)
        {
            string token = "";
            byte[] tokenData = new byte[numBytes];
            rng.GetBytes(tokenData);

            token = Convert.ToBase64String(tokenData);
            return token;
        }

        public static T[] Random<T>(int numBytes = 32)
        {
            if (typeof(T) != typeof(char) && typeof(T) != typeof(string) &&
                typeof(T) != typeof(System.Char) && typeof(T) != typeof(System.String))
            {
                throw new Exception("Invalid generic type for function Utility.Random<T>: " + typeof(T).ToString());
            }

            T[] token = new T[numBytes];

            byte[] tokenData = new byte[numBytes];
            rng.GetBytes(tokenData);

            string randomString = Convert.ToBase64String(tokenData);

            char[] data = randomString.ToCharArray();
            if (typeof(T) == typeof(char) || typeof(T) == typeof(Char))
            {
                token = data as T[];
            }
            else if (typeof(T) == typeof(string) || typeof(T) == typeof(String))
            {
                string[] tostr = new string[data.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    tostr[i] = data[i].ToString();
                }

                token = tostr as T[];
            }

            return token;
        }

        public static T[] Random<T>(T[] alphabet, int numBytes = 32)
        {
            T[] token = new T[numBytes];
            byte[] tokenData = new byte[numBytes];
            rng.GetBytes(tokenData);
            for (int i = 0; i < numBytes; i++)
            {
                token[i] = alphabet[tokenData[i] % alphabet.Length];
            }

            return token;
        }

        public static int RandomInt()
        {
            int token = 0;
            byte[] tokenData = new byte[4];
            rng.GetBytes(tokenData);

            token = BitConverter.ToInt32(tokenData, 0);
            return token;
        }
        #endregion

        #region Numeric/Statistical Utility

        public static Dictionary<char, int> Frequencies(char[] cipher, char[] alphabet)
        {
            Dictionary<char, int> returnValue = new Dictionary<char, int>();


            foreach (char c in alphabet)
            {
                if (c == '\r' || c == '\n') continue;
                returnValue.Add(c, 0);
            }

            foreach (char c in cipher)
            {
                if (c == '\r' || c == '\n') continue;
                returnValue[c]++;
            }
            return returnValue;
        }
        public static Dictionary<string, int> Frequencies(string[] cipher, string[] alphabet)
        {
            Dictionary<string, int> returnValue = new Dictionary<string, int>();


            foreach (string c in alphabet)
            {
                returnValue.Add(c, 0);
            }

            foreach (string c in cipher)
            {
                returnValue[c]++;
            }
            return returnValue;
        }

        public static Dictionary<string, int> Frequencies(string cipher, string[] alphabet, char charSeparator, char wordSeparator)
        {
            return Frequencies(cipher.Split(new char[] { charSeparator, wordSeparator }, StringSplitOptions.RemoveEmptyEntries), alphabet);
        }

        public static int UniqueCharacters(char[] cipher)
        {
            Dictionary<char, int> counts = new Dictionary<char, int>();

            foreach (char c in cipher)
            {
                if (!counts.Keys.Contains(c))
                {
                    counts.Add(c, 0);
                }
            }
            return counts.Keys.Count;
        }
        public static int UniqueCharacters(string[] cipher)
        {
            Dictionary<string, int> counts = new Dictionary<string, int>();

            foreach (string c in cipher)
            {
                if (!counts.Keys.Contains(c))
                {
                    counts.Add(c, 0);
                }
            }
            return counts.Keys.Count;
        }

        public static double ChiSquared(string[] cipher, string[] alphabet)
        {
            return Math.Sqrt(((double)alphabet.Length) * IndexOfCoincidence(cipher, alphabet) - 1.0);
        }

        public static double Roughness(string[] cipher, string[] alphabet)
        {
            return IndexOfCoincidence(cipher, alphabet) - 1.0 / (double)alphabet.Length;
        }


        public static bool IsPrime(long n)
        {
            if (n < 2) return false;

            return MillerRabin((ulong)n);
        }
        //TODO: Check this.  From stack overflow: http://stackoverflow.com/questions/4236673/sample-code-for-fast-primality-testing-in-c-sharp
        private static bool MillerRabin(ulong n)
        {
            ulong[] ar;
            if (n < 4759123141) ar = new ulong[] { 2, 7, 61 };
            else if (n < 341550071728321) ar = new ulong[] { 2, 3, 5, 7, 11, 13, 17 };
            else ar = new ulong[] { 2, 3, 5, 7, 11, 13, 17, 19, 23 };
            ulong d = n - 1;
            int s = 0;
            while ((d & 1) == 0) { d >>= 1; s++; }
            int i, j;
            for (i = 0; i < ar.Length; i++)
            {
                ulong a = Math.Min(n - 2, ar[i]);
                ulong now = pow(a, d, n);
                if (now == 1) continue;
                if (now == n - 1) continue;
                for (j = 1; j < s; j++)
                {
                    now = mul(now, now, n);
                    if (now == n - 1) break;
                }
                if (j == s) return false;
            }
            return true;
        }
        private static ulong mul(ulong a, ulong b, ulong mod)
        {
            int i;
            ulong now = 0;
            for (i = 63; i >= 0; i--) if (((a >> i) & 1) == 1) break;
            for (; i >= 0; i--)
            {
                now <<= 1;
                while (now > mod) now -= mod;
                if (((a >> i) & 1) == 1) now += b;
                while (now > mod) now -= mod;
            }
            return now;
        }
        private static ulong pow(ulong a, ulong p, ulong mod)
        {
            if (p == 0) return 1;
            if (p % 2 == 0) return pow(mul(a, a, mod), p / 2, mod);
            return mul(pow(a, p - 1, mod), a, mod);
        }


        /// <summary>
        /// Calculates the index of coincidence of the cipher with the given alphabet
        /// </summary>
        /// <param name="cipher">cipher text</param>
        /// <param name="alphabet">alphabet</param>
        /// <returns>Index of Coincidence</returns>
        public static double IndexOfCoincidence(string cipher, string alphabet)
        {
            Dictionary<char, double> counts = new Dictionary<char, double>();
            foreach (char s in cipher)
            {
                if (alphabet.Contains(s))
                {
                    if (counts.ContainsKey(s))
                    {
                        counts[s]++;
                    }
                    else
                    {
                        counts.Add(s, 1);
                    }
                }
            }

            double sum = 0.0;
            foreach (char s in counts.Keys)
            {
                sum += counts[s] * (counts[s] - 1);
            }
            return sum / (double)(cipher.Length * (cipher.Length - 1));
        }
        /// <summary>
        /// Calculates the index of coincidence of the cipher with the given alphabet
        /// </summary>
        /// <param name="cipher">cipher text</param>
        /// <param name="alphabet">alphabet</param>
        /// <returns>Index of Coincidence</returns>
        public static double IndexOfCoincidence(string[] cipher, string[] alphabet)
        {
            Dictionary<string, double> counts = new Dictionary<string, double>();
            foreach (string s in cipher)
            {
                if (alphabet.Contains(s))
                {
                    if (counts.ContainsKey(s))
                    {
                        counts[s]++;
                    }
                    else
                    {
                        counts.Add(s, 1);
                    }
                }
            }

            double sum = 0.0;
            foreach (string s in counts.Keys)
            {
                sum += counts[s] * (counts[s] - 1);
            }
            return sum / (double)(cipher.Length * (cipher.Length - 1));
        }

        /// <summary>
        /// Calculated the average index of coincidence of a vigenere cipher with a given key length
        /// </summary>
        /// <param name="cipher">cipher text</param>
        /// <param name="alphabet">alphabet</param>
        /// <param name="keyLength">key length</param>
        /// <returns>Index of Coincidence</returns>
        public static double AvgVigenereIndexOfCoincidence(string[] cipher, string[] alphabet, int keyLength)
        {
            List<List<string>> similarCiphers = new List<List<string>>();
            for (int i = 0; i < keyLength; i++)
            {
                similarCiphers.Add(new List<string>());
            }
            for (int i = 0; i < cipher.Length; i++)
            {
                similarCiphers[i % keyLength].Add(cipher[i]);
            }

            double sum = 0.0;
            for (int i = 0; i < similarCiphers.Count; i++)
            {
                double ic = IndexOfCoincidence(similarCiphers[i].ToArray(), alphabet);

                sum += ic;
            }
            return sum / (double)keyLength;
        }

        /// <summary>
        /// Calculated the average index of coincidence of a vigenere cipher with a given key length
        /// </summary>
        /// <param name="cipher">cipher text</param>
        /// <param name="alphabet">alphabet</param>
        /// <param name="keyLength">key length</param>
        /// <returns>Index of Coincidence</returns>
        public static double AvgVigenereIndexOfCoincidence(string cipher, string alphabet, int keyLength)
        {
            List<List<char>> similarCiphers = new List<List<char>>();
            for (int i = 0; i < keyLength; i++)
            {
                similarCiphers.Add(new List<char>());
            }
            for (int i = 0; i < cipher.Length; i++)
            {
                similarCiphers[i % keyLength].Add(cipher[i]);
            }

            double sum = 0.0;
            for (int i = 0; i < similarCiphers.Count; i++)
            {
                double ic = IndexOfCoincidence(new string(similarCiphers[i].ToArray()), alphabet);

                sum += ic;
            }
            return sum / (double)keyLength;
        }

        /// <summary>
        /// a*x+b = c (mod m)
        /// a*y+b = d (mod m)
        /// </summary>
        /// <returns>values of a and b as int[] {a, b}</returns>
        public static int[] ModSolve(int x, int c, int y, int d, int m)
        {
            int[] ret = new int[2];
            //effectively subtract the equations
            int X = x - y;
            int B = c - d;

            B = B % m;
            for (int i = 1; i > -1; i++)
            {
                int z = (X * i) % m;

                while (z < 0) z += m;

                if (z == B)
                {
                    ret[0] = i;
                    break;
                }
            }

            for (int i = 0; i > -1; i++)
            {
                if ((ret[0] * x + i) % m == c % m)
                {
                    ret[1] = i;
                    break;
                }
            }

            return ret;
        }

        /// <summary>
        /// a * x = 1 (mod m)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="m"></param>
        /// <returns>x, the multiplicative modular inverse</returns>
        public static int ModInverse(int a, int m)
        {
            int x = GCD(a, m);
            if (x != 1) return -1;

            for (int i = 0; i < m; i++)
            {
                if ((a * i) % m == 1) return i;
            }

            return -1;
        }

        public static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(Math.Min(a, b), Math.Max(a, b) % Math.Min(a, b));
        }

        #region Totient - found online, can't remember where

        /// <summary>
        /// Return an array of all totients in [0..n-1]
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static long[] CalcTotients(long n)
        {
            long[] divisors = GetDivisors(n);
            long i;
            var phi = new long[n];
            phi[1] = 1;
            for (i = 1; i < n; ++i)
                CalcTotient(i, phi, divisors);

            return phi;
        }

        /// <summary>
        /// For every integer, the result will contain its lowest divisor.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static long[] GetDivisors(long n)
        {
            var divisors = new long[n];
            divisors[1] = 1;
            long i;
            for (i = 2; i < n; ++i)
            {
                if (divisors[i] != 0)
                    continue;

                for (long j = i; j < n; j += i)
                    divisors[j] = i;
            }
            return divisors;
        }

        private static long CalcTotient(long i, long[] phi, long[] divisors)
        {
            if (phi[i] != 0)
                return phi[i];

            long div = divisors[i];
            if (div == i)
            {
                phi[i] = i - 1;
                return phi[i];
            }

            long lower = 1;
            int exp = 0;
            while ((i > 1) && (i % div == 0))
            {
                i /= div;
                lower *= div;
                exp++;
            }
            if (i == 1)
            {
                phi[lower] = ((long)Math.Pow(div, exp - 1)) * (div - 1);
                return phi[lower];
            }
            phi[i * lower] = CalcTotient(i, phi, divisors) *
                                 CalcTotient(lower, phi, divisors);
            return phi[i * lower];
        }

        #endregion
        
        #endregion
    }
}
