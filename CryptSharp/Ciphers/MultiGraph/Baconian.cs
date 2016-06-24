using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.MultiGraph
{
    public class Baconian : ICipher
    {
        protected string[] alphabet;
        protected Dictionary<string, int> charIndexPositions = new Dictionary<string, int>();
        public Baconian(string[] Alphabet)
        {
            alphabet = Alphabet;

            for (int i = 0; i < alphabet.Length; i++)
            {
                charIndexPositions.Add(alphabet[i], i);
            }

            SubstitutionTable = new Dictionary<string, string>();
        }

        public Dictionary<string, string> SubstitutionTable { get; set; }

        public string Encrypt(string[] clearText)
        {
            StringBuilder cipher = new StringBuilder();

            foreach (string s in clearText)
            {
                cipher.Append(SubstitutionTable[s]);
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
