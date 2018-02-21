using System;
using UnityEngine;

namespace Micropolis.MicropolisCore
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

            Debug.Log(string.Format("simCycle:{0} phaseCycle:{1}", simCycle, phaseCycle));

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
                    //mapScan((phaseCycle - 1) * WORLD_W / 8, phaseCycle * WORLD_W / 8);
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
                        //newMapFlags[MAP_TYPE_POWER] = 1;
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
            // mapScan(0, WORLD_W);
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
        }

        /// <summary>
        /// Decrease traffic memory.
        /// </summary>
        /// <remarks>
        /// Tends to empty trafficDensityMap.
        /// </remarks>
        public void decTrafficMap()
        {
        }

        /// <summary>
        /// Decrease rate of grow.
        /// </summary>
        /// <remarks>
        /// Tends to empty rateOfGrowthMap.
        /// </remarks>
        public void decRateOfGrowthMap()
        {
        }

        public void initSimMemory()
        {
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

            cityTaxAverage = (short) (cityTime % 48 * 7);

            doNilPower();

            initSimLoad = 0;
        }

        public void setCommonInits()
        {
            evalInit();
            // TODO effects
            taxFlag = false;
            taxFund = 0;
        }

        public void setValves()
        {
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

            // TODO clear maps
        }

        /// <summary>
        /// Take monthly snaphsot of all relevant data for the historic graphs.
        /// Also update variables that control building new churches and hospitals.
        /// TODO Rename to takeMonthlyCensus (or takeMonthlySnaphshot?)
        /// TODO A lot of this max stuff is also done in Graph.cs
        /// </summary>
        public void take10Census()
        {
        }

        public void take120Census()
        {
        }

        /// <summary>
        /// Collect taxes
        /// </summary>
        public void collectTax()
        {
        }

        /// <summary>
        /// Update effects of (possibly reduced) funding.
        /// It updates effects with respect to roads, police, and fire.
        /// </summary>
        public void updateFundEffects()
        {
        }

        public void mapScan(int x1, int x2)
        {
        }
    }
}
