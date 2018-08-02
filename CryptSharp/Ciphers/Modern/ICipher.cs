using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.Modern
{
    public interface ICipher
    {
        byte[] Encrypt(byte[] clearText);
        byte[] Encrypt(string clearText, char wordSeparator, char charSeparator);

        byte[] Decrypt(byte[] cipherText);
        byte[] Decrypt(string cipherText, char wordSeparator, char charSeparator);

        void EncryptFile(string clearTextFilename, string cipherTextFilename, char wordSeparator, char charSeparator);
        void DecryptFile(string clearTextFilename, string cipherTextFilename, char wordSeparator, char charSeparator);

    }
}
