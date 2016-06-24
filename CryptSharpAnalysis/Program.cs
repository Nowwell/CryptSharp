using CryptSharp.Ciphers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharpAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = "OBKRUOXOGHULBSOLIFBBWFLRVQQPRNGKSSOTWTQSJQSSEKZZWATJKLUDIAWINFBNYPVTTMZFPKWGDKZXTJCDIGKUHUAUEKCAR";// "OBKRUOXOGHULBSOLIFBBWFLRVQQPRNGKSSOTWTQSJQSSEKZZWATJKLUDIAWINFBNYPVTTMZFPKWGDKZXTJCDIGKUHUAUEKCAR";
            //string textRev = "RACKEUAUHUKGIDCJTXZKDGWKPFZMTTVPYNBFNIWAIDULKJTAWZZKESSQJSQTWTOSSKGNRPQQVRLFWBBFILOSBLUHGOXOURKBO";
            //string textRev2 = "RKBOOSSKGNRPQQVRLFWBBFILOSBLUHGOXOUPYNBFNIWAIDULKJTAWZZKESSQJSQTWTRACKEUAUHUKGIDCJTXZKDGWKPFZMTTV";
            //string kryptos = "KRYPTOS";
            //string kryptosPermutate = "KRYPTOSSKRYPTOOSKRYPTTOSKRYPPTOSKRYYPTOSKRRYPTOSK";
            //string progressiveKey = "KRYPTOSRYPTOSAYPTOSABPTOSABCTOSABCDOSABCDE";
            string kryptosAlphabet = "KRYPTOSABCDEFGHIJLMNQUVWXZ";
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //string clear = "BERLINCLOCKPXAAAAA";

            Dictionary<char, List<string>> dictionary = new Dictionary<char, List<string>>();
            using (StreamReader file = new StreamReader(@"texts\dictionary.txt"))
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

            Columnar col = new Columnar(kryptosAlphabet.ToCharArray());

            foreach (char c in dictionary.Keys)
            {
                foreach (string word in dictionary[c])
                {
                    col.Key = word;

                    string middle = col.Decrypt(text);

                    for (int i = 2; i < text.Length / 3; i++)
                    {
                        double ic = CryptSharp.Utility.AvgVigenereIndexOfCoincidence(middle, kryptosAlphabet, i);

                        if (ic >= 0.060)
                        {
                            double ic2 = CryptSharp.Utility.AvgVigenereIndexOfCoincidence(middle, kryptosAlphabet, 2 * i);
                            if (ic2 >= 0.06)
                            {
                                //Console.WriteLine("{0}, {1}, {2}", word, i, ic);
                                //Console.WriteLine("{0}, {1}, {2}", word, 2 * i, ic);
                                Vigenere vig = new CryptSharp.Ciphers.Vigenere(kryptosAlphabet.ToCharArray(), word);
                                //if (middle.Contains("BERLIN"))
                                //{
                                    Console.WriteLine("{0},{1}", word, vig.Decrypt(middle));
                                //}
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Done");
            Console.ReadKey();

            //double ic = IndexOfCoincidence(text, kryptosAlphabet);

            ////in english, using english alphabet
            //double[] letterFreq = { 0.08167, 0.01492, 0.02782, 0.04253, 0.12702, 0.02228, 0.02015, 0.06094, 0.06966, 0.00153, 0.00772, 0.04025, 0.02406, 0.06749, 0.07507, 0.01929, 0.00095, 0.05987, 0.06327, 0.09056, 0.02758, 0.00978, 0.02361, 0.0015, 0.01974, 0.00074 };
            ////in english, in order of the kyrptos alphabet
            //double[] letterFreqKryptos = { 0.00772, 0.05987, 0.01974, 0.01929, 0.09056, 0.07507, 0.06327, 0.08167, 0.01492, 0.02782, 0.04253, 0.12702, 0.02228, 0.02015, 0.06094, 0.06966, 0.00153, 0.04025, 0.02406, 0.06749, 0.00095, 0.05987, 0.02758, 0.00978, 0.02361, 0.0015, 0.00074 };

            //Vigenere vig = new CryptSharp.Ciphers.Vigenere(kryptosAlphabet.ToCharArray());
            ////"OBKRUOXOGHULBSOLIFBBWFLRVQQPRNGKSSOTWTQSJQSSEKZZWATJKLUDIAWINFBNYPVTTMZFPKWGDKZXTJCDIGKUHUAUEKCAR";
            //vig.Key = "QKEORO";// "WXZKYXZKRYPTOSABCDEFGHIJLMNQUVWXZKRZZKRYPTOSABCDEFGHIJLMNQUVWXZKRYABCDEFGHIJKLMNOPQRSTUVWXYZABCD";//"QUXAEQ";//

            //clear = vig.Encrypt("KRYPTOS");
            ////clear = vig.Encrypt(textRev);

            //double x = IndexOfCoincidence(clear, kryptosAlphabet);

            //Dictionary<string, int> digraphs = new Dictionary<string, int>();
            //Dictionary<string, int> trigraphs = new Dictionary<string, int>();
            //using (StreamReader text = new StreamReader(@"Texts\ATaleOfTwoCities.txt"))
            ////using (StreamReader text = new StreamReader(@"Texts\Gadsby.txt"))
            //{
            //    string rawdata = text.ReadToEnd().ToUpper();
            //    StringBuilder data = new StringBuilder();

            //    foreach (char c in rawdata)
            //    {
            //        if (ContainsChar(c, alphabet))
            //        {
            //            data.Append(c);
            //        }
            //    }

            //    string digraph = "" + data[0] + data[1];
            //    digraphs.Add(digraph, 1);
            //    for (int i = 2; i < data.Length; i++)
            //    {
            //        digraph = digraph.Remove(0, 1) + data[i];

            //        if (digraphs.ContainsKey(digraph))
            //        {
            //            digraphs[digraph]++;
            //        }
            //        else
            //        {
            //            digraphs.Add(digraph, 1);
            //        }

            //    }

            //    using (StreamWriter output = new StreamWriter(@"Texts\Digraphs.csv"))
            //    {
            //        foreach (string s in digraphs.Keys)
            //        {
            //            output.WriteLine("{0},{1}", s, digraphs[s]);

            //        }
            //        output.Flush();
            //        output.Close();
            //    }



            //    digraphs.Clear();
            //    for (int i = 1; i < data.Length - 2; i++)
            //    {
            //        digraph = "" + data[i] + data[i + 2];

            //        if (digraphs.ContainsKey(digraph))
            //        {
            //            digraphs[digraph]++;
            //        }
            //        else
            //        {
            //            digraphs.Add(digraph, 1);
            //        }

            //    }

            //    using (StreamWriter output = new StreamWriter(@"Texts\Skips.csv"))
            //    {
            //        foreach (string s in digraphs.Keys)
            //        {
            //            output.WriteLine("{0},{1}", s, digraphs[s]);

            //        }
            //        output.Flush();
            //        output.Close();
            //    }

            //    digraphs.Clear();
            //    for (int i = 1; i < data.Length - 3; i++)
            //    {
            //        digraph = "" + data[i] + data[i + 3];

            //        if (digraphs.ContainsKey(digraph))
            //        {
            //            digraphs[digraph]++;
            //        }
            //        else
            //        {
            //            digraphs.Add(digraph, 1);
            //        }

            //    }

            //    using (StreamWriter output = new StreamWriter(@"Texts\SkipTwos.csv"))
            //    {
            //        foreach (string s in digraphs.Keys)
            //        {
            //            output.WriteLine("{0},{1}", s, digraphs[s]);

            //        }
            //        output.Flush();
            //        output.Close();
            //    }

            //    string trigraph = "";
            //    for (int i = 0; i < data.Length - 2; i++)
            //    {
            //        trigraph = "" + data[i] + data[i + 1] + data[i + 2];

            //        if (trigraphs.ContainsKey(trigraph))
            //        {
            //            trigraphs[trigraph]++;
            //        }
            //        else
            //        {
            //            trigraphs.Add(trigraph, 1);
            //        }

            //    }

            //    using (StreamWriter output = new StreamWriter(@"Texts\Trigraphs.csv"))
            //    {
            //        foreach (string s in trigraphs.Keys)
            //        {
            //            output.WriteLine("{0},{1}", s, trigraphs[s]);

            //        }
            //        output.Flush();
            //        output.Close();
            //    }


            //    Console.WriteLine(IndexOfCoincidence(data.ToString().Substring(0, 5000), alphabet));
            //}
        }

        public static string Vigenere(string text, string alphabet, string key)
        {
            StringBuilder output = new StringBuilder(text.ToUpper());

            Dictionary<char, int> indices = new Dictionary<char, int>();
            for (int i = 0; i < alphabet.Length; i++)
            {
                indices.Add(alphabet[i], i);
            }

            int z = (int)'Z';
            int a = (int)'A';

            for (int i = 0; i < text.Length; i++)
            {
                output[i] = alphabet[(indices[output[i]] - indices[key[i % key.Length]] + alphabet.Length) % alphabet.Length];
            }
            return output.ToString();
        }
        public static string VigenereRunningKey(string text, string alphabet, string key)
        {
            StringBuilder output = new StringBuilder(text.ToUpper());

            Dictionary<char, int> indices = new Dictionary<char, int>();
            int[] keyIndices = new int[key.Length];
            for (int i = 0; i < alphabet.Length; i++)
            {
                indices.Add(alphabet[i], i);
            }

            for (int j = 0; j < key.Length; j++)
            {
                for (int i = 0; i < alphabet.Length; i++)
                {
                    if (key[j] == alphabet[i])
                    {
                        keyIndices[j] = i;
                    }
                }
            }


            int z = (int)'Z';
            int a = (int)'A';

            for (int i = 0; i < text.Length; i++)
            {
                output[i] = alphabet[(indices[output[i]] - keyIndices[i % key.Length] + alphabet.Length) % alphabet.Length];
                //output[i] = alphabet[(indices[output[i]] - indices[key[i % key.Length]] + alphabet.Length) % alphabet.Length];

                if (i > 0 && i % key.Length == 0)
                {
                    for (int j = 0; j < key.Length; j++)
                    {
                        keyIndices[j] = (keyIndices[j] + 1) % alphabet.Length;
                    }
                }

            }
            return output.ToString();
        }

        public static string VigenereAutoKey(string text, string alphabet, string key)
        {
            StringBuilder output = new StringBuilder(text.ToUpper());

            Dictionary<char, int> indices = new Dictionary<char, int>();
            for (int i = 0; i < alphabet.Length; i++)
            {
                indices.Add(alphabet[i], i);
            }

            int z = (int)'Z';
            int a = (int)'A';

            for (int i = 0; i < key.Length; i++)
            {
                output[i] = alphabet[(indices[output[i]] - indices[key[i % key.Length]] + alphabet.Length) % alphabet.Length];
            }
            for (int i = key.Length; i < text.Length; i++)
            {
                output[i] = alphabet[(indices[output[i]] - indices[output[i - key.Length]] + alphabet.Length) % alphabet.Length];
            }
            return output.ToString();
        }

        #region tools

        private static bool ContainsChar(char c, string alphabet)
        {
            foreach (char ch in alphabet)
            {
                if (ch == c) return true;
            }
            return false;
        }

        private static double IndexOfCoincidence(string cipher, string alphabet)
        {
            Dictionary<char, double> counts = new Dictionary<char, double>();
            foreach (char c in cipher)
            {
                if (alphabet.Contains(c))
                {
                    if (counts.ContainsKey(c))
                    {
                        counts[c]++;
                    }
                    else
                    {
                        counts.Add(c, 1);
                    }
                }
            }

            double sum = 0.0;
            foreach (char s in counts.Keys)
            {
                sum += counts[s] * (counts[s] - 1);
            }
            return /*alphabet.Length * */ sum / (double)(cipher.Length * (cipher.Length - 1));
        }

        private static double CalculateICOfGuessedKeyLength(string cipher, string alphabet, int keyLength)
        {
            List<StringBuilder> similarCiphers = new List<StringBuilder>();
            for (int i = 0; i < keyLength; i++)
            {
                similarCiphers.Add(new StringBuilder());
            }
            for (int i = 0; i < cipher.Length; i++)
            {
                similarCiphers[i % keyLength].Append(cipher[i]);
            }

            double sum = 0.0;
            for (int i = 0; i < similarCiphers.Count; i++)
            {
                double ic = IndexOfCoincidence(similarCiphers[i].ToString(), alphabet);

                sum += ic;
            }
            return alphabet.Length * sum / (double)keyLength;
        }

        private static int[] Frequencies(string text, string alphabet)
        {
            Dictionary<char, int> freq = new Dictionary<char, int>();

            for (int i = 0; i < alphabet.Length; i++)
            {
                freq.Add(alphabet[i], 0);
            }

            for (int i = 0; i < text.Length; i++)
            {
                freq[text[i]]++;

            }

            return freq.Values.ToArray();
        }

        #endregion
    }
}


//int[] pN = new int[text.Length];
//int[] pK = new int[text.Length];

//int[] cN = new int[text.Length];
//int[] cK = new int[text.Length];

//StringBuilder less = new StringBuilder();
//StringBuilder more = new StringBuilder();
//int k = 0;
//int l = 0;
//int m = 0;
//int n = 0;
//for (int i = 0; i < text.Length; i++)
//{
//    for (int j = 0; j < kryptosAlphabet.Length; j++)
//    {
//        if (text[i] == kryptosAlphabet[j])
//        {
//            cK[k++] = j;
//            //less.Append(kryptosAlphabet[(j + 2600 - k) % kryptosAlphabet.Length]);
//            //more.Append(kryptosAlphabet[(j + k) % kryptosAlphabet.Length]);
//            //k++;
//        }
//        if (clear[i] == kryptosAlphabet[j])
//        {
//            pK[l++] = j;
//            //less.Append(kryptosAlphabet[(j + 2600 - k) % kryptosAlphabet.Length]);
//            //more.Append(kryptosAlphabet[(j + k) % kryptosAlphabet.Length]);
//            //k++;
//        }

//        if (text[i] == alphabet[j])
//        {
//            cN[m++] = j;
//            //less.Append(kryptosAlphabet[(j + 2600 - k) % kryptosAlphabet.Length]);
//            //more.Append(kryptosAlphabet[(j + k) % kryptosAlphabet.Length]);
//            //k++;
//        }
//        if (clear[i] == alphabet[j])
//        {
//            pN[n++] = j;
//            //less.Append(kryptosAlphabet[(j + 2600 - k) % kryptosAlphabet.Length]);
//            //more.Append(kryptosAlphabet[(j + k) % kryptosAlphabet.Length]);
//            //k++;
//        }
//    }
//}

//Console.Write(pN[0]);
//for (int i = 1; i < pN.Length; i++)
//{
//    Console.Write("," + pN[i]);
//}
//Console.WriteLine();

////Console.Write(pK[0]);
////for (int i = 1; i < pN.Length; i++)
////{
////    Console.Write("," + pK[i]);
////}
////Console.WriteLine();

//Console.Write(cK[0]);
//for (int i = 1; i < pN.Length; i++)
//{
//    Console.Write("," + cK[i]);
//}
//Console.WriteLine();

////Console.Write(cN[0]);
////for (int i = 1; i < pN.Length; i++)
////{
////    Console.Write("," + cN[i]);
////}
////Console.WriteLine();

//Console.WriteLine("~~~~~~~~~~~~");

////Console.Write("Cipher (N)-Clear (N): ");
////Console.Write((cN[0] - pN[0] + 26) % alphabet.Length);
////for (int i = 1; i < pN.Length; i++)
////{
////    Console.Write("," + (cN[i] - pN[i] + 26)%alphabet.Length);
////}
////Console.WriteLine();

////Console.Write("Clear (N)-Cipher (N): ");
////Console.Write((pN[0] - cN[0] + 26) % alphabet.Length);
////for (int i = 1; i < pN.Length; i++)
////{
////    Console.Write("," + (pN[i] - cN[i] + 26) % alphabet.Length);
////}
////Console.WriteLine();

////Console.Write("Cipher (K)-Clear (K): ");
////Console.Write((cK[0] - pK[0] + 26) % alphabet.Length);
////for (int i = 1; i < pN.Length; i++)
////{
////    Console.Write("," + (cK[i] - pK[i] + 26) % alphabet.Length);
////}
////Console.WriteLine();

////Console.Write("Clear (K)-Cipher (K): ");
////Console.Write((pK[0] - cK[0] + 26) % alphabet.Length);
////for (int i = 1; i < pN.Length; i++)
////{
////    Console.Write("," + (pK[i] - cK[i] + 26) % alphabet.Length);
////}
////Console.WriteLine();


//Console.Write("Cipher (K)-Clear (N): ");
//Console.Write((cK[0] - pN[0] + 26) % alphabet.Length);
//for (int i = 1; i < pN.Length; i++)
//{
//    Console.Write("," + (cK[i] - pN[i] + 26) % alphabet.Length);
//}
//Console.WriteLine();

////Console.Write("Clear (N)-Cipher (K): ");
////Console.Write((pN[0] - cK[0] + 26) % alphabet.Length);
////for (int i = 1; i < pN.Length; i++)
////{
////    Console.Write("," + (pN[i] - cK[i] + 26) % alphabet.Length);
////}
////Console.WriteLine();

////Console.Write("Cipher (N)-Clear (K): ");
////Console.Write((cN[0] - pK[0] + 26) % alphabet.Length);
////for (int i = 1; i < pN.Length; i++)
////{
////    Console.Write("," + (cN[i] - pK[i] + 26) % alphabet.Length);
////}
////Console.WriteLine();

////Console.Write("Clear (K)-Cipher (N): ");
////Console.Write((pK[0] - cN[0] + 26) % alphabet.Length);
////for (int i = 1; i < pN.Length; i++)
////{
////    Console.Write("," + (pK[i] - cN[i] + 26) % alphabet.Length);
////}
////Console.WriteLine();






//Console.WriteLine(text);
//Console.WriteLine(less.ToString());
//Console.WriteLine(more.ToString());


//DirectoryInfo di = new DirectoryInfo(@"Texts\");

//foreach (FileInfo f in di.GetFiles("*.txt"))
//{
//    using (StreamReader file = new StreamReader(f.FullName))
//    {




//        file.Close();
//    }
//}

//List<string> ciphers = new List<string>();

//for (int skip = 0; skip < text.Length; skip++)
//{
//    StringBuilder desc = new StringBuilder();

//    //for (int i = 0; i < skip; i++)
//    {
//        int i = 0;
//        for (int j = skip; i < text.Length; j++)
//        {
//            desc.Append(text[j % text.Length]);
//            i++;
//        }
//    }
//    ciphers.Add(desc.ToString());
//}

//int count = ciphers.Count;
//for(int i=0; i< count; i++)
//{
//    char[] charArray = ciphers[i].ToCharArray();
//    Array.Reverse(charArray);
//    ciphers.Add(new string(charArray));
//}


//Dictionary<char, List<string>> dictionary = new Dictionary<char, List<string>>();
//using (StreamReader file = new StreamReader(@"texts\dictionary.txt"))
//{
//    while (!file.EndOfStream)
//    {
//        string line = file.ReadLine().ToUpper();
//        if (line.Length <= 11 && line.Length > 0)
//        {

//            if (dictionary.ContainsKey(line[0]))
//            {
//                dictionary[line[0]].Add(line);
//            }
//            else
//            {
//                dictionary.Add(line[0], new List<string>());
//                dictionary[line[0]].Add(line);
//            }
//        }
//    }
//}

//Columnar columnar = new Columnar(alphabet.ToCharArray());

//foreach (char index in dictionary.Keys)
//{
//    //char index = 'A';
//    foreach (string key in dictionary[index])
//    {
//        if (key.Length < 3) continue;

//        Dictionary<char, int> repeats = new Dictionary<char, int>();
//        foreach (char c in key)
//        {
//            if (!repeats.ContainsKey(c))
//            {
//                repeats.Add(c, 0);
//            }
//        }

//        StringBuilder sb = new StringBuilder();
//        foreach (char c in repeats.Keys)
//        {
//            sb.Append(c);
//        }

//        columnar.Key = sb.ToString();
//        Parallel.ForEach(ciphers, cipher =>
//        //foreach (string cipher in ciphers)
//        {


//            //if (key.Length != 7) continue;
//            //clear = Vigenere(cipher, alphabet, key);//
//            //clear = VigenereRunningKey(cipher, alphabet, key);//
//            //clear = VigenereAutoKey(cipher, alphabet, key);//
//            //clear = Vigenere(cipher, alphabet, columnar.Key);// XX
//            //clear = VigenereRunningKey(cipher, alphabet, columnar.Key);//
//            clear = VigenereAutoKey(cipher, alphabet, columnar.Key);//
//            //clear = Vigenere(cipher, kryptosAlphabet, key);//
//            //clear = VigenereRunningKey(cipher, kryptosAlphabet, key);//
//            //clear = VigenereAutoKey(cipher, kryptosAlphabet, key);//
//            //clear = Vigenere(cipher, kryptosAlphabet, columnar.Key);//
//            //clear = VigenereRunningKey(cipher, kryptosAlphabet, columnar.Key);//
//            //clear = VigenereAutoKey(cipher, kryptosAlphabet, columnar.Key);//

//            //columnar.Key

//            //clear = columnar.Decrypt(cipher);
//            //clear = columnar.Decrypt(clear);

//            //clear = Vigenere("OBKRUOXOGHULBSOLIFBBWFLRVQQ", kryptosAlphabet, key);//kryptosAlphabet
//            //if (clear[0] == 'X') continue;

//            double ic = IndexOfCoincidence(clear, kryptosAlphabet);

//            if (ic > 0.06)
//            {
//                Console.WriteLine("{0,10}:{1} - {2}\r\n{3}", key, clear, ic, cipher);
//                //for (int i = 0; i < 3 && i < clear.Length; i++)
//                //{
//                    //if (dictionary[clear[0]].Contains(clear.Substring(0, i)))
//                    //{
//                        //Console.WriteLine("{0,10}:{1,10} {2} - {3} - {4}", key, clear.Substring(0, key.Length), clear.Substring(key.Length, clear.Length - key.Length - 1), clear.Substring(0, i), ic);
//                        //Console.ReadKey();
//                    //}
//                //}
//            }
//        });
//    }
//}