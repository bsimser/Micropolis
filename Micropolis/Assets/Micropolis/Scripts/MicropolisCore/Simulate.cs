using System;

namespace MicropolisCore
{
    public partial class Micropolis
    {
        private void simFrame()
        {
            if (simSpeed == 0)
            {
                return;
            }

            if (++speedCycle > 1023)
            {
                speedCycle = 0;
            }

            if (simSpeed == 1 && (speedCycle % 5) != 0)
            {
                return;
            }

            if (simSpeed == 2 && (speedCycle % 3) != 0)
            {
                return;
            }

            simulate();
        }

        private T clamp<T>(T val, T lower, T upper) where T : IComparable
        {
            if (val.CompareTo(lower) < 0)
            {
                return lower;
            }
            if (val.CompareTo(upper) > 0)
            {
                return upper;
            }
            return val;
        }

        private void simulate()
        {
            short[] speedPowerScan = 
                { 2,  4,  5 };
            short[] SpeedPollutionTerrainLandValueScan =
                { 2,  7, 17 };
            short[] speedCrimeScan = 
                { 1,  8, 18 };
            short[] speedPopulationDensityScan =
                { 1,  9, 19 };
            short[] speedFireAnalysis =
                { 1, 10, 20 };

            short speedIndex = clamp((short)(simSpeed - 1), (short)0, (short)2);

            // The simulator has 16 different phases, which we cycle through
            // according to phaseCycle, which is incremented and wrapped at
            // the end of this switch.

            if (initSimLoad != 0)
            {
                phaseCycle = 0;
            }
            else
            {
                phaseCycle &= 15;
            }

            switch (phaseCycle)
            {
                case 0:
                    if (++simCycle > 1023)
                    {
                        simCycle = 0; // This is cosmic!
                    }

                    if (doInitialEval)
                    {
                        doInitialEval = false;
                        cityEvaluation();
                    }

                    cityTime++;
                    cityTaxAverage += cityTax;

                    if ((simCycle & 1) == 0)
                    {
                        setValves();
                    }

                    clearCensus();

                    break;

                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    // Scan 1/8 of the map for each of the 8 phases 1..8:
                    mapScan((phaseCycle - 1) * WORLD_W / 8, phaseCycle * WORLD_W / 8);
                    break;

                case 9:
                    // TODO census
                    break;

                case 10:
                    if (simCycle % 5 == 0)
                    {
                        decRateOfGrowthMap();
                    }

                    decTrafficMap();

                    // TODO newMapFlags

                    sendMessages();

                    break;

                case 11:
                    if (simCycle % speedPowerScan[speedIndex] == 0)
                    {
                        doPowerScan();
                        newMapFlags[(int) MapType.MAP_TYPE_POWER] = 1;
                        newPower = true;
                    }
                    break;

                case 12:
                    if (simCycle % SpeedPollutionTerrainLandValueScan[speedIndex] == 0)
                    {
                        pollutionTerrainLandValueScan();
                    }
                    break;

                case 13:
                    if (simCycle % speedCrimeScan[speedIndex] == 0)
                    {
                        crimeScan();
                    }
                    break;

                case 14:
                    if (simCycle % speedPopulationDensityScan[speedIndex] == 0)
                    {
                        populationDensityScan();
                    }
                    break;

                case 15:
                    if (simCycle % speedFireAnalysis[speedIndex] == 0)
                    {
                        fireAnalysis();
                    }
                    doDisasters();
                    break;
            }

            // Go on the the next phase.
            phaseCycle = (short) ((phaseCycle + 1) & 15);
        }

        /// <summary>
        /// Initialize simulation.
        /// </summary>
        public void doSimInit()
        {
            phaseCycle = 0;
            simCycle = 0;

            if (initSimLoad == 2)
            {
                // if new city
                initSimMemory();
            }

            if (initSimLoad == 1)
            {
                // if city just loaded
                simLoadInit();
            }

            setValves();
            clearCensus();
            mapScan(0, WORLD_W);
            doPowerScan();
            newPower = true;
            pollutionTerrainLandValueScan();
            crimeScan();
            populationDensityScan();
            fireAnalysis();
            newMap = 1;
            censusChanged = true;
            totalPop = 1;
            doInitialEval = true;
        }

        /// <summary>
        /// Copy bits from powerGridMap to the #PWRBIT in the map for all zones in the world.
        /// </summary>
        public void doNilPower()
        {
            short x, y;

            for (x = 0; x < WORLD_W; x++)
            {
                for (y = 0; y < WORLD_H; y++)
                {
                    ushort z = map[x, y];
                    if ((z & (ushort) MapTileBits.ZONEBIT) != 0)
                    {
                        setZonePower(new Position(x, y));
                    }
                }
            }
        }

