using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.Classical
{
    public class Bifid : CipherBase<char>, IClassicalCipher
    {
        public Bifid(char[] Alphabet) : base(Alphabet)
        {
        }

        public Bifid(char[] Alphabet, char[] square, int group) : base(Alphabet)
        {
            Square = square;
            Group = group;
        }

        public char[] Square { get; set; }
        public int Group { get; set; }

        public string Decrypt(string cipherText)
        {
            StringBuilder clear = new StringBuilder();

            int[] rowIndices = new int[cipherText.Length];
            int[] colIndices = new int[cipherText.Length];

            int k = 0;
            int l = 0;
            for (int i = 0; i < cipherText.Length; i += Group)
            {
                int max = Group;
                if (i + Group > cipherText.Length)
                {
                    max = cipherText.Length - i;
                }

                int m = 0;
                for (int j = 0; j < max; j ++)
                {
                    int c = Square.IndexOf(cipherText[i + j]);
                    int row = c / 5;
                    int col = c % 5;

                    if (m < max)
                    {
                        rowIndices[k++] = row;
                    }
                    else
                    {
                        colIndices[l++] = row;
                    }
                    m++;
                    if (m < max)
                    {
                        rowIndices[k++] = col;
                    }
                    else
                    {
                        colIndices[l++] = col;
                    }
                    m++;
                }
            }

            for (k = 0; k < cipherText.Length; k++)
            {
                clear.Append(Square[rowIndices[k] * 5 + colIndices[k]]);
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

            int[] rowIndices = new int[clearText.Length];
            int[] colIndices = new int[clearText.Length];

            for (int k = 0; k < clearText.Length; k++)
            {
                int i = Square.IndexOf(clearText[k]);
                rowIndices[k] = i / 5;
                colIndices[k] = i % 5;
            }

            for (int i = 0; i < clearText.Length; i += Group)
            {
                int max = Group;
                if (i + Group > clearText.Length)
                {
                    max = clearText.Length - i;
                }

                for (int j = 0; j < 2 * max; j += 2)
                {
                    int row = (j < max ? rowIndices[i + j] : colIndices[i + j - max]);
                    int col = (j + 1 < max ? rowIndices[i + j + 1] : colIndices[i + j - max + 1]);

                    cipher.Append(Square[row * 5 + col]);
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
