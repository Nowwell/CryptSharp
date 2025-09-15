using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.Classical
{
    public class Vigenere : CipherBase<char>, IClassicalCipher
    {
        public Vigenere(char[] Alphabet) : base(Alphabet)
        {
            Key = alphabet[0].ToString();
        }
        public Vigenere(char[] Alphabet, string key, bool autoKey = false) : base(Alphabet)
        {
            Key = key;
            AutoKey = autoKey;
        }

        public string Key { get; set; }
        public bool AutoKey { get; set; }

        public string Encrypt(string clearText)
        {
            StringBuilder output = new StringBuilder(clearText.ToUpper());

            if (AutoKey)
            {
                for (int i = 0; i < Key.Length; i++)
                {
                    if (Key[i % Key.Length] == '_' || output[i] == '_')
                    {
                        output[i] = '_';
                    }
                    else
                    {
                        output[i] = alphabet[(alphabet.IndexOf(output[i]) + alphabet.IndexOf(Key[i % Key.Length])) % alphabet.Length];
                    }
                }
                for (int i = Key.Length; i < clearText.Length; i++)
                {
                    if (clearText[i - Key.Length] == '_' || output[i] == '_')
                    {
                        output[i] = '_';
                    }
                    else
                    {
                        output[i] = alphabet[(alphabet.IndexOf(output[i]) + alphabet.IndexOf(clearText[i - Key.Length])) % alphabet.Length];
                    }
                }
            }
            else
            {
                for (int i = 0; i < clearText.Length; i++)
                {
                    if (clearText[i] == '_' || Key[i % Key.Length] == '_')
                    {
                        output[i] = '_';
                    }
                    else
                    {
                        output[i] = alphabet[(alphabet.IndexOf(output[i]) + alphabet.IndexOf(Key[i % Key.Length])) % alphabet.Length];
                    }
                }
            }
                
            return output.ToString();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(string cipherText)
        {
            StringBuilder output = new StringBuilder(cipherText.ToUpper());
            if (AutoKey)
            {
                for (int i = 0; i < Key.Length; i++)
                {
                    if (Key[i % Key.Length] == '_' || output[i] == '_')
                    {
                        output[i] = '_';
                    }
                    else
                    {
                        output[i] = alphabet[(alphabet.IndexOf(output[i]) - alphabet.IndexOf(Key[i % Key.Length]) + alphabet.Length) % alphabet.Length];
                    }
                }
                for (int i = Key.Length; i < cipherText.Length; i++)
                {
                    if (cipherText[i - Key.Length] == '_' || output[i] == '_')
                    {
                        output[i] = '_';
                    }
                    else
                    {
                        output[i] = alphabet[(alphabet.IndexOf(output[i]) - alphabet.IndexOf(output[i - Key.Length]) + alphabet.Length) % alphabet.Length];
                    }
                }
            }
            else
            {
                for (int i = 0; i < cipherText.Length; i++)
                {
                    if (Key[i % Key.Length] == '_' || cipherText[i] == '_')
                    {
                        output[i] = '_';
                    }
                    else
                    {
                        int keyIndex = alphabet.IndexOf(Key[i % Key.Length]);
                        int index = (alphabet.IndexOf(output[i]) - keyIndex + alphabet.Length) % alphabet.Length;
                        output[i] = alphabet[index];
                    }
                }
            }
            return output.ToString();
        }

        public void DecryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }
    }
}
