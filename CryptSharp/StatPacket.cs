using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp
{
    public class StatPacket
    {
        //across entire cipher
        public double StandardDeviation { get; set; }
        public int N { get; set; }
        public double Variance { get; set; }
        public double Mean { get; set; }
        public double MeanSquare { get; set; }
        
        //per letter stats
        public StatPacket[] Alphabet { get; set; }
    }
}
