using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.Modern
{
    public class Rijndael
    {
        private byte[] key;
        public byte[] Key
        {
            get
            {
                return key;
            }

            set
            {
                if (value.Length % 32 != 0 || value.Length < 128 || value.Length > 256)
                {
                    throw new Exception(string.Format("Invliad key length, must be between 128 and 256 bits in 32 bit increments, length={0}", value.Length * 8));
                }

                key = value;
                Nk = Key.Length / 4;
                w = new uint[(Nr + 1) * Nb];
                w = KeyExpansion(key, Nk);
            }
        }

        private int blockLength = 128;
        public int BlockLength
        {
            get
            {
                return blockLength;
            }
            set
            {
                if (value % 32 != 0 || value < 128 || value > 256)
                {
                    throw new Exception(string.Format("Invliad block length, must be between 128 and 256 bits in 32 bit increments, length={0}", value));
                }

                blockLength = value;
            }
        }

        public byte[] IV { get; set; }

        int Nk = 0;//=Key.Length in bits / 32 bits
        int Nb = 0;//constant for AES spec
        int Nr = 0;//128 keysize = 10, 192=12, 256=14 - number of rounds

        uint[] w;

        byte[,] state;

        public byte[] Encrypt(byte[] input)
        {
            int r = 4;//height in bytes of state matrix
            int Nb = blockLength / 32;

            if (input.Length != blockLength / 8) throw new Exception("Input length not equal to block length");

            Nr = 10;
            if (Nb == 6 || Nk == 6) Nr = 12;
            if (Nb == 8 || Nk == 8) Nr = 14;

            int numberOfBlocks = input.Length / (r * Nb);

            int padding = (r * Nb) - input.Length % (r * Nb);
            if (padding == (r * Nb)) padding = 0;

            if (padding != 0) numberOfBlocks++;

            byte[] output = new byte[input.Length + padding];

            state = new byte[r, Nb];

            //If we have an IV, seed the state with it
            if (IV != null)
            {
                for (int i = 0; i < r; i++)
                {
                    for (int j = 0; j < Nb; j++)
                    {
                        state[i, j] = IV[i + 4 * j];
                    }
                }
            }

            //state = new byte[r, Nb];
            for (int block = 0; block < numberOfBlocks; block++)
            {
                for (int i = 0; i < r; i++)
                {
                    for (int j = 0; j < Nb; j++)
                    {
                        //If we have an IV, XOR the output with the new input block
                        if (i + 4 * j + (r * Nb * block) >= input.Length)
                        {
                            state[i, j] = (byte)(padding ^ (IV == null ? 0 : state[i, j]));
                        }
                        else
                        {
                            state[i, j] = (byte)(input[i + 4 * j + (r * Nb * block)] ^ (IV == null ? 0 : state[i, j]));
                        }
                    }
                }

                state = AddRoundKey(state, r, Nb, 0, w);

                for (int i = 1; i < Nr; i++)
                {
                    state = SubBytes(state, r, Nb);
                    state = ShiftRows(state, r, Nb);
                    state = MixColumns(state, r, Nb);
                    state = AddRoundKey(state, r, Nb, i, w);
                }

                state = SubBytes(state, r, Nb);
                state = ShiftRows(state, r, Nb);
                state = AddRoundKey(state, r, Nb, Nr, w);

                for (int i = 0; i < r; i++)
                {
                    for (int j = 0; j < Nb; j++)
                    {
                        output[i + 4 * j + (r * Nb * block)] = state[i, j];
                    }
                }
            }

            return output;
        }

        public byte[] Decrypt(byte[] input)
        {
            int r = 4;

            int numberOfBlocks = input.Length / (r * Nb);

            int padding = (r * Nb) - input.Length % (r * Nb);
            if (padding == (r * Nb)) padding = 0;

            if (padding != 0) numberOfBlocks++;

            byte[] output = new byte[input.Length + padding];

            state = new byte[r, Nb];
            byte[,] prevstate = new byte[r, Nb];

            //If we have an IV, XOR the state with it
            if (IV != null)
            {
                for (int i = 0; i < r; i++)
                {
                    for (int j = 0; j < Nb; j++)
                    {
                        prevstate[i, j] = IV[i + 4 * j];
                    }
                }
            }

            for (int block = 0; block < numberOfBlocks; block++)
            {
                for (int i = 0; i < r; i++)
                {
                    for (int j = 0; j < Nb; j++)
                    {
                        //If we have an IV, XOR the output with the new input block
                        if (i + 4 * j + (r * Nb * block) >= input.Length)
                        {
                            state[i, j] = (byte)(padding);// ^ (IV == null ? 0 : state[i, j]));
                        }
                        else
                        {
                            state[i, j] = (byte)(input[i + 4 * j + (r * Nb * block)]);// ^ (IV == null ? 0 : state[i, j]));
                        }
                    }
                }

                state = AddRoundKey(state, r, Nb, Nr, w);

                for (int i = Nr - 1; i > 0; i--)
                {
                    state = ShiftRowsInv(state, r, Nb);
                    state = SubBytesInv(state, r, Nb);
                    state = AddRoundKey(state, r, Nb, i, w);
                    state = MixColumnsInv(state, r, Nb);
                }

                state = ShiftRowsInv(state, r, Nb);
                state = SubBytesInv(state, r, Nb);
                state = AddRoundKey(state, r, Nb, 0, w);

                for (int i = 0; i < r; i++)
                {
                    for (int j = 0; j < Nb; j++)
                    {
                        if (i + 4 * j + (r * Nb * block) >= input.Length)
                        {
                            byte o = (byte)(padding ^ (IV == null ? 0 : prevstate[i, j]));
                            output[i + 4 * j + (r * Nb * block)] = o;
                            //state[i, j] = o;
                        }
                        else
                        {
                            byte o = (byte)(state[i, j] ^ (IV == null ? 0 : prevstate[i, j]));
                            output[i + 4 * j + (r * Nb * block)] = o;
                            //state[i, j] = o;
                        }
                    }
                }

                for (int i = 0; i < r; i++)
                {
                    for (int j = 0; j < Nb; j++)
                    {
                        prevstate[i, j] = input[i + 4 * j + (r * Nb * block)];
                    }
                }
            }

            return output;
        }

        public uint[] KeyExpansion(byte[] key, int Nk)
        {
            uint temp = 0;
            int i = 0;

            while (i < Nk)
            {
                w[i] = (uint)(key[4 * i] << 24 | key[4 * i + 1] << 16 | key[4 * i + 2] << 8 | key[4 * i + 3] << 0);
                i++;
            }

            i = Nk;

            while (i < Nb * (Nr + 1))
            {
                temp = w[i - 1];
                if (i % Nk == 0)
                {
                    uint z = RotWord(temp);
                    z = SubWord(z);
                    uint rc = Rcon(i / Nk);

                    temp = z ^ rc;
                }
                else if (Nk > 6 && i % Nk == 4)
                {
                    temp = SubWord(temp);
                }

                w[i] = w[i - Nk] ^ temp;
                i++;
            }

            return w;
        }

        public uint SubWord(uint v)
        {
            return (uint)((SBox[(v >> 24) & 0xFF] << 24) | (SBox[(v >> 16) & 0xFF] << 16) | (SBox[(v >> 8) & 0xFF] << 8) | SBox[v & 0xFF]);
        }

        public uint SubWordInv(uint v)
        {
            return (uint)((SBoxInv[(v >> 24) & 0xFF] << 24) | (SBoxInv[(v >> 16) & 0xFF] << 16) | (SBoxInv[(v >> 8) & 0xFF] << 8) | SBoxInv[v & 0xFF]);
        }

        public uint RotWord(uint v)
        {
            uint temp = v << 8;
            return temp | v >> 24;
        }

        public uint Rcon(int v)
        {
            int x = 0x1;
            for (int i = 0; i < v - 1; i++)
            {
                if ((x & 0x80) > 0)
                {
                    x = (x << 1);
                    x = x ^ 0x011B;
                }
                else
                {
                    x = (x << 1);
                    x = x ^ 0x00;
                }
            }

            return (uint)(x << 24);
        }

        public byte[,] AddRoundKey(byte[,] state, int r, int Nb, int round, uint[] w)
        {
            byte[,] shift = new byte[r, Nb];
            for (int c = 0; c < Nb; c++)
            {
                ulong xform = (ulong)(((state[0, c] << 24) | (state[1, c] << 16) | (state[2, c] << 8) | state[3, c]) ^ w[round * Nb + c]);

                shift[0, c] = (byte)(xform >> 24);
                shift[1, c] = (byte)(xform >> 16);
                shift[2, c] = (byte)(xform >> 8);
                shift[3, c] = (byte)(xform >> 0);
            }

            return shift;
        }

        public byte[,] SubBytes(byte[,] state, int r, int Nb)
        {
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < Nb; j++)
                {
                    state[i, j] = SBox[state[i, j]];
                }
            }

            return state;
        }

        public byte[,] SubBytesInv(byte[,] state, int r, int Nb)
        {
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < Nb; j++)
                {
                    state[i, j] = SBoxInv[state[i, j]];
                }
            }

            return state;
        }

        public byte[,] ShiftRows(byte[,] state, int r, int Nb)
        {
            byte[,] shift = new byte[r, Nb];

            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < Nb; j++)
                {
                    shift[i, j] = state[i, (j + i) % Nb];
                }
            }

            return shift;
        }

        public byte[,] ShiftRowsInv(byte[,] state, int r, int Nb)
        {
            byte[,] shift = new byte[r, Nb];

            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < Nb; j++)
                {
                    int index = ((j - i) % Nb + Nb) % Nb;

                    shift[i, j] = state[i, index];
                }
            }

            return shift;
        }

        public byte[,] MixColumns(byte[,] state, int r, int Nb)
        {
            //this is incorrect... might have fixed, check
            byte[,] shift = new byte[r, Nb];
            for (int c = 0; c < Nb; c++)
            {
                for(int x = 0; x < r; x++)
                {
                    shift[x % r, c] = (byte)(Multiply(0x02, state[x % r, c]) ^ Multiply(0x03, state[(x + 1) % r, c]) ^ state[(x + 2) % r, c] ^ state[(x + 3) % r, c]);

                }
                //shift[0, c] = (byte)(Multiply(0x02, state[0, c]) ^ Multiply(0x03, state[1, c]) ^ state[2, c] ^ state[3, c]);
                //shift[1, c] = (byte)(Multiply(0x02, state[1, c]) ^ Multiply(0x03, state[2, c]) ^ state[3, c] ^ state[0, c]);
                //shift[2, c] = (byte)(Multiply(0x02, state[2, c]) ^ Multiply(0x03, state[3, c]) ^ state[0, c] ^ state[1, c]);
                //shift[3, c] = (byte)(Multiply(0x02, state[3, c]) ^ Multiply(0x03, state[0, c]) ^ state[1, c] ^ state[2, c]);
            }

            return shift;
        }

        public byte[,] MixColumnsInv(byte[,] state, int r, int Nb)
        {
            byte[,] shift = new byte[r, Nb];
            for (int c = 0; c < Nb; c++)
            {
                shift[0, c] = (byte)(Multiply(0x0e, state[0, c]) ^ Multiply(0x0b, state[1, c]) ^ Multiply(0x0d, state[2, c]) ^ Multiply(0x09, state[3, c]));
                shift[1, c] = (byte)(Multiply(0x09, state[0, c]) ^ Multiply(0x0e, state[1, c]) ^ Multiply(0x0b, state[2, c]) ^ Multiply(0x0d, state[3, c]));
                shift[2, c] = (byte)(Multiply(0x0d, state[0, c]) ^ Multiply(0x09, state[1, c]) ^ Multiply(0x0e, state[2, c]) ^ Multiply(0x0b, state[3, c]));
                shift[3, c] = (byte)(Multiply(0x0b, state[0, c]) ^ Multiply(0x0d, state[1, c]) ^ Multiply(0x09, state[2, c]) ^ Multiply(0x0e, state[3, c]));
            }

            return shift;
        }

        public ulong Add(ulong a, ulong b)
        {
            return a ^ b;
        }

        public byte[] Multiply(byte[] a, byte[] b)
        {
            //Spec section 4.3
            byte[] ret = new byte[4];

            ret[0] = (byte)((a[0] * b[0]) ^ (a[3] * b[1]) ^ (a[2] * b[2]) ^ (a[1] * b[3]));
            ret[1] = (byte)((a[1] * b[0]) ^ (a[0] * b[1]) ^ (a[3] * b[2]) ^ (a[2] * b[3]));
            ret[2] = (byte)((a[2] * b[0]) ^ (a[1] * b[1]) ^ (a[0] * b[2]) ^ (a[3] * b[3]));
            ret[3] = (byte)((a[3] * b[0]) ^ (a[2] * b[1]) ^ (a[1] * b[2]) ^ (a[0] * b[3]));

            return ret;
        }

        public ulong Polymod(ulong f, ulong m)
        {
            int maxModulusDigit = 0;
            int maxFunctionDigit = 0;
            ulong temp = m;
            while (temp > 0)
            {
                temp >>= 1;
                maxModulusDigit++;
            }
            temp = f;
            while (temp > 0)
            {
                temp >>= 1;
                maxFunctionDigit++;
            }

            ulong modulus = f;

            if (maxFunctionDigit > maxModulusDigit)
            {
                //find modulus...
                while (maxFunctionDigit > maxModulusDigit)
                {
                    modulus = modulus ^ (m << (maxFunctionDigit - maxModulusDigit));

                    temp = modulus;
                    maxFunctionDigit = 0;
                    while (temp > 0)
                    {
                        temp >>= 1;
                        maxFunctionDigit++;
                    }
                }

            }
            else if (maxFunctionDigit < maxModulusDigit)
            {
                //find modular inverse
                modulus = modInverse(f, m);
            }
            else
            {
                //they're equal powers
                if (f > m)
                {
                    modulus = f ^ m;
                }
                else
                {
                    modulus = f;
                }
            }

            return modulus;
        }

        public byte XTime(byte f)
        {
            //must be done byte by byte?
            if (((f) << 1) > 255)
            {
                return (byte)(((f << 1) & 0xFF) ^ 0x1B);
            }
            else
            {
                return (byte)(f << 1);
            }
        }

        public byte Multiply(byte x, byte y)
        {
            byte ret = 0x0;
            for (int i = 0; i < 32 && y > 0; i++)
            {
                if ((y & 0x01) > 0)
                {
                    ret ^= x;
                }
                bool reduce = (x & 0x80) != 0;
                x <<= 1;
                if (reduce) x ^= 0x1B;
                y >>= 1;
            }

            return ret;
        }

        public ulong Multiply(uint x, uint y)
        {
            ulong ret = 0x0;
            for (int i = 0; i < 32 && y > 0; i++)
            {
                if ((y & 0x01) > 0)
                {
                    ret ^= (x << i);
                }
                y >>= 1;
            }

            ret = Polymod(ret, 0x11B);


            return ret;
        }

        public ulong modInverse(ulong a, ulong n)
        {
            ulong i = n, v = 0, d = 1;
            while (a > 0)
            {
                ulong t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
        }

        //https://stackoverflow.com/questions/34420086/aes-decryption-encryption-implementation-in-c-sharp-not-using-libraries
        //SBoxes copied from link
        private static byte[] SBox = new byte[256] {
                //0     1    2      3     4    5     6     7      8    9     A      B    C     D     E     F
                0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76, //0
                0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0, //1
                0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15, //2
                0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75, //3
                0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84, //4
                0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf, //5
                0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8, //6
                0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2, //7
                0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73, //8
                0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb, //9
                0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79, //A
                0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08, //B
                0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a, //C
                0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e, //D
                0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf, //E
                0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16 //F
        };

        private static byte[] SBoxInv = new byte[256] {
                //0     1    2      3     4    5     6     7      8    9     A      B    C     D     E     F
                0x52, 0x09, 0x6A, 0xD5, 0x30, 0x36, 0xA5, 0x38, 0xBF, 0x40, 0xA3, 0x9E, 0x81, 0xF3, 0xD7, 0xFB, //0
                0x7C, 0xE3, 0x39, 0x82, 0x9B, 0x2F, 0xFF, 0x87, 0x34, 0x8E, 0x43, 0x44, 0xC4, 0xDE, 0xE9, 0xCB, //1
                0x54, 0x7B, 0x94, 0x32, 0xA6, 0xC2, 0x23, 0x3D, 0xEE, 0x4C, 0x95, 0x0B, 0x42, 0xFA, 0xC3, 0x4E, //2
                0x08, 0x2E, 0xA1, 0x66, 0x28, 0xD9, 0x24, 0xB2, 0x76, 0x5B, 0xA2, 0x49, 0x6D, 0x8B, 0xD1, 0x25, //3
                0x72, 0xF8, 0xF6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xD4, 0xA4, 0x5C, 0xCC, 0x5D, 0x65, 0xB6, 0x92, //4
                0x6C, 0x70, 0x48, 0x50, 0xFD, 0xED, 0xB9, 0xDA, 0x5E, 0x15, 0x46, 0x57, 0xA7, 0x8D, 0x9D, 0x84, //5
                0x90, 0xD8, 0xAB, 0x00, 0x8C, 0xBC, 0xD3, 0x0A, 0xF7, 0xE4, 0x58, 0x05, 0xB8, 0xB3, 0x45, 0x06, //6
                0xD0, 0x2C, 0x1E, 0x8F, 0xCA, 0x3F, 0x0F, 0x02, 0xC1, 0xAF, 0xBD, 0x03, 0x01, 0x13, 0x8A, 0x6B, //7
                0x3A, 0x91, 0x11, 0x41, 0x4F, 0x67, 0xDC, 0xEA, 0x97, 0xF2, 0xCF, 0xCE, 0xF0, 0xB4, 0xE6, 0x73, //8
                0x96, 0xAC, 0x74, 0x22, 0xE7, 0xAD, 0x35, 0x85, 0xE2, 0xF9, 0x37, 0xE8, 0x1C, 0x75, 0xDF, 0x6E, //9
                0x47, 0xF1, 0x1A, 0x71, 0x1D, 0x29, 0xC5, 0x89, 0x6F, 0xB7, 0x62, 0x0E, 0xAA, 0x18, 0xBE, 0x1B, //A
                0xFC, 0x56, 0x3E, 0x4B, 0xC6, 0xD2, 0x79, 0x20, 0x9A, 0xDB, 0xC0, 0xFE, 0x78, 0xCD, 0x5A, 0xF4, //B
                0x1F, 0xDD, 0xA8, 0x33, 0x88, 0x07, 0xC7, 0x31, 0xB1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xEC, 0x5F, //C
                0x60, 0x51, 0x7F, 0xA9, 0x19, 0xB5, 0x4A, 0x0D, 0x2D, 0xE5, 0x7A, 0x9F, 0x93, 0xC9, 0x9C, 0xEF, //D
                0xA0, 0xE0, 0x3B, 0x4D, 0xAE, 0x2A, 0xF5, 0xB0, 0xC8, 0xEB, 0xBB, 0x3C, 0x83, 0x53, 0x99, 0x61, //E
                0x17, 0x2B, 0x04, 0x7E, 0xBA, 0x77, 0xD6, 0x26, 0xE1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0C, 0x7D }; //F


    }
}
