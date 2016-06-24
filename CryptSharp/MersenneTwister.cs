using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MersenneTwister
{
    public class Random
    {
        const int N = 624;
        const int M = 397;
        const ulong MATRIX_A = 0x9908b0dfUL;   /* constant vector a */
        const ulong UPPER_MASK = 0x80000000UL; /* most significant w-r bits */
        const ulong LOWER_MASK = 0x7fffffffUL; /* least significant r bits */
        static ulong[] mag01 = new ulong[] { 0x0UL, MATRIX_A };

        private ulong[] mt = new ulong[N];
        int mti;
        ulong numberOfRandomsGenerated;

        public Random()
        {
            mti = N + 1;
        }
        ~Random()
        {
        }

        public void InitGenRand(ulong s)
        {
            mt[0]= s & 0xffffffffUL;
            for (mti=1; mti<N; mti++) {
                mt[mti] = 1812433253UL * ((mt[mti-1] ^ (mt[mti-1] >> 30)) + (ulong)mti);
                /* See Knuth TAOCP Vol2. 3rd Ed. P.106 for multiplier. */
                /* In the previous versions, MSBs of the seed affect   */
                /* only MSBs of the array mt[].                        */
                /* 2002/01/09 modified by Makoto Matsumoto             */
                mt[mti] = mt[mti] & 0xffffffffUL;
                /* for >32 bit machines */
            }
            numberOfRandomsGenerated = 0;
        }

        /* initialize by an array with array-length */
        /* init_key is the array for initializing keys */
        /* key_length is its length */
        public void InitByArray(ulong[] initKey, int keyLength)
        {
            int i, j, k;
            InitGenRand(19650218UL);
            i=1; j=0;
            k = (N>keyLength ? N : keyLength);
            for (; k>0; k--)
            {
                mt[i] = (mt[i] ^ ((mt[i-1] ^ (mt[i-1] >> 30)) * 1664525UL))
                  + initKey[j] + (ulong)j; /* non linear */
                mt[i] &= 0xffffffffUL; /* for WORDSIZE > 32 machines */
                i++; j++;
                if (i>=N) { mt[0] = mt[N-1]; i=1; }
                if (j>=keyLength) j=0;
            }
            for (k=N-1; k>0; k--) {
                mt[i] = (mt[i] ^ ((mt[i-1] ^ (mt[i-1] >> 30)) * 1566083941UL))
                  - (ulong)i; /* non linear */
                mt[i] &= 0xffffffffUL; /* for WORDSIZE > 32 machines */
                i++;
                if (i>=N) { mt[0] = mt[N-1]; i=1; }
            }

            mt[0] = 0x80000000UL; /* MSB is 1; assuring non-zero initial array */

            numberOfRandomsGenerated = 0;
        }

        /// <summary>
        /// Generates a random 64 bit number on the interval [0,0xffffffffffffffff]
        /// </summary>
        /// <returns>64 bit random</returns>
        public ulong Next()
        {
            return NextUIntAsUlong() + (NextUIntAsUlong() << 32);
        }
        /// <summary>
        /// Generates a 32 bit number on the interval [0,0xffffffff]
        /// </summary>
        /// <returns>32 bit unsigned random</returns>
        public uint NextUInt()
        {
            ulong y;

            /* mag01[x] = x * MATRIX_A  for x=0,1 */

            if (mti >= N)
            { /* generate N words at one time */
                int kk;

                if (mti == N + 1)   /* if init_genrand() has not been called, */
                    InitGenRand(5489UL); /* a default initial seed is used */

                for (kk = 0; kk < N - M; kk++)
                {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk + M] ^ (y >> 1) ^ mag01[y & 0x1UL];
                }
                for (; kk < N - 1; kk++)
                {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk + (M - N)] ^ (y >> 1) ^ mag01[y & 0x1UL];
                }
                y = (mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK);
                mt[N - 1] = mt[M - 1] ^ (y >> 1) ^ mag01[y & 0x1UL];

                mti = 0;
            }

            y = mt[mti++];

            /* Tempering */
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680UL;
            y ^= (y << 15) & 0xefc60000UL;
            y ^= (y >> 18);

            numberOfRandomsGenerated++;
            return (uint)y;
        }
        protected ulong NextUIntAsUlong()
        {
            ulong y;

            /* mag01[x] = x * MATRIX_A  for x=0,1 */

            if (mti >= N)
            { /* generate N words at one time */
                int kk;

                if (mti == N + 1)   /* if init_genrand() has not been called, */
                    InitGenRand(5489UL); /* a default initial seed is used */

                for (kk = 0; kk < N - M; kk++)
                {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk + M] ^ (y >> 1) ^ mag01[y & 0x1UL];
                }
                for (; kk < N - 1; kk++)
                {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk + (M - N)] ^ (y >> 1) ^ mag01[y & 0x1UL];
                }
                y = (mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK);
                mt[N - 1] = mt[M - 1] ^ (y >> 1) ^ mag01[y & 0x1UL];

                mti = 0;
            }

            y = mt[mti++];

            /* Tempering */
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680UL;
            y ^= (y << 15) & 0xefc60000UL;
            y ^= (y >> 18);

            numberOfRandomsGenerated++;
            return y;
        }

        /// <summary>
        /// Generates a 32 bit number on the interval [0,0x7fffffff]
        /// </summary>
        /// <returns>32 bit unsigned random</returns>
        public long NextUInt31()
        {
            return (long)(NextUIntAsUlong() >> 1);
        }
        
        /// <summary>
        /// Generates either a 32 or 64 bit number stored in a ulong
        /// </summary>
        /// <param name="longOutput">If true, theoutput is a ulong, if false a uint</param>
        /// <returns>a 32 or 64 bit random</returns>
        public ulong Next(bool longOutput)
        {
            if (longOutput)
            {
                return NextUIntAsUlong() + (NextUIntAsUlong() << 32);
            }
            else
            {
                return NextUIntAsUlong();
            }
        }
        /// <summary>
        /// Generates a random number with the number of bits specified
        /// </summary>
        /// <param name="numberOfBits">Number of bits of the generated number</param>
        /// <returns>a 64 bit random</returns>
        public ulong Next(int numberOfBits)
        {
            if( numberOfBits > 64 || numberOfBits < 1 ) return 0;

            ulong mask = 1;
            if ( numberOfBits>32 ) numberOfBits-=32;
            for(int i=1; i<numberOfBits;i++)
            {
                mask = mask << 1;
                mask++;
            }

            if( numberOfBits > 32 )
            {
                return ((mask & Next()) << 32) + (Next());
            }
            else if(numberOfBits > 31)
            {
                return (ulong)NextUInt31();
            }
            else
            {
                return mask & Next();
            }
        }
        /// <summary>
        /// Generates a random double on the interval [0, 1]
        /// </summary>
        /// <returns>a 64 bit floating point random</returns>
        public double NextDouble()
        {
            return (double)NextUIntAsUlong() * (1.0 / 4294967295.0);
        }
        /// <summary>
        /// Generates a random double on the interval specified, one of:
        /// [0, 1] - use NextDouble() for this interval
        /// [0, 1) - NextDouble(true, false)
        /// (0, 1] - NextDouble(false, true)
        /// (0, 1) - NextDouble(false, false)
        /// </summary>
        /// <param name="includeZero">Include zero in the interval</param>
        /// <param name="includeOne">Include one in the interval</param>
        /// <returns>a 64 bit floating point random</returns>
        public double NextDouble(bool includeZero, bool includeOne)
        {
            if (includeZero)
            {
                if (includeOne)
                {
                    return (double)NextUIntAsUlong() * (1.0 / 4294967295.0);
                }
                else
                {
                    return (double)NextUIntAsUlong() * (1.0 / 4294967296.0);
                }
            }
            else
            {
                if (includeOne)
                {
                    return 1.0 - (double)NextUIntAsUlong() * (1.0 / 4294967296.0);
                }
                else
                {
                    return (((double)NextUIntAsUlong()) + 0.5) * (1.0 / 4294967296.0);
                }
            }
        }


        public ulong GetCountOfGeneratedRandoms()
        {
            return numberOfRandomsGenerated;
        }
    }
}

       
        ///* generates a random number on [0,0xffffffffffffffff]-interval */
        //protected ulong GenRandInt64()
        //{
        //    ulong var = GenRandInt32();
        //    var = var << 32;
        //    return var + (ulong)(GenRandInt32());
        //}

        ///* generates a random number on [0,0x7fffffff]-interval */
        //protected long GenRandInt31()
        //{
        //    return (long)(NextUInt() >> 1);
        //}

        ///* generates a random number on [0,1]-real-interval */
        //protected double GenRandReal1()
        //{
        //    return GenRandInt32()*(1.0/4294967295.0);
        //    /* divided by 2^32-1 */
        //}

        ///* generates a random number on [0,1)-real-interval */
        //protected double GenRandReal2()
        //{
        //    return GenRandInt32()*(1.0/4294967296.0);
        //    /* divided by 2^32 */
        //}

        ///* generates a random number on (0,1)-real-interval */
        //protected double GenRandReal3()
        //{
        //    return (((double)GenRandInt32()) + 0.5)*(1.0/4294967296.0);
        //    /* divided by 2^32 */
        //}

        ///* generates a random number on [0,1) with 53-bit resolution*/
        //protected double GenRandRes53()
        //{
        //    ulong a=GenRandInt32()>>5, b=GenRandInt32()>>6;
        //    return(a*67108864.0+b)*(1.0/9007199254740992.0);
        //}
        ///* These real versions are due to Isaku Wada, 2002/01/09 added */

