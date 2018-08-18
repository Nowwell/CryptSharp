using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Hashes
{
    public class MD5
    {
        public MD5()
        {
            for (int i = 0; i < 64; i++)
            {
                T[i] = (uint)(4294967296.0 * Math.Abs(Math.Sin(i)));
            }
        }

        uint[] T = new uint[64];

        uint[] buffer = new uint[4];


        protected uint F(uint x, uint y, uint z)
        {
            return (x & y) | ((~y) & z);
        }
        protected uint G(uint x, uint y, uint z)
        {
            return (x & z) | ((~z) & y);
        }
        protected uint H(uint x, uint y, uint z)
        {
            return x ^ y ^ z;
        }
        protected uint I(uint x, uint y, uint z)
        {
            return y ^ ((~z) | x);
        }

    }
}
