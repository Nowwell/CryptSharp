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
            AutoKey = false;
        }
        public Xor(char[] Alphabet, string key) : base(Alphabet)
        {
            Key = key;
            AutoKey = false;
        }
        public Xor(char[] Alphabet, string key, bool autokey) : base(Alphabet)
        {
            Key = key;
            AutoKey = autokey;
        }

        public string Key { get; set; }
        public bool AutoKey { get; set; }

        public string Decrypt(string cipherText)
        {
            StringBuilder output = new StringBuilder();
            if (AutoKey)
            {
                for (int i = 0; i < Key.Length; i++)
                {
                    output.Append(alphabet[(alphabet.IndexOf(Key[i % Key.Length]) - alphabet.IndexOf(cipherText[i % cipherText.Length]) + alphabet.Length) % alphabet.Length]);
                }
                for (int i = Key.Length; i < cipherText.Length; i++)
                {
                    output.Append(alphabet[(alphabet.IndexOf(output[i - Key.Length]) - alphabet.IndexOf(cipherText[i % cipherText.Length]) + alphabet.Length) % alphabet.Length]);
                }
                //for (int i = 0; i < Key.Length; i++)
                //{
                //    output[i] = alphabet[(alphabet.IndexOf(output[i]) - alphabet.IndexOf(Key[i % Key.Length]) + alphabet.Length) % alphabet.Length];
                //}
                //for (int i = Key.Length; i < cipherText.Length; i++)
                //{
                //    output[i] = alphabet[(alphabet.IndexOf(output[i]) - alphabet.IndexOf(output[i - Key.Length]) + alphabet.Length) % alphabet.Length];
                //}
            }
            else
            {
                for (int i = 0; i < cipherText.Length; i++)
                {
                    int charIndex = -1;
                    int textIndex = -1;

                    charIndex = alphabet.IndexOf(Key[i % Key.Length]);
                    textIndex = alphabet.IndexOf(cipherText[i % cipherText.Length]);

                    //for (int j = 0; j < alphabet.Length; j++)
                    //{
                    //    if (Key[i % Key.Length] == alphabet[j])
                    //    {
                    //        charIndex = j;
                    //    }
                    //    if (cipherText[i % cipherText.Length] == alphabet[j])
                    //    {
                    //        textIndex = j;
                    //    }

                    //    if (textIndex >= 0 && charIndex >= 0)
                    //    {
                    //        break;
                    //    }
                    //}
                    output.Append(alphabet[(textIndex - charIndex + alphabet.Length) % alphabet.Length]);
                }
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

            if (AutoKey)
            {
                for (int i = 0; i < Key.Length; i++)
                {
                    output.Append(alphabet[(alphabet.IndexOf(Key[i % Key.Length]) + alphabet.IndexOf(clearText[i % clearText.Length])) % alphabet.Length]);
                }
                for (int i = Key.Length; i < clearText.Length; i++)
                {
                    output.Append(alphabet[(alphabet.IndexOf(output[i - Key.Length]) + alphabet.IndexOf(clearText[i % clearText.Length])) % alphabet.Length]);
                }
            }
            else
            {
                for (int i = 0; i < clearText.Length; i++)
                {
                    int charIndex = alphabet.IndexOf(Key[i % Key.Length]);
                    int textIndex = alphabet.IndexOf(clearText[i % clearText.Length]);
                    output.Append(alphabet[(charIndex + textIndex + alphabet.Length) % alphabet.Length]);
                }
            }

            return output.ToString();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }
    }
}
