using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Hashes
{
    public class MD5
    {
        //https://www.ietf.org/rfc/rfc1321.txt
        public MD5()
        {
            for (int i = 0; i < 64; i++)
            {
                T[i] = (uint)(4294967296.0 * Math.Abs(Math.Sin(i)));
            }
        }

        uint[] T = new uint[64];

        uint[] buffer = new uint[4];

        uint[] state = new uint[4];
        uint[] X = new uint[16];

        //#define S11 7
        //#define S12 12
        //#define S13 17
        //#define S14 22
        //#define S21 5
        //#define S22 9
        //#define S23 14
        //#define S24 20
        //#define S31 4
        //#define S32 11
        //#define S33 16
        //#define S34 23
        //#define S41 6
        //#define S42 10
        //#define S43 15
        //#define S44 21


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

        public string Hash(string clear)
        {
            //this works in blocks... need to update


            byte[] data = Pad(Encoding.Unicode.GetBytes(clear));


            uint[] M = new uint[data.Length / 4];
            for (int i = 0; i < data.Length / 4; i++)
            {
                M[i] = (uint)(data[i + 3] << 24 | data[i + 2] << 16 | data[i + 1] << 8 | data[i]);
            }

            /*
            Low order bytes listed first
                word A: 01 23 45 67
                word B: 89 ab cd ef
                word C: fe dc ba 98
                word D: 76 54 32 10
            */

            state[0] = 0x67452301;
            state[1] = 0xEFCDAB89;
            state[2] = 0x98BADCFE;
            state[3] = 0x10325476;

            uint[] tempState = new uint[4];

            for (int i = 0; i < M.Length / 16 - 1; i++)
            {
                X = new uint[16];
                for (int j = 0; j < 15; j++)
                {
                    X[j] = M[i * 16 + j];
                }

                Array.Copy(state, tempState, 4);

                //A = B + ((F(B, C, D) + X[k] + T[i]) <<< S);

                uint l = 1;

                //F Operations
                uint[] s = new uint[] { 7, 12, 17, 22 };
                state[0] += OperationF(state[0], state[1], state[2], state[3], 0, s[(l - 1) % 4], l++);
                state[1] += OperationF(state[3], state[0], state[1], state[2], 1, s[(l - 1) % 4], l++);
                state[2] += OperationF(state[2], state[3], state[0], state[1], 2, s[(l - 1) % 4], l++);
                state[3] += OperationF(state[1], state[2], state[3], state[0], 3, s[(l - 1) % 4], l++);

                state[0] += OperationF(state[0], state[1], state[2], state[3], 4, s[(l - 1) % 4], l++);
                state[1] += OperationF(state[3], state[0], state[1], state[2], 5, s[(l - 1) % 4], l++);
                state[2] += OperationF(state[2], state[3], state[0], state[1], 6, s[(l - 1) % 4], l++);
                state[3] += OperationF(state[1], state[2], state[3], state[0], 7, s[(l - 1) % 4], l++);

                state[0] += OperationF(state[0], state[1], state[2], state[3], 8, s[(l - 1) % 4], l++);
                state[1] += OperationF(state[3], state[0], state[1], state[2], 9, s[(l - 1) % 4], l++);
                state[2] += OperationF(state[2], state[3], state[0], state[1],10, s[(l - 1) % 4], l++);
                state[3] += OperationF(state[1], state[2], state[3], state[0],11, s[(l - 1) % 4], l++);

                state[0] += OperationF(state[0], state[1], state[2], state[3],12, s[(l - 1) % 4], l++);
                state[1] += OperationF(state[3], state[0], state[1], state[2],13, s[(l - 1) % 4], l++);
                state[2] += OperationF(state[2], state[3], state[0], state[1],14, s[(l - 1) % 4], l++);
                state[3] += OperationF(state[1], state[2], state[3], state[0],15, s[(l - 1) % 4], l++);

                //G Operations
                s = new uint[] { 5, 9, 14, 20 };
                state[0] += OperationG(state[0], state[1], state[2], state[3], 1, s[(l - 1) % 4], l++);
                state[1] += OperationG(state[3], state[0], state[1], state[2], 6, s[(l - 1) % 4], l++);
                state[2] += OperationG(state[2], state[3], state[0], state[1],11, s[(l - 1) % 4], l++);
                state[3] += OperationG(state[1], state[2], state[3], state[0], 0, s[(l - 1) % 4], l++);

                state[0] += OperationG(state[0], state[1], state[2], state[3], 5, s[(l - 1) % 4], l++);
                state[1] += OperationG(state[3], state[0], state[1], state[2],10, s[(l - 1) % 4], l++);
                state[2] += OperationG(state[2], state[3], state[0], state[1],15, s[(l - 1) % 4], l++);
                state[3] += OperationG(state[1], state[2], state[3], state[0], 4, s[(l - 1) % 4], l++);

                state[0] += OperationG(state[0], state[1], state[2], state[3], 9, s[(l - 1) % 4], l++);
                state[1] += OperationG(state[3], state[0], state[1], state[2],14, s[(l - 1) % 4], l++);
                state[2] += OperationG(state[2], state[3], state[0], state[1], 3, s[(l - 1) % 4], l++);
                state[3] += OperationG(state[1], state[2], state[3], state[0], 8, s[(l - 1) % 4], l++);

                state[0] += OperationG(state[0], state[1], state[2], state[3],13, s[(l - 1) % 4], l++);
                state[1] += OperationG(state[3], state[0], state[1], state[2], 2, s[(l - 1) % 4], l++);
                state[2] += OperationG(state[2], state[3], state[0], state[1], 7, s[(l - 1) % 4], l++);
                state[3] += OperationG(state[1], state[2], state[3], state[0],12, s[(l - 1) % 4], l++);

                //H Operations
                s = new uint[] { 4, 11, 16, 23 };
                state[0] += OperationH(state[0], state[1], state[2], state[3], 5, s[(l - 1) % 4], l++);
                state[1] += OperationH(state[3], state[0], state[1], state[2], 8, s[(l - 1) % 4], l++);
                state[2] += OperationH(state[2], state[3], state[0], state[1],11, s[(l - 1) % 4], l++);
                state[3] += OperationH(state[1], state[2], state[3], state[0],14, s[(l - 1) % 4], l++);

                state[0] += OperationH(state[0], state[1], state[2], state[3], 1, s[(l - 1) % 4], l++);
                state[1] += OperationH(state[3], state[0], state[1], state[2], 4, s[(l - 1) % 4], l++);
                state[2] += OperationH(state[2], state[3], state[0], state[1], 7, s[(l - 1) % 4], l++);
                state[3] += OperationH(state[1], state[2], state[3], state[0],10, s[(l - 1) % 4], l++);

                state[0] += OperationH(state[0], state[1], state[2], state[3],13, s[(l - 1) % 4], l++);
                state[1] += OperationH(state[3], state[0], state[1], state[2], 0, s[(l - 1) % 4], l++);
                state[2] += OperationH(state[2], state[3], state[0], state[1], 3, s[(l - 1) % 4], l++);
                state[3] += OperationH(state[1], state[2], state[3], state[0], 6, s[(l - 1) % 4], l++);

                state[0] += OperationH(state[0], state[1], state[2], state[3], 9, s[(l - 1) % 4], l++);
                state[1] += OperationH(state[3], state[0], state[1], state[2],12, s[(l - 1) % 4], l++);
                state[2] += OperationH(state[2], state[3], state[0], state[1],15, s[(l - 1) % 4], l++);
                state[3] += OperationH(state[1], state[2], state[3], state[0], 2, s[(l - 1) % 4], l++);

                //I Operations
                s = new uint[] { 6, 10, 15, 21 };
                state[0] += OperationI(state[0], state[1], state[2], state[3], 0, s[(l - 1) % 4], l++);
                state[1] += OperationI(state[3], state[0], state[1], state[2], 7, s[(l - 1) % 4], l++);
                state[2] += OperationI(state[2], state[3], state[0], state[1],14, s[(l - 1) % 4], l++);
                state[3] += OperationI(state[1], state[2], state[3], state[0], 5, s[(l - 1) % 4], l++);

                state[0] += OperationI(state[0], state[1], state[2], state[3],12, s[(l - 1) % 4], l++);
                state[1] += OperationI(state[3], state[0], state[1], state[2], 3, s[(l - 1) % 4], l++);
                state[2] += OperationI(state[2], state[3], state[0], state[1],10, s[(l - 1) % 4], l++);
                state[3] += OperationI(state[1], state[2], state[3], state[0], 1, s[(l - 1) % 4], l++);

                state[0] += OperationI(state[0], state[1], state[2], state[3], 8, s[(l - 1) % 4], l++);
                state[1] += OperationI(state[3], state[0], state[1], state[2],15, s[(l - 1) % 4], l++);
                state[2] += OperationI(state[2], state[3], state[0], state[1], 6, s[(l - 1) % 4], l++);
                state[3] += OperationI(state[1], state[2], state[3], state[0],13, s[(l - 1) % 4], l++);

                state[0] += OperationI(state[0], state[1], state[2], state[3], 4, s[(l - 1) % 4], l++);
                state[1] += OperationI(state[3], state[0], state[1], state[2],11, s[(l - 1) % 4], l++);
                state[2] += OperationI(state[2], state[3], state[0], state[1], 2, s[(l - 1) % 4], l++);
                state[3] += OperationI(state[1], state[2], state[3], state[0], 9, s[(l - 1) % 4], l++);

                state[0] += tempState[0];
                state[1] += tempState[1];
                state[2] += tempState[2];
                state[3] += tempState[3];



                ///* Round 1 */
                //FF(a, b, c, d, x[0], S11, 0xd76aa478); /* 1 */
                //FF(d, a, b, c, x[1], S12, 0xe8c7b756); /* 2 */
                //FF(c, d, a, b, x[2], S13, 0x242070db); /* 3 */
                //FF(b, c, d, a, x[3], S14, 0xc1bdceee); /* 4 */
                //FF(a, b, c, d, x[4], S11, 0xf57c0faf); /* 5 */
                //FF(d, a, b, c, x[5], S12, 0x4787c62a); /* 6 */
                //FF(c, d, a, b, x[6], S13, 0xa8304613); /* 7 */
                //FF(b, c, d, a, x[7], S14, 0xfd469501); /* 8 */
                //FF(a, b, c, d, x[8], S11, 0x698098d8); /* 9 */
                //FF(d, a, b, c, x[9], S12, 0x8b44f7af); /* 10 */
                //FF(c, d, a, b, x[10], S13, 0xffff5bb1); /* 11 */
                //FF(b, c, d, a, x[11], S14, 0x895cd7be); /* 12 */
                //FF(a, b, c, d, x[12], S11, 0x6b901122); /* 13 */
                //FF(d, a, b, c, x[13], S12, 0xfd987193); /* 14 */
                //FF(c, d, a, b, x[14], S13, 0xa679438e); /* 15 */
                //FF(b, c, d, a, x[15], S14, 0x49b40821); /* 16 */

                ///* Round 2 */
                //GG(a, b, c, d, x[1], S21, 0xf61e2562); /* 17 */
                //GG(d, a, b, c, x[6], S22, 0xc040b340); /* 18 */
                //GG(c, d, a, b, x[11], S23, 0x265e5a51); /* 19 */
                //GG(b, c, d, a, x[0], S24, 0xe9b6c7aa); /* 20 */
                //GG(a, b, c, d, x[5], S21, 0xd62f105d); /* 21 */
                //GG(d, a, b, c, x[10], S22, 0x2441453); /* 22 */
                //GG(c, d, a, b, x[15], S23, 0xd8a1e681); /* 23 */
                //GG(b, c, d, a, x[4], S24, 0xe7d3fbc8); /* 24 */
                //GG(a, b, c, d, x[9], S21, 0x21e1cde6); /* 25 */
                //GG(d, a, b, c, x[14], S22, 0xc33707d6); /* 26 */
                //GG(c, d, a, b, x[3], S23, 0xf4d50d87); /* 27 */
                //GG(b, c, d, a, x[8], S24, 0x455a14ed); /* 28 */
                //GG(a, b, c, d, x[13], S21, 0xa9e3e905); /* 29 */
                //GG(d, a, b, c, x[2], S22, 0xfcefa3f8); /* 30 */
                //GG(c, d, a, b, x[7], S23, 0x676f02d9); /* 31 */
                //GG(b, c, d, a, x[12], S24, 0x8d2a4c8a); /* 32 */

                ///* Round 3 */
                //HH(a, b, c, d, x[5], S31, 0xfffa3942); /* 33 */
                //HH(d, a, b, c, x[8], S32, 0x8771f681); /* 34 */
                //HH(c, d, a, b, x[11], S33, 0x6d9d6122); /* 35 */
                //HH(b, c, d, a, x[14], S34, 0xfde5380c); /* 36 */
                //HH(a, b, c, d, x[1], S31, 0xa4beea44); /* 37 */
                //HH(d, a, b, c, x[4], S32, 0x4bdecfa9); /* 38 */
                //HH(c, d, a, b, x[7], S33, 0xf6bb4b60); /* 39 */
                //HH(b, c, d, a, x[10], S34, 0xbebfbc70); /* 40 */
                //HH(a, b, c, d, x[13], S31, 0x289b7ec6); /* 41 */
                //HH(d, a, b, c, x[0], S32, 0xeaa127fa); /* 42 */
                //HH(c, d, a, b, x[3], S33, 0xd4ef3085); /* 43 */
                //HH(b, c, d, a, x[6], S34, 0x4881d05); /* 44 */
                //HH(a, b, c, d, x[9], S31, 0xd9d4d039); /* 45 */
                //HH(d, a, b, c, x[12], S32, 0xe6db99e5); /* 46 */
                //HH(c, d, a, b, x[15], S33, 0x1fa27cf8); /* 47 */
                //HH(b, c, d, a, x[2], S34, 0xc4ac5665); /* 48 */

                ///* Round 4 */
                //II(a, b, c, d, x[0], S41, 0xf4292244); /* 49 */
                //II(d, a, b, c, x[7], S42, 0x432aff97); /* 50 */
                //II(c, d, a, b, x[14], S43, 0xab9423a7); /* 51 */
                //II(b, c, d, a, x[5], S44, 0xfc93a039); /* 52 */
                //II(a, b, c, d, x[12], S41, 0x655b59c3); /* 53 */
                //II(d, a, b, c, x[3], S42, 0x8f0ccc92); /* 54 */
                //II(c, d, a, b, x[10], S43, 0xffeff47d); /* 55 */
                //II(b, c, d, a, x[1], S44, 0x85845dd1); /* 56 */
                //II(a, b, c, d, x[8], S41, 0x6fa87e4f); /* 57 */
                //II(d, a, b, c, x[15], S42, 0xfe2ce6e0); /* 58 */
                //II(c, d, a, b, x[6], S43, 0xa3014314); /* 59 */
                //II(b, c, d, a, x[13], S44, 0x4e0811a1); /* 60 */
                //II(a, b, c, d, x[4], S41, 0xf7537e82); /* 61 */
                //II(d, a, b, c, x[11], S42, 0xbd3af235); /* 62 */
                //II(c, d, a, b, x[2], S43, 0x2ad7d2bb); /* 63 */
                //II(b, c, d, a, x[9], S44, 0xeb86d391); /* 64 */
            }

            return state[0].ToString("X") + state[1].ToString("X") + state[2].ToString("X") + state[3].ToString("X");

        }

        protected byte[] Pad(byte[] data)
        {
            List<byte> padded = new List<byte>(data);

            padded.Add(0x80);
            while (padded.Count % 64 != 56)
            {
                padded.Add(0x00);
            }

            return padded.ToArray();
        }

        protected uint OperationF(uint A, uint B, uint C, uint D, uint k, uint s, uint i)
        {
            return B + ShiftLeft(A + F(B, C, D) + X[k] + T[i], s);
        }
        protected uint OperationG(uint A, uint B, uint C, uint D, uint k, uint s, uint i)
        {
            return B + ShiftLeft(A + G(B, C, D) + X[k] + T[i], s);
        }
        protected uint OperationH(uint A, uint B, uint C, uint D, uint k, uint s, uint i)
        {
            return B + ShiftLeft(A + H(B, C, D) + X[k] + T[i], s);
        }
        protected uint OperationI(uint A, uint B, uint C, uint D, uint k, uint s, uint i)
        {
            return B + ShiftLeft(A + I(B, C, D) + X[k] + T[i], s);
        }

        protected uint ShiftLeft(uint var, uint bits)
        {
            for (int i = 0; i < bits; i++)
            {
                uint bit = (uint)(0x80000000 & var);
                var <<= 1;
                var |= bit;
            }

            return var;
        }

    }
}
