namespace MicropolisCore
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

        /// <summary>
        /// Get neighbouring tile from the map.
        /// </summary>
        /// <param name="pos">Current position.</param>
        /// <param name="dir">Direction of neighbouring tile, only horizontal and vertical directions are supported.</param>
        /// <param name="defaultTile">Tile to return if off-map.</param>
        /// <returns>
        /// The tile in the indicated direction. If tile is off-world or an
        /// incorrect direction is given, DIRT is returned
        /// </returns>
        private ushort getTileFromMap(Position pos, Direction2 dir, ushort defaultTile)
        {
            switch (dir)
            {
                case Direction2.DIR2_NORTH:
                    if (pos.posY > 0)
                    {
                        return (ushort)(map[pos.posX,pos.posY - 1] & (ushort) MapTileBits.LOMASK);
                    }

                    return defaultTile;

                case Direction2.DIR2_EAST:
                    if (pos.posX < WORLD_W - 1)
                    {
                        return (ushort)(map[pos.posX + 1,pos.posY] & (ushort)MapTileBits.LOMASK);
                    }

                    return defaultTile;

                case Direction2.DIR2_SOUTH:
                    if (pos.posY < WORLD_H - 1)
                    {
                        return (ushort)(map[pos.posX,pos.posY + 1] & (ushort)MapTileBits.LOMASK);
                    }

                    return defaultTile;

                case Direction2.DIR2_WEST:
                    if (pos.posX > 0)
                    {
                        return (ushort)(map[pos.posX - 1,pos.posY] & (ushort)MapTileBits.LOMASK);
                    }

                    return defaultTile;

                default:
                    return defaultTile;
            }
        }
    }
}
