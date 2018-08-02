using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.MultiGraph
{
    public class RailFence : CipherBase<string>, IMultigraphCipher
    {
        protected Dictionary<string, int> charIndexPositions = new Dictionary<string, int>();
        public RailFence(string[] Alphabet) : base(Alphabet)
        {
            alphabet = Alphabet;
            Key = 3;

            for (int i = 0; i < alphabet.Length; i++)
            {
                charIndexPositions.Add(alphabet[i], i);
            }
        }
        public RailFence(string[] Alphabet, int key) : base(Alphabet)
        {
            alphabet = Alphabet;
            Key = key;

            for (int i = 0; i < alphabet.Length; i++)
            {
                charIndexPositions.Add(alphabet[i], i);
            }
        }

        public int Key { get; set; }

        public string[] Encrypt(string[] clearText)
        {
            List<string> cipher = new List<string>();

            int line;
            for (line = 0; line < Key - 1; line++)
            {
                int skip = 2 * (Key - line - 1);
                for (int i = line, j = 0; i < clearText.Length; j++)
                {
                    cipher.Add(clearText[i]);

                    if (line == 0 || (j & 1) == 0)
                    {
                        i += skip;
                    }
                    else
                    {
                        i += 2 * (Key - 1) - skip;
                    }

                }

            }
            for (int i = line; i < clearText.Length; i += 2 * (Key - 1))
            {
                cipher.Add(clearText[i]);
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
            string[] clear = new string[cipherText.Length];

            int k = 0;
            int line = 0;
            for (line = 0; line < Key - 1; line++)
            {
                int skip = 2 * (Key - line - 1);
                for (int i = line, j = 0; i < cipherText.Length; j++)
                {
                    clear[i] = cipherText[k++];
                    
                    if (line == 0 || (j & 1) == 0)
                    {
                        i += skip;
                    }
                    else
                    {
                        i += 2 * (Key - 1) - skip;
                    }
                }
            }

            for (int i = line; i < cipherText.Length; i += 2 * (Key - 1))
            {
                clear[i] = cipherText[k++];
            }

            return clear;
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
