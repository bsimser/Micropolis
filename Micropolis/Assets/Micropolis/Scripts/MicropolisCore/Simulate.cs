namespace Micropolis.MicropolisCore
{
    public partial class Micropolis
    {
        public void simFrame()
        {
        }

        public void simulate()
        {
        }

        /// <summary>
        /// Initialize simulation.
        /// </summary>
        public void doSimInit()
        {
        }

        /// <summary>
        /// Copy bits from powerGridMap to the #PWRBIT in the map for all zones in the world.
        /// </summary>
        public void doNilPower()
        {
        }

        /// <summary>
        /// Decrease traffic memory.
        /// </summary>
        /// <remarks>
        /// Tends to empty trafficDensityMap.
        /// </remarks>
        public void decTrafficMap()
        {
        }

        /// <summary>
        /// Decrease rate of grow.
        /// </summary>
        /// <remarks>
        /// Tends to empty rateOfGrowthMap.
        /// </remarks>
        public void decRateOfGrowthMap()
        {
        }

        public void initSimMemory()
        {
        }

        public void simLoadInit()
        {
        }

        public void setCommonInits()
        {
        }

        public void setValves()
        {
        }

        public void clearCensus()
        {
        }

        /// <summary>
        /// Take monthly snaphsot of all relevant data for the historic graphs.
        /// Also update variables that control building new churches and hospitals.
        /// TODO Rename to takeMonthlyCensus (or takeMonthlySnaphshot?)
        /// TODO A lot of this max stuff is also done in Graph.cs
        /// </summary>
        public void take10Census()
        {
        }

        public void take120Census()
        {
        }

        /// <summary>
        /// Collect taxes
        /// </summary>
        public void collectTax()
        {
        }

        /// <summary>
        /// Update effects of (possibly reduced) funding.
        /// It updates effects with respect to roads, police, and fire.
        /// </summary>
        public void updateFundEffects()
        {
        }

        public void mapScan(int x1, int x2)
        {
        }
    }
}
