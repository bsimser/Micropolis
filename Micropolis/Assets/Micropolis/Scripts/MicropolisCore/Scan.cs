namespace MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Make firerate map from firestation map.
        /// </summary>
        public void fireAnalysis()
        {
        }

        public void populationDensityScan()
        {
        }

        /// <summary>
        /// Does pollution, terrain, land value
        /// </summary>
        public void pollutionTerrainLandValueScan()
        {
        }

        /// <summary>
        /// Return pollution of a tile value
        /// </summary>
        /// <param name="loc">Tile character</param>
        /// <returns>Value of the pollution (0..255, bigger is worse)</returns>
        public int getPollutionValue(int loc)
        {
            return 0;
        }

        /// <summary>
        /// Compute Manhattan distance between given world position and center of the city.
        /// </summary>
        /// <remarks>
        /// For long distances (&gt; 64), value 64 is returned.
        /// </remarks>
        /// <param name="x">X world coordinate of given position</param>
        /// <param name="y">Y world coordinate of given position</param>
        /// <returns>Manhattan distance (dx+dy) between both positions</returns>
        public int getCityCenterDistance(int x, int y)
        {
            return 0;
        }

        /// <summary>
        /// Smooth police station map and compute crime rate
        /// </summary>
        public void crimeScan()
        {
        }

        public void smoothTerrain()
        {
        }

        /// <summary>
        /// Smooth tempMap1 to tempMap2
        /// </summary>
        public void doSmooth1()
        {
        }

        /// <summary>
        /// Smooth tempMap2 to tempMap1
        /// </summary>
        public void doSmooth2()
        {
        }

        /// <summary>
        /// Compute distance to city center for the entire map.
        /// </summary>
        public void computeComRateMap()
        {
        }
    }
}
