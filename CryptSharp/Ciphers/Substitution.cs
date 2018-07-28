using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers
{
    public class Substitution : CipherBase<char>, ICipher
    {
        public Substitution(char[] Alphabet) : base(Alphabet)
        {
        }

        public char[] Key { get; set; }

        public string Encrypt(string clearText)
        {
            Dictionary<char, char> keyIndexPositions = new Dictionary<char, char>();
            for (int i = 0; i < Key.Length; i++)
            {
                keyIndexPositions.Add(alphabet[i], Key[i]);
            }

            StringBuilder cipher = new StringBuilder();
            foreach (char s in clearText)
            {
                cipher.Append(keyIndexPositions[s]);
            }
            return cipher.ToString();
        }

        public string Decrypt(string cipherText)
        {
            StringBuilder output = new StringBuilder();

            Dictionary<char, char> keyIndexPositions = new Dictionary<char, char>();
            for (int i = 0; i < Key.Length; i++)
            {
                keyIndexPositions.Add(Key[i], alphabet[i]);
            }

            for (int i = 0; i < cipherText.Length; i++)
            {
                output.Append(keyIndexPositions[cipherText[i]]);
            }
            return output.ToString();
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
