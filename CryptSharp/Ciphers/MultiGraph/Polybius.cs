using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.MultiGraph
{
    public class Polybius : ICipher
    {
        protected string[] alphabet;
        protected Dictionary<string, int> charIndexPositions = new Dictionary<string, int>();
        public Polybius(string[] Alphabet)
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

        public string Encrypt(string[] clearText)
        {
            StringBuilder cipher = new StringBuilder();
            foreach (string s in clearText)
            {
                for (int i = 0; i < Square.Length; i++)
                {
                    if (Square[i] == s)
                    {
                        int row = Square.Length / ColumnHeaders.Length;
                        int col = Square.Length % ColumnHeaders.Length;

                        cipher.Append(RowHeaders[row]);
                        cipher.Append(ColumnHeaders[col]);
                        break;
                    }
                }
            }
            return cipher.ToString();
        }
        public string Encrypt(string clearText, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(string[] cipherText)
        {
            throw new NotImplementedException();
        }
        public string Decrypt(string cipherText, char wordSeparator, char charSeparator)
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