        /// <summary>
        /// Decrease traffic memory.
        /// </summary>
        /// <remarks>
        /// Tends to empty trafficDensityMap.
        /// </remarks>
        public void decTrafficMap()
        {
            short x, y, z;

            for (x = 0; x < WORLD_W; x += (short) trafficDensityMap.MAP_BLOCKSIZE)
            {
                for (y = 0; y < WORLD_H; y += (short) trafficDensityMap.MAP_BLOCKSIZE)
                {
                    z = trafficDensityMap.worldGet(x, y);
                    if (z == 0)
                    {
                        continue;
                    }

                    if (z <= 24)
                    {
                        trafficDensityMap.worldSet(x, y, 0);
                        continue;
                    }

                    if (z > 200)
                    {
                        trafficDensityMap.worldSet(x, y, (byte) (z - 34));
                    }
                    else
                    {
                        trafficDensityMap.worldSet(x, y, (byte)(z - 24));
                    }
                }                
            }
        }

        /// <summary>
        /// Decrease rate of grow.
        /// </summary>
        /// <remarks>
        /// Tends to empty rateOfGrowthMap.
        /// </remarks>
        public void decRateOfGrowthMap()
        {
            short x, y, z;

            for (x = 0; x < rateOfGrowthMap.MAP_W; x++)
            {
                for (y = 0; y < rateOfGrowthMap.MAP_H; y++)
                {
                    z = rateOfGrowthMap.get(x, y);
                    if (z == 0)
                    {
                        continue;
                    }

                    if (z > 0)
                    {
                        z--;
                        z = clamp(z, (short) -200, (short) 200);
                        rateOfGrowthMap.set(x, y, z);
                        continue;
                    }

                    if (z < 0)
                    {
                        z++;
                        z = clamp(z, (short) -200, (short) 200);
                        rateOfGrowthMap.set(x, y, z);
                    }
                }
            }
        }

        public void initSimMemory()
        {
            setCommonInits();

            for (short x = 0; x < 240; x++)
            {
                resHist[x] = 0;
                comHist[x] = 0;
                indHist[x] = 0;
                moneyHist[x] = 128;
                crimeHist[x] = 0;
                pollutionHist[x] = 0;
            }

            crimeRamp = 0;
            pollutionRamp = 0;
            totalPop = 0;
            resValve = 0;
            comValve = 0;
            indValve = 0;
            resCap = false; // Do not block residential growth
            comCap = false; // Do not block commercial growth
            indCap = false; // Do not block industrial growth

            externalMarket = 6.0f;
            // TODO disasterEvent = SC_NONE;
            // TODO scoreType = SC_NONE;

            /* This clears powermem */
            powerStackPointer = 0;
            doPowerScan();
            newPower = true; /* post rel */

            initSimLoad = 0;
        }

        public void simLoadInit()
        {
            // TODO

            if (cityTime < 0)
            {
                cityTime = 0;
            }

            // TODO consider changing this to an int or do a different compare
            if (externalMarket != 0)
            {
                externalMarket = 4.0f;
            }

            // TODO Set game level

            setCommonInits();

            // Load cityClass

            resCap = false;
            comCap = false;
            indCap = false;

            cityTaxAverage = (short) (cityTime % 48 * 7);

            // Set power map
            // TODO What purpose does this server? Weird...
            powerGridMap.fill(1);

            doNilPower();

            // TODO

            roadEffect = MAX_ROAD_EFFECT;
            policeEffect = MAX_POLICE_STATION_EFFECT;
            fireEffect = MAX_FIRE_STATION_EFFECT;
            initSimLoad = 0;
        }

        public void setCommonInits()
        {
            evalInit();
            roadEffect = MAX_ROAD_EFFECT;
            policeEffect = MAX_POLICE_STATION_EFFECT;
            fireEffect = MAX_FIRE_STATION_EFFECT;
            taxFlag = false;
            taxFund = 0;
        }

        public void setValves()
        {
            // TODO
            valveFlag = true;
        }

        public void clearCensus()
        {
            poweredZoneCount = 0;
            unpoweredZoneCount = 0;
            firePop = 0;
            roadTotal = 0;
            railTotal = 0;
            resPop = 0;
            comPop = 0;
            indPop = 0;
            resZonePop = 0;
            comZonePop = 0;
            indZonePop = 0;
            hospitalPop = 0;
            churchPop = 0;
            policeStationPop = 0;
            fireStationPop = 0;
            stadiumPop = 0;
            coalPowerPop = 0;
            nuclearPowerPop = 0;
            seaportPop = 0;
            airportPop = 0;
            powerStackPointer = 0; /* Reset before Mapscan */

            fireStationMap.clear();
            policeStationMap.clear();
        }

