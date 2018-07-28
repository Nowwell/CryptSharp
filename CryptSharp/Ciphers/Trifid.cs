using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers
{
    public class Trifid : CipherBase<char>, ICipher
    {
        public Trifid(char[] Alphabet) : base(Alphabet)
        {
        }

        public Trifid(char[] Alphabet, char[] squares, int group) : base(Alphabet)
        {
            Squares = squares;
            Group = group;
        }

        public char[] Squares { get; set; }
        public int Group { get; set; }

        public string Decrypt(string cipherText)
        {
            StringBuilder clear = new StringBuilder();

            int[] squIndices = new int[cipherText.Length];
            int[] rowIndices = new int[cipherText.Length];
            int[] colIndices = new int[cipherText.Length];

            int k = 0;
            int l = 0;
            int n = 0;
            for (int i = 0; i < cipherText.Length; i += Group)
            {
                int max = Group;
                if (i + Group > cipherText.Length)
                {
                    max = cipherText.Length - i;
                }

                int m = 0;
                for (int j = 0; j < max; j++)
                {
                    int c = Squares.IndexOf(cipherText[i + j]);
                    int squ = c / 9;
                    int row = (c - squ * 9) / 3;
                    int col = (c - squ * 9) % 3;

                    if (m < max)
                    {
                        squIndices[n++] = squ;
                    }
                    else if( m < 2*max)
                    {
                        rowIndices[k++] = squ;
                    }
                    else
                    {
                        colIndices[l++] = squ;
                    }
                    m++;
                    if (m < max)
                    {
                        squIndices[n++] = row;
                    }
                    else if (m < 2 * max)
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
                        squIndices[n++] = col;
                    }
                    else if (m < 2 * max)
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
                clear.Append(Squares[squIndices[k] * 9 + rowIndices[k] * 3 + colIndices[k]]);
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

            int[] squIndices = new int[clearText.Length];
            int[] rowIndices = new int[clearText.Length];
            int[] colIndices = new int[clearText.Length];

            for (int i = 0; i < clearText.Length; i++)
            {
                int index = Squares.IndexOf(clearText[i]);
                squIndices[i] = index / 9;
                rowIndices[i] = (index - squIndices[i] * 9) / 3;
                colIndices[i] = (index - squIndices[i] * 9) % 3;
            }

            for (int i = 0; i < clearText.Length; i += Group)
            {
                int max = Group;
                if (i + Group > clearText.Length)
                {
                    max = clearText.Length - i;
                }

                for (int j = 0; j < 3 * max; j += 3)
                {
                    int squ = (j < max ? squIndices[i + j] : (j < 2 * max ? rowIndices[i + j - max] : colIndices[i + j - 2 * max]));
                    int row = (j + 1 < max ? squIndices[i + j + 1] : (j + 1 < 2 * max ? rowIndices[i + j - max + 1] : colIndices[i + j - 2 * max + 1]));
                    int col = (j + 2 < max ? squIndices[i + j + 2] : (j + 2 < 2 * max ? rowIndices[i + j - max + 2] : colIndices[i + j - 2 * max + 2]));

                    cipher.Append(Squares[squ * 9 + row * 3 + col]);
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
