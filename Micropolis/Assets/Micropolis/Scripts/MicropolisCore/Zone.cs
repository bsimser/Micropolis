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
            // TODO needs pollutionDensityMap
        }

        private void makeHospital(Position pos)
        {
            // TODO
        }

        private short evalRes(Position pos, int traf)
        {
            // TODO needs landValueMap and pollutionDensityMap
            return 0;
        }

        private void doResOut(Position pos, int pop, int value)
        {
            // TODO
        }

        private short getLandPollutionValue(Position pos)
        {
            // TODO needs landValueMap and pollutionDensityMap
            return 0;
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

        private bool setZonePower(Position pos)
        {
            // TODO
            return false;
        }    
    }
}