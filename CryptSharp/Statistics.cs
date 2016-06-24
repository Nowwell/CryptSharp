using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp
{
    public class Statistics
    {
        public Statistics(string cipher, char[] alphabet, double[] frequencies)
        {
            Cipher = cipher;
            Alphabet = alphabet;
            ExpectedFrequency = frequencies;
        }
        public Statistics(string cipher)
        {
            Cipher = "";
            Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            ExpectedFrequency = new double[] { 0.08167, 0.01492, 0.02782, 0.04253, 0.12702, 0.02228, 0.02015, 0.06094, 0.06966, 0.00153, 0.00772, 0.04025, 0.02406, 0.06749, 0.07507, 0.01929, 0.00095, 0.05987, 0.06327, 0.09056, 0.02758, 0.00978, 0.02361, 0.0015, 0.01974, 0.00074 };
        }

        public string Cipher { get; private set; }
        public char[] Alphabet { get; private set; }
        public double[] ExpectedFrequency { get; private set; }

        public  Dictionary<char, int> Frequencies()
        {
            Dictionary<char, int> returnValue = new Dictionary<char, int>();

            foreach (char c in Alphabet)
            {
                returnValue.Add(c, 0);
            }

            foreach (char c in Cipher)
            {
                returnValue[c]++;
            }
            return returnValue;
        }

        public double IndexOfCoincidence()
        {
            Dictionary<char, double> counts = new Dictionary<char, double>();
            foreach (char s in Cipher)
            {
                if (Alphabet.Contains(s))
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
            return sum / (double)(Cipher.Length * (Cipher.Length - 1));
        }
        public double IndexOfCoincidence(List<char> Cipher)
        {
            Dictionary<char, double> counts = new Dictionary<char, double>();
            foreach (char s in Cipher)
            {
                if (Alphabet.Contains(s))
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
            return sum / (double)(Cipher.Count * (Cipher.Count - 1));
        }

        public double Roughness()
        {
            return IndexOfCoincidence() - 1.0 / (double)Alphabet.Length;
        }

        public double ChiSquared()
        {
            return Math.Sqrt(((double)Alphabet.Length) * IndexOfCoincidence() - 1.0);
        }

        public double AvgVigenereIndexOfCoincidence(int keyLength)
        {
            List<List<char>> similarCiphers = new List<List<char>>();
            for (int i = 0; i < keyLength; i++)
            {
                similarCiphers.Add(new List<char>());
            }
            for (int i = 0; i < Cipher.Length; i++)
            {
                similarCiphers[i % keyLength].Add(Cipher[i]);
            }

            double sum = 0.0;
            for (int i = 0; i < similarCiphers.Count; i++)
            {
                double ic = IndexOfCoincidence(similarCiphers[i]);

                sum += ic;
            }
            return sum / (double)keyLength;
        }
    }
}
