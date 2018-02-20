namespace Micropolis.MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Let disasters happen.
        /// </summary>
        public void doDisasters()
        {
        }

        /// <summary>
        /// Let disasters of the scenario happen.
        /// </summary>
        public void scenarioDisaster()
        {
        }

        /// <summary>
        /// Make a nuclear power plant melt down.
        /// TODO Randomize which nuke plant melts down.
        /// </summary>
        public void makeMeltdown()
        {
        }

        /// <summary>
        /// Let a fire bomb explode at a random location.
        /// </summary>
        public void fireBomb()
        {
        }

        /// <summary>
        /// Throw several bombs onto the city.
        /// </summary>
        public void makeFireBombs()
        {
        }

        /// <summary>
        /// Change random tiles to fire or dirt as result of the earthquake.
        /// </summary>
        public void makeEarthquake()
        {
        }

        /// <summary>
        /// Start a fire at a random place, random disaster or scenario.
        /// </summary>
        public void setFire()
        {
        }

        /// <summary>
        /// Start a fire at a random place, requested by user.
        /// </summary>
        public void makeFire()
        {
        }

        /// <summary>
        /// Is tile vulnerable for an earthquake?
        /// </summary>
        /// <param name="tem">Tile data</param>
        /// <returns>Function returns true if tile is vulnerable, and false if not</returns>
        public bool vulnerable(int tem)
        {
            return false;
        }

        /// <summary>
        /// Flood many tiles.
        /// TODO Use Direction and some form of XYPosition class here.
        /// </summary>
        public void makeFlood()
        {
        }

        /// <summary>
        /// Flood around the given position.
        /// TODO Use some form of rotating around a position.
        /// </summary>
        /// <param name="pos">Position around which to flood further</param>
        public void doFlood(Position pos)
        {
        }
    }
}
