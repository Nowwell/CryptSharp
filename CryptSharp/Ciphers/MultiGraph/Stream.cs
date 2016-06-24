using CryptSharp.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.MultiGraph
{
    public class Stream : ICipher
    {
        protected string[] alphabet;
        protected Dictionary<string, int> charIndexPositions = new Dictionary<string, int>();

        public Stream(string[] Alphabet)
        {
            alphabet = Alphabet;

            for (int i = 0; i < alphabet.Length; i++)
            {
                charIndexPositions.Add(alphabet[i], i);
            }
        }

        public byte[] Key { get; set; }
        public LinearFeedbackShiftRegister Registers {get;set;}

        public string Encrypt(string[] clearText)
        {
            StringBuilder cipher = new StringBuilder();
            int bit = 1;
            int k = 0;
            foreach (string s in clearText)
            {
                byte[] clear = UnicodeEncoding.Unicode.GetBytes(s);

                for (int i = 0; i < clear.Length; i++)
                {
                    if (Registers != null)
                    {
                        bit = Registers.Shift();

                        clear[i] = (byte)(clear[i] ^ Key[((k++) % Key.Length)]);
                    }
                    else
                    {
                        clear[i] = (byte)(clear[i] ^ Key[((k++) % Key.Length)]);
                    }
                }

                cipher.Append(UnicodeEncoding.Unicode.GetString(clear));

            }

            return cipher.ToString();
        }
        public string Encrypt(string clearText, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(string[] cipherText)
        {
            StringBuilder clearText = new StringBuilder();
            int bit = 1;
            int k = 0;
            foreach (string s in cipherText)
            {
                byte[] clear = UnicodeEncoding.Unicode.GetBytes(s);

                for (int i = 0; i < clear.Length; i++)
                {
                    if (Registers != null)
                    {
                        bit = Registers.Shift();

                        clear[i] = (byte)(clear[i] ^ Key[((k++) % Key.Length)]);
                    }
                    else
                    {
                        clear[i] = (byte)(clear[i] ^ Key[((k++) % Key.Length)]);
                    }
                }

                clearText.Append(UnicodeEncoding.Unicode.GetString(clear));

            }

            return clearText.ToString();
        }
        public string Decrypt(string cipherText, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
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
