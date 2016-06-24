using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers
{
    public class ADFGVX : CipherBase, ICipher
    {
        public ADFGVX(char[] Alphabet) : base(Alphabet)
        {
            Key = alphabet[0].ToString();
        }
        public ADFGVX(char[] Alphabet, string key, string square) : base(Alphabet)
        {
            Key = key;
            Square = square;
        }

        public string Key { get; set; }
        public string Square { get; set; }

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
            throw new NotImplementedException();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }
    }
}
