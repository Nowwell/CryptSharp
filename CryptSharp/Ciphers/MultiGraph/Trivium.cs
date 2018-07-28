using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.MultiGraph
{
    public class Trivium : CipherBase<string>, ICipher
    {
        public Trivium(string[] Alphabet) : base(Alphabet)
        {

        }

        public string Decrypt(string[] cipherText)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
        }

        public void DecryptFile(string clearTextFilename, string cipherTextFilename, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string[] clearText)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string clearText, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
        }
    }
}
