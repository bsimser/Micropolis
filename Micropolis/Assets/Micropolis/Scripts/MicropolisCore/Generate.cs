namespace Micropolis.MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Create a new map for a city.
        /// </summary>
        public void generateMap()
        {
        }

        /// <summary>
        /// Generate a map for a city.
        /// </summary>
        /// <param name="seed">Random number generator initializing seed</param>
        public void generateSomeCity(int seed)
        {
        }

        /// <summary>
        /// Generate a map.
        /// </summary>
        /// <param name="seed">Initialization seed for the random generator.</param>
        public void generateMap(int seed)
        {
        }

        /// <summary>
        /// Clear the whole world to ::DIRT tiles
        /// </summary>
        public void clearMap()
        {
        }

        /// <summary>
        /// Clear everything from all land
        /// </summary>
        public void clearUnnatural()
        {
        }

        /// <summary>
        /// Construct a plain island as world, surrounded by 5 tiles of river.
        /// </summary>
        public void makeNakedIsland()
        {
        }

        /// <summary>
        /// Construct a new world as an island
        /// </summary>
        public void makeIsland()
        {
        }

        /// <summary>
        /// Make a number of lakes, depending on the terrainLakeLevel.
        /// </summary>
        public void makeLakes()
        {
        }

        /// <summary>
        /// Make a random lake at pos.
        /// </summary>
        /// <param name="pos">Rough position of the lake.</param>
        public void makeSingleLake(Position pos)
        {
        }

        /// <summary>
        /// Splash a bunch of trees down near (xloc, yloc).
        /// 
        /// Amount of trees is controlled by terrainTreeLevel.
        /// </summary>
        /// <remarks>Trees are not smoothed.</remarks>
        /// <param name="xloc">Horizontal position of starting point for splashing trees.</param>
        /// <param name="yloc">Vertical position of starting point for splashing trees.</param>
        public void treeSplash(short xloc, short yloc)
        {
        }

        /// <summary>
        /// Splash trees around the world.
        /// </summary>
        public void doTrees()
        {
        }

        public void smoothRiver()
        {
        }

        public void smoothTrees()
        {
        }

        /// <summary>
        /// Temporary function to prevent breaking a lot of code.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="preserve"></param>
        public void smoothTreesAt(int x, int y, bool preserve)
        {
        }

        /// <summary>
        /// Construct rivers.
        /// </summary>
        /// <param name="terrainPos">Coordinate to start making a river.</param>
        public void doRivers(Position terrainPos)
        {
        }

        /// <summary>
        /// Make a big river.
        /// </summary>
        /// <param name="riverPos">Start position of making a river.</param>
        /// <param name="riverDir">Global direction of the river.</param>
        /// <param name="terrainDir">Local direction of the terrain.</param>
        /// <returns>Last used local terrain direction.</returns>
        public Direction2 doBRiver(Position riverPos, Direction2 riverDir, Direction2 terrainDir)
        {
            return terrainDir;
        }

        /// <summary>
        /// Make a small river.
        /// </summary>
        /// <param name="riverPos">Start position of making a river.</param>
        /// <param name="riverDir">Global direction of the river.</param>
        /// <param name="terrainDir">Local direction of the terrain.</param>
        /// <returns>Last used local terrain direction.</returns>
        public Direction2 doSRiver(Position riverPos, Direction2 riverDir, Direction2 terrainDir)
        {
            return terrainDir;
        }

        /// <summary>
        /// Put down a big river diamond-like shape.
        /// </summary>
        /// <param name="pos">Base coordinate of the blob (top-left position).</param>
        public void plopBRiver(Position pos)
        {
        }

        /// <summary>
        /// Put down a small river diamond-like shape.
        /// </summary>
        /// <param name="pos">Base coordinate of the blob (top-left position).</param>
        public void plopSRiver(Position pos)
        {
        }

        public void smoothWater()
        {
        }
    }
}
