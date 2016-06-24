using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CryptSharp
{
    public class Utility
    {
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
            StringBuilder alphabet;
            if (lowerCase)
            {
                alphabet = new StringBuilder(key.ToLower());
                foreach (char c in "abcdefghijklmnopqrstuvwxyz".ToCharArray())
                {
                    if (!key.Contains(c))
                    {
                        alphabet.Append(c);
                    }

                }

                return alphabet.ToString().ToCharArray();
            }

            alphabet = new StringBuilder(key.ToUpper());
            foreach (char c in "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray())
            {
                if (!key.Contains(c))
                {
                    alphabet.Append(c);
                }

            }

            return alphabet.ToString().ToCharArray();
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

        public Dictionary<string, string> GenerateGenericBaconaianTableForEnglish()
        {
            Dictionary<string, string> SubstitutionTable = new Dictionary<string, string>();
            SubstitutionTable.Add("A", "aaaaa");
            SubstitutionTable.Add("B", "aaaab");
            SubstitutionTable.Add("C", "aaaba");
            SubstitutionTable.Add("D", "aaabb");
            SubstitutionTable.Add("E", "aabaa");
            SubstitutionTable.Add("F", "aabab");
            SubstitutionTable.Add("G", "aabba");
            SubstitutionTable.Add("H", "aabbb");
            SubstitutionTable.Add("I", "abaaa");
            SubstitutionTable.Add("J", "abaaa");
            SubstitutionTable.Add("K", "abaab");
            SubstitutionTable.Add("L", "ababa");
            SubstitutionTable.Add("M", "ababb");
            SubstitutionTable.Add("N", "abbaa");
            SubstitutionTable.Add("O", "abbab");
            SubstitutionTable.Add("P", "abbba");
            SubstitutionTable.Add("Q", "abbbb");
            SubstitutionTable.Add("R", "baaaa");
            SubstitutionTable.Add("S", "baaab");
            SubstitutionTable.Add("T", "baaba");
            SubstitutionTable.Add("U", "baabb");
            SubstitutionTable.Add("V", "baabb");
            SubstitutionTable.Add("W", "babaa");
            SubstitutionTable.Add("X", "babab");
            SubstitutionTable.Add("Y", "babba");
            SubstitutionTable.Add("Z", "babbb");
            return SubstitutionTable;
        }

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

        public static string Random(int numBytes = 32)
        {
            string token = "";
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[numBytes];
                rng.GetBytes(tokenData);

                token = Convert.ToBase64String(tokenData);
            }
            return token;
        }
        public static int RandomInt()
        {
            int token = 0;
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[4];
                rng.GetBytes(tokenData);

                token = BitConverter.ToInt32(tokenData, 0);
            }
            return token;
        }

        public static string ScrambleAlphabet(char[] alphabet)
        {
            List<char> letters = new List<char>(alphabet);
            StringBuilder scramble = new StringBuilder();
            for (int i = 0; i < alphabet.Length - 1; i++)
            {
                int x = RandomInt();
                scramble.Append(letters[x % letters.Count]);
                letters.RemoveAt(x % letters.Count);
            }
            scramble.Append(letters[0]);

            return scramble.ToString();
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
        /// <returns>x, the multiplicative mudular inverse</returns>
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
    }
}
