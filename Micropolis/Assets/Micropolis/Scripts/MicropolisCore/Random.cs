using System;

namespace MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Draw a random number (internal function).
        /// TODO Use Wolfram's fast cellular automata pseudo random number generator.
        /// </summary>
        /// <returns>Unsigned 16 bit random number</returns>
        public int simRandom()
        {
            nextRandom = nextRandom * 1103515245 + 12345;
            return (int) ((nextRandom & 0xffff00) >> 8);
        }

        /// <summary>
        /// Draw a random number in a given range.
        /// </summary>
        /// <param name="range">Upper bound of the range (inclusive)</param>
        /// <returns>Random number between 0 and range (inclusive)</returns>
        public short getRandom(short range)
        {
            int maxMultiple, rnum;

            range++; /// @bug Increment may cause range overflow.
            maxMultiple = 0xffff / range;
            maxMultiple *= range;

            do
            {
                rnum = getRandom16();
            } while (rnum >= maxMultiple);

            return (short) (rnum % range);
        }

        /// <summary>
        /// Get random 16 bit number.
        /// </summary>
        /// <returns>Unsigned 16 bit random number</returns>
        public int getRandom16()
        {
            return simRandom() & 0x0000ffff;
        }

        /// <summary>
        /// Get signed 16 bit random number.
        /// </summary>
        /// <returns></returns>
        public int getRandom16Signed()
        {
            int i = getRandom16();

            if (i > 0x7fff)
            {
                i = 0x7fff - i;
            }

            return i;
        }

        /// <summary>
        /// Get a random number within a given range, with a preference to smaller 
        /// values.
        /// </summary>
        /// <param name="limit">Upper bound of the range (inclusive)</param>
        /// <returns>Random number between 0 and limit (inclusive)</returns>
        public short getERandom(short limit)
        {
            short z = getRandom(limit);
            short x = getRandom(limit);

            return Math.Min(z, x);
        }

        /// <summary>
        /// Initialize the random number generator with a 'random' seed.
        /// </summary>
        public void randomlySeedRandom()
        {
            seedRandom(new Random().Next());
        }

        /// <summary>
        /// Set seed of the random number generator.
        /// </summary>
        /// <param name="seed">New seed</param>
        public void seedRandom(int seed)
        {
            nextRandom = seed;
        }
    }
}
