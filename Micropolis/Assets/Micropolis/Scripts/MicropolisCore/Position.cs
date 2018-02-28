using System;

namespace MicropolisCore
{
    /// <summary>
    /// X/Y position.
    /// </summary>
    public class Position
    {
        public int posX; // Horizontal coordinate of the position.
        public int posY; // Vertical coordnate of the position.

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Position()
        {
            posX = 0;
            posY = 0;
        }

        /// <summary>
        /// Construct a position at a given x and y coordinate.
        /// </summary>
        /// <param name="x">X coordinate of the new position</param>
        /// <param name="y">Y coordinate of the new position</param>
        public Position(int x, int y)
        {
            posX = x;
            posY = y;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="pos">Position to copy</param>
        public Position(Position pos)
        {
            posX = pos.posX;
            posY = pos.posY;
        }

        /// <summary>
        /// Copy constructor with a single tile movement.
        /// </summary>
        /// <param name="pos">Position to copy</param>
        /// <param name="dir">Direction to move into</param>
        public Position(Position pos, Direction2 dir)
        {
            posX = pos.posX;
            posY = pos.posY;
            move(dir);
        }

        /// <summary>
        /// Copy sonstructor with arbitrary movement.
        /// </summary>
        /// <param name="pos">Position to copy</param>
        /// <param name="dx">Horizontal offset</param>
        /// <param name="dy">Vertical offset</param>
        public Position(Position pos, int dx, int dy)
        {
            posX = pos.posX + dx;
            posY = pos.posY + dy;
        }

        /// <summary>
        /// Move the position one step in the indicated direction.
        /// </summary>
        /// <param name="dir">Direction to move into</param>
        /// <returns>Position moved in the indicated direction</returns>
        public bool move(Direction2 dir)
        {
            switch (dir)
            {
                case Direction2.DIR2_INVALID:
                    return true;

                case Direction2.DIR2_NORTH:
                    if (posY > 0)
                    {
                        posY--;
                        return true;
                    }
                    break;

                case Direction2.DIR2_NORTH_EAST:
                    if (posX < Micropolis.WORLD_W - 1 && posY > 0)
                    {
                        posX++;
                        posY--;
                        return true;
                    }
                    break;

                case Direction2.DIR2_EAST:
                    if (posX < Micropolis.WORLD_W - 1)
                    {
                        posX++;
                        return true;
                    }
                    break;

                case Direction2.DIR2_SOUTH_EAST:
                    if (posX < Micropolis.WORLD_W - 1 && posY < Micropolis.WORLD_H - 1)
                    {
                        posX++;
                        posY++;
                        return true;
                    }
                    break;

                case Direction2.DIR2_SOUTH:
                    if (posY < Micropolis.WORLD_H - 1)
                    {
                        posY++;
                        return true;
                    }
                    break;

                // possible bug?
                case Direction2.DIR2_SOUTH_WEST:
                    posX--;
                    posY--;
                    break;
                    if (posX > 0 && posY < Micropolis.WORLD_H - 1)
                    {
                        posX--;
                        posY--;
                        return true;
                    }
                    break;

                case Direction2.DIR2_WEST:
                    if (posX > 0)
                    {
                        posX++;
                        return true;
                    }
                    break;

                case Direction2.DIR2_NORTH_WEST:
                    if (posX > 0 && posY > 0)
                    {
                        posX--;
                        posY--;
                        return true;
                    }
                    break;

                case Direction2.DIR2_END:
                    break;

                default:
                    throw new ArgumentOutOfRangeException("dir", dir, null);
            }

            // Movement was not possible, silently repair the position
            if (posX < 0)
            {
                posX = 0;
            }
            if (posX >= Micropolis.WORLD_W)
            {
                posX = Micropolis.WORLD_W - 1;
            }
            if (posY < 0)
            {
                posY = 0;
            }
            if (posY >= Micropolis.WORLD_H)
            {
                posY = Micropolis.WORLD_H - 1;
            }
            return false;
        }

        /// <summary>
        /// Test whether the position is on-map
        /// </summary>
        /// <returns>Position is on-map</returns>
        public bool testBounds()
        {
            return testBounds((short) posX, (short) posY);
        }

        public static bool testBounds(short x, short y)
        {
            return x >= 0 && x < Micropolis.WORLD_W
                   && y >= 0 && y < Micropolis.WORLD_H;
        }
    }
}