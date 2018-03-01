namespace MicropolisCore
{
    public class DirectionUtils
    {
        /// <summary>
        /// Rotate the direction by 90 degrees.
        /// </summary>
        /// <param name="dir">Direction to rotate.</param>
        /// <returns>Rotated direction.</returns>
        public static Direction2 rotate90(Direction2 dir)
        {
            return rotate45(dir, 2);
        }

        /// <summary>
        /// Rotate the direction by 180 degrees.
        /// </summary>
        /// <param name="dir">Direction to rotate.</param>
        /// <returns>Rotated direction.</returns>
        public static Direction2 rotate180(Direction2 dir)
        {
            return rotate45(dir, 4);
        }

        /// <summary>
        /// Increment the direction by 45 degrees.
        /// </summary>
        /// <param name="dir">Direction to rotate.</param>
        /// <param name="count"></param>
        /// <returns>Rotated direction.</returns>
        public static Direction2 rotate45(Direction2 dir, int count = 1)
        {
            return ((dir - Direction2.DIR2_NORTH + count) & 7) + Direction2.DIR2_NORTH;
        }

        /// <summary>
        /// Increment the direction by 90 degrees.
        /// </summary>
        /// <param name="dir">Direction to rotate.</param>
        /// <returns>Rotated direction, possibly >= DIR2_END.</returns>
        public static Direction2 increment90(Direction2 dir)
        {
            return increment45(dir, 2);
        }

        /// <summary>
        /// Increment the direction by 45 degrees.
        /// </summary>
        /// <param name="dir">Direction to rotate.</param>
        /// <param name="count"></param>
        /// <returns>Rotated direction, possibly >= DIR2_END.</returns>
        public static Direction2 increment45(Direction2 dir, int count = 1)
        {
            return dir + count;
        }
    }
}