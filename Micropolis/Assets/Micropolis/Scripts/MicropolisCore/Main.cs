namespace Micropolis.MicropolisCore
{
    public partial class Micropolis
    {
        public string getMicropolisVersion()
        {
            return "5.0";
        }

        /// <summary>
        /// Locate resource directory.
        /// </summary>
        public void environmentInit()
        {
        }

        /// <summary>
        /// Initialize for a simulation
        /// </summary>
        public void simInit()
        {
        }

        public void simUpdate()
        {
            // doUpdateHeads();
            // graphDoer();
            // updateBudget();
            // scoreDoer();
        }

        public void simHeat()
        {
        }

        public void simLoop(bool doSim)
        {
        }

        public void simTick()
        {
            /*
            if (simSpeed)
            {
                for (simPass = 0; simPass < simPasses; simPass++)
                {
                    simLoop(true);
                }
            }
            */
            simUpdate();
        }

        public void simRobots()
        {
        }
    }
}
