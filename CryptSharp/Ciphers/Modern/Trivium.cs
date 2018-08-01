using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers
{
    public class Trivium
    {
        //http://www.ecrypt.eu.org/stream/p3ciphers/trivium/trivium_p3.pdf

        byte[] state = new byte[36];
        byte[] IV = new byte[10];
        byte[] key = new byte[10];

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

        public byte[] SetupState(byte[] key, byte[] iv)
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
            end for            */
            Array.Copy(key, state, 10);
            Array.Copy(iv, 0, state, 11, 10);//TODO Fix
            state[35] = 0xE0;

            byte t1 = 0;
            byte t2 = 0;
            byte t3 = 0;

            for (int i = 0; i < 4 * 288; i++)
            {


                state = ShiftLeft(state, t3);
                state[11] |= (byte)(t1 << 5);
                state[22] |= (byte)(t2 << 2);
            }



            return default(byte[]);
        }

        public byte[] RequestKeyStreamBits(int N)
        {
            /*
            for i = 1 to N do
                t1 ← s66 + s93
                t2 ← s162 + s177
                t3 ← s243 + s288
                zi ← t1 + t2 + t3
                t1 ← t1 + s91 · s92 + s171
                t2 ← t2 + s175 · s176 + s264
                t3 ← t3 + s286 · s287 + s69
                (s1, s2, . . . , s93) ← (t3, s1, . . . , s92)
                (s94, s95, . . . , s177) ← (t1, s94, . . . , s176)
                (s178, s279, . . . , s288) ← (t2, s178, . . . , s287)
            end for            */

            if (N > Math.Pow(2, 64)) throw new Exception("Requested Key Stream bits must be N <= 2^64");

            byte[] keyStream = new byte[(int)Math.Ceiling(N / 8.0)];
            byte t1 = 0;
            byte t2 = 0;
            byte t3 = 0;
            for(int i=0; i<N; i++)
            {
                t1 = (byte)(((state[7] >> 2) & 0x01) ^ ((state[11] >> 5) & 0x01));

                keyStream[i / 8] |= (byte)((t1 ^ t2 ^ t3) << (i - (i / 8) * 8));

                t1 = (byte)(t1 ^ (((state[11] >> 3) & 0x01) & ((state[11] >> 4) & 0x01)) ^ ((state[20] >> 3) & 0x01));
                t2 = (byte)(t2 ^ (((state[22] >> 0) & 0x01) & ((state[22] >> 1) & 0x01)) ^ ((state[32] >> 0) & 0x01));
                t3 = (byte)(t3 ^ (((state[35] >> 6) & 0x01) & ((state[35] >> 7) & 0x01)) ^ ((state[ 7] >> 6) & 0x01));

                state = ShiftLeft(state, t3);
                state[11] |= (byte)(t1 << 5);
                state[22] |= (byte)(t2 << 2);
            }

            return keyStream;
        }

    }
}
