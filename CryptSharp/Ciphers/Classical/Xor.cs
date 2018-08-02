using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.Classical
{
    public class Xor : CipherBase<char>, IClassicalCipher
    {
        public Xor(char[] Alphabet) : base(Alphabet)
        {
            Key = alphabet[0].ToString();
        }
        public Xor(char[] Alphabet, string key) : base(Alphabet)
        {
            Key = key;
        }

        public string Key { get; set; }

        public string Decrypt(string cipherText)
        {
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < cipherText.Length; i++)
            {
                int charIndex = -1;
                int textIndex = -1;
                for (int j = 0; j < alphabet.Length; j++)
                {
                    if (Key[i % Key.Length] == alphabet[j])
                    {
                        charIndex = j;
                    }
                    if (cipherText[i % cipherText.Length] == alphabet[j])
                    {
                        textIndex = j;
                    }

                    if (textIndex >= 0 && charIndex >= 0)
                    {
                        break;
                    }
                }
                output.Append(alphabet[(textIndex - charIndex + alphabet.Length) % alphabet.Length]);
            }

            return output.ToString();
        }

        public void DecryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string clearText)
        {
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < clearText.Length; i++)
            {
                int charIndex = -1;
                int textIndex = -1;
                for (int j = 0; j < alphabet.Length; j++)
                {
                    if (Key[i % Key.Length] == alphabet[j])
                    {
                        charIndex = j;
                    }
                    if (clearText[i % clearText.Length] == alphabet[j])
                    {
                        textIndex = j;
                    }

                    if (textIndex >= 0 && charIndex >= 0)
                    {
                        break;
                    }
                }
                output.Append(alphabet[(charIndex + textIndex) % alphabet.Length]);
            }

            return output.ToString();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }
    }
}
