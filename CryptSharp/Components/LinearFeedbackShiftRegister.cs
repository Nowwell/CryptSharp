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
        /// Assumes 16 registers
        /// </summary>
        /// <returns></returns>
        public byte GaloisShift()
        {
            long leastSigBit = (long)(Registers & 0x00000001);
            Registers >>= 1;
            Registers ^= (ulong)(-leastSigBit) & 0xB400;

            return (byte)leastSigBit;
        }

        public byte Shift()
        {


            return 0;
        }

    }
}
