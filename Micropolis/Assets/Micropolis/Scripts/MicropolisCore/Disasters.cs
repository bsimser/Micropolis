namespace MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Let disasters happen.
        /// </summary>
        public void doDisasters()
        {
            // Chance of disasters at lev 0 1 2
            short[] DisChance =
            {
                10 * 48,    // Game level 0
                5 * 48,     // Game level 1
                60          // Game level 2
            };
            // assert(LEVEL_COUNT == LENGTH_OF(DisChance));

            if (floodCount != 0)
            {
                floodCount--;
            }

            // TODO scenarioDisaster()

            if (!enableDisasters)
            {
                return;
            }

            // TODO var x = gameLevel;

            // TODO if(!getRandom(DisChance[x]))
        }

        /// <summary>
        /// Let disasters of the scenario happen.
        /// </summary>
        public void scenarioDisaster()
        {
            // TODO
        }

        /// <summary>
        /// Make a nuclear power plant melt down.
        /// TODO Randomize which nuke plant melts down.
        /// </summary>
        public void makeMeltdown()
        {
            // TODO
        }

        /// <summary>
        /// Let a fire bomb explode at a random location.
        /// </summary>
        public void fireBomb()
        {
            // TODO
        }

        /// <summary>
        /// Throw several bombs onto the city.
        /// </summary>
        public void makeFireBombs()
        {
            int count = 2 + (getRandom16() & 1);

            while (count > 0)
            {
                fireBomb();
                count--;
            }
        }

        /// <summary>
        /// Change random tiles to fire or dirt as result of the earthquake.
        /// </summary>
        public void makeEarthquake()
        {
            // TODO
        }

        /// <summary>
        /// Start a fire at a random place, random disaster or scenario.
        /// </summary>
        public void setFire()
        {
            // TODO
        }

        /// <summary>
        /// Start a fire at a random place, requested by user.
        /// </summary>
        public void makeFire()
        {
            // TODO
        }

        /// <summary>
        /// Is tile vulnerable for an earthquake?
        /// </summary>
        /// <param name="tem">Tile data</param>
        /// <returns>Function returns true if tile is vulnerable, and false if not</returns>
        public bool vulnerable(int tem)
        {
            // TODO
            return false;
        }

        /// <summary>
        /// Flood many tiles.
        /// TODO Use Direction and some form of XYPosition class here.
        /// </summary>
        public void makeFlood()
        {
            // TODO
        }

        /// <summary>
        /// Flood around the given position.
        /// TODO Use some form of rotating around a position.
        /// </summary>
        /// <param name="pos">Position around which to flood further</param>
        public void doFlood(Position pos)
        {
            // TODO
        }
    }
}
