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
            Position pos = new Position();
            pos.posX = x;
            pos.posY = y;

            if (tryDrive(pos, dest)) // attempt to drive somewhere
            {
                addToTrafficDensityMap(); // if sucessful, inc trafdensity
                return 1; // traffic paused
            }

            return 0; // traffic failed
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
            Position startPos = new Position();
            startPos.posX = x;
            startPos.posY = y;
            return makeTraffic(startPos, dest);
        }

        /// <summary>
        /// Find a connection over a road from a startPos to a specified zone type.
        /// </summary>
        /// <param name="startPos">Start position of the attempt.</param>
        /// <param name="dest">Zone type to go to.</param>
        /// <returns>1 if connection found, 0 if not found, -1 if no connection to road found.</returns>
        short makeTraffic(Position startPos, ZoneType dest)
        {
            curMapStackPointer = 0; // Clear position stack

            var pos = new Position(startPos);

            if (findPerimeterRoad(pos))
            {
                if (tryDrive(pos, dest))
                {
                    addToTrafficDensityMap();
                    return 1;
                }

                return 0;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Update the trafficDensityMap from the positions at the curMapStackXY stack.
        /// </summary>
        public void addToTrafficDensityMap()
        {
            // TODO 
        }

        /// <summary>
        /// Find a connection to a road at the perimeter.
        /// TODO We could randomize the search
        /// </summary>
        /// <param name="pos">Starting position. Gets updated when a perimeter has been found.</param>
        /// <returns>Indication that a connection has been found.</returns>
        bool findPerimeterRoad(Position pos)
        {
            /* look for road on edges of zone */
            short[] PerimX = { -1, 0, 1, 2, 2, 2, 1, 0, -1, -2, -2, -2 };
            short[] PerimY = { -2, -2, -2, -1, 0, 1, 2, 2, 2, 1, 0, -1 };
            short tx, ty;

            for (short z = 0; z < 12; z++)
            {

                tx = (short)(pos.posX + PerimX[z]);
                ty = (short)(pos.posY + PerimY[z]);

                if (Position.testBounds(tx, ty))
                {

                    if (roadTest(map[tx,ty]))
                    {

                        pos.posX = tx;
                        pos.posY = ty;

                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Can the given tile be used as road?
        /// </summary>
        /// <param name="mv">Value from the map.</param>
        /// <returns>Indication that you can drive on the given tile</returns>
        bool roadTest(ushort mv)
        {
            ushort tile = (ushort) (mv & (ushort) MapTileBits.LOMASK);

            if (tile < (ushort) MapTileCharacters.ROADBASE || tile > (ushort) MapTileCharacters.LASTRAIL)
            {
                return false;
            }

            if (tile >= (ushort) MapTileCharacters.POWERBASE && tile < (ushort) MapTileCharacters.LASTPOWER)
            {
                return false;
            }

            return true;
        }

        bool tryDrive(Position startPos, ZoneType destZone)
        {
            // TODO
            return false;
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
