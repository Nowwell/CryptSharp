using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.MultiGraph
{
    public class Polybius : CipherBase<string>, IMultigraphCipher
    {
        protected Dictionary<string, int> charIndexPositions = new Dictionary<string, int>();
        public Polybius(string[] Alphabet) : base(Alphabet)
        {
            alphabet = Alphabet;

            for (int i = 0; i < alphabet.Length; i++)
            {
                charIndexPositions.Add(alphabet[i], i);
            }
        }

        public string[] Square { get; set; }
        public string[] ColumnHeaders { get; set; }
        public string[] RowHeaders { get; set; }

        public string[] Encrypt(string[] clearText)
        {
            List<string> cipher = new List<string>();
            foreach (string s in clearText)
            {
                for (int i = 0; i < Square.Length; i++)
                {
                    if (Square[i] == s)
                    {
                        int row = i / ColumnHeaders.Length;
                        int col = i % ColumnHeaders.Length;

                        cipher.Add(RowHeaders[row]);
                        cipher.Add(ColumnHeaders[col]);
                        break;
                    }
                }
            }
            return cipher.ToArray();
        }
        public string[] Encrypt(string clearText, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
        }

        public string[] Decrypt(string[] cipherText)
        {
            List<string> clear = new List<string>();

            for (int i = 0; i < cipherText.Length; i += 2)
            {
                string row = cipherText[i];
                string column = cipherText[i + 1];

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

                clear.Add(Square[ColumnHeaders.Length * r + c]);
            }

            return clear.ToArray();
        }
        public string[] Decrypt(string cipherText, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
        }
        public void DecryptFile(string clearTextFilename, string cipherTextFilename, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
        }
    }
}
