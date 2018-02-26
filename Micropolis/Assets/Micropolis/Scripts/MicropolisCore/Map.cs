using System.Runtime.CompilerServices;

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
        // Size of a cluster in number of world positions.
        public int MAP_BLOCKSIZE;
        public int MAP_W; // Number of clusters in horizontal direction.
        public int MAP_H; // Number of clusters in vertical direction.

        public Map(int blksize)
        {
            BLKSIZE = blksize;
            MAP_BLOCKSIZE = BLKSIZE;
            MAP_W = (Micropolis.WORLD_W + BLKSIZE - 1) / BLKSIZE;
            MAP_H = (Micropolis.WORLD_H + BLKSIZE - 1) / BLKSIZE;
            _mapData = new DATA[MAP_W * MAP_H];
        }

        public Map(DATA defaultValue, int blksize) : this(blksize)
        {
            _MAP_DEFAULT_VALUE = defaultValue;
        }

        protected Map(Map<DATA> map) : this(map.BLKSIZE)
        {
            for (int i = 0; i < MAP_W * MAP_H; i++)
            {
                _mapData[i] = map._mapData[i];
            }
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

        /// <summary>
        /// Return the value of a cluster.
        /// 
        /// If the coordinate is off the map, the _MAP_DEFAULT_VALUE is returned.
        /// </summary>
        /// <param name="x">X world position.</param>
        /// <param name="y">Y world position.</param>
        /// <returns>Value of the cluster.</returns>
        public DATA worldGet(int x, int y)
        {
            if (!worldOnMap(x, y))
            {
                return _MAP_DEFAULT_VALUE;
            }

            x /= BLKSIZE;
            y /= BLKSIZE;
            return _mapData[x * MAP_H + y];
        }

        /// <summary>
        /// Set the value of a cluster.
        /// 
        /// If the coordinate is off the map, the value is not stored.
        /// </summary>
        /// <param name="x">X world position.</param>
        /// <param name="y">Y world position.</param>
        /// <param name="value">Value to use.</param>
        public void worldSet(int x, int y, DATA value)
        {
            if (worldOnMap(x, y))
            {
                x /= BLKSIZE;
                y /= BLKSIZE;
                _mapData[x * MAP_H + y] = value;
            }
        }

        public void set(int x, int y, DATA value)
        {
            if (onMap(x, y))
            {
                _mapData[x * MAP_H + y] = value;
            }
        }

        public DATA get(int x, int y)
        {
            if (!onMap(x, y))
            {
                return _MAP_DEFAULT_VALUE;
            }

            return _mapData[x * MAP_H + y];
        }

        private bool onMap(int x, int y)
        {
            return x >= 0 && x < MAP_W && y >= 0 && y < MAP_H;
        }

        /// <summary>
        /// Verify that world coordinates are within map boundaries.
        /// </summary>
        /// <param name="x">X world position.</param>
        /// <param name="y">Y world position.</param>
        /// <returns>Coordinate is within map boundaries.</returns>
        private bool worldOnMap(int x, int y)
        {
            return (x >= 0 && x < Micropolis.WORLD_W) && (y >= 0 && y < Micropolis.WORLD_H);
        }
    }
}
