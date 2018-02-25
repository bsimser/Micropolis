using System;

namespace MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Handle zone
        /// </summary>
        /// <param name="pos">Position of the zone.</param>
        private void doZone(Position pos)
        {
            // Set Power Bit in Map from powerGridMap
            bool zonePowerFlag = setZonePower(pos);

            if (zonePowerFlag)
            {
                poweredZoneCount++;
            }
            else
            {
                unpoweredZoneCount++;
            }

            ushort tile = (ushort)(map[pos.posX,pos.posY] & (ushort) MapTileBits.LOMASK);

            // Do special zones.
//            if ((tile > (ushort) MapTileCharacters.PORTBASE) &&
//                (tile < (ushort)MapTileCharacters.CHURCH1BASE))
//            {
//                doSpecialZone(pos, zonePowerFlag);
//                return;
//            }

            // Do residential zones.
            if (tile < (ushort)MapTileCharacters.HOSPITAL)
            {
                doResidential(pos, zonePowerFlag);
                return;
            }

            // Do hospitals and churches.
//            if ((tile < (ushort)MapTileCharacters.COMBASE) ||
//                ((tile >= (ushort)MapTileCharacters.CHURCH1BASE) &&
//                 (tile <= (ushort)MapTileCharacters.CHURCH7LAST)))
//            {
//                doHospitalChurch(pos);
//                return;
//            }

            // Do commercial zones.
            if (tile < (ushort)MapTileCharacters.INDBASE)
            {
                //doCommercial(pos, zonePowerFlag);
                return;
            }

            // Do industrial zones.
//            if (tile < (ushort)MapTileCharacters.CHURCH1BASE)
//            {
//                doIndustrial(pos, zonePowerFlag);
//                return;
//            }

            //printf("UNEXPOECTED ZONE: %d !!!\n", tile);
        }

        /// <summary>
        /// Handle residential zone.
        /// </summary>
        /// <param name="pos">Center tile of the residential zone.</param>
        /// <param name="zonePower">Does the zone have power?</param>
        private void doResidential(Position pos, bool zonePower)
        {
            short tpop, zscore, locvalue, value, TrfGood;

            resZonePop++;

            ushort tile = (ushort)(map[pos.posX, pos.posY] & (ushort) MapTileBits.LOMASK);

            if (tile == (ushort) MapTileCharacters.FREEZ)
            {
                tpop = doFreePop(pos);
            }
            else
            {
                tpop = getResZonePop(tile);
            }

            resPop += tpop;

            if (tpop > getRandom(35))
            {
                // Try driving from residential to commercial
                TrfGood = makeTraffic(pos, ZoneType.ZT_COMMERCIAL);
            }
            else
            {
                TrfGood = 1;
            }

            if (TrfGood == -1)
            {
                value = getLandPollutionValue(pos);
                doResOut(pos, tpop, value);
                return;
            }

            if (tile == (ushort) MapTileCharacters.FREEZ || (getRandom16() & 7) != 0)
            {
                locvalue = evalRes(pos, TrfGood);
                zscore = (short) (resValve + locvalue);

                if (!zonePower)
                {
                    zscore -= 500;
                }

                if (zscore > -350 &&
                    ((short) (zscore - 26380) > ((short) getRandom16Signed())))
                {
                    if (tpop != 0 && (getRandom16() & 3) != 0)
                    {
                        makeHospital(pos);
                        return;
                    }

                    value = getLandPollutionValue(pos);
                    doResIn(pos, tpop, value);
                    return;
                }

                if (zscore < 350 &&
                    (((short) (zscore + 26380)) < ((short) getRandom16Signed())))
                {
                    value = getLandPollutionValue(pos);
                    doResOut(pos, tpop, value);
                }
            }
        }

        private void doResIn(Position pos, int pop, int value)
        {
            short pollution = populationDensityMap.worldGet(pos.posX, pos.posY);

            if (pollution > 128)
            {
                return;
            }

            ushort tile = (ushort) (map[pos.posX, pos.posY] & (ushort) MapTileBits.LOMASK);

            if (tile == (ushort) MapTileCharacters.FREEZ)
            {
                if (pop < 8)
                {
                    buildHouse(pos, value);
                    incRateOfGrowth(pos, 1);
                    return;
                }

                if (populationDensityMap.worldGet(pos.posX, pos.posY) > 64)
                {
                    resPlop(pos, 0, value);
                    incRateOfGrowth(pos, 8);
                    return;
                }

                return;
            }

            if (pop < 40)
            {
                resPlop(pos, (pop / 8) - 1, value);
                incRateOfGrowth(pos, 8);
            }
        }

        /// <summary>
        /// Try to build a house at the zone at pos.
        /// TODO Have some form of looking around the center tile (like getFromMap())
        /// </summary>
        /// <param name="pos">Center tile of the zone.</param>
        /// <param name="value">Value to build (land value?)</param>
        private void buildHouse(Position pos, int value)
        {
            short z, score, hscore, BestLoc;
            short[] ZeX = {0, -1, 0, 1, -1, 1, -1, 0, 1};
            short[] ZeY = {0, -1, -1, -1, 0, 0, 1, 1, 1};

            BestLoc = 0;
            hscore = 0;

            for (z = 1; z < 9; z++)
            {
                int xx = pos.posX + ZeX[z];
                int yy = pos.posY + ZeY[z];

                if (Position.testBounds((short) xx, (short) yy))
                {

                    score = evalLot(xx, yy);

                    // BUG score is never 0 !!
                    if (score != 0)
                    {
                        if (score > hscore)
                        {
                            hscore = score;
                            BestLoc = z;
                        }

                        // TODO Move the code below to a better place.
                        //      If we just updated hscore above, we could
                        //      trigger this code too.
                        if (score == hscore && (getRandom16() & 7) == 0)
                        {
                            BestLoc = z;
                        }
                    }
                }
            }

            if (BestLoc != 0)
            {
                int xx = pos.posX + ZeX[BestLoc];
                int yy = pos.posY + ZeY[BestLoc];

                if (Position.testBounds((short) xx, (short) yy))
                {
                    // TODO is HOUSE the proper constant here?
                    map[xx, yy] = (ushort)((ushort) MapTileCharacters.HOUSE + (ushort) MapTileBits.BLBNCNBIT + getRandom(2) + value * 3);
                }
            }
        }

        /// <summary>
        /// Evaluate suitability of the position for placing a new house.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Suitability.</returns>
        private short evalLot(int x, int y)
        {
            short z, score;
            short[] DX = {0, 1, 0, -1};
            short[] DY = {-1, 0, 1, 0};

            // test for clear lot
            z = (short) (map[x, y] & (ushort) MapTileBits.LOMASK);

            if (z > 0 && (z < (ushort) MapTileCharacters.RESBASE || z > (ushort) MapTileCharacters.RESBASE + 8))
            {
                return -1;
            }

            score = 1;

            for (z = 0; z < 4; z++)
            {
                int xx = x + DX[z];
                int yy = y + DY[z];

                if (Position.testBounds((short) xx, (short) yy) &&
                    map[xx, yy] != (ushort) MapTileCharacters.DIRT &&
                    (ushort)(map[xx, yy] & (ushort) MapTileBits.LOMASK) <= (ushort) MapTileCharacters.LASTROAD)
                {
                    score++; // look for road
                }
            }

            return score;
        }

        /// <summary>
        /// Put down a residential zone.
        /// </summary>
        /// <param name="pos">Center tile of the residential zone.</param>
        /// <param name="den">Population density (0..3)</param>
        /// <param name="value">Land value - pollution (0..3), higher is better.</param>
        private void resPlop(Position pos, int den, int value)
        {
            short baseValue;

            baseValue = (short) ((value * 4 + den) * 9 + MapTileCharacters.RZB - 4);
            zonePlop(pos, baseValue);
        }

        /// <summary>
        /// Put down a 3x3 zone around the center tile at pos..
        /// BUG This function allows partial on-map construction. Is that intentional? No!
        /// </summary>
        /// <param name="pos">Position to center around</param>
        /// <param name="baseValue">Tile number of the top-left tile.</param>
        /// <returns>Build was a success.</returns>
        private bool zonePlop(Position pos, short baseValue)
        {
            short z, x;
            short[] Zx = {-1, 0, 1, -1, 0, 1, -1, 0, 1};
            short[] Zy = {-1, -1, -1, 0, 0, 0, 1, 1, 1};

            for (z = 0; z < 9; z++)
            {
                // check for fire
                int xx = pos.posX + Zx[z];
                int yy = pos.posY + Zy[z];

                if (Position.testBounds((short) xx, (short) yy))
                {
                    x = (short)(map[xx, yy] & (ushort) MapTileBits.LOMASK);

                    if (x >= (ushort) MapTileCharacters.FLOOD && x < (ushort) MapTileCharacters.ROADBASE)
                    {
                        return false;
                    }
                }
            }

            for (z = 0; z < 9; z++)
            {
                int xx = pos.posX + Zx[z];
                int yy = pos.posY + Zy[z];

                if (Position.testBounds((short) xx, (short) yy))
                {
                    map[xx, yy] = (ushort)(baseValue + (ushort) MapTileBits.BNCNBIT);
                }

                baseValue++;
            }

            setZonePower(pos);
            map[pos.posX, pos.posY] |= (ushort)MapTileBits.ZONEBIT + (ushort) MapTileBits.BULLBIT;

            return true;
        }

        /// <summary>
        /// Update the rate of growth at position pos by amount.
        /// </summary>
        /// <param name="pos">Position to modify.</param>
        /// <param name="amount">Amount of change (can both be positive and negative).</param>
        private void incRateOfGrowth(Position pos, int amount)
        {
            int value = rateOfGrowthMap.worldGet(pos.posX, pos.posY);

            value = clamp(value + amount * 4, -200, 200);
            rateOfGrowthMap.worldSet(pos.posX, pos.posY, (short) value);
        }

        private void makeHospital(Position pos)
        {
            // TODO
        }

        private short evalRes(Position pos, int traf)
        {
            short value;

            if (traf < 0)
            {
                return -3000;
            }

            value = landValueMap.worldGet(pos.posX, pos.posY);
            value -= pollutionDensityMap.worldGet(pos.posX, pos.posY);

            if (value < 0)
            {
                value = 0; // Cap at 0
            }
            else
            {
                value = (short) Math.Min(value * 32, 6000); // Cap at 6000
            }

            value = (short) (value - 3000);

            return value;
        }

        private void doResOut(Position pos, int pop, int value)
        {
            // TODO
        }

        private short getLandPollutionValue(Position pos)
        {
            short landVal;

            landVal = landValueMap.worldGet(pos.posX, pos.posY);
            landVal -= pollutionDensityMap.worldGet(pos.posX, pos.posY);

            if (landVal < 30)
            {
                return 0;
            }

            if (landVal < 80)
            {
                return 1;
            }

            if (landVal < 150)
            {
                return 2;
            }

            return 3;
        }

        /// <summary>
        /// Return population of a residential zone center tile
        /// (RZB, RZB+9, ..., HOSPITAL - 9).
        /// </summary>
        /// <param name="mapTile">Center tile of a residential zone.</param>
        /// <returns>Population of the residential zone. (16, 24, 32, 40, 16, ..., 40 )</returns>
        private short getResZonePop(ushort mapTile)
        {
            short CzDen = (short)((mapTile - (short) MapTileCharacters.RZB) / 9 % 4);

            return (short) (CzDen * 8 + 16);
        }

        /// <summary>
        /// Count the number of single tile houses in a residential zone.
        /// </summary>
        /// <param name="pos">Position of the residential zone.</param>
        /// <returns>Number of single tile houses.</returns>
        private short doFreePop(Position pos)
        {
            short count = 0;

            for (short x = (short) (pos.posX - 1); x <= pos.posX + 1; x++)
            {
                for (short y = (short) (pos.posY - 1); y <= pos.posY + 1; y++)
                {
                    if (x >= 0 && x < WORLD_W && y >= 0 && y < WORLD_H)
                    {
                        ushort tile = (ushort)(map[x,y] & (ushort) MapTileBits.LOMASK);
                        if (tile >= (ushort) MapTileCharacters.LHTHR && tile <= (ushort) MapTileCharacters.HHTHR)
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Copy the value of powerGridMap at position pos to the map.
        /// </summary>
        /// <param name="pos">Position to copy.</param>
        /// <returns>Does the tile have power?</returns>
        private bool setZonePower(Position pos)
        {
            ushort mapValue = map[pos.posX, pos.posY];
            ushort tile = (ushort)(mapValue & (ushort) MapTileBits.LOMASK);

            if (tile == (ushort) MapTileCharacters.NUCLEAR || tile == (ushort) MapTileCharacters.POWERPLANT)
            {
                map[pos.posX, pos.posY] = (ushort)(mapValue | (ushort) MapTileBits.PWRBIT);
                return true;
            }

            if (powerGridMap.worldGet(pos.posX, pos.posY) > 0)
            {
                map[pos.posX, pos.posY] = (ushort) (mapValue | (ushort) MapTileBits.PWRBIT);
                return true;
            }
            else
            {
                map[pos.posX, pos.posY] = (ushort)(mapValue & ~(ushort)MapTileBits.PWRBIT);
                return false;
            }
        }

        /// <summary>
        /// Get commercial zone population number.
        /// </summary>
        /// <param name="tile">Tile of the commercial zone.</param>
        /// <returns>Population number of the zone.</returns>
        short getComZonePop(ushort tile)
        {
            if (tile == (ushort) MapTileCharacters.COMCLR)
            {
                return 0;
            }

            short CzDen = (short) (((tile - (ushort) MapTileCharacters.CZB) / 9) % 5 + 1);
            return CzDen;
        }

        /// <summary>
        /// Get the population value for the given industrial tile.
        /// </summary>
        /// <param name="tile">Center tile value of the industrial zone.</param>
        /// <returns>Population value.</returns>
        short getIndZonePop(ushort tile)
        {
            if (tile == (ushort)MapTileCharacters.INDCLR)
            {
                return 0;
            }

            short CzDen = (short)(((tile - (ushort)MapTileCharacters.IZB) / 9) % 4 + 1);
            return CzDen;
        }
    }
}