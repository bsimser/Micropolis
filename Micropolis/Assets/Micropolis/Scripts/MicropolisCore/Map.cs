namespace MicropolisCore
{
    /// <summary>
    /// Generic class for maps in the Micropolis game.
    /// 
    /// A map is assumed to cover a 2D grid of WORLD_W times WORLD_H positions.
    /// A block of positions may be clustered, and represented by a single data
    /// value.
    /// </summary>
    /// <typeparam name="DATA">Data type of a data value</typeparam>
    public class Map<DATA>
    {
        private int BLKSIZE;
        private DATA _MAP_DEFAULT_VALUE; // Default value of a cluster
        private DATA[] _mapData;

        public Map(DATA defaultValue, int blksize)
        {
            _MAP_DEFAULT_VALUE = defaultValue;
            BLKSIZE = blksize;
            var width = (Micropolis.WORLD_W + BLKSIZE - 1) / BLKSIZE;
            var height = (Micropolis.WORLD_H + BLKSIZE - 1) / BLKSIZE;
            _mapData = new DATA[width * height];
        }

        public void clear()
        {
            fill(_MAP_DEFAULT_VALUE);
        }

        public void fill(DATA value)
        {
            for (int i = 0; i < _mapData.Length; i++)
            {
                _mapData[i] = value;
            }
        }
    }
}
