using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers
{
    public class Rotation: CipherBase, ICipher
    {
        public Rotation(char[] Alphabet) : base(Alphabet)
        {
            Key = 0;
        }
        public Rotation(char[] Alphabet, int rotation) : base(Alphabet)
        {
            Key = rotation;
        }

        public int Key { get; set; }

        public string Encrypt(string clearText)
        {
            int alphabetLength = alphabet.Length;
            int key = Key % alphabetLength;

            StringBuilder cipher = new StringBuilder();
            foreach (char c in clearText)
            {
                cipher.Append(alphabet[(alphabet.IndexOf(c) + key) % alphabetLength]);
            }

            return cipher.ToString();
        }

        public string Decrypt(string cipherText)
        {
            int alphabetLength = alphabet.Length;
            int key = Key % alphabetLength;

            StringBuilder cipher = new StringBuilder();
            foreach (char c in cipherText)
            {
                cipher.Append(alphabet[(alphabet.IndexOf(c) - key + 2 * alphabetLength) % alphabetLength]);
            }

            return cipher.ToString();
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
