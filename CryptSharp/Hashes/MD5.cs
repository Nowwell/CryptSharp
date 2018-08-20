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
            for (int i = 0; i <= 64; i++)
            {
                T[i] = (uint)(4294967296.0 * Math.Abs(Math.Sin(i)));
            }
        }

        uint[] T = new uint[65];

        uint[] buffer = new uint[4];

        uint[] state = new uint[4];
        uint[] X = new uint[16];


        public string Hash(string clear)
        {
            //this works in blocks... need to update

            byte[] data = Pad(Encoding.Unicode.GetBytes(clear));

            state = Transform(state, data);

            return state[0].ToString("X") + state[1].ToString("X") + state[2].ToString("X") + state[3].ToString("X");
        }

        public uint[] Transform(uint[] state, byte[] block)
        {
            uint[] M = new uint[block.Length / 4];
            for (int i = 0; i < block.Length; i += 4)
            {
                M[i / 4] = (uint)(block[i + 3] << 24 | block[i + 2] << 16 | block[i + 1] << 8 | block[i]);
            }

            state[0] = 0x67452301;
            state[1] = 0xEFCDAB89;
            state[2] = 0x98BADCFE;
            state[3] = 0x10325476;

            uint[] tempState = new uint[4];

            for (int i = 0; i <= M.Length / 16 - 1; i++)
            {
                X = new uint[16];
                for (int j = 0; j < 15; j++)
                {
                    X[j] = M[i * 16 + j];
                }

                Array.Copy(state, tempState, 4);

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
                state[2] += OperationF(state[2], state[3], state[0], state[1], 10, s[(l - 1) % 4], l++);
                state[3] += OperationF(state[1], state[2], state[3], state[0], 11, s[(l - 1) % 4], l++);

                state[0] += OperationF(state[0], state[1], state[2], state[3], 12, s[(l - 1) % 4], l++);
                state[1] += OperationF(state[3], state[0], state[1], state[2], 13, s[(l - 1) % 4], l++);
                state[2] += OperationF(state[2], state[3], state[0], state[1], 14, s[(l - 1) % 4], l++);
                state[3] += OperationF(state[1], state[2], state[3], state[0], 15, s[(l - 1) % 4], l++);

                //G Operations
                s = new uint[] { 5, 9, 14, 20 };
                state[0] += OperationG(state[0], state[1], state[2], state[3], 1, s[(l - 1) % 4], l++);
                state[1] += OperationG(state[3], state[0], state[1], state[2], 6, s[(l - 1) % 4], l++);
                state[2] += OperationG(state[2], state[3], state[0], state[1], 11, s[(l - 1) % 4], l++);
                state[3] += OperationG(state[1], state[2], state[3], state[0], 0, s[(l - 1) % 4], l++);

                state[0] += OperationG(state[0], state[1], state[2], state[3], 5, s[(l - 1) % 4], l++);
                state[1] += OperationG(state[3], state[0], state[1], state[2], 10, s[(l - 1) % 4], l++);
                state[2] += OperationG(state[2], state[3], state[0], state[1], 15, s[(l - 1) % 4], l++);
                state[3] += OperationG(state[1], state[2], state[3], state[0], 4, s[(l - 1) % 4], l++);

                state[0] += OperationG(state[0], state[1], state[2], state[3], 9, s[(l - 1) % 4], l++);
                state[1] += OperationG(state[3], state[0], state[1], state[2], 14, s[(l - 1) % 4], l++);
                state[2] += OperationG(state[2], state[3], state[0], state[1], 3, s[(l - 1) % 4], l++);
                state[3] += OperationG(state[1], state[2], state[3], state[0], 8, s[(l - 1) % 4], l++);

                state[0] += OperationG(state[0], state[1], state[2], state[3], 13, s[(l - 1) % 4], l++);
                state[1] += OperationG(state[3], state[0], state[1], state[2], 2, s[(l - 1) % 4], l++);
                state[2] += OperationG(state[2], state[3], state[0], state[1], 7, s[(l - 1) % 4], l++);
                state[3] += OperationG(state[1], state[2], state[3], state[0], 12, s[(l - 1) % 4], l++);

                //H Operations
                s = new uint[] { 4, 11, 16, 23 };
                state[0] += OperationH(state[0], state[1], state[2], state[3], 5, s[(l - 1) % 4], l++);
                state[1] += OperationH(state[3], state[0], state[1], state[2], 8, s[(l - 1) % 4], l++);
                state[2] += OperationH(state[2], state[3], state[0], state[1], 11, s[(l - 1) % 4], l++);
                state[3] += OperationH(state[1], state[2], state[3], state[0], 14, s[(l - 1) % 4], l++);

                state[0] += OperationH(state[0], state[1], state[2], state[3], 1, s[(l - 1) % 4], l++);
                state[1] += OperationH(state[3], state[0], state[1], state[2], 4, s[(l - 1) % 4], l++);
                state[2] += OperationH(state[2], state[3], state[0], state[1], 7, s[(l - 1) % 4], l++);
                state[3] += OperationH(state[1], state[2], state[3], state[0], 10, s[(l - 1) % 4], l++);

                state[0] += OperationH(state[0], state[1], state[2], state[3], 13, s[(l - 1) % 4], l++);
                state[1] += OperationH(state[3], state[0], state[1], state[2], 0, s[(l - 1) % 4], l++);
                state[2] += OperationH(state[2], state[3], state[0], state[1], 3, s[(l - 1) % 4], l++);
                state[3] += OperationH(state[1], state[2], state[3], state[0], 6, s[(l - 1) % 4], l++);

                state[0] += OperationH(state[0], state[1], state[2], state[3], 9, s[(l - 1) % 4], l++);
                state[1] += OperationH(state[3], state[0], state[1], state[2], 12, s[(l - 1) % 4], l++);
                state[2] += OperationH(state[2], state[3], state[0], state[1], 15, s[(l - 1) % 4], l++);
                state[3] += OperationH(state[1], state[2], state[3], state[0], 2, s[(l - 1) % 4], l++);

                //I Operations
                s = new uint[] { 6, 10, 15, 21 };
                state[0] += OperationI(state[0], state[1], state[2], state[3], 0, s[(l - 1) % 4], l++);
                state[1] += OperationI(state[3], state[0], state[1], state[2], 7, s[(l - 1) % 4], l++);
                state[2] += OperationI(state[2], state[3], state[0], state[1], 14, s[(l - 1) % 4], l++);
                state[3] += OperationI(state[1], state[2], state[3], state[0], 5, s[(l - 1) % 4], l++);

                state[0] += OperationI(state[0], state[1], state[2], state[3], 12, s[(l - 1) % 4], l++);
                state[1] += OperationI(state[3], state[0], state[1], state[2], 3, s[(l - 1) % 4], l++);
                state[2] += OperationI(state[2], state[3], state[0], state[1], 10, s[(l - 1) % 4], l++);
                state[3] += OperationI(state[1], state[2], state[3], state[0], 1, s[(l - 1) % 4], l++);

                state[0] += OperationI(state[0], state[1], state[2], state[3], 8, s[(l - 1) % 4], l++);
                state[1] += OperationI(state[3], state[0], state[1], state[2], 15, s[(l - 1) % 4], l++);
                state[2] += OperationI(state[2], state[3], state[0], state[1], 6, s[(l - 1) % 4], l++);
                state[3] += OperationI(state[1], state[2], state[3], state[0], 13, s[(l - 1) % 4], l++);

                state[0] += OperationI(state[0], state[1], state[2], state[3], 4, s[(l - 1) % 4], l++);
                state[1] += OperationI(state[3], state[0], state[1], state[2], 11, s[(l - 1) % 4], l++);
                state[2] += OperationI(state[2], state[3], state[0], state[1], 2, s[(l - 1) % 4], l++);
                state[3] += OperationI(state[1], state[2], state[3], state[0], 9, s[(l - 1) % 4], l++);

                state[0] += tempState[0];
                state[1] += tempState[1];
                state[2] += tempState[2];
                state[3] += tempState[3];
            }

            return state;
        }
        protected byte[] Pad(byte[] data)
        {
            List<byte> padded = new List<byte>(data);

            int index = padded.Count % 64;

            if(padded.Count < 56)
            {
                padded.Add(0x80);
                for (int i = 1; i < 56 - index; i++)
                {
                    padded.Add(0x00);
                }
            }
            else
            {
                padded.Add(0x80);
                for (int i = 1; i < 120 - index; i++)
                {
                    padded.Add(0x00);
                }
            }

            long length = padded.Count;
            padded.Add((byte)(length >> 0 & 0xFF));
            padded.Add((byte)(length >> 8 & 0xFF));
            padded.Add((byte)(length >> 16 & 0xFF));
            padded.Add((byte)(length >> 24 & 0xFF));
            padded.Add((byte)(length >> 32 & 0xFF));
            padded.Add((byte)(length >> 40 & 0xFF));
            padded.Add((byte)(length >> 48 & 0xFF));
            padded.Add((byte)(length >> 56 & 0xFF));

            return padded.ToArray();
        }

        protected uint F(uint x, uint y, uint z)
        {
            return (x & y) | ((~x) & z);
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
