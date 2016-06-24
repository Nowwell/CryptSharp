using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers
{
    public class Skip : CipherBase, ICipher
    {
        public Skip(char[] Alphabet) : base(Alphabet)
        {
            Start = 0;
            Distance = 1;
        }
        public Skip(char[] Alphabet, int start, int distance) : base(Alphabet)
        {
            Start = start;
            Distance = distance;
        }

        public int Start { get; set; }
        public int Distance { get; set; }

        public string Decrypt(string cipherText)
        {
            StringBuilder sb = new StringBuilder();

            int i = Start;
            int k = 0;
            while (k++ < cipherText.Length)
            {
                sb.Append(cipherText[i]);

                i += Distance;
                i = i % cipherText.Length;
            }

            return sb.ToString();
        }

        public void DecryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string clearText)
        {
            StringBuilder cipher = new StringBuilder(clearText.Length);

            int k = Start;
            for (int i = 0; i < clearText.Length; i++)
            {
                cipher[k] = clearText[i];

                k += Distance;
                k = k % clearText.Length;
            }

            return cipher.ToString();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }
    }
}
