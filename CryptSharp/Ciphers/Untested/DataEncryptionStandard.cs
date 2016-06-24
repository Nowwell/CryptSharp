using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers
{
    public class DES
    {
        public DES()
        {
        }

        //64-bits, 8 bytes
        //High order bit is used for parity checking and not used in the actual encryption
        //this reduces the actual key to 56 bits
        public byte[] Key { get; set; }

        public byte[] Encrypt(byte[] clearText)
        {
            //64 bits at a time to encrypt, 8 bytes - ulong
            //16 rounds
            ulong key = 0x0E329232EA6D0D73;//133457799BBCDFF1;//0x3b3898371520f75e
            clearText = BitConverter.GetBytes(0x8787878787878787);

            ulong flag = 0x01;
            byte[] keyBits = new byte[64];
            byte[] keyBytes = BitConverter.GetBytes(key);
            for (int i = 0, j = 8; i < 8; i ++)
            {
                while (j-- > 0)
                {
                    keyBits[63 - (8 * i + j)] = (byte)((keyBytes[i] & (flag << j)) >> j);

                }
                j = 8;
            }


            byte[] chunk = new byte[64];
            for (int i = 0; i < clearText.Length; i += 8)
            {
                for (int k = i, j = 8; k < i + 8; k++)
                {
                    while (j-- > 0)
                    {
                        if (clearText.Length <= k)
                        {
                            //pad with zeroes
                            chunk[63 - (8 * (k - i) + j)] = 0;
                        }
                        else
                        {
                            chunk[63 - (8 * (k - i) + j)] = (byte)((clearText[k] & (flag << j)) >> j);
                        }

                    }
                    j = 8;
                }

                byte[] next = Permutate(chunk, IP);

                byte[] left = new byte[32];
                byte[] right = new byte[32];

                Array.Copy(next, 0, left, 0, 32);
                Array.Copy(next, 32, right, 0, 32);

                byte[][] keySchedule = new byte[16][];
                byte[] subkey = new byte[56];

                subkey = Permutate(keyBits, PC1);
                byte[] c = new byte[28];
                byte[] d = new byte[28];
                Array.Copy(subkey, 0, c, 0, 28);
                Array.Copy(subkey, 28, d, 0, 28);
                for (int j = 0; j < 16; j++)
                {
                    //rotate left shift once (upper 28 bits rotated independently of lower 28 bits)
                    byte temp = c[0];
                    for (int k = 1; k < c.Length; k++)
                    {
                        c[k - 1] = c[k];
                    }
                    c[c.Length - 1] = temp;

                    temp = d[0];
                    for (int k = 1; k < c.Length; k++)
                    {
                        d[k - 1] = d[k];
                    }
                    d[d.Length - 1] = temp;


                    if (!(j == 0 || j == 1 || j == 8 || j == 15))
                    {
                        //rotate left shift twice (upper 28 bits rotated independently of lower 28 bits)
                        temp = c[0];
                        for (int k = 1; k < c.Length; k++)
                        {
                            c[k - 1] = c[k];
                        }
                        c[c.Length - 1] = temp;

                        temp = d[0];
                        for (int k = 1; k < c.Length; k++)
                        {
                            d[k - 1] = d[k];
                        }
                        d[d.Length - 1] = temp;
                    }

                    Array.Copy(c, 0, subkey, 0, 28);
                    Array.Copy(d, 0, subkey, 28, 28);

                    keySchedule[j] = Permutate(subkey, PC2);
                }


                for (int j = 0; j < 16; j++)
                {
                    byte[] e = Permutate(right, EXP);
                    for (int k = 0; k < 48; k++)
                    {
                        e[k] = (byte)(e[k] ^ keySchedule[j][k]);
                    }

                    byte[] sbox = new byte[32];
                    int z = 0;
                    for (int k = 0; k < 48; k += 6)
                    {
                        byte y = (byte)(e[k] << 1 | (e[k + 5]));
                        byte x = (byte)(e[k + 1] <<3 | (e[k + 2] << 2) | (e[k + 3] << 1) | (e[k + 4]));

                        byte val = 0;
                        if (k / 6 == 0) val = S1[16 * y + x];
                        if (k / 6 == 1) val = S2[16 * y + x];
                        if (k / 6 == 2) val = S3[16 * y + x];
                        if (k / 6 == 3) val = S4[16 * y + x];
                        if (k / 6 == 4) val = S5[16 * y + x];
                        if (k / 6 == 5) val = S6[16 * y + x];
                        if (k / 6 == 6) val = S7[16 * y + x];
                        if (k / 6 == 7) val = S8[16 * y + x];

                        sbox[z++] = (byte)((val & 0x08) >> 3);
                        sbox[z++] = (byte)((val & 0x04) >> 2);
                        sbox[z++] = (byte)((val & 0x02) >> 1);
                        sbox[z++] = (byte)(val & 0x01);
                    }

                    byte[] newR = Permutate(sbox, P);

                    for (int k = 0; k < 32; k++)
                    {
                        newR[k] = (byte)(newR[k] ^ left[k]);
                    }

                    Array.Copy(right, 0, left, 0, 32);
                    Array.Copy(newR, 0, right, 0, 32);
                }

                Array.Copy(left, 0, chunk, 32, 32);
                Array.Copy(right, 0, chunk, 0, 32);

                chunk = Permutate(chunk, FIP);

                ulong output = 0;
                for (int j = 0; j < 64; j++)
                {
                    int shift = 63 - j;
                    ulong value = chunk[j];

                    output += value << shift;
                }


            }
            //this works above...








            for (int i = 0; i < clearText.Length; i += 8)
            {
                ulong next = DoIP(BitConverter.ToUInt64(clearText, i));

                uint left = (uint)(next >> 32);
                uint right = (uint)(next & 0xFFFFFFFF);
                uint temp = 0;

                ulong[] keySchedule = new ulong[16];
                keySchedule[0] = KeySchedule(key, 1);
                keySchedule[1] = KeySchedule(key, 2);
                keySchedule[2] = KeySchedule(key, 3);
                keySchedule[3] = KeySchedule(key, 4);
                keySchedule[4] = KeySchedule(key, 5);
                keySchedule[5] = KeySchedule(key, 6);
                keySchedule[6] = KeySchedule(key, 7);
                keySchedule[7] = KeySchedule(key, 8);
                keySchedule[8] = KeySchedule(key, 9);
                keySchedule[9] = KeySchedule(key, 10);
                keySchedule[10] = KeySchedule(key, 11);
                keySchedule[11] = KeySchedule(key, 12);
                keySchedule[12] = KeySchedule(key, 13);
                keySchedule[13] = KeySchedule(key, 14);
                keySchedule[14] = KeySchedule(key, 15);
                keySchedule[15] = KeySchedule(key, 16);


                for (int j = 0; j < 16; j++)
                {
                    //ulong subkey = KeySchedule(key, j);

                    temp = f(right, keySchedule[i]) ^ left;
                    left = right;
                    right = temp;
                }

                //TODO make sure these lo and hi values aren't switched.
                ulong cipher = DoFIP((ulong)right + ((ulong)left << 32));
                byte[] cipherBytes = BitConverter.GetBytes(cipher);
                cipherBytes.CopyTo(clearText, i);
            }

            return clearText;
        }
        //public string Encrypt(string[] clearText)
        //{
        //    //64 bits at a time to encrypt, 8 bytes - ulong
        //    //16 rounds
        //    ulong key = BitConverter.ToUInt64(Key, 0);
        //    byte[] array = Encoding.ASCII.GetBytes(string.Join("", clearText));

        //    for (int i = 0; i < array.Length; i += 4)
        //    {
        //        ulong next = DoIP(BitConverter.ToUInt64(array, i));

        //        uint hi = (uint)(next >> 32);
        //        uint lo = (uint)(next & 0xFFFFFFFF);
        //        uint temp = 0;

        //        for (int j = 1; j <= 16; j++)
        //        {
        //            key = KeySchedule(key, j);

        //            temp = f(hi, key) ^ lo;
        //            lo = hi;
        //            hi = temp;
        //        }

        //        //TODO make sure these lo and hi values aren't switched.
        //        ulong cipher = DoFIP((ulong)lo + ((ulong)hi << 32));
        //        byte[] cipherBytes = BitConverter.GetBytes(cipher);
        //        cipherBytes.CopyTo(array, i);
        //    }

        //    return ASCIIEncoding.ASCII.GetString(array);
        //}
        //public string Encrypt(string clearText, char wordSeparator, char charSeparator)
        //{
        //    //64 bits at a time to encrypt, 8 bytes - ulong
        //    //16 rounds
        //    ulong key = BitConverter.ToUInt64(Key, 0);
        //    byte[] array = Encoding.ASCII.GetBytes(clearText);

        //    for (int i = 0; i < array.Length; i += 4)
        //    {
        //        ulong next = DoIP(BitConverter.ToUInt64(array, i));

        //        uint hi = (uint)(next >> 32);
        //        uint lo = (uint)(next & 0xFFFFFFFF);
        //        uint temp = 0;

        //        for (int j = 16; j >= 1; j--)
        //        //for (int j = 1; j <= 16; j++)
        //        {
        //            key = KeySchedule(key, j);

        //            temp = f(hi, key) ^ lo;
        //            lo = hi;
        //            hi = temp;
        //        }

        //        //TODO make sure these lo and hi values aren't switched.
        //        ulong cipher = DoFIP((ulong)lo + ((ulong)hi << 32));
        //        byte[] cipherBytes = BitConverter.GetBytes(cipher);
        //        cipherBytes.CopyTo(array, i);
        //    }

        //    return ASCIIEncoding.ASCII.GetString(array);
        //}

        //public string Decrypt(string[] cipherText)
        //{
        //    throw new NotImplementedException();
        //}
        //public string Decrypt(string cipherText, char wordSeparator, char charSeparator)
        //{
        //    throw new NotImplementedException();
        //}

        public void EncryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }
        public void DecryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }

        //protected ulong Round(ulong input, ulong subkey)
        //{
        //    uint left = (uint)(input >> 32);
        //    uint right = (uint)(input & 0xFFFFFFFF);
        //    uint temp = 0;



        //}

        protected byte[] Permutate(byte[] chunk, byte[] permutation)
        {
            byte[] output = new byte[permutation.Length];
            for (int i = 0; i < permutation.Length; i++)
            {
                output[i] = chunk[permutation[i] - 1];
            }
            return output;
        }






        protected ulong DoIP(ulong chunk)
        {
            ulong output = 0;
            ulong flag = 0x1;
            for (int i = 0; i < 64; i++)
            {
                ulong bit = (chunk & (flag << (IP[i] - 1))) >> (IP[i] - 1);

                output |= bit << i;
            }

            return output;
        }
        protected ulong DoFIP(ulong chunk)
        {
            ulong output = 0;
            ulong flag = 0x1;
            for (int i = 0; i < 64; i++)
            {
                ulong bit = (chunk & (flag << (FIP[i] - 1))) >> (FIP[i] - 1);

                output |= bit << i;
            }

            return output;
        }

        protected ulong DoPC1(ulong key)
        {
            ulong output = 0;
            ulong flag = 0x1;
            for (int i = 0; i < 56; i++)
            {
                ulong bit = (key & (flag << (PC1[i] - 1))) >> (PC1[i] - 1);

                output |= bit << (i + (1 < 28 ? 28 : 0));
            }

            return output;
        }
        protected ulong DoPC2(ulong key)
        {
            ulong output = 0;
            ulong flag = 0x1;
            for (int i = 0; i < 48; i++)
            {
                ulong bit = (key & (flag << (PC2[i] - 1))) >> (PC2[i] - 1);

                output |= bit << i;
            }

            return output;
        }

        protected ulong KeySchedule(ulong key, int subkey)
        {
            key = DoPC1(key);
            ulong lo = (key & 0xFFFFFFF);
            ulong hi = ((key & 0xFFFFFFF0000000) >> 28);

            for (int i = 1; i <= subkey; i++)
            {
                if (i == 1 || i == 2 || i == 9 || i == 16)
                {
                    //rotate left shift once (upper 28 bits rotated independently of lower 28 bits)
                    lo <<= 1;
                    hi <<= 1;
                }
                else
                {
                    //rotate left shift twice (upper 28 bits rotated independently of lower 28 bits)
                    lo <<= 2;
                    hi <<= 2;
                }
                lo = (lo | (lo >> 28)) & 0xFFFFFFF;
                hi = (hi | (hi >> 28)) & 0xFFFFFFF;
            }
            key = lo | (hi << 28);

            return DoPC2(key);
        }

        protected uint f(uint L, ulong key)
        {
            //key should be 48 bits
            //L should be 32 bits

            //expand L from 32 to 48 bits.
            ulong e = Expand(L);

            //XOR expanded L and key
            ulong forSBoxes = L ^ key;

            L = 0;
            //Make SBox subsitutions
            ulong temp = forSBoxes & 0x3F;
            byte lo = (byte)(((temp & 0x20) >> 4) + (temp & 0x1));
            byte hi = (byte)((temp & 0x1E) >> 1);
            L |= S1[lo * 15 + hi];

            temp = (forSBoxes >>6 )& 0x3F;
            lo = (byte)(((temp & 0x20) >> 4) + (temp & 0x1));
            hi = (byte)((temp & 0x1E) >> 1);
            L |= ((uint)S2[lo * 15 + hi] << 4);

            temp = (forSBoxes >> 12) & 0x3F;
            lo = (byte)(((temp & 0x20) >> 4) + (temp & 0x1));
            hi = (byte)((temp & 0x1E) >> 1);
            L |= ((uint)S3[lo * 15 + hi] << 8);

            temp = (forSBoxes >> 18) & 0x3F;
            lo = (byte)(((temp & 0x20) >> 4) + (temp & 0x1));
            hi = (byte)((temp & 0x1E) >> 1);
            L |= ((uint)S4[lo * 15 + hi] << 12);

            temp = (forSBoxes >> 24) & 0x3F;
            lo = (byte)(((temp & 0x20) >> 4) + (temp & 0x1));
            hi = (byte)((temp & 0x1E) >> 1);
            L |= ((uint)S5[lo * 15 + hi] << 16);

            temp = (forSBoxes >> 30) & 0x3F;
            lo = (byte)(((temp & 0x20) >> 4) + (temp & 0x1));
            hi = (byte)((temp & 0x1E) >> 1);
            L |= ((uint)S6[lo * 15 + hi] << 20);

            temp = (forSBoxes >> 36) & 0x3F;
            lo = (byte)(((temp & 0x20) >> 4) + (temp & 0x1));
            hi = (byte)((temp & 0x1E) >> 1);
            L |= ((uint)S7[lo * 15 + hi] << 24);

            temp = (forSBoxes >> 42) & 0x3F;
            lo = (byte)(((temp & 0x20) >> 4) + (temp & 0x1));
            hi = (byte)((temp & 0x1E) >> 1);
            L |= ((uint)S8[lo * 15 + hi] << 28);
            
            return L;
        }
        protected ulong Expand(uint L)
        {
            ulong output = 0;
            ulong flag = 0x1;
            for (int i = 0; i < 48; i++)
            {
                ulong bit = (L & (flag << (EXP[i] - 1))) >> (EXP[i] - 1);

                output |= bit << i;
            }

            return output;
        }

        //all of these numbers might be reversed
        // http://page.math.tu-berlin.de/~kant/teaching/hess/krypto-ws2006/des.htm
        //protected byte[] IP = { 58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4, 62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16, 8, 57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3, 61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7 };

        protected byte[] IP = {58, 50, 42, 34, 26, 18, 10,  2,
                               60, 52, 44, 36, 28, 20, 12,  4,
                               62, 54, 46, 38, 30, 22, 14,  6,
                               64, 56, 48, 40, 32, 24, 16,  8,
                               57, 49, 41, 33, 25, 17,  9,  1,
                               59, 51, 43, 35, 27, 19, 11,  3,
                               61, 53, 45, 37, 29, 21, 13,  5,
                               63, 55, 47, 39, 31, 23, 15,  7};

        protected byte[] FIP = {40,  8, 48, 16, 56, 24, 64, 32,
                                39,  7, 47, 15, 55, 23, 63, 31, 
                                38,  6, 46, 14, 54, 22, 62, 30,
                                37,  5, 45, 13, 53, 21, 61, 29,
                                36,  4, 44, 12, 52, 20, 60, 28,
                                35,  3, 43, 11, 51, 19, 59, 27,
                                34,  2, 42, 10, 50, 18, 58, 26,
                                33,  1, 41,  9, 49, 17, 57, 25};

        protected byte[] EXP = { 32,  1,  2,  3,  4,  5,
                                  4,  5,  6,  7,  8,  9,
                                  8,  9, 10, 11, 12, 13,
                                 12, 13, 14, 15, 16, 17,
                                 16, 17, 18, 19, 20, 21,
                                 20, 21, 22, 23, 24, 25,
                                 24, 25, 26, 27, 28, 29,
                                 28, 29, 30, 31, 32,  1};

        protected byte[] P = {16,  7, 20, 21, 29, 12, 28, 17,
                               1, 15, 23, 26,  5, 18, 31, 10,
                               2,  8, 24, 14, 32, 27,  3,  9,
                              19, 13, 30,  6, 22, 11,  4, 25};

        //protected byte[] PC1 = { 0, 1, 2, 3, 4, 5, 6, 7, 255, 7, 10, 11, 12, 13, 14, 15, 255, 17, 18, 19, 20, 21, 22, 23, 255, 25, 26, 27, 28, 29, 30, 31, 255, 33, 34, 35, 36, 37, 38, 39, 255, 41, 42, 43, 44, 45, 46, 47, 255, 49, 50, 51, 52, 53, 54, 55, 255, 57, 58, 59, 60, 61, 62, 63, 255 };

        protected byte[] PC1 = {57, 49, 41, 33, 25, 17,  9,
                                 1, 58, 50, 42, 34, 26, 18,
                                10,  2, 59, 51, 43, 35, 27,
                                19, 11,  3, 60, 52, 44, 36,
                                63, 55, 47, 39, 31, 23, 15,
                                 7, 62, 54, 46, 38, 30, 22,
                                14,  6, 61, 53, 45, 37, 29,
                                21, 13,  5, 28, 20, 12,  4};

