namespace MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Scan the map for powered tiles, and copy them to the powerGridMap array.
        /// Also warns the user about using too much power ('buy another power plant').
        /// </summary>
        public void doPowerScan()
        {
            Direction2 anyDir, dir;
            int conNum;

            // Clear power map.
            powerGridMap.clear();

            // Power that combined coal and nuclear power plants can deliver.
            long maxPower = coalPowerPop * COAL_POWER_STRENGTH +
                            nuclearPowerPop * NUCLEAR_POWER_STRENGTH;

            long numPower = 0; // Amount of power used.

            while (powerStackPointer > 0)
            {
                Position pos = pullPowerStack();
                anyDir = Direction2.DIR2_INVALID;
                do
                {
                    numPower++;
                    if (numPower > maxPower)
                    {
                        sendMessage((short) MessageNumber.MESSAGE_NOT_ENOUGH_POWER);
                        return;
                    }
                    if (anyDir != Direction2.DIR2_INVALID)
                    {
                        pos.move(anyDir);
                    }
                    powerGridMap.worldSet(pos.posX, pos.posY, 1);
                    conNum = 0;
                    dir = Direction2.DIR2_BEGIN;
                    while (dir < Direction2.DIR2_END && conNum < 2)
                    {
                        if (testForConductive(pos, dir))
                        {
                            conNum++;
                            anyDir = dir;
                        }
                        dir = DirectionUtils.increment90(dir);
                    }
                    if (conNum > 1)
                    {
                        pushPowerStack(pos);
                    }
                } while (conNum != 0);
            }
        }

        /// <summary>
        /// Check at position pos for a power-less conducting tile in the
        /// direction testDir.
        /// TODO Re-use something like Micropolis::getFromMap(), and fold this function into its caller.
        /// </summary>
        /// <param name="pos">Position to start from.</param>
        /// <param name="testDir">Direction to investigate.</param>
        /// <returns>Unpowered tile has been found in the indicated direction.</returns>
        private bool testForConductive(Position pos, Direction2 testDir)
        {
            var movedPos = new Position(pos);

            if (movedPos.move(testDir))
            {
                if ((ushort)(map[movedPos.posX, movedPos.posY] & (ushort) MapTileBits.CONDBIT) == (ushort) MapTileBits.CONDBIT)
                {
                    if (powerGridMap.worldGet(movedPos.posX, movedPos.posY) == 0)
                    {
                        return true;
                    }                    
                }
            }

            return false;
        }

        /// <summary>
        /// Pull a position from the power stack.
        /// </summary>
        /// <returns>Pulled position.</returns>
        private Position pullPowerStack()
        {
            //assert(powerStackPointer > 0);
            powerStackPointer--;
            return powerStackXY[powerStackPointer + 1];
        }

        /// <summary>
        /// Push position a pos onto the power stack if there is room.
        /// </summary>
        /// <param name="pos">Position to push.</param>
        private void pushPowerStack(Position pos)
        {
            if (powerStackPointer < (POWER_STACK_SIZE - 2))
            {
                powerStackPointer++;
                powerStackXY[powerStackPointer] = pos;
            }
        }
    }
}