/*
 * 
 * 
        protected ulong GenRandInt32()
        {
            ulong y;
            
            /* mag01[x] = x * MATRIX_A  for x=0,1 * /

            if (mti >= N)
            { /* generate N words at one time * /
                int kk;

                if (mti == N+1)   /* if init_genrand() has not been called, * /
                    InitGenRand(5489UL); /* a default initial seed is used * /

                for (kk=0;kk<N-M;kk++) {
                    y = (mt[kk]&UPPER_MASK)|(mt[kk+1]&LOWER_MASK);
                    mt[kk] = mt[kk+M] ^ (y >> 1) ^ mag01[y & 0x1UL];
                }
                for (;kk<N-1;kk++) {
                    y = (mt[kk]&UPPER_MASK)|(mt[kk+1]&LOWER_MASK);
                    mt[kk] = mt[kk+(M-N)] ^ (y >> 1) ^ mag01[y & 0x1UL];
                }
                y = (mt[N-1]&UPPER_MASK)|(mt[0]&LOWER_MASK);
                mt[N-1] = mt[M-1] ^ (y >> 1) ^ mag01[y & 0x1UL];

                mti = 0;
            }

            y = mt[mti++];

            /* Tempering * /
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680UL;
            y ^= (y << 15) & 0xefc60000UL;
            y ^= (y >> 18);

            numberOfRandomsGenerated++;
            return y;
        }


*/