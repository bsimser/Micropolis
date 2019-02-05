using System;

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

#if false
            if ((!getRandom(2)) && findPerimeterTelecom(pos)) {
                /* printf("Telecom!\n"); */
                return 1;
            }
#endif

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
            /* For each saved position of the drive */
            while (curMapStackPointer > 0)
            {

                Position pos = pullPos();
                if (pos.testBounds())
                {

                    ushort tile = (ushort)(map[pos.posX,pos.posY] & (ushort) MapTileBits.LOMASK);

                    if (tile >= (ushort) MapTileCharacters.ROADBASE && tile < (ushort) MapTileCharacters.POWERBASE)
                    {
                        SimSprite sprite;

                        // Update traffic density.
                        int traffic = trafficDensityMap.worldGet(pos.posX, pos.posY);
                        traffic += 50;
                        traffic = Math.Min(traffic, 240);
                        trafficDensityMap.worldSet(pos.posX, pos.posY, (byte)traffic);

                        // Check for heavy traffic.
                        if (traffic >= 240 && getRandom(5) == 0)
                        {

                            trafMaxX = (short) pos.posX;
                            trafMaxY = (short) pos.posY;

                            /* Direct helicopter towards heavy traffic */
                            sprite = getSprite((int) SpriteType.SPRITE_HELICOPTER);
                            if (sprite != null && sprite.control == -1)
                            {

                                sprite.destX = trafMaxX * 16;
                                sprite.destY = trafMaxY * 16;

                            }
                        }
                    }
                }
            }
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

        /// <summary>
        /// Try to drive to a destination.
        /// </summary>
        /// <param name="startPos">Starting position.</param>
        /// <param name="destZone">Zonetype to drive to.</param>
        /// <returns>Was drive succesful?</returns>
        bool tryDrive(Position startPos, ZoneType destZone)
        {
            Direction2 dirLast = Direction2.DIR2_INVALID;
            Position drivePos = new Position(startPos);

            /* Maximum distance to try */
            for (short dist = 0; dist < MAX_TRAFFIC_DISTANCE; dist++)
            {
                Direction2 dir = tryGo(drivePos, dirLast);
                if (dir != Direction2.DIR2_INVALID)
                { 
                    // we found a road
                    drivePos.move(dir);
                    dirLast = DirectionUtils.rotate180(dir);

                    /* Save pos every other move.
                     * This also relates to
                     * Micropolis::trafficDensityMap::MAP_BLOCKSIZE
                     */
                    if ((dist & 1) != 0)
                    {
                        pushPos(drivePos);
                    }

                    if (driveDone(drivePos, destZone))
                    { 
                        // if destination is reached
                        return true; /* pass */
                    }
                }
                else
                {
                    if (curMapStackPointer > 0)
                    { 
                        /* dead end, backup */
                        curMapStackPointer--;
                        dist += 3;
                    }
                    else
                    {
                        return false; /* give up at start  */
                    }

                }
            }

            return false; /* gone MAX_TRAFFIC_DISTANCE */
        }

        /// <summary>
        /// Has the journey arrived at its destination?
        /// </summary>
        /// <param name="pos">Current position.</param>
        /// <param name="destZone">Zonetype to drive to.</param>
        /// <returns>Destination has been reached.</returns>
        private bool driveDone(Position pos, ZoneType destZone)
        {
            // TODO Use macros to determine the zone type: residential, commercial or industrial.
            // commercial, industrial, residential destinations
            ushort[] targetLow = { (ushort) MapTileCharacters.COMBASE, (ushort) MapTileCharacters.LHTHR, (ushort) MapTileCharacters.LHTHR };
            ushort[] targetHigh = { (ushort)MapTileCharacters.NUCLEAR, (ushort)MapTileCharacters.PORT, (ushort)MapTileCharacters.COMBASE };

            //assert(ZT_NUM_DESTINATIONS == LENGTH_OF(targetLow));
            //assert(ZT_NUM_DESTINATIONS == LENGTH_OF(targetHigh));

            ushort l = targetLow[(int) destZone]; // Lowest acceptable tile value
            ushort h = targetHigh[(int) destZone]; // Highest acceptable tile value

            if (pos.posY > 0)
            {
                ushort z = (ushort)(map[pos.posX,pos.posY - 1] & (ushort)MapTileBits.LOMASK);
                if (z >= l && z <= h)
                {
                    return true;
                }
            }

            if (pos.posX < (WORLD_W - 1))
            {
                ushort z = (ushort)(map[pos.posX + 1,pos.posY] & (ushort) MapTileBits.LOMASK);
                if (z >= l && z <= h)
                {
                    return true;
                }
            }

            if (pos.posY < (WORLD_H - 1))
            {
                ushort z = (ushort)(map[pos.posX,pos.posY + 1] & (ushort)MapTileBits.LOMASK);
                if (z >= l && z <= h)
                {
                    return true;
                }
            }

            if (pos.posX > 0)
            {
                ushort z = (ushort)(map[pos.posX - 1,pos.posY] & (ushort) MapTileBits.LOMASK);
                if (z >= l && z <= h)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Try to drive one tile in a random direction.
        /// </summary>
        /// <param name="pos">Current position.</param>
        /// <param name="dirLast">Forbidden direction for movement (to prevent reversing).</param>
        /// <returns>Direction of movement, DIR2_INVALID is returned if not moved.</returns>
        private Direction2 tryGo(Position pos, Direction2 dirLast)
        {
            Direction2[] directions = new Direction2[4];

            // Find connections from current position.
            Direction2 dir = Direction2.DIR2_NORTH;
            int count = 0;
            int i;
            for (i = 0; i < 4; i++)
            {
                if (dir != dirLast && roadTest(getTileFromMap(pos, dir, (ushort) MapTileCharacters.DIRT)))
                {
                    // found a road in an allowed direction
                    directions[i] = dir;
                    count++;
                }
                else
                {
                    directions[i] = Direction2.DIR2_INVALID;
                }

                dir = DirectionUtils.rotate90(dir);
            }

            if (count == 0)
            { 
                // dead end
                return Direction2.DIR2_INVALID;
            }

            // We have at least one way to go.

            if (count == 1)
            { 
                // only one solution
                for (i = 0; i < 4; i++)
                {
                    if (directions[i] != Direction2.DIR2_INVALID)
                    {
                        return directions[i];
                    }
                }
            }

            // more than one choice, draw a random number.
            i = getRandom16() & 3;
            while (directions[i] == Direction2.DIR2_INVALID)
            {
                i = (i + 1) & 3;
            }
            return directions[i];
        }

        /// <summary>
        /// Push a position onto the position stack.
        /// TODO use the .NET Stack class instead for this
        /// </summary>
        /// <param name="pos">Position to push.</param>
        private void pushPos(Position pos)
        {
            curMapStackPointer++;
            //assert(curMapStackPointer < MAX_TRAFFIC_DISTANCE + 1);
            curMapStackXY[curMapStackPointer] = pos;
        }

        /// <summary>
        /// Pull top-most position from the position stack.
        /// TODO use the .NET Stack class instead for this
        /// </summary>
        /// <returns>Pulled position.</returns>
        private Position pullPos()
        {
            //assert(curMapStackPointer > 0);
            curMapStackPointer--;
            return curMapStackXY[curMapStackPointer + 1];
        }

        /// <summary>
        /// Get neighbouring tile from the map.
        /// TODO should probably be in a Tile or Map class or something.
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
