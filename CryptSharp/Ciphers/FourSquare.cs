using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers
{
    public class FourSquare : CipherBase, ICipher
    {
        public FourSquare(char[] Alphabet) : base(Alphabet)
        {
            KeySquare1 = alphabet[0].ToString();
        }
        public FourSquare(char[] Alphabet, string keySquare1, string keySquare2) : base(Alphabet)
        {
            KeySquare1 = keySquare1;
            KeySquare2 = keySquare2;
        }

        public string KeySquare1 { get; set; }
        public string KeySquare2 { get; set; }

        public string Decrypt(string cipherText)
        {
            StringBuilder clear = new StringBuilder();

            for (int i = 0; i < cipherText.Length; i += 2)
            {
                int c1 = KeySquare1.IndexOf(cipherText[i]) % 5;
                int r1 = KeySquare1.IndexOf(cipherText[i]) / 5;
                int c2 = KeySquare2.IndexOf(cipherText[i + 1]) % 5;
                int r2 = KeySquare2.IndexOf(cipherText[i + 1]) / 5;
                clear.Append(alphabet[5 * r1 + c2]);
                clear.Append(alphabet[5 * r2 + c1]);
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

            if ((clearText.Length & 0x01) == 1)
            {
                clearText += alphabet[alphabet.Length - 1];
            }

            for (int i = 0; i < clearText.Length; i += 2)
            {
                int c1 = alphabet.indexOf(clearText[i]) % 5;
                int r1 = alphabet.indexOf(clearText[i]) / 5;
                int c2 = alphabet.indexOf(clearText[i + 1]) % 5;
                int r2 = alphabet.indexOf(clearText[i + 1]) / 5;
                cipher.Append(KeySquare1[5 * r1 + c2]);
                cipher.Append(KeySquare2[5 * r2 + c1]);
            }

            return cipher.ToString();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }
    }
}
