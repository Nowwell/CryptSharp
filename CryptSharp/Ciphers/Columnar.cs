using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers
{
    public class Columnar : CipherBase, ICipher
    {
        public Columnar(char[] Alphabet) : base(Alphabet)
        {
            Key = alphabet[0].ToString();
        }
        public Columnar(char[] Alphabet, string key) : base(Alphabet)
        {
            Key = key;
        }

        public string Key { get; set; }

        public string Encrypt(string clearText)
        {
            StringBuilder output = new StringBuilder();

            KeyIndexer key = new KeyIndexer();
            for (int i = 0; i < Key.Length; i++)
            {
                for (int j = 0; j < alphabet.Length; j++)
                {
                    if (Key[i] == alphabet[j])
                    {
                        key.Add(i, j);
                    }
                }
            }
            key.KeyedBy = KeyedBy.Value;

            for (int i = 0; i < Key.Length; i++)
            {
                for (int j = key[i]; j < clearText.Length; j += Key.Length)
                {
                    output.Append(clearText[j]);
                }
            }

            return output.ToString();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(string cipherText)
        {
            int[] num = new int[Key.Length];
            int[] back = new int[Key.Length];
            string[] columns = new string[Key.Length];
            int minNum;
            StringBuilder output = new StringBuilder();

            minNum = cipherText.Length / Key.Length;

            KeyIndexer key = new KeyIndexer();
            for (int i = 0; i < Key.Length; i++)
            {
                for (int j = 0; j < alphabet.Length; j++)
                {
                    if (Key[i] == alphabet[j])
                    {
                        key.Add(i, j);
                    }
                }
            }
            key.ReindexValues();

            for (int i = 0; i < num.Length; i++)
            {
                num[i] = minNum;
                back[key[i]] = i;
            }
            
            int k = 0;
            for (int j = minNum * Key.Length; j < cipherText.Length; j++)
            {
                num[key[k++]]++;
            }

            for (int i = 0; i < Key.Length; i++)
            {
                columns[back[i]] = cipherText.Substring(0, num[i]);
                cipherText = cipherText.Substring(num[i], cipherText.Length - num[i]);
            }

            for (int i = 0; i < minNum + 1; i++)
            {
                for (int j = 0; j < columns.Length; j++)
                {
                    if (columns[j].Length > i)
                    {
                        output.Append(columns[j][i]);
                    }
                }
            }

            return output.ToString();


            //StringBuilder output = new StringBuilder(cipherText.ToUpper());
            //StringBuilder descramble = new StringBuilder(cipherText.ToUpper());


            //Dictionary<int, int> temp = new Dictionary<int, int>();
            //for (int i = 0; i < Key.Length; i++)
            //{
            //    for (int j = 0; j < alphabet.Length; j++)
            //    {
            //        if (Key[i] == alphabet[j])
            //        {
            //            temp.Add(j, i);
            //        }
            //    }
            //}

            //List<int> locations = new List<int>(temp.Keys);
            //locations.Sort();

            //Dictionary<int, int> indices = new Dictionary<int, int>();
            //for (int i = 0; i < locations.Count; i++)
            //{
            //    indices.Add(i, temp[locations[i]]);
            //}

            //int rows = output.Length / Key.Length;
            //if (output.Length % Key.Length != 0)
            //{
            //    rows++;
            //}
            //int k = 0;
            //for (int j = 0; j < rows; j++)
            //{
            //    for (int i = j; i < output.Length; i += rows)
            //    {
            //        descramble[k++] = cipherText[i];
            //    }
            //}
            //k = 0;
            //for (int j = 0; j < Key.Length; j++)
            //{
            //    for (int i = 0; i < output.Length; i += Key.Length)
            //    {
            //        if (i + indices[j] < output.Length && i + j < output.Length)
            //        {
            //            output[i + indices[j]] = descramble[j + i];
            //        }
            //    }
            //}

            //return output.ToString();
        }

        public void DecryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }

    }
}
