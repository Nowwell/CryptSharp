using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers
{
    public class CipherBase
    {
        public CipherBase()
        {
        }

        public CipherBase(char[] Alphabet)
        {
            alphabet = Alphabet;
        }

        protected char[] alphabet;

        public bool IsInAlphabet(char c)
        {
            return alphabet.Contains(c);
        }

        public string GenerateRandomString(int length = 0)
        {
            StringBuilder toEncrypt = new StringBuilder();

            string generated = Utility.Random(1024).ToUpper();
            int i = length;

            foreach (char c in generated)
            {
                if (IsInAlphabet(c))
                {
                    toEncrypt.Append(c);
                    i--;
                }
                if (length != 0 && i == 0)
                {
                    break;
                }
            }

            return toEncrypt.ToString();
        }

        //Untested
        public string ScrambledAlphabet()
        {
            string copy = new string(alphabet);

            StringBuilder sb = new StringBuilder();
            byte[] tokenData = new byte[2];
            using (System.Security.Cryptography.RandomNumberGenerator rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                for (int i = 0; i < alphabet.Length; i++)
                {
                    rng.GetBytes(tokenData);

                    int value = (int)(BitConverter.ToUInt16(tokenData, 0) % copy.Length);

                    sb.Append(copy[value]);

                    copy = copy.Replace(copy[value].ToString(), "");
                }
            }
            return sb.ToString();
        }
    }
}
