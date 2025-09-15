using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.Classical
{
    public class Diana : CipherBase<char>, IClassicalCipher
    {
        public Diana(char[] Alphabet) : base(Alphabet)
        {
            Key = alphabet[0].ToString();
        }
        public Diana(char[] Alphabet, string key) : base(Alphabet)
        {
            Key = key;
        }

        public string Key { get; set; }

        public string Decrypt(string cipherText)
        {
            StringBuilder clear = new StringBuilder();
            int i = 0;
            foreach (char c in cipherText)
            {
                int charIndex = alphabet.IndexOf(c);
                int keyIndex = alphabet.IndexOf(Key[i % Key.Length]);
                int index = alphabet.Length - 1 - (charIndex + keyIndex) % alphabet.Length;
                clear.Append(alphabet[index]);

                i++;
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
            int i = 0;
            foreach (char c in clearText)
            {
                int charIndex = alphabet.IndexOf(c);
                int keyIndex = alphabet.IndexOf(Key[i % Key.Length]);
                int index = alphabet.Length - 1 - (charIndex + keyIndex) % alphabet.Length;
                cipher.Append(alphabet[index]);

                i++;
            }
            return cipher.ToString();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }
    }
}
