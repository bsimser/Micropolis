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
            crimeHist = new short[HISTORY_LENGTH];
            pollutionHist = new short[HISTORY_LENGTH];
            moneyHist = new short[HISTORY_LENGTH];
            miscHist = new short[HISTORY_LENGTH];
        }

        /// <summary>
        /// Free all map arrays.
        /// </summary>
        private void destroyMapArrays()
        {
            // TODO clear the map

            // TODO clear density maps

            // TODO clear temp maps

            // TODO free histories
        }
    }
}
