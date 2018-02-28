using System;

namespace MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Allocate and initialize arrays for the maps.
        /// </summary>
        private void initMapArrays()
        {
            map = new ushort[WORLD_W, WORLD_H];

            resHist = new short[HISTORY_LENGTH];
            comHist = new short[HISTORY_LENGTH];
            indHist = new short[HISTORY_LENGTH];
            moneyHist = new short[HISTORY_LENGTH];
            pollutionHist = new short[HISTORY_LENGTH];
            crimeHist = new short[HISTORY_LENGTH];
            miscHist = new short[HISTORY_LENGTH];
        }

        /// <summary>
        /// Free all map arrays.
        /// </summary>
        private void destroyMapArrays()
        {
            Array.Clear(map, 0, map.Length);

            populationDensityMap.clear();
            trafficDensityMap.clear();
            pollutionDensityMap.clear();
            landValueMap.clear();
            crimeRateMap.clear();

            tempMap1.clear();
            tempMap2.clear();
            tempMap3.clear();

            terrainDensityMap.clear();

            Array.Clear(resHist, 0, resHist.Length);
            Array.Clear(comHist, 0, comHist.Length);
            Array.Clear(indHist, 0, indHist.Length);
            Array.Clear(moneyHist, 0, moneyHist.Length);
            Array.Clear(pollutionHist, 0, pollutionHist.Length);
            Array.Clear(crimeHist, 0, crimeHist.Length);
            Array.Clear(miscHist, 0, miscHist.Length);
        }
    }
}
