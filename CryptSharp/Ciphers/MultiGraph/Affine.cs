using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.MultiGraph
{
    public class Affine : ICipher
    {
        protected string[] alphabet;
        protected Dictionary<string, int> charIndexPositions = new Dictionary<string, int>();
        public Affine(string[] Alphabet)
        {
            alphabet = Alphabet;
            A = 3;
            B = 7;
            M = alphabet.Length;

            for (int i = 0; i < alphabet.Length; i++)
            {
                charIndexPositions.Add(alphabet[i], i);
            }
        }
        public Affine(string[] Alphabet, int a, int b, int m)
        {
            alphabet = Alphabet;
            A = a;
            B = b;
            M = m;

            for (int i = 0; i < alphabet.Length; i++)
            {
                charIndexPositions.Add(alphabet[i], i);
            }
        }

        public int A { get; set; }
        public int B { get; set; }
        public int M { get; set; }

        public string Encrypt(string[] clearText)
        {
            StringBuilder cipher = new StringBuilder();
            foreach (string c in clearText)
            {
                cipher.Append(alphabet[(A * charIndexPositions[c] + B) % M]);
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
            int Ainv = Utility.ModInverse(A, M);

            StringBuilder cipher = new StringBuilder();
            foreach (string c in cipherText)
            {
                int index = ((charIndexPositions[c] - B) * Ainv) % M;
                if( index < 0 ) index += M;
                cipher.Append(alphabet[index]);
            }

            return cipher.ToString();
        }
        public string Decrypt(string cipherText, char wordSeparator, char charSeparator)
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
