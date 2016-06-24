using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.MultiGraph
{
    public class Atbash : ICipher
    {
        protected string[] alphabet;
        protected Dictionary<string, int> charIndexPositions = new Dictionary<string, int>();

        public Atbash(string[] Alphabet)
        {
            alphabet = Alphabet;

            for (int i = 0; i < alphabet.Length; i++)
            {
                charIndexPositions.Add(alphabet[i], i);
            }
        }

        public string Encrypt(string[] clearText)
        {
            int alphabetLength = alphabet.Length;

            StringBuilder cipher = new StringBuilder();
            foreach (string c in clearText)
            {
                cipher.Append(alphabet[alphabetLength - charIndexPositions[c]]);
            }

            return cipher.ToString();
        }
        public string Encrypt(string clearText, char wordSeparator, char charSeparator)
        {
            string[] plainText = clearText.Replace("\r", "").Replace("\n", "").Split(new char[] { wordSeparator, charSeparator }, StringSplitOptions.RemoveEmptyEntries);
            return Encrypt(plainText);
        }

        public string Decrypt(string[] cipherText)
        {
            int alphabetLength = alphabet.Length;

            StringBuilder cipher = new StringBuilder();
            foreach (string c in cipherText)
            {
                cipher.Append(alphabet[alphabetLength - charIndexPositions[c]]);
            }

            return cipher.ToString();
        }
        public string Decrypt(string cipherText, char wordSeparator, char charSeparator)
        {
            string[] clearText = cipherText.Replace("\r", "").Replace("\n", "").Split(new char[] { wordSeparator, charSeparator }, StringSplitOptions.RemoveEmptyEntries);
            return Decrypt(clearText);
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename, char wordSeparator, char charSeparator)
        {

        }
        public void DecryptFile(string clearTextFilename, string cipherTextFilename, char wordSeparator, char charSeparator)
        {

        }
    }
}
