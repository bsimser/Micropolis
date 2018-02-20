namespace Micropolis.MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Makes traffic starting from the road tile at x, y.
        /// </summary>
        /// <param name="x">Start x position of the attempt</param>
        /// <param name="y">Start y position of the attempt</param>
        /// <param name="dest">Zone type to go to</param>
        /// <returns>1 if connection found, 0 if not found, -1 if no connection to road found.</returns>
        public short makeTrafficAt(int x, int y, ZoneType dest)
        {
            return 0;
        }

        /// <summary>
        /// Find a connection over a road from position x y to a specified zone type.
        /// </summary>
        /// <param name="x">Start x position of the attempt</param>
        /// <param name="y">Start y position of the attempt</param>
        /// <param name="dest">Zone type to go to</param>
        /// <returns>1 if connection found, 0 if not found, -1 if no connection to road found.</returns>
        public short makeTraffic(int x, int y, ZoneType dest)
        {
            return 0;
        }

        /// <summary>
        /// Update the trafficDensityMap from the positions at the curMapStackXY stack.
        /// </summary>
        public void addToTrafficDensityMap()
        {
        }
    }
}
