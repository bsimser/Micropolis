namespace MicropolisCore
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
            // TODO
        }

        /// <summary>
        /// Initialize for a simulation
        /// </summary>
        public void simInit()
        {
            setEnableSound(true); // Enable sound
            mustUpdateOptions = true; // Update options displayed at user
            // TODO scenario = SC_NONE;
            startingYear = 1900;
            simPasses = 1;
            simPass = 0;
            setAutoGoto(true);
            setCityTax(7);
            cityTime = 50;
            setEnableDisasters(true); // Enable disasters
            setAutoBulldoze(true); // Enable auto bulldoze
            setAutoBudget(true); // Enable auto-budget
            blinkFlag = 1;
            simSpeed = 3;
            changeEval();
            simPaused = false; // Simulation is running
            simLoops = 0;
            initSimLoad = 2;

            initMapArrays();
            initGraphs();
            initFundingLevel();
            resetMapState();
            resetEditorState();
            clearMap();
            initWillStuff();
            setFunds(5000);
            // TODO setGameLevelFunds(LEVEL_EASY);
            setSpeed(0);
            setPasses(1);
        }

        public void simUpdate()
        {
            blinkFlag = (short) (tickCount() % 60 < 30 ? 1 : -1);

            if (simSpeed != 0 && heatSteps == 0)
            {
                tilesAnimated = false;
            }

            doUpdateHeads();
            graphDoer();
            updateBudget();
            scoreDoer();
        }

        public void simHeat()
        {
            // TODO
        }

        public void simLoop(bool doSim)
        {
            if (heatSteps != 0)
            {
                for (int j = 0; j < heatSteps; j++)
                {
                    simHeat();
                }

                moveObjects();
                simRobots();

                newMap = 1;
            }
            else
            {
                if (doSim)
                {
                    simFrame();
                }

                moveObjects();
                simRobots();
            }

            simLoops++;
        }

        /// <summary>
        /// Move simulation forward.
        /// </summary>
        public void simTick()
        {
            if (simSpeed != 0)
            {
                for (simPass = 0; simPass < simPasses; simPass++)
                {
                    simLoop(true);
                }
            }
            simUpdate();
        }

        public void simRobots()
        {
            callback("simRobots", "");
        }
    }
}
