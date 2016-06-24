using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers
{
    public class Affine : CipherBase, ICipher
    {
        public Affine(char[] Alphabet) : base(Alphabet)
        {
            A = 3;
            B = 7;
            M = alphabet.Length;
        }
        public Affine(char[] Alphabet, int a, int b, int m) : base(Alphabet)
        {
            A = a;
            B = b;
            M = m;
        }

        public int A { get; set; }
        public int B { get; set; }
        public int M { get; set; }

        public string Encrypt(string clearText)
        {
            StringBuilder cipher = new StringBuilder();
            foreach (char c in clearText)
            {
                cipher.Append(alphabet[(A * alphabet.indexOf(c) + B) % M]);
            }

            return cipher.ToString();
        }

        public string Decrypt(string cipherText)
        {
            int Ainv = Utility.ModInverse(A, M);

            StringBuilder cipher = new StringBuilder();
            foreach (char c in cipherText)
            {
                int index = ((alphabet.indexOf(c) - B) * Ainv) % M;
                if( index < 0 ) index += M;
                cipher.Append(alphabet[index]);
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
