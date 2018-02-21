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
            // setEnableSound(true);
            // mustUpdateOptions = true;
            // scenario = SC_NONE;
            // startingYear = 1900;
            // simPasses = 1;
            // simPass = 0;
            setAutoGoto(true);
            setCityTax(7);
            // cityTime = 50;
            setEnableDisasters(true);
            setAutoBulldoze(true);
            setAutoBudget(true);
            // blinkFlag = 1;
            // simSpeed = 3;
            changeEval();
            // simPaused = false; // Simulation is running
            // simLoops = 0;
            // initSimLoad = 2;

            initMapArrays();
            initGraphs();
            initFundingLevel();
            resetMapState();
            resetEditorState();
            clearMap();
            initWillStuff();
            setFunds(5000);
            // setGameLevelFunds(LEVEL_EASY);
            setSpeed(0);
            setPasses(1);
        }

        public void simUpdate()
        {
            // blinkFlag = ((tickCount() % 60) < 30) ? 1 : -1;

            // if(simSpeed && !heatSteps)
            //   tilesAnimated = false;

            doUpdateHeads();
            graphDoer();
            updateBudget();
            scoreDoer();
        }

        public void simHeat()
        {
        }

        public void simLoop(bool doSim)
        {
            // if(heatSteps)
            // simHeat()
            // moveObjects()
            // simRobots()
            // else
            // if(doSim)
            // simFrame()
            // moveObjects()
            // simRobots();
            // simLoops++;
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
