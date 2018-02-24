namespace MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Allocate and initialize arrays for the maps.
        /// </summary>
        public void initMapArrays()
        {
            map = new short[WORLD_W, WORLD_H];
        }

        /// <summary>
        /// Free all map arrays.
        /// </summary>
        public void destroyMapArrays()
        {
        }
    }
}
