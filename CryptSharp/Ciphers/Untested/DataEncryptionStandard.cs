using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers
{
    public enum Mode { ElectronicCodeBook, ChainBlockCoding/*, CipherFeedback*/ };
    public class DES
    {
        public DES()
        {
        }

        //64-bits, 8 bytes
        //High order bit is used for parity checking and not used in the actual encryption
        //this reduces the actual key to 56 bits
        public byte[] Key { get; set; }

        public byte[] IV { get; set; }

        public Mode Mode { get; set; } = Mode.ElectronicCodeBook;

        public byte[] Encrypt(byte[] clearText)
        {
            //example http://page.math.tu-berlin.de/~kant/teaching/hess/krypto-ws2006/des.htm

            byte[] cipherText = new byte[clearText.Length + clearText.Length % 8];

            //ulong key = 0x0101010100000000;
            //ulong key = 0x0202020200000000;
            ulong key = BitConverter.ToUInt64(Key, 0);//  0x133457799BBCDFF1;
            ulong message = 0;// 0x0123456789ABCDEF;
            ulong prevmessage = BitConverter.ToUInt64(IV, 0);

            ulong[] subkeys = new ulong[17];
            uint[] C = new uint[17];
            uint[] D = new uint[17];

            //get initial key permutation, PC1
            subkeys[0] = DoPC1(key);

            //permutate the Lo and Hi parts of the PC1 key
            for (int i = 0; i <= 16; i++)
            {
                if (i == 0)
                {
                    C[0] = (uint)((subkeys[0] & 0x00FFFFFFF0000000) >> 28);
                    D[0] = (uint)(subkeys[0] & 0x000000000FFFFFFF);
                }
                else if (i == 1 || i == 2 || i == 16 || i == 9)
                {
                    C[i] = (C[i - 1] << 1 | C[i - 1] >> 27) & 0x00FFFFFFF;
                    D[i] = (D[i - 1] << 1 | D[i - 1] >> 27) & 0x00FFFFFFF;
                }
                else
                {
                    C[i] = (C[i - 1] << 2 | C[i - 1] >> 26) & 0x00FFFFFFF;
                    D[i] = (D[i - 1] << 2 | D[i - 1] >> 26) & 0x00FFFFFFF;
                }

            }

            //permutate the keys with PC2, putting the Lo and Hi parts back together again
            for (int i = 1; i <= 16; i++)
            {
                subkeys[i] = DoPC2((((ulong)C[i]) << 28) | (ulong)D[i]);
            }

            //Do initial permutation (IP) on 64 bits of data

            for (int index = 0; index < clearText.Length; index += 8)
            {
                message = BitConverter.ToUInt64(clearText, index);

                if(Mode == Mode.ChainBlockCoding)
                {
                    message = message ^ prevmessage;
                }

                ulong ipmess = DoIP(message);

                //previous L and R
                ulong L = ((ipmess & 0xFFFFFFFF00000000) >> 32);
                ulong R = (ipmess & 0x00000000FFFFFFFF);

                ulong Ln = 0;//next L
                ulong Rn = 0;//next R

                //Do the feistel network
                for (int i = 1; i <= 16; i++)
                {
                    Ln = R;

                    Rn = L ^ f((uint)R, subkeys[i]);

                    R = Rn;
                    L = Ln;
                }

                message = (R << 32) | L;

                message = DoFIP(message);

                Array.Copy(BitConverter.GetBytes(message), 0, cipherText, index, 8);

                prevmessage = message;
            }

            return cipherText;
        }

        public byte[] Decrypt(byte[] cipherText)
        {
            //example http://page.math.tu-berlin.de/~kant/teaching/hess/krypto-ws2006/des.htm

            byte[] clearText = new byte[cipherText.Length + cipherText.Length % 8];

            //ulong key = 0x0101010100000000;
            //ulong key = 0x0202020200000000;
            ulong key = BitConverter.ToUInt64(Key, 0);//  0x133457799BBCDFF1;
            ulong message = 0;// 0x0123456789ABCDEF;
            ulong prevmessage = BitConverter.ToUInt64(IV, 0);

            ulong[] subkeys = new ulong[17];
            uint[] C = new uint[17];
            uint[] D = new uint[17];

            //get initial key permutation, PC1
            subkeys[0] = DoPC1(key);

            //permutate the Lo and Hi parts of the PC1 key
            for (int i = 0; i <= 16; i++)
            {
                if (i == 0)
                {
                    C[0] = (uint)((subkeys[0] & 0x00FFFFFFF0000000) >> 28);
                    D[0] = (uint)(subkeys[0] & 0x000000000FFFFFFF);
                }
                else if (i == 1 || i == 2 || i == 16 || i == 9)
                {
                    C[i] = (C[i - 1] << 1 | C[i - 1] >> 27) & 0x00FFFFFFF;
                    D[i] = (D[i - 1] << 1 | D[i - 1] >> 27) & 0x00FFFFFFF;
                }
                else
                {
                    C[i] = (C[i - 1] << 2 | C[i - 1] >> 26) & 0x00FFFFFFF;
                    D[i] = (D[i - 1] << 2 | D[i - 1] >> 26) & 0x00FFFFFFF;
                }

            }

            //permutate the keys with PC2, putting the Lo and Hi parts back together again
            for (int i = 1; i <= 16; i++)
            {
                subkeys[i] = DoPC2((((ulong)C[i]) << 28) | (ulong)D[i]);
            }

            //Do initial permutation (IP) on 64 bits of data

            for (int index = 0; index < cipherText.Length; index += 8)
            {
                message = BitConverter.ToUInt64(cipherText, index);

                ulong ipmess = DoIP(message);

                //previous L and R
                ulong L = ((ipmess & 0xFFFFFFFF00000000) >> 32);
                ulong R = (ipmess & 0x00000000FFFFFFFF);

                ulong Ln = 0;//next L
                ulong Rn = 0;//next R

                //Do the feistel network
                for (int i = 1; i <= 16; i++)
                {
                    Ln = R;

                    Rn = L ^ f((uint)R, subkeys[17 - i]);

                    R = Rn;
                    L = Ln;
                }

                message = (R << 32) | L;

                message = DoFIP(message);


                if (Mode == Mode.ChainBlockCoding)
                {
                    message = message ^ prevmessage;
                }

                Array.Copy(BitConverter.GetBytes(message), 0, clearText, index, 8);

                prevmessage = message;
            }

            return clearText;
        }

        protected ulong DoPC1(ulong key)
        {
            ulong output = 0;
            for (int i = 0; i < PC1.Length; i++)
            {
                output |= ((key >> (64 - PC1[i])) & 0x01) << (55 - i);
            }

            return output;
        }

        protected ulong DoPC2(ulong key)
        {
            ulong output = 0;
            for (int i = 0; i < PC2.Length; i++)
            {
                output |= ((key >> (56 - PC2[i])) & 0x01) << (47 - i);
            }

            return output;
        }

        protected ulong DoIP(ulong message)
        {
            ulong output = 0;
            for (int i = 0; i < IP.Length; i++)
            {
                output |= ((message >> (64 - IP[i])) & 0x01) << (63 - i);
            }

            return output;
        }

        protected ulong DoFIP(ulong message)
        {
            ulong output = 0;
            for (int i = 0; i < FIP.Length; i++)
            {
                output |= ((message >> (64 - FIP[i])) & 0x01) << (63 - i);
            }

            return output;
        }

        protected ulong DoEXP(ulong message)
        {
            ulong output = 0;
            for (int i = 0; i < EXP.Length; i++)
            {
                output |= ((message >> (32 - EXP[i])) & 0x01) << (47 - i);
            }

            return output;
        }

        protected uint DoP(uint message)
        {
            uint output = 0;
            for (int i = 0; i < P.Length; i++)
            {
                output |= ((message >> (32 - P[i])) & 0x01) << (31 - i);
            }

            return output;
        }

        protected uint f(uint L, ulong key)
        {
            //key should be 48 bits
            //L should be 32 bits

            //expand L from 32 to 48 bits.
            ulong e = DoEXP(L);

            //XOR expanded e and key
            ulong forSBoxes = e ^ key;

            L = 0;
            ulong temp = 0;
            //Make SBox subsitutions

            //temp = forSBoxes & 0x3F;
            temp = (forSBoxes >> 42) & 0x3F;
            byte lo = (byte)(((temp & 0x20) >> 4) + (temp & 0x1));
            byte hi = (byte)((temp & 0x1E) >> 1);
            L |= (uint)(S1[lo * 16 + hi] << 28);

            //temp = (forSBoxes >>6 )& 0x3F;
            temp = (forSBoxes >> 36) & 0x3F;
            lo = (byte)(((temp & 0x20) >> 4) + (temp & 0x1));
            hi = (byte)((temp & 0x1E) >> 1);
            L |= ((uint)S2[lo * 16 + hi] << 24);

            temp = (forSBoxes >> 30) & 0x3F;
            //temp = (forSBoxes >> 12) & 0x3F;
            lo = (byte)(((temp & 0x20) >> 4) + (temp & 0x1));
            hi = (byte)((temp & 0x1E) >> 1);
            L |= ((uint)S3[lo * 16 + hi] << 20);

            temp = (forSBoxes >> 24) & 0x3F;
            //temp = (forSBoxes >> 18) & 0x3F;
            lo = (byte)(((temp & 0x20) >> 4) + (temp & 0x1));
            hi = (byte)((temp & 0x1E) >> 1);
            L |= ((uint)S4[lo * 16 + hi] << 16);

            temp = (forSBoxes >> 18) & 0x3F;
            //temp = (forSBoxes >> 24) & 0x3F;
            lo = (byte)(((temp & 0x20) >> 4) + (temp & 0x1));
            hi = (byte)((temp & 0x1E) >> 1);
            L |= ((uint)S5[lo * 16 + hi] << 12);

            //temp = (forSBoxes >> 30) & 0x3F;
            temp = (forSBoxes >> 12) & 0x3F;
            lo = (byte)(((temp & 0x20) >> 4) + (temp & 0x1));
            hi = (byte)((temp & 0x1E) >> 1);
            L |= ((uint)S6[lo * 16 + hi] << 8);

            temp = (forSBoxes >> 6) & 0x3F;
            //temp = (forSBoxes >> 36) & 0x3F;
            lo = (byte)(((temp & 0x20) >> 4) + (temp & 0x1));
            hi = (byte)((temp & 0x1E) >> 1);
            L |= ((uint)S7[lo * 16 + hi] << 4);

            temp = forSBoxes & 0x3F;
            //temp = (forSBoxes >> 42) & 0x3F;
            lo = (byte)(((temp & 0x20) >> 4) + (temp & 0x1));
            hi = (byte)((temp & 0x1E) >> 1);
            L |= ((uint)S8[lo * 16 + hi]);
            
            return DoP(L);
        }

        #region Static Tables
        // http://page.math.tu-berlin.de/~kant/teaching/hess/krypto-ws2006/des.htm

        //Initial Permutation
        protected byte[] IP = {58, 50, 42, 34, 26, 18, 10,  2,
                               60, 52, 44, 36, 28, 20, 12,  4,
                               62, 54, 46, 38, 30, 22, 14,  6,
                               64, 56, 48, 40, 32, 24, 16,  8,
                               57, 49, 41, 33, 25, 17,  9,  1,
                               59, 51, 43, 35, 27, 19, 11,  3,
                               61, 53, 45, 37, 29, 21, 13,  5,
                               63, 55, 47, 39, 31, 23, 15,  7};

        //Inverse of IP
        protected byte[] FIP = {40,  8, 48, 16, 56, 24, 64, 32,
                                39,  7, 47, 15, 55, 23, 63, 31, 
                                38,  6, 46, 14, 54, 22, 62, 30,
                                37,  5, 45, 13, 53, 21, 61, 29,
                                36,  4, 44, 12, 52, 20, 60, 28,
                                35,  3, 43, 11, 51, 19, 59, 27,
                                34,  2, 42, 10, 50, 18, 58, 26,
                                33,  1, 41,  9, 49, 17, 57, 25};

        //E Bit-Selection table
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

        //Permutation Choice 1
        protected byte[] PC1 = {57, 49, 41, 33, 25, 17,  9,
                                 1, 58, 50, 42, 34, 26, 18,
                                10,  2, 59, 51, 43, 35, 27,
                                19, 11,  3, 60, 52, 44, 36,
                                63, 55, 47, 39, 31, 23, 15,
                                 7, 62, 54, 46, 38, 30, 22,
                                14,  6, 61, 53, 45, 37, 29,
                                21, 13,  5, 28, 20, 12,  4};

        //Permutation Choice 2
        protected byte[] PC2 = {14, 17, 11, 24,  1,  5,
                                 3, 28, 15,  6, 21, 10,
                                23, 19, 12,  4, 26,  8,
                                16,  7, 27, 20, 13,  2,
                                41, 52, 31, 37, 47, 55,
                                30, 40, 51, 45, 33, 48,
                                44, 49, 39, 56, 34, 53,
                                46, 42, 50, 36, 29, 32};


        //S-Boxes
        protected byte[] S1 = { 14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7, 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8, 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0, 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13 };
        protected byte[] S2 = { 15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10, 3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5, 0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15, 13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 };
        protected byte[] S3 = { 10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8, 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1, 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7, 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12 };
        protected byte[] S4 = { 7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15, 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9, 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4, 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14 };
        protected byte[] S5 = { 2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9, 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6, 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14, 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 };
        protected byte[] S6 = { 12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11, 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8, 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6, 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 };
        protected byte[] S7 = { 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1, 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6, 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2, 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 };
        protected byte[] S8 = { 13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7, 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2, 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8, 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 };
        #endregion
    }
}
