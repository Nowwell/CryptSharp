using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers
{
    public class Trifid : CipherBase, ICipher
    {
        public Trifid(char[] Alphabet) : base(Alphabet)
        {
        }

        public Trifid(char[] Alphabet, char[] squares, int group) : base(Alphabet)
        {
            Squares = squares;
            Group = group;
        }

        public char[] Squares { get; set; }
        public int Group { get; set; }

        public string Decrypt(string cipherText)
        {
            throw new NotImplementedException();
        }

        public void DecryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string clearText)
        {
            StringBuilder cipher = new StringBuilder();

            int[] squIndices = new int[clearText.Length];
            int[] rowIndices = new int[clearText.Length];
            int[] colIndices = new int[clearText.Length];

            for (int i = 0; i < clearText.Length; i++)
            {
                int index = Squares.IndexOf(clearText[i]);
                squIndices[i] = index / 9;
                rowIndices[i] = (index - squIndices[i] * 9) / 3;
                colIndices[i] = (index - squIndices[i] * 9) % 3;
            }

            for (int i = 0; i < clearText.Length; i += Group)
            {
                int max = Group;
                if (i + Group > clearText.Length)
                {
                    max = clearText.Length - i;
                }

                for (int j = 0; j < 3 * max; j += 3)
                {
                    int squ = (j < max ? squIndices[i + j] : (j < 2 * max ? rowIndices[i + j - max] : colIndices[i + j - max]));
                    int row = (j + 1 < max ? squIndices[i + j + 1] : (j < 2 * max ? rowIndices[i + j - max + 1] : colIndices[i + j - max + 1]));
                    int col = (j + 2 < max ? squIndices[i + j + 2] : (j < 2 * max ? rowIndices[i + j - max + 2] : colIndices[i + j - max + 2]));

                    cipher.Append(Squares[squ * 9 + row * 3 + col]);
                }
            }

            return cipher.ToString();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }
    }
}
