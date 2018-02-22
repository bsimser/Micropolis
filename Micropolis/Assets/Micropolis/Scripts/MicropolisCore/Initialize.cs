namespace Micropolis.MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Reset many game state variables
        /// </summary>
        public void initWillStuff()
        {
            randomlySeedRandom();
            initGraphMax();
            //destroyAllSprites();

            //roadEffect = MAX_ROAD_EFFECT;
            //policeEffect = MAX_POLICE_STATION_EFFECT;
            //fireEffect = MAX_FIRE_STATION_EFFECT;
            cityScore = 500;
            cityPop = -1;
            cityTimeLast = -1;
            cityYearLast = -1;
            cityMonthLast = -1;
            totalFundsLast = -1;
            resLast = comLast = indLast = -999999;
            roadFund = 0;
            policeFund = 0;
            fireFund = 0;
            valveFlag = true;
            //disasterEvent = SC_NONE;
            taxFlag = false;

            //populationDensityMap.clear();
            //trafficDensityMap.clear();
            //pollutionDensityMap.clear();
            //landValueMap.clear();
            //crimeRateMap.clear();
            //terrainDensityMap.clear();
            //rateOfGrowthMap.clear();
            //comRateMap.clear();
            //policeStationMap.clear();
            //policeStationEffectMap.clear();
            //fireStationMap.clear();
            //fireStationEffectMap.clear();

            doNewGame();
            doUpdateHeads();
        }

        /// <summary>
        /// Reset all maps in the simulator.
        /// </summary>
        public void resetMapState()
        {
        }

        /// <summary>
        /// Reset all tools in the simulator editor.
        /// </summary>
        public void resetEditorState()
        {
        }
    }
}