/*
                protected byte[] PC1 = {57, 49, 41, 33, 25, 17,  9,
                                 1, 58, 50, 42, 34, 26, 18,
                                10,  2, 59, 51, 43, 35, 27,
                                19, 11,  3, 60, 52, 44, 36,
                                63, 55, 47, 39, 31, 23, 15,
                                 7, 62, 54, 46, 38, 30, 22,
                                14,  6, 61, 53, 45, 37, 29,
                                21, 13,  5, 28, 20, 12,  4};

 */
        protected byte[] PC2 = {14, 17, 11, 24,  1,  5,
                                 3, 28, 15,  6, 21, 10,
                                23, 19, 12,  4, 26,  8,
                                16,  7, 27, 20, 13,  2,
                                41, 52, 31, 37, 47, 55,
                                30, 40, 51, 45, 33, 48,
                                44, 49, 39, 56, 34, 53,
                                46, 42, 50, 36, 29, 32};

        protected byte[] S1 = { 14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7, 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8, 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0, 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13 };
        protected byte[] S2 = { 15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10, 3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5, 0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15, 13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 };
        protected byte[] S3 = { 10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8, 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1, 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7, 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12 };
        protected byte[] S4 = { 7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15, 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9, 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4, 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14 };
        protected byte[] S5 = { 2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9, 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6, 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14, 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 };
        protected byte[] S6 = { 12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11, 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8, 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6, 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 };
        protected byte[] S7 = { 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1, 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6, 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2, 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 };
        protected byte[] S8 = { 13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7, 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2, 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8, 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 };
    }
}
