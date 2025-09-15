using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.Classical
{
    public class Playfair : CipherBase<char>, IClassicalCipher
    {
        public Playfair(char[] Alphabet) : base(Alphabet)
        {
        }

        public string Decrypt(string cipherText)
        {
            StringBuilder clear = new StringBuilder();

            char c = alphabet[0];
            char d = alphabet[0];

            for (int i = 0; i < cipherText.Length - 1; i += 2)
            {
                char a = cipherText[i];
                char b = cipherText[i + 1];
                int row1 = alphabet.IndexOf(a) / 5;
                int col1 = alphabet.IndexOf(a) % 5;
                int row2 = alphabet.IndexOf(b) / 5;
                int col2 = alphabet.IndexOf(b) % 5;
                if (row1 == row2)
                {
                    if (col1 == 0) c = alphabet[row1 * 5 + 4];
                    else c = alphabet[row1 * 5 + col1 - 1];
                    if (col2 == 0) d = alphabet[row2 * 5 + 4];
                    else d = alphabet[row2 * 5 + col2 - 1];
                }
                else if (col1 == col2)
                {
                    if (row1 == 0) c = alphabet[20 + col1];
                    else c = alphabet[(row1 - 1) * 5 + col1];
                    if (row2 == 0) d = alphabet[20 + col2];
                    else d = alphabet[(row2 - 1) * 5 + col2];
                }
                else
                {
                    c = alphabet[row1 * 5 + col2];
                    d = alphabet[row2 * 5 + col1];
                }

                clear.Append("" + c + d);
            }

            if (clear[clear.Length - 1] == 'X') clear.Length--;

            return clear.ToString();
        }

        public void DecryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string clearText)
        {
            StringBuilder cipher = new StringBuilder();

            char c = alphabet[0];
            char d = alphabet[0];

            if ((clearText.Length & 0x01) == 1) clearText += 'X';
            for (int i = 0; i < clearText.Length; i += 2)
            {
                char a = clearText[i];
                char b = clearText[i + 1];
                if (a == b) b = 'X';
                int row1 = alphabet.IndexOf(a) / 5;
                int col1 = alphabet.IndexOf(a) % 5;
                int row2 = alphabet.IndexOf(b) / 5;
                int col2 = alphabet.IndexOf(b) % 5;
                if (row1 == row2)
                {
                    if (col1 == 4) c = alphabet[row1 * 5];
                    else c = alphabet[row1 * 5 + col1 + 1];
                    if (col2 == 4) d = alphabet[row2 * 5];
                    else d = alphabet[row2 * 5 + col2 + 1];
                }
                else if (col1 == col2)
                {
                    if (row1 == 4) c = alphabet[col1];
                    else c = alphabet[(row1 + 1) * 5 + col1];
                    if (row2 == 4) d = alphabet[col2];
                    else d = alphabet[(row2 + 1) * 5 + col2];
                }
                else
                {
                    c = alphabet[row1 * 5 + col2];
                    d = alphabet[row2 * 5 + col1];
                }

                cipher.Append("" + c + d);
            }

            return cipher.ToString();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }
    }
}
