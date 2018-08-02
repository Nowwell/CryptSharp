using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.MultiGraph
{
    public class Affine : CipherBase<string>, IMultigraphCipher
    {
        //protected string[] alphabet;
        protected Dictionary<string, int> charIndexPositions = new Dictionary<string, int>();
        public Affine(string[] Alphabet) : base(Alphabet)
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

        public Affine(string[] Alphabet, int a, int b, int m) : base(Alphabet)
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

        public string[] Encrypt(string[] clearText)
        {
            List<string> cipher = new List<string>();
            foreach (string c in clearText)
            {
                cipher.Add(alphabet[(A * charIndexPositions[c] + B) % M]);
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
            int Ainv = Utility.ModInverse(A, M);

            List<string> cipher = new List<string>();
            foreach (string c in cipherText)
            {
                int index = ((charIndexPositions[c] - B) * Ainv) % M;
                if( index < 0 ) index += M;
                cipher.Add(alphabet[index]);
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