        /// <summary>
        /// Take monthly snaphsot of all relevant data for the historic graphs.
        /// Also update variables that control building new churches and hospitals.
        /// TODO Rename to takeMonthlyCensus (or takeMonthlySnaphshot?)
        /// TODO A lot of this max stuff is also done in Graph.cs
        /// </summary>
        public void take10Census()
        {
            // TODO
        }

        public void take120Census()
        {
            // TODO
        }

        /// <summary>
        /// Collect taxes
        /// </summary>
        public void collectTax()
        {
            // TODO
        }

        /// <summary>
        /// Update effects of (possibly reduced) funding.
        /// It updates effects with respect to roads, police, and fire.
        /// </summary>
        public void updateFundEffects()
        {
            // TODO
        }

        public void mapScan(int x1, int x2)
        {
            for (int x = x1; x < x2; x++)
            {
                for (int y = 0; y < WORLD_H; y++)
                {
                    var mapVal = map[x, y];
                    if (mapVal == (ushort) MapTileCharacters.DIRT)
                    {
                        continue;
                    }

                    ushort tile = (ushort)(mapVal & (ushort) MapTileBits.LOMASK); // Make off status bits

                    if (tile < (ushort) MapTileCharacters.FLOOD)
                    {
                        continue;
                    }

                    // tile >= FLOOD

                    var pos = new Position(x, y);

                    if (tile < (ushort) MapTileCharacters.ROADBASE)
                    {
                        if (tile >= (ushort) MapTileCharacters.FIREBASE)
                        {
                            firePop++;
                            if ((getRandom16() & 3) == 0)
                            {
                                doFire(pos); // 1 in 4 times
                            }
                            continue;
                        }

                        if (tile < (ushort) MapTileCharacters.RADTILE)
                        {
                            doFlood(pos);
                        }
                        else
                        {
                            doRadTile(pos);
                        }

                        continue;
                    }

                    if (newPower && (mapVal & (ushort) MapTileBits.CONDBIT) != 0)
                    {
                        // Copy PWRBIT from powerGridMap
                        setZonePower(pos);
                    }

                    if (tile >= (ushort) MapTileCharacters.ROADBASE && tile < (ushort) MapTileCharacters.POWERBASE)
                    {
                        doRoad(pos);
                        continue;
                    }

                    if ((mapVal & (ushort) MapTileBits.ZONEBIT) != 0)
                    {
                        doZone(pos);
                        continue;
                    }

                    if (tile >= (ushort) MapTileCharacters.RAILBASE && tile < (ushort) MapTileCharacters.RESBASE)
                    {
                        doRail(pos);
                        continue;
                    }

                    if (tile >= (ushort) MapTileCharacters.SOMETINYEXP &&
                        tile <= (ushort) MapTileCharacters.LASTTINYEXP)
                    {
                        // clear AniRubble
                        map[x, y] = randomRubble();
                    }
                }
            }
        }

        /// <summary>
        /// Handle rail track.
        /// Generate a train, and handle road deteriorating effects.
        /// </summary>
        /// <param name="pos">Position of the rail.</param>
        private void doRail(Position pos)
        {
            railTotal++;

            generateTrain(pos.posX, pos.posY);

            if (roadEffect < (15 * MAX_ROAD_EFFECT / 16))
            {
                // roadEffect < 15/16 of max road, enable deteriorating rail
                if ((getRandom16() & 511) == 0)
                {

                    ushort curValue = map[pos.posX,pos.posY];
                    if ((curValue & (ushort) MapTileBits.CONDBIT) == 0)
                    {
                        // Otherwise the '(getRandom16() & 31)' makes no sense
                        // assert(MAX_ROAD_EFFECT == 32);
                        if (roadEffect < (getRandom16() & 31))
                        {
                            ushort tile = (ushort)(curValue & (ushort) MapTileBits.LOMASK);
                            if (tile < (ushort) MapTileCharacters.RAILBASE + 2)
                            {
                                map[pos.posX,pos.posY] = (ushort) MapTileCharacters.RIVER;
                            }
                            else
                            {
                                map[pos.posX,pos.posY] = randomRubble();
                            }
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handle decay of radio-active tile
        /// </summary>
        /// <param name="pos">Position of the radio-active tile.</param>
        private void doRadTile(Position pos)
        {
            if ((getRandom16() & 4095) == 0)
            {
                map[pos.posX, pos.posY] = (ushort) MapTileCharacters.DIRT; // Radioactive decay
            }
        }

        private void doRoad(Position pos)
        {
            // TODO
        }

        /// <summary>
        /// Handle tile being on fire.
        /// TODO Needs a notion of iterative neighbour tiles computing.
        /// TODO Use a getFromMap()-like function here.
        /// TODO Extract constants of fire station effectiveness from here.
        /// </summary>
        /// <param name="pos">Position of the fire.</param>
        private void doFire(Position pos)
        {
            // TODO
        }
    }
}
