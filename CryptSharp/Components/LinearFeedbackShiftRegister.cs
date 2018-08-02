using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Components
{
    public class LinearFeedbackShiftRegister
    {
        public LinearFeedbackShiftRegister()
        {
            Registers = 0x8080808;
            NumberOfRegisters = 16;
        }
        public LinearFeedbackShiftRegister(ulong initialState, int numberOfRegisters)
        {
            Registers = initialState;
            NumberOfRegisters = numberOfRegisters;
        }



        ulong Registers { get; set; }
        int NumberOfRegisters { get; set; }
        ulong RegistersToXOR { get; set; }

        /// <summary>
        /// Assumes 16 registers
        /// </summary>
        /// <returns></returns>
        public byte FibonacciShift()
        {
            ulong ret = ((Registers ^ (Registers >> 2) ^ (Registers >> 3) ^ (Registers >> 5)) & 1);

            Registers = (Registers >> 1) | (ret << 15);

            return (byte)ret;
        }

        /// <summary>
        /// Assumes 16 registers.
        /// </summary>
        /// <returns></returns>
        public byte GaloisShift()
        {
            long leastSigBit = (long)(Registers & 0x00000001);
            Registers >>= 1;
            Registers ^= (ulong)(-leastSigBit) & 0xB400;

            return (byte)leastSigBit;
        }

        /// <summary>
        /// Shift.  Algorithm handles at most 64 registers
        /// </summary>
        /// <param name="polyomial">list of exponents that correspond to bit positions</param>
        /// <returns></returns>
        public byte Shift(byte[] polyomial)
        {
            ulong ret = Registers >> polyomial[0];
            for (int i = 1; i < polyomial.Length; i++)
            {
                ret = ret ^ (Registers >> polyomial[i]);
            }
            ret = ret & 1;

            Registers = (Registers >> 1) | (ret << (NumberOfRegisters - 1));

            return (byte)ret;
        }

    }
}
