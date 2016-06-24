using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers
{
    public class Amsco : CipherBase, ICipher
    {
        public Amsco(char[] Alphabet) : base(Alphabet)
        {
        }
        public Amsco(char[] Alphabet, int[] key) : base(Alphabet)
        {
            Key = key;
        }

        public int[] Key { get; set; }

        public string Decrypt(string cipherText)
        {
            KeyIndexer oto = new KeyIndexer();
            for (int i = 0; i < Key.Length; i++)
            {
                oto.Add(i, Key[i]);
            }
            oto.KeyedBy = KeyedBy.Value;

            int numPositions = 0;
            if ((cipherText.Length & 0x01) > 0)
            {
                //odd
                numPositions = (2 * cipherText.Length) / 3;
            }
            else
            {
                //even
                numPositions = (2 * cipherText.Length) / 3 + 1;
            }

            int lastChar = cipherText.Length % 3;//r=0, 2 means no special consideration when decrypting, mod = 1 means single should be double char
            int numRows = numPositions / Key.Length;
            int remainder = numPositions % Key.Length;

            string[,] pieces = new string[numRows + 1, Key.Length];

            int k = 0;
            for (int i = 0; i < oto.Count; i++)
            {
                int x = oto[i];
                for (int j = 0; j < numRows + (x < remainder ? 1 : 0); j++)
                {
                    if ((x & 0x01) == 0)
                    {
                        if ((j & 0x01) == 0)
                        {
                            pieces[j, x] = cipherText[k++].ToString();
                            if (k < cipherText.Length && !(x == remainder - 1 && lastChar == 1 && j == numRows - 1 + (x < remainder ? 1 : 0)))
                                pieces[j, x] += cipherText[k++].ToString();
                        }
                        else
                        {
                            pieces[j, x] = cipherText[k++].ToString();
                        }
                    }
                    else
                    {
                        if ((j & 0x01) == 1)
                        {
                            pieces[j, x] = cipherText[k++].ToString();
                            if (k < cipherText.Length && !(x == remainder - 1 && lastChar == 1 && j == numRows - 1 + (x < remainder ? 1 : 0)))
                                pieces[j, x] += cipherText[k++].ToString();
                        }
                        else
                        {
                            pieces[j, x] = cipherText[k++].ToString();
                        }
                    }
                }
            }

            StringBuilder output = new StringBuilder();
            for (int i = 0; i < numRows + (remainder>0?1:0); i++)
            {
                for (int j = 0; j < Key.Length; j++)
                {
                    output.Append(pieces[i, j] ?? "");
                }
            }

            return output.ToString();
        }

        public void DecryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string clearText)
        {
            string[] pieces = new string[clearText.Length * 2 / 3 + 1];
            int k = 0;
            for (int i=0; i<clearText.Length; i++)
            {
                if ((k & 0x01) == 0)
                {
                    if (i + 1 >= clearText.Length)
                    {
                        pieces[k++] = clearText[i].ToString();
                    }
                    else
                    {
                        pieces[k++] = clearText[i] + "" + clearText[i + 1];
                        i++;
                    }
                }
                else
                {
                    pieces[k++] = clearText[i].ToString();
                }
            }

            StringBuilder output = new StringBuilder();

            StringBuilder[] scramble = new StringBuilder[Key.Length];
            for (int i = 0; i < scramble.Length; i++)
            {
                scramble[i] = new StringBuilder();
            }
            for (int i = 0; i < pieces.Length; i++)
            {
                if (pieces[i] != null)
                {
                    scramble[Key[i % Key.Length]].Append(pieces[i]);
                }
            }

            for (int i = 0; i < scramble.Length; i++)
            {
                output.Append(scramble[i]);
            }

            return output.ToString();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }
    }
}
