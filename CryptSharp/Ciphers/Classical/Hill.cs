using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.Classical
{
    public class Hill : CipherBase<char>, IClassicalCipher
    {
        public Hill(char[] Alphabet) : base(Alphabet)
        {
            Key = new Matrix(3, 3);
            Key.SetData(new double[,] { { 2, 4, 5 }, { 9, 2, 1 }, { 3, 17, 7 } });
        }
        public Hill(char[] Alphabet, double[,] key) : base(Alphabet)
        {
            Key = new Matrix(key.GetLength(0), key.GetLength(1));
            Key.SetData(key);
        }

        public Matrix Key { get; set; }

        public string Decrypt(string cipherText)
        {
            //ensure cipherText.Length is a multiple of the Key dimensions (Key is an NxN matrix)
            //  this won't ruin the message, it will put up to N-1 extra characters at the end
            while (cipherText.Length % Key.Rows != 0)
            {
                cipherText += alphabet[alphabet.Length - 1];
            }

            Matrix inv = new Matrix(Key);
            inv.ModInvert(alphabet.Length);

            StringBuilder clear = new StringBuilder();

            for (int i = 0; i < cipherText.Length; i += inv.Columns)
            {
                for (int j = 0; j < inv.Rows; j++)
                {
                    double outchar = 0;
                    for (int k = 0; k < inv.Columns; k++)
                    {
                        outchar += alphabet.IndexOf(cipherText[i + k]) * inv[j, k];
                    }
                    clear.Append(alphabet[(((int)outchar % alphabet.Length) + alphabet.Length) % alphabet.Length]);
                }
            }

            return clear.ToString();

        }
        public void DecryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string clearText)
        {
            StringBuilder cipher = new StringBuilder();

            //ensure clearText.Length is a multiple of the Key dimensions (Key is an NxN matrix)
            //  this won't ruin the message, it will put up to N-1 extra characters at the end
            while (clearText.Length % Key.Rows != 0)
            {
                clearText += alphabet[alphabet.Length - 1];
            }

            for (int i = 0; i < clearText.Length; i += Key.Columns)
            {
                //do Key * Vector % alphabet.Length
                for(int j = 0; j<Key.Rows; j++)
                {
                    double sum = 0.0;
                    for (int k = 0; k < Key.Columns; k++)
                    {
                        //sum += Vector[k] * Key[j, k]
                        sum += alphabet.IndexOf(clearText[i + k]) * Key[j, k];
                    }
                    cipher.Append(alphabet[((int)sum % alphabet.Length + alphabet.Length) % alphabet.Length]);
                }
            }

            return cipher.ToString();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }
    }
}
