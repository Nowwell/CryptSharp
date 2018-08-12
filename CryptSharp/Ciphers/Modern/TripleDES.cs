using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.Modern
{
    public class TripleDES : DES
    {
        public byte[] Key2 { get; set; }
        public override byte[] Encrypt(byte[] clearText)
        {
            byte[] tempKey = Key;
            byte[] temp = base.Encrypt(clearText);

            Key = Key2;

            temp = base.Decrypt(temp);

            Key2 = tempKey;

            temp = base.Encrypt(temp);

            Key2 = Key;
            Key = tempKey;

            return temp;

        }

        public override byte[] Decrypt(byte[] cipherText)
        {
            byte[] tempKey = Key;
            byte[] temp = base.Decrypt(cipherText);

            Key = Key2;

            temp = base.Encrypt(temp);

            Key2 = tempKey;

            temp = base.Decrypt(temp);

            Key2 = Key;
            Key = tempKey;

            return temp;
        }

    }
}
