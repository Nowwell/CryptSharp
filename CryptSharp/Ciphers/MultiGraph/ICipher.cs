using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.MultiGraph
{
    public interface ICipher
    {
        //string Encrypt(string clearText, string alphabet);
        //string Encrypt(string clearText, string alphabet);
        //void EncryptFile(string clearTextFilename, string cipherTextFilename, string alphabet);
        //void DecryptFile(string clearTextFilename, string cipherTextFilename, string alphabet);

        string Encrypt(string[] clearText);
        string Encrypt(string clearText, char wordSeparator, char charSeparator);

        string Decrypt(string[] cipherText);
        string Decrypt(string cipherText, char wordSeparator, char charSeparator);

        void EncryptFile(string clearTextFilename, string cipherTextFilename, char wordSeparator, char charSeparator);
        void DecryptFile(string clearTextFilename, string cipherTextFilename, char wordSeparator, char charSeparator);

    }
}
