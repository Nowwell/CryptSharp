using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.MultiGraph
{
    public class Substitution : CipherBase<string>, ICipher
    {
        protected Dictionary<string, int> charIndexPositions = new Dictionary<string, int>();
        public Substitution(string[] Alphabet) : base(Alphabet)
        {
            alphabet = Alphabet;

            for (int i = 0; i < alphabet.Length; i++)
            {
                charIndexPositions.Add(alphabet[i], i);
            }
        }

        public string[] Key { get; set; }

        public string[] Encrypt(string[] clearText)
        {
            Dictionary<string, string> keyIndexPositions = new Dictionary<string, string>();
            for (int i = 0; i < Key.Length; i++)
            {
                keyIndexPositions.Add(alphabet[i], Key[i]);
            }

            List<string> cipher = new List<string>();
            foreach (string s in clearText)
            {
                cipher.Add(keyIndexPositions[s]);
            }
            return cipher.ToArray();
        }
        public string[] Encrypt(string clearText, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
        }

        public string[] Decrypt(string[] cipherText)
        {
            List<string> output = new List<string>();

            Dictionary<string, string> keyIndexPositions = new Dictionary<string, string>();
            for (int i = 0; i < Key.Length; i++)
            {
                keyIndexPositions.Add(Key[i], alphabet[i]);
            }

            for (int i = 0; i < cipherText.Length; i++)
            {
                output.Add(keyIndexPositions[cipherText[i]]);
            }
            return output.ToArray();
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
