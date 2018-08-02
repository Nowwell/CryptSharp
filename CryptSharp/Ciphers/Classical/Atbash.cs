using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.Classical
{
    public class Atbash : CipherBase<char>, IClassicalCipher
    {
        public Atbash(char[] Alphabet) : base(Alphabet)
        {
        }

        public string Encrypt(string clearText)
        {
            int alphabetLength = alphabet.Length - 1;

            StringBuilder cipher = new StringBuilder();
            foreach (char c in clearText)
            {
                cipher.Append(alphabet[alphabetLength - alphabet.IndexOf(c)]);
            }

            return cipher.ToString();
        }

        public string Decrypt(string cipherText)
        {
            int alphabetLength = alphabet.Length - 1;

            StringBuilder cipher = new StringBuilder();
            foreach (char c in cipherText)
            {
                cipher.Append(alphabet[alphabetLength - alphabet.IndexOf(c)]);
            }

            return cipher.ToString();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename)
        {

        }
        public void DecryptFile(string clearTextFilename, string cipherTextFilename)
        {

        }
    }
}
