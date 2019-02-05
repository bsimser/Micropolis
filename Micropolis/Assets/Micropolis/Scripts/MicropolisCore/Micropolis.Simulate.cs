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
                    if (cityTime % CENSUS_FREQUENCY_10 == 0)
                    {
                        take10Census();
                    }

                    if (cityTime % CENSUS_FREQUENCY_120 == 0)
                    {
                        take120Census();
                    }

                    if (cityTime % TAX_FREQUENCY == 0)
                    {
                        collectTax();
                        cityEvaluation();
                    }
                    break;

                case 10:
                    if (simCycle % 5 == 0)
                    {
                        decRateOfGrowthMap();
                    }

                    decTrafficMap();

                    newMapFlags[(int)MapType.MAP_TYPE_TRAFFIC_DENSITY] = 1;
                    newMapFlags[(int)MapType.MAP_TYPE_ROAD] = 1;
                    newMapFlags[(int)MapType.MAP_TYPE_ALL] = 1;
                    newMapFlags[(int)MapType.MAP_TYPE_RES] = 1;
                    newMapFlags[(int)MapType.MAP_TYPE_COM] = 1;
                    newMapFlags[(int)MapType.MAP_TYPE_IND] = 1;
                    newMapFlags[(int)MapType.MAP_TYPE_DYNAMIC] = 1;

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
        /// Copy bits from powerGridMap to the PWRBIT in the map for all zones in the world.
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
            disasterEvent = ScenarioType.SC_NONE;
            scoreType = ScenarioType.SC_NONE;

            /* This clears powermem */
            powerStackPointer = 0;
            doPowerScan();
            newPower = true; /* post rel */

            initSimLoad = 0;
        }

        public void simLoadInit()
        {
            // Disaster delay table for each scenario
            short[] disasterWaitTable = {
                0,          // No scenario (free playing)
                2,          // Dullsville (boredom)
                10,         // San francisco (earth quake)
                4 * 10,     // Hamburg (fire bombs)
                20,         // Bern (traffic)
                3,          // Tokyo (scary monster)
                5,          // Detroit (crime)
                5,          // Boston (nuclear meltdown)
                2 * 48,     // Rio (flooding)
            };

            // Time to wait before score calculation for each scenario
            short[] scoreWaitTable = {
                0,          // No scenario (free playing)
                30 * 48,    // Dullsville (boredom)
                5 * 48,     // San francisco (earth quake)
                5 * 48,     // Hamburg (fire bombs)
                10 * 48,    // Bern (traffic)
                5 * 48,     // Tokyo (scary monster)
                10 * 48,    // Detroit (crime)
                5 * 48,     // Boston (nuclear meltdown)
                10 * 48,    // Rio (flooding)
            };

            externalMarket = (float)miscHist[1];
            resPop = miscHist[2];
            comPop = miscHist[3];
            indPop = miscHist[4];
            resValve = miscHist[5];
            comValve = miscHist[6];
            indValve = miscHist[7];
            crimeRamp = miscHist[10];
            pollutionRamp = miscHist[11];
            landValueAverage = miscHist[12];
            crimeAverage = miscHist[13];
            pollutionAverage = miscHist[14];
            gameLevel = (GameLevel)miscHist[15];

            if (cityTime < 0)
            {
                cityTime = 0;
            }

            if (!(externalMarket != 0))
            {
                externalMarket = 4.0f;
            }

            // Set game level
            if (gameLevel > GameLevel.LEVEL_LAST || gameLevel < GameLevel.LEVEL_FIRST)
            {
                gameLevel = GameLevel.LEVEL_FIRST;
            }
            setGameLevel(gameLevel);

            setCommonInits();

            cityClass = (CityClass)(miscHist[16]);
            if (cityClass > CityClass.CC_MEGALOPOLIS || cityClass < CityClass.CC_VILLAGE)
            {
                cityClass = CityClass.CC_VILLAGE;
            }

            cityScore = miscHist[17];
            if (cityScore > 999 || cityScore < 1)
            {
                cityScore = 500;
            }

            resCap = false;
            comCap = false;
            indCap = false;

            cityTaxAverage = (short) (cityTime % 48 * 7);

            // Set power map
            // TODO What purpose does this server? Weird...
            powerGridMap.fill(1);

            doNilPower();

            if (scenario >= ScenarioType.SC_COUNT)
            {
                scenario = ScenarioType.SC_NONE;
            }

            if (scenario != ScenarioType.SC_NONE)
            {
                //assert(LENGTH_OF(disasterWaitTable) == SC_COUNT);
                //assert(LENGTH_OF(scoreWaitTable) == SC_COUNT);

                disasterEvent = scenario;
                disasterWait = disasterWaitTable[(int) disasterEvent];
                scoreType = disasterEvent;
                scoreWait = scoreWaitTable[(int) disasterEvent];
            }
            else
            {
                disasterEvent = ScenarioType.SC_NONE;
                disasterWait = 0;
                scoreType = ScenarioType.SC_NONE;
                scoreWait = 0;
            }

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
            short[] taxTable = {
                200, 150, 120, 100, 80, 50, 30, 0, -10, -40, -100,
                -150, -200, -250, -300, -350, -400, -450, -500, -550, -600,
            };
            float[] extMarketParamTable = {
                1.2f, 1.1f, 0.98f,
            };
            //assert(LEVEL_COUNT == LENGTH_OF(extMarketParamTable));

            // TODO Make configurable parameters.
            short resPopDenom = 8;
            float birthRate = 0.02f;
            float laborBaseMax = 1.3f;
            float internalMarketDenom = 3.7f;
            float projectedIndPopMin = 5.0f;
            float resRatioDefault = 1.3f;
            float resRatioMax = 2;
            float comRatioMax = 2;
            float indRatioMax = 2;
            short taxMax = 20;
            float taxTableScale = 600;

            // TODO Break the interesting values out into public member
            //      variables so the user interface can display them.
            float employment, migration, births, laborBase, internalMarket;
            float resRatio, comRatio, indRatio;
            float normalizedResPop, projectedResPop, projectedComPop, projectedIndPop;

            miscHist[1] = (short)externalMarket;
            miscHist[2] = resPop;
            miscHist[3] = comPop;
            miscHist[4] = indPop;
            miscHist[5] = resValve;
            miscHist[6] = comValve;
            miscHist[7] = indValve;
            miscHist[10] = crimeRamp;
            miscHist[11] = pollutionRamp;
            miscHist[12] = landValueAverage;
            miscHist[13] = crimeAverage;
            miscHist[14] = pollutionAverage;
            miscHist[15] = (short) gameLevel;
            miscHist[16] = (short)cityClass;
            miscHist[17] = cityScore;

            normalizedResPop = (float)resPop / (float)resPopDenom;
            totalPopLast = totalPop;
            totalPop = (short)(normalizedResPop + comPop + indPop);

            if (resPop > 0)
            {
                employment = (comHist[1] + indHist[1]) / normalizedResPop;
            }
            else
            {
                employment = 1;
            }

            migration = normalizedResPop * (employment - 1);
            births = normalizedResPop * birthRate;
            projectedResPop = normalizedResPop + migration + births;   // Projected res pop.

            // Compute laborBase
            float temp = comHist[1] + indHist[1];
            if (temp > 0.0)
            {
                laborBase = (resHist[1] / temp);
            }
            else
            {
                laborBase = 1;
            }
            laborBase = clamp(laborBase, 0.0f, laborBaseMax);

            internalMarket = (float)(normalizedResPop + comPop + indPop) / internalMarketDenom;

            projectedComPop = internalMarket * laborBase;

            //assert(gameLevel >= LEVEL_FIRST && gameLevel <= LEVEL_LAST);
            projectedIndPop = indPop * laborBase * extMarketParamTable[(int) gameLevel];
            projectedIndPop = Math.Max(projectedIndPop, projectedIndPopMin);

            if (normalizedResPop > 0)
            {
                resRatio = (float)projectedResPop / (float)normalizedResPop; // projected -vs- actual.
            }
            else
            {
                resRatio = resRatioDefault;
            }

            if (comPop > 0)
            {
                comRatio = (float)projectedComPop / (float)comPop;
            }
            else
            {
                comRatio = (float)projectedComPop;
            }

            if (indPop > 0)
            {
                indRatio = (float)projectedIndPop / (float)indPop;
            }
            else
            {
                indRatio = (float)projectedIndPop;
            }

            resRatio = Math.Min(resRatio, resRatioMax);
            comRatio = Math.Min(comRatio, comRatioMax);
            resRatio = Math.Min(indRatio, indRatioMax);

            // Global tax and game level effects.
            short z = Math.Min((short)(cityTax + gameLevel), taxMax);
            resRatio = (resRatio - 1) * taxTableScale + taxTable[z];
            comRatio = (comRatio - 1) * taxTableScale + taxTable[z];
            indRatio = (indRatio - 1) * taxTableScale + taxTable[z];

            // Ratios are velocity changes to valves.
            resValve = (short) clamp(resValve + (short)resRatio, -RES_VALVE_RANGE, RES_VALVE_RANGE);
            comValve = (short) clamp(comValve + (short)comRatio, -COM_VALVE_RANGE, COM_VALVE_RANGE);
            indValve = (short) clamp(indValve + (short)indRatio, -IND_VALVE_RANGE, IND_VALVE_RANGE);

            if (resCap && resValve > 0)
            {
                resValve = 0; // Need a stadium, so cap resValve.
            }

            if (comCap && comValve > 0)
            {
                comValve = 0; // Need a airport, so cap comValve.
            }

            if (indCap && indValve > 0)
            {
                indValve = 0; // Need an seaport, so cap indValve.
            }

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
            //fireStationEffectMap.clear(); // Added in rev293
            policeStationMap.clear();
            //policeStationEffectMap.clear(); // Added in rev293
        }

        /// <summary>
        /// Take monthly snaphsot of all relevant data for the historic graphs.
        /// Also update variables that control building new churches and hospitals.
        /// TODO Rename to takeMonthlyCensus (or takeMonthlySnaphshot?)
        /// TODO A lot of this max stuff is also done in Graph.cs
        /// </summary>
        public void take10Census()
        {
            // TODO: Make configurable parameters.
            int resPopDenom = 8;

            short x;

            /* put census#s in Historical Graphs and scroll data  */
            resHist10Max = 0;
            comHist10Max = 0;
            indHist10Max = 0;

            for (x = 118; x >= 0; x--)
            {

                resHist10Max = Math.Max(resHist10Max, resHist[x]);
                comHist10Max = Math.Max(comHist10Max, comHist[x]);
                indHist10Max = Math.Max(indHist10Max, indHist[x]);

                resHist[x + 1] = resHist[x];
                comHist[x + 1] = comHist[x];
                indHist[x + 1] = indHist[x];
                crimeHist[x + 1] = crimeHist[x];
                pollutionHist[x + 1] = pollutionHist[x];
                moneyHist[x + 1] = moneyHist[x];

            }

            graph10Max = resHist10Max;
            graph10Max = Math.Max(graph10Max, comHist10Max);
            graph10Max = Math.Max(graph10Max, indHist10Max);

            resHist[0] = (short) (resPop / resPopDenom);
            comHist[0] = comPop;
            indHist[0] = indPop;

            crimeRamp += (short)((crimeAverage - crimeRamp) / 4);
            crimeHist[0] = Math.Min(crimeRamp, (short)255);

            pollutionRamp += (short)((pollutionAverage - pollutionRamp) / 4);
            pollutionHist[0] = Math.Min(pollutionRamp, (short)255);

            x = (short) ((cashFlow / 20) + 128);    /* scale to 0..255  */
            moneyHist[0] = clamp(x, (short)0, (short)255);

            changeCensus();

            short resPopScaled = (short) (resPop >> 8);

            if (hospitalPop < resPopScaled)
            {
                needHospital = 1;
            }

            if (hospitalPop > resPopScaled)
            {
                needHospital = -1;
            }

            if (hospitalPop == resPopScaled)
            {
                needHospital = 0;
            }

            int faithfulPop = resPopScaled + faith;

            if (churchPop < faithfulPop)
            {
                needChurch = 1;
            }

            if (churchPop > faithfulPop)
            {
                needChurch = -1;
            }

            if (churchPop == faithfulPop)
            {
                needChurch = 0;
            }
        }

        public void take120Census()
        {
            // TODO: Make configurable parameters.
            int resPopDenom = 8;

            /* Long Term Graphs */
            short x;

            resHist120Max = 0;
            comHist120Max = 0;
            indHist120Max = 0;

            for (x = 238; x >= 120; x--)
            {

                resHist120Max = Math.Max(resHist120Max, resHist[x]);
                comHist120Max = Math.Max(comHist120Max, comHist[x]);
                indHist120Max = Math.Max(indHist120Max, indHist[x]);

                resHist[x + 1] = resHist[x];
                comHist[x + 1] = comHist[x];
                indHist[x + 1] = indHist[x];
                crimeHist[x + 1] = crimeHist[x];
                pollutionHist[x + 1] = pollutionHist[x];
                moneyHist[x + 1] = moneyHist[x];

            }

            graph120Max = resHist120Max;
            graph120Max = Math.Max(graph120Max, comHist120Max);
            graph120Max = Math.Max(graph120Max, indHist120Max);

            resHist[120] = (short) (resPop / resPopDenom);
            comHist[120] = comPop;
            indHist[120] = indPop;
            crimeHist[120] = crimeHist[0];
            pollutionHist[120] = pollutionHist[0];
            moneyHist[120] = moneyHist[0];
            changeCensus();
        }

        /// <summary>
        /// Collect taxes
        /// BUG Function seems to be doing different things depending on
        ///     totalPop value. With an non-empty city it does fund
        ///     calculations. For an empty city, it immediately sets effects of
        ///     funding, which seems inconsistent at least, and may be wrong
        /// BUG If taxFlag is set, no variable is touched which seems non-robust at least
        /// </summary>
        public void collectTax()
        {
            short z;

            /**
             * @todo Break out so the user interface can configure this.
             */
            float[] RLevels = { 0.7f, 0.9f, 1.2f };
            float[] FLevels = { 1.4f, 1.2f, 0.8f };

            //assert(LEVEL_COUNT == LENGTH_OF(RLevels));
            //assert(LEVEL_COUNT == LENGTH_OF(FLevels));

            cashFlow = 0;

            /**
             * @todo Apparently taxFlag is never set to true in MicropolisEngine
             *       or the TCL code, so this always runs.
             * @todo Check old Mac code to see if it's ever set, and why.
             */

            if (!taxFlag)
            { 
                // If the Tax Port is clear

                // TODO Do something with z? Check old Mac code to see if it's used.
                z = (short) (cityTaxAverage / 48);  // post release

                cityTaxAverage = 0;

                policeFund = (long)policeStationPop * 100;
                fireFund = (long)fireStationPop * 100;
                roadFund = (long)((roadTotal + (railTotal * 2)) * RLevels[(int) gameLevel]);
                taxFund = (long)((((long)totalPop * landValueAverage) / 120) * cityTax * FLevels[(int) gameLevel]);

                if (totalPop > 0)
                {
                    /* There are people to tax. */
                    cashFlow = (short)(taxFund - (policeFund + fireFund + roadFund));
                    doBudget();
                }
                else
                {
                    /* Nobody lives here. */
                    roadEffect = MAX_ROAD_EFFECT;
                    policeEffect = MAX_POLICE_STATION_EFFECT;
                    fireEffect = MAX_FIRE_STATION_EFFECT;
                }
            }
        }

        /// <summary>
        /// Update effects of (possibly reduced) funding.
        /// 
        /// It updates effects with respect to roads, police, and fire.
        /// </summary>
        public void updateFundEffects()
        {
            // Compute road effects of funding
            roadEffect = MAX_ROAD_EFFECT;
            if (roadFund > 0)
            {
                // Multiply with funding fraction
                roadEffect = (short)((float)roadEffect * (float)roadSpend / (float)roadFund);
            }

            // Compute police station effects of funding
            policeEffect = MAX_POLICE_STATION_EFFECT;
            if (policeFund > 0)
            {
                // Multiply with funding fraction
                policeEffect = (short)((float)policeEffect * (float)policeSpend / (float)policeFund);
            }

            // Compute fire station effects of funding
            fireEffect = MAX_FIRE_STATION_EFFECT;
            if (fireFund > 0)
            {
                // Multiply with funding fraction
                fireEffect = (short)((float)fireEffect * (float)fireSpend / (float)fireFund);
            }

#if false
            printf("========== updateFundEffects road %d %d %d fire %d %d %d police %d %d %d\n",
                (int)roadEffect, (int)roadSpend, (int)roadFund,
                (int)fireEffect, (int)fireSpend, (int)fireFund,
                (int)policeEffect, (int)policeSpend, (int)policeFund);
#endif

            mustDrawBudget = 1;
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

        /// <summary>
        /// Handle road tile.
        /// </summary>
        /// <param name="pos">Position of the road.</param>
        private void doRoad(Position pos)
        {
            short tden, z;
            short[] densityTable =
            {
                (short) MapTileCharacters.ROADBASE,
                (short) MapTileCharacters.LTRFBASE,
                (short) MapTileCharacters.HTRFBASE
            };

            roadTotal++;

            ushort mapValue = map[pos.posX,pos.posY];
            ushort tile = (ushort)(mapValue & (ushort)MapTileBits.LOMASK);

            /* generateBus(pos.posX, pos.posY); */

            if (roadEffect < (15 * MAX_ROAD_EFFECT / 16))
            {
                // roadEffect < 15/16 of max road, enable deteriorating road
                if ((getRandom16() & 511) == 0)
                {
                    if ((ushort)(mapValue & (ushort)MapTileBits.CONDBIT) == 0)
                    {
                        //assert(MAX_ROAD_EFFECT == 32); // Otherwise the '(getRandom16() & 31)' makes no sense
                        if (roadEffect < (getRandom16() & 31))
                        {
                            if ((tile & 15) < 2 || (tile & 15) == 15)
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

            if ((mapValue & (ushort) MapTileBits.BURNBIT) == 0)
            { 
                /* If Bridge */
                roadTotal += 4; // Bridge counts as 4 road tiles
                if (doBridge(new Position(pos.posX, pos.posY), tile))
                {
                    return;
                }
            }

            if (tile < (ushort) MapTileCharacters.LTRFBASE)
            {
                tden = 0;
            }
            else if (tile < (ushort) MapTileCharacters.HTRFBASE)
            {
                tden = 1;
            }
            else
            {
                roadTotal++; // Heavy traffic counts as 2 roads.
                tden = 2;
            }

            short trafficDensity = (short) (trafficDensityMap.worldGet(pos.posX, pos.posY) >> 6);

            if (trafficDensity > 1)
            {
                trafficDensity--;
            }

            if (tden != trafficDensity)
            { 
                /* tden 0..2   */
                z = (short) (((tile - (ushort) MapTileCharacters.ROADBASE) & 15) + densityTable[trafficDensity]);
                z |= (short)(mapValue & (MapTileBits.ALLBITS - MapTileBits.ANIMBIT));

                if (trafficDensity > 0)
                {
                    z |= (short) MapTileBits.ANIMBIT;
                }

                map[pos.posX,pos.posY] = (ushort) z;
            }
        }

        /// <summary>
        /// Handle a bridge
        /// </summary>
        /// <param name="pos">Position of the bridge.</param>
        /// <param name="tile">Tile value of the bridge.</param>
        /// <returns></returns>
        private bool doBridge(Position pos, ushort tile)
        {
            short[] HDx = { -2, 2, -2, -1, 0, 1, 2 };
            short[] HDy = { -1, -1, 0, 0, 0, 0, 0 };
            short[] HBRTAB =
            {
                (short)MapTileCharacters.HBRDG1 | (short)MapTileBits.BULLBIT,
                (short)MapTileCharacters.HBRDG3 | (short)MapTileBits.BULLBIT,
                (short)MapTileCharacters.HBRDG0 | (short)MapTileBits.BULLBIT,
                (short)MapTileCharacters.RIVER,
                (short)MapTileCharacters.BRWH | (short)MapTileBits.BULLBIT,
                (short)MapTileCharacters.RIVER,
                (short)MapTileCharacters.HBRDG2 | (short)MapTileBits.BULLBIT,
            };
            short[] HBRTAB2 =
            {
                (short)MapTileCharacters.RIVER,
                (short)MapTileCharacters.RIVER,
                (short)MapTileCharacters.HBRIDGE | (short)MapTileBits.BULLBIT,
                (short)MapTileCharacters.HBRIDGE | (short)MapTileBits.BULLBIT,
                (short)MapTileCharacters.HBRIDGE | (short)MapTileBits.BULLBIT,
                (short)MapTileCharacters.HBRIDGE | (short)MapTileBits.BULLBIT,
                (short)MapTileCharacters.HBRIDGE | (short)MapTileBits.BULLBIT,
            };
            short[] VDx = { 0, 1, 0, 0, 0, 0, 1 };
            short[] VDy = { -2, -2, -1, 0, 1, 2, 2 };
            short[] VBRTAB =
            {
                (short)MapTileCharacters.VBRDG0 | (short)MapTileBits.BULLBIT,
                (short)MapTileCharacters.VBRDG1 | (short)MapTileBits.BULLBIT,
                (short)MapTileCharacters.RIVER,
                (short)MapTileCharacters.BRWV | (short)MapTileBits.BULLBIT,
                (short)MapTileCharacters.RIVER,
                (short)MapTileCharacters.VBRDG2 | (short)MapTileBits.BULLBIT,
                (short)MapTileCharacters.VBRDG3 | (short)MapTileBits.BULLBIT,
            };
            short[] VBRTAB2 =
            {
                (short)MapTileCharacters.VBRIDGE | (short)MapTileBits.BULLBIT,
                (short)MapTileCharacters.RIVER,
                (short)MapTileCharacters.VBRIDGE | (short)MapTileBits.BULLBIT,
                (short)MapTileCharacters.VBRIDGE | (short)MapTileBits.BULLBIT,
                (short)MapTileCharacters.VBRIDGE | (short)MapTileBits.BULLBIT,
                (short)MapTileCharacters.VBRIDGE | (short)MapTileBits.BULLBIT,
                (short)MapTileCharacters.RIVER,
            };
            int z, x, y, MPtem;

            if (tile == (ushort) MapTileCharacters.BRWV)
            { 
                /*  Vertical bridge close */

                if ((getRandom16() & 3) == 0 && getBoatDistance(pos) > 340)
                {
                    for (z = 0; z < 7; z++)
                    { 
                        /* Close */
                        x = pos.posX + VDx[z];
                        y = pos.posY + VDy[z];

                        if (Position.testBounds((short) x, (short) y))
                        {
                            if ((map[x,y] & (ushort) MapTileBits.LOMASK) == (VBRTAB[z] & (short) MapTileBits.LOMASK))
                            {
                                map[x,y] = (ushort) VBRTAB2[z];
                            }
                        }
                    }
                }
                return true;
            }

            if (tile == (ushort) MapTileCharacters.BRWH)
            { 
                /*  Horizontal bridge close  */
                if ((getRandom16() & 3) == 0 && getBoatDistance(pos) > 340)
                {
                    for (z = 0; z < 7; z++)
                    { 
                        /* Close */
                        x = pos.posX + HDx[z];
                        y = pos.posY + HDy[z];

                        if (Position.testBounds((short) x, (short) y))
                        {
                            if ((map[x,y] & (ushort) MapTileBits.LOMASK) == (HBRTAB[z] & (short) MapTileBits.LOMASK))
                            {
                                map[x,y] = (ushort) HBRTAB2[z];
                            }
                        }
                    }
                }
                return true;
            }

            if (getBoatDistance(pos) < 300 || (getRandom16() & 7) == 0)
            {
                if ((tile & 1) != 0)
                {
                    if (pos.posX < WORLD_W - 1)
                    {
                        if (map[pos.posX + 1,pos.posY] == (ushort) MapTileCharacters.CHANNEL)
                        { 
                            /* Vertical open */
                            for (z = 0; z < 7; z++)
                            {
                                x = pos.posX + VDx[z];
                                y = pos.posY + VDy[z];

                                if (Position.testBounds((short) x, (short) y))
                                {
                                    MPtem = map[x,y];
                                    if (MPtem == (int) MapTileCharacters.CHANNEL || ((MPtem & 15) == (VBRTAB2[z] & 15)))
                                    {
                                        map[x,y] = (ushort) VBRTAB[z];
                                    }
                                }
                            }
                            return true;
                        }
                    }
                    return false;

                }
                else
                {
                    if (pos.posY > 0)
                    {
                        if (map[pos.posX,pos.posY - 1] == (ushort) MapTileCharacters.CHANNEL)
                        {
                            /* Horizontal open  */
                            for (z = 0; z < 7; z++)
                            {
                                x = pos.posX + HDx[z];
                                y = pos.posY + HDy[z];

                                if (Position.testBounds((short) x, (short) y))
                                {
                                    MPtem = map[x,y];
                                    if (((MPtem & 15) == (HBRTAB2[z] & 15)) || MPtem == (int) MapTileCharacters.CHANNEL)
                                    {
                                        map[x,y] = (ushort) HBRTAB[z];
                                    }
                                }
                            }
                            return true;
                        }
                    }
                    return false;
                }

            }
            return false;
        }

        /// <summary>
        /// Compute distance to nearest boat from a given bridge.
        /// </summary>
        /// <param name="pos">Position of bridge.</param>
        /// <returns>Distance to nearest boat.</returns>
        private int getBoatDistance(Position pos)
        {
            // TODO need spritelist
            return 9999;
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
            short[] DX = { -1, 0, 1, 0 };
            short[] DY = { 0, -1, 0, 1 };
            short z;

            // Try to set neighbouring tiles on fire as well
            for (z = 0; z < 4; z++)
            {
                if ((getRandom16() & 7) == 0)
                {
                    short xTem = (short) (pos.posX + DX[z]);
                    short yTem = (short) (pos.posY + DY[z]);

                    if (Position.testBounds(xTem, yTem))
                    {
                        ushort c = map[xTem, yTem];
                        if ((ushort)(c & (ushort) MapTileBits.BURNBIT) == 0)
                        {
                            continue;
                        }

                        if ((ushort)(c & (ushort) MapTileBits.ZONEBIT) != 0)
                        {
                            // Neighbour is a zone and burnable
                            fireZone(new Position(xTem, yTem), c);

                            if ((c & (ushort) MapTileBits.LOMASK) > (ushort) MapTileCharacters.IZB)
                            {
                                makeExplosionAt(xTem * 16 + 8, yTem * 16 + 8);
                            }
                        }

                        map[xTem, yTem] = randomFire();
                    }
                }
            }

            // Compute likelyhood of fire running out of fuel
            short rate = 10; // Likelyhood of extinguishing (bigger means less chance)
            z = fireStationEffectMap.worldGet(pos.posX, pos.posY);

            if (z > 0)
            {
                rate = 3;
                if (z > 20)
                {
                    rate = 2;
                }
                if (z > 100)
                {
                    rate = 1;
                }
            }

            // Decide whether to put out the fire.
            if (getRandom(rate) == 0)
            {
                map[pos.posX, pos.posY] = randomRubble();
            }
        }

        /// <summary>
        /// Generate a random animated FIRE tile
        /// </summary>
        /// <returns></returns>
        private ushort randomFire()
        {
            return (ushort)((ushort)((ushort) MapTileCharacters.FIRE + (getRandom16() & 7)) | (ushort) MapTileBits.ANIMBIT);
        }

        /// <summary>
        /// Generate a random RUBBLE tile
        /// </summary>
        /// <returns></returns>
        private ushort randomRubble()
        {
            return (ushort)((ushort)((ushort)MapTileCharacters.RUBBLE + (getRandom16() & 3)) | (ushort)MapTileBits.BULLBIT);
        }

        /// <summary>
        /// Handle a zone on fire.
        /// 
        /// Decreases rate of growth of the zone, and makes remaining tiles bulldozable.
        /// 
        /// TODO maybe move this to the zone file (or class if we create one)
        /// </summary>
        /// <param name="pos">Position of the zone on fire.</param>
        /// <param name="ch">Character of the zone.</param>
        private void fireZone(Position pos, ushort ch)
        {
            short XYmax;

            int value = rateOfGrowthMap.worldGet(pos.posX, pos.posY);
            value = clamp(value - 20, -200, 200);
            rateOfGrowthMap.worldSet(pos.posX, pos.posY, (short) value);

            ch = (ushort)(ch & (ushort) MapTileBits.LOMASK);

            if (ch < (ushort) MapTileCharacters.PORTBASE)
            {
                XYmax = 2;
            }
            else
            {
                if (ch == (ushort) MapTileCharacters.AIRPORT)
                {
                    XYmax = 5;
                }
                else
                {
                    XYmax = 4;
                }
            }

            // Make remaining tiles of the zone bulldozable
            for (short x = -1; x < XYmax; x++)
            {
                for (short y = -1; y < XYmax; y++)
                {
                    short xTem = (short) (pos.posX + x);
                    short yTem = (short) (pos.posY + y);

                    if (!Position.testBounds(xTem, yTem))
                    {
                        continue;
                    }

                    if ((ushort)(map[xTem,yTem] & (ushort) MapTileBits.LOMASK) >= (ushort) MapTileCharacters.ROADBASE)
                    {
                        /* post release */
                        map[xTem,yTem] |= (ushort) MapTileBits.BULLBIT;
                    }

                }
            }
        }
    }
}
