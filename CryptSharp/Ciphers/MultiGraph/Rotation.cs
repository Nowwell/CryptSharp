using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.MultiGraph
{
    public class Rotation : CipherBase<string>, ICipher
    {
        protected Dictionary<string, int> charIndexPositions = new Dictionary<string, int>();

        public Rotation(string[] Alphabet) : base(Alphabet)
        {
            alphabet = Alphabet;
            Key = 0;

            for (int i = 0; i < alphabet.Length; i++)
            {
                charIndexPositions.Add(alphabet[i], i);
            }
        }
        public Rotation(string[] Alphabet, int rotation) : base(Alphabet)
        {
            alphabet = Alphabet;
            Key = rotation;

            for (int i = 0; i < alphabet.Length; i++)
            {
                charIndexPositions.Add(alphabet[i], i);
            }
        }

        public int Key { get; set; }

        public string[] Encrypt(string[] clearText)
        {
            int alphabetLength = alphabet.Length;

            List<string> cipher = new List<string>();
            foreach (string c in clearText)
            {
                cipher.Add(alphabet[(charIndexPositions[c] + Key) % alphabetLength]);
            }

            return cipher.ToArray();
        }
        public string[] Encrypt(string clearText, char wordSeparator, char charSeparator)
        {
            string[] plainText = clearText.Replace("\r", "").Replace("\n", "").Split(new char[] { wordSeparator, charSeparator }, StringSplitOptions.RemoveEmptyEntries);
            return Encrypt(plainText);
        }

        public string[] Decrypt(string[] cipherText)
        {
            int alphabetLength = alphabet.Length;

            List<string> cipher = new List<string>();
            foreach (string c in cipherText)
            {
                cipher.Add(alphabet[(charIndexPositions[c] - Key + alphabetLength) % alphabetLength]);
            }

            return cipher.ToArray();
        }
        public string[] Decrypt(string cipherText, char wordSeparator, char charSeparator)
        {
            string[] plainText = cipherText.Replace("\r", "").Replace("\n", "").Split(new char[] { wordSeparator, charSeparator }, StringSplitOptions.RemoveEmptyEntries);
            return Decrypt(plainText);
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
