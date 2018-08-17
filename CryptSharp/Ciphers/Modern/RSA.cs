using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.Modern
{
    public class RSA
    {
        public KeyPair Key { get; set; }

        public KeyPair GenerateKey()
        {
            return default(KeyPair);
        }

        public byte[] Encrypt(byte[] input)
        {
            BigInteger bi = new BigInteger(input);

            return BigInteger.ModPow(bi, Key.PublicKey.e, Key.PublicKey.n).ToByteArray();
        }

        public byte[] Decrypt(byte[] input)
        {
            BigInteger bi = new BigInteger(input);

            return BigInteger.ModPow(bi, Key.PublicKey.d, Key.PublicKey.n).ToByteArray();
        }
    }

    public class KeyPair
    {
        public RSAKey PublicKey { get; set; }//n, e, and d
        public RSAKey PrivateKey { get; set; }//n, e, and d
    }

    public class RSAKey
    {
        public BigInteger n { get; set; }
        public BigInteger e { get; set; }
        public BigInteger d { get; set; }
    }
}
