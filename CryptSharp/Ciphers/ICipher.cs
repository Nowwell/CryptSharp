using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp
{
    public interface ICipher
    {
        string Encrypt(string clearText);
        void EncryptFile(string clearTextFilename, string cipherTextFilename);

        string Decrypt(string cipherText);
        void DecryptFile(string clearTextFilename, string cipherTextFilename);
    }
}
