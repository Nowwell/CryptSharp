using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.Classical
{
    public class Polybius : CipherBase<char>, IClassicalCipher
    {
        public Polybius(char[] Alphabet) : base(Alphabet)
        {
        }

        public char[] Square { get; set; }
        public char[] ColumnHeaders { get; set; }
        public char[] RowHeaders { get; set; }

        public string Encrypt(string clearText)
        {
            StringBuilder cipher = new StringBuilder();
            foreach (char s in clearText)
            {
                bool isFound = false;
                for (int i = 0; i < Square.Length; i++)
                {
                    if (Square[i] == s)
                    {
                        int row = i / ColumnHeaders.Length;
                        int col = i % ColumnHeaders.Length;

                        cipher.Append(RowHeaders[row]);
                        cipher.Append(ColumnHeaders[col]);
                        isFound = true;
                        break;
                    }
                }
                if (!isFound) throw new Exception(string.Format("character {0} not in square.", s));
            }
            return cipher.ToString();
        }

        public string Decrypt(string cipherText)
        {
            StringBuilder clear = new StringBuilder();

            for (int i = 0; i < cipherText.Length; i += 2)
            {
                char row = cipherText[i];
                char column = cipherText[i + 1];

                int r = 0;
                int c = 0;
                for (int j = 0; j < RowHeaders.Length; j++)
                {
                    if (row == RowHeaders[j])
                    {
                        r = j;
                        break;
                    }
                }
                for (int j = 0; j < ColumnHeaders.Length; j++)
                {
                    if (column == ColumnHeaders[j])
                    {
                        c = j;
                        break;
                    }
                }

                clear.Append(Square[ColumnHeaders.Length * r + c]);
            }

            return clear.ToString();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }
        public void DecryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }
    }
}
