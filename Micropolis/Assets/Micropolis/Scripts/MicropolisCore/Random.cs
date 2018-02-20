namespace Micropolis.MicropolisCore
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
            return 0;
        }

        /// <summary>
        /// Draw a random number in a given range.
        /// </summary>
        /// <param name="range">Upper bound of the range (inclusive)</param>
        /// <returns>Random number between 0 and range (inclusive)</returns>
        public short getRandom(short range)
        {
            return 0;
        }

        /// <summary>
        /// Get random 16 bit number.
        /// </summary>
        /// <returns>Unsigned 16 bit random number</returns>
        public int getRandom16()
        {
            return 0;
        }

        /// <summary>
        /// Get signed 16 bit random number.
        /// </summary>
        /// <returns></returns>
        public int getRandom16Signed()
        {
            return 0;
        }

        /// <summary>
        /// Get a random number within a given range, with a preference to smaller 
        /// values.
        /// </summary>
        /// <param name="limit">Upper bound of the range (inclusive)</param>
        /// <returns>Random number between 0 and limit (inclusive)</returns>
        public short getERandom(short limit)
        {
            return 0;
        }

        /// <summary>
        /// Initialize the random number generator with a 'random' seed.
        /// </summary>
        public void randomlySeedRandom()
        {
        }

        /// <summary>
        /// Set seed of the random number generator.
        /// </summary>
        /// <param name="seed">New seed</param>
        public void seedRandom(int seed)
        {
        }
    }
}
