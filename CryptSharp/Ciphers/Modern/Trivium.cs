using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.Modern
{
    public class Trivium : IModernCipher
    {
        //http://www.ecrypt.eu.org/stream/p3ciphers/trivium/trivium_p3.pdf

        public Trivium(byte[] key, byte[] iv)
        {
            IV = iv;
            Key = key;

            SetupState(key, iv);
        }

        byte[] state = new byte[36];
        public byte[] IV { get; private set; }
        public byte[] Key { get; private set; }

        public void ResetState()
        {
            Array.Clear(state, 0, state.Length);
            SetupState(Key, IV);
        }
        public byte[] RequestKeyStreamBits(uint N)
        {
            if (N > Math.Pow(2, 64)) throw new Exception("Requested Key Stream bits must be N <= 2^64");

            byte[] keyStream = new byte[(int)Math.Ceiling(N / 8.0)];
            byte t1 = 0;
            byte t2 = 0;
            byte t3 = 0;
            for (int i = 0; i < N; i++)
            {
                t1 = (byte)(BitAtPosition(66) ^ BitAtPosition(93));
                t2 = (byte)(BitAtPosition(162) ^ BitAtPosition(177));
                t3 = (byte)(BitAtPosition(243) ^ BitAtPosition(288));

                keyStream[i / 8] |= (byte)((t1 ^ t2 ^ t3) << (i - (i / 8) * 8));

                t1 = (byte)(t1 ^ (BitAtPosition(91) & BitAtPosition(92)) ^ BitAtPosition(171));
                t2 = (byte)(t2 ^ (BitAtPosition(175) & BitAtPosition(176)) ^ BitAtPosition(264));
                t3 = (byte)(t3 ^ (BitAtPosition(286) & BitAtPosition(287)) ^ BitAtPosition(69));

                state = ShiftLeft(state, t3);
                state[11] = (byte)((state[11] & 0xEF) | t1 << 4);
                state[22] = (byte)((state[12] & 0xFE) | t2 << 0);
                //state[11] |= (byte)(t1 << 4);
                //state[22] |= (byte)(t2 << 0);
            }

            return keyStream;
        }

        protected byte[] ShiftLeft(byte[] state, byte newBit)
        {
            byte register = (byte)((state[0] & 0x80) >> 7);
            state[0] = (byte)(state[0] << 1);
            state[0] |= newBit;
            for (int i = 1; i < state.Length - 1; i++)
            {
                byte newregister = (byte)((state[i] & 0x80) >> 7);

                state[i] = (byte)(state[i] << 1);
                state[i] |= register;

                register = newregister;
            }

            return state;
        }
        protected byte[] SetupState(byte[] key, byte[] iv)
        {
            /*
            (s1, s2, . . . , s93) ← (K1, . . . , K80, 0, . . . , 0)
            (s94, s95, . . . , s177) ← (IV1, . . . , IV80, 0, . . . , 0)
            (s178, s279, . . . , s288) ← (0, . . . , 0, 1, 1, 1)
            for i = 1 to 4 · 288 do
                t1 ← s66 + s91 · s92 + s93 + s171
                t2 ← s162 + s175 · s176 + s177 + s264
                t3 ← s243 + s286 · s287 + s288 + s69
                (s1, s2, . . . , s93) ← (t3, s1, . . . , s92)
                (s94, s95, . . . , s177) ← (t1, s94, . . . , s176)
                (s178, s279, . . . , s288) ← (t2, s178, . . . , s287)
            end for
            */
            Array.Copy(Key, state, 10);
            //Array.Copy(iv, 0, state, 11, 10);//TODO Fix

            state[11] = (byte)(iv[0] << 4);
            state[12] = (byte)((iv[1] << 4) | iv[0] >> 4);
            state[13] = (byte)((iv[2] << 4) | iv[1] >> 4);
            state[14] = (byte)((iv[3] << 4) | iv[2] >> 4);
            state[15] = (byte)((iv[4] << 4) | iv[3] >> 4);
            state[16] = (byte)((iv[5] << 4) | iv[4] >> 4);
            state[17] = (byte)((iv[6] << 4) | iv[5] >> 4);
            state[18] = (byte)((iv[7] << 4) | iv[6] >> 4);
            state[19] = (byte)((iv[8] << 4) | iv[7] >> 4);
            state[20] = (byte)((iv[9] << 4) | iv[8] >> 4);
            state[21] = (byte)((iv[9] >> 4));

            state[35] = 0xE0;

            byte t1 = 0;
            byte t2 = 0;
            byte t3 = 0;

            for (int i = 0; i < 4 * 288; i++)
            {
                t1 = (byte)(BitAtPosition(66) ^ (BitAtPosition(91) & BitAtPosition(92)) ^ BitAtPosition(66) ^ BitAtPosition(171));
                t2 = (byte)(BitAtPosition(162) ^ (BitAtPosition(175) & BitAtPosition(176)) ^ BitAtPosition(177) ^ BitAtPosition(264));
                t3 = (byte)(BitAtPosition(243) ^ (BitAtPosition(286) & BitAtPosition(287)) ^ BitAtPosition(288) ^ BitAtPosition(69));

                state = ShiftLeft(state, t3);
                state[11] = (byte)((state[11] & 0xEF) | t1 << 4);
                state[22] = (byte)((state[12] & 0xFE) | t2 << 0);
            }

            return state;
        }

        protected int IndexFromBitPosition(int pos)
        {
            return (int)Math.Floor((pos - 1.0) / 8.0);
        }
        protected int ShiftFromBitPosition(int pos)
        {
            return (pos - 1) % 8;
        }
        protected byte BitAtPosition(int pos)
        {
            return (byte)((state[IndexFromBitPosition(pos)] >> ShiftFromBitPosition(pos)) & 0x01);
        }
        protected int PositionFromByteAndOffset(int b, int o)
        {
            return b * 8 + o + 1;
        }

        public byte[] Encrypt(byte[] clearText)
        {
            byte[] streamKey = RequestKeyStreamBits((uint)(8 * clearText.Length));

            byte[] cipher = new byte[clearText.Length];
            for(int i = 0; i<clearText.Length; i++)
            {
                cipher[i] = (byte)(clearText[i] ^ streamKey[i]);
            }

            return cipher;
        }

        public byte[] Encrypt(string clearText, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
        }

        public byte[] Decrypt(byte[] cipherText)
        {
            byte[] streamKey = RequestKeyStreamBits((uint)(8 * cipherText.Length));

            byte[] clear = new byte[cipherText.Length];
            for (int i = 0; i < cipherText.Length; i++)
            {
                clear[i] = (byte)(cipherText[i] ^ streamKey[i]);
            }

            return clear;
        }

        public byte[] Decrypt(string cipherText, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
        }

        public void DecryptFile(string clearTextFilename, string cipherTextFilename, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
        }
    }
}
