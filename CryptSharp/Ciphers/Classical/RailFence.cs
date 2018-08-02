using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.Classical
{
    public class RailFence : CipherBase<char>, IClassicalCipher
    {
        public RailFence(char[] Alphabet) : base(Alphabet)
        {
            Key = 3;
        }
        public RailFence(char[] Alphabet, int key) : base(Alphabet)
        {
            Key = key;
        }

        public int Key { get; set; }

        public string Encrypt(string clearText)
        {
            StringBuilder cipher = new StringBuilder();

            int line;
            for (line = 0; line < Key - 1; line++)
            {
                int skip = 2 * (Key - line - 1);
                for (int i = line, j = 0; i < clearText.Length; j++)
                {
                    cipher.Append(clearText[i]);

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
                cipher.Append(clearText[i]);
            }

            return cipher.ToString();
        }

        public string Decrypt(string cipherText)
        {
            char[] clear = new char[cipherText.Length];

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

            return string.Join("", clear);
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
